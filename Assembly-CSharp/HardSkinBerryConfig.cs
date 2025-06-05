using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020002AB RID: 683
public class HardSkinBerryConfig : IEntityConfig, IHasDlcRestrictions
{
	// Token: 0x060009F2 RID: 2546 RVA: 0x000AA536 File Offset: 0x000A8736
	public string[] GetRequiredDlcIds()
	{
		return DlcManager.DLC2;
	}

	// Token: 0x060009F3 RID: 2547 RVA: 0x000AA765 File Offset: 0x000A8965
	public string[] GetForbiddenDlcIds()
	{
		return null;
	}

	// Token: 0x060009F4 RID: 2548 RVA: 0x00172AC8 File Offset: 0x00170CC8
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateLooseEntity("HardSkinBerry", STRINGS.ITEMS.FOOD.HARDSKINBERRY.NAME, STRINGS.ITEMS.FOOD.HARDSKINBERRY.DESC, 1f, false, Assets.GetAnim("iceBerry_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.CIRCLE, 0.25f, 0.25f, true, 0, SimHashes.Creature, null);
		EntityTemplates.ExtendEntityToFood(gameObject, FOOD.FOOD_TYPES.HARDSKINBERRY);
		return gameObject;
	}

	// Token: 0x060009F5 RID: 2549 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x060009F6 RID: 2550 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x040007E1 RID: 2017
	public const string ID = "HardSkinBerry";
}
