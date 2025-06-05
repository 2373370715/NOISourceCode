using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x0200059B RID: 1435
public class SludgePressConfig : IBuildingConfig
{
	// Token: 0x060018C8 RID: 6344 RVA: 0x000AA117 File Offset: 0x000A8317
	public override string[] GetRequiredDlcIds()
	{
		return DlcManager.EXPANSION1;
	}

	// Token: 0x060018C9 RID: 6345 RVA: 0x001AC3D8 File Offset: 0x001AA5D8
	public override BuildingDef CreateBuildingDef()
	{
		string id = "SludgePress";
		int width = 4;
		int height = 3;
		string anim = "sludge_press_kanim";
		int hitpoints = 100;
		float construction_time = 30f;
		float[] tier = TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER3;
		string[] all_MINERALS = MATERIALS.ALL_MINERALS;
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER5;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, all_MINERALS, melting_point, build_location_rule, TUNING.BUILDINGS.DECOR.PENALTY.TIER1, tier2, 0.2f);
		buildingDef.RequiresPowerInput = true;
		buildingDef.EnergyConsumptionWhenActive = 120f;
		buildingDef.SelfHeatKilowattsWhenActive = 4f;
		buildingDef.OutputConduitType = ConduitType.Liquid;
		buildingDef.UtilityOutputOffset = new CellOffset(1, 0);
		buildingDef.ViewMode = OverlayModes.Power.ID;
		buildingDef.AudioCategory = "HollowMetal";
		buildingDef.AudioSize = "large";
		return buildingDef;
	}

	// Token: 0x060018CA RID: 6346 RVA: 0x001AC470 File Offset: 0x001AA670
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.IndustrialMachinery, false);
		go.AddOrGet<DropAllWorkable>();
		go.AddOrGet<BuildingComplete>().isManuallyOperated = true;
		ComplexFabricator complexFabricator = go.AddOrGet<ComplexFabricator>();
		complexFabricator.sideScreenStyle = ComplexFabricatorSideScreen.StyleSetting.ListQueueHybrid;
		complexFabricator.duplicantOperated = true;
		go.AddOrGet<FabricatorIngredientStatusManager>();
		go.AddOrGet<CopyBuildingSettings>();
		ComplexFabricatorWorkable complexFabricatorWorkable = go.AddOrGet<ComplexFabricatorWorkable>();
		BuildingTemplates.CreateComplexFabricatorStorage(go, complexFabricator);
		complexFabricatorWorkable.overrideAnims = new KAnimFile[]
		{
			Assets.GetAnim("anim_interacts_sludge_press_kanim")
		};
		complexFabricatorWorkable.workingPstComplete = new HashedString[]
		{
			"working_pst_complete"
		};
		ConduitDispenser conduitDispenser = go.AddOrGet<ConduitDispenser>();
		conduitDispenser.conduitType = ConduitType.Liquid;
		conduitDispenser.alwaysDispense = true;
		conduitDispenser.elementFilter = null;
		conduitDispenser.storage = go.GetComponent<ComplexFabricator>().outStorage;
		this.AddRecipes(go);
		Prioritizable.AddRef(go);
	}

	// Token: 0x060018CB RID: 6347 RVA: 0x001AC548 File Offset: 0x001AA748
	private void AddRecipes(GameObject go)
	{
		float num = 150f;
		foreach (Element element in ElementLoader.elements.FindAll((Element e) => e.elementComposition != null))
		{
			ComplexRecipe.RecipeElement[] array = new ComplexRecipe.RecipeElement[]
			{
				new ComplexRecipe.RecipeElement(element.tag, num)
			};
			ComplexRecipe.RecipeElement[] array2 = new ComplexRecipe.RecipeElement[element.elementComposition.Length];
			for (int i = 0; i < element.elementComposition.Length; i++)
			{
				ElementLoader.ElementComposition elementComposition = element.elementComposition[i];
				Element element2 = ElementLoader.FindElementByName(elementComposition.elementID);
				bool isLiquid = element2.IsLiquid;
				array2[i] = new ComplexRecipe.RecipeElement(element2.tag, num * elementComposition.percentage, ComplexRecipe.RecipeElement.TemperatureOperation.AverageTemperature, isLiquid);
			}
			string obsolete_id = ComplexRecipeManager.MakeObsoleteRecipeID("SludgePress", element.tag);
			string text = ComplexRecipeManager.MakeRecipeID("SludgePress", array, array2);
			ComplexRecipe complexRecipe = new ComplexRecipe(text, array, array2);
			complexRecipe.time = 20f;
			complexRecipe.description = string.Format(STRINGS.BUILDINGS.PREFABS.SLUDGEPRESS.RECIPE_DESCRIPTION, element.name);
			complexRecipe.nameDisplay = ComplexRecipe.RecipeNameDisplay.Composite;
			complexRecipe.fabricators = new List<Tag>
			{
				TagManager.Create("SludgePress")
			};
			ComplexRecipeManager.Get().AddObsoleteIDMapping(obsolete_id, text);
		}
	}

	// Token: 0x060018CC RID: 6348 RVA: 0x000B4CAD File Offset: 0x000B2EAD
	public override void DoPostConfigureComplete(GameObject go)
	{
		SymbolOverrideControllerUtil.AddToPrefab(go);
		go.GetComponent<KPrefabID>().prefabSpawnFn += delegate(GameObject game_object)
		{
			ComplexFabricatorWorkable component = game_object.GetComponent<ComplexFabricatorWorkable>();
			component.WorkerStatusItem = Db.Get().DuplicantStatusItems.Processing;
			component.AttributeConverter = Db.Get().AttributeConverters.MachinerySpeed;
			component.AttributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.PART_DAY_EXPERIENCE;
			component.SkillExperienceSkillGroup = Db.Get().SkillGroups.Technicals.Id;
			component.SkillExperienceMultiplier = SKILLS.PART_DAY_EXPERIENCE;
		};
	}

	// Token: 0x04001033 RID: 4147
	public const string ID = "SludgePress";
}
