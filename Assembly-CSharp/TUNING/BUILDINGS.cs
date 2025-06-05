using System;
using System.Collections.Generic;

namespace TUNING
{
	// Token: 0x02002266 RID: 8806
	public class BUILDINGS
	{
		// Token: 0x04009979 RID: 39289
		public const float DEFAULT_STORAGE_CAPACITY = 2000f;

		// Token: 0x0400997A RID: 39290
		public const float STANDARD_MANUAL_REFILL_LEVEL = 0.2f;

		// Token: 0x0400997B RID: 39291
		public const float MASS_TEMPERATURE_SCALE = 0.2f;

		// Token: 0x0400997C RID: 39292
		public const float AIRCONDITIONER_TEMPDELTA = -14f;

		// Token: 0x0400997D RID: 39293
		public const float MAX_ENVIRONMENT_DELTA = -50f;

		// Token: 0x0400997E RID: 39294
		public const float COMPOST_FLIP_TIME = 20f;

		// Token: 0x0400997F RID: 39295
		public const int TUBE_LAUNCHER_MAX_CHARGES = 3;

		// Token: 0x04009980 RID: 39296
		public const float TUBE_LAUNCHER_RECHARGE_TIME = 10f;

		// Token: 0x04009981 RID: 39297
		public const float TUBE_LAUNCHER_WORK_TIME = 1f;

		// Token: 0x04009982 RID: 39298
		public const float SMELTER_INGOT_INPUTKG = 500f;

		// Token: 0x04009983 RID: 39299
		public const float SMELTER_INGOT_OUTPUTKG = 100f;

		// Token: 0x04009984 RID: 39300
		public const float SMELTER_FABRICATIONTIME = 120f;

		// Token: 0x04009985 RID: 39301
		public const float GEOREFINERY_SLAB_INPUTKG = 1000f;

		// Token: 0x04009986 RID: 39302
		public const float GEOREFINERY_SLAB_OUTPUTKG = 200f;

		// Token: 0x04009987 RID: 39303
		public const float GEOREFINERY_FABRICATIONTIME = 120f;

		// Token: 0x04009988 RID: 39304
		public const float MASS_BURN_RATE_HYDROGENGENERATOR = 0.1f;

		// Token: 0x04009989 RID: 39305
		public const float COOKER_FOOD_TEMPERATURE = 368.15f;

		// Token: 0x0400998A RID: 39306
		public const float OVERHEAT_DAMAGE_INTERVAL = 7.5f;

		// Token: 0x0400998B RID: 39307
		public const float MIN_BUILD_TEMPERATURE = 0f;

		// Token: 0x0400998C RID: 39308
		public const float MAX_BUILD_TEMPERATURE = 318.15f;

		// Token: 0x0400998D RID: 39309
		public const float MELTDOWN_TEMPERATURE = 533.15f;

		// Token: 0x0400998E RID: 39310
		public const float REPAIR_FORCE_TEMPERATURE = 293.15f;

		// Token: 0x0400998F RID: 39311
		public const int REPAIR_EFFECTIVENESS_BASE = 10;

