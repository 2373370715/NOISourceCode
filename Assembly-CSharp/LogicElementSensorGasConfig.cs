﻿using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

public class LogicElementSensorGasConfig : IBuildingConfig
{
	public override BuildingDef CreateBuildingDef()
	{
		string id = LogicElementSensorGasConfig.ID;
		int width = 1;
		int height = 1;
		string anim = "world_element_sensor_kanim";
		int hitpoints = 30;
		float construction_time = 30f;
		float[] tier = TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER0;
		string[] refined_METALS = MATERIALS.REFINED_METALS;
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.Anywhere;
		EffectorValues none = NOISE_POLLUTION.NONE;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, refined_METALS, melting_point, build_location_rule, TUNING.BUILDINGS.DECOR.PENALTY.TIER0, none, 0.2f);
		buildingDef.Overheatable = false;
		buildingDef.Floodable = true;
		buildingDef.Entombable = true;
		buildingDef.ViewMode = OverlayModes.Logic.ID;
		buildingDef.AudioCategory = "Metal";
		buildingDef.SceneLayer = Grid.SceneLayer.Building;
		buildingDef.LogicOutputPorts = new List<LogicPorts.Port>
		{
			LogicPorts.Port.OutputPort(LogicSwitch.PORT_ID, new CellOffset(0, 0), STRINGS.BUILDINGS.PREFABS.LOGICELEMENTSENSORGAS.LOGIC_PORT, STRINGS.BUILDINGS.PREFABS.LOGICELEMENTSENSORGAS.LOGIC_PORT_ACTIVE, STRINGS.BUILDINGS.PREFABS.LOGICELEMENTSENSORGAS.LOGIC_PORT_INACTIVE, true, false)
		};
		SoundEventVolumeCache.instance.AddVolume("world_element_sensor_kanim", "PowerSwitch_on", NOISE_POLLUTION.NOISY.TIER3);
		SoundEventVolumeCache.instance.AddVolume("world_element_sensor_kanim", "PowerSwitch_off", NOISE_POLLUTION.NOISY.TIER3);
		GeneratedBuildings.RegisterWithOverlay(OverlayModes.Logic.HighlightItemIDs, LogicElementSensorGasConfig.ID);
		buildingDef.AddSearchTerms(SEARCH_TERMS.AUTOMATION);
		return buildingDef;
	}

	public override void DoPostConfigureComplete(GameObject go)
	{
		go.AddOrGet<Filterable>().filterElementState = Filterable.ElementState.Gas;
		LogicElementSensor logicElementSensor = go.AddOrGet<LogicElementSensor>();
		logicElementSensor.manuallyControlled = false;
		logicElementSensor.desiredState = Element.State.Gas;
	}

	public static string ID = "LogicElementSensorGas";
}
