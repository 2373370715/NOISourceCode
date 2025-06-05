using System;
using STRINGS;
using UnityEngine;

// Token: 0x02000261 RID: 609
[EntityConfigOrder(2)]
public class BabyPacuTropicalConfig : IEntityConfig
{
	// Token: 0x060008A5 RID: 2213 RVA: 0x000AE521 File Offset: 0x000AC721
	public GameObject CreatePrefab()
	{
		GameObject gameObject = PacuTropicalConfig.CreatePacu("PacuTropicalBaby", CREATURES.SPECIES.PACU.VARIANT_TROPICAL.BABY.NAME, CREATURES.SPECIES.PACU.VARIANT_TROPICAL.BABY.DESC, "baby_pacu_kanim", true);
		EntityTemplates.ExtendEntityToBeingABaby(gameObject, "PacuTropical", null, false, 5f);
		return gameObject;
	}

	// Token: 0x060008A6 RID: 2214 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnPrefabInit(GameObject prefab)
	{
	}

	// Token: 0x060008A7 RID: 2215 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000697 RID: 1687
	public const string ID = "PacuTropicalBaby";
}
