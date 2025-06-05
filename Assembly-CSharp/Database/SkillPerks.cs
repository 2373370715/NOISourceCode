using System;
using STRINGS;
using TUNING;

namespace Database
{
	// Token: 0x02002230 RID: 8752
	public class SkillPerks : ResourceSet<SkillPerk>
	{
		// Token: 0x0600BA13 RID: 47635 RVA: 0x0047A494 File Offset: 0x00478694
		public SkillPerks(ResourceSet parent) : base("SkillPerks", parent)
		{
			this.IncreaseDigSpeedSmall = base.Add(new SkillAttributePerk("IncreaseDigSpeedSmall", Db.Get().Attributes.Digging.Id, (float)ROLES.ATTRIBUTE_BONUS_FIRST, DUPLICANTS.ROLES.JUNIOR_MINER.NAME, false));
			this.IncreaseDigSpeedMedium = base.Add(new SkillAttributePerk("IncreaseDigSpeedMedium", Db.Get().Attributes.Digging.Id, (float)ROLES.ATTRIBUTE_BONUS_SECOND, DUPLICANTS.ROLES.MINER.NAME, false));
			this.IncreaseDigSpeedLarge = base.Add(new SkillAttributePerk("IncreaseDigSpeedLarge", Db.Get().Attributes.Digging.Id, (float)ROLES.ATTRIBUTE_BONUS_THIRD, DUPLICANTS.ROLES.SENIOR_MINER.NAME, false));
			this.CanDigVeryFirm = base.Add(new SimpleSkillPerk("CanDigVeryFirm", UI.ROLES_SCREEN.PERKS.CAN_DIG_VERY_FIRM.DESCRIPTION));
			this.CanDigNearlyImpenetrable = base.Add(new SimpleSkillPerk("CanDigAbyssalite", UI.ROLES_SCREEN.PERKS.CAN_DIG_NEARLY_IMPENETRABLE.DESCRIPTION));
			this.CanDigSuperDuperHard = base.Add(new SimpleSkillPerk("CanDigDiamondAndObsidan", UI.ROLES_SCREEN.PERKS.CAN_DIG_SUPER_SUPER_HARD.DESCRIPTION));
			this.CanDigRadioactiveMaterials = base.Add(new SimpleSkillPerk("CanDigCorium", UI.ROLES_SCREEN.PERKS.CAN_DIG_RADIOACTIVE_MATERIALS.DESCRIPTION));
			this.CanDigUnobtanium = base.Add(new SimpleSkillPerk("CanDigUnobtanium", UI.ROLES_SCREEN.PERKS.CAN_DIG_UNOBTANIUM.DESCRIPTION));
			this.IncreaseConstructionSmall = base.Add(new SkillAttributePerk("IncreaseConstructionSmall", Db.Get().Attributes.Construction.Id, (float)ROLES.ATTRIBUTE_BONUS_FIRST, DUPLICANTS.ROLES.JUNIOR_BUILDER.NAME, false));
			this.IncreaseConstructionMedium = base.Add(new SkillAttributePerk("IncreaseConstructionMedium", Db.Get().Attributes.Construction.Id, (float)ROLES.ATTRIBUTE_BONUS_SECOND, DUPLICANTS.ROLES.BUILDER.NAME, false));
			this.IncreaseConstructionLarge = base.Add(new SkillAttributePerk("IncreaseConstructionLarge", Db.Get().Attributes.Construction.Id, (float)ROLES.ATTRIBUTE_BONUS_THIRD, DUPLICANTS.ROLES.SENIOR_BUILDER.NAME, false));
			this.IncreaseConstructionMechatronics = base.Add(new SkillAttributePerk("IncreaseConstructionMechatronics", Db.Get().Attributes.Construction.Id, (float)ROLES.ATTRIBUTE_BONUS_THIRD, DUPLICANTS.ROLES.MECHATRONIC_ENGINEER.NAME, false));
			this.CanDemolish = base.Add(new SimpleSkillPerk("CanDemonlish", UI.ROLES_SCREEN.PERKS.CAN_DEMOLISH.DESCRIPTION));
			this.IncreaseLearningSmall = base.Add(new SkillAttributePerk("IncreaseLearningSmall", Db.Get().Attributes.Learning.Id, (float)ROLES.ATTRIBUTE_BONUS_FIRST, DUPLICANTS.ROLES.JUNIOR_RESEARCHER.NAME, false));
			this.IncreaseLearningMedium = base.Add(new SkillAttributePerk("IncreaseLearningMedium", Db.Get().Attributes.Learning.Id, (float)ROLES.ATTRIBUTE_BONUS_SECOND, DUPLICANTS.ROLES.RESEARCHER.NAME, false));
			this.IncreaseLearningLarge = base.Add(new SkillAttributePerk("IncreaseLearningLarge", Db.Get().Attributes.Learning.Id, (float)ROLES.ATTRIBUTE_BONUS_THIRD, DUPLICANTS.ROLES.SENIOR_RESEARCHER.NAME, false));
			this.IncreaseLearningLargeSpace = base.Add(new SkillAttributePerk("IncreaseLearningLargeSpace", Db.Get().Attributes.Learning.Id, (float)ROLES.ATTRIBUTE_BONUS_THIRD, DUPLICANTS.ROLES.SPACE_RESEARCHER.NAME, false));
			this.IncreaseBotanySmall = base.Add(new SkillAttributePerk("IncreaseBotanySmall", Db.Get().Attributes.Botanist.Id, (float)ROLES.ATTRIBUTE_BONUS_FIRST, DUPLICANTS.ROLES.JUNIOR_FARMER.NAME, false));
			this.IncreaseBotanyMedium = base.Add(new SkillAttributePerk("IncreaseBotanyMedium", Db.Get().Attributes.Botanist.Id, (float)ROLES.ATTRIBUTE_BONUS_SECOND, DUPLICANTS.ROLES.FARMER.NAME, false));
			this.IncreaseBotanyLarge = base.Add(new SkillAttributePerk("IncreaseBotanyLarge", Db.Get().Attributes.Botanist.Id, (float)ROLES.ATTRIBUTE_BONUS_THIRD, DUPLICANTS.ROLES.SENIOR_FARMER.NAME, false));
			this.CanFarmTinker = base.Add(new SimpleSkillPerk("CanFarmTinker", UI.ROLES_SCREEN.PERKS.CAN_FARM_TINKER.DESCRIPTION));
			this.CanIdentifyMutantSeeds = base.Add(new SimpleSkillPerk("CanIdentifyMutantSeeds", UI.ROLES_SCREEN.PERKS.CAN_IDENTIFY_MUTANT_SEEDS.DESCRIPTION));
			this.CanFarmStation = base.Add(new SimpleSkillPerk("CanFarmStation", UI.ROLES_SCREEN.PERKS.CAN_FARM_STATION.DESCRIPTION));
			this.IncreaseRanchingSmall = base.Add(new SkillAttributePerk("IncreaseRanchingSmall", Db.Get().Attributes.Ranching.Id, (float)ROLES.ATTRIBUTE_BONUS_FIRST, DUPLICANTS.ROLES.RANCHER.NAME, false));
			this.IncreaseRanchingMedium = base.Add(new SkillAttributePerk("IncreaseRanchingMedium", Db.Get().Attributes.Ranching.Id, (float)ROLES.ATTRIBUTE_BONUS_SECOND, DUPLICANTS.ROLES.SENIOR_RANCHER.NAME, false));
			this.CanWrangleCreatures = base.Add(new SimpleSkillPerk("CanWrangleCreatures", UI.ROLES_SCREEN.PERKS.CAN_WRANGLE_CREATURES.DESCRIPTION));
			this.CanUseRanchStation = base.Add(new SimpleSkillPerk("CanUseRanchStation", UI.ROLES_SCREEN.PERKS.CAN_USE_RANCH_STATION.DESCRIPTION));
			this.CanUseMilkingStation = base.Add(new SimpleSkillPerk("CanUseMilkingStation", UI.ROLES_SCREEN.PERKS.CAN_USE_MILKING_STATION.DESCRIPTION));
			this.IncreaseAthleticsSmall = base.Add(new SkillAttributePerk("IncreaseAthleticsSmall", Db.Get().Attributes.Athletics.Id, (float)ROLES.ATTRIBUTE_BONUS_FIRST, DUPLICANTS.ROLES.HAULER.NAME, false));
			this.IncreaseAthleticsMedium = base.Add(new SkillAttributePerk("IncreaseAthletics", Db.Get().Attributes.Athletics.Id, (float)ROLES.ATTRIBUTE_BONUS_SECOND, DUPLICANTS.ROLES.SUIT_EXPERT.NAME, false));
			this.IncreaseAthleticsLarge = base.Add(new SkillAttributePerk("IncreaseAthleticsLarge", Db.Get().Attributes.Athletics.Id, (float)ROLES.ATTRIBUTE_BONUS_THIRD, DUPLICANTS.ROLES.SUIT_DURABILITY.NAME, false));
			this.IncreaseStrengthGofer = base.Add(new SkillAttributePerk("IncreaseStrengthGofer", Db.Get().Attributes.Strength.Id, (float)ROLES.ATTRIBUTE_BONUS_FIRST, DUPLICANTS.ROLES.HAULER.NAME, false));
			this.IncreaseStrengthCourier = base.Add(new SkillAttributePerk("IncreaseStrengthCourier", Db.Get().Attributes.Strength.Id, (float)ROLES.ATTRIBUTE_BONUS_SECOND, DUPLICANTS.ROLES.MATERIALS_MANAGER.NAME, false));
			this.IncreaseStrengthGroundskeeper = base.Add(new SkillAttributePerk("IncreaseStrengthGroundskeeper", Db.Get().Attributes.Strength.Id, (float)ROLES.ATTRIBUTE_BONUS_FIRST, DUPLICANTS.ROLES.HANDYMAN.NAME, false));
			this.IncreaseStrengthPlumber = base.Add(new SkillAttributePerk("IncreaseStrengthPlumber", Db.Get().Attributes.Strength.Id, (float)ROLES.ATTRIBUTE_BONUS_SECOND, DUPLICANTS.ROLES.PLUMBER.NAME, false));
			this.IncreaseCarryAmountSmall = base.Add(new SkillAttributePerk("IncreaseCarryAmountSmall", Db.Get().Attributes.CarryAmount.Id, 400f, DUPLICANTS.ROLES.HAULER.NAME, false));
			this.IncreaseCarryAmountMedium = base.Add(new SkillAttributePerk("IncreaseCarryAmountMedium", Db.Get().Attributes.CarryAmount.Id, 800f, DUPLICANTS.ROLES.MATERIALS_MANAGER.NAME, false));
			this.IncreaseArtSmall = base.Add(new SkillAttributePerk("IncreaseArtSmall", Db.Get().Attributes.Art.Id, (float)ROLES.ATTRIBUTE_BONUS_FIRST, DUPLICANTS.ROLES.JUNIOR_ARTIST.NAME, false));
			this.IncreaseArtMedium = base.Add(new SkillAttributePerk("IncreaseArt", Db.Get().Attributes.Art.Id, (float)ROLES.ATTRIBUTE_BONUS_SECOND, DUPLICANTS.ROLES.ARTIST.NAME, false));
			this.IncreaseArtLarge = base.Add(new SkillAttributePerk("IncreaseArtLarge", Db.Get().Attributes.Art.Id, (float)ROLES.ATTRIBUTE_BONUS_THIRD, DUPLICANTS.ROLES.MASTER_ARTIST.NAME, false));
			this.CanArt = base.Add(new SimpleSkillPerk("CanArt", UI.ROLES_SCREEN.PERKS.CAN_ART.DESCRIPTION));
			this.CanArtUgly = base.Add(new SimpleSkillPerk("CanArtUgly", UI.ROLES_SCREEN.PERKS.CAN_ART_UGLY.DESCRIPTION));
			this.CanArtOkay = base.Add(new SimpleSkillPerk("CanArtOkay", UI.ROLES_SCREEN.PERKS.CAN_ART_OKAY.DESCRIPTION));
			this.CanArtGreat = base.Add(new SimpleSkillPerk("CanArtGreat", UI.ROLES_SCREEN.PERKS.CAN_ART_GREAT.DESCRIPTION));
			this.CanStudyArtifact = base.Add(new SimpleSkillPerk("CanStudyArtifact", UI.ROLES_SCREEN.PERKS.CAN_STUDY_ARTIFACTS.DESCRIPTION));
			this.CanClothingAlteration = base.Add(new SimpleSkillPerk("CanClothingAlteration", UI.ROLES_SCREEN.PERKS.CAN_CLOTHING_ALTERATION.DESCRIPTION));
			this.IncreaseMachinerySmall = base.Add(new SkillAttributePerk("IncreaseMachinerySmall", Db.Get().Attributes.Machinery.Id, (float)ROLES.ATTRIBUTE_BONUS_FIRST, DUPLICANTS.ROLES.MACHINE_TECHNICIAN.NAME, false));
			this.IncreaseMachineryMedium = base.Add(new SkillAttributePerk("IncreaseMachineryMedium", Db.Get().Attributes.Machinery.Id, (float)ROLES.ATTRIBUTE_BONUS_SECOND, DUPLICANTS.ROLES.POWER_TECHNICIAN.NAME, false));
			this.IncreaseMachineryLarge = base.Add(new SkillAttributePerk("IncreaseMachineryLarge", Db.Get().Attributes.Machinery.Id, (float)ROLES.ATTRIBUTE_BONUS_THIRD, DUPLICANTS.ROLES.MECHATRONIC_ENGINEER.NAME, false));
			this.ConveyorBuild = base.Add(new SimpleSkillPerk("ConveyorBuild", UI.ROLES_SCREEN.PERKS.CONVEYOR_BUILD.DESCRIPTION));
			this.CanPowerTinker = base.Add(new SimpleSkillPerk("CanPowerTinker", UI.ROLES_SCREEN.PERKS.CAN_POWER_TINKER.DESCRIPTION));
			this.CanMakeMissiles = base.Add(new SimpleSkillPerk("CanMakeMissiles", UI.ROLES_SCREEN.PERKS.CAN_MAKE_MISSILES.DESCRIPTION));
			this.CanCraftElectronics = base.Add(new SimpleSkillPerk("CanCraftElectronics", UI.ROLES_SCREEN.PERKS.CAN_CRAFT_ELECTRONICS.DESCRIPTION, DlcManager.DLC3));
			this.CanElectricGrill = base.Add(new SimpleSkillPerk("CanElectricGrill", UI.ROLES_SCREEN.PERKS.CAN_ELECTRIC_GRILL.DESCRIPTION));
			this.CanGasRange = base.Add(new SimpleSkillPerk("CanGasRange", UI.ROLES_SCREEN.PERKS.CAN_GAS_RANGE.DESCRIPTION));
			this.CanDeepFry = base.Add(new SimpleSkillPerk("CanDeepFry", UI.ROLES_SCREEN.PERKS.CAN_DEEP_FRYER.DESCRIPTION));
			this.IncreaseCookingSmall = base.Add(new SkillAttributePerk("IncreaseCookingSmall", Db.Get().Attributes.Cooking.Id, (float)ROLES.ATTRIBUTE_BONUS_FIRST, DUPLICANTS.ROLES.JUNIOR_COOK.NAME, false));
			this.IncreaseCookingMedium = base.Add(new SkillAttributePerk("IncreaseCookingMedium", Db.Get().Attributes.Cooking.Id, (float)ROLES.ATTRIBUTE_BONUS_SECOND, DUPLICANTS.ROLES.COOK.NAME, false));
			this.CanSpiceGrinder = base.Add(new SimpleSkillPerk("CanSpiceGrinder ", UI.ROLES_SCREEN.PERKS.CAN_SPICE_GRINDER.DESCRIPTION));
			this.IncreaseCaringSmall = base.Add(new SkillAttributePerk("IncreaseCaringSmall", Db.Get().Attributes.Caring.Id, (float)ROLES.ATTRIBUTE_BONUS_FIRST, DUPLICANTS.ROLES.JUNIOR_MEDIC.NAME, false));
			this.IncreaseCaringMedium = base.Add(new SkillAttributePerk("IncreaseCaringMedium", Db.Get().Attributes.Caring.Id, (float)ROLES.ATTRIBUTE_BONUS_SECOND, DUPLICANTS.ROLES.MEDIC.NAME, false));
			this.IncreaseCaringLarge = base.Add(new SkillAttributePerk("IncreaseCaringLarge", Db.Get().Attributes.Caring.Id, (float)ROLES.ATTRIBUTE_BONUS_THIRD, DUPLICANTS.ROLES.SENIOR_MEDIC.NAME, false));
			this.CanCompound = base.Add(new SimpleSkillPerk("CanCompound", UI.ROLES_SCREEN.PERKS.CAN_COMPOUND.DESCRIPTION));
			this.CanDoctor = base.Add(new SimpleSkillPerk("CanDoctor", UI.ROLES_SCREEN.PERKS.CAN_DOCTOR.DESCRIPTION));
			this.CanAdvancedMedicine = base.Add(new SimpleSkillPerk("CanAdvancedMedicine", UI.ROLES_SCREEN.PERKS.CAN_ADVANCED_MEDICINE.DESCRIPTION));
			this.ExosuitExpertise = base.Add(new SimpleSkillPerk("ExosuitExpertise", UI.ROLES_SCREEN.PERKS.EXOSUIT_EXPERTISE.DESCRIPTION));
			this.ExosuitDurability = base.Add(new SimpleSkillPerk("ExosuitDurability", UI.ROLES_SCREEN.PERKS.EXOSUIT_DURABILITY.DESCRIPTION));
			this.AllowAdvancedResearch = base.Add(new SimpleSkillPerk("AllowAdvancedResearch", UI.ROLES_SCREEN.PERKS.ADVANCED_RESEARCH.DESCRIPTION));
			this.AllowInterstellarResearch = base.Add(new SimpleSkillPerk("AllowInterStellarResearch", UI.ROLES_SCREEN.PERKS.INTERSTELLAR_RESEARCH.DESCRIPTION));
			this.AllowNuclearResearch = base.Add(new SimpleSkillPerk("AllowNuclearResearch", UI.ROLES_SCREEN.PERKS.NUCLEAR_RESEARCH.DESCRIPTION));
			this.AllowOrbitalResearch = base.Add(new SimpleSkillPerk("AllowOrbitalResearch", UI.ROLES_SCREEN.PERKS.ORBITAL_RESEARCH.DESCRIPTION));
			this.AllowGeyserTuning = base.Add(new SimpleSkillPerk("AllowGeyserTuning", UI.ROLES_SCREEN.PERKS.GEYSER_TUNING.DESCRIPTION));
			this.CanStudyWorldObjects = base.Add(new SimpleSkillPerk("CanStudyWorldObjects", UI.ROLES_SCREEN.PERKS.CAN_STUDY_WORLD_OBJECTS.DESCRIPTION));
			this.CanUseClusterTelescope = base.Add(new SimpleSkillPerk("CanUseClusterTelescope", UI.ROLES_SCREEN.PERKS.CAN_USE_CLUSTER_TELESCOPE.DESCRIPTION));
			this.CanUseClusterTelescopeEnclosed = base.Add(new SimpleSkillPerk("CanUseClusterTelescopeEnclosed", UI.ROLES_SCREEN.PERKS.CAN_CLUSTERTELESCOPEENCLOSED.DESCRIPTION));
			this.CanDoPlumbing = base.Add(new SimpleSkillPerk("CanDoPlumbing", UI.ROLES_SCREEN.PERKS.CAN_DO_PLUMBING.DESCRIPTION));
			this.CanUseRockets = base.Add(new SimpleSkillPerk("CanUseRockets", UI.ROLES_SCREEN.PERKS.CAN_USE_ROCKETS.DESCRIPTION));
			this.FasterSpaceFlight = base.Add(new SkillAttributePerk("FasterSpaceFlight", Db.Get().Attributes.SpaceNavigation.Id, 0.1f, DUPLICANTS.ROLES.ASTRONAUT.NAME, false));
			this.CanTrainToBeAstronaut = base.Add(new SimpleSkillPerk("CanTrainToBeAstronaut", UI.ROLES_SCREEN.PERKS.CAN_DO_ASTRONAUT_TRAINING.DESCRIPTION));
			this.CanMissionControl = base.Add(new SimpleSkillPerk("CanMissionControl", UI.ROLES_SCREEN.PERKS.CAN_MISSION_CONTROL.DESCRIPTION));
			this.CanUseRocketControlStation = base.Add(new SimpleSkillPerk("CanUseRocketControlStation", UI.ROLES_SCREEN.PERKS.CAN_PILOT_ROCKET.DESCRIPTION));
			this.IncreaseRocketSpeedSmall = base.Add(new SkillAttributePerk("IncreaseRocketSpeedSmall", Db.Get().Attributes.SpaceNavigation.Id, (float)ROLES.ATTRIBUTE_BONUS_FIRST, DUPLICANTS.ROLES.ROCKETPILOT.NAME, false));
			if (DlcManager.IsContentSubscribed("DLC3_ID"))
			{
				this.IncreaseCarryAmountBionic = base.Add(new SkillAttributePerk("IncreaseCarryAmountBionic", Db.Get().Attributes.CarryAmount.Id, 600f, DUPLICANTS.ROLES.MATERIALS_MANAGER.NAME, false));
				this.ExtraBionicBooster1 = base.Add(new SkillAttributePerk("ExtraBionicBooster1", Db.Get().Attributes.BionicBoosterSlots.Id, 1f, DUPLICANTS.ATTRIBUTES.BIONICBOOSTERSLOTS.DESC, false));
				this.ExtraBionicBooster2 = base.Add(new SkillAttributePerk("ExtraBionicBooster2", Db.Get().Attributes.BionicBoosterSlots.Id, 1f, DUPLICANTS.ATTRIBUTES.BIONICBOOSTERSLOTS.DESC, false));
				this.ExtraBionicBooster3 = base.Add(new SkillAttributePerk("ExtraBionicBooster3", Db.Get().Attributes.BionicBoosterSlots.Id, 2f, DUPLICANTS.ATTRIBUTES.BIONICBOOSTERSLOTS.DESC, false));
				this.ExtraBionicBooster4 = base.Add(new SkillAttributePerk("ExtraBionicBooster4", Db.Get().Attributes.BionicBoosterSlots.Id, 1f, DUPLICANTS.ATTRIBUTES.BIONICBOOSTERSLOTS.DESC, false));
				this.ExtraBionicBooster5 = base.Add(new SkillAttributePerk("ExtraBionicBooster5", Db.Get().Attributes.BionicBoosterSlots.Id, 1f, "", false));
				this.ExtraBionicBooster6 = base.Add(new SkillAttributePerk("ExtraBionicBooster6", Db.Get().Attributes.BionicBoosterSlots.Id, 1f, DUPLICANTS.ATTRIBUTES.BIONICBOOSTERSLOTS.DESC, false));
				this.ExtraBionicBatteries = base.Add(new SkillAttributePerk("ExtraBionicBatteries", Db.Get().Attributes.BionicBatteryCountCapacity.Id, 2f, UI.ROLES_SCREEN.PERKS.EXTRA_BIONIC_BATTERIES.DESCRIPTION, false));
				this.ReducedBionicGunkProduction = base.Add(new SimpleSkillPerk("ReducedBionicGunkProduction", UI.ROLES_SCREEN.PERKS.REDUCED_GUNK_PRODUCTION.DESCRIPTION));
				this.EfficientBionicGears = base.Add(new SimpleSkillPerk("EfficientBionicGears", UI.ROLES_SCREEN.PERKS.EFFICIENT_BIONIC_GEARS.DESCRIPTION));
				this.IncreaseAthleticsBionicsC1 = base.Add(new SkillAttributePerk("IncreaseAthleticsBionicsC1", Db.Get().Attributes.Athletics.Id, 2f, DUPLICANTS.ROLES.BIONICS_C1.NAME, false));
				this.IncreaseAthleticsBionicsC2 = base.Add(new SkillAttributePerk("IncreaseAthleticsBionicsC2", Db.Get().Attributes.Athletics.Id, 2f, DUPLICANTS.ROLES.BIONICS_C2.NAME, false));
				this.IncreaseAthleticsBionicsB2 = base.Add(new SkillAttributePerk("IncreaseAthleticsBionicsB2", Db.Get().Attributes.Athletics.Id, 2f, DUPLICANTS.ROLES.BIONICS_B2.NAME, false));
				this.IncreaseAthleticsBionicsA2 = base.Add(new SkillAttributePerk("IncreaseAthleticsBionicsA2", Db.Get().Attributes.Athletics.Id, 2f, DUPLICANTS.ROLES.BIONICS_A2.NAME, false));
				this.IncreasedCarryBionics = base.Add(new SkillAttributePerk("IncreasedCarryBionics", Db.Get().Attributes.CarryAmount.Id, 400f, STRINGS.ITEMS.BIONIC_BOOSTERS.BOOSTER_CARRY1.NAME, true));
			}
		}

