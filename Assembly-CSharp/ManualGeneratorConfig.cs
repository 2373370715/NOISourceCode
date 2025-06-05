using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000407 RID: 1031
public class ManualGeneratorConfig : IBuildingConfig
{
	// Token: 0x0600111A RID: 4378 RVA: 0x0018CAF8 File Offset: 0x0018ACF8
	public override BuildingDef CreateBuildingDef()
	{
		string id = "ManualGenerator";
		int width = 2;
		int height = 2;
		string anim = "generatormanual_kanim";
		int hitpoints = 30;
		float construction_time = 30f;
		float[] tier = TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER3;
		string[] all_METALS = MATERIALS.ALL_METALS;
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER3;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, all_METALS, melting_point, build_location_rule, TUNING.BUILDINGS.DECOR.NONE, tier2, 0.2f);
		buildingDef.GeneratorWattageRating = 400f;
		buildingDef.GeneratorBaseCapacity = buildingDef.GeneratorWattageRating;
		buildingDef.RequiresPowerOutput = true;
		buildingDef.PowerOutputOffset = new CellOffset(0, 0);
		buildingDef.ViewMode = OverlayModes.Power.ID;
		buildingDef.AudioCategory = "Metal";
		buildingDef.Breakable = true;
		buildingDef.ForegroundLayer = Grid.SceneLayer.BuildingFront;
		buildingDef.LogicInputPorts = LogicOperationalController.CreateSingleInputPortList(new CellOffset(0, 0));
		buildingDef.SelfHeatKilowattsWhenActive = 1f;
		buildingDef.AddSearchTerms(SEARCH_TERMS.POWER);
		return buildingDef;
	}

	// Token: 0x0600111B RID: 4379 RVA: 0x0018CBBC File Offset: 0x0018ADBC
	public override void DoPostConfigureComplete(GameObject go)
	{
		go.AddOrGet<LogicOperationalController>();
		go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.IndustrialMachinery, false);
		go.AddOrGet<BuildingComplete>().isManuallyOperated = true;
		go.AddOrGet<LoopingSounds>();
		Prioritizable.AddRef(go);
		go.AddOrGet<Generator>().powerDistributionOrder = 10;
		ManualGenerator manualGenerator = go.AddOrGet<ManualGenerator>();
		manualGenerator.SetSliderValue(50f, 0);
		manualGenerator.workLayer = Grid.SceneLayer.BuildingFront;
		KBatchedAnimController kbatchedAnimController = go.AddOrGet<KBatchedAnimController>();
		kbatchedAnimController.fgLayer = Grid.SceneLayer.BuildingFront;
		kbatchedAnimController.initialAnim = "off";
	}

	// Token: 0x0600111C RID: 4380 RVA: 0x000B20DB File Offset: 0x000B02DB
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		base.ConfigureBuildingTemplate(go, prefab_tag);
		go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.GeneratorType, false);
		go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.LightDutyGeneratorType, false);
	}

	// Token: 0x04000BE0 RID: 3040
	public const string ID = "ManualGenerator";
}
