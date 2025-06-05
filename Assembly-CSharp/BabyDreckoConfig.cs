using System;
using STRINGS;
using UnityEngine;

// Token: 0x0200022E RID: 558
[EntityConfigOrder(2)]
public class BabyDreckoConfig : IEntityConfig
{
	// Token: 0x060007A5 RID: 1957 RVA: 0x000ADCB4 File Offset: 0x000ABEB4
	public GameObject CreatePrefab()
	{
		GameObject gameObject = DreckoConfig.CreateDrecko("DreckoBaby", CREATURES.SPECIES.DRECKO.BABY.NAME, CREATURES.SPECIES.DRECKO.BABY.DESC, "baby_drecko_kanim", true);
		EntityTemplates.ExtendEntityToBeingABaby(gameObject, "Drecko", null, false, 5f);
		return gameObject;
	}

	// Token: 0x060007A6 RID: 1958 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnPrefabInit(GameObject prefab)
	{
	}

	// Token: 0x060007A7 RID: 1959 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x040005BF RID: 1471
	public const string ID = "DreckoBaby";
}
