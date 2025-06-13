using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

public class SmokerConfig : IBuildingConfig
{
	public override string[] GetRequiredDlcIds()
	{
		return DlcManager.DLC4;
	}

	public override BuildingDef CreateBuildingDef()
	{
		string id = "Smoker";
		int width = 4;
		int height = 3;
		string anim = "smoker_kanim";
		int hitpoints = 30;
		float construction_time = 30f;
		float[] tier = TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER4;
		string[] all_METALS = MATERIALS.ALL_METALS;
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER3;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, all_METALS, melting_point, build_location_rule, TUNING.BUILDINGS.DECOR.NONE, tier2, 0.2f);
		buildingDef.AudioCategory = "Metal";
		buildingDef.AudioSize = "large";
		buildingDef.ExhaustKilowattsWhenActive = 1f;
		buildingDef.SelfHeatKilowattsWhenActive = 8f;
		buildingDef.OutputConduitType = ConduitType.Gas;
		buildingDef.UtilityOutputOffset = new CellOffset(1, 1);
		buildingDef.RequiredSkillPerkID = Db.Get().SkillPerks.CanGasRange.Id;
		buildingDef.AddSearchTerms(SEARCH_TERMS.FOOD);
		return buildingDef;
	}

	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.AddOrGet<DropAllWorkable>();
		go.AddOrGet<BuildingComplete>().isManuallyOperated = true;
		ComplexFabricator complexFabricator = go.AddOrGet<ComplexFabricator>();
		complexFabricator.heatedTemperature = 368.15f;
		complexFabricator.duplicantOperated = false;
		complexFabricator.sideScreenStyle = ComplexFabricatorSideScreen.StyleSetting.ListQueueHybrid;
		complexFabricator.showProgressBar = true;
		complexFabricator.storeProduced = true;
		go.AddOrGet<FabricatorIngredientStatusManager>();
		go.AddOrGet<CopyBuildingSettings>();
		Storage storage = go.AddComponent<Storage>();
		ManualDeliveryKG manualDeliveryKG = go.AddComponent<ManualDeliveryKG>();
		manualDeliveryKG.SetStorage(storage);
		manualDeliveryKG.choreTypeIDHash = Db.Get().ChoreTypes.MachineFetch.IdHash;
		manualDeliveryKG.RequestedItemTag = SimHashes.Peat.CreateTag();
		manualDeliveryKG.capacity = 240f;
		manualDeliveryKG.refillMass = 120f;
		ElementConverter elementConverter = go.AddOrGet<ElementConverter>();
		elementConverter.SetStorage(storage);
		elementConverter.outputElements = new ElementConverter.OutputElement[]
		{
			new ElementConverter.OutputElement(0.02f, SimHashes.CarbonDioxide, 348.15f, false, true, 0f, 2f, 1f, byte.MaxValue, 0, true)
		};
		elementConverter.OperationalRequirement = Operational.State.Active;
		ConduitDispenser conduitDispenser = go.AddOrGet<ConduitDispenser>();
		conduitDispenser.conduitType = ConduitType.Gas;
		conduitDispenser.alwaysDispense = true;
		conduitDispenser.elementFilter = null;
		conduitDispenser.storage = storage;
		BuildingTemplates.CreateComplexFabricatorStorage(go, complexFabricator);
		complexFabricator.inStorage.SetDefaultStoredItemModifiers(SmokerConfig.GourmetCookingStationStoredItemModifiers);
		complexFabricator.buildStorage.SetDefaultStoredItemModifiers(SmokerConfig.GourmetCookingStationStoredItemModifiers);
		complexFabricator.outStorage.SetDefaultStoredItemModifiers(SmokerConfig.GourmetCookingStationStoredItemModifiers);
		this.ConfigureRecipes();
		Prioritizable.AddRef(go);
		go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.CookTop, false);
		go.AddOrGetDef<FoodSmoker.Def>();
		KAnimFile[] overrideAnims = new KAnimFile[]
		{
			Assets.GetAnim("anim_interacts_smoker_kanim")
		};
		FoodSmokerWorkableEmpty foodSmokerWorkableEmpty = go.AddOrGet<FoodSmokerWorkableEmpty>();
		foodSmokerWorkableEmpty.workTime = 50f;
		foodSmokerWorkableEmpty.overrideAnims = overrideAnims;
		foodSmokerWorkableEmpty.workLayer = Grid.SceneLayer.Front;
	}

	public override void DoPostConfigureComplete(GameObject go)
	{
	}

	private void ConfigureRecipes()
	{
		ComplexRecipe.RecipeElement[] array = new ComplexRecipe.RecipeElement[]
		{
			new ComplexRecipe.RecipeElement("DinosaurMeat", 6f),
			new ComplexRecipe.RecipeElement(new Tag[]
			{
				SimHashes.WoodLog.CreateTag(),
				SimHashes.Peat.CreateTag()
			}, 100f, ComplexRecipe.RecipeElement.TemperatureOperation.AverageTemperature, "", false, false)
		};
		ComplexRecipe.RecipeElement[] array2 = new ComplexRecipe.RecipeElement[]
		{
			new ComplexRecipe.RecipeElement("SmokedDinosaurMeat", 3.2f, ComplexRecipe.RecipeElement.TemperatureOperation.Heated, false)
		};
		ComplexRecipe complexRecipe = new ComplexRecipe(ComplexRecipeManager.MakeRecipeID("Smoker", array, array2), array, array2);
		complexRecipe.time = 600f;
		complexRecipe.description = STRINGS.ITEMS.FOOD.SMOKEDDINOSAURMEAT.RECIPEDESC;
		complexRecipe.nameDisplay = ComplexRecipe.RecipeNameDisplay.Result;
		complexRecipe.fabricators = new List<Tag>
		{
			"Smoker"
		};
		complexRecipe.sortOrder = 600;
		ComplexRecipe.RecipeElement[] array3 = new ComplexRecipe.RecipeElement[]
		{
			new ComplexRecipe.RecipeElement(new Tag[]
			{
				"FishMeat",
				"PrehistoricPacuFillet"
			}, 6f),
			new ComplexRecipe.RecipeElement(new Tag[]
			{
				SimHashes.WoodLog.CreateTag(),
				SimHashes.Peat.CreateTag()
			}, 100f, ComplexRecipe.RecipeElement.TemperatureOperation.AverageTemperature, "", false, false)
		};
		ComplexRecipe.RecipeElement[] array4 = new ComplexRecipe.RecipeElement[]
		{
			new ComplexRecipe.RecipeElement("SmokedFish", 4f, ComplexRecipe.RecipeElement.TemperatureOperation.Heated, false)
		};
		ComplexRecipe complexRecipe2 = new ComplexRecipe(ComplexRecipeManager.MakeRecipeID("Smoker", array3, array4), array3, array4);
		complexRecipe2.time = 600f;
		complexRecipe2.description = STRINGS.ITEMS.FOOD.SMOKEDFISH.RECIPEDESC;
		complexRecipe2.nameDisplay = ComplexRecipe.RecipeNameDisplay.Result;
		complexRecipe2.fabricators = new List<Tag>
		{
			"Smoker"
		};
		complexRecipe2.sortOrder = 600;
		ComplexRecipe.RecipeElement[] array5 = new ComplexRecipe.RecipeElement[]
		{
			new ComplexRecipe.RecipeElement(new Tag[]
			{
				"GardenFoodPlantFood",
				"HardSkinBerry",
				"WormBasicFruit"
			}, 7f),
			new ComplexRecipe.RecipeElement(new Tag[]
			{
				SimHashes.WoodLog.CreateTag(),
				SimHashes.Peat.CreateTag()
			}, 100f, ComplexRecipe.RecipeElement.TemperatureOperation.AverageTemperature, "", false, false)
		};
		ComplexRecipe.RecipeElement[] array6 = new ComplexRecipe.RecipeElement[]
		{
			new ComplexRecipe.RecipeElement("SmokedVegetables", 4f, ComplexRecipe.RecipeElement.TemperatureOperation.Heated, false)
		};
		ComplexRecipe complexRecipe3 = new ComplexRecipe(ComplexRecipeManager.MakeRecipeID("Smoker", array5, array6), array5, array6);
		complexRecipe3.time = 600f;
		complexRecipe3.description = STRINGS.ITEMS.FOOD.SMOKEDVEGETABLES.RECIPEDESC;
		complexRecipe3.nameDisplay = ComplexRecipe.RecipeNameDisplay.Result;
		complexRecipe3.fabricators = new List<Tag>
		{
			"Smoker"
		};
		complexRecipe3.sortOrder = 600;
	}

	public const string ID = "Smoker";

	private const float FUEL_CONSUME_RATE = 0.2f;

	private const float CO2_EMIT_RATE = 0.02f;

	public const float EMPTYING_WORK_TIME = 50f;

	private static readonly List<Storage.StoredItemModifier> GourmetCookingStationStoredItemModifiers = new List<Storage.StoredItemModifier>
	{
		Storage.StoredItemModifier.Hide,
		Storage.StoredItemModifier.Preserve,
		Storage.StoredItemModifier.Insulate,
		Storage.StoredItemModifier.Seal
	};
}
