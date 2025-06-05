using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x0200032E RID: 814
public class TofuConfig : IEntityConfig
{
	// Token: 0x06000CB6 RID: 3254 RVA: 0x0017AF84 File Offset: 0x00179184
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateLooseEntity("Tofu", STRINGS.ITEMS.FOOD.TOFU.NAME, STRINGS.ITEMS.FOOD.TOFU.DESC, 1f, false, Assets.GetAnim("loafu_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.9f, 0.6f, true, 0, SimHashes.Creature, null);
		gameObject = EntityTemplates.ExtendEntityToFood(gameObject, FOOD.FOOD_TYPES.TOFU);
		ComplexRecipeManager.Get().GetRecipe(TofuConfig.recipe.id).FabricationVisualizer = MushBarConfig.CreateFabricationVisualizer(gameObject);
		return gameObject;
	}

	// Token: 0x06000CB7 RID: 3255 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000CB8 RID: 3256 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x0400098C RID: 2444
	public const string ID = "Tofu";

	// Token: 0x0400098D RID: 2445
	public static ComplexRecipe recipe;
}
