using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020002FD RID: 765
public class CookedMeatConfig : IEntityConfig
{
	// Token: 0x06000BD1 RID: 3025 RVA: 0x001799A0 File Offset: 0x00177BA0
	public GameObject CreatePrefab()
	{
		return EntityTemplates.ExtendEntityToFood(EntityTemplates.CreateLooseEntity("CookedMeat", STRINGS.ITEMS.FOOD.COOKEDMEAT.NAME, STRINGS.ITEMS.FOOD.COOKEDMEAT.DESC, 1f, false, Assets.GetAnim("barbeque_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.8f, 0.4f, true, 0, SimHashes.Creature, null), FOOD.FOOD_TYPES.COOKED_MEAT);
	}

	// Token: 0x06000BD2 RID: 3026 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000BD3 RID: 3027 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000926 RID: 2342
	public const string ID = "CookedMeat";

	// Token: 0x04000927 RID: 2343
	public static ComplexRecipe recipe;
}
