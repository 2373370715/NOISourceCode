using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000328 RID: 808
public class DehydratedSurfAndTurfConfig : IEntityConfig
{
	// Token: 0x06000C98 RID: 3224 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000C99 RID: 3225 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x06000C9A RID: 3226 RVA: 0x0017AD4C File Offset: 0x00178F4C
	public GameObject CreatePrefab()
	{
		KAnimFile anim = Assets.GetAnim("dehydrated_food_surf_and_turf_kanim");
		GameObject gameObject = EntityTemplates.CreateLooseEntity(DehydratedSurfAndTurfConfig.ID.Name, STRINGS.ITEMS.FOOD.SURFANDTURF.DEHYDRATED.NAME, STRINGS.ITEMS.FOOD.SURFANDTURF.DEHYDRATED.DESC, 1f, true, anim, "idle", Grid.SceneLayer.BuildingFront, EntityTemplates.CollisionShape.RECTANGLE, 0.6f, 0.7f, true, 0, SimHashes.Polypropylene, null);
		EntityTemplates.ExtendEntityToDehydratedFoodPackage(gameObject, FOOD.FOOD_TYPES.SURF_AND_TURF);
		return gameObject;
	}

	// Token: 0x0400097E RID: 2430
	public static Tag ID = new Tag("DehydratedSurfAndTurf");

	// Token: 0x0400097F RID: 2431
	public const float MASS = 1f;

	// Token: 0x04000980 RID: 2432
	public const int FABRICATION_TIME_SECONDS = 300;

	// Token: 0x04000981 RID: 2433
	public const string ANIM_FILE = "dehydrated_food_surf_and_turf_kanim";

	// Token: 0x04000982 RID: 2434
	public const string INITIAL_ANIM = "idle";
}
