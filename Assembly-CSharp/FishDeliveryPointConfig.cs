using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000280 RID: 640
public class FishDeliveryPointConfig : IBuildingConfig
{
	// Token: 0x0600094D RID: 2381 RVA: 0x0016E98C File Offset: 0x0016CB8C
	public override BuildingDef CreateBuildingDef()
	{
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef("FishDeliveryPoint", 1, 3, "fishrelocator_kanim", 10, 10f, TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER1, MATERIALS.RAW_METALS, 1600f, BuildLocationRule.Anywhere, TUNING.BUILDINGS.DECOR.PENALTY.TIER2, NOISE_POLLUTION.NOISY.TIER0, 0.2f);
		buildingDef.AudioCategory = "Metal";
		buildingDef.Entombable = true;
		buildingDef.Floodable = true;
		buildingDef.ForegroundLayer = Grid.SceneLayer.TileMain;
		buildingDef.ViewMode = OverlayModes.Rooms.ID;
		buildingDef.AddSearchTerms(SEARCH_TERMS.RANCHING);
		buildingDef.AddSearchTerms(SEARCH_TERMS.CRITTER);
		return buildingDef;
	}

	// Token: 0x0600094E RID: 2382 RVA: 0x0016EA1C File Offset: 0x0016CC1C
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.GetComponent<KPrefabID>().AddTag(GameTags.CodexCategories.CreatureRelocator, false);
		Storage storage = go.AddOrGet<Storage>();
		storage.allowItemRemoval = false;
		storage.showDescriptor = true;
		storage.storageFilters = STORAGEFILTERS.SWIMMING_CREATURES;
		storage.workAnims = new HashedString[]
		{
			new HashedString("working_pre")
		};
		storage.workAnimPlayMode = KAnim.PlayMode.Once;
		storage.overrideAnims = new KAnimFile[]
		{
			Assets.GetAnim("anim_interacts_fishrelocator_kanim")
		};
		storage.synchronizeAnims = false;
		storage.useGunForDelivery = false;
		storage.allowSettingOnlyFetchMarkedItems = false;
		storage.faceTargetWhenWorking = false;
		CreatureDeliveryPoint creatureDeliveryPoint = go.AddOrGet<CreatureDeliveryPoint>();
		creatureDeliveryPoint.deliveryOffsets = new CellOffset[]
		{
			new CellOffset(0, 1)
		};
		creatureDeliveryPoint.spawnOffset = new CellOffset(0, -1);
		creatureDeliveryPoint.playAnimsOnFetch = true;
		BaggableCritterCapacityTracker baggableCritterCapacityTracker = go.AddOrGet<BaggableCritterCapacityTracker>();
		baggableCritterCapacityTracker.maximumCreatures = 20;
		baggableCritterCapacityTracker.cavityOffset = CellOffset.down;
		go.AddOrGet<TreeFilterable>();
	}

	// Token: 0x0600094F RID: 2383 RVA: 0x000AEA8B File Offset: 0x000ACC8B
	public override void DoPostConfigureComplete(GameObject go)
	{
		go.AddOrGetDef<MakeBaseSolid.Def>().solidOffsets = new CellOffset[]
		{
			new CellOffset(0, 0)
		};
	}

	// Token: 0x04000731 RID: 1841
	public const string ID = "FishDeliveryPoint";
}
