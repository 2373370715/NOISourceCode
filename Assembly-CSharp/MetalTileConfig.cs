using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x0200041E RID: 1054
public class MetalTileConfig : IBuildingConfig
{
	// Token: 0x06001184 RID: 4484 RVA: 0x0018F184 File Offset: 0x0018D384
	public override BuildingDef CreateBuildingDef()
	{
		string id = "MetalTile";
		int width = 1;
		int height = 1;
		string anim = "floor_metal_kanim";
		int hitpoints = 100;
		float construction_time = 30f;
		float[] tier = TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER2;
		string[] refined_METALS = MATERIALS.REFINED_METALS;
		float melting_point = 800f;
		BuildLocationRule build_location_rule = BuildLocationRule.Tile;
		EffectorValues none = NOISE_POLLUTION.NONE;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, refined_METALS, melting_point, build_location_rule, TUNING.BUILDINGS.DECOR.BONUS.TIER2, none, 0.2f);
		BuildingTemplates.CreateFoundationTileDef(buildingDef);
		buildingDef.Floodable = false;
		buildingDef.Entombable = false;
		buildingDef.Overheatable = false;
		buildingDef.UseStructureTemperature = false;
		buildingDef.AudioCategory = "Metal";
		buildingDef.AudioSize = "small";
		buildingDef.BaseTimeUntilRepair = -1f;
		buildingDef.SceneLayer = Grid.SceneLayer.TileMain;
		buildingDef.isKAnimTile = true;
		buildingDef.BlockTileMaterial = Assets.GetMaterial("tiles_solid");
		buildingDef.BlockTileAtlas = Assets.GetTextureAtlas("tiles_metal");
		buildingDef.BlockTilePlaceAtlas = Assets.GetTextureAtlas("tiles_metal_place");
		buildingDef.BlockTileShineAtlas = Assets.GetTextureAtlas("tiles_metal_spec");
		buildingDef.DecorBlockTileInfo = Assets.GetBlockTileDecorInfo("tiles_metal_tops_decor_info");
		buildingDef.DecorPlaceBlockTileInfo = Assets.GetBlockTileDecorInfo("tiles_metal_tops_decor_place_info");
		buildingDef.ConstructionOffsetFilter = BuildingDef.ConstructionOffsetFilter_OneDown;
		buildingDef.AddSearchTerms(SEARCH_TERMS.TILE);
		return buildingDef;
	}

	// Token: 0x06001185 RID: 4485 RVA: 0x0018F298 File Offset: 0x0018D498
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		GeneratedBuildings.MakeBuildingAlwaysOperational(go);
		BuildingConfigManager.Instance.IgnoreDefaultKComponent(typeof(RequiresFoundation), prefab_tag);
		SimCellOccupier simCellOccupier = go.AddOrGet<SimCellOccupier>();
		simCellOccupier.movementSpeedMultiplier = DUPLICANTSTATS.MOVEMENT_MODIFIERS.BONUS_3;
		simCellOccupier.notifyOnMelt = true;
		go.AddOrGet<TileTemperature>();
		go.AddOrGet<KAnimGridTileVisualizer>().blockTileConnectorID = MetalTileConfig.BlockTileConnectorID;
		go.AddOrGet<BuildingHP>().destroyOnDamaged = true;
	}

	// Token: 0x06001186 RID: 4486 RVA: 0x000B0779 File Offset: 0x000AE979
	public override void DoPostConfigureComplete(GameObject go)
	{
		GeneratedBuildings.RemoveLoopingSounds(go);
		go.GetComponent<KPrefabID>().AddTag(GameTags.FloorTiles, false);
	}

	// Token: 0x06001187 RID: 4487 RVA: 0x000AA509 File Offset: 0x000A8709
	public override void DoPostConfigureUnderConstruction(GameObject go)
	{
		base.DoPostConfigureUnderConstruction(go);
		go.AddOrGet<KAnimGridTileVisualizer>();
	}

	// Token: 0x04000C38 RID: 3128
	public const string ID = "MetalTile";

	// Token: 0x04000C39 RID: 3129
	public static readonly int BlockTileConnectorID = Hash.SDBMLower("tiles_metal_tops");
}