		// Token: 0x04009990 RID: 39312
		public static Dictionary<string, string> PLANSUBCATEGORYSORTING = new Dictionary<string, string>
		{
			{
				"Ladder",
				"ladders"
			},
			{
				"FirePole",
				"ladders"
			},
			{
				"LadderFast",
				"ladders"
			},
			{
				"Tile",
				"tiles"
			},
			{
				"SnowTile",
				"tiles"
			},
			{
				"WoodTile",
				"tiles"
			},
			{
				"GasPermeableMembrane",
				"tiles"
			},
			{
				"MeshTile",
				"tiles"
			},
			{
				"InsulationTile",
				"tiles"
			},
			{
				"PlasticTile",
				"tiles"
			},
			{
				"MetalTile",
				"tiles"
			},
			{
				"GlassTile",
				"tiles"
			},
			{
				"StorageTile",
				"tiles"
			},
			{
				"BunkerTile",
				"tiles"
			},
			{
				"ExteriorWall",
				"tiles"
			},
			{
				"CarpetTile",
				"tiles"
			},
			{
				"ExobaseHeadquarters",
				"printingpods"
			},
			{
				"Door",
				"doors"
			},
			{
				"ManualPressureDoor",
				"doors"
			},
			{
				"PressureDoor",
				"doors"
			},
			{
				"BunkerDoor",
				"doors"
			},
			{
				"StorageLocker",
				"storage"
			},
			{
				"StorageLockerSmart",
				"storage"
			},
			{
				"LiquidReservoir",
				"storage"
			},
			{
				"GasReservoir",
				"storage"
			},
			{
				"ObjectDispenser",
				"storage"
			},
			{
				"TravelTube",
				"transport"
			},
			{
				"TravelTubeEntrance",
				"transport"
			},
			{
				"TravelTubeWallBridge",
				"transport"
			},
			{
				RemoteWorkerDockConfig.ID,
				"operations"
			},
			{
				RemoteWorkTerminalConfig.ID,
				"operations"
			},
			{
				"MineralDeoxidizer",
				"producers"
			},
			{
				"SublimationStation",
				"producers"
			},
			{
				"Oxysconce",
				"producers"
			},
			{
				"Electrolyzer",
				"producers"
			},
			{
				"RustDeoxidizer",
				"producers"
			},
			{
				"AirFilter",
				"scrubbers"
			},
			{
				"CO2Scrubber",
				"scrubbers"
			},
			{
				"AlgaeHabitat",
				"scrubbers"
			},
			{
				"DevGenerator",
				"generators"
			},
			{
				"ManualGenerator",
				"generators"
			},
			{
				"Generator",
				"generators"
			},
			{
				"WoodGasGenerator",
				"generators"
			},
			{
				"HydrogenGenerator",
				"generators"
			},
			{
				"MethaneGenerator",
				"generators"
			},
			{
				"PetroleumGenerator",
				"generators"
			},
			{
				"SteamTurbine",
				"generators"
			},
			{
				"SteamTurbine2",
				"generators"
			},
			{
				"SolarPanel",
				"generators"
			},
			{
				"Wire",
				"wires"
			},
			{
				"WireBridge",
				"wires"
			},
			{
				"HighWattageWire",
				"wires"
			},
			{
				"WireBridgeHighWattage",
				"wires"
			},
			{
				"WireRefined",
				"wires"
			},
			{
				"WireRefinedBridge",
				"wires"
			},
			{
				"WireRefinedHighWattage",
				"wires"
			},
			{
				"WireRefinedBridgeHighWattage",
				"wires"
			},
			{
				"Battery",
				"batteries"
			},
			{
				"BatteryMedium",
				"batteries"
			},
			{
				"BatterySmart",
				"batteries"
			},
			{
				"ElectrobankCharger",
				"electrobankbuildings"
			},
			{
				"SmallElectrobankDischarger",
				"electrobankbuildings"
			},
			{
				"LargeElectrobankDischarger",
				"electrobankbuildings"
			},
			{
				"PowerTransformerSmall",
				"powercontrol"
			},
			{
				"PowerTransformer",
				"powercontrol"
			},
			{
				SwitchConfig.ID,
				"switches"
			},
			{
				LogicPowerRelayConfig.ID,
				"switches"
			},
			{
				TemperatureControlledSwitchConfig.ID,
				"switches"
			},
			{
				PressureSwitchLiquidConfig.ID,
				"switches"
			},
			{
				PressureSwitchGasConfig.ID,
				"switches"
			},
			{
				"MicrobeMusher",
				"cooking"
			},
			{
				"CookingStation",
				"cooking"
			},
			{
				"Deepfryer",
				"cooking"
			},
			{
				"GourmetCookingStation",
				"cooking"
			},
			{
				"SpiceGrinder",
				"cooking"
			},
			{
				"FoodDehydrator",
				"cooking"
			},
			{
				"FoodRehydrator",
				"cooking"
			},
			{
				"PlanterBox",
				"farming"
			},
			{
				"FarmTile",
				"farming"
			},
			{
				"HydroponicFarm",
				"farming"
			},
			{
				"RationBox",
				"storage"
			},
			{
				"Refrigerator",
				"storage"
			},
			{
				"CreatureDeliveryPoint",
				"ranching"
			},
			{
				"CritterDropOff",
				"ranching"
			},
			{
				"CritterPickUp",
				"ranching"
			},
			{
				"FishDeliveryPoint",
				"ranching"
			},
			{
				"CreatureFeeder",
				"ranching"
			},
			{
				"FishFeeder",
				"ranching"
			},
			{
				"MilkFeeder",
				"ranching"
			},
			{
				"EggIncubator",
				"ranching"
			},
			{
				"EggCracker",
				"ranching"
			},
			{
				"CreatureGroundTrap",
				"ranching"
			},
			{
				"CreatureAirTrap",
				"ranching"
			},
			{
				"WaterTrap",
				"ranching"
			},
			{
				"CritterCondo",
				"ranching"
			},
			{
				"UnderwaterCritterCondo",
				"ranching"
			},
			{
				"AirBorneCritterCondo",
				"ranching"
			},
			{
				"Outhouse",
				"washroom"
			},
			{
				"FlushToilet",
				"washroom"
			},
			{
				"WallToilet",
				"washroom"
			},
			{
				ShowerConfig.ID,
				"washroom"
			},
			{
				"GunkEmptier",
				"washroom"
			},
			{
				"LiquidConduit",
				"pipes"
			},
			{
				"InsulatedLiquidConduit",
				"pipes"
			},
			{
				"LiquidConduitRadiant",
				"pipes"
			},
			{
				"LiquidConduitBridge",
				"pipes"
			},
			{
				"ContactConductivePipeBridge",
				"pipes"
			},
			{
				"LiquidVent",
				"pipes"
			},
			{
				"LiquidPump",
				"pumps"
			},
			{
				"LiquidMiniPump",
				"pumps"
			},
			{
				"LiquidPumpingStation",
				"pumps"
			},
			{
				"DevPumpLiquid",
				"pumps"
			},
			{
				"BottleEmptier",
				"valves"
			},
			{
				"LiquidFilter",
				"valves"
			},
			{
				"LiquidConduitPreferentialFlow",
				"valves"
			},
			{
				"LiquidConduitOverflow",
				"valves"
			},
			{
				"LiquidValve",
				"valves"
			},
			{
				"LiquidLogicValve",
				"valves"
			},
			{
				"LiquidLimitValve",
				"valves"
			},
			{
				"LiquidBottler",
				"valves"
			},
			{
				"BottleEmptierConduitLiquid",
				"valves"
			},
			{
				LiquidConduitElementSensorConfig.ID,
				"sensors"
			},
			{
				LiquidConduitDiseaseSensorConfig.ID,
				"sensors"
			},
			{
				LiquidConduitTemperatureSensorConfig.ID,
				"sensors"
			},
			{
				"ModularLaunchpadPortLiquid",
				"buildmenuports"
			},
			{
				"ModularLaunchpadPortLiquidUnloader",
				"buildmenuports"
			},
			{
				"GasConduit",
				"pipes"
			},
			{
				"InsulatedGasConduit",
				"pipes"
			},
			{
				"GasConduitRadiant",
				"pipes"
			},
			{
				"GasConduitBridge",
				"pipes"
			},
			{
				"GasVent",
				"pipes"
			},
			{
				"GasVentHighPressure",
				"pipes"
			},
			{
				"GasPump",
				"pumps"
			},
			{
				"GasMiniPump",
				"pumps"
			},
			{
				"DevPumpGas",
				"pumps"
			},
			{
				"GasBottler",
				"valves"
			},
			{
				"BottleEmptierGas",
				"valves"
			},
			{
				"BottleEmptierConduitGas",
				"valves"
			},
			{
				"GasFilter",
				"valves"
			},
			{
				"GasConduitPreferentialFlow",
				"valves"
			},
			{
				"GasConduitOverflow",
				"valves"
			},
			{
				"GasValve",
				"valves"
			},
			{
				"GasLogicValve",
				"valves"
			},
			{
				"GasLimitValve",
				"valves"
			},
			{
				GasConduitElementSensorConfig.ID,
				"sensors"
			},
			{
				GasConduitDiseaseSensorConfig.ID,
				"sensors"
			},
			{
				GasConduitTemperatureSensorConfig.ID,
				"sensors"
			},
			{
				"ModularLaunchpadPortGas",
				"buildmenuports"
			},
			{
				"ModularLaunchpadPortGasUnloader",
				"buildmenuports"
			},
			{
				"Compost",
				"organic"
			},
			{
				"FertilizerMaker",
				"organic"
			},
			{
				"AlgaeDistillery",
				"organic"
			},
			{
				"EthanolDistillery",
				"organic"
			},
			{
				"SludgePress",
				"organic"
			},
			{
				"MilkFatSeparator",
				"organic"
			},
			{
				"MilkPress",
				"organic"
			},
			{
				"IceKettle",
				"materials"
			},
			{
				"WaterPurifier",
				"materials"
			},
			{
				"Desalinator",
				"materials"
			},
			{
				"RockCrusher",
				"materials"
			},
			{
				"Kiln",
				"materials"
			},
			{
				"MetalRefinery",
				"materials"
			},
			{
				"GlassForge",
				"materials"
			},
			{
				"OilRefinery",
				"oil"
			},
			{
				"Polymerizer",
				"oil"
			},
			{
				"OxyliteRefinery",
				"advanced"
			},
			{
				"SupermaterialRefinery",
				"advanced"
			},
			{
				"DiamondPress",
				"advanced"
			},
			{
				"Chlorinator",
				"advanced"
			},
			{
				"WashBasin",
				"hygiene"
			},
			{
				"WashSink",
				"hygiene"
			},
			{
				"HandSanitizer",
				"hygiene"
			},
			{
				"DecontaminationShower",
				"hygiene"
			},
			{
				"Apothecary",
				"medical"
			},
			{
				"DoctorStation",
				"medical"
			},
			{
				"AdvancedDoctorStation",
				"medical"
			},
			{
				"MedicalCot",
				"medical"
			},
			{
				"DevLifeSupport",
				"medical"
			},
			{
				"MassageTable",
				"wellness"
			},
			{
				"Grave",
				"wellness"
			},
			{
				"OilChanger",
				"wellness"
			},
			{
				"Bed",
				"beds"
			},
			{
				"LuxuryBed",
				"beds"
			},
			{
				LadderBedConfig.ID,
				"beds"
			},
			{
				"FloorLamp",
				"lights"
			},
			{
				"CeilingLight",
				"lights"
			},
			{
				"SunLamp",
				"lights"
			},
			{
				"DevLightGenerator",
				"lights"
			},
			{
				"MercuryCeilingLight",
				"lights"
			},
			{
				"DiningTable",
				"dining"
			},
			{
				"WaterCooler",
				"recreation"
			},
			{
				"Phonobox",
				"recreation"
			},
			{
				"ArcadeMachine",
				"recreation"
			},
			{
				"EspressoMachine",
				"recreation"
			},
			{
				"HotTub",
				"recreation"
			},
			{
				"MechanicalSurfboard",
				"recreation"
			},
			{
				"Sauna",
				"recreation"
			},
			{
				"Juicer",
				"recreation"
			},
			{
				"SodaFountain",
				"recreation"
			},
			{
				"BeachChair",
				"recreation"
			},
			{
				"VerticalWindTunnel",
				"recreation"
			},
			{
				"Telephone",
				"recreation"
			},
			{
				"FlowerVase",
				"decor"
			},
			{
				"FlowerVaseWall",
				"decor"
			},
			{
				"FlowerVaseHanging",
				"decor"
			},
			{
				"FlowerVaseHangingFancy",
				"decor"
			},
			{
				PixelPackConfig.ID,
				"decor"
			},
			{
				"SmallSculpture",
				"decor"
			},
			{
				"Sculpture",
				"decor"
			},
			{
				"IceSculpture",
				"decor"
			},
			{
				"MarbleSculpture",
				"decor"
			},
			{
				"MetalSculpture",
				"decor"
			},
			{
				"WoodSculpture",
				"decor"
			},
			{
				"CrownMoulding",
				"decor"
			},
			{
				"CornerMoulding",
				"decor"
			},
			{
				"Canvas",
				"decor"
			},
			{
				"CanvasWide",
				"decor"
			},
			{
				"CanvasTall",
				"decor"
			},
			{
				"ItemPedestal",
				"decor"
			},
			{
				"ParkSign",
				"decor"
			},
			{
				"MonumentBottom",
				"decor"
			},
			{
				"MonumentMiddle",
				"decor"
			},
			{
				"MonumentTop",
				"decor"
			},
			{
				"ResearchCenter",
				"research"
			},
			{
				"AdvancedResearchCenter",
				"research"
			},
			{
				"GeoTuner",
				"research"
			},
			{
				"NuclearResearchCenter",
				"research"
			},
			{
				"OrbitalResearchCenter",
				"research"
			},
			{
				"CosmicResearchCenter",
				"research"
			},
			{
				"DLC1CosmicResearchCenter",
				"research"
			},
			{
				"DataMiner",
				"research"
			},
			{
				"ArtifactAnalysisStation",
				"archaeology"
			},
			{
				"MissileFabricator",
				"meteordefense"
			},
			{
				"AstronautTrainingCenter",
				"exploration"
			},
			{
				"PowerControlStation",
				"industrialstation"
			},
			{
				"ResetSkillsStation",
				"industrialstation"
			},
			{
				"RoleStation",
				"workstations"
			},
			{
				"RanchStation",
				"ranching"
			},
			{
				"ShearingStation",
				"ranching"
			},
			{
				"MilkingStation",
				"ranching"
			},
			{
				"FarmStation",
				"farming"
			},
			{
				"GeneticAnalysisStation",
				"farming"
			},
			{
				"CraftingTable",
				"manufacturing"
			},
			{
				"AdvancedCraftingTable",
				"manufacturing"
			},
			{
				"ClothingFabricator",
				"manufacturing"
			},
			{
				"ClothingAlterationStation",
				"manufacturing"
			},
			{
				"SuitFabricator",
				"manufacturing"
			},
			{
				"OxygenMaskMarker",
				"equipment"
			},
			{
				"OxygenMaskLocker",
				"equipment"
			},
			{
				"SuitMarker",
				"equipment"
			},
			{
				"SuitLocker",
				"equipment"
			},
			{
				"JetSuitMarker",
				"equipment"
			},
			{
				"JetSuitLocker",
				"equipment"
			},
			{
				"MissileLauncher",
				"missiles"
			},
			{
				"LeadSuitMarker",
				"equipment"
			},
			{
				"LeadSuitLocker",
				"equipment"
			},
			{
				"Campfire",
				"temperature"
			},
			{
				"DevHeater",
				"temperature"
			},
			{
				"SpaceHeater",
				"temperature"
			},
			{
				"LiquidHeater",
				"temperature"
			},
			{
				"LiquidConditioner",
				"temperature"
			},
			{
				"LiquidCooledFan",
				"temperature"
			},
			{
				"IceCooledFan",
				"temperature"
			},
			{
				"IceMachine",
				"temperature"
			},
			{
				"AirConditioner",
				"temperature"
			},
			{
				"ThermalBlock",
				"temperature"
			},
			{
				"OreScrubber",
				"sanitation"
			},
			{
				"OilWellCap",
				"oil"
			},
			{
				"SweepBotStation",
				"sanitation"
			},
			{
				"LogicWire",
				"wires"
			},
			{
				"LogicWireBridge",
				"wires"
			},
			{
				"LogicRibbon",
				"wires"
			},
			{
				"LogicRibbonBridge",
				"wires"
			},
			{
				LogicRibbonReaderConfig.ID,
				"wires"
			},
			{
				LogicRibbonWriterConfig.ID,
				"wires"
			},
			{
				"LogicDuplicantSensor",
				"sensors"
			},
			{
				LogicPressureSensorGasConfig.ID,
				"sensors"
			},
			{
				LogicPressureSensorLiquidConfig.ID,
				"sensors"
			},
			{
				LogicTemperatureSensorConfig.ID,
				"sensors"
			},
			{
				LogicLightSensorConfig.ID,
				"sensors"
			},
			{
				LogicWattageSensorConfig.ID,
				"sensors"
			},
			{
				LogicTimeOfDaySensorConfig.ID,
				"sensors"
			},
			{
				LogicTimerSensorConfig.ID,
				"sensors"
			},
			{
				LogicDiseaseSensorConfig.ID,
				"sensors"
			},
			{
				LogicElementSensorGasConfig.ID,
				"sensors"
			},
			{
				LogicElementSensorLiquidConfig.ID,
				"sensors"
			},
			{
				LogicCritterCountSensorConfig.ID,
				"sensors"
			},
			{
				LogicRadiationSensorConfig.ID,
				"sensors"
			},
			{
				LogicHEPSensorConfig.ID,
				"sensors"
			},
			{
				CometDetectorConfig.ID,
				"sensors"
			},
			{
				LogicCounterConfig.ID,
				"logicmanager"
			},
			{
				"Checkpoint",
				"logicmanager"
			},
			{
				LogicAlarmConfig.ID,
				"logicmanager"
			},
			{
				LogicHammerConfig.ID,
				"logicaudio"
			},
			{
				LogicSwitchConfig.ID,
				"switches"
			},
			{
				"FloorSwitch",
				"switches"
			},
			{
				"LogicGateNOT",
				"logicgates"
			},
			{
				"LogicGateAND",
				"logicgates"
			},
			{
				"LogicGateOR",
				"logicgates"
			},
			{
				"LogicGateBUFFER",
				"logicgates"
			},
			{
				"LogicGateFILTER",
				"logicgates"
			},
			{
				"LogicGateXOR",
				"logicgates"
			},
			{
				LogicMemoryConfig.ID,
				"logicgates"
			},
			{
				"LogicGateMultiplexer",
				"logicgates"
			},
			{
				"LogicGateDemultiplexer",
				"logicgates"
			},
			{
				"LogicInterasteroidSender",
				"transmissions"
			},
			{
				"LogicInterasteroidReceiver",
				"transmissions"
			},
			{
				"SolidConduit",
				"conveyancestructures"
			},
			{
				"SolidConduitBridge",
				"conveyancestructures"
			},
			{
				"SolidConduitInbox",
				"conveyancestructures"
			},
			{
				"SolidConduitOutbox",
				"conveyancestructures"
			},
			{
				"SolidFilter",
				"conveyancestructures"
			},
			{
				"SolidVent",
				"conveyancestructures"
			},
			{
				"DevPumpSolid",
				"pumps"
			},
			{
				"SolidLogicValve",
				"valves"
			},
			{
				"SolidLimitValve",
				"valves"
			},
			{
				SolidConduitDiseaseSensorConfig.ID,
				"sensors"
			},
			{
				SolidConduitElementSensorConfig.ID,
				"sensors"
			},
			{
				SolidConduitTemperatureSensorConfig.ID,
				"sensors"
			},
			{
				"AutoMiner",
				"automated"
			},
			{
				"SolidTransferArm",
				"automated"
			},
			{
				"ModularLaunchpadPortSolid",
				"buildmenuports"
			},
			{
				"ModularLaunchpadPortSolidUnloader",
				"buildmenuports"
			},
			{
				"Telescope",
				"telescopes"
			},
			{
				"ClusterTelescope",
				"telescopes"
			},
			{
				"ClusterTelescopeEnclosed",
				"telescopes"
			},
			{
				"LaunchPad",
				"rocketstructures"
			},
			{
				"Gantry",
				"rocketstructures"
			},
			{
				"ModularLaunchpadPortBridge",
				"rocketstructures"
			},
			{
				"RailGun",
				"fittings"
			},
			{
				"RailGunPayloadOpener",
				"fittings"
			},
			{
				"LandingBeacon",
				"rocketnav"
			},
			{
				"SteamEngine",
				"engines"
			},
			{
				"KeroseneEngine",
				"engines"
			},
			{
				"HydrogenEngine",
				"engines"
			},
			{
				"SolidBooster",
				"engines"
			},
			{
				"LiquidFuelTank",
				"tanks"
			},
			{
				"OxidizerTank",
				"tanks"
			},
			{
				"OxidizerTankLiquid",
				"tanks"
			},
			{
				"CargoBay",
				"cargo"
			},
			{
				"GasCargoBay",
				"cargo"
			},
			{
				"LiquidCargoBay",
				"cargo"
			},
			{
				"SpecialCargoBay",
				"cargo"
			},
			{
				"CommandModule",
				"rocketnav"
			},
			{
				RocketControlStationConfig.ID,
				"rocketnav"
			},
			{
				LogicClusterLocationSensorConfig.ID,
				"rocketnav"
			},
			{
				"MissionControl",
				"rocketnav"
			},
			{
				"MissionControlCluster",
				"rocketnav"
			},
			{
				"RoboPilotCommandModule",
				"rocketnav"
			},
			{
				"TouristModule",
				"module"
			},
			{
				"ResearchModule",
				"module"
			},
			{
				"RocketInteriorPowerPlug",
				"fittings"
			},
			{
				"RocketInteriorLiquidInput",
				"fittings"
			},
			{
				"RocketInteriorLiquidOutput",
				"fittings"
			},
			{
				"RocketInteriorGasInput",
				"fittings"
			},
			{
				"RocketInteriorGasOutput",
				"fittings"
			},
			{
				"RocketInteriorSolidInput",
				"fittings"
			},
			{
				"RocketInteriorSolidOutput",
				"fittings"
			},
			{
				"ManualHighEnergyParticleSpawner",
				"producers"
			},
			{
				"HighEnergyParticleSpawner",
				"producers"
			},
			{
				"DevHEPSpawner",
				"producers"
			},
			{
				"HighEnergyParticleRedirector",
				"transmissions"
			},
			{
				"HEPBattery",
				"batteries"
			},
			{
				"HEPBridgeTile",
				"transmissions"
			},
			{
				"NuclearReactor",
				"producers"
			},
			{
				"UraniumCentrifuge",
				"producers"
			},
			{
				"RadiationLight",
				"producers"
			},
			{
				"DevRadiationGenerator",
				"producers"
			}
		};

