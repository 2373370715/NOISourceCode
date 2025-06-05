using System;
using STRINGS;
using UnityEngine;

// Token: 0x0200024E RID: 590
[EntityConfigOrder(2)]
public class LightBugPurpleBabyConfig : IEntityConfig
{
	// Token: 0x06000847 RID: 2119 RVA: 0x000AE1E4 File Offset: 0x000AC3E4
	public GameObject CreatePrefab()
	{
		GameObject gameObject = LightBugPurpleConfig.CreateLightBug("LightBugPurpleBaby", CREATURES.SPECIES.LIGHTBUG.VARIANT_PURPLE.BABY.NAME, CREATURES.SPECIES.LIGHTBUG.VARIANT_PURPLE.BABY.DESC, "baby_lightbug_kanim", true);
		EntityTemplates.ExtendEntityToBeingABaby(gameObject, "LightBugPurple", null, false, 5f);
		return gameObject;
	}

	// Token: 0x06000848 RID: 2120 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnPrefabInit(GameObject prefab)
	{
	}

	// Token: 0x06000849 RID: 2121 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000644 RID: 1604
	public const string ID = "LightBugPurpleBaby";
}
