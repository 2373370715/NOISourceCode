using System;
using TUNING;
using UnityEngine;

// Token: 0x0200035C RID: 860
public class GasValveConfig : IBuildingConfig
{
	// Token: 0x06000DA2 RID: 3490 RVA: 0x0017E318 File Offset: 0x0017C518
	public override BuildingDef CreateBuildingDef()
	{
		string id = "GasValve";
		int width = 1;
		int height = 2;
		string anim = "valvegas_kanim";
		int hitpoints = 30;
		float construction_time = 10f;
		float[] tier = BUILDINGS.CONSTRUCTION_MASS_KG.TIER1;
		string[] raw_METALS = MATERIALS.RAW_METALS;
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.Anywhere;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER1;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, raw_METALS, melting_point, build_location_rule, BUILDINGS.DECOR.PENALTY.TIER0, tier2, 0.2f);
		buildingDef.InputConduitType = ConduitType.Gas;
		buildingDef.OutputConduitType = ConduitType.Gas;
		buildingDef.Floodable = false;
		buildingDef.ViewMode = OverlayModes.GasConduits.ID;
		buildingDef.AudioCategory = "Metal";
		buildingDef.PermittedRotations = PermittedRotations.R360;
		buildingDef.UtilityInputOffset = new CellOffset(0, 0);
		buildingDef.UtilityOutputOffset = new CellOffset(0, 1);
		GeneratedBuildings.RegisterWithOverlay(OverlayScreen.GasVentIDs, "GasValve");
		return buildingDef;
	}

	// Token: 0x06000DA3 RID: 3491 RVA: 0x0017E3BC File Offset: 0x0017C5BC
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		GeneratedBuildings.MakeBuildingAlwaysOperational(go);
		BuildingConfigManager.Instance.IgnoreDefaultKComponent(typeof(RequiresFoundation), prefab_tag);
		ValveBase valveBase = go.AddOrGet<ValveBase>();
		valveBase.conduitType = ConduitType.Gas;
		valveBase.maxFlow = 1f;
		valveBase.animFlowRanges = new ValveBase.AnimRangeInfo[]
		{
			new ValveBase.AnimRangeInfo(0.25f, "lo"),
			new ValveBase.AnimRangeInfo(0.5f, "med"),
			new ValveBase.AnimRangeInfo(0.75f, "hi")
		};
		go.AddOrGet<Valve>();
		go.AddOrGet<Workable>().workTime = 5f;
	}

	// Token: 0x06000DA4 RID: 3492 RVA: 0x000B05D4 File Offset: 0x000AE7D4
	public override void DoPostConfigureComplete(GameObject go)
	{
		UnityEngine.Object.DestroyImmediate(go.GetComponent<RequireInputs>());
		UnityEngine.Object.DestroyImmediate(go.GetComponent<ConduitConsumer>());
		UnityEngine.Object.DestroyImmediate(go.GetComponent<ConduitDispenser>());
		go.AddOrGet<BuildingComplete>().isManuallyOperated = true;
		go.GetComponent<KPrefabID>().AddTag(GameTags.OverlayInFrontOfConduits, false);
	}

	// Token: 0x040009FA RID: 2554
	public const string ID = "GasValve";

	// Token: 0x040009FB RID: 2555
	private const ConduitType CONDUIT_TYPE = ConduitType.Gas;
}