		// Token: 0x04009991 RID: 39313
		public static List<PlanScreen.PlanInfo> PLANORDER = new List<PlanScreen.PlanInfo>
		{
			new PlanScreen.PlanInfo(new HashedString("Base"), false, new List<string>
			{
				"Ladder",
				"FirePole",
				"LadderFast",
				"Tile",
				"SnowTile",
				"WoodTile",
				"GasPermeableMembrane",
				"MeshTile",
				"InsulationTile",
				"PlasticTile",
				"MetalTile",
				"GlassTile",
				"StorageTile",
				"BunkerTile",
				"CarpetTile",
				"ExteriorWall",
				"ExobaseHeadquarters",
				"Door",
				"ManualPressureDoor",
				"PressureDoor",
				"BunkerDoor",
				"StorageLocker",
				"StorageLockerSmart",
				"LiquidReservoir",
				"GasReservoir",
				"ObjectDispenser",
				"TravelTube",
				"TravelTubeEntrance",
				"TravelTubeWallBridge"
			}, null, null),
			new PlanScreen.PlanInfo(new HashedString("Oxygen"), false, new List<string>
			{
				"MineralDeoxidizer",
				"SublimationStation",
				"Oxysconce",
				"AlgaeHabitat",
				"AirFilter",
				"CO2Scrubber",
				"Electrolyzer",
				"RustDeoxidizer"
			}, null, null),
			new PlanScreen.PlanInfo(new HashedString("Power"), false, new List<string>
			{
				"DevGenerator",
				"ManualGenerator",
				"Generator",
				"WoodGasGenerator",
				"HydrogenGenerator",
				"MethaneGenerator",
				"PetroleumGenerator",
				"SteamTurbine",
				"SteamTurbine2",
				"SolarPanel",
				"Wire",
				"WireBridge",
				"HighWattageWire",
				"WireBridgeHighWattage",
				"WireRefined",
				"WireRefinedBridge",
				"WireRefinedHighWattage",
				"WireRefinedBridgeHighWattage",
				"Battery",
				"BatteryMedium",
				"BatterySmart",
				"ElectrobankCharger",
				"SmallElectrobankDischarger",
				"LargeElectrobankDischarger",
				"PowerTransformerSmall",
				"PowerTransformer",
				SwitchConfig.ID,
				LogicPowerRelayConfig.ID,
				TemperatureControlledSwitchConfig.ID,
				PressureSwitchLiquidConfig.ID,
				PressureSwitchGasConfig.ID
			}, null, null),
			new PlanScreen.PlanInfo(new HashedString("Food"), false, new List<string>
			{
				"MicrobeMusher",
				"CookingStation",
				"Deepfryer",
				"GourmetCookingStation",
				"SpiceGrinder",
				"FoodDehydrator",
				"FoodRehydrator",
				"PlanterBox",
				"FarmTile",
				"HydroponicFarm",
				"RationBox",
				"Refrigerator",
				"CreatureDeliveryPoint",
				"CritterPickUp",
				"CritterDropOff",
				"FishDeliveryPoint",
				"CreatureFeeder",
				"FishFeeder",
				"MilkFeeder",
				"EggIncubator",
				"EggCracker",
				"CreatureGroundTrap",
				"WaterTrap",
				"CreatureAirTrap",
				"CritterCondo",
				"UnderwaterCritterCondo",
				"AirBorneCritterCondo"
			}, null, null),
			new PlanScreen.PlanInfo(new HashedString("Plumbing"), false, new List<string>
			{
				"DevPumpLiquid",
				"Outhouse",
				"FlushToilet",
				"WallToilet",
				ShowerConfig.ID,
				"GunkEmptier",
				"LiquidPumpingStation",
				"BottleEmptier",
				"BottleEmptierConduitLiquid",
				"LiquidBottler",
				"LiquidConduit",
				"InsulatedLiquidConduit",
				"LiquidConduitRadiant",
				"LiquidConduitBridge",
				"LiquidConduitPreferentialFlow",
				"LiquidConduitOverflow",
				"LiquidPump",
				"LiquidMiniPump",
				"LiquidVent",
				"LiquidFilter",
				"LiquidValve",
				"LiquidLogicValve",
				"LiquidLimitValve",
				LiquidConduitElementSensorConfig.ID,
				LiquidConduitDiseaseSensorConfig.ID,
				LiquidConduitTemperatureSensorConfig.ID,
				"ModularLaunchpadPortLiquid",
				"ModularLaunchpadPortLiquidUnloader",
				"ContactConductivePipeBridge"
			}, null, null),
			new PlanScreen.PlanInfo(new HashedString("HVAC"), false, new List<string>
			{
				"DevPumpGas",
				"GasConduit",
				"InsulatedGasConduit",
				"GasConduitRadiant",
				"GasConduitBridge",
				"GasConduitPreferentialFlow",
				"GasConduitOverflow",
				"GasPump",
				"GasMiniPump",
				"GasVent",
				"GasVentHighPressure",
				"GasFilter",
				"GasValve",
				"GasLogicValve",
				"GasLimitValve",
				"GasBottler",
				"BottleEmptierGas",
				"BottleEmptierConduitGas",
				"ModularLaunchpadPortGas",
				"ModularLaunchpadPortGasUnloader",
				GasConduitElementSensorConfig.ID,
				GasConduitDiseaseSensorConfig.ID,
				GasConduitTemperatureSensorConfig.ID
			}, null, null),
			new PlanScreen.PlanInfo(new HashedString("Refining"), false, new List<string>
			{
				"Compost",
				"WaterPurifier",
				"Desalinator",
				"FertilizerMaker",
				"AlgaeDistillery",
				"EthanolDistillery",
				"RockCrusher",
				"Kiln",
				"SludgePress",
				"MetalRefinery",
				"GlassForge",
				"OilRefinery",
				"Polymerizer",
				"OxyliteRefinery",
				"Chlorinator",
				"SupermaterialRefinery",
				"DiamondPress",
				"MilkFatSeparator",
				"MilkPress"
			}, null, null),
			new PlanScreen.PlanInfo(new HashedString("Medical"), false, new List<string>
			{
				"DevLifeSupport",
				"WashBasin",
				"WashSink",
				"HandSanitizer",
				"DecontaminationShower",
				"OilChanger",
				"Apothecary",
				"DoctorStation",
				"AdvancedDoctorStation",
				"MedicalCot",
				"MassageTable",
				"Grave"
			}, null, null),
			new PlanScreen.PlanInfo(new HashedString("Furniture"), false, new List<string>
			{
				"Bed",
				"LuxuryBed",
				LadderBedConfig.ID,
				"FloorLamp",
				"CeilingLight",
				"SunLamp",
				"DevLightGenerator",
				"MercuryCeilingLight",
				"DiningTable",
				"WaterCooler",
				"Phonobox",
				"ArcadeMachine",
				"EspressoMachine",
				"HotTub",
				"MechanicalSurfboard",
				"Sauna",
				"Juicer",
				"SodaFountain",
				"BeachChair",
				"VerticalWindTunnel",
				PixelPackConfig.ID,
				"Telephone",
				"FlowerVase",
				"FlowerVaseWall",
				"FlowerVaseHanging",
				"FlowerVaseHangingFancy",
				"SmallSculpture",
				"Sculpture",
				"IceSculpture",
				"WoodSculpture",
				"MarbleSculpture",
				"MetalSculpture",
				"CrownMoulding",
				"CornerMoulding",
				"Canvas",
				"CanvasWide",
				"CanvasTall",
				"ItemPedestal",
				"MonumentBottom",
				"MonumentMiddle",
				"MonumentTop",
				"ParkSign"
			}, null, null),
			new PlanScreen.PlanInfo(new HashedString("Equipment"), false, new List<string>
			{
				"ResearchCenter",
				"AdvancedResearchCenter",
				"NuclearResearchCenter",
				"OrbitalResearchCenter",
				"CosmicResearchCenter",
				"DLC1CosmicResearchCenter",
				"Telescope",
				"GeoTuner",
				"DataMiner",
				"PowerControlStation",
				"FarmStation",
				"GeneticAnalysisStation",
				"RanchStation",
				"ShearingStation",
				"MilkingStation",
				"RoleStation",
				"ResetSkillsStation",
				"ArtifactAnalysisStation",
				RemoteWorkerDockConfig.ID,
				RemoteWorkTerminalConfig.ID,
				"MissileFabricator",
				"CraftingTable",
				"AdvancedCraftingTable",
				"ClothingFabricator",
				"ClothingAlterationStation",
				"SuitFabricator",
				"OxygenMaskMarker",
				"OxygenMaskLocker",
				"SuitMarker",
				"SuitLocker",
				"JetSuitMarker",
				"JetSuitLocker",
				"LeadSuitMarker",
				"LeadSuitLocker",
				"AstronautTrainingCenter"
			}, null, null),
			new PlanScreen.PlanInfo(new HashedString("Utilities"), true, new List<string>
			{
				"Campfire",
				"DevHeater",
				"IceKettle",
				"SpaceHeater",
				"LiquidHeater",
				"LiquidCooledFan",
				"IceCooledFan",
				"IceMachine",
				"AirConditioner",
				"LiquidConditioner",
				"OreScrubber",
				"OilWellCap",
				"ThermalBlock",
				"SweepBotStation"
			}, null, null),
			new PlanScreen.PlanInfo(new HashedString("Automation"), true, new List<string>
			{
				"LogicWire",
				"LogicWireBridge",
				"LogicRibbon",
				"LogicRibbonBridge",
				LogicSwitchConfig.ID,
				"LogicDuplicantSensor",
				LogicPressureSensorGasConfig.ID,
				LogicPressureSensorLiquidConfig.ID,
				LogicTemperatureSensorConfig.ID,
				LogicLightSensorConfig.ID,
				LogicWattageSensorConfig.ID,
				LogicTimeOfDaySensorConfig.ID,
				LogicTimerSensorConfig.ID,
				LogicDiseaseSensorConfig.ID,
				LogicElementSensorGasConfig.ID,
				LogicElementSensorLiquidConfig.ID,
				LogicCritterCountSensorConfig.ID,
				LogicRadiationSensorConfig.ID,
				LogicHEPSensorConfig.ID,
				LogicCounterConfig.ID,
				LogicAlarmConfig.ID,
				LogicHammerConfig.ID,
				"LogicInterasteroidSender",
				"LogicInterasteroidReceiver",
				LogicRibbonReaderConfig.ID,
				LogicRibbonWriterConfig.ID,
				"FloorSwitch",
				"Checkpoint",
				CometDetectorConfig.ID,
				"LogicGateNOT",
				"LogicGateAND",
				"LogicGateOR",
				"LogicGateBUFFER",
				"LogicGateFILTER",
				"LogicGateXOR",
				LogicMemoryConfig.ID,
				"LogicGateMultiplexer",
				"LogicGateDemultiplexer"
			}, null, null),
			new PlanScreen.PlanInfo(new HashedString("Conveyance"), true, new List<string>
			{
				"DevPumpSolid",
				"SolidTransferArm",
				"SolidConduit",
				"SolidConduitBridge",
				"SolidConduitInbox",
				"SolidConduitOutbox",
				"SolidFilter",
				"SolidVent",
				"SolidLogicValve",
				"SolidLimitValve",
				SolidConduitDiseaseSensorConfig.ID,
				SolidConduitElementSensorConfig.ID,
				SolidConduitTemperatureSensorConfig.ID,
				"AutoMiner",
				"ModularLaunchpadPortSolid",
				"ModularLaunchpadPortSolidUnloader"
			}, null, null),
			new PlanScreen.PlanInfo(new HashedString("Rocketry"), true, new List<string>
			{
				"ClusterTelescope",
				"ClusterTelescopeEnclosed",
				"MissionControl",
				"MissionControlCluster",
				"LaunchPad",
				"Gantry",
				"SteamEngine",
				"KeroseneEngine",
				"SolidBooster",
				"LiquidFuelTank",
				"OxidizerTank",
				"OxidizerTankLiquid",
				"CargoBay",
				"GasCargoBay",
				"LiquidCargoBay",
				"CommandModule",
				"RoboPilotCommandModule",
				"TouristModule",
				"ResearchModule",
				"SpecialCargoBay",
				"HydrogenEngine",
				RocketControlStationConfig.ID,
				"RocketInteriorPowerPlug",
				"RocketInteriorLiquidInput",
				"RocketInteriorLiquidOutput",
				"RocketInteriorGasInput",
				"RocketInteriorGasOutput",
				"RocketInteriorSolidInput",
				"RocketInteriorSolidOutput",
				LogicClusterLocationSensorConfig.ID,
				"RailGun",
				"RailGunPayloadOpener",
				"LandingBeacon",
				"MissileLauncher",
				"ModularLaunchpadPortBridge"
			}, null, null),
			new PlanScreen.PlanInfo(new HashedString("HEP"), true, new List<string>
			{
				"RadiationLight",
				"ManualHighEnergyParticleSpawner",
				"NuclearReactor",
				"UraniumCentrifuge",
				"HighEnergyParticleSpawner",
				"DevHEPSpawner",
				"HighEnergyParticleRedirector",
				"HEPBattery",
				"HEPBridgeTile",
				"DevRadiationGenerator"
			}, DlcManager.EXPANSION1, null)
		};

