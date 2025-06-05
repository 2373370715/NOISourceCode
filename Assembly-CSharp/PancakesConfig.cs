using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000315 RID: 789
public class PancakesConfig : IEntityConfig
{
	// Token: 0x06000C41 RID: 3137 RVA: 0x0017A46C File Offset: 0x0017866C
	public GameObject CreatePrefab()
	{
		return EntityTemplates.ExtendEntityToFood(EntityTemplates.CreateLooseEntity("Pancakes", STRINGS.ITEMS.FOOD.PANCAKES.NAME, STRINGS.ITEMS.FOOD.PANCAKES.DESC, 1f, false, Assets.GetAnim("stackedpancakes_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.8f, 0.8f, true, 0, SimHashes.Creature, null), FOOD.FOOD_TYPES.PANCAKES);
	}

	// Token: 0x06000C42 RID: 3138 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000C43 RID: 3139 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000954 RID: 2388
	public const string ID = "Pancakes";

	// Token: 0x04000955 RID: 2389
	public static ComplexRecipe recipe;
}
