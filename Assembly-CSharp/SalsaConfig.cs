using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x0200031F RID: 799
public class SalsaConfig : IEntityConfig
{
	// Token: 0x06000C70 RID: 3184 RVA: 0x0017A96C File Offset: 0x00178B6C
	public GameObject CreatePrefab()
	{
		return EntityTemplates.ExtendEntityToFood(EntityTemplates.CreateLooseEntity("Salsa", STRINGS.ITEMS.FOOD.SALSA.NAME, STRINGS.ITEMS.FOOD.SALSA.DESC, 1f, false, Assets.GetAnim("zestysalsa_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.8f, 0.5f, true, 0, SimHashes.Creature, null), FOOD.FOOD_TYPES.SALSA);
	}

	// Token: 0x06000C71 RID: 3185 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000C72 RID: 3186 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000967 RID: 2407
	public const string ID = "Salsa";

	// Token: 0x04000968 RID: 2408
	public static ComplexRecipe recipe;
}
