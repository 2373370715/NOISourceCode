using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x0200032D RID: 813
public class TableSaltConfig : IEntityConfig
{
	// Token: 0x06000CB1 RID: 3249 RVA: 0x0017AEFC File Offset: 0x001790FC
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateLooseEntity(TableSaltConfig.ID, STRINGS.ITEMS.INDUSTRIAL_PRODUCTS.TABLE_SALT.NAME, STRINGS.ITEMS.INDUSTRIAL_PRODUCTS.TABLE_SALT.DESC, 1f, false, Assets.GetAnim("seed_saltPlant_kanim"), "object", Grid.SceneLayer.BuildingBack, EntityTemplates.CollisionShape.RECTANGLE, 0.8f, 0.45f, true, SORTORDER.BUILDINGELEMENTS + TableSaltTuning.SORTORDER, SimHashes.Salt, new List<Tag>
		{
			GameTags.Other,
			GameTags.Experimental
		});
		gameObject.AddOrGet<EntitySplitter>();
		return gameObject;
	}

	// Token: 0x06000CB2 RID: 3250 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnPrefabInit(GameObject go)
	{
	}

	// Token: 0x06000CB3 RID: 3251 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x0400098B RID: 2443
	public static string ID = "TableSalt";
}