		// Token: 0x04009992 RID: 39314
		public static List<Type> COMPONENT_DESCRIPTION_ORDER = new List<Type>
		{
			typeof(BottleEmptier),
			typeof(CookingStation),
			typeof(GourmetCookingStation),
			typeof(RoleStation),
			typeof(ResearchCenter),
			typeof(NuclearResearchCenter),
			typeof(LiquidCooledFan),
			typeof(HandSanitizer),
			typeof(HandSanitizer.Work),
			typeof(PlantAirConditioner),
			typeof(Clinic),
			typeof(BuildingElementEmitter),
			typeof(ElementConverter),
			typeof(ElementConsumer),
			typeof(PassiveElementConsumer),
			typeof(TinkerStation),
			typeof(EnergyConsumer),
			typeof(AirConditioner),
			typeof(Storage),
			typeof(Battery),
			typeof(AirFilter),
			typeof(FlushToilet),
			typeof(Toilet),
			typeof(EnergyGenerator),
			typeof(MassageTable),
			typeof(Shower),
			typeof(Ownable),
			typeof(PlantablePlot),
			typeof(RelaxationPoint),
			typeof(BuildingComplete),
			typeof(Building),
			typeof(BuildingPreview),
			typeof(BuildingUnderConstruction),
			typeof(Crop),
			typeof(Growing),
			typeof(Equippable),
			typeof(ColdBreather),
			typeof(ResearchPointObject),
			typeof(SuitTank),
			typeof(IlluminationVulnerable),
			typeof(TemperatureVulnerable),
			typeof(ExternalTemperatureMonitor),
			typeof(CritterTemperatureMonitor),
			typeof(PressureVulnerable),
			typeof(SubmersionMonitor),
			typeof(BatterySmart),
			typeof(Compost),
			typeof(Refrigerator),
			typeof(Bed),
			typeof(OreScrubber),
			typeof(OreScrubber.Work),
			typeof(MinimumOperatingTemperature),
			typeof(RoomTracker),
			typeof(EnergyConsumerSelfSustaining),
			typeof(ArcadeMachine),
			typeof(Telescope),
			typeof(EspressoMachine),
			typeof(JetSuitTank),
			typeof(Phonobox),
			typeof(ArcadeMachine),
			typeof(BeachChair),
			typeof(Sauna),
			typeof(VerticalWindTunnel),
			typeof(HotTub),
			typeof(Juicer),
			typeof(SodaFountain),
			typeof(MechanicalSurfboard),
			typeof(BottleEmptier),
			typeof(AccessControl),
			typeof(GammaRayOven),
			typeof(Reactor),
			typeof(HighEnergyParticlePort),
			typeof(LeadSuitTank),
			typeof(ActiveParticleConsumer.Def),
			typeof(WaterCooler),
			typeof(Edible),
			typeof(PlantableSeed),
			typeof(SicknessTrigger),
			typeof(MedicinalPill),
			typeof(SeedProducer),
			typeof(Geyser),
			typeof(SpaceHeater),
			typeof(Overheatable),
			typeof(CreatureCalorieMonitor.Def),
			typeof(LureableMonitor.Def),
			typeof(CropSleepingMonitor.Def),
			typeof(FertilizationMonitor.Def),
			typeof(IrrigationMonitor.Def),
			typeof(ScaleGrowthMonitor.Def),
			typeof(TravelTubeEntrance.Work),
			typeof(ToiletWorkableUse),
			typeof(ReceptacleMonitor),
			typeof(Light2D),
			typeof(Ladder),
			typeof(SimCellOccupier),
			typeof(Vent),
			typeof(LogicPorts),
			typeof(Capturable),
			typeof(Trappable),
			typeof(SpaceArtifact),
			typeof(MessStation),
			typeof(PlantElementEmitter),
			typeof(Radiator),
			typeof(DecorProvider)
		};

