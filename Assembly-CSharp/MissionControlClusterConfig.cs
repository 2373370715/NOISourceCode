using System;
using TUNING;
using UnityEngine;

// Token: 0x020004A8 RID: 1192
public class MissionControlClusterConfig : IBuildingConfig
{
	// Token: 0x0600146A RID: 5226 RVA: 0x000AA117 File Offset: 0x000A8317
	public override string[] GetRequiredDlcIds()
	{
		return DlcManager.EXPANSION1;
	}

	// Token: 0x0600146B RID: 5227 RVA: 0x0019BC70 File Offset: 0x00199E70
	public override BuildingDef CreateBuildingDef()
	{
		string id = "MissionControlCluster";
		int width = 3;
		int height = 3;
		string anim = "mission_control_station_kanim";
		int hitpoints = 100;
		float construction_time = 30f;
		float[] tier = BUILDINGS.CONSTRUCTION_MASS_KG.TIER4;
		string[] refined_METALS = MATERIALS.REFINED_METALS;
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
		EffectorValues none = NOISE_POLLUTION.NONE;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, refined_METALS, melting_point, build_location_rule, BUILDINGS.DECOR.PENALTY.TIER0, none, 0.2f);
		buildingDef.RequiresPowerInput = true;
		buildingDef.EnergyConsumptionWhenActive = 960f;
		buildingDef.ExhaustKilowattsWhenActive = 0.5f;
		buildingDef.SelfHeatKilowattsWhenActive = 2f;
		buildingDef.ViewMode = OverlayModes.Power.ID;
		buildingDef.AudioCategory = "Metal";
		buildingDef.AudioSize = "large";
		buildingDef.DefaultAnimState = "off";
		buildingDef.RequiredSkillPerkID = Db.Get().SkillPerks.CanMissionControl.Id;
		return buildingDef;
	}

	// Token: 0x0600146C RID: 5228 RVA: 0x0019BD24 File Offset: 0x00199F24
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.ScienceBuilding, false);
		BuildingDef def = go.GetComponent<BuildingComplete>().Def;
		Prioritizable.AddRef(go);
		go.AddOrGet<BuildingComplete>().isManuallyOperated = true;
		go.AddOrGetDef<PoweredController.Def>();
		go.AddOrGetDef<SkyVisibilityMonitor.Def>().skyVisibilityInfo = MissionControlClusterConfig.SKY_VISIBILITY_INFO;
		go.AddOrGetDef<MissionControlCluster.Def>();
		MissionControlClusterWorkable missionControlClusterWorkable = go.AddOrGet<MissionControlClusterWorkable>();
		missionControlClusterWorkable.requiredSkillPerk = Db.Get().SkillPerks.CanMissionControl.Id;
		missionControlClusterWorkable.workLayer = Grid.SceneLayer.BuildingUse;
	}

	// Token: 0x0600146D RID: 5229 RVA: 0x000B34B4 File Offset: 0x000B16B4
	public override void DoPostConfigureComplete(GameObject go)
	{
		RoomTracker roomTracker = go.AddOrGet<RoomTracker>();
		roomTracker.requiredRoomType = Db.Get().RoomTypes.Laboratory.Id;
		roomTracker.requirement = RoomTracker.Requirement.Required;
		MissionControlClusterConfig.AddVisualizer(go);
	}

	// Token: 0x0600146E RID: 5230 RVA: 0x000B34E2 File Offset: 0x000B16E2
	public override void DoPostConfigurePreview(BuildingDef def, GameObject go)
	{
		MissionControlClusterConfig.AddVisualizer(go);
	}

	// Token: 0x0600146F RID: 5231 RVA: 0x000B34EA File Offset: 0x000B16EA
	public override void DoPostConfigureUnderConstruction(GameObject go)
	{
		MissionControlClusterConfig.AddVisualizer(go);
	}

	// Token: 0x06001470 RID: 5232 RVA: 0x000B34F2 File Offset: 0x000B16F2
	private static void AddVisualizer(GameObject prefab)
	{
		SkyVisibilityVisualizer skyVisibilityVisualizer = prefab.AddOrGet<SkyVisibilityVisualizer>();
		skyVisibilityVisualizer.OriginOffset.y = 2;
		skyVisibilityVisualizer.RangeMin = -1;
		skyVisibilityVisualizer.RangeMax = 1;
		skyVisibilityVisualizer.SkipOnModuleInteriors = true;
	}

	// Token: 0x04000DE7 RID: 3559
	public const string ID = "MissionControlCluster";

	// Token: 0x04000DE8 RID: 3560
	public const int WORK_RANGE_RADIUS = 2;

	// Token: 0x04000DE9 RID: 3561
	public const float EFFECT_DURATION = 600f;

	// Token: 0x04000DEA RID: 3562
	public const float SPEED_MULTIPLIER = 1.2f;

	// Token: 0x04000DEB RID: 3563
	public const int SCAN_RADIUS = 1;

	// Token: 0x04000DEC RID: 3564
	public const int VERTICAL_SCAN_OFFSET = 2;

	// Token: 0x04000DED RID: 3565
	public static readonly SkyVisibilityInfo SKY_VISIBILITY_INFO = new SkyVisibilityInfo(new CellOffset(0, 2), 1, new CellOffset(0, 2), 1, 0);
}
