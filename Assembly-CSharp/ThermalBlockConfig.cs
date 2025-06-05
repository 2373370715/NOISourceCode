using System;
using System.Collections.Generic;
using TUNING;
using UnityEngine;

// Token: 0x020005E5 RID: 1509
public class ThermalBlockConfig : IBuildingConfig
{
	// Token: 0x06001A6C RID: 6764 RVA: 0x001B3C68 File Offset: 0x001B1E68
	public override BuildingDef CreateBuildingDef()
	{
		string id = "ThermalBlock";
		int width = 1;
		int height = 1;
		string anim = "thermalblock_kanim";
		int hitpoints = 30;
		float construction_time = 120f;
		float[] tier = BUILDINGS.CONSTRUCTION_MASS_KG.TIER5;
		string[] any_BUILDABLE = MATERIALS.ANY_BUILDABLE;
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.NotInTiles;
		EffectorValues none = NOISE_POLLUTION.NONE;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, any_BUILDABLE, melting_point, build_location_rule, DECOR.NONE, none, 0.2f);
		buildingDef.Floodable = false;
		buildingDef.Entombable = false;
		buildingDef.Overheatable = false;
		buildingDef.AudioCategory = "Metal";
		buildingDef.BaseTimeUntilRepair = -1f;
		buildingDef.ViewMode = OverlayModes.Temperature.ID;
		buildingDef.DefaultAnimState = "off";
		buildingDef.ObjectLayer = ObjectLayer.Backwall;
		buildingDef.SceneLayer = Grid.SceneLayer.Backwall;
		buildingDef.ReplacementLayer = ObjectLayer.ReplacementBackwall;
		buildingDef.ReplacementCandidateLayers = new List<ObjectLayer>
		{
			ObjectLayer.FoundationTile,
			ObjectLayer.Backwall
		};
		buildingDef.ReplacementTags = new List<Tag>
		{
			GameTags.FloorTiles,
			GameTags.Backwall
		};
		return buildingDef;
	}

	// Token: 0x06001A6D RID: 6765 RVA: 0x000B59BE File Offset: 0x000B3BBE
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.AddOrGet<AnimTileable>().objectLayer = ObjectLayer.Backwall;
		go.AddComponent<ZoneTile>();
		BuildingConfigManager.Instance.IgnoreDefaultKComponent(typeof(RequiresFoundation), prefab_tag);
	}

	// Token: 0x06001A6E RID: 6766 RVA: 0x000B59E8 File Offset: 0x000B3BE8
	public override void DoPostConfigureComplete(GameObject go)
	{
		KPrefabID component = go.GetComponent<KPrefabID>();
		component.AddTag(GameTags.Backwall, false);
		component.prefabSpawnFn += delegate(GameObject game_object)
		{
			HandleVector<int>.Handle handle = GameComps.StructureTemperatures.GetHandle(game_object);
			StructureTemperaturePayload payload = GameComps.StructureTemperatures.GetPayload(handle);
			int cell = Grid.PosToCell(game_object);
			payload.OverrideExtents(new Extents(cell, ThermalBlockConfig.overrideOffsets, Extents.BoundsCheckCoords));
			GameComps.StructureTemperatures.SetPayload(handle, ref payload);
		};
	}

	// Token: 0x04001119 RID: 4377
	public const string ID = "ThermalBlock";

	// Token: 0x0400111A RID: 4378
	private static readonly CellOffset[] overrideOffsets = new CellOffset[]
	{
		new CellOffset(-1, -1),
		new CellOffset(1, -1),
		new CellOffset(-1, 1),
		new CellOffset(1, 1)
	};
}
