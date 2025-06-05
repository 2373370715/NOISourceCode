using System;

// Token: 0x020015EF RID: 5615
public class MingleMonitor : GameStateMachine<MingleMonitor, MingleMonitor.Instance>
{
	// Token: 0x0600746F RID: 29807 RVA: 0x000F0E21 File Offset: 0x000EF021
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.mingle;
		base.serializable = StateMachine.SerializeType.Never;
		this.mingle.ToggleRecurringChore(new Func<MingleMonitor.Instance, Chore>(this.CreateMingleChore), null);
	}

	// Token: 0x06007470 RID: 29808 RVA: 0x000F0E4B File Offset: 0x000EF04B
	private Chore CreateMingleChore(MingleMonitor.Instance smi)
	{
		return new MingleChore(smi.master);
	}

	// Token: 0x04005778 RID: 22392
	public GameStateMachine<MingleMonitor, MingleMonitor.Instance, IStateMachineTarget, object>.State mingle;

	// Token: 0x020015F0 RID: 5616
	public new class Instance : GameStateMachine<MingleMonitor, MingleMonitor.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x06007472 RID: 29810 RVA: 0x000F0E60 File Offset: 0x000EF060
		public Instance(IStateMachineTarget master) : base(master)
		{
		}
	}
}