		// Token: 0x040097ED RID: 38893
		public SkillPerk IncreaseDigSpeedSmall;

		// Token: 0x040097EE RID: 38894
		public SkillPerk IncreaseDigSpeedMedium;

		// Token: 0x040097EF RID: 38895
		public SkillPerk IncreaseDigSpeedLarge;

		// Token: 0x040097F0 RID: 38896
		public SkillPerk CanDigVeryFirm;

		// Token: 0x040097F1 RID: 38897
		public SkillPerk CanDigNearlyImpenetrable;

		// Token: 0x040097F2 RID: 38898
		public SkillPerk CanDigSuperDuperHard;

		// Token: 0x040097F3 RID: 38899
		public SkillPerk CanDigRadioactiveMaterials;

		// Token: 0x040097F4 RID: 38900
		public SkillPerk CanDigUnobtanium;

		// Token: 0x040097F5 RID: 38901
		public SkillPerk IncreaseConstructionSmall;

		// Token: 0x040097F6 RID: 38902
		public SkillPerk IncreaseConstructionMedium;

		// Token: 0x040097F7 RID: 38903
		public SkillPerk IncreaseConstructionLarge;

		// Token: 0x040097F8 RID: 38904
		public SkillPerk IncreaseConstructionMechatronics;

		// Token: 0x040097F9 RID: 38905
		public SkillPerk CanDemolish;

