using System;

// Token: 0x02001602 RID: 5634
public class QuarantineFeedableMonitor : GameStateMachine<QuarantineFeedableMonitor, QuarantineFeedableMonitor.Instance>
{
	// Token: 0x060074C3 RID: 29891 RVA: 0x003130D0 File Offset: 0x003112D0
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.satisfied;
		base.serializable = StateMachine.SerializeType.Both_DEPRECATED;
		this.satisfied.EventTransition(GameHashes.AddUrge, this.hungry, (QuarantineFeedableMonitor.Instance smi) => smi.IsHungry());
		this.hungry.EventTransition(GameHashes.RemoveUrge, this.satisfied, (QuarantineFeedableMonitor.Instance smi) => !smi.IsHungry());
	}

	// Token: 0x0400579E RID: 22430
	public GameStateMachine<QuarantineFeedableMonitor, QuarantineFeedableMonitor.Instance, IStateMachineTarget, object>.State satisfied;

	// Token: 0x0400579F RID: 22431
	public GameStateMachine<QuarantineFeedableMonitor, QuarantineFeedableMonitor.Instance, IStateMachineTarget, object>.State hungry;

	// Token: 0x02001603 RID: 5635
	public new class Instance : GameStateMachine<QuarantineFeedableMonitor, QuarantineFeedableMonitor.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x060074C5 RID: 29893 RVA: 0x000F12CC File Offset: 0x000EF4CC
		public Instance(IStateMachineTarget master) : base(master)
		{
		}

		// Token: 0x060074C6 RID: 29894 RVA: 0x000F12D5 File Offset: 0x000EF4D5
		public bool IsHungry()
		{
			return base.GetComponent<ChoreConsumer>().HasUrge(Db.Get().Urges.Eat);
		}
	}
}
