using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x02000396 RID: 918
public class CrabWoodShellConfig : IEntityConfig
{
	// Token: 0x06000EC3 RID: 3779 RVA: 0x00184C28 File Offset: 0x00182E28
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateLooseEntity("CrabWoodShell", ITEMS.INDUSTRIAL_PRODUCTS.CRAB_SHELL.VARIANT_WOOD.NAME, ITEMS.INDUSTRIAL_PRODUCTS.CRAB_SHELL.VARIANT_WOOD.DESC, 100f, true, Assets.GetAnim("crabshells_large_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.9f, 0.6f, true, 0, SimHashes.Creature, new List<Tag>
		{
			GameTags.IndustrialIngredient,
			GameTags.Organics,
			GameTags.MoltShell
		});
		gameObject.AddOrGet<EntitySplitter>();
		gameObject.AddOrGet<SimpleMassStatusItem>().symbolPrefix = "wood_";
		SymbolOverrideControllerUtil.AddToPrefab(gameObject).ApplySymbolOverridesByAffix(Assets.GetAnim("crabshells_large_kanim"), "wood_", null, 0);
		EntityTemplates.CreateAndRegisterCompostableFromPrefab(gameObject);
		return gameObject;
	}

	// Token: 0x06000EC4 RID: 3780 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000EC5 RID: 3781 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000AEC RID: 2796
	public const string ID = "CrabWoodShell";

	// Token: 0x04000AED RID: 2797
	public static readonly Tag TAG = TagManager.Create("CrabWoodShell");

	// Token: 0x04000AEE RID: 2798
	public const float MASS = 100f;

	// Token: 0x04000AEF RID: 2799
	public const string symbolPrefix = "wood_";
}
