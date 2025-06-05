using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020004D8 RID: 1240
public class NuclearReactorConfig : IBuildingConfig
{
	// Token: 0x06001554 RID: 5460 RVA: 0x000AA117 File Offset: 0x000A8317
	public override string[] GetRequiredDlcIds()
	{
		return DlcManager.EXPANSION1;
	}

	// Token: 0x06001555 RID: 5461 RVA: 0x0019E400 File Offset: 0x0019C600
	public override BuildingDef CreateBuildingDef()
	{
		string id = "NuclearReactor";
		int width = 5;
		int height = 6;
		string anim = "generatornuclear_kanim";
		int hitpoints = 100;
		float construction_time = 480f;
		float[] tier = TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER5;
		string[] refined_METALS = MATERIALS.REFINED_METALS;
		float melting_point = 9999f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER5;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, refined_METALS, melting_point, build_location_rule, TUNING.BUILDINGS.DECOR.PENALTY.TIER2, tier2, 0.2f);
		buildingDef.GeneratorWattageRating = 0f;
		buildingDef.GeneratorBaseCapacity = buildingDef.GeneratorWattageRating;
		buildingDef.RequiresPowerInput = false;
		buildingDef.RequiresPowerOutput = false;
		buildingDef.ThermalConductivity = 0.1f;
		buildingDef.ExhaustKilowattsWhenActive = 0f;
		buildingDef.SelfHeatKilowattsWhenActive = 0f;
		buildingDef.Overheatable = false;
		buildingDef.InputConduitType = ConduitType.Liquid;
		buildingDef.ViewMode = OverlayModes.LiquidConduits.ID;
		buildingDef.UtilityInputOffset = new CellOffset(-2, 2);
		buildingDef.LogicInputPorts = new List<LogicPorts.Port>
		{
			LogicPorts.Port.InputPort("CONTROL_FUEL_DELIVERY", new CellOffset(0, 0), STRINGS.BUILDINGS.PREFABS.NUCLEARREACTOR.LOGIC_PORT, STRINGS.BUILDINGS.PREFABS.NUCLEARREACTOR.INPUT_PORT_ACTIVE, STRINGS.BUILDINGS.PREFABS.NUCLEARREACTOR.INPUT_PORT_INACTIVE, false, true)
		};
		buildingDef.ViewMode = OverlayModes.Temperature.ID;
		buildingDef.AudioCategory = "HollowMetal";
		buildingDef.AudioSize = "large";
		buildingDef.Floodable = false;
		buildingDef.Entombable = false;
		buildingDef.Breakable = false;
		buildingDef.Invincible = true;
		buildingDef.DiseaseCellVisName = "RadiationSickness";
		buildingDef.UtilityOutputOffset = new CellOffset(0, 2);
		buildingDef.Deprecated = !Sim.IsRadiationEnabled();
		return buildingDef;
	}

	// Token: 0x06001556 RID: 5462 RVA: 0x0019E560 File Offset: 0x0019C760
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.IndustrialMachinery, false);
		UnityEngine.Object.Destroy(go.GetComponent<BuildingEnabledButton>());
		RadiationEmitter radiationEmitter = go.AddComponent<RadiationEmitter>();
		radiationEmitter.emitType = RadiationEmitter.RadiationEmitterType.Constant;
		radiationEmitter.emitRadiusX = 25;
		radiationEmitter.emitRadiusY = 25;
		radiationEmitter.radiusProportionalToRads = false;
		radiationEmitter.emissionOffset = new Vector3(0f, 2f, 0f);
		Storage storage = go.AddComponent<Storage>();
		storage.SetDefaultStoredItemModifiers(new List<Storage.StoredItemModifier>
		{
			Storage.StoredItemModifier.Seal,
			Storage.StoredItemModifier.Insulate,
			Storage.StoredItemModifier.Hide
		});
		go.AddComponent<Storage>().SetDefaultStoredItemModifiers(new List<Storage.StoredItemModifier>
		{
			Storage.StoredItemModifier.Seal,
			Storage.StoredItemModifier.Insulate,
			Storage.StoredItemModifier.Hide
		});
		go.AddComponent<Storage>().SetDefaultStoredItemModifiers(new List<Storage.StoredItemModifier>
		{
			Storage.StoredItemModifier.Seal,
			Storage.StoredItemModifier.Insulate,
			Storage.StoredItemModifier.Hide
		});
		ManualDeliveryKG manualDeliveryKG = go.AddComponent<ManualDeliveryKG>();
		manualDeliveryKG.RequestedItemTag = ElementLoader.FindElementByHash(SimHashes.EnrichedUranium).tag;
		manualDeliveryKG.SetStorage(storage);
		manualDeliveryKG.choreTypeIDHash = Db.Get().ChoreTypes.PowerFetch.IdHash;
		manualDeliveryKG.capacity = 180f;
		manualDeliveryKG.MinimumMass = 0.5f;
		go.AddOrGet<Reactor>();
		go.AddOrGet<LoopingSounds>();
		Prioritizable.AddRef(go);
		ConduitConsumer conduitConsumer = go.AddOrGet<ConduitConsumer>();
		conduitConsumer.conduitType = ConduitType.Liquid;
		conduitConsumer.consumptionRate = 10f;
		conduitConsumer.capacityKG = 90f;
		conduitConsumer.capacityTag = GameTags.AnyWater;
		conduitConsumer.forceAlwaysSatisfied = true;
		conduitConsumer.wrongElementResult = ConduitConsumer.WrongElementResult.Dump;
		conduitConsumer.storage = storage;
	}

	// Token: 0x06001557 RID: 5463 RVA: 0x000B3FB3 File Offset: 0x000B21B3
	public override void DoPostConfigureComplete(GameObject go)
	{
		go.AddTag(GameTags.CorrosionProof);
	}

	// Token: 0x04000EAC RID: 3756
	public const string ID = "NuclearReactor";

	// Token: 0x04000EAD RID: 3757
	private const float FUEL_CAPACITY = 180f;

	// Token: 0x04000EAE RID: 3758
	public const float VENT_STEAM_TEMPERATURE = 673.15f;

	// Token: 0x04000EAF RID: 3759
	public const float MELT_DOWN_TEMPERATURE = 3000f;

	// Token: 0x04000EB0 RID: 3760
	public const float MAX_VENT_PRESSURE = 150f;

	// Token: 0x04000EB1 RID: 3761
	public const float INCREASED_CONDUCTION_SCALE = 5f;

	// Token: 0x04000EB2 RID: 3762
	public const float REACTION_STRENGTH = 100f;

	// Token: 0x04000EB3 RID: 3763
	public const int RADIATION_EMITTER_RANGE = 25;

	// Token: 0x04000EB4 RID: 3764
	public const float OPERATIONAL_RADIATOR_INTENSITY = 2400f;

	// Token: 0x04000EB5 RID: 3765
	public const float MELT_DOWN_RADIATOR_INTENSITY = 4800f;

	// Token: 0x04000EB6 RID: 3766
	public const float FUEL_CONSUMPTION_SPEED = 0.016666668f;

	// Token: 0x04000EB7 RID: 3767
	public const float BEGIN_REACTION_MASS = 0.5f;

	// Token: 0x04000EB8 RID: 3768
	public const float STOP_REACTION_MASS = 0.25f;

	// Token: 0x04000EB9 RID: 3769
	public const float DUMP_WASTE_AMOUNT = 100f;

	// Token: 0x04000EBA RID: 3770
	public const float WASTE_MASS_MULTIPLIER = 100f;

	// Token: 0x04000EBB RID: 3771
	public const float REACTION_MASS_TARGET = 60f;

	// Token: 0x04000EBC RID: 3772
	public const float COOLANT_AMOUNT = 30f;

	// Token: 0x04000EBD RID: 3773
	public const float COOLANT_CAPACITY = 90f;

	// Token: 0x04000EBE RID: 3774
	public const float MINIMUM_COOLANT_MASS = 30f;

	// Token: 0x04000EBF RID: 3775
	public const float WASTE_GERMS_PER_KG = 50f;

	// Token: 0x04000EC0 RID: 3776
	public const float PST_MELTDOWN_COOLING_TIME = 3000f;

	// Token: 0x04000EC1 RID: 3777
	public const string INPUT_PORT_ID = "CONTROL_FUEL_DELIVERY";
}
