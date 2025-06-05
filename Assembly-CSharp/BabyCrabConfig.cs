using System;
using STRINGS;
using UnityEngine;

// Token: 0x02000224 RID: 548
[EntityConfigOrder(2)]
public class BabyCrabConfig : IEntityConfig
{
	// Token: 0x0600076B RID: 1899 RVA: 0x0016836C File Offset: 0x0016656C
	public GameObject CreatePrefab()
	{
		GameObject gameObject = CrabConfig.CreateCrab("CrabBaby", CREATURES.SPECIES.CRAB.BABY.NAME, CREATURES.SPECIES.CRAB.BABY.DESC, "baby_pincher_kanim", true, "BabyCrabShell");
		EntityTemplates.ExtendEntityToBeingABaby(gameObject, "Crab", "BabyCrabShell", false, 5f);
		return gameObject;
	}

	// Token: 0x0600076C RID: 1900 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnPrefabInit(GameObject prefab)
	{
	}

	// Token: 0x0600076D RID: 1901 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000584 RID: 1412
	public const string ID = "CrabBaby";
}
