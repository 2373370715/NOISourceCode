using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000390 RID: 912
public class IceKettleConfig : IBuildingConfig
{
	// Token: 0x06000EA5 RID: 3749 RVA: 0x000AA536 File Offset: 0x000A8736
	public override string[] GetRequiredDlcIds()
	{
		return DlcManager.DLC2;
	}

	// Token: 0x06000EA6 RID: 3750 RVA: 0x00184558 File Offset: 0x00182758
	public override BuildingDef CreateBuildingDef()
	{
		string id = "IceKettle";
		int width = 2;
		int height = 2;
		string anim = "icemelter_kettle_kanim";
		int hitpoints = 100;
		float construction_time = 10f;
		float[] tier = TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER4;
		string[] raw_METALS = MATERIALS.RAW_METALS;
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
		EffectorValues none = NOISE_POLLUTION.NONE;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, raw_METALS, melting_point, build_location_rule, TUNING.BUILDINGS.DECOR.NONE, none, 0.2f);
		float num = 3.7500002f;
		buildingDef.SelfHeatKilowattsWhenActive = num * 0.4f;
		buildingDef.ExhaustKilowattsWhenActive = num - buildingDef.SelfHeatKilowattsWhenActive;
		buildingDef.Floodable = false;
		buildingDef.Entombable = true;
		buildingDef.AudioCategory = "Metal";
		buildingDef.AudioSize = "large";
		buildingDef.UtilityInputOffset = new CellOffset(0, 0);
		buildingDef.UtilityOutputOffset = new CellOffset(0, 0);
		buildingDef.DefaultAnimState = "on";
		buildingDef.POIUnlockable = true;
		buildingDef.ShowInBuildMenu = true;
		buildingDef.AddSearchTerms(SEARCH_TERMS.WATER);
		return buildingDef;
	}

	// Token: 0x06000EA7 RID: 3751 RVA: 0x00184628 File Offset: 0x00182828
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.AddOrGet<LoopingSounds>();
		go.AddOrGet<BuildingComplete>().isManuallyOperated = true;
		go.AddTag(GameTags.LiquidSource);
		Storage storage = go.AddOrGet<Storage>();
		storage.capacityKg = Mathf.Ceil(152.80188f);
		storage.showInUI = true;
		storage.allowItemRemoval = false;
		storage.SetDefaultStoredItemModifiers(Storage.StandardInsulatedStorage);
		ManualDeliveryKG manualDeliveryKG = go.AddOrGet<ManualDeliveryKG>();
		manualDeliveryKG.capacity = Mathf.Ceil(152.80188f);
		manualDeliveryKG.SetStorage(storage);
		manualDeliveryKG.requestedItemTag = IceKettleConfig.FUEL_TAG;
		manualDeliveryKG.choreTypeIDHash = Db.Get().ChoreTypes.FetchCritical.IdHash;
		manualDeliveryKG.ShowStatusItem = false;
		Storage storage2 = go.AddComponent<Storage>();
		storage2.capacityKg = 1000f;
		storage2.showInUI = true;
		storage2.allowItemRemoval = false;
		storage2.SetDefaultStoredItemModifiers(Storage.StandardInsulatedStorage);
		ManualDeliveryKG manualDeliveryKG2 = go.AddComponent<ManualDeliveryKG>();
		manualDeliveryKG2.capacity = 1000f;
		manualDeliveryKG2.SetStorage(storage2);
		manualDeliveryKG2.requestedItemTag = IceKettleConfig.TARGET_ELEMENT_TAG;
		manualDeliveryKG2.choreTypeIDHash = Db.Get().ChoreTypes.FetchCritical.IdHash;
		manualDeliveryKG2.refillMass = 100f;
		manualDeliveryKG2.ShowStatusItem = false;
		Storage storage3 = go.AddComponent<Storage>();
		storage3.capacityKg = 500f;
		storage3.showInUI = true;
		storage3.allowItemRemoval = true;
		storage3.showDescriptor = true;
		storage3.SetDefaultStoredItemModifiers(Storage.StandardInsulatedStorage);
		IceKettle.Def def = go.AddOrGetDef<IceKettle.Def>();
		def.exhaust_tag = SimHashes.CarbonDioxide;
		def.targetElementTag = IceKettleConfig.TARGET_ELEMENT_TAG;
		def.KGToMeltPerBatch = 100f;
		def.KGMeltedPerSecond = 20f;
		def.fuelElementTag = IceKettleConfig.FUEL_TAG;
		def.TargetTemperature = 298.15f;
		def.EnergyPerUnitOfLumber = 4000f;
		def.ExhaustMassPerUnitOfLumber = 0.142f;
		go.AddOrGet<IceKettleWorkable>().storage = storage3;
	}

	// Token: 0x06000EA8 RID: 3752 RVA: 0x000AA038 File Offset: 0x000A8238
	public override void DoPostConfigureComplete(GameObject go)
	{
	}

	// Token: 0x04000AC7 RID: 2759
	public const string ID = "IceKettle";

	// Token: 0x04000AC8 RID: 2760
	public const SimHashes TARGET_ELEMENT = SimHashes.Ice;

	// Token: 0x04000AC9 RID: 2761
	public const float MASS_KG_PER_BATCH = 100f;

	// Token: 0x04000ACA RID: 2762
	public const float CAPACITY = 1000f;

	// Token: 0x04000ACB RID: 2763
	public const float FINAL_PRODUCT_CAPACITY = 500f;

	// Token: 0x04000ACC RID: 2764
	public static Tag TARGET_ELEMENT_TAG = SimHashes.Ice.CreateTag();

	// Token: 0x04000ACD RID: 2765
	public const float TARGET_TEMPERATURE = 298.15f;

	// Token: 0x04000ACE RID: 2766
	public const float PRODUCTION_PER_SECOND = 20f;

	// Token: 0x04000ACF RID: 2767
	public static Tag FUEL_TAG = SimHashes.WoodLog.CreateTag();

	// Token: 0x04000AD0 RID: 2768
	public const SimHashes EXHAUST_TAG = SimHashes.CarbonDioxide;

	// Token: 0x04000AD1 RID: 2769
	public const float TOTAL_ENERGY_OF_LUMBER = 7750f;

	// Token: 0x04000AD2 RID: 2770
	public const float ENERGY_OF_LUMBER_TAKEN_FOR_BUILDING_SELF_HEAT = 3750f;

	// Token: 0x04000AD3 RID: 2771
	public const float ENERGY_PER_UNIT_OF_LUMBER_TAKEN_FOR_MELTING = 4000f;

	// Token: 0x04000AD4 RID: 2772
	public const float FUEL_UNITS_REQUIRED_TO_MELT_ABSOLUTE_ZERO_BATCH = 15.280188f;

	// Token: 0x04000AD5 RID: 2773
	public const float FUEL_CAPACITY = 152.80188f;
}
