using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x0200047F RID: 1151
public class GeneShufflerRechargeConfig : IEntityConfig
{
	// Token: 0x06001395 RID: 5013 RVA: 0x00198CF0 File Offset: 0x00196EF0
	public GameObject CreatePrefab()
	{
		return EntityTemplates.CreateLooseEntity("GeneShufflerRecharge", ITEMS.INDUSTRIAL_PRODUCTS.GENE_SHUFFLER_RECHARGE.NAME, ITEMS.INDUSTRIAL_PRODUCTS.GENE_SHUFFLER_RECHARGE.DESC, 5f, true, Assets.GetAnim("vacillator_charge_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.8f, 0.6f, true, 0, SimHashes.Creature, new List<Tag>
		{
			GameTags.IndustrialIngredient
		});
	}

	// Token: 0x06001396 RID: 5014 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06001397 RID: 5015 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000D6A RID: 3434
	public const string ID = "GeneShufflerRecharge";

	// Token: 0x04000D6B RID: 3435
	public static readonly Tag tag = TagManager.Create("GeneShufflerRecharge");

	// Token: 0x04000D6C RID: 3436
	public const float MASS = 5f;
}
