using System;
using STRINGS;
using UnityEngine;

// Token: 0x02000252 RID: 594
[EntityConfigOrder(2)]
public class BabyMoleDelicacyConfig : IEntityConfig
{
	// Token: 0x0600085D RID: 2141 RVA: 0x000AE290 File Offset: 0x000AC490
	public GameObject CreatePrefab()
	{
		GameObject gameObject = MoleDelicacyConfig.CreateMole("MoleDelicacyBaby", CREATURES.SPECIES.MOLE.VARIANT_DELICACY.BABY.NAME, CREATURES.SPECIES.MOLE.VARIANT_DELICACY.BABY.DESC, "baby_driller_kanim", true);
		EntityTemplates.ExtendEntityToBeingABaby(gameObject, "MoleDelicacy", null, false, 5f);
		return gameObject;
	}

	// Token: 0x0600085E RID: 2142 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnPrefabInit(GameObject prefab)
	{
	}

	// Token: 0x0600085F RID: 2143 RVA: 0x000AE222 File Offset: 0x000AC422
	public void OnSpawn(GameObject inst)
	{
		MoleConfig.SetSpawnNavType(inst);
	}

	// Token: 0x04000659 RID: 1625
	public const string ID = "MoleDelicacyBaby";
}
