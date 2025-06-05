using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000330 RID: 816
public class WormBasicFruitConfig : IEntityConfig, IHasDlcRestrictions
{
	// Token: 0x06000CC0 RID: 3264 RVA: 0x000AA117 File Offset: 0x000A8317
	public string[] GetRequiredDlcIds()
	{
		return DlcManager.EXPANSION1;
	}

	// Token: 0x06000CC1 RID: 3265 RVA: 0x000AA765 File Offset: 0x000A8965
	public string[] GetForbiddenDlcIds()
	{
		return null;
	}

	// Token: 0x06000CC2 RID: 3266 RVA: 0x0017B070 File Offset: 0x00179270
	public GameObject CreatePrefab()
	{
		return EntityTemplates.ExtendEntityToFood(EntityTemplates.CreateLooseEntity("WormBasicFruit", STRINGS.ITEMS.FOOD.WORMBASICFRUIT.NAME, STRINGS.ITEMS.FOOD.WORMBASICFRUIT.DESC, 1f, false, Assets.GetAnim("wormwood_basic_fruit_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.7f, 0.4f, true, 0, SimHashes.Creature, null), FOOD.FOOD_TYPES.WORMBASICFRUIT);
	}

	// Token: 0x06000CC3 RID: 3267 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000CC4 RID: 3268 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000990 RID: 2448
	public const string ID = "WormBasicFruit";
}
