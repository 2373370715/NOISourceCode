using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020005EA RID: 1514
public class TravelTubeConfig : IBuildingConfig
{
	// Token: 0x06001A85 RID: 6789 RVA: 0x001B41AC File Offset: 0x001B23AC
	public override BuildingDef CreateBuildingDef()
	{
		string id = "TravelTube";
		int width = 1;
		int height = 1;
		string anim = "travel_tube_kanim";
		int hitpoints = 30;
		float construction_time = 10f;
		float[] tier = TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER1;
		string[] plastics = MATERIALS.PLASTICS;
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.NotInTiles;
		EffectorValues none = NOISE_POLLUTION.NONE;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, plastics, melting_point, build_location_rule, TUNING.BUILDINGS.DECOR.BONUS.TIER0, none, 0.2f);
		buildingDef.Floodable = false;
		buildingDef.Overheatable = false;
		buildingDef.Entombable = false;
		buildingDef.ObjectLayer = ObjectLayer.Building;
		buildingDef.TileLayer = ObjectLayer.TravelTubeTile;
		buildingDef.ReplacementLayer = ObjectLayer.ReplacementTravelTube;
		buildingDef.AudioCategory = "Plastic";
		buildingDef.AudioSize = "small";
		buildingDef.BaseTimeUntilRepair = 0f;
		buildingDef.UtilityInputOffset = new CellOffset(0, 0);
		buildingDef.UtilityOutputOffset = new CellOffset(0, 0);
		buildingDef.SceneLayer = Grid.SceneLayer.BuildingFront;
		buildingDef.isKAnimTile = true;
		buildingDef.isUtility = true;
		buildingDef.DragBuild = true;
		buildingDef.AddSearchTerms(SEARCH_TERMS.TRANSPORT);
		return buildingDef;
	}

	// Token: 0x06001A86 RID: 6790 RVA: 0x000B5ADB File Offset: 0x000B3CDB
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		GeneratedBuildings.MakeBuildingAlwaysOperational(go);
		BuildingConfigManager.Instance.IgnoreDefaultKComponent(typeof(RequiresFoundation), prefab_tag);
		go.AddOrGet<TravelTube>();
	}

	// Token: 0x06001A87 RID: 6791 RVA: 0x000B5AFF File Offset: 0x000B3CFF
	public override void DoPostConfigureUnderConstruction(GameObject go)
	{
		KAnimGraphTileVisualizer kanimGraphTileVisualizer = go.AddComponent<KAnimGraphTileVisualizer>();
		kanimGraphTileVisualizer.connectionSource = KAnimGraphTileVisualizer.ConnectionSource.Tube;
		kanimGraphTileVisualizer.isPhysicalBuilding = false;
	}

	// Token: 0x06001A88 RID: 6792 RVA: 0x000B5B14 File Offset: 0x000B3D14
	public override void DoPostConfigureComplete(GameObject go)
	{
		go.GetComponent<Building>().Def.BuildingUnderConstruction.GetComponent<Constructable>().isDiggingRequired = false;
		KAnimGraphTileVisualizer kanimGraphTileVisualizer = go.AddComponent<KAnimGraphTileVisualizer>();
		kanimGraphTileVisualizer.connectionSource = KAnimGraphTileVisualizer.ConnectionSource.Tube;
		kanimGraphTileVisualizer.isPhysicalBuilding = true;
	}

	// Token: 0x04001121 RID: 4385
	public const string ID = "TravelTube";
}
