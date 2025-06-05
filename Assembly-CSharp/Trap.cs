using System;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x02001A41 RID: 6721
public class Trap : StateMachineComponent<Trap.StatesInstance>
{
	// Token: 0x06008C1C RID: 35868 RVA: 0x00370ABC File Offset: 0x0036ECBC
	private static void CreateStatusItems()
	{
		if (Trap.statusSprung == null)
		{
			Trap.statusReady = new StatusItem("Ready", BUILDING.STATUSITEMS.CREATURE_TRAP.READY.NAME, BUILDING.STATUSITEMS.CREATURE_TRAP.READY.TOOLTIP, "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, 129022, true, null);
			Trap.statusSprung = new StatusItem("Sprung", BUILDING.STATUSITEMS.CREATURE_TRAP.SPRUNG.NAME, BUILDING.STATUSITEMS.CREATURE_TRAP.SPRUNG.TOOLTIP, "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, 129022, true, null);
			Trap.statusSprung.resolveTooltipCallback = delegate(string str, object obj)
			{
				Trap.StatesInstance statesInstance = (Trap.StatesInstance)obj;
				return string.Format(str, statesInstance.master.contents.Get().GetProperName());
			};
		}
	}

	// Token: 0x06008C1D RID: 35869 RVA: 0x001002B5 File Offset: 0x000FE4B5
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.contents = new Ref<KPrefabID>();
		Trap.CreateStatusItems();
	}

	// Token: 0x06008C1E RID: 35870 RVA: 0x00370B6C File Offset: 0x0036ED6C
	protected override void OnSpawn()
	{
		base.OnSpawn();
		Storage component = base.GetComponent<Storage>();
		base.smi.StartSM();
		if (!component.IsEmpty())
		{
			KPrefabID component2 = component.items[0].GetComponent<KPrefabID>();
			if (component2 != null)
			{
				this.contents.Set(component2);
				base.smi.GoTo(base.smi.sm.occupied);
				return;
			}
			component.DropAll(false, false, default(Vector3), true, null);
		}
	}

	// Token: 0x040069C7 RID: 27079
	[Serialize]
	private Ref<KPrefabID> contents;

	// Token: 0x040069C8 RID: 27080
	public TagSet captureTags = new TagSet();

	// Token: 0x040069C9 RID: 27081
	private static StatusItem statusReady;

	// Token: 0x040069CA RID: 27082
	private static StatusItem statusSprung;

	// Token: 0x02001A42 RID: 6722
	public class StatesInstance : GameStateMachine<Trap.States, Trap.StatesInstance, Trap, object>.GameInstance
	{
		// Token: 0x06008C20 RID: 35872 RVA: 0x001002E0 File Offset: 0x000FE4E0
		public StatesInstance(Trap master) : base(master)
		{
		}

		// Token: 0x06008C21 RID: 35873 RVA: 0x00370BF0 File Offset: 0x0036EDF0
		public void OnTrapTriggered(object data)
		{
			KPrefabID component = ((GameObject)data).GetComponent<KPrefabID>();
			base.master.contents.Set(component);
			base.smi.sm.trapTriggered.Trigger(base.smi);
		}
	}

	// Token: 0x02001A43 RID: 6723
	public class States : GameStateMachine<Trap.States, Trap.StatesInstance, Trap>
	{
		// Token: 0x06008C22 RID: 35874 RVA: 0x00370C38 File Offset: 0x0036EE38
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.ready;
			base.serializable = StateMachine.SerializeType.Never;
			Trap.CreateStatusItems();
			this.ready.EventHandler(GameHashes.TrapTriggered, delegate(Trap.StatesInstance smi, object data)
			{
				smi.OnTrapTriggered(data);
			}).OnSignal(this.trapTriggered, this.trapping).ToggleStatusItem(Trap.statusReady, null);
			this.trapping.PlayAnim("working_pre").OnAnimQueueComplete(this.occupied);
			this.occupied.ToggleTag(GameTags.Trapped).ToggleStatusItem(Trap.statusSprung, (Trap.StatesInstance smi) => smi).DefaultState(this.occupied.idle).EventTransition(GameHashes.OnStorageChange, this.finishedUsing, (Trap.StatesInstance smi) => smi.master.GetComponent<Storage>().IsEmpty());
			this.occupied.idle.PlayAnim("working_loop", KAnim.PlayMode.Loop);
			this.finishedUsing.PlayAnim("working_pst").OnAnimQueueComplete(this.destroySelf);
			this.destroySelf.Enter(delegate(Trap.StatesInstance smi)
			{
				Util.KDestroyGameObject(smi.master.gameObject);
			});
		}

		// Token: 0x040069CB RID: 27083
		public GameStateMachine<Trap.States, Trap.StatesInstance, Trap, object>.State ready;

		// Token: 0x040069CC RID: 27084
		public GameStateMachine<Trap.States, Trap.StatesInstance, Trap, object>.State trapping;

		// Token: 0x040069CD RID: 27085
		public GameStateMachine<Trap.States, Trap.StatesInstance, Trap, object>.State finishedUsing;

		// Token: 0x040069CE RID: 27086
		public GameStateMachine<Trap.States, Trap.StatesInstance, Trap, object>.State destroySelf;

		// Token: 0x040069CF RID: 27087
		public StateMachine<Trap.States, Trap.StatesInstance, Trap, object>.Signal trapTriggered;

		// Token: 0x040069D0 RID: 27088
		public Trap.States.OccupiedStates occupied;

		// Token: 0x02001A44 RID: 6724
		public class OccupiedStates : GameStateMachine<Trap.States, Trap.StatesInstance, Trap, object>.State
		{
			// Token: 0x040069D1 RID: 27089
			public GameStateMachine<Trap.States, Trap.StatesInstance, Trap, object>.State idle;
		}
	}
}
