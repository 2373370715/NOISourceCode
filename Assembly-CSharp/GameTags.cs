﻿using System;
using System.Collections.Generic;
using System.Reflection;
using STRINGS;

public class GameTags
{
	public static Tag[] Reflection_GetTagsInClass(Type classAddress, BindingFlags variableFlags = BindingFlags.Static | BindingFlags.Public)
	{
		List<FieldInfo> list = new List<FieldInfo>(classAddress.GetFields(variableFlags)).FindAll((FieldInfo f) => f.FieldType == typeof(Tag));
		Tag[] array = new Tag[list.Count];
		for (int i = 0; i < array.Length; i++)
		{
			array[i] = list[i].Name;
		}
		return array;
	}

	public static readonly Tag DeprecatedContent = TagManager.Create("DeprecatedContent");

	public static readonly Tag Any = TagManager.Create("Any");

	public static readonly Tag SpawnsInWorld = TagManager.Create("SpawnsInWorld");

	public static readonly Tag Experimental = TagManager.Create("Experimental");

	public static readonly Tag Gravitas = TagManager.Create("Gravitas");

	public static readonly Tag Miscellaneous = TagManager.Create("Miscellaneous");

	public static readonly Tag Specimen = TagManager.Create("Specimen");

	public static readonly Tag Seed = TagManager.Create("Seed");

	public static readonly Tag Dehydrated = TagManager.Create("Dehydrated");

	public static readonly Tag Rehydrated = TagManager.Create("Rehydrated");

	public static readonly Tag Edible = TagManager.Create("Edible");

	public static readonly Tag CookingIngredient = TagManager.Create("CookingIngredient");

	public static readonly Tag Medicine = TagManager.Create("Medicine");

	public static readonly Tag MedicalSupplies = TagManager.Create("MedicalSupplies");

	public static readonly Tag Plant = TagManager.Create("Plant");

	public static readonly Tag PlantBranch = TagManager.Create("PlantBranch");

	public static readonly Tag GrowingPlant = TagManager.Create("GrowingPlant");

	public static readonly Tag FullyGrown = TagManager.Create("FullyGrown");

	public static readonly Tag PlantedOnFloorVessel = TagManager.Create("PlantedOnFloorVessel");

	public static readonly Tag Pickupable = TagManager.Create("Pickupable");

	public static readonly Tag Liquifiable = TagManager.Create("Liquifiable");

	public static readonly Tag IceOre = TagManager.Create("IceOre");

	public static readonly Tag OxyRock = TagManager.Create("OxyRock");

	public static readonly Tag Life = TagManager.Create("Life");

	public static readonly Tag Fertilizer = TagManager.Create("Fertilizer");

	public static readonly Tag Farmable = TagManager.Create("Farmable");

	public static readonly Tag Agriculture = TagManager.Create("Agriculture");

	public static readonly Tag Organics = TagManager.Create("Organics");

	public static readonly Tag IndustrialProduct = TagManager.Create("IndustrialProduct");

	public static readonly Tag IndustrialIngredient = TagManager.Create("IndustrialIngredient");

	public static readonly Tag Other = TagManager.Create("Other");

	public static readonly Tag ManufacturedMaterial = TagManager.Create("ManufacturedMaterial");

	public static readonly Tag Plastic = TagManager.Create("Plastic");

	public static readonly Tag Steel = TagManager.Create("Steel");

	public static readonly Tag BuildableAny = TagManager.Create("BuildableAny");

	public static readonly Tag Decoration = TagManager.Create("Decoration");

	public static readonly Tag Window = TagManager.Create("Window");

	public static readonly Tag Bunker = TagManager.Create("Bunker");

	public static readonly Tag Transition = TagManager.Create("Transition");

	public static readonly Tag Detecting = TagManager.Create("Detecting");

	public static readonly Tag RareMaterials = TagManager.Create("RareMaterials");

	public static readonly Tag BuildingFiber = TagManager.Create("BuildingFiber");

	public static readonly Tag Transparent = TagManager.Create("Transparent");

	public static readonly Tag Insulator = TagManager.Create("Insulator");

	public static readonly Tag Plumbable = TagManager.Create("Plumbable");

	public static readonly Tag BuildingWood = TagManager.Create("BuildingWood");

	public static readonly Tag PreciousRock = TagManager.Create("PreciousRock");

	public static readonly Tag Artifact = TagManager.Create("Artifact");

	public static readonly Tag BionicUpgrade = TagManager.Create("BionicUpgrade");

	public static readonly Tag BionicBedTime = TagManager.Create("BionicBedTime");

	public static readonly Tag CharmedArtifact = TagManager.Create("CharmedArtifact");

