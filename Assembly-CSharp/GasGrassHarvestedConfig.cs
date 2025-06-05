using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x0200047E RID: 1150
public class GasGrassHarvestedConfig : IEntityConfig
{
	// Token: 0x06001391 RID: 5009 RVA: 0x00198C80 File Offset: 0x00196E80
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateLooseEntity("GasGrassHarvested", CREATURES.SPECIES.GASGRASS.NAME, CREATURES.SPECIES.GASGRASS.DESC, 1f, false, Assets.GetAnim("harvested_gassygrass_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.CIRCLE, 0.25f, 0.25f, true, 0, SimHashes.Creature, new List<Tag>
		{
			GameTags.Other
		});
		gameObject.AddOrGet<EntitySplitter>();
		return gameObject;
	}

	// Token: 0x06001392 RID: 5010 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06001393 RID: 5011 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000D69 RID: 3433
	public const string ID = "GasGrassHarvested";
}
