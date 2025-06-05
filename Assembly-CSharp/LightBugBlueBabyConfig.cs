using System;
using STRINGS;
using UnityEngine;

// Token: 0x02000244 RID: 580
[EntityConfigOrder(2)]
public class LightBugBlueBabyConfig : IEntityConfig
{
	// Token: 0x06000815 RID: 2069 RVA: 0x000AE029 File Offset: 0x000AC229
	public GameObject CreatePrefab()
	{
		GameObject gameObject = LightBugBlueConfig.CreateLightBug("LightBugBlueBaby", CREATURES.SPECIES.LIGHTBUG.VARIANT_BLUE.BABY.NAME, CREATURES.SPECIES.LIGHTBUG.VARIANT_BLUE.BABY.DESC, "baby_lightbug_kanim", true);
		EntityTemplates.ExtendEntityToBeingABaby(gameObject, "LightBugBlue", null, false, 5f);
		return gameObject;
	}

	// Token: 0x06000816 RID: 2070 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnPrefabInit(GameObject prefab)
	{
	}

	// Token: 0x06000817 RID: 2071 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000621 RID: 1569
	public const string ID = "LightBugBlueBaby";
}
