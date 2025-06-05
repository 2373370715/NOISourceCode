using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000553 RID: 1363
public class RefrigeratorConfig : IBuildingConfig
{
	// Token: 0x06001771 RID: 6001 RVA: 0x001A5ECC File Offset: 0x001A40CC
	public override BuildingDef CreateBuildingDef()
	{
		string id = "Refrigerator";
		int width = 1;
		int height = 2;
		string anim = "fridge_kanim";
		int hitpoints = 30;
		float construction_time = 10f;
		float[] tier = TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER4;
		string[] raw_MINERALS = MATERIALS.RAW_MINERALS;
		float melting_point = 800f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER0;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, raw_MINERALS, melting_point, build_location_rule, TUNING.BUILDINGS.DECOR.BONUS.TIER1, tier2, 0.2f);
		buildingDef.RequiresPowerInput = true;
		buildingDef.AddLogicPowerPort = false;
		buildingDef.EnergyConsumptionWhenActive = 120f;
		buildingDef.SelfHeatKilowattsWhenActive = 0.125f;
		buildingDef.ExhaustKilowattsWhenActive = 0f;
		buildingDef.LogicOutputPorts = new List<LogicPorts.Port>
		{
			LogicPorts.Port.OutputPort(FilteredStorage.FULL_PORT_ID, new CellOffset(0, 1), STRINGS.BUILDINGS.PREFABS.REFRIGERATOR.LOGIC_PORT, STRINGS.BUILDINGS.PREFABS.REFRIGERATOR.LOGIC_PORT_ACTIVE, STRINGS.BUILDINGS.PREFABS.REFRIGERATOR.LOGIC_PORT_INACTIVE, false, false)
		};
		buildingDef.Floodable = false;
		buildingDef.ViewMode = OverlayModes.Power.ID;
		buildingDef.AudioCategory = "Metal";
		SoundEventVolumeCache.instance.AddVolume("fridge_kanim", "Refrigerator_open", NOISE_POLLUTION.NOISY.TIER1);
		SoundEventVolumeCache.instance.AddVolume("fridge_kanim", "Refrigerator_close", NOISE_POLLUTION.NOISY.TIER1);
		buildingDef.AddSearchTerms(SEARCH_TERMS.FRIDGE);
		return buildingDef;
	}

	// Token: 0x06001772 RID: 6002 RVA: 0x000B448F File Offset: 0x000B268F
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.GetComponent<KPrefabID>();
	}

	// Token: 0x06001773 RID: 6003 RVA: 0x001A5FE4 File Offset: 0x001A41E4
	public override void DoPostConfigureComplete(GameObject go)
	{
		Storage storage = go.AddOrGet<Storage>();
		storage.showInUI = true;
		storage.showDescriptor = true;
		storage.storageFilters = STORAGEFILTERS.FOOD;
		storage.allowItemRemoval = true;
		storage.capacityKg = 100f;
		storage.storageFullMargin = STORAGE.STORAGE_LOCKER_FILLED_MARGIN;
		storage.fetchCategory = Storage.FetchCategory.GeneralStorage;
		storage.showCapacityStatusItem = true;
		Prioritizable.AddRef(go);
		go.AddOrGet<TreeFilterable>().allResourceFilterLabelString = UI.UISIDESCREENS.TREEFILTERABLESIDESCREEN.ALLBUTTON_EDIBLES;
		go.AddOrGet<FoodStorage>();
		go.AddOrGet<Refrigerator>();
		RefrigeratorController.Def def = go.AddOrGetDef<RefrigeratorController.Def>();
		def.powerSaverEnergyUsage = 20f;
		def.coolingHeatKW = 0.375f;
		def.steadyHeatKW = 0f;
		go.AddOrGet<UserNameable>();
		go.AddOrGet<DropAllWorkable>();
		go.AddOrGetDef<RocketUsageRestriction.Def>().restrictOperational = false;
		go.AddOrGetDef<StorageController.Def>();
	}

	// Token: 0x04000F75 RID: 3957
	public const string ID = "Refrigerator";

	// Token: 0x04000F76 RID: 3958
	private const int ENERGY_SAVER_POWER = 20;
}
