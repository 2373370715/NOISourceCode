using System;
using TUNING;
using UnityEngine;

// Token: 0x02000023 RID: 35
public class ArtifactAnalysisStationConfig : IBuildingConfig
{
	// Token: 0x0600008B RID: 139 RVA: 0x000AA117 File Offset: 0x000A8317
	public override string[] GetRequiredDlcIds()
	{
		return DlcManager.EXPANSION1;
	}

	// Token: 0x0600008C RID: 140 RVA: 0x00148AE4 File Offset: 0x00146CE4
	public override BuildingDef CreateBuildingDef()
	{
		string id = "ArtifactAnalysisStation";
		int width = 4;
		int height = 4;
		string anim = "artifact_analysis_kanim";
		int hitpoints = 30;
		float construction_time = 60f;
		float[] tier = BUILDINGS.CONSTRUCTION_MASS_KG.TIER5;
		string[] all_METALS = MATERIALS.ALL_METALS;
		float melting_point = 2400f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER6;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, all_METALS, melting_point, build_location_rule, BUILDINGS.DECOR.PENALTY.TIER2, tier2, 0.2f);
		buildingDef.RequiresPowerInput = true;
		buildingDef.EnergyConsumptionWhenActive = 480f;
		buildingDef.SelfHeatKilowattsWhenActive = 1f;
		buildingDef.ViewMode = OverlayModes.Power.ID;
		buildingDef.AudioCategory = "HollowMetal";
		buildingDef.AudioSize = "large";
		buildingDef.RequiredSkillPerkID = Db.Get().SkillPerks.CanStudyArtifact.Id;
		return buildingDef;
	}

	// Token: 0x0600008D RID: 141 RVA: 0x00148B84 File Offset: 0x00146D84
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.AddOrGet<DropAllWorkable>();
		go.AddOrGet<BuildingComplete>().isManuallyOperated = true;
		go.AddOrGetDef<ArtifactAnalysisStation.Def>();
		go.AddOrGet<ArtifactAnalysisStationWorkable>();
		Prioritizable.AddRef(go);
		Storage storage = go.AddOrGet<Storage>();
		ManualDeliveryKG manualDeliveryKG = go.AddOrGet<ManualDeliveryKG>();
		manualDeliveryKG.SetStorage(storage);
		manualDeliveryKG.choreTypeIDHash = Db.Get().ChoreTypes.MachineFetch.IdHash;
		manualDeliveryKG.RequestedItemTag = GameTags.CharmedArtifact;
		manualDeliveryKG.refillMass = 1f;
		manualDeliveryKG.MinimumMass = 1f;
		manualDeliveryKG.capacity = 1f;
	}

	// Token: 0x0600008E RID: 142 RVA: 0x000AA038 File Offset: 0x000A8238
	public override void DoPostConfigureComplete(GameObject go)
	{
	}

	// Token: 0x0400006C RID: 108
	public const string ID = "ArtifactAnalysisStation";

	// Token: 0x0400006D RID: 109
	public const float WORK_TIME = 150f;
}
