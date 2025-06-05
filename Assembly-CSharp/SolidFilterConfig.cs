using System;
using TUNING;
using UnityEngine;

// Token: 0x020005AF RID: 1455
public class SolidFilterConfig : IBuildingConfig
{
	// Token: 0x06001930 RID: 6448 RVA: 0x001ADCD8 File Offset: 0x001ABED8
	public override BuildingDef CreateBuildingDef()
	{
		string id = "SolidFilter";
		int width = 3;
		int height = 1;
		string anim = "filter_material_conveyor_kanim";
		int hitpoints = 30;
		float construction_time = 10f;
		float[] tier = BUILDINGS.CONSTRUCTION_MASS_KG.TIER1;
		string[] raw_METALS = MATERIALS.RAW_METALS;
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.Anywhere;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER1;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, raw_METALS, melting_point, build_location_rule, BUILDINGS.DECOR.PENALTY.TIER0, tier2, 0.2f);
		buildingDef.RequiresPowerInput = true;
		buildingDef.EnergyConsumptionWhenActive = 120f;
		buildingDef.SelfHeatKilowattsWhenActive = 0f;
		buildingDef.ExhaustKilowattsWhenActive = 0f;
		buildingDef.InputConduitType = ConduitType.Solid;
		buildingDef.OutputConduitType = ConduitType.Solid;
		buildingDef.Floodable = false;
		buildingDef.ViewMode = OverlayModes.SolidConveyor.ID;
		buildingDef.AudioCategory = "Metal";
		buildingDef.UtilityInputOffset = new CellOffset(-1, 0);
		buildingDef.UtilityOutputOffset = new CellOffset(1, 0);
		buildingDef.PermittedRotations = PermittedRotations.R360;
		GeneratedBuildings.RegisterWithOverlay(OverlayScreen.SolidConveyorIDs, "SolidFilter");
		return buildingDef;
	}

	// Token: 0x06001931 RID: 6449 RVA: 0x000B4FBB File Offset: 0x000B31BB
	private void AttachPort(GameObject go)
	{
		go.AddComponent<ConduitSecondaryOutput>().portInfo = this.secondaryPort;
	}

	// Token: 0x06001932 RID: 6450 RVA: 0x000B4FCE File Offset: 0x000B31CE
	public override void DoPostConfigurePreview(BuildingDef def, GameObject go)
	{
		base.DoPostConfigurePreview(def, go);
		this.AttachPort(go);
	}

	// Token: 0x06001933 RID: 6451 RVA: 0x000B4FDF File Offset: 0x000B31DF
	public override void DoPostConfigureUnderConstruction(GameObject go)
	{
		base.DoPostConfigureUnderConstruction(go);
		this.AttachPort(go);
	}

	// Token: 0x06001934 RID: 6452 RVA: 0x000B4FEF File Offset: 0x000B31EF
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.IndustrialMachinery, false);
		go.AddOrGet<ElementFilter>().portInfo = this.secondaryPort;
		go.AddOrGet<Filterable>().filterElementState = Filterable.ElementState.Solid;
	}

	// Token: 0x06001935 RID: 6453 RVA: 0x000B501F File Offset: 0x000B321F
	public override void DoPostConfigureComplete(GameObject go)
	{
		go.AddOrGetDef<PoweredActiveController.Def>().showWorkingStatus = true;
	}

	// Token: 0x04001058 RID: 4184
	public const string ID = "SolidFilter";

	// Token: 0x04001059 RID: 4185
	private const ConduitType CONDUIT_TYPE = ConduitType.Solid;

	// Token: 0x0400105A RID: 4186
	private ConduitPortInfo secondaryPort = new ConduitPortInfo(ConduitType.Solid, new CellOffset(0, 0));
}
