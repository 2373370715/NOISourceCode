using System;
using UnityEngine;

// Token: 0x020004B6 RID: 1206
public class ModularLaunchpadPortSolidUnloaderConfig : IBuildingConfig
{
	// Token: 0x060014A6 RID: 5286 RVA: 0x000AA117 File Offset: 0x000A8317
	public override string[] GetRequiredDlcIds()
	{
		return DlcManager.EXPANSION1;
	}

	// Token: 0x060014A7 RID: 5287 RVA: 0x000B36BB File Offset: 0x000B18BB
	public override BuildingDef CreateBuildingDef()
	{
		return BaseModularLaunchpadPortConfig.CreateBaseLaunchpadPort("ModularLaunchpadPortSolidUnloader", "conduit_port_solid_unloader_kanim", ConduitType.Solid, false, 2, 3);
	}

	// Token: 0x060014A8 RID: 5288 RVA: 0x000B36D0 File Offset: 0x000B18D0
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		BaseModularLaunchpadPortConfig.ConfigureBuildingTemplate(go, prefab_tag, ConduitType.Solid, 20f, false);
	}

	// Token: 0x060014A9 RID: 5289 RVA: 0x000B3643 File Offset: 0x000B1843
	public override void DoPostConfigureComplete(GameObject go)
	{
		BaseModularLaunchpadPortConfig.DoPostConfigureComplete(go, false);
	}

	// Token: 0x04000E10 RID: 3600
	public const string ID = "ModularLaunchpadPortSolidUnloader";
}