		// Token: 0x040097FA RID: 38906
		public SkillPerk IncreaseLearningSmall;

		// Token: 0x040097FB RID: 38907
		public SkillPerk IncreaseLearningMedium;

		// Token: 0x040097FC RID: 38908
		public SkillPerk IncreaseLearningLarge;

		// Token: 0x040097FD RID: 38909
		public SkillPerk IncreaseLearningLargeSpace;

		// Token: 0x040097FE RID: 38910
		public SkillPerk IncreaseBotanySmall;

		// Token: 0x040097FF RID: 38911
		public SkillPerk IncreaseBotanyMedium;

		// Token: 0x04009800 RID: 38912
		public SkillPerk IncreaseBotanyLarge;

		// Token: 0x04009801 RID: 38913
		public SkillPerk CanFarmTinker;

		// Token: 0x04009802 RID: 38914
		public SkillPerk CanIdentifyMutantSeeds;

		// Token: 0x04009803 RID: 38915
		public SkillPerk CanFarmStation;

		// Token: 0x04009804 RID: 38916
		public SkillPerk CanWrangleCreatures;

		// Token: 0x04009805 RID: 38917
		public SkillPerk CanUseRanchStation;

		// Token: 0x04009806 RID: 38918
		public SkillPerk CanUseMilkingStation;

