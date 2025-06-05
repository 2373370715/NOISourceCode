using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020002ED RID: 749
public class FlowerVaseHangingConfig : IBuildingConfig
{
	// Token: 0x06000B85 RID: 2949 RVA: 0x00178CFC File Offset: 0x00176EFC
	public override BuildingDef CreateBuildingDef()
	{
		string id = "FlowerVaseHanging";
		int width = 1;
		int height = 2;
		string anim = "flowervase_hanging_basic_kanim";
		int hitpoints = 10;
		float construction_time = 10f;
		float[] tier = TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER1;
		string[] raw_METALS = MATERIALS.RAW_METALS;
		float melting_point = 800f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnCeiling;
		EffectorValues none = NOISE_POLLUTION.NONE;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, raw_METALS, melting_point, build_location_rule, TUNING.BUILDINGS.DECOR.NONE, none, 0.2f);
		buildingDef.Floodable = false;
		buildingDef.Overheatable = false;
		buildingDef.ViewMode = OverlayModes.Decor.ID;
		buildingDef.AudioCategory = "Glass";
		buildingDef.AudioSize = "large";
		buildingDef.AddSearchTerms(SEARCH_TERMS.MORALE);
		buildingDef.GenerateOffsets(1, 1);
		return buildingDef;
	}

	// Token: 0x06000B86 RID: 2950 RVA: 0x00178D8C File Offset: 0x00176F8C
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.AddOrGet<Storage>();
		Prioritizable.AddRef(go);
		PlantablePlot plantablePlot = go.AddOrGet<PlantablePlot>();
		plantablePlot.AddDepositTag(GameTags.DecorSeed);
		plantablePlot.occupyingObjectVisualOffset = new Vector3(0f, -0.25f, 0f);
		go.AddOrGet<FlowerVase>();
		go.GetComponent<KPrefabID>().AddTag(GameTags.Decoration, false);
	}

	// Token: 0x06000B87 RID: 2951 RVA: 0x000AA038 File Offset: 0x000A8238
	public override void DoPostConfigureComplete(GameObject go)
	{
	}

	// Token: 0x04000907 RID: 2311
	public const string ID = "FlowerVaseHanging";
}
