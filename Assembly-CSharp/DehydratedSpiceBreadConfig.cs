using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000323 RID: 803
public class DehydratedSpiceBreadConfig : IEntityConfig
{
	// Token: 0x06000C81 RID: 3201 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000C82 RID: 3202 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x06000C83 RID: 3203 RVA: 0x0017AB0C File Offset: 0x00178D0C
	public GameObject CreatePrefab()
	{
		KAnimFile anim = Assets.GetAnim("dehydrated_food_spicebread_kanim");
		GameObject gameObject = EntityTemplates.CreateLooseEntity(DehydratedSpiceBreadConfig.ID.Name, STRINGS.ITEMS.FOOD.SPICEBREAD.DEHYDRATED.NAME, STRINGS.ITEMS.FOOD.SPICEBREAD.DEHYDRATED.DESC, 1f, true, anim, "idle", Grid.SceneLayer.BuildingFront, EntityTemplates.CollisionShape.RECTANGLE, 0.6f, 0.7f, true, 0, SimHashes.Polypropylene, null);
		EntityTemplates.ExtendEntityToDehydratedFoodPackage(gameObject, FOOD.FOOD_TYPES.SPICEBREAD);
		return gameObject;
	}

	// Token: 0x04000970 RID: 2416
	public static Tag ID = new Tag("DehydratedSpiceBread");

	// Token: 0x04000971 RID: 2417
	public const float MASS = 1f;

	// Token: 0x04000972 RID: 2418
	public const string ANIM_FILE = "dehydrated_food_spicebread_kanim";

	// Token: 0x04000973 RID: 2419
	public const string INITIAL_ANIM = "idle";
}
