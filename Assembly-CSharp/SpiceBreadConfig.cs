using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000322 RID: 802
public class SpiceBreadConfig : IEntityConfig
{
	// Token: 0x06000C7D RID: 3197 RVA: 0x0017AAA8 File Offset: 0x00178CA8
	public GameObject CreatePrefab()
	{
		return EntityTemplates.ExtendEntityToFood(EntityTemplates.CreateLooseEntity("SpiceBread", STRINGS.ITEMS.FOOD.SPICEBREAD.NAME, STRINGS.ITEMS.FOOD.SPICEBREAD.DESC, 1f, false, Assets.GetAnim("pepperbread_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.9f, 0.6f, true, 0, SimHashes.Creature, null), FOOD.FOOD_TYPES.SPICEBREAD);
	}

	// Token: 0x06000C7E RID: 3198 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000C7F RID: 3199 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x0400096E RID: 2414
	public const string ID = "SpiceBread";

	// Token: 0x0400096F RID: 2415
	public static ComplexRecipe recipe;
}
