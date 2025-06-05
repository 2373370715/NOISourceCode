using System;
using STRINGS;
using UnityEngine;

// Token: 0x02000106 RID: 262
[EntityConfigOrder(2)]
public class BabyBeeConfig : IEntityConfig, IHasDlcRestrictions
{
	// Token: 0x0600040D RID: 1037 RVA: 0x000AA117 File Offset: 0x000A8317
	public string[] GetRequiredDlcIds()
	{
		return DlcManager.EXPANSION1;
	}

	// Token: 0x0600040E RID: 1038 RVA: 0x000AA765 File Offset: 0x000A8965
	public string[] GetForbiddenDlcIds()
	{
		return null;
	}

	// Token: 0x0600040F RID: 1039 RVA: 0x0015DEB4 File Offset: 0x0015C0B4
	public GameObject CreatePrefab()
	{
		GameObject gameObject = BeeConfig.CreateBee("BeeBaby", CREATURES.SPECIES.BEE.BABY.NAME, CREATURES.SPECIES.BEE.BABY.DESC, "baby_blarva_kanim", true);
		EntityTemplates.ExtendEntityToBeingABaby(gameObject, "Bee", null, true, 2f);
		gameObject.GetComponent<KPrefabID>().AddTag(GameTags.Creatures.Walker, false);
		return gameObject;
	}

	// Token: 0x06000410 RID: 1040 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnPrefabInit(GameObject prefab)
	{
	}

	// Token: 0x06000411 RID: 1041 RVA: 0x000AB5D8 File Offset: 0x000A97D8
	public void OnSpawn(GameObject inst)
	{
		BaseBeeConfig.SetupLoopingSounds(inst);
	}

	// Token: 0x040002E7 RID: 743
	public const string ID = "BeeBaby";
}
