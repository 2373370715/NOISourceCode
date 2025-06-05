using System;
using UnityEngine;

// Token: 0x020004B5 RID: 1205
public class ModularLaunchpadPortSolidConfig : IBuildingConfig
{
	// Token: 0x060014A1 RID: 5281 RVA: 0x000AA117 File Offset: 0x000A8317
	public override string[] GetRequiredDlcIds()
	{
		return DlcManager.EXPANSION1;
	}

	// Token: 0x060014A2 RID: 5282 RVA: 0x000B3696 File Offset: 0x000B1896
	public override BuildingDef CreateBuildingDef()
	{
		return BaseModularLaunchpadPortConfig.CreateBaseLaunchpadPort("ModularLaunchpadPortSolid", "conduit_port_solid_loader_kanim", ConduitType.Solid, true, 2, 2);
	}

	// Token: 0x060014A3 RID: 5283 RVA: 0x000B36AB File Offset: 0x000B18AB
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		BaseModularLaunchpadPortConfig.ConfigureBuildingTemplate(go, prefab_tag, ConduitType.Solid, 20f, true);
	}

	// Token: 0x060014A4 RID: 5284 RVA: 0x000B3615 File Offset: 0x000B1815
	public override void DoPostConfigureComplete(GameObject go)
	{
		BaseModularLaunchpadPortConfig.DoPostConfigureComplete(go, true);
	}

	// Token: 0x04000E0F RID: 3599
	public const string ID = "ModularLaunchpadPortSolid";
}
