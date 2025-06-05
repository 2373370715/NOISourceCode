using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020005BA RID: 1466
public class SpecialCargoBayClusterConfig : IBuildingConfig
{
	// Token: 0x06001965 RID: 6501 RVA: 0x000AA117 File Offset: 0x000A8317
	public override string[] GetRequiredDlcIds()
	{
		return DlcManager.EXPANSION1;
	}

	// Token: 0x06001966 RID: 6502 RVA: 0x001AE984 File Offset: 0x001ACB84
	public override BuildingDef CreateBuildingDef()
	{
		string id = "SpecialCargoBayCluster";
		int width = 3;
		int height = 1;
		string anim = "rocket_storage_live_small_kanim";
		int hitpoints = 1000;
		float construction_time = 60f;
		float[] hollow_TIER = TUNING.BUILDINGS.ROCKETRY_MASS_KG.HOLLOW_TIER1;
		string[] refined_METALS = MATERIALS.REFINED_METALS;
		float melting_point = 9999f;
		BuildLocationRule build_location_rule = BuildLocationRule.BuildingAttachPoint;
		EffectorValues tier = NOISE_POLLUTION.NOISY.TIER2;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, hollow_TIER, refined_METALS, melting_point, build_location_rule, TUNING.BUILDINGS.DECOR.NONE, tier, 0.2f);
		BuildingTemplates.CreateRocketBuildingDef(buildingDef);
		buildingDef.SceneLayer = Grid.SceneLayer.Building;
		buildingDef.OverheatTemperature = 2273.15f;
		buildingDef.Floodable = false;
		buildingDef.AttachmentSlotTag = GameTags.Rocket;
		buildingDef.ObjectLayer = ObjectLayer.Building;
		buildingDef.RequiresPowerInput = false;
		buildingDef.attachablePosition = new CellOffset(0, 0);
		buildingDef.CanMove = true;
		buildingDef.Cancellable = false;
		buildingDef.ShowInBuildMenu = false;
		buildingDef.AddSearchTerms(SEARCH_TERMS.TRANSPORT);
		return buildingDef;
	}

	// Token: 0x06001967 RID: 6503 RVA: 0x00148CE0 File Offset: 0x00146EE0
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		BuildingConfigManager.Instance.IgnoreDefaultKComponent(typeof(RequiresFoundation), prefab_tag);
		go.AddOrGet<LoopingSounds>();
		go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.IndustrialMachinery, false);
		go.AddOrGet<BuildingAttachPoint>().points = new BuildingAttachPoint.HardPoint[]
		{
			new BuildingAttachPoint.HardPoint(new CellOffset(0, 1), GameTags.Rocket, null)
		};
	}

	// Token: 0x06001968 RID: 6504 RVA: 0x001AEA3C File Offset: 0x001ACC3C
	public override void DoPostConfigureComplete(GameObject go)
	{
		Storage storage = go.AddOrGet<Storage>();
		storage.SetDefaultStoredItemModifiers(SpecialCargoBayClusterConfig.StoredCrittersModifiers);
		storage.showCapacityStatusItem = false;
		storage.showInUI = false;
		storage.storageFilters = new List<Tag>
		{
			GameTags.BagableCreature
		};
		storage.allowSettingOnlyFetchMarkedItems = false;
		storage.allowItemRemoval = false;
		Storage storage2 = go.AddComponent<Storage>();
		storage2.SetDefaultStoredItemModifiers(SpecialCargoBayClusterConfig.StoredLootModifiers);
		storage2.showCapacityStatusItem = false;
		storage2.showInUI = false;
		storage2.allowSettingOnlyFetchMarkedItems = false;
		storage2.allowItemRemoval = false;
		go.AddOrGet<CopyBuildingSettings>();
		go.AddOrGetDef<SpecialCargoBayCluster.Def>();
		SpecialCargoBayClusterReceptacle specialCargoBayClusterReceptacle = go.AddOrGet<SpecialCargoBayClusterReceptacle>();
		specialCargoBayClusterReceptacle.AddDepositTag(GameTags.BagableCreature);
		specialCargoBayClusterReceptacle.AddAdditionalCriteria((GameObject obj) => obj.HasTag(GameTags.Creatures.Deliverable));
		specialCargoBayClusterReceptacle.sideProductStorage = storage2;
		BuildingTemplates.ExtendBuildingToRocketModuleCluster(go, null, ROCKETRY.BURDEN.INSIGNIFICANT, 0f, 0f);
		SymbolOverrideControllerUtil.AddToPrefab(go);
		go.AddOrGet<Prioritizable>();
		Prioritizable.AddRef(go);
	}

	// Token: 0x04001082 RID: 4226
	public const string ID = "SpecialCargoBayCluster";

	// Token: 0x04001083 RID: 4227
	private static readonly List<Storage.StoredItemModifier> StoredCrittersModifiers = new List<Storage.StoredItemModifier>
	{
		Storage.StoredItemModifier.Insulate
	};

	// Token: 0x04001084 RID: 4228
	private static readonly List<Storage.StoredItemModifier> StoredLootModifiers = new List<Storage.StoredItemModifier>
	{
		Storage.StoredItemModifier.Hide,
		Storage.StoredItemModifier.Seal
	};
}
