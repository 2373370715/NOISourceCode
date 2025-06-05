using System;

// Token: 0x02001188 RID: 4488
public class CritterElementMonitor : GameStateMachine<CritterElementMonitor, CritterElementMonitor.Instance, IStateMachineTarget>
{
	// Token: 0x06005B62 RID: 23394 RVA: 0x000DFF4D File Offset: 0x000DE14D
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.root;
		this.root.Update("UpdateInElement", delegate(CritterElementMonitor.Instance smi, float dt)
		{
			smi.UpdateCurrentElement(dt);
		}, UpdateRate.SIM_1000ms, false);
	}

	// Token: 0x02001189 RID: 4489
	public new class Instance : GameStateMachine<CritterElementMonitor, CritterElementMonitor.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x14000018 RID: 24
		// (add) Token: 0x06005B64 RID: 23396 RVA: 0x002A5C08 File Offset: 0x002A3E08
		// (remove) Token: 0x06005B65 RID: 23397 RVA: 0x002A5C40 File Offset: 0x002A3E40
		public event Action<float> OnUpdateEggChances;

		// Token: 0x06005B66 RID: 23398 RVA: 0x000DFF91 File Offset: 0x000DE191
		public Instance(IStateMachineTarget master) : base(master)
		{
		}

		// Token: 0x06005B67 RID: 23399 RVA: 0x000DFF9A File Offset: 0x000DE19A
		public void UpdateCurrentElement(float dt)
		{
			this.OnUpdateEggChances(dt);
		}
	}
}
