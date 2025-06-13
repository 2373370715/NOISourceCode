﻿using System;
using STRINGS;
using TUNING;
using UnityEngine;

public class BerryPieConfig : IEntityConfig, IHasDlcRestrictions
{
	public string[] GetRequiredDlcIds()
	{
		return DlcManager.EXPANSION1;
	}

	public string[] GetForbiddenDlcIds()
	{
		return null;
	}

	public GameObject CreatePrefab()
	{
		return EntityTemplates.ExtendEntityToFood(EntityTemplates.CreateLooseEntity("BerryPie", STRINGS.ITEMS.FOOD.BERRYPIE.NAME, STRINGS.ITEMS.FOOD.BERRYPIE.DESC, 1f, false, Assets.GetAnim("wormwood_berry_pie_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.8f, 0.55f, true, 0, SimHashes.Creature, null), FOOD.FOOD_TYPES.BERRY_PIE);
	}

	public void OnPrefabInit(GameObject inst)
	{
	}

	public void OnSpawn(GameObject inst)
	{
	}

	public const string ID = "BerryPie";

	public static ComplexRecipe recipe;
}
