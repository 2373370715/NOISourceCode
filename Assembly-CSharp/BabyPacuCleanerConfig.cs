using System;
using STRINGS;
using UnityEngine;

// Token: 0x0200025C RID: 604
[EntityConfigOrder(2)]
public class BabyPacuCleanerConfig : IEntityConfig
{
	// Token: 0x06000890 RID: 2192 RVA: 0x000AE483 File Offset: 0x000AC683
	public GameObject CreatePrefab()
	{
		GameObject gameObject = PacuCleanerConfig.CreatePacu("PacuCleanerBaby", CREATURES.SPECIES.PACU.VARIANT_CLEANER.BABY.NAME, CREATURES.SPECIES.PACU.VARIANT_CLEANER.BABY.DESC, "baby_pacu_kanim", true);
		EntityTemplates.ExtendEntityToBeingABaby(gameObject, "PacuCleaner", null, false, 5f);
		return gameObject;
	}

	// Token: 0x06000891 RID: 2193 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnPrefabInit(GameObject prefab)
	{
	}

	// Token: 0x06000892 RID: 2194 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x0400068C RID: 1676
	public const string ID = "PacuCleanerBaby";
}
