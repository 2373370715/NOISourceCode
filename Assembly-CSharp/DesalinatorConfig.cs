using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000D57 RID: 3415
public class DesalinatorConfig : IBuildingConfig
{
	// Token: 0x06004248 RID: 16968 RVA: 0x0024E9C4 File Offset: 0x0024CBC4
	public override BuildingDef CreateBuildingDef()
	{
		string id = "Desalinator";
		int width = 4;
		int height = 3;
		string anim = "desalinator_kanim";
		int hitpoints = 30;
		float construction_time = 10f;
		float[] tier = TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER3;
		string[] raw_METALS = MATERIALS.RAW_METALS;
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER1;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, raw_METALS, melting_point, build_location_rule, TUNING.BUILDINGS.DECOR.PENALTY.TIER0, tier2, 0.2f);
		buildingDef.RequiresPowerInput = true;
		buildingDef.EnergyConsumptionWhenActive = 480f;
		buildingDef.SelfHeatKilowattsWhenActive = 8f;
		buildingDef.ExhaustKilowattsWhenActive = 0f;
		buildingDef.InputConduitType = ConduitType.Liquid;
		buildingDef.OutputConduitType = ConduitType.Liquid;
		buildingDef.Floodable = false;
		buildingDef.ViewMode = OverlayModes.LiquidConduits.ID;
		buildingDef.AudioCategory = "Metal";
		buildingDef.UtilityInputOffset = new CellOffset(-1, 0);
		buildingDef.UtilityOutputOffset = new CellOffset(0, 0);
		buildingDef.PermittedRotations = PermittedRotations.FlipH;
		buildingDef.AddSearchTerms(SEARCH_TERMS.FILTER);
		return buildingDef;
	}

	// Token: 0x06004249 RID: 16969 RVA: 0x0024EA90 File Offset: 0x0024CC90
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.IndustrialMachinery, false);
		Storage storage = go.AddOrGet<Storage>();
		storage.SetDefaultStoredItemModifiers(Storage.StandardSealedStorage);
		storage.showInUI = true;
		go.AddOrGet<Desalinator>().maxSalt = 945f;
		ElementConverter elementConverter = go.AddComponent<ElementConverter>();
		elementConverter.consumedElements = new ElementConverter.ConsumedElement[]
		{
			new ElementConverter.ConsumedElement(new Tag("SaltWater"), 5f, true)
		};
		elementConverter.outputElements = new ElementConverter.OutputElement[]
		{
			new ElementConverter.OutputElement(4.65f, SimHashes.Water, 0f, false, true, 0f, 0.5f, 0.75f, byte.MaxValue, 0, true),
			new ElementConverter.OutputElement(0.35f, SimHashes.Salt, 0f, false, true, 0f, 0.5f, 0.25f, byte.MaxValue, 0, true)
		};
		ElementConverter elementConverter2 = go.AddComponent<ElementConverter>();
		elementConverter2.consumedElements = new ElementConverter.ConsumedElement[]
		{
			new ElementConverter.ConsumedElement(new Tag("Brine"), 5f, true)
		};
		elementConverter2.outputElements = new ElementConverter.OutputElement[]
		{
			new ElementConverter.OutputElement(3.5f, SimHashes.Water, 0f, false, true, 0f, 0.5f, 0.75f, byte.MaxValue, 0, true),
			new ElementConverter.OutputElement(1.5f, SimHashes.Salt, 0f, false, true, 0f, 0.5f, 0.25f, byte.MaxValue, 0, true)
		};
		DesalinatorWorkableEmpty desalinatorWorkableEmpty = go.AddOrGet<DesalinatorWorkableEmpty>();
		desalinatorWorkableEmpty.workTime = 90f;
		desalinatorWorkableEmpty.workLayer = Grid.SceneLayer.BuildingFront;
		ConduitConsumer conduitConsumer = go.AddOrGet<ConduitConsumer>();
		conduitConsumer.conduitType = ConduitType.Liquid;
		conduitConsumer.consumptionRate = 10f;
		conduitConsumer.capacityKG = 20f;
		conduitConsumer.capacityTag = GameTags.AnyWater;
		conduitConsumer.forceAlwaysSatisfied = true;
		conduitConsumer.wrongElementResult = ConduitConsumer.WrongElementResult.Store;
		ConduitDispenser conduitDispenser = go.AddOrGet<ConduitDispenser>();
		conduitDispenser.conduitType = ConduitType.Liquid;
		conduitDispenser.invertElementFilter = true;
		conduitDispenser.elementFilter = new SimHashes[]
		{
			SimHashes.SaltWater,
			SimHashes.Brine
		};
		Prioritizable.AddRef(go);
	}

	// Token: 0x0600424A RID: 16970 RVA: 0x000CF564 File Offset: 0x000CD764
	public override void DoPostConfigurePreview(BuildingDef def, GameObject go)
	{
		base.DoPostConfigurePreview(def, go);
	}

	// Token: 0x0600424B RID: 16971 RVA: 0x000CF56E File Offset: 0x000CD76E
	public override void DoPostConfigureUnderConstruction(GameObject go)
	{
		base.DoPostConfigureUnderConstruction(go);
	}

	// Token: 0x0600424C RID: 16972 RVA: 0x000B501F File Offset: 0x000B321F
	public override void DoPostConfigureComplete(GameObject go)
	{
		go.AddOrGetDef<PoweredActiveController.Def>().showWorkingStatus = true;
	}

	// Token: 0x04002DBB RID: 11707
	public const string ID = "Desalinator";

	// Token: 0x04002DBC RID: 11708
	private const ConduitType CONDUIT_TYPE = ConduitType.Liquid;

	// Token: 0x04002DBD RID: 11709
	private const float INPUT_RATE = 5f;

	// Token: 0x04002DBE RID: 11710
	private const float SALT_WATER_TO_SALT_OUTPUT_RATE = 0.35f;

	// Token: 0x04002DBF RID: 11711
	private const float SALT_WATER_TO_CLEAN_WATER_OUTPUT_RATE = 4.65f;

	// Token: 0x04002DC0 RID: 11712
	private const float BRINE_TO_SALT_OUTPUT_RATE = 1.5f;

	// Token: 0x04002DC1 RID: 11713
	private const float BRINE_TO_CLEAN_WATER_OUTPUT_RATE = 3.5f;
}
