using System;
using TUNING;
using UnityEngine;

// Token: 0x02000387 RID: 903
public class HighEnergyParticleSpawnerConfig : IBuildingConfig
{
	// Token: 0x06000E7C RID: 3708 RVA: 0x000AA117 File Offset: 0x000A8317
	public override string[] GetRequiredDlcIds()
	{
		return DlcManager.EXPANSION1;
	}

	// Token: 0x06000E7D RID: 3709 RVA: 0x001838CC File Offset: 0x00181ACC
	public override BuildingDef CreateBuildingDef()
	{
		string id = "HighEnergyParticleSpawner";
		int width = 1;
		int height = 2;
		string anim = "radiation_collector_kanim";
		int hitpoints = 30;
		float construction_time = 10f;
		float[] tier = BUILDINGS.CONSTRUCTION_MASS_KG.TIER4;
		string[] raw_MINERALS = MATERIALS.RAW_MINERALS;
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.NotInTiles;
		EffectorValues none = NOISE_POLLUTION.NONE;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, raw_MINERALS, melting_point, build_location_rule, BUILDINGS.DECOR.PENALTY.TIER1, none, 0.2f);
		buildingDef.Floodable = false;
		buildingDef.AudioCategory = "Metal";
		buildingDef.Overheatable = false;
		buildingDef.ViewMode = OverlayModes.Radiation.ID;
		buildingDef.PermittedRotations = PermittedRotations.R360;
		buildingDef.UseHighEnergyParticleOutputPort = true;
		buildingDef.HighEnergyParticleOutputOffset = new CellOffset(0, 1);
		buildingDef.RequiresPowerInput = true;
		buildingDef.PowerInputOffset = new CellOffset(0, 0);
		buildingDef.EnergyConsumptionWhenActive = 480f;
		buildingDef.ExhaustKilowattsWhenActive = 1f;
		buildingDef.SelfHeatKilowattsWhenActive = 4f;
		buildingDef.DiseaseCellVisName = "RadiationSickness";
		buildingDef.UtilityOutputOffset = CellOffset.none;
		GeneratedBuildings.RegisterWithOverlay(OverlayScreen.RadiationIDs, "HighEnergyParticleSpawner");
		buildingDef.Deprecated = !Sim.IsRadiationEnabled();
		return buildingDef;
	}

	// Token: 0x06000E7E RID: 3710 RVA: 0x001839BC File Offset: 0x00181BBC
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		BuildingConfigManager.Instance.IgnoreDefaultKComponent(typeof(RequiresFoundation), prefab_tag);
		Prioritizable.AddRef(go);
		go.AddOrGet<HighEnergyParticleStorage>().capacity = 500f;
		go.AddOrGet<LoopingSounds>();
		HighEnergyParticleSpawner highEnergyParticleSpawner = go.AddOrGet<HighEnergyParticleSpawner>();
		highEnergyParticleSpawner.minLaunchInterval = 2f;
		highEnergyParticleSpawner.radiationSampleRate = 0.2f;
		highEnergyParticleSpawner.minSlider = 50;
		highEnergyParticleSpawner.maxSlider = 500;
	}

	// Token: 0x06000E7F RID: 3711 RVA: 0x000AA038 File Offset: 0x000A8238
	public override void DoPostConfigureComplete(GameObject go)
	{
	}

	// Token: 0x04000AAB RID: 2731
	public const string ID = "HighEnergyParticleSpawner";

	// Token: 0x04000AAC RID: 2732
	public const float MIN_LAUNCH_INTERVAL = 2f;

	// Token: 0x04000AAD RID: 2733
	public const float RADIATION_SAMPLE_RATE = 0.2f;

	// Token: 0x04000AAE RID: 2734
	public const float HEP_PER_RAD = 0.1f;

	// Token: 0x04000AAF RID: 2735
	public const int MIN_SLIDER = 50;

	// Token: 0x04000AB0 RID: 2736
	public const int MAX_SLIDER = 500;

	// Token: 0x04000AB1 RID: 2737
	public const float DISABLED_CONSUMPTION_RATE = 1f;
}
