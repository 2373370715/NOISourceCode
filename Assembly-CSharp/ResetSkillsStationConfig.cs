using System;
using TUNING;
using UnityEngine;

// Token: 0x02000559 RID: 1369
public class ResetSkillsStationConfig : IBuildingConfig
{
	// Token: 0x06001790 RID: 6032 RVA: 0x001A6730 File Offset: 0x001A4930
	public override BuildingDef CreateBuildingDef()
	{
		string id = "ResetSkillsStation";
		int width = 3;
		int height = 3;
		string anim = "reSpeccer_kanim";
		int hitpoints = 30;
		float construction_time = 30f;
		float[] tier = BUILDINGS.CONSTRUCTION_MASS_KG.TIER4;
		string[] all_METALS = MATERIALS.ALL_METALS;
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER1;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, all_METALS, melting_point, build_location_rule, BUILDINGS.DECOR.NONE, tier2, 0.2f);
		buildingDef.RequiresPowerInput = true;
		buildingDef.EnergyConsumptionWhenActive = 480f;
		buildingDef.ExhaustKilowattsWhenActive = 0.5f;
		buildingDef.SelfHeatKilowattsWhenActive = 4f;
		buildingDef.ViewMode = OverlayModes.Power.ID;
		buildingDef.AudioCategory = "Metal";
		buildingDef.AudioSize = "large";
		return buildingDef;
	}

	// Token: 0x06001791 RID: 6033 RVA: 0x001A67C0 File Offset: 0x001A49C0
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.IndustrialMachinery, false);
		go.AddTag(GameTags.NotRoomAssignable);
		go.AddOrGet<BuildingComplete>().isManuallyOperated = true;
		go.AddOrGet<Ownable>().slotID = Db.Get().AssignableSlots.ResetSkillsStation.Id;
		ResetSkillsStation resetSkillsStation = go.AddOrGet<ResetSkillsStation>();
		resetSkillsStation.workTime = 180f;
		resetSkillsStation.overrideAnims = new KAnimFile[]
		{
			Assets.GetAnim("anim_interacts_reSpeccer_kanim")
		};
		resetSkillsStation.workLayer = Grid.SceneLayer.BuildingFront;
	}

	// Token: 0x06001792 RID: 6034 RVA: 0x000AA038 File Offset: 0x000A8238
	public override void DoPostConfigureComplete(GameObject go)
	{
	}

	// Token: 0x04000F8E RID: 3982
	public const string ID = "ResetSkillsStation";
}
