using System;
using UnityEngine;

// Token: 0x020004B1 RID: 1201
public class ModularLaunchpadPortGasConfig : IBuildingConfig
{
	// Token: 0x0600148D RID: 5261 RVA: 0x000AA117 File Offset: 0x000A8317
	public override string[] GetRequiredDlcIds()
	{
		return DlcManager.EXPANSION1;
	}

	// Token: 0x0600148E RID: 5262 RVA: 0x000B35F0 File Offset: 0x000B17F0
	public override BuildingDef CreateBuildingDef()
	{
		return BaseModularLaunchpadPortConfig.CreateBaseLaunchpadPort("ModularLaunchpadPortGas", "conduit_port_gas_loader_kanim", ConduitType.Gas, true, 2, 2);
	}

	// Token: 0x0600148F RID: 5263 RVA: 0x000B3605 File Offset: 0x000B1805
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		BaseModularLaunchpadPortConfig.ConfigureBuildingTemplate(go, prefab_tag, ConduitType.Gas, 1f, true);
	}

	// Token: 0x06001490 RID: 5264 RVA: 0x000B3615 File Offset: 0x000B1815
	public override void DoPostConfigureComplete(GameObject go)
	{
		BaseModularLaunchpadPortConfig.DoPostConfigureComplete(go, true);
	}

	// Token: 0x04000E0B RID: 3595
	public const string ID = "ModularLaunchpadPortGas";
}
