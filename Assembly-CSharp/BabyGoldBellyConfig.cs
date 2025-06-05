using System;
using Klei.AI;
using STRINGS;
using UnityEngine;

// Token: 0x02000234 RID: 564
[EntityConfigOrder(2)]
public class BabyGoldBellyConfig : IEntityConfig, IHasDlcRestrictions
{
	// Token: 0x060007C2 RID: 1986 RVA: 0x000AA536 File Offset: 0x000A8736
	public string[] GetRequiredDlcIds()
	{
		return DlcManager.DLC2;
	}

	// Token: 0x060007C3 RID: 1987 RVA: 0x000AA765 File Offset: 0x000A8965
	public string[] GetForbiddenDlcIds()
	{
		return null;
	}

	// Token: 0x060007C4 RID: 1988 RVA: 0x00169A34 File Offset: 0x00167C34
	public GameObject CreatePrefab()
	{
		GameObject gameObject = GoldBellyConfig.CreateGoldBelly("GoldBellyBaby", CREATURES.SPECIES.ICEBELLY.VARIANT_GOLD.BABY.NAME, CREATURES.SPECIES.ICEBELLY.VARIANT_GOLD.BABY.DESC, "baby_icebelly_kanim", true);
		EntityTemplates.ExtendEntityToBeingABaby(gameObject, "GoldBelly", null, false, 5f).AddOrGetDef<BabyMonitor.Def>().configureAdultOnMaturation = delegate(GameObject go)
		{
			AmountInstance amountInstance = Db.Get().Amounts.ScaleGrowth.Lookup(go);
			amountInstance.value = amountInstance.GetMax() * GoldBellyConfig.SCALE_INITIAL_GROWTH_PCT;
		};
		return gameObject;
	}

	// Token: 0x060007C5 RID: 1989 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnPrefabInit(GameObject prefab)
	{
	}

	// Token: 0x060007C6 RID: 1990 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x040005E3 RID: 1507
	public const string ID = "GoldBellyBaby";
}
