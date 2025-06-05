using System;
using TUNING;
using UnityEngine;

// Token: 0x0200050D RID: 1293
public class PowerTransformerConfig : IBuildingConfig
{
	// Token: 0x0600162C RID: 5676 RVA: 0x001A2310 File Offset: 0x001A0510
	public override BuildingDef CreateBuildingDef()
	{
		string id = "PowerTransformer";
		int width = 3;
		int height = 2;
		string anim = "transformer_kanim";
		int hitpoints = 30;
		float construction_time = 30f;
		float[] tier = BUILDINGS.CONSTRUCTION_MASS_KG.TIER3;
		string[] refined_METALS = MATERIALS.REFINED_METALS;
		float melting_point = 800f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER5;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, refined_METALS, melting_point, build_location_rule, BUILDINGS.DECOR.PENALTY.TIER1, tier2, 0.2f);
		buildingDef.RequiresPowerInput = true;
		buildingDef.RequiresPowerOutput = true;
		buildingDef.PowerInputOffset = new CellOffset(-1, 1);
		buildingDef.PowerOutputOffset = new CellOffset(1, 0);
		buildingDef.ElectricalArrowOffset = new CellOffset(1, 0);
		buildingDef.ViewMode = OverlayModes.Power.ID;
		buildingDef.AudioCategory = "Metal";
		buildingDef.ExhaustKilowattsWhenActive = 0f;
		buildingDef.SelfHeatKilowattsWhenActive = 1f;
		buildingDef.Entombable = true;
		buildingDef.GeneratorWattageRating = 4000f;
		buildingDef.GeneratorBaseCapacity = 4000f;
		buildingDef.PermittedRotations = PermittedRotations.FlipH;
		return buildingDef;
	}

	// Token: 0x0600162D RID: 5677 RVA: 0x001A23DC File Offset: 0x001A05DC
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.IndustrialMachinery, false);
		go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.PowerBuilding, false);
		go.AddComponent<RequireInputs>();
		BuildingDef def = go.GetComponent<Building>().Def;
		Battery battery = go.AddOrGet<Battery>();
		battery.powerSortOrder = 1000;
		battery.capacity = def.GeneratorWattageRating;
		battery.chargeWattage = def.GeneratorWattageRating;
		go.AddComponent<PowerTransformer>().powerDistributionOrder = 9;
	}

	// Token: 0x0600162E RID: 5678 RVA: 0x000B4278 File Offset: 0x000B2478
	public override void DoPostConfigureComplete(GameObject go)
	{
		UnityEngine.Object.DestroyImmediate(go.GetComponent<EnergyConsumer>());
		go.AddOrGetDef<PoweredActiveController.Def>();
	}

	// Token: 0x04000F3D RID: 3901
	public const string ID = "PowerTransformer";
}
