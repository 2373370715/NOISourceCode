using System;

// Token: 0x020015AB RID: 5547
public class EmoteHighPriorityMonitor : GameStateMachine<EmoteHighPriorityMonitor, EmoteHighPriorityMonitor.Instance>
{
	// Token: 0x06007340 RID: 29504 RVA: 0x0030EDF0 File Offset: 0x0030CFF0
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.ready;
		base.serializable = StateMachine.SerializeType.Both_DEPRECATED;
		this.ready.ToggleUrge(Db.Get().Urges.EmoteHighPriority).EventHandler(GameHashes.BeginChore, delegate(EmoteHighPriorityMonitor.Instance smi, object o)
		{
			smi.OnStartChore(o);
		});
		this.resetting.GoTo(this.ready);
	}

	// Token: 0x0400566E RID: 22126
	public GameStateMachine<EmoteHighPriorityMonitor, EmoteHighPriorityMonitor.Instance, IStateMachineTarget, object>.State ready;

	// Token: 0x0400566F RID: 22127
	public GameStateMachine<EmoteHighPriorityMonitor, EmoteHighPriorityMonitor.Instance, IStateMachineTarget, object>.State resetting;

	// Token: 0x020015AC RID: 5548
	public new class Instance : GameStateMachine<EmoteHighPriorityMonitor, EmoteHighPriorityMonitor.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x06007342 RID: 29506 RVA: 0x000EFF6F File Offset: 0x000EE16F
		public Instance(IStateMachineTarget master) : base(master)
		{
		}

		// Token: 0x06007343 RID: 29507 RVA: 0x000EFF78 File Offset: 0x000EE178
		public void OnStartChore(object o)
		{
			if (((Chore)o).SatisfiesUrge(Db.Get().Urges.EmoteHighPriority))
			{
				this.GoTo(base.sm.resetting);
			}
		}
	}
}
