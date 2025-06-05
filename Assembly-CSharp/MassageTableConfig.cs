using System;
using TUNING;
using UnityEngine;

// Token: 0x0200040B RID: 1035
public class MassageTableConfig : IBuildingConfig
{
	// Token: 0x0600112C RID: 4396 RVA: 0x0018D164 File Offset: 0x0018B364
	public override BuildingDef CreateBuildingDef()
	{
		string id = "MassageTable";
		int width = 2;
		int height = 2;
		string anim = "masseur_kanim";
		int hitpoints = 10;
		float construction_time = 10f;
		float[] tier = BUILDINGS.CONSTRUCTION_MASS_KG.TIER3;
		string[] raw_MINERALS = MATERIALS.RAW_MINERALS;
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER0;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, raw_MINERALS, melting_point, build_location_rule, BUILDINGS.DECOR.NONE, tier2, 0.2f);
		buildingDef.RequiresPowerInput = true;
		buildingDef.PowerInputOffset = new CellOffset(0, 0);
		buildingDef.Overheatable = true;
		buildingDef.EnergyConsumptionWhenActive = 240f;
		buildingDef.ExhaustKilowattsWhenActive = 0.125f;
		buildingDef.SelfHeatKilowattsWhenActive = 0.5f;
		buildingDef.AudioCategory = "Metal";
		return buildingDef;
	}

	// Token: 0x0600112D RID: 4397 RVA: 0x0018D1F4 File Offset: 0x0018B3F4
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.AddOrGet<LoopingSounds>();
		go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.DeStressingBuilding, false);
		MassageTable massageTable = go.AddOrGet<MassageTable>();
		massageTable.overrideAnims = new KAnimFile[]
		{
			Assets.GetAnim("anim_interacts_masseur_kanim")
		};
		massageTable.stressModificationValue = -30f;
		massageTable.roomStressModificationValue = -60f;
		massageTable.workLayer = Grid.SceneLayer.BuildingFront;
		Ownable ownable = go.AddOrGet<Ownable>();
		ownable.slotID = Db.Get().AssignableSlots.MassageTable.Id;
		ownable.canBePublic = true;
		RoomTracker roomTracker = go.AddOrGet<RoomTracker>();
		roomTracker.requiredRoomType = Db.Get().RoomTypes.MassageClinic.Id;
		roomTracker.requirement = RoomTracker.Requirement.Recommended;
	}

	// Token: 0x0600112E RID: 4398 RVA: 0x000B2145 File Offset: 0x000B0345
	public override void DoPostConfigureComplete(GameObject go)
	{
		go.GetComponent<KAnimControllerBase>().initialAnim = "off";
		go.AddOrGet<CopyBuildingSettings>();
	}

	// Token: 0x04000BEF RID: 3055
	public const string ID = "MassageTable";
}
