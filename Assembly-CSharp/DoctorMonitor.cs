using System;

// Token: 0x020015A6 RID: 5542
public class DoctorMonitor : GameStateMachine<DoctorMonitor, DoctorMonitor.Instance>
{
	// Token: 0x06007335 RID: 29493 RVA: 0x000EFED1 File Offset: 0x000EE0D1
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.root;
		base.serializable = StateMachine.SerializeType.Both_DEPRECATED;
		this.root.ToggleUrge(Db.Get().Urges.Doctor);
	}

	// Token: 0x020015A7 RID: 5543
	public new class Instance : GameStateMachine<DoctorMonitor, DoctorMonitor.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x06007337 RID: 29495 RVA: 0x000EFF05 File Offset: 0x000EE105
		public Instance(IStateMachineTarget master) : base(master)
		{
		}
	}
}
