﻿using System;
using STRINGS;
using TUNING;
using UnityEngine;

public class CookedMeatConfig : IEntityConfig
{
	public GameObject CreatePrefab()
	{
		return EntityTemplates.ExtendEntityToFood(EntityTemplates.CreateLooseEntity("CookedMeat", STRINGS.ITEMS.FOOD.COOKEDMEAT.NAME, STRINGS.ITEMS.FOOD.COOKEDMEAT.DESC, 1f, false, Assets.GetAnim("barbeque_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.8f, 0.4f, true, 0, SimHashes.Creature, null), FOOD.FOOD_TYPES.COOKED_MEAT);
	}

	public void OnPrefabInit(GameObject inst)
	{
	}

	public void OnSpawn(GameObject inst)
	{
	}

	public const string ID = "CookedMeat";

	public static ComplexRecipe recipe;
}
