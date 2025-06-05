using System;
using TUNING;
using UnityEngine;

// Token: 0x020003B0 RID: 944
public class LadderFastConfig : IBuildingConfig
{
	// Token: 0x06000F46 RID: 3910 RVA: 0x00186710 File Offset: 0x00184910
	public override BuildingDef CreateBuildingDef()
	{
		string id = "LadderFast";
		int width = 1;
		int height = 1;
		string anim = "ladder_plastic_kanim";
		int hitpoints = 10;
		float construction_time = 10f;
		float[] tier = BUILDINGS.CONSTRUCTION_MASS_KG.TIER1;
		string[] plastics = MATERIALS.PLASTICS;
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.Anywhere;
		EffectorValues none = NOISE_POLLUTION.NONE;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, plastics, melting_point, build_location_rule, BUILDINGS.DECOR.PENALTY.TIER0, none, 0.2f);
		BuildingTemplates.CreateLadderDef(buildingDef);
		buildingDef.Floodable = false;
		buildingDef.Overheatable = false;
		buildingDef.Entombable = false;
		buildingDef.AudioCategory = "Plastic";
		buildingDef.AudioSize = "small";
		buildingDef.BaseTimeUntilRepair = -1f;
		buildingDef.DragBuild = true;
		return buildingDef;
	}

	// Token: 0x06000F47 RID: 3911 RVA: 0x000B0F70 File Offset: 0x000AF170
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		GeneratedBuildings.MakeBuildingAlwaysOperational(go);
		Ladder ladder = go.AddOrGet<Ladder>();
		ladder.upwardsMovementSpeedMultiplier = 1.2f;
		ladder.downwardsMovementSpeedMultiplier = 1.2f;
		go.AddOrGet<AnimTileable>();
	}

	// Token: 0x06000F48 RID: 3912 RVA: 0x000AA038 File Offset: 0x000A8238
	public override void DoPostConfigureComplete(GameObject go)
	{
	}

	// Token: 0x04000B30 RID: 2864
	public const string ID = "LadderFast";
}
