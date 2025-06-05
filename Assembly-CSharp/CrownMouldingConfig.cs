using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000093 RID: 147
public class CrownMouldingConfig : IBuildingConfig
{
	// Token: 0x06000254 RID: 596 RVA: 0x0015033C File Offset: 0x0014E53C
	public override BuildingDef CreateBuildingDef()
	{
		string id = "CrownMoulding";
		int width = 1;
		int height = 1;
		string anim = "crown_moulding_kanim";
		int hitpoints = 10;
		float construction_time = 30f;
		float[] tier = TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER2;
		string[] raw_MINERALS = MATERIALS.RAW_MINERALS;
		float melting_point = 800f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnCeiling;
		EffectorValues none = NOISE_POLLUTION.NONE;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, raw_MINERALS, melting_point, build_location_rule, new EffectorValues
		{
			amount = 5,
			radius = 3
		}, none, 0.2f);
		buildingDef.DefaultAnimState = "S_U";
		buildingDef.Floodable = false;
		buildingDef.Overheatable = false;
		buildingDef.ViewMode = OverlayModes.Decor.ID;
		buildingDef.AudioCategory = "Metal";
		buildingDef.AudioSize = "small";
		buildingDef.AddSearchTerms(SEARCH_TERMS.MORALE);
		return buildingDef;
	}

	// Token: 0x06000255 RID: 597 RVA: 0x000AAD35 File Offset: 0x000A8F35
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.GetComponent<KPrefabID>().AddTag(GameTags.Decoration, false);
		go.AddOrGet<AnimTileable>();
	}

	// Token: 0x06000256 RID: 598 RVA: 0x000AA038 File Offset: 0x000A8238
	public override void DoPostConfigureComplete(GameObject go)
	{
	}

	// Token: 0x0400017A RID: 378
	public const string ID = "CrownMoulding";
}
