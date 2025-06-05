using System;
using TUNING;

// Token: 0x020007E0 RID: 2016
public class RoboDancer : GameStateMachine<RoboDancer, RoboDancer.Instance>
{
	// Token: 0x060023A0 RID: 9120 RVA: 0x001D299C File Offset: 0x001D0B9C
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.neutral;
		this.root.TagTransition(GameTags.Dead, null, false);
		this.neutral.TagTransition(GameTags.Overjoyed, this.overjoyed, false);
		this.overjoyed.TagTransition(GameTags.Overjoyed, this.neutral, true).DefaultState(this.overjoyed.idle).ParamTransition<float>(this.timeSpentDancing, this.overjoyed.exitEarly, (RoboDancer.Instance smi, float p) => p >= TRAITS.JOY_REACTIONS.ROBO_DANCER.DANCE_DURATION && !this.hasAudience.Get(smi)).Exit(delegate(RoboDancer.Instance smi)
		{
			this.timeSpentDancing.Set(0f, smi, false);
		});
		this.overjoyed.idle.Enter(delegate(RoboDancer.Instance smi)
		{
			if (smi.IsRecTime())
			{
				smi.GoTo(this.overjoyed.dancing);
			}
		}).ToggleStatusItem(Db.Get().DuplicantStatusItems.RoboDancerPlanning, null).EventTransition(GameHashes.ScheduleBlocksTick, this.overjoyed.dancing, (RoboDancer.Instance smi) => smi.IsRecTime());
		this.overjoyed.dancing.ToggleStatusItem(Db.Get().DuplicantStatusItems.RoboDancerDancing, null).EventTransition(GameHashes.ScheduleBlocksTick, this.overjoyed.idle, (RoboDancer.Instance smi) => !smi.IsRecTime()).ToggleChore((RoboDancer.Instance smi) => new RoboDancerChore(smi.master), this.overjoyed.idle);
		this.overjoyed.exitEarly.Enter(delegate(RoboDancer.Instance smi)
		{
			smi.ExitJoyReactionEarly();
		});
	}

	// Token: 0x040017EF RID: 6127
	public StateMachine<RoboDancer, RoboDancer.Instance, IStateMachineTarget, object>.FloatParameter timeSpentDancing;

	// Token: 0x040017F0 RID: 6128
	public StateMachine<RoboDancer, RoboDancer.Instance, IStateMachineTarget, object>.BoolParameter hasAudience;

	// Token: 0x040017F1 RID: 6129
	public GameStateMachine<RoboDancer, RoboDancer.Instance, IStateMachineTarget, object>.State neutral;

	// Token: 0x040017F2 RID: 6130
	public RoboDancer.OverjoyedStates overjoyed;

	// Token: 0x020007E1 RID: 2017
	public class OverjoyedStates : GameStateMachine<RoboDancer, RoboDancer.Instance, IStateMachineTarget, object>.State
	{
		// Token: 0x040017F3 RID: 6131
		public GameStateMachine<RoboDancer, RoboDancer.Instance, IStateMachineTarget, object>.State idle;

		// Token: 0x040017F4 RID: 6132
		public GameStateMachine<RoboDancer, RoboDancer.Instance, IStateMachineTarget, object>.State dancing;

		// Token: 0x040017F5 RID: 6133
		public GameStateMachine<RoboDancer, RoboDancer.Instance, IStateMachineTarget, object>.State exitEarly;
	}

	// Token: 0x020007E2 RID: 2018
	public new class Instance : GameStateMachine<RoboDancer, RoboDancer.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x060023A6 RID: 9126 RVA: 0x000BBA60 File Offset: 0x000B9C60
		public Instance(IStateMachineTarget master) : base(master)
		{
		}

		// Token: 0x060023A7 RID: 9127 RVA: 0x000BBA69 File Offset: 0x000B9C69
		public bool IsRecTime()
		{
			return base.master.GetComponent<Schedulable>().IsAllowed(Db.Get().ScheduleBlockTypes.Recreation);
		}

		// Token: 0x060023A8 RID: 9128 RVA: 0x001D2B54 File Offset: 0x001D0D54
		public void ExitJoyReactionEarly()
		{
			JoyBehaviourMonitor.Instance smi = base.master.gameObject.GetSMI<JoyBehaviourMonitor.Instance>();
			smi.sm.exitEarly.Trigger(smi);
		}
	}
}
