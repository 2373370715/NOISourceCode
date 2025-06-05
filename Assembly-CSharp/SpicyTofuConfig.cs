using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000325 RID: 805
public class SpicyTofuConfig : IEntityConfig
{
	// Token: 0x06000C8B RID: 3211 RVA: 0x0017AC14 File Offset: 0x00178E14
	public GameObject CreatePrefab()
	{
		return EntityTemplates.ExtendEntityToFood(EntityTemplates.CreateLooseEntity("SpicyTofu", STRINGS.ITEMS.FOOD.SPICYTOFU.NAME, STRINGS.ITEMS.FOOD.SPICYTOFU.DESC, 1f, false, Assets.GetAnim("spicey_tofu_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.9f, 0.6f, true, 0, SimHashes.Creature, null), FOOD.FOOD_TYPES.SPICY_TOFU);
	}

	// Token: 0x06000C8C RID: 3212 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000C8D RID: 3213 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000976 RID: 2422
	public const string ID = "SpicyTofu";

	// Token: 0x04000977 RID: 2423
	public static ComplexRecipe recipe;
}
