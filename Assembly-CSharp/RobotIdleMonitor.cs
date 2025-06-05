using System;

// Token: 0x02001206 RID: 4614
public class RobotIdleMonitor : GameStateMachine<RobotIdleMonitor, RobotIdleMonitor.Instance>
{
	// Token: 0x06005DBC RID: 23996 RVA: 0x002ADC90 File Offset: 0x002ABE90
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.idle;
		this.idle.PlayAnim("idle_loop", KAnim.PlayMode.Loop).Transition(this.working, (RobotIdleMonitor.Instance smi) => !RobotIdleMonitor.CheckShouldIdle(smi), UpdateRate.SIM_200ms);
		this.working.Transition(this.idle, (RobotIdleMonitor.Instance smi) => RobotIdleMonitor.CheckShouldIdle(smi), UpdateRate.SIM_200ms);
	}

	// Token: 0x06005DBD RID: 23997 RVA: 0x002ADD14 File Offset: 0x002ABF14
	private static bool CheckShouldIdle(RobotIdleMonitor.Instance smi)
	{
		FallMonitor.Instance smi2 = smi.master.gameObject.GetSMI<FallMonitor.Instance>();
		return smi2 == null || (!smi.master.gameObject.GetComponent<ChoreConsumer>().choreDriver.HasChore() && smi2.GetCurrentState() == smi2.sm.standing);
	}

	// Token: 0x040042E0 RID: 17120
	public GameStateMachine<RobotIdleMonitor, RobotIdleMonitor.Instance, IStateMachineTarget, object>.State idle;

	// Token: 0x040042E1 RID: 17121
	public GameStateMachine<RobotIdleMonitor, RobotIdleMonitor.Instance, IStateMachineTarget, object>.State working;

	// Token: 0x02001207 RID: 4615
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x02001208 RID: 4616
	public new class Instance : GameStateMachine<RobotIdleMonitor, RobotIdleMonitor.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x06005DC0 RID: 24000 RVA: 0x000E19C1 File Offset: 0x000DFBC1
		public Instance(IStateMachineTarget master, RobotIdleMonitor.Def def) : base(master)
		{
		}

		// Token: 0x040042E2 RID: 17122
		public KBatchedAnimController eyes;
	}
}
