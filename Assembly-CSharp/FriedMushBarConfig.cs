using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000307 RID: 775
public class FriedMushBarConfig : IEntityConfig
{
	// Token: 0x06000C04 RID: 3076 RVA: 0x00179D98 File Offset: 0x00177F98
	public GameObject CreatePrefab()
	{
		return EntityTemplates.ExtendEntityToFood(EntityTemplates.CreateLooseEntity("FriedMushBar", STRINGS.ITEMS.FOOD.FRIEDMUSHBAR.NAME, STRINGS.ITEMS.FOOD.FRIEDMUSHBAR.DESC, 1f, false, Assets.GetAnim("mushbarfried_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.8f, 0.4f, true, 0, SimHashes.Creature, null), FOOD.FOOD_TYPES.FRIEDMUSHBAR);
	}

	// Token: 0x06000C05 RID: 3077 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000C06 RID: 3078 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000939 RID: 2361
	public const string ID = "FriedMushBar";

	// Token: 0x0400093A RID: 2362
	public static ComplexRecipe recipe;
}
