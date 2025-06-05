using System;
using TUNING;
using UnityEngine;

// Token: 0x020005EC RID: 1516
public class TravelTubeWallBridgeConfig : IBuildingConfig
{
	// Token: 0x06001A8E RID: 6798 RVA: 0x001B43E8 File Offset: 0x001B25E8
	public override BuildingDef CreateBuildingDef()
	{
		string id = "TravelTubeWallBridge";
		int width = 1;
		int height = 1;
		string anim = "tube_tile_bridge_kanim";
		int hitpoints = 100;
		float construction_time = 3f;
		float[] tier = BUILDINGS.CONSTRUCTION_MASS_KG.TIER2;
		string[] plastics = MATERIALS.PLASTICS;
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.Tile;
		EffectorValues none = NOISE_POLLUTION.NONE;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, plastics, melting_point, build_location_rule, BUILDINGS.DECOR.NONE, none, 0.2f);
		BuildingTemplates.CreateFoundationTileDef(buildingDef);
		buildingDef.Overheatable = false;
		buildingDef.Floodable = false;
		buildingDef.Entombable = false;
		buildingDef.ObjectLayer = ObjectLayer.Building;
		buildingDef.AudioCategory = "Plastic";
		buildingDef.AudioSize = "small";
		buildingDef.BaseTimeUntilRepair = -1f;
		buildingDef.PermittedRotations = PermittedRotations.R90;
		buildingDef.UtilityInputOffset = new CellOffset(0, 0);
		buildingDef.UtilityOutputOffset = new CellOffset(0, 2);
		buildingDef.SceneLayer = Grid.SceneLayer.BuildingFront;
		buildingDef.ForegroundLayer = Grid.SceneLayer.TileMain;
		return buildingDef;
	}

	// Token: 0x06001A8F RID: 6799 RVA: 0x000B5B52 File Offset: 0x000B3D52
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		GeneratedBuildings.MakeBuildingAlwaysOperational(go);
		SimCellOccupier simCellOccupier = go.AddOrGet<SimCellOccupier>();
		simCellOccupier.doReplaceElement = true;
		simCellOccupier.movementSpeedMultiplier = DUPLICANTSTATS.MOVEMENT_MODIFIERS.PENALTY_3;
		simCellOccupier.notifyOnMelt = true;
		go.AddOrGet<BuildingHP>().destroyOnDamaged = true;
		go.AddOrGet<TileTemperature>();
		go.AddOrGet<TravelTubeBridge>();
	}

	// Token: 0x06001A90 RID: 6800 RVA: 0x000B5B92 File Offset: 0x000B3D92
	public override void DoPostConfigurePreview(BuildingDef def, GameObject go)
	{
		base.DoPostConfigurePreview(def, go);
		this.AddNetworkLink(go).visualizeOnly = true;
		go.AddOrGet<BuildingCellVisualizer>();
	}

	// Token: 0x06001A91 RID: 6801 RVA: 0x000B5BB0 File Offset: 0x000B3DB0
	public override void DoPostConfigureUnderConstruction(GameObject go)
	{
		base.DoPostConfigureUnderConstruction(go);
		this.AddNetworkLink(go).visualizeOnly = true;
		go.AddOrGet<BuildingCellVisualizer>();
	}

	// Token: 0x06001A92 RID: 6802 RVA: 0x000B5BCD File Offset: 0x000B3DCD
	public override void DoPostConfigureComplete(GameObject go)
	{
		this.AddNetworkLink(go).visualizeOnly = false;
		go.AddOrGet<BuildingCellVisualizer>();
		go.AddOrGet<KPrefabID>().AddTag(GameTags.TravelTubeBridges, false);
	}

	// Token: 0x06001A93 RID: 6803 RVA: 0x000B5BF4 File Offset: 0x000B3DF4
	protected virtual TravelTubeUtilityNetworkLink AddNetworkLink(GameObject go)
	{
		TravelTubeUtilityNetworkLink travelTubeUtilityNetworkLink = go.AddOrGet<TravelTubeUtilityNetworkLink>();
		travelTubeUtilityNetworkLink.link1 = new CellOffset(-1, 0);
		travelTubeUtilityNetworkLink.link2 = new CellOffset(1, 0);
		return travelTubeUtilityNetworkLink;
	}

	// Token: 0x04001127 RID: 4391
	public const string ID = "TravelTubeWallBridge";
}
