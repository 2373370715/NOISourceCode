using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x0200032F RID: 815
public class WormBasicFoodConfig : IEntityConfig, IHasDlcRestrictions
{
	// Token: 0x06000CBA RID: 3258 RVA: 0x000AA117 File Offset: 0x000A8317
	public string[] GetRequiredDlcIds()
	{
		return DlcManager.EXPANSION1;
	}

	// Token: 0x06000CBB RID: 3259 RVA: 0x000AA765 File Offset: 0x000A8965
	public string[] GetForbiddenDlcIds()
	{
		return null;
	}

	// Token: 0x06000CBC RID: 3260 RVA: 0x0017B00C File Offset: 0x0017920C
	public GameObject CreatePrefab()
	{
		return EntityTemplates.ExtendEntityToFood(EntityTemplates.CreateLooseEntity("WormBasicFood", STRINGS.ITEMS.FOOD.WORMBASICFOOD.NAME, STRINGS.ITEMS.FOOD.WORMBASICFOOD.DESC, 1f, false, Assets.GetAnim("wormwood_roast_nuts_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.7f, 0.4f, true, 0, SimHashes.Creature, null), FOOD.FOOD_TYPES.WORMBASICFOOD);
	}

	// Token: 0x06000CBD RID: 3261 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000CBE RID: 3262 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x0400098E RID: 2446
	public const string ID = "WormBasicFood";

	// Token: 0x0400098F RID: 2447
	public static ComplexRecipe recipe;
}
