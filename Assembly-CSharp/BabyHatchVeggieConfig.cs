using System;
using STRINGS;
using UnityEngine;

// Token: 0x0200023D RID: 573
[EntityConfigOrder(2)]
public class BabyHatchVeggieConfig : IEntityConfig
{
	// Token: 0x060007F0 RID: 2032 RVA: 0x000ADEEC File Offset: 0x000AC0EC
	public GameObject CreatePrefab()
	{
		GameObject gameObject = HatchVeggieConfig.CreateHatch("HatchVeggieBaby", CREATURES.SPECIES.HATCH.VARIANT_VEGGIE.BABY.NAME, CREATURES.SPECIES.HATCH.VARIANT_VEGGIE.BABY.DESC, "baby_hatch_kanim", true);
		EntityTemplates.ExtendEntityToBeingABaby(gameObject, "HatchVeggie", null, false, 5f);
		return gameObject;
	}

	// Token: 0x060007F1 RID: 2033 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnPrefabInit(GameObject prefab)
	{
	}

	// Token: 0x060007F2 RID: 2034 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000608 RID: 1544
	public const string ID = "HatchVeggieBaby";
}
