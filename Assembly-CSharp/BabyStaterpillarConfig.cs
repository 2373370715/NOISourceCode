using System;
using STRINGS;
using UnityEngine;

// Token: 0x02000275 RID: 629
[EntityConfigOrder(2)]
public class BabyStaterpillarConfig : IEntityConfig, IHasDlcRestrictions
{
	// Token: 0x0600090F RID: 2319 RVA: 0x000AA117 File Offset: 0x000A8317
	public string[] GetRequiredDlcIds()
	{
		return DlcManager.EXPANSION1;
	}

	// Token: 0x06000910 RID: 2320 RVA: 0x000AA765 File Offset: 0x000A8965
	public string[] GetForbiddenDlcIds()
	{
		return null;
	}

	// Token: 0x06000911 RID: 2321 RVA: 0x000AE90D File Offset: 0x000ACB0D
	public GameObject CreatePrefab()
	{
		GameObject gameObject = StaterpillarConfig.CreateStaterpillar("StaterpillarBaby", CREATURES.SPECIES.STATERPILLAR.BABY.NAME, CREATURES.SPECIES.STATERPILLAR.BABY.DESC, "baby_caterpillar_kanim", true);
		EntityTemplates.ExtendEntityToBeingABaby(gameObject, "Staterpillar", null, false, 5f);
		return gameObject;
	}

	// Token: 0x06000912 RID: 2322 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnPrefabInit(GameObject prefab)
	{
	}

	// Token: 0x06000913 RID: 2323 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x040006FA RID: 1786
	public const string ID = "StaterpillarBaby";
}
