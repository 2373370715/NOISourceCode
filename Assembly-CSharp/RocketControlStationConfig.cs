using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000575 RID: 1397
public class RocketControlStationConfig : IBuildingConfig
{
	// Token: 0x060017FF RID: 6143 RVA: 0x000AA117 File Offset: 0x000A8317
	public override string[] GetRequiredDlcIds()
	{
		return DlcManager.EXPANSION1;
	}

	// Token: 0x06001800 RID: 6144 RVA: 0x001A986C File Offset: 0x001A7A6C
	public override BuildingDef CreateBuildingDef()
	{
		string id = RocketControlStationConfig.ID;
		int width = 2;
		int height = 2;
		string anim = "rocket_control_station_kanim";
		int hitpoints = 30;
		float construction_time = 60f;
		float[] tier = TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER2;
		string[] raw_METALS = MATERIALS.RAW_METALS;
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER3;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, raw_METALS, melting_point, build_location_rule, TUNING.BUILDINGS.DECOR.BONUS.TIER2, tier2, 0.2f);
		buildingDef.Overheatable = false;
		buildingDef.Repairable = false;
		buildingDef.Floodable = false;
		buildingDef.AudioCategory = "Metal";
		buildingDef.AudioSize = "large";
		buildingDef.DefaultAnimState = "off";
		buildingDef.OnePerWorld = true;
		buildingDef.RequiredSkillPerkID = Db.Get().SkillPerks.CanUseRocketControlStation.Id;
		buildingDef.LogicInputPorts = new List<LogicPorts.Port>
		{
			LogicPorts.Port.InputPort(RocketControlStation.PORT_ID, new CellOffset(0, 0), STRINGS.BUILDINGS.PREFABS.ROCKETCONTROLSTATION.LOGIC_PORT, STRINGS.BUILDINGS.PREFABS.ROCKETCONTROLSTATION.LOGIC_PORT_ACTIVE, STRINGS.BUILDINGS.PREFABS.ROCKETCONTROLSTATION.LOGIC_PORT_INACTIVE, false, false)
		};
		return buildingDef;
	}

	// Token: 0x06001801 RID: 6145 RVA: 0x000B49A5 File Offset: 0x000B2BA5
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		KPrefabID component = go.GetComponent<KPrefabID>();
		component.AddTag(GameTags.RocketInteriorBuilding, false);
		component.AddTag(GameTags.UniquePerWorld, false);
	}

	// Token: 0x06001802 RID: 6146 RVA: 0x001A994C File Offset: 0x001A7B4C
	public override void DoPostConfigureComplete(GameObject go)
	{
		go.AddOrGet<BuildingComplete>().isManuallyOperated = true;
		go.AddOrGet<RocketControlStationIdleWorkable>().workLayer = Grid.SceneLayer.BuildingUse;
		go.AddOrGet<RocketControlStationLaunchWorkable>().workLayer = Grid.SceneLayer.BuildingUse;
		go.AddOrGet<RocketControlStation>();
		go.AddOrGetDef<PoweredController.Def>();
		go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.RocketInterior, false);
	}

	// Token: 0x04000FE7 RID: 4071
	public static string ID = "RocketControlStation";

	// Token: 0x04000FE8 RID: 4072
	public const float CONSOLE_WORK_TIME = 30f;

	// Token: 0x04000FE9 RID: 4073
	public const float CONSOLE_IDLE_TIME = 120f;

	// Token: 0x04000FEA RID: 4074
	public const float WARNING_COOLDOWN = 30f;

	// Token: 0x04000FEB RID: 4075
	public const float DEFAULT_SPEED = 1f;

	// Token: 0x04000FEC RID: 4076
	public const float SLOW_SPEED = 0.5f;

	// Token: 0x04000FED RID: 4077
	public const float SUPER_SPEED = 1.5f;

	// Token: 0x04000FEE RID: 4078
	public const float DEFAULT_PILOT_MODIFIER = 1f;
}
