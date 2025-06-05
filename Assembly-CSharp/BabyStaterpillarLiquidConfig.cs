using System;
using STRINGS;
using UnityEngine;

// Token: 0x02000279 RID: 633
[EntityConfigOrder(2)]
public class BabyStaterpillarLiquidConfig : IEntityConfig, IHasDlcRestrictions
{
	// Token: 0x0600092B RID: 2347 RVA: 0x000AA117 File Offset: 0x000A8317
	public string[] GetRequiredDlcIds()
	{
		return DlcManager.EXPANSION1;
	}

	// Token: 0x0600092C RID: 2348 RVA: 0x000AA765 File Offset: 0x000A8965
	public string[] GetForbiddenDlcIds()
	{
		return null;
	}

	// Token: 0x0600092D RID: 2349 RVA: 0x000AE9CA File Offset: 0x000ACBCA
	public GameObject CreatePrefab()
	{
		GameObject gameObject = StaterpillarLiquidConfig.CreateStaterpillarLiquid("StaterpillarLiquidBaby", CREATURES.SPECIES.STATERPILLAR.VARIANT_LIQUID.BABY.NAME, CREATURES.SPECIES.STATERPILLAR.VARIANT_LIQUID.BABY.DESC, "baby_caterpillar_kanim", true);
		EntityTemplates.ExtendEntityToBeingABaby(gameObject, "StaterpillarLiquid", null, false, 5f);
		return gameObject;
	}

	// Token: 0x0600092E RID: 2350 RVA: 0x000AE9B2 File Offset: 0x000ACBB2
	public void OnPrefabInit(GameObject prefab)
	{
		prefab.GetComponent<KBatchedAnimController>().SetSymbolVisiblity("electric_bolt_c_bloom", false);
	}

	// Token: 0x0600092F RID: 2351 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000712 RID: 1810
	public const string ID = "StaterpillarLiquidBaby";
}
