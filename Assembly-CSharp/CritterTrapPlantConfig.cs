using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000294 RID: 660
public class CritterTrapPlantConfig : IEntityConfig, IHasDlcRestrictions
{
	// Token: 0x060009A6 RID: 2470 RVA: 0x000AA117 File Offset: 0x000A8317
	public string[] GetRequiredDlcIds()
	{
		return DlcManager.EXPANSION1;
	}

	// Token: 0x060009A7 RID: 2471 RVA: 0x000AA765 File Offset: 0x000A8965
	public string[] GetForbiddenDlcIds()
	{
		return null;
	}

	// Token: 0x060009A8 RID: 2472 RVA: 0x001706EC File Offset: 0x0016E8EC
	public GameObject CreatePrefab()
	{
		string id = "CritterTrapPlant";
		string name = STRINGS.CREATURES.SPECIES.CRITTERTRAPPLANT.NAME;
		string desc = STRINGS.CREATURES.SPECIES.CRITTERTRAPPLANT.DESC;
		float mass = 4f;
		EffectorValues tier = DECOR.BONUS.TIER1;
		KAnimFile anim = Assets.GetAnim("venus_critter_trap_kanim");
		string initialAnim = "idle_open";
		Grid.SceneLayer sceneLayer = Grid.SceneLayer.BuildingBack;
		int width = 1;
		int height = 2;
		EffectorValues decor = tier;
		float freezing_ = TUNING.CREATURES.TEMPERATURE.FREEZING_3;
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, name, desc, mass, anim, initialAnim, sceneLayer, width, height, decor, default(EffectorValues), SimHashes.Creature, null, freezing_);
		EntityTemplates.ExtendEntityToBasicPlant(gameObject, TUNING.CREATURES.TEMPERATURE.FREEZING_10, TUNING.CREATURES.TEMPERATURE.FREEZING_9, TUNING.CREATURES.TEMPERATURE.FREEZING, TUNING.CREATURES.TEMPERATURE.COOL, null, false, 0f, 0.15f, "PlantMeat", true, true, true, false, 2400f, 0f, 2200f, "CritterTrapPlantOriginal", STRINGS.CREATURES.SPECIES.CRITTERTRAPPLANT.NAME);
		UnityEngine.Object.DestroyImmediate(gameObject.GetComponent<MutantPlant>());
		TrapTrigger trapTrigger = gameObject.AddOrGet<TrapTrigger>();
		trapTrigger.trappableCreatures = new Tag[]
		{
			GameTags.Creatures.Walker,
			GameTags.Creatures.Hoverer
		};
		trapTrigger.trappedOffset = new Vector2(0.5f, 0f);
		trapTrigger.enabled = false;
		CritterTrapPlant critterTrapPlant = gameObject.AddOrGet<CritterTrapPlant>();
		critterTrapPlant.gasOutputRate = 0.041666668f;
		critterTrapPlant.outputElement = SimHashes.Hydrogen;
		critterTrapPlant.gasVentThreshold = 33.25f;
		gameObject.AddOrGet<LoopingSounds>();
		gameObject.AddOrGet<Storage>();
		Tag tag = ElementLoader.FindElementByHash(SimHashes.DirtyWater).tag;
		EntityTemplates.ExtendPlantToIrrigated(gameObject, new PlantElementAbsorber.ConsumeInfo[]
		{
			new PlantElementAbsorber.ConsumeInfo
			{
				tag = tag,
				massConsumptionRate = 0.016666668f
			}
		});
		GameObject plant = gameObject;
		SeedProducer.ProductionType productionType = SeedProducer.ProductionType.Hidden;
		string id2 = "CritterTrapPlantSeed";
		string name2 = STRINGS.CREATURES.SPECIES.SEEDS.CRITTERTRAPPLANT.NAME;
		string desc2 = STRINGS.CREATURES.SPECIES.SEEDS.CRITTERTRAPPLANT.DESC;
		KAnimFile anim2 = Assets.GetAnim("seed_critter_trap_kanim");
		string initialAnim2 = "object";
		int numberOfSeeds = 1;
		List<Tag> list = new List<Tag>();
		list.Add(GameTags.CropSeed);
		SingleEntityReceptacle.ReceptacleDirection planterDirection = SingleEntityReceptacle.ReceptacleDirection.Top;
		string domesticatedDescription = STRINGS.CREATURES.SPECIES.CRITTERTRAPPLANT.DOMESTICATEDDESC;
		EntityTemplates.CreateAndRegisterPreviewForPlant(EntityTemplates.CreateAndRegisterSeedForPlant(plant, this, productionType, id2, name2, desc2, anim2, initialAnim2, numberOfSeeds, list, planterDirection, default(Tag), 21, domesticatedDescription, EntityTemplates.CollisionShape.CIRCLE, 0.3f, 0.3f, null, "", false), "CritterTrapPlant_preview", Assets.GetAnim("venus_critter_trap_kanim"), "place", 1, 2);
		return gameObject;
	}

	// Token: 0x060009A9 RID: 2473 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x060009AA RID: 2474 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000768 RID: 1896
	public const string ID = "CritterTrapPlant";

	// Token: 0x04000769 RID: 1897
	public const float WATER_RATE = 0.016666668f;

	// Token: 0x0400076A RID: 1898
	public const float GAS_RATE = 0.041666668f;

	// Token: 0x0400076B RID: 1899
	public const float GAS_VENT_THRESHOLD = 33.25f;
}
