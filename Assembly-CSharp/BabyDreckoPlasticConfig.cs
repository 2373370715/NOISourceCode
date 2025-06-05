using System;
using STRINGS;
using UnityEngine;

// Token: 0x02000230 RID: 560
[EntityConfigOrder(2)]
public class BabyDreckoPlasticConfig : IEntityConfig
{
	// Token: 0x060007AF RID: 1967 RVA: 0x000ADCF2 File Offset: 0x000ABEF2
	public GameObject CreatePrefab()
	{
		GameObject gameObject = DreckoPlasticConfig.CreateDrecko("DreckoPlasticBaby", CREATURES.SPECIES.DRECKO.VARIANT_PLASTIC.BABY.NAME, CREATURES.SPECIES.DRECKO.VARIANT_PLASTIC.BABY.DESC, "baby_drecko_kanim", true);
		EntityTemplates.ExtendEntityToBeingABaby(gameObject, "DreckoPlastic", null, false, 5f);
		return gameObject;
	}

	// Token: 0x060007B0 RID: 1968 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnPrefabInit(GameObject prefab)
	{
	}

	// Token: 0x060007B1 RID: 1969 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x040005CD RID: 1485
	public const string ID = "DreckoPlasticBaby";
}
