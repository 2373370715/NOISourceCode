using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x02000394 RID: 916
public class BabyCrabWoodShellConfig : IEntityConfig
{
	// Token: 0x06000EB9 RID: 3769 RVA: 0x00184ADC File Offset: 0x00182CDC
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateLooseEntity("BabyCrabWoodShell", ITEMS.INDUSTRIAL_PRODUCTS.BABY_CRAB_SHELL.VARIANT_WOOD.NAME, ITEMS.INDUSTRIAL_PRODUCTS.BABY_CRAB_SHELL.VARIANT_WOOD.DESC, 10f, true, Assets.GetAnim("crabshells_small_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.9f, 0.6f, true, 0, SimHashes.Creature, new List<Tag>
		{
			GameTags.IndustrialIngredient,
			GameTags.Organics,
			GameTags.MoltShell
		});
		gameObject.AddOrGet<EntitySplitter>();
		gameObject.AddOrGet<SimpleMassStatusItem>().symbolPrefix = "wood_";
		SymbolOverrideControllerUtil.AddToPrefab(gameObject).ApplySymbolOverridesByAffix(Assets.GetAnim("crabshells_small_kanim"), "wood_", null, 0);
		EntityTemplates.CreateAndRegisterCompostableFromPrefab(gameObject);
		return gameObject;
	}

	// Token: 0x06000EBA RID: 3770 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000EBB RID: 3771 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000AE5 RID: 2789
	public const string ID = "BabyCrabWoodShell";

	// Token: 0x04000AE6 RID: 2790
	public static readonly Tag TAG = TagManager.Create("BabyCrabWoodShell");

	// Token: 0x04000AE7 RID: 2791
	public const float MASS = 10f;

	// Token: 0x04000AE8 RID: 2792
	public const string symbolPrefix = "wood_";
}
