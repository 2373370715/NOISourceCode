using System;
using TUNING;
using UnityEngine;

// Token: 0x02000598 RID: 1432
public class ShowerConfig : IBuildingConfig
{
	// Token: 0x060018B9 RID: 6329 RVA: 0x001AC110 File Offset: 0x001AA310
	public override BuildingDef CreateBuildingDef()
	{
		string id = ShowerConfig.ID;
		int width = 2;
		int height = 4;
		string anim = "shower_kanim";
		int hitpoints = 30;
		float construction_time = 30f;
		float[] tier = BUILDINGS.CONSTRUCTION_MASS_KG.TIER4;
		string[] raw_METALS = MATERIALS.RAW_METALS;
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER3;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, raw_METALS, melting_point, build_location_rule, BUILDINGS.DECOR.BONUS.TIER1, tier2, 0.2f);
		buildingDef.Overheatable = false;
		buildingDef.ExhaustKilowattsWhenActive = 0.25f;
		buildingDef.InputConduitType = ConduitType.Liquid;
		buildingDef.OutputConduitType = ConduitType.Liquid;
		buildingDef.ViewMode = OverlayModes.LiquidConduits.ID;
		buildingDef.AudioCategory = "Metal";
		buildingDef.UtilityInputOffset = new CellOffset(0, 0);
		buildingDef.UtilityOutputOffset = new CellOffset(1, 1);
		return buildingDef;
	}

	// Token: 0x060018BA RID: 6330 RVA: 0x001AC1A8 File Offset: 0x001AA3A8
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.AddOrGet<LoopingSounds>();
		go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.WashStation, false);
		go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.AdvancedWashStation, false);
		Shower shower = go.AddOrGet<Shower>();
		shower.overrideAnims = new KAnimFile[]
		{
			Assets.GetAnim("anim_interacts_shower_kanim")
		};
		shower.workTime = 15f;
		shower.outputTargetElement = SimHashes.DirtyWater;
		shower.fractionalDiseaseRemoval = 0.95f;
		shower.absoluteDiseaseRemoval = -2000;
		shower.workLayer = Grid.SceneLayer.BuildingFront;
		shower.trackUses = true;
		ConduitConsumer conduitConsumer = go.AddOrGet<ConduitConsumer>();
		conduitConsumer.conduitType = ConduitType.Liquid;
		conduitConsumer.capacityTag = ElementLoader.FindElementByHash(SimHashes.Water).tag;
		conduitConsumer.wrongElementResult = ConduitConsumer.WrongElementResult.Store;
		conduitConsumer.capacityKG = 5f;
		ConduitDispenser conduitDispenser = go.AddOrGet<ConduitDispenser>();
		conduitDispenser.conduitType = ConduitType.Liquid;
		conduitDispenser.invertElementFilter = true;
		conduitDispenser.elementFilter = new SimHashes[]
		{
			SimHashes.Water
		};
		ElementConverter elementConverter = go.AddOrGet<ElementConverter>();
		elementConverter.consumedElements = new ElementConverter.ConsumedElement[]
		{
			new ElementConverter.ConsumedElement(new Tag("Water"), 1f, true)
		};
		elementConverter.outputElements = new ElementConverter.OutputElement[]
		{
			new ElementConverter.OutputElement(1f, SimHashes.DirtyWater, 0f, false, true, 0f, 0.5f, 1f, byte.MaxValue, 0, true)
		};
		Storage storage = go.AddOrGet<Storage>();
		storage.capacityKg = 10f;
		storage.SetDefaultStoredItemModifiers(Storage.StandardSealedStorage);
		go.AddOrGet<RequireOutputs>().ignoreFullPipe = true;
		go.AddOrGetDef<RocketUsageRestriction.Def>();
	}

	// Token: 0x060018BB RID: 6331 RVA: 0x000AA038 File Offset: 0x000A8238
	public override void DoPostConfigureComplete(GameObject go)
	{
	}

	// Token: 0x04001029 RID: 4137
	public static string ID = "Shower";
}
