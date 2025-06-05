using System;
using TUNING;
using UnityEngine;

// Token: 0x020004F3 RID: 1267
public class OxyliteRefineryConfig : IBuildingConfig
{
	// Token: 0x060015C9 RID: 5577 RVA: 0x001A0838 File Offset: 0x0019EA38
	public override BuildingDef CreateBuildingDef()
	{
		string id = "OxyliteRefinery";
		int width = 3;
		int height = 4;
		string anim = "oxylite_refinery_kanim";
		int hitpoints = 100;
		float construction_time = 480f;
		string[] array = new string[]
		{
			"RefinedMetal",
			"Plastic"
		};
		float[] construction_mass = new float[]
		{
			BUILDINGS.CONSTRUCTION_MASS_KG.TIER5[0],
			BUILDINGS.CONSTRUCTION_MASS_KG.TIER2[0]
		};
		string[] construction_materials = array;
		float melting_point = 2400f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
		EffectorValues tier = NOISE_POLLUTION.NOISY.TIER5;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, construction_mass, construction_materials, melting_point, build_location_rule, BUILDINGS.DECOR.PENALTY.TIER1, tier, 0.2f);
		buildingDef.Overheatable = false;
		buildingDef.RequiresPowerInput = true;
		buildingDef.PowerInputOffset = new CellOffset(0, 0);
		buildingDef.EnergyConsumptionWhenActive = 1200f;
		buildingDef.ExhaustKilowattsWhenActive = 8f;
		buildingDef.SelfHeatKilowattsWhenActive = 4f;
		buildingDef.LogicInputPorts = LogicOperationalController.CreateSingleInputPortList(new CellOffset(0, 1));
		buildingDef.AudioCategory = "HollowMetal";
		buildingDef.InputConduitType = ConduitType.Gas;
		buildingDef.UtilityInputOffset = new CellOffset(1, 0);
		return buildingDef;
	}

	// Token: 0x060015CA RID: 5578 RVA: 0x001A0914 File Offset: 0x0019EB14
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		Tag tag = SimHashes.Oxygen.CreateTag();
		Tag tag2 = SimHashes.Gold.CreateTag();
		go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.IndustrialMachinery, false);
		OxyliteRefinery oxyliteRefinery = go.AddOrGet<OxyliteRefinery>();
		oxyliteRefinery.emitTag = SimHashes.OxyRock.CreateTag();
		oxyliteRefinery.emitMass = 10f;
		oxyliteRefinery.dropOffset = new Vector3(0f, 1f);
		ConduitConsumer conduitConsumer = go.AddOrGet<ConduitConsumer>();
		conduitConsumer.conduitType = ConduitType.Gas;
		conduitConsumer.consumptionRate = 1.2f;
		conduitConsumer.capacityTag = tag;
		conduitConsumer.capacityKG = 6f;
		conduitConsumer.forceAlwaysSatisfied = true;
		conduitConsumer.wrongElementResult = ConduitConsumer.WrongElementResult.Dump;
		Storage storage = go.AddOrGet<Storage>();
		storage.capacityKg = 23.2f;
		storage.SetDefaultStoredItemModifiers(Storage.StandardSealedStorage);
		storage.showInUI = true;
		ManualDeliveryKG manualDeliveryKG = go.AddOrGet<ManualDeliveryKG>();
		manualDeliveryKG.SetStorage(storage);
		manualDeliveryKG.RequestedItemTag = tag2;
		manualDeliveryKG.refillMass = 1.8000001f;
		manualDeliveryKG.capacity = 7.2000003f;
		manualDeliveryKG.choreTypeIDHash = Db.Get().ChoreTypes.MachineFetch.IdHash;
		ElementConverter elementConverter = go.AddOrGet<ElementConverter>();
		elementConverter.consumedElements = new ElementConverter.ConsumedElement[]
		{
			new ElementConverter.ConsumedElement(tag, 0.6f, true),
			new ElementConverter.ConsumedElement(tag2, 0.003f, true)
		};
		elementConverter.outputElements = new ElementConverter.OutputElement[]
		{
			new ElementConverter.OutputElement(0.6f, SimHashes.OxyRock, 303.15f, false, true, 0f, 0.5f, 1f, byte.MaxValue, 0, true)
		};
		Prioritizable.AddRef(go);
	}

	// Token: 0x060015CB RID: 5579 RVA: 0x000AA1AD File Offset: 0x000A83AD
	public override void DoPostConfigureComplete(GameObject go)
	{
		go.AddOrGet<LogicOperationalController>();
		go.AddOrGetDef<PoweredActiveController.Def>();
	}

	// Token: 0x04000EFD RID: 3837
	public const string ID = "OxyliteRefinery";

	// Token: 0x04000EFE RID: 3838
	public const float EMIT_MASS = 10f;

	// Token: 0x04000EFF RID: 3839
	public const float INPUT_O2_PER_SECOND = 0.6f;

	// Token: 0x04000F00 RID: 3840
	public const float OXYLITE_PER_SECOND = 0.6f;

	// Token: 0x04000F01 RID: 3841
	public const float GOLD_PER_SECOND = 0.003f;

	// Token: 0x04000F02 RID: 3842
	public const float OUTPUT_TEMP = 303.15f;

	// Token: 0x04000F03 RID: 3843
	public const float REFILL_RATE = 2400f;

	// Token: 0x04000F04 RID: 3844
	public const float GOLD_STORAGE_AMOUNT = 7.2000003f;

	// Token: 0x04000F05 RID: 3845
	public const float O2_STORAGE_AMOUNT = 6f;

	// Token: 0x04000F06 RID: 3846
	public const float STORAGE_CAPACITY = 23.2f;
}
