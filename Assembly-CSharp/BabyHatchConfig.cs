using System;
using STRINGS;
using UnityEngine;

// Token: 0x02000237 RID: 567
[EntityConfigOrder(2)]
public class BabyHatchConfig : IEntityConfig
{
	// Token: 0x060007D1 RID: 2001 RVA: 0x000ADD9C File Offset: 0x000ABF9C
	public GameObject CreatePrefab()
	{
		GameObject gameObject = HatchConfig.CreateHatch("HatchBaby", CREATURES.SPECIES.HATCH.BABY.NAME, CREATURES.SPECIES.HATCH.BABY.DESC, "baby_hatch_kanim", true);
		EntityTemplates.ExtendEntityToBeingABaby(gameObject, "Hatch", null, false, 5f);
		return gameObject;
	}

	// Token: 0x060007D2 RID: 2002 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnPrefabInit(GameObject prefab)
	{
	}

	// Token: 0x060007D3 RID: 2003 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x040005EE RID: 1518
	public const string ID = "HatchBaby";
}
