﻿using System;
using System.Collections.Generic;
using Klei.AI;
using STRINGS;
using TUNING;
using UnityEngine;

public static class BaseHatchConfig
{
	public static GameObject BaseHatch(string id, string name, string desc, string anim_file, string traitId, bool is_baby, string symbolOverridePrefix = null)
	{
		float mass = 100f;
		EffectorValues tier = DECOR.BONUS.TIER0;
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, name, desc, mass, Assets.GetAnim(anim_file), "idle_loop", Grid.SceneLayer.Creatures, 1, 1, tier, default(EffectorValues), SimHashes.Creature, null, 293f);
		string navGridName = "WalkerNavGrid1x1";
		if (is_baby)
		{
			navGridName = "WalkerBabyNavGrid";
		}
		EntityTemplates.ExtendEntityToBasicCreature(gameObject, FactionManager.FactionID.Pest, traitId, navGridName, NavType.Floor, 32, 2f, "Meat", 2f, true, false, 283.15f, 313.15f, 228.15f, 373.15f);
		if (symbolOverridePrefix != null)
		{
			gameObject.AddOrGet<SymbolOverrideController>().ApplySymbolOverridesByAffix(Assets.GetAnim(anim_file), symbolOverridePrefix, null, 0);
		}
		Pickupable pickupable = gameObject.AddOrGet<Pickupable>();
		int sortOrder = TUNING.CREATURES.SORTING.CRITTER_ORDER["Hatch"];
		pickupable.sortOrder = sortOrder;
		gameObject.AddOrGet<Trappable>();
		gameObject.AddOrGetDef<CreatureFallMonitor.Def>();
		gameObject.AddOrGetDef<BurrowMonitor.Def>();
		gameObject.AddOrGetDef<WorldSpawnableMonitor.Def>().adjustSpawnLocationCb = new Func<int, int>(BaseHatchConfig.AdjustSpawnLocationCB);
		gameObject.AddOrGetDef<ThreatMonitor.Def>().fleethresholdState = Health.HealthState.Dead;
		gameObject.AddWeapon(1f, 1f, AttackProperties.DamageType.Standard, AttackProperties.TargetType.Single, 1, 0f);
		SoundEventVolumeCache.instance.AddVolume("hatch_kanim", "Hatch_voice_idle", NOISE_POLLUTION.CREATURES.TIER2);
		SoundEventVolumeCache.instance.AddVolume("FloorSoundEvent", "Hatch_footstep", NOISE_POLLUTION.CREATURES.TIER1);
		SoundEventVolumeCache.instance.AddVolume("hatch_kanim", "Hatch_land", NOISE_POLLUTION.CREATURES.TIER3);
		SoundEventVolumeCache.instance.AddVolume("hatch_kanim", "Hatch_chew", NOISE_POLLUTION.CREATURES.TIER3);
		SoundEventVolumeCache.instance.AddVolume("hatch_kanim", "Hatch_voice_hurt", NOISE_POLLUTION.CREATURES.TIER5);
		SoundEventVolumeCache.instance.AddVolume("hatch_kanim", "Hatch_voice_die", NOISE_POLLUTION.CREATURES.TIER5);
		SoundEventVolumeCache.instance.AddVolume("hatch_kanim", "Hatch_drill_emerge", NOISE_POLLUTION.CREATURES.TIER6);
		SoundEventVolumeCache.instance.AddVolume("hatch_kanim", "Hatch_drill_hide", NOISE_POLLUTION.CREATURES.TIER6);
		EntityTemplates.CreateAndRegisterBaggedCreature(gameObject, true, true, false);
		KPrefabID component = gameObject.GetComponent<KPrefabID>();
		component.AddTag(GameTags.Creatures.Walker, false);
		component.prefabInitFn += delegate(GameObject inst)
		{
			inst.GetAttributes().Add(Db.Get().Attributes.MaxUnderwaterTravelCost);
		};
		bool condition = !is_baby;
		ChoreTable.Builder chore_table = new ChoreTable.Builder().Add(new DeathStates.Def(), true, -1).Add(new AnimInterruptStates.Def(), true, -1).Add(new ExitBurrowStates.Def(), condition, -1).Add(new PlayAnimsStates.Def(GameTags.Creatures.Burrowed, true, "idle_mound", STRINGS.CREATURES.STATUSITEMS.BURROWED.NAME, STRINGS.CREATURES.STATUSITEMS.BURROWED.TOOLTIP), condition, -1).Add(new GrowUpStates.Def(), is_baby, -1).Add(new TrappedStates.Def(), true, -1).Add(new IncubatingStates.Def(), is_baby, -1).Add(new BaggedStates.Def(), true, -1).Add(new FallStates.Def(), true, -1).Add(new StunnedStates.Def(), true, -1).Add(new DrowningStates.Def(), true, -1).Add(new DebugGoToStates.Def(), true, -1).Add(new FleeStates.Def(), true, -1).Add(new AttackStates.Def("eat_pre", "eat_pst", null), condition, -1).PushInterruptGroup().Add(new CreatureSleepStates.Def(), true, -1).Add(new FixedCaptureStates.Def(), true, -1).Add(new RanchedStates.Def(), !is_baby, -1).Add(new PlayAnimsStates.Def(GameTags.Creatures.WantsToEnterBurrow, false, "hide", STRINGS.CREATURES.STATUSITEMS.BURROWING.NAME, STRINGS.CREATURES.STATUSITEMS.BURROWING.TOOLTIP), condition, -1).Add(new LayEggStates.Def(), !is_baby, -1).Add(new EatStates.Def(), true, -1).Add(new DrinkMilkStates.Def
		{
			shouldBeBehindMilkTank = is_baby
		}, true, -1).Add(new PlayAnimsStates.Def(GameTags.Creatures.Poop, false, "poop", STRINGS.CREATURES.STATUSITEMS.EXPELLING_SOLID.NAME, STRINGS.CREATURES.STATUSITEMS.EXPELLING_SOLID.TOOLTIP), true, -1).Add(new CallAdultStates.Def(), is_baby, -1).Add(new CritterCondoStates.Def(), !is_baby, -1).PopInterruptGroup().Add(new IdleStates.Def(), true, -1);
		EntityTemplates.AddCreatureBrain(gameObject, chore_table, GameTags.Creatures.Species.HatchSpecies, symbolOverridePrefix);
		return gameObject;
	}

	public static List<Diet.Info> BasicRockDiet(Tag poopTag, float caloriesPerKg, float producedConversionRate, string diseaseId, float diseasePerKgProduced)
	{
		HashSet<Tag> hashSet = new HashSet<Tag>();
		hashSet.Add(SimHashes.Sand.CreateTag());
		hashSet.Add(SimHashes.SandStone.CreateTag());
		hashSet.Add(SimHashes.Clay.CreateTag());
		hashSet.Add(SimHashes.CrushedRock.CreateTag());
		hashSet.Add(SimHashes.Dirt.CreateTag());
		hashSet.Add(SimHashes.SedimentaryRock.CreateTag());
		hashSet.Add(SimHashes.Shale.CreateTag());
		return new List<Diet.Info>
		{
			new Diet.Info(hashSet, poopTag, caloriesPerKg, producedConversionRate, diseaseId, diseasePerKgProduced, false, Diet.Info.FoodType.EatSolid, false, null)
		};
	}

	public static List<Diet.Info> HardRockDiet(Tag poopTag, float caloriesPerKg, float producedConversionRate, string diseaseId, float diseasePerKgProduced)
	{
		HashSet<Tag> hashSet = new HashSet<Tag>();
		hashSet.Add(SimHashes.SedimentaryRock.CreateTag());
		hashSet.Add(SimHashes.IgneousRock.CreateTag());
		hashSet.Add(SimHashes.Obsidian.CreateTag());
		hashSet.Add(SimHashes.Granite.CreateTag());
		return new List<Diet.Info>
		{
			new Diet.Info(hashSet, poopTag, caloriesPerKg, producedConversionRate, diseaseId, diseasePerKgProduced, false, Diet.Info.FoodType.EatSolid, false, null)
		};
	}

	public static List<Diet.Info> MetalDiet(Tag poopTag, float caloriesPerKg, float producedConversionRate, string diseaseId, float diseasePerKgProduced)
	{
		List<Diet.Info> list = new List<Diet.Info>();
		foreach (Tag tag in GameTags.BasicMetalOres)
		{
			Tag produced_element = (poopTag == GameTags.Metal) ? ElementLoader.FindElementByTag(tag).refinedMetalTarget.CreateTag() : poopTag;
			list.Add(new Diet.Info(new HashSet<Tag>(new Tag[]
			{
				tag
			}), produced_element, caloriesPerKg, producedConversionRate, diseaseId, diseasePerKgProduced, false, Diet.Info.FoodType.EatSolid, false, null));
		}
		list.Add(new Diet.Info(new HashSet<Tag>(new Tag[]
		{
			SimHashes.GoldAmalgam.CreateTag()
		}), (poopTag == GameTags.Metal) ? SimHashes.Gold.CreateTag() : poopTag, caloriesPerKg, producedConversionRate, diseaseId, diseasePerKgProduced, false, Diet.Info.FoodType.EatSolid, false, null));
		list.Add(new Diet.Info(new HashSet<Tag>(new Tag[]
		{
			SimHashes.Wolframite.CreateTag()
		}), (poopTag == GameTags.Metal) ? SimHashes.Tungsten.CreateTag() : poopTag, caloriesPerKg, producedConversionRate, diseaseId, diseasePerKgProduced, false, Diet.Info.FoodType.EatSolid, false, null));
		return list;
	}

	public static List<Diet.Info> VeggieDiet(Tag poopTag, float caloriesPerKg, float producedConversionRate, string diseaseId, float diseasePerKgProduced)
	{
		HashSet<Tag> hashSet = new HashSet<Tag>();
		hashSet.Add(SimHashes.Dirt.CreateTag());
		hashSet.Add(SimHashes.SlimeMold.CreateTag());
		hashSet.Add(SimHashes.Algae.CreateTag());
		hashSet.Add(SimHashes.Fertilizer.CreateTag());
		hashSet.Add(SimHashes.ToxicSand.CreateTag());
		return new List<Diet.Info>
		{
			new Diet.Info(hashSet, poopTag, caloriesPerKg, producedConversionRate, diseaseId, diseasePerKgProduced, false, Diet.Info.FoodType.EatSolid, false, null)
		};
	}

	public static List<Diet.Info> FoodDiet(Tag poopTag, float caloriesPerKg, float producedConversionRate, string diseaseId, float diseasePerKgProduced)
	{
		List<Diet.Info> list = new List<Diet.Info>();
		foreach (EdiblesManager.FoodInfo foodInfo in EdiblesManager.GetAllLoadedFoodTypes())
		{
			if (foodInfo.CaloriesPerUnit > 0f)
			{
				list.Add(new Diet.Info(new HashSet<Tag>
				{
					new Tag(foodInfo.Id)
				}, poopTag, foodInfo.CaloriesPerUnit, producedConversionRate, diseaseId, diseasePerKgProduced, false, Diet.Info.FoodType.EatSolid, false, null));
			}
		}
		return list;
	}

	public static GameObject SetupDiet(GameObject prefab, List<Diet.Info> diet_infos, float referenceCaloriesPerKg, float minPoopSizeInKg)
	{
		Diet diet = new Diet(diet_infos.ToArray());
		CreatureCalorieMonitor.Def def = prefab.AddOrGetDef<CreatureCalorieMonitor.Def>();
		def.diet = diet;
		def.minConsumedCaloriesBeforePooping = referenceCaloriesPerKg * minPoopSizeInKg;
		prefab.AddOrGetDef<SolidConsumerMonitor.Def>().diet = diet;
		return prefab;
	}

	private static int AdjustSpawnLocationCB(int cell)
	{
		while (!Grid.Solid[cell])
		{
			int num = Grid.CellBelow(cell);
			if (!Grid.IsValidCell(cell))
			{
				break;
			}
			cell = num;
		}
		return cell;
	}
}