		// Token: 0x02002267 RID: 8807
		public class PHARMACY
		{
			// Token: 0x02002268 RID: 8808
			public class FABRICATIONTIME
			{
				// Token: 0x04009993 RID: 39315
				public const float TIER0 = 50f;

				// Token: 0x04009994 RID: 39316
				public const float TIER1 = 100f;

				// Token: 0x04009995 RID: 39317
				public const float TIER2 = 200f;
			}
		}

		// Token: 0x02002269 RID: 8809
		public class NUCLEAR_REACTOR
		{
			// Token: 0x0200226A RID: 8810
			public class REACTOR_MASSES
			{
				// Token: 0x04009996 RID: 39318
				public const float MIN = 1f;

				// Token: 0x04009997 RID: 39319
				public const float MAX = 10f;
			}
		}

		// Token: 0x0200226B RID: 8811
		public class OVERPRESSURE
		{
			// Token: 0x04009998 RID: 39320
			public const float TIER0 = 1.8f;
		}

		// Token: 0x0200226C RID: 8812
		public class OVERHEAT_TEMPERATURES
		{
			// Token: 0x04009999 RID: 39321
			public const float LOW_3 = 10f;

			// Token: 0x0400999A RID: 39322
			public const float LOW_2 = 328.15f;

			// Token: 0x0400999B RID: 39323
			public const float LOW_1 = 338.15f;

