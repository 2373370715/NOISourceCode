using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020002B5 RID: 693
public class OxyfernConfig : IEntityConfig
{
	// Token: 0x06000A26 RID: 2598 RVA: 0x001735F0 File Offset: 0x001717F0
	public GameObject CreatePrefab()
	{
		string id = "Oxyfern";
		string name = STRINGS.CREATURES.SPECIES.OXYFERN.NAME;
		string desc = STRINGS.CREATURES.SPECIES.OXYFERN.DESC;
		float mass = 1f;
		EffectorValues tier = DECOR.PENALTY.TIER1;
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, name, desc, mass, Assets.GetAnim("oxy_fern_kanim"), "idle_full", Grid.SceneLayer.BuildingBack, 1, 2, tier, default(EffectorValues), SimHashes.Creature, null, 293f);
		gameObject = EntityTemplates.ExtendEntityToBasicPlant(gameObject, 253.15f, 273.15f, 313.15f, 373.15f, new SimHashes[]
		{
			SimHashes.CarbonDioxide
		}, true, 0f, 0.025f, null, true, false, true, true, 2400f, 0f, 2200f, "OxyfernOriginal", STRINGS.CREATURES.SPECIES.OXYFERN.NAME);
		Tag tag = ElementLoader.FindElementByHash(SimHashes.Water).tag;
		EntityTemplates.ExtendPlantToIrrigated(gameObject, new PlantElementAbsorber.ConsumeInfo[]
		{
			new PlantElementAbsorber.ConsumeInfo
			{
				tag = tag,
				massConsumptionRate = 0.031666666f
			}
		});
		EntityTemplates.ExtendPlantToFertilizable(gameObject, new PlantElementAbsorber.ConsumeInfo[]
		{
			new PlantElementAbsorber.ConsumeInfo
			{
				tag = GameTags.Dirt,
				massConsumptionRate = 0.006666667f
			}
		});
		gameObject.AddOrGet<Oxyfern>();
		gameObject.AddOrGet<LoopingSounds>();
		Storage storage = gameObject.AddOrGet<Storage>();
		storage.showInUI = false;
		storage.capacityKg = 1f;
		ElementConsumer elementConsumer = gameObject.AddOrGet<ElementConsumer>();
		elementConsumer.showInStatusPanel = false;
		elementConsumer.storeOnConsume = true;
		elementConsumer.storage = storage;
		elementConsumer.elementToConsume = SimHashes.CarbonDioxide;
		elementConsumer.configuration = ElementConsumer.Configuration.Element;
		elementConsumer.consumptionRadius = 2;
		elementConsumer.EnableConsumption(true);
		elementConsumer.sampleCellOffset = new Vector3(0f, 0f);
		elementConsumer.consumptionRate = 0.00015625001f;
		ElementConverter elementConverter = gameObject.AddOrGet<ElementConverter>();
		elementConverter.OutputMultiplier = 50f;
		elementConverter.consumedElements = new ElementConverter.ConsumedElement[]
		{
			new ElementConverter.ConsumedElement(SimHashes.CarbonDioxide.ToString().ToTag(), 0.00062500004f, true)
		};
		elementConverter.outputElements = new ElementConverter.OutputElement[]
		{
			new ElementConverter.OutputElement(0.031250004f, SimHashes.Oxygen, 0f, true, false, 0f, 1f, 0.75f, byte.MaxValue, 0, true)
		};
		GameObject plant = gameObject;
		IHasDlcRestrictions dlcRestrictions = this as IHasDlcRestrictions;
		SeedProducer.ProductionType productionType = SeedProducer.ProductionType.Hidden;
		string id2 = "OxyfernSeed";
		string name2 = STRINGS.CREATURES.SPECIES.SEEDS.OXYFERN.NAME;
		string desc2 = STRINGS.CREATURES.SPECIES.SEEDS.OXYFERN.DESC;
		KAnimFile anim = Assets.GetAnim("seed_oxyfern_kanim");
		string initialAnim = "object";
		int numberOfSeeds = 1;
		List<Tag> list = new List<Tag>();
		list.Add(GameTags.CropSeed);
		SingleEntityReceptacle.ReceptacleDirection planterDirection = SingleEntityReceptacle.ReceptacleDirection.Top;
		string domesticatedDescription = STRINGS.CREATURES.SPECIES.OXYFERN.DOMESTICATEDDESC;
		EntityTemplates.CreateAndRegisterPreviewForPlant(EntityTemplates.CreateAndRegisterSeedForPlant(plant, dlcRestrictions, productionType, id2, name2, desc2, anim, initialAnim, numberOfSeeds, list, planterDirection, default(Tag), 20, domesticatedDescription, EntityTemplates.CollisionShape.CIRCLE, 0.3f, 0.3f, null, "", false), "Oxyfern_preview", Assets.GetAnim("oxy_fern_kanim"), "place", 1, 2);
		SoundEventVolumeCache.instance.AddVolume("oxy_fern_kanim", "MealLice_harvest", NOISE_POLLUTION.CREATURES.TIER3);
		SoundEventVolumeCache.instance.AddVolume("oxy_fern_kanim", "MealLice_LP", NOISE_POLLUTION.CREATURES.TIER4);
		return gameObject;
	}

	// Token: 0x06000A27 RID: 2599 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnPrefabInit(GameObject prefab)
	{
	}

	// Token: 0x06000A28 RID: 2600 RVA: 0x000AECD6 File Offset: 0x000ACED6
	public void OnSpawn(GameObject inst)
	{
		inst.GetComponent<Oxyfern>().SetConsumptionRate();
	}

	// Token: 0x040007F9 RID: 2041
	public const string ID = "Oxyfern";

	// Token: 0x040007FA RID: 2042
	public const string SEED_ID = "OxyfernSeed";

	// Token: 0x040007FB RID: 2043
	public const float WATER_CONSUMPTION_RATE = 0.031666666f;

	// Token: 0x040007FC RID: 2044
	public const float FERTILIZATION_RATE = 0.006666667f;

	// Token: 0x040007FD RID: 2045
	public const float CO2_RATE = 0.00062500004f;

	// Token: 0x040007FE RID: 2046
	private const float CONVERSION_RATIO = 50f;

	// Token: 0x040007FF RID: 2047
	public const float OXYGEN_RATE = 0.031250004f;
}
