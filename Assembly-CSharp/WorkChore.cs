using System;
using UnityEngine;

// Token: 0x02000780 RID: 1920
public class WorkChore<WorkableType> : Chore<WorkChore<WorkableType>.StatesInstance> where WorkableType : Workable
{
	// Token: 0x170000DA RID: 218
	// (get) Token: 0x06002181 RID: 8577 RVA: 0x000BA52E File Offset: 0x000B872E
	// (set) Token: 0x06002182 RID: 8578 RVA: 0x000BA536 File Offset: 0x000B8736
	public bool onlyWhenOperational { get; private set; }

	// Token: 0x06002183 RID: 8579 RVA: 0x000BA53F File Offset: 0x000B873F
	public override string ToString()
	{
		return "WorkChore<" + typeof(WorkableType).ToString() + ">";
	}

	// Token: 0x06002184 RID: 8580 RVA: 0x001CD4DC File Offset: 0x001CB6DC
	public WorkChore(ChoreType chore_type, IStateMachineTarget target, ChoreProvider chore_provider = null, bool run_until_complete = true, Action<Chore> on_complete = null, Action<Chore> on_begin = null, Action<Chore> on_end = null, bool allow_in_red_alert = true, ScheduleBlockType schedule_block = null, bool ignore_schedule_block = false, bool only_when_operational = true, KAnimFile override_anims = null, bool is_preemptable = false, bool allow_in_context_menu = true, bool allow_prioritization = true, PriorityScreen.PriorityClass priority_class = PriorityScreen.PriorityClass.basic, int priority_class_value = 5, bool ignore_building_assignment = false, bool add_to_daily_report = true) : base(chore_type, target, chore_provider, run_until_complete, on_complete, on_begin, on_end, priority_class, priority_class_value, is_preemptable, allow_in_context_menu, 0, add_to_daily_report, ReportManager.ReportType.WorkTime)
	{
		base.smi = new WorkChore<WorkableType>.StatesInstance(this, target.gameObject, override_anims);
		this.onlyWhenOperational = only_when_operational;
		if (allow_prioritization)
		{
			base.SetPrioritizable(target.GetComponent<Prioritizable>());
		}
		this.AddPrecondition(ChorePreconditions.instance.IsNotTransferArm, null);
		if (!allow_in_red_alert)
		{
			this.AddPrecondition(ChorePreconditions.instance.IsNotRedAlert, null);
		}
		if (schedule_block != null)
		{
			this.AddPrecondition(ChorePreconditions.instance.IsScheduledTime, schedule_block);
		}
		else if (!ignore_schedule_block)
		{
			this.AddPrecondition(ChorePreconditions.instance.IsScheduledTime, Db.Get().ScheduleBlockTypes.Work);
		}
		this.AddPrecondition(ChorePreconditions.instance.CanMoveTo, base.smi.sm.workable.Get<WorkableType>(base.smi));
		Operational component = target.GetComponent<Operational>();
		if (only_when_operational && component != null)
		{
			this.AddPrecondition(ChorePreconditions.instance.IsOperational, component);
		}
		if (only_when_operational)
		{
			Deconstructable component2 = target.GetComponent<Deconstructable>();
			if (component2 != null)
			{
				this.AddPrecondition(ChorePreconditions.instance.IsNotMarkedForDeconstruction, component2);
			}
			BuildingEnabledButton component3 = target.GetComponent<BuildingEnabledButton>();
			if (component3 != null)
			{
				this.AddPrecondition(ChorePreconditions.instance.IsNotMarkedForDisable, component3);
			}
		}
		if (!ignore_building_assignment && base.smi.sm.workable.Get(base.smi).GetComponent<Assignable>() != null)
		{
			this.AddPrecondition(ChorePreconditions.instance.IsAssignedtoMe, base.smi.sm.workable.Get<Assignable>(base.smi));
		}
		if (override_anims != null)
		{
			this.AddPrecondition(ChorePreconditions.instance.IsNotARobot, null);
		}
		WorkableType workableType = target as WorkableType;
		if (workableType != null)
		{
			if (!string.IsNullOrEmpty(workableType.requiredSkillPerk))
			{
				HashedString hashedString = workableType.requiredSkillPerk;
				this.AddPrecondition(ChorePreconditions.instance.HasSkillPerk, hashedString);
			}
			if (workableType.requireMinionToWork)
			{
				this.AddPrecondition(ChorePreconditions.instance.IsMinion, null);
			}
		}
	}

	// Token: 0x06002185 RID: 8581 RVA: 0x000BA55F File Offset: 0x000B875F
	public override void Begin(Chore.Precondition.Context context)
	{
		base.smi.sm.worker.Set(context.consumerState.gameObject, base.smi, false);
		base.Begin(context);
	}

	// Token: 0x06002186 RID: 8582 RVA: 0x001CD720 File Offset: 0x001CB920
	public override bool IsValid()
	{
		WorkableType workableType = this.target as WorkableType;
		if (workableType != null)
		{
			return this.provider != null && Grid.IsWorldValidCell(workableType.GetCell());
		}
		return base.IsValid();
	}

