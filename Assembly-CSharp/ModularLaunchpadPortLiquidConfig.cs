using System;
using UnityEngine;

// Token: 0x020004B3 RID: 1203
public class ModularLaunchpadPortLiquidConfig : IBuildingConfig
{
	// Token: 0x06001497 RID: 5271 RVA: 0x000AA117 File Offset: 0x000A8317
	public override string[] GetRequiredDlcIds()
	{
		return DlcManager.EXPANSION1;
	}

	// Token: 0x06001498 RID: 5272 RVA: 0x000B364C File Offset: 0x000B184C
	public override BuildingDef CreateBuildingDef()
	{
		return BaseModularLaunchpadPortConfig.CreateBaseLaunchpadPort("ModularLaunchpadPortLiquid", "conduit_port_liquid_loader_kanim", ConduitType.Liquid, true, 2, 2);
	}

	// Token: 0x06001499 RID: 5273 RVA: 0x000B3661 File Offset: 0x000B1861
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		BaseModularLaunchpadPortConfig.ConfigureBuildingTemplate(go, prefab_tag, ConduitType.Liquid, 10f, true);
	}

	// Token: 0x0600149A RID: 5274 RVA: 0x000B3615 File Offset: 0x000B1815
	public override void DoPostConfigureComplete(GameObject go)
	{
		BaseModularLaunchpadPortConfig.DoPostConfigureComplete(go, true);
	}

	// Token: 0x04000E0D RID: 3597
	public const string ID = "ModularLaunchpadPortLiquid";
}
