using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x0200030A RID: 778
public class FruitCakeConfig : IEntityConfig
{
	// Token: 0x06000C12 RID: 3090 RVA: 0x00179EC4 File Offset: 0x001780C4
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateLooseEntity("FruitCake", STRINGS.ITEMS.FOOD.FRUITCAKE.NAME, STRINGS.ITEMS.FOOD.FRUITCAKE.DESC, 1f, false, Assets.GetAnim("fruitcake_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.8f, 0.4f, true, 0, SimHashes.Creature, null);
		gameObject = EntityTemplates.ExtendEntityToFood(gameObject, FOOD.FOOD_TYPES.FRUITCAKE);
		ComplexRecipeManager.Get().GetRecipe(FruitCakeConfig.recipe.id).FabricationVisualizer = MushBarConfig.CreateFabricationVisualizer(gameObject);
		return gameObject;
	}

	// Token: 0x06000C13 RID: 3091 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000C14 RID: 3092 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x0400093F RID: 2367
	public const string ID = "FruitCake";

	// Token: 0x04000940 RID: 2368
	public static ComplexRecipe recipe;
}
