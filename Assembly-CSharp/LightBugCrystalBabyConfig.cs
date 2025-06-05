using System;
using STRINGS;
using UnityEngine;

// Token: 0x02000248 RID: 584
[EntityConfigOrder(2)]
public class LightBugCrystalBabyConfig : IEntityConfig
{
	// Token: 0x06000829 RID: 2089 RVA: 0x000AE0B2 File Offset: 0x000AC2B2
	public GameObject CreatePrefab()
	{
		GameObject gameObject = LightBugCrystalConfig.CreateLightBug("LightBugCrystalBaby", CREATURES.SPECIES.LIGHTBUG.VARIANT_CRYSTAL.BABY.NAME, CREATURES.SPECIES.LIGHTBUG.VARIANT_CRYSTAL.BABY.DESC, "baby_lightbug_kanim", true);
		EntityTemplates.ExtendEntityToBeingABaby(gameObject, "LightBugCrystal", null, false, 5f);
		return gameObject;
	}

	// Token: 0x0600082A RID: 2090 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnPrefabInit(GameObject prefab)
	{
	}

	// Token: 0x0600082B RID: 2091 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x0400062F RID: 1583
	public const string ID = "LightBugCrystalBaby";
}
