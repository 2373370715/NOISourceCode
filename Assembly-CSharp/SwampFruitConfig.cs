using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x0200032A RID: 810
public class SwampFruitConfig : IEntityConfig, IHasDlcRestrictions
{
	// Token: 0x06000CA3 RID: 3235 RVA: 0x000AA117 File Offset: 0x000A8317
	public string[] GetRequiredDlcIds()
	{
		return DlcManager.EXPANSION1;
	}

	// Token: 0x06000CA4 RID: 3236 RVA: 0x000AA765 File Offset: 0x000A8965
	public string[] GetForbiddenDlcIds()
	{
		return null;
	}

	// Token: 0x06000CA5 RID: 3237 RVA: 0x0017AE20 File Offset: 0x00179020
	public GameObject CreatePrefab()
	{
		return EntityTemplates.ExtendEntityToFood(EntityTemplates.CreateLooseEntity(SwampFruitConfig.ID, STRINGS.ITEMS.FOOD.SWAMPFRUIT.NAME, STRINGS.ITEMS.FOOD.SWAMPFRUIT.DESC, 1f, false, Assets.GetAnim("swampcrop_fruit_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 1f, 0.72f, true, 0, SimHashes.Creature, null), FOOD.FOOD_TYPES.SWAMPFRUIT);
	}

	// Token: 0x06000CA6 RID: 3238 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000CA7 RID: 3239 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000984 RID: 2436
	public static string ID = "SwampFruit";
}
