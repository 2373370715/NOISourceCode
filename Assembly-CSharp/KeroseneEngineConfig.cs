using System;
using TUNING;
using UnityEngine;

// Token: 0x020003AC RID: 940
public class KeroseneEngineConfig : IBuildingConfig
{
	// Token: 0x06000F32 RID: 3890 RVA: 0x000AA117 File Offset: 0x000A8317
	public override string[] GetForbiddenDlcIds()
	{
		return DlcManager.EXPANSION1;
	}

	// Token: 0x06000F33 RID: 3891 RVA: 0x00186004 File Offset: 0x00184204
	public override BuildingDef CreateBuildingDef()
	{
		string id = "KeroseneEngine";
		int width = 7;
		int height = 5;
		string anim = "rocket_petroleum_engine_kanim";
		int hitpoints = 1000;
		float construction_time = 60f;
		float[] engine_MASS_SMALL = BUILDINGS.ROCKETRY_MASS_KG.ENGINE_MASS_SMALL;
		string[] construction_materials = new string[]
		{
			SimHashes.Steel.ToString()
		};
		float melting_point = 9999f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
		EffectorValues tier = NOISE_POLLUTION.NOISY.TIER2;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, engine_MASS_SMALL, construction_materials, melting_point, build_location_rule, BUILDINGS.DECOR.NONE, tier, 0.2f);
		BuildingTemplates.CreateRocketBuildingDef(buildingDef);
		buildingDef.SceneLayer = Grid.SceneLayer.BuildingFront;
		buildingDef.OverheatTemperature = 2273.15f;
		buildingDef.Floodable = false;
		buildingDef.AttachmentSlotTag = GameTags.Rocket;
		buildingDef.ObjectLayer = ObjectLayer.Building;
		buildingDef.attachablePosition = new CellOffset(0, 0);
		buildingDef.RequiresPowerInput = false;
		buildingDef.CanMove = true;
		return buildingDef;
	}

	// Token: 0x06000F34 RID: 3892 RVA: 0x0017D1D0 File Offset: 0x0017B3D0
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		BuildingConfigManager.Instance.IgnoreDefaultKComponent(typeof(RequiresFoundation), prefab_tag);
		go.AddOrGet<LoopingSounds>();
		go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.IndustrialMachinery, false);
		go.AddOrGet<BuildingAttachPoint>().points = new BuildingAttachPoint.HardPoint[]
		{
			new BuildingAttachPoint.HardPoint(new CellOffset(0, 5), GameTags.Rocket, null)
		};
	}

	// Token: 0x06000F35 RID: 3893 RVA: 0x000B0EE4 File Offset: 0x000AF0E4
	public override void DoPostConfigureComplete(GameObject go)
	{
		RocketEngine rocketEngine = go.AddOrGet<RocketEngine>();
		rocketEngine.fuelTag = ElementLoader.FindElementByHash(SimHashes.Petroleum).tag;
		rocketEngine.efficiency = ROCKETRY.ENGINE_EFFICIENCY.MEDIUM;
		rocketEngine.explosionEffectHash = SpawnFXHashes.MeteorImpactDust;
		BuildingTemplates.ExtendBuildingToRocketModule(go, "rocket_petroleum_engine_bg_kanim", false);
	}

	// Token: 0x04000B25 RID: 2853
	public const string ID = "KeroseneEngine";
}
