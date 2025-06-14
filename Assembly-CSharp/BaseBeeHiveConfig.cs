﻿using System;
using System.Collections.Generic;
using Klei.AI;
using STRINGS;
using TUNING;
using UnityEngine;

public class BaseBeeHiveConfig : IEntityConfig, IHasDlcRestrictions
{
	public string[] GetRequiredDlcIds()
	{
		return DlcManager.EXPANSION1;
	}

	public string[] GetForbiddenDlcIds()
	{
		return null;
	}

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

	public void OnPrefabInit(GameObject inst)
	{
		inst.GetComponent<OccupyArea>().objectLayers = new ObjectLayer[]
		{
			ObjectLayer.Building
		};
	}

	public void OnSpawn(GameObject inst)
	{
	}

	public const string ID = "BeeHive";

	public const string BASE_TRAIT_ID = "BeeHiveBaseTrait";

	private const int WIDTH = 2;

	private const int HEIGHT = 3;
}
