using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

public class KelpPlantConfig : IEntityConfig, IHasDlcRestrictions
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
		string id = "KelpPlant";
		string name = STRINGS.CREATURES.SPECIES.KELPPLANT.NAME;
		string desc = STRINGS.CREATURES.SPECIES.KELPPLANT.DESC;
		float mass = 4f;
		EffectorValues tier = DECOR.BONUS.TIER1;
		KAnimFile anim = Assets.GetAnim("kelp_kanim");
		string initialAnim = "idle_empty";
		Grid.SceneLayer sceneLayer = Grid.SceneLayer.BuildingFront;
		int width = 1;
		int height = 2;
		EffectorValues decor = tier;
		List<Tag> additionalTags = new List<Tag>
		{
			GameTags.Hanging
		};
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, name, desc, mass, anim, initialAnim, sceneLayer, width, height, decor, default(EffectorValues), SimHashes.Creature, additionalTags, 297.15f);
		EntityTemplates.MakeHangingOffsets(gameObject, 1, 2);
		GameObject template = gameObject;
		float temperature_lethal_low = 253.15f;
		float temperature_warning_low = 263.15f;
		float temperature_warning_high = 358.15f;
		float temperature_lethal_high = 373.15f;
		string id2 = KelpConfig.ID;
		string text = STRINGS.CREATURES.SPECIES.KELPPLANT.NAME;
		EntityTemplates.ExtendEntityToBasicPlant(template, temperature_lethal_low, temperature_warning_low, temperature_warning_high, temperature_lethal_high, KelpPlantConfig.ALLOWED_ELEMENTS, false, 0f, 0.15f, id2, false, true, true, true, 2400f, 0f, 7400f, "KelpPlantOriginal", text);
		gameObject.AddOrGet<PressureVulnerable>().allCellsMustBeSafe = true;
		EntityTemplates.ExtendPlantToFertilizable(gameObject, new PlantElementAbsorber.ConsumeInfo[]
		{
			new PlantElementAbsorber.ConsumeInfo
			{
				tag = SimHashes.ToxicSand.ToString(),
				massConsumptionRate = 0.016666668f
			}
		});
		gameObject.AddOrGet<DirectlyEdiblePlant_Growth>();
		gameObject.GetComponent<UprootedMonitor>().monitorCells = new CellOffset[]
		{
			new CellOffset(0, 1)
		};
		gameObject.AddOrGet<StandardCropPlant>();
		GameObject plant = gameObject;
		SeedProducer.ProductionType productionType = SeedProducer.ProductionType.Harvest;
		string id3 = "KelpPlantSeed";
		string name2 = STRINGS.CREATURES.SPECIES.SEEDS.KELPPLANT.NAME;
		string desc2 = STRINGS.CREATURES.SPECIES.SEEDS.KELPPLANT.DESC;
		KAnimFile anim2 = Assets.GetAnim("seed_kelp_kanim");
		string initialAnim2 = "object";
		int numberOfSeeds = 1;
		List<Tag> list = new List<Tag>();
		list.Add(GameTags.CropSeed);
		SingleEntityReceptacle.ReceptacleDirection planterDirection = SingleEntityReceptacle.ReceptacleDirection.Bottom;
		text = STRINGS.CREATURES.SPECIES.KELPPLANT.DOMESTICATEDDESC;
		EntityTemplates.MakeHangingOffsets(EntityTemplates.CreateAndRegisterPreviewForPlant(EntityTemplates.CreateAndRegisterSeedForPlant(plant, this, productionType, id3, name2, desc2, anim2, initialAnim2, numberOfSeeds, list, planterDirection, default(Tag), 4, text, EntityTemplates.CollisionShape.CIRCLE, 0.3f, 0.3f, null, "", false), "KelpPlant_preview", Assets.GetAnim("kelp_kanim"), "place", 1, 2), 1, 2);
		return gameObject;
	}

	public void OnPrefabInit(GameObject inst)
	{
	}

	public void OnSpawn(GameObject inst)
	{
	}

	public const string ID = "KelpPlant";

	public const string SEED_ID = "KelpPlantSeed";

	public const int YIELD_UNITS_PER_HARVEST = 50;

	public const float LIFETIME_CYCLES = 5f;

	public const float FERTILIZATION_RATE = 0.016666668f;

	public static SimHashes[] ALLOWED_ELEMENTS = new SimHashes[]
	{
		SimHashes.Water,
		SimHashes.DirtyWater,
		SimHashes.SaltWater,
		SimHashes.Brine,
		SimHashes.PhytoOil,
		SimHashes.NaturalResin
	};

	public const float CALCULATED_YIELD_MASS_PER_HARVEST = 50f;

	public const float CALCULATED_YIELD_MASS_PER_CYCLE = 10f;

	public const float CALCULATED_GROWTH_PER_CYCLE = 0.2f;

	public const float CALCULATED_LIFETIME_SEC = 3000f;
}
