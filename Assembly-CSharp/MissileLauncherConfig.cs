using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000498 RID: 1176
public class MissileLauncherConfig : IBuildingConfig
{
	// Token: 0x06001416 RID: 5142 RVA: 0x0019AE18 File Offset: 0x00199018
	public override BuildingDef CreateBuildingDef()
	{
		string id = "MissileLauncher";
		int width = 3;
		int height = 5;
		string anim = "missile_launcher_kanim";
		int hitpoints = 250;
		float construction_time = 30f;
		float[] tier = TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER4;
		string[] all_METALS = MATERIALS.ALL_METALS;
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER5;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, all_METALS, melting_point, build_location_rule, TUNING.BUILDINGS.DECOR.NONE, tier2, 0.2f);
		buildingDef.SceneLayer = Grid.SceneLayer.BuildingFront;
		buildingDef.Floodable = false;
		buildingDef.Overheatable = false;
		buildingDef.AudioCategory = "Metal";
		buildingDef.BaseTimeUntilRepair = 400f;
		buildingDef.DefaultAnimState = "off";
		buildingDef.RequiresPowerInput = true;
		buildingDef.PowerInputOffset = new CellOffset(-1, 0);
		buildingDef.EnergyConsumptionWhenActive = 240f;
		buildingDef.InputConduitType = ConduitType.Solid;
		buildingDef.UtilityInputOffset = new CellOffset(0, 0);
		buildingDef.ViewMode = OverlayModes.SolidConveyor.ID;
		buildingDef.ExhaustKilowattsWhenActive = 0.5f;
		buildingDef.SelfHeatKilowattsWhenActive = 2f;
		buildingDef.AddSearchTerms(SEARCH_TERMS.MISSILE);
		return buildingDef;
	}

	// Token: 0x06001417 RID: 5143 RVA: 0x000B32B0 File Offset: 0x000B14B0
	public override void DoPostConfigurePreview(BuildingDef def, GameObject go)
	{
		this.AddVisualizer(go);
	}

	// Token: 0x06001418 RID: 5144 RVA: 0x000B32B9 File Offset: 0x000B14B9
	public override void DoPostConfigureUnderConstruction(GameObject go)
	{
		this.AddVisualizer(go);
	}

	// Token: 0x06001419 RID: 5145 RVA: 0x0019AEFC File Offset: 0x001990FC
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.AddOrGetDef<MissileLauncher.Def>();
		go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.IndustrialMachinery, false);
		Storage storage = BuildingTemplates.CreateDefaultStorage(go, false);
		storage.showInUI = true;
		storage.SetDefaultStoredItemModifiers(Storage.StandardSealedStorage);
		storage.storageFilters = new List<Tag>
		{
			"MissileBasic"
		};
		storage.allowSettingOnlyFetchMarkedItems = false;
		storage.fetchCategory = Storage.FetchCategory.GeneralStorage;
		storage.capacityKg = 300f;
		ManualDeliveryKG manualDeliveryKG = go.AddComponent<ManualDeliveryKG>();
		manualDeliveryKG.SetStorage(storage);
		manualDeliveryKG.RequestedItemTag = "MissileBasic";
		manualDeliveryKG.choreTypeIDHash = Db.Get().ChoreTypes.MachineFetch.IdHash;
		manualDeliveryKG.operationalRequirement = Operational.State.None;
		manualDeliveryKG.refillMass = 5f;
		manualDeliveryKG.MinimumMass = 1f;
		manualDeliveryKG.capacity = storage.Capacity() / 10f;
		manualDeliveryKG.MassPerUnit = 10f;
		SolidConduitConsumer solidConduitConsumer = go.AddOrGet<SolidConduitConsumer>();
		solidConduitConsumer.alwaysConsume = true;
		solidConduitConsumer.capacityKG = storage.Capacity();
		this.AddVisualizer(go);
	}

	// Token: 0x0600141A RID: 5146 RVA: 0x0019B000 File Offset: 0x00199200
	private void AddVisualizer(GameObject go)
	{
		RangeVisualizer rangeVisualizer = go2.AddOrGet<RangeVisualizer>();
		rangeVisualizer.OriginOffset = MissileLauncher.Def.LaunchOffset.ToVector2I();
		rangeVisualizer.RangeMin.x = -MissileLauncher.Def.launchRange.x;
		rangeVisualizer.RangeMax.x = MissileLauncher.Def.launchRange.x;
		rangeVisualizer.RangeMin.y = 0;
		rangeVisualizer.RangeMax.y = MissileLauncher.Def.launchRange.y;
		rangeVisualizer.AllowLineOfSightInvalidCells = true;
		go2.GetComponent<KPrefabID>().instantiateFn += delegate(GameObject go)
		{
			go.GetComponent<RangeVisualizer>().BlockingCb = new Func<int, bool>(MissileLauncherConfig.IsCellSkyBlocked);
		};
	}

	// Token: 0x0600141B RID: 5147 RVA: 0x000B32C2 File Offset: 0x000B14C2
	public override void DoPostConfigureComplete(GameObject go)
	{
		SymbolOverrideControllerUtil.AddToPrefab(go);
		go.AddOrGet<TreeFilterable>().dropIncorrectOnFilterChange = false;
		FlatTagFilterable flatTagFilterable = go.AddOrGet<FlatTagFilterable>();
		flatTagFilterable.displayOnlyDiscoveredTags = false;
		flatTagFilterable.headerText = STRINGS.BUILDINGS.PREFABS.MISSILELAUNCHER.TARGET_SELECTION_HEADER;
	}

	// Token: 0x0600141C RID: 5148 RVA: 0x0019B0A4 File Offset: 0x001992A4
	public static bool IsCellSkyBlocked(int cell)
	{
		if (PlayerController.Instance != null)
		{
			int num = Grid.InvalidCell;
			BuildTool buildTool = PlayerController.Instance.ActiveTool as BuildTool;
			SelectTool selectTool = PlayerController.Instance.ActiveTool as SelectTool;
			if (buildTool != null)
			{
				num = buildTool.GetLastCell;
			}
			else if (selectTool != null)
			{
				num = Grid.PosToCell(selectTool.selected);
			}
			if (Grid.IsValidCell(cell) && Grid.IsValidCell(num) && Grid.WorldIdx[cell] == Grid.WorldIdx[num])
			{
				return Grid.Solid[cell];
			}
		}
		return false;
	}

	// Token: 0x04000DCD RID: 3533
	public const string ID = "MissileLauncher";
}
