using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020002AA RID: 682
public class GingerConfig : IEntityConfig
{
	// Token: 0x060009ED RID: 2541 RVA: 0x00172A4C File Offset: 0x00170C4C
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateLooseEntity(GingerConfig.ID, STRINGS.ITEMS.INGREDIENTS.GINGER.NAME, STRINGS.ITEMS.INGREDIENTS.GINGER.DESC, 1f, true, Assets.GetAnim("ginger_kanim"), "object", Grid.SceneLayer.BuildingBack, EntityTemplates.CollisionShape.RECTANGLE, 0.45f, 0.4f, true, TUNING.SORTORDER.BUILDINGELEMENTS + GingerConfig.SORTORDER, SimHashes.Creature, new List<Tag>
		{
			GameTags.IndustrialIngredient
		});
		gameObject.AddOrGet<EntitySplitter>();
		return gameObject;
	}

	// Token: 0x060009EE RID: 2542 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x060009EF RID: 2543 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x040007DF RID: 2015
	public static string ID = "GingerConfig";

	// Token: 0x040007E0 RID: 2016
	public static int SORTORDER = 1;
}
