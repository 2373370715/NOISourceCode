using System;
using STRINGS;
using UnityEngine;

// Token: 0x0200025A RID: 602
[EntityConfigOrder(2)]
public class OilFloaterHighTempBabyConfig : IEntityConfig
{
	// Token: 0x06000886 RID: 2182 RVA: 0x000AE439 File Offset: 0x000AC639
	public GameObject CreatePrefab()
	{
		GameObject gameObject = OilFloaterHighTempConfig.CreateOilFloater("OilfloaterHighTempBaby", CREATURES.SPECIES.OILFLOATER.VARIANT_HIGHTEMP.BABY.NAME, CREATURES.SPECIES.OILFLOATER.VARIANT_HIGHTEMP.BABY.DESC, "baby_oilfloater_kanim", true);
		EntityTemplates.ExtendEntityToBeingABaby(gameObject, "OilfloaterHighTemp", null, false, 5f);
		return gameObject;
	}

	// Token: 0x06000887 RID: 2183 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnPrefabInit(GameObject prefab)
	{
	}

	// Token: 0x06000888 RID: 2184 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000683 RID: 1667
	public const string ID = "OilfloaterHighTempBaby";
}
