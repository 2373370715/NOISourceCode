using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020002EE RID: 750
public class FlowerVaseHangingFancyConfig : IBuildingConfig
{
	// Token: 0x06000B89 RID: 2953 RVA: 0x00178DE8 File Offset: 0x00176FE8
	public override BuildingDef CreateBuildingDef()
	{
		string id = "FlowerVaseHangingFancy";
		int width = 1;
		int height = 2;
		string anim = "flowervase_hanging_kanim";
		int hitpoints = 10;
		float construction_time = 10f;
		float[] tier = TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER1;
		string[] transparents = MATERIALS.TRANSPARENTS;
		float melting_point = 800f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnCeiling;
		EffectorValues none = NOISE_POLLUTION.NONE;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, transparents, melting_point, build_location_rule, new EffectorValues
		{
			amount = TUNING.BUILDINGS.DECOR.BONUS.TIER1.amount,
			radius = TUNING.BUILDINGS.DECOR.BONUS.TIER3.radius
		}, none, 0.2f);
		buildingDef.Floodable = false;
		buildingDef.Overheatable = false;
		buildingDef.ViewMode = OverlayModes.Decor.ID;
		buildingDef.AudioCategory = "Glass";
		buildingDef.AudioSize = "large";
		buildingDef.SceneLayer = Grid.SceneLayer.BuildingBack;
		buildingDef.ForegroundLayer = Grid.SceneLayer.BuildingUse;
		buildingDef.GenerateOffsets(1, 1);
		buildingDef.AddSearchTerms(SEARCH_TERMS.GLASS);
		buildingDef.AddSearchTerms(SEARCH_TERMS.MORALE);
		return buildingDef;
	}

	// Token: 0x06000B8A RID: 2954 RVA: 0x00178EBC File Offset: 0x001770BC
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.AddOrGet<Storage>();
		Prioritizable.AddRef(go);
		PlantablePlot plantablePlot = go.AddOrGet<PlantablePlot>();
		plantablePlot.AddDepositTag(GameTags.DecorSeed);
		plantablePlot.plantLayer = Grid.SceneLayer.BuildingUse;
		plantablePlot.occupyingObjectVisualOffset = new Vector3(0f, -0.45f, 0f);
		go.AddOrGet<FlowerVase>();
		go.GetComponent<KPrefabID>().AddTag(GameTags.Decoration, false);
	}

	// Token: 0x06000B8B RID: 2955 RVA: 0x000AA038 File Offset: 0x000A8238
	public override void DoPostConfigureComplete(GameObject go)
	{
	}

	// Token: 0x04000908 RID: 2312
	public const string ID = "FlowerVaseHangingFancy";
}
