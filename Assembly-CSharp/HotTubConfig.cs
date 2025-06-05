using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000388 RID: 904
public class HotTubConfig : IBuildingConfig
{
	// Token: 0x06000E81 RID: 3713 RVA: 0x00183A2C File Offset: 0x00181C2C
	public override BuildingDef CreateBuildingDef()
	{
		string id = "HotTub";
		int width = 5;
		int height = 2;
		string anim = "hottub_kanim";
		int hitpoints = 30;
		float construction_time = 10f;
		float[] construction_mass = new float[]
		{
			200f,
			200f
		};
		string[] construction_materials = new string[]
		{
			"Metal",
			"BuildingWood"
		};
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
		EffectorValues none = NOISE_POLLUTION.NONE;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, construction_mass, construction_materials, melting_point, build_location_rule, TUNING.BUILDINGS.DECOR.BONUS.TIER3, none, 0.2f);
		buildingDef.SceneLayer = Grid.SceneLayer.BuildingFront;
		buildingDef.ViewMode = OverlayModes.LiquidConduits.ID;
		buildingDef.Floodable = true;
		buildingDef.AudioCategory = "Metal";
		buildingDef.Overheatable = true;
		buildingDef.OverheatTemperature = this.MINIMUM_WATER_TEMPERATURE;
		buildingDef.InputConduitType = ConduitType.Liquid;
		buildingDef.OutputConduitType = ConduitType.Liquid;
		buildingDef.UtilityInputOffset = new CellOffset(-1, 0);
		buildingDef.UtilityOutputOffset = new CellOffset(2, 0);
		buildingDef.RequiresPowerInput = true;
		buildingDef.PowerInputOffset = new CellOffset(-2, 0);
		buildingDef.EnergyConsumptionWhenActive = 240f;
		buildingDef.SelfHeatKilowattsWhenActive = 4f;
		buildingDef.ExhaustKilowattsWhenActive = 1f;
		buildingDef.AddSearchTerms(SEARCH_TERMS.MORALE);
		return buildingDef;
	}

	// Token: 0x06000E82 RID: 3714 RVA: 0x00183B3C File Offset: 0x00181D3C
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.RecBuilding, false);
		Storage storage = go.AddOrGet<Storage>();
		storage.SetDefaultStoredItemModifiers(Storage.StandardSealedStorage);
		ConduitConsumer conduitConsumer = go.AddOrGet<ConduitConsumer>();
		conduitConsumer.conduitType = ConduitType.Liquid;
		conduitConsumer.capacityTag = ElementLoader.FindElementByHash(SimHashes.Water).tag;
		conduitConsumer.capacityKG = this.WATER_AMOUNT;
		conduitConsumer.wrongElementResult = ConduitConsumer.WrongElementResult.Dump;
		conduitConsumer.storage = storage;
		conduitConsumer.SetOnState(false);
		ConduitDispenser conduitDispenser = go.AddOrGet<ConduitDispenser>();
		conduitDispenser.conduitType = ConduitType.Liquid;
		conduitDispenser.storage = storage;
		conduitDispenser.SetOnState(false);
		ManualDeliveryKG manualDeliveryKG = go.AddComponent<ManualDeliveryKG>();
		manualDeliveryKG.SetStorage(storage);
		manualDeliveryKG.RequestedItemTag = new Tag("BleachStone");
		manualDeliveryKG.capacity = 100f;
		manualDeliveryKG.refillMass = 10f;
		manualDeliveryKG.MinimumMass = 1f;
		manualDeliveryKG.choreTypeIDHash = Db.Get().ChoreTypes.MachineFetch.IdHash;
		HotTub hotTub = go.AddOrGet<HotTub>();
		hotTub.waterStorage = storage;
		hotTub.hotTubCapacity = this.WATER_AMOUNT;
		hotTub.waterCoolingRate = 15f;
		hotTub.minimumWaterTemperature = this.MINIMUM_WATER_TEMPERATURE;
		hotTub.bleachStoneConsumption = this.BLEACH_STONE_CONSUMPTION_RATE;
		hotTub.maxOperatingTemperature = this.MAXIMUM_TUB_TEMPERATURE;
		hotTub.specificEffect = "HotTub";
		hotTub.trackingEffect = "RecentlyHotTub";
		hotTub.basePriority = RELAXATION.PRIORITY.TIER4;
		RoomTracker roomTracker = go.AddOrGet<RoomTracker>();
		roomTracker.requiredRoomType = Db.Get().RoomTypes.RecRoom.Id;
		roomTracker.requirement = RoomTracker.Requirement.Recommended;
		go.AddOrGetDef<RocketUsageRestriction.Def>();
	}

	// Token: 0x06000E83 RID: 3715 RVA: 0x000B0AEB File Offset: 0x000AECEB
	public override void DoPostConfigureComplete(GameObject go)
	{
		go.GetComponent<RequireInputs>().requireConduitHasMass = false;
	}

	// Token: 0x04000AB2 RID: 2738
	public const string ID = "HotTub";

	// Token: 0x04000AB3 RID: 2739
	public const string COLD_IMMUNITY_EFFECT_NAME = "WarmTouch";

	// Token: 0x04000AB4 RID: 2740
	public const float COLD_IMMUNITY_DURATION = 1800f;

	// Token: 0x04000AB5 RID: 2741
	private float WATER_AMOUNT = 100f;

	// Token: 0x04000AB6 RID: 2742
	private const float KDTU_TRANSFER_RATE = 15f;

	// Token: 0x04000AB7 RID: 2743
	private float MINIMUM_WATER_TEMPERATURE = 310.85f;

	// Token: 0x04000AB8 RID: 2744
	private float MAXIMUM_TUB_TEMPERATURE = 310.85f;

	// Token: 0x04000AB9 RID: 2745
	private float BLEACH_STONE_CONSUMPTION_RATE = 0.11666667f;
}
