using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000324 RID: 804
public class SpiceNutConfig : IEntityConfig
{
	// Token: 0x06000C86 RID: 3206 RVA: 0x0017AB7C File Offset: 0x00178D7C
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateLooseEntity(SpiceNutConfig.ID, STRINGS.ITEMS.FOOD.SPICENUT.NAME, STRINGS.ITEMS.FOOD.SPICENUT.DESC, 1f, false, Assets.GetAnim("spicenut_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.CIRCLE, 0.3f, 0.3f, true, 0, SimHashes.Creature, null);
		EntityTemplates.ExtendEntityToFood(gameObject, FOOD.FOOD_TYPES.SPICENUT);
		SoundEventVolumeCache.instance.AddVolume("vinespicenut_kanim", "VineSpiceNut_grow", NOISE_POLLUTION.CREATURES.TIER3);
		SoundEventVolumeCache.instance.AddVolume("vinespicenut_kanim", "VineSpiceNut_harvest", NOISE_POLLUTION.CREATURES.TIER3);
		return gameObject;
	}

	// Token: 0x06000C87 RID: 3207 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000C88 RID: 3208 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000974 RID: 2420
	public static float SEEDS_PER_FRUIT = 1f;

	// Token: 0x04000975 RID: 2421
	public static string ID = "SpiceNut";
}
