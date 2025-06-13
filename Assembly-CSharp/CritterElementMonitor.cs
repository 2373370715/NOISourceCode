using System;

public class CritterElementMonitor : GameStateMachine<CritterElementMonitor, CritterElementMonitor.Instance, IStateMachineTarget>
{
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.root;
		this.root.Update("UpdateInElement", delegate(CritterElementMonitor.Instance smi, float dt)
		{
			smi.UpdateCurrentElement(dt);
		}, UpdateRate.SIM_1000ms, false);
	}

	public new class Instance : GameStateMachine<CritterElementMonitor, CritterElementMonitor.Instance, IStateMachineTarget, object>.GameInstance
	{
add) Token: 0x06005B64 RID: 23396 RVA: 0x002A5C08 File Offset: 0x002A3E08
remove) Token: 0x06005B65 RID: 23397 RVA: 0x002A5C40 File Offset: 0x002A3E40
		public event Action<float> OnUpdateEggChances;

		public Instance(IStateMachineTarget master) : base(master)
		{
		}

		public void UpdateCurrentElement(float dt)
		{
			this.OnUpdateEggChances(dt);
		}
	}
}
