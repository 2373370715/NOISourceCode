using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000296 RID: 662
public class EvilFlowerConfig : IEntityConfig
{
	// Token: 0x060009B3 RID: 2483 RVA: 0x00170A6C File Offset: 0x0016EC6C
	public GameObject CreatePrefab()
	{
		string id = "EvilFlower";
		string name = STRINGS.CREATURES.SPECIES.EVILFLOWER.NAME;
		string desc = STRINGS.CREATURES.SPECIES.EVILFLOWER.DESC;
		float mass = 1f;
		EffectorValues positive_DECOR_EFFECT = this.POSITIVE_DECOR_EFFECT;
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, name, desc, mass, Assets.GetAnim("potted_evilflower_kanim"), "grow_seed", Grid.SceneLayer.BuildingFront, 1, 1, positive_DECOR_EFFECT, default(EffectorValues), SimHashes.Creature, null, 293f);
		EntityTemplates.ExtendEntityToBasicPlant(gameObject, 168.15f, 258.15f, 513.15f, 563.15f, new SimHashes[]
		{
			SimHashes.CarbonDioxide
		}, true, 0f, 0.15f, null, true, false, true, true, 2400f, 0f, 12200f, "EvilFlowerOriginal", STRINGS.CREATURES.SPECIES.EVILFLOWER.NAME);
		EvilFlower evilFlower = gameObject.AddOrGet<EvilFlower>();
		evilFlower.positive_decor_effect = this.POSITIVE_DECOR_EFFECT;
		evilFlower.negative_decor_effect = this.NEGATIVE_DECOR_EFFECT;
		GameObject plant = gameObject;
		IHasDlcRestrictions dlcRestrictions = this as IHasDlcRestrictions;
		SeedProducer.ProductionType productionType = SeedProducer.ProductionType.Hidden;
		string id2 = "EvilFlowerSeed";
		string name2 = STRINGS.CREATURES.SPECIES.SEEDS.EVILFLOWER.NAME;
		string desc2 = STRINGS.CREATURES.SPECIES.SEEDS.EVILFLOWER.DESC;
		KAnimFile anim = Assets.GetAnim("seed_potted_evilflower_kanim");
		string initialAnim = "object";
		int numberOfSeeds = 1;
		List<Tag> list = new List<Tag>();
		list.Add(GameTags.DecorSeed);
		SingleEntityReceptacle.ReceptacleDirection planterDirection = SingleEntityReceptacle.ReceptacleDirection.Top;
		string domesticatedDescription = STRINGS.CREATURES.SPECIES.EVILFLOWER.DOMESTICATEDDESC;
		EntityTemplates.CreateAndRegisterPreviewForPlant(EntityTemplates.CreateAndRegisterSeedForPlant(plant, dlcRestrictions, productionType, id2, name2, desc2, anim, initialAnim, numberOfSeeds, list, planterDirection, default(Tag), 19, domesticatedDescription, EntityTemplates.CollisionShape.CIRCLE, 0.4f, 0.4f, null, "", false), "EvilFlower_preview", Assets.GetAnim("potted_evilflower_kanim"), "place", 1, 1);
		DiseaseDropper.Def def = gameObject.AddOrGetDef<DiseaseDropper.Def>();
		def.diseaseIdx = Db.Get().Diseases.GetIndex("ZombieSpores");
		def.emitFrequency = 1f;
		def.averageEmitPerSecond = 1000;
		def.singleEmitQuantity = 100000;
		gameObject.AddOrGet<DiseaseSourceVisualizer>().alwaysShowDisease = "ZombieSpores";
		return gameObject;
	}

	// Token: 0x060009B4 RID: 2484 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x060009B5 RID: 2485 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000770 RID: 1904
	public const string ID = "EvilFlower";

	// Token: 0x04000771 RID: 1905
	public const string SEED_ID = "EvilFlowerSeed";

	// Token: 0x04000772 RID: 1906
	public readonly EffectorValues POSITIVE_DECOR_EFFECT = DECOR.BONUS.TIER7;

	// Token: 0x04000773 RID: 1907
	public readonly EffectorValues NEGATIVE_DECOR_EFFECT = DECOR.PENALTY.TIER5;

	// Token: 0x04000774 RID: 1908
	public const int GERMS_PER_SECOND = 1000;
}
