using System;
using TUNING;
using UnityEngine;

// Token: 0x020003AF RID: 943
public class LadderConfig : IBuildingConfig
{
	// Token: 0x06000F42 RID: 3906 RVA: 0x00186684 File Offset: 0x00184884
	public override BuildingDef CreateBuildingDef()
	{
		string id = "Ladder";
		int width = 1;
		int height = 1;
		string anim = "ladder_kanim";
		int hitpoints = 10;
		float construction_time = 10f;
		float[] tier = BUILDINGS.CONSTRUCTION_MASS_KG.TIER2;
		string[] raw_MINERALS_OR_WOOD = MATERIALS.RAW_MINERALS_OR_WOOD;
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.Anywhere;
		EffectorValues none = NOISE_POLLUTION.NONE;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, raw_MINERALS_OR_WOOD, melting_point, build_location_rule, BUILDINGS.DECOR.PENALTY.TIER0, none, 0.2f);
		BuildingTemplates.CreateLadderDef(buildingDef);
		buildingDef.Floodable = false;
		buildingDef.Overheatable = false;
		buildingDef.Entombable = false;
		buildingDef.AudioCategory = "Metal";
		buildingDef.AudioSize = "small";
		buildingDef.BaseTimeUntilRepair = -1f;
		buildingDef.DragBuild = true;
		return buildingDef;
	}

	// Token: 0x06000F43 RID: 3907 RVA: 0x000B0F46 File Offset: 0x000AF146
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		GeneratedBuildings.MakeBuildingAlwaysOperational(go);
		Ladder ladder = go.AddOrGet<Ladder>();
		ladder.upwardsMovementSpeedMultiplier = 1f;
		ladder.downwardsMovementSpeedMultiplier = 1f;
		go.AddOrGet<AnimTileable>();
	}

	// Token: 0x06000F44 RID: 3908 RVA: 0x000AA038 File Offset: 0x000A8238
	public override void DoPostConfigureComplete(GameObject go)
	{
	}

	// Token: 0x04000B2F RID: 2863
	public const string ID = "Ladder";
}
