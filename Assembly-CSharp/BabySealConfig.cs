using System;
using STRINGS;
using UnityEngine;

// Token: 0x0200026E RID: 622
[EntityConfigOrder(2)]
public class BabySealConfig : IEntityConfig, IHasDlcRestrictions
{
	// Token: 0x060008E9 RID: 2281 RVA: 0x000AA536 File Offset: 0x000A8736
	public string[] GetRequiredDlcIds()
	{
		return DlcManager.DLC2;
	}

	// Token: 0x060008EA RID: 2282 RVA: 0x000AA765 File Offset: 0x000A8965
	public string[] GetForbiddenDlcIds()
	{
		return null;
	}

	// Token: 0x060008EB RID: 2283 RVA: 0x000AE7B3 File Offset: 0x000AC9B3
	public GameObject CreatePrefab()
	{
		GameObject gameObject = SealConfig.CreateSeal("SealBaby", CREATURES.SPECIES.SEAL.BABY.NAME, CREATURES.SPECIES.SEAL.BABY.DESC, "baby_seal_kanim", true);
		EntityTemplates.ExtendEntityToBeingABaby(gameObject, "Seal", null, false, 5f);
		return gameObject;
	}

	// Token: 0x060008EC RID: 2284 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnPrefabInit(GameObject prefab)
	{
	}

	// Token: 0x060008ED RID: 2285 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x040006DA RID: 1754
	public const string ID = "SealBaby";
}
