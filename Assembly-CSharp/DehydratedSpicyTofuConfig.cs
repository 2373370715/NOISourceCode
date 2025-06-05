using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000326 RID: 806
public class DehydratedSpicyTofuConfig : IEntityConfig
{
	// Token: 0x06000C8F RID: 3215 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000C90 RID: 3216 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x06000C91 RID: 3217 RVA: 0x0017AC78 File Offset: 0x00178E78
	public GameObject CreatePrefab()
	{
		KAnimFile anim = Assets.GetAnim("dehydrated_food_spicy_tofu_kanim");
		GameObject gameObject = EntityTemplates.CreateLooseEntity(DehydratedSpicyTofuConfig.ID.Name, STRINGS.ITEMS.FOOD.SPICYTOFU.DEHYDRATED.NAME, STRINGS.ITEMS.FOOD.SPICYTOFU.DEHYDRATED.DESC, 1f, true, anim, "idle", Grid.SceneLayer.BuildingFront, EntityTemplates.CollisionShape.RECTANGLE, 0.6f, 0.7f, true, 0, SimHashes.Polypropylene, null);
		EntityTemplates.ExtendEntityToDehydratedFoodPackage(gameObject, FOOD.FOOD_TYPES.SPICY_TOFU);
		return gameObject;
	}

	// Token: 0x04000978 RID: 2424
	public static Tag ID = new Tag("DehydratedSpicyTofu");

	// Token: 0x04000979 RID: 2425
	public const float MASS = 1f;

	// Token: 0x0400097A RID: 2426
	public const string ANIM_FILE = "dehydrated_food_spicy_tofu_kanim";

	// Token: 0x0400097B RID: 2427
	public const string INITIAL_ANIM = "idle";
}