			// Token: 0x0400999C RID: 39324
			public const float NORMAL = 348.15f;

			// Token: 0x0400999D RID: 39325
			public const float HIGH_1 = 363.15f;

			// Token: 0x0400999E RID: 39326
			public const float HIGH_2 = 398.15f;

			// Token: 0x0400999F RID: 39327
			public const float HIGH_3 = 1273.15f;

			// Token: 0x040099A0 RID: 39328
			public const float HIGH_4 = 2273.15f;
		}

		// Token: 0x0200226D RID: 8813
		public class OVERHEAT_MATERIAL_MOD
		{
			// Token: 0x040099A1 RID: 39329
			public const float LOW_3 = -200f;

			// Token: 0x040099A2 RID: 39330
			public const float LOW_2 = -20f;

			// Token: 0x040099A3 RID: 39331
			public const float LOW_1 = -10f;

			// Token: 0x040099A4 RID: 39332
			public const float NORMAL = 0f;

			// Token: 0x040099A5 RID: 39333
			public const float HIGH_1 = 15f;

			// Token: 0x040099A6 RID: 39334
			public const float HIGH_2 = 50f;

			// Token: 0x040099A7 RID: 39335
			public const float HIGH_3 = 200f;

			// Token: 0x040099A8 RID: 39336
			public const float HIGH_4 = 500f;

			// Token: 0x040099A9 RID: 39337
			public const float HIGH_5 = 900f;
		}

		// Token: 0x0200226E RID: 8814
		public class DECOR_MATERIAL_MOD
		{
			// Token: 0x040099AA RID: 39338
			public const float NORMAL = 0f;

			// Token: 0x040099AB RID: 39339
			public const float HIGH_1 = 0.1f;

			// Token: 0x040099AC RID: 39340
			public const float HIGH_2 = 0.2f;

			// Token: 0x040099AD RID: 39341
			public const float HIGH_3 = 0.5f;

			// Token: 0x040099AE RID: 39342
			public const float HIGH_4 = 1f;
		}

		// Token: 0x0200226F RID: 8815
		public class CONSTRUCTION_MASS_KG
		{
			// Token: 0x040099AF RID: 39343
			public static readonly float[] TIER_TINY = new float[]
			{
				5f
			};

			// Token: 0x040099B0 RID: 39344
			public static readonly float[] TIER0 = new float[]
			{
				25f
			};

			// Token: 0x040099B1 RID: 39345
			public static readonly float[] TIER1 = new float[]
			{
				50f
			};

			// Token: 0x040099B2 RID: 39346
			public static readonly float[] TIER2 = new float[]
			{
				100f
			};

			// Token: 0x040099B3 RID: 39347
			public static readonly float[] TIER3 = new float[]
			{
				200f
			};

			// Token: 0x040099B4 RID: 39348
			public static readonly float[] TIER4 = new float[]
			{
				400f
			};

			// Token: 0x040099B5 RID: 39349
			public static readonly float[] TIER5 = new float[]
			{
				800f
			};

			// Token: 0x040099B6 RID: 39350
			public static readonly float[] TIER6 = new float[]
			{
				1200f
			};

			// Token: 0x040099B7 RID: 39351
			public static readonly float[] TIER7 = new float[]
			{
				2000f
			};
		}

		// Token: 0x02002270 RID: 8816
		public class ROCKETRY_MASS_KG
		{
			// Token: 0x040099B8 RID: 39352
			public static float[] COMMAND_MODULE_MASS = new float[]
			{
				200f
			};

			// Token: 0x040099B9 RID: 39353
			public static float[] CARGO_MASS = new float[]
			{
				1000f
			};

