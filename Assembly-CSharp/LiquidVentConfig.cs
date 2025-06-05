using System;
using TUNING;
using UnityEngine;

// Token: 0x020003D5 RID: 981
public class LiquidVentConfig : IBuildingConfig
{
	// Token: 0x06000FF2 RID: 4082 RVA: 0x00189570 File Offset: 0x00187770
	public override BuildingDef CreateBuildingDef()
	{
		string id = "LiquidVent";
		int width = 1;
		int height = 1;
		string anim = "ventliquid_kanim";
		int hitpoints = 30;
		float construction_time = 30f;
		float[] tier = BUILDINGS.CONSTRUCTION_MASS_KG.TIER4;
		string[] all_METALS = MATERIALS.ALL_METALS;
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.Anywhere;
		EffectorValues none = NOISE_POLLUTION.NONE;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, all_METALS, melting_point, build_location_rule, BUILDINGS.DECOR.PENALTY.TIER1, none, 0.2f);
		buildingDef.InputConduitType = ConduitType.Liquid;
		buildingDef.Floodable = false;
		buildingDef.Overheatable = false;
		buildingDef.ViewMode = OverlayModes.LiquidConduits.ID;
		buildingDef.AudioCategory = "Metal";
		buildingDef.UtilityInputOffset = new CellOffset(0, 0);
		buildingDef.UtilityOutputOffset = new CellOffset(0, 0);
		buildingDef.LogicInputPorts = LogicOperationalController.CreateSingleInputPortList(new CellOffset(0, 0));
		GeneratedBuildings.RegisterWithOverlay(OverlayScreen.LiquidVentIDs, "LiquidVent");
		SoundEventVolumeCache.instance.AddVolume("ventliquid_kanim", "LiquidVent_squirt", NOISE_POLLUTION.NOISY.TIER0);
		return buildingDef;
	}

	// Token: 0x06000FF3 RID: 4083 RVA: 0x00189638 File Offset: 0x00187838
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.AddOrGet<LoopingSounds>();
		go.AddOrGet<Exhaust>();
		go.AddOrGet<LogicOperationalController>();
		Vent vent = go.AddOrGet<Vent>();
		vent.conduitType = ConduitType.Liquid;
		vent.endpointType = Endpoint.Sink;
		vent.overpressureMass = 1000f;
		ConduitConsumer conduitConsumer = go.AddOrGet<ConduitConsumer>();
		conduitConsumer.conduitType = ConduitType.Liquid;
		conduitConsumer.ignoreMinMassCheck = true;
		BuildingTemplates.CreateDefaultStorage(go, false).showInUI = true;
		go.AddOrGet<SimpleVent>();
	}

	// Token: 0x06000FF4 RID: 4084 RVA: 0x000B1450 File Offset: 0x000AF650
	public override void DoPostConfigureComplete(GameObject go)
	{
		VentController.Def def = go.AddOrGetDef<VentController.Def>();
		def.usingDynamicColor = true;
		def.outputSubstanceAnimName = "leak";
		go.GetComponent<KPrefabID>().AddTag(GameTags.OverlayInFrontOfConduits, false);
	}

	// Token: 0x04000B7A RID: 2938
	public const string ID = "LiquidVent";

	// Token: 0x04000B7B RID: 2939
	public const float OVERPRESSURE_MASS = 1000f;

	// Token: 0x04000B7C RID: 2940
	private const ConduitType CONDUIT_TYPE = ConduitType.Liquid;
}
