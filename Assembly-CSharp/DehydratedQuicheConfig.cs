using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x0200031C RID: 796
public class DehydratedQuicheConfig : IEntityConfig
{
	// Token: 0x06000C62 RID: 3170 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000C63 RID: 3171 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x06000C64 RID: 3172 RVA: 0x0017A7D4 File Offset: 0x001789D4
	public GameObject CreatePrefab()
	{
		KAnimFile anim = Assets.GetAnim("dehydrated_food_quiche_kanim");
		GameObject gameObject = EntityTemplates.CreateLooseEntity(DehydratedQuicheConfig.ID.Name, STRINGS.ITEMS.FOOD.QUICHE.DEHYDRATED.NAME, STRINGS.ITEMS.FOOD.QUICHE.DEHYDRATED.DESC, 1f, true, anim, "idle", Grid.SceneLayer.BuildingFront, EntityTemplates.CollisionShape.RECTANGLE, 0.6f, 0.7f, true, 0, SimHashes.Polypropylene, null);
		EntityTemplates.ExtendEntityToDehydratedFoodPackage(gameObject, FOOD.FOOD_TYPES.QUICHE);
		return gameObject;
	}

	// Token: 0x04000961 RID: 2401
	public static Tag ID = new Tag("DehydratedQuiche");

	// Token: 0x04000962 RID: 2402
	public const float MASS = 1f;

	// Token: 0x04000963 RID: 2403
	public const string ANIM_FILE = "dehydrated_food_quiche_kanim";

	// Token: 0x04000964 RID: 2404
	public const string INITIAL_ANIM = "idle";
}