	// Token: 0x06002187 RID: 8583 RVA: 0x001CD774 File Offset: 0x001CB974
	public bool IsOperationalValid()
	{
		if (this.onlyWhenOperational)
		{
			Operational component = base.smi.master.GetComponent<Operational>();
			if (component != null && !component.IsOperational)
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x06002188 RID: 8584 RVA: 0x001CD7B0 File Offset: 0x001CB9B0
	public override bool CanPreempt(Chore.Precondition.Context context)
	{
		if (!base.CanPreempt(context))
		{
			return false;
		}
		if (context.chore.driver == null)
		{
			return false;
		}
		if (context.chore.driver == context.consumerState.choreDriver)
		{
			return false;
		}
		Workable workable = base.smi.sm.workable.Get<WorkableType>(base.smi);
		if (workable == null)
		{
			return false;
		}
		if (workable.worker != null && (workable.worker.GetState() == WorkerBase.State.PendingCompletion || workable.worker.GetState() == WorkerBase.State.Completing))
		{
			return false;
		}
		if (this.preemption_cb != null)
		{
			if (!this.preemption_cb(context))
			{
				return false;
			}
		}
		else
		{
			int num = 4;
			int navigationCost = context.chore.driver.GetComponent<Navigator>().GetNavigationCost(workable);
			if (navigationCost == -1 || navigationCost < num)
			{
				return false;
			}
			if (context.consumerState.navigator.GetNavigationCost(workable) * 2 > navigationCost)
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x04001690 RID: 5776
	public Func<Chore.Precondition.Context, bool> preemption_cb;

	// Token: 0x02000781 RID: 1921
	public class StatesInstance : GameStateMachine<WorkChore<WorkableType>.States, WorkChore<WorkableType>.StatesInstance, WorkChore<WorkableType>, object>.GameInstance
	{
		// Token: 0x06002189 RID: 8585 RVA: 0x000BA590 File Offset: 0x000B8790
		public StatesInstance(WorkChore<WorkableType> master, GameObject workable, KAnimFile override_anims) : base(master)
		{
			this.overrideAnims = override_anims;
			base.sm.workable.Set(workable, base.smi, false);
		}

		// Token: 0x0600218A RID: 8586 RVA: 0x000BA5B9 File Offset: 0x000B87B9
		public void EnableAnimOverrides()
		{
			if (this.overrideAnims != null)
			{
				base.sm.worker.Get<KAnimControllerBase>(base.smi).AddAnimOverrides(this.overrideAnims, 0f);
			}
		}

		// Token: 0x0600218B RID: 8587 RVA: 0x000BA5EF File Offset: 0x000B87EF
		public void DisableAnimOverrides()
		{
			if (this.overrideAnims != null)
			{
				base.sm.worker.Get<KAnimControllerBase>(base.smi).RemoveAnimOverrides(this.overrideAnims);
			}
		}

		// Token: 0x04001692 RID: 5778
		private KAnimFile overrideAnims;
	}

	// Token: 0x02000782 RID: 1922
	public class States : GameStateMachine<WorkChore<WorkableType>.States, WorkChore<WorkableType>.StatesInstance, WorkChore<WorkableType>>
	{
		// Token: 0x0600218C RID: 8588 RVA: 0x001CD8A8 File Offset: 0x001CBAA8
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.approach;
			base.Target(this.worker);
			this.approach.InitializeStates(this.worker, this.workable, this.work, null, null, null).Update("CheckOperational", delegate(WorkChore<WorkableType>.StatesInstance smi, float dt)
			{
				if (!smi.master.IsOperationalValid())
				{
					smi.StopSM("Building not operational");
				}
			}, UpdateRate.SIM_200ms, false);
			this.work.Enter(delegate(WorkChore<WorkableType>.StatesInstance smi)
			{
				smi.EnableAnimOverrides();
			}).ToggleWork<WorkableType>(this.workable, this.success, null, (WorkChore<WorkableType>.StatesInstance smi) => smi.master.IsOperationalValid()).Exit(delegate(WorkChore<WorkableType>.StatesInstance smi)
			{
				smi.DisableAnimOverrides();
			});
			this.success.ReturnSuccess();
		}

		// Token: 0x04001693 RID: 5779
		public GameStateMachine<WorkChore<WorkableType>.States, WorkChore<WorkableType>.StatesInstance, WorkChore<WorkableType>, object>.ApproachSubState<WorkableType> approach;

		// Token: 0x04001694 RID: 5780
		public GameStateMachine<WorkChore<WorkableType>.States, WorkChore<WorkableType>.StatesInstance, WorkChore<WorkableType>, object>.State work;

		// Token: 0x04001695 RID: 5781
		public GameStateMachine<WorkChore<WorkableType>.States, WorkChore<WorkableType>.StatesInstance, WorkChore<WorkableType>, object>.State success;

		// Token: 0x04001696 RID: 5782
		public StateMachine<WorkChore<WorkableType>.States, WorkChore<WorkableType>.StatesInstance, WorkChore<WorkableType>, object>.TargetParameter workable;

		// Token: 0x04001697 RID: 5783
		public StateMachine<WorkChore<WorkableType>.States, WorkChore<WorkableType>.StatesInstance, WorkChore<WorkableType>, object>.TargetParameter worker;
	}
}
