using System;

// Token: 0x02000070 RID: 112
public class PoweredActiveStoppableController : GameStateMachine<PoweredActiveStoppableController, PoweredActiveStoppableController.Instance>
{
	// Token: 0x060001DE RID: 478 RVA: 0x0014DDB4 File Offset: 0x0014BFB4
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.off;
		this.off.PlayAnim("off").EventTransition(GameHashes.ActiveChanged, this.working_pre, (PoweredActiveStoppableController.Instance smi) => smi.GetComponent<Operational>().IsActive);
		this.working_pre.PlayAnim("working_pre").OnAnimQueueComplete(this.working_loop);
		this.working_loop.PlayAnim("working_loop", KAnim.PlayMode.Loop).EventTransition(GameHashes.OperationalChanged, this.stop, (PoweredActiveStoppableController.Instance smi) => !smi.GetComponent<Operational>().IsOperational).EventTransition(GameHashes.ActiveChanged, this.working_pst, (PoweredActiveStoppableController.Instance smi) => !smi.GetComponent<Operational>().IsActive);
		this.working_pst.PlayAnim("working_pst").OnAnimQueueComplete(this.off);
		this.stop.PlayAnim("stop").OnAnimQueueComplete(this.off);
	}

	// Token: 0x0400012C RID: 300
	public GameStateMachine<PoweredActiveStoppableController, PoweredActiveStoppableController.Instance, IStateMachineTarget, object>.State off;

	// Token: 0x0400012D RID: 301
	public GameStateMachine<PoweredActiveStoppableController, PoweredActiveStoppableController.Instance, IStateMachineTarget, object>.State working_pre;

	// Token: 0x0400012E RID: 302
	public GameStateMachine<PoweredActiveStoppableController, PoweredActiveStoppableController.Instance, IStateMachineTarget, object>.State working_loop;

	// Token: 0x0400012F RID: 303
	public GameStateMachine<PoweredActiveStoppableController, PoweredActiveStoppableController.Instance, IStateMachineTarget, object>.State working_pst;

	// Token: 0x04000130 RID: 304
	public GameStateMachine<PoweredActiveStoppableController, PoweredActiveStoppableController.Instance, IStateMachineTarget, object>.State stop;

	// Token: 0x02000071 RID: 113
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x02000072 RID: 114
	public new class Instance : GameStateMachine<PoweredActiveStoppableController, PoweredActiveStoppableController.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x060001E1 RID: 481 RVA: 0x000AAA85 File Offset: 0x000A8C85
		public Instance(IStateMachineTarget master, PoweredActiveStoppableController.Def def) : base(master, def)
		{
		}
	}
}
