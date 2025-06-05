using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000555 RID: 1365
public class RemoteWorkerDockConfig : IBuildingConfig
{
	// Token: 0x0600177A RID: 6010 RVA: 0x001A6218 File Offset: 0x001A4418
	public override BuildingDef CreateBuildingDef()
	{
		string id = RemoteWorkerDockConfig.ID;
		int width = 1;
		int height = 2;
		string anim = "remote_work_dock_kanim";
		int hitpoints = 100;
		float construction_time = 60f;
		float[] tier = TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER4;
		string[] plastics = MATERIALS.PLASTICS;
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
		EffectorValues none = NOISE_POLLUTION.NONE;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, plastics, melting_point, build_location_rule, TUNING.BUILDINGS.DECOR.PENALTY.TIER1, none, 0.2f);
		buildingDef.Overheatable = false;
		buildingDef.AudioCategory = "Plastic";
		buildingDef.InputConduitType = ConduitType.Liquid;
		buildingDef.OutputConduitType = ConduitType.Liquid;
		buildingDef.ViewMode = OverlayModes.LiquidConduits.ID;
		buildingDef.UtilityInputOffset = new CellOffset(0, 1);
		buildingDef.UtilityOutputOffset = new CellOffset(0, 0);
		buildingDef.PowerInputOffset = new CellOffset(0, 0);
		buildingDef.RequiresPowerInput = true;
		buildingDef.EnergyConsumptionWhenActive = 120f;
		buildingDef.SelfHeatKilowattsWhenActive = 2f;
		buildingDef.ExhaustKilowattsWhenActive = 0f;
		buildingDef.AddSearchTerms(SEARCH_TERMS.ROBOT);
		return buildingDef;
	}

	// Token: 0x0600177B RID: 6011 RVA: 0x000B44BE File Offset: 0x000B26BE
	public override void DoPostConfigurePreview(BuildingDef def, GameObject go)
	{
		base.DoPostConfigurePreview(def, go);
		this.AddVisualizer(go);
	}

	// Token: 0x0600177C RID: 6012 RVA: 0x000B44CF File Offset: 0x000B26CF
	public override void DoPostConfigureUnderConstruction(GameObject go)
	{
		this.AddVisualizer(go);
	}

	// Token: 0x0600177D RID: 6013 RVA: 0x001A62E8 File Offset: 0x001A44E8
	public override void DoPostConfigureComplete(GameObject go)
	{
		go.AddOrGet<RemoteWorkerDock>();
		go.AddOrGet<RemoteWorkerDockAnimSM>();
		go.AddOrGet<Operational>();
		go.AddOrGet<UserNameable>();
		go.AddComponent<Storage>().SetDefaultStoredItemModifiers(Storage.StandardInsulatedStorage);
		ConduitConsumer conduitConsumer = go.AddOrGet<ConduitConsumer>();
		conduitConsumer.conduitType = ConduitType.Liquid;
		conduitConsumer.capacityTag = GameTags.LubricatingOil;
		conduitConsumer.capacityKG = 50f;
		conduitConsumer.wrongElementResult = ConduitConsumer.WrongElementResult.Dump;
		ConduitDispenser conduitDispenser = go.AddOrGet<ConduitDispenser>();
		conduitDispenser.conduitType = ConduitType.Liquid;
		conduitDispenser.elementFilter = new SimHashes[]
		{
			SimHashes.LiquidGunk
		};
		this.AddVisualizer(go);
		go.AddOrGet<RangeVisualizer>();
	}

	// Token: 0x0600177E RID: 6014 RVA: 0x000AA12F File Offset: 0x000A832F
	public override string[] GetRequiredDlcIds()
	{
		return DlcManager.DLC3;
	}

	// Token: 0x0600177F RID: 6015 RVA: 0x001A6378 File Offset: 0x001A4578
	private void AddVisualizer(GameObject prefab)
	{
		RangeVisualizer rangeVisualizer = prefab.AddOrGet<RangeVisualizer>();
		rangeVisualizer.RangeMin.x = -12;
		rangeVisualizer.RangeMin.y = 0;
		rangeVisualizer.RangeMax.x = 12;
		rangeVisualizer.RangeMax.y = 0;
		rangeVisualizer.OriginOffset = default(Vector2I);
		rangeVisualizer.BlockingTileVisible = false;
		prefab.GetComponent<KPrefabID>().instantiateFn += delegate(GameObject go)
		{
			go.GetComponent<RangeVisualizer>().BlockingCb = new Func<int, bool>(RemoteWorkerDockConfig.DockPathBlockingCB);
		};
	}

	// Token: 0x06001780 RID: 6016 RVA: 0x001A63FC File Offset: 0x001A45FC
	public static bool DockPathBlockingCB(int cell)
	{
		int num = Grid.CellAbove(cell);
		int num2 = Grid.CellBelow(cell);
		return num == Grid.InvalidCell || num2 == Grid.InvalidCell || (!Grid.Foundation[num2] && !Grid.Solid[num2]) || (Grid.Solid[cell] || Grid.Solid[num]);
	}

	// Token: 0x04000F7C RID: 3964
	public static string ID = "RemoteWorkerDock";

	// Token: 0x04000F7D RID: 3965
	public const float NEW_WORKER_DELAY_SECONDS = 2f;

	// Token: 0x04000F7E RID: 3966
	public const int WORK_RANGE = 12;

	// Token: 0x04000F7F RID: 3967
	public const float LUBRICANT_CAPACITY_KG = 50f;

	// Token: 0x04000F80 RID: 3968
	public const string ON_EMPTY_ANIM = "on_empty";

	// Token: 0x04000F81 RID: 3969
	public const string ON_FULL_ANIM = "on_full";

	// Token: 0x04000F82 RID: 3970
	public const string OFF_EMPTY_ANIM = "off_empty";

	// Token: 0x04000F83 RID: 3971
	public const string OFF_FULL_ANIM = "off_full";

	// Token: 0x04000F84 RID: 3972
	public const string NEW_WORKER_ANIM = "new_worker";
}