			// Token: 0x040099BA RID: 39354
			public static float[] CARGO_MASS_SMALL = new float[]
			{
				400f
			};

			// Token: 0x040099BB RID: 39355
			public static float[] FUEL_TANK_DRY_MASS = new float[]
			{
				100f
			};

			// Token: 0x040099BC RID: 39356
			public static float[] FUEL_TANK_WET_MASS = new float[]
			{
				900f
			};

			// Token: 0x040099BD RID: 39357
			public static float[] FUEL_TANK_WET_MASS_SMALL = new float[]
			{
				300f
			};

			// Token: 0x040099BE RID: 39358
			public static float[] FUEL_TANK_WET_MASS_GAS = new float[]
			{
				100f
			};

			// Token: 0x040099BF RID: 39359
			public static float[] FUEL_TANK_WET_MASS_GAS_LARGE = new float[]
			{
				150f
			};

			// Token: 0x040099C0 RID: 39360
			public static float[] OXIDIZER_TANK_OXIDIZER_MASS = new float[]
			{
				900f
			};

			// Token: 0x040099C1 RID: 39361
			public static float[] ENGINE_MASS_SMALL = new float[]
			{
				200f
			};

			// Token: 0x040099C2 RID: 39362
			public static float[] ENGINE_MASS_LARGE = new float[]
			{
				500f
			};

			// Token: 0x040099C3 RID: 39363
			public static float[] NOSE_CONE_TIER1 = new float[]
			{
				200f,
				100f
			};

			// Token: 0x040099C4 RID: 39364
			public static float[] NOSE_CONE_TIER2 = new float[]
			{
				400f,
				200f
			};

			// Token: 0x040099C5 RID: 39365
			public static float[] HOLLOW_TIER1 = new float[]
			{
				200f
			};

			// Token: 0x040099C6 RID: 39366
			public static float[] HOLLOW_TIER2 = new float[]
			{
				400f
			};

			// Token: 0x040099C7 RID: 39367
			public static float[] HOLLOW_TIER3 = new float[]
			{
				800f
			};

			// Token: 0x040099C8 RID: 39368
			public static float[] DENSE_TIER0 = new float[]
			{
				200f
			};

			// Token: 0x040099C9 RID: 39369
			public static float[] DENSE_TIER1 = new float[]
			{
				500f
			};

			// Token: 0x040099CA RID: 39370
			public static float[] DENSE_TIER2 = new float[]
			{
				1000f
			};

			// Token: 0x040099CB RID: 39371
			public static float[] DENSE_TIER3 = new float[]
			{
				2000f
			};
		}

		// Token: 0x02002271 RID: 8817
		public class ENERGY_CONSUMPTION_WHEN_ACTIVE
		{
			// Token: 0x040099CC RID: 39372
			public const float TIER0 = 0f;

			// Token: 0x040099CD RID: 39373
			public const float TIER1 = 5f;

			// Token: 0x040099CE RID: 39374
			public const float TIER2 = 60f;

			// Token: 0x040099CF RID: 39375
			public const float TIER3 = 120f;

			// Token: 0x040099D0 RID: 39376
			public const float TIER4 = 240f;

			// Token: 0x040099D1 RID: 39377
			public const float TIER5 = 480f;

			// Token: 0x040099D2 RID: 39378
			public const float TIER6 = 960f;

			// Token: 0x040099D3 RID: 39379
			public const float TIER7 = 1200f;

			// Token: 0x040099D4 RID: 39380
			public const float TIER8 = 1600f;
		}

		// Token: 0x02002272 RID: 8818
		public class EXHAUST_ENERGY_ACTIVE
		{
			// Token: 0x040099D5 RID: 39381
			public const float TIER0 = 0f;

			// Token: 0x040099D6 RID: 39382
			public const float TIER1 = 0.125f;

			// Token: 0x040099D7 RID: 39383
			public const float TIER2 = 0.25f;

			// Token: 0x040099D8 RID: 39384
			public const float TIER3 = 0.5f;

			// Token: 0x040099D9 RID: 39385
			public const float TIER4 = 1f;

			// Token: 0x040099DA RID: 39386
			public const float TIER5 = 2f;

			// Token: 0x040099DB RID: 39387
			public const float TIER6 = 4f;

			// Token: 0x040099DC RID: 39388
			public const float TIER7 = 8f;

			// Token: 0x040099DD RID: 39389
			public const float TIER8 = 16f;
		}

		// Token: 0x02002273 RID: 8819
		public class JOULES_LEAK_PER_CYCLE
		{
			// Token: 0x040099DE RID: 39390
			public const float TIER0 = 400f;

			// Token: 0x040099DF RID: 39391
			public const float TIER1 = 1000f;

			// Token: 0x040099E0 RID: 39392
			public const float TIER2 = 2000f;
		}

		// Token: 0x02002274 RID: 8820
		public class SELF_HEAT_KILOWATTS
		{
			// Token: 0x040099E1 RID: 39393
			public const float TIER0 = 0f;

			// Token: 0x040099E2 RID: 39394
			public const float TIER1 = 0.5f;

			// Token: 0x040099E3 RID: 39395
			public const float TIER2 = 1f;

			// Token: 0x040099E4 RID: 39396
			public const float TIER3 = 2f;

			// Token: 0x040099E5 RID: 39397
			public const float TIER4 = 4f;

			// Token: 0x040099E6 RID: 39398
			public const float TIER5 = 8f;

			// Token: 0x040099E7 RID: 39399
			public const float TIER6 = 16f;

			// Token: 0x040099E8 RID: 39400
			public const float TIER7 = 32f;

			// Token: 0x040099E9 RID: 39401
			public const float TIER8 = 64f;

			// Token: 0x040099EA RID: 39402
			public const float TIER_NUCLEAR = 16384f;
		}

		// Token: 0x02002275 RID: 8821
		public class MELTING_POINT_KELVIN
		{
			// Token: 0x040099EB RID: 39403
			public const float TIER0 = 800f;

			// Token: 0x040099EC RID: 39404
			public const float TIER1 = 1600f;

			// Token: 0x040099ED RID: 39405
			public const float TIER2 = 2400f;

			// Token: 0x040099EE RID: 39406
			public const float TIER3 = 3200f;

			// Token: 0x040099EF RID: 39407
			public const float TIER4 = 9999f;
		}

		// Token: 0x02002276 RID: 8822
		public class CONSTRUCTION_TIME_SECONDS
		{
			// Token: 0x040099F0 RID: 39408
			public const float TIER0 = 3f;

			// Token: 0x040099F1 RID: 39409
			public const float TIER1 = 10f;

			// Token: 0x040099F2 RID: 39410
			public const float TIER2 = 30f;

			// Token: 0x040099F3 RID: 39411
			public const float TIER3 = 60f;

			// Token: 0x040099F4 RID: 39412
			public const float TIER4 = 120f;

			// Token: 0x040099F5 RID: 39413
			public const float TIER5 = 240f;

			// Token: 0x040099F6 RID: 39414
			public const float TIER6 = 480f;
		}

		// Token: 0x02002277 RID: 8823
		public class HITPOINTS
		{
			// Token: 0x040099F7 RID: 39415
			public const int TIER0 = 10;

			// Token: 0x040099F8 RID: 39416
			public const int TIER1 = 30;

			// Token: 0x040099F9 RID: 39417
			public const int TIER2 = 100;

			// Token: 0x040099FA RID: 39418
			public const int TIER3 = 250;

			// Token: 0x040099FB RID: 39419
			public const int TIER4 = 1000;
		}

		// Token: 0x02002278 RID: 8824
		public class DAMAGE_SOURCES
		{
			// Token: 0x040099FC RID: 39420
			public const int CONDUIT_CONTENTS_BOILED = 1;

			// Token: 0x040099FD RID: 39421
			public const int CONDUIT_CONTENTS_FROZE = 1;

			// Token: 0x040099FE RID: 39422
			public const int BAD_INPUT_ELEMENT = 1;

			// Token: 0x040099FF RID: 39423
			public const int BUILDING_OVERHEATED = 1;

			// Token: 0x04009A00 RID: 39424
			public const int HIGH_LIQUID_PRESSURE = 10;

