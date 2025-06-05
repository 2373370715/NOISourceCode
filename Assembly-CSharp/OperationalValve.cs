using System;
using KSerialization;

// Token: 0x02000F35 RID: 3893
[SerializationConfig(MemberSerialization.OptIn)]
public class OperationalValve : ValveBase
{
	// Token: 0x06004E1E RID: 19998 RVA: 0x000D7216 File Offset: 0x000D5416
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.Subscribe<OperationalValve>(-592767678, OperationalValve.OnOperationalChangedDelegate);
	}

	// Token: 0x06004E1F RID: 19999 RVA: 0x000D722F File Offset: 0x000D542F
	protected override void OnSpawn()
	{
		this.OnOperationalChanged(this.operational.IsOperational);
		base.OnSpawn();
	}

	// Token: 0x06004E20 RID: 20000 RVA: 0x000D724D File Offset: 0x000D544D
	protected override void OnCleanUp()
	{
		base.Unsubscribe<OperationalValve>(-592767678, OperationalValve.OnOperationalChangedDelegate, false);
		base.OnCleanUp();
	}

	// Token: 0x06004E21 RID: 20001 RVA: 0x002759E8 File Offset: 0x00273BE8
	private void OnOperationalChanged(object data)
	{
		bool flag = (bool)data;
		if (flag)
		{
			base.CurrentFlow = base.MaxFlow;
		}
		else
		{
			base.CurrentFlow = 0f;
		}
		this.operational.SetActive(flag, false);
	}

	// Token: 0x06004E22 RID: 20002 RVA: 0x000D7266 File Offset: 0x000D5466
	protected override void OnMassTransfer(float amount)
	{
		this.isDispensing = (amount > 0f);
	}

	// Token: 0x06004E23 RID: 20003 RVA: 0x00275A28 File Offset: 0x00273C28
	public override void UpdateAnim()
	{
		if (!this.operational.IsOperational)
		{
			this.controller.Queue("off", KAnim.PlayMode.Once, 1f, 0f);
			return;
		}
		if (this.isDispensing)
		{
			this.controller.Queue("on_flow", KAnim.PlayMode.Loop, 1f, 0f);
			return;
		}
		this.controller.Queue("on", KAnim.PlayMode.Once, 1f, 0f);
	}

	// Token: 0x040036D5 RID: 14037
	[MyCmpReq]
	private Operational operational;

	// Token: 0x040036D6 RID: 14038
	private bool isDispensing;

	// Token: 0x040036D7 RID: 14039
	private static readonly EventSystem.IntraObjectHandler<OperationalValve> OnOperationalChangedDelegate = new EventSystem.IntraObjectHandler<OperationalValve>(delegate(OperationalValve component, object data)
	{
		component.OnOperationalChanged(data);
	});
}
