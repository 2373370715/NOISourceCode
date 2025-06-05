using System;
using STRINGS;
using UnityEngine;

// Token: 0x02000267 RID: 615
[EntityConfigOrder(2)]
public class BabyPuftConfig : IEntityConfig
{
	// Token: 0x060008C3 RID: 2243 RVA: 0x000AE690 File Offset: 0x000AC890
	public GameObject CreatePrefab()
	{
		GameObject gameObject = PuftConfig.CreatePuft("PuftBaby", CREATURES.SPECIES.PUFT.BABY.NAME, CREATURES.SPECIES.PUFT.BABY.DESC, "baby_puft_kanim", true);
		EntityTemplates.ExtendEntityToBeingABaby(gameObject, "Puft", null, false, 5f);
		return gameObject;
	}

	// Token: 0x060008C4 RID: 2244 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnPrefabInit(GameObject prefab)
	{
	}

	// Token: 0x060008C5 RID: 2245 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x040006B9 RID: 1721
	public const string ID = "PuftBaby";
}
