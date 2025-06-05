using System;
using KSerialization;

// Token: 0x02000F33 RID: 3891
[SerializationConfig(MemberSerialization.OptIn)]
public class OperationalControlledSwitch : CircuitSwitch
{
	// Token: 0x06004E16 RID: 19990 RVA: 0x000D71B5 File Offset: 0x000D53B5
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.manuallyControlled = false;
	}

	// Token: 0x06004E17 RID: 19991 RVA: 0x000D71C4 File Offset: 0x000D53C4
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.Subscribe<OperationalControlledSwitch>(-592767678, OperationalControlledSwitch.OnOperationalChangedDelegate);
	}

	// Token: 0x06004E18 RID: 19992 RVA: 0x002759CC File Offset: 0x00273BCC
	private void OnOperationalChanged(object data)
	{
		bool state = (bool)data;
		this.SetState(state);
	}

	// Token: 0x040036D3 RID: 14035
	private static readonly EventSystem.IntraObjectHandler<OperationalControlledSwitch> OnOperationalChangedDelegate = new EventSystem.IntraObjectHandler<OperationalControlledSwitch>(delegate(OperationalControlledSwitch component, object data)
	{
		component.OnOperationalChanged(data);
	});
}