	public static readonly Tag TerrestrialArtifact = TagManager.Create("TerrestrialArtifact");

	public static readonly Tag Keepsake = TagManager.Create("Keepsake");

	public static readonly Tag MiscPickupable = TagManager.Create("MiscPickupable");

	public static readonly Tag PlastifiableLiquid = TagManager.Create("PlastifiableLiquid");

	public static readonly Tag CombustibleGas = TagManager.Create("CombustibleGas");

	public static readonly Tag CombustibleLiquid = TagManager.Create("CombustibleLiquid");

	public static readonly Tag CombustibleSolid = TagManager.Create("CombustibleSolid");

	public static readonly Tag FlyingCritterEdible = TagManager.Create("FlyingCritterEdible");

	public static readonly Tag Comet = TagManager.Create("Comet");

	public static readonly Tag DeadReactor = TagManager.Create("DeadReactor");

	public static readonly Tag Robot = TagManager.Create("Robot");

	public static readonly Tag StoryTraitResource = TagManager.Create("StoryTraitResource");

	public static readonly Tag RoomProberBuilding = TagManager.Create("RoomProberBuilding");

	public static readonly Tag DevBuilding = TagManager.Create("DevBuilding");

	public static readonly Tag MarkedForMove = TagManager.Create("MarkedForMove");

	public static readonly Tag HideHealthBar = TagManager.Create("HideHealthBar");

	public static readonly Tag Incapacitated = TagManager.Create("Incapacitated");

	public static readonly Tag CaloriesDepleted = TagManager.Create("CaloriesDepleted");

	public static readonly Tag HitPointsDepleted = TagManager.Create("HitPointsDepleted");

	public static readonly Tag RadiationSicknessIncapacitation = TagManager.Create("RadiationSickness");

	public static readonly Tag Wilting = TagManager.Create("Wilting");

	public static readonly Tag Blighted = TagManager.Create("Blighted");

	public static readonly Tag PreventEmittingDisease = TagManager.Create("EmittingDisease");

	public static readonly Tag Creature = TagManager.Create("Creature");

	public static readonly Tag OriginalCreature = TagManager.Create("OriginalCreature");

	public static readonly Tag Hexaped = TagManager.Create("Hexaped");

	public static readonly Tag HeatBulb = TagManager.Create("HeatBulb");

	public static readonly Tag Egg = TagManager.Create("Egg");

	public static readonly Tag IncubatableEgg = TagManager.Create("IncubatableEgg");

	public static readonly Tag Trapped = TagManager.Create("Trapped");

	public static readonly Tag BagableCreature = TagManager.Create("BagableCreature");

	public static readonly Tag SwimmingCreature = TagManager.Create("SwimmingCreature");

	public static readonly Tag Spawner = TagManager.Create("Spawner");

	public static readonly Tag FullyIncubated = TagManager.Create("FullyIncubated");

	public static readonly Tag Amphibious = TagManager.Create("Amphibious");

	public static readonly Tag LargeCreature = TagManager.Create("LargeCreature");

	public static readonly Tag MoltShell = TagManager.Create("MoltShell");

	public static readonly Tag BaseMinion = TagManager.Create("BaseMinion");

	public static readonly Tag Corpse = TagManager.Create("Corpse");

	public static readonly Tag Alloy = TagManager.Create("Alloy");

	public static readonly Tag Metal = TagManager.Create("Metal");

	public static readonly Tag RefinedMetal = TagManager.Create("RefinedMetal");

	public static readonly Tag PreciousMetal = TagManager.Create("PreciousMetal");

	public static readonly Tag StoredMetal = TagManager.Create("StoredMetal");

	public static readonly Tag Solid = TagManager.Create("Solid");

	public static readonly Tag Liquid = TagManager.Create("Liquid");

	public static readonly Tag LiquidSource = TagManager.Create("LiquidSource");

	public static readonly Tag GasSource = TagManager.Create("GasSource");

	public static readonly Tag Water = TagManager.Create("Water");

	public static readonly Tag DirtyWater = TagManager.Create("DirtyWater");

	public static readonly Tag AnyWater = TagManager.Create("AnyWater");

	public static readonly Tag LubricatingOil = TagManager.Create("LubricatingOil");

	public static readonly Tag Algae = TagManager.Create("Algae");

	public static readonly Tag Void = TagManager.Create("Void");

	public static readonly Tag Chlorine = TagManager.Create("Chlorine");

	public static readonly Tag Oxygen = TagManager.Create("Oxygen");

	public static readonly Tag Hydrogen = TagManager.Create("Hydrogen");

