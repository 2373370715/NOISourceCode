using System;
using STRINGS;
using UnityEngine;

// Token: 0x02000258 RID: 600
[EntityConfigOrder(2)]
public class OilFloaterDecorBabyConfig : IEntityConfig
{
	// Token: 0x0600087C RID: 2172 RVA: 0x000AE3C9 File Offset: 0x000AC5C9
	public GameObject CreatePrefab()
	{
		GameObject gameObject = OilFloaterDecorConfig.CreateOilFloater("OilfloaterDecorBaby", CREATURES.SPECIES.OILFLOATER.VARIANT_DECOR.BABY.NAME, CREATURES.SPECIES.OILFLOATER.VARIANT_DECOR.BABY.DESC, "baby_oilfloater_kanim", true);
		EntityTemplates.ExtendEntityToBeingABaby(gameObject, "OilfloaterDecor", null, false, 5f);
		return gameObject;
	}

	// Token: 0x0600087D RID: 2173 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnPrefabInit(GameObject prefab)
	{
	}

	// Token: 0x0600087E RID: 2174 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000679 RID: 1657
	public const string ID = "OilfloaterDecorBaby";
}
