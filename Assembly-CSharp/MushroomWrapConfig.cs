using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000312 RID: 786
public class MushroomWrapConfig : IEntityConfig
{
	// Token: 0x06000C34 RID: 3124 RVA: 0x0017A330 File Offset: 0x00178530
	public GameObject CreatePrefab()
	{
		return EntityTemplates.ExtendEntityToFood(EntityTemplates.CreateLooseEntity("MushroomWrap", STRINGS.ITEMS.FOOD.MUSHROOMWRAP.NAME, STRINGS.ITEMS.FOOD.MUSHROOMWRAP.DESC, 1f, false, Assets.GetAnim("mushroom_wrap_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.8f, 0.5f, true, 0, SimHashes.Creature, null), FOOD.FOOD_TYPES.MUSHROOM_WRAP);
	}

	// Token: 0x06000C35 RID: 3125 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000C36 RID: 3126 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x0400094D RID: 2381
	public const string ID = "MushroomWrap";

	// Token: 0x0400094E RID: 2382
	public static ComplexRecipe recipe;
}
