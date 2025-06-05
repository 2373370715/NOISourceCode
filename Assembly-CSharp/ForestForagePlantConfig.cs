using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000298 RID: 664
public class ForestForagePlantConfig : IEntityConfig
{
	// Token: 0x060009BD RID: 2493 RVA: 0x00170E7C File Offset: 0x0016F07C
	public GameObject CreatePrefab()
	{
		return EntityTemplates.ExtendEntityToFood(EntityTemplates.CreateLooseEntity("ForestForagePlant", STRINGS.ITEMS.FOOD.FORESTFORAGEPLANT.NAME, STRINGS.ITEMS.FOOD.FORESTFORAGEPLANT.DESC, 1f, false, Assets.GetAnim("podmelon_fruit_kanim"), "object", Grid.SceneLayer.BuildingBack, EntityTemplates.CollisionShape.CIRCLE, 0.3f, 0.3f, true, 0, SimHashes.Creature, null), FOOD.FOOD_TYPES.FORESTFORAGEPLANT);
	}

	// Token: 0x060009BE RID: 2494 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x060009BF RID: 2495 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x0400077A RID: 1914
	public const string ID = "ForestForagePlant";
}
