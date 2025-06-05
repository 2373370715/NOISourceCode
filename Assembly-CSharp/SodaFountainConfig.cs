using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020005A1 RID: 1441
public class SodaFountainConfig : IBuildingConfig
{
	// Token: 0x060018E9 RID: 6377 RVA: 0x001ACD4C File Offset: 0x001AAF4C
	public override BuildingDef CreateBuildingDef()
	{
		string id = "SodaFountain";
		int width = 2;
		int height = 2;
		string anim = "sodamaker_kanim";
		int hitpoints = 30;
		float construction_time = 10f;
		float[] tier = TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER3;
		string[] refined_METALS = MATERIALS.REFINED_METALS;
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
		EffectorValues none = NOISE_POLLUTION.NONE;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, refined_METALS, melting_point, build_location_rule, TUNING.BUILDINGS.DECOR.BONUS.TIER1, none, 0.2f);
		buildingDef.ViewMode = OverlayModes.LiquidConduits.ID;
		buildingDef.Floodable = true;
		buildingDef.AudioCategory = "Metal";
		buildingDef.Overheatable = true;
		buildingDef.InputConduitType = ConduitType.Liquid;
		buildingDef.UtilityInputOffset = new CellOffset(1, 1);
		buildingDef.RequiresPowerInput = true;
		buildingDef.PowerInputOffset = new CellOffset(1, 0);
		buildingDef.EnergyConsumptionWhenActive = 480f;
		buildingDef.SelfHeatKilowattsWhenActive = 1f;
		buildingDef.AddSearchTerms(SEARCH_TERMS.MORALE);
		return buildingDef;
	}

	// Token: 0x060018EA RID: 6378 RVA: 0x001ACE04 File Offset: 0x001AB004
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		KPrefabID component = go.GetComponent<KPrefabID>();
		component.AddTag(RoomConstraints.ConstraintTags.RecBuilding, false);
		Storage storage = go.AddOrGet<Storage>();
		storage.SetDefaultStoredItemModifiers(Storage.StandardFabricatorStorage);
		ConduitConsumer conduitConsumer = go.AddOrGet<ConduitConsumer>();
		conduitConsumer.conduitType = ConduitType.Liquid;
		conduitConsumer.capacityTag = ElementLoader.FindElementByHash(SimHashes.Water).tag;
		conduitConsumer.capacityKG = 20f;
		conduitConsumer.wrongElementResult = ConduitConsumer.WrongElementResult.Dump;
		ManualDeliveryKG manualDeliveryKG = go.AddOrGet<ManualDeliveryKG>();
		manualDeliveryKG.SetStorage(storage);
		manualDeliveryKG.RequestedItemTag = SimHashes.CarbonDioxide.CreateTag();
		manualDeliveryKG.capacity = 4f;
		manualDeliveryKG.refillMass = 1f;
		manualDeliveryKG.MinimumMass = 0.5f;
		manualDeliveryKG.choreTypeIDHash = Db.Get().ChoreTypes.MachineFetch.IdHash;
		go.AddOrGet<SodaFountainWorkable>().basePriority = RELAXATION.PRIORITY.TIER5;
		SodaFountain sodaFountain = go.AddOrGet<SodaFountain>();
		sodaFountain.specificEffect = "SodaFountain";
		sodaFountain.trackingEffect = "RecentlyRecDrink";
		sodaFountain.ingredientTag = SimHashes.CarbonDioxide.CreateTag();
		sodaFountain.ingredientMassPerUse = 1f;
		sodaFountain.waterMassPerUse = 5f;
		RoomTracker roomTracker = go.AddOrGet<RoomTracker>();
		roomTracker.requiredRoomType = Db.Get().RoomTypes.RecRoom.Id;
		roomTracker.requirement = RoomTracker.Requirement.Recommended;
		go.AddOrGetDef<RocketUsageRestriction.Def>();
		component.prefabInitFn += this.OnInit;
	}

	// Token: 0x060018EB RID: 6379 RVA: 0x001ACF50 File Offset: 0x001AB150
	private void OnInit(GameObject go)
	{
		SodaFountainWorkable component = go.GetComponent<SodaFountainWorkable>();
		KAnimFile[] value = new KAnimFile[]
		{
			Assets.GetAnim("anim_interacts_sodamaker_kanim")
		};
		component.workerTypeOverrideAnims.Add(MinionConfig.ID, value);
		component.workerTypeOverrideAnims.Add(BionicMinionConfig.ID, new KAnimFile[]
		{
			Assets.GetAnim("anim_bionic_interacts_sodamaker_kanim")
		});
	}

	// Token: 0x060018EC RID: 6380 RVA: 0x000AA038 File Offset: 0x000A8238
	public override void DoPostConfigureComplete(GameObject go)
	{
	}

	// Token: 0x04001040 RID: 4160
	public const string ID = "SodaFountain";
}
