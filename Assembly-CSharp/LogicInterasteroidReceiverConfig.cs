using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020003E9 RID: 1001
public class LogicInterasteroidReceiverConfig : IBuildingConfig
{
	// Token: 0x0600106A RID: 4202 RVA: 0x000AA117 File Offset: 0x000A8317
	public override string[] GetRequiredDlcIds()
	{
		return DlcManager.EXPANSION1;
	}

	// Token: 0x0600106B RID: 4203 RVA: 0x0018A6B4 File Offset: 0x001888B4
	public override BuildingDef CreateBuildingDef()
	{
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef("LogicInterasteroidReceiver", 1, 1, "inter_asteroid_automation_signal_receiver_kanim", 30, 30f, TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER2, MATERIALS.REFINED_METALS, 1600f, BuildLocationRule.OnFloor, TUNING.BUILDINGS.DECOR.PENALTY.TIER0, NOISE_POLLUTION.NOISY.TIER0, 0.2f);
		buildingDef.Floodable = false;
		buildingDef.Overheatable = false;
		buildingDef.Entombable = true;
		buildingDef.AudioCategory = "Metal";
		buildingDef.ViewMode = OverlayModes.Logic.ID;
		buildingDef.SceneLayer = Grid.SceneLayer.Building;
		buildingDef.PermittedRotations = PermittedRotations.Unrotatable;
		buildingDef.AlwaysOperational = false;
		buildingDef.LogicOutputPorts = new List<LogicPorts.Port>
		{
			LogicPorts.Port.OutputPort("OutputPort", new CellOffset(0, 0), STRINGS.BUILDINGS.PREFABS.LOGICINTERASTEROIDRECEIVER.LOGIC_PORT, STRINGS.BUILDINGS.PREFABS.LOGICINTERASTEROIDRECEIVER.LOGIC_PORT_ACTIVE, STRINGS.BUILDINGS.PREFABS.LOGICINTERASTEROIDRECEIVER.LOGIC_PORT_INACTIVE, true, false)
		};
		GeneratedBuildings.RegisterWithOverlay(OverlayModes.Logic.HighlightItemIDs, "LogicInterasteroidReceiver");
		buildingDef.AddSearchTerms(SEARCH_TERMS.AUTOMATION);
		return buildingDef;
	}

	// Token: 0x0600106C RID: 4204 RVA: 0x000B1954 File Offset: 0x000AFB54
	public override void DoPostConfigurePreview(BuildingDef def, GameObject go)
	{
		LogicInterasteroidReceiverConfig.AddVisualizer(go);
	}

	// Token: 0x0600106D RID: 4205 RVA: 0x000B195C File Offset: 0x000AFB5C
	public override void DoPostConfigureComplete(GameObject go)
	{
		go.AddOrGet<LogicBroadcastReceiver>().PORT_ID = "OutputPort";
		LogicInterasteroidReceiverConfig.AddVisualizer(go);
	}

	// Token: 0x0600106E RID: 4206 RVA: 0x000B1974 File Offset: 0x000AFB74
	public override void DoPostConfigureUnderConstruction(GameObject go)
	{
		LogicInterasteroidReceiverConfig.AddVisualizer(go);
	}

	// Token: 0x0600106F RID: 4207 RVA: 0x000B197C File Offset: 0x000AFB7C
	private static void AddVisualizer(GameObject prefab)
	{
		SkyVisibilityVisualizer skyVisibilityVisualizer = prefab.AddOrGet<SkyVisibilityVisualizer>();
		skyVisibilityVisualizer.RangeMin = 0;
		skyVisibilityVisualizer.RangeMax = 0;
		skyVisibilityVisualizer.SkipOnModuleInteriors = true;
	}

	// Token: 0x04000B91 RID: 2961
	public const string ID = "LogicInterasteroidReceiver";

	// Token: 0x04000B92 RID: 2962
	public const string OUTPUT_PORT_ID = "OutputPort";
}
