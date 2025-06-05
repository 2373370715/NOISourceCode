using System;
using STRINGS;
using UnityEngine;

// Token: 0x0200025F RID: 607
[EntityConfigOrder(2)]
public class BabyPacuConfig : IEntityConfig
{
	// Token: 0x0600089B RID: 2203 RVA: 0x000AE4D7 File Offset: 0x000AC6D7
	public GameObject CreatePrefab()
	{
		GameObject gameObject = PacuConfig.CreatePacu("PacuBaby", CREATURES.SPECIES.PACU.BABY.NAME, CREATURES.SPECIES.PACU.BABY.DESC, "baby_pacu_kanim", true);
		EntityTemplates.ExtendEntityToBeingABaby(gameObject, "Pacu", null, false, 5f);
		return gameObject;
	}

	// Token: 0x0600089C RID: 2204 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnPrefabInit(GameObject prefab)
	{
	}

	// Token: 0x0600089D RID: 2205 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000691 RID: 1681
	public const string ID = "PacuBaby";
}
