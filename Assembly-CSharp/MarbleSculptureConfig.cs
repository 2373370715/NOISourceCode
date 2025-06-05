using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x0200040A RID: 1034
public class MarbleSculptureConfig : IBuildingConfig
{
	// Token: 0x06001128 RID: 4392 RVA: 0x0018D07C File Offset: 0x0018B27C
	public override BuildingDef CreateBuildingDef()
	{
		string id = "MarbleSculpture";
		int width = 2;
		int height = 3;
		string anim = "sculpture_marble_kanim";
		int hitpoints = 10;
		float construction_time = 120f;
		float[] tier = TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER4;
		string[] precious_ROCKS = MATERIALS.PRECIOUS_ROCKS;
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
		EffectorValues none = NOISE_POLLUTION.NONE;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, precious_ROCKS, melting_point, build_location_rule, new EffectorValues
		{
			amount = 20,
			radius = 8
		}, none, 0.2f);
		buildingDef.Floodable = false;
		buildingDef.Overheatable = false;
		buildingDef.AudioCategory = "Metal";
		buildingDef.BaseTimeUntilRepair = -1f;
		buildingDef.ViewMode = OverlayModes.Decor.ID;
		buildingDef.DefaultAnimState = "slab";
		buildingDef.PermittedRotations = PermittedRotations.FlipH;
		buildingDef.RequiredSkillPerkID = Db.Get().SkillPerks.CanArt.Id;
		buildingDef.AddSearchTerms(SEARCH_TERMS.STATUE);
		buildingDef.AddSearchTerms(SEARCH_TERMS.ARTWORK);
		buildingDef.AddSearchTerms(SEARCH_TERMS.MORALE);
		return buildingDef;
	}

	// Token: 0x06001129 RID: 4393 RVA: 0x000AA54F File Offset: 0x000A874F
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.AddOrGet<BuildingComplete>().isArtable = true;
		go.GetComponent<KPrefabID>().AddTag(GameTags.Decoration, false);
	}

	// Token: 0x0600112A RID: 4394 RVA: 0x000B0C21 File Offset: 0x000AEE21
	public override void DoPostConfigureComplete(GameObject go)
	{
		go.AddComponent<Sculpture>().defaultAnimName = "slab";
	}

	// Token: 0x04000BEE RID: 3054
	public const string ID = "MarbleSculpture";
}
