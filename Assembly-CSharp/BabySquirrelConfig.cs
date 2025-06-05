using System;
using STRINGS;
using UnityEngine;

// Token: 0x02000271 RID: 625
[EntityConfigOrder(2)]
public class BabySquirrelConfig : IEntityConfig
{
	// Token: 0x060008F9 RID: 2297 RVA: 0x000AE827 File Offset: 0x000ACA27
	public GameObject CreatePrefab()
	{
		GameObject gameObject = SquirrelConfig.CreateSquirrel("SquirrelBaby", CREATURES.SPECIES.SQUIRREL.BABY.NAME, CREATURES.SPECIES.SQUIRREL.BABY.DESC, "baby_squirrel_kanim", true);
		EntityTemplates.ExtendEntityToBeingABaby(gameObject, "Squirrel", null, false, 5f);
		return gameObject;
	}

	// Token: 0x060008FA RID: 2298 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnPrefabInit(GameObject prefab)
	{
	}

	// Token: 0x060008FB RID: 2299 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x040006E7 RID: 1767
	public const string ID = "SquirrelBaby";
}