		// Token: 0x04009807 RID: 38919
		public SkillPerk IncreaseRanchingSmall;

		// Token: 0x04009808 RID: 38920
		public SkillPerk IncreaseRanchingMedium;

		// Token: 0x04009809 RID: 38921
		public SkillPerk IncreaseAthleticsSmall;

		// Token: 0x0400980A RID: 38922
		public SkillPerk IncreaseAthleticsMedium;

		// Token: 0x0400980B RID: 38923
		public SkillPerk IncreaseAthleticsLarge;

		// Token: 0x0400980C RID: 38924
		public SkillPerk IncreaseStrengthSmall;

		// Token: 0x0400980D RID: 38925
		public SkillPerk IncreaseStrengthMedium;

		// Token: 0x0400980E RID: 38926
		public SkillPerk IncreaseStrengthGofer;

		// Token: 0x0400980F RID: 38927
		public SkillPerk IncreaseStrengthCourier;

		// Token: 0x04009810 RID: 38928
		public SkillPerk IncreaseStrengthGroundskeeper;

		// Token: 0x04009811 RID: 38929
		public SkillPerk IncreaseStrengthPlumber;

		// Token: 0x04009812 RID: 38930
		public SkillPerk IncreaseCarryAmountSmall;

		// Token: 0x04009813 RID: 38931
		public SkillPerk IncreaseCarryAmountMedium;

