using System;
using STRINGS;
using UnityEngine;

// Token: 0x0200022C RID: 556
[EntityConfigOrder(2)]
public class BabyWormConfig : IEntityConfig, IHasDlcRestrictions
{
	// Token: 0x06000799 RID: 1945 RVA: 0x000AA117 File Offset: 0x000A8317
	public string[] GetRequiredDlcIds()
	{
		return DlcManager.EXPANSION1;
	}

	// Token: 0x0600079A RID: 1946 RVA: 0x000AA765 File Offset: 0x000A8965
	public string[] GetForbiddenDlcIds()
	{
		return null;
	}

	// Token: 0x0600079B RID: 1947 RVA: 0x000ADC76 File Offset: 0x000ABE76
	public GameObject CreatePrefab()
	{
		GameObject gameObject = DivergentWormConfig.CreateWorm("DivergentWormBaby", CREATURES.SPECIES.DIVERGENT.VARIANT_WORM.BABY.NAME, CREATURES.SPECIES.DIVERGENT.VARIANT_WORM.BABY.DESC, "baby_worm_kanim", true);
		EntityTemplates.ExtendEntityToBeingABaby(gameObject, "DivergentWorm", null, false, 5f);
		return gameObject;
	}

	// Token: 0x0600079C RID: 1948 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnPrefabInit(GameObject prefab)
	{
	}

	// Token: 0x0600079D RID: 1949 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x040005B1 RID: 1457
	public const string ID = "DivergentWormBaby";
}
