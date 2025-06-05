using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020002FA RID: 762
public class ColdWheatBreadConfig : IEntityConfig
{
	// Token: 0x06000BC5 RID: 3013 RVA: 0x00179874 File Offset: 0x00177A74
	public GameObject CreatePrefab()
	{
		return EntityTemplates.ExtendEntityToFood(EntityTemplates.CreateLooseEntity("ColdWheatBread", STRINGS.ITEMS.FOOD.COLDWHEATBREAD.NAME, STRINGS.ITEMS.FOOD.COLDWHEATBREAD.DESC, 1f, false, Assets.GetAnim("frostbread_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.8f, 0.4f, true, 0, SimHashes.Creature, null), FOOD.FOOD_TYPES.COLD_WHEAT_BREAD);
	}

	// Token: 0x06000BC6 RID: 3014 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000BC7 RID: 3015 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000920 RID: 2336
	public const string ID = "ColdWheatBread";

	// Token: 0x04000921 RID: 2337
	public static ComplexRecipe recipe;
}
