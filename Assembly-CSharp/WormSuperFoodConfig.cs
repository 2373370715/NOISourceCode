using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000331 RID: 817
public class WormSuperFoodConfig : IEntityConfig, IHasDlcRestrictions
{
	// Token: 0x06000CC6 RID: 3270 RVA: 0x000AA117 File Offset: 0x000A8317
	public string[] GetRequiredDlcIds()
	{
		return DlcManager.EXPANSION1;
	}

	// Token: 0x06000CC7 RID: 3271 RVA: 0x000AA765 File Offset: 0x000A8965
	public string[] GetForbiddenDlcIds()
	{
		return null;
	}

	// Token: 0x06000CC8 RID: 3272 RVA: 0x0017B0D4 File Offset: 0x001792D4
	public GameObject CreatePrefab()
	{
		return EntityTemplates.ExtendEntityToFood(EntityTemplates.CreateLooseEntity("WormSuperFood", STRINGS.ITEMS.FOOD.WORMSUPERFOOD.NAME, STRINGS.ITEMS.FOOD.WORMSUPERFOOD.DESC, 1f, false, Assets.GetAnim("wormwood_preserved_berries_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.7f, 0.6f, true, 0, SimHashes.Creature, null), FOOD.FOOD_TYPES.WORMSUPERFOOD);
	}

	// Token: 0x06000CC9 RID: 3273 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000CCA RID: 3274 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000991 RID: 2449
	public const string ID = "WormSuperFood";

	// Token: 0x04000992 RID: 2450
	public static ComplexRecipe recipe;
}
