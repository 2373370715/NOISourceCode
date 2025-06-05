using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x02000399 RID: 921
public class FarmStationToolsConfig : IEntityConfig
{
	// Token: 0x06000ED2 RID: 3794 RVA: 0x00184DE8 File Offset: 0x00182FE8
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateLooseEntity("FarmStationTools", ITEMS.INDUSTRIAL_PRODUCTS.FARM_STATION_TOOLS.NAME, ITEMS.INDUSTRIAL_PRODUCTS.FARM_STATION_TOOLS.DESC, 5f, true, Assets.GetAnim("kit_planttender_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.8f, 0.6f, true, 0, SimHashes.Creature, new List<Tag>
		{
			GameTags.MiscPickupable
		});
		gameObject.AddOrGet<EntitySplitter>();
		return gameObject;
	}

	// Token: 0x06000ED3 RID: 3795 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000ED4 RID: 3796 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000AF9 RID: 2809
	public const string ID = "FarmStationTools";

	// Token: 0x04000AFA RID: 2810
	public static readonly Tag tag = TagManager.Create("FarmStationTools");

	// Token: 0x04000AFB RID: 2811
	public const float MASS = 5f;
}
