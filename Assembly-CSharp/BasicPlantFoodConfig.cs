using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020002F3 RID: 755
public class BasicPlantFoodConfig : IEntityConfig
{
	// Token: 0x06000BA0 RID: 2976 RVA: 0x0017952C File Offset: 0x0017772C
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateLooseEntity("BasicPlantFood", STRINGS.ITEMS.FOOD.BASICPLANTFOOD.NAME, STRINGS.ITEMS.FOOD.BASICPLANTFOOD.DESC, 1f, false, Assets.GetAnim("meallicegrain_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.CIRCLE, 0.25f, 0.25f, true, 0, SimHashes.Creature, null);
		EntityTemplates.ExtendEntityToFood(gameObject, FOOD.FOOD_TYPES.BASICPLANTFOOD);
		return gameObject;
	}

	// Token: 0x06000BA1 RID: 2977 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000BA2 RID: 2978 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x0400090F RID: 2319
	public const string ID = "BasicPlantFood";
}