	public static readonly Tag Methane = TagManager.Create("Methane");

	public static readonly Tag CarbonDioxide = TagManager.Create("CarbonDioxide");

	public static readonly Tag Carbon = TagManager.Create("Carbon");

	public static readonly Tag BuildableRaw = TagManager.Create("BuildableRaw");

	public static readonly Tag BuildableProcessed = TagManager.Create("BuildableProcessed");

	public static readonly Tag Phosphorus = TagManager.Create("Phosphorus");

	public static readonly Tag Phosphorite = TagManager.Create("Phosphorite");

	public static readonly Tag SlimeMold = TagManager.Create("SlimeMold");

	public static readonly Tag Filler = TagManager.Create("Filler");

	public static readonly Tag Item = TagManager.Create("Item");

	public static readonly Tag Ore = TagManager.Create("Ore");

	public static readonly Tag GenericOre = TagManager.Create("GenericOre");

	public static readonly Tag Ingot = TagManager.Create("Ingot");

	public static readonly Tag Dirt = TagManager.Create("Dirt");

	public static readonly Tag Filter = TagManager.Create("Filter");

	public static readonly Tag ConsumableOre = TagManager.Create("ConsumableOre");

	public static readonly Tag Unstable = TagManager.Create("Unstable");

	public static readonly Tag Slippery = TagManager.Create("Slippery");

	public static readonly Tag Sublimating = TagManager.Create("Sublimating");

	public static readonly Tag HideFromSpawnTool = TagManager.Create("HideFromSpawnTool");

	public static readonly Tag HideFromCodex = TagManager.Create("HideFromCodex");

	public static readonly Tag EmitsLight = TagManager.Create("EmitsLight");

	public static readonly Tag Special = TagManager.Create("Special");

	public static readonly Tag Breathable = TagManager.Create("Breathable");

	public static readonly Tag Unbreathable = TagManager.Create("Unbreathable");

	public static readonly Tag Gas = TagManager.Create("Gas");

	public static readonly Tag Crushable = TagManager.Create("Crushable");

	public static readonly Tag Noncrushable = TagManager.Create("Noncrushable");

	public static readonly Tag IronOre = TagManager.Create("IronOre");

	public static readonly Tag HighEnergyParticle = TagManager.Create("HighEnergyParticle");

	public static readonly Tag IgnoreMaterialCategory = TagManager.Create("IgnoreMaterialCategory");

	public static readonly Tag Oxidizer = TagManager.Create("Oxidizer");

	public static readonly Tag UnrefinedOil = TagManager.Create("UnrefinedOil");

	public static readonly Tag RiverSource = TagManager.Create("RiverSource");

	public static readonly Tag RiverSink = TagManager.Create("RiverSink");

	public static readonly Tag Garbage = TagManager.Create("Garbage");

	public static readonly Tag OilWell = TagManager.Create("OilWell");

	public static readonly Tag Glass = TagManager.Create("Glass");

	public static readonly Tag Door = TagManager.Create("Door");

	public static readonly Tag Farm = TagManager.Create("Farm");

	public static readonly Tag StorageLocker = TagManager.Create("StorageLocker");

	public static readonly Tag LadderBed = TagManager.Create("LadderBed");

	public static readonly Tag FloorTiles = TagManager.Create("FloorTiles");

	public static readonly Tag Carpeted = TagManager.Create("Carpeted");

	public static readonly Tag FarmTiles = TagManager.Create("FarmTiles");

	public static readonly Tag Ladders = TagManager.Create("Ladders");

	public static readonly Tag NavTeleporters = TagManager.Create("NavTeleporters");

	public static readonly Tag Wires = TagManager.Create("Wires");

	public static readonly Tag Vents = TagManager.Create("Vents");

	public static readonly Tag Pipes = TagManager.Create("Pipes");

	public static readonly Tag WireBridges = TagManager.Create("WireBridges");

	public static readonly Tag TravelTubeBridges = TagManager.Create("TravelTubeBridges");

	public static readonly Tag Backwall = TagManager.Create("Backwall");

	public static readonly Tag MISSING_TAG = TagManager.Create("MISSING_TAG");

	public static readonly Tag PlantRenderer = TagManager.Create("PlantRenderer");

	public static readonly Tag Usable = TagManager.Create("Usable");

	public static readonly Tag PedestalDisplayable = TagManager.Create("PedestalDisplayable");

	public static readonly Tag HasChores = TagManager.Create("HasChores");

	public static readonly Tag Suit = TagManager.Create("Suit");

	public static readonly Tag AirtightSuit = TagManager.Create("AirtightSuit");

	public static readonly Tag AtmoSuit = TagManager.Create("Atmo_Suit");

