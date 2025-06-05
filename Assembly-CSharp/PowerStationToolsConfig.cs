using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x0200039C RID: 924
public class PowerStationToolsConfig : IEntityConfig
{
	// Token: 0x06000EE3 RID: 3811 RVA: 0x00184FA0 File Offset: 0x001831A0
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateLooseEntity("PowerStationTools", ITEMS.INDUSTRIAL_PRODUCTS.POWER_STATION_TOOLS.NAME, ITEMS.INDUSTRIAL_PRODUCTS.POWER_STATION_TOOLS.DESC, 5f, true, Assets.GetAnim("kit_electrician_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.8f, 0.6f, true, 0, SimHashes.Creature, new List<Tag>
		{
			GameTags.IndustrialProduct,
			GameTags.MiscPickupable
		});
		gameObject.AddOrGet<EntitySplitter>();
		return gameObject;
	}

	// Token: 0x06000EE4 RID: 3812 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000EE5 RID: 3813 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000B02 RID: 2818
	public const string ID = "PowerStationTools";

	// Token: 0x04000B03 RID: 2819
	public static readonly Tag tag = TagManager.Create("PowerStationTools");

	// Token: 0x04000B04 RID: 2820
	public const float MASS = 5f;
}
