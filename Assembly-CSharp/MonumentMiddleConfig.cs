using System;
using Database;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020004B9 RID: 1209
public class MonumentMiddleConfig : IBuildingConfig
{
	// Token: 0x060014B4 RID: 5300 RVA: 0x0019C3EC File Offset: 0x0019A5EC
	public override BuildingDef CreateBuildingDef()
	{
		string id = "MonumentMiddle";
		int width = 5;
		int height = 5;
		string anim = "monument_mid_a_kanim";
		int hitpoints = 1000;
		float construction_time = 60f;
		float[] construction_mass = new float[]
		{
			2500f,
			2500f,
			5000f
		};
		string[] construction_materials = new string[]
		{
			SimHashes.Ceramic.ToString(),
			SimHashes.Polypropylene.ToString(),
			SimHashes.Steel.ToString()
		};
		float melting_point = 9999f;
		BuildLocationRule build_location_rule = BuildLocationRule.BuildingAttachPoint;
		EffectorValues tier = NOISE_POLLUTION.NOISY.TIER2;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, construction_mass, construction_materials, melting_point, build_location_rule, TUNING.BUILDINGS.DECOR.BONUS.MONUMENT.INCOMPLETE, tier, 0.2f);
		BuildingTemplates.CreateMonumentBuildingDef(buildingDef);
		buildingDef.SceneLayer = Grid.SceneLayer.Building;
		buildingDef.OverheatTemperature = 2273.15f;
		buildingDef.Floodable = false;
		buildingDef.AttachmentSlotTag = "MonumentMiddle";
		buildingDef.ObjectLayer = ObjectLayer.Building;
		buildingDef.PermittedRotations = PermittedRotations.FlipH;
		buildingDef.attachablePosition = new CellOffset(0, 0);
		buildingDef.RequiresPowerInput = false;
		buildingDef.CanMove = false;
		buildingDef.AddSearchTerms(SEARCH_TERMS.STATUE);
		buildingDef.AddSearchTerms(SEARCH_TERMS.MORALE);
		return buildingDef;
	}

	// Token: 0x060014B5 RID: 5301 RVA: 0x0019C500 File Offset: 0x0019A700
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		BuildingConfigManager.Instance.IgnoreDefaultKComponent(typeof(RequiresFoundation), prefab_tag);
		go.AddOrGet<LoopingSounds>();
		go.AddOrGet<BuildingAttachPoint>().points = new BuildingAttachPoint.HardPoint[]
		{
			new BuildingAttachPoint.HardPoint(new CellOffset(0, 5), "MonumentTop", null)
		};
		go.AddOrGet<MonumentPart>().part = MonumentPartResource.Part.Middle;
	}

	// Token: 0x060014B6 RID: 5302 RVA: 0x000AA038 File Offset: 0x000A8238
	public override void DoPostConfigurePreview(BuildingDef def, GameObject go)
	{
	}

	// Token: 0x060014B7 RID: 5303 RVA: 0x000AA038 File Offset: 0x000A8238
	public override void DoPostConfigureUnderConstruction(GameObject go)
	{
	}

	// Token: 0x060014B8 RID: 5304 RVA: 0x000B3741 File Offset: 0x000B1941
	public override void DoPostConfigureComplete(GameObject go)
	{
		go.AddOrGet<KBatchedAnimController>().initialAnim = "option_a";
		go.GetComponent<KPrefabID>().prefabSpawnFn += delegate(GameObject game_object)
		{
			MonumentPart monumentPart = game_object.AddOrGet<MonumentPart>();
			monumentPart.part = MonumentPartResource.Part.Middle;
			monumentPart.stateUISymbol = "mid";
		};
	}

	// Token: 0x04000E14 RID: 3604
	public const string ID = "MonumentMiddle";
}
