using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020002DB RID: 731
public class SwampForagePlantConfig : IEntityConfig, IHasDlcRestrictions
{
	// Token: 0x06000B32 RID: 2866 RVA: 0x000AA117 File Offset: 0x000A8317
	public string[] GetRequiredDlcIds()
	{
		return DlcManager.EXPANSION1;
	}

	// Token: 0x06000B33 RID: 2867 RVA: 0x000AA765 File Offset: 0x000A8965
	public string[] GetForbiddenDlcIds()
	{
		return null;
	}

	// Token: 0x06000B34 RID: 2868 RVA: 0x00177AD0 File Offset: 0x00175CD0
	public GameObject CreatePrefab()
	{
		return EntityTemplates.ExtendEntityToFood(EntityTemplates.CreateLooseEntity("SwampForagePlant", STRINGS.ITEMS.FOOD.SWAMPFORAGEPLANT.NAME, STRINGS.ITEMS.FOOD.SWAMPFORAGEPLANT.DESC, 1f, false, Assets.GetAnim("swamptuber_vegetable_kanim"), "object", Grid.SceneLayer.BuildingBack, EntityTemplates.CollisionShape.CIRCLE, 0.3f, 0.3f, true, 0, SimHashes.Creature, null), FOOD.FOOD_TYPES.SWAMPFORAGEPLANT);
	}

	// Token: 0x06000B35 RID: 2869 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000B36 RID: 2870 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x040008D3 RID: 2259
	public const string ID = "SwampForagePlant";
}
