using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020003C8 RID: 968
public class LiquidFilterConfig : IBuildingConfig
{
	// Token: 0x06000FBD RID: 4029 RVA: 0x00188464 File Offset: 0x00186664
	public override BuildingDef CreateBuildingDef()
	{
		string id = "LiquidFilter";
		int width = 3;
		int height = 1;
		string anim = "filter_liquid_kanim";
		int hitpoints = 30;
		float construction_time = 10f;
		float[] tier = TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER3;
		string[] raw_METALS = MATERIALS.RAW_METALS;
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.Anywhere;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER1;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, raw_METALS, melting_point, build_location_rule, TUNING.BUILDINGS.DECOR.PENALTY.TIER0, tier2, 0.2f);
		buildingDef.RequiresPowerInput = true;
		buildingDef.EnergyConsumptionWhenActive = 120f;
		buildingDef.SelfHeatKilowattsWhenActive = 4f;
		buildingDef.ExhaustKilowattsWhenActive = 0f;
		buildingDef.InputConduitType = ConduitType.Liquid;
		buildingDef.OutputConduitType = ConduitType.Liquid;
		buildingDef.Floodable = false;
		buildingDef.ViewMode = OverlayModes.LiquidConduits.ID;
		buildingDef.AudioCategory = "Metal";
		buildingDef.UtilityInputOffset = new CellOffset(-1, 0);
		buildingDef.UtilityOutputOffset = new CellOffset(1, 0);
		buildingDef.PermittedRotations = PermittedRotations.R360;
		GeneratedBuildings.RegisterWithOverlay(OverlayScreen.LiquidVentIDs, "LiquidFilter");
		buildingDef.AddSearchTerms(SEARCH_TERMS.FILTER);
		return buildingDef;
	}

	// Token: 0x06000FBE RID: 4030 RVA: 0x000B132E File Offset: 0x000AF52E
	private void AttachPort(GameObject go)
	{
		go.AddComponent<ConduitSecondaryOutput>().portInfo = this.secondaryPort;
	}

	// Token: 0x06000FBF RID: 4031 RVA: 0x000B1341 File Offset: 0x000AF541
	public override void DoPostConfigurePreview(BuildingDef def, GameObject go)
	{
		base.DoPostConfigurePreview(def, go);
		this.AttachPort(go);
	}

	// Token: 0x06000FC0 RID: 4032 RVA: 0x000B1352 File Offset: 0x000AF552
	public override void DoPostConfigureUnderConstruction(GameObject go)
	{
		base.DoPostConfigureUnderConstruction(go);
		this.AttachPort(go);
	}

	// Token: 0x06000FC1 RID: 4033 RVA: 0x000B1362 File Offset: 0x000AF562
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.IndustrialMachinery, false);
		go.AddOrGet<ElementFilter>().portInfo = this.secondaryPort;
		go.AddOrGet<Filterable>().filterElementState = Filterable.ElementState.Liquid;
	}

	// Token: 0x06000FC2 RID: 4034 RVA: 0x000B04DD File Offset: 0x000AE6DD
	public override void DoPostConfigureComplete(GameObject go)
	{
		go.AddOrGetDef<PoweredActiveController.Def>().showWorkingStatus = true;
		go.GetComponent<KPrefabID>().AddTag(GameTags.OverlayInFrontOfConduits, false);
	}

	// Token: 0x04000B5F RID: 2911
	public const string ID = "LiquidFilter";

	// Token: 0x04000B60 RID: 2912
	private const ConduitType CONDUIT_TYPE = ConduitType.Liquid;

	// Token: 0x04000B61 RID: 2913
	private ConduitPortInfo secondaryPort = new ConduitPortInfo(ConduitType.Liquid, new CellOffset(0, 0));
}
