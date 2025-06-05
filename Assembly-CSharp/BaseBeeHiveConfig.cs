using System;
using System.Collections.Generic;
using Klei.AI;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020000D5 RID: 213
public class BaseBeeHiveConfig : IEntityConfig, IHasDlcRestrictions
{
	// Token: 0x0600037B RID: 891 RVA: 0x000AA117 File Offset: 0x000A8317
	public string[] GetRequiredDlcIds()
	{
		return DlcManager.EXPANSION1;
	}

	// Token: 0x0600037C RID: 892 RVA: 0x000AA765 File Offset: 0x000A8965
	public string[] GetForbiddenDlcIds()
	{
		return null;
	}

	// Token: 0x0600037D RID: 893 RVA: 0x00157534 File Offset: 0x00155734
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreatePlacedEntity("BeeHive", STRINGS.BUILDINGS.PREFABS.BEEHIVE.NAME, STRINGS.BUILDINGS.PREFABS.BEEHIVE.DESC, 100f, Assets.GetAnim("beehive_kanim"), "grow_pre", Grid.SceneLayer.Creatures, 2, 3, TUNING.BUILDINGS.DECOR.BONUS.TIER0, NOISE_POLLUTION.NOISY.TIER0, SimHashes.Creature, null, TUNING.CREATURES.TEMPERATURE.FREEZING_3);
		gameObject.GetComponent<InfoDescription>().effect = STRINGS.BUILDINGS.PREFABS.BEEHIVE.EFFECT;
		KPrefabID kprefabID = gameObject.AddOrGet<KPrefabID>();
		kprefabID.AddTag(GameTags.Experimental, false);
		kprefabID.AddTag(GameTags.Creature, false);
		if (Sim.IsRadiationEnabled())
		{
			gameObject.AddOrGet<Storage>().storageFXOffset = new Vector3(1f, 1f, 0f);
			BeeHive.Def def = gameObject.AddOrGetDef<BeeHive.Def>();
			def.beePrefabID = "Bee";
			def.larvaPrefabID = "BeeBaby";
			KAnimFile[] overrideAnims = new KAnimFile[]
			{
				Assets.GetAnim("anim_interacts_beehive_kanim")
			};
			HiveWorkableEmpty hiveWorkableEmpty = gameObject.AddOrGet<HiveWorkableEmpty>();
			hiveWorkableEmpty.workTime = 15f;
			hiveWorkableEmpty.overrideAnims = overrideAnims;
			hiveWorkableEmpty.workLayer = Grid.SceneLayer.Front;
			RadiationEmitter radiationEmitter = gameObject.AddComponent<RadiationEmitter>();
			radiationEmitter.emitRadiusX = 7;
			radiationEmitter.emitRadiusY = 6;
			radiationEmitter.emitRate = 3f;
			radiationEmitter.emitRads = 0f;
			radiationEmitter.emitType = RadiationEmitter.RadiationEmitterType.Pulsing;
			radiationEmitter.emissionOffset = new Vector3(0.5f, 1f, 0f);
			kprefabID.prefabSpawnFn += delegate(GameObject inst)
			{
				inst.GetComponent<RadiationEmitter>().SetEmitting(true);
			};
			gameObject.AddOrGet<Traits>();
			gameObject.AddOrGet<Health>();
			gameObject.AddOrGet<CharacterOverlay>();
			gameObject.AddOrGet<RangedAttackable>();
			FactionAlignment factionAlignment = gameObject.AddOrGet<FactionAlignment>();
			factionAlignment.Alignment = FactionManager.FactionID.Hostile;
			factionAlignment.updatePrioritizable = false;
			gameObject.AddOrGet<Prioritizable>();
			Prioritizable.AddRef(gameObject);
			gameObject.AddOrGet<Effects>();
			gameObject.AddOrGet<TemperatureVulnerable>().Configure(TUNING.CREATURES.TEMPERATURE.FREEZING_9, TUNING.CREATURES.TEMPERATURE.FREEZING_10, TUNING.CREATURES.TEMPERATURE.FREEZING_1, TUNING.CREATURES.TEMPERATURE.FREEZING);
			gameObject.AddOrGet<DrowningMonitor>().canDrownToDeath = false;
			gameObject.AddOrGet<EntombVulnerable>();
			gameObject.AddOrGetDef<DeathMonitor.Def>();
			gameObject.AddOrGetDef<AnimInterruptMonitor.Def>();
			gameObject.AddOrGetDef<HiveGrowthMonitor.Def>();
			gameObject.AddOrGet<FoundationMonitor>().monitorCells = new CellOffset[]
			{
				new CellOffset(0, -1),
				new CellOffset(1, -1)
			};
			gameObject.AddOrGetDef<HiveEatingMonitor.Def>().consumedOre = BeeHiveTuning.CONSUMED_ORE;
			HiveHarvestMonitor.Def def2 = gameObject.AddOrGetDef<HiveHarvestMonitor.Def>();
			def2.producedOre = BeeHiveTuning.PRODUCED_ORE;
			def2.harvestThreshold = 10f;
			Diet diet = new Diet(new Diet.Info[]
			{
				new Diet.Info(new HashSet<Tag>
				{
					BeeHiveTuning.CONSUMED_ORE
				}, BeeHiveTuning.PRODUCED_ORE, BeeHiveTuning.CALORIES_PER_KG_OF_ORE, BeeHiveTuning.POOP_CONVERSTION_RATE, null, 0f, false, Diet.Info.FoodType.EatSolid, false, null)
			});
			gameObject.AddOrGetDef<BeehiveCalorieMonitor.Def>().diet = diet;
			Trait trait = Db.Get().CreateTrait("BeeHiveBaseTrait", STRINGS.BUILDINGS.PREFABS.BEEHIVE.NAME, STRINGS.BUILDINGS.PREFABS.BEEHIVE.DESC, null, false, null, true, true);
			trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.maxAttribute.Id, BeeHiveTuning.STANDARD_STOMACH_SIZE, STRINGS.BUILDINGS.PREFABS.BEEHIVE.NAME, false, false, true));
			trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.deltaAttribute.Id, -BeeHiveTuning.STANDARD_CALORIES_PER_CYCLE / 600f, STRINGS.BUILDINGS.PREFABS.BEEHIVE.NAME, false, false, true));
			trait.Add(new AttributeModifier(Db.Get().Amounts.HitPoints.maxAttribute.Id, 25f, STRINGS.BUILDINGS.PREFABS.BEEHIVE.NAME, false, false, true));
			Modifiers modifiers = gameObject.AddOrGet<Modifiers>();
			modifiers.initialTraits.Add("BeeHiveBaseTrait");
			modifiers.initialAmounts.Add(Db.Get().Amounts.HitPoints.Id);
			modifiers.initialAttributes.Add(Db.Get().CritterAttributes.Metabolism.Id);
			ChoreTable.Builder chore_table = new ChoreTable.Builder().Add(new DeathStates.Def(), true, -1).Add(new AnimInterruptStates.Def(), true, -1).Add(new DisabledCreatureStates.Def("inactive"), true, -1).PushInterruptGroup().Add(new HiveGrowingStates.Def(), true, -1).Add(new HiveHarvestStates.Def(), true, -1).Add(new PlayAnimsStates.Def(GameTags.Creatures.Poop, false, "poop", STRINGS.CREATURES.STATUSITEMS.EXPELLING_SOLID.NAME, STRINGS.CREATURES.STATUSITEMS.EXPELLING_SOLID.TOOLTIP), true, -1).Add(new HiveEatingStates.Def(BeeHiveTuning.CONSUMED_ORE), true, -1).PopInterruptGroup().Add(new IdleStandStillStates.Def(), true, -1);
			EntityTemplates.AddCreatureBrain(gameObject, chore_table, GameTags.Creatures.Species.BeetaSpecies, null);
		}
		return gameObject;
	}

	// Token: 0x0600037E RID: 894 RVA: 0x000AA768 File Offset: 0x000A8968
	public void OnPrefabInit(GameObject inst)
	{
		inst.GetComponent<OccupyArea>().objectLayers = new ObjectLayer[]
		{
			ObjectLayer.Building
		};
	}

	// Token: 0x0600037F RID: 895 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x0400022B RID: 555
	public const string ID = "BeeHive";

	// Token: 0x0400022C RID: 556
	public const string BASE_TRAIT_ID = "BeeHiveBaseTrait";

	// Token: 0x0400022D RID: 557
	private const int WIDTH = 2;

	// Token: 0x0400022E RID: 558
	private const int HEIGHT = 3;
}
