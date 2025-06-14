﻿using System;
using STRINGS;
using TUNING;
using UnityEngine;

public class BasicForagePlantConfig : IEntityConfig
{
	public GameObject CreatePrefab()
	{
		return EntityTemplates.ExtendEntityToFood(EntityTemplates.CreateLooseEntity("BasicForagePlant", STRINGS.ITEMS.FOOD.BASICFORAGEPLANT.NAME, STRINGS.ITEMS.FOOD.BASICFORAGEPLANT.DESC, 1f, false, Assets.GetAnim("muckrootvegetable_kanim"), "object", Grid.SceneLayer.BuildingBack, EntityTemplates.CollisionShape.CIRCLE, 0.3f, 0.3f, true, 0, SimHashes.Creature, null), FOOD.FOOD_TYPES.BASICFORAGEPLANT);
	}

	public void OnPrefabInit(GameObject inst)
	{
	}

	public void OnSpawn(GameObject inst)
	{
	}

	public const string ID = "BasicForagePlant";
}
