using System;
using KSerialization;

// Token: 0x0200091E RID: 2334
[SerializationConfig(MemberSerialization.OptIn)]
public abstract class StateMachineComponent : KMonoBehaviour, ISaveLoadable, IStateMachineTarget
{
	// Token: 0x060028E5 RID: 10469
	public abstract StateMachine.Instance GetSMI();

	// Token: 0x04001BE2 RID: 7138
	[MyCmpAdd]
	protected StateMachineController stateMachineController;
}
