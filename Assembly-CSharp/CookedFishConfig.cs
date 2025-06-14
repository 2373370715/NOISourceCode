﻿using System;
using STRINGS;
using TUNING;
using UnityEngine;

public class CookedFishConfig : IEntityConfig
{
	public GameObject CreatePrefab()
	{
		return EntityTemplates.ExtendEntityToFood(EntityTemplates.CreateLooseEntity("CookedFish", STRINGS.ITEMS.FOOD.COOKEDFISH.NAME, STRINGS.ITEMS.FOOD.COOKEDFISH.DESC, 1f, false, Assets.GetAnim("grilled_pacu_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.8f, 0.4f, true, 0, SimHashes.Creature, null), FOOD.FOOD_TYPES.COOKED_FISH);
	}

	public void OnPrefabInit(GameObject inst)
	{
	}

	public void OnSpawn(GameObject inst)
	{
	}

	public const string ID = "CookedFish";

	public static ComplexRecipe recipe;
}
