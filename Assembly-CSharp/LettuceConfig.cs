using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x0200030D RID: 781
public class LettuceConfig : IEntityConfig
{
	// Token: 0x06000C1E RID: 3102 RVA: 0x0017A014 File Offset: 0x00178214
	public GameObject CreatePrefab()
	{
		return EntityTemplates.ExtendEntityToFood(EntityTemplates.CreateLooseEntity("Lettuce", STRINGS.ITEMS.FOOD.LETTUCE.NAME, STRINGS.ITEMS.FOOD.LETTUCE.DESC, 1f, false, Assets.GetAnim("sea_lettuce_leaves_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.8f, 0.4f, true, 0, SimHashes.Creature, null), FOOD.FOOD_TYPES.LETTUCE);
	}

	// Token: 0x06000C1F RID: 3103 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000C20 RID: 3104 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000945 RID: 2373
	public const string ID = "Lettuce";
}
