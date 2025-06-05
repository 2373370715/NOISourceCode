using System;
using STRINGS;
using UnityEngine;

// Token: 0x0200024A RID: 586
[EntityConfigOrder(2)]
public class LightBugOrangeBabyConfig : IEntityConfig
{
	// Token: 0x06000833 RID: 2099 RVA: 0x000AE118 File Offset: 0x000AC318
	public GameObject CreatePrefab()
	{
		GameObject gameObject = LightBugOrangeConfig.CreateLightBug("LightBugOrangeBaby", CREATURES.SPECIES.LIGHTBUG.VARIANT_ORANGE.BABY.NAME, CREATURES.SPECIES.LIGHTBUG.VARIANT_ORANGE.BABY.DESC, "baby_lightbug_kanim", true);
		EntityTemplates.ExtendEntityToBeingABaby(gameObject, "LightBugOrange", null, false, 5f);
		return gameObject;
	}

	// Token: 0x06000834 RID: 2100 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnPrefabInit(GameObject prefab)
	{
	}

	// Token: 0x06000835 RID: 2101 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000636 RID: 1590
	public const string ID = "LightBugOrangeBaby";
}
