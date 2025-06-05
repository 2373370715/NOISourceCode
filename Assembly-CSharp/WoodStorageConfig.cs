using System;
using System.Collections.Generic;
using TUNING;
using UnityEngine;

// Token: 0x02000607 RID: 1543
public class WoodStorageConfig : IBuildingConfig
{
	// Token: 0x06001B43 RID: 6979 RVA: 0x000AA536 File Offset: 0x000A8736
	public override string[] GetRequiredDlcIds()
	{
		return DlcManager.DLC2;
	}

	// Token: 0x06001B44 RID: 6980 RVA: 0x001B6C20 File Offset: 0x001B4E20
	public override BuildingDef CreateBuildingDef()
	{
		string id = "WoodStorage";
		int width = 3;
		int height = 2;
		string anim = "storageWood_kanim";
		int hitpoints = 30;
		float construction_time = 10f;
		float[] tier = BUILDINGS.CONSTRUCTION_MASS_KG.TIER4;
		string[] woods = MATERIALS.WOODS;
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
		EffectorValues none = NOISE_POLLUTION.NONE;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, woods, melting_point, build_location_rule, BUILDINGS.DECOR.BONUS.TIER1, none, 0.2f);
		buildingDef.Floodable = false;
		buildingDef.AudioCategory = "Metal";
		buildingDef.Overheatable = false;
		buildingDef.ShowInBuildMenu = false;
		return buildingDef;
	}

	// Token: 0x06001B45 RID: 6981 RVA: 0x001B6C88 File Offset: 0x001B4E88
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		SoundEventVolumeCache.instance.AddVolume("storagelocker_kanim", "StorageLocker_Hit_metallic_low", NOISE_POLLUTION.NOISY.TIER1);
		Prioritizable.AddRef(go);
		Storage storage = go.AddOrGet<Storage>();
		storage.showInUI = true;
		storage.allowItemRemoval = true;
		storage.showDescriptor = true;
		storage.storageFilters = new List<Tag>
		{
			GameTags.Organics
		};
		storage.storageFullMargin = STORAGE.STORAGE_LOCKER_FILLED_MARGIN;
		storage.fetchCategory = Storage.FetchCategory.GeneralStorage;
		storage.showCapacityStatusItem = true;
		storage.showCapacityAsMainStatus = true;
		storage.capacityKg = 20000f;
		go.AddOrGet<StorageMeter>();
		go.AddOrGetDef<RocketUsageRestriction.Def>();
		ManualDeliveryKG manualDeliveryKG = go.AddOrGet<ManualDeliveryKG>();
		manualDeliveryKG.SetStorage(storage);
		manualDeliveryKG.RequestedItemTag = "WoodLog".ToTag();
		manualDeliveryKG.capacity = storage.Capacity();
		manualDeliveryKG.refillMass = storage.Capacity();
		manualDeliveryKG.choreTypeIDHash = Db.Get().ChoreTypes.StorageFetch.IdHash;
	}

	// Token: 0x06001B46 RID: 6982 RVA: 0x000AAC8E File Offset: 0x000A8E8E
	public override void DoPostConfigureComplete(GameObject go)
	{
		go.AddOrGetDef<StorageController.Def>();
	}

	// Token: 0x04001179 RID: 4473
	public const string ID = "WoodStorage";
}
