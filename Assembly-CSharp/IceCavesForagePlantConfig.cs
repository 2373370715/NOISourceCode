using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020002AD RID: 685
public class IceCavesForagePlantConfig : IEntityConfig, IHasDlcRestrictions
{
	// Token: 0x060009FE RID: 2558 RVA: 0x000AA536 File Offset: 0x000A8736
	public string[] GetRequiredDlcIds()
	{
		return DlcManager.DLC2;
	}

	// Token: 0x060009FF RID: 2559 RVA: 0x000AA765 File Offset: 0x000A8965
	public string[] GetForbiddenDlcIds()
	{
		return null;
	}

	// Token: 0x06000A00 RID: 2560 RVA: 0x00172D0C File Offset: 0x00170F0C
	public GameObject CreatePrefab()
	{
		return EntityTemplates.ExtendEntityToFood(EntityTemplates.CreateLooseEntity("IceCavesForagePlant", STRINGS.ITEMS.FOOD.ICECAVESFORAGEPLANT.NAME, STRINGS.ITEMS.FOOD.ICECAVESFORAGEPLANT.DESC, 1f, false, Assets.GetAnim("frozenberries_fruit_kanim"), "object", Grid.SceneLayer.BuildingBack, EntityTemplates.CollisionShape.CIRCLE, 0.3f, 0.3f, true, 0, SimHashes.Creature, null), FOOD.FOOD_TYPES.ICECAVESFORAGEPLANT);
	}

	// Token: 0x06000A01 RID: 2561 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000A02 RID: 2562 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x040007E9 RID: 2025
	public const string ID = "IceCavesForagePlant";
}
