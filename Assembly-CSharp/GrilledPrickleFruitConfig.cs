using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x0200030C RID: 780
public class GrilledPrickleFruitConfig : IEntityConfig
{
	// Token: 0x06000C1A RID: 3098 RVA: 0x00179FB0 File Offset: 0x001781B0
	public GameObject CreatePrefab()
	{
		return EntityTemplates.ExtendEntityToFood(EntityTemplates.CreateLooseEntity("GrilledPrickleFruit", STRINGS.ITEMS.FOOD.GRILLEDPRICKLEFRUIT.NAME, STRINGS.ITEMS.FOOD.GRILLEDPRICKLEFRUIT.DESC, 1f, false, Assets.GetAnim("gristleberry_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.8f, 0.7f, true, 0, SimHashes.Creature, null), FOOD.FOOD_TYPES.GRILLED_PRICKLEFRUIT);
	}

	// Token: 0x06000C1B RID: 3099 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000C1C RID: 3100 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000943 RID: 2371
	public const string ID = "GrilledPrickleFruit";

	// Token: 0x04000944 RID: 2372
	public static ComplexRecipe recipe;
}
