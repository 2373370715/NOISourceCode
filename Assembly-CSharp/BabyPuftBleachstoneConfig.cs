using System;
using STRINGS;
using UnityEngine;

// Token: 0x02000265 RID: 613
[EntityConfigOrder(2)]
public class BabyPuftBleachstoneConfig : IEntityConfig
{
	// Token: 0x060008B9 RID: 2233 RVA: 0x000AE622 File Offset: 0x000AC822
	public GameObject CreatePrefab()
	{
		GameObject gameObject = PuftBleachstoneConfig.CreatePuftBleachstone("PuftBleachstoneBaby", CREATURES.SPECIES.PUFT.VARIANT_BLEACHSTONE.BABY.NAME, CREATURES.SPECIES.PUFT.VARIANT_BLEACHSTONE.BABY.DESC, "baby_puft_kanim", true);
		EntityTemplates.ExtendEntityToBeingABaby(gameObject, "PuftBleachstone", null, false, 5f);
		return gameObject;
	}

	// Token: 0x060008BA RID: 2234 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnPrefabInit(GameObject prefab)
	{
	}

	// Token: 0x060008BB RID: 2235 RVA: 0x000AE578 File Offset: 0x000AC778
	public void OnSpawn(GameObject inst)
	{
		BasePuftConfig.OnSpawn(inst);
	}

	// Token: 0x040006AD RID: 1709
	public const string ID = "PuftBleachstoneBaby";
}
