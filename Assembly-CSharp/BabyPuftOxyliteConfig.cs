using System;
using STRINGS;
using UnityEngine;

// Token: 0x02000269 RID: 617
[EntityConfigOrder(2)]
public class BabyPuftOxyliteConfig : IEntityConfig
{
	// Token: 0x060008CD RID: 2253 RVA: 0x000AE700 File Offset: 0x000AC900
	public GameObject CreatePrefab()
	{
		GameObject gameObject = PuftOxyliteConfig.CreatePuftOxylite("PuftOxyliteBaby", CREATURES.SPECIES.PUFT.VARIANT_OXYLITE.BABY.NAME, CREATURES.SPECIES.PUFT.VARIANT_OXYLITE.BABY.DESC, "baby_puft_kanim", true);
		EntityTemplates.ExtendEntityToBeingABaby(gameObject, "PuftOxylite", null, false, 5f);
		return gameObject;
	}

	// Token: 0x060008CE RID: 2254 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnPrefabInit(GameObject prefab)
	{
	}

	// Token: 0x060008CF RID: 2255 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x040006C3 RID: 1731
	public const string ID = "PuftOxyliteBaby";
}
