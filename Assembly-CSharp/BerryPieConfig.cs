using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020002F4 RID: 756
public class BerryPieConfig : IEntityConfig, IHasDlcRestrictions
{
	// Token: 0x06000BA4 RID: 2980 RVA: 0x000AA117 File Offset: 0x000A8317
	public string[] GetRequiredDlcIds()
	{
		return DlcManager.EXPANSION1;
	}

	// Token: 0x06000BA5 RID: 2981 RVA: 0x000AA765 File Offset: 0x000A8965
	public string[] GetForbiddenDlcIds()
	{
		return null;
	}

	// Token: 0x06000BA6 RID: 2982 RVA: 0x00179594 File Offset: 0x00177794
	public GameObject CreatePrefab()
	{
		return EntityTemplates.ExtendEntityToFood(EntityTemplates.CreateLooseEntity("BerryPie", STRINGS.ITEMS.FOOD.BERRYPIE.NAME, STRINGS.ITEMS.FOOD.BERRYPIE.DESC, 1f, false, Assets.GetAnim("wormwood_berry_pie_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.8f, 0.55f, true, 0, SimHashes.Creature, null), FOOD.FOOD_TYPES.BERRY_PIE);
	}

	// Token: 0x06000BA7 RID: 2983 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000BA8 RID: 2984 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000910 RID: 2320
	public const string ID = "BerryPie";

	// Token: 0x04000911 RID: 2321
	public static ComplexRecipe recipe;
}
