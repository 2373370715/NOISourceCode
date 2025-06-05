using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000305 RID: 773
public class FieldRationConfig : IEntityConfig
{
	// Token: 0x06000BFC RID: 3068 RVA: 0x00179CCC File Offset: 0x00177ECC
	public GameObject CreatePrefab()
	{
		return EntityTemplates.ExtendEntityToFood(EntityTemplates.CreateLooseEntity("FieldRation", STRINGS.ITEMS.FOOD.FIELDRATION.NAME, STRINGS.ITEMS.FOOD.FIELDRATION.DESC, 1f, false, Assets.GetAnim("fieldration_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.8f, 0.4f, true, 0, SimHashes.Creature, null), FOOD.FOOD_TYPES.FIELDRATION);
	}

	// Token: 0x06000BFD RID: 3069 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000BFE RID: 3070 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000937 RID: 2359
	public const string ID = "FieldRation";
}
