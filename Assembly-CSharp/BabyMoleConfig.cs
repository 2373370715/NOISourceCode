using System;
using STRINGS;
using UnityEngine;

// Token: 0x02000250 RID: 592
[EntityConfigOrder(2)]
public class BabyMoleConfig : IEntityConfig
{
	// Token: 0x06000852 RID: 2130 RVA: 0x000AE24A File Offset: 0x000AC44A
	public GameObject CreatePrefab()
	{
		GameObject gameObject = MoleConfig.CreateMole("MoleBaby", CREATURES.SPECIES.MOLE.BABY.NAME, CREATURES.SPECIES.MOLE.BABY.DESC, "baby_driller_kanim", true);
		EntityTemplates.ExtendEntityToBeingABaby(gameObject, "Mole", null, false, 5f);
		return gameObject;
	}

	// Token: 0x06000853 RID: 2131 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnPrefabInit(GameObject prefab)
	{
	}

	// Token: 0x06000854 RID: 2132 RVA: 0x000AE222 File Offset: 0x000AC422
	public void OnSpawn(GameObject inst)
	{
		MoleConfig.SetSpawnNavType(inst);
	}

	// Token: 0x0400064B RID: 1611
	public const string ID = "MoleBaby";
}
