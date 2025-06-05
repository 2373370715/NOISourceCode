using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000321 RID: 801
public class ShellfishMeatConfig : IEntityConfig
{
	// Token: 0x06000C79 RID: 3193 RVA: 0x0017AA40 File Offset: 0x00178C40
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateLooseEntity("ShellfishMeat", STRINGS.ITEMS.FOOD.SHELLFISHMEAT.NAME, STRINGS.ITEMS.FOOD.SHELLFISHMEAT.DESC, 1f, false, Assets.GetAnim("shellfish_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.8f, 0.4f, true, 0, SimHashes.Creature, null);
		EntityTemplates.ExtendEntityToFood(gameObject, FOOD.FOOD_TYPES.SHELLFISH_MEAT);
		return gameObject;
	}

	// Token: 0x06000C7A RID: 3194 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000C7B RID: 3195 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x0400096D RID: 2413
	public const string ID = "ShellfishMeat";
}
