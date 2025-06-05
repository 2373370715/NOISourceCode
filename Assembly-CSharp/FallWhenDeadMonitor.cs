using System;

// Token: 0x02000A84 RID: 2692
public class FallWhenDeadMonitor : GameStateMachine<FallWhenDeadMonitor, FallWhenDeadMonitor.Instance>
{
	// Token: 0x06003104 RID: 12548 RVA: 0x0020C060 File Offset: 0x0020A260
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.standing;
		this.standing.Transition(this.entombed, (FallWhenDeadMonitor.Instance smi) => smi.IsEntombed(), UpdateRate.SIM_200ms).Transition(this.falling, (FallWhenDeadMonitor.Instance smi) => smi.IsFalling(), UpdateRate.SIM_200ms);
		this.falling.ToggleGravity(this.standing);
		this.entombed.Transition(this.standing, (FallWhenDeadMonitor.Instance smi) => !smi.IsEntombed(), UpdateRate.SIM_200ms);
	}

	// Token: 0x040021BE RID: 8638
	public GameStateMachine<FallWhenDeadMonitor, FallWhenDeadMonitor.Instance, IStateMachineTarget, object>.State standing;

	// Token: 0x040021BF RID: 8639
	public GameStateMachine<FallWhenDeadMonitor, FallWhenDeadMonitor.Instance, IStateMachineTarget, object>.State falling;

	// Token: 0x040021C0 RID: 8640
	public GameStateMachine<FallWhenDeadMonitor, FallWhenDeadMonitor.Instance, IStateMachineTarget, object>.State entombed;

	// Token: 0x02000A85 RID: 2693
	public new class Instance : GameStateMachine<FallWhenDeadMonitor, FallWhenDeadMonitor.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x06003106 RID: 12550 RVA: 0x000C450B File Offset: 0x000C270B
		public Instance(IStateMachineTarget master) : base(master)
		{
		}

		// Token: 0x06003107 RID: 12551 RVA: 0x0020C118 File Offset: 0x0020A318
		public bool IsEntombed()
		{
			Pickupable component = base.GetComponent<Pickupable>();
			return component != null && component.IsEntombed;
		}

		// Token: 0x06003108 RID: 12552 RVA: 0x0020C140 File Offset: 0x0020A340
		public bool IsFalling()
		{
			int num = Grid.CellBelow(Grid.PosToCell(base.master.transform.GetPosition()));
			return Grid.IsValidCell(num) && !Grid.Solid[num];
		}
	}
}
