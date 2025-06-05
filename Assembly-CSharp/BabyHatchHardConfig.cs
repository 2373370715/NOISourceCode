using System;
using STRINGS;
using UnityEngine;

// Token: 0x02000239 RID: 569
[EntityConfigOrder(2)]
public class BabyHatchHardConfig : IEntityConfig
{
	// Token: 0x060007DB RID: 2011 RVA: 0x000ADE0C File Offset: 0x000AC00C
	public GameObject CreatePrefab()
	{
		GameObject gameObject = HatchHardConfig.CreateHatch("HatchHardBaby", CREATURES.SPECIES.HATCH.VARIANT_HARD.BABY.NAME, CREATURES.SPECIES.HATCH.VARIANT_HARD.BABY.DESC, "baby_hatch_kanim", true);
		EntityTemplates.ExtendEntityToBeingABaby(gameObject, "HatchHard", null, false, 5f);
		return gameObject;
	}

	// Token: 0x060007DC RID: 2012 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnPrefabInit(GameObject prefab)
	{
	}

	// Token: 0x060007DD RID: 2013 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x040005F7 RID: 1527
	public const string ID = "HatchHardBaby";
}
