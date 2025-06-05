using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020003DB RID: 987
public class LogicDuplicantSensorConfig : IBuildingConfig
{
	// Token: 0x0600100C RID: 4108 RVA: 0x00189CC4 File Offset: 0x00187EC4
	public override BuildingDef CreateBuildingDef()
	{
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef("LogicDuplicantSensor", 1, 1, "presence_sensor_kanim", 30, 30f, TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER0, MATERIALS.REFINED_METALS, 1600f, BuildLocationRule.OnFoundationRotatable, TUNING.BUILDINGS.DECOR.PENALTY.TIER0, NOISE_POLLUTION.NOISY.TIER0, 0.2f);
		buildingDef.Floodable = false;
		buildingDef.Overheatable = false;
		buildingDef.Entombable = false;
		buildingDef.AudioCategory = "Metal";
		buildingDef.ViewMode = OverlayModes.Logic.ID;
		buildingDef.SceneLayer = Grid.SceneLayer.Building;
		buildingDef.PermittedRotations = PermittedRotations.R360;
		buildingDef.AlwaysOperational = true;
		buildingDef.LogicOutputPorts = new List<LogicPorts.Port>
		{
			LogicPorts.Port.OutputPort(LogicSwitch.PORT_ID, new CellOffset(0, 0), STRINGS.BUILDINGS.PREFABS.LOGICDUPLICANTSENSOR.LOGIC_PORT, STRINGS.BUILDINGS.PREFABS.LOGICDUPLICANTSENSOR.LOGIC_PORT_ACTIVE, STRINGS.BUILDINGS.PREFABS.LOGICDUPLICANTSENSOR.LOGIC_PORT_INACTIVE, true, false)
		};
		GeneratedBuildings.RegisterWithOverlay(OverlayModes.Logic.HighlightItemIDs, "LogicDuplicantSensor");
		buildingDef.AddSearchTerms(SEARCH_TERMS.AUTOMATION);
		return buildingDef;
	}

	// Token: 0x0600100D RID: 4109 RVA: 0x000B1580 File Offset: 0x000AF780
	public override void DoPostConfigurePreview(BuildingDef def, GameObject go)
	{
		LogicDuplicantSensorConfig.AddVisualizer(go, true);
	}

	// Token: 0x0600100E RID: 4110 RVA: 0x000B1589 File Offset: 0x000AF789
	public override void DoPostConfigureComplete(GameObject go)
	{
		LogicDuplicantSensor logicDuplicantSensor = go.AddOrGet<LogicDuplicantSensor>();
		logicDuplicantSensor.defaultState = false;
		logicDuplicantSensor.manuallyControlled = false;
		logicDuplicantSensor.pickupRange = 4;
		LogicDuplicantSensorConfig.AddVisualizer(go, false);
		go.GetComponent<KPrefabID>().AddTag(GameTags.OverlayInFrontOfConduits, false);
	}

	// Token: 0x0600100F RID: 4111 RVA: 0x00189DAC File Offset: 0x00187FAC
	private static void AddVisualizer(GameObject prefab, bool movable)
	{
		RangeVisualizer rangeVisualizer = prefab.AddOrGet<RangeVisualizer>();
		rangeVisualizer.OriginOffset = new Vector2I(0, 0);
		rangeVisualizer.RangeMin.x = -2;
		rangeVisualizer.RangeMin.y = 0;
		rangeVisualizer.RangeMax.x = 2;
		rangeVisualizer.RangeMax.y = 4;
		rangeVisualizer.BlockingTileVisible = true;
	}

	// Token: 0x04000B82 RID: 2946
	public const string ID = "LogicDuplicantSensor";

	// Token: 0x04000B83 RID: 2947
	private const int RANGE = 4;
}
