using System;
using UnityEngine;

// Token: 0x020004B4 RID: 1204
public class ModularLaunchpadPortLiquidUnloaderConfig : IBuildingConfig
{
	// Token: 0x0600149C RID: 5276 RVA: 0x000AA117 File Offset: 0x000A8317
	public override string[] GetRequiredDlcIds()
	{
		return DlcManager.EXPANSION1;
	}

	// Token: 0x0600149D RID: 5277 RVA: 0x000B3671 File Offset: 0x000B1871
	public override BuildingDef CreateBuildingDef()
	{
		return BaseModularLaunchpadPortConfig.CreateBaseLaunchpadPort("ModularLaunchpadPortLiquidUnloader", "conduit_port_liquid_unloader_kanim", ConduitType.Liquid, false, 2, 3);
	}

	// Token: 0x0600149E RID: 5278 RVA: 0x000B3686 File Offset: 0x000B1886
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		BaseModularLaunchpadPortConfig.ConfigureBuildingTemplate(go, prefab_tag, ConduitType.Liquid, 10f, false);
	}

	// Token: 0x0600149F RID: 5279 RVA: 0x000B3643 File Offset: 0x000B1843
	public override void DoPostConfigureComplete(GameObject go)
	{
		BaseModularLaunchpadPortConfig.DoPostConfigureComplete(go, false);
	}

	// Token: 0x04000E0E RID: 3598
	public const string ID = "ModularLaunchpadPortLiquidUnloader";
}