		// Token: 0x04009814 RID: 38932
		public SkillPerk IncreaseCarryAmountBionic;

		// Token: 0x04009815 RID: 38933
		public SkillPerk IncreaseArtSmall;

		// Token: 0x04009816 RID: 38934
		public SkillPerk IncreaseArtMedium;

		// Token: 0x04009817 RID: 38935
		public SkillPerk IncreaseArtLarge;

		// Token: 0x04009818 RID: 38936
		public SkillPerk CanArt;

		// Token: 0x04009819 RID: 38937
		public SkillPerk CanArtUgly;

		// Token: 0x0400981A RID: 38938
		public SkillPerk CanArtOkay;

		// Token: 0x0400981B RID: 38939
		public SkillPerk CanArtGreat;

		// Token: 0x0400981C RID: 38940
		public SkillPerk CanStudyArtifact;

		// Token: 0x0400981D RID: 38941
		public SkillPerk CanClothingAlteration;

		// Token: 0x0400981E RID: 38942
		public SkillPerk IncreaseMachinerySmall;

		// Token: 0x0400981F RID: 38943
		public SkillPerk IncreaseMachineryMedium;

		// Token: 0x04009820 RID: 38944
		public SkillPerk IncreaseMachineryLarge;

		// Token: 0x04009821 RID: 38945
		public SkillPerk ConveyorBuild;

