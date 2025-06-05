using System;
using TUNING;
using UnityEngine;

// Token: 0x020004D6 RID: 1238
public class NoseconeBasicConfig : IBuildingConfig
{
	// Token: 0x0600154A RID: 5450 RVA: 0x000AA117 File Offset: 0x000A8317
	public override string[] GetRequiredDlcIds()
	{
		return DlcManager.EXPANSION1;
	}

	// Token: 0x0600154B RID: 5451 RVA: 0x0019E1A8 File Offset: 0x0019C3A8
	public override BuildingDef CreateBuildingDef()
	{
		string id = "NoseconeBasic";
		int width = 5;
		int height = 2;
		string anim = "rocket_nosecone_default_kanim";
		int hitpoints = 1000;
		float construction_time = 60f;
		float[] nose_CONE_TIER = BUILDINGS.ROCKETRY_MASS_KG.NOSE_CONE_TIER2;
		string[] construction_materials = new string[]
		{
			"RefinedMetal",
			"Insulator"
		};
		float melting_point = 9999f;
		BuildLocationRule build_location_rule = BuildLocationRule.Anywhere;
		EffectorValues tier = NOISE_POLLUTION.NOISY.TIER2;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, nose_CONE_TIER, construction_materials, melting_point, build_location_rule, BUILDINGS.DECOR.NONE, tier, 0.2f);
		BuildingTemplates.CreateRocketBuildingDef(buildingDef);
		buildingDef.AttachmentSlotTag = GameTags.Rocket;
		buildingDef.SceneLayer = Grid.SceneLayer.Building;
		buildingDef.OverheatTemperature = 2273.15f;
		buildingDef.Floodable = false;
		buildingDef.ObjectLayer = ObjectLayer.Building;
		buildingDef.ForegroundLayer = Grid.SceneLayer.Front;
		buildingDef.RequiresPowerInput = false;
		buildingDef.attachablePosition = new CellOffset(0, 0);
		buildingDef.CanMove = true;
		buildingDef.Cancellable = false;
		buildingDef.ShowInBuildMenu = false;
		return buildingDef;
	}

	// Token: 0x0600154C RID: 5452 RVA: 0x000B3F21 File Offset: 0x000B2121
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		BuildingConfigManager.Instance.IgnoreDefaultKComponent(typeof(RequiresFoundation), prefab_tag);
		go.AddOrGet<LoopingSounds>();
		go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.IndustrialMachinery, false);
		go.GetComponent<KPrefabID>().AddTag(GameTags.NoseRocketModule, false);
	}

	// Token: 0x0600154D RID: 5453 RVA: 0x000B3F61 File Offset: 0x000B2161
	public override void DoPostConfigureComplete(GameObject go)
	{
		BuildingTemplates.ExtendBuildingToRocketModuleCluster(go, null, ROCKETRY.BURDEN.MINOR, 0f, 0f);
		go.GetComponent<ReorderableBuilding>().buildConditions.Add(new TopOnly());
	}

	// Token: 0x04000EA6 RID: 3750
	public const string ID = "NoseconeBasic";
}
