﻿using System;
using Klei.AI;
using TUNING;
using UnityEngine;

public static class BaseMosquitoConfig
{
	public static GameObject BaseMosquito(string id, string name, string desc, string anim_file, string traitId, string symbol_override_prefix, bool isBaby, float warningLowTemperature, float warningHighTemperature, float lethalLowTemperature, float lethalHighTemperature, string poke_anim_pre, string poke_anim_loop, string poke_anim_pst, string goingToPokeStatusItemSTRAddress, string pokingStatusItemSTRAddress)
	{
		float mass = 5f;
		EffectorValues tier = DECOR.PENALTY.TIER0;
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, name, desc, mass, Assets.GetAnim(anim_file), "idle_loop", Grid.SceneLayer.Creatures, 1, 1, tier, default(EffectorValues), SimHashes.Creature, null, 293f);
		EntityTemplates.ExtendEntityToBasicCreature(gameObject, FactionManager.FactionID.Prey, traitId, isBaby ? "SwimmerNavGrid" : "FlyerNavGrid1x1", isBaby ? NavType.Swim : NavType.Hover, 32, 2f, null, 0f, !isBaby, true, warningLowTemperature, warningHighTemperature, lethalLowTemperature, lethalHighTemperature);
		if (!string.IsNullOrEmpty(symbol_override_prefix))
		{
			gameObject.AddOrGet<SymbolOverrideController>().ApplySymbolOverridesByAffix(Assets.GetAnim(anim_file), symbol_override_prefix, null, 0);
		}
		Pickupable pickupable = gameObject.AddOrGet<Pickupable>();
		int sortOrder = CREATURES.SORTING.CRITTER_ORDER["Mosquito"];
		pickupable.sortOrder = sortOrder;
		KPrefabID component = gameObject.GetComponent<KPrefabID>();
		gameObject.AddOrGet<LoopingSounds>();
		gameObject.AddOrGet<Trappable>();
		LureableMonitor.Def def = gameObject.AddOrGetDef<LureableMonitor.Def>();
		Tag[] lures;
		if (!isBaby)
		{
			(lures = new Tag[1])[0] = GameTags.Creatures.FlyersLure;
		}
		else
		{
			(lures = new Tag[1])[0] = GameTags.Creatures.FishTrapLure;
		}
		def.lures = lures;
		gameObject.AddOrGetDef<ThreatMonitor.Def>();
		if (!isBaby)
		{
			component.AddTag(GameTags.Creatures.Flyer, false);
			gameObject.AddOrGetDef<SubmergedMonitor.Def>();
			gameObject.AddOrGet<PokeMonitor>();
			gameObject.AddOrGetDef<OvercrowdingMonitor.Def>().spaceRequiredPerCreature = CREATURES.SPACE_REQUIREMENTS.TIER1;
			component.prefabInitFn += delegate(GameObject inst)
			{
				inst.GetAttributes().Add(Db.Get().Attributes.MaxUnderwaterTravelCost);
			};
		}
		else
		{
			component.AddTag(GameTags.SwimmingCreature, false);
			component.AddTag(GameTags.Creatures.Swimmer, false);
			gameObject.AddComponent<Movable>();
		}
		EntityTemplates.CreateAndRegisterBaggedCreature(gameObject, !isBaby, !isBaby, isBaby);
		ChoreTable.Builder chore_table = new ChoreTable.Builder().Add(new DeathStates.Def(), true, -1).Add(new AnimInterruptStates.Def(), true, -1).Add(new GrowUpStates.Def(), isBaby, -1).Add(new TrappedStates.Def(), true, -1).Add(new IncubatingStates.Def(), isBaby, -1).Add(new BaggedStates.Def(), true, -1).Add(new FallStates.Def
		{
			getLandAnim = new Func<FallStates.Instance, string>(BaseMosquitoConfig.GetLandAnim)
		}, isBaby, -1).Add(new StunnedStates.Def(), true, -1).Add(new DebugGoToStates.Def(), true, -1).Add(new FlopStates.Def(), isBaby, -1).Add(new DrowningStates.Def(), !isBaby, -1).PushInterruptGroup().Add(new CreatureSleepStates.Def(), true, -1).Add(new FixedCaptureStates.Def(), true, -1).Add(new UpTopPoopStates.Def(), true, -1).Add(new LayEggStates.Def(), !isBaby, -1).Add(new AliveEntityPoker.Def
		{
			PokeAnim_Pre = poke_anim_pre,
			PokeAnim_Loop = poke_anim_loop,
			PokeAnim_Pst = poke_anim_pst,
			statusItemSTR_goingToPoke = goingToPokeStatusItemSTRAddress,
			statusItemSTR_poking = pokingStatusItemSTRAddress
		}, !isBaby, -1).Add(new MoveToLureStates.Def(), true, -1).PopInterruptGroup().Add(new IdleStates.Def(), true, -1);
		CreatureFallMonitor.Def def2 = gameObject.AddOrGetDef<CreatureFallMonitor.Def>();
		def2.canSwim = isBaby;
		def2.checkHead = !isBaby;
		gameObject.AddOrGetDef<FixedCapturableMonitor.Def>();
		if (isBaby)
		{
			gameObject.AddOrGetDef<FlopMonitor.Def>();
			gameObject.AddOrGetDef<FishOvercrowdingMonitor.Def>();
			gameObject.AddOrGetDef<AquaticCreatureSuffocationMonitor.Def>();
		}
		gameObject.AddOrGet<LoopingSounds>();
		EntityTemplates.AddCreatureBrain(gameObject, chore_table, GameTags.Creatures.Species.MosquitoSpecies, symbol_override_prefix);
		return gameObject;
	}

	private static string GetLandAnim(FallStates.Instance smi)
	{
		if (smi.GetSMI<CreatureFallMonitor.Instance>().CanSwimAtCurrentLocation())
		{
			return "idle_loop";
		}
		return "flop_loop";
	}
}
