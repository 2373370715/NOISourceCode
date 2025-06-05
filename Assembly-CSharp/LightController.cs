using System;

// Token: 0x02000063 RID: 99
public class LightController : GameStateMachine<LightController, LightController.Instance>
{
	// Token: 0x060001BE RID: 446 RVA: 0x0014D9FC File Offset: 0x0014BBFC
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.off;
		this.off.PlayAnim("off").EventTransition(GameHashes.OperationalChanged, this.on, (LightController.Instance smi) => smi.GetComponent<Operational>().IsOperational);
		this.on.PlayAnim("on").EventTransition(GameHashes.OperationalChanged, this.off, (LightController.Instance smi) => !smi.GetComponent<Operational>().IsOperational).ToggleStatusItem(Db.Get().BuildingStatusItems.EmittingLight, null).Enter("SetActive", delegate(LightController.Instance smi)
		{
			smi.GetComponent<Operational>().SetActive(true, false);
		});
	}

	// Token: 0x0400010F RID: 271
	public GameStateMachine<LightController, LightController.Instance, IStateMachineTarget, object>.State off;

	// Token: 0x04000110 RID: 272
	public GameStateMachine<LightController, LightController.Instance, IStateMachineTarget, object>.State on;

	// Token: 0x02000064 RID: 100
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x02000065 RID: 101
	public new class Instance : GameStateMachine<LightController, LightController.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x060001C1 RID: 449 RVA: 0x000AA9A1 File Offset: 0x000A8BA1
		public Instance(IStateMachineTarget master, LightController.Def def) : base(master, def)
		{
		}
	}
}
