using System;
using TUNING;
using UnityEngine;

// Token: 0x020005B4 RID: 1460
public class SpaceHeaterConfig : IBuildingConfig
{
	// Token: 0x0600194B RID: 6475 RVA: 0x001AE2AC File Offset: 0x001AC4AC
	public override BuildingDef CreateBuildingDef()
	{
		string id = "SpaceHeater";
		int width = 2;
		int height = 2;
		string anim = "spaceheater_kanim";
		int hitpoints = 30;
		float construction_time = 30f;
		float[] tier = BUILDINGS.CONSTRUCTION_MASS_KG.TIER4;
		string[] all_METALS = MATERIALS.ALL_METALS;
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER2;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, all_METALS, melting_point, build_location_rule, BUILDINGS.DECOR.BONUS.TIER1, tier2, 0.2f);
		buildingDef.RequiresPowerInput = true;
		buildingDef.EnergyConsumptionWhenActive = 240f;
		buildingDef.ExhaustKilowattsWhenActive = 0f;
		buildingDef.SelfHeatKilowattsWhenActive = 0f;
		buildingDef.LogicInputPorts = LogicOperationalController.CreateSingleInputPortList(new CellOffset(1, 0));
		buildingDef.ViewMode = OverlayModes.Temperature.ID;
		buildingDef.AudioCategory = "HollowMetal";
		buildingDef.OverheatTemperature = 398.15f;
		return buildingDef;
	}

	// Token: 0x0600194C RID: 6476 RVA: 0x001AE350 File Offset: 0x001AC550
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.AddOrGet<LoopingSounds>();
		go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.WarmingStation, false);
		go.AddOrGet<KBatchedAnimHeatPostProcessingEffect>();
		SpaceHeater spaceHeater = go.AddOrGet<SpaceHeater>();
		spaceHeater.targetTemperature = 343.15f;
		spaceHeater.produceHeat = true;
		WarmthProvider.Def def = go.AddOrGetDef<WarmthProvider.Def>();
		def.RangeMax = SpaceHeaterConfig.MAX_RANGE;
		def.RangeMin = SpaceHeaterConfig.MIN_RANGE;
		go.AddOrGetDef<ColdImmunityProvider.Def>().range = new CellOffset[][]
		{
			new CellOffset[]
			{
				new CellOffset(-1, 0),
				new CellOffset(2, 0)
			},
			new CellOffset[]
			{
				new CellOffset(0, 0),
				new CellOffset(1, 0)
			}
		};
		this.AddVisualizer(go);
	}

	// Token: 0x0600194D RID: 6477 RVA: 0x000B510A File Offset: 0x000B330A
	public override void DoPostConfigureUnderConstruction(GameObject go)
	{
		this.AddVisualizer(go);
	}

	// Token: 0x0600194E RID: 6478 RVA: 0x000B5113 File Offset: 0x000B3313
	public override void DoPostConfigurePreview(BuildingDef def, GameObject go)
	{
		this.AddVisualizer(go);
	}

	// Token: 0x0600194F RID: 6479 RVA: 0x000AA1AD File Offset: 0x000A83AD
	public override void DoPostConfigureComplete(GameObject go)
	{
		go.AddOrGet<LogicOperationalController>();
		go.AddOrGetDef<PoweredActiveController.Def>();
	}

	// Token: 0x06001950 RID: 6480 RVA: 0x001AE410 File Offset: 0x001AC610
	private void AddVisualizer(GameObject go)
	{
		RangeVisualizer rangeVisualizer = go.AddOrGet<RangeVisualizer>();
		rangeVisualizer.RangeMax = SpaceHeaterConfig.MAX_RANGE;
		rangeVisualizer.RangeMin = SpaceHeaterConfig.MIN_RANGE;
		rangeVisualizer.BlockingTileVisible = false;
		go.AddOrGet<EntityCellVisualizer>().AddPort(EntityCellVisualizer.Ports.HeatSource, default(CellOffset));
	}

	// Token: 0x04001063 RID: 4195
	public const string ID = "SpaceHeater";

	// Token: 0x04001064 RID: 4196
	public const float MAX_SELF_HEAT = 32f;

	// Token: 0x04001065 RID: 4197
	public const float MAX_EXHAUST_HEAT = 4f;

	// Token: 0x04001066 RID: 4198
	public const float MIN_POWER_USAGE = 120f;

	// Token: 0x04001067 RID: 4199
	public const float MAX_POWER_USAGE = 240f;

	// Token: 0x04001068 RID: 4200
	public static Vector2I MAX_RANGE = new Vector2I(5, 5);

	// Token: 0x04001069 RID: 4201
	public static Vector2I MIN_RANGE = new Vector2I(-4, -4);
}
