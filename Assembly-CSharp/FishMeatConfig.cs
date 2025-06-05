using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000306 RID: 774
public class FishMeatConfig : IEntityConfig
{
	// Token: 0x06000C00 RID: 3072 RVA: 0x00179D30 File Offset: 0x00177F30
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateLooseEntity("FishMeat", STRINGS.ITEMS.FOOD.FISHMEAT.NAME, STRINGS.ITEMS.FOOD.FISHMEAT.DESC, 1f, false, Assets.GetAnim("pacufillet_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.8f, 0.4f, true, 0, SimHashes.Creature, null);
		EntityTemplates.ExtendEntityToFood(gameObject, FOOD.FOOD_TYPES.FISH_MEAT);
		return gameObject;
	}

	// Token: 0x06000C01 RID: 3073 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000C02 RID: 3074 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000938 RID: 2360
	public const string ID = "FishMeat";
}
