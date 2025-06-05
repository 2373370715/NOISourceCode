using System;

// Token: 0x02000078 RID: 120
public class PoweredController : GameStateMachine<PoweredController, PoweredController.Instance>
{
	// Token: 0x060001F4 RID: 500 RVA: 0x0014E084 File Offset: 0x0014C284
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.off;
		this.off.PlayAnim("off").EventTransition(GameHashes.OperationalChanged, this.on, (PoweredController.Instance smi) => smi.GetComponent<Operational>().IsOperational);
		this.on.PlayAnim("on").EventTransition(GameHashes.OperationalChanged, this.off, (PoweredController.Instance smi) => !smi.GetComponent<Operational>().IsOperational);
	}

	// Token: 0x04000143 RID: 323
	public GameStateMachine<PoweredController, PoweredController.Instance, IStateMachineTarget, object>.State off;

	// Token: 0x04000144 RID: 324
	public GameStateMachine<PoweredController, PoweredController.Instance, IStateMachineTarget, object>.State on;

	// Token: 0x02000079 RID: 121
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x0200007A RID: 122
	public new class Instance : GameStateMachine<PoweredController, PoweredController.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x060001F7 RID: 503 RVA: 0x000AAB17 File Offset: 0x000A8D17
		public Instance(IStateMachineTarget master, PoweredController.Def def) : base(master, def)
		{
		}

		// Token: 0x04000145 RID: 325
		public bool ShowWorkingStatus;
	}
}
