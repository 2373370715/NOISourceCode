using System;
using System.Collections.Generic;
using Klei.AI;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x020016EE RID: 5870
[SerializationConfig(MemberSerialization.OptIn)]
public class Phonobox : StateMachineComponent<Phonobox.StatesInstance>, IGameObjectEffectDescriptor
{
	// Token: 0x0600790F RID: 30991 RVA: 0x00321F50 File Offset: 0x00320150
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.smi.StartSM();
		GameScheduler.Instance.Schedule("Scheduling Tutorial", 2f, delegate(object obj)
		{
			Tutorial.Instance.TutorialMessage(Tutorial.TutorialMessages.TM_Schedule, true);
		}, null, null);
		this.workables = new PhonoboxWorkable[this.choreOffsets.Length];
		this.chores = new Chore[this.choreOffsets.Length];
		for (int i = 0; i < this.workables.Length; i++)
		{
			Vector3 pos = Grid.CellToPosCBC(Grid.OffsetCell(Grid.PosToCell(this), this.choreOffsets[i]), Grid.SceneLayer.Move);
			GameObject go = ChoreHelpers.CreateLocator("PhonoboxWorkable", pos);
			KSelectable kselectable = go.AddOrGet<KSelectable>();
			kselectable.SetName(this.GetProperName());
			kselectable.IsSelectable = false;
			PhonoboxWorkable phonoboxWorkable = go.AddOrGet<PhonoboxWorkable>();
			phonoboxWorkable.owner = this;
			this.workables[i] = phonoboxWorkable;
		}
	}

	// Token: 0x06007910 RID: 30992 RVA: 0x00322038 File Offset: 0x00320238
	protected override void OnCleanUp()
	{
		this.UpdateChores(false);
		for (int i = 0; i < this.workables.Length; i++)
		{
			if (this.workables[i])
			{
				Util.KDestroyGameObject(this.workables[i]);
				this.workables[i] = null;
			}
		}
		base.OnCleanUp();
	}

	// Token: 0x06007911 RID: 30993 RVA: 0x0032208C File Offset: 0x0032028C
	private Chore CreateChore(int i)
	{
		Workable workable = this.workables[i];
		ChoreType relax = Db.Get().ChoreTypes.Relax;
		IStateMachineTarget target = workable;
		ChoreProvider chore_provider = null;
		bool run_until_complete = true;
		Action<Chore> on_complete = null;
		Action<Chore> on_begin = null;
		ScheduleBlockType recreation = Db.Get().ScheduleBlockTypes.Recreation;
		WorkChore<PhonoboxWorkable> workChore = new WorkChore<PhonoboxWorkable>(relax, target, chore_provider, run_until_complete, on_complete, on_begin, new Action<Chore>(this.OnSocialChoreEnd), false, recreation, false, true, null, false, true, false, PriorityScreen.PriorityClass.high, 5, false, true);
		workChore.AddPrecondition(ChorePreconditions.instance.CanDoWorkerPrioritizable, workable);
		return workChore;
	}

	// Token: 0x06007912 RID: 30994 RVA: 0x000F4026 File Offset: 0x000F2226
	private void OnSocialChoreEnd(Chore chore)
	{
		if (base.gameObject.HasTag(GameTags.Operational))
		{
			this.UpdateChores(true);
		}
	}

	// Token: 0x06007913 RID: 30995 RVA: 0x003220F4 File Offset: 0x003202F4
	public void UpdateChores(bool update = true)
	{
		for (int i = 0; i < this.choreOffsets.Length; i++)
		{
			Chore chore = this.chores[i];
			if (update)
			{
				if (chore == null || chore.isComplete)
				{
					this.chores[i] = this.CreateChore(i);
				}
			}
			else if (chore != null)
			{
				chore.Cancel("locator invalidated");
				this.chores[i] = null;
			}
		}
	}

	// Token: 0x06007914 RID: 30996 RVA: 0x000F4041 File Offset: 0x000F2241
	public void AddWorker(WorkerBase player)
	{
		this.players.Add(player);
		base.smi.sm.playerCount.Set(this.players.Count, base.smi, false);
	}

	// Token: 0x06007915 RID: 30997 RVA: 0x000F4078 File Offset: 0x000F2278
	public void RemoveWorker(WorkerBase player)
	{
		this.players.Remove(player);
		base.smi.sm.playerCount.Set(this.players.Count, base.smi, false);
	}

	// Token: 0x06007916 RID: 30998 RVA: 0x00322154 File Offset: 0x00320354
	List<Descriptor> IGameObjectEffectDescriptor.GetDescriptors(GameObject go)
	{
		List<Descriptor> list = new List<Descriptor>();
		Descriptor item = default(Descriptor);
		item.SetupDescriptor(UI.BUILDINGEFFECTS.RECREATION, UI.BUILDINGEFFECTS.TOOLTIPS.RECREATION, Descriptor.DescriptorType.Effect);
		list.Add(item);
		Effect.AddModifierDescriptions(base.gameObject, list, "Danced", true);
		return list;
	}

	// Token: 0x04005ADF RID: 23263
	public const string SPECIFIC_EFFECT = "Danced";

	// Token: 0x04005AE0 RID: 23264
	public const string TRACKING_EFFECT = "RecentlyDanced";

	// Token: 0x04005AE1 RID: 23265
	public CellOffset[] choreOffsets = new CellOffset[]
	{
		new CellOffset(0, 0),
		new CellOffset(-1, 0),
		new CellOffset(1, 0),
		new CellOffset(-2, 0),
		new CellOffset(2, 0)
	};

	// Token: 0x04005AE2 RID: 23266
	private PhonoboxWorkable[] workables;

	// Token: 0x04005AE3 RID: 23267
	private Chore[] chores;

	// Token: 0x04005AE4 RID: 23268
	private HashSet<WorkerBase> players = new HashSet<WorkerBase>();

	// Token: 0x04005AE5 RID: 23269
	private static string[] building_anims = new string[]
	{
		"working_loop",
		"working_loop2",
		"working_loop3"
	};

	// Token: 0x020016EF RID: 5871
	public class States : GameStateMachine<Phonobox.States, Phonobox.StatesInstance, Phonobox>
	{
		// Token: 0x06007919 RID: 31001 RVA: 0x0032221C File Offset: 0x0032041C
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.unoperational;
			this.unoperational.Enter(delegate(Phonobox.StatesInstance smi)
			{
				smi.SetActive(false);
			}).TagTransition(GameTags.Operational, this.operational, false).PlayAnim("off");
			this.operational.TagTransition(GameTags.Operational, this.unoperational, true).Enter("CreateChore", delegate(Phonobox.StatesInstance smi)
			{
				smi.master.UpdateChores(true);
			}).Exit("CancelChore", delegate(Phonobox.StatesInstance smi)
			{
				smi.master.UpdateChores(false);
			}).DefaultState(this.operational.stopped);
			this.operational.stopped.Enter(delegate(Phonobox.StatesInstance smi)
			{
				smi.SetActive(false);
			}).ParamTransition<int>(this.playerCount, this.operational.pre, (Phonobox.StatesInstance smi, int p) => p > 0).PlayAnim("on");
			this.operational.pre.PlayAnim("working_pre").OnAnimQueueComplete(this.operational.playing);
			this.operational.playing.Enter(delegate(Phonobox.StatesInstance smi)
			{
				smi.SetActive(true);
			}).ScheduleGoTo(25f, this.operational.song_end).ParamTransition<int>(this.playerCount, this.operational.post, (Phonobox.StatesInstance smi, int p) => p == 0).PlayAnim(new Func<Phonobox.StatesInstance, string>(Phonobox.States.GetPlayAnim), KAnim.PlayMode.Loop);
			this.operational.song_end.ParamTransition<int>(this.playerCount, this.operational.bridge, (Phonobox.StatesInstance smi, int p) => p > 0).ParamTransition<int>(this.playerCount, this.operational.post, (Phonobox.StatesInstance smi, int p) => p == 0);
			this.operational.bridge.PlayAnim("working_trans").OnAnimQueueComplete(this.operational.playing);
			this.operational.post.PlayAnim("working_pst").OnAnimQueueComplete(this.operational.stopped);
		}

		// Token: 0x0600791A RID: 31002 RVA: 0x003224D4 File Offset: 0x003206D4
		public static string GetPlayAnim(Phonobox.StatesInstance smi)
		{
			int num = UnityEngine.Random.Range(0, Phonobox.building_anims.Length);
			return Phonobox.building_anims[num];
		}

		// Token: 0x04005AE6 RID: 23270
		public StateMachine<Phonobox.States, Phonobox.StatesInstance, Phonobox, object>.IntParameter playerCount;

		// Token: 0x04005AE7 RID: 23271
		public GameStateMachine<Phonobox.States, Phonobox.StatesInstance, Phonobox, object>.State unoperational;

		// Token: 0x04005AE8 RID: 23272
		public Phonobox.States.OperationalStates operational;

		// Token: 0x020016F0 RID: 5872
		public class OperationalStates : GameStateMachine<Phonobox.States, Phonobox.StatesInstance, Phonobox, object>.State
		{
			// Token: 0x04005AE9 RID: 23273
			public GameStateMachine<Phonobox.States, Phonobox.StatesInstance, Phonobox, object>.State stopped;

			// Token: 0x04005AEA RID: 23274
			public GameStateMachine<Phonobox.States, Phonobox.StatesInstance, Phonobox, object>.State pre;

			// Token: 0x04005AEB RID: 23275
			public GameStateMachine<Phonobox.States, Phonobox.StatesInstance, Phonobox, object>.State bridge;

			// Token: 0x04005AEC RID: 23276
			public GameStateMachine<Phonobox.States, Phonobox.StatesInstance, Phonobox, object>.State playing;

			// Token: 0x04005AED RID: 23277
			public GameStateMachine<Phonobox.States, Phonobox.StatesInstance, Phonobox, object>.State song_end;

			// Token: 0x04005AEE RID: 23278
			public GameStateMachine<Phonobox.States, Phonobox.StatesInstance, Phonobox, object>.State post;
		}
	}

	// Token: 0x020016F2 RID: 5874
	public class StatesInstance : GameStateMachine<Phonobox.States, Phonobox.StatesInstance, Phonobox, object>.GameInstance
	{
		// Token: 0x06007928 RID: 31016 RVA: 0x000F411E File Offset: 0x000F231E
		public StatesInstance(Phonobox smi) : base(smi)
		{
			this.operational = base.master.GetComponent<Operational>();
		}

		// Token: 0x06007929 RID: 31017 RVA: 0x000F4138 File Offset: 0x000F2338
		public void SetActive(bool active)
		{
			this.operational.SetActive(this.operational.IsOperational && active, false);
		}

		// Token: 0x04005AF9 RID: 23289
		private FetchChore chore;

		// Token: 0x04005AFA RID: 23290
		private Operational operational;
	}
}
