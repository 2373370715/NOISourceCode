using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020004FB RID: 1275
public class PetroleumGeneratorConfig : IBuildingConfig
{
	// Token: 0x060015EA RID: 5610 RVA: 0x001A1130 File Offset: 0x0019F330
	public override BuildingDef CreateBuildingDef()
	{
		string id = "PetroleumGenerator";
		int width = 3;
		int height = 4;
		string anim = "generatorpetrol_kanim";
		int hitpoints = 100;
		float construction_time = 480f;
		string[] array = new string[]
		{
			"Metal"
		};
		float[] construction_mass = new float[]
		{
			TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER5[0]
		};
		string[] construction_materials = array;
		float melting_point = 2400f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
		EffectorValues tier = NOISE_POLLUTION.NOISY.TIER5;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, construction_mass, construction_materials, melting_point, build_location_rule, TUNING.BUILDINGS.DECOR.PENALTY.TIER2, tier, 0.2f);
		buildingDef.GeneratorWattageRating = 2000f;
		buildingDef.GeneratorBaseCapacity = buildingDef.GeneratorWattageRating;
		buildingDef.ExhaustKilowattsWhenActive = 4f;
		buildingDef.SelfHeatKilowattsWhenActive = 16f;
		buildingDef.ViewMode = OverlayModes.Power.ID;
		buildingDef.AudioCategory = "Metal";
		buildingDef.UtilityInputOffset = new CellOffset(-1, 0);
		buildingDef.RequiresPowerOutput = true;
		buildingDef.PowerOutputOffset = new CellOffset(1, 0);
		buildingDef.LogicInputPorts = LogicOperationalController.CreateSingleInputPortList(new CellOffset(0, 0));
		buildingDef.InputConduitType = ConduitType.Liquid;
		buildingDef.AddSearchTerms(SEARCH_TERMS.POWER);
		return buildingDef;
	}

	// Token: 0x060015EB RID: 5611 RVA: 0x001A121C File Offset: 0x0019F41C
	public override void DoPostConfigureComplete(GameObject go)
	{
		go.AddOrGet<LogicOperationalController>();
		go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.IndustrialMachinery, false);
		go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.PowerBuilding, false);
		go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.GeneratorType, false);
		go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.HeavyDutyGeneratorType, false);
		go.AddOrGet<LoopingSounds>();
		go.AddOrGet<Storage>();
		BuildingDef def = go.GetComponent<Building>().Def;
		float num = 20f;
		go.AddOrGet<LoopingSounds>();
		ConduitConsumer conduitConsumer = go.AddOrGet<ConduitConsumer>();
		conduitConsumer.conduitType = def.InputConduitType;
		conduitConsumer.consumptionRate = 10f;
		conduitConsumer.capacityTag = GameTags.CombustibleLiquid;
		conduitConsumer.capacityKG = num;
		conduitConsumer.forceAlwaysSatisfied = true;
		conduitConsumer.wrongElementResult = ConduitConsumer.WrongElementResult.Dump;
		EnergyGenerator energyGenerator = go.AddOrGet<EnergyGenerator>();
		energyGenerator.powerDistributionOrder = 8;
		energyGenerator.ignoreBatteryRefillPercent = true;
		energyGenerator.hasMeter = true;
		energyGenerator.formula = new EnergyGenerator.Formula
		{
			inputs = new EnergyGenerator.InputItem[]
			{
				new EnergyGenerator.InputItem(GameTags.CombustibleLiquid, 2f, num)
			},
			outputs = new EnergyGenerator.OutputItem[]
			{
				new EnergyGenerator.OutputItem(SimHashes.CarbonDioxide, 0.5f, false, new CellOffset(0, 3), 383.15f),
				new EnergyGenerator.OutputItem(SimHashes.DirtyWater, 0.75f, false, new CellOffset(1, 1), 313.15f)
			}
		};
		Tinkerable.MakePowerTinkerable(go);
		go.AddOrGetDef<PoweredActiveController.Def>();
	}

	// Token: 0x04000F10 RID: 3856
	public const string ID = "PetroleumGenerator";

	// Token: 0x04000F11 RID: 3857
	public const float CONSUMPTION_RATE = 2f;

	// Token: 0x04000F12 RID: 3858
	private const SimHashes INPUT_ELEMENT = SimHashes.Petroleum;

	// Token: 0x04000F13 RID: 3859
	private const SimHashes EXHAUST_ELEMENT_GAS = SimHashes.CarbonDioxide;

	// Token: 0x04000F14 RID: 3860
	private const SimHashes EXHAUST_ELEMENT_LIQUID = SimHashes.DirtyWater;

	// Token: 0x04000F15 RID: 3861
	public const float EFFICIENCY_RATE = 0.5f;

	// Token: 0x04000F16 RID: 3862
	public const float EXHAUST_GAS_RATE = 0.5f;

	// Token: 0x04000F17 RID: 3863
	public const float EXHAUST_LIQUID_RATE = 0.75f;

	// Token: 0x04000F18 RID: 3864
	private const int WIDTH = 3;

	// Token: 0x04000F19 RID: 3865
	private const int HEIGHT = 4;
}
