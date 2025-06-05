using System;
using STRINGS;
using UnityEngine;

// Token: 0x0200023B RID: 571
[EntityConfigOrder(2)]
public class BabyHatchMetalConfig : IEntityConfig
{
	// Token: 0x060007E6 RID: 2022 RVA: 0x000ADE7C File Offset: 0x000AC07C
	public GameObject CreatePrefab()
	{
		GameObject gameObject = HatchMetalConfig.CreateHatch("HatchMetalBaby", CREATURES.SPECIES.HATCH.VARIANT_METAL.BABY.NAME, CREATURES.SPECIES.HATCH.VARIANT_METAL.BABY.DESC, "baby_hatch_kanim", true);
		EntityTemplates.ExtendEntityToBeingABaby(gameObject, "HatchMetal", null, false, 5f);
		return gameObject;
	}

	// Token: 0x060007E7 RID: 2023 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnPrefabInit(GameObject prefab)
	{
	}

	// Token: 0x060007E8 RID: 2024 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x040005FF RID: 1535
	public const string ID = "HatchMetalBaby";
}
