using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000295 RID: 661
public class CylindricaConfig : IEntityConfig, IHasDlcRestrictions
{
	// Token: 0x060009AC RID: 2476 RVA: 0x000AA117 File Offset: 0x000A8317
	public string[] GetRequiredDlcIds()
	{
		return DlcManager.EXPANSION1;
	}

	// Token: 0x060009AD RID: 2477 RVA: 0x000AA765 File Offset: 0x000A8965
	public string[] GetForbiddenDlcIds()
	{
		return null;
	}

	// Token: 0x060009AE RID: 2478 RVA: 0x00170900 File Offset: 0x0016EB00
	public GameObject CreatePrefab()
	{
		string id = "Cylindrica";
		string name = STRINGS.CREATURES.SPECIES.CYLINDRICA.NAME;
		string desc = STRINGS.CREATURES.SPECIES.CYLINDRICA.DESC;
		float mass = 1f;
		EffectorValues positive_DECOR_EFFECT = CylindricaConfig.POSITIVE_DECOR_EFFECT;
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, name, desc, mass, Assets.GetAnim("potted_cylindricafan_kanim"), "grow_seed", Grid.SceneLayer.BuildingFront, 1, 1, positive_DECOR_EFFECT, default(EffectorValues), SimHashes.Creature, null, 298.15f);
		EntityTemplates.ExtendEntityToBasicPlant(gameObject, 288.15f, 293.15f, 323.15f, 373.15f, new SimHashes[]
		{
			SimHashes.Oxygen,
			SimHashes.ContaminatedOxygen,
			SimHashes.CarbonDioxide
		}, true, 0f, 0.15f, null, true, false, true, true, 2400f, 0f, 2200f, "CylindricaOriginal", STRINGS.CREATURES.SPECIES.CYLINDRICA.NAME);
		PrickleGrass prickleGrass = gameObject.AddOrGet<PrickleGrass>();
		prickleGrass.positive_decor_effect = CylindricaConfig.POSITIVE_DECOR_EFFECT;
		prickleGrass.negative_decor_effect = CylindricaConfig.NEGATIVE_DECOR_EFFECT;
		GameObject plant = gameObject;
		SeedProducer.ProductionType productionType = SeedProducer.ProductionType.Hidden;
		string id2 = "CylindricaSeed";
		string name2 = STRINGS.CREATURES.SPECIES.SEEDS.CYLINDRICA.NAME;
		string desc2 = STRINGS.CREATURES.SPECIES.SEEDS.CYLINDRICA.DESC;
		KAnimFile anim = Assets.GetAnim("seed_potted_cylindricafan_kanim");
		string initialAnim = "object";
		int numberOfSeeds = 1;
		List<Tag> list = new List<Tag>();
		list.Add(GameTags.DecorSeed);
		SingleEntityReceptacle.ReceptacleDirection planterDirection = SingleEntityReceptacle.ReceptacleDirection.Top;
		string domesticatedDescription = STRINGS.CREATURES.SPECIES.CYLINDRICA.DOMESTICATEDDESC;
		EntityTemplates.CreateAndRegisterPreviewForPlant(EntityTemplates.CreateAndRegisterSeedForPlant(plant, this, productionType, id2, name2, desc2, anim, initialAnim, numberOfSeeds, list, planterDirection, default(Tag), 12, domesticatedDescription, EntityTemplates.CollisionShape.CIRCLE, 0.25f, 0.25f, null, "", false), "Cylindrica_preview", Assets.GetAnim("potted_cylindricafan_kanim"), "place", 1, 1);
		return gameObject;
	}

	// Token: 0x060009AF RID: 2479 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x060009B0 RID: 2480 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x0400076C RID: 1900
	public const string ID = "Cylindrica";

	// Token: 0x0400076D RID: 1901
	public const string SEED_ID = "CylindricaSeed";

	// Token: 0x0400076E RID: 1902
	public static readonly EffectorValues POSITIVE_DECOR_EFFECT = DECOR.BONUS.TIER3;

	// Token: 0x0400076F RID: 1903
	public static readonly EffectorValues NEGATIVE_DECOR_EFFECT = DECOR.PENALTY.TIER3;
}
