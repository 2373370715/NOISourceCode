using System;
using TUNING;
using UnityEngine;

// Token: 0x02000099 RID: 153
public class DevGeneratorConfig : IBuildingConfig
{
	// Token: 0x06000275 RID: 629 RVA: 0x00150FB8 File Offset: 0x0014F1B8
	public override BuildingDef CreateBuildingDef()
	{
		string id = "DevGenerator";
		int width = 1;
		int height = 1;
		string anim = "dev_generator_kanim";
		int hitpoints = 100;
		float construction_time = 3f;
		float[] tier = BUILDINGS.CONSTRUCTION_MASS_KG.TIER0;
		string[] all_METALS = MATERIALS.ALL_METALS;
		float melting_point = 2400f;
		BuildLocationRule build_location_rule = BuildLocationRule.Anywhere;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER5;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, all_METALS, melting_point, build_location_rule, BUILDINGS.DECOR.PENALTY.TIER2, tier2, 0.2f);
		buildingDef.GeneratorWattageRating = 100000f;
		buildingDef.GeneratorBaseCapacity = buildingDef.GeneratorWattageRating;
		buildingDef.RequiresPowerOutput = true;
		buildingDef.PowerOutputOffset = new CellOffset(0, 0);
		buildingDef.ViewMode = OverlayModes.Power.ID;
		buildingDef.AudioCategory = "HollowMetal";
		buildingDef.AudioSize = "large";
		buildingDef.Floodable = false;
		buildingDef.DebugOnly = true;
		return buildingDef;
	}

	// Token: 0x06000276 RID: 630 RVA: 0x000AADBE File Offset: 0x000A8FBE
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.AddTag(GameTags.DevBuilding);
		DevGenerator devGenerator = go.AddOrGet<DevGenerator>();
		devGenerator.powerDistributionOrder = 9;
		devGenerator.wattageRating = 100000f;
	}

	// Token: 0x06000277 RID: 631 RVA: 0x000AADE3 File Offset: 0x000A8FE3
	public override void DoPostConfigureComplete(GameObject go)
	{
		go.AddOrGetDef<PoweredActiveController.Def>();
	}

	// Token: 0x0400019C RID: 412
	public const string ID = "DevGenerator";
}
