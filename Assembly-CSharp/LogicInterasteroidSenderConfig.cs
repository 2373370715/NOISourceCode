using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020003EA RID: 1002
public class LogicInterasteroidSenderConfig : IBuildingConfig
{
	// Token: 0x06001071 RID: 4209 RVA: 0x000AA117 File Offset: 0x000A8317
	public override string[] GetRequiredDlcIds()
	{
		return DlcManager.EXPANSION1;
	}

	// Token: 0x06001072 RID: 4210 RVA: 0x0018A7A0 File Offset: 0x001889A0
	public override BuildingDef CreateBuildingDef()
	{
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef("LogicInterasteroidSender", 1, 1, "inter_asteroid_automation_signal_sender_kanim", 30, 30f, TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER2, MATERIALS.REFINED_METALS, 1600f, BuildLocationRule.OnFloor, TUNING.BUILDINGS.DECOR.PENALTY.TIER0, NOISE_POLLUTION.NOISY.TIER0, 0.2f);
		buildingDef.Floodable = false;
		buildingDef.Overheatable = false;
		buildingDef.Entombable = true;
		buildingDef.AudioCategory = "Metal";
		buildingDef.ViewMode = OverlayModes.Logic.ID;
		buildingDef.SceneLayer = Grid.SceneLayer.Building;
		buildingDef.PermittedRotations = PermittedRotations.Unrotatable;
		buildingDef.AlwaysOperational = false;
		buildingDef.LogicInputPorts = new List<LogicPorts.Port>
		{
			LogicPorts.Port.InputPort("InputPort", new CellOffset(0, 0), STRINGS.BUILDINGS.PREFABS.LOGICDUPLICANTSENSOR.LOGIC_PORT, STRINGS.BUILDINGS.PREFABS.LOGICINTERASTEROIDSENDER.LOGIC_PORT_ACTIVE, STRINGS.BUILDINGS.PREFABS.LOGICINTERASTEROIDSENDER.LOGIC_PORT_INACTIVE, true, false)
		};
		GeneratedBuildings.RegisterWithOverlay(OverlayModes.Logic.HighlightItemIDs, "LogicInterasteroidSender");
		buildingDef.AddSearchTerms(SEARCH_TERMS.AUTOMATION);
		return buildingDef;
	}

	// Token: 0x06001073 RID: 4211 RVA: 0x000B1998 File Offset: 0x000AFB98
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		base.ConfigureBuildingTemplate(go, prefab_tag);
		go.AddOrGet<UserNameable>().savedName = STRINGS.BUILDINGS.PREFABS.LOGICINTERASTEROIDSENDER.DEFAULTNAME;
		go.AddOrGet<LogicBroadcaster>().PORT_ID = "InputPort";
	}

	// Token: 0x06001074 RID: 4212 RVA: 0x000B19C7 File Offset: 0x000AFBC7
	public override void DoPostConfigurePreview(BuildingDef def, GameObject go)
	{
		LogicInterasteroidSenderConfig.AddVisualizer(go);
	}

	// Token: 0x06001075 RID: 4213 RVA: 0x000B19CF File Offset: 0x000AFBCF
	public override void DoPostConfigureComplete(GameObject go)
	{
		LogicInterasteroidSenderConfig.AddVisualizer(go);
	}

	// Token: 0x06001076 RID: 4214 RVA: 0x000B19CF File Offset: 0x000AFBCF
	public override void DoPostConfigureUnderConstruction(GameObject go)
	{
		LogicInterasteroidSenderConfig.AddVisualizer(go);
	}

	// Token: 0x06001077 RID: 4215 RVA: 0x000B197C File Offset: 0x000AFB7C
	private static void AddVisualizer(GameObject prefab)
	{
		SkyVisibilityVisualizer skyVisibilityVisualizer = prefab.AddOrGet<SkyVisibilityVisualizer>();
		skyVisibilityVisualizer.RangeMin = 0;
		skyVisibilityVisualizer.RangeMax = 0;
		skyVisibilityVisualizer.SkipOnModuleInteriors = true;
	}

	// Token: 0x04000B93 RID: 2963
	public const string ID = "LogicInterasteroidSender";

	// Token: 0x04000B94 RID: 2964
	public const string INPUT_PORT_ID = "InputPort";
}
