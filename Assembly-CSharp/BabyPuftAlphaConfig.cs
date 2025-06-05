using System;
using STRINGS;
using UnityEngine;

// Token: 0x02000263 RID: 611
[EntityConfigOrder(2)]
public class BabyPuftAlphaConfig : IEntityConfig
{
	// Token: 0x060008AF RID: 2223 RVA: 0x000AE5B2 File Offset: 0x000AC7B2
	public GameObject CreatePrefab()
	{
		GameObject gameObject = PuftAlphaConfig.CreatePuftAlpha("PuftAlphaBaby", CREATURES.SPECIES.PUFT.VARIANT_ALPHA.BABY.NAME, CREATURES.SPECIES.PUFT.VARIANT_ALPHA.BABY.DESC, "baby_puft_kanim", true);
		EntityTemplates.ExtendEntityToBeingABaby(gameObject, "PuftAlpha", null, false, 5f);
		return gameObject;
	}

	// Token: 0x060008B0 RID: 2224 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnPrefabInit(GameObject prefab)
	{
	}

	// Token: 0x060008B1 RID: 2225 RVA: 0x000AE578 File Offset: 0x000AC778
	public void OnSpawn(GameObject inst)
	{
		BasePuftConfig.OnSpawn(inst);
	}

	// Token: 0x040006A3 RID: 1699
	public const string ID = "PuftAlphaBaby";
}
