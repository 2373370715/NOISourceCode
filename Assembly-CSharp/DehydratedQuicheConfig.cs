﻿using System;
using STRINGS;
using TUNING;
using UnityEngine;

public class DehydratedQuicheConfig : IEntityConfig
{
	public void OnPrefabInit(GameObject inst)
	{
	}

	public void OnSpawn(GameObject inst)
	{
	}

	public GameObject CreatePrefab()
	{
		KAnimFile anim = Assets.GetAnim("dehydrated_food_quiche_kanim");
		GameObject gameObject = EntityTemplates.CreateLooseEntity(DehydratedQuicheConfig.ID.Name, STRINGS.ITEMS.FOOD.QUICHE.DEHYDRATED.NAME, STRINGS.ITEMS.FOOD.QUICHE.DEHYDRATED.DESC, 1f, true, anim, "idle", Grid.SceneLayer.BuildingFront, EntityTemplates.CollisionShape.RECTANGLE, 0.6f, 0.7f, true, 0, SimHashes.Polypropylene, null);
		EntityTemplates.ExtendEntityToDehydratedFoodPackage(gameObject, FOOD.FOOD_TYPES.QUICHE);
		return gameObject;
	}

	public static Tag ID = new Tag("DehydratedQuiche");

	public const float MASS = 1f;

	public const string ANIM_FILE = "dehydrated_food_quiche_kanim";

	public const string INITIAL_ANIM = "idle";
}
