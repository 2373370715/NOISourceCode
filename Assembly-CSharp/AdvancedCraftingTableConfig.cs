using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000016 RID: 22
public class AdvancedCraftingTableConfig : IBuildingConfig
{
	// Token: 0x06000053 RID: 83 RVA: 0x000AA12F File Offset: 0x000A832F
	public override string[] GetRequiredDlcIds()
	{
		return DlcManager.DLC3;
	}

	// Token: 0x06000054 RID: 84 RVA: 0x00147324 File Offset: 0x00145524
	public override BuildingDef CreateBuildingDef()
	{
		string id = "AdvancedCraftingTable";
		int width = 3;
		int height = 3;
		string anim = "advanced_crafting_table_kanim";
		int hitpoints = 100;
		float construction_time = 30f;
		float[] tier = TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER3;
		string[] raw_METALS = MATERIALS.RAW_METALS;
		float melting_point = 800f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER3;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, raw_METALS, melting_point, build_location_rule, TUNING.BUILDINGS.DECOR.NONE, tier2, 0.2f);
		buildingDef.RequiresPowerInput = true;
		buildingDef.EnergyConsumptionWhenActive = 480f;
		buildingDef.ViewMode = OverlayModes.Power.ID;
		buildingDef.AudioCategory = "Metal";
		buildingDef.PowerInputOffset = new CellOffset(0, 0);
		buildingDef.RequiredSkillPerkID = Db.Get().SkillPerks.CanCraftElectronics.Id;
		buildingDef.AddSearchTerms(SEARCH_TERMS.ROBOT);
		return buildingDef;
	}

	// Token: 0x06000055 RID: 85 RVA: 0x001473CC File Offset: 0x001455CC
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.AddOrGet<DropAllWorkable>();
		go.AddOrGet<Prioritizable>();
		ComplexFabricator complexFabricator = go.AddOrGet<ComplexFabricator>();
		complexFabricator.heatedTemperature = 318.15f;
		complexFabricator.sideScreenStyle = ComplexFabricatorSideScreen.StyleSetting.ListQueueHybrid;
		go.AddOrGet<FabricatorIngredientStatusManager>();
		go.AddOrGet<CopyBuildingSettings>();
		go.AddOrGet<BuildingComplete>().isManuallyOperated = true;
		go.AddOrGet<ComplexFabricatorWorkable>().overrideAnims = new KAnimFile[]
		{
			Assets.GetAnim("anim_interacts_advanced_crafting_table_kanim")
		};
		Prioritizable.AddRef(go);
		BuildingTemplates.CreateComplexFabricatorStorage(go, complexFabricator);
		this.ConfigureRecipes();
	}

	// Token: 0x06000056 RID: 86 RVA: 0x00147450 File Offset: 0x00145650
	private void ConfigureRecipes()
	{
		ComplexRecipe.RecipeElement[] array = new ComplexRecipe.RecipeElement[]
		{
			new ComplexRecipe.RecipeElement(SimHashes.Katairite.CreateTag(), 200f, true)
		};
		ComplexRecipe.RecipeElement[] array2 = new ComplexRecipe.RecipeElement[]
		{
			new ComplexRecipe.RecipeElement("EmptyElectrobank".ToTag(), 1f, ComplexRecipe.RecipeElement.TemperatureOperation.AverageTemperature, false)
		};
		ElectrobankConfig.recipe = new ComplexRecipe(ComplexRecipeManager.MakeRecipeID("AdvancedCraftingTable", array, array2), array, array2)
		{
			time = INDUSTRIAL.RECIPES.STANDARD_FABRICATION_TIME * 2f,
			description = string.Format(STRINGS.BUILDINGS.PREFABS.ADVANCEDCRAFTINGTABLE.GENERIC_RECIPE_DESCRIPTION, ElementLoader.FindElementByHash(SimHashes.Katairite).name, STRINGS.ITEMS.INDUSTRIAL_PRODUCTS.ELECTROBANK.NAME),
			nameDisplay = ComplexRecipe.RecipeNameDisplay.Custom,
			customName = STRINGS.ITEMS.INDUSTRIAL_PRODUCTS.ELECTROBANK.NAME,
			customSpritePrefabID = "Electrobank",
			fabricators = new List<Tag>
			{
				"AdvancedCraftingTable"
			},
			requiredTech = Db.Get().TechItems.electrobank.parentTechId,
			sortOrder = 0
		};
		ComplexRecipe.RecipeElement[] array3 = new ComplexRecipe.RecipeElement[]
		{
			new ComplexRecipe.RecipeElement(SimHashes.Polypropylene.CreateTag(), 200f, true)
		};
		ComplexRecipe.RecipeElement[] array4 = new ComplexRecipe.RecipeElement[]
		{
			new ComplexRecipe.RecipeElement("FetchDrone".ToTag(), 1f, ComplexRecipe.RecipeElement.TemperatureOperation.AverageTemperature, false)
		};
		ElectrobankConfig.recipe = new ComplexRecipe(ComplexRecipeManager.MakeRecipeID("AdvancedCraftingTable", array3, array4), array3, array4)
		{
			time = INDUSTRIAL.RECIPES.STANDARD_FABRICATION_TIME * 4f,
			description = string.Format(STRINGS.BUILDINGS.PREFABS.ADVANCEDCRAFTINGTABLE.GENERIC_RECIPE_DESCRIPTION, ElementLoader.FindElementByHash(SimHashes.Polypropylene).name, STRINGS.ROBOTS.MODELS.FLYDO.NAME),
			nameDisplay = ComplexRecipe.RecipeNameDisplay.ResultWithIngredient,
			fabricators = new List<Tag>
			{
				"AdvancedCraftingTable"
			},
			requiredTech = Db.Get().TechItems.fetchDrone.parentTechId,
			sortOrder = 1
		};
		ComplexRecipe.RecipeElement[] array5 = new ComplexRecipe.RecipeElement[]
		{
			new ComplexRecipe.RecipeElement(SimHashes.HardPolypropylene.CreateTag(), 200f, true)
		};
		ComplexRecipe.RecipeElement[] array6 = new ComplexRecipe.RecipeElement[]
		{
			new ComplexRecipe.RecipeElement("FetchDrone".ToTag(), 1f, ComplexRecipe.RecipeElement.TemperatureOperation.AverageTemperature, false)
		};
		ElectrobankConfig.recipe = new ComplexRecipe(ComplexRecipeManager.MakeRecipeID("AdvancedCraftingTable", array5, array6), array5, array6)
		{
			time = INDUSTRIAL.RECIPES.STANDARD_FABRICATION_TIME * 4f,
			description = string.Format(STRINGS.BUILDINGS.PREFABS.ADVANCEDCRAFTINGTABLE.GENERIC_RECIPE_DESCRIPTION, ElementLoader.FindElementByHash(SimHashes.HardPolypropylene).name, STRINGS.ROBOTS.MODELS.FLYDO.NAME),
			nameDisplay = ComplexRecipe.RecipeNameDisplay.ResultWithIngredient,
			fabricators = new List<Tag>
			{
				"AdvancedCraftingTable"
			},
			requiredTech = Db.Get().TechItems.fetchDrone.parentTechId,
			sortOrder = 2
		};
	}

	// Token: 0x06000057 RID: 87 RVA: 0x000AA136 File Offset: 0x000A8336
	public override void DoPostConfigureComplete(GameObject go)
	{
		go.GetComponent<KPrefabID>().prefabSpawnFn += delegate(GameObject game_object)
		{
			ComplexFabricatorWorkable component = game_object.GetComponent<ComplexFabricatorWorkable>();
			component.WorkerStatusItem = Db.Get().DuplicantStatusItems.Fabricating;
			component.AttributeConverter = Db.Get().AttributeConverters.MachinerySpeed;
			component.AttributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.PART_DAY_EXPERIENCE;
			component.SkillExperienceSkillGroup = Db.Get().SkillGroups.Technicals.Id;
			component.SkillExperienceMultiplier = SKILLS.PART_DAY_EXPERIENCE;
			component.requiredSkillPerk = Db.Get().SkillPerks.CanCraftElectronics.Id;
		};
	}

	// Token: 0x04000044 RID: 68
	public const string ID = "AdvancedCraftingTable";
}
