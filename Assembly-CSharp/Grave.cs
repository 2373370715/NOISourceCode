using System;
using System.Collections.Generic;
using KSerialization;
using UnityEngine;

// Token: 0x02000E08 RID: 3592
public class Grave : StateMachineComponent<Grave.StatesInstance>
{
	// Token: 0x06004628 RID: 17960 RVA: 0x000D1D46 File Offset: 0x000CFF46
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.Subscribe<Grave>(-1697596308, Grave.OnStorageChangedDelegate);
		this.epitaphIdx = UnityEngine.Random.Range(0, int.MaxValue);
	}

	// Token: 0x06004629 RID: 17961 RVA: 0x0025C4E0 File Offset: 0x0025A6E0
	protected override void OnSpawn()
	{
		base.GetComponent<Storage>().SetOffsets(Grave.DELIVERY_OFFSETS);
		Storage component = base.GetComponent<Storage>();
		Storage storage = component;
		storage.OnWorkableEventCB = (Action<Workable, Workable.WorkableEvent>)Delegate.Combine(storage.OnWorkableEventCB, new Action<Workable, Workable.WorkableEvent>(this.OnWorkEvent));
		KAnimFile anim = Assets.GetAnim("anim_bury_dupe_kanim");
		int num = 0;
		KAnim.Anim anim2;
		for (;;)
		{
			anim2 = anim.GetData().GetAnim(num);
			if (anim2 == null)
			{
				goto IL_8F;
			}
			if (anim2.name == "working_pre")
			{
				break;
			}
			num++;
		}
		float workTime = (float)(anim2.numFrames - 3) / anim2.frameRate;
		component.SetWorkTime(workTime);
		IL_8F:
		base.OnSpawn();
		base.smi.StartSM();
		Components.Graves.Add(this);
	}

	// Token: 0x0600462A RID: 17962 RVA: 0x000D1D70 File Offset: 0x000CFF70
	protected override void OnCleanUp()
	{
		Components.Graves.Remove(this);
		base.OnCleanUp();
	}

	// Token: 0x0600462B RID: 17963 RVA: 0x0025C598 File Offset: 0x0025A798
	private void OnStorageChanged(object data)
	{
		GameObject gameObject = (GameObject)data;
		if (gameObject != null)
		{
			this.graveName = gameObject.name;
			MinionIdentity component = gameObject.GetComponent<MinionIdentity>();
			if (component != null)
			{
				Personality personality = Db.Get().Personalities.TryGet(component.personalityResourceId);
				KAnimFile anim = Assets.GetAnim("gravestone_kanim");
				if (personality != null && anim.GetData().GetAnim(personality.graveStone) != null)
				{
					this.graveAnim = personality.graveStone;
				}
			}
			Util.KDestroyGameObject(gameObject);
		}
	}

	// Token: 0x0600462C RID: 17964 RVA: 0x000D1D83 File Offset: 0x000CFF83
	private void OnWorkEvent(Workable workable, Workable.WorkableEvent evt)
	{
	}

	// Token: 0x040030F3 RID: 12531
	[Serialize]
	public string graveName;

	// Token: 0x040030F4 RID: 12532
	[Serialize]
	public string graveAnim = "closed";

	// Token: 0x040030F5 RID: 12533
	[Serialize]
	public int epitaphIdx;

	// Token: 0x040030F6 RID: 12534
	[Serialize]
	public float burialTime = -1f;

	// Token: 0x040030F7 RID: 12535
	private static readonly CellOffset[] DELIVERY_OFFSETS = new CellOffset[1];

	// Token: 0x040030F8 RID: 12536
	private static readonly EventSystem.IntraObjectHandler<Grave> OnStorageChangedDelegate = new EventSystem.IntraObjectHandler<Grave>(delegate(Grave component, object data)
	{
		component.OnStorageChanged(data);
	});

	// Token: 0x02000E09 RID: 3593
	public class StatesInstance : GameStateMachine<Grave.States, Grave.StatesInstance, Grave, object>.GameInstance
	{
		// Token: 0x0600462F RID: 17967 RVA: 0x000D1DCC File Offset: 0x000CFFCC
		public StatesInstance(Grave master) : base(master)
		{
		}

		// Token: 0x06004630 RID: 17968 RVA: 0x0025C620 File Offset: 0x0025A820
		public void CreateFetchTask()
		{
			this.chore = new FetchChore(Db.Get().ChoreTypes.FetchCritical, base.GetComponent<Storage>(), 1f, new HashSet<Tag>
			{
				GameTags.BaseMinion
			}, FetchChore.MatchCriteria.MatchTags, GameTags.Corpse, null, null, true, null, null, null, Operational.State.Operational, 0);
			this.chore.allowMultifetch = false;
		}

		// Token: 0x06004631 RID: 17969 RVA: 0x000D1DD5 File Offset: 0x000CFFD5
		public void CancelFetchTask()
		{
			this.chore.Cancel("Exit State");
			this.chore = null;
		}

		// Token: 0x040030F9 RID: 12537
		private FetchChore chore;
	}

	// Token: 0x02000E0A RID: 3594
	public class States : GameStateMachine<Grave.States, Grave.StatesInstance, Grave>
	{
		// Token: 0x06004632 RID: 17970 RVA: 0x0025C680 File Offset: 0x0025A880
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.empty;
			base.serializable = StateMachine.SerializeType.Both_DEPRECATED;
			this.empty.PlayAnim("open").Enter("CreateFetchTask", delegate(Grave.StatesInstance smi)
			{
				smi.CreateFetchTask();
			}).Exit("CancelFetchTask", delegate(Grave.StatesInstance smi)
			{
				smi.CancelFetchTask();
			}).ToggleMainStatusItem(Db.Get().BuildingStatusItems.GraveEmpty, null).EventTransition(GameHashes.OnStorageChange, this.full, null);
			this.full.PlayAnim((Grave.StatesInstance smi) => smi.master.graveAnim, KAnim.PlayMode.Once).ToggleMainStatusItem(Db.Get().BuildingStatusItems.Grave, null).Enter(delegate(Grave.StatesInstance smi)
			{
				if (smi.master.burialTime < 0f)
				{
					smi.master.burialTime = GameClock.Instance.GetTime();
				}
			});
		}

		// Token: 0x040030FA RID: 12538
		public GameStateMachine<Grave.States, Grave.StatesInstance, Grave, object>.State empty;

		// Token: 0x040030FB RID: 12539
		public GameStateMachine<Grave.States, Grave.StatesInstance, Grave, object>.State full;
	}
}
