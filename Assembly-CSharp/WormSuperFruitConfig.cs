using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000332 RID: 818
public class WormSuperFruitConfig : IEntityConfig, IHasDlcRestrictions
{
	// Token: 0x06000CCC RID: 3276 RVA: 0x000AA117 File Offset: 0x000A8317
	public string[] GetRequiredDlcIds()
	{
		return DlcManager.EXPANSION1;
	}

	// Token: 0x06000CCD RID: 3277 RVA: 0x000AA765 File Offset: 0x000A8965
	public string[] GetForbiddenDlcIds()
	{
		return null;
	}

	// Token: 0x06000CCE RID: 3278 RVA: 0x0017B138 File Offset: 0x00179338
	public GameObject CreatePrefab()
	{
		return EntityTemplates.ExtendEntityToFood(EntityTemplates.CreateLooseEntity("WormSuperFruit", STRINGS.ITEMS.FOOD.WORMSUPERFRUIT.NAME, STRINGS.ITEMS.FOOD.WORMSUPERFRUIT.DESC, 1f, false, Assets.GetAnim("wormwood_super_fruits_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.6f, 0.4f, true, 0, SimHashes.Creature, null), FOOD.FOOD_TYPES.WORMSUPERFRUIT);
	}

	// Token: 0x06000CCF RID: 3279 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000CD0 RID: 3280 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000993 RID: 2451
	public const string ID = "WormSuperFruit";
}
