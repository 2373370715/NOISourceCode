using System;
using STRINGS;
using UnityEngine;

// Token: 0x02000242 RID: 578
[EntityConfigOrder(2)]
public class LightBugBlackBabyConfig : IEntityConfig
{
	// Token: 0x0600080B RID: 2059 RVA: 0x000ADFC3 File Offset: 0x000AC1C3
	public GameObject CreatePrefab()
	{
		GameObject gameObject = LightBugBlackConfig.CreateLightBug("LightBugBlackBaby", CREATURES.SPECIES.LIGHTBUG.VARIANT_BLACK.BABY.NAME, CREATURES.SPECIES.LIGHTBUG.VARIANT_BLACK.BABY.DESC, "baby_lightbug_kanim", true);
		EntityTemplates.ExtendEntityToBeingABaby(gameObject, "LightBugBlack", null, false, 5f);
		return gameObject;
	}

	// Token: 0x0600080C RID: 2060 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnPrefabInit(GameObject prefab)
	{
	}

	// Token: 0x0600080D RID: 2061 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x0400061A RID: 1562
	public const string ID = "LightBugBlackBaby";
}
