using System;
using Klei.AI;
using STRINGS;
using UnityEngine;

// Token: 0x0200027C RID: 636
[EntityConfigOrder(2)]
public class BabyWoodDeerConfig : IEntityConfig, IHasDlcRestrictions
{
	// Token: 0x0600093C RID: 2364 RVA: 0x000AA536 File Offset: 0x000A8736
	public string[] GetRequiredDlcIds()
	{
		return DlcManager.DLC2;
	}

	// Token: 0x0600093D RID: 2365 RVA: 0x000AA765 File Offset: 0x000A8965
	public string[] GetForbiddenDlcIds()
	{
		return null;
	}

	// Token: 0x0600093E RID: 2366 RVA: 0x0016E7D4 File Offset: 0x0016C9D4
	public GameObject CreatePrefab()
	{
		GameObject gameObject = WoodDeerConfig.CreateWoodDeer("WoodDeerBaby", CREATURES.SPECIES.WOODDEER.BABY.NAME, CREATURES.SPECIES.WOODDEER.BABY.DESC, "baby_ice_floof_kanim", true);
		EntityTemplates.ExtendEntityToBeingABaby(gameObject, "WoodDeer", null, false, 5f).AddOrGetDef<BabyMonitor.Def>().configureAdultOnMaturation = delegate(GameObject go)
		{
			AmountInstance amountInstance = Db.Get().Amounts.ScaleGrowth.Lookup(go);
			amountInstance.value = amountInstance.GetMax() * WoodDeerConfig.ANTLER_STARTING_GROWTH_PCT;
		};
		return gameObject;
	}

	// Token: 0x0600093F RID: 2367 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnPrefabInit(GameObject prefab)
	{
	}

	// Token: 0x06000940 RID: 2368 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000725 RID: 1829
	public const string ID = "WoodDeerBaby";
}