		// Token: 0x04009822 RID: 38946
		public SkillPerk CanMakeMissiles;

		// Token: 0x04009823 RID: 38947
		public SkillPerk CanPowerTinker;

		// Token: 0x04009824 RID: 38948
		public SkillPerk CanCraftElectronics;

		// Token: 0x04009825 RID: 38949
		public SkillPerk CanElectricGrill;

		// Token: 0x04009826 RID: 38950
		public SkillPerk CanGasRange;

		// Token: 0x04009827 RID: 38951
		public SkillPerk CanDeepFry;

		// Token: 0x04009828 RID: 38952
		public SkillPerk IncreaseCookingSmall;

		// Token: 0x04009829 RID: 38953
		public SkillPerk IncreaseCookingMedium;

		// Token: 0x0400982A RID: 38954
		public SkillPerk CanSpiceGrinder;

		// Token: 0x0400982B RID: 38955
		public SkillPerk IncreaseCaringSmall;

		// Token: 0x0400982C RID: 38956
		public SkillPerk IncreaseCaringMedium;

		// Token: 0x0400982D RID: 38957
		public SkillPerk IncreaseCaringLarge;

		// Token: 0x0400982E RID: 38958
		public SkillPerk CanCompound;

		// Token: 0x0400982F RID: 38959
		public SkillPerk CanDoctor;

