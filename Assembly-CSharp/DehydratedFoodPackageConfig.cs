using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020002F7 RID: 759
public class DehydratedFoodPackageConfig : IEntityConfig
{
	// Token: 0x06000BB5 RID: 2997 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000BB6 RID: 2998 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x06000BB7 RID: 2999 RVA: 0x001796CC File Offset: 0x001778CC
	public GameObject CreatePrefab()
	{
		KAnimFile anim = Assets.GetAnim("dehydrated_food_burger_kanim");
		GameObject gameObject = EntityTemplates.CreateLooseEntity(DehydratedFoodPackageConfig.ID.Name, STRINGS.ITEMS.FOOD.BURGER.DEHYDRATED.NAME, STRINGS.ITEMS.FOOD.BURGER.DEHYDRATED.DESC, 1f, true, anim, "idle", Grid.SceneLayer.BuildingFront, EntityTemplates.CollisionShape.RECTANGLE, 0.6f, 0.7f, true, 0, SimHashes.Polypropylene, null);
		EntityTemplates.ExtendEntityToDehydratedFoodPackage(gameObject, FOOD.FOOD_TYPES.BURGER);
		return gameObject;
	}

	// Token: 0x04000918 RID: 2328
	public static Tag ID = new Tag("DehydratedFoodPackage");

	// Token: 0x04000919 RID: 2329
	public const float MASS = 1f;

	// Token: 0x0400091A RID: 2330
	public const string ANIM_FILE = "dehydrated_food_burger_kanim";

	// Token: 0x0400091B RID: 2331
	public const string INITIAL_ANIM = "idle";
}
