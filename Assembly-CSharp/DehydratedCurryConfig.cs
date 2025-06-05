using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000300 RID: 768
public class DehydratedCurryConfig : IEntityConfig
{
	// Token: 0x06000BDF RID: 3039 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000BE0 RID: 3040 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x06000BE1 RID: 3041 RVA: 0x00179ACC File Offset: 0x00177CCC
	public GameObject CreatePrefab()
	{
		KAnimFile anim = Assets.GetAnim("dehydrated_food_curry_kanim");
		GameObject gameObject = EntityTemplates.CreateLooseEntity(DehydratedCurryConfig.ID.Name, STRINGS.ITEMS.FOOD.CURRY.DEHYDRATED.NAME, STRINGS.ITEMS.FOOD.CURRY.DEHYDRATED.DESC, 1f, true, anim, "idle", Grid.SceneLayer.BuildingFront, EntityTemplates.CollisionShape.RECTANGLE, 0.6f, 0.7f, true, 0, SimHashes.Polypropylene, null);
		EntityTemplates.ExtendEntityToDehydratedFoodPackage(gameObject, FOOD.FOOD_TYPES.CURRY);
		return gameObject;
	}

	// Token: 0x0400092B RID: 2347
	public static Tag ID = new Tag("DehydratedCurry");

	// Token: 0x0400092C RID: 2348
	public const float MASS = 1f;

	// Token: 0x0400092D RID: 2349
	public const string ANIM_FILE = "dehydrated_food_curry_kanim";

	// Token: 0x0400092E RID: 2350
	public const string INITIAL_ANIM = "idle";
}
