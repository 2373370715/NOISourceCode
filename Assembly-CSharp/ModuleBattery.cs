using System;

// Token: 0x02000F16 RID: 3862
public class ModuleBattery : Battery
{
	// Token: 0x06004D5D RID: 19805 RVA: 0x000D692D File Offset: 0x000D4B2D
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.connectedTags = new Tag[0];
		base.IsVirtual = true;
	}

	// Token: 0x06004D5E RID: 19806 RVA: 0x00273B2C File Offset: 0x00271D2C
	protected override void OnSpawn()
	{
		CraftModuleInterface craftInterface = base.GetComponent<RocketModuleCluster>().CraftInterface;
		base.VirtualCircuitKey = craftInterface;
		base.OnSpawn();
		this.meter.gameObject.GetComponent<KBatchedAnimTracker>().matchParentOffset = true;
	}
}
