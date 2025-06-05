using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000608 RID: 1544
public class WoodTileConfig : IBuildingConfig
{
	// Token: 0x06001B48 RID: 6984 RVA: 0x001B6D70 File Offset: 0x001B4F70
	public override BuildingDef CreateBuildingDef()
	{
		string id = "WoodTile";
		int width = 1;
		int height = 1;
		string anim = "floor_wood_kanim";
		int hitpoints = 100;
		float construction_time = 3f;
		float[] tier = TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER3;
		string[] construction_materials = new string[]
		{
			SimHashes.WoodLog.ToString()
		};
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.Tile;
		EffectorValues none = NOISE_POLLUTION.NONE;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, construction_materials, melting_point, build_location_rule, TUNING.BUILDINGS.DECOR.BONUS.TIER2, none, 0.2f);
		BuildingTemplates.CreateFoundationTileDef(buildingDef);
		buildingDef.Floodable = false;
		buildingDef.Overheatable = false;
		buildingDef.Entombable = false;
		buildingDef.UseStructureTemperature = false;
		buildingDef.AudioCategory = "Metal";
		buildingDef.AudioSize = "small";
		buildingDef.BaseTimeUntilRepair = -1f;
		buildingDef.SceneLayer = Grid.SceneLayer.TileMain;
		buildingDef.isKAnimTile = true;
		buildingDef.BlockTileAtlas = Assets.GetTextureAtlas("tiles_wood");
		buildingDef.BlockTilePlaceAtlas = Assets.GetTextureAtlas("tiles_wood_place");
		buildingDef.BlockTileMaterial = Assets.GetMaterial("tiles_solid");
		buildingDef.DecorBlockTileInfo = Assets.GetBlockTileDecorInfo("tiles_wood_decor_info");
		buildingDef.DecorPlaceBlockTileInfo = Assets.GetBlockTileDecorInfo("tiles_wood_decor_place_info");
		buildingDef.POIUnlockable = true;
		buildingDef.AddSearchTerms(SEARCH_TERMS.TILE);
		buildingDef.AddSearchTerms(SEARCH_TERMS.LUMBER);
		buildingDef.AddSearchTerms(SEARCH_TERMS.MORALE);
		buildingDef.DragBuild = true;
		return buildingDef;
	}

	// Token: 0x06001B49 RID: 6985 RVA: 0x001B6EB0 File Offset: 0x001B50B0
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		GeneratedBuildings.MakeBuildingAlwaysOperational(go);
		BuildingConfigManager.Instance.IgnoreDefaultKComponent(typeof(RequiresFoundation), prefab_tag);
		SimCellOccupier simCellOccupier = go.AddOrGet<SimCellOccupier>();
		simCellOccupier.doReplaceElement = true;
		simCellOccupier.strengthMultiplier = 1.5f;
		simCellOccupier.movementSpeedMultiplier = DUPLICANTSTATS.MOVEMENT_MODIFIERS.BONUS_2;
		simCellOccupier.notifyOnMelt = true;
		go.AddOrGet<TileTemperature>();
		go.AddOrGet<KAnimGridTileVisualizer>().blockTileConnectorID = WoodTileConfig.BlockTileConnectorID;
		go.AddOrGet<BuildingHP>().destroyOnDamaged = true;
	}

	// Token: 0x06001B4A RID: 6986 RVA: 0x000AA536 File Offset: 0x000A8736
	public override string[] GetRequiredDlcIds()
	{
		return DlcManager.DLC2;
	}

	// Token: 0x06001B4B RID: 6987 RVA: 0x000B0779 File Offset: 0x000AE979
	public override void DoPostConfigureComplete(GameObject go)
	{
		GeneratedBuildings.RemoveLoopingSounds(go);
		go.GetComponent<KPrefabID>().AddTag(GameTags.FloorTiles, false);
	}

	// Token: 0x06001B4C RID: 6988 RVA: 0x000AA509 File Offset: 0x000A8709
	public override void DoPostConfigureUnderConstruction(GameObject go)
	{
		base.DoPostConfigureUnderConstruction(go);
		go.AddOrGet<KAnimGridTileVisualizer>();
	}

	// Token: 0x0400117A RID: 4474
	public const string ID = "WoodTile";

	// Token: 0x0400117B RID: 4475
	public static readonly int BlockTileConnectorID = Hash.SDBMLower("tiles_wood_tops");
}
