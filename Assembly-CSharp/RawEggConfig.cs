using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x0200031D RID: 797
public class RawEggConfig : IEntityConfig
{
	// Token: 0x06000C67 RID: 3175 RVA: 0x0017A844 File Offset: 0x00178A44
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateLooseEntity("RawEgg", STRINGS.ITEMS.FOOD.RAWEGG.NAME, STRINGS.ITEMS.FOOD.RAWEGG.DESC, 1f, false, Assets.GetAnim("rawegg_kanim"), "object", Grid.SceneLayer.Ore, EntityTemplates.CollisionShape.RECTANGLE, 0.8f, 0.4f, true, 0, SimHashes.Creature, null);
		EntityTemplates.ExtendEntityToFood(gameObject, FOOD.FOOD_TYPES.RAWEGG);
		TemperatureCookable temperatureCookable = gameObject.AddOrGet<TemperatureCookable>();
		temperatureCookable.cookTemperature = 344.15f;
		temperatureCookable.cookedID = "CookedEgg";
		return gameObject;
	}

	// Token: 0x06000C68 RID: 3176 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000C69 RID: 3177 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000965 RID: 2405
	public const string ID = "RawEgg";
}
