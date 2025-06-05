using System;
using Klei.AI;
using STRINGS;
using TUNING;

// Token: 0x02000710 RID: 1808
public class RancherChore : Chore<RancherChore.RancherChoreStates.Instance>
{
	// Token: 0x06001FE3 RID: 8163 RVA: 0x001C67E8 File Offset: 0x001C49E8
	public RancherChore(KPrefabID rancher_station)
	{
		Chore.Precondition isOpenForRanching = default(Chore.Precondition);
		isOpenForRanching.id = "IsCreatureAvailableForRanching";
		isOpenForRanching.description = DUPLICANTS.CHORES.PRECONDITIONS.IS_CREATURE_AVAILABLE_FOR_RANCHING;
		isOpenForRanching.sortOrder = -3;
		isOpenForRanching.fn = delegate(ref Chore.Precondition.Context context, object data)
		{
			RanchStation.Instance instance = data as RanchStation.Instance;
			return !instance.HasRancher && instance.IsCritterAvailableForRanching;
		};
		this.IsOpenForRanching = isOpenForRanching;
		base..ctor(Db.Get().ChoreTypes.Ranch, rancher_station, null, false, null, null, null, PriorityScreen.PriorityClass.basic, 5, false, true, 0, false, ReportManager.ReportType.WorkTime);
		this.AddPrecondition(this.IsOpenForRanching, rancher_station.GetSMI<RanchStation.Instance>());
		SkillPerkMissingComplainer component = base.GetComponent<SkillPerkMissingComplainer>();
		this.AddPrecondition(ChorePreconditions.instance.HasSkillPerk, component.requiredSkillPerk);
		this.AddPrecondition(ChorePreconditions.instance.IsScheduledTime, Db.Get().ScheduleBlockTypes.Work);
		this.AddPrecondition(ChorePreconditions.instance.CanMoveTo, rancher_station.GetComponent<Building>());
		Operational component2 = rancher_station.GetComponent<Operational>();
		this.AddPrecondition(ChorePreconditions.instance.IsOperational, component2);
		Deconstructable component3 = rancher_station.GetComponent<Deconstructable>();
		this.AddPrecondition(ChorePreconditions.instance.IsNotMarkedForDeconstruction, component3);
		BuildingEnabledButton component4 = rancher_station.GetComponent<BuildingEnabledButton>();
		this.AddPrecondition(ChorePreconditions.instance.IsNotMarkedForDisable, component4);
		base.smi = new RancherChore.RancherChoreStates.Instance(rancher_station);
		base.SetPrioritizable(rancher_station.GetComponent<Prioritizable>());
	}

	// Token: 0x06001FE4 RID: 8164 RVA: 0x000B95FB File Offset: 0x000B77FB
	public override void Begin(Chore.Precondition.Context context)
	{
		base.smi.sm.rancher.Set(context.consumerState.gameObject, base.smi, false);
		base.Begin(context);
	}

	// Token: 0x06001FE5 RID: 8165 RVA: 0x000B962C File Offset: 0x000B782C
	protected override void End(string reason)
	{
		base.End(reason);
		base.smi.sm.rancher.Set(null, base.smi);
	}

	// Token: 0x04001520 RID: 5408
	public Chore.Precondition IsOpenForRanching;

