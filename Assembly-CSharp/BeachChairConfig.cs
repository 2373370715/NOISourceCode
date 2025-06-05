using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000031 RID: 49
public class BeachChairConfig : IBuildingConfig
{
	// Token: 0x060000CE RID: 206 RVA: 0x00149C7C File Offset: 0x00147E7C
	public override BuildingDef CreateBuildingDef()
	{
		string id = "BeachChair";
		int width = 2;
		int height = 3;
		string anim = "beach_chair_kanim";
		int hitpoints = 30;
		float construction_time = 60f;
		float[] construction_mass = new float[]
		{
			400f,
			2f
		};
		string[] construction_materials = new string[]
		{
			"BuildableRaw",
			"BuildingFiber"
		};
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
		EffectorValues none = NOISE_POLLUTION.NONE;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, construction_mass, construction_materials, melting_point, build_location_rule, TUNING.BUILDINGS.DECOR.BONUS.TIER4, none, 0.2f);
		buildingDef.Floodable = true;
		buildingDef.AudioCategory = "Metal";
		buildingDef.Overheatable = true;
		buildingDef.AddSearchTerms(SEARCH_TERMS.MORALE);
		return buildingDef;
	}

	// Token: 0x060000CF RID: 207 RVA: 0x00149D10 File Offset: 0x00147F10
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.RecBuilding, false);
		go.AddOrGet<BeachChairWorkable>().basePriority = RELAXATION.PRIORITY.TIER4;
		BeachChair beachChair = go.AddOrGet<BeachChair>();
		beachChair.specificEffectUnlit = "BeachChairUnlit";
		beachChair.specificEffectLit = "BeachChairLit";
		beachChair.trackingEffect = "RecentlyBeachChair";
		RoomTracker roomTracker = go.AddOrGet<RoomTracker>();
		roomTracker.requiredRoomType = Db.Get().RoomTypes.RecRoom.Id;
		roomTracker.requirement = RoomTracker.Requirement.Recommended;
		go.AddOrGet<AnimTileable>();
		go.AddOrGetDef<RocketUsageRestriction.Def>();
	}

	// Token: 0x060000D0 RID: 208 RVA: 0x000AA038 File Offset: 0x000A8238
	public override void DoPostConfigureComplete(GameObject go)
	{
	}

	// Token: 0x04000088 RID: 136
	public const string ID = "BeachChair";

	// Token: 0x04000089 RID: 137
	public static readonly int TAN_LUX = DUPLICANTSTATS.STANDARD.Light.HIGH_LIGHT;

	// Token: 0x0400008A RID: 138
	private const float TANK_SIZE_KG = 20f;

	// Token: 0x0400008B RID: 139
	private const float SPILL_RATE_KG = 0.05f;
}
