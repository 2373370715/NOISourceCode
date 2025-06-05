using System;

// Token: 0x02000074 RID: 116
public class PoweredActiveTransitionController : GameStateMachine<PoweredActiveTransitionController, PoweredActiveTransitionController.Instance, IStateMachineTarget, PoweredActiveTransitionController.Def>
{
	// Token: 0x060001E7 RID: 487 RVA: 0x0014DED0 File Offset: 0x0014C0D0
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.off;
		this.off.PlayAnim("off").EventTransition(GameHashes.OperationalChanged, this.on_pre, (PoweredActiveTransitionController.Instance smi) => smi.GetComponent<Operational>().IsOperational);
		this.on_pre.PlayAnim("working_pre").OnAnimQueueComplete(this.on);
		this.on.PlayAnim("on").EventTransition(GameHashes.OperationalChanged, this.on_pst, (PoweredActiveTransitionController.Instance smi) => !smi.GetComponent<Operational>().IsOperational).EventTransition(GameHashes.ActiveChanged, this.working, (PoweredActiveTransitionController.Instance smi) => smi.GetComponent<Operational>().IsActive);
		this.on_pst.PlayAnim("working_pst").OnAnimQueueComplete(this.off);
		this.working.PlayAnim("working_loop", KAnim.PlayMode.Loop).EventTransition(GameHashes.OperationalChanged, this.on_pst, (PoweredActiveTransitionController.Instance smi) => !smi.GetComponent<Operational>().IsOperational).EventTransition(GameHashes.ActiveChanged, this.on, (PoweredActiveTransitionController.Instance smi) => !smi.GetComponent<Operational>().IsActive).Enter(delegate(PoweredActiveTransitionController.Instance smi)
		{
			if (smi.def.showWorkingStatus)
			{
				smi.GetComponent<KSelectable>().AddStatusItem(Db.Get().BuildingStatusItems.Working, null);
			}
		}).Exit(delegate(PoweredActiveTransitionController.Instance smi)
		{
			if (smi.def.showWorkingStatus)
			{
				smi.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().BuildingStatusItems.Working, false);
			}
		});
	}

	// Token: 0x04000135 RID: 309
	public GameStateMachine<PoweredActiveTransitionController, PoweredActiveTransitionController.Instance, IStateMachineTarget, PoweredActiveTransitionController.Def>.State off;

	// Token: 0x04000136 RID: 310
	public GameStateMachine<PoweredActiveTransitionController, PoweredActiveTransitionController.Instance, IStateMachineTarget, PoweredActiveTransitionController.Def>.State on;

	// Token: 0x04000137 RID: 311
	public GameStateMachine<PoweredActiveTransitionController, PoweredActiveTransitionController.Instance, IStateMachineTarget, PoweredActiveTransitionController.Def>.State on_pre;

	// Token: 0x04000138 RID: 312
	public GameStateMachine<PoweredActiveTransitionController, PoweredActiveTransitionController.Instance, IStateMachineTarget, PoweredActiveTransitionController.Def>.State on_pst;

	// Token: 0x04000139 RID: 313
	public GameStateMachine<PoweredActiveTransitionController, PoweredActiveTransitionController.Instance, IStateMachineTarget, PoweredActiveTransitionController.Def>.State working;

	// Token: 0x02000075 RID: 117
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x0400013A RID: 314
		public bool showWorkingStatus;
	}

	// Token: 0x02000076 RID: 118
	public new class Instance : GameStateMachine<PoweredActiveTransitionController, PoweredActiveTransitionController.Instance, IStateMachineTarget, PoweredActiveTransitionController.Def>.GameInstance
	{
		// Token: 0x060001EA RID: 490 RVA: 0x000AAAA3 File Offset: 0x000A8CA3
		public Instance(IStateMachineTarget master, PoweredActiveTransitionController.Def def) : base(master, def)
		{
		}
	}
}
