using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000416 RID: 1046
public class IntermediateRadPillConfig : IEntityConfig, IHasDlcRestrictions
{
	// Token: 0x06001159 RID: 4441 RVA: 0x000AA117 File Offset: 0x000A8317
	public string[] GetRequiredDlcIds()
	{
		return DlcManager.EXPANSION1;
	}

	// Token: 0x0600115A RID: 4442 RVA: 0x000AA765 File Offset: 0x000A8965
	public string[] GetForbiddenDlcIds()
	{
		return null;
	}

	// Token: 0x0600115B RID: 4443 RVA: 0x0018DF20 File Offset: 0x0018C120
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateLooseEntity("IntermediateRadPill", STRINGS.ITEMS.PILLS.INTERMEDIATERADPILL.NAME, STRINGS.ITEMS.PILLS.INTERMEDIATERADPILL.DESC, 1f, true, Assets.GetAnim("vial_radiation_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.8f, 0.4f, true, 0, SimHashes.Creature, null);
		EntityTemplates.ExtendEntityToMedicine(gameObject, MEDICINE.INTERMEDIATERADPILL);
		ComplexRecipe.RecipeElement[] array = new ComplexRecipe.RecipeElement[]
		{
			new ComplexRecipe.RecipeElement("Carbon", 1f)
		};
		ComplexRecipe.RecipeElement[] array2 = new ComplexRecipe.RecipeElement[]
		{
			new ComplexRecipe.RecipeElement("IntermediateRadPill".ToTag(), 1f, ComplexRecipe.RecipeElement.TemperatureOperation.AverageTemperature, false)
		};
		IntermediateRadPillConfig.recipe = new ComplexRecipe(ComplexRecipeManager.MakeRecipeID("AdvancedApothecary", array, array2), array, array2)
		{
			time = 50f,
			description = STRINGS.ITEMS.PILLS.INTERMEDIATERADPILL.RECIPEDESC,
			nameDisplay = ComplexRecipe.RecipeNameDisplay.Result,
			fabricators = new List<Tag>
			{
				"AdvancedApothecary"
			},
			sortOrder = 21
		};
		return gameObject;
	}

	// Token: 0x0600115C RID: 4444 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x0600115D RID: 4445 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000C05 RID: 3077
	public const string ID = "IntermediateRadPill";

	// Token: 0x04000C06 RID: 3078
	public static ComplexRecipe recipe;
}
