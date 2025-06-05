using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000308 RID: 776
public class FriedMushroomConfig : IEntityConfig
{
	// Token: 0x06000C08 RID: 3080 RVA: 0x00179DFC File Offset: 0x00177FFC
	public GameObject CreatePrefab()
	{
		return EntityTemplates.ExtendEntityToFood(EntityTemplates.CreateLooseEntity("FriedMushroom", STRINGS.ITEMS.FOOD.FRIEDMUSHROOM.NAME, STRINGS.ITEMS.FOOD.FRIEDMUSHROOM.DESC, 1f, false, Assets.GetAnim("funguscapfried_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.8f, 0.6f, true, 0, SimHashes.Creature, null), FOOD.FOOD_TYPES.FRIED_MUSHROOM);
	}

	// Token: 0x06000C09 RID: 3081 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000C0A RID: 3082 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x0400093B RID: 2363
	public const string ID = "FriedMushroom";

	// Token: 0x0400093C RID: 2364
	public static ComplexRecipe recipe;
}