			// Token: 0x04009A01 RID: 39425
			public const int MICROMETEORITE = 1;

			// Token: 0x04009A02 RID: 39426
			public const int CORROSIVE_ELEMENT = 1;
		}

		// Token: 0x02002279 RID: 8825
		public class RELOCATION_TIME_SECONDS
		{
			// Token: 0x04009A03 RID: 39427
			public const float DECONSTRUCT = 4f;

			// Token: 0x04009A04 RID: 39428
			public const float CONSTRUCT = 4f;
		}

		// Token: 0x0200227A RID: 8826
		public class WORK_TIME_SECONDS
		{
			// Token: 0x04009A05 RID: 39429
			public const float VERYSHORT_WORK_TIME = 5f;

			// Token: 0x04009A06 RID: 39430
			public const float SHORT_WORK_TIME = 15f;

			// Token: 0x04009A07 RID: 39431
			public const float MEDIUM_WORK_TIME = 30f;

			// Token: 0x04009A08 RID: 39432
			public const float LONG_WORK_TIME = 90f;

			// Token: 0x04009A09 RID: 39433
			public const float VERY_LONG_WORK_TIME = 150f;

			// Token: 0x04009A0A RID: 39434
			public const float EXTENSIVE_WORK_TIME = 180f;
		}

		// Token: 0x0200227B RID: 8827
		public class FABRICATION_TIME_SECONDS
		{
			// Token: 0x04009A0B RID: 39435
			public const float VERY_SHORT = 20f;

			// Token: 0x04009A0C RID: 39436
			public const float SHORT = 40f;

			// Token: 0x04009A0D RID: 39437
			public const float MODERATE = 80f;

			// Token: 0x04009A0E RID: 39438
			public const float LONG = 250f;
		}

		// Token: 0x0200227C RID: 8828
		public class DECOR
		{
			// Token: 0x04009A0F RID: 39439
			public static readonly EffectorValues NONE = new EffectorValues
			{
				amount = 0,
				radius = 1
			};

			// Token: 0x0200227D RID: 8829
			public class BONUS
			{
				// Token: 0x04009A10 RID: 39440
				public static readonly EffectorValues TIER0 = new EffectorValues
				{
					amount = 5,
					radius = 1
				};

				// Token: 0x04009A11 RID: 39441
				public static readonly EffectorValues TIER1 = new EffectorValues
				{
					amount = 10,
					radius = 2
				};

				// Token: 0x04009A12 RID: 39442
				public static readonly EffectorValues TIER2 = new EffectorValues
				{
					amount = 15,
					radius = 3
				};

				// Token: 0x04009A13 RID: 39443
				public static readonly EffectorValues TIER3 = new EffectorValues
				{
					amount = 20,
					radius = 4
				};

				// Token: 0x04009A14 RID: 39444
				public static readonly EffectorValues TIER4 = new EffectorValues
				{
					amount = 25,
					radius = 5
				};

				// Token: 0x04009A15 RID: 39445
				public static readonly EffectorValues TIER5 = new EffectorValues
				{
					amount = 30,
					radius = 6
				};

				// Token: 0x0200227E RID: 8830
				public class MONUMENT
				{
					// Token: 0x04009A16 RID: 39446
					public static readonly EffectorValues COMPLETE = new EffectorValues
					{
						amount = 40,
						radius = 10
					};

					// Token: 0x04009A17 RID: 39447
					public static readonly EffectorValues INCOMPLETE = new EffectorValues
					{
						amount = 10,
						radius = 5
					};
				}
			}

			// Token: 0x0200227F RID: 8831
			public class PENALTY
			{
				// Token: 0x04009A18 RID: 39448
				public static readonly EffectorValues TIER0 = new EffectorValues
				{
					amount = -5,
					radius = 1
				};

				// Token: 0x04009A19 RID: 39449
				public static readonly EffectorValues TIER1 = new EffectorValues
				{
					amount = -10,
					radius = 2
				};

				// Token: 0x04009A1A RID: 39450
				public static readonly EffectorValues TIER2 = new EffectorValues
				{
					amount = -15,
					radius = 3
				};

				// Token: 0x04009A1B RID: 39451
				public static readonly EffectorValues TIER3 = new EffectorValues
				{
					amount = -20,
					radius = 4
				};

				// Token: 0x04009A1C RID: 39452
				public static readonly EffectorValues TIER4 = new EffectorValues
				{
					amount = -20,
					radius = 5
				};

				// Token: 0x04009A1D RID: 39453
				public static readonly EffectorValues TIER5 = new EffectorValues
				{
					amount = -25,
					radius = 6
				};
			}
		}

		// Token: 0x02002280 RID: 8832
		public class MASS_KG
		{
			// Token: 0x04009A1E RID: 39454
			public const float TIER0 = 25f;

			// Token: 0x04009A1F RID: 39455
			public const float TIER1 = 50f;

			// Token: 0x04009A20 RID: 39456
			public const float TIER2 = 100f;

			// Token: 0x04009A21 RID: 39457
			public const float TIER3 = 200f;

			// Token: 0x04009A22 RID: 39458
			public const float TIER4 = 400f;

			// Token: 0x04009A23 RID: 39459
			public const float TIER5 = 800f;

			// Token: 0x04009A24 RID: 39460
			public const float TIER6 = 1200f;

			// Token: 0x04009A25 RID: 39461
			public const float TIER7 = 2000f;
		}

		// Token: 0x02002281 RID: 8833
		public class UPGRADES
		{
			// Token: 0x04009A26 RID: 39462
			public const float BUILDTIME_TIER0 = 120f;

			// Token: 0x02002282 RID: 8834
			public class MATERIALTAGS
			{
				// Token: 0x04009A27 RID: 39463
				public const string METAL = "Metal";

				// Token: 0x04009A28 RID: 39464
				public const string REFINEDMETAL = "RefinedMetal";

				// Token: 0x04009A29 RID: 39465
				public const string CARBON = "Carbon";
			}

			// Token: 0x02002283 RID: 8835
			public class MATERIALMASS
			{
				// Token: 0x04009A2A RID: 39466
				public const int TIER0 = 100;

				// Token: 0x04009A2B RID: 39467
				public const int TIER1 = 200;

				// Token: 0x04009A2C RID: 39468
				public const int TIER2 = 400;

				// Token: 0x04009A2D RID: 39469
				public const int TIER3 = 500;
			}

			// Token: 0x02002284 RID: 8836
			public class MODIFIERAMOUNTS
			{
				// Token: 0x04009A2E RID: 39470
				public const float MANUALGENERATOR_ENERGYGENERATION = 1.2f;

				// Token: 0x04009A2F RID: 39471
				public const float MANUALGENERATOR_CAPACITY = 2f;

				// Token: 0x04009A30 RID: 39472
				public const float PROPANEGENERATOR_ENERGYGENERATION = 1.6f;

				// Token: 0x04009A31 RID: 39473
				public const float PROPANEGENERATOR_HEATGENERATION = 1.6f;

				// Token: 0x04009A32 RID: 39474
				public const float GENERATOR_HEATGENERATION = 0.8f;

				// Token: 0x04009A33 RID: 39475
				public const float GENERATOR_ENERGYGENERATION = 1.3f;

				// Token: 0x04009A34 RID: 39476
				public const float TURBINE_ENERGYGENERATION = 1.2f;

				// Token: 0x04009A35 RID: 39477
				public const float TURBINE_CAPACITY = 1.2f;

				// Token: 0x04009A36 RID: 39478
				public const float SUITRECHARGER_EXECUTIONTIME = 1.2f;

				// Token: 0x04009A37 RID: 39479
				public const float SUITRECHARGER_HEATGENERATION = 1.2f;

				// Token: 0x04009A38 RID: 39480
				public const float STORAGELOCKER_CAPACITY = 2f;

				// Token: 0x04009A39 RID: 39481
				public const float SOLARPANEL_ENERGYGENERATION = 1.2f;

				// Token: 0x04009A3A RID: 39482
				public const float SMELTER_HEATGENERATION = 0.7f;
			}
		}
	}
}
