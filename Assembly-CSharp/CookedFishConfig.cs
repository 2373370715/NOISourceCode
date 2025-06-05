using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020002FC RID: 764
public class CookedFishConfig : IEntityConfig
{
	// Token: 0x06000BCD RID: 3021 RVA: 0x0017993C File Offset: 0x00177B3C
	public GameObject CreatePrefab()
	{
		return EntityTemplates.ExtendEntityToFood(EntityTemplates.CreateLooseEntity("CookedFish", STRINGS.ITEMS.FOOD.COOKEDFISH.NAME, STRINGS.ITEMS.FOOD.COOKEDFISH.DESC, 1f, false, Assets.GetAnim("grilled_pacu_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.8f, 0.4f, true, 0, SimHashes.Creature, null), FOOD.FOOD_TYPES.COOKED_FISH);
	}

	// Token: 0x06000BCE RID: 3022 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000BCF RID: 3023 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000924 RID: 2340
	public const string ID = "CookedFish";

	// Token: 0x04000925 RID: 2341
	public static ComplexRecipe recipe;
}
