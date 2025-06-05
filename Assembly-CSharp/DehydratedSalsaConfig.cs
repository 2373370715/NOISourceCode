using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000320 RID: 800
public class DehydratedSalsaConfig : IEntityConfig
{
	// Token: 0x06000C74 RID: 3188 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000C75 RID: 3189 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x06000C76 RID: 3190 RVA: 0x0017A9D0 File Offset: 0x00178BD0
	public GameObject CreatePrefab()
	{
		KAnimFile anim = Assets.GetAnim("dehydrated_food_salsa_kanim");
		GameObject gameObject = EntityTemplates.CreateLooseEntity(DehydratedSalsaConfig.ID.Name, STRINGS.ITEMS.FOOD.SALSA.DEHYDRATED.NAME, STRINGS.ITEMS.FOOD.SALSA.DEHYDRATED.DESC, 1f, true, anim, "idle", Grid.SceneLayer.BuildingFront, EntityTemplates.CollisionShape.RECTANGLE, 0.6f, 0.7f, true, 0, SimHashes.Polypropylene, null);
		EntityTemplates.ExtendEntityToDehydratedFoodPackage(gameObject, FOOD.FOOD_TYPES.SALSA);
		return gameObject;
	}

	// Token: 0x04000969 RID: 2409
	public static Tag ID = new Tag("DehydratedSalsa");

	// Token: 0x0400096A RID: 2410
	public const float MASS = 1f;

	// Token: 0x0400096B RID: 2411
	public const string ANIM_FILE = "dehydrated_food_salsa_kanim";

	// Token: 0x0400096C RID: 2412
	public const string INITIAL_ANIM = "idle";
}
