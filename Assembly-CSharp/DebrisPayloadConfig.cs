using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x02000096 RID: 150
public class DebrisPayloadConfig : IEntityConfig, IHasDlcRestrictions
{
	// Token: 0x06000264 RID: 612 RVA: 0x000AA117 File Offset: 0x000A8317
	public string[] GetRequiredDlcIds()
	{
		return DlcManager.EXPANSION1;
	}

	// Token: 0x06000265 RID: 613 RVA: 0x000AA765 File Offset: 0x000A8965
	public string[] GetForbiddenDlcIds()
	{
		return null;
	}

	// Token: 0x06000266 RID: 614 RVA: 0x001507A8 File Offset: 0x0014E9A8
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateLooseEntity("DebrisPayload", ITEMS.DEBRISPAYLOAD.NAME, ITEMS.DEBRISPAYLOAD.DESC, 100f, true, Assets.GetAnim("rocket_debris_combined_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 1f, 1f, true, 0, SimHashes.Creature, new List<Tag>
		{
			GameTags.IgnoreMaterialCategory,
			GameTags.Experimental
		});
		RailGunPayload.Def def = gameObject.AddOrGetDef<RailGunPayload.Def>();
		def.attractToBeacons = false;
		def.clusterAnimSymbolSwapTarget = "debris1";
		def.randomClusterSymbolSwaps = new List<string>
		{
			"debris1",
			"debris2",
			"debris3"
		};
		def.worldAnimSymbolSwapTarget = "debris";
		def.randomWorldSymbolSwaps = new List<string>
		{
			"debris",
			"2_debris",
			"3_debris"
		};
		SymbolOverrideControllerUtil.AddToPrefab(gameObject);
		gameObject.AddOrGet<LoopingSounds>();
		Storage storage = BuildingTemplates.CreateDefaultStorage(gameObject, false);
		storage.showInUI = true;
		storage.SetDefaultStoredItemModifiers(Storage.StandardSealedStorage);
		storage.allowSettingOnlyFetchMarkedItems = false;
		storage.allowItemRemoval = false;
		storage.capacityKg = 5000f;
		DropAllWorkable dropAllWorkable = gameObject.AddOrGet<DropAllWorkable>();
		dropAllWorkable.dropWorkTime = 30f;
		dropAllWorkable.choreTypeID = Db.Get().ChoreTypes.Fetch.Id;
		dropAllWorkable.ConfigureMultitoolContext("build", EffectConfigs.BuildSplashId);
		ClusterDestinationSelector clusterDestinationSelector = gameObject.AddOrGet<ClusterDestinationSelector>();
		clusterDestinationSelector.assignable = false;
		clusterDestinationSelector.shouldPointTowardsPath = true;
		clusterDestinationSelector.requireAsteroidDestination = true;
		clusterDestinationSelector.canNavigateFogOfWar = true;
		BallisticClusterGridEntity ballisticClusterGridEntity = gameObject.AddOrGet<BallisticClusterGridEntity>();
		ballisticClusterGridEntity.clusterAnimName = "rocket_debris_kanim";
		ballisticClusterGridEntity.isWorldEntity = true;
		ballisticClusterGridEntity.nameKey = new StringKey("STRINGS.ITEMS.DEBRISPAYLOAD.NAME");
		gameObject.AddOrGet<ClusterTraveler>();
		return gameObject;
	}

	// Token: 0x06000267 RID: 615 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000268 RID: 616 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x0400018F RID: 399
	public const string ID = "DebrisPayload";

	// Token: 0x04000190 RID: 400
	public const float MASS = 100f;

	// Token: 0x04000191 RID: 401
	public const float MAX_STORAGE_KG_MASS = 5000f;

	// Token: 0x04000192 RID: 402
	public const float STARMAP_SPEED = 10f;
}
