using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

public class ButterflyPlantConfig : IEntityConfig, IHasDlcRestrictions
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
		string id = "ButterflyPlant";
		string name = STRINGS.CREATURES.SPECIES.BUTTERFLYPLANT.NAME;
		string desc = STRINGS.CREATURES.SPECIES.BUTTERFLYPLANT.DESC;
		float mass = 1f;
		EffectorValues tier = DECOR.BONUS.TIER1;
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, name, desc, mass, Assets.GetAnim("pollinator_plant_kanim"), "idle_empty", Grid.SceneLayer.BuildingBack, 1, 2, tier, default(EffectorValues), SimHashes.Creature, null, 293f);
		EntityTemplates.ExtendEntityToBasicPlant(gameObject, 233.15f, 283.15f, 318.15f, 353.15f, new SimHashes[]
		{
			SimHashes.Oxygen,
			SimHashes.ContaminatedOxygen,
			SimHashes.CarbonDioxide,
			SimHashes.ChlorineGas
		}, true, 0f, 0.15f, "Butterfly", true, true, true, true, 2400f, 0f, 7400f, "ButterflyPlantOriginal", STRINGS.CREATURES.SPECIES.BUTTERFLYPLANT.NAME);
		UnityEngine.Object.DestroyImmediate(gameObject.GetComponent<MutantPlant>());
		UnityEngine.Object.DestroyImmediate(gameObject.GetComponent<HarvestDesignatable>());
		EntityTemplates.ExtendPlantToFertilizable(gameObject, new PlantElementAbsorber.ConsumeInfo[]
		{
			new PlantElementAbsorber.ConsumeInfo
			{
				tag = GameTags.Dirt,
				massConsumptionRate = 0.016666668f
			}
		});
		gameObject.AddOrGet<StandardCropPlant>();
		gameObject.AddOrGet<LoopingSounds>();
		GameObject plant = gameObject;
		SeedProducer.ProductionType productionType = SeedProducer.ProductionType.Crop;
		string id2 = "ButterflyPlantSeed";
		string name2 = STRINGS.CREATURES.SPECIES.SEEDS.BUTTERFLYPLANTSEED.NAME;
		string desc2 = STRINGS.CREATURES.SPECIES.SEEDS.BUTTERFLYPLANTSEED.DESC;
		KAnimFile anim = Assets.GetAnim("seed_pollinator_plant_kanim");
		string initialAnim = "object";
		int numberOfSeeds = 1;
		List<Tag> list = new List<Tag>();
		list.Add(GameTags.CropSeed);
		SingleEntityReceptacle.ReceptacleDirection planterDirection = SingleEntityReceptacle.ReceptacleDirection.Top;
		string domesticatedDescription = STRINGS.CREATURES.SPECIES.BUTTERFLYPLANT.DOMESTICATEDDESC;
		GameObject gameObject2 = EntityTemplates.CreateAndRegisterSeedForPlant(plant, this, productionType, id2, name2, desc2, anim, initialAnim, numberOfSeeds, list, planterDirection, default(Tag), 2, domesticatedDescription, EntityTemplates.CollisionShape.CIRCLE, 0.3f, 0.3f, null, "", true);
		EntityTemplates.ExtendEntityToFood(gameObject2, FOOD.FOOD_TYPES.BUTTERFLY_SEED);
		EntityTemplates.CreateAndRegisterPreviewForPlant(gameObject2, "ButterflyPlant_preview", Assets.GetAnim("pollinator_plant_kanim"), "place", 1, 2);
		gameObject.AddOrGet<Growing>().maxAge = 0f;
		gameObject.AddOrGet<Crop>().cropSpawnOffset = new Vector3(-0.0365f, 1.26175f, 0f);
		return gameObject;
	}

	public void OnPrefabInit(GameObject prefab)
	{
	}

	public void OnSpawn(GameObject inst)
	{
	}

	public const string ID = "ButterflyPlant";

	public const string SEED_ID = "ButterflyPlantSeed";
}
