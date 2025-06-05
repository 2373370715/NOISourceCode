using System;
using TUNING;

// Token: 0x020015FB RID: 5627
public class PeeChoreMonitor : GameStateMachine<PeeChoreMonitor, PeeChoreMonitor.Instance>
{
	// Token: 0x060074A4 RID: 29860 RVA: 0x00312E20 File Offset: 0x00311020
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.building;
		base.serializable = StateMachine.SerializeType.Both_DEPRECATED;
		this.building.Update(delegate(PeeChoreMonitor.Instance smi, float dt)
		{
			this.pee_fuse.Delta(-dt, smi);
		}, UpdateRate.SIM_200ms, false).Transition(this.paused, (PeeChoreMonitor.Instance smi) => this.IsSleeping(smi), UpdateRate.SIM_200ms).Transition(this.critical, (PeeChoreMonitor.Instance smi) => this.pee_fuse.Get(smi) <= 60f, UpdateRate.SIM_200ms);
		this.critical.Update(delegate(PeeChoreMonitor.Instance smi, float dt)
		{
			this.pee_fuse.Delta(-dt, smi);
		}, UpdateRate.SIM_200ms, false).Transition(this.paused, (PeeChoreMonitor.Instance smi) => this.IsSleeping(smi), UpdateRate.SIM_200ms).Transition(this.pee, (PeeChoreMonitor.Instance smi) => this.pee_fuse.Get(smi) <= 0f, UpdateRate.SIM_200ms).Toggle("Components", delegate(PeeChoreMonitor.Instance smi)
		{
			Components.CriticalBladders.Add(smi);
		}, delegate(PeeChoreMonitor.Instance smi)
		{
			Components.CriticalBladders.Remove(smi);
		});
		this.paused.Transition(this.building, (PeeChoreMonitor.Instance smi) => !this.IsSleeping(smi), UpdateRate.SIM_200ms);
		this.pee.ToggleChore(new Func<PeeChoreMonitor.Instance, Chore>(this.CreatePeeChore), this.building);
	}

	// Token: 0x060074A5 RID: 29861 RVA: 0x00312F58 File Offset: 0x00311158
	private bool IsSleeping(PeeChoreMonitor.Instance smi)
	{
		StaminaMonitor.Instance smi2 = smi.master.gameObject.GetSMI<StaminaMonitor.Instance>();
		if (smi2 != null)
		{
			smi2.IsSleeping();
		}
		return false;
	}

	// Token: 0x060074A6 RID: 29862 RVA: 0x000F113A File Offset: 0x000EF33A
	private Chore CreatePeeChore(PeeChoreMonitor.Instance smi)
	{
		return new PeeChore(smi.master);
	}

	// Token: 0x0400578E RID: 22414
	public GameStateMachine<PeeChoreMonitor, PeeChoreMonitor.Instance, IStateMachineTarget, object>.State building;

	// Token: 0x0400578F RID: 22415
	public GameStateMachine<PeeChoreMonitor, PeeChoreMonitor.Instance, IStateMachineTarget, object>.State critical;

	// Token: 0x04005790 RID: 22416
	public GameStateMachine<PeeChoreMonitor, PeeChoreMonitor.Instance, IStateMachineTarget, object>.State paused;

	// Token: 0x04005791 RID: 22417
	public GameStateMachine<PeeChoreMonitor, PeeChoreMonitor.Instance, IStateMachineTarget, object>.State pee;

	// Token: 0x04005792 RID: 22418
	private StateMachine<PeeChoreMonitor, PeeChoreMonitor.Instance, IStateMachineTarget, object>.FloatParameter pee_fuse = new StateMachine<PeeChoreMonitor, PeeChoreMonitor.Instance, IStateMachineTarget, object>.FloatParameter(DUPLICANTSTATS.STANDARD.Secretions.PEE_FUSE_TIME);

	// Token: 0x020015FC RID: 5628
	public new class Instance : GameStateMachine<PeeChoreMonitor, PeeChoreMonitor.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x060074AF RID: 29871 RVA: 0x000F11BF File Offset: 0x000EF3BF
		public Instance(IStateMachineTarget master) : base(master)
		{
		}

		// Token: 0x060074B0 RID: 29872 RVA: 0x000F11C8 File Offset: 0x000EF3C8
		public bool IsCritical()
		{
			return base.IsInsideState(base.sm.critical);
		}
	}
}
