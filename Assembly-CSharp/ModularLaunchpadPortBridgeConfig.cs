using System;
using TUNING;
using UnityEngine;

// Token: 0x020004B0 RID: 1200
public class ModularLaunchpadPortBridgeConfig : IBuildingConfig
{
	// Token: 0x06001488 RID: 5256 RVA: 0x000AA117 File Offset: 0x000A8317
	public override string[] GetRequiredDlcIds()
	{
		return DlcManager.EXPANSION1;
	}

	// Token: 0x06001489 RID: 5257 RVA: 0x0019C180 File Offset: 0x0019A380
	public override BuildingDef CreateBuildingDef()
	{
		string id = "ModularLaunchpadPortBridge";
		int width = 1;
		int height = 2;
		string anim = "rocket_loader_extension_kanim";
		int hitpoints = 1000;
		float construction_time = 60f;
		float[] tier = BUILDINGS.CONSTRUCTION_MASS_KG.TIER3;
		string[] refined_METALS = MATERIALS.REFINED_METALS;
		float melting_point = 9999f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER0;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, refined_METALS, melting_point, build_location_rule, BUILDINGS.DECOR.NONE, tier2, 0.2f);
		buildingDef.SceneLayer = Grid.SceneLayer.BuildingBack;
		buildingDef.OverheatTemperature = 2273.15f;
		buildingDef.Floodable = false;
		buildingDef.Entombable = false;
		buildingDef.DefaultAnimState = "idle";
		buildingDef.AudioCategory = "Metal";
		buildingDef.AudioSize = "medium";
		return buildingDef;
	}

	// Token: 0x0600148A RID: 5258 RVA: 0x0019C20C File Offset: 0x0019A40C
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		KPrefabID component = go.GetComponent<KPrefabID>();
		component.AddTag(GameTags.ModularConduitPort, false);
		component.AddTag(GameTags.NotRocketInteriorBuilding, false);
		component.AddTag(BaseModularLaunchpadPortConfig.LinkTag, false);
		ChainedBuilding.Def def = go.AddOrGetDef<ChainedBuilding.Def>();
		def.headBuildingTag = "LaunchPad".ToTag();
		def.linkBuildingTag = BaseModularLaunchpadPortConfig.LinkTag;
		def.objectLayer = ObjectLayer.Building;
		go.AddOrGet<FakeFloorAdder>().floorOffsets = new CellOffset[]
		{
			new CellOffset(0, 1)
		};
	}

	// Token: 0x0600148B RID: 5259 RVA: 0x000AA038 File Offset: 0x000A8238
	public override void DoPostConfigureComplete(GameObject go)
	{
	}

	// Token: 0x04000E0A RID: 3594
	public const string ID = "ModularLaunchpadPortBridge";
}