	// Token: 0x02000711 RID: 1809
	public class RancherChoreStates : GameStateMachine<RancherChore.RancherChoreStates, RancherChore.RancherChoreStates.Instance>
	{
		// Token: 0x06001FE6 RID: 8166 RVA: 0x001C693C File Offset: 0x001C4B3C
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.moveToRanch;
			base.Target(this.rancher);
			this.root.Exit("TriggerRanchStationNoLongerAvailable", delegate(RancherChore.RancherChoreStates.Instance smi)
			{
				smi.ranchStation.TriggerRanchStationNoLongerAvailable();
			});
			this.moveToRanch.MoveTo((RancherChore.RancherChoreStates.Instance smi) => Grid.PosToCell(smi.transform.GetPosition()), this.waitForAvailableRanchable, null, false);
			this.waitForAvailableRanchable.Enter("FindRanchable", delegate(RancherChore.RancherChoreStates.Instance smi)
			{
				smi.WaitForAvailableRanchable(0f);
			}).Update("FindRanchable", delegate(RancherChore.RancherChoreStates.Instance smi, float dt)
			{
				smi.WaitForAvailableRanchable(dt);
			}, UpdateRate.SIM_200ms, false);
			this.ranchCritter.ScheduleGoTo(0.5f, this.ranchCritter.callForCritter).EventTransition(GameHashes.CreatureAbandonedRanchStation, this.waitForAvailableRanchable, null);
			this.ranchCritter.callForCritter.ToggleAnims("anim_interacts_rancherstation_kanim", 0f).PlayAnim("calling_loop", KAnim.PlayMode.Loop).ScheduleActionNextFrame("TellCreatureRancherIsReady", delegate(RancherChore.RancherChoreStates.Instance smi)
			{
				smi.ranchStation.MessageRancherReady();
			}).Target(this.masterTarget).EventTransition(GameHashes.CreatureArrivedAtRanchStation, this.ranchCritter.working, null);
			this.ranchCritter.working.ToggleWork<RancherChore.RancherWorkable>(this.masterTarget, this.ranchCritter.pst, this.waitForAvailableRanchable, null);
			this.ranchCritter.pst.ToggleAnims(new Func<RancherChore.RancherChoreStates.Instance, HashedString>(RancherChore.RancherChoreStates.GetRancherInteractAnim)).QueueAnim("wipe_brow", false, null).OnAnimQueueComplete(this.waitForAvailableRanchable);
		}

		// Token: 0x06001FE7 RID: 8167 RVA: 0x000B9651 File Offset: 0x000B7851
		private static HashedString GetRancherInteractAnim(RancherChore.RancherChoreStates.Instance smi)
		{
			return smi.ranchStation.def.RancherInteractAnim;
		}

		// Token: 0x06001FE8 RID: 8168 RVA: 0x001C6B18 File Offset: 0x001C4D18
		public static bool TryRanchCreature(RancherChore.RancherChoreStates.Instance smi)
		{
			Debug.Assert(smi.ranchStation != null, "smi.ranchStation was null");
			RanchedStates.Instance activeRanchable = smi.ranchStation.ActiveRanchable;
			if (activeRanchable.IsNullOrStopped())
			{
				return false;
			}
			KPrefabID component = activeRanchable.GetComponent<KPrefabID>();
			smi.sm.rancher.Get(smi).Trigger(937885943, component.PrefabTag.Name);
			smi.ranchStation.RanchCreature();
			return true;
		}

		// Token: 0x04001521 RID: 5409
		public StateMachine<RancherChore.RancherChoreStates, RancherChore.RancherChoreStates.Instance, IStateMachineTarget, object>.TargetParameter rancher;

		// Token: 0x04001522 RID: 5410
		private GameStateMachine<RancherChore.RancherChoreStates, RancherChore.RancherChoreStates.Instance, IStateMachineTarget, object>.State moveToRanch;

		// Token: 0x04001523 RID: 5411
		private RancherChore.RancherChoreStates.RanchState ranchCritter;

		// Token: 0x04001524 RID: 5412
		private GameStateMachine<RancherChore.RancherChoreStates, RancherChore.RancherChoreStates.Instance, IStateMachineTarget, object>.State waitForAvailableRanchable;

		// Token: 0x02000712 RID: 1810
		private class RanchState : GameStateMachine<RancherChore.RancherChoreStates, RancherChore.RancherChoreStates.Instance, IStateMachineTarget, object>.State
		{
			// Token: 0x04001525 RID: 5413
			public GameStateMachine<RancherChore.RancherChoreStates, RancherChore.RancherChoreStates.Instance, IStateMachineTarget, object>.State callForCritter;

			// Token: 0x04001526 RID: 5414
			public GameStateMachine<RancherChore.RancherChoreStates, RancherChore.RancherChoreStates.Instance, IStateMachineTarget, object>.State working;

			// Token: 0x04001527 RID: 5415
			public GameStateMachine<RancherChore.RancherChoreStates, RancherChore.RancherChoreStates.Instance, IStateMachineTarget, object>.State pst;
		}

