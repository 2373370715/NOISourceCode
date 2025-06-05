using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020003E6 RID: 998
public abstract class LogicGateBaseConfig : IBuildingConfig
{
	// Token: 0x06001055 RID: 4181 RVA: 0x0018A324 File Offset: 0x00188524
	protected BuildingDef CreateBuildingDef(string ID, string anim, int width = 2, int height = 2)
	{
		int hitpoints = 10;
		float construction_time = 3f;
		float[] tier = TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER0;
		string[] refined_METALS = MATERIALS.REFINED_METALS;
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.Anywhere;
		EffectorValues none = NOISE_POLLUTION.NONE;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(ID, width, height, anim, hitpoints, construction_time, tier, refined_METALS, melting_point, build_location_rule, TUNING.BUILDINGS.DECOR.PENALTY.TIER0, none, 0.2f);
		buildingDef.ViewMode = OverlayModes.Logic.ID;
		buildingDef.ObjectLayer = ObjectLayer.LogicGate;
		buildingDef.SceneLayer = Grid.SceneLayer.LogicGates;
		buildingDef.ThermalConductivity = 0.05f;
		buildingDef.Floodable = false;
		buildingDef.Overheatable = false;
		buildingDef.Entombable = false;
		buildingDef.AudioCategory = "Metal";
		buildingDef.AudioSize = "small";
		buildingDef.BaseTimeUntilRepair = -1f;
		buildingDef.PermittedRotations = PermittedRotations.R360;
		buildingDef.DragBuild = true;
		buildingDef.AddSearchTerms(SEARCH_TERMS.AUTOMATION);
		LogicGateBase.uiSrcData = Assets.instance.logicModeUIData;
		GeneratedBuildings.RegisterWithOverlay(OverlayModes.Logic.HighlightItemIDs, ID);
		return buildingDef;
	}

	// Token: 0x1700005E RID: 94
	// (get) Token: 0x06001056 RID: 4182
	protected abstract CellOffset[] InputPortOffsets { get; }

	// Token: 0x1700005F RID: 95
	// (get) Token: 0x06001057 RID: 4183
	protected abstract CellOffset[] OutputPortOffsets { get; }

	// Token: 0x17000060 RID: 96
	// (get) Token: 0x06001058 RID: 4184
	protected abstract CellOffset[] ControlPortOffsets { get; }

	// Token: 0x06001059 RID: 4185
	protected abstract LogicGateBase.Op GetLogicOp();

	// Token: 0x0600105A RID: 4186
	protected abstract LogicGate.LogicGateDescriptions GetDescriptions();

	// Token: 0x0600105B RID: 4187 RVA: 0x000B1845 File Offset: 0x000AFA45
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		GeneratedBuildings.MakeBuildingAlwaysOperational(go);
		BuildingConfigManager.Instance.IgnoreDefaultKComponent(typeof(RequiresFoundation), prefab_tag);
	}

	// Token: 0x0600105C RID: 4188 RVA: 0x000B1862 File Offset: 0x000AFA62
	public override void DoPostConfigurePreview(BuildingDef def, GameObject go)
	{
		base.DoPostConfigurePreview(def, go);
		MoveableLogicGateVisualizer moveableLogicGateVisualizer = go.AddComponent<MoveableLogicGateVisualizer>();
		moveableLogicGateVisualizer.op = this.GetLogicOp();
		moveableLogicGateVisualizer.inputPortOffsets = this.InputPortOffsets;
		moveableLogicGateVisualizer.outputPortOffsets = this.OutputPortOffsets;
		moveableLogicGateVisualizer.controlPortOffsets = this.ControlPortOffsets;
	}

	// Token: 0x0600105D RID: 4189 RVA: 0x000B18A1 File Offset: 0x000AFAA1
	public override void DoPostConfigureUnderConstruction(GameObject go)
	{
		base.DoPostConfigureUnderConstruction(go);
		LogicGateVisualizer logicGateVisualizer = go.AddComponent<LogicGateVisualizer>();
		logicGateVisualizer.op = this.GetLogicOp();
		logicGateVisualizer.inputPortOffsets = this.InputPortOffsets;
		logicGateVisualizer.outputPortOffsets = this.OutputPortOffsets;
		logicGateVisualizer.controlPortOffsets = this.ControlPortOffsets;
	}

	// Token: 0x0600105E RID: 4190 RVA: 0x0018A3F8 File Offset: 0x001885F8
	public override void DoPostConfigureComplete(GameObject go)
	{
		LogicGate logicGate = go.AddComponent<LogicGate>();
		logicGate.op = this.GetLogicOp();
		logicGate.inputPortOffsets = this.InputPortOffsets;
		logicGate.outputPortOffsets = this.OutputPortOffsets;
		logicGate.controlPortOffsets = this.ControlPortOffsets;
		go.GetComponent<KPrefabID>().prefabInitFn += delegate(GameObject game_object)
		{
			game_object.GetComponent<LogicGate>().SetPortDescriptions(this.GetDescriptions());
		};
		go.GetComponent<KPrefabID>().AddTag(GameTags.OverlayBehindConduits, false);
	}
}
