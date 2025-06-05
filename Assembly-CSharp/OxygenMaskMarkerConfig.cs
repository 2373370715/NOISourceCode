using System;
using TUNING;
using UnityEngine;

// Token: 0x020004F1 RID: 1265
public class OxygenMaskMarkerConfig : IBuildingConfig
{
	// Token: 0x060015C1 RID: 5569 RVA: 0x001A0560 File Offset: 0x0019E760
	public override BuildingDef CreateBuildingDef()
	{
		string id = "OxygenMaskMarker";
		int width = 1;
		int height = 2;
		string anim = "oxygen_checkpoint_arrow_kanim";
		int hitpoints = 30;
		float construction_time = 30f;
		string[] raw_METALS = MATERIALS.RAW_METALS;
		float[] tier = BUILDINGS.CONSTRUCTION_MASS_KG.TIER2;
		string[] construction_materials = raw_METALS;
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
		EffectorValues none = NOISE_POLLUTION.NONE;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, construction_materials, melting_point, build_location_rule, BUILDINGS.DECOR.BONUS.TIER1, none, 0.2f);
		buildingDef.PermittedRotations = PermittedRotations.FlipH;
		buildingDef.PreventIdleTraversalPastBuilding = true;
		buildingDef.LogicInputPorts = LogicOperationalController.CreateSingleInputPortList(new CellOffset(0, 0));
		GeneratedBuildings.RegisterWithOverlay(OverlayScreen.SuitIDs, "OxygenMaskMarker");
		return buildingDef;
	}

	// Token: 0x060015C2 RID: 5570 RVA: 0x001A05D8 File Offset: 0x0019E7D8
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		SuitMarker suitMarker = go.AddOrGet<SuitMarker>();
		suitMarker.LockerTags = new Tag[]
		{
			new Tag("OxygenMaskLocker")
		};
		suitMarker.PathFlag = PathFinder.PotentialPath.Flags.HasOxygenMask;
		go.AddOrGet<AnimTileable>().tags = new Tag[]
		{
			new Tag("OxygenMaskMarker"),
			new Tag("OxygenMaskLocker")
		};
		go.AddTag(GameTags.JetSuitBlocker);
	}

	// Token: 0x060015C3 RID: 5571 RVA: 0x000AAF59 File Offset: 0x000A9159
	public override void DoPostConfigureComplete(GameObject go)
	{
		go.AddOrGet<LogicOperationalController>();
	}

	// Token: 0x04000EF6 RID: 3830
	public const string ID = "OxygenMaskMarker";
}
