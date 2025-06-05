using System;
using Database;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020004BB RID: 1211
public class MonumentTopConfig : IBuildingConfig
{
	// Token: 0x060014BD RID: 5309 RVA: 0x0019C564 File Offset: 0x0019A764
	public override BuildingDef CreateBuildingDef()
	{
		string id = "MonumentTop";
		int width = 5;
		int height = 5;
		string anim = "monument_upper_a_kanim";
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
			SimHashes.Glass.ToString(),
			SimHashes.Diamond.ToString(),
			SimHashes.Steel.ToString()
		};
		float melting_point = 9999f;
		BuildLocationRule build_location_rule = BuildLocationRule.BuildingAttachPoint;
		EffectorValues tier = NOISE_POLLUTION.NOISY.TIER2;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, construction_mass, construction_materials, melting_point, build_location_rule, TUNING.BUILDINGS.DECOR.BONUS.MONUMENT.INCOMPLETE, tier, 0.2f);
		BuildingTemplates.CreateMonumentBuildingDef(buildingDef);
		buildingDef.SceneLayer = Grid.SceneLayer.BuildingFront;
		buildingDef.OverheatTemperature = 2273.15f;
		buildingDef.Floodable = false;
		buildingDef.PermittedRotations = PermittedRotations.FlipH;
		buildingDef.AttachmentSlotTag = "MonumentTop";
		buildingDef.ObjectLayer = ObjectLayer.Building;
		buildingDef.attachablePosition = new CellOffset(0, 0);
		buildingDef.RequiresPowerInput = false;
		buildingDef.CanMove = false;
		buildingDef.AddSearchTerms(SEARCH_TERMS.STATUE);
		buildingDef.AddSearchTerms(SEARCH_TERMS.MORALE);
		return buildingDef;
	}

	// Token: 0x060014BE RID: 5310 RVA: 0x000B37A2 File Offset: 0x000B19A2
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		BuildingConfigManager.Instance.IgnoreDefaultKComponent(typeof(RequiresFoundation), prefab_tag);
		go.AddOrGet<LoopingSounds>();
		go.AddOrGet<MonumentPart>().part = MonumentPartResource.Part.Top;
	}

	// Token: 0x060014BF RID: 5311 RVA: 0x000AA038 File Offset: 0x000A8238
	public override void DoPostConfigurePreview(BuildingDef def, GameObject go)
	{
	}

	// Token: 0x060014C0 RID: 5312 RVA: 0x000AA038 File Offset: 0x000A8238
	public override void DoPostConfigureUnderConstruction(GameObject go)
	{
	}

	// Token: 0x060014C1 RID: 5313 RVA: 0x000B37CC File Offset: 0x000B19CC
	public override void DoPostConfigureComplete(GameObject go)
	{
		go.AddOrGet<KBatchedAnimController>().initialAnim = "option_a";
		go.GetComponent<KPrefabID>().prefabSpawnFn += delegate(GameObject game_object)
		{
			MonumentPart monumentPart = game_object.AddOrGet<MonumentPart>();
			monumentPart.part = MonumentPartResource.Part.Top;
			monumentPart.stateUISymbol = "upper";
		};
	}

	// Token: 0x04000E17 RID: 3607
	public const string ID = "MonumentTop";
}
