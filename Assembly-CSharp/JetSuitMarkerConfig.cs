using System;
using TUNING;
using UnityEngine;

// Token: 0x020003A5 RID: 933
public class JetSuitMarkerConfig : IBuildingConfig
{
	// Token: 0x06000F16 RID: 3862 RVA: 0x00185750 File Offset: 0x00183950
	public override BuildingDef CreateBuildingDef()
	{
		string id = "JetSuitMarker";
		int width = 2;
		int height = 4;
		string anim = "changingarea_jetsuit_arrow_kanim";
		int hitpoints = 30;
		float construction_time = 30f;
		string[] refined_METALS = MATERIALS.REFINED_METALS;
		float[] construction_mass = new float[]
		{
			BUILDINGS.CONSTRUCTION_MASS_KG.TIER3[0]
		};
		string[] construction_materials = refined_METALS;
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
		EffectorValues none = NOISE_POLLUTION.NONE;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, construction_mass, construction_materials, melting_point, build_location_rule, BUILDINGS.DECOR.BONUS.TIER1, none, 0.2f);
		buildingDef.PermittedRotations = PermittedRotations.FlipH;
		buildingDef.PreventIdleTraversalPastBuilding = true;
		buildingDef.SceneLayer = Grid.SceneLayer.BuildingUse;
		buildingDef.ForegroundLayer = Grid.SceneLayer.TileMain;
		buildingDef.LogicInputPorts = LogicOperationalController.CreateSingleInputPortList(new CellOffset(0, 0));
		GeneratedBuildings.RegisterWithOverlay(OverlayScreen.SuitIDs, "JetSuitMarker");
		return buildingDef;
	}

	// Token: 0x06000F17 RID: 3863 RVA: 0x001857E4 File Offset: 0x001839E4
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		SuitMarker suitMarker = go.AddOrGet<SuitMarker>();
		suitMarker.LockerTags = new Tag[]
		{
			new Tag("JetSuitLocker")
		};
		suitMarker.PathFlag = PathFinder.PotentialPath.Flags.HasJetPack;
		suitMarker.interactAnim = Assets.GetAnim("anim_interacts_changingarea_jetsuit_arrow_kanim");
		go.AddOrGet<AnimTileable>().tags = new Tag[]
		{
			new Tag("JetSuitMarker"),
			new Tag("JetSuitLocker")
		};
		go.AddTag(GameTags.JetSuitBlocker);
	}

	// Token: 0x06000F18 RID: 3864 RVA: 0x000AAF59 File Offset: 0x000A9159
	public override void DoPostConfigureComplete(GameObject go)
	{
		go.AddOrGet<LogicOperationalController>();
	}

	// Token: 0x04000B16 RID: 2838
	public const string ID = "JetSuitMarker";
}
