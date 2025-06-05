using System;

// Token: 0x020015D9 RID: 5593
public class IdleMonitor : GameStateMachine<IdleMonitor, IdleMonitor.Instance>
{
	// Token: 0x06007418 RID: 29720 RVA: 0x000F0937 File Offset: 0x000EEB37
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.idle;
		this.idle.TagTransition(GameTags.Dying, this.stopped, false).ToggleRecurringChore(new Func<IdleMonitor.Instance, Chore>(this.CreateIdleChore), null);
		this.stopped.DoNothing();
	}

	// Token: 0x06007419 RID: 29721 RVA: 0x000F0977 File Offset: 0x000EEB77
	private Chore CreateIdleChore(IdleMonitor.Instance smi)
	{
		return new IdleChore(smi.master);
	}

	// Token: 0x04005729 RID: 22313
	public GameStateMachine<IdleMonitor, IdleMonitor.Instance, IStateMachineTarget, object>.State idle;

	// Token: 0x0400572A RID: 22314
	public GameStateMachine<IdleMonitor, IdleMonitor.Instance, IStateMachineTarget, object>.State stopped;

	// Token: 0x020015DA RID: 5594
	public new class Instance : GameStateMachine<IdleMonitor, IdleMonitor.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x0600741B RID: 29723 RVA: 0x000F098C File Offset: 0x000EEB8C
		public Instance(IStateMachineTarget master) : base(master)
		{
		}
	}
}
