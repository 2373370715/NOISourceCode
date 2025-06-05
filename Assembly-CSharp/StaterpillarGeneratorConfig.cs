using System;
using Klei.AI;
using TUNING;
using UnityEngine;

// Token: 0x020005C7 RID: 1479
public class StaterpillarGeneratorConfig : IBuildingConfig
{
	// Token: 0x060019D1 RID: 6609 RVA: 0x000AA117 File Offset: 0x000A8317
	public override string[] GetRequiredDlcIds()
	{
		return DlcManager.EXPANSION1;
	}

	// Token: 0x060019D2 RID: 6610 RVA: 0x001B07F4 File Offset: 0x001AE9F4
	public override BuildingDef CreateBuildingDef()
	{
		string id = StaterpillarGeneratorConfig.ID;
		int width = 1;
		int height = 2;
		string anim = "egg_caterpillar_kanim";
		int hitpoints = 1000;
		float construction_time = 10f;
		string[] all_METALS = MATERIALS.ALL_METALS;
		float[] tier = BUILDINGS.CONSTRUCTION_MASS_KG.TIER3;
		string[] construction_materials = all_METALS;
		float melting_point = 9999f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnFoundationRotatable;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER0;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, construction_materials, melting_point, build_location_rule, BUILDINGS.DECOR.NONE, tier2, 0.2f);
		buildingDef.GeneratorWattageRating = 1600f;
		buildingDef.GeneratorBaseCapacity = buildingDef.GeneratorWattageRating;
		buildingDef.ExhaustKilowattsWhenActive = 2f;
		buildingDef.SelfHeatKilowattsWhenActive = 4f;
		buildingDef.Overheatable = false;
		buildingDef.Floodable = false;
		buildingDef.OverheatTemperature = 423.15f;
		buildingDef.PermittedRotations = PermittedRotations.FlipV;
		buildingDef.ViewMode = OverlayModes.Power.ID;
		buildingDef.AudioCategory = "Plastic";
		buildingDef.RequiresPowerOutput = true;
		buildingDef.PowerOutputOffset = new CellOffset(0, 1);
		buildingDef.PlayConstructionSounds = false;
		buildingDef.ShowInBuildMenu = false;
		return buildingDef;
	}

	// Token: 0x060019D3 RID: 6611 RVA: 0x000B01B2 File Offset: 0x000AE3B2
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		BuildingConfigManager.Instance.IgnoreDefaultKComponent(typeof(RequiresFoundation), prefab_tag);
	}

	// Token: 0x060019D4 RID: 6612 RVA: 0x000AA038 File Offset: 0x000A8238
	public override void DoPostConfigurePreview(BuildingDef def, GameObject go)
	{
	}

	// Token: 0x060019D5 RID: 6613 RVA: 0x000AA038 File Offset: 0x000A8238
	public override void DoPostConfigureUnderConstruction(GameObject go)
	{
	}

	// Token: 0x060019D6 RID: 6614 RVA: 0x000B56F0 File Offset: 0x000B38F0
	public override void DoPostConfigureComplete(GameObject go)
	{
		go.AddOrGet<StaterpillarGenerator>().powerDistributionOrder = 9;
		go.GetComponent<Deconstructable>().SetAllowDeconstruction(false);
		go.AddOrGet<Modifiers>();
		go.AddOrGet<Effects>();
		go.GetComponent<KSelectable>().IsSelectable = false;
	}

	// Token: 0x040010C7 RID: 4295
	public static readonly string ID = "StaterpillarGenerator";

	// Token: 0x040010C8 RID: 4296
	private const int WIDTH = 1;

	// Token: 0x040010C9 RID: 4297
	private const int HEIGHT = 2;
}
