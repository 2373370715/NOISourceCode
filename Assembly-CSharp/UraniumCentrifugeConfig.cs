using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020005EF RID: 1519
public class UraniumCentrifugeConfig : IBuildingConfig
{
	// Token: 0x06001A9F RID: 6815 RVA: 0x000AA117 File Offset: 0x000A8317
	public override string[] GetRequiredDlcIds()
	{
		return DlcManager.EXPANSION1;
	}

	// Token: 0x06001AA0 RID: 6816 RVA: 0x001B46A0 File Offset: 0x001B28A0
	public override BuildingDef CreateBuildingDef()
	{
		string id = "UraniumCentrifuge";
		int width = 3;
		int height = 4;
		string anim = "enrichmentCentrifuge_kanim";
		int hitpoints = 100;
		float construction_time = 480f;
		string[] array = new string[]
		{
			"RefinedMetal",
			"Plastic"
		};
		float[] construction_mass = new float[]
		{
			TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER5[0],
			TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER2[0]
		};
		string[] construction_materials = array;
		float melting_point = 2400f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
		EffectorValues tier = NOISE_POLLUTION.NOISY.TIER5;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, construction_mass, construction_materials, melting_point, build_location_rule, TUNING.BUILDINGS.DECOR.PENALTY.TIER1, tier, 0.2f);
		buildingDef.Overheatable = false;
		buildingDef.RequiresPowerInput = true;
		buildingDef.PowerInputOffset = new CellOffset(0, 0);
		buildingDef.EnergyConsumptionWhenActive = 480f;
		buildingDef.ExhaustKilowattsWhenActive = 0.125f;
		buildingDef.SelfHeatKilowattsWhenActive = 0.5f;
		buildingDef.AudioCategory = "HollowMetal";
		buildingDef.OutputConduitType = ConduitType.Liquid;
		buildingDef.UtilityOutputOffset = UraniumCentrifugeConfig.outPipeOffset;
		buildingDef.LogicInputPorts = LogicOperationalController.CreateSingleInputPortList(new CellOffset(0, 0));
		buildingDef.Deprecated = !Sim.IsRadiationEnabled();
		return buildingDef;
	}

	// Token: 0x06001AA1 RID: 6817 RVA: 0x001B4788 File Offset: 0x001B2988
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.IndustrialMachinery, false);
		UraniumCentrifuge uraniumCentrifuge = go.AddOrGet<UraniumCentrifuge>();
		BuildingTemplates.CreateComplexFabricatorStorage(go, uraniumCentrifuge);
		uraniumCentrifuge.outStorage.capacityKg = 2000f;
		uraniumCentrifuge.storeProduced = true;
		uraniumCentrifuge.sideScreenStyle = ComplexFabricatorSideScreen.StyleSetting.ListQueueHybrid;
		go.AddOrGet<FabricatorIngredientStatusManager>();
		uraniumCentrifuge.duplicantOperated = false;
		uraniumCentrifuge.inStorage.SetDefaultStoredItemModifiers(UraniumCentrifugeConfig.storedItemModifiers);
		uraniumCentrifuge.buildStorage.SetDefaultStoredItemModifiers(UraniumCentrifugeConfig.storedItemModifiers);
		uraniumCentrifuge.outStorage.SetDefaultStoredItemModifiers(UraniumCentrifugeConfig.storedItemModifiers);
		ConduitDispenser conduitDispenser = go.AddOrGet<ConduitDispenser>();
		conduitDispenser.alwaysDispense = true;
		conduitDispenser.conduitType = ConduitType.Liquid;
		conduitDispenser.storage = uraniumCentrifuge.outStorage;
		ComplexRecipe.RecipeElement[] array = new ComplexRecipe.RecipeElement[]
		{
			new ComplexRecipe.RecipeElement(ElementLoader.FindElementByHash(SimHashes.UraniumOre).tag, 10f)
		};
		ComplexRecipe.RecipeElement[] array2 = new ComplexRecipe.RecipeElement[]
		{
			new ComplexRecipe.RecipeElement(ElementLoader.FindElementByHash(SimHashes.EnrichedUranium).tag, 2f, ComplexRecipe.RecipeElement.TemperatureOperation.AverageTemperature, false),
			new ComplexRecipe.RecipeElement(ElementLoader.FindElementByHash(SimHashes.MoltenUranium).tag, 8f, ComplexRecipe.RecipeElement.TemperatureOperation.Melted, false)
		};
		ComplexRecipe complexRecipe = new ComplexRecipe(ComplexRecipeManager.MakeRecipeID("UraniumCentrifuge", array, array2), array, array2);
		complexRecipe.time = 40f;
		complexRecipe.nameDisplay = ComplexRecipe.RecipeNameDisplay.Result;
		complexRecipe.description = STRINGS.BUILDINGS.PREFABS.URANIUMCENTRIFUGE.RECIPE_DESCRIPTION;
		complexRecipe.fabricators = new List<Tag>
		{
			TagManager.Create("UraniumCentrifuge")
		};
		Prioritizable.AddRef(go);
	}

	// Token: 0x06001AA2 RID: 6818 RVA: 0x000AA1AD File Offset: 0x000A83AD
	public override void DoPostConfigureComplete(GameObject go)
	{
		go.AddOrGet<LogicOperationalController>();
		go.AddOrGetDef<PoweredActiveController.Def>();
	}

	// Token: 0x0400112C RID: 4396
	public const string ID = "UraniumCentrifuge";

	// Token: 0x0400112D RID: 4397
	public const float OUTPUT_TEMP = 1173.15f;

	// Token: 0x0400112E RID: 4398
	public const float REFILL_RATE = 2400f;

	// Token: 0x0400112F RID: 4399
	public static readonly CellOffset outPipeOffset = new CellOffset(1, 3);

	// Token: 0x04001130 RID: 4400
	private static readonly List<Storage.StoredItemModifier> storedItemModifiers = new List<Storage.StoredItemModifier>
	{
		Storage.StoredItemModifier.Hide,
		Storage.StoredItemModifier.Preserve,
		Storage.StoredItemModifier.Insulate
	};
}
