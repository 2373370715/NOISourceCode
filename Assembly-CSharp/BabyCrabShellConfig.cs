using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x02000393 RID: 915
public class BabyCrabShellConfig : IEntityConfig
{
	// Token: 0x06000EB4 RID: 3764 RVA: 0x00184A50 File Offset: 0x00182C50
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateLooseEntity("BabyCrabShell", ITEMS.INDUSTRIAL_PRODUCTS.BABY_CRAB_SHELL.NAME, ITEMS.INDUSTRIAL_PRODUCTS.BABY_CRAB_SHELL.DESC, 5f, true, Assets.GetAnim("crabshells_small_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.9f, 0.6f, true, 0, SimHashes.Creature, new List<Tag>
		{
			GameTags.IndustrialIngredient,
			GameTags.Organics
		});
		gameObject.AddOrGet<EntitySplitter>();
		gameObject.AddOrGet<SimpleMassStatusItem>();
		EntityTemplates.CreateAndRegisterCompostableFromPrefab(gameObject);
		return gameObject;
	}

	// Token: 0x06000EB5 RID: 3765 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000EB6 RID: 3766 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000AE2 RID: 2786
	public const string ID = "BabyCrabShell";

	// Token: 0x04000AE3 RID: 2787
	public static readonly Tag TAG = TagManager.Create("BabyCrabShell");

	// Token: 0x04000AE4 RID: 2788
	public const float MASS = 5f;
}
