using System;
using UnityEngine;

// Token: 0x020004B2 RID: 1202
public class ModularLaunchpadPortGasUnloaderConfig : IBuildingConfig
{
	// Token: 0x06001492 RID: 5266 RVA: 0x000AA117 File Offset: 0x000A8317
	public override string[] GetRequiredDlcIds()
	{
		return DlcManager.EXPANSION1;
	}

	// Token: 0x06001493 RID: 5267 RVA: 0x000B361E File Offset: 0x000B181E
	public override BuildingDef CreateBuildingDef()
	{
		return BaseModularLaunchpadPortConfig.CreateBaseLaunchpadPort("ModularLaunchpadPortGasUnloader", "conduit_port_gas_unloader_kanim", ConduitType.Gas, false, 2, 3);
	}

	// Token: 0x06001494 RID: 5268 RVA: 0x000B3633 File Offset: 0x000B1833
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		BaseModularLaunchpadPortConfig.ConfigureBuildingTemplate(go, prefab_tag, ConduitType.Gas, 1f, false);
	}

	// Token: 0x06001495 RID: 5269 RVA: 0x000B3643 File Offset: 0x000B1843
	public override void DoPostConfigureComplete(GameObject go)
	{
		BaseModularLaunchpadPortConfig.DoPostConfigureComplete(go, false);
	}

	// Token: 0x04000E0C RID: 3596
	public const string ID = "ModularLaunchpadPortGasUnloader";
}
