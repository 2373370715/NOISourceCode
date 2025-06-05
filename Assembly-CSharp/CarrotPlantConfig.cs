using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000290 RID: 656
public class CarrotPlantConfig : IEntityConfig, IHasDlcRestrictions
{
	// Token: 0x06000993 RID: 2451 RVA: 0x000AA536 File Offset: 0x000A8736
	public string[] GetRequiredDlcIds()
	{
		return DlcManager.DLC2;
	}

	// Token: 0x06000994 RID: 2452 RVA: 0x000AA765 File Offset: 0x000A8965
	public string[] GetForbiddenDlcIds()
	{
		return null;
	}

	// Token: 0x06000995 RID: 2453 RVA: 0x0016FEE8 File Offset: 0x0016E0E8
	public GameObject CreatePrefab()
	{
		string id = "CarrotPlant";
		string name = STRINGS.CREATURES.SPECIES.CARROTPLANT.NAME;
		string desc = STRINGS.CREATURES.SPECIES.CARROTPLANT.DESC;
		float mass = 1f;
		EffectorValues tier = DECOR.PENALTY.TIER1;
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, name, desc, mass, Assets.GetAnim("purpleroot_kanim"), "idle_empty", Grid.SceneLayer.BuildingBack, 1, 2, tier, default(EffectorValues), SimHashes.Creature, null, 255f);
		GameObject template = gameObject;
		float temperature_lethal_low = 118.149994f;
		float temperature_warning_low = 218.15f;
		float temperature_warning_high = 259.15f;
		float temperature_lethal_high = 269.15f;
		string text = CarrotConfig.ID;
		EntityTemplates.ExtendEntityToBasicPlant(template, temperature_lethal_low, temperature_warning_low, temperature_warning_high, temperature_lethal_high, new SimHashes[]
		{
			SimHashes.Oxygen,
			SimHashes.ContaminatedOxygen,
			SimHashes.CarbonDioxide
		}, true, 0f, 0.15f, text, true, true, true, true, 2400f, 0f, 4600f, "CarrotPlantOriginal", STRINGS.CREATURES.SPECIES.CARROTPLANT.NAME);
		gameObject.AddOrGet<StandardCropPlant>();
		gameObject.AddOrGet<DirectlyEdiblePlant_Growth>();
		gameObject.AddOrGet<LoopingSounds>();
		GameObject plant = gameObject;
		SeedProducer.ProductionType productionType = SeedProducer.ProductionType.Harvest;
		string id2 = "CarrotPlantSeed";
		string name2 = STRINGS.CREATURES.SPECIES.SEEDS.CARROTPLANT.NAME;
		string desc2 = STRINGS.CREATURES.SPECIES.SEEDS.CARROTPLANT.DESC;
		KAnimFile anim = Assets.GetAnim("seed_purpleroot_kanim");
		string initialAnim = "object";
		int numberOfSeeds = 1;
		List<Tag> list = new List<Tag>();
		list.Add(GameTags.CropSeed);
		SingleEntityReceptacle.ReceptacleDirection planterDirection = SingleEntityReceptacle.ReceptacleDirection.Top;
		text = STRINGS.CREATURES.SPECIES.CARROTPLANT.DOMESTICATEDDESC;
		GameObject seed = EntityTemplates.CreateAndRegisterSeedForPlant(plant, this, productionType, id2, name2, desc2, anim, initialAnim, numberOfSeeds, list, planterDirection, default(Tag), 1, text, EntityTemplates.CollisionShape.CIRCLE, 0.3f, 0.3f, null, "", false);
		EntityTemplates.ExtendPlantToIrrigated(gameObject, new PlantElementAbsorber.ConsumeInfo[]
		{
			new PlantElementAbsorber.ConsumeInfo
			{
				tag = SimHashes.Ethanol.CreateTag(),
				massConsumptionRate = 0.025f
			}
		});
		EntityTemplates.CreateAndRegisterPreviewForPlant(seed, "CarrotPlant_preview", Assets.GetAnim("purpleroot_kanim"), "place", 1, 2);
		SoundEventVolumeCache.instance.AddVolume("bristleblossom_kanim", "PrickleFlower_harvest", NOISE_POLLUTION.CREATURES.TIER3);
		SoundEventVolumeCache.instance.AddVolume("bristleblossom_kanim", "PrickleFlower_grow", NOISE_POLLUTION.CREATURES.TIER3);
		return gameObject;
	}

	// Token: 0x06000996 RID: 2454 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnPrefabInit(GameObject prefab)
	{
	}

	// Token: 0x06000997 RID: 2455 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000753 RID: 1875
	public const string ID = "CarrotPlant";

	// Token: 0x04000754 RID: 1876
	public const string SEED_ID = "CarrotPlantSeed";

	// Token: 0x04000755 RID: 1877
	public const float Temperature_lethal_low = 118.149994f;

	// Token: 0x04000756 RID: 1878
	public const float Temperature_warning_low = 218.15f;

	// Token: 0x04000757 RID: 1879
	public const float Temperature_lethal_high = 269.15f;

	// Token: 0x04000758 RID: 1880
	public const float Temperature_warning_high = 259.15f;

	// Token: 0x04000759 RID: 1881
	public const float FERTILIZATION_RATE = 0.025f;
}
