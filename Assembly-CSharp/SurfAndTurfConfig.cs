using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000327 RID: 807
public class SurfAndTurfConfig : IEntityConfig
{
	// Token: 0x06000C94 RID: 3220 RVA: 0x0017ACE8 File Offset: 0x00178EE8
	public GameObject CreatePrefab()
	{
		return EntityTemplates.ExtendEntityToFood(EntityTemplates.CreateLooseEntity("SurfAndTurf", STRINGS.ITEMS.FOOD.SURFANDTURF.NAME, STRINGS.ITEMS.FOOD.SURFANDTURF.DESC, 1f, false, Assets.GetAnim("surfnturf_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.8f, 0.4f, true, 0, SimHashes.Creature, null), FOOD.FOOD_TYPES.SURF_AND_TURF);
	}

	// Token: 0x06000C95 RID: 3221 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000C96 RID: 3222 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x0400097C RID: 2428
	public const string ID = "SurfAndTurf";

	// Token: 0x0400097D RID: 2429
	public static ComplexRecipe recipe;
}
