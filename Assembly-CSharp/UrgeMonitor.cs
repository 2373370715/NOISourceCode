using System;
using Klei.AI;

// Token: 0x0200166B RID: 5739
public class UrgeMonitor : GameStateMachine<UrgeMonitor, UrgeMonitor.Instance>
{
	// Token: 0x060076A2 RID: 30370 RVA: 0x00319048 File Offset: 0x00317248
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.satisfied;
		this.satisfied.Transition(this.hasurge, (UrgeMonitor.Instance smi) => smi.HasUrge(), UpdateRate.SIM_200ms);
		this.hasurge.Transition(this.satisfied, (UrgeMonitor.Instance smi) => !smi.HasUrge(), UpdateRate.SIM_200ms).ToggleUrge((UrgeMonitor.Instance smi) => smi.GetUrge());
	}

	// Token: 0x04005933 RID: 22835
	public GameStateMachine<UrgeMonitor, UrgeMonitor.Instance, IStateMachineTarget, object>.State satisfied;

	// Token: 0x04005934 RID: 22836
	public GameStateMachine<UrgeMonitor, UrgeMonitor.Instance, IStateMachineTarget, object>.State hasurge;

	// Token: 0x0200166C RID: 5740
	public new class Instance : GameStateMachine<UrgeMonitor, UrgeMonitor.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x060076A4 RID: 30372 RVA: 0x003190E8 File Offset: 0x003172E8
		public Instance(IStateMachineTarget master, Urge urge, Amount amount, ScheduleBlockType schedule_block, float in_schedule_threshold, float out_of_schedule_threshold, bool is_threshold_minimum) : base(master)
		{
			this.urge = urge;
			this.scheduleBlock = schedule_block;
			this.schedulable = base.GetComponent<Schedulable>();
			this.amountInstance = base.gameObject.GetAmounts().Get(amount);
			this.isThresholdMinimum = is_threshold_minimum;
			this.inScheduleThreshold = in_schedule_threshold;
			this.outOfScheduleThreshold = out_of_schedule_threshold;
		}

		// Token: 0x060076A5 RID: 30373 RVA: 0x000F2860 File Offset: 0x000F0A60
		private float GetThreshold()
		{
			if (this.schedulable.IsAllowed(this.scheduleBlock))
			{
				return this.inScheduleThreshold;
			}
			return this.outOfScheduleThreshold;
		}

		// Token: 0x060076A6 RID: 30374 RVA: 0x000F2882 File Offset: 0x000F0A82
		public Urge GetUrge()
		{
			return this.urge;
		}

		// Token: 0x060076A7 RID: 30375 RVA: 0x000F288A File Offset: 0x000F0A8A
		public bool HasUrge()
		{
			if (this.isThresholdMinimum)
			{
				return this.amountInstance.value >= this.GetThreshold();
			}
			return this.amountInstance.value <= this.GetThreshold();
		}

		// Token: 0x04005935 RID: 22837
		private AmountInstance amountInstance;

		// Token: 0x04005936 RID: 22838
		private Urge urge;

		// Token: 0x04005937 RID: 22839
		private ScheduleBlockType scheduleBlock;

		// Token: 0x04005938 RID: 22840
		private Schedulable schedulable;

		// Token: 0x04005939 RID: 22841
		private float inScheduleThreshold;

		// Token: 0x0400593A RID: 22842
		private float outOfScheduleThreshold;

		// Token: 0x0400593B RID: 22843
		private bool isThresholdMinimum;
	}
}
