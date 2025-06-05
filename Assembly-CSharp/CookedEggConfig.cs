using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020002FB RID: 763
public class CookedEggConfig : IEntityConfig
{
	// Token: 0x06000BC9 RID: 3017 RVA: 0x001798D8 File Offset: 0x00177AD8
	public GameObject CreatePrefab()
	{
		return EntityTemplates.ExtendEntityToFood(EntityTemplates.CreateLooseEntity("CookedEgg", STRINGS.ITEMS.FOOD.COOKEDEGG.NAME, STRINGS.ITEMS.FOOD.COOKEDEGG.DESC, 1f, false, Assets.GetAnim("cookedegg_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.8f, 0.4f, true, 0, SimHashes.Creature, null), FOOD.FOOD_TYPES.COOKED_EGG);
	}

	// Token: 0x06000BCA RID: 3018 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000BCB RID: 3019 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000922 RID: 2338
	public const string ID = "CookedEgg";

	// Token: 0x04000923 RID: 2339
	public static ComplexRecipe recipe;
}
