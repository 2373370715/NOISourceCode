using System;
using STRINGS;
using UnityEngine;

// Token: 0x02000226 RID: 550
[EntityConfigOrder(2)]
public class BabyCrabFreshWaterConfig : IEntityConfig
{
	// Token: 0x06000775 RID: 1909 RVA: 0x000ADB97 File Offset: 0x000ABD97
	public GameObject CreatePrefab()
	{
		GameObject gameObject = CrabFreshWaterConfig.CreateCrabFreshWater("CrabFreshWaterBaby", CREATURES.SPECIES.CRAB.VARIANT_FRESH_WATER.BABY.NAME, CREATURES.SPECIES.CRAB.VARIANT_FRESH_WATER.BABY.DESC, "baby_pincher_kanim", true, null);
		EntityTemplates.ExtendEntityToBeingABaby(gameObject, "CrabFreshWater", null, false, 5f);
		return gameObject;
	}

	// Token: 0x06000776 RID: 1910 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnPrefabInit(GameObject prefab)
	{
	}

	// Token: 0x06000777 RID: 1911 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x0400058E RID: 1422
	public const string ID = "CrabFreshWaterBaby";
}