	public static readonly Tag OxygenMask = TagManager.Create("Oxygen_Mask");

	public static readonly Tag LeadSuit = TagManager.Create("Lead_Suit");

	public static readonly Tag JetSuit = TagManager.Create("Jet_Suit");

	public static readonly Tag JetSuitOutOfFuel = TagManager.Create("JetSuitOutOfFuel");

	public static readonly Tag SuitBatteryLow = TagManager.Create("SuitBatteryLow");

	public static readonly Tag SuitBatteryOut = TagManager.Create("SuitBatteryOut");

	public static readonly List<Tag> AllSuitTags = new List<Tag>
	{
		GameTags.Suit,
		GameTags.AtmoSuit,
		GameTags.JetSuit,
		GameTags.LeadSuit
	};

	public static readonly List<Tag> OxygenSuitTags = new List<Tag>
	{
		GameTags.AtmoSuit,
		GameTags.JetSuit,
		GameTags.LeadSuit
	};

	public static readonly Tag EquippableBalloon = TagManager.Create("EquippableBalloon");

	public static readonly Tag Clothes = TagManager.Create("Clothes");

	public static readonly Tag WarmVest = TagManager.Create("Warm_Vest");

	public static readonly Tag FunkyVest = TagManager.Create("Funky_Vest");

	public static readonly List<Tag> AllClothesTags = new List<Tag>
	{
		GameTags.Clothes,
		GameTags.WarmVest,
		GameTags.FunkyVest
	};

	public static readonly Tag Assigned = TagManager.Create("Assigned");

	public static readonly Tag Helmet = TagManager.Create("Helmet");

	public static readonly Tag Equipped = TagManager.Create("Equipped");

	public static readonly Tag DisposablePortableBattery = TagManager.Create("DisposablePortableBattery");

	public static readonly Tag ChargedPortableBattery = TagManager.Create("ChargedPortableBattery");

	public static readonly Tag EmptyPortableBattery = TagManager.Create("EmptyPortableBattery");

	public static readonly Tag SolidLubricant = TagManager.Create("SolidLubricant");

	public static readonly Tag Entombed = TagManager.Create("Entombed");

	public static readonly Tag Uprooted = TagManager.Create("Uprooted");

	public static readonly Tag Preserved = TagManager.Create("Preserved");

	public static readonly Tag Compostable = TagManager.Create("Compostable");

	public static readonly Tag Pickled = TagManager.Create("Pickled");

	public static readonly Tag UnspicedFood = TagManager.Create("UnspicedFood");

	public static readonly Tag SpicedFood = TagManager.Create("SpicedFood");

	public static readonly Tag Dying = TagManager.Create("Dying");

	public static readonly Tag Dead = TagManager.Create("Dead");

	public static readonly Tag PreventDeadAnimation = TagManager.Create("PreventDeadAnimation");

	public static readonly Tag Reachable = TagManager.Create("Reachable");

	public static readonly Tag PreventChoreInterruption = TagManager.Create("PreventChoreInterruption");

	public static readonly Tag PerformingWorkRequest = TagManager.Create("PerformingWorkRequest");

	public static readonly Tag RecoveringBreath = TagManager.Create("RecoveringBreath");

	public static readonly Tag FeelingCold = TagManager.Create("FeelingCold");

	public static readonly Tag FeelingWarm = TagManager.Create("FeelingWarm");

	public static readonly Tag RecoveringWarmnth = TagManager.Create("RecoveringWarmnth");

	public static readonly Tag RecoveringFromHeat = TagManager.Create("RecoveringFromHeat");

	public static readonly Tag NoOxygen = TagManager.Create("NoOxygen");

	public static readonly Tag Idle = TagManager.Create("Idle");

	public static readonly Tag StationaryIdling = TagManager.Create("StationaryIdling");

	public static readonly Tag AlwaysConverse = TagManager.Create("AlwaysConverse");

	public static readonly Tag HasDebugDestination = TagManager.Create("HasDebugDestination");

	public static readonly Tag Shaded = TagManager.Create("Shaded");

	public static readonly Tag TakingMedicine = TagManager.Create("TakingMedicine");

	public static readonly Tag Partying = TagManager.Create("Partying");

	public static readonly Tag MakingMess = TagManager.Create("MakingMess");

	public static readonly Tag DupeBrain = TagManager.Create("DupeBrain");

	public static readonly Tag CreatureBrain = TagManager.Create("CreatureBrain");

	public static readonly Tag Asleep = TagManager.Create("Asleep");

	public static readonly Tag HoldingBreath = TagManager.Create("HoldingBreath");

