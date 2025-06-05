using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020002F2 RID: 754
public class BasicPlantBarConfig : IEntityConfig
{
	// Token: 0x06000B9C RID: 2972 RVA: 0x001794A4 File Offset: 0x001776A4
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateLooseEntity("BasicPlantBar", STRINGS.ITEMS.FOOD.BASICPLANTBAR.NAME, STRINGS.ITEMS.FOOD.BASICPLANTBAR.DESC, 1f, false, Assets.GetAnim("liceloaf_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.8f, 0.4f, true, 0, SimHashes.Creature, null);
		gameObject = EntityTemplates.ExtendEntityToFood(gameObject, FOOD.FOOD_TYPES.BASICPLANTBAR);
		ComplexRecipeManager.Get().GetRecipe(BasicPlantBarConfig.recipe.id).FabricationVisualizer = MushBarConfig.CreateFabricationVisualizer(gameObject);
		return gameObject;
	}

	// Token: 0x06000B9D RID: 2973 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000B9E RID: 2974 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x0400090D RID: 2317
	public const string ID = "BasicPlantBar";

	// Token: 0x0400090E RID: 2318
	public static ComplexRecipe recipe;
}
