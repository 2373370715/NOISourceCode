using System;
using TUNING;
using UnityEngine;

// Token: 0x020003CC RID: 972
public class LiquidHeaterConfig : IBuildingConfig
{
	// Token: 0x06000FD1 RID: 4049 RVA: 0x001889BC File Offset: 0x00186BBC
	public override BuildingDef CreateBuildingDef()
	{
		string id = "LiquidHeater";
		int width = 4;
		int height = 1;
		string anim = "boiler_kanim";
		int hitpoints = 30;
		float construction_time = 30f;
		float[] tier = BUILDINGS.CONSTRUCTION_MASS_KG.TIER4;
		string[] all_METALS = MATERIALS.ALL_METALS;
		float melting_point = 3200f;
		BuildLocationRule build_location_rule = BuildLocationRule.Anywhere;
		EffectorValues none = NOISE_POLLUTION.NONE;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, all_METALS, melting_point, build_location_rule, BUILDINGS.DECOR.PENALTY.TIER1, none, 0.2f);
		buildingDef.RequiresPowerInput = true;
		buildingDef.Floodable = false;
		buildingDef.EnergyConsumptionWhenActive = 960f;
		buildingDef.ExhaustKilowattsWhenActive = 4000f;
		buildingDef.SelfHeatKilowattsWhenActive = 64f;
		buildingDef.ViewMode = OverlayModes.Power.ID;
		buildingDef.AudioCategory = "SolidMetal";
		buildingDef.OverheatTemperature = 398.15f;
		buildingDef.LogicInputPorts = LogicOperationalController.CreateSingleInputPortList(new CellOffset(1, 0));
		return buildingDef;
	}

	// Token: 0x06000FD2 RID: 4050 RVA: 0x000B13B9 File Offset: 0x000AF5B9
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.AddOrGet<LoopingSounds>();
		SpaceHeater spaceHeater = go.AddOrGet<SpaceHeater>();
		spaceHeater.SetLiquidHeater();
		spaceHeater.targetTemperature = 358.15f;
		spaceHeater.minimumCellMass = 400f;
	}

	// Token: 0x06000FD3 RID: 4051 RVA: 0x000AA1AD File Offset: 0x000A83AD
	public override void DoPostConfigureComplete(GameObject go)
	{
		go.AddOrGet<LogicOperationalController>();
		go.AddOrGetDef<PoweredActiveController.Def>();
	}

	// Token: 0x04000B68 RID: 2920
	public const string ID = "LiquidHeater";

	// Token: 0x04000B69 RID: 2921
	public const float CONSUMPTION_RATE = 1f;
}
