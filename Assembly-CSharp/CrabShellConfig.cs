using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x02000395 RID: 917
public class CrabShellConfig : IEntityConfig
{
	// Token: 0x06000EBE RID: 3774 RVA: 0x00184B9C File Offset: 0x00182D9C
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateLooseEntity("CrabShell", ITEMS.INDUSTRIAL_PRODUCTS.CRAB_SHELL.NAME, ITEMS.INDUSTRIAL_PRODUCTS.CRAB_SHELL.DESC, 10f, true, Assets.GetAnim("crabshells_large_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.9f, 0.6f, true, 0, SimHashes.Creature, new List<Tag>
		{
			GameTags.IndustrialIngredient,
			GameTags.Organics
		});
		gameObject.AddOrGet<EntitySplitter>();
		gameObject.AddOrGet<SimpleMassStatusItem>();
		EntityTemplates.CreateAndRegisterCompostableFromPrefab(gameObject);
		return gameObject;
	}

	// Token: 0x06000EBF RID: 3775 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000EC0 RID: 3776 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000AE9 RID: 2793
	public const string ID = "CrabShell";

	// Token: 0x04000AEA RID: 2794
	public static readonly Tag TAG = TagManager.Create("CrabShell");

	// Token: 0x04000AEB RID: 2795
	public const float MASS = 10f;
}
