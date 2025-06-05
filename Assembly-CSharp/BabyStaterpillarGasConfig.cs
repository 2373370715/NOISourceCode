using System;
using STRINGS;
using UnityEngine;

// Token: 0x02000277 RID: 631
[EntityConfigOrder(2)]
public class BabyStaterpillarGasConfig : IEntityConfig, IHasDlcRestrictions
{
	// Token: 0x0600091D RID: 2333 RVA: 0x000AA117 File Offset: 0x000A8317
	public string[] GetRequiredDlcIds()
	{
		return DlcManager.EXPANSION1;
	}

	// Token: 0x0600091E RID: 2334 RVA: 0x000AA765 File Offset: 0x000A8965
	public string[] GetForbiddenDlcIds()
	{
		return null;
	}

	// Token: 0x0600091F RID: 2335 RVA: 0x000AE974 File Offset: 0x000ACB74
	public GameObject CreatePrefab()
	{
		GameObject gameObject = StaterpillarGasConfig.CreateStaterpillarGas("StaterpillarGasBaby", CREATURES.SPECIES.STATERPILLAR.VARIANT_GAS.BABY.NAME, CREATURES.SPECIES.STATERPILLAR.VARIANT_GAS.BABY.DESC, "baby_caterpillar_kanim", true);
		EntityTemplates.ExtendEntityToBeingABaby(gameObject, "StaterpillarGas", null, false, 5f);
		return gameObject;
	}

	// Token: 0x06000920 RID: 2336 RVA: 0x000AE9B2 File Offset: 0x000ACBB2
	public void OnPrefabInit(GameObject prefab)
	{
		prefab.GetComponent<KBatchedAnimController>().SetSymbolVisiblity("electric_bolt_c_bloom", false);
	}

	// Token: 0x06000921 RID: 2337 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000706 RID: 1798
	public const string ID = "StaterpillarGasBaby";
}
