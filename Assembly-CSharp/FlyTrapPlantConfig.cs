using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

public class FlyTrapPlantConfig : IEntityConfig, IHasDlcRestrictions
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
		string id = "FlyTrapPlant";
		string name = STRINGS.CREATURES.SPECIES.FLYTRAPPLANT.NAME;
		string desc = STRINGS.CREATURES.SPECIES.FLYTRAPPLANT.DESC;
		float mass = 1f;
		EffectorValues tier = DECOR.BONUS.TIER1;
		KAnimFile anim = Assets.GetAnim("ceiling_carnie_kanim");
		string initialAnim = "idle_empty";
		Grid.SceneLayer sceneLayer = Grid.SceneLayer.BuildingFront;
		int width = 1;
		int height = 2;
		EffectorValues decor = tier;
		List<Tag> additionalTags = new List<Tag>
		{
			GameTags.Hanging
		};
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, name, desc, mass, anim, initialAnim, sceneLayer, width, height, decor, default(EffectorValues), SimHashes.Creature, additionalTags, 291.15f);
		EntityTemplates.MakeHangingOffsets(gameObject, 1, 2);
		EntityTemplates.ExtendEntityToBasicPlant(gameObject, 273.15f, 283.15f, 328.15f, 348.15f, null, true, 0f, 0.15f, SimHashes.Amber.ToString(), true, true, true, true, 2400f, 0f, 7400f, "FlyTrapPlantOriginal", STRINGS.CREATURES.SPECIES.FLYTRAPPLANT.NAME);
		gameObject.GetComponent<UprootedMonitor>().monitorCells = new CellOffset[]
		{
			new CellOffset(0, 1)
		};
		gameObject.AddOrGet<StandardCropPlant>();
		gameObject.AddOrGet<FlytrapConsumptionMonitor>();
		gameObject.AddOrGet<Growing>().MaxMaturityValuePercentageToSpawnWith = 0f;
		GameObject plant = gameObject;
		SeedProducer.ProductionType productionType = SeedProducer.ProductionType.Harvest;
		string id2 = "FlyTrapPlantSeed";
		string name2 = STRINGS.CREATURES.SPECIES.SEEDS.FLYTRAPPLANT.NAME;
		string desc2 = STRINGS.CREATURES.SPECIES.SEEDS.FLYTRAPPLANT.DESC;
		KAnimFile anim2 = Assets.GetAnim("seed_ceiling_carnie_kanim");
		string initialAnim2 = "object";
		int numberOfSeeds = 1;
		List<Tag> list = new List<Tag>();
		list.Add(GameTags.CropSeed);
		SingleEntityReceptacle.ReceptacleDirection planterDirection = SingleEntityReceptacle.ReceptacleDirection.Bottom;
		string domesticatedDescription = STRINGS.CREATURES.SPECIES.FLYTRAPPLANT.DOMESTICATEDDESC;
		EntityTemplates.MakeHangingOffsets(EntityTemplates.CreateAndRegisterPreviewForPlant(EntityTemplates.CreateAndRegisterSeedForPlant(plant, this, productionType, id2, name2, desc2, anim2, initialAnim2, numberOfSeeds, list, planterDirection, default(Tag), 4, domesticatedDescription, EntityTemplates.CollisionShape.CIRCLE, 0.3f, 0.3f, null, "", false), "FlyTrapPlant_preview", Assets.GetAnim("ceiling_carnie_kanim"), "place", 1, 2), 1, 2);
		return gameObject;
	}

	public void OnPrefabInit(GameObject inst)
	{
		inst.AddOrGet<StandardCropPlant>().anims = FlyTrapPlantConfig.Default_StandardCropAnimSet;
	}

	public void OnSpawn(GameObject inst)
	{
	}

	public const string ID = "FlyTrapPlant";

	public const string SEED_ID = "FlyTrapPlantSeed";

	public static readonly StandardCropPlant.AnimSet Default_StandardCropAnimSet = new StandardCropPlant.AnimSet
	{
		pre_grow = "grow_pre",
		grow = "grow",
		grow_pst = "grow_pst",
		idle_full = "idle_full",
		wilt_base = "wilt",
		harvest = "harvest",
		waning = "waning",
		grow_playmode = KAnim.PlayMode.Paused
	};

	public const int DIGESTION_DURATION_CYCLES = 12;

	public const float DIGESTION_DURATION = 7200f;

	public const int AMBER_PER_HARVEST_KG = 264;
}
