using System;
using STRINGS;
using UnityEngine;

// Token: 0x02000246 RID: 582
[EntityConfigOrder(2)]
public class LightBugBabyConfig : IEntityConfig
{
	// Token: 0x0600081F RID: 2079 RVA: 0x0016AB94 File Offset: 0x00168D94
	public GameObject CreatePrefab()
	{
		GameObject gameObject = LightBugConfig.CreateLightBug("LightBugBaby", CREATURES.SPECIES.LIGHTBUG.BABY.NAME, CREATURES.SPECIES.LIGHTBUG.BABY.DESC, "baby_lightbug_kanim", true);
		EntityTemplates.ExtendEntityToBeingABaby(gameObject, "LightBug", null, false, 5f);
		gameObject.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.LightSource, false);
		return gameObject;
	}

	// Token: 0x06000820 RID: 2080 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnPrefabInit(GameObject prefab)
	{
	}

	// Token: 0x06000821 RID: 2081 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000628 RID: 1576
	public const string ID = "LightBugBaby";
}
