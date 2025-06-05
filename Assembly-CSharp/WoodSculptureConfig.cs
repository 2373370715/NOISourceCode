using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000606 RID: 1542
public class WoodSculptureConfig : IBuildingConfig
{
	// Token: 0x06001B3E RID: 6974 RVA: 0x000AA536 File Offset: 0x000A8736
	public override string[] GetRequiredDlcIds()
	{
		return DlcManager.DLC2;
	}

	// Token: 0x06001B3F RID: 6975 RVA: 0x001B6B30 File Offset: 0x001B4D30
	public override BuildingDef CreateBuildingDef()
	{
		string id = "WoodSculpture";
		int width = 1;
		int height = 1;
		string anim = "sculpture_wood_kanim";
		int hitpoints = 10;
		float construction_time = 120f;
		float[] tier = TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER4;
		string[] woods = MATERIALS.WOODS;
		float melting_point = 800f;
		BuildLocationRule build_location_rule = BuildLocationRule.Anywhere;
		EffectorValues none = NOISE_POLLUTION.NONE;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, woods, melting_point, build_location_rule, new EffectorValues
		{
			amount = 4,
			radius = 4
		}, none, 0.2f);
		buildingDef.SceneLayer = Grid.SceneLayer.InteriorWall;
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

	// Token: 0x06001B40 RID: 6976 RVA: 0x000AA54F File Offset: 0x000A874F
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.AddOrGet<BuildingComplete>().isArtable = true;
		go.GetComponent<KPrefabID>().AddTag(GameTags.Decoration, false);
	}

	// Token: 0x06001B41 RID: 6977 RVA: 0x000B633C File Offset: 0x000B453C
	public override void DoPostConfigureComplete(GameObject go)
	{
		go.AddComponent<LongRangeSculpture>().defaultAnimName = "slab";
	}

	// Token: 0x04001178 RID: 4472
	public const string ID = "WoodSculpture";
}
