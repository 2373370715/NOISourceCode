using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020002FF RID: 767
public class CurryConfig : IEntityConfig
{
	// Token: 0x06000BDB RID: 3035 RVA: 0x00179A68 File Offset: 0x00177C68
	public GameObject CreatePrefab()
	{
		return EntityTemplates.ExtendEntityToFood(EntityTemplates.CreateLooseEntity("Curry", STRINGS.ITEMS.FOOD.CURRY.NAME, STRINGS.ITEMS.FOOD.CURRY.DESC, 1f, false, Assets.GetAnim("curried_beans_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.9f, 0.5f, true, 0, SimHashes.Creature, null), FOOD.FOOD_TYPES.CURRY);
	}

	// Token: 0x06000BDC RID: 3036 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000BDD RID: 3037 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x0400092A RID: 2346
	public const string ID = "Curry";
}
