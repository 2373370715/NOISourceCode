using System;
using STRINGS;
using UnityEngine;

// Token: 0x02000228 RID: 552
[EntityConfigOrder(2)]
public class BabyCrabWoodConfig : IEntityConfig
{
	// Token: 0x0600077F RID: 1919 RVA: 0x00168934 File Offset: 0x00166B34
	public GameObject CreatePrefab()
	{
		GameObject gameObject = CrabWoodConfig.CreateCrabWood("CrabWoodBaby", CREATURES.SPECIES.CRAB.VARIANT_WOOD.BABY.NAME, CREATURES.SPECIES.CRAB.VARIANT_WOOD.BABY.DESC, "baby_pincher_kanim", true, "BabyCrabWoodShell");
		EntityTemplates.ExtendEntityToBeingABaby(gameObject, "CrabWood", "BabyCrabWoodShell", false, 5f);
		return gameObject;
	}

	// Token: 0x06000780 RID: 1920 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnPrefabInit(GameObject prefab)
	{
	}

	// Token: 0x06000781 RID: 1921 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000598 RID: 1432
	public const string ID = "CrabWoodBaby";
}
