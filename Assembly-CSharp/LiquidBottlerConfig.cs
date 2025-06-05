using System;
using System.Collections.Generic;
using TUNING;
using UnityEngine;

// Token: 0x020003BD RID: 957
public class LiquidBottlerConfig : IBuildingConfig
{
	// Token: 0x06000F83 RID: 3971 RVA: 0x001878A8 File Offset: 0x00185AA8
	public override BuildingDef CreateBuildingDef()
	{
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef("LiquidBottler", 3, 2, "liquid_bottler_kanim", 100, 120f, BUILDINGS.CONSTRUCTION_MASS_KG.TIER4, MATERIALS.ALL_METALS, 800f, BuildLocationRule.OnFloor, BUILDINGS.DECOR.PENALTY.TIER1, NOISE_POLLUTION.NOISY.TIER0, 0.2f);
		buildingDef.InputConduitType = ConduitType.Liquid;
		buildingDef.Floodable = false;
		buildingDef.ViewMode = OverlayModes.LiquidConduits.ID;
		buildingDef.AudioCategory = "HollowMetal";
		buildingDef.UtilityInputOffset = new CellOffset(0, 0);
		GeneratedBuildings.RegisterWithOverlay(OverlayScreen.LiquidVentIDs, "LiquidBottler");
		return buildingDef;
	}

	// Token: 0x06000F84 RID: 3972 RVA: 0x0018792C File Offset: 0x00185B2C
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		Storage storage = BuildingTemplates.CreateDefaultStorage(go, false);
		storage.showDescriptor = true;
		storage.storageFilters = STORAGEFILTERS.LIQUIDS;
		storage.capacityKg = 200f;
		storage.SetDefaultStoredItemModifiers(LiquidBottlerConfig.LiquidBottlerStoredItemModifiers);
		storage.allowItemRemoval = false;
		go.AddTag(GameTags.LiquidSource);
		DropAllWorkable dropAllWorkable = go.AddOrGet<DropAllWorkable>();
		dropAllWorkable.removeTags = new List<Tag>
		{
			GameTags.LiquidSource
		};
		dropAllWorkable.resetTargetWorkableOnCompleteWork = true;
		ConduitConsumer conduitConsumer = go.AddOrGet<ConduitConsumer>();
		conduitConsumer.storage = storage;
		conduitConsumer.conduitType = ConduitType.Liquid;
		conduitConsumer.ignoreMinMassCheck = true;
		conduitConsumer.forceAlwaysSatisfied = true;
		conduitConsumer.alwaysConsume = true;
		conduitConsumer.capacityKG = 200f;
		conduitConsumer.keepZeroMassObject = false;
		Bottler bottler = go.AddOrGet<Bottler>();
		bottler.storage = storage;
		bottler.workTime = 9f;
		bottler.consumer = conduitConsumer;
		bottler.userMaxCapacity = 200f;
	}

	// Token: 0x06000F85 RID: 3973 RVA: 0x000B020D File Offset: 0x000AE40D
	public override void DoPostConfigureComplete(GameObject go)
	{
		go.GetComponent<KPrefabID>().AddTag(GameTags.OverlayBehindConduits, false);
	}

	// Token: 0x04000B47 RID: 2887
	public const string ID = "LiquidBottler";

	// Token: 0x04000B48 RID: 2888
	private const ConduitType CONDUIT_TYPE = ConduitType.Liquid;

	// Token: 0x04000B49 RID: 2889
	private const int WIDTH = 3;

	// Token: 0x04000B4A RID: 2890
	private const int HEIGHT = 2;

	// Token: 0x04000B4B RID: 2891
	private const float CAPACITY = 200f;

	// Token: 0x04000B4C RID: 2892
	private static readonly List<Storage.StoredItemModifier> LiquidBottlerStoredItemModifiers = new List<Storage.StoredItemModifier>
	{
		Storage.StoredItemModifier.Hide,
		Storage.StoredItemModifier.Seal
	};
}