	public static readonly Tag Overjoyed = TagManager.Create("Overjoyed");

	public static readonly Tag PleasantConversation = TagManager.Create("PleasantConversation");

	public static readonly Tag HasSuitTank = TagManager.Create("HasSuitTank");

	public static readonly Tag HasAirtightSuit = TagManager.Create("HasAirtightSuit");

	public static readonly Tag NoCreatureIdling = TagManager.Create("NoCreatureIdling");

	public static readonly Tag UnderConstruction = TagManager.Create("UnderConstruction");

	public static readonly Tag Operational = TagManager.Create("Operational");

	public static readonly Tag JetSuitBlocker = TagManager.Create("JetSuitBlocker");

	public static readonly Tag HasInvalidPorts = TagManager.Create("HasInvalidPorts");

	public static readonly Tag NotRoomAssignable = TagManager.Create("NotRoomAssignable");

	public static readonly Tag OneTimeUseLure = TagManager.Create("OneTimeUseLure");

	public static readonly Tag LureUsed = TagManager.Create("LureUsed");

	public static readonly Tag TemplateBuilding = TagManager.Create("TemplateBuilding");

	public static readonly Tag ModularConduitPort = TagManager.Create("ModularConduitPort");

	public static readonly Tag WarpTech = TagManager.Create("WarpTech");

	public static readonly Tag HEPPassThrough = TagManager.Create("HEPPassThrough");

	public static readonly Tag TelephoneRinging = TagManager.Create("TelephoneRinging");

	public static readonly Tag LongDistanceCall = TagManager.Create("LongDistanceCall");

	public static readonly Tag Telepad = TagManager.Create("Telepad");

	public static readonly Tag InTransitTube = TagManager.Create("InTransitTube");

	public static readonly Tag TrapArmed = TagManager.Create("TrapArmed");

	public static readonly Tag GeyserFeature = TagManager.Create("GeyserFeature");

	public static readonly Tag Rocket = TagManager.Create("Rocket");

	public static readonly Tag RocketOnGround = TagManager.Create("RocketOnGround");

	public static readonly Tag RocketNotOnGround = TagManager.Create("RocketNotOnGround");

	public static readonly Tag RocketInSpace = TagManager.Create("RocketInSpace");

	public static readonly Tag RocketStranded = TagManager.Create("RocketStranded");

	public static readonly Tag RailGunPayloadEmptyable = TagManager.Create("RailGunPayloadEmptyable");

	public static readonly Tag TransferringCargoComplete = TagManager.Create("TransferringCargoComplete");

	public static readonly Tag NoseRocketModule = TagManager.Create("NoseRocketModule");

	public static readonly Tag LaunchButtonRocketModule = TagManager.Create("LaunchButtonRocketModule");

	public static readonly Tag RocketInteriorBuilding = TagManager.Create("RocketInteriorBuilding");

	public static readonly Tag NotRocketInteriorBuilding = TagManager.Create("NotRocketInteriorBuilding");

	public static readonly Tag UniquePerWorld = TagManager.Create("UniquePerWorld");

	public static readonly Tag RocketEnvelopeTile = TagManager.Create("RocketEnvelopeTile");

	public static readonly Tag NoRocketRefund = TagManager.Create("NoRocketRefund");

	public static readonly Tag RocketModule = TagManager.Create("RocketModule");

	public static readonly Tag GantryExtended = TagManager.Create("GantryExtended");

	public static readonly Tag POIHarvesting = TagManager.Create("POIHarvesting");

	public static readonly Tag BallisticEntityLanding = TagManager.Create("BallisticEntityLanding");

	public static readonly Tag BallisticEntityLaunching = TagManager.Create("BallisticEntityLaunching");

	public static readonly Tag BallisticEntityMoving = TagManager.Create("BallisticEntityMoving");

	public static readonly Tag ClusterEntityGrounded = TagManager.Create("ClusterEntityGrounded ");

	public static readonly Tag LongRangeMissileMoving = TagManager.Create("LongRangeMissileMoving");

	public static readonly Tag LongRangeMissileIdle = TagManager.Create("LongRangeMissileIdle");

	public static readonly Tag LongRangeMissileExploding = TagManager.Create("LongRangeMissileExploding");

	public static readonly Tag EntityInSpace = TagManager.Create("EntityInSpace");

	public static readonly Tag Monument = TagManager.Create("Monument");

	public static readonly Tag Stored = TagManager.Create("Stored");

	public static readonly Tag StoredPrivate = TagManager.Create("StoredPrivate");

	public static readonly Tag Sealed = TagManager.Create("Sealed");

	public static readonly Tag CorrosionProof = TagManager.Create("CorrosionProof");

