using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x0200032B RID: 811
public class SwampLilyFlowerConfig : IEntityConfig
{
	// Token: 0x06000CAA RID: 3242 RVA: 0x0017AE84 File Offset: 0x00179084
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateLooseEntity(SwampLilyFlowerConfig.ID, ITEMS.INGREDIENTS.SWAMPLILYFLOWER.NAME, ITEMS.INGREDIENTS.SWAMPLILYFLOWER.DESC, 1f, false, Assets.GetAnim("swamplilyflower_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.8f, 0.4f, true, 0, SimHashes.Creature, new List<Tag>
		{
			GameTags.IndustrialIngredient
		});
		EntityTemplates.CreateAndRegisterCompostableFromPrefab(gameObject);
		gameObject.AddOrGet<EntitySplitter>();
		return gameObject;
	}

	// Token: 0x06000CAB RID: 3243 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000CAC RID: 3244 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000985 RID: 2437
	public static float SEEDS_PER_FRUIT = 1f;

	// Token: 0x04000986 RID: 2438
	public static string ID = "SwampLilyFlower";
}
