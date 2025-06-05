using System;
using STRINGS;
using UnityEngine;

// Token: 0x0200022A RID: 554
[EntityConfigOrder(2)]
public class BabyDivergentBeetleConfig : IEntityConfig, IHasDlcRestrictions
{
	// Token: 0x0600078B RID: 1931 RVA: 0x000AA117 File Offset: 0x000A8317
	public string[] GetRequiredDlcIds()
	{
		return DlcManager.EXPANSION1;
	}

	// Token: 0x0600078C RID: 1932 RVA: 0x000AA765 File Offset: 0x000A8965
	public string[] GetForbiddenDlcIds()
	{
		return null;
	}

	// Token: 0x0600078D RID: 1933 RVA: 0x000ADC38 File Offset: 0x000ABE38
	public GameObject CreatePrefab()
	{
		GameObject gameObject = DivergentBeetleConfig.CreateDivergentBeetle("DivergentBeetleBaby", CREATURES.SPECIES.DIVERGENT.VARIANT_BEETLE.BABY.NAME, CREATURES.SPECIES.DIVERGENT.VARIANT_BEETLE.BABY.DESC, "baby_critter_kanim", true);
		EntityTemplates.ExtendEntityToBeingABaby(gameObject, "DivergentBeetle", null, false, 5f);
		return gameObject;
	}

	// Token: 0x0600078E RID: 1934 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnPrefabInit(GameObject prefab)
	{
	}

	// Token: 0x0600078F RID: 1935 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x040005A2 RID: 1442
	public const string ID = "DivergentBeetleBaby";
}
