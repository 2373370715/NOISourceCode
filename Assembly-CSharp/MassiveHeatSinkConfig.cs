using System;
using TUNING;
using UnityEngine;

// Token: 0x0200040C RID: 1036
public class MassiveHeatSinkConfig : IBuildingConfig
{
	// Token: 0x06001130 RID: 4400 RVA: 0x0018D2A8 File Offset: 0x0018B4A8
	public override BuildingDef CreateBuildingDef()
	{
		string id = "MassiveHeatSink";
		int width = 4;
		int height = 4;
		string anim = "massiveheatsink_kanim";
		int hitpoints = 100;
		float construction_time = 120f;
		float[] tier = BUILDINGS.CONSTRUCTION_MASS_KG.TIER5;
		string[] raw_METALS = MATERIALS.RAW_METALS;
		float melting_point = 2400f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER5;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, raw_METALS, melting_point, build_location_rule, BUILDINGS.DECOR.BONUS.TIER2, tier2, 0.2f);
		buildingDef.ExhaustKilowattsWhenActive = -16f;
		buildingDef.SelfHeatKilowattsWhenActive = -64f;
		buildingDef.Floodable = true;
		buildingDef.Entombable = false;
		buildingDef.AudioCategory = "Metal";
		buildingDef.UtilityInputOffset = new CellOffset(0, 0);
		buildingDef.InputConduitType = ConduitType.Gas;
		buildingDef.ShowInBuildMenu = false;
		return buildingDef;
	}

	// Token: 0x06001131 RID: 4401 RVA: 0x0018D338 File Offset: 0x0018B538
	public override void DoPostConfigureComplete(GameObject go)
	{
		go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.IndustrialMachinery, false);
		go.AddOrGet<MassiveHeatSink>();
		go.AddOrGet<MinimumOperatingTemperature>().minimumTemperature = 100f;
		PrimaryElement component = go.GetComponent<PrimaryElement>();
		component.SetElement(SimHashes.Iron, true);
		component.Temperature = 294.15f;
		go.AddOrGet<LoopingSounds>();
		go.AddOrGet<Storage>().capacityKg = 0.099999994f;
		ConduitConsumer conduitConsumer = go.AddOrGet<ConduitConsumer>();
		conduitConsumer.conduitType = ConduitType.Gas;
		conduitConsumer.consumptionRate = 1f;
		conduitConsumer.capacityTag = GameTagExtensions.Create(SimHashes.Hydrogen);
		conduitConsumer.capacityKG = 0.099999994f;
		conduitConsumer.forceAlwaysSatisfied = true;
		conduitConsumer.wrongElementResult = ConduitConsumer.WrongElementResult.Dump;
		go.AddOrGet<ElementConverter>().consumedElements = new ElementConverter.ConsumedElement[]
		{
			new ElementConverter.ConsumedElement(ElementLoader.FindElementByHash(SimHashes.Hydrogen).tag, 0.01f, true)
		};
		go.AddOrGetDef<PoweredActiveController.Def>();
		go.GetComponent<Deconstructable>().allowDeconstruction = false;
		go.AddOrGet<Demolishable>();
	}

	// Token: 0x04000BF0 RID: 3056
	public const string ID = "MassiveHeatSink";

	// Token: 0x04000BF1 RID: 3057
	private const float CONSUMPTION_RATE = 0.01f;

	// Token: 0x04000BF2 RID: 3058
	private const float STORAGE_CAPACITY = 0.099999994f;
}
