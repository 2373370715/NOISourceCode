using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000317 RID: 791
public class PickledMealConfig : IEntityConfig
{
	// Token: 0x06000C4B RID: 3147 RVA: 0x0017A558 File Offset: 0x00178758
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.ExtendEntityToFood(EntityTemplates.CreateLooseEntity("PickledMeal", STRINGS.ITEMS.FOOD.PICKLEDMEAL.NAME, STRINGS.ITEMS.FOOD.PICKLEDMEAL.DESC, 1f, false, Assets.GetAnim("pickledmeal_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.6f, 0.7f, true, 0, SimHashes.Creature, null), FOOD.FOOD_TYPES.PICKLEDMEAL);
		gameObject.GetComponent<KPrefabID>().AddTag(GameTags.Pickled, false);
		return gameObject;
	}

	// Token: 0x06000C4C RID: 3148 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000C4D RID: 3149 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000958 RID: 2392
	public const string ID = "PickledMeal";

	// Token: 0x04000959 RID: 2393
	public static ComplexRecipe recipe;
}
