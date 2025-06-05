using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x0200030B RID: 779
public class GammaMushConfig : IEntityConfig
{
	// Token: 0x06000C16 RID: 3094 RVA: 0x00179F4C File Offset: 0x0017814C
	public GameObject CreatePrefab()
	{
		return EntityTemplates.ExtendEntityToFood(EntityTemplates.CreateLooseEntity("GammaMush", STRINGS.ITEMS.FOOD.GAMMAMUSH.NAME, STRINGS.ITEMS.FOOD.GAMMAMUSH.DESC, 1f, false, Assets.GetAnim("mushbarfried_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.8f, 0.4f, true, 0, SimHashes.Creature, null), FOOD.FOOD_TYPES.GAMMAMUSH);
	}

	// Token: 0x06000C17 RID: 3095 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000C18 RID: 3096 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000941 RID: 2369
	public const string ID = "GammaMush";

	// Token: 0x04000942 RID: 2370
	public static ComplexRecipe recipe;
}
