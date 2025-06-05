using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020002DA RID: 730
public class SuperWormPlantConfig : IEntityConfig, IHasDlcRestrictions
{
	// Token: 0x06000B2B RID: 2859 RVA: 0x000AA117 File Offset: 0x000A8317
	public string[] GetRequiredDlcIds()
	{
		return DlcManager.EXPANSION1;
	}

	// Token: 0x06000B2C RID: 2860 RVA: 0x000AA765 File Offset: 0x000A8965
	public string[] GetForbiddenDlcIds()
	{
		return null;
	}

	// Token: 0x06000B2D RID: 2861 RVA: 0x001779DC File Offset: 0x00175BDC
	public GameObject CreatePrefab()
	{
		GameObject gameObject = WormPlantConfig.BaseWormPlant("SuperWormPlant", STRINGS.CREATURES.SPECIES.SUPERWORMPLANT.NAME, STRINGS.CREATURES.SPECIES.SUPERWORMPLANT.DESC, "wormwood_kanim", SuperWormPlantConfig.SUPER_DECOR, "WormSuperFruit");
		gameObject.AddOrGet<SeedProducer>().Configure("WormPlantSeed", SeedProducer.ProductionType.Harvest, 1);
		return gameObject;
	}

	// Token: 0x06000B2E RID: 2862 RVA: 0x00177A28 File Offset: 0x00175C28
	public void OnPrefabInit(GameObject prefab)
	{
		TransformingPlant transformingPlant = prefab.AddOrGet<TransformingPlant>();
		transformingPlant.SubscribeToTransformEvent(GameHashes.HarvestComplete);
		transformingPlant.transformPlantId = "WormPlant";
		prefab.GetComponent<KAnimControllerBase>().SetSymbolVisiblity("flower", false);
		prefab.AddOrGet<StandardCropPlant>().anims = SuperWormPlantConfig.animSet;
	}

	// Token: 0x06000B2F RID: 2863 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x040008CE RID: 2254
	public const string ID = "SuperWormPlant";

	// Token: 0x040008CF RID: 2255
	public static readonly EffectorValues SUPER_DECOR = DECOR.BONUS.TIER1;

	// Token: 0x040008D0 RID: 2256
	public const string SUPER_CROP_ID = "WormSuperFruit";

	// Token: 0x040008D1 RID: 2257
	public const int CROP_YIELD = 8;

	// Token: 0x040008D2 RID: 2258
	private static StandardCropPlant.AnimSet animSet = new StandardCropPlant.AnimSet
	{
		grow = "super_grow",
		grow_pst = "super_grow_pst",
		idle_full = "super_idle_full",
		wilt_base = "super_wilt",
		harvest = "super_harvest"
	};
}
