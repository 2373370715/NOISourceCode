using System;
using TUNING;
using UnityEngine;

// Token: 0x020004F8 RID: 1272
public class POIDoorInternalConfig : IBuildingConfig
{
	// Token: 0x060015DD RID: 5597 RVA: 0x001A0E78 File Offset: 0x0019F078
	public override BuildingDef CreateBuildingDef()
	{
		string id = POIDoorInternalConfig.ID;
		int width = 1;
		int height = 2;
		string anim = "door_poi_internal_kanim";
		int hitpoints = 30;
		float construction_time = 10f;
		float[] tier = BUILDINGS.CONSTRUCTION_MASS_KG.TIER2;
		string[] all_METALS = MATERIALS.ALL_METALS;
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.Tile;
		EffectorValues none = NOISE_POLLUTION.NONE;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, all_METALS, melting_point, build_location_rule, BUILDINGS.DECOR.NONE, none, 1f);
		buildingDef.ShowInBuildMenu = false;
		buildingDef.Entombable = false;
		buildingDef.Floodable = false;
		buildingDef.Invincible = true;
		buildingDef.IsFoundation = true;
		buildingDef.AudioCategory = "Metal";
		buildingDef.PermittedRotations = PermittedRotations.R90;
		buildingDef.SceneLayer = Grid.SceneLayer.TileMain;
		buildingDef.ForegroundLayer = Grid.SceneLayer.InteriorWall;
		buildingDef.LogicInputPorts = DoorConfig.CreateSingleInputPortList(new CellOffset(0, 0));
		SoundEventVolumeCache.instance.AddVolume("door_poi_internal_kanim", "Open_DoorInternal", NOISE_POLLUTION.NOISY.TIER2);
		SoundEventVolumeCache.instance.AddVolume("door_poi_internal_kanim", "Close_DoorInternal", NOISE_POLLUTION.NOISY.TIER2);
		return buildingDef;
	}

	// Token: 0x060015DE RID: 5598 RVA: 0x001A0F48 File Offset: 0x0019F148
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		Door door = go.AddOrGet<Door>();
		door.unpoweredAnimSpeed = 1f;
		door.doorType = Door.DoorType.Internal;
		go.AddOrGet<ZoneTile>();
		go.AddOrGet<AccessControl>();
		go.AddOrGet<Workable>().workTime = 3f;
		go.AddOrGet<KBoxCollider2D>();
		Prioritizable.AddRef(go);
		UnityEngine.Object.DestroyImmediate(go.GetComponent<BuildingEnabledButton>());
	}

	// Token: 0x060015DF RID: 5599 RVA: 0x001A0FA4 File Offset: 0x0019F1A4
	public override void DoPostConfigureComplete(GameObject go)
	{
		go.AddTag(GameTags.Gravitas);
		AccessControl component = go.GetComponent<AccessControl>();
		go.GetComponent<Door>().hasComplexUserControls = false;
		component.controlEnabled = false;
		go.GetComponent<Deconstructable>().allowDeconstruction = true;
		go.GetComponent<KBatchedAnimController>().initialAnim = "closed";
	}

	// Token: 0x04000F0D RID: 3853
	public static string ID = "POIDoorInternal";
}
