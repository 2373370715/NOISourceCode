using System;
using TUNING;
using UnityEngine;

// Token: 0x020005B2 RID: 1458
public class SolidTransferArmConfig : IBuildingConfig
{
	// Token: 0x0600193F RID: 6463 RVA: 0x001AE100 File Offset: 0x001AC300
	public override BuildingDef CreateBuildingDef()
	{
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef("SolidTransferArm", 3, 1, "conveyor_transferarm_kanim", 10, 10f, BUILDINGS.CONSTRUCTION_MASS_KG.TIER3, MATERIALS.REFINED_METALS, 1600f, BuildLocationRule.Anywhere, BUILDINGS.DECOR.PENALTY.TIER2, NOISE_POLLUTION.NOISY.TIER0, 0.2f);
		buildingDef.Floodable = false;
		buildingDef.AudioCategory = "Metal";
		buildingDef.RequiresPowerInput = true;
		buildingDef.EnergyConsumptionWhenActive = 120f;
		buildingDef.ExhaustKilowattsWhenActive = 0f;
		buildingDef.SelfHeatKilowattsWhenActive = 2f;
		buildingDef.LogicInputPorts = LogicOperationalController.CreateSingleInputPortList(new CellOffset(0, 0));
		buildingDef.PermittedRotations = PermittedRotations.R360;
		GeneratedBuildings.RegisterWithOverlay(OverlayScreen.SolidConveyorIDs, "SolidTransferArm");
		return buildingDef;
	}

	// Token: 0x06001940 RID: 6464 RVA: 0x000B507E File Offset: 0x000B327E
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.AddOrGet<Operational>();
		go.AddOrGet<LoopingSounds>();
	}

	// Token: 0x06001941 RID: 6465 RVA: 0x000B508E File Offset: 0x000B328E
	public override void DoPostConfigurePreview(BuildingDef def, GameObject go)
	{
		SolidTransferArmConfig.AddVisualizer(go, true);
	}

	// Token: 0x06001942 RID: 6466 RVA: 0x000B5097 File Offset: 0x000B3297
	public override void DoPostConfigureUnderConstruction(GameObject go)
	{
		SolidTransferArmConfig.AddVisualizer(go, false);
		go.GetComponent<Constructable>().requiredSkillPerk = Db.Get().SkillPerks.ConveyorBuild.Id;
	}

	// Token: 0x06001943 RID: 6467 RVA: 0x000B50BF File Offset: 0x000B32BF
	public override void DoPostConfigureComplete(GameObject go)
	{
		go.AddOrGet<LogicOperationalController>();
		go.AddOrGet<SolidTransferArm>().pickupRange = 4;
		SolidTransferArmConfig.AddVisualizer(go, false);
	}

	// Token: 0x06001944 RID: 6468 RVA: 0x001AE1A8 File Offset: 0x001AC3A8
	private static void AddVisualizer(GameObject prefab, bool movable)
	{
		RangeVisualizer rangeVisualizer = prefab.AddOrGet<RangeVisualizer>();
		rangeVisualizer.OriginOffset = new Vector2I(0, 0);
		rangeVisualizer.RangeMin.x = -4;
		rangeVisualizer.RangeMin.y = -4;
		rangeVisualizer.RangeMax.x = 4;
		rangeVisualizer.RangeMax.y = 4;
		rangeVisualizer.BlockingTileVisible = true;
	}

	// Token: 0x0400105F RID: 4191
	public const string ID = "SolidTransferArm";

	// Token: 0x04001060 RID: 4192
	private const int RANGE = 4;
}
