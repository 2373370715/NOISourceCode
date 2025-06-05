using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x0200031B RID: 795
public class QuicheConfig : IEntityConfig
{
	// Token: 0x06000C5E RID: 3166 RVA: 0x0017A770 File Offset: 0x00178970
	public GameObject CreatePrefab()
	{
		return EntityTemplates.ExtendEntityToFood(EntityTemplates.CreateLooseEntity("Quiche", STRINGS.ITEMS.FOOD.QUICHE.NAME, STRINGS.ITEMS.FOOD.QUICHE.DESC, 1f, false, Assets.GetAnim("quiche_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.8f, 0.4f, true, 0, SimHashes.Creature, null), FOOD.FOOD_TYPES.QUICHE);
	}

	// Token: 0x06000C5F RID: 3167 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000C60 RID: 3168 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x0400095F RID: 2399
	public const string ID = "Quiche";

	// Token: 0x04000960 RID: 2400
	public static ComplexRecipe recipe;
}
