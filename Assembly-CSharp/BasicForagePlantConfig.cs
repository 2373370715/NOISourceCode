using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000289 RID: 649
public class BasicForagePlantConfig : IEntityConfig
{
	// Token: 0x06000975 RID: 2421 RVA: 0x0016F4AC File Offset: 0x0016D6AC
	public GameObject CreatePrefab()
	{
		return EntityTemplates.ExtendEntityToFood(EntityTemplates.CreateLooseEntity("BasicForagePlant", STRINGS.ITEMS.FOOD.BASICFORAGEPLANT.NAME, STRINGS.ITEMS.FOOD.BASICFORAGEPLANT.DESC, 1f, false, Assets.GetAnim("muckrootvegetable_kanim"), "object", Grid.SceneLayer.BuildingBack, EntityTemplates.CollisionShape.CIRCLE, 0.3f, 0.3f, true, 0, SimHashes.Creature, null), FOOD.FOOD_TYPES.BASICFORAGEPLANT);
	}

	// Token: 0x06000976 RID: 2422 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000977 RID: 2423 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x0400073E RID: 1854
	public const string ID = "BasicForagePlant";
}
