using System;
using System.Collections.Generic;
using STRINGS;

{

	public static readonly Tag Any = TagManager.Create("Any");

	public static readonly Tag SpawnsInWorld = TagManager.Create("SpawnsInWorld");

	public static readonly Tag Experimental = TagManager.Create("Experimental");











































































































































































	{
		GameTags.Suit,
		GameTags.JetSuit,
		GameTags.LeadSuit

	{
		GameTags.AtmoSuit,
		GameTags.LeadSuit
	};
	public static readonly Tag EquippableBalloon = TagManager.Create("EquippableBalloon");

	public static readonly Tag Clothes = TagManager.Create("Clothes");

	public static readonly Tag WarmVest = TagManager.Create("Warm_Vest");
	public static readonly Tag FunkyVest = TagManager.Create("Funky_Vest");

	public static readonly List<Tag> AllClothesTags = new List<Tag>
	{
		GameTags.Clothes,
		GameTags.FunkyVest
	};
	public static readonly Tag Assigned = TagManager.Create("Assigned");
	public static readonly Tag Helmet = TagManager.Create("Helmet");
	public static readonly Tag Equipped = TagManager.Create("Equipped");
	public static readonly Tag DisposablePortableBattery = TagManager.Create("DisposablePortableBattery");

	public static readonly Tag ChargedPortableBattery = TagManager.Create("ChargedPortableBattery");







































































































	{
		GameTags.Edible

	{
		GameTags.Medicine,
		GameTags.Seed,
		GameTags.Egg,
		GameTags.IndustrialIngredient,
		GameTags.IndustrialProduct,
		GameTags.HighEnergyParticle,
		GameTags.StoryTraitResource,
		GameTags.ChargedPortableBattery,
		GameTags.BionicUpgrade

	{
		GameTags.Special,
	};

	public static TagSet MaterialCategories = new TagSet
	{
		GameTags.Metal,
		GameTags.RefinedMetal,
		GameTags.BuildableRaw,
		GameTags.BuildableProcessed,
		GameTags.Filter,
		GameTags.Liquifiable,
		GameTags.Liquid,
		GameTags.Breathable,
		GameTags.Unbreathable,
		GameTags.ConsumableOre,
		GameTags.Sublimating,
		GameTags.Organics,
		GameTags.Farmable,
		GameTags.Agriculture,
		GameTags.Other,
		GameTags.ManufacturedMaterial,
		GameTags.CookingIngredient,
	};

	public static TagSet BionicCompatibleBatteries = new TagSet
	{
		"Electrobank",
		GameTags.EmptyPortableBattery
	};

	public static TagSet BionicIncompatibleBatteries = new TagSet
	{
		"SelfChargingElectrobank"
	};

	public static TagSet MaterialBuildingElements = new TagSet
	{
		GameTags.BuildingFiber,
		GameTags.BuildingWood
	};

	public static TagSet OtherEntityTags = new TagSet
	{
		GameTags.BagableCreature,
		GameTags.SwimmingCreature,
		GameTags.MiscPickupable
	};
	public static TagSet AllCategories = new TagSet(new TagSet[]
	{
		GameTags.CalorieCategories,
		GameTags.UnitCategories,
		GameTags.MaterialCategories,
		GameTags.MaterialBuildingElements,
	});

	public static TagSet DisplayAsCalories = new TagSet(GameTags.CalorieCategories);

	public static TagSet DisplayAsUnits = new TagSet(GameTags.UnitCategories);

	public static TagSet DisplayAsInformation = new TagSet();


	public static Tag StartingRefinedMetalOre = new Tag("StartingRefinedMetalOre");

	public static Tag[] StartingMetalOres;

	public static Tag[] StartingRefinedMetalOres = null;

	public static TagSet HiddenElementTags = new TagSet
	{
		GameTags.HideFromCodex,
		GameTags.HideFromSpawnTool,
		GameTags.StartingMetalOre,
		GameTags.StartingRefinedMetalOre

	{
	}

	public abstract class ChoreTypes
	{
		public static readonly Tag Farming = TagManager.Create("Farming");

		public static readonly Tag Ranching = TagManager.Create("Ranching");

		public static readonly Tag Research = TagManager.Create("Research");

		public static readonly Tag Power = TagManager.Create("Power");

		public static readonly Tag Building = TagManager.Create("Building");

		public static readonly Tag Cooking = TagManager.Create("Cooking");

		public static readonly Tag Fabricating = TagManager.Create("Fabricating");

		public static readonly Tag Wiring = TagManager.Create("Wiring");

		public static readonly Tag Art = TagManager.Create("Art");

		public static readonly Tag Digging = TagManager.Create("Digging");

		public static readonly Tag Doctoring = TagManager.Create("Doctoring");

		public static readonly Tag Conveyor = TagManager.Create("Conveyor");
	}

	public static class Creatures
	{
		public static readonly Tag ReservedByCreature = TagManager.Create("ReservedByCreature");














		public static readonly Tag WantsToEat = TagManager.Create("WantsToEat");
		public static readonly Tag Behaviour_TryToDrinkMilkFromFeeder = TagManager.Create("Behaviour_TryToDrinkMilkFromFeeder");
		public static readonly Tag Behaviour_InteractWithCritterCondo = TagManager.Create("Behaviour_InteractWithCritterCondo");
		public static readonly Tag WantsToGetRanched = TagManager.Create("WantsToGetRanched");
		public static readonly Tag WantsToGetCaptured = TagManager.Create("WantsToGetCaptured");
		public static readonly Tag WantsToClimbTree = TagManager.Create("WantsToClimbTree");
		public static readonly Tag WantsToPlantSeed = TagManager.Create("WantsToPlantSeed");
		public static readonly Tag WantsToForage = TagManager.Create("WantsToForage");
		public static readonly Tag WantsToLayEgg = TagManager.Create("WantsToLayEgg");
		public static readonly Tag WantsToTendEgg = TagManager.Create("WantsToTendEgg");
		public static readonly Tag WantsAHug = TagManager.Create("WantsAHug");
		public static readonly Tag WantsConduitConnection = TagManager.Create("WantsConduitConnection");
		public static readonly Tag WantsToGoHome = TagManager.Create("WantsToGoHome");
		public static readonly Tag WantsToMakeHome = TagManager.Create("WantsToMakeHome");
		public static readonly Tag BeeWantsToSleep = TagManager.Create("BeeWantsToSleep");
		public static readonly Tag WantsToTendCrops = TagManager.Create("WantsToTendPlants");
		public static readonly Tag WantsToStore = TagManager.Create("WantsToStore");
		public static readonly Tag WantsToBeckon = TagManager.Create("WantsToBeckon");
		public static readonly Tag Flee = TagManager.Create("Flee");
		public static readonly Tag Attack = TagManager.Create("Attack");
		public static readonly Tag Defend = TagManager.Create("Defend");
		public static readonly Tag ReturnToEgg = TagManager.Create("ReturnToEgg");
		public static readonly Tag CrabFriend = TagManager.Create("CrabFriend");
		public static readonly Tag Die = TagManager.Create("Die");
		public static readonly Tag Poop = TagManager.Create("Poop");
		public static readonly Tag MoveToLure = TagManager.Create("MoveToLure");
		public static readonly Tag Drowning = TagManager.Create("Drowning");
		public static readonly Tag Hungry = TagManager.Create("Hungry");
		public static readonly Tag Flyer = TagManager.Create("Flyer");
		public static readonly Tag FishTrapLure = TagManager.Create("FishTrapLure");
		public static readonly Tag FlyersLure = TagManager.Create("MasterLure");
		public static readonly Tag Walker = TagManager.Create("Walker");
		public static readonly Tag Hoverer = TagManager.Create("Hoverer");
		public static readonly Tag Swimmer = TagManager.Create("Swimmer");
		public static readonly Tag Fertile = TagManager.Create("Fertile");
		public static readonly Tag Submerged = TagManager.Create("Submerged");
		public static readonly Tag ExitSubmerged = TagManager.Create("ExitSubmerged");
		public static readonly Tag WantsToDropElements = TagManager.Create("WantsToDropElements");
		public static readonly Tag OriginallyWild = TagManager.Create("Wild");
		public static readonly Tag Wild = TagManager.Create("Wild");
		public static readonly Tag Overcrowded = TagManager.Create("Overcrowded");
		public static readonly Tag Expecting = TagManager.Create("Expecting");
		public static readonly Tag Confined = TagManager.Create("Confined");
		public static readonly Tag Digger = TagManager.Create("Digger");
		public static readonly Tag Tunnel = TagManager.Create("Tunnel");
		public static readonly Tag Builder = TagManager.Create("Builder");
		public static readonly Tag ScalesGrown = TagManager.Create("ScalesGrown");
		public static readonly Tag CanMolt = TagManager.Create("CanMolt");
		public static readonly Tag ReadyToMolt = TagManager.Create("ReadyToMolt");
		public static readonly Tag CantReachEgg = TagManager.Create("CantReachEgg");
		public static readonly Tag HasNoFoundation = TagManager.Create("HasNoFoundation");
		public static readonly Tag Cleaning = TagManager.Create("Cleaning");
		public static readonly Tag Happy = TagManager.Create("Happy");
		public static readonly Tag Unhappy = TagManager.Create("Unhappy");
		public static readonly Tag RequiresMilking = TagManager.Create("RequiresMilking");
		public static class Species
			public static readonly Tag HatchSpecies = TagManager.Create("HatchSpecies");
			public static readonly Tag LightBugSpecies = TagManager.Create("LightBugSpecies");
			public static readonly Tag OilFloaterSpecies = TagManager.Create("OilFloaterSpecies");
			public static readonly Tag DreckoSpecies = TagManager.Create("DreckoSpecies");
			public static readonly Tag GlomSpecies = TagManager.Create("GlomSpecies");
			public static readonly Tag PuftSpecies = TagManager.Create("PuftSpecies");
			public static readonly Tag PacuSpecies = TagManager.Create("PacuSpecies");
			public static readonly Tag MooSpecies = TagManager.Create("MooSpecies");
			public static readonly Tag MoleSpecies = TagManager.Create("MoleSpecies");
			public static readonly Tag SquirrelSpecies = TagManager.Create("SquirrelSpecies");
			public static readonly Tag CrabSpecies = TagManager.Create("CrabSpecies");
			public static readonly Tag StaterpillarSpecies = TagManager.Create("StaterpillarSpecies");
			public static readonly Tag BeetaSpecies = TagManager.Create("BeetaSpecies");
			public static readonly Tag DivergentSpecies = TagManager.Create("DivergentSpecies");
			public static readonly Tag DeerSpecies = TagManager.Create("DeerSpecies");
			public static readonly Tag BellySpecies = TagManager.Create("BellySpecies");
			public static readonly Tag SealSpecies = TagManager.Create("SealSpecies");

		{






			public static readonly Tag DisableCreature = TagManager.Create("DisableCreature");
		}
	}
	public static class StoragesIds
		public static readonly Tag DefaultStorage = TagManager.Create("Storage");
		public static readonly Tag BionicBatteryStorage = TagManager.Create("BionicBatteryStorage");
		public static readonly Tag BionicUpgradeStorage = TagManager.Create("BionicUpgradeStorage");
		public static readonly Tag BionicOxygenTankStorage = TagManager.Create("BionicOxygenTankStorage");

	{
		{
			{
				if (modelTag == GameTags.Minions.Models.Bionic)
					return DUPLICANTS.MODEL.BIONIC.NAME_TOOLTIP;
				}
			}

			public static readonly Tag Standard = TagManager.Create("Minion", DUPLICANTS.MODEL.STANDARD.NAME);

			public static readonly Tag Bionic = TagManager.Create("BionicMinion", DUPLICANTS.MODEL.BIONIC.NAME);

			public static readonly Tag[] AllModels = new Tag[]
			{
				GameTags.Minions.Models.Bionic
			};
	}

	public static class CodexCategories
	{
		public static string GetCategoryLabelText(Tag tag)
		{
			string text = "STRINGS.CODEX.CATEGORIES." + tag.ToString().ToUpper() + ".NAME";
			if (!Strings.TryGet(new StringKey(text), out entry))
				return ROOMS.CRITERIA.IN_CODE_ERROR.text.Replace("{0}", text);
			}
		}

		public static List<Tag> AllTags = new List<Tag>();

		public static Tag CreatureRelocator = GameTags.CodexCategories.AllTags.AddAndReturn(TagManager.Create("CreatureRelocator"));

	}

	public static class Robots
	{
		public static class Models
		{
			public static readonly Tag SweepBot = TagManager.Create("SweepBot");

			public static readonly Tag ScoutRover = TagManager.Create("ScoutRover");

			public static readonly Tag MorbRover = TagManager.Create("MorbRover");

			public static readonly Tag FetchDrone = TagManager.Create("FetchDrone");

			public static readonly Tag RemoteWorker = TagManager.Create("RemoteWorker");
		}

		{




			public static readonly Tag NoElectroBank = TagManager.Create("NoElectroBank");
	}

	public class Search
	{
		public static readonly Tag Tile = TagManager.Create("Tile");

		public static readonly Tag Ladder = TagManager.Create("Ladder");

		public static readonly Tag Powered = TagManager.Create("Powered");




		public static readonly Tag Cooking = TagManager.Create("Cooking");
	}
}