		// Token: 0x04009830 RID: 38960
		public SkillPerk CanAdvancedMedicine;

		// Token: 0x04009831 RID: 38961
		public SkillPerk ExosuitExpertise;

		// Token: 0x04009832 RID: 38962
		public SkillPerk ExosuitDurability;

		// Token: 0x04009833 RID: 38963
		public SkillPerk AllowAdvancedResearch;

		// Token: 0x04009834 RID: 38964
		public SkillPerk AllowInterstellarResearch;

		// Token: 0x04009835 RID: 38965
		public SkillPerk AllowNuclearResearch;

		// Token: 0x04009836 RID: 38966
		public SkillPerk AllowOrbitalResearch;

		// Token: 0x04009837 RID: 38967
		public SkillPerk AllowGeyserTuning;

		// Token: 0x04009838 RID: 38968
		public SkillPerk CanStudyWorldObjects;

		// Token: 0x04009839 RID: 38969
		public SkillPerk CanUseClusterTelescope;

		// Token: 0x0400983A RID: 38970
		public SkillPerk CanUseClusterTelescopeEnclosed;

		// Token: 0x0400983B RID: 38971
		public SkillPerk IncreaseRocketSpeedSmall;

		// Token: 0x0400983C RID: 38972
		public SkillPerk CanMissionControl;

