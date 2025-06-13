﻿using System;
using System.Collections.Generic;
using Klei.AI;
using STRINGS;
using TUNING;
using UnityEngine;

public class VineMotherConfig : IEntityConfig, IHasDlcRestrictions
{
	public string[] GetRequiredDlcIds()
	{
		return DlcManager.DLC4;
	}

	public string[] GetForbiddenDlcIds()
	{
		return null;
	}

	public GameObject CreatePrefab()
	{
		string id = "VineMother";
		string name = STRINGS.CREATURES.SPECIES.VINEMOTHER.NAME;
		string desc = STRINGS.CREATURES.SPECIES.VINEMOTHER.DESC;
		float mass = 2f;
		EffectorValues tier = DECOR.BONUS.TIER1;
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, name, desc, mass, Assets.GetAnim("vine_mother_kanim"), "object", Grid.SceneLayer.BuildingFront, 1, 2, tier, default(EffectorValues), SimHashes.Creature, null, 308.15f);
		string text = "VineMotherOriginal";
		EntityTemplates.ExtendEntityToBasicPlant(gameObject, 273.15f, 298.15f, 318.15f, 378.15f, VineMotherConfig.ALLOWED_ELEMENTS, false, 0f, 0.15f, null, true, false, true, false, 2400f, 0f, 2200f, text, STRINGS.CREATURES.SPECIES.VINEMOTHER.NAME);
		WiltCondition component = gameObject.GetComponent<WiltCondition>();
		component.WiltDelay = 0f;
		component.RecoveryDelay = 0f;
		KPrefabID component2 = gameObject.GetComponent<KPrefabID>();
		GeneratedBuildings.RegisterWithOverlay(OverlayScreen.HarvestableIDs, component2.PrefabID().ToString());
		gameObject.AddOrGet<Traits>();
		Db.Get().traits.Get(text);
		gameObject.GetComponent<Modifiers>().initialTraits.Add(text);
		VineMother.Def def = gameObject.AddOrGetDef<VineMother.Def>();
		def.BRANCH_PREFAB_NAME = "VineBranch";
		def.MAX_BRANCH_COUNT = 24;
		gameObject.AddOrGet<HarvestDesignatable>();
		EntityTemplates.ExtendPlantToIrrigated(gameObject, new PlantElementAbsorber.ConsumeInfo[]
		{
			new PlantElementAbsorber.ConsumeInfo
			{
				tag = GameTags.Water,
				massConsumptionRate = 0.15f
			}
		});
		GameObject plant = gameObject;
		SeedProducer.ProductionType productionType = SeedProducer.ProductionType.Hidden;
		string id2 = "VineMotherSeed";
		string name2 = STRINGS.CREATURES.SPECIES.SEEDS.VINEMOTHER.NAME;
		string desc2 = STRINGS.CREATURES.SPECIES.SEEDS.VINEMOTHER.DESC;
		KAnimFile anim = Assets.GetAnim("seed_vine_kanim");
		string initialAnim = "object";
		int numberOfSeeds = 1;
		List<Tag> list = new List<Tag>();
		list.Add(GameTags.CropSeed);
		SingleEntityReceptacle.ReceptacleDirection planterDirection = SingleEntityReceptacle.ReceptacleDirection.Top;
		string domesticatedDescription = STRINGS.CREATURES.SPECIES.VINEMOTHER.DOMESTICATEDDESC;
		EntityTemplates.CreateAndRegisterPreviewForPlant(EntityTemplates.CreateAndRegisterSeedForPlant(plant, this, productionType, id2, name2, desc2, anim, initialAnim, numberOfSeeds, list, planterDirection, default(Tag), 12, domesticatedDescription, EntityTemplates.CollisionShape.RECTANGLE, 0.8f, 0.6f, null, "", false), "VineMother_preview", Assets.GetAnim("vine_mother_kanim"), "place", 1, 2);
		return gameObject;
	}

	public void OnPrefabInit(GameObject inst)
	{
	}

	public void OnSpawn(GameObject inst)
	{
	}

	public const string ID = "VineMother";

	public const string SEED_ID = "VineMotherSeed";

	public const int MAX_BRANCH_NETWORK_COUNT = 12;

	public static SimHashes[] ALLOWED_ELEMENTS = new SimHashes[]
	{
		SimHashes.Oxygen,
		SimHashes.CarbonDioxide,
		SimHashes.ContaminatedOxygen
	};

	public const float IRRIGATION_RATE = 0.15f;

	public const float TEMPERATURE_LETHAL_LOW = 273.15f;

	public const float TEMPERATURE_WARNING_LOW = 298.15f;

	public const float TEMPERATURE_WARNING_HIGH = 318.15f;

	public const float TEMPERATURE_LETHAL_HIGH = 378.15f;
}
