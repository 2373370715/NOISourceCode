using System;
using STRINGS;
using UnityEngine;

// Token: 0x0200024C RID: 588
[EntityConfigOrder(2)]
public class LightBugPinkBabyConfig : IEntityConfig
{
	// Token: 0x0600083D RID: 2109 RVA: 0x000AE17E File Offset: 0x000AC37E
	public GameObject CreatePrefab()
	{
		GameObject gameObject = LightBugPinkConfig.CreateLightBug("LightBugPinkBaby", CREATURES.SPECIES.LIGHTBUG.VARIANT_PINK.BABY.NAME, CREATURES.SPECIES.LIGHTBUG.VARIANT_PINK.BABY.DESC, "baby_lightbug_kanim", true);
		EntityTemplates.ExtendEntityToBeingABaby(gameObject, "LightBugPink", null, false, 5f);
		return gameObject;
	}

	// Token: 0x0600083E RID: 2110 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnPrefabInit(GameObject prefab)
	{
	}

	// Token: 0x0600083F RID: 2111 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x0400063D RID: 1597
	public const string ID = "LightBugPinkBaby";
}
