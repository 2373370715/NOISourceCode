using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000415 RID: 1045
public class IntermediateCureConfig : IEntityConfig
{
	// Token: 0x06001155 RID: 4437 RVA: 0x0018DDFC File Offset: 0x0018BFFC
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateLooseEntity("IntermediateCure", STRINGS.ITEMS.PILLS.INTERMEDIATECURE.NAME, STRINGS.ITEMS.PILLS.INTERMEDIATECURE.DESC, 1f, true, Assets.GetAnim("iv_slimelung_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.8f, 0.4f, true, 0, SimHashes.Creature, null);
		gameObject = EntityTemplates.ExtendEntityToMedicine(gameObject, MEDICINE.INTERMEDIATECURE);
		ComplexRecipe.RecipeElement[] array = new ComplexRecipe.RecipeElement[]
		{
			new ComplexRecipe.RecipeElement(SwampLilyFlowerConfig.ID, 1f),
			new ComplexRecipe.RecipeElement(SimHashes.Phosphorite.CreateTag(), 1f)
		};
		ComplexRecipe.RecipeElement[] array2 = new ComplexRecipe.RecipeElement[]
		{
			new ComplexRecipe.RecipeElement("IntermediateCure", 1f)
		};
		string text = "Apothecary";
		IntermediateCureConfig.recipe = new ComplexRecipe(ComplexRecipeManager.MakeRecipeID(text, array, array2), array, array2)
		{
			time = 100f,
			description = STRINGS.ITEMS.PILLS.INTERMEDIATECURE.RECIPEDESC,
			nameDisplay = ComplexRecipe.RecipeNameDisplay.Result,
			fabricators = new List<Tag>
			{
				text
			},
			sortOrder = 10,
			requiredTech = "MedicineII"
		};
		return gameObject;
	}

	// Token: 0x06001156 RID: 4438 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06001157 RID: 4439 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000C03 RID: 3075
	public const string ID = "IntermediateCure";

	// Token: 0x04000C04 RID: 3076
	public static ComplexRecipe recipe;
}