		// Token: 0x02000713 RID: 1811
		public new class Instance : GameStateMachine<RancherChore.RancherChoreStates, RancherChore.RancherChoreStates.Instance, IStateMachineTarget, object>.GameInstance
		{
			// Token: 0x06001FEB RID: 8171 RVA: 0x000B9673 File Offset: 0x000B7873
			public Instance(KPrefabID rancher_station) : base(rancher_station)
			{
				this.ranchStation = rancher_station.GetSMI<RanchStation.Instance>();
			}

			// Token: 0x06001FEC RID: 8172 RVA: 0x001C6B88 File Offset: 0x001C4D88
			public void WaitForAvailableRanchable(float dt)
			{
				this.waitTime += dt;
				GameStateMachine<RancherChore.RancherChoreStates, RancherChore.RancherChoreStates.Instance, IStateMachineTarget, object>.State state = this.ranchStation.IsCritterAvailableForRanching ? base.sm.ranchCritter : null;
				if (state != null || this.waitTime >= 2f)
				{
					this.waitTime = 0f;
					this.GoTo(state);
				}
			}

			// Token: 0x04001528 RID: 5416
			private const float WAIT_FOR_RANCHABLE_TIMEOUT = 2f;

			// Token: 0x04001529 RID: 5417
			public RanchStation.Instance ranchStation;

			// Token: 0x0400152A RID: 5418
			private float waitTime;
		}
	}

	// Token: 0x02000715 RID: 1813
	public class RancherWorkable : Workable
	{
		// Token: 0x06001FF4 RID: 8180 RVA: 0x001C6BE4 File Offset: 0x001C4DE4
		protected override void OnPrefabInit()
		{
			base.OnPrefabInit();
			this.ranch = base.gameObject.GetSMI<RanchStation.Instance>();
			this.overrideAnims = new KAnimFile[]
			{
				Assets.GetAnim(this.ranch.def.RancherInteractAnim)
			};
			base.SetWorkTime(this.ranch.def.WorkTime);
			base.SetWorkerStatusItem(this.ranch.def.RanchingStatusItem);
			this.attributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.MOST_DAY_EXPERIENCE;
			this.skillExperienceSkillGroup = Db.Get().SkillGroups.Ranching.Id;
			this.skillExperienceMultiplier = SKILLS.MOST_DAY_EXPERIENCE;
			this.lightEfficiencyBonus = false;
		}

		// Token: 0x06001FF5 RID: 8181 RVA: 0x000B96C4 File Offset: 0x000B78C4
		public override Klei.AI.Attribute GetWorkAttribute()
		{
			return Db.Get().Attributes.Ranching;
		}

		// Token: 0x06001FF6 RID: 8182 RVA: 0x001C6C90 File Offset: 0x001C4E90
		protected override void OnStartWork(WorkerBase worker)
		{
			if (this.ranch == null)
			{
				return;
			}
			this.critterAnimController = this.ranch.ActiveRanchable.AnimController;
			this.critterAnimController.Play(this.ranch.def.RanchedPreAnim, KAnim.PlayMode.Once, 1f, 0f);
			this.critterAnimController.Queue(this.ranch.def.RanchedLoopAnim, KAnim.PlayMode.Loop, 1f, 0f);
		}

		// Token: 0x06001FF7 RID: 8183 RVA: 0x001C6D08 File Offset: 0x001C4F08
		protected override bool OnWorkTick(WorkerBase worker, float dt)
		{
			if (this.ranch.def.OnRanchWorkTick != null)
			{
				this.ranch.def.OnRanchWorkTick(this.ranch.ActiveRanchable.gameObject, dt, this);
			}
			return base.OnWorkTick(worker, dt);
		}

		// Token: 0x06001FF8 RID: 8184 RVA: 0x001C6D58 File Offset: 0x001C4F58
		public override void OnPendingCompleteWork(WorkerBase work)
		{
			RancherChore.RancherChoreStates.Instance smi = base.gameObject.GetSMI<RancherChore.RancherChoreStates.Instance>();
			if (this.ranch == null || smi == null)
			{
				return;
			}
			if (RancherChore.RancherChoreStates.TryRanchCreature(smi))
			{
				this.critterAnimController.Play(this.ranch.def.RanchedPstAnim, KAnim.PlayMode.Once, 1f, 0f);
			}
		}

		// Token: 0x06001FF9 RID: 8185 RVA: 0x000B96D5 File Offset: 0x000B78D5
		protected override void OnAbortWork(WorkerBase worker)
		{
			if (this.ranch == null || this.critterAnimController == null)
			{
				return;
			}
			this.critterAnimController.Play(this.ranch.def.RanchedAbortAnim, KAnim.PlayMode.Once, 1f, 0f);
		}

		// Token: 0x04001531 RID: 5425
		private RanchStation.Instance ranch;

		// Token: 0x04001532 RID: 5426
		private KBatchedAnimController critterAnimController;
	}
}