	public static readonly Tag PickupableStorage = TagManager.Create("PickupableStorage");

	public static readonly Tag UnidentifiedSeed = TagManager.Create("UnidentifiedSeed");

	public static readonly Tag CropSeed = TagManager.Create("CropSeed");

	public static readonly Tag DecorSeed = TagManager.Create("DecorSeed");

	public static readonly Tag WaterSeed = TagManager.Create("WaterSeed");

	public static readonly Tag Harvestable = TagManager.Create("Harvestable");

	public static readonly Tag Hanging = TagManager.Create("Hanging");

	public static readonly Tag FarmingMaterial = TagManager.Create("FarmingMaterial");

	public static readonly Tag MutatedSeed = TagManager.Create("MutatedSeed");

	public static readonly Tag OverlayInFrontOfConduits = TagManager.Create("OverlayFrontLayer");

	public static readonly Tag OverlayBehindConduits = TagManager.Create("OverlayBackLayer");

	public static readonly Tag MassChunk = TagManager.Create("MassChunk");

	public static readonly Tag UnitChunk = TagManager.Create("UnitChunk");

	public static readonly Tag NotConversationTopic = TagManager.Create("NotConversationTopic");

	public static readonly Tag MinionSelectPreview = TagManager.Create("MinionSelectPreview");

	public static readonly Tag Empty = TagManager.Create("Empty");

	public static readonly Tag ExcludeFromTemplate = TagManager.Create("ExcludeFromTemplate");

	public static readonly Tag SpaceDanger = TagManager.Create("SpaceDanger");

	public static TagSet SolidElements = new TagSet();

	public static TagSet LiquidElements = new TagSet();

	public static TagSet GasElements = new TagSet();

	public static TagSet CalorieCategories = new TagSet
	{
		GameTags.Edible
	};

	public static TagSet UnitCategories = new TagSet
	{
		GameTags.Medicine,
		GameTags.MedicalSupplies,
		GameTags.Seed,
		GameTags.Egg,
		GameTags.Clothes,
		GameTags.IndustrialIngredient,
		GameTags.IndustrialProduct,
		GameTags.Compostable,
		GameTags.HighEnergyParticle,
		GameTags.StoryTraitResource,
		GameTags.Dehydrated,
		GameTags.ChargedPortableBattery,
		GameTags.BionicUpgrade
	};

	public static TagSet IgnoredMaterialCategories = new TagSet
	{
		GameTags.Special,
		GameTags.IgnoreMaterialCategory
	};

	public static TagSet MaterialCategories = new TagSet
	{
		GameTags.Alloy,
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
		GameTags.RareMaterials
	};

