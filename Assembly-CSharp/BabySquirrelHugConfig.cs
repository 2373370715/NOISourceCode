using System;
using STRINGS;
using UnityEngine;

// Token: 0x02000273 RID: 627
[EntityConfigOrder(2)]
public class BabySquirrelHugConfig : IEntityConfig
{
	// Token: 0x06000903 RID: 2307 RVA: 0x000AE89B File Offset: 0x000ACA9B
	public GameObject CreatePrefab()
	{
		GameObject gameObject = SquirrelHugConfig.CreateSquirrelHug("SquirrelHugBaby", CREATURES.SPECIES.SQUIRREL.VARIANT_HUG.BABY.NAME, CREATURES.SPECIES.SQUIRREL.VARIANT_HUG.BABY.DESC, "baby_squirrel_kanim", true);
		EntityTemplates.ExtendEntityToBeingABaby(gameObject, "SquirrelHug", null, false, 5f);
		return gameObject;
	}

	// Token: 0x06000904 RID: 2308 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnPrefabInit(GameObject prefab)
	{
	}

	// Token: 0x06000905 RID: 2309 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x040006F3 RID: 1779
	public const string ID = "SquirrelHugBaby";
}
