using System;
using STRINGS;
using UnityEngine;

// Token: 0x02000256 RID: 598
[EntityConfigOrder(2)]
public class OilFloaterBabyConfig : IEntityConfig
{
	// Token: 0x06000872 RID: 2162 RVA: 0x000AE363 File Offset: 0x000AC563
	public GameObject CreatePrefab()
	{
		GameObject gameObject = OilFloaterConfig.CreateOilFloater("OilfloaterBaby", CREATURES.SPECIES.OILFLOATER.BABY.NAME, CREATURES.SPECIES.OILFLOATER.BABY.DESC, "baby_oilfloater_kanim", true);
		EntityTemplates.ExtendEntityToBeingABaby(gameObject, "Oilfloater", null, false, 5f);
		return gameObject;
	}

	// Token: 0x06000873 RID: 2163 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnPrefabInit(GameObject prefab)
	{
	}

	// Token: 0x06000874 RID: 2164 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000671 RID: 1649
	public const string ID = "OilfloaterBaby";
}
