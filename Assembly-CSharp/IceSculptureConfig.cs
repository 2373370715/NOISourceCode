using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000392 RID: 914
public class IceSculptureConfig : IBuildingConfig
{
	// Token: 0x06000EB0 RID: 3760 RVA: 0x00184954 File Offset: 0x00182B54
	public override BuildingDef CreateBuildingDef()
	{
		string id = "IceSculpture";
		int width = 2;
		int height = 2;
		string anim = "icesculpture_kanim";
		int hitpoints = 10;
		float construction_time = 120f;
		float[] tier = TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER4;
		string[] construction_materials = new string[]
		{
			"Ice"
		};
		float melting_point = 273.15f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
		EffectorValues none = NOISE_POLLUTION.NONE;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, construction_materials, melting_point, build_location_rule, new EffectorValues
		{
			amount = 35,
			radius = 8
		}, none, 0.2f);
		buildingDef.Floodable = false;
		buildingDef.Overheatable = false;
		buildingDef.AudioCategory = "Metal";
		buildingDef.BaseTimeUntilRepair = -1f;
		buildingDef.ViewMode = OverlayModes.Decor.ID;
		buildingDef.DefaultAnimState = "slab";
		buildingDef.PermittedRotations = PermittedRotations.FlipH;
		buildingDef.Temperature = 253.15f;
		buildingDef.RequiredSkillPerkID = Db.Get().SkillPerks.CanArt.Id;
		buildingDef.AddSearchTerms(SEARCH_TERMS.STATUE);
		buildingDef.AddSearchTerms(SEARCH_TERMS.MORALE);
		buildingDef.AddSearchTerms(SEARCH_TERMS.ARTWORK);
		return buildingDef;
	}

	// Token: 0x06000EB1 RID: 3761 RVA: 0x000AA54F File Offset: 0x000A874F
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.AddOrGet<BuildingComplete>().isArtable = true;
		go.GetComponent<KPrefabID>().AddTag(GameTags.Decoration, false);
	}

	// Token: 0x06000EB2 RID: 3762 RVA: 0x000B0C21 File Offset: 0x000AEE21
	public override void DoPostConfigureComplete(GameObject go)
	{
		go.AddComponent<Sculpture>().defaultAnimName = "slab";
	}

	// Token: 0x04000AE1 RID: 2785
	public const string ID = "IceSculpture";
}
