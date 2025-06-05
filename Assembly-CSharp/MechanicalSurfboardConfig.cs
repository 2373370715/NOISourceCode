using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x0200040D RID: 1037
public class MechanicalSurfboardConfig : IBuildingConfig
{
	// Token: 0x06001133 RID: 4403 RVA: 0x0018D42C File Offset: 0x0018B62C
	public override BuildingDef CreateBuildingDef()
	{
		string id = "MechanicalSurfboard";
		int width = 2;
		int height = 3;
		string anim = "mechanical_surfboard_kanim";
		int hitpoints = 30;
		float construction_time = 60f;
		float[] tier = TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER4;
		string[] raw_METALS = MATERIALS.RAW_METALS;
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
		EffectorValues none = NOISE_POLLUTION.NONE;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, raw_METALS, melting_point, build_location_rule, TUNING.BUILDINGS.DECOR.BONUS.TIER1, none, 0.2f);
		buildingDef.ViewMode = OverlayModes.LiquidConduits.ID;
		buildingDef.Floodable = false;
		buildingDef.AudioCategory = "Metal";
		buildingDef.Overheatable = true;
		buildingDef.InputConduitType = ConduitType.Liquid;
		buildingDef.UtilityInputOffset = new CellOffset(1, 0);
		buildingDef.RequiresPowerInput = true;
		buildingDef.PowerInputOffset = new CellOffset(0, 0);
		buildingDef.EnergyConsumptionWhenActive = 480f;
		buildingDef.SelfHeatKilowattsWhenActive = 1f;
		buildingDef.AddSearchTerms(SEARCH_TERMS.MORALE);
		return buildingDef;
	}

	// Token: 0x06001134 RID: 4404 RVA: 0x0018D4E4 File Offset: 0x0018B6E4
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.RecBuilding, false);
		go.AddOrGet<Storage>().SetDefaultStoredItemModifiers(Storage.StandardFabricatorStorage);
		ConduitConsumer conduitConsumer = go.AddOrGet<ConduitConsumer>();
		conduitConsumer.conduitType = ConduitType.Liquid;
		conduitConsumer.capacityTag = ElementLoader.FindElementByHash(SimHashes.Water).tag;
		conduitConsumer.capacityKG = 20f;
		conduitConsumer.wrongElementResult = ConduitConsumer.WrongElementResult.Dump;
		go.AddOrGet<MechanicalSurfboardWorkable>().basePriority = RELAXATION.PRIORITY.TIER3;
		MechanicalSurfboard mechanicalSurfboard = go.AddOrGet<MechanicalSurfboard>();
		mechanicalSurfboard.waterSpillRateKG = 0.05f;
		mechanicalSurfboard.minOperationalWaterKG = 2f;
		mechanicalSurfboard.specificEffect = "MechanicalSurfboard";
		mechanicalSurfboard.trackingEffect = "RecentlyMechanicalSurfboard";
		RoomTracker roomTracker = go.AddOrGet<RoomTracker>();
		roomTracker.requiredRoomType = Db.Get().RoomTypes.RecRoom.Id;
		roomTracker.requirement = RoomTracker.Requirement.Recommended;
		go.AddOrGetDef<RocketUsageRestriction.Def>();
	}

	// Token: 0x06001135 RID: 4405 RVA: 0x000AA038 File Offset: 0x000A8238
	public override void DoPostConfigureComplete(GameObject go)
	{
	}

	// Token: 0x04000BF3 RID: 3059
	public const string ID = "MechanicalSurfboard";

	// Token: 0x04000BF4 RID: 3060
	private const float TANK_SIZE_KG = 20f;

	// Token: 0x04000BF5 RID: 3061
	private const float SPILL_RATE_KG = 0.05f;
}
