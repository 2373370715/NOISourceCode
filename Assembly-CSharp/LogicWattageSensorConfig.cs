using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020003F9 RID: 1017
public class LogicWattageSensorConfig : IBuildingConfig
{
	// Token: 0x060010B6 RID: 4278 RVA: 0x0018B948 File Offset: 0x00189B48
	public override BuildingDef CreateBuildingDef()
	{
		string id = LogicWattageSensorConfig.ID;
		int width = 1;
		int height = 1;
		string anim = LogicWattageSensorConfig.kanim;
		int hitpoints = 30;
		float construction_time = 30f;
		float[] tier = TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER0;
		string[] refined_METALS = MATERIALS.REFINED_METALS;
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.Anywhere;
		EffectorValues none = NOISE_POLLUTION.NONE;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, refined_METALS, melting_point, build_location_rule, TUNING.BUILDINGS.DECOR.PENALTY.TIER0, none, 0.2f);
		buildingDef.Overheatable = false;
		buildingDef.Floodable = false;
		buildingDef.Entombable = false;
		buildingDef.ViewMode = OverlayModes.Logic.ID;
		buildingDef.AudioCategory = "Metal";
		buildingDef.SceneLayer = Grid.SceneLayer.Building;
		buildingDef.AlwaysOperational = true;
		buildingDef.LogicOutputPorts = new List<LogicPorts.Port>
		{
			LogicPorts.Port.OutputPort(LogicSwitch.PORT_ID, new CellOffset(0, 0), STRINGS.BUILDINGS.PREFABS.LOGICWATTAGESENSOR.LOGIC_PORT, STRINGS.BUILDINGS.PREFABS.LOGICWATTAGESENSOR.LOGIC_PORT_ACTIVE, STRINGS.BUILDINGS.PREFABS.LOGICWATTAGESENSOR.LOGIC_PORT_INACTIVE, true, false)
		};
		SoundEventVolumeCache.instance.AddVolume(LogicWattageSensorConfig.kanim, "PowerSwitch_on", NOISE_POLLUTION.NOISY.TIER3);
		SoundEventVolumeCache.instance.AddVolume(LogicWattageSensorConfig.kanim, "PowerSwitch_off", NOISE_POLLUTION.NOISY.TIER3);
		GeneratedBuildings.RegisterWithOverlay(OverlayModes.Logic.HighlightItemIDs, LogicWattageSensorConfig.ID);
		buildingDef.AddSearchTerms(SEARCH_TERMS.AUTOMATION);
		return buildingDef;
	}

	// Token: 0x060010B7 RID: 4279 RVA: 0x000B1C7F File Offset: 0x000AFE7F
	public override void DoPostConfigureComplete(GameObject go)
	{
		LogicWattageSensor logicWattageSensor = go.AddOrGet<LogicWattageSensor>();
		logicWattageSensor.manuallyControlled = false;
		logicWattageSensor.activateOnHigherThan = true;
		go.GetComponent<KPrefabID>().AddTag(GameTags.OverlayInFrontOfConduits, false);
	}

	// Token: 0x04000BA4 RID: 2980
	public static string ID = "LogicWattageSensor";

	// Token: 0x04000BA5 RID: 2981
	private static readonly string kanim = "wattage_sensor_kanim";
}
