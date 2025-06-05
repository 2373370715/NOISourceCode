using System;
using System.Collections.Generic;
using Klei.AI;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x02000C4F RID: 3151
[SerializationConfig(MemberSerialization.OptIn)]
public class ArcadeMachine : StateMachineComponent<ArcadeMachine.StatesInstance>, IGameObjectEffectDescriptor
{
	// Token: 0x06003B83 RID: 15235 RVA: 0x00238DE0 File Offset: 0x00236FE0
	protected override void OnSpawn()
	{
		base.OnSpawn();
		GameScheduler.Instance.Schedule("Scheduling Tutorial", 2f, delegate(object obj)
		{
			Tutorial.Instance.TutorialMessage(Tutorial.TutorialMessages.TM_Schedule, true);
		}, null, null);
		this.workables = new ArcadeMachineWorkable[this.choreOffsets.Length];
		this.chores = new Chore[this.choreOffsets.Length];
		for (int i = 0; i < this.workables.Length; i++)
		{
			Vector3 pos = Grid.CellToPosCBC(Grid.OffsetCell(Grid.PosToCell(this), this.choreOffsets[i]), Grid.SceneLayer.Move);
			GameObject go = ChoreHelpers.CreateLocator("ArcadeMachineWorkable", pos);
			ArcadeMachineWorkable arcadeMachineWorkable = go.AddOrGet<ArcadeMachineWorkable>();
			KSelectable kselectable = go.AddOrGet<KSelectable>();
			kselectable.SetName(this.GetProperName());
			kselectable.IsSelectable = false;
			int player_index = i;
			ArcadeMachineWorkable arcadeMachineWorkable2 = arcadeMachineWorkable;
			arcadeMachineWorkable2.OnWorkableEventCB = (Action<Workable, Workable.WorkableEvent>)Delegate.Combine(arcadeMachineWorkable2.OnWorkableEventCB, new Action<Workable, Workable.WorkableEvent>(delegate(Workable workable, Workable.WorkableEvent ev)
			{
				this.OnWorkableEvent(player_index, ev);
			}));
			arcadeMachineWorkable.overrideAnims = this.overrideAnims[i];
			arcadeMachineWorkable.workAnims = this.workAnims[i];
			this.workables[i] = arcadeMachineWorkable;
			this.workables[i].owner = this;
		}
		base.smi.StartSM();
	}

	// Token: 0x06003B84 RID: 15236 RVA: 0x00238F24 File Offset: 0x00237124
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

