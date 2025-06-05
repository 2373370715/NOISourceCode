using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020005D4 RID: 1492
public class StorageTileConfig : IBuildingConfig
{
	// Token: 0x06001A16 RID: 6678 RVA: 0x001B1804 File Offset: 0x001AFA04
	public override BuildingDef CreateBuildingDef()
	{
		string id = "StorageTile";
		int width = 1;
		int height = 1;
		string anim = "storagetile_kanim";
		int hitpoints = 30;
		float construction_time = 30f;
		float[] construction_mass = new float[]
		{
			100f,
			100f
		};
		string[] construction_materials = new string[]
		{
			"RefinedMetal",
			"Glass"
		};
		float melting_point = 800f;
		BuildLocationRule build_location_rule = BuildLocationRule.Tile;
		EffectorValues none = NOISE_POLLUTION.NONE;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, construction_mass, construction_materials, melting_point, build_location_rule, TUNING.BUILDINGS.DECOR.PENALTY.TIER1, none, 0.2f);
		BuildingTemplates.CreateFoundationTileDef(buildingDef);
		buildingDef.Floodable = false;
		buildingDef.Entombable = false;
		buildingDef.Overheatable = false;
		buildingDef.UseStructureTemperature = false;
		buildingDef.AudioCategory = "Glass";
		buildingDef.AudioSize = "small";
		buildingDef.BaseTimeUntilRepair = -1f;
		buildingDef.SceneLayer = Grid.SceneLayer.TileMain;
		buildingDef.AddSearchTerms(SEARCH_TERMS.TILE);
		buildingDef.AddSearchTerms(SEARCH_TERMS.STORAGE);
		buildingDef.ConstructionOffsetFilter = BuildingDef.ConstructionOffsetFilter_OneDown;
		return buildingDef;
	}

	// Token: 0x06001A17 RID: 6679 RVA: 0x001B18E4 File Offset: 0x001AFAE4
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		GeneratedBuildings.MakeBuildingAlwaysOperational(go);
		BuildingConfigManager.Instance.IgnoreDefaultKComponent(typeof(RequiresFoundation), prefab_tag);
		SimCellOccupier simCellOccupier = go.AddOrGet<SimCellOccupier>();
		simCellOccupier.movementSpeedMultiplier = DUPLICANTSTATS.MOVEMENT_MODIFIERS.PENALTY_2;
		simCellOccupier.notifyOnMelt = true;
		Storage storage = go.AddOrGet<Storage>();
		storage.SetDefaultStoredItemModifiers(StorageTileConfig.StoredItemModifiers);
		storage.capacityKg = StorageTileConfig.CAPACITY;
		storage.showInUI = true;
		storage.allowItemRemoval = true;
		storage.showDescriptor = true;
		storage.storageFilters = STORAGEFILTERS.STORAGE_LOCKERS_STANDARD;
		storage.fetchCategory = Storage.FetchCategory.GeneralStorage;
		storage.showCapacityStatusItem = true;
		storage.showCapacityAsMainStatus = true;
		go.AddOrGet<StorageTileSwitchItemWorkable>();
		TreeFilterable treeFilterable = go.AddOrGet<TreeFilterable>();
		treeFilterable.copySettingsEnabled = false;
		treeFilterable.dropIncorrectOnFilterChange = false;
		treeFilterable.preventAutoAddOnDiscovery = true;
		StorageTile.Def def = go.AddOrGetDef<StorageTile.Def>();
		def.MaxCapacity = StorageTileConfig.CAPACITY;
		def.specialItemCases = new StorageTile.SpecificItemTagSizeInstruction[]
		{
			new StorageTile.SpecificItemTagSizeInstruction(GameTags.AirtightSuit, 0.5f),
			new StorageTile.SpecificItemTagSizeInstruction(GameTags.Dehydrated, 0.6f)
		};
		go.AddOrGet<TileTemperature>();
		go.AddOrGet<BuildingHP>().destroyOnDamaged = true;
		Prioritizable.AddRef(go);
		go.AddOrGetDef<RocketUsageRestriction.Def>().restrictOperational = false;
	}

	// Token: 0x06001A18 RID: 6680 RVA: 0x000B0779 File Offset: 0x000AE979
	public override void DoPostConfigureComplete(GameObject go)
	{
		GeneratedBuildings.RemoveLoopingSounds(go);
		go.GetComponent<KPrefabID>().AddTag(GameTags.FloorTiles, false);
	}

	// Token: 0x040010E5 RID: 4325
	public const string ANIM_NAME = "storagetile_kanim";

	// Token: 0x040010E6 RID: 4326
	public const string ID = "StorageTile";

	// Token: 0x040010E7 RID: 4327
	public static float CAPACITY = 1000f;

	// Token: 0x040010E8 RID: 4328
	private static readonly List<Storage.StoredItemModifier> StoredItemModifiers = new List<Storage.StoredItemModifier>
	{
		Storage.StoredItemModifier.Insulate,
		Storage.StoredItemModifier.Seal,
		Storage.StoredItemModifier.Hide
	};
}
