using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000313 RID: 787
public class DehydratedMushroomWrapConfig : IEntityConfig
{
	// Token: 0x06000C38 RID: 3128 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000C39 RID: 3129 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x06000C3A RID: 3130 RVA: 0x0017A394 File Offset: 0x00178594
	public GameObject CreatePrefab()
	{
		KAnimFile anim = Assets.GetAnim("dehydrated_food_mushroom_wrap_kanim");
		GameObject gameObject = EntityTemplates.CreateLooseEntity(DehydratedMushroomWrapConfig.ID.Name, STRINGS.ITEMS.FOOD.MUSHROOMWRAP.DEHYDRATED.NAME, STRINGS.ITEMS.FOOD.MUSHROOMWRAP.DEHYDRATED.DESC, 1f, true, anim, "idle", Grid.SceneLayer.BuildingFront, EntityTemplates.CollisionShape.RECTANGLE, 0.6f, 0.7f, true, 0, SimHashes.Polypropylene, null);
		EntityTemplates.ExtendEntityToDehydratedFoodPackage(gameObject, FOOD.FOOD_TYPES.MUSHROOM_WRAP);
		return gameObject;
	}

	// Token: 0x0400094F RID: 2383
	public static Tag ID = new Tag("DehydratedMushroomWrap");

	// Token: 0x04000950 RID: 2384
	public const float MASS = 1f;

	// Token: 0x04000951 RID: 2385
	public const string ANIM_FILE = "dehydrated_food_mushroom_wrap_kanim";

	// Token: 0x04000952 RID: 2386
	public const string INITIAL_ANIM = "idle";
}
