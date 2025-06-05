using System;

// Token: 0x02000067 RID: 103
public class OperationalController : GameStateMachine<OperationalController, OperationalController.Instance>
{
	// Token: 0x060001C7 RID: 455 RVA: 0x0014DAD0 File Offset: 0x0014BCD0
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.off;
		this.root.EventTransition(GameHashes.OperationalChanged, this.off, (OperationalController.Instance smi) => !smi.GetComponent<Operational>().IsOperational);
		this.off.PlayAnim("off").EventTransition(GameHashes.OperationalChanged, this.working_pre, (OperationalController.Instance smi) => smi.GetComponent<Operational>().IsOperational);
		this.working_pre.PlayAnim("working_pre").OnAnimQueueComplete(this.working_loop);
		this.working_loop.PlayAnim("working_loop", KAnim.PlayMode.Loop).EventTransition(GameHashes.OperationalChanged, this.working_pst, (OperationalController.Instance smi) => !smi.GetComponent<Operational>().IsOperational);
		this.working_pst.PlayAnim("working_pst").OnAnimQueueComplete(this.off);
	}

	// Token: 0x04000115 RID: 277
	public GameStateMachine<OperationalController, OperationalController.Instance, IStateMachineTarget, object>.State off;

	// Token: 0x04000116 RID: 278
	public GameStateMachine<OperationalController, OperationalController.Instance, IStateMachineTarget, object>.State working_pre;

	// Token: 0x04000117 RID: 279
	public GameStateMachine<OperationalController, OperationalController.Instance, IStateMachineTarget, object>.State working_loop;

	// Token: 0x04000118 RID: 280
	public GameStateMachine<OperationalController, OperationalController.Instance, IStateMachineTarget, object>.State working_pst;

	// Token: 0x02000068 RID: 104
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x02000069 RID: 105
	public new class Instance : GameStateMachine<OperationalController, OperationalController.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x060001CA RID: 458 RVA: 0x000AA9EB File Offset: 0x000A8BEB
		public Instance(IStateMachineTarget master, OperationalController.Def def) : base(master, def)
		{
		}
	}
}