	// Token: 0x06003B85 RID: 15237 RVA: 0x00238F78 File Offset: 0x00237178
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
		WorkChore<ArcadeMachineWorkable> workChore = new WorkChore<ArcadeMachineWorkable>(relax, target, chore_provider, run_until_complete, on_complete, on_begin, new Action<Chore>(this.OnSocialChoreEnd), false, recreation, false, true, null, false, true, false, PriorityScreen.PriorityClass.high, 5, false, true);
		workChore.AddPrecondition(ChorePreconditions.instance.CanDoWorkerPrioritizable, workable);
		return workChore;
	}

	// Token: 0x06003B86 RID: 15238 RVA: 0x000CAE6F File Offset: 0x000C906F
	private void OnSocialChoreEnd(Chore chore)
	{
		if (base.gameObject.HasTag(GameTags.Operational))
		{
			this.UpdateChores(true);
		}
	}

	// Token: 0x06003B87 RID: 15239 RVA: 0x00238FE0 File Offset: 0x002371E0
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

	// Token: 0x06003B88 RID: 15240 RVA: 0x00239040 File Offset: 0x00237240
	public void OnWorkableEvent(int player, Workable.WorkableEvent ev)
	{
		if (ev == Workable.WorkableEvent.WorkStarted)
		{
			this.players.Add(player);
		}
		else
		{
			this.players.Remove(player);
		}
		base.smi.sm.playerCount.Set(this.players.Count, base.smi, false);
	}

	// Token: 0x06003B89 RID: 15241 RVA: 0x00239098 File Offset: 0x00237298
	List<Descriptor> IGameObjectEffectDescriptor.GetDescriptors(GameObject go)
	{
		List<Descriptor> list = new List<Descriptor>();
		Descriptor item = default(Descriptor);
		item.SetupDescriptor(UI.BUILDINGEFFECTS.RECREATION, UI.BUILDINGEFFECTS.TOOLTIPS.RECREATION, Descriptor.DescriptorType.Effect);
		list.Add(item);
		Effect.AddModifierDescriptions(base.gameObject, list, "PlayedArcade", true);
		return list;
	}

	// Token: 0x0400293F RID: 10559
	public CellOffset[] choreOffsets = new CellOffset[]
	{
		new CellOffset(-1, 0),
		new CellOffset(1, 0)
	};

	// Token: 0x04002940 RID: 10560
	private ArcadeMachineWorkable[] workables;

	// Token: 0x04002941 RID: 10561
	private Chore[] chores;

	// Token: 0x04002942 RID: 10562
	public HashSet<int> players = new HashSet<int>();

	// Token: 0x04002943 RID: 10563
	public KAnimFile[][] overrideAnims = new KAnimFile[][]
	{
		new KAnimFile[]
		{
			Assets.GetAnim("anim_interacts_arcade_cabinet_playerone_kanim")
		},
		new KAnimFile[]
		{
			Assets.GetAnim("anim_interacts_arcade_cabinet_playertwo_kanim")
		}
	};

	// Token: 0x04002944 RID: 10564
	public HashedString[][] workAnims = new HashedString[][]
	{
		new HashedString[]
		{
			"working_pre",
			"working_loop_one_p"
		},
		new HashedString[]
		{
			"working_pre",
			"working_loop_two_p"
		}
	};

	// Token: 0x02000C50 RID: 3152
	public class States : GameStateMachine<ArcadeMachine.States, ArcadeMachine.StatesInstance, ArcadeMachine>
	{
		// Token: 0x06003B8B RID: 15243 RVA: 0x002391D8 File Offset: 0x002373D8
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.unoperational;
			this.unoperational.Enter(delegate(ArcadeMachine.StatesInstance smi)
			{
				smi.SetActive(false);
			}).TagTransition(GameTags.Operational, this.operational, false).PlayAnim("off");
			this.operational.TagTransition(GameTags.Operational, this.unoperational, true).Enter("CreateChore", delegate(ArcadeMachine.StatesInstance smi)
			{
				smi.master.UpdateChores(true);
			}).Exit("CancelChore", delegate(ArcadeMachine.StatesInstance smi)
			{
				smi.master.UpdateChores(false);
			}).DefaultState(this.operational.stopped);
			this.operational.stopped.Enter(delegate(ArcadeMachine.StatesInstance smi)
			{
				smi.SetActive(false);
			}).PlayAnim("on").ParamTransition<int>(this.playerCount, this.operational.pre, (ArcadeMachine.StatesInstance smi, int p) => p > 0);
			this.operational.pre.Enter(delegate(ArcadeMachine.StatesInstance smi)
			{
				smi.SetActive(true);
			}).PlayAnim("working_pre").OnAnimQueueComplete(this.operational.playing);
			this.operational.playing.PlayAnim(new Func<ArcadeMachine.StatesInstance, string>(this.GetPlayingAnim), KAnim.PlayMode.Loop).ParamTransition<int>(this.playerCount, this.operational.post, (ArcadeMachine.StatesInstance smi, int p) => p == 0).ParamTransition<int>(this.playerCount, this.operational.playing_coop, (ArcadeMachine.StatesInstance smi, int p) => p > 1);
			this.operational.playing_coop.PlayAnim(new Func<ArcadeMachine.StatesInstance, string>(this.GetPlayingAnim), KAnim.PlayMode.Loop).ParamTransition<int>(this.playerCount, this.operational.post, (ArcadeMachine.StatesInstance smi, int p) => p == 0).ParamTransition<int>(this.playerCount, this.operational.playing, (ArcadeMachine.StatesInstance smi, int p) => p == 1);
			this.operational.post.PlayAnim("working_pst").OnAnimQueueComplete(this.operational.stopped);
		}

		// Token: 0x06003B8C RID: 15244 RVA: 0x0023949C File Offset: 0x0023769C
		private string GetPlayingAnim(ArcadeMachine.StatesInstance smi)
		{
			bool flag = smi.master.players.Contains(0);
			bool flag2 = smi.master.players.Contains(1);
			if (flag && !flag2)
			{
				return "working_loop_one_p";
			}
			if (flag2 && !flag)
			{
				return "working_loop_two_p";
			}
			return "working_loop_coop_p";
		}

		// Token: 0x04002945 RID: 10565
		public StateMachine<ArcadeMachine.States, ArcadeMachine.StatesInstance, ArcadeMachine, object>.IntParameter playerCount;

		// Token: 0x04002946 RID: 10566
		public GameStateMachine<ArcadeMachine.States, ArcadeMachine.StatesInstance, ArcadeMachine, object>.State unoperational;

		// Token: 0x04002947 RID: 10567
		public ArcadeMachine.States.OperationalStates operational;

		// Token: 0x02000C51 RID: 3153
		public class OperationalStates : GameStateMachine<ArcadeMachine.States, ArcadeMachine.StatesInstance, ArcadeMachine, object>.State
		{
			// Token: 0x04002948 RID: 10568
			public GameStateMachine<ArcadeMachine.States, ArcadeMachine.StatesInstance, ArcadeMachine, object>.State stopped;

			// Token: 0x04002949 RID: 10569
			public GameStateMachine<ArcadeMachine.States, ArcadeMachine.StatesInstance, ArcadeMachine, object>.State pre;

			// Token: 0x0400294A RID: 10570
			public GameStateMachine<ArcadeMachine.States, ArcadeMachine.StatesInstance, ArcadeMachine, object>.State playing;

			// Token: 0x0400294B RID: 10571
			public GameStateMachine<ArcadeMachine.States, ArcadeMachine.StatesInstance, ArcadeMachine, object>.State playing_coop;

			// Token: 0x0400294C RID: 10572
			public GameStateMachine<ArcadeMachine.States, ArcadeMachine.StatesInstance, ArcadeMachine, object>.State post;
		}
	}

	// Token: 0x02000C53 RID: 3155
	public class StatesInstance : GameStateMachine<ArcadeMachine.States, ArcadeMachine.StatesInstance, ArcadeMachine, object>.GameInstance
	{
		// Token: 0x06003B9B RID: 15259 RVA: 0x000CAED4 File Offset: 0x000C90D4
		public StatesInstance(ArcadeMachine smi) : base(smi)
		{
			this.operational = base.master.GetComponent<Operational>();
		}

		// Token: 0x06003B9C RID: 15260 RVA: 0x000CAEEE File Offset: 0x000C90EE
		public void SetActive(bool active)
		{
			this.operational.SetActive(this.operational.IsOperational && active, false);
		}

		// Token: 0x04002958 RID: 10584
		private Operational operational;
	}
}
