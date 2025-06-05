using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x0200040F RID: 1039
public class AdvancedCureConfig : IEntityConfig
{
	// Token: 0x0600113B RID: 4411 RVA: 0x0018D794 File Offset: 0x0018B994
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateLooseEntity("AdvancedCure", STRINGS.ITEMS.PILLS.ADVANCEDCURE.NAME, STRINGS.ITEMS.PILLS.ADVANCEDCURE.DESC, 1f, true, Assets.GetAnim("vial_spore_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.8f, 0.4f, true, 0, SimHashes.Creature, null);
		gameObject = EntityTemplates.ExtendEntityToMedicine(gameObject, MEDICINE.ADVANCEDCURE);
		ComplexRecipe.RecipeElement[] array = new ComplexRecipe.RecipeElement[]
		{
			new ComplexRecipe.RecipeElement(SimHashes.Steel.CreateTag(), 1f),
			new ComplexRecipe.RecipeElement("LightBugOrangeEgg", 1f)
		};
		ComplexRecipe.RecipeElement[] array2 = new ComplexRecipe.RecipeElement[]
		{
			new ComplexRecipe.RecipeElement("AdvancedCure", 1f, ComplexRecipe.RecipeElement.TemperatureOperation.AverageTemperature, false)
		};
		string text = "Apothecary";
		AdvancedCureConfig.recipe = new ComplexRecipe(ComplexRecipeManager.MakeRecipeID(text, array, array2), array, array2)
		{
			time = 200f,
			description = STRINGS.ITEMS.PILLS.ADVANCEDCURE.RECIPEDESC,
			nameDisplay = ComplexRecipe.RecipeNameDisplay.Result,
			fabricators = new List<Tag>
			{
				text
			},
			sortOrder = 20,
			requiredTech = "MedicineIV"
		};
		return gameObject;
	}

	// Token: 0x0600113C RID: 4412 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x0600113D RID: 4413 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000BF7 RID: 3063
	public const string ID = "AdvancedCure";

	// Token: 0x04000BF8 RID: 3064
	public static ComplexRecipe recipe;
}
