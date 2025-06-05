using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000318 RID: 792
public class PlantMeatConfig : IEntityConfig, IHasDlcRestrictions
{
	// Token: 0x06000C4F RID: 3151 RVA: 0x000AA117 File Offset: 0x000A8317
	public string[] GetRequiredDlcIds()
	{
		return DlcManager.EXPANSION1;
	}

	// Token: 0x06000C50 RID: 3152 RVA: 0x000AA765 File Offset: 0x000A8965
	public string[] GetForbiddenDlcIds()
	{
		return null;
	}

	// Token: 0x06000C51 RID: 3153 RVA: 0x0017A5D0 File Offset: 0x001787D0
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateLooseEntity("PlantMeat", STRINGS.ITEMS.FOOD.PLANTMEAT.NAME, STRINGS.ITEMS.FOOD.PLANTMEAT.DESC, 1f, false, Assets.GetAnim("critter_trap_fruit_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.8f, 0.4f, true, 0, SimHashes.Creature, null);
		EntityTemplates.ExtendEntityToFood(gameObject, FOOD.FOOD_TYPES.PLANTMEAT);
		return gameObject;
	}

	// Token: 0x06000C52 RID: 3154 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000C53 RID: 3155 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x0400095A RID: 2394
	public const string ID = "PlantMeat";
}
