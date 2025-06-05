using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x0200030E RID: 782
public class MeatConfig : IEntityConfig
{
	// Token: 0x06000C22 RID: 3106 RVA: 0x0017A078 File Offset: 0x00178278
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateLooseEntity("Meat", STRINGS.ITEMS.FOOD.MEAT.NAME, STRINGS.ITEMS.FOOD.MEAT.DESC, 1f, false, Assets.GetAnim("creaturemeat_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.8f, 0.4f, true, 0, SimHashes.Creature, null);
		EntityTemplates.ExtendEntityToFood(gameObject, FOOD.FOOD_TYPES.MEAT);
		return gameObject;
	}

	// Token: 0x06000C23 RID: 3107 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000C24 RID: 3108 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000946 RID: 2374
	public const string ID = "Meat";
}
