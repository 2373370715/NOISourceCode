using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

public class GardenFoodPlantConfig : IEntityConfig, IHasDlcRestrictions
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
		string id = "GardenFoodPlant";
		string name = STRINGS.CREATURES.SPECIES.GARDENFOODPLANT.NAME;
		string desc = STRINGS.CREATURES.SPECIES.GARDENFOODPLANT.DESC;
		float mass = 1f;
		EffectorValues tier = DECOR.PENALTY.TIER1;
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, name, desc, mass, Assets.GetAnim("spike_fruit_kanim"), "idle_empty", Grid.SceneLayer.BuildingBack, 1, 2, tier, default(EffectorValues), SimHashes.Creature, null, 293f);
		EntityTemplates.ExtendEntityToBasicPlant(gameObject, 263.15f, 268.15f, 313.15f, 323.15f, new SimHashes[]
		{
			SimHashes.Oxygen,
			SimHashes.ContaminatedOxygen,
			SimHashes.CarbonDioxide
		}, true, 0f, 0.15f, "GardenFoodPlantFood", true, true, true, true, 2400f, 0f, 4600f, "GardenFoodPlantOriginal", STRINGS.CREATURES.SPECIES.GARDENFOODPLANT.NAME);
		gameObject.AddOrGet<StandardCropPlant>();
		gameObject.AddOrGet<DirectlyEdiblePlant_Growth>();
		gameObject.AddOrGet<LoopingSounds>();
		gameObject.AddOrGetDef<PollinationMonitor.Def>();
		GameObject plant = gameObject;
		SeedProducer.ProductionType productionType = SeedProducer.ProductionType.Harvest;
		string id2 = "GardenFoodPlantSeed";
		string name2 = STRINGS.CREATURES.SPECIES.SEEDS.GARDENFOODPLANT.NAME;
		string desc2 = STRINGS.CREATURES.SPECIES.SEEDS.GARDENFOODPLANT.DESC;
		KAnimFile anim = Assets.GetAnim("seed_spikefruit_kanim");
		string initialAnim = "object";
		int numberOfSeeds = 1;
		List<Tag> list = new List<Tag>();
		list.Add(GameTags.CropSeed);
		SingleEntityReceptacle.ReceptacleDirection planterDirection = SingleEntityReceptacle.ReceptacleDirection.Top;
		string domesticatedDescription = STRINGS.CREATURES.SPECIES.GARDENFOODPLANT.DOMESTICATEDDESC;
		GameObject seed = EntityTemplates.CreateAndRegisterSeedForPlant(plant, this, productionType, id2, name2, desc2, anim, initialAnim, numberOfSeeds, list, planterDirection, default(Tag), 1, domesticatedDescription, EntityTemplates.CollisionShape.CIRCLE, 0.3f, 0.3f, null, "", false);
		EntityTemplates.ExtendPlantToFertilizable(gameObject, new PlantElementAbsorber.ConsumeInfo[]
		{
			new PlantElementAbsorber.ConsumeInfo
			{
				tag = SimHashes.Peat.CreateTag(),
				massConsumptionRate = 0.016666668f
			}
		});
		EntityTemplates.CreateAndRegisterPreviewForPlant(seed, "GardenFoodPlant_preview", Assets.GetAnim("spike_fruit_kanim"), "place", 1, 2);
		SoundEventVolumeCache.instance.AddVolume("spike_fruit_kanim", "spike_fruit_harvest", NOISE_POLLUTION.CREATURES.TIER3);
		SoundEventVolumeCache.instance.AddVolume("spike_fruit_kanim", "spike_fruit_LP", NOISE_POLLUTION.CREATURES.TIER4);
		return gameObject;
	}

	public void OnPrefabInit(GameObject prefab)
	{
	}

	public void OnSpawn(GameObject inst)
	{
	}

	public const string ID = "GardenFoodPlant";

	public const string SEED_ID = "GardenFoodPlantSeed";
}
