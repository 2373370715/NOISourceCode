using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000422 RID: 1058
public class MilkFeederConfig : IBuildingConfig
{
	// Token: 0x06001199 RID: 4505 RVA: 0x0018FCA8 File Offset: 0x0018DEA8
	public override BuildingDef CreateBuildingDef()
	{
		string id = "MilkFeeder";
		int width = 3;
		int height = 3;
		string anim = "critter_milk_feeder_kanim";
		int hitpoints = 100;
		float construction_time = 120f;
		float[] tier = TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER4;
		string[] refined_METALS = MATERIALS.REFINED_METALS;
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
		EffectorValues none = NOISE_POLLUTION.NONE;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, refined_METALS, melting_point, build_location_rule, TUNING.BUILDINGS.DECOR.PENALTY.TIER2, none, 0.2f);
		buildingDef.AudioCategory = "Metal";
		buildingDef.PermittedRotations = PermittedRotations.FlipH;
		buildingDef.InputConduitType = ConduitType.Liquid;
		buildingDef.UtilityInputOffset = new CellOffset(0, 0);
		buildingDef.LogicInputPorts = LogicOperationalController.CreateSingleInputPortList(new CellOffset(0, 0));
		buildingDef.AddSearchTerms(SEARCH_TERMS.RANCHING);
		buildingDef.AddSearchTerms(SEARCH_TERMS.CRITTER);
		return buildingDef;
	}

	// Token: 0x0600119A RID: 4506 RVA: 0x000AA038 File Offset: 0x000A8238
	public override void DoPostConfigureUnderConstruction(GameObject go)
	{
	}

	// Token: 0x0600119B RID: 4507 RVA: 0x0018FD48 File Offset: 0x0018DF48
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		Prioritizable.AddRef(go);
		go.AddOrGet<LogicOperationalController>();
		RoomTracker roomTracker = go.AddOrGet<RoomTracker>();
		roomTracker.requiredRoomType = Db.Get().RoomTypes.CreaturePen.Id;
		roomTracker.requirement = RoomTracker.Requirement.Required;
		Storage storage = go.AddOrGet<Storage>();
		storage.capacityKg = 80f;
		storage.showInUI = true;
		storage.showDescriptor = true;
		storage.allowItemRemoval = false;
		storage.allowSettingOnlyFetchMarkedItems = false;
		storage.showCapacityStatusItem = true;
		storage.showCapacityAsMainStatus = true;
		ConduitConsumer conduitConsumer = go.AddOrGet<ConduitConsumer>();
		conduitConsumer.conduitType = ConduitType.Liquid;
		conduitConsumer.consumptionRate = 10f;
		conduitConsumer.capacityTag = GameTagExtensions.Create(SimHashes.Milk);
		conduitConsumer.forceAlwaysSatisfied = true;
		conduitConsumer.wrongElementResult = ConduitConsumer.WrongElementResult.Dump;
		conduitConsumer.storage = storage;
		go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.RanchStationType, false);
	}

	// Token: 0x0600119C RID: 4508 RVA: 0x000B2287 File Offset: 0x000B0487
	public override void DoPostConfigureComplete(GameObject go)
	{
		go.AddOrGetDef<MilkFeeder.Def>();
	}

	// Token: 0x0600119D RID: 4509 RVA: 0x000AA038 File Offset: 0x000A8238
	public override void ConfigurePost(BuildingDef def)
	{
	}

	// Token: 0x04000C4B RID: 3147
	public const string ID = "MilkFeeder";

	// Token: 0x04000C4C RID: 3148
	public const string HAD_CONSUMED_MILK_RECENTLY_EFFECT_ID = "HadMilk";

	// Token: 0x04000C4D RID: 3149
	public const float EFFECT_DURATION_IN_SECONDS = 600f;

	// Token: 0x04000C4E RID: 3150
	public static readonly CellOffset DRINK_FROM_OFFSET = new CellOffset(1, 0);

	// Token: 0x04000C4F RID: 3151
	public static readonly Tag MILK_TAG = SimHashes.Milk.CreateTag();

	// Token: 0x04000C50 RID: 3152
	public const float UNITS_OF_MILK_CONSUMED_PER_FEEDING = 5f;
}
