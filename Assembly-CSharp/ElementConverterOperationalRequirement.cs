using System;
using KSerialization;

// Token: 0x020012C7 RID: 4807
[SerializationConfig(MemberSerialization.OptIn)]
public class ElementConverterOperationalRequirement : KMonoBehaviour
{
	// Token: 0x06006265 RID: 25189 RVA: 0x000E49C7 File Offset: 0x000E2BC7
	private void onStorageChanged(object _)
	{
		this.operational.SetFlag(this.sufficientResources, this.converter.HasEnoughMassToStartConverting(false));
	}

	// Token: 0x06006266 RID: 25190 RVA: 0x000E49E6 File Offset: 0x000E2BE6
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.sufficientResources = new Operational.Flag("sufficientResources", this.operationalReq);
		base.Subscribe(-1697596308, new Action<object>(this.onStorageChanged));
		this.onStorageChanged(null);
	}

	// Token: 0x0400469F RID: 18079
	[MyCmpReq]
	private ElementConverter converter;

	// Token: 0x040046A0 RID: 18080
	[MyCmpReq]
	private Operational operational;

	// Token: 0x040046A1 RID: 18081
	private Operational.Flag.Type operationalReq;

	// Token: 0x040046A2 RID: 18082
	private Operational.Flag sufficientResources;
}
