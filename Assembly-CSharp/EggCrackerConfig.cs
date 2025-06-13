using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

[EntityConfigOrder(2)]
public class EggCrackerConfig : IBuildingConfig
{
	public static void RegisterEgg(Tag eggPrefabTag, string name, string description, float mass, string[] requiredDLC, string[] forbiddenDLC)
	{
		EggCrackerConfig.EggData item = new EggCrackerConfig.EggData(eggPrefabTag, name, description, mass, requiredDLC, forbiddenDLC);
		EggCrackerConfig.uncategorizedEggData.Add(item);
	}

	public static void CategorizeEggs()
	{
		foreach (EggCrackerConfig.EggData eggData in EggCrackerConfig.uncategorizedEggData)
		{
			Tag species = Assets.GetPrefab(Assets.GetPrefab(eggData.id).GetDef<IncubationMonitor.Def>().spawnedCreature).GetComponent<CreatureBrain>().species;
			if (!EggCrackerConfig.EggsBySpecies.ContainsKey(species))
			{
				EggCrackerConfig.EggsBySpecies.Add(species, new List<EggCrackerConfig.EggData>());
			}
			EggCrackerConfig.EggsBySpecies[species].Add(eggData);
		}
	}

	public override BuildingDef CreateBuildingDef()
	{
		string id = "EggCracker";
		int width = 2;
		int height = 2;
		string anim = "egg_cracker_kanim";
		int hitpoints = 30;
		float construction_time = 10f;
		float[] tier = TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER1;
		string[] raw_METALS = MATERIALS.RAW_METALS;
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
		EffectorValues none = NOISE_POLLUTION.NONE;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, raw_METALS, melting_point, build_location_rule, TUNING.BUILDINGS.DECOR.BONUS.TIER0, none, 0.2f);
		buildingDef.AudioCategory = "Metal";
		buildingDef.SceneLayer = Grid.SceneLayer.Building;
		buildingDef.ForegroundLayer = Grid.SceneLayer.BuildingFront;
		buildingDef.LogicInputPorts = LogicOperationalController.CreateSingleInputPortList(new CellOffset(0, 0));
		buildingDef.AddSearchTerms(SEARCH_TERMS.FOOD);
		return buildingDef;
	}

	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.AddOrGet<DropAllWorkable>();
		go.AddOrGet<BuildingComplete>().isManuallyOperated = true;
		go.AddOrGet<KBatchedAnimController>().SetSymbolVisiblity("snapto_egg", false);
		ComplexFabricator complexFabricator = go.AddOrGet<ComplexFabricator>();
		complexFabricator.labelByResult = false;
		complexFabricator.sideScreenStyle = ComplexFabricatorSideScreen.StyleSetting.ListQueueHybrid;
		complexFabricator.duplicantOperated = true;
		go.AddOrGet<FabricatorIngredientStatusManager>();
		go.AddOrGet<CopyBuildingSettings>();
		Workable workable = go.AddOrGet<ComplexFabricatorWorkable>();
		BuildingTemplates.CreateComplexFabricatorStorage(go, complexFabricator);
		workable.overrideAnims = new KAnimFile[]
		{
			Assets.GetAnim("anim_interacts_egg_cracker_kanim")
		};
		complexFabricator.outputOffset = new Vector3(1f, 1f, 0f);
		Prioritizable.AddRef(go);
		go.AddOrGet<EggCracker>();
	}

	public override void DoPostConfigureComplete(GameObject go)
	{
		go.AddOrGet<LogicOperationalController>();
	}

	public override void ConfigurePost(BuildingDef def)
	{
		base.ConfigurePost(def);
		this.MakeRecipes();
	}

	public void MakeRecipes()
	{
		EggCrackerConfig.CategorizeEggs();
		foreach (KeyValuePair<Tag, List<EggCrackerConfig.EggData>> keyValuePair in EggCrackerConfig.EggsBySpecies)
		{
			Tag[] array = new Tag[keyValuePair.Value.Count];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = keyValuePair.Value[i].id;
			}
			EggCrackerConfig.EggData eggData = keyValuePair.Value[0];
			string arg = string.Format(STRINGS.BUILDINGS.PREFABS.EGGCRACKER.RESULT_DESCRIPTION, eggData.name);
			ComplexRecipe.RecipeElement[] array2 = new ComplexRecipe.RecipeElement[]
			{
				new ComplexRecipe.RecipeElement(array, 1f)
				{
					material = array[0]
				}
			};
			ComplexRecipe.RecipeElement[] array3 = new ComplexRecipe.RecipeElement[]
			{
				new ComplexRecipe.RecipeElement("RawEgg", 0.5f * eggData.mass, ComplexRecipe.RecipeElement.TemperatureOperation.AverageTemperature, false),
				new ComplexRecipe.RecipeElement("EggShell", 0.5f * eggData.mass, ComplexRecipe.RecipeElement.TemperatureOperation.AverageTemperature, false)
			};
			string obsolete_id = ComplexRecipeManager.MakeObsoleteRecipeID("EggCracker", "RawEgg");
			string text = ComplexRecipeManager.MakeRecipeID("EggCracker", array2, array3);
			ComplexRecipe complexRecipe = new ComplexRecipe(text, array2, array3, eggData.requiredDlcIds, eggData.forbiddenDlcIds);
			complexRecipe.description = string.Format(STRINGS.BUILDINGS.PREFABS.EGGCRACKER.RECIPE_DESCRIPTION, eggData.name, arg);
			complexRecipe.fabricators = new List<Tag>
			{
				"EggCracker"
			};
			complexRecipe.time = 5f;
			complexRecipe.nameDisplay = ComplexRecipe.RecipeNameDisplay.Custom;
			complexRecipe.customName = keyValuePair.Key.ProperName();
			complexRecipe.customSpritePrefabID = ((array2[0].material != null) ? array2[0].material.Name : array2[0].possibleMaterials[0].Name);
			ComplexRecipeManager.Get().AddObsoleteIDMapping(obsolete_id, text);
		}
	}

	public const string ID = "EggCracker";

	private static Dictionary<Tag, List<EggCrackerConfig.EggData>> EggsBySpecies = new Dictionary<Tag, List<EggCrackerConfig.EggData>>();

	private static List<EggCrackerConfig.EggData> uncategorizedEggData = new List<EggCrackerConfig.EggData>();

	private class EggData : IHasDlcRestrictions
	{
		public EggData(Tag id, string name, string description, float mass, string[] requiredDLC, string[] forbiddenDLC)
		{
			this.id = id;
			this.name = name;
			this.description = description;
			this.mass = mass;
			this.requiredDlcIds = requiredDLC;
			this.forbiddenDlcIds = forbiddenDLC;
		}

		public string[] GetRequiredDlcIds()
		{
			return this.requiredDlcIds;
		}

		public string[] GetForbiddenDlcIds()
		{
			return this.forbiddenDlcIds;
		}

		public Tag id;

		public float mass;

		public string name;

		public string description;

		public string[] requiredDlcIds;

		public string[] forbiddenDlcIds;
	}
}
