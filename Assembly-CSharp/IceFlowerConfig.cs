using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020002AF RID: 687
public class IceFlowerConfig : IEntityConfig, IHasDlcRestrictions
{
	// Token: 0x06000A0A RID: 2570 RVA: 0x000AA536 File Offset: 0x000A8736
	public string[] GetRequiredDlcIds()
	{
		return DlcManager.DLC2;
	}

	// Token: 0x06000A0B RID: 2571 RVA: 0x000AA765 File Offset: 0x000A8965
	public string[] GetForbiddenDlcIds()
	{
		return null;
	}

	// Token: 0x06000A0C RID: 2572 RVA: 0x00172E7C File Offset: 0x0017107C
	public GameObject CreatePrefab()
	{
		string id = "IceFlower";
		string name = STRINGS.CREATURES.SPECIES.ICEFLOWER.NAME;
		string desc = STRINGS.CREATURES.SPECIES.ICEFLOWER.DESC;
		float mass = 1f;
		EffectorValues positive_DECOR_EFFECT = this.POSITIVE_DECOR_EFFECT;
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, name, desc, mass, Assets.GetAnim("potted_ice_flower_kanim"), "grow_seed", Grid.SceneLayer.BuildingFront, 1, 1, positive_DECOR_EFFECT, default(EffectorValues), SimHashes.Creature, null, 243.15f);
		EntityTemplates.ExtendEntityToBasicPlant(gameObject, 173.15f, 203.15f, 278.15f, 318.15f, new SimHashes[]
		{
			SimHashes.Oxygen,
			SimHashes.ContaminatedOxygen,
			SimHashes.CarbonDioxide,
			SimHashes.ChlorineGas,
			SimHashes.Hydrogen
		}, true, 0f, 0.15f, null, true, false, true, true, 2400f, 0f, 2200f, "IceFlowerOriginal", STRINGS.CREATURES.SPECIES.ICEFLOWER.NAME);
		PrickleGrass prickleGrass = gameObject.AddOrGet<PrickleGrass>();
		prickleGrass.positive_decor_effect = this.POSITIVE_DECOR_EFFECT;
		prickleGrass.negative_decor_effect = this.NEGATIVE_DECOR_EFFECT;
		GameObject plant = gameObject;
		SeedProducer.ProductionType productionType = SeedProducer.ProductionType.Hidden;
		string id2 = "IceFlowerSeed";
		string name2 = STRINGS.CREATURES.SPECIES.SEEDS.ICEFLOWER.NAME;
		string desc2 = STRINGS.CREATURES.SPECIES.SEEDS.ICEFLOWER.DESC;
		KAnimFile anim = Assets.GetAnim("seed_ice_flower_kanim");
		string initialAnim = "object";
		int numberOfSeeds = 1;
		List<Tag> list = new List<Tag>();
		list.Add(GameTags.DecorSeed);
		SingleEntityReceptacle.ReceptacleDirection planterDirection = SingleEntityReceptacle.ReceptacleDirection.Top;
		string domesticatedDescription = STRINGS.CREATURES.SPECIES.ICEFLOWER.DOMESTICATEDDESC;
		EntityTemplates.CreateAndRegisterPreviewForPlant(EntityTemplates.CreateAndRegisterSeedForPlant(plant, this, productionType, id2, name2, desc2, anim, initialAnim, numberOfSeeds, list, planterDirection, default(Tag), 12, domesticatedDescription, EntityTemplates.CollisionShape.RECTANGLE, 0.8f, 0.6f, null, "", false), "IceFlower_preview", Assets.GetAnim("potted_ice_flower_kanim"), "place", 1, 1);
		return gameObject;
	}

	// Token: 0x06000A0D RID: 2573 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000A0E RID: 2574 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x040007EB RID: 2027
	public const string ID = "IceFlower";

	// Token: 0x040007EC RID: 2028
	public const string SEED_ID = "IceFlowerSeed";

	// Token: 0x040007ED RID: 2029
	public readonly EffectorValues POSITIVE_DECOR_EFFECT = DECOR.BONUS.TIER3;

	// Token: 0x040007EE RID: 2030
	public readonly EffectorValues NEGATIVE_DECOR_EFFECT = DECOR.PENALTY.TIER3;
}
