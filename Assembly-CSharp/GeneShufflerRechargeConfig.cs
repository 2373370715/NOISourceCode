﻿using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

public class GeneShufflerRechargeConfig : IEntityConfig
{
	public GameObject CreatePrefab()
	{
		return EntityTemplates.CreateLooseEntity("GeneShufflerRecharge", ITEMS.INDUSTRIAL_PRODUCTS.GENE_SHUFFLER_RECHARGE.NAME, ITEMS.INDUSTRIAL_PRODUCTS.GENE_SHUFFLER_RECHARGE.DESC, 5f, true, Assets.GetAnim("vacillator_charge_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.8f, 0.6f, true, 0, SimHashes.Creature, new List<Tag>
		{
			GameTags.IndustrialIngredient
		});
	}

	public void OnPrefabInit(GameObject inst)
	{
	}

	public void OnSpawn(GameObject inst)
	{
	}

	public const string ID = "GeneShufflerRecharge";

	public static readonly Tag tag = TagManager.Create("GeneShufflerRecharge");

	public const float MASS = 5f;
}
