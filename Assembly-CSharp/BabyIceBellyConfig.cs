using System;
using Klei.AI;
using STRINGS;
using UnityEngine;

// Token: 0x0200023F RID: 575
[EntityConfigOrder(2)]
public class BabyIceBellyConfig : IEntityConfig, IHasDlcRestrictions
{
	// Token: 0x060007FC RID: 2044 RVA: 0x000AA536 File Offset: 0x000A8736
	public string[] GetRequiredDlcIds()
	{
		return DlcManager.DLC2;
	}

	// Token: 0x060007FD RID: 2045 RVA: 0x000AA765 File Offset: 0x000A8965
	public string[] GetForbiddenDlcIds()
	{
		return null;
	}

	// Token: 0x060007FE RID: 2046 RVA: 0x0016A494 File Offset: 0x00168694
	public GameObject CreatePrefab()
	{
		GameObject gameObject = IceBellyConfig.CreateIceBelly("IceBellyBaby", CREATURES.SPECIES.ICEBELLY.BABY.NAME, CREATURES.SPECIES.ICEBELLY.BABY.DESC, "baby_icebelly_kanim", true);
		EntityTemplates.ExtendEntityToBeingABaby(gameObject, "IceBelly", null, false, 5f).AddOrGetDef<BabyMonitor.Def>().configureAdultOnMaturation = delegate(GameObject go)
		{
			AmountInstance amountInstance = Db.Get().Amounts.ScaleGrowth.Lookup(go);
			amountInstance.value = amountInstance.GetMax() * IceBellyConfig.SCALE_INITIAL_GROWTH_PCT;
		};
		return gameObject;
	}

	// Token: 0x060007FF RID: 2047 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnPrefabInit(GameObject prefab)
	{
	}

	// Token: 0x06000800 RID: 2048 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000611 RID: 1553
	public const string ID = "IceBellyBaby";
}
