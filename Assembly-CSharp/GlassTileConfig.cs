using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x0200036E RID: 878
public class GlassTileConfig : IBuildingConfig
{
	// Token: 0x06000DED RID: 3565 RVA: 0x0018082C File Offset: 0x0017EA2C
	public override BuildingDef CreateBuildingDef()
	{
		string id = "GlassTile";
		int width = 1;
		int height = 1;
		string anim = "floor_glass_kanim";
		int hitpoints = 100;
		float construction_time = 30f;
		float[] tier = TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER2;
		string[] transparents = MATERIALS.TRANSPARENTS;
		float melting_point = 800f;
		BuildLocationRule build_location_rule = BuildLocationRule.Tile;
		EffectorValues none = NOISE_POLLUTION.NONE;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, transparents, melting_point, build_location_rule, TUNING.BUILDINGS.DECOR.BONUS.TIER0, none, 0.2f);
		BuildingTemplates.CreateFoundationTileDef(buildingDef);
		buildingDef.Floodable = false;
		buildingDef.Entombable = false;
		buildingDef.Overheatable = false;
		buildingDef.UseStructureTemperature = false;
		buildingDef.AudioCategory = "Glass";
		buildingDef.AudioSize = "small";
		buildingDef.BaseTimeUntilRepair = -1f;
		buildingDef.SceneLayer = Grid.SceneLayer.GlassTile;
		buildingDef.isKAnimTile = true;
		buildingDef.BlockTileIsTransparent = true;
		buildingDef.BlockTileAtlas = Assets.GetTextureAtlas("tiles_glass");
		buildingDef.BlockTilePlaceAtlas = Assets.GetTextureAtlas("tiles_glass_place");
		buildingDef.BlockTileMaterial = Assets.GetMaterial("tiles_solid");
		buildingDef.DecorBlockTileInfo = Assets.GetBlockTileDecorInfo("tiles_glass_tops_decor_info");
		buildingDef.DecorPlaceBlockTileInfo = Assets.GetBlockTileDecorInfo("tiles_glass_tops_decor_place_info");
		buildingDef.ConstructionOffsetFilter = BuildingDef.ConstructionOffsetFilter_OneDown;
		buildingDef.AddSearchTerms(SEARCH_TERMS.TILE);
		buildingDef.AddSearchTerms(SEARCH_TERMS.GLASS);
		return buildingDef;
	}

	// Token: 0x06000DEE RID: 3566 RVA: 0x00180948 File Offset: 0x0017EB48
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		GeneratedBuildings.MakeBuildingAlwaysOperational(go);
		BuildingConfigManager.Instance.IgnoreDefaultKComponent(typeof(RequiresFoundation), prefab_tag);
		SimCellOccupier simCellOccupier = go.AddOrGet<SimCellOccupier>();
		simCellOccupier.setTransparent = true;
		simCellOccupier.notifyOnMelt = true;
		go.AddOrGet<TileTemperature>();
		go.AddOrGet<KAnimGridTileVisualizer>().blockTileConnectorID = GlassTileConfig.BlockTileConnectorID;
		go.AddOrGet<BuildingHP>().destroyOnDamaged = true;
		go.GetComponent<KPrefabID>().AddTag(GameTags.Window, false);
	}

	// Token: 0x06000DEF RID: 3567 RVA: 0x000B0779 File Offset: 0x000AE979
	public override void DoPostConfigureComplete(GameObject go)
	{
		GeneratedBuildings.RemoveLoopingSounds(go);
		go.GetComponent<KPrefabID>().AddTag(GameTags.FloorTiles, false);
	}

	// Token: 0x06000DF0 RID: 3568 RVA: 0x000AA509 File Offset: 0x000A8709
	public override void DoPostConfigureUnderConstruction(GameObject go)
	{
		base.DoPostConfigureUnderConstruction(go);
		go.AddOrGet<KAnimGridTileVisualizer>();
	}

	// Token: 0x04000A5C RID: 2652
	public const string ID = "GlassTile";

	// Token: 0x04000A5D RID: 2653
	public static readonly int BlockTileConnectorID = Hash.SDBMLower("tiles_glass_tops");
}