		// Token: 0x0400983D RID: 38973
		public SkillPerk CanDoPlumbing;

		// Token: 0x0400983E RID: 38974
		public SkillPerk CanUseRockets;

		// Token: 0x0400983F RID: 38975
		public SkillPerk FasterSpaceFlight;

		// Token: 0x04009840 RID: 38976
		public SkillPerk CanTrainToBeAstronaut;

		// Token: 0x04009841 RID: 38977
		public SkillPerk CanUseRocketControlStation;

		// Token: 0x04009842 RID: 38978
		public SkillPerk ExtraBionicBooster1;

		// Token: 0x04009843 RID: 38979
		public SkillPerk ExtraBionicBooster2;

		// Token: 0x04009844 RID: 38980
		public SkillPerk ExtraBionicBooster3;

		// Token: 0x04009845 RID: 38981
		public SkillPerk ExtraBionicBooster4;

		// Token: 0x04009846 RID: 38982
		public SkillPerk ExtraBionicBooster5;

		// Token: 0x04009847 RID: 38983
		public SkillPerk ExtraBionicBooster6;

		// Token: 0x04009848 RID: 38984
		public SkillPerk ReducedBionicGunkProduction;

		// Token: 0x04009849 RID: 38985
		public SkillPerk EfficientBionicGears;

		// Token: 0x0400984A RID: 38986
		public SkillPerk ExtraBionicBatteries;

		// Token: 0x0400984B RID: 38987
		public SkillPerk IncreaseAthleticsBionicsC1;

		// Token: 0x0400984C RID: 38988
		public SkillPerk IncreaseAthleticsBionicsC2;

		// Token: 0x0400984D RID: 38989
		public SkillPerk IncreaseAthleticsBionicsB2;

		// Token: 0x0400984E RID: 38990
		public SkillPerk IncreaseAthleticsBionicsA2;

		// Token: 0x0400984F RID: 38991
		public SkillPerk IncreasedCarryBionics;
	}
}
