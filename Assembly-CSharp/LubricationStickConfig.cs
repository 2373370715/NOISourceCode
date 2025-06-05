using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x02000417 RID: 1047
public class LubricationStickConfig : IEntityConfig, IHasDlcRestrictions
{
	// Token: 0x0600115F RID: 4447 RVA: 0x000AA12F File Offset: 0x000A832F
	public string[] GetRequiredDlcIds()
	{
		return DlcManager.DLC3;
	}

	// Token: 0x06001160 RID: 4448 RVA: 0x000AA765 File Offset: 0x000A8965
	public string[] GetForbiddenDlcIds()
	{
		return null;
	}

	// Token: 0x06001161 RID: 4449 RVA: 0x0018E024 File Offset: 0x0018C224
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateLooseEntity("LubricationStick", ITEMS.LUBRICATIONSTICK.NAME, ITEMS.LUBRICATIONSTICK.DESC, this.MASS_PER_RECIPE, true, Assets.GetAnim("lubricant_applicator_kanim"), "idle1", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.4f, 1f, true, 0, SimHashes.Creature, null);
		gameObject.AddOrGet<EntitySplitter>();
		gameObject.AddTag(GameTags.MedicalSupplies);
		gameObject.AddTag(GameTags.SolidLubricant);
		ComplexRecipe.RecipeElement[] array = new ComplexRecipe.RecipeElement[]
		{
			new ComplexRecipe.RecipeElement(SimHashes.LiquidGunk.CreateTag(), GunkMonitor.GUNK_CAPACITY),
			new ComplexRecipe.RecipeElement(SimHashes.Water.CreateTag(), 200f)
		};
		ComplexRecipe.RecipeElement[] array2 = new ComplexRecipe.RecipeElement[]
		{
			new ComplexRecipe.RecipeElement("LubricationStick".ToTag(), 1f, ComplexRecipe.RecipeElement.TemperatureOperation.AverageTemperature, false),
			new ComplexRecipe.RecipeElement(SimHashes.DirtyWater.CreateTag(), 200f, ComplexRecipe.RecipeElement.TemperatureOperation.AverageTemperature, false)
		};
		LubricationStickConfig.recipe = new ComplexRecipe(ComplexRecipeManager.MakeRecipeID("Apothecary", array, array2), array, array2)
		{
			time = 100f,
			description = ITEMS.LUBRICATIONSTICK.RECIPEDESC,
			nameDisplay = ComplexRecipe.RecipeNameDisplay.Result,
			fabricators = new List<Tag>
			{
				"Apothecary"
			},
			sortOrder = 1,
			requiredTech = Db.Get().TechItems.lubricationStick.parentTechId
		};
		return gameObject;
	}

	// Token: 0x06001162 RID: 4450 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06001163 RID: 4451 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000C07 RID: 3079
	public const string ID = "LubricationStick";

	// Token: 0x04000C08 RID: 3080
	public static ComplexRecipe recipe;

	// Token: 0x04000C09 RID: 3081
	private const float WATER_MASS = 200f;

	// Token: 0x04000C0A RID: 3082
	private float MASS_PER_RECIPE = GunkMonitor.GUNK_CAPACITY;
}
