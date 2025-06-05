using System;
using FoodRehydrator;
using Klei.AI;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000335 RID: 821
public class FoodRehydratorConfig : IBuildingConfig
{
	// Token: 0x06000CDA RID: 3290 RVA: 0x0017B630 File Offset: 0x00179830
	private static Effect ConstructRehydrationEffect()
	{
		Effect effect = new Effect("RehydratedFoodConsumed", "RehydratedFoodConsumed", STRINGS.ITEMS.DEHYDRATEDFOODPACKAGE.CONSUMED, 600f, false, false, true, null, -1f, 0f, null, "");
		effect.Add(new AttributeModifier(Db.Get().Attributes.QualityOfLife.Id, -1f, STRINGS.ITEMS.DEHYDRATEDFOODPACKAGE.CONSUMED, false, false, true));
		return effect;
	}

	// Token: 0x06000CDB RID: 3291 RVA: 0x0017B6A0 File Offset: 0x001798A0
	public override BuildingDef CreateBuildingDef()
	{
		string id = "FoodRehydrator";
		int width = 1;
		int height = 2;
		string anim = "Rehydrator_kanim";
		int hitpoints = 10;
		float construction_time = 120f;
		float[] construction_mass = new float[]
		{
			TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER2[0],
			TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER1[0]
		};
		string[] construction_materials = new string[]
		{
			"RefinedMetal",
			"Plastic"
		};
		float melting_point = 800f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
		EffectorValues none = NOISE_POLLUTION.NONE;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, construction_mass, construction_materials, melting_point, build_location_rule, TUNING.BUILDINGS.DECOR.BONUS.TIER0, none, 0.2f);
		buildingDef.Overheatable = true;
		buildingDef.Floodable = false;
		buildingDef.AudioCategory = "Plastic";
		buildingDef.AudioSize = "small";
		buildingDef.RequiresPowerInput = true;
		buildingDef.InputConduitType = ConduitType.Liquid;
		buildingDef.UtilityInputOffset = new CellOffset(0, 0);
		buildingDef.EnergyConsumptionWhenActive = 60f;
		buildingDef.SelfHeatKilowattsWhenActive = 0.5f;
		buildingDef.ViewMode = OverlayModes.Power.ID;
		buildingDef.AddSearchTerms(SEARCH_TERMS.FOOD);
		return buildingDef;
	}

	// Token: 0x06000CDC RID: 3292 RVA: 0x0017B77C File Offset: 0x0017997C
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		Storage storage = go.AddComponent<Storage>();
		storage.capacityKg = 5f;
		storage.showInUI = true;
		storage.showDescriptor = false;
		storage.allowItemRemoval = false;
		storage.showCapacityStatusItem = true;
		storage.showCapacityAsMainStatus = true;
		ManualDeliveryKG manualDeliveryKG = go.AddOrGet<ManualDeliveryKG>();
		manualDeliveryKG.SetStorage(storage);
		manualDeliveryKG.capacity = 5f;
		manualDeliveryKG.refillMass = 5f;
		manualDeliveryKG.MinimumMass = 1f;
		manualDeliveryKG.choreTypeIDHash = Db.Get().ChoreTypes.StorageFetch.Id;
		manualDeliveryKG.requestedItemTag = GameTags.Dehydrated;
		manualDeliveryKG.operationalRequirement = Operational.State.Functional;
		Storage storage2 = go.AddComponent<Storage>();
		storage2.showCapacityStatusItem = true;
		storage2.allowItemRemoval = false;
		storage2.showCapacityStatusItem = true;
		storage2.capacityKg = 20f;
		ConduitConsumer conduitConsumer = go.AddComponent<ConduitConsumer>();
		conduitConsumer.capacityTag = FoodRehydratorConfig.REHYDRATION_TAG;
		conduitConsumer.capacityKG = storage2.capacityKg;
		conduitConsumer.storage = storage2;
		conduitConsumer.alwaysConsume = true;
		Prioritizable.AddRef(go);
		go.AddOrGet<AccessabilityManager>();
		go.AddOrGet<DehydratedManager>();
		go.AddOrGet<ResourceRequirementMonitor>();
		go.AddOrGetDef<FoodRehydratorSM.Def>();
		go.AddOrGet<UserNameable>();
		go.AddOrGetDef<RocketUsageRestriction.Def>();
	}

	// Token: 0x06000CDD RID: 3293 RVA: 0x000AA038 File Offset: 0x000A8238
	public override void DoPostConfigureComplete(GameObject go)
	{
	}

	// Token: 0x040009A3 RID: 2467
	public const string ID = "FoodRehydrator";

	// Token: 0x040009A4 RID: 2468
	public static Tag REHYDRATION_TAG = GameTags.Water;

	// Token: 0x040009A5 RID: 2469
	public const float REHYDRATION_COST = 1f;

	// Token: 0x040009A6 RID: 2470
	public const float REHYDRATOR_PACKAGES_CAPACITY = 5f;

	// Token: 0x040009A7 RID: 2471
	public const float REHYDRATION_WORK_TIME = 5f;

	// Token: 0x040009A8 RID: 2472
	public static Effect RehydrationEffect = FoodRehydratorConfig.ConstructRehydrationEffect();

	// Token: 0x040009A9 RID: 2473
	public const string REHYDRATION_DEBUFF_ID = "RehydratedFoodConsumed";

	// Token: 0x040009AA RID: 2474
	public const string REHDYRATION_DEBUFF_NAME = "RehydratedFoodConsumed";

	// Token: 0x040009AB RID: 2475
	public const float REHYDRATION_DEBUFF_DURATION = 600f;

	// Token: 0x040009AC RID: 2476
	public const float REHYDRATION_DEBUFF_EFFECT = -1f;
}
