using System;
using TUNING;
using UnityEngine;

// Token: 0x020004A9 RID: 1193
public class MissionControlConfig : IBuildingConfig
{
	// Token: 0x06001473 RID: 5235 RVA: 0x000AA117 File Offset: 0x000A8317
	public override string[] GetForbiddenDlcIds()
	{
		return DlcManager.EXPANSION1;
	}

	// Token: 0x06001474 RID: 5236 RVA: 0x0019BDA8 File Offset: 0x00199FA8
	public override BuildingDef CreateBuildingDef()
	{
		string id = "MissionControl";
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

	// Token: 0x06001475 RID: 5237 RVA: 0x0019BE5C File Offset: 0x0019A05C
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.ScienceBuilding, false);
		BuildingDef def = go.GetComponent<BuildingComplete>().Def;
		Prioritizable.AddRef(go);
		go.AddOrGet<BuildingComplete>().isManuallyOperated = true;
		go.AddOrGetDef<PoweredController.Def>();
		go.AddOrGetDef<SkyVisibilityMonitor.Def>().skyVisibilityInfo = MissionControlConfig.SKY_VISIBILITY_INFO;
		go.AddOrGetDef<MissionControl.Def>();
		MissionControlWorkable missionControlWorkable = go.AddOrGet<MissionControlWorkable>();
		missionControlWorkable.requiredSkillPerk = Db.Get().SkillPerks.CanMissionControl.Id;
		missionControlWorkable.workLayer = Grid.SceneLayer.BuildingUse;
	}

	// Token: 0x06001476 RID: 5238 RVA: 0x000B3537 File Offset: 0x000B1737
	public override void DoPostConfigureComplete(GameObject go)
	{
		RoomTracker roomTracker = go.AddOrGet<RoomTracker>();
		roomTracker.requiredRoomType = Db.Get().RoomTypes.Laboratory.Id;
		roomTracker.requirement = RoomTracker.Requirement.Required;
		MissionControlConfig.AddVisualizer(go);
	}

	// Token: 0x06001477 RID: 5239 RVA: 0x000B3565 File Offset: 0x000B1765
	public override void DoPostConfigurePreview(BuildingDef def, GameObject go)
	{
		MissionControlConfig.AddVisualizer(go);
	}

	// Token: 0x06001478 RID: 5240 RVA: 0x000B356D File Offset: 0x000B176D
	public override void DoPostConfigureUnderConstruction(GameObject go)
	{
		MissionControlConfig.AddVisualizer(go);
	}

	// Token: 0x06001479 RID: 5241 RVA: 0x000B34F2 File Offset: 0x000B16F2
	private static void AddVisualizer(GameObject prefab)
	{
		SkyVisibilityVisualizer skyVisibilityVisualizer = prefab.AddOrGet<SkyVisibilityVisualizer>();
		skyVisibilityVisualizer.OriginOffset.y = 2;
		skyVisibilityVisualizer.RangeMin = -1;
		skyVisibilityVisualizer.RangeMax = 1;
		skyVisibilityVisualizer.SkipOnModuleInteriors = true;
	}

	// Token: 0x04000DEE RID: 3566
	public const string ID = "MissionControl";

	// Token: 0x04000DEF RID: 3567
	public const float EFFECT_DURATION = 600f;

	// Token: 0x04000DF0 RID: 3568
	public const float SPEED_MULTIPLIER = 1.2f;

	// Token: 0x04000DF1 RID: 3569
	public const int SCAN_RADIUS = 1;

	// Token: 0x04000DF2 RID: 3570
	public const int VERTICAL_SCAN_OFFSET = 2;

	// Token: 0x04000DF3 RID: 3571
	public static readonly SkyVisibilityInfo SKY_VISIBILITY_INFO = new SkyVisibilityInfo(new CellOffset(0, 2), 1, new CellOffset(0, 2), 1, 0);
}
