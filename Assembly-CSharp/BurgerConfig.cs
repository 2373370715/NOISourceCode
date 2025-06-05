using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020002F6 RID: 758
public class BurgerConfig : IEntityConfig
{
	// Token: 0x06000BB1 RID: 2993 RVA: 0x00179668 File Offset: 0x00177868
	public GameObject CreatePrefab()
	{
		return EntityTemplates.ExtendEntityToFood(EntityTemplates.CreateLooseEntity("Burger", STRINGS.ITEMS.FOOD.BURGER.NAME, STRINGS.ITEMS.FOOD.BURGER.DESC, 1f, false, Assets.GetAnim("frost_burger_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.8f, 0.4f, true, 0, SimHashes.Creature, null), FOOD.FOOD_TYPES.BURGER);
	}

	// Token: 0x06000BB2 RID: 2994 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000BB3 RID: 2995 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000916 RID: 2326
	public const string ID = "Burger";

	// Token: 0x04000917 RID: 2327
	public static ComplexRecipe recipe;
}
