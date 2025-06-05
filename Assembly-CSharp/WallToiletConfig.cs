using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020005F4 RID: 1524
public class WallToiletConfig : IBuildingConfig
{
	// Token: 0x06001AD8 RID: 6872 RVA: 0x000AA117 File Offset: 0x000A8317
	public override string[] GetRequiredDlcIds()
	{
		return DlcManager.EXPANSION1;
	}

	// Token: 0x06001AD9 RID: 6873 RVA: 0x001B5080 File Offset: 0x001B3280
	public override BuildingDef CreateBuildingDef()
	{
		string id = "WallToilet";
		int width = 1;
		int height = 3;
		string anim = "toilet_wall_kanim";
		int hitpoints = 30;
		float construction_time = 30f;
		float[] tier = TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER2;
		string[] plastics = MATERIALS.PLASTICS;
		float melting_point = 800f;
		BuildLocationRule build_location_rule = BuildLocationRule.WallFloor;
		EffectorValues none = NOISE_POLLUTION.NONE;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, plastics, melting_point, build_location_rule, TUNING.BUILDINGS.DECOR.PENALTY.TIER1, none, 0.2f);
		buildingDef.Overheatable = false;
		buildingDef.ExhaustKilowattsWhenActive = 0.25f;
		buildingDef.SelfHeatKilowattsWhenActive = 0f;
		buildingDef.InputConduitType = ConduitType.Liquid;
		buildingDef.ViewMode = OverlayModes.LiquidConduits.ID;
		buildingDef.DiseaseCellVisName = DUPLICANTSTATS.STANDARD.Secretions.PEE_DISEASE;
		buildingDef.UtilityOutputOffset = new CellOffset(-2, 0);
		buildingDef.AudioCategory = "Metal";
		buildingDef.UtilityInputOffset = new CellOffset(0, 0);
		buildingDef.PermittedRotations = PermittedRotations.FlipH;
		buildingDef.AddSearchTerms(SEARCH_TERMS.TOILET);
		return buildingDef;
	}

	// Token: 0x06001ADA RID: 6874 RVA: 0x001B5148 File Offset: 0x001B3348
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		KPrefabID component = go.GetComponent<KPrefabID>();
		go.AddOrGet<LoopingSounds>();
		component.AddTag(RoomConstraints.ConstraintTags.ToiletType, false);
		component.AddTag(RoomConstraints.ConstraintTags.FlushToiletType, false);
		FlushToilet flushToilet = go.AddOrGet<FlushToilet>();
		flushToilet.massConsumedPerUse = 2.5f;
		flushToilet.massEmittedPerUse = 2.5f + DUPLICANTSTATS.STANDARD.Secretions.PEE_PER_TOILET_PEE;
		flushToilet.newPeeTemperature = DUPLICANTSTATS.STANDARD.Temperature.Internal.IDEAL;
		flushToilet.diseaseId = DUPLICANTSTATS.STANDARD.Secretions.PEE_DISEASE;
		flushToilet.diseasePerFlush = DUPLICANTSTATS.STANDARD.Secretions.DISEASE_PER_PEE;
		flushToilet.diseaseOnDupePerFlush = DUPLICANTSTATS.STANDARD.Secretions.DISEASE_PER_PEE / 5;
		flushToilet.requireOutput = false;
		flushToilet.meterOffset = Meter.Offset.Infront;
		ToiletWorkableUse toiletWorkableUse = go.AddOrGet<ToiletWorkableUse>();
		toiletWorkableUse.workLayer = Grid.SceneLayer.BuildingUse;
		toiletWorkableUse.resetProgressOnStop = true;
		ConduitConsumer conduitConsumer = go.AddOrGet<ConduitConsumer>();
		conduitConsumer.conduitType = ConduitType.Liquid;
		conduitConsumer.capacityTag = ElementLoader.FindElementByHash(SimHashes.Water).tag;
		conduitConsumer.capacityKG = 2.5f;
		conduitConsumer.wrongElementResult = ConduitConsumer.WrongElementResult.Store;
		AutoStorageDropper.Def def = go.AddOrGetDef<AutoStorageDropper.Def>();
		def.dropOffset = new CellOffset(-2, 0);
		def.elementFilter = new SimHashes[]
		{
			SimHashes.Water
		};
		def.invertElementFilterInitialValue = true;
		def.blockedBySubstantialLiquid = true;
		def.fxOffset = new Vector3(0.5f, 0f, 0f);
		def.leftFx = new AutoStorageDropper.DropperFxConfig
		{
			animFile = "liquidleak_kanim",
			animName = "side",
			flipX = true,
			layer = Grid.SceneLayer.BuildingBack
		};
		def.rightFx = new AutoStorageDropper.DropperFxConfig
		{
			animFile = "liquidleak_kanim",
			animName = "side",
			flipX = false,
			layer = Grid.SceneLayer.BuildingBack
		};
		def.delay = 0f;
		Storage storage = go.AddOrGet<Storage>();
		storage.capacityKg = 12.5f;
		storage.SetDefaultStoredItemModifiers(Storage.StandardSealedStorage);
		Ownable ownable = go.AddOrGet<Ownable>();
		ownable.slotID = Db.Get().AssignableSlots.Toilet.Id;
		ownable.canBePublic = true;
		ToiletWorkableClean toiletWorkableClean = go.AddOrGet<ToiletWorkableClean>();
		toiletWorkableClean.workTime = 90f;
		toiletWorkableClean.overrideAnims = new KAnimFile[]
		{
			Assets.GetAnim("anim_interacts_toilet_wall_kanim")
		};
		toiletWorkableClean.workLayer = Grid.SceneLayer.BuildingFront;
		toiletWorkableClean.SetIsCloggedByGunk(true);
		go.AddOrGetDef<RocketUsageRestriction.Def>();
		component.prefabInitFn += this.OnInit;
	}

	// Token: 0x06001ADB RID: 6875 RVA: 0x001B53AC File Offset: 0x001B35AC
	private void OnInit(GameObject go)
	{
		ToiletWorkableUse component = go.GetComponent<ToiletWorkableUse>();
		HashedString[] value = new HashedString[]
		{
			"working_pst"
		};
		KAnimFile[] value2 = new KAnimFile[]
		{
			Assets.GetAnim("anim_interacts_toilet_wall_kanim")
		};
		component.workerTypeOverrideAnims.Add(MinionConfig.ID, value2);
		component.workerTypeOverrideAnims.Add(BionicMinionConfig.ID, new KAnimFile[]
		{
			Assets.GetAnim("anim_bionic_interacts_toilet_wall_kanim")
		});
		component.workerTypePstAnims.Add(MinionConfig.ID, value);
		component.workerTypePstAnims.Add(BionicMinionConfig.ID, new HashedString[]
		{
			"working_gunky_pst"
		});
	}

	// Token: 0x06001ADC RID: 6876 RVA: 0x000AA038 File Offset: 0x000A8238
	public override void DoPostConfigureComplete(GameObject go)
	{
	}

	// Token: 0x04001147 RID: 4423
	private const float WATER_USAGE = 2.5f;

	// Token: 0x04001148 RID: 4424
	public const string ID = "WallToilet";
}