	public static TagSet BionicCompatibleBatteries = new TagSet
	{
		"Electrobank",
		GameTags.DisposablePortableBattery,
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
		GameTags.OtherEntityTags
	});

	public static TagSet DisplayAsCalories = new TagSet(GameTags.CalorieCategories);

	public static TagSet DisplayAsUnits = new TagSet(GameTags.UnitCategories);

	public static TagSet DisplayAsInformation = new TagSet();

	public static Tag StartingMetalOre = new Tag("StartingMetalOre");

	public static Tag StartingRefinedMetal = new Tag("StartingRefinedMetal");

	public static Tag[] StartingMetalOres;

	public static Tag[] StartingRefinedMetals = null;

	public static Tag[] BasicMetalOres = new Tag[]
	{
		SimHashes.IronOre.CreateTag()
	};

	public static Tag[] BasicRefinedMetals = new Tag[]
	{
		SimHashes.Iron.CreateTag()
	};

	public static TagSet HiddenElementTags = new TagSet
	{
		GameTags.HideFromCodex,
		GameTags.HideFromSpawnTool,
		GameTags.StartingMetalOre,
		GameTags.StartingRefinedMetal
	};

	public static Tag[] Fabrics = new Tag[]
	{
		"BasicFabric".ToTag(),
		FeatherFabricConfig.ID
	};

	public static class Worlds
	{
		public static readonly Tag Ceres = TagManager.Create("Ceres");
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

		public static readonly Tag PreventGrowAnimation = TagManager.Create("PreventGrowAnimation");

		public static readonly Tag TrappedInCargoBay = TagManager.Create("TrappedInCargoBay");

		public static readonly Tag PausedHunger = TagManager.Create("PausedHunger");

		public static readonly Tag PausedReproduction = TagManager.Create("PausedReproduction");

		public static readonly Tag Bagged = TagManager.Create("Bagged");

		public static readonly Tag InIncubator = TagManager.Create("InIncubator");

		public static readonly Tag Deliverable = TagManager.Create("Deliverable");

		public static readonly Tag StunnedForCapture = TagManager.Create("StunnedForCapture");

		public static readonly Tag StunnedBeingEaten = TagManager.Create("StunnedBeingEaten");

		public static readonly Tag Falling = TagManager.Create("Falling");

		public static readonly Tag Flopping = TagManager.Create("Flopping");

		public static readonly Tag WantsToEnterBurrow = TagManager.Create("WantsToBurrow");

		public static readonly Tag Burrowed = TagManager.Create("Burrowed");

		public static readonly Tag WantsToExitBurrow = TagManager.Create("WantsToExitBurrow");

		public static readonly Tag WantsToEat = TagManager.Create("WantsToEat");

		public static readonly Tag SuppressedDiet = TagManager.Create("SuppressedDiet");

		public static readonly Tag UrgeToPoke = TagManager.Create("UrgeToPoke");

		public static readonly Tag WantsToStomp = TagManager.Create("WantsToStomp");

		public static readonly Tag WantsToHarvest = TagManager.Create("WantsToHarvest");

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

		public static readonly Tag TargetedPreyBehaviour = TagManager.Create("TargetedPrey");

		public static readonly Tag WantsToPollinate = TagManager.Create("WantsToPollinate");

		public static readonly Tag Pollinator = TagManager.Create("Pollinator");

		public static class Species
		{
			public static Tag[] AllSpecies_REFLECTION()
			{
				return GameTags.Reflection_GetTagsInClass(typeof(GameTags.Creatures.Species), BindingFlags.Static | BindingFlags.Public);
			}

			public static readonly Tag HatchSpecies = TagManager.Create("HatchSpecies", CREATURES.FAMILY_PLURAL.HATCHSPECIES);

			public static readonly Tag LightBugSpecies = TagManager.Create("LightBugSpecies", CREATURES.FAMILY_PLURAL.LIGHTBUGSPECIES);

			public static readonly Tag OilFloaterSpecies = TagManager.Create("OilFloaterSpecies", CREATURES.FAMILY_PLURAL.OILFLOATERSPECIES);

			public static readonly Tag DreckoSpecies = TagManager.Create("DreckoSpecies", CREATURES.FAMILY_PLURAL.DRECKOSPECIES);

			public static readonly Tag GlomSpecies = TagManager.Create("GlomSpecies", CREATURES.FAMILY_PLURAL.GLOMSPECIES);

			public static readonly Tag PuftSpecies = TagManager.Create("PuftSpecies", CREATURES.FAMILY_PLURAL.PUFTSPECIES);

			public static readonly Tag MosquitoSpecies = TagManager.Create("MosquitoSpecies", CREATURES.FAMILY_PLURAL.MOSQUITOSPECIES);

			public static readonly Tag PacuSpecies = TagManager.Create("PacuSpecies", CREATURES.FAMILY_PLURAL.PACUSPECIES);

			public static readonly Tag MooSpecies = TagManager.Create("MooSpecies", CREATURES.FAMILY_PLURAL.MOOSPECIES);

			public static readonly Tag MoleSpecies = TagManager.Create("MoleSpecies", CREATURES.FAMILY_PLURAL.MOLESPECIES);

			public static readonly Tag SquirrelSpecies = TagManager.Create("SquirrelSpecies", CREATURES.FAMILY_PLURAL.SQUIRRELSPECIES);

			public static readonly Tag CrabSpecies = TagManager.Create("CrabSpecies", CREATURES.FAMILY_PLURAL.CRABSPECIES);

			public static readonly Tag StaterpillarSpecies = TagManager.Create("StaterpillarSpecies", CREATURES.FAMILY_PLURAL.STATERPILLARSPECIES);

			public static readonly Tag BeetaSpecies = TagManager.Create("BeetaSpecies", CREATURES.FAMILY_PLURAL.BEETASPECIES);

			public static readonly Tag DivergentSpecies = TagManager.Create("DivergentSpecies", CREATURES.FAMILY_PLURAL.DIVERGENTSPECIES);

			public static readonly Tag DeerSpecies = TagManager.Create("DeerSpecies", CREATURES.FAMILY_PLURAL.DEERSPECIES);

			public static readonly Tag BellySpecies = TagManager.Create("BellySpecies", CREATURES.FAMILY_PLURAL.BELLYSPECIES);

			public static readonly Tag SealSpecies = TagManager.Create("SealSpecies", CREATURES.FAMILY_PLURAL.SEALSPECIES);

			public static readonly Tag RaptorSpecies = TagManager.Create("RaptorSpecies", CREATURES.FAMILY_PLURAL.RAPTORSPECIES);

			public static readonly Tag ChameleonSpecies = TagManager.Create("ChameleonSpecies", CREATURES.FAMILY_PLURAL.CHAMELEONSPECIES);

			public static readonly Tag PrehistoricPacuSpecies = TagManager.Create("PrehistoricPacuSpecies", CREATURES.FAMILY_PLURAL.PREHISTORICPACUSPECIES);

			public static readonly Tag StegoSpecies = TagManager.Create("StegoSpecies", CREATURES.FAMILY_PLURAL.STEGOSPECIES);

			public static readonly Tag ButterflySpecies = TagManager.Create("ButterflySpecies", CREATURES.FAMILY_PLURAL.BUTTERFLYSPECIES);
		}

		public static class Behaviours
		{
			public static readonly Tag HarvestHiveBehaviour = TagManager.Create("HarvestHiveBehaviour");

			public static readonly Tag GrowUpBehaviour = TagManager.Create("GrowUpBehaviour");

			public static readonly Tag SleepBehaviour = TagManager.Create("SleepBehaviour");

			public static readonly Tag CallAdultBehaviour = TagManager.Create("CallAdultBehaviour");

			public static readonly Tag SearchForEggBehaviour = TagManager.Create("SearchForEggBehaviour");

			public static readonly Tag PlayInterruptAnim = TagManager.Create("PlayInterruptAnim");

			public static readonly Tag DisableCreature = TagManager.Create("DisableCreature");
		}
	}

	public static class StoragesIds
	{
		public static readonly Tag DefaultStorage = TagManager.Create("Storage");

		public static readonly Tag BionicBatteryStorage = TagManager.Create("BionicBatteryStorage");

		public static readonly Tag BionicUpgradeStorage = TagManager.Create("BionicUpgradeStorage");

		public static readonly Tag BionicOxygenTankStorage = TagManager.Create("BionicOxygenTankStorage");
	}

	public static class Minions
	{
		public static class Models
		{
			public static string GetModelTooltipForTag(Tag modelTag)
			{
				if (modelTag == GameTags.Minions.Models.Bionic)
				{
					return DUPLICANTS.MODEL.BIONIC.NAME_TOOLTIP;
				}
				return "";
			}

			public static readonly Tag Standard = TagManager.Create("Minion", DUPLICANTS.MODEL.STANDARD.NAME);

			public static readonly Tag Bionic = TagManager.Create("BionicMinion", DUPLICANTS.MODEL.BIONIC.NAME);

			public static readonly Tag[] AllModels = new Tag[]
			{
				GameTags.Minions.Models.Standard,
				GameTags.Minions.Models.Bionic
			};
		}
	}

	public static class CodexCategories
	{
		public static string GetCategoryLabelText(Tag tag)
		{
			StringEntry entry = null;
			string text = "STRINGS.CODEX.CATEGORIES." + tag.ToString().ToUpper() + ".NAME";
			if (!Strings.TryGet(new StringKey(text), out entry))
			{
				return ROOMS.CRITERIA.IN_CODE_ERROR.text.Replace("{0}", text);
			}
			return entry;
		}

		public static List<Tag> AllTags = new List<Tag>();

		public static Tag CreatureRelocator = GameTags.CodexCategories.AllTags.AddAndReturn(TagManager.Create("CreatureRelocator"));

		public static Tag FarmBuilding = GameTags.CodexCategories.AllTags.AddAndReturn("FarmBuilding".ToTag());

		public static Tag BionicBuilding = GameTags.CodexCategories.AllTags.AddAndReturn("BionicBuilding".ToTag());
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

		public static class Behaviours
		{
			public static readonly Tag UnloadBehaviour = TagManager.Create("UnloadBehaviour");

			public static readonly Tag RechargeBehaviour = TagManager.Create("RechargeBehaviour");

			public static readonly Tag EmoteBehaviour = TagManager.Create("EmoteBehaviour");

			public static readonly Tag TrappedBehaviour = TagManager.Create("TrappedBehaviour");

			public static readonly Tag NoElectroBank = TagManager.Create("NoElectroBank");
		}
	}

	public class Search
	{
		public static readonly Tag Tile = TagManager.Create("Tile");

		public static readonly Tag Ladder = TagManager.Create("Ladder");

		public static readonly Tag Powered = TagManager.Create("Powered");

		public static readonly Tag Rocket = TagManager.Create("Rocket");

		public static readonly Tag Monument = TagManager.Create("Monument");

		public static readonly Tag Farming = TagManager.Create("Farming");

		public static readonly Tag Cooking = TagManager.Create("Cooking");
	}
}
