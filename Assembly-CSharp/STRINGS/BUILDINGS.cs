﻿using System;
using TUNING;

namespace STRINGS
{
	public class BUILDINGS
	{
		public class PREFABS
		{
			public class FOSSILSCULPTURE
			{
				public static LocString NAME = UI.FormatAsLink("Fossil Block", "FOSSILSCULPTURE");

				public static LocString DESC = "Duplicants who have learned art skills can produce more decorative sculptures.";

				public static LocString EFFECT = string.Concat(new string[]
				{
					"Majorly increases ",
					UI.FormatAsLink("Decor", "DECOR"),
					", contributing to ",
					UI.FormatAsLink("Morale", "MORALE"),
					".\n\nMust be sculpted by a Duplicant."
				});
			}

			public class CEILINGFOSSILSCULPTURE
			{
				public static LocString NAME = UI.FormatAsLink("Hanging Fossil Block", "CEILINGFOSSILSCULPTURE");

				public static LocString DESC = "Duplicants who have learned art skills can produce more decorative ceiling sculptures.";

				public static LocString EFFECT = string.Concat(new string[]
				{
					"Majorly increases ",
					UI.FormatAsLink("Decor", "DECOR"),
					", contributing to ",
					UI.FormatAsLink("Morale", "MORALE"),
					".\n\nMust be sculpted by a Duplicant."
				});
			}

			public class HEADQUARTERSCOMPLETE
			{
				public static LocString NAME = UI.FormatAsLink("Printing Pod", "HEADQUARTERS");

				public static LocString UNIQUE_POPTEXT = "A clone of the cloning machine? What a novel thought.\n\nAlas, it won't work.";
			}

			public class EXOBASEHEADQUARTERS
			{
				public static LocString NAME = UI.FormatAsLink("Mini-Pod", "EXOBASEHEADQUARTERS");

				public static LocString DESC = "A quick and easy substitute, though it'll never live up to the original.";

				public static LocString EFFECT = "A portable bioprinter that produces new Duplicants or care packages containing resources.\n\nOnly one Printing Pod or Mini-Pod is permitted per Planetoid.";
			}

			public class AIRCONDITIONER
			{
				public static LocString NAME = UI.FormatAsLink("Thermo Regulator", "AIRCONDITIONER");

				public static LocString DESC = "A thermo regulator doesn't remove heat, but relocates it to a new area.";

				public static LocString EFFECT = string.Concat(new string[]
				{
					"Cools the ",
					UI.FormatAsLink("Gas", "ELEMENTS_GAS"),
					" piped through it, but outputs ",
					UI.FormatAsLink("Heat", "HEAT"),
					" in its immediate vicinity."
				});
			}

			public class STATERPILLAREGG
			{
				public static LocString NAME = UI.FormatAsLink("Slug Egg", "STATERPILLAREGG");

				public static LocString DESC = "The electrifying egg of the " + UI.FormatAsLink("Plug Slug", "STATERPILLAR") + ".";

				public static LocString EFFECT = "Slug Eggs can be connected to a " + UI.FormatAsLink("Power", "POWER") + " circuit as an energy source.";
			}

			public class STATERPILLARGENERATOR
			{
				public static LocString NAME = UI.FormatAsLink("Plug Slug", "STATERPILLAR");

				public class MODIFIERS
				{
					public static LocString WILD = "Wild!";

					public static LocString HUNGRY = "Hungry!";
				}
			}

			public class BEEHIVE
			{
				public static LocString NAME = UI.FormatAsLink("Beeta Hive", "BEEHIVE");

				public static LocString DESC = string.Concat(new string[]
				{
					"A moderately ",
					UI.FormatAsLink("Radioactive", "RADIATION"),
					" nest made by ",
					UI.FormatAsLink("Beetas", "BEE"),
					".\n\nConverts ",
					UI.FormatAsLink("Uranium Ore", "URANIUMORE"),
					" into ",
					UI.FormatAsLink("Enriched Uranium", "ENRICHEDURANIUM"),
					" when worked by a Beeta.\nWill not function if ground below has been destroyed."
				});

				public static LocString EFFECT = "The cozy home of a Beeta.";
			}

			public class ETHANOLDISTILLERY
			{
				public static LocString NAME = UI.FormatAsLink("Ethanol Distiller", "ETHANOLDISTILLERY");

				public static LocString DESC = string.Concat(new string[]
				{
					"Ethanol distillers convert ",
					ITEMS.INDUSTRIAL_PRODUCTS.WOOD.NAME,
					" into burnable ",
					ELEMENTS.ETHANOL.NAME,
					" fuel."
				});

				public static LocString EFFECT = string.Concat(new string[]
				{
					"Refines ",
					UI.FormatAsLink("Wood", "WOOD"),
					" into ",
					UI.FormatAsLink("Ethanol", "ETHANOL"),
					"."
				});
			}

			public class ALGAEDISTILLERY
			{
				public static LocString NAME = UI.FormatAsLink("Algae Distiller", "ALGAEDISTILLERY");

				public static LocString DESC = "Algae distillers convert disease-causing slime into algae for oxygen production.";

				public static LocString EFFECT = string.Concat(new string[]
				{
					"Refines ",
					UI.FormatAsLink("Slime", "SLIMEMOLD"),
					" into ",
					UI.FormatAsLink("Algae", "ALGAE"),
					"."
				});
			}

			public class GUNKEMPTIER
			{
				public static LocString NAME = UI.FormatAsLink("Gunk Extractor", "GUNKEMPTIER");

				public static LocString DESC = "Bionic Duplicants are much more relaxed after a visit to the gunk extractor.";

				public static LocString EFFECT = "Cleanses stale " + UI.FormatAsLink("Gunk", "LIQUIDGUNK") + " build-up from Duplicants' bionic parts.";
			}

			public class OILCHANGER
			{
				public static LocString NAME = UI.FormatAsLink("Lubrication Station", "OILCHANGER");

				public static LocString DESC = "A fresh supply of oil keeps the ol' joints from getting too creaky.";

				public static LocString EFFECT = "Uses " + UI.FormatAsLink("Gear Oil", "LUBRICATINGOIL") + " to keep Duplicants' bionic parts running smoothly.";
			}

			public class OXYLITEREFINERY
			{
				public static LocString NAME = UI.FormatAsLink("Oxylite Refinery", "OXYLITEREFINERY");

				public static LocString DESC = "Oxylite is a solid and easily transportable source of consumable oxygen.";

				public static LocString EFFECT = string.Concat(new string[]
				{
					"Synthesizes ",
					UI.FormatAsLink("Oxylite", "OXYROCK"),
					" using ",
					UI.FormatAsLink("Oxygen", "OXYGEN"),
					" and a small amount of ",
					UI.FormatAsLink("Gold", "GOLD"),
					"."
				});
			}

			public class OXYSCONCE
			{
				public static LocString NAME = UI.FormatAsLink("Oxylite Sconce", "OXYSCONCE");

				public static LocString DESC = "Sconces prevent diffused oxygen from being wasted inside storage bins.";

				public static LocString EFFECT = string.Concat(new string[]
				{
					"Stores a small chunk of ",
					UI.FormatAsLink("Oxylite", "OXYROCK"),
					" which gradually releases ",
					UI.FormatAsLink("Oxygen", "OXYGEN"),
					" into the environment."
				});
			}

			public class FERTILIZERMAKER
			{
				public static LocString NAME = UI.FormatAsLink("Fertilizer Synthesizer", "FERTILIZERMAKER");

				public static LocString DESC = "Fertilizer synthesizers convert polluted dirt into fertilizer for domestic plants.";

				public static LocString EFFECT = string.Concat(new string[]
				{
					"Uses ",
					UI.FormatAsLink("Polluted Water", "DIRTYWATER"),
					" and ",
					UI.FormatAsLink("Phosphorite", "PHOSPHORITE"),
					" to produce ",
					UI.FormatAsLink("Fertilizer", "FERTILIZER"),
					"."
				});
			}

			public class ALGAEHABITAT
			{
				public static LocString NAME = UI.FormatAsLink("Algae Terrarium", "ALGAEHABITAT");

				public static LocString DESC = "Algae colony, Duplicant colony... we're more alike than we are different.";

				public static LocString EFFECT = string.Concat(new string[]
				{
					"Consumes ",
					UI.FormatAsLink("Algae", "ALGAE"),
					" to produce ",
					UI.FormatAsLink("Oxygen", "OXYGEN"),
					" and remove some ",
					UI.FormatAsLink("Carbon Dioxide", "CARBONDIOXIDE"),
					".\n\nGains a 10% efficiency boost in direct ",
					UI.FormatAsLink("Light", "LIGHT"),
					"."
				});

				public static LocString SIDESCREEN_TITLE = "Empty " + UI.FormatAsLink("Polluted Water", "DIRTYWATER") + " Threshold";
			}

			public class BATTERY
			{
				public static LocString NAME = UI.FormatAsLink("Battery", "BATTERY");

				public static LocString DESC = "Batteries allow power from generators to be stored for later.";

				public static LocString EFFECT = "Stores " + UI.FormatAsLink("Power", "POWER") + " from generators, then provides that power to buildings.\n\nLoses charge over time.";

				public static LocString CHARGE_LOSS = "{Battery} charge loss";
			}

			public class FLYINGCREATUREBAIT
			{
				public static LocString NAME = UI.FormatAsLink("Airborne Critter Bait", "FLYINGCREATUREBAIT");

				public static LocString DESC = "The type of critter attracted by critter bait depends on the construction material.";

				public static LocString EFFECT = "Attracts one type of airborne critter.\n\nSingle use.";
			}

			public class WATERTRAP
			{
				public static LocString NAME = UI.FormatAsLink("Fish Trap", "WATERTRAP");

				public static LocString DESC = "Trapped fish will automatically be bagged for transport.";

				public static LocString EFFECT = string.Concat(new string[]
				{
					"Attracts and traps swimming ",
					UI.FormatAsLink("Pacu", "PACU"),
					".\n\nOnly Duplicants with the ",
					UI.FormatAsLink("Critter Ranching I", "RANCHING1"),
					" skill can arm this trap. It's reusable!"
				});
			}

			public class REUSABLETRAP
			{
				public static LocString LOGIC_PORT = "Trap Occupied";

				public static LocString LOGIC_PORT_ACTIVE = "Sends a " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + " when a critter has been trapped";

				public static LocString LOGIC_PORT_INACTIVE = "Otherwise, sends a " + UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby);

				public static LocString INPUT_LOGIC_PORT = "Trap Setter";

				public static LocString INPUT_LOGIC_PORT_ACTIVE = UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + ": Set trap";

				public static LocString INPUT_LOGIC_PORT_INACTIVE = UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby) + ": Disarm and empty trap";
			}

			public class CREATUREAIRTRAP
			{
				public static LocString NAME = UI.FormatAsLink("Airborne Critter Trap", "FLYINGCREATUREBAIT");

				public static LocString DESC = "It needs to be armed prior to use.";

				public static LocString EFFECT = string.Concat(new string[]
				{
					"Attracts and captures airborne ",
					UI.FormatAsLink("Critters", "CREATURES"),
					".\n\nOnly Duplicants with the ",
					UI.FormatAsLink("Critter Ranching I", "RANCHING1"),
					" skill can arm this trap. It's reusable!"
				});
			}

			public class AIRBORNECREATURELURE
			{
				public static LocString NAME = UI.FormatAsLink("Airborne Critter Lure", "AIRBORNECREATURELURE");

				public static LocString DESC = "Lures can relocate Pufts or Shine Bugs to specific locations in my colony.";

				public static LocString EFFECT = string.Concat(new string[]
				{
					"Attracts one type of airborne critter at a time.\n\nMust be baited with ",
					UI.FormatAsLink("Slime", "SLIMEMOLD"),
					" or ",
					UI.FormatAsLink("Phosphorite", "PHOSPHORITE"),
					"."
				});
			}

			public class BATTERYMEDIUM
			{
				public static LocString NAME = UI.FormatAsLink("Jumbo Battery", "BATTERYMEDIUM");

				public static LocString DESC = "Larger batteries hold more power and keep systems running longer before recharging.";

				public static LocString EFFECT = "Stores " + UI.FormatAsLink("Power", "POWER") + " from generators, then provides that power to buildings.\n\nSlightly loses charge over time.";
			}

			public class BATTERYSMART
			{
				public static LocString NAME = UI.FormatAsLink("Smart Battery", "BATTERYSMART");

				public static LocString DESC = "Smart batteries send a " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + " when they require charging.";

				public static LocString EFFECT = string.Concat(new string[]
				{
					"Stores ",
					UI.FormatAsLink("Power", "POWER"),
					" from generators, then provides that power to buildings.\n\nSends a ",
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					" or ",
					UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby),
					" based on the configuration of the Logic Activation Parameters.\n\nVery slightly loses charge over time."
				});

				public static LocString LOGIC_PORT = "Charge Parameters";

				public static LocString LOGIC_PORT_ACTIVE = "Sends a " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + " when battery is less than <b>Low Threshold</b> charged, until <b>High Threshold</b> is reached again";

				public static LocString LOGIC_PORT_INACTIVE = "Sends a " + UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby) + " when the battery is more than <b>High Threshold</b> charged, until <b>Low Threshold</b> is reached again";

				public static LocString ACTIVATE_TOOLTIP = "Sends a " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + " when battery is less than <b>{0}%</b> charged, until it is <b>{1}% (High Threshold)</b> charged";

				public static LocString DEACTIVATE_TOOLTIP = "Sends a " + UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby) + " when battery is <b>{0}%</b> charged, until it is less than <b>{1}% (Low Threshold)</b> charged";

				public static LocString SIDESCREEN_TITLE = "Logic Activation Parameters";

				public static LocString SIDESCREEN_ACTIVATE = "Low Threshold:";

				public static LocString SIDESCREEN_DEACTIVATE = "High Threshold:";
			}

			public class BED
			{
				public static LocString NAME = UI.FormatAsLink("Cot", "BED");

				public static LocString DESC = "Duplicants without a bed will develop sore backs from sleeping on the floor.";

				public static LocString EFFECT = "Gives one Duplicant a place to sleep.\n\nDuplicants will automatically return to their cots to sleep at night.";

				public class FACADES
				{
					public class DEFAULT_BED
					{
						public static LocString NAME = UI.FormatAsLink("Cot", "BED");

						public static LocString DESC = "A safe place to sleep.";
					}

					public class STARCURTAIN
					{
						public static LocString NAME = UI.FormatAsLink("Stargazer Cot", "BED");

						public static LocString DESC = "Now Duplicants can sleep beneath the stars without wearing an Atmo Suit to bed.";
					}

					public class SCIENCELAB
					{
						public static LocString NAME = UI.FormatAsLink("Lab Cot", "BED");

						public static LocString DESC = "For the Duplicant who dreams of scientific discoveries.";
					}

					public class STAYCATION
					{
						public static LocString NAME = UI.FormatAsLink("Staycation Cot", "BED");

						public static LocString DESC = "Like a weekend away, except... not.";
					}

					public class CREAKY
					{
						public static LocString NAME = UI.FormatAsLink("Camping Cot", "BED");

						public static LocString DESC = "It's sturdier than it looks.";
					}

					public class STRINGLIGHTS
					{
						public static LocString NAME = "Good Job Cot";

						public static LocString DESC = "Wrapped in shiny gold stars, to help sleepy Duplicants feel accomplished.";
					}
				}
			}

			public class BOTTLEEMPTIER
			{
				public static LocString NAME = UI.FormatAsLink("Bottle Emptier", "BOTTLEEMPTIER");

				public static LocString DESC = "A bottle emptier's Element Filter can be used to designate areas for specific liquid storage.";

				public static LocString EFFECT = "Empties bottled " + UI.FormatAsLink("Liquids", "ELEMENTS_LIQUID") + " back into the world.";
			}

			public class BOTTLEEMPTIERGAS
			{
				public static LocString NAME = UI.FormatAsLink("Canister Emptier", "BOTTLEEMPTIERGAS");

				public static LocString DESC = "A canister emptier's Element Filter can designate areas for specific gas storage.";

				public static LocString EFFECT = "Empties " + UI.FormatAsLink("Gas", "ELEMENTS_GAS") + " canisters back into the world.";
			}

			public class BOTTLEEMPTIERCONDUITLIQUID
			{
				public static LocString NAME = UI.FormatAsLink("Bottle Drainer", "BOTTLEEMPTIERCONDUITLIQUID");

				public static LocString DESC = "A bottle drainer's Element Filter can be used to designate areas for specific liquid storage.";

				public static LocString EFFECT = string.Concat(new string[]
				{
					"Drains bottled ",
					UI.FormatAsLink("Liquids", "ELEMENTS_LIQUID"),
					" into ",
					UI.FormatAsLink("Liquid Pipes", "LIQUIDCONDUIT"),
					"."
				});
			}

			public class BOTTLEEMPTIERCONDUITGAS
			{
				public static LocString NAME = UI.FormatAsLink("Canister Drainer", "BOTTLEEMPTIERCONDUITGAS");

				public static LocString DESC = "A canister drainer's Element Filter can designate areas for specific gas storage.";

				public static LocString EFFECT = string.Concat(new string[]
				{
					"Drains ",
					UI.FormatAsLink("Gas", "ELEMENTS_GAS"),
					" canisters into ",
					UI.FormatAsLink("Gas Pipes", "GASCONDUIT"),
					"."
				});
			}

			public class ARTIFACTCARGOBAY
			{
				public static LocString NAME = UI.FormatAsLink("Artifact Transport Module", "ARTIFACTCARGOBAY");

				public static LocString DESC = "Holds artifacts found in space.";

				public static LocString EFFECT = "Allows Duplicants to store any artifacts they uncover during space missions.\n\nArtifacts become available to the colony upon the rocket's return. \n\nMust be built via " + BUILDINGS.PREFABS.LAUNCHPAD.NAME + ".";
			}

			public class CARGOBAY
			{
				public static LocString NAME = UI.FormatAsLink("Cargo Bay", "CARGOBAY");

				public static LocString DESC = "Duplicants will fill cargo bays with any resources they find during space missions.";

				public static LocString EFFECT = "Allows Duplicants to store any " + UI.FormatAsLink("Solid Materials", "ELEMENTS_SOLID") + " found during space missions.\n\nStored resources become available to the colony upon the rocket's return.";
			}

			public class CARGOBAYCLUSTER
			{
				public static LocString NAME = UI.FormatAsLink("Large Cargo Bay", "CARGOBAY");

				public static LocString DESC = "Holds more than a regular cargo bay.";

				public static LocString EFFECT = string.Concat(new string[]
				{
					"Allows Duplicants to store most of the ",
					UI.FormatAsLink("Solid Materials", "ELEMENTS_SOLID"),
					" found during space missions.\n\nStored resources become available to the colony upon the rocket's return. \n\nMust be built via ",
					BUILDINGS.PREFABS.LAUNCHPAD.NAME,
					"."
				});
			}

			public class SOLIDCARGOBAYSMALL
			{
				public static LocString NAME = UI.FormatAsLink("Cargo Bay", "SOLIDCARGOBAYSMALL");

				public static LocString DESC = "Duplicants will fill cargo bays with any resources they find during space missions.";

				public static LocString EFFECT = string.Concat(new string[]
				{
					"Allows Duplicants to store some of the ",
					UI.FormatAsLink("Solid Materials", "ELEMENTS_SOLID"),
					" found during space missions.\n\nStored resources become available to the colony upon the rocket's return. \n\nMust be built via ",
					BUILDINGS.PREFABS.LAUNCHPAD.NAME,
					"."
				});
			}

			public class SPECIALCARGOBAY
			{
				public static LocString NAME = UI.FormatAsLink("Biological Cargo Bay", "SPECIALCARGOBAY");

				public static LocString DESC = "Biological cargo bays allow Duplicants to retrieve alien plants and wildlife from space.";

				public static LocString EFFECT = "Allows Duplicants to store unusual or organic resources found during space missions.\n\nStored resources become available to the colony upon the rocket's return.";
			}

			public class SPECIALCARGOBAYCLUSTER
			{
				public static LocString NAME = UI.FormatAsLink("Critter Cargo Bay", "SPECIALCARGOBAY");

				public static LocString DESC = "Critters do not require feeding during transit.";

				public static LocString EFFECT = string.Concat(new string[]
				{
					"Allows Duplicants to transport ",
					UI.CODEX.CATEGORYNAMES.CREATURES,
					" through space.\n\nSpecimens can be released into the colony upon the rocket's return.\n\nMust be built via ",
					BUILDINGS.PREFABS.LAUNCHPAD.NAME,
					"."
				});

				public static LocString RELEASE_BTN = "Release Critter";

				public static LocString RELEASE_BTN_TOOLTIP = "Release the critter stored inside";
			}

			public class COMMANDMODULE
			{
				public static LocString NAME = UI.FormatAsLink("Command Capsule", "COMMANDMODULE");

				public static LocString DESC = "At least one astronaut must be assigned to the command module to pilot a rocket.";

				public static LocString EFFECT = "Contains passenger seating for Duplicant " + UI.FormatAsLink("Astronauts", "ASTRONAUTING1") + ".\n\nA Command Capsule must be the last module installed at the top of a rocket.";

				public static LocString LOGIC_PORT_READY = "Rocket Checklist";

				public static LocString LOGIC_PORT_READY_ACTIVE = "Sends a " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + " when its rocket launch checklist is complete";

				public static LocString LOGIC_PORT_READY_INACTIVE = "Otherwise, sends a " + UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby);

				public static LocString LOGIC_PORT_LAUNCH = "Launch Rocket";

				public static LocString LOGIC_PORT_LAUNCH_ACTIVE = UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + ": Launch rocket";

				public static LocString LOGIC_PORT_LAUNCH_INACTIVE = UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby) + ": Awaits launch command";
			}

			public class CLUSTERCOMMANDMODULE
			{
				public static LocString NAME = UI.FormatAsLink("Command Capsule", "CLUSTERCOMMANDMODULE");

				public static LocString DESC = "";

				public static LocString EFFECT = "";

				public static LocString LOGIC_PORT_READY = "Rocket Checklist";

				public static LocString LOGIC_PORT_READY_ACTIVE = "Sends a " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + " when its rocket launch checklist is complete";

				public static LocString LOGIC_PORT_READY_INACTIVE = "Otherwise, sends a " + UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby);

				public static LocString LOGIC_PORT_LAUNCH = "Launch Rocket";

				public static LocString LOGIC_PORT_LAUNCH_ACTIVE = UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + ": Launch rocket";

				public static LocString LOGIC_PORT_LAUNCH_INACTIVE = UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby) + ": Awaits launch command";
			}

			public class CLUSTERCRAFTINTERIORDOOR
			{
				public static LocString NAME = UI.FormatAsLink("Interior Hatch", "CLUSTERCRAFTINTERIORDOOR");

				public static LocString DESC = "A hatch for getting in and out of the rocket.";

				public static LocString EFFECT = "Warning: Do not open mid-flight.";
			}

			public class ROBOPILOTMODULE
			{
				public static LocString NAME = UI.FormatAsLink("Robo-Pilot Module", "ROBOPILOTMODULE");

				public static LocString DESC = "Robo-pilot modules do not require a Duplicant astronaut.";

				public static LocString EFFECT = string.Concat(new string[]
				{
					"Enables rockets to travel swfitly without a ",
					UI.FormatAsLink("Rocket Control Station", "ROCKETCONTROLSTATION"),
					".\n\nMust be built via ",
					BUILDINGS.PREFABS.LAUNCHPAD.NAME,
					"."
				});
			}

			public class ROBOPILOTCOMMANDMODULE
			{
				public static LocString NAME = UI.FormatAsLink("Robo-Pilot Capsule", "ROBOPILOTCOMMANDMODULE");

				public static LocString DESC = "Robo-pilot modules do not require a Duplicant astronaut.";

				public static LocString EFFECT = "Enables rockets to travel swiftly and safely without a " + UI.FormatAsLink("Command Capsule", "COMMANDMODULE") + ".\n\nA Robo-Pilot Capsule must be the last module installed at the top of a rocket.";
			}

			public class ROCKETCONTROLSTATION
			{
				public static LocString NAME = UI.FormatAsLink("Rocket Control Station", "ROCKETCONTROLSTATION");

				public static LocString DESC = "Someone needs to be around to jiggle the controls when the screensaver comes on.";

				public static LocString EFFECT = "Allows Duplicants to use pilot-operated rockets and control access to interior buildings.\n\nAssigned Duplicants must have the " + UI.FormatAsLink("Rocket Piloting", "ROCKETPILOTING1") + " skill.";

				public static LocString LOGIC_PORT = "Restrict Building Usage";

				public static LocString LOGIC_PORT_ACTIVE = UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + ": Restrict access to interior buildings";

				public static LocString LOGIC_PORT_INACTIVE = UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby) + ": Unrestrict access to interior buildings";
			}

			public class RESEARCHMODULE
			{
				public static LocString NAME = UI.FormatAsLink("Research Module", "RESEARCHMODULE");

				public static LocString DESC = "Data banks can be used at virtual planetariums to produce additional research.";

				public static LocString EFFECT = string.Concat(new string[]
				{
					"Completes one ",
					UI.FormatAsLink("Research Task", "RESEARCH"),
					" per space mission.\n\nProduces a small Data Bank regardless of mission destination.\n\nGenerated ",
					UI.FormatAsLink("Research Points", "RESEARCH"),
					" become available upon the rocket's return."
				});
			}

			public class TOURISTMODULE
			{
				public static LocString NAME = UI.FormatAsLink("Sight-Seeing Module", "TOURISTMODULE");

				public static LocString DESC = "An astronaut must accompany sight seeing Duplicants on rocket flights.";

				public static LocString EFFECT = "Allows one non-Astronaut Duplicant to visit space.\n\nSight-Seeing Rocket flights decrease " + UI.FormatAsLink("Stress", "STRESS") + ".";
			}

			public class SCANNERMODULE
			{
				public static LocString NAME = UI.FormatAsLink("Cartographic Module", "SCANNERMODULE");

				public static LocString DESC = "Allows Duplicants to boldly go where other Duplicants haven't been yet.";

				public static LocString EFFECT = "Automatically analyzes adjacent space while on a voyage. \n\nMust be built via " + BUILDINGS.PREFABS.LAUNCHPAD.NAME + ".";
			}

			public class HABITATMODULESMALL
			{
				public static LocString NAME = UI.FormatAsLink("Solo Spacefarer Nosecone", "HABITATMODULESMALL");

				public static LocString DESC = "One lucky Duplicant gets the best view from the whole rocket.";

				public static LocString EFFECT = "Functions as a Command Module and a Nosecone.\n\nHolds one Duplicant traveller.\n\nOne Command Module may be installed per rocket.\n\nMust be built via " + BUILDINGS.PREFABS.LAUNCHPAD.NAME + ". \n\nMust be built at the top of a rocket.";
			}

			public class HABITATMODULEMEDIUM
			{
				public static LocString NAME = UI.FormatAsLink("Spacefarer Module", "HABITATMODULEMEDIUM");

				public static LocString DESC = "Allows Duplicants to survive space travel... Hopefully.";

				public static LocString EFFECT = "Functions as a Command Module.\n\nHolds up to ten Duplicant travellers.\n\nOne Command Module may be installed per rocket. \n\nEngine must be built via " + BUILDINGS.PREFABS.LAUNCHPAD.NAME + ".";
			}

			public class NOSECONEBASIC
			{
				public static LocString NAME = UI.FormatAsLink("Basic Nosecone", "NOSECONEBASIC");

				public static LocString DESC = "Every rocket requires a nosecone to fly.";

				public static LocString EFFECT = "Protects a rocket during takeoff and entry, enabling space travel.\n\nEngine must be built via " + BUILDINGS.PREFABS.LAUNCHPAD.NAME + ". \n\nMust be built at the top of a rocket.";
			}

			public class NOSECONEHARVEST
			{
				public static LocString NAME = UI.FormatAsLink("Drillcone", "NOSECONEHARVEST");

				public static LocString DESC = "Harvests resources from the universe.";

				public static LocString EFFECT = string.Concat(new string[]
				{
					"Enables a rocket to drill into interstellar debris and collect ",
					UI.FormatAsLink("gas", "ELEMENTS_GAS"),
					", ",
					UI.FormatAsLink("liquid", "ELEMENTS_LIQUID"),
					" and ",
					UI.FormatAsLink("solid", "ELEMENTS_SOLID"),
					" resources from space.\n\nEngine must be built via ",
					BUILDINGS.PREFABS.LAUNCHPAD.NAME,
					". \n\nMust be built at the top of a rocket with ",
					UI.FormatAsLink("gas", "ELEMENTS_GAS"),
					", ",
					UI.FormatAsLink("liquid", "ELEMENTS_LIQUID"),
					" or ",
					UI.FormatAsLink("solid", "ELEMENTS_SOLID"),
					" Cargo Module attached to store the appropriate resources."
				});
			}

			public class CO2ENGINE
			{
				public static LocString NAME = UI.FormatAsLink("Carbon Dioxide Engine", "CO2ENGINE");

				public static LocString DESC = "Rockets can be used to send Duplicants into space and retrieve rare resources.";

				public static LocString EFFECT = string.Concat(new string[]
				{
					"Uses pressurized ",
					UI.FormatAsLink("Carbon Dioxide", "CARBONDIOXIDE"),
					" to propel rockets for short range space exploration.\n\nCarbon Dioxide Engines are relatively fast engine for their size but with limited height restrictions.\n\nEngine must be built via ",
					BUILDINGS.PREFABS.LAUNCHPAD.NAME,
					". \n\nOnce the engine has been built, more rocket modules can be added."
				});
			}

			public class KEROSENEENGINE
			{
				public static LocString NAME = UI.FormatAsLink("Petroleum Engine", "KEROSENEENGINE");

				public static LocString DESC = "Rockets can be used to send Duplicants into space and retrieve rare resources.";

				public static LocString EFFECT = "Burns " + UI.FormatAsLink("Petroleum", "PETROLEUM") + " to propel rockets for mid-range space exploration.\n\nPetroleum Engines have generous height restrictions, ideal for hauling many modules.\n\nThe engine must be built first before more rocket modules can be added.";
			}

			public class KEROSENEENGINECLUSTER
			{
				public static LocString NAME = UI.FormatAsLink("Petroleum Engine", "KEROSENEENGINECLUSTER");

				public static LocString DESC = "More powerful rocket engines can propel heavier burdens.";

				public static LocString EFFECT = string.Concat(new string[]
				{
					"Burns ",
					UI.FormatAsLink("Petroleum", "PETROLEUM"),
					" to propel rockets for mid-range space exploration.\n\nPetroleum Engines have generous height restrictions, ideal for hauling many modules.\n\nEngine must be built via ",
					BUILDINGS.PREFABS.LAUNCHPAD.NAME,
					". \n\nOnce the engine has been built, more rocket modules can be added."
				});
			}

			public class KEROSENEENGINECLUSTERSMALL
			{
				public static LocString NAME = UI.FormatAsLink("Small Petroleum Engine", "KEROSENEENGINECLUSTERSMALL");

				public static LocString DESC = "Rockets can be used to send Duplicants into space and retrieve rare resources.";

				public static LocString EFFECT = string.Concat(new string[]
				{
					"Burns ",
					UI.FormatAsLink("Petroleum", "PETROLEUM"),
					" to propel rockets for mid-range space exploration.\n\nSmall Petroleum Engines possess the same speed as a ",
					UI.FormatAsLink("Petroleum Engines", "KEROSENEENGINE"),
					" but have smaller height restrictions.\n\nEngine must be built via ",
					BUILDINGS.PREFABS.LAUNCHPAD.NAME,
					". \n\nOnce the engine has been built, more rocket modules can be added."
				});
			}

			public class HYDROGENENGINE
			{
				public static LocString NAME = UI.FormatAsLink("Hydrogen Engine", "HYDROGENENGINE");

				public static LocString DESC = "Hydrogen engines can propel rockets further than steam or petroleum engines.";

				public static LocString EFFECT = string.Concat(new string[]
				{
					"Burns ",
					UI.FormatAsLink("Liquid Hydrogen", "LIQUIDHYDROGEN"),
					" to propel rockets for long-range space exploration.\n\nHydrogen Engines have the same generous height restrictions as ",
					UI.FormatAsLink("Petroleum Engines", "KEROSENEENGINE"),
					" but are slightly faster.\n\nThe engine must be built first before more rocket modules can be added."
				});
			}

			public class HYDROGENENGINECLUSTER
			{
				public static LocString NAME = UI.FormatAsLink("Hydrogen Engine", "HYDROGENENGINECLUSTER");

				public static LocString DESC = "Hydrogen engines can propel rockets further than steam or petroleum engines.";

				public static LocString EFFECT = string.Concat(new string[]
				{
					"Burns ",
					UI.FormatAsLink("Liquid Hydrogen", "LIQUIDHYDROGEN"),
					" to propel rockets for long-range space exploration.\n\nHydrogen Engines have the same generous height restrictions as ",
					UI.FormatAsLink("Petroleum Engines", "KEROSENEENGINE"),
					" but are slightly faster.\n\nEngine must be built via ",
					BUILDINGS.PREFABS.LAUNCHPAD.NAME,
					".\n\nOnce the engine has been built, more rocket modules can be added."
				});
			}

			public class SUGARENGINE
			{
				public static LocString NAME = UI.FormatAsLink("Sugar Engine", "SUGARENGINE");

				public static LocString DESC = "Not the most stylish way to travel space, but certainly the tastiest.";

				public static LocString EFFECT = string.Concat(new string[]
				{
					"Burns ",
					UI.FormatAsLink("Sucrose", "SUCROSE"),
					" to propel rockets for short range space exploration.\n\nSugar Engines have higher height restrictions than ",
					UI.FormatAsLink("Carbon Dioxide Engines", "CO2ENGINE"),
					", but move slower.\n\nEngine must be built via ",
					BUILDINGS.PREFABS.LAUNCHPAD.NAME,
					". \n\nOnce the engine has been built, more rocket modules can be added."
				});
			}

			public class HEPENGINE
			{
				public static LocString NAME = UI.FormatAsLink("Radbolt Engine", "HEPENGINE");

				public static LocString DESC = "Radbolt-fueled rockets support few modules, but travel exceptionally far.";

				public static LocString EFFECT = string.Concat(new string[]
				{
					"Injects ",
					UI.FormatAsLink("Radbolts", "RADIATION"),
					" into a reaction chamber to propel rockets for long-range space exploration.\n\nRadbolt Engines are faster than ",
					UI.FormatAsLink("Hydrogen Engines", "HYDROGENENGINE"),
					" but with a more restrictive height allowance.\n\nEngine must be built via ",
					BUILDINGS.PREFABS.LAUNCHPAD.NAME,
					". \n\nOnce the engine has been built, more rocket modules can be added."
				});

				public static LocString LOGIC_PORT_STORAGE = "Radbolt Storage";

				public static LocString LOGIC_PORT_STORAGE_ACTIVE = "Sends a " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + " when its Radbolt Storage is full";

				public static LocString LOGIC_PORT_STORAGE_INACTIVE = "Otherwise, sends a " + UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby);
			}

			public class ORBITALCARGOMODULE
			{
				public static LocString NAME = UI.FormatAsLink("Orbital Cargo Module", "ORBITALCARGOMODULE");

				public static LocString DESC = "It's a generally good idea to pack some supplies when exploring unknown worlds.";

				public static LocString EFFECT = "Delivers cargo to the surface of Planetoids that do not yet have a " + BUILDINGS.PREFABS.LAUNCHPAD.NAME + ". \n\nMust be built via Rocket Platform.";
			}

			public class BATTERYMODULE
			{
				public static LocString NAME = UI.FormatAsLink("Battery Module", "BATTERYMODULE");

				public static LocString DESC = "Charging a battery module before takeoff makes it easier to power buildings during flight.";

				public static LocString EFFECT = string.Concat(new string[]
				{
					"Stores the excess ",
					UI.FormatAsLink("Power", "POWER"),
					" generated by a Rocket Engine or ",
					BUILDINGS.PREFABS.LAUNCHPAD.NAME,
					".\n\nProvides stored power to ",
					UI.FormatAsLink("Interior Rocket Outlets", "ROCKETINTERIORPOWERPLUG"),
					".\n\nLoses charge over time. \n\nMust be built via Rocket Platform."
				});
			}

			public class PIONEERMODULE
			{
				public static LocString NAME = UI.FormatAsLink("Trailblazer Module", "PIONEERMODULE");

				public static LocString DESC = "That's one small step for Dupekind.";

				public static LocString EFFECT = "Enables travel to Planetoids that do not yet have a " + BUILDINGS.PREFABS.LAUNCHPAD.NAME + ".\n\nCan hold one Duplicant traveller.\n\nDeployment is available while in a Starmap hex adjacent to a Planetoid. \n\nMust be built via Rocket Platform.";
			}

			public class SOLARPANELMODULE
			{
				public static LocString NAME = UI.FormatAsLink("Solar Panel Module", "SOLARPANELMODULE");

				public static LocString DESC = "Collect solar energy before takeoff and during flight.";

				public static LocString EFFECT = string.Concat(new string[]
				{
					"Converts ",
					UI.FormatAsLink("Sunlight", "LIGHT"),
					" into electrical ",
					UI.FormatAsLink("Power", "POWER"),
					" for use on rockets.\n\nMust be built via ",
					BUILDINGS.PREFABS.LAUNCHPAD.NAME,
					". \n\nMust be exposed to space."
				});
			}

			public class SCOUTMODULE
			{
				public static LocString NAME = UI.FormatAsLink("Rover's Module", "SCOUTMODULE");

				public static LocString DESC = "Rover can conduct explorations of planetoids that don't have rocket platforms built.";

				public static LocString EFFECT = string.Concat(new string[]
				{
					"Deploys one ",
					UI.FormatAsLink("Rover Bot", "SCOUT"),
					" for remote Planetoid exploration.\n\nDeployment is available while in a Starmap hex adjacent to a Planetoid. \n\nMust be built via ",
					BUILDINGS.PREFABS.LAUNCHPAD.NAME,
					"."
				});
			}

			public class PIONEERLANDER
			{
				public static LocString NAME = UI.FormatAsLink("Trailblazer Lander", "PIONEERLANDER");

				public static LocString DESC = "Lands a Duplicant on a Planetoid from an orbiting " + BUILDINGS.PREFABS.PIONEERMODULE.NAME + ".";
			}

			public class SCOUTLANDER
			{
				public static LocString NAME = UI.FormatAsLink("Rover's Lander", "SCOUTLANDER");

				public static LocString DESC = string.Concat(new string[]
				{
					"Lands ",
					UI.FormatAsLink("Rover", "SCOUT"),
					" on a Planetoid when ",
					BUILDINGS.PREFABS.SCOUTMODULE.NAME,
					" is in orbit."
				});
			}

			public class GANTRY
			{
				public static LocString NAME = UI.FormatAsLink("Gantry", "GANTRY");

				public static LocString DESC = "A gantry can be built over rocket pieces where ladders and tile cannot.";

				public static LocString EFFECT = "Provides scaffolding across rocket modules to allow Duplicant access.";

				public static LocString LOGIC_PORT = "Extend/Retract";

				public static LocString LOGIC_PORT_ACTIVE = "<b>Extends gantry</b> when a " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + " signal is received";

				public static LocString LOGIC_PORT_INACTIVE = "<b>Retracts gantry</b> when a " + UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby) + " signal is received";
			}

			public class ROCKETINTERIORPOWERPLUG
			{
				public static LocString NAME = UI.FormatAsLink("Power Outlet Fitting", "ROCKETINTERIORPOWERPLUG");

				public static LocString DESC = "Outlets conveniently power buildings inside a cockpit using their rocket's power stores.";

				public static LocString EFFECT = string.Concat(new string[]
				{
					"Provides ",
					UI.FormatAsLink("Power", "POWER"),
					" to connected buildings.\n\nPulls power from ",
					UI.FormatAsLink("Battery Modules", "BATTERYMODULE"),
					" and Rocket Engines.\n\nMust be built within the interior of a Rocket Module."
				});
			}

			public class ROCKETINTERIORLIQUIDINPUT
			{
				public static LocString NAME = UI.FormatAsLink("Liquid Intake Fitting", "ROCKETINTERIORLIQUIDINPUT");

				public static LocString DESC = "Begone, foul waters!";

				public static LocString EFFECT = string.Concat(new string[]
				{
					"Allows ",
					UI.FormatAsLink("Liquids", "ELEMENTS_LIQUID"),
					" to be pumped into rocket storage via ",
					UI.FormatAsLink("Pipes", "LIQUIDCONDUIT"),
					".\n\nSends liquid to the first Rocket Module with available space.\n\nMust be built within the interior of a Rocket Module."
				});
			}

			public class ROCKETINTERIORLIQUIDOUTPUT
			{
				public static LocString NAME = UI.FormatAsLink("Liquid Output Fitting", "ROCKETINTERIORLIQUIDOUTPUT");

				public static LocString DESC = "Now if only we had some water balloons...";

				public static LocString EFFECT = string.Concat(new string[]
				{
					"Allows ",
					UI.FormatAsLink("Liquids", "ELEMENTS_LIQUID"),
					" to be drawn from rocket storage via ",
					UI.FormatAsLink("Pipes", "LIQUIDCONDUIT"),
					".\n\nDraws liquid from the first Rocket Module with the requested material.\n\nMust be built within the interior of a Rocket Module."
				});
			}

			public class ROCKETINTERIORGASINPUT
			{
				public static LocString NAME = UI.FormatAsLink("Gas Intake Fitting", "ROCKETINTERIORGASINPUT");

				public static LocString DESC = "It's basically central-vac.";

				public static LocString EFFECT = string.Concat(new string[]
				{
					"Allows ",
					UI.FormatAsLink("Gases", "ELEMENTS_GAS"),
					" to be pumped into rocket storage via ",
					UI.FormatAsLink("Pipes", "GASCONDUIT"),
					".\n\nSends gas to the first Rocket Module with available space.\n\nMust be built within the interior of a Rocket Module."
				});
			}

			public class ROCKETINTERIORGASOUTPUT
			{
				public static LocString NAME = UI.FormatAsLink("Gas Output Fitting", "ROCKETINTERIORGASOUTPUT");

				public static LocString DESC = "Refreshing breezes, on-demand.";

				public static LocString EFFECT = string.Concat(new string[]
				{
					"Allows ",
					UI.FormatAsLink("Gases", "ELEMENTS_GAS"),
					" to be drawn from rocket storage via ",
					UI.FormatAsLink("Pipes", "GASCONDUIT"),
					".\n\nDraws gas from the first Rocket Module with the requested material.\n\nMust be built within the interior of a Rocket Module."
				});
			}

			public class ROCKETINTERIORSOLIDINPUT
			{
				public static LocString NAME = UI.FormatAsLink("Conveyor Receptacle Fitting", "ROCKETINTERIORSOLIDINPUT");

				public static LocString DESC = "Why organize your shelves when you can just shove everything in here?";

				public static LocString EFFECT = string.Concat(new string[]
				{
					"Allows ",
					UI.FormatAsLink("Solid Materials", "ELEMENTS_SOLID"),
					" to be moved into rocket storage via ",
					UI.FormatAsLink("Conveyor Rails", "SOLIDCONDUIT"),
					".\n\nSends solid material to the first Rocket Module with available space.\n\nMust be built within the interior of a Rocket Module."
				});
			}

			public class ROCKETINTERIORSOLIDOUTPUT
			{
				public static LocString NAME = UI.FormatAsLink("Conveyor Loader Fitting", "ROCKETINTERIORSOLIDOUTPUT");

				public static LocString DESC = "For accessing your stored luggage mid-flight.";

				public static LocString EFFECT = string.Concat(new string[]
				{
					"Allows ",
					UI.FormatAsLink("Solid Materials", "ELEMENTS_SOLID"),
					" to be moved out of rocket storage via ",
					UI.FormatAsLink("Conveyor Rails", "SOLIDCONDUIT"),
					".\n\nDraws solid material from the first Rocket Module with the requested material.\n\nMust be built within the interior of a Rocket Module."
				});
			}

			public class WATERCOOLER
			{
				public static LocString NAME = UI.FormatAsLink("Water Cooler", "WATERCOOLER");

				public static LocString DESC = "Chatting with friends improves Duplicants' moods and reduces their stress.";

				public static LocString EFFECT = "Provides a gathering place for Duplicants during Downtime.\n\nImproves Duplicant " + UI.FormatAsLink("Morale", "MORALE") + ".";

				public class OPTION_TOOLTIPS
				{
					public static LocString WATER = ELEMENTS.WATER.NAME + "\nPlain potable water";

					public static LocString MILK = ELEMENTS.MILK.NAME + "\nA salty, green-hued beverage";
				}

				public class FACADES
				{
					public class DEFAULT_WATERCOOLER
					{
						public static LocString NAME = UI.FormatAsLink("Water Cooler", "WATERCOOLER");

						public static LocString DESC = "Where Duplicants sip and socialize.";
					}

					public class ROUND_BODY
					{
						public static LocString NAME = UI.FormatAsLink("Elegant Water Cooler", "WATERCOOLER");

						public static LocString DESC = "It really classes up a breakroom.";
					}

					public class BALLOON
					{
						public static LocString NAME = UI.FormatAsLink("Inflatable Water Cooler", "WATERCOOLER");

						public static LocString DESC = "There's a funny aftertaste.";
					}

					public class YELLOW_TARTAR
					{
						public static LocString NAME = UI.FormatAsLink("Ick Yellow Water Cooler", "WATERCOOLER");

						public static LocString DESC = "Did someone boil eggs in this water?";
					}

					public class RED_ROSE
					{
						public static LocString NAME = UI.FormatAsLink("Puce Pink Water Cooler", "WATERCOOLER");

						public static LocString DESC = "Rose-colored paper cups: the shatter-proof alternative to rose-colored glasses.";
					}

					public class GREEN_MUSH
					{
						public static LocString NAME = UI.FormatAsLink("Mush Green Water Cooler", "WATERCOOLER");

						public static LocString DESC = "Ideal for post-Mush Bar palate cleansing.";
					}

					public class PURPLE_BRAINFAT
					{
						public static LocString NAME = UI.FormatAsLink("Faint Purple Water Cooler", "WATERCOOLER");

						public static LocString DESC = "Most Duplicants agree that it really should dispense juice.";
					}

					public class BLUE_BABYTEARS
					{
						public static LocString NAME = UI.FormatAsLink("Weepy Blue Water Cooler", "WATERCOOLER");

						public static LocString DESC = "Lightly salted with Duplicants' tears.";
					}
				}
			}

			public class ARCADEMACHINE
			{
				public static LocString NAME = UI.FormatAsLink("Arcade Cabinet", "ARCADEMACHINE");

				public static LocString DESC = "Komet Kablam-O!\nFor up to two players.";

				public static LocString EFFECT = "Allows Duplicants to play video games on their breaks.\n\nIncreases Duplicant " + UI.FormatAsLink("Morale", "MORALE") + ".";
			}

			public class SINGLEPLAYERARCADE
			{
				public static LocString NAME = UI.FormatAsLink("Single Player Arcade", "SINGLEPLAYERARCADE");

				public static LocString DESC = "Space Brawler IV! For one player.";

				public static LocString EFFECT = "Allows a Duplicant to play video games solo on their breaks.\n\nIncreases Duplicant " + UI.FormatAsLink("Morale", "MORALE") + ".";
			}

			public class PHONOBOX
			{
				public static LocString NAME = UI.FormatAsLink("Jukebot", "PHONOBOX");

				public static LocString DESC = "Dancing helps Duplicants get their innermost feelings out.";

				public static LocString EFFECT = "Plays music for Duplicants to dance to on their breaks.\n\nIncreases Duplicant " + UI.FormatAsLink("Morale", "MORALE") + ".";
			}

			public class JUICER
			{
				public static LocString NAME = UI.FormatAsLink("Juicer", "JUICER");

				public static LocString DESC = "Fruity juice can really brighten a Duplicant's breaktime";

				public static LocString EFFECT = "Provides refreshment for Duplicants on their breaks.\n\nDrinking juice increases Duplicant " + UI.FormatAsLink("Morale", "MORALE") + ".";
			}

			public class ESPRESSOMACHINE
			{
				public static LocString NAME = UI.FormatAsLink("Espresso Machine", "ESPRESSOMACHINE");

				public static LocString DESC = "A shot of espresso helps Duplicants relax after a long day.";

				public static LocString EFFECT = "Provides refreshment for Duplicants on their breaks.\n\nIncreases Duplicant " + UI.FormatAsLink("Morale", "MORALE") + ".";
			}

			public class TELEPHONE
			{
				public static LocString NAME = UI.FormatAsLink("Party Line Phone", "TELEPHONE");

				public static LocString DESC = "You never know who you'll meet on the other line.";

				public static LocString EFFECT = "Can be used by one Duplicant to chat with themselves or with other Duplicants in different locations.\n\nChatting increases Duplicant " + UI.FormatAsLink("Morale", "MORALE") + ".";

				public static LocString EFFECT_BABBLE = "{attrib}: {amount} (No One)";

				public static LocString EFFECT_BABBLE_TOOLTIP = "Duplicants will gain {amount} {attrib} if they chat only with themselves.";

				public static LocString EFFECT_CHAT = "{attrib}: {amount} (At least one Duplicant)";

				public static LocString EFFECT_CHAT_TOOLTIP = "Duplicants will gain {amount} {attrib} if they chat with at least one other Duplicant.";

				public static LocString EFFECT_LONG_DISTANCE = "{attrib}: {amount} (At least one Duplicant across space)";

				public static LocString EFFECT_LONG_DISTANCE_TOOLTIP = "Duplicants will gain {amount} {attrib} if they chat with at least one other Duplicant across space.";
			}

			public class MODULARLIQUIDINPUT
			{
				public static LocString NAME = UI.FormatAsLink("Liquid Input Hub", "MODULARLIQUIDINPUT");

				public static LocString DESC = "A hub from which to input " + UI.FormatAsLink("Liquids", "ELEMENTS_LIQUID") + ".";
			}

			public class MODULARSOLIDINPUT
			{
				public static LocString NAME = UI.FormatAsLink("Solid Input Hub", "MODULARSOLIDINPUT");

				public static LocString DESC = "A hub from which to input " + UI.FormatAsLink("Solids", "ELEMENTS_SOLID") + ".";
			}

			public class MODULARGASINPUT
			{
				public static LocString NAME = UI.FormatAsLink("Gas Input Hub", "MODULARGASINPUT");

				public static LocString DESC = "A hub from which to input " + UI.FormatAsLink("Gas", "ELEMENTS_GAS") + ".";
			}

			public class MECHANICALSURFBOARD
			{
				public static LocString NAME = UI.FormatAsLink("Mechanical Surfboard", "MECHANICALSURFBOARD");

				public static LocString DESC = "Mechanical waves make for radical relaxation time.";

				public static LocString EFFECT = string.Concat(new string[]
				{
					"Increases Duplicant ",
					UI.FormatAsLink("Morale", "MORALE"),
					".\n\nSome ",
					UI.FormatAsLink("Water", "WATER"),
					" gets splashed on the floor during use."
				});

				public static LocString WATER_REQUIREMENT = "{element}: {amount}";

				public static LocString WATER_REQUIREMENT_TOOLTIP = "This building must be filled with {amount} {element} in order to function.";

				public static LocString LEAK_REQUIREMENT = "Spillage: {amount}";

				public static LocString LEAK_REQUIREMENT_TOOLTIP = "This building will spill {amount} of its contents on to the floor during use, which must be replenished.";
			}

			public class SAUNA
			{
				public static LocString NAME = UI.FormatAsLink("Sauna", "SAUNA");

				public static LocString DESC = "A steamy sauna soothes away all the aches and pains.";

				public static LocString EFFECT = string.Concat(new string[]
				{
					"Uses ",
					UI.FormatAsLink("Steam", "STEAM"),
					" to create a relaxing atmosphere.\n\nIncreases Duplicant ",
					UI.FormatAsLink("Morale", "MORALE"),
					" and provides a lingering sense of warmth."
				});
			}

			public class BEACHCHAIR
			{
				public static LocString NAME = UI.FormatAsLink("Beach Chair", "BEACHCHAIR");

				public static LocString DESC = "Soak up some relaxing sun rays.";

				public static LocString EFFECT = string.Concat(new string[]
				{
					"Duplicants can relax by lounging in ",
					UI.FormatAsLink("Sunlight", "LIGHT"),
					".\n\nIncreases Duplicant ",
					UI.FormatAsLink("Morale", "MORALE"),
					"."
				});

				public static LocString LIGHTEFFECT_LOW = "{attrib}: {amount} (Dim Light)";

				public static LocString LIGHTEFFECT_LOW_TOOLTIP = "Duplicants will gain {amount} {attrib} if this building is in light dimmer than {lux}.";

				public static LocString LIGHTEFFECT_HIGH = "{attrib}: {amount} (Bright Light)";

				public static LocString LIGHTEFFECT_HIGH_TOOLTIP = "Duplicants will gain {amount} {attrib} if this building is in at least {lux} light.";
			}

			public class SUNLAMP
			{
				public static LocString NAME = UI.FormatAsLink("Sun Lamp", "SUNLAMP");

				public static LocString DESC = "An artificial ray of sunshine.";

				public static LocString EFFECT = string.Concat(new string[]
				{
					"Gives off ",
					UI.FormatAsLink("Sunlight", "LIGHT"),
					" level Lux.\n\nCan be paired with ",
					UI.FormatAsLink("Beach Chairs", "BEACHCHAIR"),
					"."
				});
			}

			public class VERTICALWINDTUNNEL
			{
				public static LocString NAME = UI.FormatAsLink("Vertical Wind Tunnel", "VERTICALWINDTUNNEL");

				public static LocString DESC = "Duplicants love the feeling of high-powered wind through their hair.";

				public static LocString EFFECT = string.Concat(new string[]
				{
					"Must be connected to a ",
					UI.FormatAsLink("Power Source", "POWER"),
					". To properly function, the area under this building must be left vacant.\n\nIncreases Duplicants ",
					UI.FormatAsLink("Morale", "MORALE"),
					"."
				});

				public static LocString DISPLACEMENTEFFECT = "Gas Displacement: {amount}";

				public static LocString DISPLACEMENTEFFECT_TOOLTIP = "This building will displace {amount} Gas while in use.";
			}

			public class TELEPORTALPAD
			{
				public static LocString NAME = "Teleporter Pad";

				public static LocString DESC = "Duplicants are just atoms as far as the pad's concerned.";

				public static LocString EFFECT = "Instantly transports Duplicants and items to another portal with the same portal code.";

				public static LocString LOGIC_PORT = "Portal Code Input";

				public static LocString LOGIC_PORT_ACTIVE = "1";

				public static LocString LOGIC_PORT_INACTIVE = "0";
			}

			public class CHECKPOINT
			{
				public static LocString NAME = UI.FormatAsLink("Duplicant Checkpoint", "CHECKPOINT");

				public static LocString DESC = "Checkpoints can be connected to automated sensors to determine when it's safe to enter.";

				public static LocString EFFECT = string.Concat(new string[]
				{
					"Allows Duplicants to pass when receiving a ",
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					".\n\nPrevents Duplicants from passing when receiving a ",
					UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby),
					"."
				});

				public static LocString LOGIC_PORT = "Duplicant Stop/Go";

				public static LocString LOGIC_PORT_ACTIVE = UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + ": Allow Duplicant passage";

				public static LocString LOGIC_PORT_INACTIVE = UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby) + ": Prevent Duplicant passage";
			}

			public class FIREPOLE
			{
				public static LocString NAME = UI.FormatAsLink("Fire Pole", "FIREPOLE");

				public static LocString DESC = "Build these in addition to ladders for efficient upward and downward movement.";

				public static LocString EFFECT = "Allows rapid Duplicant descent.\n\nSignificantly slows upward climbing.";
			}

			public class FLOORSWITCH
			{
				public static LocString NAME = UI.FormatAsLink("Weight Plate", "FLOORSWITCH");

				public static LocString DESC = "Weight plates can be used to turn on amenities only when Duplicants pass by.";

				public static LocString EFFECT = string.Concat(new string[]
				{
					"Sends a ",
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					" when an object or Duplicant is placed atop of it.\n\nCannot be triggered by ",
					UI.FormatAsLink("Gas", "ELEMENTS_GAS"),
					" or ",
					UI.FormatAsLink("Liquids", "ELEMENTS_LIQUID"),
					"."
				});

				public static LocString LOGIC_PORT_DESC = UI.FormatAsLink("Active", "LOGIC") + "/" + UI.FormatAsLink("Inactive", "LOGIC");
			}

			public class KILN
			{
				public static LocString NAME = UI.FormatAsLink("Kiln", "KILN");

				public static LocString DESC = "It gets quite hot.";

				public static LocString EFFECT = string.Concat(new string[]
				{
					"Fires ",
					UI.FormatAsLink("Clay", "CLAY"),
					" to produce ",
					UI.FormatAsLink("Ceramic", "CERAMIC"),
					", and ",
					UI.FormatAsLink("Coal", "CARBON"),
					" or ",
					UI.FormatAsLink("Wood", "WOOD"),
					" to produce ",
					UI.FormatAsLink("Refined Carbon", "REFINEDCARBON"),
					".\n\nDuplicants will not fabricate items unless recipes are queued."
				});
			}

			public class LIQUIDFUELTANK
			{
				public static LocString NAME = UI.FormatAsLink("Liquid Fuel Tank", "LIQUIDFUELTANK");

				public static LocString DESC = "Storing additional fuel increases the distance a rocket can travel before returning.";

				public static LocString EFFECT = "Stores the " + UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID") + " fuel piped into it to supply rocket engines.\n\nThe stored fuel type is determined by the rocket engine it is built upon.";
			}

			public class LIQUIDFUELTANKCLUSTER
			{
				public static LocString NAME = UI.FormatAsLink("Large Liquid Fuel Tank", "LIQUIDFUELTANKCLUSTER");

				public static LocString DESC = "Storing additional fuel increases the distance a rocket can travel before returning.";

				public static LocString EFFECT = string.Concat(new string[]
				{
					"Stores the ",
					UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID"),
					" fuel piped into it to supply rocket engines.\n\nThe stored fuel type is determined by the rocket engine it is built upon. \n\nMust be built via ",
					BUILDINGS.PREFABS.LAUNCHPAD.NAME,
					"."
				});
			}

			public class LANDING_POD
			{
				public static LocString NAME = "Spacefarer Deploy Pod";

				public static LocString DESC = "Geronimo!";

				public static LocString EFFECT = "Contains a Duplicant deployed from orbit.\n\nPod will disintegrate on arrival.";
			}

			public class ROCKETPOD
			{
				public static LocString NAME = UI.FormatAsLink("Trailblazer Deploy Pod", "ROCKETPOD");

				public static LocString DESC = "The Duplicant inside is equal parts nervous and excited.";

				public static LocString EFFECT = "Contains a Duplicant deployed from orbit by a " + BUILDINGS.PREFABS.PIONEERMODULE.NAME + ".\n\nPod will disintegrate on arrival.";
			}

			public class SCOUTROCKETPOD
			{
				public static LocString NAME = UI.FormatAsLink("Rover's Doghouse", "SCOUTROCKETPOD");

				public static LocString DESC = "Good luck out there, boy!";

				public static LocString EFFECT = string.Concat(new string[]
				{
					"Contains a ",
					UI.FormatAsLink("Rover", "SCOUT"),
					" deployed from an orbiting ",
					BUILDINGS.PREFABS.SCOUTMODULE.NAME,
					".\n\nPod will disintegrate on arrival."
				});
			}

			public class ROCKETCOMMANDCONSOLE
			{
				public static LocString NAME = UI.FormatAsLink("Rocket Cockpit", "ROCKETCOMMANDCONSOLE");

				public static LocString DESC = "Looks kinda fun.";

				public static LocString EFFECT = "Allows a Duplicant to pilot a rocket.\n\nCargo rockets must possess a Rocket Cockpit in order to function.";
			}

			public class ROCKETENVELOPETILE
			{
				public static LocString NAME = UI.FormatAsLink("Rocket", "ROCKETENVELOPETILE");

				public static LocString DESC = "Keeps the space out.";

				public static LocString EFFECT = "The walls of a rocket.";
			}

			public class ROCKETENVELOPEWINDOWTILE
			{
				public static LocString NAME = UI.FormatAsLink("Rocket Window", "ROCKETENVELOPEWINDOWTILE");

				public static LocString DESC = "I can see my asteroid from here!";

				public static LocString EFFECT = "The window of a rocket.";
			}

			public class ROCKETWALLTILE
			{
				public static LocString NAME = UI.FormatAsLink("Rocket Wall", "ROCKETENVELOPETILE");

				public static LocString DESC = "Keeps the space out.";

				public static LocString EFFECT = "The walls of a rocket.";
			}

			public class SMALLOXIDIZERTANK
			{
				public static LocString NAME = UI.FormatAsLink("Small Solid Oxidizer Tank", "SMALLOXIDIZERTANK");

				public static LocString DESC = "Solid oxidizers allows rocket fuel to be efficiently burned in the vacuum of space.";

				public static LocString EFFECT = string.Concat(new string[]
				{
					"Stores ",
					UI.FormatAsLink("Fertilizer", "Fertilizer"),
					" and ",
					UI.FormatAsLink("Oxylite", "OXYROCK"),
					" for burning rocket fuels. \n\nMust be built via ",
					BUILDINGS.PREFABS.LAUNCHPAD.NAME,
					"."
				});

				public static LocString UI_FILTER_CATEGORY = "Accepted Oxidizers";
			}

			public class OXIDIZERTANK
			{
				public static LocString NAME = UI.FormatAsLink("Solid Oxidizer Tank", "OXIDIZERTANK");

				public static LocString DESC = "Solid oxidizers allows rocket fuel to be efficiently burned in the vacuum of space.";

				public static LocString EFFECT = "Stores " + UI.FormatAsLink("Oxylite", "OXYROCK") + " and other oxidizers for burning rocket fuels.";

				public static LocString UI_FILTER_CATEGORY = "Accepted Oxidizers";
			}

			public class OXIDIZERTANKCLUSTER
			{
				public static LocString NAME = UI.FormatAsLink("Large Solid Oxidizer Tank", "OXIDIZERTANKCLUSTER");

				public static LocString DESC = "Solid oxidizers allows rocket fuel to be efficiently burned in the vacuum of space.";

				public static LocString EFFECT = string.Concat(new string[]
				{
					"Stores ",
					UI.FormatAsLink("Oxylite", "OXYROCK"),
					" and other oxidizers for burning rocket fuels.\n\nMust be built via ",
					BUILDINGS.PREFABS.LAUNCHPAD.NAME,
					"."
				});

				public static LocString UI_FILTER_CATEGORY = "Accepted Oxidizers";
			}

			public class OXIDIZERTANKLIQUID
			{
				public static LocString NAME = UI.FormatAsLink("Liquid Oxidizer Tank", "OXIDIZERTANKLIQUID");

				public static LocString DESC = "Liquid oxygen improves the thrust-to-mass ratio of rocket fuels.";

				public static LocString EFFECT = "Stores " + UI.FormatAsLink("Liquid Oxygen", "LIQUIDOXYGEN") + " for burning rocket fuels.";
			}

			public class OXIDIZERTANKLIQUIDCLUSTER
			{
				public static LocString NAME = UI.FormatAsLink("Liquid Oxidizer Tank", "OXIDIZERTANKLIQUIDCLUSTER");

				public static LocString DESC = "Liquid oxygen improves the thrust-to-mass ratio of rocket fuels.";

				public static LocString EFFECT = string.Concat(new string[]
				{
					"Stores ",
					UI.FormatAsLink("Liquid Oxygen", "LIQUIDOXYGEN"),
					" for burning rocket fuels. \n\nMust be built via ",
					BUILDINGS.PREFABS.LAUNCHPAD.NAME,
					"."
				});
			}

			public class LIQUIDCONDITIONER
			{
				public static LocString NAME = UI.FormatAsLink("Thermo Aquatuner", "LIQUIDCONDITIONER");

				public static LocString DESC = "A thermo aquatuner cools liquid and outputs the heat elsewhere.";

				public static LocString EFFECT = string.Concat(new string[]
				{
					"Cools the ",
					UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID"),
					" piped through it, but outputs ",
					UI.FormatAsLink("Heat", "HEAT"),
					" in its immediate vicinity."
				});
			}

			public class LIQUIDCARGOBAY
			{
				public static LocString NAME = UI.FormatAsLink("Liquid Cargo Tank", "LIQUIDCARGOBAY");

				public static LocString DESC = "Duplicants will fill cargo bays with any resources they find during space missions.";

				public static LocString EFFECT = "Allows Duplicants to store any " + UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID") + " resources found during space missions.\n\nStored resources become available to the colony upon the rocket's return.";
			}

			public class LIQUIDCARGOBAYCLUSTER
			{
				public static LocString NAME = UI.FormatAsLink("Large Liquid Cargo Tank", "LIQUIDCARGOBAY");

				public static LocString DESC = "Holds more than a regular cargo tank.";

				public static LocString EFFECT = string.Concat(new string[]
				{
					"Allows Duplicants to store most of the ",
					UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID"),
					" resources found during space missions.\n\nStored resources become available to the colony upon the rocket's return.\n\nMust be built via ",
					BUILDINGS.PREFABS.LAUNCHPAD.NAME,
					"."
				});
			}

			public class LIQUIDCARGOBAYSMALL
			{
				public static LocString NAME = UI.FormatAsLink("Liquid Cargo Tank", "LIQUIDCARGOBAYSMALL");

				public static LocString DESC = "Duplicants will fill cargo tanks with whatever resources they find during space missions.";

				public static LocString EFFECT = string.Concat(new string[]
				{
					"Allows Duplicants to store some of the ",
					UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID"),
					" resources found during space missions.\n\nStored resources become available to the colony upon the rocket's return. \n\nMust be built via ",
					BUILDINGS.PREFABS.LAUNCHPAD.NAME,
					"."
				});
			}

			public class LUXURYBED
			{
				public static LocString NAME = UI.FormatAsLink("Comfy Bed", "LUXURYBED");

				public static LocString DESC = "Duplicants prefer comfy beds to cots and wake up more rested after sleeping in them.";

				public static LocString EFFECT = "Provides a sleeping area for one Duplicant and restores additional stamina.\n\nDuplicants will automatically sleep in their assigned beds at night.";

				public class FACADES
				{
					public class DEFAULT_LUXURYBED
					{
						public static LocString NAME = UI.FormatAsLink("Comfy Bed", "LUXURYBED");

						public static LocString DESC = "Much comfier than a cot.";
					}

					public class GRANDPRIX
					{
						public static LocString NAME = UI.FormatAsLink("Grand Prix Bed", "LUXURYBED");

						public static LocString DESC = "Where every Duplicant wakes up a winner.";
					}

					public class BOAT
					{
						public static LocString NAME = UI.FormatAsLink("Dreamboat Bed", "LUXURYBED");

						public static LocString DESC = "Ahoy! Set sail for zzzzz's.";
					}

					public class ROCKET_BED
					{
						public static LocString NAME = UI.FormatAsLink("S.S. Napmaster Bed", "LUXURYBED");

						public static LocString DESC = "Launches sleepy Duplicants into a deep-space slumber.";
					}

					public class BOUNCY_BED
					{
						public static LocString NAME = UI.FormatAsLink("Bouncy Castle Bed", "LUXURYBED");

						public static LocString DESC = "An inflatable party prop makes a surprisingly good bed.";
					}

					public class PUFT_BED
					{
						public static LocString NAME = UI.FormatAsLink("Puft Bed", "LUXURYBED");

						public static LocString DESC = "A comfy, if somewhat 'fragrant', place to sleep.";
					}

					public class HAND
					{
						public static LocString NAME = UI.FormatAsLink("Cradled Bed", "LUXURYBED");

						public static LocString DESC = "It's so nice to be held.";
					}

					public class RUBIKS
					{
						public static LocString NAME = UI.FormatAsLink("Puzzle Cube Bed", "LUXURYBED");

						public static LocString DESC = "A little pattern recognition at bedtime soothes the mind.";
					}

					public class RED_ROSE
					{
						public static LocString NAME = UI.FormatAsLink("Comfy Puce Bed", "LUXURYBED");

						public static LocString DESC = "A pink-hued bed for rosy dreams.";
					}

					public class GREEN_MUSH
					{
						public static LocString NAME = UI.FormatAsLink("Comfy Mush Bed", "LUXURYBED");

						public static LocString DESC = "The mattress is so soft, it's almost impossible to climb out of.";
					}

					public class YELLOW_TARTAR
					{
						public static LocString NAME = UI.FormatAsLink("Comfy Ick Bed", "LUXURYBED");

						public static LocString DESC = "When life is icky, bed rest is the only answer.";
					}

					public class PURPLE_BRAINFAT
					{
						public static LocString NAME = UI.FormatAsLink("Comfy Fainting Bed", "LUXURYBED");

						public static LocString DESC = "A soft landing spot for swooners.";
					}
				}
			}

			public class LADDERBED
			{
				public static LocString NAME = UI.FormatAsLink("Ladder Bed", "LADDERBED");

				public static LocString DESC = "Duplicant's sleep will be interrupted if another Duplicant uses the ladder.";

				public static LocString EFFECT = "Provides a sleeping area for one Duplicant and also functions as a ladder.\n\nDuplicants will automatically sleep in their assigned beds at night.";
			}

			public class MEDICALCOT
			{
				public static LocString NAME = UI.FormatAsLink("Triage Cot", "MEDICALCOT");

				public static LocString DESC = "Duplicants use triage cots to recover from physical injuries and receive aid from peers.";

				public static LocString EFFECT = "Accelerates " + UI.FormatAsLink("Health", "HEALTH") + " restoration and the healing of physical injuries.\n\nRevives incapacitated Duplicants.";
			}

			public class DOCTORSTATION
			{
				public static LocString NAME = UI.FormatAsLink("Sick Bay", "DOCTORSTATION");

				public static LocString DESC = "Sick bays can be placed in hospital rooms to decrease the likelihood of disease spreading.";

				public static LocString EFFECT = "Allows Duplicants to administer basic treatments to sick Duplicants.\n\nDuplicants must possess the Bedside Manner " + UI.FormatAsLink("Skill", "ROLES") + " to treat peers.";
			}

			public class ADVANCEDDOCTORSTATION
			{
				public static LocString NAME = UI.FormatAsLink("Disease Clinic", "ADVANCEDDOCTORSTATION");

				public static LocString DESC = "Disease clinics require power, but treat more serious illnesses than sick bays alone.";

				public static LocString EFFECT = "Allows Duplicants to administer powerful treatments to sick Duplicants.\n\nDuplicants must possess the Advanced Medical Care " + UI.FormatAsLink("Skill", "ROLES") + " to treat peers.";
			}

			public class MASSAGETABLE
			{
				public static LocString NAME = UI.FormatAsLink("Massage Table", "MASSAGETABLE");

				public static LocString DESC = "Massage tables quickly reduce extreme stress, at the cost of power production.";

				public static LocString EFFECT = string.Concat(new string[]
				{
					"Rapidly reduces ",
					UI.FormatAsLink("Stress", "STRESS"),
					" for the Duplicant user.\n\nDuplicants will automatically seek a massage table when ",
					UI.FormatAsLink("Stress", "STRESS"),
					" exceeds breaktime range."
				});

				public static LocString ACTIVATE_TOOLTIP = "Duplicants must take a massage break when their " + UI.FormatAsKeyWord("Stress") + " reaches {0}%";

				public static LocString DEACTIVATE_TOOLTIP = "Breaktime ends when " + UI.FormatAsKeyWord("Stress") + " is reduced to {0}%";

				public class FACADES
				{
					public class DEFAULT_MASSAGETABLE
					{
						public static LocString NAME = UI.FormatAsLink("Massage Table", "MASSAGETABLE");

						public static LocString DESC = "Massage tables quickly reduce extreme stress, at the cost of power production.";
					}

					public class SHIATSU
					{
						public static LocString NAME = UI.FormatAsLink("Shiatsu Table", "MASSAGETABLE");

						public static LocString DESC = "Deep pressure for deep-seated stress.";
					}

					public class MASSEUR_BALLOON
					{
						public static LocString NAME = UI.FormatAsLink("Inflatable Massage Table", "MASSAGETABLE");

						public static LocString DESC = "Inflates well-being, deflates stress.";
					}
				}
			}

			public class CEILINGLIGHT
			{
				public static LocString NAME = UI.FormatAsLink("Ceiling Light", "CEILINGLIGHT");

				public static LocString DESC = "Light reduces Duplicant stress and is required to grow certain plants.";

				public static LocString EFFECT = string.Concat(new string[]
				{
					"Provides ",
					UI.FormatAsLink("Light", "LIGHT"),
					" when ",
					UI.FormatAsLink("Powered", "POWER"),
					".\n\nIncreases Duplicant workspeed within light radius."
				});

				public class FACADES
				{
					public class DEFAULT_CEILINGLIGHT
					{
						public static LocString NAME = UI.FormatAsLink("Ceiling Light", "CEILINGLIGHT");

						public static LocString DESC = "It does not go on the floor.";
					}

					public class LABFLASK
					{
						public static LocString NAME = UI.FormatAsLink("Lab Flask Ceiling Light", "CEILINGLIGHT");

						public static LocString DESC = "For best results, do not fill with liquids.";
					}

					public class FAUXPIPE
					{
						public static LocString NAME = UI.FormatAsLink("Faux Pipe Ceiling Light", "CEILINGLIGHT");

						public static LocString DESC = "The height of plumbing-inspired interior design.";
					}

					public class MINING
					{
						public static LocString NAME = UI.FormatAsLink("Mining Ceiling Light", "CEILINGLIGHT");

						public static LocString DESC = "The protective cage makes it the safest choice for underground parties.";
					}

					public class BLOSSOM
					{
						public static LocString NAME = UI.FormatAsLink("Blossom Ceiling Light", "CEILINGLIGHT");

						public static LocString DESC = "For Duplicants who can't keep real plants alive.";
					}

					public class POLKADOT
					{
						public static LocString NAME = UI.FormatAsLink("Polka Dot Ceiling Light", "CEILINGLIGHT");

						public static LocString DESC = "A fun lampshade for fun spaces.";
					}

					public class RUBIKS
					{
						public static LocString NAME = UI.FormatAsLink("Puzzle Cube Ceiling Light", "CEILINGLIGHT");

						public static LocString DESC = "The initials E.R. are sewn into the lampshade.";
					}
				}
			}

			public class MERCURYCEILINGLIGHT
			{
				public static LocString NAME = UI.FormatAsLink("Mercury Ceiling Light", "MERCURYCEILINGLIGHT");

				public static LocString DESC = "Mercury ceiling lights take a while to reach full brightness, but once they do...zowie!";

				public static LocString EFFECT = string.Concat(new string[]
				{
					"Uses ",
					UI.FormatAsLink("Mercury", "MERCURY"),
					" and ",
					UI.FormatAsLink("Power", "POWER"),
					" to produce ",
					UI.FormatAsLink("Light", "LIGHT"),
					".\n\nLight reduces Duplicant stress and is required to grow certain plants."
				});
			}

			public class AIRFILTER
			{
				public static LocString NAME = UI.FormatAsLink("Deodorizer", "AIRFILTER");

				public static LocString DESC = "Oh! Citrus scented!";

				public static LocString EFFECT = string.Concat(new string[]
				{
					"Uses ",
					UI.FormatAsLink("Sand", "SAND"),
					" to filter ",
					UI.FormatAsLink("Polluted Oxygen", "CONTAMINATEDOXYGEN"),
					" from the air, reducing ",
					UI.FormatAsLink("Disease", "DISEASE"),
					" spread."
				});
			}

			public class ARTIFACTANALYSISSTATION
			{
				public static LocString NAME = UI.FormatAsLink("Artifact Analysis Station", "ARTIFACTANALYSISSTATION");

				public static LocString DESC = "Discover the mysteries of the past.";

				public static LocString EFFECT = "Analyses and extracts " + UI.FormatAsLink("Neutronium", "UNOBTANIUM") + " from artifacts of interest.";

				public static LocString PAYLOAD_DROP_RATE = ITEMS.INDUSTRIAL_PRODUCTS.GENE_SHUFFLER_RECHARGE.NAME + " drop chance: {chance}";

				public static LocString PAYLOAD_DROP_RATE_TOOLTIP = "This artifact has a {chance} to drop a " + ITEMS.INDUSTRIAL_PRODUCTS.GENE_SHUFFLER_RECHARGE.NAME + " when analyzed at the " + BUILDINGS.PREFABS.ARTIFACTANALYSISSTATION.NAME;
			}

			public class CANVAS
			{
				public static LocString NAME = UI.FormatAsLink("Blank Canvas", "CANVAS");

				public static LocString DESC = "Once built, a Duplicant can paint a blank canvas to produce a decorative painting.";

				public static LocString EFFECT = string.Concat(new string[]
				{
					"Increases ",
					UI.FormatAsLink("Decor", "DECOR"),
					", contributing to ",
					UI.FormatAsLink("Morale", "MORALE"),
					".\n\nMust be painted by a Duplicant."
				});

				public static LocString POORQUALITYNAME = "Crude Painting";

				public static LocString AVERAGEQUALITYNAME = "Mediocre Painting";

				public static LocString EXCELLENTQUALITYNAME = "Masterpiece";

				public class FACADES
				{
					public class ART_A
					{
						public static LocString NAME = UI.FormatAsLink("Doodle Dee Duplicant", "ART_A");

						public static LocString DESC = "A sweet, amateurish interpretation of the Duplicant form.";
					}

					public class ART_B
					{
						public static LocString NAME = UI.FormatAsLink("Midnight Meal", "ART_B");

						public static LocString DESC = "The fast-food equivalent of high art.";
					}

					public class ART_C
					{
						public static LocString NAME = UI.FormatAsLink("Dupa Leesa", "ART_C");

						public static LocString DESC = "Some viewers swear they've seen it blink.";
					}

					public class ART_D
					{
						public static LocString NAME = UI.FormatAsLink("The Screech", "ART_D");

						public static LocString DESC = "If art could speak, this piece would be far less popular.";
					}

					public class ART_E
					{
						public static LocString NAME = UI.FormatAsLink("Fridup Kallo", "ART_E");

						public static LocString DESC = "Scratching and sniffing the flower yields no scent.";
					}

					public class ART_F
					{
						public static LocString NAME = UI.FormatAsLink("Moopoleon Bonafarte", "ART_F");

						public static LocString DESC = "Portrait of a leader astride their mighty steed.";
					}

					public class ART_G
					{
						public static LocString NAME = UI.FormatAsLink("Expressive Genius", "ART_G");

						public static LocString DESC = "The raw emotion conveyed here often renders viewers speechless.";
					}

					public class ART_H
					{
						public static LocString NAME = UI.FormatAsLink("The Smooch", "ART_H");

						public static LocString DESC = "A candid moment of affection between two organisms.";
					}

					public class ART_I
					{
						public static LocString NAME = UI.FormatAsLink("Self-Self-Self Portrait", "ART_I");

						public static LocString DESC = "A multi-layered exploration of the artist as a subject.";
					}

					public class ART_J
					{
						public static LocString NAME = UI.FormatAsLink("Nikola Devouring His Mush Bar", "ART_J");

						public static LocString DESC = "A painting that captures the true nature of hunger.";
					}

					public class ART_K
					{
						public static LocString NAME = UI.FormatAsLink("Sketchy Fungi", "ART_K");

						public static LocString DESC = "The perfect painting for dark, dank spaces.";
					}

					public class ART_L
					{
						public static LocString NAME = UI.FormatAsLink("Post-Ear Era", "ART_L");

						public static LocString DESC = "The furry hat helped keep the artist's bandage on.";
					}

					public class ART_M
					{
						public static LocString NAME = UI.FormatAsLink("Maternal Gaze", "ART_M");

						public static LocString DESC = "She's not angry, just disappointed.";
					}

					public class ART_O
					{
						public static LocString NAME = UI.FormatAsLink("Hands-On", "ART_O");

						public static LocString DESC = "It's all about cooperation, really.";
					}

					public class ART_N
					{
						public static LocString NAME = UI.FormatAsLink("Always Hope", "ART_N");

						public static LocString DESC = "Most Duplicants believe that the balloon in this image is about to be caught.";
					}

					public class ART_P
					{
						public static LocString NAME = UI.FormatAsLink("Pour Soul", "ART_P");

						public static LocString DESC = "It is a cruel guest who does not RSVP.";
					}

					public class ART_Q
					{
						public static LocString NAME = UI.FormatAsLink("Ore Else", "ART_Q");

						public static LocString DESC = "The only kind of gift that poorly behaved Duplicants can expect to receive.";
					}

					public class ART_R
					{
						public static LocString NAME = UI.FormatAsLink("Lazer Pipz", "ART_R");

						public static LocString DESC = "It combines two things that everyone loves: pips and lasers.";
					}
				}
			}

			public class CANVASWIDE
			{
				public static LocString NAME = UI.FormatAsLink("Landscape Canvas", "CANVASWIDE");

				public static LocString DESC = "Once built, a Duplicant can paint a blank canvas to produce a decorative painting.";

				public static LocString EFFECT = string.Concat(new string[]
				{
					"Moderately increases ",
					UI.FormatAsLink("Decor", "DECOR"),
					", contributing to ",
					UI.FormatAsLink("Morale", "MORALE"),
					".\n\nMust be painted by a Duplicant."
				});

				public static LocString POORQUALITYNAME = "Crude Painting";

				public static LocString AVERAGEQUALITYNAME = "Mediocre Painting";

				public static LocString EXCELLENTQUALITYNAME = "Masterpiece";

				public class FACADES
				{
					public class ART_WIDE_A
					{
						public static LocString NAME = UI.FormatAsLink("The Twins", "ART_WIDE_A");

						public static LocString DESC = "The effort is admirable, though the execution is not.";
					}

					public class ART_WIDE_B
					{
						public static LocString NAME = UI.FormatAsLink("Ground Zero", "ART_WIDE_B");

						public static LocString DESC = "Every story has its origin.";
					}

					public class ART_WIDE_C
					{
						public static LocString NAME = UI.FormatAsLink("Still Life with Barbeque and Frost Bun", "ART_WIDE_C");

						public static LocString DESC = "Food this good deserves to be immortalized.";
					}

					public class ART_WIDE_D
					{
						public static LocString NAME = UI.FormatAsLink("Composition with Three Colors", "ART_WIDE_D");

						public static LocString DESC = "All the other colors in the artist's palette had dried up.";
					}

					public class ART_WIDE_E
					{
						public static LocString NAME = UI.FormatAsLink("Behold, A Fork", "ART_WIDE_E");

						public static LocString DESC = "Each tine represents a branch of science.";
					}

					public class ART_WIDE_F
					{
						public static LocString NAME = UI.FormatAsLink("The Astronomer at Home", "ART_WIDE_F");

						public static LocString DESC = "Its companion piece, \"The Astronomer at Work\" was lost in a meteor shower.";
					}

					public class ART_WIDE_G
					{
						public static LocString NAME = UI.FormatAsLink("Iconic Iteration", "ART_WIDE_G");

						public static LocString DESC = "For the art collector who doesn't mind a bit of repetition.";
					}

					public class ART_WIDE_H
					{
						public static LocString NAME = UI.FormatAsLink("La Belle Meep", "ART_WIDE_H");

						public static LocString DESC = "A daring piece, guaranteed to cause a stir.";
					}

					public class ART_WIDE_I
					{
						public static LocString NAME = UI.FormatAsLink("Glorious Vole", "ART_WIDE_I");

						public static LocString DESC = "A moody study of the renowned tunneler.";
					}

					public class ART_WIDE_J
					{
						public static LocString NAME = UI.FormatAsLink("The Swell Swell", "ART_WIDE_J");

						public static LocString DESC = "As far as wave-themed art goes, it's great.";
					}

					public class ART_WIDE_K
					{
						public static LocString NAME = UI.FormatAsLink("Flight of the Slicksters", "ART_WIDE_K");

						public static LocString DESC = "The delight on the subjects' faces is contagious.";
					}

					public class ART_WIDE_L
					{
						public static LocString NAME = UI.FormatAsLink("The Shiny Night", "ART_WIDE_L");

						public static LocString DESC = "A dreamy abundance of swirls, whirls and whorls.";
					}

					public class ART_WIDE_M
					{
						public static LocString NAME = UI.FormatAsLink("Hot Afternoon", "ART_WIDE_M");

						public static LocString DESC = "Things get a bit melty if they're forgotten in the sun.";
					}

					public class ART_WIDE_O
					{
						public static LocString NAME = UI.FormatAsLink("Super Old Mural", "ART_WIDE_O");

						public static LocString DESC = "Even just exhaling nearby could damage this historical work.";
					}
				}
			}

			public class CANVASTALL
			{
				public static LocString NAME = UI.FormatAsLink("Portrait Canvas", "CANVASTALL");

				public static LocString DESC = "Once built, a Duplicant can paint a blank canvas to produce a decorative painting.";

				public static LocString EFFECT = string.Concat(new string[]
				{
					"Moderately increases ",
					UI.FormatAsLink("Decor", "DECOR"),
					", contributing to ",
					UI.FormatAsLink("Morale", "MORALE"),
					".\n\nMust be painted by a Duplicant."
				});

				public static LocString POORQUALITYNAME = "Crude Painting";

				public static LocString AVERAGEQUALITYNAME = "Mediocre Painting";

				public static LocString EXCELLENTQUALITYNAME = "Masterpiece";

				public class FACADES
				{
					public class ART_TALL_A
					{
						public static LocString NAME = UI.FormatAsLink("Ode to O2", "ART_TALL_A");

						public static LocString DESC = "Even amateur art is essential to life.";
					}

					public class ART_TALL_B
					{
						public static LocString NAME = UI.FormatAsLink("A Cool Wheeze", "ART_TALL_B");

						public static LocString DESC = "It certainly is colorful.";
					}

					public class ART_TALL_C
					{
						public static LocString NAME = UI.FormatAsLink("Luxe Splatter", "ART_TALL_C");

						public static LocString DESC = "Chaotic, yet compelling.";
					}

					public class ART_TALL_D
					{
						public static LocString NAME = UI.FormatAsLink("Pickled Meal Lice II", "ART_TALL_D");

						public static LocString DESC = "It doesn't have to taste good, it's art.";
					}

					public class ART_TALL_E
					{
						public static LocString NAME = UI.FormatAsLink("Fruit Face", "ART_TALL_E");

						public static LocString DESC = "Rumor has it that the model was self-conscious about their uneven eyebrows.";
					}

					public class ART_TALL_F
					{
						public static LocString NAME = UI.FormatAsLink("Girl with the Blue Scarf", "ART_TALL_F");

						public static LocString DESC = "The earring is nice too.";
					}

					public class ART_TALL_G
					{
						public static LocString NAME = UI.FormatAsLink("A Farewell at Sunrise", "ART_TALL_G");

						public static LocString DESC = "A poetic ink painting depicting the beginning of an end.";
					}

					public class ART_TALL_H
					{
						public static LocString NAME = UI.FormatAsLink("Conqueror of Clusters", "ART_TALL_H");

						public static LocString DESC = "The type of painting that ambitious Duplicants gravitate to.";
					}

					public class ART_TALL_I
					{
						public static LocString NAME = UI.FormatAsLink("Pei Phone", "ART_TALL_I");

						public static LocString DESC = "When the future calls, Duplicants answer.";
					}

					public class ART_TALL_J
					{
						public static LocString NAME = UI.FormatAsLink("Duplicants of the Galaxy", "ART_TALL_J");

						public static LocString DESC = "A poster for a blockbuster film that was never made.";
					}

					public class ART_TALL_K
					{
						public static LocString NAME = UI.FormatAsLink("Cubist Loo", "ART_TALL_K");

						public static LocString DESC = "The glass and frame are hydrophobic, for easy cleaning.";
					}

					public class ART_TALL_M
					{
						public static LocString NAME = UI.FormatAsLink("Do Not Disturb", "ART_TALL_M");

						public static LocString DESC = "No one likes being interrupted when they're waiting for inspiration to strike.";
					}

					public class ART_TALL_L
					{
						public static LocString NAME = UI.FormatAsLink("Mirror Ball", "ART_TALL_L");

						public static LocString DESC = "Nearby, a companion animal waited for the object to be thrown.";
					}

					public class ART_TALL_P
					{
						public static LocString NAME = "The Feast";

						public static LocString DESC = "There were greasy fingerprints on the canvas even before the paint had dried.";
					}
				}
			}

			public class CO2SCRUBBER
			{
				public static LocString NAME = UI.FormatAsLink("Carbon Skimmer", "CO2SCRUBBER");

				public static LocString DESC = "Skimmers remove large amounts of carbon dioxide, but produce no breathable air.";

				public static LocString EFFECT = string.Concat(new string[]
				{
					"Uses ",
					UI.FormatAsLink("Water", "WATER"),
					" to filter ",
					UI.FormatAsLink("Carbon Dioxide", "CARBONDIOXIDE"),
					" from the air."
				});
			}

			public class COMPOST
			{
				public static LocString NAME = UI.FormatAsLink("Compost", "COMPOST");

				public static LocString DESC = "Composts safely deal with biological waste, producing fresh dirt.";

				public static LocString EFFECT = string.Concat(new string[]
				{
					"Reduces ",
					UI.FormatAsLink("Polluted Dirt", "TOXICSAND"),
					", rotting ",
					UI.FormatAsLink("Foods", "FOOD"),
					", and discarded organics down into ",
					UI.FormatAsLink("Dirt", "DIRT"),
					"."
				});
			}

			public class COOKINGSTATION
			{
				public static LocString NAME = UI.FormatAsLink("Electric Grill", "COOKINGSTATION");

				public static LocString DESC = "Proper cooking eliminates foodborne disease and produces tasty, stress-relieving meals.";

				public static LocString EFFECT = "Cooks a wide variety of improved " + UI.FormatAsLink("Foods", "FOOD") + ".\n\nDuplicants will not fabricate items unless recipes are queued.";
			}

			public class CRYOTANK
			{
				public static LocString NAME = UI.FormatAsLink("Cryotank 3000", "CRYOTANK");

				public static LocString DESC = "The tank appears impossibly old, but smells crisp and brand new.\n\nA silhouette just barely visible through the frost of the glass.";

				public static LocString DEFROSTBUTTON = "Defrost Friend";

				public static LocString DEFROSTBUTTONTOOLTIP = "A new pal is just an icebreaker away";
			}

			public class GOURMETCOOKINGSTATION
			{
				public static LocString NAME = UI.FormatAsLink("Gas Range", "GOURMETCOOKINGSTATION");

				public static LocString DESC = "Luxury meals increase Duplicants' morale and prevents them from becoming stressed.";

				public static LocString EFFECT = "Cooks a wide variety of quality " + UI.FormatAsLink("Foods", "FOOD") + ".\n\nDuplicants will not fabricate items unless recipes are queued.";
			}

			public class DININGTABLE
			{
				public static LocString NAME = UI.FormatAsLink("Mess Table", "DININGTABLE");

				public static LocString DESC = "Duplicants prefer to dine at a table, rather than eat off the floor.";

				public static LocString EFFECT = "Gives one Duplicant a place to eat.\n\nDuplicants will automatically eat at their assigned table when hungry.";
			}

			public class DOOR
			{
				public static LocString NAME = UI.FormatAsLink("Pneumatic Door", "DOOR");

				public static LocString DESC = "Door controls can be used to prevent Duplicants from entering restricted areas.";

				public static LocString EFFECT = string.Concat(new string[]
				{
					"Encloses areas without blocking ",
					UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID"),
					" or ",
					UI.FormatAsLink("Gas", "ELEMENTS_GAS"),
					" flow.\n\nWild ",
					UI.FormatAsLink("Critters", "CREATURES"),
					" cannot pass through doors."
				});

				public static LocString PRESSURE_SUIT_REQUIRED = UI.FormatAsLink("Atmo Suit", "ATMO_SUIT") + " required {0}";

				public static LocString PRESSURE_SUIT_NOT_REQUIRED = UI.FormatAsLink("Atmo Suit", "ATMO_SUIT") + " not required {0}";

				public static LocString ABOVE = "above";

				public static LocString BELOW = "below";

				public static LocString LEFT = "on left";

				public static LocString RIGHT = "on right";

				public static LocString LOGIC_OPEN = "Open/Close";

				public static LocString LOGIC_OPEN_ACTIVE = UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + ": Open";

				public static LocString LOGIC_OPEN_INACTIVE = UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby) + ": Close and lock";

				public static class CONTROL_STATE
				{
					public class OPEN
					{
						public static LocString NAME = "Open";

						public static LocString TOOLTIP = "This door will remain open";
					}

					public class CLOSE
					{
						public static LocString NAME = "Lock";

						public static LocString TOOLTIP = "Nothing may pass through";
					}

					public class AUTO
					{
						public static LocString NAME = "Auto";

						public static LocString TOOLTIP = "Duplicants open and close this door as needed";
					}
				}
			}

			public class ELECTROBANKCHARGER
			{
				public static LocString NAME = UI.FormatAsLink("Power Bank Charger", "ELECTROBANKCHARGER");

				public static LocString DESC = "Bionic Duplicants rely on a steady supply of power to function.";

				public static LocString EFFECT = "Converts empty " + UI.FormatAsLink("Eco Power Banks", "ELECTROBANK") + " into fully charged units ready for reuse.";
			}

			public class SMALLELECTROBANKDISCHARGER
			{
				public static LocString NAME = UI.FormatAsLink("Compact Discharger", "SMALLELECTROBANKDISCHARGER");

				public static LocString DESC = "A small standalone power center that can be mounted on the floor or wall.";

				public static LocString EFFECT = "Converts stored energy from " + UI.FormatAsLink("Power Banks", "ELECTROBANK") + " into power for connected buildings.";
			}

			public class LARGEELECTROBANKDISCHARGER
			{
				public static LocString NAME = UI.FormatAsLink("Large Discharger", "LARGEELECTROBANKDISCHARGER");

				public static LocString DESC = "It's basically its own power grid.";

				public static LocString EFFECT = "Efficiently converts stored energy from " + UI.FormatAsLink("Power Banks", "ELECTROBANK") + " into power for connected buildings.";
			}

			public class ELECTROLYZER
			{
				public static LocString NAME = UI.FormatAsLink("Electrolyzer", "ELECTROLYZER");

				public static LocString DESC = "Water goes in one end, life sustaining oxygen comes out the other.";

				public static LocString EFFECT = string.Concat(new string[]
				{
					"Converts ",
					UI.FormatAsLink("Water", "WATER"),
					" into ",
					UI.FormatAsLink("Oxygen", "OXYGEN"),
					" and ",
					UI.FormatAsLink("Hydrogen Gas", "HYDROGEN"),
					".\n\nBecomes idle when the area reaches maximum pressure capacity."
				});
			}

			public class RUSTDEOXIDIZER
			{
				public static LocString NAME = UI.FormatAsLink("Rust Deoxidizer", "RUSTDEOXIDIZER");

				public static LocString DESC = "Rust and salt goes in, oxygen comes out.";

				public static LocString EFFECT = string.Concat(new string[]
				{
					"Converts ",
					UI.FormatAsLink("Rust", "RUST"),
					" into ",
					UI.FormatAsLink("Oxygen", "OXYGEN"),
					" and ",
					UI.FormatAsLink("Chlorine Gas", "CHLORINE"),
					".\n\nBecomes idle when the area reaches maximum pressure capacity."
				});
			}

			public class DESALINATOR
			{
				public static LocString NAME = UI.FormatAsLink("Desalinator", "DESALINATOR");

				public static LocString DESC = "Salt can be refined into table salt for a mealtime morale boost.";

				public static LocString EFFECT = string.Concat(new string[]
				{
					"Removes ",
					UI.FormatAsLink("Salt", "SALT"),
					" from ",
					UI.FormatAsLink("Brine", "BRINE"),
					" or ",
					UI.FormatAsLink("Salt Water", "SALTWATER"),
					", producing ",
					UI.FormatAsLink("Water", "WATER"),
					"."
				});
			}

			public class POWERTRANSFORMERSMALL
			{
				public static LocString NAME = UI.FormatAsLink("Power Transformer", "POWERTRANSFORMERSMALL");

				public static LocString DESC = "Limiting the power drawn by wires prevents them from incurring overload damage.";

				public static LocString EFFECT = string.Concat(new string[]
				{
					"Limits ",
					UI.FormatAsLink("Power", "POWER"),
					" flowing through the Transformer to 1000 W.\n\nConnect ",
					UI.FormatAsLink("Batteries", "BATTERY"),
					" on the large side to act as a valve and prevent ",
					UI.FormatAsLink("Wires", "WIRE"),
					" from drawing more than 1000 W.\n\nCan be rotated before construction."
				});
			}

			public class POWERTRANSFORMER
			{
				public static LocString NAME = UI.FormatAsLink("Large Power Transformer", "POWERTRANSFORMER");

				public static LocString DESC = "It's a power transformer, but larger.";

				public static LocString EFFECT = string.Concat(new string[]
				{
					"Limits ",
					UI.FormatAsLink("Power", "POWER"),
					" flowing through the Transformer to 4 kW.\n\nConnect ",
					UI.FormatAsLink("Batteries", "BATTERY"),
					" on the large side to act as a valve and prevent ",
					UI.FormatAsLink("Wires", "WIRE"),
					" from drawing more than 4 kW.\n\nCan be rotated before construction."
				});
			}

			public class FLOORLAMP
			{
				public static LocString NAME = UI.FormatAsLink("Lamp", "FLOORLAMP");

				public static LocString DESC = "Any building's light emitting radius can be viewed in the light overlay.";

				public static LocString EFFECT = string.Concat(new string[]
				{
					"Provides ",
					UI.FormatAsLink("Light", "LIGHT"),
					" when ",
					UI.FormatAsLink("Powered", "POWER"),
					".\n\nIncreases Duplicant workspeed within light radius."
				});

				public class FACADES
				{
					public class DEFAULT_FLOORLAMP
					{
						public static LocString NAME = UI.FormatAsLink("Lamp", "FLOORLAMP");

						public static LocString DESC = "Any building's light emitting radius can be viewed in the light overlay.";
					}

					public class LEG
					{
						public static LocString NAME = UI.FormatAsLink("Fragile Leg Lamp", "FLOORLAMP");

						public static LocString DESC = "This lamp blazes forth in unparalleled glory.";
					}

					public class BRISTLEBLOSSOM
					{
						public static LocString NAME = UI.FormatAsLink("Holiday Lamp", "FLOORLAMP");

						public static LocString DESC = "It's a bit prickly, but it casts a festive glow.";
					}
				}
			}

			public class FLOWERVASE
			{
				public static LocString NAME = UI.FormatAsLink("Flower Pot", "FLOWERVASE");

				public static LocString DESC = "Flower pots allow decorative plants to be moved to new locations.";

				public static LocString EFFECT = string.Concat(new string[]
				{
					"Houses a single ",
					UI.FormatAsLink("Plant", "PLANTS"),
					" when sown with a ",
					UI.FormatAsLink("Seed", "PLANTS"),
					".\n\nIncreases ",
					UI.FormatAsLink("Decor", "DECOR"),
					", contributing to ",
					UI.FormatAsLink("Morale", "MORALE"),
					"."
				});

				public class FACADES
				{
					public class DEFAULT_FLOWERVASE
					{
						public static LocString NAME = UI.FormatAsLink("Flower Pot", "FLOWERVASE");

						public static LocString DESC = "The original container for plants on the move.";
					}

					public class RETRO_SUNNY
					{
						public static LocString NAME = UI.FormatAsLink("Sunny Retro Flower Pot", "FLOWERVASE");

						public static LocString DESC = "A funky yellow flower pot for plants on the move.";
					}

					public class RETRO_BOLD
					{
						public static LocString NAME = UI.FormatAsLink("Bold Retro Flower Pot", "FLOWERVASE");

						public static LocString DESC = "A funky red flower pot for plants on the move.";
					}

					public class RETRO_BRIGHT
					{
						public static LocString NAME = UI.FormatAsLink("Bright Retro Flower Pot", "FLOWERVASE");

						public static LocString DESC = "A funky green flower pot for plants on the move.";
					}

					public class RETRO_DREAMY
					{
						public static LocString NAME = UI.FormatAsLink("Dreamy Retro Flower Pot", "FLOWERVASE");

						public static LocString DESC = "A funky blue flower pot for plants on the move.";
					}

					public class RETRO_ELEGANT
					{
						public static LocString NAME = UI.FormatAsLink("Elegant Retro Flower Pot", "FLOWERVASE");

						public static LocString DESC = "A funky white flower pot for plants on the move.";
					}
				}
			}

			public class FLOWERVASEWALL
			{
				public static LocString NAME = UI.FormatAsLink("Wall Pot", "FLOWERVASEWALL");

				public static LocString DESC = "Placing a plant in a wall pot can add a spot of Decor to otherwise bare walls.";

				public static LocString EFFECT = string.Concat(new string[]
				{
					"Houses a single ",
					UI.FormatAsLink("Plant", "PLANTS"),
					" when sown with a ",
					UI.FormatAsLink("Seed", "PLANTS"),
					".\n\nIncreases ",
					UI.FormatAsLink("Decor", "DECOR"),
					", contributing to ",
					UI.FormatAsLink("Morale", "MORALE"),
					".\n\nMust be hung from a wall."
				});

				public class FACADES
				{
					public class DEFAULT_FLOWERVASEWALL
					{
						public static LocString NAME = UI.FormatAsLink("Wall Pot", "FLOWERVASEWALL");

						public static LocString DESC = "Facilitates vertical plant displays.";
					}

					public class RETRO_GREEN
					{
						public static LocString NAME = UI.FormatAsLink("Bright Retro Wall Pot", "FLOWERVASEWALL");

						public static LocString DESC = "Vertical gardens are pretty nifty.";
					}

					public class RETRO_YELLOW
					{
						public static LocString NAME = UI.FormatAsLink("Sunny Retro Wall Pot", "FLOWERVASEWALL");

						public static LocString DESC = "Vertical gardens are pretty nifty.";
					}

					public class RETRO_RED
					{
						public static LocString NAME = UI.FormatAsLink("Bold Retro Wall Pot", "FLOWERVASEWALL");

						public static LocString DESC = "Vertical gardens are pretty nifty.";
					}

					public class RETRO_BLUE
					{
						public static LocString NAME = UI.FormatAsLink("Dreamy Retro Wall Pot", "FLOWERVASEWALL");

						public static LocString DESC = "Vertical gardens are pretty nifty.";
					}

					public class RETRO_WHITE
					{
						public static LocString NAME = UI.FormatAsLink("Elegant Retro Wall Pot", "FLOWERVASEWALL");

						public static LocString DESC = "Vertical gardens are pretty nifty.";
					}
				}
			}

			public class FLOWERVASEHANGING
			{
				public static LocString NAME = UI.FormatAsLink("Hanging Pot", "FLOWERVASEHANGING");

				public static LocString DESC = "Hanging pots can add some Decor to a room, without blocking buildings on the floor.";

				public static LocString EFFECT = string.Concat(new string[]
				{
					"Houses a single ",
					UI.FormatAsLink("Plant", "PLANTS"),
					" when sown with a ",
					UI.FormatAsLink("Seed", "PLANTS"),
					".\n\nIncreases ",
					UI.FormatAsLink("Decor", "DECOR"),
					", contributing to ",
					UI.FormatAsLink("Morale", "MORALE"),
					".\n\nMust be hung from a ceiling."
				});

				public class FACADES
				{
					public class RETRO_RED
					{
						public static LocString NAME = UI.FormatAsLink("Bold Hanging Pot", "FLOWERVASEHANGING");

						public static LocString DESC = "Suspended vessels really elevate a plant display.";
					}

					public class RETRO_GREEN
					{
						public static LocString NAME = UI.FormatAsLink("Bright Hanging Pot", "FLOWERVASEHANGING");

						public static LocString DESC = "Suspended vessels really elevate a plant display.";
					}

					public class RETRO_BLUE
					{
						public static LocString NAME = UI.FormatAsLink("Dreamy Hanging Pot", "FLOWERVASEHANGING");

						public static LocString DESC = "Suspended vessels really elevate a plant display.";
					}

					public class RETRO_YELLOW
					{
						public static LocString NAME = UI.FormatAsLink("Sunny Hanging Pot", "FLOWERVASEHANGING");

						public static LocString DESC = "Suspended vessels really elevate a plant display.";
					}

					public class RETRO_WHITE
					{
						public static LocString NAME = UI.FormatAsLink("Elegant Hanging Pot", "FLOWERVASEHANGING");

						public static LocString DESC = "Suspended vessels really elevate a plant display.";
					}

					public class BEAKER
					{
						public static LocString NAME = UI.FormatAsLink("Beaker Hanging Pot", "FLOWERVASEHANGING");

						public static LocString DESC = "A measured approach to indoor plant decor.";
					}

					public class RUBIKS
					{
						public static LocString NAME = UI.FormatAsLink("Puzzle Cube Hanging Pot", "FLOWERVASEHANGING");

						public static LocString DESC = "The real puzzle is how to keep indoor plants alive.";
					}
				}
			}

			public class FLOWERVASEHANGINGFANCY
			{
				public static LocString NAME = UI.FormatAsLink("Aero Pot", "FLOWERVASEHANGINGFANCY");

				public static LocString DESC = "Aero pots can be hung from the ceiling and have extremely high Decor.";

				public static LocString EFFECT = string.Concat(new string[]
				{
					"Houses a single ",
					UI.FormatAsLink("Plant", "PLANTS"),
					" when sown with a ",
					UI.FormatAsLink("Seed", "PLANTS"),
					".\n\nIncreases ",
					UI.FormatAsLink("Decor", "DECOR"),
					", contributing to ",
					UI.FormatAsLink("Morale", "MORALE"),
					".\n\nMust be hung from a ceiling."
				});

				public class FACADES
				{
				}
			}

			public class FLUSHTOILET
			{
				public static LocString NAME = UI.FormatAsLink("Lavatory", "FLUSHTOILET");

				public static LocString DESC = "Lavatories transmit fewer germs to Duplicants' skin and require no emptying.";

				public static LocString EFFECT = "Gives Duplicants a place to relieve themselves.\n\nSpreads very few " + UI.FormatAsLink("Germs", "DISEASE") + ".";

				public class FACADES
				{
					public class DEFAULT_FLUSHTOILET
					{
						public static LocString NAME = UI.FormatAsLink("Lavatory", "FLUSHTOILET");

						public static LocString DESC = "Lavatories transmit fewer germs to Duplicants' skin and require no emptying.";
					}

					public class POLKA_DARKPURPLERESIN
					{
						public static LocString NAME = UI.FormatAsLink("Mod Dot Lavatory", "FLUSHTOILET");

						public static LocString DESC = "For those who've really got to a-go-go.";
					}

					public class POLKA_DARKNAVYNOOKGREEN
					{
						public static LocString NAME = UI.FormatAsLink("Party Dot Lavatory", "FLUSHTOILET");

						public static LocString DESC = "Smooth moves happen here.";
					}

					public class PURPLE_BRAINFAT
					{
						public static LocString NAME = UI.FormatAsLink("Faint Purple Lavatory", "FLUSHTOILET");

						public static LocString DESC = "It's like pooping inside Hexalent fruit!";
					}

					public class YELLOW_TARTAR
					{
						public static LocString NAME = UI.FormatAsLink("Ick Yellow Lavatory", "FLUSHTOILET");

						public static LocString DESC = "Someone thought it'd be a good idea to have the outside match the inside.";
					}

					public class RED_ROSE
					{
						public static LocString NAME = UI.FormatAsLink("Puce Pink Lavatory", "FLUSHTOILET");

						public static LocString DESC = "The scented pink toilet paper smells like a rosebush in a sewage plant.";
					}

					public class GREEN_MUSH
					{
						public static LocString NAME = UI.FormatAsLink("Mush Green Lavatory", "FLUSHTOILET");

						public static LocString DESC = "Mush in, mush out.";
					}

					public class BLUE_BABYTEARS
					{
						public static LocString NAME = UI.FormatAsLink("Weepy Lavatory", "FLUSHTOILET");

						public static LocString DESC = "A private place to feel big feelings.";
					}
				}
			}

			public class SHOWER
			{
				public static LocString NAME = UI.FormatAsLink("Shower", "SHOWER");

				public static LocString DESC = "Regularly showering will prevent Duplicants spreading germs to the things they touch.";

				public static LocString EFFECT = string.Concat(new string[]
				{
					"Improves Duplicant ",
					UI.FormatAsLink("Morale", "MORALE"),
					" and removes surface ",
					UI.FormatAsLink("Germs", "DISEASE"),
					"."
				});
			}

			public class CONDUIT
			{
				public class STATUS_ITEM
				{
					public static LocString NAME = "Marked for Emptying";

					public static LocString TOOLTIP = "Awaiting a " + UI.FormatAsLink("Plumber", "PLUMBER") + " to clear this pipe";
				}
			}

			public class MORBROVERMAKER
			{
				public static LocString NAME = UI.FormatAsLink("Biobot Builder", "STORYTRAITMORBROVER");

				public static LocString DESC = "Allows a skilled Duplicant to manufacture a steady supply of icky yet effective bots.";

				public static LocString EFFECT = string.Concat(new string[]
				{
					"Uses ",
					UI.FormatAsLink("Zombie Spores", "ZOMBIESPORES"),
					" and ",
					UI.FormatAsLink("Steel", "STEEL"),
					" to craft biofueled machines that can be sent into hostile environments.\n\nDefunct ",
					UI.FormatAsLink("Biobots", "STORYTRAITMORBROVER"),
					" drop harvestable ",
					UI.FormatAsLink("Steel", "STEEL"),
					"."
				});
			}

			public class FOSSILDIG
			{
				public static LocString NAME = "Ancient Specimen";

				public static LocString DESC = "It's not from around here.";

				public static LocString EFFECT = "Contains a partial " + UI.FormatAsLink("Fossil", "FOSSIL") + " left behind by a giant critter.\n\nStudying the full skeleton could yield the information required to access a valuable new resource.";
			}

			public class FOSSILDIG_COMPLETED
			{
				public static LocString NAME = "Fossil Quarry";

				public static LocString DESC = "There sure are a lot of old bones in this area.";

				public static LocString EFFECT = "Contains a deep cache of harvestable " + UI.FormatAsLink("Fossils", "FOSSIL") + ".";
			}

			public class GAMMARAYOVEN
			{
				public static LocString NAME = UI.FormatAsLink("Gamma Ray Oven", "GAMMARAYOVEN");

				public static LocString DESC = "Nuke your food.";

				public static LocString EFFECT = "Cooks a variety of " + UI.FormatAsLink("Foods", "FOOD") + ".\n\nDuplicants will not fabricate items unless recipes are queued.";
			}

			public class GASCARGOBAY
			{
				public static LocString NAME = UI.FormatAsLink("Gas Cargo Canister", "GASCARGOBAY");

				public static LocString DESC = "Duplicants will fill cargo bays with any resources they find during space missions.";

				public static LocString EFFECT = "Allows Duplicants to store any " + UI.FormatAsLink("Gas", "ELEMENTS_GAS") + " resources found during space missions.\n\nStored resources become available to the colony upon the rocket's return.";
			}

			public class GASCARGOBAYCLUSTER
			{
				public static LocString NAME = UI.FormatAsLink("Large Gas Cargo Canister", "GASCARGOBAY");

				public static LocString DESC = "Holds more than a typical gas cargo canister.";

				public static LocString EFFECT = string.Concat(new string[]
				{
					"Allows Duplicants to store most of the ",
					UI.FormatAsLink("Gas", "ELEMENTS_GAS"),
					" resources found during space missions.\n\nStored resources become available to the colony upon the rocket's return.\n\nMust be built via ",
					BUILDINGS.PREFABS.LAUNCHPAD.NAME,
					"."
				});
			}

			public class GASCARGOBAYSMALL
			{
				public static LocString NAME = UI.FormatAsLink("Gas Cargo Canister", "GASCARGOBAYSMALL");

				public static LocString DESC = "Duplicants fill cargo canisters with any resources they find during space missions.";

				public static LocString EFFECT = string.Concat(new string[]
				{
					"Allows Duplicants to store some of the ",
					UI.FormatAsLink("Gas", "ELEMENTS_GAS"),
					" resources found during space missions.\n\nStored resources become available to the colony upon the rocket's return. \n\nMust be built via ",
					BUILDINGS.PREFABS.LAUNCHPAD.NAME,
					"."
				});
			}

			public class GASCONDUIT
			{
				public static LocString NAME = UI.FormatAsLink("Gas Pipe", "GASCONDUIT");

				public static LocString DESC = "Gas pipes are used to connect the inputs and outputs of ventilated buildings.";

				public static LocString EFFECT = string.Concat(new string[]
				{
					"Carries ",
					UI.FormatAsLink("Gas", "ELEMENTS_GAS"),
					" between ",
					UI.FormatAsLink("Outputs", "GASPIPING"),
					" and ",
					UI.FormatAsLink("Intakes", "GASPIPING"),
					".\n\nCan be run through wall and floor tile."
				});
			}

			public class GASCONDUITBRIDGE
			{
				public static LocString NAME = UI.FormatAsLink("Gas Bridge", "GASCONDUITBRIDGE");

				public static LocString DESC = "Separate pipe systems prevent mingled contents from causing building damage.";

				public static LocString EFFECT = "Runs one " + UI.FormatAsLink("Gas Pipe", "GASPIPING") + " section over another without joining them.\n\nCan be run through wall and floor tile.";
			}

			public class GASCONDUITPREFERENTIALFLOW
			{
				public static LocString NAME = UI.FormatAsLink("Priority Gas Flow", "GASCONDUITPREFERENTIALFLOW");

				public static LocString DESC = "Priority flows ensure important buildings are filled first when on a system with other buildings.";

				public static LocString EFFECT = "Diverts " + UI.FormatAsLink("Gas", "ELEMENTS_GAS") + " to a secondary input when its primary input overflows.";
			}

			public class LIQUIDCONDUITPREFERENTIALFLOW
			{
				public static LocString NAME = UI.FormatAsLink("Priority Liquid Flow", "LIQUIDCONDUITPREFERENTIALFLOW");

				public static LocString DESC = "Priority flows ensure important buildings are filled first when on a system with other buildings.";

				public static LocString EFFECT = "Diverts " + UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID") + " to a secondary input when its primary input overflows.";
			}

			public class GASCONDUITOVERFLOW
			{
				public static LocString NAME = UI.FormatAsLink("Gas Overflow Valve", "GASCONDUITOVERFLOW");

				public static LocString DESC = "Overflow valves can be used to prioritize which buildings should receive precious resources first.";

				public static LocString EFFECT = "Fills a secondary" + UI.FormatAsLink("Gas", "ELEMENTS_GAS") + " output only when its primary output is blocked.";
			}

			public class LIQUIDCONDUITOVERFLOW
			{
				public static LocString NAME = UI.FormatAsLink("Liquid Overflow Valve", "LIQUIDCONDUITOVERFLOW");

				public static LocString DESC = "Overflow valves can be used to prioritize which buildings should receive precious resources first.";

				public static LocString EFFECT = "Fills a secondary" + UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID") + " output only when its primary output is blocked.";
			}

			public class LAUNCHPAD
			{
				public static LocString NAME = UI.FormatAsLink("Rocket Platform", "LAUNCHPAD");

				public static LocString DESC = "A platform from which rockets can be launched and on which they can land.";

				public static LocString EFFECT = "Precursor to construction of all other Rocket modules.\n\nAllows Rockets to launch from or land on the host Planetoid.\n\nAutomatically links up to " + BUILDINGS.PREFABS.MODULARLAUNCHPADPORT.NAME + UI.FormatAsLink("s", "MODULARLAUNCHPADPORTSOLID") + " built to either side of the platform.";

				public static LocString LOGIC_PORT_READY = "Rocket Checklist";

				public static LocString LOGIC_PORT_READY_ACTIVE = "Sends a " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + " when its rocket is ready for flight";

				public static LocString LOGIC_PORT_READY_INACTIVE = "Otherwise, sends a " + UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby);

				public static LocString LOGIC_PORT_LANDED_ROCKET = "Landed Rocket";

				public static LocString LOGIC_PORT_LANDED_ROCKET_ACTIVE = "Sends a " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + " when its rocket is on the " + BUILDINGS.PREFABS.LAUNCHPAD.NAME;

				public static LocString LOGIC_PORT_LANDED_ROCKET_INACTIVE = "Otherwise, sends a " + UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby);

				public static LocString LOGIC_PORT_LAUNCH = "Launch Rocket";

				public static LocString LOGIC_PORT_LAUNCH_ACTIVE = UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + ": Launch rocket";

				public static LocString LOGIC_PORT_LAUNCH_INACTIVE = UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby) + ": Cancel launch";
			}

			public class GASFILTER
			{
				public static LocString NAME = UI.FormatAsLink("Gas Filter", "GASFILTER");

				public static LocString DESC = "All gases are sent into the building's output pipe, except the gas chosen for filtering.";

				public static LocString EFFECT = string.Concat(new string[]
				{
					"Sieves one ",
					UI.FormatAsLink("Gas", "ELEMENTS_GAS"),
					" from the air, sending it into a dedicated ",
					UI.FormatAsLink("Pipe", "GASPIPING"),
					"."
				});

				public static LocString STATUS_ITEM = "Filters: {0}";

				public static LocString ELEMENT_NOT_SPECIFIED = "Not Specified";
			}

			public class SOLIDFILTER
			{
				public static LocString NAME = UI.FormatAsLink("Solid Filter", "SOLIDFILTER");

				public static LocString DESC = "All solids are sent into the building's output conveyor, except the solid chosen for filtering.";

				public static LocString EFFECT = string.Concat(new string[]
				{
					"Separates one ",
					UI.FormatAsLink("Solid Material", "ELEMENTS_SOLID"),
					" from the conveyor, sending it into a dedicated ",
					BUILDINGS.PREFABS.SOLIDCONDUIT.NAME,
					"."
				});

				public static LocString STATUS_ITEM = "Filters: {0}";

				public static LocString ELEMENT_NOT_SPECIFIED = "Not Specified";
			}

			public class GASPERMEABLEMEMBRANE
			{
				public static LocString NAME = UI.FormatAsLink("Airflow Tile", "GASPERMEABLEMEMBRANE");

				public static LocString DESC = "Building with airflow tile promotes better gas circulation within a colony.";

				public static LocString EFFECT = string.Concat(new string[]
				{
					"Used to build the walls and floors of rooms.\n\nBlocks ",
					UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID"),
					" flow without obstructing ",
					UI.FormatAsLink("Gas", "ELEMENTS_GAS"),
					"."
				});
			}

			public class DEVPUMPGAS
			{
				public static LocString NAME = "Dev Pump Gas";

				public static LocString DESC = "Piping a pump's output to a building's intake will send gas to that building.";

				public static LocString EFFECT = string.Concat(new string[]
				{
					"Draws in ",
					UI.FormatAsLink("Gas", "ELEMENTS_GAS"),
					" and runs it through ",
					UI.FormatAsLink("Pipes", "GASPIPING"),
					".\n\nMust be immersed in ",
					UI.FormatAsLink("Gas", "ELEMENTS_GAS"),
					"."
				});
			}

			public class GASPUMP
			{
				public static LocString NAME = UI.FormatAsLink("Gas Pump", "GASPUMP");

				public static LocString DESC = "Piping a pump's output to a building's intake will send gas to that building.";

				public static LocString EFFECT = string.Concat(new string[]
				{
					"Draws in ",
					UI.FormatAsLink("Gas", "ELEMENTS_GAS"),
					" and runs it through ",
					UI.FormatAsLink("Pipes", "GASPIPING"),
					".\n\nMust be immersed in ",
					UI.FormatAsLink("Gas", "ELEMENTS_GAS"),
					"."
				});
			}

			public class GASMINIPUMP
			{
				public static LocString NAME = UI.FormatAsLink("Mini Gas Pump", "GASMINIPUMP");

				public static LocString DESC = "Mini pumps are useful for moving small quantities of gas with minimum power.";

				public static LocString EFFECT = string.Concat(new string[]
				{
					"Draws in a small amount of ",
					UI.FormatAsLink("Gas", "ELEMENTS_GAS"),
					" and runs it through ",
					UI.FormatAsLink("Pipes", "GASPIPING"),
					".\n\nMust be immersed in ",
					UI.FormatAsLink("Gas", "ELEMENTS_GAS"),
					"."
				});
			}

			public class GASVALVE
			{
				public static LocString NAME = UI.FormatAsLink("Gas Valve", "GASVALVE");

				public static LocString DESC = "Valves control the amount of gas that moves through pipes, preventing waste.";

				public static LocString EFFECT = string.Concat(new string[]
				{
					"Controls the ",
					UI.FormatAsLink("Gas", "ELEMENTS_GAS"),
					" volume permitted through ",
					UI.FormatAsLink("Pipes", "GASPIPING"),
					"."
				});
			}

			public class GASLOGICVALVE
			{
				public static LocString NAME = UI.FormatAsLink("Gas Shutoff", "GASLOGICVALVE");

				public static LocString DESC = "Automated piping saves power and time by removing the need for Duplicant input.";

				public static LocString EFFECT = string.Concat(new string[]
				{
					"Connects to an ",
					UI.FormatAsLink("Automation", "LOGIC"),
					" grid to automatically turn ",
					UI.FormatAsLink("Gas", "ELEMENTS_GAS"),
					" flow on or off."
				});

				public static LocString LOGIC_PORT = "Open/Close";

				public static LocString LOGIC_PORT_ACTIVE = UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + ": Allow gas flow";

				public static LocString LOGIC_PORT_INACTIVE = UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby) + ": Prevent gas flow";
			}

			public class GASLIMITVALVE
			{
				public static LocString NAME = UI.FormatAsLink("Gas Meter Valve", "GASLIMITVALVE");

				public static LocString DESC = "Meter Valves let an exact amount of gas pass through before shutting off.";

				public static LocString EFFECT = string.Concat(new string[]
				{
					"Connects to an ",
					UI.FormatAsLink("Automation", "LOGIC"),
					" grid to automatically turn ",
					UI.FormatAsLink("Gas", "ELEMENTS_GAS"),
					" flow off when the specified amount has passed through it."
				});

				public static LocString LOGIC_PORT_OUTPUT = "Limit Reached";

				public static LocString OUTPUT_PORT_ACTIVE = "Sends a " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + " if limit has been reached";

				public static LocString OUTPUT_PORT_INACTIVE = "Otherwise, sends a " + UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby);

				public static LocString LOGIC_PORT_RESET = "Reset Meter";

				public static LocString RESET_PORT_ACTIVE = UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + ": Reset the amount";

				public static LocString RESET_PORT_INACTIVE = UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby) + ": Nothing";
			}

			public class GASVENT
			{
				public static LocString NAME = UI.FormatAsLink("Gas Vent", "GASVENT");

				public static LocString DESC = "Vents are an exit point for gases from ventilation systems.";

				public static LocString EFFECT = string.Concat(new string[]
				{
					"Releases ",
					UI.FormatAsLink("Gas", "ELEMENTS_GAS"),
					" from ",
					UI.FormatAsLink("Gas Pipes", "GASPIPING"),
					"."
				});
			}

			public class GASVENTHIGHPRESSURE
			{
				public static LocString NAME = UI.FormatAsLink("High Pressure Gas Vent", "GASVENTHIGHPRESSURE");

				public static LocString DESC = "High pressure vents can expel gas into more highly pressurized environments.";

				public static LocString EFFECT = string.Concat(new string[]
				{
					"Releases ",
					UI.FormatAsLink("Gas", "ELEMENTS_GAS"),
					" from ",
					UI.FormatAsLink("Gas Pipes", "GASPIPING"),
					" into high pressure locations."
				});
			}

			public class GASBOTTLER
			{
				public static LocString NAME = UI.FormatAsLink("Canister Filler", "GASBOTTLER");

				public static LocString DESC = "Canisters allow Duplicants to manually deliver gases from place to place.";

				public static LocString EFFECT = "Automatically stores piped " + UI.FormatAsLink("Gases", "ELEMENTS_GAS") + " into canisters for manual transport.";
			}

			public class LIQUIDBOTTLER
			{
				public static LocString NAME = UI.FormatAsLink("Bottle Filler", "LIQUIDBOTTLER");

				public static LocString DESC = "Bottle fillers allow Duplicants to manually deliver liquids from place to place.";

				public static LocString EFFECT = "Automatically stores piped " + UI.FormatAsLink("Liquids", "ELEMENTS_LIQUID") + " into bottles for manual transport.";
			}

			public class GENERATOR
			{
				public static LocString NAME = UI.FormatAsLink("Coal Generator", "GENERATOR");

				public static LocString DESC = "Burning coal produces more energy than manual power, but emits heat and exhaust.";

				public static LocString EFFECT = string.Concat(new string[]
				{
					"Converts ",
					UI.FormatAsLink("Coal", "CARBON"),
					" into electrical ",
					UI.FormatAsLink("Power", "POWER"),
					".\n\nProduces ",
					UI.FormatAsLink("Carbon Dioxide", "CARBONDIOXIDE"),
					"."
				});

				public static LocString OVERPRODUCTION = "{Generator} overproduction";
			}

			public class GENETICANALYSISSTATION
			{
				public static LocString NAME = UI.FormatAsLink("Botanical Analyzer", "GENETICANALYSISSTATION");

				public static LocString DESC = "Would a mutated rose still smell as sweet?";

				public static LocString EFFECT = "Identifies new " + UI.FormatAsLink("Seed", "PLANTS") + " subspecies.";
			}

			public class DEVGENERATOR
			{
				public static LocString NAME = "Dev Generator";

				public static LocString DESC = "Runs on coffee.";

				public static LocString EFFECT = "Generates testing power for late nights.";
			}

			public class DEVLIFESUPPORT
			{
				public static LocString NAME = "Dev Life Support";

				public static LocString DESC = "Keeps Duplicants cozy and breathing.";

				public static LocString EFFECT = "Generates warm, oxygen-rich air.";
			}

			public class DEVLIGHTGENERATOR
			{
				public static LocString NAME = "Dev Light Source";

				public static LocString DESC = "Brightens up a dev's darkest hours.";

				public static LocString EFFECT = "Generates dimmable light on demand.";

				public static LocString FALLOFF_LABEL = "Falloff Rate";

				public static LocString BRIGHTNESS_LABEL = "Brightness";

				public static LocString RANGE_LABEL = "Range";
			}

			public class DEVRADIATIONGENERATOR
			{
				public static LocString NAME = "Dev Radiation Emitter";

				public static LocString DESC = "That's some <i>strong</i> coffee.";

				public static LocString EFFECT = "Generates on-demand radiation to keep things clear. <i>Nu-</i>clear.";
			}

			public class DEVHEATER
			{
				public static LocString NAME = "Dev Heater";

				public static LocString DESC = "Did someone touch the thermostat?";

				public static LocString EFFECT = "Generates on-demand heat for testing toastiness.";
			}

			public class GENERICFABRICATOR
			{
				public static LocString NAME = UI.FormatAsLink("Omniprinter", "GENERICFABRICATOR");

				public static LocString DESC = "Omniprinters are incapable of printing organic matter.";

				public static LocString EFFECT = "Converts " + UI.FormatAsLink("Raw Mineral", "RAWMINERAL") + " into unique materials and objects.";
			}

			public class GEOTUNER
			{
				public static LocString NAME = UI.FormatAsLink("Geotuner", "GEOTUNER");

				public static LocString DESC = "The targeted geyser receives stored amplification data when it is erupting.";

				public static LocString EFFECT = string.Concat(new string[]
				{
					"Increases the ",
					UI.FormatAsLink("Temperature", "HEAT"),
					" and output of an analyzed ",
					UI.FormatAsLink("Geyser", "GEYSERS"),
					".\n\nMultiple Geotuners can be directed at a single ",
					UI.FormatAsLink("Geyser", "GEYSERS"),
					" anywhere on an asteroid."
				});

				public static LocString LOGIC_PORT = "Geyser Eruption Monitor";

				public static LocString LOGIC_PORT_ACTIVE = "Sends a " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + " when geyser is erupting";

				public static LocString LOGIC_PORT_INACTIVE = "Otherwise, sends a " + UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby);
			}

			public class GRAVE
			{
				public static LocString NAME = UI.FormatAsLink("Tasteful Memorial", "GRAVE");

				public static LocString DESC = "Burying dead Duplicants reduces health hazards and stress on the colony.";

				public static LocString EFFECT = "Provides a final resting place for deceased Duplicants.\n\nLiving Duplicants will automatically place an unburied corpse inside.";
			}

			public class HEADQUARTERS
			{
				public static LocString NAME = UI.FormatAsLink("Printing Pod", "HEADQUARTERS");

				public static LocString DESC = "New Duplicants come out here, but thank goodness, they never go back in.";

				public static LocString EFFECT = "An exceptionally advanced bioprinter of unknown origin.\n\nIt periodically produces new Duplicants or care packages containing resources.";
			}

			public class HYDROGENGENERATOR
			{
				public static LocString NAME = UI.FormatAsLink("Hydrogen Generator", "HYDROGENGENERATOR");

				public static LocString DESC = "Hydrogen generators are extremely efficient, emitting next to no waste.";

				public static LocString EFFECT = string.Concat(new string[]
				{
					"Converts ",
					UI.FormatAsLink("Hydrogen Gas", "HYDROGEN"),
					" into electrical ",
					UI.FormatAsLink("Power", "POWER"),
					"."
				});
			}

			public class METHANEGENERATOR
			{
				public static LocString NAME = UI.FormatAsLink("Natural Gas Generator", "METHANEGENERATOR");

				public static LocString DESC = "Natural gas generators leak polluted water and are best built above a waste reservoir.";

				public static LocString EFFECT = string.Concat(new string[]
				{
					"Converts ",
					UI.FormatAsLink("Natural Gas", "METHANE"),
					" into electrical ",
					UI.FormatAsLink("Power", "POWER"),
					".\n\nProduces ",
					UI.FormatAsLink("Carbon Dioxide", "CARBONDIOXIDE"),
					" and ",
					UI.FormatAsLink("Polluted Water", "DIRTYWATER"),
					"."
				});
			}

			public class NUCLEARREACTOR
			{
				public static LocString NAME = UI.FormatAsLink("Research Reactor", "NUCLEARREACTOR");

				public static LocString DESC = "Radbolt generators and reflectors make radiation useable by other buildings.";

				public static LocString EFFECT = string.Concat(new string[]
				{
					"Uses ",
					UI.FormatAsLink("Enriched Uranium", "ENRICHEDURANIUM"),
					" to produce ",
					UI.FormatAsLink("Radiation", "RADIATION"),
					" for Radbolt production.\n\nGenerates a massive amount of ",
					UI.FormatAsLink("Heat", "HEAT"),
					". Overheating will result in an explosive meltdown."
				});

				public static LocString LOGIC_PORT = "Fuel Delivery Control";

				public static LocString INPUT_PORT_ACTIVE = "Fuel Delivery Enabled";

				public static LocString INPUT_PORT_INACTIVE = "Fuel Delivery Disabled";
			}

			public class WOODGASGENERATOR
			{
				public static LocString NAME = UI.FormatAsLink("Wood Burner", "WOODGASGENERATOR");

				public static LocString DESC = "Wood burners are small and easy to maintain, but produce a fair amount of heat.";

				public static LocString EFFECT = string.Concat(new string[]
				{
					"Burns ",
					UI.FormatAsLink("Wood", "WOOD"),
					" to produce electrical ",
					UI.FormatAsLink("Power", "POWER"),
					".\n\nProduces ",
					UI.FormatAsLink("Carbon Dioxide", "CARBONDIOXIDE"),
					" and ",
					UI.FormatAsLink("Heat", "HEAT"),
					"."
				});
			}

			public class PEATGENERATOR
			{
				public static LocString NAME = UI.FormatAsLink("Peat Burner", "PEATGENERATOR");

				public static LocString DESC = "It gives off an aroma that some Duplicants find inexplicably nostalgic.";

				public static LocString EFFECT = string.Concat(new string[]
				{
					"Burns ",
					UI.FormatAsLink("Peat", "PEAT"),
					" to produce electrical ",
					UI.FormatAsLink("Power", "POWER"),
					".\n\nProduces a small amount of ",
					UI.FormatAsLink("Carbon Dioxide", "CARBONDIOXIDE"),
					" and ",
					ELEMENTS.DIRTYWATER.NAME,
					"."
				});
			}

			public class PETROLEUMGENERATOR
			{
				public static LocString NAME = UI.FormatAsLink("Petroleum Generator", "PETROLEUMGENERATOR");

				public static LocString DESC = "Petroleum generators have a high energy output but produce a great deal of waste.";

				public static LocString EFFECT = string.Concat(new string[]
				{
					"Converts ",
					UI.FormatAsLink("Petroleum", "PETROLEUM"),
					", ",
					UI.FormatAsLink("Ethanol", "ETHANOL"),
					" or ",
					UI.FormatAsLink("Biodiesel", "REFINEDLIPID"),
					" into electrical ",
					UI.FormatAsLink("Power", "POWER"),
					".\n\nProduces ",
					UI.FormatAsLink("Carbon Dioxide", "CARBONDIOXIDE"),
					" and ",
					UI.FormatAsLink("Polluted Water", "DIRTYWATER"),
					"."
				});
			}

			public class HYDROPONICFARM
			{
				public static LocString NAME = UI.FormatAsLink("Hydroponic Farm", "HYDROPONICFARM");

				public static LocString DESC = "Hydroponic farms reduce Duplicant traffic by automating irrigating crops.";

				public static LocString EFFECT = string.Concat(new string[]
				{
					"Grows one ",
					UI.FormatAsLink("Plant", "PLANTS"),
					" from a ",
					UI.FormatAsLink("Seed", "PLANTS"),
					".\n\nCan be used as floor tile and rotated before construction.\n\nMust be irrigated through ",
					UI.FormatAsLink("Liquid Piping", "LIQUIDPIPING"),
					"."
				});
			}

			public class INSULATEDGASCONDUIT
			{
				public static LocString NAME = UI.FormatAsLink("Insulated Gas Pipe", "INSULATEDGASCONDUIT");

				public static LocString DESC = "Pipe insulation prevents gas contents from significantly changing temperature in transit.";

				public static LocString EFFECT = string.Concat(new string[]
				{
					"Carries ",
					UI.FormatAsLink("Gas", "ELEMENTS_GAS"),
					" with minimal change in ",
					UI.FormatAsLink("Temperature", "HEAT"),
					".\n\nCan be run through wall and floor tile."
				});
			}

			public class GASCONDUITRADIANT
			{
				public static LocString NAME = UI.FormatAsLink("Radiant Gas Pipe", "GASCONDUITRADIANT");

				public static LocString DESC = "Radiant pipes pumping cold gas can be run through hot areas to help cool them down.";

				public static LocString EFFECT = string.Concat(new string[]
				{
					"Carries ",
					UI.FormatAsLink("Gas", "ELEMENTS_GAS"),
					", allowing extreme ",
					UI.FormatAsLink("Temperature", "HEAT"),
					" exchange with the surrounding environment.\n\nCan be run through wall and floor tile."
				});
			}

			public class INSULATEDLIQUIDCONDUIT
			{
				public static LocString NAME = UI.FormatAsLink("Insulated Liquid Pipe", "INSULATEDLIQUIDCONDUIT");

				public static LocString DESC = "Pipe insulation prevents liquid contents from significantly changing temperature in transit.";

				public static LocString EFFECT = string.Concat(new string[]
				{
					"Carries ",
					UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID"),
					" with minimal change in ",
					UI.FormatAsLink("Temperature", "HEAT"),
					".\n\nCan be run through wall and floor tile."
				});
			}

			public class LIQUIDCONDUITRADIANT
			{
				public static LocString NAME = UI.FormatAsLink("Radiant Liquid Pipe", "LIQUIDCONDUITRADIANT");

				public static LocString DESC = "Radiant pipes pumping cold liquid can be run through hot areas to help cool them down.";

				public static LocString EFFECT = string.Concat(new string[]
				{
					"Carries ",
					UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID"),
					", allowing extreme ",
					UI.FormatAsLink("Temperature", "HEAT"),
					" exchange with the surrounding environment.\n\nCan be run through wall and floor tile."
				});
			}

			public class CONTACTCONDUCTIVEPIPEBRIDGE
			{
				public static LocString NAME = UI.FormatAsLink("Conduction Panel", "CONTACTCONDUCTIVEPIPEBRIDGE");

				public static LocString DESC = "It can transfer heat effectively even if no liquid is passing through.";

				public static LocString EFFECT = string.Concat(new string[]
				{
					"Carries ",
					UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID"),
					", allowing extreme ",
					UI.FormatAsLink("Temperature", "HEAT"),
					" exchange with overlapping buildings.\n\nCan function in a vacuum.\n\nCan be run through wall and floor tiles."
				});
			}

			public class INSULATEDWIRE
			{
				public static LocString NAME = UI.FormatAsLink("Insulated Wire", "INSULATEDWIRE");

				public static LocString DESC = "This stuff won't go melting if things get heated.";

				public static LocString EFFECT = string.Concat(new string[]
				{
					"Connects buildings to ",
					UI.FormatAsLink("Power", "POWER"),
					" sources in extreme ",
					UI.FormatAsLink("Heat", "HEAT"),
					".\n\nCan be run through wall and floor tile."
				});
			}

			public class INSULATIONTILE
			{
				public static LocString NAME = UI.FormatAsLink("Insulated Tile", "INSULATIONTILE");

				public static LocString DESC = "The low thermal conductivity of insulated tiles slows any heat passing through them.";

				public static LocString EFFECT = "Used to build the walls and floors of rooms.\n\nReduces " + UI.FormatAsLink("Heat", "HEAT") + " transfer between walls, retaining ambient heat in an area.";
			}

			public class EXTERIORWALL
			{
				public static LocString NAME = UI.FormatAsLink("Drywall", "EXTERIORWALL");

				public static LocString DESC = "Drywall can be used in conjunction with tiles to build airtight rooms on the surface.";

				public static LocString EFFECT = string.Concat(new string[]
				{
					"Prevents ",
					UI.FormatAsLink("Gas", "ELEMENTS_GAS"),
					" and ",
					UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID"),
					" loss in space.\n\nBuilds an insulating backwall behind buildings."
				});

				public class FACADES
				{
					public class DEFAULT_EXTERIORWALL
					{
						public static LocString NAME = UI.FormatAsLink("Drywall", "EXTERIORWALL");

						public static LocString DESC = "It gets the job done.";
					}

					public class BALM_LILY
					{
						public static LocString NAME = UI.FormatAsLink("Balm Lily Print", "EXTERIORWALL");

						public static LocString DESC = "A mellow floral wallpaper.";
					}

					public class CLOUDS
					{
						public static LocString NAME = UI.FormatAsLink("Cloud Print", "EXTERIORWALL");

						public static LocString DESC = "A soft, fluffy wallpaper.";
					}

					public class MUSHBAR
					{
						public static LocString NAME = UI.FormatAsLink("Mush Bar Print", "EXTERIORWALL");

						public static LocString DESC = "A gag-inducing wallpaper.";
					}

					public class PLAID
					{
						public static LocString NAME = UI.FormatAsLink("Aqua Plaid Print", "EXTERIORWALL");

						public static LocString DESC = "A cozy flannel wallpaper.";
					}

					public class RAIN
					{
						public static LocString NAME = UI.FormatAsLink("Rainy Print", "EXTERIORWALL");

						public static LocString DESC = "A precipitation-themed wallpaper.";
					}

					public class AQUATICMOSAIC
					{
						public static LocString NAME = UI.FormatAsLink("Aquatic Mosaic", "EXTERIORWALL");

						public static LocString DESC = "A multi-hued blue wallpaper.";
					}

					public class RAINBOW
					{
						public static LocString NAME = UI.FormatAsLink("Rainbow Stripe", "EXTERIORWALL");

						public static LocString DESC = "A wallpaper with <i>all</i> the colors.";
					}

					public class SNOW
					{
						public static LocString NAME = UI.FormatAsLink("Snowflake Print", "EXTERIORWALL");

						public static LocString DESC = "A wallpaper as unique as my colony.";
					}

					public class SUN
					{
						public static LocString NAME = UI.FormatAsLink("Sunshine Print", "EXTERIORWALL");

						public static LocString DESC = "A UV-free wallpaper.";
					}

					public class COFFEE
					{
						public static LocString NAME = UI.FormatAsLink("Cafe Print", "EXTERIORWALL");

						public static LocString DESC = "A caffeine-themed wallpaper.";
					}

					public class PASTELPOLKA
					{
						public static LocString NAME = UI.FormatAsLink("Pastel Polka Print", "EXTERIORWALL");

						public static LocString DESC = "A soothing, dotted wallpaper.";
					}

					public class PASTELBLUE
					{
						public static LocString NAME = UI.FormatAsLink("Pastel Blue", "EXTERIORWALL");

						public static LocString DESC = "A soothing blue wallpaper.";
					}

					public class PASTELGREEN
					{
						public static LocString NAME = UI.FormatAsLink("Pastel Green", "EXTERIORWALL");

						public static LocString DESC = "A soothing green wallpaper.";
					}

					public class PASTELPINK
					{
						public static LocString NAME = UI.FormatAsLink("Pastel Pink", "EXTERIORWALL");

						public static LocString DESC = "A soothing pink wallpaper.";
					}

					public class PASTELPURPLE
					{
						public static LocString NAME = UI.FormatAsLink("Pastel Purple", "EXTERIORWALL");

						public static LocString DESC = "A soothing purple wallpaper.";
					}

					public class PASTELYELLOW
					{
						public static LocString NAME = UI.FormatAsLink("Pastel Yellow", "EXTERIORWALL");

						public static LocString DESC = "A soothing yellow wallpaper.";
					}

					public class BASIC_WHITE
					{
						public static LocString NAME = UI.FormatAsLink("Fresh White", "EXTERIORWALL");

						public static LocString DESC = "It's just so fresh and so clean.";
					}

					public class DIAGONAL_RED_DEEP_WHITE
					{
						public static LocString NAME = UI.FormatAsLink("Magma Diagonal", "EXTERIORWALL");

						public static LocString DESC = "A red wallpaper with a diagonal stripe.";
					}

					public class DIAGONAL_ORANGE_SATSUMA_WHITE
					{
						public static LocString NAME = UI.FormatAsLink("Bright Diagonal", "EXTERIORWALL");

						public static LocString DESC = "An orange wallpaper with a diagonal stripe.";
					}

					public class DIAGONAL_YELLOW_LEMON_WHITE
					{
						public static LocString NAME = UI.FormatAsLink("Yellowcake Diagonal", "EXTERIORWALL");

						public static LocString DESC = "A radiation-free wallpaper with a diagonal stripe.";
					}

					public class DIAGONAL_GREEN_KELLY_WHITE
					{
						public static LocString NAME = UI.FormatAsLink("Algae Diagonal", "EXTERIORWALL");

						public static LocString DESC = "A slippery wallpaper with a diagonal stripe.";
					}

					public class DIAGONAL_BLUE_COBALT_WHITE
					{
						public static LocString NAME = UI.FormatAsLink("H2O Diagonal", "EXTERIORWALL");

						public static LocString DESC = "A damp wallpaper with a diagonal stripe.";
					}

					public class DIAGONAL_PINK_FLAMINGO_WHITE
					{
						public static LocString NAME = UI.FormatAsLink("Petal Diagonal", "EXTERIORWALL");

						public static LocString DESC = "A pink wallpaper with a diagonal stripe.";
					}

					public class DIAGONAL_GREY_CHARCOAL_WHITE
					{
						public static LocString NAME = UI.FormatAsLink("Charcoal Diagonal", "EXTERIORWALL");

						public static LocString DESC = "A sleek wallpaper with a diagonal stripe.";
					}

					public class CIRCLE_RED_DEEP_WHITE
					{
						public static LocString NAME = UI.FormatAsLink("Magma Wedge", "EXTERIORWALL");

						public static LocString DESC = "It can be arranged into giant red polka dots.";
					}

					public class CIRCLE_ORANGE_SATSUMA_WHITE
					{
						public static LocString NAME = UI.FormatAsLink("Bright Wedge", "EXTERIORWALL");

						public static LocString DESC = "It can be arranged into giant orange polka dots.";
					}

					public class CIRCLE_YELLOW_LEMON_WHITE
					{
						public static LocString NAME = UI.FormatAsLink("Yellowcake Wedge", "EXTERIORWALL");

						public static LocString DESC = "A radiation-free pattern that can be arranged into giant polka dots.";
					}

					public class CIRCLE_GREEN_KELLY_WHITE
					{
						public static LocString NAME = UI.FormatAsLink("Algae Wedge", "EXTERIORWALL");

						public static LocString DESC = "It can be arranged into giant green polka dots.";
					}

					public class CIRCLE_BLUE_COBALT_WHITE
					{
						public static LocString NAME = UI.FormatAsLink("H2O Wedge", "EXTERIORWALL");

						public static LocString DESC = "It can be arranged into giant blue polka dots.";
					}

					public class CIRCLE_PINK_FLAMINGO_WHITE
					{
						public static LocString NAME = UI.FormatAsLink("Petal Wedge", "EXTERIORWALL");

						public static LocString DESC = "It can be arranged into giant pink polka dots.";
					}

					public class CIRCLE_GREY_CHARCOAL_WHITE
					{
						public static LocString NAME = UI.FormatAsLink("Charcoal Wedge", "EXTERIORWALL");

						public static LocString DESC = "It can be arranged into giant shadowy polka dots.";
					}

					public class BASIC_BLUE_COBALT
					{
						public static LocString NAME = UI.FormatAsLink("Solid Cobalt", "EXTERIORWALL");

						public static LocString DESC = "It doesn't cure the blues, so much as emphasize them.";
					}

					public class BASIC_GREEN_KELLY
					{
						public static LocString NAME = UI.FormatAsLink("Spring Green", "EXTERIORWALL");

						public static LocString DESC = "It's cheaper than having a garden.";
					}

					public class BASIC_GREY_CHARCOAL
					{
						public static LocString NAME = UI.FormatAsLink("Solid Charcoal", "EXTERIORWALL");

						public static LocString DESC = "An elevated take on \"gray\".";
					}

					public class BASIC_ORANGE_SATSUMA
					{
						public static LocString NAME = UI.FormatAsLink("Solid Satsuma", "EXTERIORWALL");

						public static LocString DESC = "Less fruit-forward, but just as fresh.";
					}

					public class BASIC_PINK_FLAMINGO
					{
						public static LocString NAME = UI.FormatAsLink("Solid Pink", "EXTERIORWALL");

						public static LocString DESC = "A bold statement, for bold Duplicants.";
					}

					public class BASIC_RED_DEEP
					{
						public static LocString NAME = UI.FormatAsLink("Chili Red", "EXTERIORWALL");

						public static LocString DESC = "It really spices up dull walls.";
					}

					public class BASIC_YELLOW_LEMON
					{
						public static LocString NAME = UI.FormatAsLink("Canary Yellow", "EXTERIORWALL");

						public static LocString DESC = "The original coal-mine chic.";
					}

					public class BLUEBERRIES
					{
						public static LocString NAME = UI.FormatAsLink("Juicy Blueberry", "EXTERIORWALL");

						public static LocString DESC = "It stains the fingers.";
					}

					public class GRAPES
					{
						public static LocString NAME = UI.FormatAsLink("Grape Escape", "EXTERIORWALL");

						public static LocString DESC = "It's seedless, if that matters.";
					}

					public class LEMON
					{
						public static LocString NAME = UI.FormatAsLink("Sour Lemon", "EXTERIORWALL");

						public static LocString DESC = "A bitter yet refreshing style.";
					}

					public class LIME
					{
						public static LocString NAME = UI.FormatAsLink("Juicy Lime", "EXTERIORWALL");

						public static LocString DESC = "Contains no actual vitamin C.";
					}

					public class SATSUMA
					{
						public static LocString NAME = UI.FormatAsLink("Satsuma Slice", "EXTERIORWALL");

						public static LocString DESC = "Adds some much-needed zest to the room.";
					}

					public class STRAWBERRY
					{
						public static LocString NAME = UI.FormatAsLink("Strawberry Speckle", "EXTERIORWALL");

						public static LocString DESC = "Fruity freckles for naturally sweet spaces.";
					}

					public class WATERMELON
					{
						public static LocString NAME = UI.FormatAsLink("Juicy Watermelon", "EXTERIORWALL");

						public static LocString DESC = "Far more practical than gluing real fruit on a wall.";
					}

					public class TROPICAL
					{
						public static LocString NAME = UI.FormatAsLink("Sporechid Print", "EXTERIORWALL");

						public static LocString DESC = "The original scratch-and-sniff version was immediately recalled.";
					}

					public class TOILETPAPER
					{
						public static LocString NAME = UI.FormatAsLink("De-loo-xe", "EXTERIORWALL");

						public static LocString DESC = "Softly undulating lines create an undeniable air of loo-xury.";
					}

					public class PLUNGER
					{
						public static LocString NAME = UI.FormatAsLink("Plunger Print", "EXTERIORWALL");

						public static LocString DESC = "Unclogs one's creative impulses.";
					}

					public class STRIPES_BLUE
					{
						public static LocString NAME = UI.FormatAsLink("Blue Awning Stripe", "EXTERIORWALL");

						public static LocString DESC = "Thick stripes in alternating shades of blue.";
					}

					public class STRIPES_DIAGONAL_BLUE
					{
						public static LocString NAME = UI.FormatAsLink("Blue Regimental Stripe", "EXTERIORWALL");

						public static LocString DESC = "Inspired by the ties worn during intraoffice sports.";
					}

					public class STRIPES_CIRCLE_BLUE
					{
						public static LocString NAME = UI.FormatAsLink("Blue Circle Stripe", "EXTERIORWALL");

						public static LocString DESC = "A stripe that curves to the right.";
					}

					public class SQUARES_RED_DEEP_WHITE
					{
						public static LocString NAME = UI.FormatAsLink("Magma Checkers", "EXTERIORWALL");

						public static LocString DESC = "They're so hot right now!";
					}

					public class SQUARES_ORANGE_SATSUMA_WHITE
					{
						public static LocString NAME = UI.FormatAsLink("Bright Checkers", "EXTERIORWALL");

						public static LocString DESC = "Every tile feels like four tiles!";
					}

					public class SQUARES_YELLOW_LEMON_WHITE
					{
						public static LocString NAME = UI.FormatAsLink("Yellowcake Checkers", "EXTERIORWALL");

						public static LocString DESC = "Any brighter, and they'd be radioactive!";
					}

					public class SQUARES_GREEN_KELLY_WHITE
					{
						public static LocString NAME = UI.FormatAsLink("Algae Checkers", "EXTERIORWALL");

						public static LocString DESC = "Now with real simulated algae color!";
					}

					public class SQUARES_BLUE_COBALT_WHITE
					{
						public static LocString NAME = UI.FormatAsLink("H2O Checkers", "EXTERIORWALL");

						public static LocString DESC = "Drink it all in!";
					}

					public class SQUARES_PINK_FLAMINGO_WHITE
					{
						public static LocString NAME = UI.FormatAsLink("Petal Checkers", "EXTERIORWALL");

						public static LocString DESC = "Fiercely fluorescent floral-inspired pink!";
					}

					public class SQUARES_GREY_CHARCOAL_WHITE
					{
						public static LocString NAME = UI.FormatAsLink("Charcoal Checkers", "EXTERIORWALL");

						public static LocString DESC = "So retro!";
					}

					public class KITCHEN_RETRO1
					{
						public static LocString NAME = UI.FormatAsLink("Cafeteria Kitsch", "EXTERIORWALL");

						public static LocString DESC = "Some diners find it nostalgic.";
					}

					public class PLUS_RED_DEEP_WHITE
					{
						public static LocString NAME = UI.FormatAsLink("Digital Chili", "EXTERIORWALL");

						public static LocString DESC = "A pixelated red-and-white print.";
					}

					public class PLUS_ORANGE_SATSUMA_WHITE
					{
						public static LocString NAME = UI.FormatAsLink("Digital Satsuma", "EXTERIORWALL");

						public static LocString DESC = "A pixelated orange-and-white print.";
					}

					public class PLUS_YELLOW_LEMON_WHITE
					{
						public static LocString NAME = UI.FormatAsLink("Digital Lemon", "EXTERIORWALL");

						public static LocString DESC = "A pixelated yellow-and-white print.";
					}

					public class PLUS_GREEN_KELLY_WHITE
					{
						public static LocString NAME = UI.FormatAsLink("Digital Lawn", "EXTERIORWALL");

						public static LocString DESC = "A pixelated green-and-white print.";
					}

					public class PLUS_BLUE_COBALT_WHITE
					{
						public static LocString NAME = UI.FormatAsLink("Digital Cobalt", "EXTERIORWALL");

						public static LocString DESC = "A pixelated blue-and-white print.";
					}

					public class PLUS_PINK_FLAMINGO_WHITE
					{
						public static LocString NAME = UI.FormatAsLink("Digital Pink", "EXTERIORWALL");

						public static LocString DESC = "A pixelated pink-and-white print.";
					}

					public class PLUS_GREY_CHARCOAL_WHITE
					{
						public static LocString NAME = UI.FormatAsLink("Digital Charcoal", "EXTERIORWALL");

						public static LocString DESC = "It's futuristic, so it must be good.";
					}

					public class STRIPES_ROSE
					{
						public static LocString NAME = UI.FormatAsLink("Puce Stripe", "EXTERIORWALL");

						public static LocString DESC = "Vertical stripes make it quite obvious when nearby objects are askew.";
					}

					public class STRIPES_DIAGONAL_ROSE
					{
						public static LocString NAME = UI.FormatAsLink("Puce Diagonal", "EXTERIORWALL");

						public static LocString DESC = "Some describe this color as \"squashed bug.\"";
					}

					public class STRIPES_CIRCLE_ROSE
					{
						public static LocString NAME = UI.FormatAsLink("Puce Curves", "EXTERIORWALL");

						public static LocString DESC = "It's pronounced \"peeyoo-ss,\" a sound that Duplicants just can't seem to reproduce.";
					}

					public class STRIPES_MUSH
					{
						public static LocString NAME = UI.FormatAsLink("Mush Stripe", "EXTERIORWALL");

						public static LocString DESC = "The kind of green that makes one feel slightly nauseated.";
					}

					public class STRIPES_DIAGONAL_MUSH
					{
						public static LocString NAME = UI.FormatAsLink("Mush Diagonal", "EXTERIORWALL");

						public static LocString DESC = "Diagonal stripes in alternating shades of mush bar.";
					}

					public class STRIPES_CIRCLE_MUSH
					{
						public static LocString NAME = UI.FormatAsLink("Mush Curves", "EXTERIORWALL");

						public static LocString DESC = "This wallpaper, like this colony's journey, is full of twists and turns.";
					}

					public class STRIPES_YELLOW_TARTAR
					{
						public static LocString NAME = UI.FormatAsLink("Ick Stripe", "EXTERIORWALL");

						public static LocString DESC = "Vertical stripes make it quite obvious when nearby objects are askew.";
					}

					public class STRIPES_DIAGONAL_YELLOW_TARTAR
					{
						public static LocString NAME = UI.FormatAsLink("Ick Diagonal", "EXTERIORWALL");

						public static LocString DESC = "Diagonal stripes in alternating shades of yellow.";
					}

					public class STRIPES_CIRCLE_YELLOW_TARTAR
					{
						public static LocString NAME = UI.FormatAsLink("Ick Curves", "EXTERIORWALL");

						public static LocString DESC = "This wallpaper, like this colony's journey, is full of twists and turns.";
					}

					public class STRIPES_PURPLE_BRAINFAT
					{
						public static LocString NAME = UI.FormatAsLink("Fainting Stripe", "EXTERIORWALL");

						public static LocString DESC = "Vertical stripes make it quite obvious when nearby objects are askew.";
					}

					public class STRIPES_DIAGONAL_PURPLE_BRAINFAT
					{
						public static LocString NAME = UI.FormatAsLink("Fainting Diagonal", "EXTERIORWALL");

						public static LocString DESC = "Diagonal stripes in alternating shades of purple.";
					}

					public class STRIPES_CIRCLE_PURPLE_BRAINFAT
					{
						public static LocString NAME = UI.FormatAsLink("Fainting Curves", "EXTERIORWALL");

						public static LocString DESC = "This wallpaper, like this colony's journey, is full of twists and turns.";
					}

					public class FLOPPY_AZULENE_VITRO
					{
						public static LocString NAME = UI.FormatAsLink("Waterlogged Databank", "EXTERIORWALL");

						public static LocString DESC = "A fun blue print in honor of information storage.";
					}

					public class FLOPPY_BLACK_WHITE
					{
						public static LocString NAME = UI.FormatAsLink("Monochrome Databank", "EXTERIORWALL");

						public static LocString DESC = "A chic black-and-white print in honor of information storage.";
					}

					public class FLOPPY_PEAGREEN_BALMY
					{
						public static LocString NAME = UI.FormatAsLink("Lush Databank", "EXTERIORWALL");

						public static LocString DESC = "A fun green print in honor of information storage.";
					}

					public class FLOPPY_SATSUMA_YELLOWCAKE
					{
						public static LocString NAME = UI.FormatAsLink("Hi-Vis Databank", "EXTERIORWALL");

						public static LocString DESC = "A fun orange print in honor of information storage.";
					}

					public class FLOPPY_MAGMA_AMINO
					{
						public static LocString NAME = UI.FormatAsLink("Flashy Databank", "EXTERIORWALL");

						public static LocString DESC = "A fun red print in honor of information storage.";
					}

					public class ORANGE_JUICE
					{
						public static LocString NAME = UI.FormatAsLink("Infinite Spill", "EXTERIORWALL");

						public static LocString DESC = "If the liquids never hit the floor, is it really a spill?";
					}

					public class PAINT_BLOTS
					{
						public static LocString NAME = UI.FormatAsLink("Happy Accidents", "EXTERIORWALL");

						public static LocString DESC = "There are no mistakes, only cheerful little splotches.";
					}

					public class TELESCOPE
					{
						public static LocString NAME = UI.FormatAsLink("Telescope Print", "EXTERIORWALL");

						public static LocString DESC = "The perfect wallpaper for skygazers.";
					}

					public class TICTACTOE_O
					{
						public static LocString NAME = UI.FormatAsLink("TicTacToe O", "EXTERIORWALL");

						public static LocString DESC = "A crisp black 'O' on a clean white background. Ideal for monochromatic games rooms.";
					}

					public class TICTACTOE_X
					{
						public static LocString NAME = UI.FormatAsLink("TicTacToe X", "EXTERIORWALL");

						public static LocString DESC = "A crisp black 'X' on a clean white background. Ideal for monochromatic games rooms.";
					}

					public class DICE_1
					{
						public static LocString NAME = UI.FormatAsLink("Roll One", "EXTERIORWALL");

						public static LocString DESC = "Inspired by classic dice.";
					}

					public class DICE_2
					{
						public static LocString NAME = UI.FormatAsLink("Roll Two", "EXTERIORWALL");

						public static LocString DESC = "Inspired by classic dice.";
					}

					public class DICE_3
					{
						public static LocString NAME = UI.FormatAsLink("Roll Three", "EXTERIORWALL");

						public static LocString DESC = "Inspired by classic dice.";
					}

					public class DICE_4
					{
						public static LocString NAME = UI.FormatAsLink("Roll Four", "EXTERIORWALL");

						public static LocString DESC = "Inspired by classic dice.";
					}

					public class DICE_5
					{
						public static LocString NAME = UI.FormatAsLink("Roll Five", "EXTERIORWALL");

						public static LocString DESC = "Inspired by classic dice.";
					}

					public class DICE_6
					{
						public static LocString NAME = UI.FormatAsLink("High Roller", "EXTERIORWALL");

						public static LocString DESC = "Inspired by classic dice.";
					}
				}
			}

			public class FARMTILE
			{
				public static LocString NAME = UI.FormatAsLink("Farm Tile", "FARMTILE");

				public static LocString DESC = "Duplicants can deliver fertilizer and liquids to farm tiles, accelerating plant growth.";

				public static LocString EFFECT = string.Concat(new string[]
				{
					"Grows one ",
					UI.FormatAsLink("Plant", "PLANTS"),
					" from a ",
					UI.FormatAsLink("Seed", "PLANTS"),
					".\n\nCan be used as floor tile and rotated before construction."
				});
			}

			public class LADDER
			{
				public static LocString NAME = UI.FormatAsLink("Ladder", "LADDER");

				public static LocString DESC = "(That means they climb it.)";

				public static LocString EFFECT = "Enables vertical mobility for Duplicants.";
			}

			public class LADDERFAST
			{
				public static LocString NAME = UI.FormatAsLink("Plastic Ladder", "LADDERFAST");

				public static LocString DESC = "Plastic ladders are mildly antiseptic and can help limit the spread of germs in a colony.";

				public static LocString EFFECT = "Increases Duplicant climbing speed.";
			}

			public class LIQUIDCONDUIT
			{
				public static LocString NAME = UI.FormatAsLink("Liquid Pipe", "LIQUIDCONDUIT");

				public static LocString DESC = "Liquid pipes are used to connect the inputs and outputs of plumbed buildings.";

				public static LocString EFFECT = string.Concat(new string[]
				{
					"Carries ",
					UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID"),
					" between ",
					UI.FormatAsLink("Outputs", "LIQUIDPIPING"),
					" and ",
					UI.FormatAsLink("Intakes", "LIQUIDPIPING"),
					".\n\nCan be run through wall and floor tile."
				});
			}

			public class LIQUIDCONDUITBRIDGE
			{
				public static LocString NAME = UI.FormatAsLink("Liquid Bridge", "LIQUIDCONDUITBRIDGE");

				public static LocString DESC = "Separate pipe systems help prevent building damage caused by mingled pipe contents.";

				public static LocString EFFECT = "Runs one " + UI.FormatAsLink("Liquid Pipe", "LIQUIDPIPING") + " section over another without joining them.\n\nCan be run through wall and floor tile.";
			}

			public class ICECOOLEDFAN
			{
				public static LocString NAME = UI.FormatAsLink("Ice-E Fan", "ICECOOLEDFAN");

				public static LocString DESC = "A Duplicant can work an Ice-E fan to temporarily cool small areas as needed.";

				public static LocString EFFECT = string.Concat(new string[]
				{
					"Uses ",
					UI.FormatAsLink("Ice", "ICEORE"),
					" to dissipate a small amount of the ",
					UI.FormatAsLink("Heat", "HEAT"),
					"."
				});
			}

			public class ICEMACHINE
			{
				public static LocString NAME = UI.FormatAsLink("Ice Maker", "ICEMACHINE");

				public static LocString DESC = "Ice makers can be used as a small renewable source of ice and snow.";

				public static LocString EFFECT = string.Concat(new string[]
				{
					"Converts ",
					UI.FormatAsLink("Water", "WATER"),
					" into ",
					UI.FormatAsLink("Ice", "ICE"),
					" or ",
					UI.FormatAsLink("Snow", "SNOW"),
					"."
				});

				public class OPTION_TOOLTIPS
				{
					public static LocString ICE = "Convert " + UI.FormatAsLink("Water", "WATER") + " into " + UI.FormatAsLink("Ice", "ICE");

					public static LocString SNOW = "Convert " + UI.FormatAsLink("Water", "WATER") + " into " + UI.FormatAsLink("Snow", "SNOW");
				}
			}

			public class LIQUIDCOOLEDFAN
			{
				public static LocString NAME = UI.FormatAsLink("Hydrofan", "LIQUIDCOOLEDFAN");

				public static LocString DESC = "A Duplicant can work a hydrofan to temporarily cool small areas as needed.";

				public static LocString EFFECT = "Dissipates a small amount of the " + UI.FormatAsLink("Heat", "HEAT") + ".";
			}

			public class CREATURETRAP
			{
				public static LocString NAME = UI.FormatAsLink("Critter Trap", "CREATURETRAP");

				public static LocString DESC = "Critter traps cannot catch swimming or flying critters.";

				public static LocString EFFECT = "Captures a living " + UI.FormatAsLink("Critter", "CREATURES") + " for transport.\n\nSingle use.";
			}

			public class CREATUREGROUNDTRAP
			{
				public static LocString NAME = UI.FormatAsLink("Critter Trap", "CREATUREGROUNDTRAP");

				public static LocString DESC = "It's designed for land critters, but flopping fish sometimes find their way in too.";

				public static LocString EFFECT = string.Concat(new string[]
				{
					"Captures a living ",
					UI.FormatAsLink("Critter", "CREATURES"),
					" for transport.\n\nOnly Duplicants with the ",
					UI.FormatAsLink("Critter Ranching I", "RANCHING1"),
					" skill can arm this trap. It's reusable!"
				});
			}

			public class CREATUREDELIVERYPOINT
			{
				public static LocString NAME = UI.FormatAsLink("Critter Drop-Off", "CREATUREDELIVERYPOINT");

				public static LocString DESC = "Duplicants automatically bring captured critters to these relocation points for release.";

				public static LocString EFFECT = "Releases trapped " + UI.FormatAsLink("Critters", "CREATURES") + " back into the world.\n\nCan be used multiple times.";
			}

			public class CRITTERPICKUP
			{
				public static LocString NAME = UI.FormatAsLink("Critter Pick-Up", "CRITTERPICKUP");

				public static LocString DESC = "Duplicants will automatically wrangle excess critters.";

				public static LocString EFFECT = "Ensures the prompt relocation of " + UI.FormatAsLink("Critters", "CREATURES") + " that exceed the maximum amount set.\n\nMonitoring and pick-up are limited to the specified species.";

				public class LOGIC_INPUT
				{
					public static LocString DESC = "Enable/Disable";

					public static LocString LOGIC_PORT_ACTIVE = UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + ": Wrangle excess critters";

					public static LocString LOGIC_PORT_INACTIVE = BUILDINGS.PREFABS.CRITTERPICKUP.LOGIC_INPUT.LOGIC_PORT_INACTIVE = UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby) + ": Ignore excess critters";
				}
			}

			public class CRITTERDROPOFF
			{
				public static LocString NAME = UI.FormatAsLink("Critter Drop-Off", "CRITTERDROPOFF");

				public static LocString DESC = "Duplicants automatically bring captured critters to these relocation points for release.";

				public static LocString EFFECT = "Releases trapped " + UI.FormatAsLink("Critters", "CREATURES") + " back into the world.\n\nMonitoring and drop-off are limited to the specified species.";

				public class LOGIC_INPUT
				{
					public static LocString DESC = "Enable/Disable";

					public static LocString LOGIC_PORT_ACTIVE = UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + ": Enable critter drop-off";

					public static LocString LOGIC_PORT_INACTIVE = UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby) + ": Disable critter drop-off";
				}
			}

			public class LIQUIDFILTER
			{
				public static LocString NAME = UI.FormatAsLink("Liquid Filter", "LIQUIDFILTER");

				public static LocString DESC = "All liquids are sent into the building's output pipe, except the liquid chosen for filtering.";

				public static LocString EFFECT = string.Concat(new string[]
				{
					"Sieves one ",
					UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID"),
					" out of a mix, sending it into a dedicated ",
					UI.FormatAsLink("Filtered Output Pipe", "LIQUIDPIPING"),
					".\n\nCan only filter one liquid type at a time."
				});
			}

			public class DEVPUMPLIQUID
			{
				public static LocString NAME = "Dev Pump Liquid";

				public static LocString DESC = "Piping a pump's output to a building's intake will send liquid to that building.";

				public static LocString EFFECT = string.Concat(new string[]
				{
					"Draws in ",
					UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID"),
					" and runs it through ",
					UI.FormatAsLink("Pipes", "LIQUIDPIPING"),
					".\n\nMust be submerged in ",
					UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID"),
					"."
				});
			}

			public class LIQUIDPUMP
			{
				public static LocString NAME = UI.FormatAsLink("Liquid Pump", "LIQUIDPUMP");

				public static LocString DESC = "Piping a pump's output to a building's intake will send liquid to that building.";

				public static LocString EFFECT = string.Concat(new string[]
				{
					"Draws in ",
					UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID"),
					" and runs it through ",
					UI.FormatAsLink("Pipes", "LIQUIDPIPING"),
					".\n\nMust be submerged in ",
					UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID"),
					"."
				});
			}

			public class LIQUIDMINIPUMP
			{
				public static LocString NAME = UI.FormatAsLink("Mini Liquid Pump", "LIQUIDMINIPUMP");

				public static LocString DESC = "Mini pumps are useful for moving small quantities of liquid with minimum power.";

				public static LocString EFFECT = string.Concat(new string[]
				{
					"Draws in a small amount of ",
					UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID"),
					" and runs it through ",
					UI.FormatAsLink("Pipes", "LIQUIDPIPING"),
					".\n\nMust be submerged in ",
					UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID"),
					"."
				});
			}

			public class LIQUIDPUMPINGSTATION
			{
				public static LocString NAME = UI.FormatAsLink("Pitcher Pump", "LIQUIDPUMPINGSTATION");

				public static LocString DESC = "Pitcher pumps allow Duplicants to bottle and deliver liquids from place to place.";

				public static LocString EFFECT = "Manually pumps " + UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID") + " into bottles for transport.\n\nDuplicants can only carry liquids that are bottled.";
			}

			public class LIQUIDVALVE
			{
				public static LocString NAME = UI.FormatAsLink("Liquid Valve", "LIQUIDVALVE");

				public static LocString DESC = "Valves control the amount of liquid that moves through pipes, preventing waste.";

				public static LocString EFFECT = string.Concat(new string[]
				{
					"Controls the ",
					UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID"),
					" volume permitted through ",
					UI.FormatAsLink("Pipes", "LIQUIDPIPING"),
					"."
				});
			}

			public class LIQUIDLOGICVALVE
			{
				public static LocString NAME = UI.FormatAsLink("Liquid Shutoff", "LIQUIDLOGICVALVE");

				public static LocString DESC = "Automated piping saves power and time by removing the need for Duplicant input.";

				public static LocString EFFECT = string.Concat(new string[]
				{
					"Connects to an ",
					UI.FormatAsLink("Automation", "LOGIC"),
					" grid to automatically turn ",
					UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID"),
					" flow on or off."
				});

				public static LocString LOGIC_PORT = "Open/Close";

				public static LocString LOGIC_PORT_ACTIVE = UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + ": Allow Liquid flow";

				public static LocString LOGIC_PORT_INACTIVE = UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby) + ": Prevent Liquid flow";
			}

			public class LIQUIDLIMITVALVE
			{
				public static LocString NAME = UI.FormatAsLink("Liquid Meter Valve", "LIQUIDLIMITVALVE");

				public static LocString DESC = "Meter Valves let an exact amount of liquid pass through before shutting off.";

				public static LocString EFFECT = string.Concat(new string[]
				{
					"Connects to an ",
					UI.FormatAsLink("Automation", "LOGIC"),
					" grid to automatically turn ",
					UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID"),
					" flow off when the specified amount has passed through it."
				});

				public static LocString LOGIC_PORT_OUTPUT = "Limit Reached";

				public static LocString OUTPUT_PORT_ACTIVE = "Sends a " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + " if limit has been reached";

				public static LocString OUTPUT_PORT_INACTIVE = "Otherwise, sends a " + UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby);

				public static LocString LOGIC_PORT_RESET = "Reset Meter";

				public static LocString RESET_PORT_ACTIVE = UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + ": Reset the amount";

				public static LocString RESET_PORT_INACTIVE = UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby) + ": Nothing";
			}

			public class LIQUIDVENT
			{
				public static LocString NAME = UI.FormatAsLink("Liquid Vent", "LIQUIDVENT");

				public static LocString DESC = "Vents are an exit point for liquids from plumbing systems.";

				public static LocString EFFECT = string.Concat(new string[]
				{
					"Releases ",
					UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID"),
					" from ",
					UI.FormatAsLink("Liquid Pipes", "LIQUIDPIPING"),
					"."
				});
			}

			public class MANUALGENERATOR
			{
				public static LocString NAME = UI.FormatAsLink("Manual Generator", "MANUALGENERATOR");

				public static LocString DESC = "Watching Duplicants run on it is adorable... the electrical power is just an added bonus.";

				public static LocString EFFECT = "Converts manual labor into electrical " + UI.FormatAsLink("Power", "POWER") + ".";
			}

			public class MANUALPRESSUREDOOR
			{
				public static LocString NAME = UI.FormatAsLink("Manual Airlock", "MANUALPRESSUREDOOR");

				public static LocString DESC = "Airlocks can quarter off dangerous areas and prevent gases from seeping into the colony.";

				public static LocString EFFECT = string.Concat(new string[]
				{
					"Blocks ",
					UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID"),
					" and ",
					UI.FormatAsLink("Gas", "ELEMENTS_GAS"),
					" flow, maintaining pressure between areas.\n\nWild ",
					UI.FormatAsLink("Critters", "CREATURES"),
					" cannot pass through doors."
				});
			}

			public class MESHTILE
			{
				public static LocString NAME = UI.FormatAsLink("Mesh Tile", "MESHTILE");

				public static LocString DESC = "Mesh tile can be used to make Duplicant pathways in areas where liquid flows.";

				public static LocString EFFECT = string.Concat(new string[]
				{
					"Used to build the walls and floors of rooms.\n\nDoes not obstruct ",
					UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID"),
					" or ",
					UI.FormatAsLink("Gas", "ELEMENTS_GAS"),
					" flow."
				});
			}

			public class PLASTICTILE
			{
				public static LocString NAME = UI.FormatAsLink("Plastic Tile", "PLASTICTILE");

				public static LocString DESC = "Plastic tile is mildly antiseptic and can help limit the spread of germs in a colony.";

				public static LocString EFFECT = "Used to build the walls and floors of rooms.\n\nSignificantly increases Duplicant runspeed.";
			}

			public class GLASSTILE
			{
				public static LocString NAME = UI.FormatAsLink("Window Tile", "GLASSTILE");

				public static LocString DESC = "Window tiles provide a barrier against liquid and gas and are completely transparent.";

				public static LocString EFFECT = string.Concat(new string[]
				{
					"Used to build the walls and floors of rooms.\n\nAllows ",
					UI.FormatAsLink("Light", "LIGHT"),
					" and ",
					UI.FormatAsLink("Decor", "DECOR"),
					" to pass through."
				});
			}

			public class METALTILE
			{
				public static LocString NAME = UI.FormatAsLink("Metal Tile", "METALTILE");

				public static LocString DESC = "Heat travels much more quickly through metal tile than other types of flooring.";

				public static LocString EFFECT = "Used to build the walls and floors of rooms.\n\nSignificantly increases Duplicant runspeed.";
			}

			public class BUNKERTILE
			{
				public static LocString NAME = UI.FormatAsLink("Bunker Tile", "BUNKERTILE");

				public static LocString DESC = "Bunker tile can build strong shelters in otherwise dangerous environments.";

				public static LocString EFFECT = "Used to build the walls and floors of rooms.\n\nCan withstand extreme pressures and impacts.";
			}

			public class STORAGETILE
			{
				public static LocString NAME = UI.FormatAsLink("Storage Tile", "STORAGETILE");

				public static LocString DESC = "Storage tiles keep selected non-edible solids out of the way.";

				public static LocString EFFECT = "Used to build the walls and floors of rooms.\n\nProvides built-in storage for small spaces.";
			}

			public class CARPETTILE
			{
				public static LocString NAME = UI.FormatAsLink("Carpeted Tile", "CARPETTILE");

				public static LocString DESC = "Soft on little Duplicant toesies.";

				public static LocString EFFECT = string.Concat(new string[]
				{
					"Used to build the walls and floors of rooms.\n\nIncreases ",
					UI.FormatAsLink("Decor", "DECOR"),
					", contributing to ",
					UI.FormatAsLink("Morale", "MORALE"),
					"."
				});
			}

			public class MOULDINGTILE
			{
				public static LocString NAME = UI.FormatAsLink("Trimming Tile", "MOUDLINGTILE");

				public static LocString DESC = "Trimming is used as purely decorative lining for walls and structures.";

				public static LocString EFFECT = string.Concat(new string[]
				{
					"Used to build the walls and floors of rooms.\n\nIncreases ",
					UI.FormatAsLink("Decor", "DECOR"),
					", contributing to ",
					UI.FormatAsLink("Morale", "MORALE"),
					"."
				});
			}

			public class MONUMENTBOTTOM
			{
				public static LocString NAME = UI.FormatAsLink("Monument Base", "MONUMENTBOTTOM");

				public static LocString DESC = "The base of a monument must be constructed first.";

				public static LocString EFFECT = "Builds the bottom section of a Great Monument.\n\nCan be customized.\n\nA Great Monument must be built to achieve the Colonize Imperative.";

				public class FACADES
				{
					public class OPTION_A
					{
						public static LocString NAME = "On Asteroid I";

						public static LocString DESC = "Standing tall.";
					}

					public class OPTION_B
					{
						public static LocString NAME = "On Asteroid II";

						public static LocString DESC = "Standing purposefully.";
					}

					public class OPTION_C
					{
						public static LocString NAME = "On Asteroid III";

						public static LocString DESC = "Their knees were knockin'.";
					}

					public class OPTION_D
					{
						public static LocString NAME = "Scientific Seat";

						public static LocString DESC = "In celebration of science!";
					}

					public class OPTION_E
					{
						public static LocString NAME = "On Asteroid IV";

						public static LocString DESC = "It's a confident stance.";
					}

					public class OPTION_F
					{
						public static LocString NAME = "On Asteroid V";

						public static LocString DESC = "One knee tucked toward the other.";
					}

					public class OPTION_G
					{
						public static LocString NAME = "On Asteroid VI";

						public static LocString DESC = "One small step for Duplicantkind...";
					}

					public class OPTION_H
					{
						public static LocString NAME = "Hatch Hunter";

						public static LocString DESC = "Atop a pair of conquered critters.";
					}

					public class OPTION_I
					{
						public static LocString NAME = "Trash Tranquility";

						public static LocString DESC = "Finding peace amid the debris.";
					}

					public class OPTION_J
					{
						public static LocString NAME = "Fish Stomper";

						public static LocString DESC = "That can't be comfortable.";
					}

					public class OPTION_K
					{
						public static LocString NAME = "Egg Equanimity";

						public static LocString DESC = "One must give the soul time to truly hatch.";
					}

					public class OPTION_L
					{
						public static LocString NAME = "Tilted Nosecone";

						public static LocString DESC = "A slightly unbalanced base.";
					}

					public class OPTION_M
					{
						public static LocString NAME = "Sweet Seat";

						public static LocString DESC = "In honor of the sugar engine.";
					}

					public class OPTION_N
					{
						public static LocString NAME = "CO2 Straddle";

						public static LocString DESC = "Riding a carbon dioxide rocket engine to glory.";
					}

					public class OPTION_O
					{
						public static LocString NAME = "Petroleum Pose";

						public static LocString DESC = "Atop a small petroleum rocket engine.";
					}

					public class OPTION_P
					{
						public static LocString NAME = "Spacefarer Stance";

						public static LocString DESC = "Atop a solo spacefarer rocket nosecone.";
					}

					public class OPTION_Q
					{
						public static LocString NAME = "Seat of Power";

						public static LocString DESC = "Atop a radbolt rocket engine.";
					}

					public class OPTION_R
					{
						public static LocString NAME = "Sweepy I";

						public static LocString DESC = "Atop a sleeping Sweepy bot.";
					}

					public class OPTION_S
					{
						public static LocString NAME = "Sweepy II";

						public static LocString DESC = "Atop a curious Sweepy bot.";
					}

					public class OPTION_T
					{
						public static LocString NAME = "Sweepy III";

						public static LocString DESC = "Atop a happy lil' Sweepy bot.";
					}
				}
			}

			public class MONUMENTMIDDLE
			{
				public static LocString NAME = UI.FormatAsLink("Monument Midsection", "MONUMENTMIDDLE");

				public static LocString DESC = "Customized sections of a Great Monument can be mixed and matched.";

				public static LocString EFFECT = "Builds the middle section of a Great Monument.\n\nCan be customized.\n\nA Great Monument must be built to achieve the Colonize Imperative.";

				public class FACADES
				{
					public class OPTION_A
					{
						public static LocString NAME = "Thumbs Up";

						public static LocString DESC = "Good job, sculptor!";
					}

					public class OPTION_B
					{
						public static LocString NAME = "Big Wrench";

						public static LocString DESC = "Lefty loose-y, righty tighty.";
					}

					public class OPTION_C
					{
						public static LocString NAME = "Um, Excuse Me";

						public static LocString DESC = "Celebrates uncertainty.";
					}

					public class OPTION_D
					{
						public static LocString NAME = "Hands on Hips";

						public static LocString DESC = "Makes the torso seem bigger and more intimidating than it is.";
					}

					public class OPTION_E
					{
						public static LocString NAME = "The Shrug";

						public static LocString DESC = "Sometimes things are good enough just as they are.";
					}

					public class OPTION_F
					{
						public static LocString NAME = "You Betcha";

						public static LocString DESC = "The finger gun of approval.";
					}

					public class OPTION_G
					{
						public static LocString NAME = "Well Hello There";

						public static LocString DESC = "It's quite torso-forward.";
					}

					public class OPTION_H
					{
						public static LocString NAME = "Fists of Fury";

						public static LocString DESC = "Let 'em fly!";
					}

					public class OPTION_I
					{
						public static LocString NAME = "Hatch Hug";

						public static LocString DESC = "Cradling a cozy critter.";
					}

					public class OPTION_J
					{
						public static LocString NAME = "Casual Elegance";

						public static LocString DESC = "Leaning casually, with grace.";
					}

					public class OPTION_K
					{
						public static LocString NAME = "Arms Ajar";

						public static LocString DESC = "Hands hover slightly away from the body, as though raised in wonder.";
					}

					public class OPTION_L
					{
						public static LocString NAME = "Babes in Arms I";

						public static LocString DESC = "Cradling a couple of smooth lil' critter babies.";
					}

					public class OPTION_M
					{
						public static LocString NAME = "Model Rocket";

						public static LocString DESC = "Celebrates a cosmic undertaking.";
					}

					public class OPTION_N
					{
						public static LocString NAME = "Babes in Arms II";

						public static LocString DESC = "An armful of chonky lil' critter babies.";
					}

					public class OPTION_O
					{
						public static LocString NAME = "Babes in Arms III";

						public static LocString DESC = "Embracing buggy lil' critter babies.";
					}
				}
			}

			public class MONUMENTTOP
			{
				public static LocString NAME = UI.FormatAsLink("Monument Top", "MONUMENTTOP");

				public static LocString DESC = "Building a Great Monument will declare to the universe that this hunk of rock is your own.";

				public static LocString EFFECT = "Builds the top section of a Great Monument.\n\nCan be customized.\n\nA Great Monument must be built to achieve the Colonize Imperative.";

				public class FACADES
				{
					public class OPTION_A
					{
						public static LocString NAME = "Leira Noggin";

						public static LocString DESC = "A massive replica of Leira's smiling face.";
					}

					public class OPTION_B
					{
						public static LocString NAME = "Gossmann Noggin";

						public static LocString DESC = "A massive replica of Gossmann's determined gaze.";
					}

					public class OPTION_C
					{
						public static LocString NAME = "Puft Top";

						public static LocString DESC = "A great-monument-sized puft.";
					}

					public class OPTION_D
					{
						public static LocString NAME = "Nikola Noggin";

						public static LocString DESC = "A massive replica of Nikola's post-explosion expression.";
					}

					public class OPTION_E
					{
						public static LocString NAME = "Burt Noggin";

						public static LocString DESC = "A massive replica of Burt's critter-spotting expression.";
					}

					public class OPTION_F
					{
						public static LocString NAME = "Rowan Noggin";

						public static LocString DESC = "A massive replica of Rowan's serene smile.";
					}

					public class OPTION_G
					{
						public static LocString NAME = "Nisbet Noggin";

						public static LocString DESC = "A massive replica of Nisbet when she sees someone whose name she's forgotten.";
					}

					public class OPTION_H
					{
						public static LocString NAME = "Ashkan Noggin";

						public static LocString DESC = "A massive replica of Ashkan's fossil-discovering expression.";
					}

					public class OPTION_I
					{
						public static LocString NAME = "Ren Noggin";

						public static LocString DESC = "A massive replica of Ren's smoochy face.";
					}

					public class OPTION_J
					{
						public static LocString NAME = "Hatch Top";

						public static LocString DESC = "A great-monument-sized Hatch.";
					}

					public class OPTION_K
					{
						public static LocString NAME = "Glossy Drecko Top";

						public static LocString DESC = "A great-monument-sized Glossy Drecko.";
					}

					public class OPTION_L
					{
						public static LocString NAME = "Shove Vole Top";

						public static LocString DESC = "A great-monument-sized Shove Vole.";
					}

					public class OPTION_M
					{
						public static LocString NAME = "Gassy Moo Top";

						public static LocString DESC = "A great-monument-sized Gassy Moo. Gassier and moo-ier than ever.";
					}

					public class OPTION_N
					{
						public static LocString NAME = "Morb Top";

						public static LocString DESC = "A great-monument-sized Morb.";
					}

					public class OPTION_O
					{
						public static LocString NAME = "Shine Bug Top";

						public static LocString DESC = "A great-monument-sized Shine Bug.";
					}

					public class OPTION_P
					{
						public static LocString NAME = "Slickster Top";

						public static LocString DESC = "A great-monument-sized Slickster.";
					}

					public class OPTION_Q
					{
						public static LocString NAME = "Pacu Top";

						public static LocString DESC = "A great-monument-sized underbite.";
					}

					public class OPTION_R
					{
						public static LocString NAME = "Beeta Top";

						public static LocString DESC = "A great-monument-sized Beeta.";
					}

					public class OPTION_S
					{
						public static LocString NAME = "Sweetle Top";

						public static LocString DESC = "A great-monument-sized Sweetle.";
					}

					public class OPTION_T
					{
						public static LocString NAME = "Plug Slug Top";

						public static LocString DESC = "A great-monument-sized Plug Slug. Does not require a power source.";
					}

					public class OPTION_U
					{
						public static LocString NAME = "Grubgrub Top";

						public static LocString DESC = "A great-monument-sized garden critter.";
					}

					public class OPTION_V
					{
						public static LocString NAME = "Rover Top";

						public static LocString DESC = "It has no mouth, but still looks like it's smiling.";
					}

					public class OPTION_W
					{
						public static LocString NAME = "Radsick Top I";

						public static LocString DESC = "A visual reminder about radiation safety.";
					}

					public class OPTION_X
					{
						public static LocString NAME = "Radsick Top II";

						public static LocString DESC = "Progress comes at a price.";
					}

					public class OPTION_Y
					{
						public static LocString NAME = "Radsick Top III";

						public static LocString DESC = "A cautionary tale for careless Duplicants.";
					}

					public class OPTION_Z
					{
						public static LocString NAME = "Radsick Top IV";

						public static LocString DESC = "Excellent choice of decor for the entrance to highly radioactive site.";
					}
				}
			}

			public class MICROBEMUSHER
			{
				public static LocString NAME = UI.FormatAsLink("Microbe Musher", "MICROBEMUSHER");

				public static LocString DESC = "Musher recipes will keep Duplicants fed, but may impact health and morale over time.";

				public static LocString EFFECT = "Produces low quality " + UI.FormatAsLink("Food", "FOOD") + " using common ingredients.\n\nDuplicants will not fabricate items unless recipes are queued.";

				public class FACADES
				{
					public class DEFAULT_MICROBEMUSHER
					{
						public static LocString NAME = UI.FormatAsLink("Microbe Musher", "MICROBEMUSHER");

						public static LocString DESC = "Musher recipes will keep Duplicants fed, but may impact health and morale over time.";
					}

					public class PURPLE_BRAINFAT
					{
						public static LocString NAME = UI.FormatAsLink("Faint Purple Microbe Musher", "MICROBEMUSHER");

						public static LocString DESC = "A colorful distraction from the actual quality of the food.";
					}

					public class YELLOW_TARTAR
					{
						public static LocString NAME = UI.FormatAsLink("Ick Yellow Microbe Musher", "MICROBEMUSHER");

						public static LocString DESC = "Makes meals that are memorable for all the wrong reasons.";
					}

					public class RED_ROSE
					{
						public static LocString NAME = UI.FormatAsLink("Puce Pink Microbe Musher", "MICROBEMUSHER");

						public static LocString DESC = "Hunger strikes are not an option, but color-coordination is.";
					}

					public class GREEN_MUSH
					{
						public static LocString NAME = UI.FormatAsLink("Mush Green Microbe Musher", "MICROBEMUSHER");

						public static LocString DESC = "Edible colloids for dinner <i>again</i>?";
					}

					public class BLUE_BABYTEARS
					{
						public static LocString NAME = UI.FormatAsLink("Weepy Blue Microbe Musher", "MICROBEMUSHER");

						public static LocString DESC = "Prioritizes nutritional value over flavor.";
					}
				}
			}

			public class MINERALDEOXIDIZER
			{
				public static LocString NAME = UI.FormatAsLink("Oxygen Diffuser", "MINERALDEOXIDIZER");

				public static LocString DESC = "Oxygen diffusers are inefficient, but output enough oxygen to keep a colony breathing.";

				public static LocString EFFECT = string.Concat(new string[]
				{
					"Converts large amounts of ",
					UI.FormatAsLink("Algae", "ALGAE"),
					" into ",
					UI.FormatAsLink("Oxygen", "OXYGEN"),
					".\n\nBecomes idle when the area reaches maximum pressure capacity."
				});
			}

			public class SUBLIMATIONSTATION
			{
				public static LocString NAME = UI.FormatAsLink("Sublimation Station", "SUBLIMATIONSTATION");

				public static LocString DESC = "Sublimation is the sublime process by which solids convert directly into gas.";

				public static LocString EFFECT = string.Concat(new string[]
				{
					"Speeds up the conversion of ",
					UI.FormatAsLink("Polluted Dirt", "TOXICSAND"),
					" into ",
					UI.FormatAsLink("Polluted Oxygen", "CONTAMINATEDOXYGEN"),
					".\n\nBecomes idle when the area reaches maximum pressure capacity."
				});
			}

			public class WOODTILE
			{
				public static LocString NAME = "Wood Tile";

				public static LocString DESC = "Rooms built with wood tile are cozy and pleasant.";

				public static LocString EFFECT = string.Concat(new string[]
				{
					"Used to build the walls and floors of rooms.\n\nProvides good insulation and boosts ",
					UI.FormatAsLink("Decor", "DECOR"),
					", contributing to ",
					UI.FormatAsLink("Morale", "MORALE"),
					"."
				});
			}

			public class SNOWTILE
			{
				public static LocString NAME = "Snow Tile";

				public static LocString DESC = "Snow tiles have low thermal conductivity, but will melt if temperatures get too high.";

				public static LocString EFFECT = "Used to build the walls and floors of rooms.\n\nInsulates rooms to reduce " + UI.FormatAsLink("Heat", "HEAT") + " loss in cold climates.";
			}

			public class CAMPFIRE
			{
				public static LocString NAME = UI.FormatAsLink("Wood Heater", "CAMPFIRE");

				public static LocString DESC = "Wood heaters dry out soggy feet and help Duplicants forget how cold they are.";

				public static LocString EFFECT = string.Concat(new string[]
				{
					"Consumes ",
					UI.FormatAsLink("Wood", "WOOD"),
					" in order to ",
					UI.FormatAsLink("Heat", "HEAT"),
					" chilly surroundings."
				});
			}

			public class ICEKETTLE
			{
				public static LocString NAME = UI.FormatAsLink("Ice Liquefier", "ICEKETTLE");

				public static LocString DESC = "The water never gets hot enough to burn the tongue.";

				public static LocString EFFECT = string.Concat(new string[]
				{
					"Consumes ",
					UI.FormatAsLink("Wood", "WOOD"),
					" to melt ",
					UI.FormatAsLink("Ice", "ICE"),
					" into ",
					UI.FormatAsLink("Water", "WATER"),
					", which can be bottled for transport."
				});
			}

			public class WOODSTORAGE
			{
				public static LocString NAME = "Wood Pile";

				public static LocString DESC = "Once it's empty, there's no use pining for more.";

				public static LocString EFFECT = string.Concat(new string[]
				{
					"Stores a finite supply of ",
					UI.FormatAsLink("Wood", "WOOD"),
					", which can be used for construction or to produce ",
					UI.FormatAsLink("Heat", "HEAT"),
					"."
				});
			}

			public class DLC2POITECHUNLOCKS
			{
				public static LocString NAME = "Research Portal";

				public static LocString DESC = "A functional research decrypter with one transmission remaining.\n\nIt was designed to support colony survival.";
			}

			public class DLC4POITECHUNLOCKS
			{
				public static LocString NAME = "Research Portal";

				public static LocString DESC = "A functional research decrypter with one transmission remaining.\n\nIt was designed to support colony survival.";
			}

			public class DEEPFRYER
			{
				public static LocString NAME = UI.FormatAsLink("Deep Fryer", "DEEPFRYER");

				public static LocString DESC = "Everything tastes better when it's deep-fried.";

				public static LocString EFFECT = string.Concat(new string[]
				{
					"Uses ",
					UI.FormatAsLink("Tallow", "TALLOW"),
					" to cook a wide variety of improved ",
					UI.FormatAsLink("Foods", "FOOD"),
					".\n\nDuplicants will not fabricate items unless recipes are queued."
				});

				public class STATUSITEMS
				{
					public class OUTSIDE_KITCHEN
					{
						public static LocString NAME = "Outside of Kitchen";

						public static LocString TOOLTIP = "This building must be in a Kitchen before it can be used";
					}
				}
			}

			public class ORESCRUBBER
			{
				public static LocString NAME = UI.FormatAsLink("Ore Scrubber", "ORESCRUBBER");

				public static LocString DESC = "Scrubbers sanitize freshly mined materials before they're brought into the colony.";

				public static LocString EFFECT = string.Concat(new string[]
				{
					"Kills a significant amount of ",
					UI.FormatAsLink("Germs", "DISEASE"),
					" present on ",
					UI.FormatAsLink("Raw Ore", "RAWMINERAL"),
					"."
				});
			}

			public class OUTHOUSE
			{
				public static LocString NAME = UI.FormatAsLink("Outhouse", "OUTHOUSE");

				public static LocString DESC = "The colony that eats together, excretes together.";

				public static LocString EFFECT = string.Concat(new string[]
				{
					"Gives Duplicants a place to relieve themselves.\n\nRequires no ",
					UI.FormatAsLink("Piping", "LIQUIDPIPING"),
					".\n\nMust be periodically emptied of ",
					UI.FormatAsLink("Polluted Dirt", "TOXICSAND"),
					"."
				});
			}

			public class APOTHECARY
			{
				public static LocString NAME = UI.FormatAsLink("Apothecary", "APOTHECARY");

				public static LocString DESC = "Some medications help prevent diseases, while others aim to alleviate existing illness.";

				public static LocString EFFECT = string.Concat(new string[]
				{
					"Produces ",
					UI.FormatAsLink("Medicine", "MEDICINE"),
					" to cure most basic ",
					UI.FormatAsLink("Diseases", "DISEASE"),
					".\n\nDuplicants must possess the Medicine Compounding ",
					UI.FormatAsLink("Skill", "ROLES"),
					" to fabricate medicines.\n\nDuplicants will not fabricate items unless recipes are queued."
				});
			}

			public class ADVANCEDAPOTHECARY
			{
				public static LocString NAME = UI.FormatAsLink("Nuclear Apothecary", "ADVANCEDAPOTHECARY");

				public static LocString DESC = "Some medications help prevent diseases, while others aim to alleviate existing illness.";

				public static LocString EFFECT = string.Concat(new string[]
				{
					"Produces ",
					UI.FormatAsLink("Medicine", "MEDICINE"),
					" to cure most basic ",
					UI.FormatAsLink("Diseases", "DISEASE"),
					".\n\nDuplicants must possess the Medicine Compounding ",
					UI.FormatAsLink("Skill", "ROLES"),
					" to fabricate medicines.\n\nDuplicants will not fabricate items unless recipes are queued."
				});
			}

			public class PLANTERBOX
			{
				public static LocString NAME = UI.FormatAsLink("Planter Box", "PLANTERBOX");

				public static LocString DESC = "Domestically grown seeds mature more quickly than wild plants.";

				public static LocString EFFECT = string.Concat(new string[]
				{
					"Grows one ",
					UI.FormatAsLink("Plant", "PLANTS"),
					" from a ",
					UI.FormatAsLink("Seed", "PLANTS"),
					"."
				});

				public class FACADES
				{
					public class DEFAULT_PLANTERBOX
					{
						public static LocString NAME = UI.FormatAsLink("Planter Box", "PLANTERBOX");

						public static LocString DESC = "Domestically grown seeds mature more quickly than wild plants.";
					}

					public class MEALWOOD
					{
						public static LocString NAME = UI.FormatAsLink("Mealy Teal Planter Box", "PLANTERBOX");

						public static LocString DESC = "Inspired by genetically modified nature.";
					}

					public class BRISTLEBLOSSOM
					{
						public static LocString NAME = UI.FormatAsLink("Bristly Green Planter Box", "PLANTERBOX");

						public static LocString DESC = "The interior is lined with tiny barbs.";
					}

					public class WHEEZEWORT
					{
						public static LocString NAME = UI.FormatAsLink("Wheezy Whorl Planter Box", "PLANTERBOX");

						public static LocString DESC = "For the dreamy agriculturalist.";
					}

					public class SLEETWHEAT
					{
						public static LocString NAME = UI.FormatAsLink("Sleet Blue Planter Box", "PLANTERBOX");

						public static LocString DESC = "The thick paint drips are invisible from a distance.";
					}

					public class SALMON_PINK
					{
						public static LocString NAME = UI.FormatAsLink("Flashy Planter Box", "PLANTERBOX");

						public static LocString DESC = "It's not exactly a subtle color.";
					}
				}
			}

			public class PRESSUREDOOR
			{
				public static LocString NAME = UI.FormatAsLink("Mechanized Airlock", "PRESSUREDOOR");

				public static LocString DESC = "Mechanized airlocks open and close more quickly than other types of door.";

				public static LocString EFFECT = string.Concat(new string[]
				{
					"Blocks ",
					UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID"),
					" and ",
					UI.FormatAsLink("Gas", "ELEMENTS_GAS"),
					" flow, maintaining pressure between areas.\n\nFunctions as a ",
					UI.FormatAsLink("Manual Airlock", "MANUALPRESSUREDOOR"),
					" when no ",
					UI.FormatAsLink("Power", "POWER"),
					" is available.\n\nWild ",
					UI.FormatAsLink("Critters", "CREATURES"),
					" cannot pass through doors."
				});
			}

			public class BUNKERDOOR
			{
				public static LocString NAME = UI.FormatAsLink("Bunker Door", "BUNKERDOOR");

				public static LocString DESC = "A massive, slow-moving door which is nearly indestructible.";

				public static LocString EFFECT = string.Concat(new string[]
				{
					"Blocks ",
					UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID"),
					" and ",
					UI.FormatAsLink("Gas", "ELEMENTS_GAS"),
					" flow, maintaining pressure between areas.\n\nCan withstand extremely high pressures and impacts."
				});
			}

			public class RATIONBOX
			{
				public static LocString NAME = UI.FormatAsLink("Ration Box", "RATIONBOX");

				public static LocString DESC = "Ration boxes keep food safe from hungry critters, but don't slow food spoilage.";

				public static LocString EFFECT = "Stores a small amount of " + UI.FormatAsLink("Food", "FOOD") + ".\n\nFood must be delivered to boxes by Duplicants.";
			}

			public class PARKSIGN
			{
				public static LocString NAME = UI.FormatAsLink("Park Sign", "PARKSIGN");

				public static LocString DESC = "Passing through parks will increase Duplicant Morale.";

				public static LocString EFFECT = "Classifies an area as a Park or Nature Reserve.";
			}

			public class RADIATIONLIGHT
			{
				public static LocString NAME = UI.FormatAsLink("Radiation Lamp", "RADIATIONLIGHT");

				public static LocString DESC = "Duplicants can become sick if exposed to radiation without protection.";

				public static LocString EFFECT = string.Concat(new string[]
				{
					"Emits ",
					UI.FormatAsLink("Radiation", "RADIATION"),
					" when ",
					UI.FormatAsLink("Powered", "POWER"),
					" that can be collected by a ",
					UI.FormatAsLink("Radbolt Generator", "HIGHENERGYPARTICLESPAWNER"),
					"."
				});
			}

			public class REFRIGERATOR
			{
				public static LocString NAME = UI.FormatAsLink("Refrigerator", "REFRIGERATOR");

				public static LocString DESC = "Food spoilage can be slowed by ambient conditions as well as by refrigerators.";

				public static LocString EFFECT = string.Concat(new string[]
				{
					"Stores ",
					UI.FormatAsLink("Food", "FOOD"),
					" at an ideal ",
					UI.FormatAsLink("Temperature", "HEAT"),
					" to prevent spoilage."
				});

				public static LocString LOGIC_PORT = "Full/Not Full";

				public static LocString LOGIC_PORT_ACTIVE = "Sends a " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + " when full";

				public static LocString LOGIC_PORT_INACTIVE = "Otherwise, sends a " + UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby);

				public class FACADES
				{
					public class DEFAULT_REFRIGERATOR
					{
						public static LocString NAME = UI.FormatAsLink("Refrigerator", "REFRIGERATOR");

						public static LocString DESC = "Food spoilage can be slowed by ambient conditions as well as by refrigerators.";
					}

					public class STRIPES_RED_WHITE
					{
						public static LocString NAME = UI.FormatAsLink("Bold Stripe Refrigerator", "REFRIGERATOR");

						public static LocString DESC = "Bold on the outside, cold on the inside!";
					}

					public class BLUE_BABYTEARS
					{
						public static LocString NAME = UI.FormatAsLink("Weepy Blue Refrigerator", "REFRIGERATOR");

						public static LocString DESC = "For food so cold, it brings a tear to the eye.";
					}

					public class GREEN_MUSH
					{
						public static LocString NAME = UI.FormatAsLink("Mush Green Refrigerator", "REFRIGERATOR");

						public static LocString DESC = "Honestly, this hue is particularly chilling.";
					}

					public class RED_ROSE
					{
						public static LocString NAME = UI.FormatAsLink("Puce Pink Refrigerator", "REFRIGERATOR");

						public static LocString DESC = "Inspired by the Duplicant poem, \"Pretty in Puce.\"";
					}

					public class YELLOW_TARTAR
					{
						public static LocString NAME = UI.FormatAsLink("Ick Yellow Refrigerator", "REFRIGERATOR");

						public static LocString DESC = "Some Duplicants call it \"sunny\" yellow, but only because they've never seen the sun.";
					}

					public class PURPLE_BRAINFAT
					{
						public static LocString NAME = UI.FormatAsLink("Faint Purple Refrigerator", "REFRIGERATOR");

						public static LocString DESC = "This fridge makes color-coordination a (cold) snap.";
					}
				}
			}

			public class ROLESTATION
			{
				public static LocString NAME = UI.FormatAsLink("Skills Board", "ROLESTATION");

				public static LocString DESC = "A skills board can teach special skills to Duplicants they can't learn on their own.";

				public static LocString EFFECT = "Allows Duplicants to spend Skill Points to learn new " + UI.FormatAsLink("Skills", "JOBS") + ".";
			}

			public class RESETSKILLSSTATION
			{
				public static LocString NAME = UI.FormatAsLink("Skill Scrubber", "RESETSKILLSSTATION");

				public static LocString DESC = "Erase skills from a Duplicant's mind, returning them to their default abilities.";

				public static LocString EFFECT = "Refunds a Duplicant's Skill Points for reassignment.\n\nDuplicants will lose all assigned skills in the process.";
			}

			public class RESEARCHCENTER
			{
				public static LocString NAME = UI.FormatAsLink("Research Station", "RESEARCHCENTER");

				public static LocString DESC = "Research stations are necessary for unlocking all research tiers.";

				public static LocString EFFECT = string.Concat(new string[]
				{
					"Conducts ",
					UI.FormatAsLink("Novice Research", "RESEARCH"),
					" to unlock new technologies.\n\nConsumes ",
					UI.FormatAsLink("Dirt", "DIRT"),
					"."
				});
			}

			public class ADVANCEDRESEARCHCENTER
			{
				public static LocString NAME = UI.FormatAsLink("Super Computer", "ADVANCEDRESEARCHCENTER");

				public static LocString DESC = "Super computers unlock higher technology tiers than research stations alone.";

				public static LocString EFFECT = string.Concat(new string[]
				{
					"Conducts ",
					UI.FormatAsLink("Advanced Research", "RESEARCH"),
					" to unlock new technologies.\n\nConsumes ",
					UI.FormatAsLink("Water", "WATER"),
					".\n\nAssigned Duplicants must possess the ",
					UI.FormatAsLink("Advanced Research", "RESEARCHING1"),
					" skill."
				});
			}

			public class NUCLEARRESEARCHCENTER
			{
				public static LocString NAME = UI.FormatAsLink("Materials Study Terminal", "NUCLEARRESEARCHCENTER");

				public static LocString DESC = "Comes with a few ions thrown in, free of charge.";

				public static LocString EFFECT = string.Concat(new string[]
				{
					"Conducts ",
					UI.FormatAsLink("Materials Science Research", "RESEARCHDLC1"),
					" to unlock new technologies.\n\nConsumes Radbolts.\n\nAssigned Duplicants must possess the ",
					UI.FormatAsLink("Applied Sciences Research", "ATOMICRESEARCH"),
					" skill."
				});
			}

			public class ORBITALRESEARCHCENTER
			{
				public static LocString NAME = UI.FormatAsLink("Orbital Data Collection Lab", "ORBITALRESEARCHCENTER");

				public static LocString DESC = "Orbital Data Collection Labs record data while orbiting a Planetoid and write it to a " + UI.FormatAsLink("Data Bank", "ORBITALRESEARCHDATABANK") + ". ";

				public static LocString EFFECT = string.Concat(new string[]
				{
					"Creates ",
					UI.FormatAsLink("Data Banks", "ORBITALRESEARCHDATABANK"),
					" that can be consumed at a ",
					UI.FormatAsLink("Virtual Planetarium", "DLC1COSMICRESEARCHCENTER"),
					" to unlock new technologies.\n\nConsumes ",
					UI.FormatAsLink("Plastic", "POLYPROPYLENE"),
					" and ",
					UI.FormatAsLink("Power", "POWER"),
					"."
				});
			}

			public class COSMICRESEARCHCENTER
			{
				public static LocString NAME = UI.FormatAsLink("Virtual Planetarium", "COSMICRESEARCHCENTER");

				public static LocString DESC = "Planetariums allow the simulated exploration of locations discovered with a telescope.";

				public static LocString EFFECT = string.Concat(new string[]
				{
					"Conducts ",
					UI.FormatAsLink("Interstellar Research", "RESEARCH"),
					" to unlock new technologies.\n\nConsumes data from ",
					UI.FormatAsLink("Research Modules", "RESEARCHMODULE"),
					".\n\nAssigned Duplicants must possess the ",
					UI.FormatAsLink("Astronomy", "ASTRONOMY"),
					" skill."
				});
			}

			public class DLC1COSMICRESEARCHCENTER
			{
				public static LocString NAME = UI.FormatAsLink("Virtual Planetarium", "DLC1COSMICRESEARCHCENTER");

				public static LocString DESC = "Planetariums allow the simulated exploration of locations recorded in " + UI.FormatAsLink("Data Banks", "ORBITALRESEARCHDATABANK") + ".";

				public static LocString EFFECT = string.Concat(new string[]
				{
					"Conducts ",
					UI.FormatAsLink("Data Analysis Research", "RESEARCH"),
					" to unlock new technologies.\n\nConsumes ",
					UI.FormatAsLink("Data Banks", "ORBITALRESEARCHDATABANK"),
					" generated by exploration."
				});
			}

			public class TELESCOPE
			{
				public static LocString NAME = UI.FormatAsLink("Telescope", "TELESCOPE");

				public static LocString DESC = "Telescopes are necessary for learning starmaps and conducting rocket missions.";

				public static LocString EFFECT = "Maps Starmap destinations.\n\nAssigned Duplicants must possess the " + UI.FormatAsLink("Field Research", "RESEARCHING2") + " skill.\n\nBuilding must be exposed to space to function.";

				public static LocString REQUIREMENT_TOOLTIP = "A steady {0} supply is required to sustain working Duplicants.";
			}

			public class CLUSTERTELESCOPE
			{
				public static LocString NAME = UI.FormatAsLink("Telescope", "CLUSTERTELESCOPE");

				public static LocString DESC = "Telescopes are necessary for studying space, allowing rocket travel to other worlds.";

				public static LocString EFFECT = "Reveals visitable Planetoids in space.\n\nAssigned Duplicants must possess the " + UI.FormatAsLink("Astronomy", "ASTRONOMY") + " skill.\n\nBuilding must be exposed to space to function.";

				public static LocString REQUIREMENT_TOOLTIP = "A steady {0} supply is required to sustain working Duplicants.";
			}

			public class CLUSTERTELESCOPEENCLOSED
			{
				public static LocString NAME = UI.FormatAsLink("Enclosed Telescope", "CLUSTERTELESCOPEENCLOSED");

				public static LocString DESC = "Telescopes are necessary for studying space, allowing rocket travel to other worlds.";

				public static LocString EFFECT = string.Concat(new string[]
				{
					"Reveals visitable Planetoids in space... in comfort!\n\nAssigned Duplicants must possess the ",
					UI.FormatAsLink("Astronomy", "ASTRONOMY"),
					" skill.\n\nExcellent sunburn protection (100%), partial ",
					UI.FormatAsLink("Radiation", "RADIATION"),
					" protection (",
					GameUtil.GetFormattedPercent(FIXEDTRAITS.COSMICRADIATION.TELESCOPE_RADIATION_SHIELDING * 100f, GameUtil.TimeSlice.None),
					") .\n\nBuilding must be exposed to space to function."
				});

				public static LocString REQUIREMENT_TOOLTIP = "A steady {0} supply is required to sustain working Duplicants.";
			}

			public class MISSIONCONTROL
			{
				public static LocString NAME = UI.FormatAsLink("Mission Control Station", "MISSIONCONTROL");

				public static LocString DESC = "Like a backseat driver who actually does know better.";

				public static LocString EFFECT = "Provides guidance data to rocket pilots, to improve rocket speed.\n\nMust be operated by a Duplicant with the " + UI.FormatAsLink("Astronomy", "ASTRONOMY") + " skill.\n\nRequires a clear line of sight to space in order to function.";
			}

			public class MISSIONCONTROLCLUSTER
			{
				public static LocString NAME = UI.FormatAsLink("Mission Control Station", "MISSIONCONTROLCLUSTER");

				public static LocString DESC = "Like a backseat driver who actually does know better.";

				public static LocString EFFECT = "Provides guidance data to rocket pilots within range, to improve rocket speed.\n\nMust be operated by a Duplicant with the " + UI.FormatAsLink("Astronomy", "ASTRONOMY") + " skill.\n\nRequires a clear line of sight to space in order to function.";
			}

			public class SCULPTURE
			{
				public static LocString NAME = UI.FormatAsLink("Large Sculpting Block", "SCULPTURE");

				public static LocString DESC = "Duplicants who have learned art skills can produce more decorative sculptures.";

				public static LocString EFFECT = string.Concat(new string[]
				{
					"Moderately increases ",
					UI.FormatAsLink("Decor", "DECOR"),
					", contributing to ",
					UI.FormatAsLink("Morale", "MORALE"),
					".\n\nMust be sculpted by a Duplicant."
				});

				public static LocString POORQUALITYNAME = "\"Abstract\" Sculpture";

				public static LocString AVERAGEQUALITYNAME = "Mediocre Sculpture";

				public static LocString EXCELLENTQUALITYNAME = "Genius Sculpture";

				public class FACADES
				{
					public class SCULPTURE_GOOD_1
					{
						public static LocString NAME = UI.FormatAsLink("O Cupid, My Cupid", "SCULPTURE_GOOD_1");

						public static LocString DESC = "Ode to the bow and arrow, love's equivalent to a mining gun...but for hearts.";
					}

					public class SCULPTURE_CRAP_1
					{
						public static LocString NAME = UI.FormatAsLink("Inexplicable", "SCULPTURE_CRAP_1");

						public static LocString DESC = "A valiant attempt at art.";
					}

					public class SCULPTURE_AMAZING_2
					{
						public static LocString NAME = UI.FormatAsLink("Plate Chucker", "SCULPTURE_AMAZING_2");

						public static LocString DESC = "A masterful portrayal of an athlete who's been banned from the communal kitchen.";
					}

					public class SCULPTURE_AMAZING_3
					{
						public static LocString NAME = UI.FormatAsLink("Before Battle", "SCULPTURE_AMAZING_3");

						public static LocString DESC = "A masterful portrayal of a slingshot-wielding hero.";
					}

					public class SCULPTURE_AMAZING_4
					{
						public static LocString NAME = UI.FormatAsLink("Grandiose Grub-Grub", "SCULPTURE_AMAZING_4");

						public static LocString DESC = "A masterful portrayal of a gentle, plant-tending critter.";
					}

					public class SCULPTURE_AMAZING_1
					{
						public static LocString NAME = UI.FormatAsLink("The Hypothesizer", "SCULPTURE_AMAZING_1");

						public static LocString DESC = "A masterful portrayal of a scientist lost in thought.";
					}

					public class SCULPTURE_AMAZING_5
					{
						public static LocString NAME = UI.FormatAsLink("Vertical Cosmos", "SCULPTURE_AMAZING_5");

						public static LocString DESC = "It contains multitudes.";
					}

					public class SCULPTURE_AMAZING_6
					{
						public static LocString NAME = UI.FormatAsLink("Into the Voids", "SCULPTURE_AMAZING_6");

						public static LocString DESC = "No amount of material success will ever fill the void within.";
					}
				}
			}

			public class ICESCULPTURE
			{
				public static LocString NAME = UI.FormatAsLink("Ice Block", "ICESCULPTURE");

				public static LocString DESC = "Prone to melting.";

				public static LocString EFFECT = string.Concat(new string[]
				{
					"Majorly increases ",
					UI.FormatAsLink("Decor", "DECOR"),
					", contributing to ",
					UI.FormatAsLink("Morale", "MORALE"),
					".\n\nMust be sculpted by a Duplicant."
				});

				public static LocString POORQUALITYNAME = "\"Abstract\" Ice Sculpture";

				public static LocString AVERAGEQUALITYNAME = "Mediocre Ice Sculpture";

				public static LocString EXCELLENTQUALITYNAME = "Genius Ice Sculpture";

				public class FACADES
				{
					public class ICESCULPTURE_CRAP
					{
						public static LocString NAME = UI.FormatAsLink("Cubi I", "ICESCULPTURE_CRAP");

						public static LocString DESC = "It's structurally unsound, but otherwise not entirely terrible.";
					}

					public class ICESCULPTURE_AMAZING_1
					{
						public static LocString NAME = UI.FormatAsLink("Exquisite Chompers", "ICESCULPTURE_AMAZING_1");

						public static LocString DESC = "These incisors are the stuff of dental legend.";
					}

					public class ICESCULPTURE_AMAZING_2
					{
						public static LocString NAME = UI.FormatAsLink("Frosty Crustacean", "ICESCULPTURE_AMAZING_2");

						public static LocString DESC = "A charming depiction of the mighty Pokeshell in mid-rampage.";
					}

					public class ICESCULPTURE_AMAZING_3
					{
						public static LocString NAME = UI.FormatAsLink("The Chase", "ICESCULPTURE_AMAZING_3");

						public static LocString DESC = "Some aquarists posit that Pacus are the original creators of the game now known as \"Tag.\"";
					}
				}
			}

			public class MARBLESCULPTURE
			{
				public static LocString NAME = UI.FormatAsLink("Marble Block", "MARBLESCULPTURE");

				public static LocString DESC = "Duplicants who have learned art skills can produce more decorative sculptures.";

				public static LocString EFFECT = string.Concat(new string[]
				{
					"Majorly increases ",
					UI.FormatAsLink("Decor", "DECOR"),
					", contributing to ",
					UI.FormatAsLink("Morale", "MORALE"),
					".\n\nMust be sculpted by a Duplicant."
				});

				public static LocString POORQUALITYNAME = "\"Abstract\" Marble Sculpture";

				public static LocString AVERAGEQUALITYNAME = "Mediocre Marble Sculpture";

				public static LocString EXCELLENTQUALITYNAME = "Genius Marble Sculpture";

				public class FACADES
				{
					public class SCULPTURE_MARBLE_CRAP_1
					{
						public static LocString NAME = UI.FormatAsLink("Lumpy Fungus", "SCULPTURE_MARBLE_CRAP_1");

						public static LocString DESC = "The artist was a very fungi.";
					}

					public class SCULPTURE_MARBLE_GOOD_1
					{
						public static LocString NAME = UI.FormatAsLink("Unicorn Bust", "SCULPTURE_MARBLE_GOOD_1");

						public static LocString DESC = "It has real \"mane\" character energy.";
					}

					public class SCULPTURE_MARBLE_AMAZING_1
					{
						public static LocString NAME = UI.FormatAsLink("The Large-ish Mermaid", "SCULPTURE_MARBLE_AMAZING_1");

						public static LocString DESC = "She's not afraid to take up space.";
					}

					public class SCULPTURE_MARBLE_AMAZING_2
					{
						public static LocString NAME = UI.FormatAsLink("Grouchy Beast", "SCULPTURE_MARBLE_AMAZING_2");

						public static LocString DESC = "The artist took great pleasure in conveying their displeasure.";
					}

					public class SCULPTURE_MARBLE_AMAZING_3
					{
						public static LocString NAME = UI.FormatAsLink("The Guardian", "SCULPTURE_MARBLE_AMAZING_3");

						public static LocString DESC = "Will not play fetch.";
					}

					public class SCULPTURE_MARBLE_AMAZING_4
					{
						public static LocString NAME = UI.FormatAsLink("Truly A-Moo-Zing", "SCULPTURE_MARBLE_AMAZING_4");

						public static LocString DESC = "A masterful celebration of one of the universe's most mysterious - and flatulent - organisms.";
					}

					public class SCULPTURE_MARBLE_AMAZING_5
					{
						public static LocString NAME = UI.FormatAsLink("Green Goddess", "SCULPTURE_MARBLE_AMAZING_5");

						public static LocString DESC = "A masterful celebration of the deep bond between a horticulturalist and her prize Bristle Blossom.";
					}
				}
			}

			public class METALSCULPTURE
			{
				public static LocString NAME = UI.FormatAsLink("Metal Block", "METALSCULPTURE");

				public static LocString DESC = "Duplicants who have learned art skills can produce more decorative sculptures.";

				public static LocString EFFECT = string.Concat(new string[]
				{
					"Majorly increases ",
					UI.FormatAsLink("Decor", "DECOR"),
					", contributing to ",
					UI.FormatAsLink("Morale", "MORALE"),
					".\n\nMust be sculpted by a Duplicant."
				});

				public static LocString POORQUALITYNAME = "\"Abstract\" Metal Sculpture";

				public static LocString AVERAGEQUALITYNAME = "Mediocre Metal Sculpture";

				public static LocString EXCELLENTQUALITYNAME = "Genius Metal Sculpture";

				public class FACADES
				{
					public class SCULPTURE_METAL_CRAP_1
					{
						public static LocString NAME = UI.FormatAsLink("Unnatural Beauty", "SCULPTURE_METAL_CRAP_1");

						public static LocString DESC = "Actually, it's a very good likeness.";
					}

					public class SCULPTURE_METAL_GOOD_1
					{
						public static LocString NAME = UI.FormatAsLink("Beautiful Biohazard", "SCULPTURE_METAL_GOOD_1");

						public static LocString DESC = "The Morb's eye is mounted on a swivel that activates at random intervals.";
					}

					public class SCULPTURE_METAL_AMAZING_1
					{
						public static LocString NAME = UI.FormatAsLink("Insatiable Appetite", "SCULPTURE_METAL_AMAZING_1");

						public static LocString DESC = "It's quite lovely, until someone stubs their toe on it in the dark.";
					}

					public class SCULPTURE_METAL_AMAZING_2
					{
						public static LocString NAME = UI.FormatAsLink("Agape", "SCULPTURE_METAL_AMAZING_2");

						public static LocString DESC = "Not quite expressionist, but undeniably expressive.";
					}

					public class SCULPTURE_METAL_AMAZING_3
					{
						public static LocString NAME = UI.FormatAsLink("Friendly Flier", "SCULPTURE_METAL_AMAZING_3");

						public static LocString DESC = "It emits no light, but it sure does brighten up a room.";
					}

					public class SCULPTURE_METAL_AMAZING_4
					{
						public static LocString NAME = UI.FormatAsLink("Whatta Pip", "SCULPTURE_METAL_AMAZING_4");

						public static LocString DESC = "A masterful likeness of the mischievous critter that Duplicants love to love.";
					}

					public class SCULPTURE_METAL_AMAZING_5
					{
						public static LocString NAME = UI.FormatAsLink("Phrenologist's Dream", "SCULPTURE_METAL_AMAZING_5");

						public static LocString DESC = "What if the entire head is one big bump?";
					}
				}
			}

			public class SMALLSCULPTURE
			{
				public static LocString NAME = UI.FormatAsLink("Sculpting Block", "SMALLSCULPTURE");

				public static LocString DESC = "Duplicants who have learned art skills can produce more decorative sculptures.";

				public static LocString EFFECT = string.Concat(new string[]
				{
					"Minorly increases ",
					UI.FormatAsLink("Decor", "DECOR"),
					", contributing to ",
					UI.FormatAsLink("Morale", "MORALE"),
					".\n\nMust be sculpted by a Duplicant."
				});

				public static LocString POORQUALITYNAME = "\"Abstract\" Sculpture";

				public static LocString AVERAGEQUALITYNAME = "Mediocre Sculpture";

				public static LocString EXCELLENTQUALITYNAME = "Genius Sculpture";

				public class FACADES
				{
					public class SCULPTURE_1x2_GOOD
					{
						public static LocString NAME = UI.FormatAsLink("Lunar Slice", "SCULPTURE_1x2_GOOD");

						public static LocString DESC = "It must be a moon, because there are no bananas in space.";
					}

					public class SCULPTURE_1x2_CRAP
					{
						public static LocString NAME = UI.FormatAsLink("Unrequited", "SCULPTURE_1x2_CRAP");

						public static LocString DESC = "It's a heavy heart.";
					}

					public class SCULPTURE_1x2_AMAZING_1
					{
						public static LocString NAME = UI.FormatAsLink("Not a Funnel", "SCULPTURE_1x2_AMAZING_1");

						public static LocString DESC = "<i>Ceci n'est pas un entonnoir.</i>";
					}

					public class SCULPTURE_1x2_AMAZING_2
					{
						public static LocString NAME = UI.FormatAsLink("Equilibrium", "SCULPTURE_1x2_AMAZING_2");

						public static LocString DESC = "Part of a well-balanced exhibit.";
					}

					public class SCULPTURE_1x2_AMAZING_3
					{
						public static LocString NAME = UI.FormatAsLink("Opaque Orb", "SCULPTURE_1x2_AMAZING_3");

						public static LocString DESC = "It lacks transparency.";
					}

					public class SCULPTURE_1x2_AMAZING_4
					{
						public static LocString NAME = UI.FormatAsLink("Employee of the Month", "SCULPTURE_1x2_AMAZING_4");

						public static LocString DESC = "A masterful celebration of the Sweepy's unbeatable work ethic and cheerful, can-clean attitude.";
					}

					public class SCULPTURE_1x2_AMAZING_5
					{
						public static LocString NAME = UI.FormatAsLink("Pointy Impossibility", "SCULPTURE_1x2_AMAZING_5");

						public static LocString DESC = "A three-dimensional rebellion against the rules of Euclidean space.";
					}

					public class SCULPTURE_1x2_AMAZING_6
					{
						public static LocString NAME = UI.FormatAsLink("Fireball", "SCULPTURE_1x2_AMAZING_6");

						public static LocString DESC = "Tribute to the artist's friend, who once attempted to catch a meteor with their bare hands.";
					}
				}
			}

			public class WOODSCULPTURE
			{
				public static LocString NAME = UI.FormatAsLink("Wood Block", "WOODSCULPTURE");

				public static LocString DESC = "A great fit for smaller spaces.";

				public static LocString EFFECT = string.Concat(new string[]
				{
					"Moderately increases ",
					UI.FormatAsLink("Decor", "DECOR"),
					", contributing to ",
					UI.FormatAsLink("Morale", "MORALE"),
					".\n\nMust be sculpted by a Duplicant."
				});

				public static LocString POORQUALITYNAME = "\"Abstract\" Wood Sculpture";

				public static LocString AVERAGEQUALITYNAME = "Mediocre Wood Sculpture";

				public static LocString EXCELLENTQUALITYNAME = "Genius Wood Sculpture";
			}

			public class SHEARINGSTATION
			{
				public static LocString NAME = UI.FormatAsLink("Shearing Station", "SHEARINGSTATION");

				public static LocString DESC = "Those critters aren't gonna shear themselves.";

				public static LocString EFFECT = string.Concat(new string[]
				{
					"Shearing stations allow eligible ",
					UI.FormatAsLink("Critters", "CREATURES"),
					" to be safely sheared for useful raw materials.\n\nVisiting this building restores ",
					UI.FormatAsLink("Critters'", "CREATURES"),
					" physical and emotional well-being."
				});
			}

			public class OXYGENMASKSTATION
			{
				public static LocString NAME = UI.FormatAsLink("Oxygen Mask Station", "OXYGENMASKSTATION");

				public static LocString DESC = "Duplicants can't pass by a station if it lacks enough oxygen to fill a mask.";

				public static LocString EFFECT = string.Concat(new string[]
				{
					"Uses designated ",
					UI.FormatAsLink("Metal Ores", "METAL"),
					" from filter settings to create ",
					UI.FormatAsLink("Oxygen Masks", "OXYGENMASK"),
					".\n\nAutomatically draws in ambient ",
					UI.FormatAsLink("Oxygen", "OXYGEN"),
					" to fill masks.\n\nMarks a threshold where Duplicants must put on or take off a mask.\n\nCan be rotated before construction."
				});
			}

			public class SWEEPBOTSTATION
			{
				public static LocString NAME = UI.FormatAsLink("Sweepy's Dock", "SWEEPBOTSTATION");

				public static LocString NAMEDSTATION = "{0}'s Dock";

				public static LocString DESC = "The cute little face comes pre-installed.";

				public static LocString EFFECT = string.Concat(new string[]
				{
					"Deploys an automated ",
					UI.FormatAsLink("Sweepy Bot", "SWEEPBOT"),
					" to sweep up ",
					UI.FormatAsLink("Solid", "ELEMENTS_SOLID"),
					" debris and ",
					UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID"),
					" spills.\n\nDock stores ",
					UI.FormatAsLink("Liquids", "ELEMENTS_LIQUID"),
					" and ",
					UI.FormatAsLink("Solids", "ELEMENTS_SOLID"),
					" gathered by the Sweepy.\n\nUses ",
					UI.FormatAsLink("Power", "POWER"),
					" to recharge the Sweepy.\n\nDuplicants will empty Dock storage into available storage bins."
				});
			}

			public class OXYGENMASKMARKER
			{
				public static LocString NAME = UI.FormatAsLink("Oxygen Mask Checkpoint", "OXYGENMASKMARKER");

				public static LocString DESC = "A checkpoint must have a correlating dock built on the opposite side its arrow faces.";

				public static LocString EFFECT = string.Concat(new string[]
				{
					"Marks a threshold where Duplicants must put on or take off an ",
					UI.FormatAsLink("Oxygen Mask", "OXYGEN_MASK"),
					".\n\nMust be built next to an ",
					UI.FormatAsLink("Oxygen Mask Dock", "OXYGENMASKLOCKER"),
					".\n\nCan be rotated before construction."
				});
			}

			public class OXYGENMASKLOCKER
			{
				public static LocString NAME = UI.FormatAsLink("Oxygen Mask Dock", "OXYGENMASKLOCKER");

				public static LocString DESC = "An oxygen mask dock will store and refill masks while they're not in use.";

				public static LocString EFFECT = string.Concat(new string[]
				{
					"Stores ",
					UI.FormatAsLink("Oxygen Masks", "OXYGEN_MASK"),
					" and refuels them with ",
					UI.FormatAsLink("Oxygen", "OXYGEN"),
					".\n\nBuild next to an ",
					UI.FormatAsLink("Oxygen Mask Checkpoint", "OXYGENMASKMARKER"),
					" to make Duplicants put on masks when passing by."
				});
			}

			public class SUITMARKER
			{
				public static LocString NAME = UI.FormatAsLink("Atmo Suit Checkpoint", "SUITMARKER");

				public static LocString DESC = "A checkpoint must have a correlating dock built on the opposite side its arrow faces.";

				public static LocString EFFECT = string.Concat(new string[]
				{
					"Marks a threshold where Duplicants must change into or out of ",
					UI.FormatAsLink("Atmo Suits", "ATMO_SUIT"),
					".\n\nMust be built next to an ",
					UI.FormatAsLink("Atmo Suit Dock", "SUITLOCKER"),
					".\n\nCan be rotated before construction."
				});
			}

			public class SUITLOCKER
			{
				public static LocString NAME = UI.FormatAsLink("Atmo Suit Dock", "SUITLOCKER");

				public static LocString DESC = "An atmo suit dock will empty atmo suits of waste, but only one suit can charge at a time.";

				public static LocString EFFECT = string.Concat(new string[]
				{
					"Stores ",
					UI.FormatAsLink("Atmo Suits", "ATMO_SUIT"),
					" and refuels them with ",
					UI.FormatAsLink("Oxygen", "OXYGEN"),
					".\n\nEmpties suits of ",
					UI.FormatAsLink("Polluted Water", "DIRTYWATER"),
					".\n\nBuild next to an ",
					UI.FormatAsLink("Atmo Suit Checkpoint", "SUITMARKER"),
					" to make Duplicants change into suits when passing by."
				});
			}

			public class JETSUITMARKER
			{
				public static LocString NAME = UI.FormatAsLink("Jet Suit Checkpoint", "JETSUITMARKER");

				public static LocString DESC = "A checkpoint must have a correlating dock built on the opposite side its arrow faces.";

				public static LocString EFFECT = string.Concat(new string[]
				{
					"Marks a threshold where Duplicants must change into or out of ",
					UI.FormatAsLink("Jet Suits", "JET_SUIT"),
					".\n\nMust be built next to a ",
					UI.FormatAsLink("Jet Suit Dock", "JETSUITLOCKER"),
					".\n\nCan be rotated before construction."
				});
			}

			public class JETSUITLOCKER
			{
				public static LocString NAME = UI.FormatAsLink("Jet Suit Dock", "JETSUITLOCKER");

				public static LocString DESC = "Jet suit docks can refill jet suits with air and fuel, or empty them of waste.";

				public static LocString EFFECT = string.Concat(new string[]
				{
					"Stores ",
					UI.FormatAsLink("Jet Suits", "JET_SUIT"),
					" and refuels them with ",
					UI.FormatAsLink("Oxygen", "OXYGEN"),
					" and ",
					UI.FormatAsLink("Petroleum", "PETROLEUM"),
					".\n\nEmpties suits of ",
					UI.FormatAsLink("Polluted Water", "DIRTYWATER"),
					".\n\nBuild next to a ",
					UI.FormatAsLink("Jet Suit Checkpoint", "JETSUITMARKER"),
					" to make Duplicants change into suits when passing by."
				});
			}

			public class LEADSUITMARKER
			{
				public static LocString NAME = UI.FormatAsLink("Lead Suit Checkpoint", "LEADSUITMARKER");

				public static LocString DESC = "A checkpoint must have a correlating dock built on the opposite side its arrow faces.";

				public static LocString EFFECT = string.Concat(new string[]
				{
					"Marks a threshold where Duplicants must change into or out of ",
					UI.FormatAsLink("Lead Suits", "LEAD_SUIT"),
					".\n\nMust be built next to a ",
					UI.FormatAsLink("Lead Suit Dock", "LEADSUITLOCKER"),
					"\n\nCan be rotated before construction."
				});
			}

			public class LEADSUITLOCKER
			{
				public static LocString NAME = UI.FormatAsLink("Lead Suit Dock", "LEADSUITLOCKER");

				public static LocString DESC = "Lead suit docks can refill lead suits with air and empty them of waste.";

				public static LocString EFFECT = string.Concat(new string[]
				{
					"Stores ",
					UI.FormatAsLink("Lead Suits", "LEAD_SUIT"),
					" and refuels them with ",
					UI.FormatAsLink("Oxygen", "OXYGEN"),
					".\n\nEmpties suits of ",
					UI.FormatAsLink("Polluted Water", "DIRTYWATER"),
					".\n\nBuild next to a ",
					UI.FormatAsLink("Lead Suit Checkpoint", "LEADSUITMARKER"),
					" to make Duplicants change into suits when passing by."
				});
			}

			public class CRAFTINGTABLE
			{
				public static LocString NAME = UI.FormatAsLink("Crafting Station", "CRAFTINGTABLE");

				public static LocString DESC = "Crafting stations allow Duplicants to make oxygen masks to wear in low breathability areas.";

				public static LocString EFFECT = "Produces items and equipment for Duplicant use.\n\nDuplicants will not fabricate items unless recipes are queued.";

				public static LocString RECIPE_DESCRIPTION = "Converts {0} to {1}";
			}

			public class ADVANCEDCRAFTINGTABLE
			{
				public static LocString NAME = UI.FormatAsLink("Soldering Station", "ADVANCEDCRAFTINGTABLE");

				public static LocString DESC = "Soldering stations allow Duplicants to build helpful Flydo retriever bots.";

				public static LocString EFFECT = "Produces advanced electronics and bionic " + UI.FormatAsLink("Boosters", "BIONIC_UPGRADE") + ".\n\nDuplicants will not fabricate items unless recipes are queued.";

				public static LocString BIONIC_COMPONENT_RECIPE_DESC = "Converts {0} to {1}";

				public static LocString GENERIC_RECIPE_DESCRIPTION = "Converts {0} to {1}";

				public static LocString COLONY_HAS_BOOSTER_ASSIGNED_NONE = "My colony has no Bionic Duplicants with this booster assigned";

				public static LocString COLONY_HAS_BOOSTER_ASSIGNED_COUNT = "My colony has {0} Bionic Duplicant(s) with this booster assigned";
			}

			public class DATAMINER
			{
				public static LocString NAME = UI.FormatAsLink("Data Miner", "DATAMINER");

				public static LocString DESC = "Data banks can also be used to program robo-pilot rocket modules.";

				public static LocString EFFECT = string.Concat(new string[]
				{
					"Mass-produces ",
					UI.FormatAsLink(DatabankHelper.NAME_PLURAL, "Databank"),
					" that can be processed into ",
					UI.FormatAsLink(DatabankHelper.RESEARCH_NAME, DatabankHelper.RESEARCH_CODEXID),
					" points.\n\nDuplicants will not fabricate items unless recipes are queued."
				});

				public static LocString RECIPE_DESCRIPTION = "Turns {0} into {1}.";
			}

			public class REMOTEWORKTERMINAL
			{
				public static LocString NAME = UI.FormatAsLink("Remote Controller", "REMOTEWORKTERMINAL");

				public static LocString DESC = "Remote controllers cut down on colony commute times.";

				public static LocString EFFECT = "Enables Duplicants to operate machinery remotely via a connected " + UI.FormatAsLink("Remote Worker Dock", "REMOTEWORKERDOCK") + ".";
			}

			public class REMOTEWORKERDOCK
			{
				public static LocString NAME = UI.FormatAsLink("Remote Worker Dock", "REMOTEWORKERDOCK");

				public static LocString NAME_FMT = "Dock {ID}";

				public static LocString DESC = "It's a Duplicant's duplicate's dock.";

				public static LocString EFFECT = UI.FormatAsLink("Remote Worker Docks", "REMOTEWORKERDOCK") + " deploy automatons that operate machinery based on instructions received from a connected " + UI.FormatAsLink("Remote Controller", "REMOTEWORKTERMINAL") + ".\n\nMust be placed within range of its target building.";
			}

			public class SUITFABRICATOR
			{
				public static LocString NAME = UI.FormatAsLink("Exosuit Forge", "SUITFABRICATOR");

				public static LocString DESC = "Exosuits can be filled with oxygen to allow Duplicants to safely enter hazardous areas.";

				public static LocString EFFECT = "Forges protective " + UI.FormatAsLink("Exosuits", "EQUIPMENT") + " for Duplicants to wear.\n\nDuplicants will not fabricate items unless recipes are queued.";
			}

			public class CLOTHINGALTERATIONSTATION
			{
				public static LocString NAME = UI.FormatAsLink("Clothing Refashionator", "CLOTHINGALTERATIONSTATION");

				public static LocString DESC = "Allows skilled Duplicants to add extra personal pizzazz to their wardrobe.";

				public static LocString EFFECT = string.Concat(new string[]
				{
					"Upgrades ",
					UI.FormatAsLink("Snazzy Suits", "FUNKY_VEST"),
					" into ",
					UI.FormatAsLink("Primo Garb", "CUSTOM_CLOTHING"),
					".\n\nDuplicants will not fabricate items unless recipes are queued."
				});
			}

			public class CLOTHINGFABRICATOR
			{
				public static LocString NAME = UI.FormatAsLink("Textile Loom", "CLOTHINGFABRICATOR");

				public static LocString DESC = "A textile loom can be used to spin Reed Fiber into wearable Duplicant clothing.";

				public static LocString EFFECT = "Tailors Duplicant " + UI.FormatAsLink("Clothing", "EQUIPMENT") + " items.\n\nDuplicants will not fabricate items unless recipes are queued.";
			}

			public class SOLIDBOOSTER
			{
				public static LocString NAME = UI.FormatAsLink("Solid Fuel Thruster", "SOLIDBOOSTER");

				public static LocString DESC = "Additional thrusters allow rockets to reach far away space destinations.";

				public static LocString EFFECT = string.Concat(new string[]
				{
					"Burns ",
					UI.FormatAsLink("Iron", "IRON"),
					" and ",
					UI.FormatAsLink("Oxylite", "OXYROCK"),
					" to increase rocket exploration distance."
				});
			}

			public class SPACEHEATER
			{
				public static LocString NAME = UI.FormatAsLink("Space Heater", "SPACEHEATER");

				public static LocString DESC = "Space heaters are a welcome cure for cold, soggy feet.";

				public static LocString EFFECT = string.Concat(new string[]
				{
					"Radiates a moderate amount of ",
					UI.FormatAsLink("Heat", "HEAT"),
					".\n\nRequires ",
					UI.FormatAsLink("Power", "POWER"),
					" in order to function."
				});
			}

			public class SPICEGRINDER
			{
				public static LocString NAME = UI.FormatAsLink("Spice Grinder", "SPICEGRINDER");

				public static LocString DESC = "Crushed seeds and other edibles make excellent meal-enhancing additives.";

				public static LocString EFFECT = "Produces ingredients that add benefits to " + UI.FormatAsLink("foods", "FOOD") + " prepared at skilled cooking stations.";

				public static LocString INGREDIENTHEADER = "Ingredients per 1000kcal:";
			}

			public class SMOKER
			{
				public static LocString NAME = UI.FormatAsLink("Smoker", "SMOKER");

				public static LocString DESC = "With a little patience, even tough meat can become deliciously edible.";

				public static LocString EFFECT = "Cooks improved " + UI.FormatAsLink("foods", "FOOD") + " over low, slow heat.\n\nDuplicants will not fabricate items unless recipes are queued.";
			}

			public class STORAGELOCKER
			{
				public static LocString NAME = UI.FormatAsLink("Storage Bin", "STORAGELOCKER");

				public static LocString DESC = "Resources left on the floor become \"debris\" and lower decor when not put away.";

				public static LocString EFFECT = "Stores the " + UI.FormatAsLink("Solid Materials", "ELEMENTS_SOLID") + " of your choosing.";

				public class FACADES
				{
					public class DEFAULT_STORAGELOCKER
					{
						public static LocString NAME = UI.FormatAsLink("Storage Bin", "STORAGELOCKER");

						public static LocString DESC = "Resources left on the floor become \"debris\" and lower decor when not put away.";
					}

					public class GREEN_MUSH
					{
						public static LocString NAME = UI.FormatAsLink("Mush Green Storage Bin", "STORAGELOCKER");

						public static LocString DESC = "Color-coded storage makes things easier to find.";
					}

					public class RED_ROSE
					{
						public static LocString NAME = UI.FormatAsLink("Puce Pink Storage Bin", "STORAGELOCKER");

						public static LocString DESC = "Color-coded storage makes things easier to find.";
					}

					public class BLUE_BABYTEARS
					{
						public static LocString NAME = UI.FormatAsLink("Weepy Blue Storage Bin", "STORAGELOCKER");

						public static LocString DESC = "Color-coded storage makes things easier to find.";
					}

					public class PURPLE_BRAINFAT
					{
						public static LocString NAME = UI.FormatAsLink("Faint Purple Storage Bin", "STORAGELOCKER");

						public static LocString DESC = "Color-coded storage makes things easier to find.";
					}

					public class YELLOW_TARTAR
					{
						public static LocString NAME = UI.FormatAsLink("Ick Yellow Storage Bin", "STORAGELOCKER");

						public static LocString DESC = "Color-coded storage makes things easier to find.";
					}

					public class POLKA_DARKNAVYNOOKGREEN
					{
						public static LocString NAME = UI.FormatAsLink("Party Dot Storage Bin", "STORAGELOCKER");

						public static LocString DESC = "A fun storage solution for fun-damental materials.";
					}

					public class POLKA_DARKPURPLERESIN
					{
						public static LocString NAME = UI.FormatAsLink("Mod Dot Storage Bin", "STORAGELOCKER");

						public static LocString DESC = "Groovy storage, because messy colonies are such a drag.";
					}

					public class STRIPES_RED_WHITE
					{
						public static LocString NAME = "Bold Stripe Storage Bin";

						public static LocString DESC = "It's the merriest storage bin of all.";
					}
				}
			}

			public class STORAGELOCKERSMART
			{
				public static LocString NAME = UI.FormatAsLink("Smart Storage Bin", "STORAGELOCKERSMART");

				public static LocString DESC = "Smart storage bins can automate resource organization based on type and mass.";

				public static LocString EFFECT = string.Concat(new string[]
				{
					"Stores the ",
					UI.FormatAsLink("Solid Materials", "ELEMENTS_SOLID"),
					" of your choosing.\n\nSends a ",
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					" when bin is full."
				});

				public static LocString LOGIC_PORT = "Full/Not Full";

				public static LocString LOGIC_PORT_ACTIVE = "Sends a " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + " when full";

				public static LocString LOGIC_PORT_INACTIVE = "Otherwise, sends a " + UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby);
			}

			public class OBJECTDISPENSER
			{
				public static LocString NAME = UI.FormatAsLink("Automatic Dispenser", "OBJECTDISPENSER");

				public static LocString DESC = "Automatic dispensers will store and drop resources in small quantities.";

				public static LocString EFFECT = string.Concat(new string[]
				{
					"Stores any ",
					UI.FormatAsLink("Solid Materials", "ELEMENTS_SOLID"),
					" delivered to it by Duplicants.\n\nDumps stored materials back into the world when it receives a ",
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					"."
				});

				public static LocString LOGIC_PORT = "Dump Trigger";

				public static LocString LOGIC_PORT_ACTIVE = UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + ": Dump all stored materials";

				public static LocString LOGIC_PORT_INACTIVE = UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby) + ": Store materials";
			}

			public class LIQUIDRESERVOIR
			{
				public static LocString NAME = UI.FormatAsLink("Liquid Reservoir", "LIQUIDRESERVOIR");

				public static LocString DESC = "Reservoirs cannot receive manually delivered resources.";

				public static LocString EFFECT = "Stores any " + UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID") + " resources piped into it.";
			}

			public class GASRESERVOIR
			{
				public static LocString NAME = UI.FormatAsLink("Gas Reservoir", "GASRESERVOIR");

				public static LocString DESC = "Reservoirs cannot receive manually delivered resources.";

				public static LocString EFFECT = "Stores any " + UI.FormatAsLink("Gas", "ELEMENTS_GAS") + " resources piped into it.";

				public class FACADES
				{
					public class DEFAULT_GASRESERVOIR
					{
						public static LocString NAME = UI.FormatAsLink("Gas Reservoir", "GASRESERVOIR");

						public static LocString DESC = "Reservoirs cannot receive manually delivered resources.";
					}

					public class LIGHTGOLD
					{
						public static LocString NAME = UI.FormatAsLink("Golden Gas Reservoir", "GASRESERVOIR");

						public static LocString DESC = "A colorful reservoir keeps gases neatly organized.";
					}

					public class PEAGREEN
					{
						public static LocString NAME = UI.FormatAsLink("Greenpea Gas Reservoir", "GASRESERVOIR");

						public static LocString DESC = "A colorful reservoir keeps gases neatly organized.";
					}

					public class LIGHTCOBALT
					{
						public static LocString NAME = UI.FormatAsLink("Bluemoon Gas Reservoir", "GASRESERVOIR");

						public static LocString DESC = "A colorful reservoir keeps gases neatly organized.";
					}

					public class POLKA_DARKPURPLERESIN
					{
						public static LocString NAME = UI.FormatAsLink("Mod Dot Gas Reservoir", "GASRESERVOIR");

						public static LocString DESC = "It sports the cheeriest of paint jobs. What a gas!";
					}

					public class POLKA_DARKNAVYNOOKGREEN
					{
						public static LocString NAME = UI.FormatAsLink("Party Dot Gas Reservoir", "GASRESERVOIR");

						public static LocString DESC = "Safe gas storage doesn't have to be dull.";
					}

					public class BLUE_BABYTEARS
					{
						public static LocString NAME = UI.FormatAsLink("Weepy Blue Gas Reservoir", "GASRESERVOIR");

						public static LocString DESC = "A colorful reservoir keeps gases neatly organized.";
					}

					public class YELLOW_TARTAR
					{
						public static LocString NAME = UI.FormatAsLink("Ick Yellow Gas Reservoir", "GASRESERVOIR");

						public static LocString DESC = "A colorful reservoir keeps gases neatly organized.";
					}

					public class GREEN_MUSH
					{
						public static LocString NAME = UI.FormatAsLink("Mush Green Gas Reservoir", "GASRESERVOIR");

						public static LocString DESC = "A colorful reservoir keeps gases neatly organized.";
					}

					public class RED_ROSE
					{
						public static LocString NAME = UI.FormatAsLink("Puce Pink Gas Reservoir", "GASRESERVOIR");

						public static LocString DESC = "A colorful reservoir keeps gases neatly organized.";
					}

					public class PURPLE_BRAINFAT
					{
						public static LocString NAME = UI.FormatAsLink("Faint Purple Gas Reservoir", "GASRESERVOIR");

						public static LocString DESC = "A colorful reservoir keeps gases neatly organized.";
					}
				}
			}

			public class SMARTRESERVOIR
			{
				public static LocString LOGIC_PORT = "Refill Parameters";

				public static LocString LOGIC_PORT_ACTIVE = "Sends a " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + " when reservoir is less than <b>Low Threshold</b> full, until <b>High Threshold</b> is reached again";

				public static LocString LOGIC_PORT_INACTIVE = "Sends a " + UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby) + " when reservoir is <b>High Threshold</b> full, until <b>Low Threshold</b> is reached again";

				public static LocString ACTIVATE_TOOLTIP = "Sends a " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + " when reservoir is less than <b>{0}%</b> full, until it is <b>{1}% (High Threshold)</b> full";

				public static LocString DEACTIVATE_TOOLTIP = "Sends a " + UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby) + " when reservoir is <b>{0}%</b> full, until it is less than <b>{1}% (Low Threshold)</b> full";

				public static LocString SIDESCREEN_TITLE = "Logic Activation Parameters";

				public static LocString SIDESCREEN_ACTIVATE = "Low Threshold:";

				public static LocString SIDESCREEN_DEACTIVATE = "High Threshold:";
			}

			public class LIQUIDHEATER
			{
				public static LocString NAME = UI.FormatAsLink("Liquid Tepidizer", "LIQUIDHEATER");

				public static LocString DESC = "Tepidizers heat liquid which can kill waterborne germs.";

				public static LocString EFFECT = "Warms large bodies of " + UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID") + ".\n\nMust be fully submerged.";
			}

			public class SWITCH
			{
				public static LocString NAME = UI.FormatAsLink("Switch", "SWITCH");

				public static LocString DESC = "Switches can only affect buildings that come after them on a circuit.";

				public static LocString EFFECT = "Turns " + UI.FormatAsLink("Power", "POWER") + " on or off.\n\nDoes not affect circuitry preceding the switch.";

				public static LocString SIDESCREEN_TITLE = "Switch";

				public static LocString TURN_ON = "Turn On";

				public static LocString TURN_ON_TOOLTIP = "Turn On {Hotkey}";

				public static LocString TURN_OFF = "Turn Off";

				public static LocString TURN_OFF_TOOLTIP = "Turn Off {Hotkey}";
			}

			public class LOGICPOWERRELAY
			{
				public static LocString NAME = UI.FormatAsLink("Power Shutoff", "LOGICPOWERRELAY");

				public static LocString DESC = "Automated systems save power and time by removing the need for Duplicant input.";

				public static LocString EFFECT = string.Concat(new string[]
				{
					"Connects to an ",
					UI.FormatAsLink("Automation", "LOGIC"),
					" grid to automatically turn ",
					UI.FormatAsLink("Power", "POWER"),
					" on or off.\n\nDoes not affect circuitry preceding the switch."
				});

				public static LocString LOGIC_PORT = "Kill Power";

				public static LocString LOGIC_PORT_ACTIVE = string.Concat(new string[]
				{
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					": Allow ",
					UI.PRE_KEYWORD,
					"Power",
					UI.PST_KEYWORD,
					" through connected circuits"
				});

				public static LocString LOGIC_PORT_INACTIVE = string.Concat(new string[]
				{
					UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby),
					": Prevent ",
					UI.PRE_KEYWORD,
					"Power",
					UI.PST_KEYWORD,
					" from flowing through connected circuits"
				});
			}

			public class LOGICINTERASTEROIDSENDER
			{
				public static LocString NAME = UI.FormatAsLink("Automation Broadcaster", "LOGICINTERASTEROIDSENDER");

				public static LocString DESC = "Sends automation signals into space.";

				public static LocString EFFECT = string.Concat(new string[]
				{
					"Sends a ",
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					" or a ",
					UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby),
					" to an ",
					UI.FormatAsLink("Automation Receiver", "LOGICINTERASTEROIDRECEIVER"),
					" over vast distances in space.\n\nBoth the Automation Broadcaster and the Automation Receiver must be exposed to space to function."
				});

				public static LocString DEFAULTNAME = "Unnamed Broadcaster";

				public static LocString LOGIC_PORT = "Broadcasting Signal";

				public static LocString LOGIC_PORT_ACTIVE = "Broadcasting: " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active);

				public static LocString LOGIC_PORT_INACTIVE = "Broadcasting: " + UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby);
			}

			public class LOGICINTERASTEROIDRECEIVER
			{
				public static LocString NAME = UI.FormatAsLink("Automation Receiver", "LOGICINTERASTEROIDRECEIVER");

				public static LocString DESC = "Receives automation signals from space.";

				public static LocString EFFECT = string.Concat(new string[]
				{
					"Receives a ",
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					" or a ",
					UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby),
					" from an ",
					UI.FormatAsLink("Automation Broadcaster", "LOGICINTERASTEROIDSENDER"),
					" over vast distances in space.\n\nBoth the Automation Receiver and the Automation Broadcaster must be exposed to space to function."
				});

				public static LocString LOGIC_PORT = "Receiving Signal";

				public static LocString LOGIC_PORT_ACTIVE = "Receiving: " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active);

				public static LocString LOGIC_PORT_INACTIVE = "Receiving: " + UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby);
			}

			public class TEMPERATURECONTROLLEDSWITCH
			{
				public static LocString NAME = UI.FormatAsLink("Thermo Switch", "TEMPERATURECONTROLLEDSWITCH");

				public static LocString DESC = "Automated switches can be used to manage circuits in areas where Duplicants cannot enter.";

				public static LocString EFFECT = string.Concat(new string[]
				{
					"Automatically turns ",
					UI.FormatAsLink("Power", "POWER"),
					" on or off using ambient ",
					UI.FormatAsLink("Temperature", "HEAT"),
					".\n\nDoes not affect circuitry preceding the switch."
				});
			}

			public class PRESSURESWITCHLIQUID
			{
				public static LocString NAME = UI.FormatAsLink("Hydro Switch", "PRESSURESWITCHLIQUID");

				public static LocString DESC = "A hydro switch shuts off power when the liquid pressure surrounding it surpasses the set threshold.";

				public static LocString EFFECT = string.Concat(new string[]
				{
					"Automatically turns ",
					UI.FormatAsLink("Power", "POWER"),
					" on or off using ambient ",
					UI.FormatAsLink("Liquid Pressure", "PRESSURE"),
					".\n\nDoes not affect circuitry preceding the switch.\n\nMust be submerged in ",
					UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID"),
					"."
				});
			}

			public class PRESSURESWITCHGAS
			{
				public static LocString NAME = UI.FormatAsLink("Atmo Switch", "PRESSURESWITCHGAS");

				public static LocString DESC = "An atmo switch shuts off power when the air pressure surrounding it surpasses the set threshold.";

				public static LocString EFFECT = string.Concat(new string[]
				{
					"Automatically turns ",
					UI.FormatAsLink("Power", "POWER"),
					" on or off using ambient ",
					UI.FormatAsLink("Gas Pressure", "PRESSURE"),
					" .\n\nDoes not affect circuitry preceding the switch."
				});
			}

			public class TILE
			{
				public static LocString NAME = UI.FormatAsLink("Tile", "TILE");

				public static LocString DESC = "Tile can be used to bridge gaps and get to unreachable areas.";

				public static LocString EFFECT = "Used to build the walls and floors of rooms.\n\nIncreases Duplicant runspeed.";
			}

			public class WALLTOILET
			{
				public static LocString NAME = UI.FormatAsLink("Wall Toilet", "WALLTOILET");

				public static LocString DESC = "Wall Toilets transmit fewer germs to Duplicants and require no emptying.";

				public static LocString EFFECT = "Gives Duplicants a place to relieve themselves. Empties directly on the other side of the wall.\n\nSpreads very few " + UI.FormatAsLink("Germs", "DISEASE") + ".";
			}

			public class WATERPURIFIER
			{
				public static LocString NAME = UI.FormatAsLink("Water Sieve", "WATERPURIFIER");

				public static LocString DESC = "Sieves cannot kill germs and pass any they receive into their waste and water output.";

				public static LocString EFFECT = string.Concat(new string[]
				{
					"Produces clean ",
					UI.FormatAsLink("Water", "WATER"),
					" from ",
					UI.FormatAsLink("Polluted Water", "DIRTYWATER"),
					" using ",
					UI.FormatAsLink("Sand", "SAND"),
					".\n\nProduces ",
					UI.FormatAsLink("Polluted Dirt", "TOXICSAND"),
					"."
				});
			}

			public class DISTILLATIONCOLUMN
			{
				public static LocString NAME = UI.FormatAsLink("Distillation Column", "DISTILLATIONCOLUMN");

				public static LocString DESC = "Gets hot and steamy.";

				public static LocString EFFECT = string.Concat(new string[]
				{
					"Separates any ",
					UI.FormatAsLink("Contaminated Water", "DIRTYWATER"),
					" piped through it into ",
					UI.FormatAsLink("Steam", "STEAM"),
					" and ",
					UI.FormatAsLink("Polluted Dirt", "TOXICSAND"),
					"."
				});
			}

			public class WIRE
			{
				public static LocString NAME = UI.FormatAsLink("Wire", "WIRE");

				public static LocString DESC = "Electrical wire is used to connect generators, batteries, and buildings.";

				public static LocString EFFECT = "Connects buildings to " + UI.FormatAsLink("Power", "POWER") + " sources.\n\nCan be run through wall and floor tile.";
			}

			public class WIREBRIDGE
			{
				public static LocString NAME = UI.FormatAsLink("Wire Bridge", "WIREBRIDGE");

				public static LocString DESC = "Splitting generators onto separate grids can prevent overloads and wasted electricity.";

				public static LocString EFFECT = "Runs one wire section over another without joining them.\n\nCan be run through wall and floor tile.";
			}

			public class HIGHWATTAGEWIRE
			{
				public static LocString NAME = UI.FormatAsLink("Heavi-Watt Wire", "HIGHWATTAGEWIRE");

				public static LocString DESC = "Higher wattage wire is used to avoid power overloads, particularly for strong generators.";

				public static LocString EFFECT = string.Concat(new string[]
				{
					"Carries more ",
					UI.FormatAsLink("Wattage", "POWER"),
					" than regular ",
					UI.FormatAsLink("Wire", "WIRE"),
					" without overloading.\n\nCannot be run through wall and floor tile."
				});
			}

			public class WIREBRIDGEHIGHWATTAGE
			{
				public static LocString NAME = UI.FormatAsLink("Heavi-Watt Joint Plate", "WIREBRIDGEHIGHWATTAGE");

				public static LocString DESC = "Joint plates can run Heavi-Watt wires through walls without leaking gas or liquid.";

				public static LocString EFFECT = "Allows " + UI.FormatAsLink("Heavi-Watt Wire", "HIGHWATTAGEWIRE") + " to be run through wall and floor tile.\n\nFunctions as regular tile.";
			}

			public class WIREREFINED
			{
				public static LocString NAME = UI.FormatAsLink("Conductive Wire", "WIREREFINED");

				public static LocString DESC = "My Duplicants prefer the look of conductive wire to the regular raggedy stuff.";

				public static LocString EFFECT = "Connects buildings to " + UI.FormatAsLink("Power", "POWER") + " sources.\n\nCan be run through wall and floor tile.";
			}

			public class WIREREFINEDBRIDGE
			{
				public static LocString NAME = UI.FormatAsLink("Conductive Wire Bridge", "WIREREFINEDBRIDGE");

				public static LocString DESC = "Splitting generators onto separate systems can prevent overloads and wasted electricity.";

				public static LocString EFFECT = string.Concat(new string[]
				{
					"Carries more ",
					UI.FormatAsLink("Wattage", "POWER"),
					" than a regular ",
					UI.FormatAsLink("Wire Bridge", "WIREBRIDGE"),
					" without overloading.\n\nRuns one wire section over another without joining them.\n\nCan be run through wall and floor tile."
				});
			}

			public class WIREREFINEDHIGHWATTAGE
			{
				public static LocString NAME = UI.FormatAsLink("Heavi-Watt Conductive Wire", "WIREREFINEDHIGHWATTAGE");

				public static LocString DESC = "Higher wattage wire is used to avoid power overloads, particularly for strong generators.";

				public static LocString EFFECT = string.Concat(new string[]
				{
					"Carries more ",
					UI.FormatAsLink("Wattage", "POWER"),
					" than regular ",
					UI.FormatAsLink("Wire", "WIRE"),
					" without overloading.\n\nCannot be run through wall and floor tile."
				});
			}

			public class WIREREFINEDBRIDGEHIGHWATTAGE
			{
				public static LocString NAME = UI.FormatAsLink("Heavi-Watt Conductive Joint Plate", "WIREREFINEDBRIDGEHIGHWATTAGE");

				public static LocString DESC = "Joint plates can run Heavi-Watt wires through walls without leaking gas or liquid.";

				public static LocString EFFECT = string.Concat(new string[]
				{
					"Carries more ",
					UI.FormatAsLink("Wattage", "POWER"),
					" than a regular ",
					UI.FormatAsLink("Heavi-Watt Joint Plate", "WIREBRIDGEHIGHWATTAGE"),
					" without overloading.\n\nAllows ",
					UI.FormatAsLink("Heavi-Watt Wire", "HIGHWATTAGEWIRE"),
					" to be run through wall and floor tile."
				});
			}

			public class HANDSANITIZER
			{
				public static LocString NAME = UI.FormatAsLink("Hand Sanitizer", "HANDSANITIZER");

				public static LocString DESC = "Hand sanitizers kill germs more effectively than wash basins.";

				public static LocString EFFECT = "Removes most " + UI.FormatAsLink("Germs", "DISEASE") + " from Duplicants.\n\nGerm-covered Duplicants use Hand Sanitizers when passing by in the selected direction.";
			}

			public class WASHBASIN
			{
				public static LocString NAME = UI.FormatAsLink("Wash Basin", "WASHBASIN");

				public static LocString DESC = "Germ spread can be reduced by building these where Duplicants often get dirty.";

				public static LocString EFFECT = "Removes some " + UI.FormatAsLink("Germs", "DISEASE") + " from Duplicants.\n\nGerm-covered Duplicants use Wash Basins when passing by in the selected direction.";
			}

			public class WASHSINK
			{
				public static LocString NAME = UI.FormatAsLink("Sink", "WASHSINK");

				public static LocString DESC = "Sinks are plumbed and do not need to be manually emptied or refilled.";

				public static LocString EFFECT = "Removes " + UI.FormatAsLink("Germs", "DISEASE") + " from Duplicants.\n\nGerm-covered Duplicants use Sinks when passing by in the selected direction.";

				public class FACADES
				{
					public class DEFAULT_WASHSINK
					{
						public static LocString NAME = UI.FormatAsLink("Sink", "WASHSINK");

						public static LocString DESC = "Sinks are plumbed and do not need to be manually emptied or refilled.";
					}

					public class PURPLE_BRAINFAT
					{
						public static LocString NAME = UI.FormatAsLink("Faint Purple Sink", "WASHSINK");

						public static LocString DESC = "A refreshing splash of color for the light-headed.";
					}

					public class BLUE_BABYTEARS
					{
						public static LocString NAME = UI.FormatAsLink("Weepy Blue Sink", "WASHSINK");

						public static LocString DESC = "A calm, colorful sink for heavy-hearted Duplicants.";
					}

					public class GREEN_MUSH
					{
						public static LocString NAME = UI.FormatAsLink("Mush Green Sink", "WASHSINK");

						public static LocString DESC = "Even the soap is mush-colored.";
					}

					public class YELLOW_TARTAR
					{
						public static LocString NAME = UI.FormatAsLink("Ick Yellow Sink", "WASHSINK");

						public static LocString DESC = "The juxtaposition of 'ick' and 'clean' can be very satisfying.";
					}

					public class RED_ROSE
					{
						public static LocString NAME = UI.FormatAsLink("Puce Pink Sink", "WASHSINK");

						public static LocString DESC = "Some Duplicants say it looks like a germ-devouring mouth.";
					}
				}
			}

			public class DECONTAMINATIONSHOWER
			{
				public static LocString NAME = UI.FormatAsLink("Decontamination Shower", "DECONTAMINATIONSHOWER");

				public static LocString DESC = "Don't forget to decontaminate behind your ears.";

				public static LocString EFFECT = string.Concat(new string[]
				{
					"Uses ",
					UI.FormatAsLink("Water", "WATER"),
					" to remove ",
					UI.FormatAsLink("Germs", "DISEASE"),
					" and ",
					UI.FormatAsLink("Radiation", "RADIATION"),
					".\n\nDecontaminates both Duplicants and their ",
					UI.FormatAsLink("Clothing", "EQUIPMENT"),
					"."
				});
			}

			public class TILEPOI
			{
				public static LocString NAME = UI.FormatAsLink("Tile", "TILEPOI");

				public static LocString DESC = "";

				public static LocString EFFECT = "Used to build the walls and floor of rooms.";
			}

			public class POLYMERIZER
			{
				public static LocString NAME = UI.FormatAsLink("Polymer Press", "POLYMERIZER");

				public static LocString DESC = "Plastic can be used to craft unique buildings and goods.";

				public static LocString EFFECT = string.Concat(new string[]
				{
					"Converts ",
					UI.FormatAsLink("Plastic Monomers", "PLASTIFIABLELIQUID"),
					" into raw ",
					UI.FormatAsLink("Plastic", "POLYPROPYLENE"),
					"."
				});
			}

			public class DIRECTIONALWORLDPUMPLIQUID
			{
				public static LocString NAME = UI.FormatAsLink("Liquid Channel", "DIRECTIONALWORLDPUMPLIQUID");

				public static LocString DESC = "Channels move more volume than pumps and require no power, but need sufficient pressure to function.";

				public static LocString EFFECT = "Directionally moves large volumes of " + UI.FormatAsLink("LIQUID", "ELEMENTS_LIQUID") + " through a channel.\n\nCan be used as floor tile and rotated before construction.";
			}

			public class STEAMTURBINE
			{
				public static LocString NAME = UI.FormatAsLink("[DEPRECATED] Steam Turbine", "STEAMTURBINE");

				public static LocString DESC = "Useful for converting the geothermal energy of magma into usable power.";

				public static LocString EFFECT = string.Concat(new string[]
				{
					"THIS BUILDING HAS BEEN DEPRECATED AND CANNOT BE BUILT.\n\nGenerates exceptional electrical ",
					UI.FormatAsLink("Power", "POWER"),
					" using pressurized, ",
					UI.FormatAsLink("Scalding", "HEAT"),
					" ",
					UI.FormatAsLink("Steam", "STEAM"),
					".\n\nOutputs significantly cooler ",
					UI.FormatAsLink("Steam", "STEAM"),
					" than it receives.\n\nAir pressure beneath this building must be higher than pressure above for air to flow."
				});
			}

			public class STEAMTURBINE2
			{
				public static LocString NAME = UI.FormatAsLink("Steam Turbine", "STEAMTURBINE2");

				public static LocString DESC = "Useful for converting the geothermal energy into usable power.";

				public static LocString EFFECT = string.Concat(new string[]
				{
					"Draws in ",
					UI.FormatAsLink("Steam", "STEAM"),
					" from the tiles directly below the machine's foundation and uses it to generate electrical ",
					UI.FormatAsLink("Power", "POWER"),
					".\n\nOutputs ",
					UI.FormatAsLink("Water", "WATER"),
					"."
				});

				public static LocString HEAT_SOURCE = "Power Generation Waste";
			}

			public class STEAMENGINE
			{
				public static LocString NAME = UI.FormatAsLink("Steam Engine", "STEAMENGINE");

				public static LocString DESC = "Rockets can be used to send Duplicants into space and retrieve rare resources.";

				public static LocString EFFECT = "Utilizes " + UI.FormatAsLink("Steam", "STEAM") + " to propel rockets for space exploration.\n\nThe engine of a rocket must be built first before more rocket modules may be added.";
			}

			public class STEAMENGINECLUSTER
			{
				public static LocString NAME = UI.FormatAsLink("Steam Engine", "STEAMENGINECLUSTER");

				public static LocString DESC = "Rockets can be used to send Duplicants into space and retrieve rare resources.";

				public static LocString EFFECT = string.Concat(new string[]
				{
					"Utilizes ",
					UI.FormatAsLink("Steam", "STEAM"),
					" to propel rockets for space exploration.\n\nEngine must be built via ",
					BUILDINGS.PREFABS.LAUNCHPAD.NAME,
					". \n\nOnce the engine has been built, more rocket modules can be added."
				});
			}

			public class SOLARPANEL
			{
				public static LocString NAME = UI.FormatAsLink("Solar Panel", "SOLARPANEL");

				public static LocString DESC = "Solar panels convert high intensity sunlight into power and produce zero waste.";

				public static LocString EFFECT = string.Concat(new string[]
				{
					"Converts ",
					UI.FormatAsLink("Sunlight", "LIGHT"),
					" into electrical ",
					UI.FormatAsLink("Power", "POWER"),
					".\n\nMust be exposed to space."
				});
			}

			public class COMETDETECTOR
			{
				public static LocString NAME = UI.FormatAsLink("Space Scanner", "COMETDETECTOR");

				public static LocString DESC = "Networks of many scanners will scan more efficiently than one on its own.";

				public static LocString EFFECT = "Sends a " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + " to its automation circuit when it detects incoming objects from space.\n\nCan be configured to detect incoming meteor showers or returning space rockets.";
			}

			public class OILREFINERY
			{
				public static LocString NAME = UI.FormatAsLink("Oil Refinery", "OILREFINERY");

				public static LocString DESC = "Petroleum can only be produced from the refinement of crude oil.";

				public static LocString EFFECT = string.Concat(new string[]
				{
					"Converts ",
					UI.FormatAsLink("Crude Oil", "CRUDEOIL"),
					" into ",
					UI.FormatAsLink("Petroleum", "PETROLEUM"),
					" and ",
					UI.FormatAsLink("Natural Gas", "METHANE"),
					"."
				});
			}

			public class OILWELLCAP
			{
				public static LocString NAME = UI.FormatAsLink("Oil Well", "OILWELLCAP");

				public static LocString DESC = "Water pumped into an oil reservoir cannot be recovered.";

				public static LocString EFFECT = string.Concat(new string[]
				{
					"Extracts ",
					UI.FormatAsLink("Crude Oil", "CRUDEOIL"),
					" using clean ",
					UI.FormatAsLink("Water", "WATER"),
					".\n\nMust be built atop an ",
					UI.FormatAsLink("Oil Reservoir", "OIL_WELL"),
					"."
				});
			}

			public class METALREFINERY
			{
				public static LocString NAME = UI.FormatAsLink("Metal Refinery", "METALREFINERY");

				public static LocString DESC = "Refined metals are necessary to build advanced electronics and technologies.";

				public static LocString EFFECT = string.Concat(new string[]
				{
					"Produces ",
					UI.FormatAsLink("Refined Metals", "REFINEDMETAL"),
					" from raw ",
					UI.FormatAsLink("Metal Ore", "RAWMETAL"),
					".\n\nSignificantly ",
					UI.FormatAsLink("Heats", "HEAT"),
					" and outputs the ",
					UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID"),
					" piped into it.\n\nDuplicants will not fabricate items unless recipes are queued."
				});

				public static LocString RECIPE_DESCRIPTION = "Extracts pure {0} from {1}.";
			}

			public class MISSILEFABRICATOR
			{
				public static LocString NAME = UI.FormatAsLink("Blastshot Maker", "MISSILEFABRICATOR");

				public static LocString DESC = "Blastshot shells are an effective defense against incoming meteor showers.";

				public static LocString EFFECT = string.Concat(new string[]
				{
					"Produces ",
					UI.FormatAsLink("Blastshot", "MISSILELAUNCHER"),
					" from ",
					UI.FormatAsLink("Refined Metals", "REFINEDMETAL"),
					" combined with ",
					UI.FormatAsLink("Petroleum", "PETROLEUM"),
					".\n\nDuplicants will not fabricate items unless recipes are queued."
				});

				public static LocString RECIPE_DESCRIPTION = "Produces {0} from {1} and {2}.";

				public static LocString RECIPE_DESCRIPTION_LONGRANGE = "Produces {0} from {1}, {2}, and {3}.";
			}

			public class GLASSFORGE
			{
				public static LocString NAME = UI.FormatAsLink("Glass Forge", "GLASSFORGE");

				public static LocString DESC = "Glass can be used to construct window tile.";

				public static LocString EFFECT = string.Concat(new string[]
				{
					"Produces ",
					UI.FormatAsLink("Molten Glass", "MOLTENGLASS"),
					" from raw ",
					UI.FormatAsLink("Sand", "SAND"),
					".\n\nOutputs ",
					UI.FormatAsLink("High Temperature", "HEAT"),
					" ",
					UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID"),
					".\n\nDuplicants will not fabricate items unless recipes are queued."
				});

				public static LocString RECIPE_DESCRIPTION = "Extracts pure {0} from {1}.";
			}

			public class ROCKCRUSHER
			{
				public static LocString NAME = UI.FormatAsLink("Rock Crusher", "ROCKCRUSHER");

				public static LocString DESC = "Rock Crushers loosen nuggets from raw ore and can process many different resources.";

				public static LocString EFFECT = "Inefficiently produces refined materials from raw resources.\n\nDuplicants will not fabricate items unless recipes are queued.";

				public static LocString RECIPE_DESCRIPTION = "Crushes {0} into {1}";

				public static LocString RECIPE_DESCRIPTION_TWO_OUTPUT = "Crushes {0} into {1} and {2}";

				public static LocString METAL_RECIPE_DESCRIPTION = "Crushes {1} into " + UI.FormatAsLink("Sand", "SAND") + " and pure {0}";

				public static LocString LIME_RECIPE_DESCRIPTION = "Crushes {1} into {0}";

				public static LocString LIME_FROM_LIMESTONE_RECIPE_DESCRIPTION = "Crushes {0} into {1} and a small amount of pure {2}";

				public static LocString RESIN_FROM_AMBER_RECIPE_DESCRIPTION = "Crushes {0} into {1} and {2}, and a small amount of {3}";

				public static LocString SAND_FROM_RAW_MINERAL_NAME = UI.FormatAsLink("Raw Mineral", "BUILDABLERAW") + " to " + UI.FormatAsLink("Sand", "SAND");

				public static LocString SAND_FROM_RAW_MINERAL_DESCRIPTION = "Crushes " + UI.FormatAsLink("Raw Minerals", "BUILDABLERAW") + " into " + UI.FormatAsLink("Sand", "SAND");

				public class FACADES
				{
					public class DEFAULT_ROCKCRUSHER
					{
						public static LocString NAME = UI.FormatAsLink("Rock Crusher", "ROCKCRUSHER");

						public static LocString DESC = "Rock Crushers loosen nuggets from raw ore and can process many different resources.";
					}

					public class HANDS
					{
						public static LocString NAME = UI.FormatAsLink("Punchy Rock Crusher", "ROCKCRUSHER");

						public static LocString DESC = "Smashy smashy!";
					}

					public class TEETH
					{
						public static LocString NAME = UI.FormatAsLink("Toothy Rock Crusher", "ROCKCRUSHER");

						public static LocString DESC = "Not designed to handle overcooked food waste.";
					}

					public class ROUNDSTAMP
					{
						public static LocString NAME = UI.FormatAsLink("Smooth Rock Crusher", "ROCKCRUSHER");

						public static LocString DESC = "Inspired by the traditional mortar and pestle.";
					}

					public class SPIKEBEDS
					{
						public static LocString NAME = UI.FormatAsLink("Spiked Rock Crusher", "ROCKCRUSHER");

						public static LocString DESC = "Mashes rocks into oblivion.";
					}

					public class CHOMP
					{
						public static LocString NAME = UI.FormatAsLink("Mani Rock Crusher", "ROCKCRUSHER");

						public static LocString DESC = "Buffs rough ore into smooth little nuggets.";
					}

					public class GEARS
					{
						public static LocString NAME = UI.FormatAsLink("Super-Mech Rock Crusher", "ROCKCRUSHER");

						public static LocString DESC = "Uncrushed ore really grinds its gears.";
					}

					public class BALLOON
					{
						public static LocString NAME = UI.FormatAsLink("Pop-A-Rocks-E", "ROCKCRUSHER");

						public static LocString DESC = "Wherever there's raw ore, there's a rock crusher lurking nearby.";
					}
				}
			}

			public class SLUDGEPRESS
			{
				public static LocString NAME = UI.FormatAsLink("Sludge Press", "SLUDGEPRESS");

				public static LocString DESC = "What Duplicant doesn't love playing with mud?";

				public static LocString EFFECT = "Separates " + UI.FormatAsLink("Mud", "MUD") + " and other sludges into their base elements.\n\nDuplicants will not fabricate items unless recipes are queued.";

				public static LocString RECIPE_DESCRIPTION = "Separates {0} into its base elements.";
			}

			public class CHEMICALREFINERY
			{
				public static LocString NAME = UI.FormatAsLink("Emulsifier", "CHEMICALREFINERY");

				public static LocString DESC = "It's like a blender, but better.";

				public static LocString EFFECT = "Combines " + UI.FormatAsLink("Liquids", "ELEMENTS_LIQUID") + " and other inputs into fluid compounds.\n\nDuplicants will not fabricate emulsions unless recipes are queued.";

				public static LocString REFINEDLIPID_RECIPE_DESCRIPTION = string.Concat(new string[]
				{
					"Biodiesel is a ",
					UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID"),
					" used in ",
					UI.FormatAsLink("Power", "POWER"),
					" production."
				});

				public static LocString SALTWATER_RECIPE_DESCRIPTION = "Salt Water is a " + UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID") + " with insulating and radiation-absorbing properties.";
			}

			public class SUPERMATERIALREFINERY
			{
				public static LocString NAME = UI.FormatAsLink("Molecular Forge", "SUPERMATERIALREFINERY");

				public static LocString DESC = "Rare materials can be procured through rocket missions into space.";

				public static LocString EFFECT = "Processes " + UI.FormatAsLink("Rare Materials", "RAREMATERIALS") + " into advanced industrial goods.\n\nRare materials can be retrieved from space missions.\n\nDuplicants will not fabricate items unless recipes are queued.";

				public static LocString SUPERCOOLANT_RECIPE_DESCRIPTION = "Super Coolant is an industrial-grade " + UI.FormatAsLink("Fullerene", "FULLERENE") + " coolant.";

				public static LocString SUPERINSULATOR_RECIPE_DESCRIPTION = string.Concat(new string[]
				{
					"Insulite reduces ",
					UI.FormatAsLink("Heat Transfer", "HEAT"),
					" and is composed of recrystallized ",
					UI.FormatAsLink("Abyssalite", "KATAIRITE"),
					"."
				});

				public static LocString TEMPCONDUCTORSOLID_RECIPE_DESCRIPTION = "Thermium is an industrial metal alloy formulated to maximize " + UI.FormatAsLink("Heat Transfer", "HEAT") + " and thermal dispersion.";

				public static LocString VISCOGEL_RECIPE_DESCRIPTION = "Visco-Gel Fluid is a " + UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID") + " polymer with high surface tension.";

				public static LocString YELLOWCAKE_RECIPE_DESCRIPTION = "Yellowcake is a " + UI.FormatAsLink("Solid Material", "ELEMENTS_SOLID") + " used in uranium enrichment.";

				public static LocString FULLERENE_RECIPE_DESCRIPTION = string.Concat(new string[]
				{
					"Fullerene is a ",
					UI.FormatAsLink("Solid Material", "ELEMENTS_SOLID"),
					" used in the production of ",
					UI.FormatAsLink("Super Coolant", "SUPERCOOLANT"),
					"."
				});

				public static LocString HARDPLASTIC_RECIPE_DESCRIPTION = "Plastium is a highly heat-resistant, plastic-like " + UI.FormatAsLink("Solid Material", "ELEMENTS_SOLID") + " used for space buildings.";

				public static LocString SELF_CHARGING_POWERBANK_RECIPE_DESCRIPTION = "Atomic Power Banks are portable, self-charging units used for isolated " + UI.FormatAsLink("Power", "POWER") + " grids.";
			}

			public class THERMALBLOCK
			{
				public static LocString NAME = UI.FormatAsLink("Tempshift Plate", "THERMALBLOCK");

				public static LocString DESC = "The thermal properties of construction materials determine their heat retention.";

				public static LocString EFFECT = "Accelerates or buffers " + UI.FormatAsLink("Heat", "HEAT") + " dispersal based on the construction material used.\n\nHas a small area of effect.";
			}

			public class POWERCONTROLSTATION
			{
				public static LocString NAME = UI.FormatAsLink("Power Control Station", "POWERCONTROLSTATION");

				public static LocString DESC = "Only one Duplicant may be assigned to a station at a time.";

				public static LocString EFFECT = string.Concat(new string[]
				{
					"Produces ",
					ITEMS.INDUSTRIAL_PRODUCTS.POWER_STATION_TOOLS.NAME,
					" to increase the ",
					UI.FormatAsLink("Power", "POWER"),
					" output of generators.\n\nAssigned Duplicants must possess the ",
					UI.FormatAsLink("Tune Up", "TECHNICALS2"),
					" trait."
				});
			}

			public class FARMSTATION
			{
				public static LocString NAME = UI.FormatAsLink("Farm Station", "FARMSTATION");

				public static LocString DESC = "This station only has an effect on crops grown within the same room.";

				public static LocString EFFECT = string.Concat(new string[]
				{
					"Produces ",
					UI.FormatAsLink("Micronutrient Fertilizer", "FARM_STATION_TOOLS"),
					" to increase ",
					UI.FormatAsLink("Plant", "PLANTS"),
					" growth rates.\n\nAssigned Duplicants must possess the ",
					UI.FormatAsLink("Crop Tending", "FARMING2"),
					" trait.\n\nThis building is a necessary component of the Greenhouse room."
				});
			}

			public class FISHDELIVERYPOINT
			{
				public static LocString NAME = UI.FormatAsLink("Fish Release", "FISHDELIVERYPOINT");

				public static LocString DESC = "A fish release must be built in liquid to prevent released fish from suffocating.";

				public static LocString EFFECT = "Releases trapped " + UI.FormatAsLink("Pacu", "PACU") + " back into the world.\n\nCan be used multiple times.";
			}

			public class FISHFEEDER
			{
				public static LocString NAME = UI.FormatAsLink("Fish Feeder", "FISHFEEDER");

				public static LocString DESC = "Build this feeder above a body of water to feed the fish within.";

				public static LocString EFFECT = "Automatically dispenses stored " + UI.FormatAsLink("Critter", "CREATURES") + " food into the area below.\n\nDispenses continuously as food is consumed.";
			}

			public class FISHTRAP
			{
				public static LocString NAME = UI.FormatAsLink("Fish Trap", "FISHTRAP");

				public static LocString DESC = "Trapped fish will automatically be bagged for transport.";

				public static LocString EFFECT = "Attracts and traps swimming " + UI.FormatAsLink("Pacu", "PACU") + ".\n\nSingle use.";
			}

			public class RANCHSTATION
			{
				public static LocString NAME = UI.FormatAsLink("Grooming Station", "RANCHSTATION");

				public static LocString DESC = "A groomed critter is a happy, healthy, productive critter.";

				public static LocString EFFECT = string.Concat(new string[]
				{
					"Allows the assigned ",
					UI.FormatAsLink("Rancher", "RANCHER"),
					" to care for ",
					UI.FormatAsLink("Critters", "CREATURES"),
					".\n\nAssigned Duplicants must possess the ",
					UI.FormatAsLink("Critter Ranching", "RANCHING1"),
					" skill."
				});
			}

			public class MACHINESHOP
			{
				public static LocString NAME = UI.FormatAsLink("Mechanics Station", "MACHINESHOP");

				public static LocString DESC = "Duplicants will only improve the efficiency of buildings in the same room as this station.";

				public static LocString EFFECT = "Allows the assigned " + UI.FormatAsLink("Engineer", "MACHINE_TECHNICIAN") + " to improve building production efficiency.\n\nThis building is a necessary component of the Machine Shop room.";
			}

			public class LOGICWIRE
			{
				public static LocString NAME = UI.FormatAsLink("Automation Wire", "LOGICWIRE");

				public static LocString DESC = "Automation wire is used to connect building ports to automation gates.";

				public static LocString EFFECT = "Connects buildings to " + UI.FormatAsLink("Sensors", "LOGIC") + ".\n\nCan be run through wall and floor tile.";
			}

			public class LOGICRIBBON
			{
				public static LocString NAME = UI.FormatAsLink("Automation Ribbon", "LOGICRIBBON");

				public static LocString DESC = "Logic ribbons use significantly less space to carry multiple automation signals.";

				public static LocString EFFECT = string.Concat(new string[]
				{
					"A 4-Bit ",
					BUILDINGS.PREFABS.LOGICWIRE.NAME,
					" which can carry up to four automation signals.\n\nUse a ",
					UI.FormatAsLink("Ribbon Writer", "LOGICRIBBONWRITER"),
					" to output to multiple Bits, and a ",
					UI.FormatAsLink("Ribbon Reader", "LOGICRIBBONREADER"),
					" to input from multiple Bits."
				});
			}

			public class LOGICWIREBRIDGE
			{
				public static LocString NAME = UI.FormatAsLink("Automation Wire Bridge", "LOGICWIREBRIDGE");

				public static LocString DESC = "Wire bridges allow multiple automation grids to exist in a small area without connecting.";

				public static LocString EFFECT = "Runs one " + UI.FormatAsLink("Automation Wire", "LOGICWIRE") + " section over another without joining them.\n\nCan be run through wall and floor tile.";

				public static LocString LOGIC_PORT = "Transmit Signal";

				public static LocString LOGIC_PORT_ACTIVE = UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + ": Pass through the " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active);

				public static LocString LOGIC_PORT_INACTIVE = UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby) + ": Pass through the " + UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby);
			}

			public class LOGICRIBBONBRIDGE
			{
				public static LocString NAME = UI.FormatAsLink("Automation Ribbon Bridge", "LOGICRIBBONBRIDGE");

				public static LocString DESC = "Wire bridges allow multiple automation grids to exist in a small area without connecting.";

				public static LocString EFFECT = "Runs one " + UI.FormatAsLink("Automation Ribbon", "LOGICRIBBON") + " section over another without joining them.\n\nCan be run through wall and floor tile.";

				public static LocString LOGIC_PORT = "Transmit Signal";

				public static LocString LOGIC_PORT_ACTIVE = UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + ": Pass through the " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active);

				public static LocString LOGIC_PORT_INACTIVE = UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby) + ": Pass through the " + UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby);
			}

			public class LOGICGATEAND
			{
				public static LocString NAME = UI.FormatAsLink("AND Gate", "LOGICGATEAND");

				public static LocString DESC = "This gate outputs a Green Signal when both its inputs are receiving Green Signals at the same time.";

				public static LocString EFFECT = string.Concat(new string[]
				{
					"Outputs a ",
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					" when both Input A <b>AND</b> Input B are receiving ",
					UI.FormatAsAutomationState("Green", UI.AutomationState.Active),
					".\n\nOutputs a ",
					UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby),
					" when even one Input is receiving ",
					UI.FormatAsAutomationState("Red", UI.AutomationState.Standby),
					"."
				});

				public static LocString OUTPUT_NAME = "OUTPUT";

				public static LocString OUTPUT_ACTIVE = "Sends a " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + " if both Inputs are receiving " + UI.FormatAsAutomationState("Green", UI.AutomationState.Active);

				public static LocString OUTPUT_INACTIVE = "Sends a " + UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby) + " if any Input is receiving " + UI.FormatAsAutomationState("Red", UI.AutomationState.Standby);
			}

			public class LOGICGATEOR
			{
				public static LocString NAME = UI.FormatAsLink("OR Gate", "LOGICGATEOR");

				public static LocString DESC = "This gate outputs a Green Signal if receiving one or more Green Signals.";

				public static LocString EFFECT = string.Concat(new string[]
				{
					"Outputs a ",
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					" if at least one of Input A <b>OR</b> Input B is receiving ",
					UI.FormatAsAutomationState("Green", UI.AutomationState.Active),
					".\n\nOutputs a ",
					UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby),
					" when neither Input A or Input B are receiving ",
					UI.FormatAsAutomationState("Green", UI.AutomationState.Active),
					"."
				});

				public static LocString OUTPUT_NAME = "OUTPUT";

				public static LocString OUTPUT_ACTIVE = "Sends a " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + " if any Input is receiving " + UI.FormatAsAutomationState("Green", UI.AutomationState.Active);

				public static LocString OUTPUT_INACTIVE = "Sends a " + UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby) + " if both Inputs are receiving " + UI.FormatAsAutomationState("Red", UI.AutomationState.Standby);
			}

			public class LOGICGATENOT
			{
				public static LocString NAME = UI.FormatAsLink("NOT Gate", "LOGICGATENOT");

				public static LocString DESC = "This gate reverses automation signals, turning a Green Signal into a Red Signal and vice versa.";

				public static LocString EFFECT = string.Concat(new string[]
				{
					"Outputs a ",
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					" if the Input is receiving a ",
					UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby),
					".\n\nOutputs a ",
					UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby),
					" when its Input is receiving a ",
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					"."
				});

				public static LocString OUTPUT_NAME = "OUTPUT";

				public static LocString OUTPUT_ACTIVE = "Sends a " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + " if receiving " + UI.FormatAsAutomationState("Red", UI.AutomationState.Standby);

				public static LocString OUTPUT_INACTIVE = "Sends a " + UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby) + " if receiving " + UI.FormatAsAutomationState("Green", UI.AutomationState.Active);
			}

			public class LOGICGATEXOR
			{
				public static LocString NAME = UI.FormatAsLink("XOR Gate", "LOGICGATEXOR");

				public static LocString DESC = "This gate outputs a Green Signal if exactly one of its Inputs is receiving a Green Signal.";

				public static LocString EFFECT = string.Concat(new string[]
				{
					"Outputs a ",
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					" if exactly one of its Inputs is receiving ",
					UI.FormatAsAutomationState("Green", UI.AutomationState.Active),
					".\n\nOutputs a ",
					UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby),
					" if both or neither Inputs are receiving a ",
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					"."
				});

				public static LocString OUTPUT_NAME = "OUTPUT";

				public static LocString OUTPUT_ACTIVE = "Sends a " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + " if exactly one of its Inputs is receiving " + UI.FormatAsAutomationState("Green", UI.AutomationState.Active);

				public static LocString OUTPUT_INACTIVE = "Sends a " + UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby) + " if both Input signals match (any color)";
			}

			public class LOGICGATEBUFFER
			{
				public static LocString NAME = UI.FormatAsLink("BUFFER Gate", "LOGICGATEBUFFER");

				public static LocString DESC = "This gate continues outputting a Green Signal for a short time after the gate stops receiving a Green Signal.";

				public static LocString EFFECT = string.Concat(new string[]
				{
					"Outputs a ",
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					" if the Input is receiving a ",
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					".\n\nContinues sending a ",
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					" for an amount of buffer time after the Input receives a ",
					UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby),
					"."
				});

				public static LocString OUTPUT_NAME = "OUTPUT";

				public static LocString OUTPUT_ACTIVE = string.Concat(new string[]
				{
					"Sends a ",
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					" while receiving ",
					UI.FormatAsAutomationState("Green", UI.AutomationState.Active),
					". After receiving ",
					UI.FormatAsAutomationState("Red", UI.AutomationState.Standby),
					", will continue sending ",
					UI.FormatAsAutomationState("Green", UI.AutomationState.Active),
					" until the timer has expired"
				});

				public static LocString OUTPUT_INACTIVE = "Otherwise, sends a " + UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby) + ".";
			}

			public class LOGICGATEFILTER
			{
				public static LocString NAME = UI.FormatAsLink("FILTER Gate", "LOGICGATEFILTER");

				public static LocString DESC = "This gate only lets a Green Signal through if its Input has received a Green Signal that lasted longer than the selected filter time.";

				public static LocString EFFECT = string.Concat(new string[]
				{
					"Only lets a ",
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					" through if the Input has received a ",
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					" for longer than the selected filter time.\n\nWill continue outputting a ",
					UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby),
					" if the ",
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					" did not last long enough."
				});

				public static LocString OUTPUT_NAME = "OUTPUT";

				public static LocString OUTPUT_ACTIVE = string.Concat(new string[]
				{
					"Sends a ",
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					" after receiving ",
					UI.FormatAsAutomationState("Green", UI.AutomationState.Active),
					" for longer than the selected filter timer"
				});

				public static LocString OUTPUT_INACTIVE = "Otherwise, sends a " + UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby) + ".";
			}

			public class LOGICMEMORY
			{
				public static LocString NAME = UI.FormatAsLink("Memory Toggle", "LOGICMEMORY");

				public static LocString DESC = "A Memory stores a Green Signal received in the Set Port (S) until the Reset Port (R) receives a Green Signal.";

				public static LocString EFFECT = string.Concat(new string[]
				{
					"Contains an internal Memory, and will output whatever signal is stored in that Memory.\n\nSignals sent to the Inputs <i>only</i> affect the Memory, and do not pass through to the Output. \n\nSending a ",
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					" to the Set Port (S) will set the memory to ",
					UI.FormatAsAutomationState("Green", UI.AutomationState.Active),
					". \n\nSending a ",
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					" to the Reset Port (R) will reset the memory back to ",
					UI.FormatAsAutomationState("Red", UI.AutomationState.Standby),
					"."
				});

				public static LocString STATUS_ITEM_VALUE = "Current Value: {0}";

				public static LocString READ_PORT = "MEMORY OUTPUT";

				public static LocString READ_PORT_ACTIVE = "Outputs a " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + " if the internal Memory is set to " + UI.FormatAsAutomationState("Green", UI.AutomationState.Active);

				public static LocString READ_PORT_INACTIVE = "Outputs a " + UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby) + " if the internal Memory is set to " + UI.FormatAsAutomationState("Red", UI.AutomationState.Standby);

				public static LocString SET_PORT = "SET PORT (S)";

				public static LocString SET_PORT_ACTIVE = UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + ": Set the internal Memory to " + UI.FormatAsAutomationState("Green", UI.AutomationState.Active);

				public static LocString SET_PORT_INACTIVE = UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby) + ": No effect";

				public static LocString RESET_PORT = "RESET PORT (R)";

				public static LocString RESET_PORT_ACTIVE = UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + ": Reset the internal Memory to " + UI.FormatAsAutomationState("Red", UI.AutomationState.Standby);

				public static LocString RESET_PORT_INACTIVE = UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby) + ": No effect";
			}

			public class LOGICGATEMULTIPLEXER
			{
				public static LocString NAME = UI.FormatAsLink("Signal Selector", "LOGICGATEMULTIPLEXER");

				public static LocString DESC = "Signal Selectors can be used to select which automation signal is relevant to pass through to a given circuit";

				public static LocString EFFECT = string.Concat(new string[]
				{
					"Select which one of four Input signals should be sent out the Output, using Control Inputs.\n\nSend a ",
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					" or a ",
					UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby),
					" to the two Control Inputs to determine which Input is selected."
				});

				public static LocString OUTPUT_NAME = "OUTPUT";

				public static LocString OUTPUT_ACTIVE = string.Concat(new string[]
				{
					"Receives a ",
					UI.FormatAsAutomationState("Green", UI.AutomationState.Active),
					" or ",
					UI.FormatAsAutomationState("Red", UI.AutomationState.Standby),
					" signal from the selected input"
				});

				public static LocString OUTPUT_INACTIVE = "Nothing";
			}

			public class LOGICGATEDEMULTIPLEXER
			{
				public static LocString NAME = UI.FormatAsLink("Signal Distributor", "LOGICGATEDEMULTIPLEXER");

				public static LocString DESC = "Signal Distributors can be used to choose which circuit should receive a given automation signal.";

				public static LocString EFFECT = string.Concat(new string[]
				{
					"Route a single Input signal out one of four possible Outputs, based on the selection made by the Control Inputs.\n\nSend a ",
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					" or a ",
					UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby),
					" to the two Control Inputs to determine which Output is selected."
				});

				public static LocString OUTPUT_NAME = "OUTPUT";

				public static LocString OUTPUT_ACTIVE = string.Concat(new string[]
				{
					"Sends a ",
					UI.FormatAsAutomationState("Green", UI.AutomationState.Active),
					" or ",
					UI.FormatAsAutomationState("Red", UI.AutomationState.Standby),
					" signal to the selected output"
				});

				public static LocString OUTPUT_INACTIVE = "Nothing";
			}

			public class LOGICSWITCH
			{
				public static LocString NAME = UI.FormatAsLink("Signal Switch", "LOGICSWITCH");

				public static LocString DESC = "Signal switches don't turn grids on and off like power switches, but add an extra signal.";

				public static LocString EFFECT = string.Concat(new string[]
				{
					"Sends a ",
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					" or a ",
					UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby),
					" on an ",
					UI.FormatAsLink("Automation", "LOGIC"),
					" grid."
				});

				public static LocString SIDESCREEN_TITLE = "Signal Switch";

				public static LocString LOGIC_PORT = "Signal Toggle";

				public static LocString LOGIC_PORT_ACTIVE = "Sends a " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + " if toggled ON";

				public static LocString LOGIC_PORT_INACTIVE = "Sends a " + UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby) + " if toggled OFF";
			}

			public class LOGICPRESSURESENSORGAS
			{
				public static LocString NAME = UI.FormatAsLink("Atmo Sensor", "LOGICPRESSURESENSORGAS");

				public static LocString DESC = "Atmo sensors can be used to prevent excess oxygen production and overpressurization.";

				public static LocString EFFECT = string.Concat(new string[]
				{
					"Sends a ",
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					" or a ",
					UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby),
					" when ",
					UI.FormatAsLink("Gas", "ELEMENTS_GAS"),
					" pressure enters the chosen range."
				});

				public static LocString LOGIC_PORT = UI.FormatAsLink("Gas", "ELEMENTS_GAS") + " Pressure";

				public static LocString LOGIC_PORT_ACTIVE = "Sends a " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + " if Gas pressure is within the selected range";

				public static LocString LOGIC_PORT_INACTIVE = "Otherwise, sends a " + UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby);
			}

			public class LOGICPRESSURESENSORLIQUID
			{
				public static LocString NAME = UI.FormatAsLink("Hydro Sensor", "LOGICPRESSURESENSORLIQUID");

				public static LocString DESC = "A hydro sensor can tell a pump to refill its basin as soon as it contains too little liquid.";

				public static LocString EFFECT = string.Concat(new string[]
				{
					"Sends a ",
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					" or a ",
					UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby),
					" when ",
					UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID"),
					" pressure enters the chosen range.\n\nMust be submerged in ",
					UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID"),
					"."
				});

				public static LocString LOGIC_PORT = UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID") + " Pressure";

				public static LocString LOGIC_PORT_ACTIVE = "Sends a " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + " if Liquid pressure is within the selected range";

				public static LocString LOGIC_PORT_INACTIVE = "Otherwise, sends a " + UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby);
			}

			public class LOGICTEMPERATURESENSOR
			{
				public static LocString NAME = UI.FormatAsLink("Thermo Sensor", "LOGICTEMPERATURESENSOR");

				public static LocString DESC = "Thermo sensors can disable buildings when they approach dangerous temperatures.";

				public static LocString EFFECT = string.Concat(new string[]
				{
					"Sends a ",
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					" or a ",
					UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby),
					" when ambient ",
					UI.FormatAsLink("Temperature", "HEAT"),
					" enters the chosen range."
				});

				public static LocString LOGIC_PORT = "Ambient " + UI.FormatAsLink("Temperature", "HEAT");

				public static LocString LOGIC_PORT_ACTIVE = string.Concat(new string[]
				{
					"Sends a ",
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					" if ambient ",
					UI.FormatAsLink("Temperature", "HEAT"),
					" is within the selected range"
				});

				public static LocString LOGIC_PORT_INACTIVE = "Otherwise, sends a " + UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby);
			}

			public class LOGICLIGHTSENSOR
			{
				public static LocString NAME = UI.FormatAsLink("Light Sensor", "LOGICLIGHTSENSOR");

				public static LocString DESC = "Light sensors can tell surface bunker doors above solar panels to open or close based on solar light levels.";

				public static LocString EFFECT = string.Concat(new string[]
				{
					"Sends a ",
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					" or a ",
					UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby),
					" when ambient ",
					UI.FormatAsLink("Brightness", "LIGHT"),
					" enters the chosen range."
				});

				public static LocString LOGIC_PORT = "Ambient " + UI.FormatAsLink("Brightness", "LIGHT");

				public static LocString LOGIC_PORT_ACTIVE = string.Concat(new string[]
				{
					"Sends a ",
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					" if ambient ",
					UI.FormatAsLink("Brightness", "LIGHT"),
					" is within the selected range"
				});

				public static LocString LOGIC_PORT_INACTIVE = "Otherwise, sends a " + UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby);
			}

			public class LOGICWATTAGESENSOR
			{
				public static LocString NAME = UI.FormatAsLink("Wattage Sensor", "LOGICWATTSENSOR");

				public static LocString DESC = "Wattage sensors can send a signal when a building has switched on or off.";

				public static LocString EFFECT = string.Concat(new string[]
				{
					"Sends a ",
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					" or a ",
					UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby),
					" when ",
					UI.FormatAsLink("Wattage", "POWER"),
					" consumed enters the chosen range."
				});

				public static LocString LOGIC_PORT = "Consumed " + UI.FormatAsLink("Wattage", "POWER");

				public static LocString LOGIC_PORT_ACTIVE = string.Concat(new string[]
				{
					"Sends a ",
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					" if current ",
					UI.FormatAsLink("Wattage", "POWER"),
					" is within the selected range"
				});

				public static LocString LOGIC_PORT_INACTIVE = "Otherwise, sends a " + UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby);
			}

			public class LOGICHEPSENSOR
			{
				public static LocString NAME = UI.FormatAsLink("Radbolt Sensor", "LOGICHEPSENSOR");

				public static LocString DESC = "Radbolt sensors can send a signal when a Radbolt passes over them.";

				public static LocString EFFECT = string.Concat(new string[]
				{
					"Sends a ",
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					" or a ",
					UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby),
					" when Radbolts detected enters the chosen range."
				});

				public static LocString LOGIC_PORT = "Detected Radbolts";

				public static LocString LOGIC_PORT_ACTIVE = "Sends a " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + " if detected Radbolts are within the selected range";

				public static LocString LOGIC_PORT_INACTIVE = "Otherwise, sends a " + UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby);
			}

			public class LOGICTIMEOFDAYSENSOR
			{
				public static LocString NAME = UI.FormatAsLink("Cycle Sensor", "LOGICTIMEOFDAYSENSOR");

				public static LocString DESC = "Cycle sensors ensure systems always turn on at the same time, day or night, every cycle.";

				public static LocString EFFECT = string.Concat(new string[]
				{
					"Sets an automatic ",
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					" and ",
					UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby),
					" schedule within one day-night cycle."
				});

				public static LocString LOGIC_PORT = "Cycle Time";

				public static LocString LOGIC_PORT_ACTIVE = string.Concat(new string[]
				{
					"Sends a ",
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					" if current time is within the selected ",
					UI.FormatAsAutomationState("Green", UI.AutomationState.Active),
					" range"
				});

				public static LocString LOGIC_PORT_INACTIVE = "Otherwise, sends a " + UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby);
			}

			public class LOGICTIMERSENSOR
			{
				public static LocString NAME = UI.FormatAsLink("Timer Sensor", "LOGICTIMERSENSOR");

				public static LocString DESC = "Timer sensors create automation schedules for very short or very long periods of time.";

				public static LocString EFFECT = string.Concat(new string[]
				{
					"Creates a timer to send ",
					UI.FormatAsAutomationState("Green Signals", UI.AutomationState.Active),
					" and ",
					UI.FormatAsAutomationState("Red Signals", UI.AutomationState.Standby),
					" for specific amounts of time."
				});

				public static LocString LOGIC_PORT = "Timer Schedule";

				public static LocString LOGIC_PORT_ACTIVE = "Sends a " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + " for the selected amount of Green time";

				public static LocString LOGIC_PORT_INACTIVE = "Then, sends a " + UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby) + " for the selected amount of Red time";
			}

			public class LOGICCRITTERCOUNTSENSOR
			{
				public static LocString NAME = UI.FormatAsLink("Critter Sensor", "LOGICCRITTERCOUNTSENSOR");

				public static LocString DESC = "Detecting critter populations can help adjust their automated feeding and care regimens.";

				public static LocString EFFECT = string.Concat(new string[]
				{
					"Sends a ",
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					" or a ",
					UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby),
					" based on the number of eggs and critters in a room."
				});

				public static LocString LOGIC_PORT = "Critter Count";

				public static LocString LOGIC_PORT_ACTIVE = "Sends a " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + " if the number of Critters and Eggs in the Room is greater than the selected threshold.";

				public static LocString LOGIC_PORT_INACTIVE = "Otherwise, sends a " + UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby);

				public static LocString SIDESCREEN_TITLE = "Critter Sensor";

				public static LocString COUNT_CRITTER_LABEL = "Count Critters";

				public static LocString COUNT_EGG_LABEL = "Count Eggs";
			}

			public class LOGICCLUSTERLOCATIONSENSOR
			{
				public static LocString NAME = UI.FormatAsLink("Starmap Location Sensor", "LOGICCLUSTERLOCATIONSENSOR");

				public static LocString DESC = "Starmap Location sensors can signal when a spacecraft is at a certain location";

				public static LocString EFFECT = string.Concat(new string[]
				{
					"Send ",
					UI.FormatAsAutomationState("Green Signals", UI.AutomationState.Active),
					" at the chosen Starmap locations and ",
					UI.FormatAsAutomationState("Red Signals", UI.AutomationState.Standby),
					" everywhere else."
				});

				public static LocString LOGIC_PORT = "Starmap Location Sensor";

				public static LocString LOGIC_PORT_ACTIVE = "Sends a " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + "when a spacecraft is at the chosen Starmap locations";

				public static LocString LOGIC_PORT_INACTIVE = "Otherwise, sends a " + UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby);
			}

			public class LOGICDUPLICANTSENSOR
			{
				public static LocString NAME = UI.FormatAsLink("Duplicant Motion Sensor", "DUPLICANTSENSOR");

				public static LocString DESC = "Motion sensors save power by only enabling buildings when Duplicants are nearby.";

				public static LocString EFFECT = string.Concat(new string[]
				{
					"Sends a ",
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					" or a ",
					UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby),
					" based on whether a Duplicant is in the sensor's range."
				});

				public static LocString LOGIC_PORT = "Duplicant Motion Sensor";

				public static LocString LOGIC_PORT_ACTIVE = "Sends a " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + " while a Duplicant is in the sensor's tile range";

				public static LocString LOGIC_PORT_INACTIVE = "Otherwise, sends a " + UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby);
			}

			public class LOGICDISEASESENSOR
			{
				public static LocString NAME = UI.FormatAsLink("Germ Sensor", "LOGICDISEASESENSOR");

				public static LocString DESC = "Detecting germ populations can help block off or clean up dangerous areas.";

				public static LocString EFFECT = string.Concat(new string[]
				{
					"Sends a ",
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					" or a ",
					UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby),
					" based on quantity of surrounding ",
					UI.FormatAsLink("Germs", "DISEASE"),
					"."
				});

				public static LocString LOGIC_PORT = UI.FormatAsLink("Germ", "DISEASE") + " Count";

				public static LocString LOGIC_PORT_ACTIVE = "Sends a " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + " if the number of Germs is within the selected range";

				public static LocString LOGIC_PORT_INACTIVE = "Otherwise, sends a " + UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby);
			}

			public class LOGICELEMENTSENSORGAS
			{
				public static LocString NAME = UI.FormatAsLink("Gas Element Sensor", "LOGICELEMENTSENSORGAS");

				public static LocString DESC = "These sensors can detect the presence of a specific gas and alter systems accordingly.";

				public static LocString EFFECT = string.Concat(new string[]
				{
					"Sends a ",
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					" when the selected ",
					UI.FormatAsLink("Gas", "ELEMENTS_GAS"),
					" is detected on this sensor's tile.\n\nSends a ",
					UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby),
					" when the selected ",
					UI.FormatAsLink("Gas", "ELEMENTS_GAS"),
					" is not present."
				});

				public static LocString LOGIC_PORT = "Specific " + UI.FormatAsLink("Gas", "ELEMENTS_GAS") + " Presence";

				public static LocString LOGIC_PORT_ACTIVE = "Sends a " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + " if the selected Gas is detected";

				public static LocString LOGIC_PORT_INACTIVE = "Otherwise, sends a " + UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby);
			}

			public class LOGICELEMENTSENSORLIQUID
			{
				public static LocString NAME = UI.FormatAsLink("Liquid Element Sensor", "LOGICELEMENTSENSORLIQUID");

				public static LocString DESC = "These sensors can detect the presence of a specific liquid and alter systems accordingly.";

				public static LocString EFFECT = string.Concat(new string[]
				{
					"Sends a ",
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					" when the selected ",
					UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID"),
					" is detected on this sensor's tile.\n\nSends a ",
					UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby),
					" when the selected ",
					UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID"),
					" is not present."
				});

				public static LocString LOGIC_PORT = "Specific " + UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID") + " Presence";

				public static LocString LOGIC_PORT_ACTIVE = "Sends a " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + " if the selected Liquid is detected";

				public static LocString LOGIC_PORT_INACTIVE = "Otherwise, sends a " + UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby);
			}

			public class LOGICRADIATIONSENSOR
			{
				public static LocString NAME = UI.FormatAsLink("Radiation Sensor", "LOGICRADIATIONSENSOR");

				public static LocString DESC = "Radiation sensors can disable buildings when they detect dangerous levels of radiation.";

				public static LocString EFFECT = string.Concat(new string[]
				{
					"Sends a ",
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					" or a ",
					UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby),
					" when ambient ",
					UI.FormatAsLink("Radiation", "RADIATION"),
					" enters the chosen range."
				});

				public static LocString LOGIC_PORT = "Ambient " + UI.FormatAsLink("Radiation", "RADIATION");

				public static LocString LOGIC_PORT_ACTIVE = string.Concat(new string[]
				{
					"Sends a ",
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					" if ambient ",
					UI.FormatAsLink("Radiation", "RADIATION"),
					" is within the selected range"
				});

				public static LocString LOGIC_PORT_INACTIVE = "Otherwise, sends a " + UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby);
			}

			public class GASCONDUITDISEASESENSOR
			{
				public static LocString NAME = UI.FormatAsLink("Gas Pipe Germ Sensor", "GASCONDUITDISEASESENSOR");

				public static LocString DESC = "Germ sensors can help control automation behavior in the presence of germs.";

				public static LocString EFFECT = string.Concat(new string[]
				{
					"Sends a ",
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					" or a ",
					UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby),
					" based on the internal ",
					UI.FormatAsLink("Germ", "DISEASE"),
					" count of the pipe."
				});

				public static LocString LOGIC_PORT = "Internal " + UI.FormatAsLink("Germ", "DISEASE") + " Count";

				public static LocString LOGIC_PORT_ACTIVE = "Sends a " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + " if the number of Germs in the pipe is within the selected range";

				public static LocString LOGIC_PORT_INACTIVE = "Otherwise, sends a " + UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby);
			}

			public class LIQUIDCONDUITDISEASESENSOR
			{
				public static LocString NAME = UI.FormatAsLink("Liquid Pipe Germ Sensor", "LIQUIDCONDUITDISEASESENSOR");

				public static LocString DESC = "Germ sensors can help control automation behavior in the presence of germs.";

				public static LocString EFFECT = string.Concat(new string[]
				{
					"Sends a ",
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					" or a ",
					UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby),
					" based on the internal ",
					UI.FormatAsLink("Germ", "DISEASE"),
					" count of the pipe."
				});

				public static LocString LOGIC_PORT = "Internal " + UI.FormatAsLink("Germ", "DISEASE") + " Count";

				public static LocString LOGIC_PORT_ACTIVE = "Sends a " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + " if the number of Germs in the pipe is within the selected range";

				public static LocString LOGIC_PORT_INACTIVE = "Otherwise, sends a " + UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby);
			}

			public class SOLIDCONDUITDISEASESENSOR
			{
				public static LocString NAME = UI.FormatAsLink("Conveyor Rail Germ Sensor", "SOLIDCONDUITDISEASESENSOR");

				public static LocString DESC = "Germ sensors can help control automation behavior in the presence of germs.";

				public static LocString EFFECT = string.Concat(new string[]
				{
					"Sends a ",
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					" or a ",
					UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby),
					" based on the internal ",
					UI.FormatAsLink("Germ", "DISEASE"),
					" count of the object on the rail."
				});

				public static LocString LOGIC_PORT = "Internal " + UI.FormatAsLink("Germ", "DISEASE") + " Count";

				public static LocString LOGIC_PORT_ACTIVE = "Sends a " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + " if the number of Germs on the object on the rail is within the selected range";

				public static LocString LOGIC_PORT_INACTIVE = "Otherwise, sends a " + UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby);
			}

			public class GASCONDUITELEMENTSENSOR
			{
				public static LocString NAME = UI.FormatAsLink("Gas Pipe Element Sensor", "GASCONDUITELEMENTSENSOR");

				public static LocString DESC = "Element sensors can be used to detect the presence of a specific gas in a pipe.";

				public static LocString EFFECT = string.Concat(new string[]
				{
					"Sends a ",
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					" when the selected ",
					UI.FormatAsLink("Gas", "ELEMENTS_GAS"),
					" is detected within a pipe."
				});

				public static LocString LOGIC_PORT = "Internal " + UI.FormatAsLink("Gas", "ELEMENTS_GAS") + " Presence";

				public static LocString LOGIC_PORT_ACTIVE = "Sends a " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + " if the configured Gas is detected";

				public static LocString LOGIC_PORT_INACTIVE = "Otherwise, sends a " + UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby);
			}

			public class LIQUIDCONDUITELEMENTSENSOR
			{
				public static LocString NAME = UI.FormatAsLink("Liquid Pipe Element Sensor", "LIQUIDCONDUITELEMENTSENSOR");

				public static LocString DESC = "Element sensors can be used to detect the presence of a specific liquid in a pipe.";

				public static LocString EFFECT = string.Concat(new string[]
				{
					"Sends a ",
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					" when the selected ",
					UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID"),
					" is detected within a pipe."
				});

				public static LocString LOGIC_PORT = "Internal " + UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID") + " Presence";

				public static LocString LOGIC_PORT_ACTIVE = "Sends a " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + " if the configured Liquid is detected within the pipe";

				public static LocString LOGIC_PORT_INACTIVE = "Otherwise, sends a " + UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby);
			}

			public class SOLIDCONDUITELEMENTSENSOR
			{
				public static LocString NAME = UI.FormatAsLink("Conveyor Rail Element Sensor", "SOLIDCONDUITELEMENTSENSOR");

				public static LocString DESC = "Element sensors can be used to detect the presence of a specific item on a rail.";

				public static LocString EFFECT = "Sends a " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + " when the selected item is detected on a rail.";

				public static LocString LOGIC_PORT = "Internal " + UI.FormatAsLink("Item", "ELEMENTS_LIQUID") + " Presence";

				public static LocString LOGIC_PORT_ACTIVE = "Sends a " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + " if the configured item is detected on the rail";

				public static LocString LOGIC_PORT_INACTIVE = "Otherwise, sends a " + UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby);
			}

			public class GASCONDUITTEMPERATURESENSOR
			{
				public static LocString NAME = UI.FormatAsLink("Gas Pipe Thermo Sensor", "GASCONDUITTEMPERATURESENSOR");

				public static LocString DESC = "Thermo sensors disable buildings when their pipe contents reach a certain temperature.";

				public static LocString EFFECT = string.Concat(new string[]
				{
					"Sends a ",
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					" or a ",
					UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby),
					" when pipe contents enter the chosen ",
					UI.FormatAsLink("Temperature", "HEAT"),
					" range."
				});

				public static LocString LOGIC_PORT = "Internal " + UI.FormatAsLink("Gas", "ELEMENTS_GAS") + " " + UI.FormatAsLink("Temperature", "HEAT");

				public static LocString LOGIC_PORT_ACTIVE = "Sends a " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + " if the contained Gas is within the selected Temperature range";

				public static LocString LOGIC_PORT_INACTIVE = "Otherwise, sends a " + UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby);
			}

			public class LIQUIDCONDUITTEMPERATURESENSOR
			{
				public static LocString NAME = UI.FormatAsLink("Liquid Pipe Thermo Sensor", "LIQUIDCONDUITTEMPERATURESENSOR");

				public static LocString DESC = "Thermo sensors disable buildings when their pipe contents reach a certain temperature.";

				public static LocString EFFECT = string.Concat(new string[]
				{
					"Sends a ",
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					" or a ",
					UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby),
					" when pipe contents enter the chosen ",
					UI.FormatAsLink("Temperature", "HEAT"),
					" range."
				});

				public static LocString LOGIC_PORT = "Internal " + UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID") + " " + UI.FormatAsLink("Temperature", "HEAT");

				public static LocString LOGIC_PORT_ACTIVE = "Sends a " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + " if the contained Liquid is within the selected Temperature range";

				public static LocString LOGIC_PORT_INACTIVE = "Otherwise, sends a " + UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby);
			}

			public class SOLIDCONDUITTEMPERATURESENSOR
			{
				public static LocString NAME = UI.FormatAsLink("Conveyor Rail Thermo Sensor", "SOLIDCONDUITTEMPERATURESENSOR");

				public static LocString DESC = "Thermo sensors disable buildings when their rail contents reach a certain temperature.";

				public static LocString EFFECT = string.Concat(new string[]
				{
					"Sends a ",
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					" or a ",
					UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby),
					" when rail contents enter the chosen ",
					UI.FormatAsLink("Temperature", "HEAT"),
					" range."
				});

				public static LocString LOGIC_PORT = "Internal Item " + UI.FormatAsLink("Temperature", "HEAT");

				public static LocString LOGIC_PORT_ACTIVE = "Sends a " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + " if the contained item is within the selected Temperature range";

				public static LocString LOGIC_PORT_INACTIVE = "Otherwise, sends a " + UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby);
			}

			public class LOGICCOUNTER
			{
				public static LocString NAME = UI.FormatAsLink("Signal Counter", "LOGICCOUNTER");

				public static LocString DESC = "For numbers higher than ten connect multiple counters together.";

				public static LocString EFFECT = string.Concat(new string[]
				{
					"Counts how many times a ",
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					" has been received up to a chosen number.\n\nWhen the chosen number is reached it sends a ",
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					" until it receives another ",
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					", when it resets automatically and begins counting again."
				});

				public static LocString LOGIC_PORT = "Internal Counter Value";

				public static LocString INPUT_PORT_ACTIVE = UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + ": Increase counter by one";

				public static LocString INPUT_PORT_INACTIVE = UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby) + ": Nothing";

				public static LocString LOGIC_PORT_RESET = "Reset Counter";

				public static LocString RESET_PORT_ACTIVE = UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + ": Reset counter";

				public static LocString RESET_PORT_INACTIVE = UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby) + ": Nothing";

				public static LocString LOGIC_PORT_OUTPUT = "Number Reached";

				public static LocString OUTPUT_PORT_ACTIVE = "Sends a " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + " when the counter matches the selected value";

				public static LocString OUTPUT_PORT_INACTIVE = "Otherwise, sends a " + UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby);
			}

			public class LOGICALARM
			{
				public static LocString NAME = UI.FormatAsLink("Automated Notifier", "LOGICALARM");

				public static LocString DESC = "Sends a notification when it receives a " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + ".";

				public static LocString EFFECT = "Attach to sensors to send a notification when certain conditions are met.\n\nNotifications can be customized.";

				public static LocString LOGIC_PORT = "Notification";

				public static LocString INPUT_NAME = "INPUT";

				public static LocString INPUT_PORT_ACTIVE = UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + ": Push notification";

				public static LocString INPUT_PORT_INACTIVE = UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby) + ": Nothing";
			}

			public class PIXELPACK
			{
				public static LocString NAME = UI.FormatAsLink("Pixel Pack", "PIXELPACK");

				public static LocString DESC = "Four pixels which can be individually designated different colors.";

				public static LocString EFFECT = string.Concat(new string[]
				{
					"Pixels can be designated a color when it receives a ",
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					" and a different color when it receives a ",
					UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby),
					".\n\nInput from an ",
					UI.FormatAsLink("Automation Wire", "LOGICWIRE"),
					" controls the whole strip. Input from an ",
					UI.FormatAsLink("Automation Ribbon", "LOGICRIBBON"),
					" can control individual pixels on the strip."
				});

				public static LocString LOGIC_PORT = "Color Selection";

				public static LocString INPUT_NAME = "RIBBON INPUT";

				public static LocString INPUT_PORT_ACTIVE = UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + ": Display the configured " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + " pixels";

				public static LocString INPUT_PORT_INACTIVE = UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby) + ": Display the configured " + UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby) + " pixels";

				public static LocString SIDESCREEN_TITLE = "Pixel Pack";
			}

			public class LOGICHAMMER
			{
				public static LocString NAME = UI.FormatAsLink("Hammer", "LOGICHAMMER");

				public static LocString DESC = "The hammer makes neat sounds when it strikes buildings.";

				public static LocString EFFECT = "In its default orientation, the hammer strikes the building to the left when it receives a " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + ".\n\nEach building has a unique sound when struck by the hammer.\n\nThe hammer does no damage when it strikes.";

				public static LocString LOGIC_PORT = "Resonating Buildings";

				public static LocString INPUT_NAME = "INPUT";

				public static LocString INPUT_PORT_ACTIVE = UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + ": Hammer strikes once";

				public static LocString INPUT_PORT_INACTIVE = UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby) + ": Nothing";
			}

			public class LOGICRIBBONWRITER
			{
				public static LocString NAME = UI.FormatAsLink("Ribbon Writer", "LOGICRIBBONWRITER");

				public static LocString DESC = "Translates the signal from an " + UI.FormatAsLink("Automation Wire", "LOGICWIRE") + " to a single Bit in an " + UI.FormatAsLink("Automation Ribbon", "LOGICRIBBON");

				public static LocString EFFECT = string.Concat(new string[]
				{
					"Writes a ",
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					" or a ",
					UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby),
					" to the specified Bit of an ",
					BUILDINGS.PREFABS.LOGICRIBBON.NAME,
					"\n\n",
					BUILDINGS.PREFABS.LOGICRIBBON.NAME,
					" must be used as the output wire to avoid overloading."
				});

				public static LocString LOGIC_PORT = "1-Bit Input";

				public static LocString INPUT_NAME = "INPUT";

				public static LocString INPUT_PORT_ACTIVE = UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + ": Receives " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + " to be written to selected Bit";

				public static LocString INPUT_PORT_INACTIVE = UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby) + ": Receives " + UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby) + " to to be written selected Bit";

				public static LocString LOGIC_PORT_OUTPUT = "Bit Writing";

				public static LocString OUTPUT_NAME = "RIBBON OUTPUT";

				public static LocString OUTPUT_PORT_ACTIVE = string.Concat(new string[]
				{
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					": Writes a ",
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					" to selected Bit of an ",
					BUILDINGS.PREFABS.LOGICRIBBON.NAME
				});

				public static LocString OUTPUT_PORT_INACTIVE = string.Concat(new string[]
				{
					UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby),
					": Writes a ",
					UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby),
					" to selected Bit of an ",
					BUILDINGS.PREFABS.LOGICRIBBON.NAME
				});
			}

			public class LOGICRIBBONREADER
			{
				public static LocString NAME = UI.FormatAsLink("Ribbon Reader", "LOGICRIBBONREADER");

				public static LocString DESC = string.Concat(new string[]
				{
					"Inputs the signal from a single Bit in an ",
					UI.FormatAsLink("Automation Ribbon", "LOGICRIBBON"),
					" into an ",
					UI.FormatAsLink("Automation Wire", "LOGICWIRE"),
					"."
				});

				public static LocString EFFECT = string.Concat(new string[]
				{
					"Reads a ",
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					" or a ",
					UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby),
					" from the specified Bit of an ",
					BUILDINGS.PREFABS.LOGICRIBBON.NAME,
					" onto an ",
					BUILDINGS.PREFABS.LOGICWIRE.NAME,
					"."
				});

				public static LocString LOGIC_PORT = "4-Bit Input";

				public static LocString INPUT_NAME = "RIBBON INPUT";

				public static LocString INPUT_PORT_ACTIVE = UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + ": Reads a " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + " from selected Bit";

				public static LocString INPUT_PORT_INACTIVE = UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby) + ": Reads a " + UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby) + " from selected Bit";

				public static LocString LOGIC_PORT_OUTPUT = "Bit Reading";

				public static LocString OUTPUT_NAME = "OUTPUT";

				public static LocString OUTPUT_PORT_ACTIVE = string.Concat(new string[]
				{
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					": Sends a ",
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					" to attached ",
					UI.FormatAsLink("Automation Wire", "LOGICWIRE")
				});

				public static LocString OUTPUT_PORT_INACTIVE = string.Concat(new string[]
				{
					UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby),
					": Sends a ",
					UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby),
					" to attached ",
					UI.FormatAsLink("Automation Wire", "LOGICWIRE")
				});
			}

			public class TRAVELTUBEENTRANCE
			{
				public static LocString NAME = UI.FormatAsLink("Transit Tube Access", "TRAVELTUBEENTRANCE");

				public static LocString DESC = "Duplicants require access points to enter tubes, but not to exit them.";

				public static LocString EFFECT = string.Concat(new string[]
				{
					"Allows Duplicants to enter the connected ",
					UI.FormatAsLink("Transit Tube", "TRAVELTUBE"),
					" system.\n\nStops drawing ",
					UI.FormatAsLink("Power", "POWER"),
					" once fully charged."
				});
			}

			public class TRAVELTUBE
			{
				public static LocString NAME = UI.FormatAsLink("Transit Tube", "TRAVELTUBE");

				public static LocString DESC = "Duplicants will only exit a transit tube when a safe landing area is available beneath it.";

				public static LocString EFFECT = "Quickly transports Duplicants from a " + UI.FormatAsLink("Transit Tube Access", "TRAVELTUBEENTRANCE") + " to the tube's end.\n\nOnly transports Duplicants.";
			}

			public class TRAVELTUBEWALLBRIDGE
			{
				public static LocString NAME = UI.FormatAsLink("Transit Tube Crossing", "TRAVELTUBEWALLBRIDGE");

				public static LocString DESC = "Tube crossings can run transit tubes through walls without leaking gas or liquid.";

				public static LocString EFFECT = "Allows " + UI.FormatAsLink("Transit Tubes", "TRAVELTUBE") + " to be run through wall and floor tile.\n\nFunctions as regular tile.";
			}

			public class SOLIDCONDUIT
			{
				public static LocString NAME = UI.FormatAsLink("Conveyor Rail", "SOLIDCONDUIT");

				public static LocString DESC = "Rails move materials where they'll be needed most, saving Duplicants the walk.";

				public static LocString EFFECT = string.Concat(new string[]
				{
					"Transports ",
					UI.FormatAsLink("Solid Materials", "ELEMENTS_SOLID"),
					" on a track between ",
					UI.FormatAsLink("Conveyor Loader", "SOLIDCONDUITINBOX"),
					" and ",
					UI.FormatAsLink("Conveyor Receptacle", "SOLIDCONDUITOUTBOX"),
					".\n\nCan be run through wall and floor tile."
				});
			}

			public class SOLIDCONDUITINBOX
			{
				public static LocString NAME = UI.FormatAsLink("Conveyor Loader", "SOLIDCONDUITINBOX");

				public static LocString DESC = "Material filters can be used to determine what resources are sent down the rail.";

				public static LocString EFFECT = string.Concat(new string[]
				{
					"Loads ",
					UI.FormatAsLink("Solid Materials", "ELEMENTS_SOLID"),
					" onto ",
					UI.FormatAsLink("Conveyor Rail", "SOLIDCONDUIT"),
					" for transport.\n\nOnly loads the resources of your choosing."
				});
			}

			public class SOLIDCONDUITOUTBOX
			{
				public static LocString NAME = UI.FormatAsLink("Conveyor Receptacle", "SOLIDCONDUITOUTBOX");

				public static LocString DESC = "When materials reach the end of a rail they enter a receptacle to be used by Duplicants.";

				public static LocString EFFECT = string.Concat(new string[]
				{
					"Unloads ",
					UI.FormatAsLink("Solid Materials", "ELEMENTS_SOLID"),
					" from a ",
					UI.FormatAsLink("Conveyor Rail", "SOLIDCONDUIT"),
					" into storage."
				});
			}

			public class SOLIDTRANSFERARM
			{
				public static LocString NAME = UI.FormatAsLink("Auto-Sweeper", "SOLIDTRANSFERARM");

				public static LocString DESC = "An auto-sweeper's range can be viewed at any time by " + UI.CLICK(UI.ClickType.clicking) + " on the building.";

				public static LocString EFFECT = string.Concat(new string[]
				{
					"Automates ",
					UI.FormatAsLink("Sweeping", "CHORES"),
					" and ",
					UI.FormatAsLink("Supplying", "CHORES"),
					" errands by sucking up all nearby ",
					UI.FormatAsLink("Debris", "DECOR"),
					".\n\nMaterials are automatically delivered to any ",
					UI.FormatAsLink("Conveyor Loader", "SOLIDCONDUITINBOX"),
					", ",
					UI.FormatAsLink("Conveyor Receptacle", "SOLIDCONDUITOUTBOX"),
					", storage, or buildings within range."
				});
			}

			public class SOLIDCONDUITBRIDGE
			{
				public static LocString NAME = UI.FormatAsLink("Conveyor Bridge", "SOLIDCONDUITBRIDGE");

				public static LocString DESC = "Separating rail systems helps ensure materials go to the intended destinations.";

				public static LocString EFFECT = "Runs one " + UI.FormatAsLink("Conveyor Rail", "SOLIDCONDUIT") + " section over another without joining them.\n\nCan be run through wall and floor tile.";
			}

			public class SOLIDVENT
			{
				public static LocString NAME = UI.FormatAsLink("Conveyor Chute", "SOLIDVENT");

				public static LocString DESC = "When materials reach the end of a rail they are dropped back into the world.";

				public static LocString EFFECT = string.Concat(new string[]
				{
					"Unloads ",
					UI.FormatAsLink("Solid Materials", "ELEMENTS_SOLID"),
					" from a ",
					UI.FormatAsLink("Conveyor Rail", "SOLIDCONDUIT"),
					" onto the floor."
				});
			}

			public class SOLIDLOGICVALVE
			{
				public static LocString NAME = UI.FormatAsLink("Conveyor Shutoff", "SOLIDLOGICVALVE");

				public static LocString DESC = "Automated conveyors save power and time by removing the need for Duplicant input.";

				public static LocString EFFECT = string.Concat(new string[]
				{
					"Connects to an ",
					UI.FormatAsLink("Automation", "LOGIC"),
					" grid to automatically turn ",
					UI.FormatAsLink("Solid Material", "ELEMENTS_SOLID"),
					" transport on or off."
				});

				public static LocString LOGIC_PORT = "Open/Close";

				public static LocString LOGIC_PORT_ACTIVE = UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + ": Allow material transport";

				public static LocString LOGIC_PORT_INACTIVE = UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby) + ": Prevent material transport";
			}

			public class SOLIDLIMITVALVE
			{
				public static LocString NAME = UI.FormatAsLink("Conveyor Meter", "SOLIDLIMITVALVE");

				public static LocString DESC = "Conveyor Meters let an exact amount of materials pass through before shutting off.";

				public static LocString EFFECT = "Connects to an " + UI.FormatAsLink("Automation", "LOGIC") + " grid to automatically turn material transfer off when the specified amount has passed through it.";

				public static LocString LOGIC_PORT_OUTPUT = "Limit Reached";

				public static LocString OUTPUT_PORT_ACTIVE = "Sends a " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + " if limit has been reached";

				public static LocString OUTPUT_PORT_INACTIVE = "Otherwise, sends a " + UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby);

				public static LocString LOGIC_PORT_RESET = "Reset Meter";

				public static LocString RESET_PORT_ACTIVE = UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + ": Reset the amount";

				public static LocString RESET_PORT_INACTIVE = UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby) + ": Nothing";
			}

			public class DEVPUMPSOLID
			{
				public static LocString NAME = "Dev Pump Solid";

				public static LocString DESC = "Piping a pump's output to a building's intake will send solids to that building.";

				public static LocString EFFECT = "Generates chosen " + UI.FormatAsLink("Solid Materials", "ELEMENTS_SOLID") + " and runs it through " + UI.FormatAsLink("Conveyor Rail", "SOLIDCONDUIT");
			}

			public class AUTOMINER
			{
				public static LocString NAME = UI.FormatAsLink("Robo-Miner", "AUTOMINER");

				public static LocString DESC = "A robo-miner's range can be viewed at any time by selecting the building.";

				public static LocString EFFECT = "Automatically digs out all materials in a set range.";
			}

			public class CREATUREFEEDER
			{
				public static LocString NAME = UI.FormatAsLink("Critter Feeder", "CREATUREFEEDER");

				public static LocString DESC = "Critters tend to stay close to their food source and wander less when given a feeder.";

				public static LocString EFFECT = "Automatically dispenses food for hungry " + UI.FormatAsLink("Critters", "CREATURES") + ".";
			}

			public class GRAVITASPEDESTAL
			{
				public static LocString NAME = UI.FormatAsLink("Pedestal", "ITEMPEDESTAL");

				public static LocString DESC = "Perception can be drastically changed by a bit of thoughtful presentation.";

				public static LocString EFFECT = "Displays a single object, doubling its " + UI.FormatAsLink("Decor", "DECOR") + " value.\n\nObjects with negative Decor will gain some positive Decor when displayed.";

				public static LocString DISPLAYED_ITEM_FMT = "Displayed {0}";
			}

			public class ITEMPEDESTAL
			{
				public static LocString NAME = UI.FormatAsLink("Pedestal", "ITEMPEDESTAL");

				public static LocString DESC = "Perception can be drastically changed by a bit of thoughtful presentation.";

				public static LocString EFFECT = "Displays a single object, doubling its " + UI.FormatAsLink("Decor", "DECOR") + " value.\n\nObjects with negative Decor will gain some positive Decor when displayed.";

				public static LocString DISPLAYED_ITEM_FMT = "Displayed {0}";

				public class FACADES
				{
					public class DEFAULT_ITEMPEDESTAL
					{
						public static LocString NAME = UI.FormatAsLink("Pedestal", "ITEMPEDESTAL");

						public static LocString DESC = "Perception can be drastically changed by a bit of thoughtful presentation.";
					}

					public class HAND
					{
						public static LocString NAME = UI.FormatAsLink("Hand of Dupe Pedestal", "ITEMPEDESTAL");

						public static LocString DESC = "This pedestal cradles precious objects in the palm of its hand.";
					}
				}
			}

			public class CROWNMOULDING
			{
				public static LocString NAME = UI.FormatAsLink("Ceiling Trim", "CROWNMOULDING");

				public static LocString DESC = "Ceiling trim is a purely decorative addition to one's overhead area.";

				public static LocString EFFECT = string.Concat(new string[]
				{
					"Used to decorate the ceilings of rooms.\n\nIncreases ",
					UI.FormatAsLink("Decor", "DECOR"),
					", contributing to ",
					UI.FormatAsLink("Morale", "MORALE"),
					"."
				});

				public class FACADES
				{
					public class DEFAULT_CROWNMOULDING
					{
						public static LocString NAME = UI.FormatAsLink("Ceiling Trim", "CROWNMOULDING");

						public static LocString DESC = "Ceiling trim is a purely decorative addition to one's overhead area.";
					}

					public class SHINEORNAMENTS
					{
						public static LocString NAME = UI.FormatAsLink("Fancy Bug Ceiling Garland", "CROWNMOULDING");

						public static LocString DESC = "Someone spent their entire weekend gluing ribbons to paper Shine Bug cut-outs, and it shows.";
					}
				}
			}

			public class CORNERMOULDING
			{
				public static LocString NAME = UI.FormatAsLink("Corner Trim", "CORNERMOULDING");

				public static LocString DESC = "Corner trim is a purely decorative addition for ceiling corners.";

				public static LocString EFFECT = string.Concat(new string[]
				{
					"Used to decorate the ceiling corners of rooms.\n\nIncreases ",
					UI.FormatAsLink("Decor", "DECOR"),
					", contributing to ",
					UI.FormatAsLink("Morale", "MORALE"),
					"."
				});

				public class FACADES
				{
					public class DEFAULT_CORNERMOULDING
					{
						public static LocString NAME = UI.FormatAsLink("Corner Trim", "CORNERMOULDING");

						public static LocString DESC = "It really dresses up a ceiling corner.";
					}

					public class SHINEORNAMENTS
					{
						public static LocString NAME = UI.FormatAsLink("Fancy Bug Corner Garland", "CORNERMOULDING");

						public static LocString DESC = "Why deck the halls, when you could <i>festoon</i> them?";
					}
				}
			}

			public class EGGINCUBATOR
			{
				public static LocString NAME = UI.FormatAsLink("Incubator", "EGGINCUBATOR");

				public static LocString DESC = "Incubators can maintain the ideal internal conditions for several species of critter egg.";

				public static LocString EFFECT = string.Concat(new string[]
				{
					"Incubates ",
					UI.FormatAsLink("Critter", "CREATURES"),
					" eggs until ready to hatch.\n\nAssigned Duplicants must possess the ",
					UI.FormatAsLink("Critter Ranching", "RANCHING1"),
					" skill."
				});
			}

			public class EGGCRACKER
			{
				public static LocString NAME = UI.FormatAsLink("Egg Cracker", "EGGCRACKER");

				public static LocString DESC = "Raw eggs are an ingredient in certain high quality food recipes.";

				public static LocString EFFECT = "Converts viable " + UI.FormatAsLink("Critter", "CREATURES") + " eggs into cooking ingredients.\n\nCracked Eggs cannot hatch.\n\nDuplicants will not crack eggs unless tasks are queued.";

				public static LocString RECIPE_DESCRIPTION = "Turns {0} into {1}.";

				public static LocString RESULT_DESCRIPTION = "Cracked {0}";

				public class FACADES
				{
					public class DEFAULT_EGGCRACKER
					{
						public static LocString NAME = UI.FormatAsLink("Egg Cracker", "EGGCRACKER");

						public static LocString DESC = "It cracks eggs.";
					}

					public class BEAKER
					{
						public static LocString NAME = UI.FormatAsLink("Beaker Cracker", "EGGCRACKER");

						public static LocString DESC = "A practical exercise in physics.";
					}

					public class FLOWER
					{
						public static LocString NAME = UI.FormatAsLink("Blossom Cracker", "EGGCRACKER");

						public static LocString DESC = "Now with EZ-clean petals.";
					}

					public class HANDS
					{
						public static LocString NAME = UI.FormatAsLink("Handy Cracker", "EGGCRACKER");

						public static LocString DESC = "Just like Mi-Ma used to have.";
					}
				}
			}

			public class URANIUMCENTRIFUGE
			{
				public static LocString NAME = UI.FormatAsLink("Uranium Centrifuge", "URANIUMCENTRIFUGE");

				public static LocString DESC = "Enriched uranium is a specialized substance that can be used to fuel powerful research reactors.";

				public static LocString EFFECT = string.Concat(new string[]
				{
					"Extracts ",
					UI.FormatAsLink("Enriched Uranium", "ENRICHEDURANIUM"),
					" from ",
					UI.FormatAsLink("Uranium Ore", "URANIUMORE"),
					".\n\nOutputs ",
					UI.FormatAsLink("Depleted Uranium", "DEPLETEDURANIUM"),
					" in molten form."
				});

				public static LocString RECIPE_DESCRIPTION = "Convert Uranium ore to Molten Uranium and Enriched Uranium";
			}

			public class HIGHENERGYPARTICLEREDIRECTOR
			{
				public static LocString NAME = UI.FormatAsLink("Radbolt Reflector", "HIGHENERGYPARTICLEREDIRECTOR");

				public static LocString DESC = "We were all out of mirrors.";

				public static LocString EFFECT = "Receives and redirects Radbolts from " + UI.FormatAsLink("Radbolt Generators", "HIGHENERGYPARTICLESPAWNER") + ".";

				public static LocString LOGIC_PORT = "Ignore incoming Radbolts";

				public static LocString LOGIC_PORT_ACTIVE = UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + ": Allow incoming Radbolts";

				public static LocString LOGIC_PORT_INACTIVE = UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby) + ": Ignore incoming Radbolts";
			}

			public class MANUALHIGHENERGYPARTICLESPAWNER
			{
				public static LocString NAME = UI.FormatAsLink("Manual Radbolt Generator", "MANUALHIGHENERGYPARTICLESPAWNER");

				public static LocString DESC = "Radbolts are necessary for producing Materials Science research.";

				public static LocString EFFECT = "Refines radioactive ores to generate Radbolts.\n\nEmits generated Radbolts in the direction of your choosing.";

				public static LocString RECIPE_DESCRIPTION = "Creates " + UI.FormatAsLink("Radbolts", "RADIATION") + " by processing {0}. Also creates {1} as a byproduct.";
			}

			public class HIGHENERGYPARTICLESPAWNER
			{
				public static LocString NAME = UI.FormatAsLink("Radbolt Generator", "HIGHENERGYPARTICLESPAWNER");

				public static LocString DESC = "Radbolts are necessary for producing Materials Science research.";

				public static LocString EFFECT = "Attracts nearby " + UI.FormatAsLink("Radiation", "RADIATION") + " to generate Radbolts.\n\nEmits generated Radbolts in the direction of your choosing when the set Radbolt threshold is reached.\n\nRadbolts collected will rapidly decay while this building is disabled.";

				public static LocString LOGIC_PORT = "Do not emit Radbolts";

				public static LocString LOGIC_PORT_ACTIVE = UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + ": Emit Radbolts";

				public static LocString LOGIC_PORT_INACTIVE = UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby) + ": Do not emit Radbolts";
			}

			public class DEVHEPSPAWNER
			{
				public static LocString NAME = "Dev Radbolt Generator";

				public static LocString DESC = "Radbolts are necessary for producing Materials Science research.";

				public static LocString EFFECT = "Generates Radbolts.";

				public static LocString LOGIC_PORT = "Do not emit Radbolts";

				public static LocString LOGIC_PORT_ACTIVE = UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + ": Emit Radbolts";

				public static LocString LOGIC_PORT_INACTIVE = UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby) + ": Do not emit Radbolts";
			}

			public class HEPBATTERY
			{
				public static LocString NAME = UI.FormatAsLink("Radbolt Chamber", "HEPBATTERY");

				public static LocString DESC = "Particles packed up and ready to go.";

				public static LocString EFFECT = "Stores Radbolts in a high-energy state, ready for transport.\n\nRequires a " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + " to release radbolts from storage when the Radbolt threshold is reached.\n\nRadbolts in storage will rapidly decay while this building is disabled.";

				public static LocString LOGIC_PORT = "Do not emit Radbolts";

				public static LocString LOGIC_PORT_ACTIVE = UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + ": Emit Radbolts";

				public static LocString LOGIC_PORT_INACTIVE = UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby) + ": Do not emit Radbolts";

				public static LocString LOGIC_PORT_STORAGE = "Radbolt Storage";

				public static LocString LOGIC_PORT_STORAGE_ACTIVE = "Sends a " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + " when its Radbolt Storage is full";

				public static LocString LOGIC_PORT_STORAGE_INACTIVE = "Otherwise, sends a " + UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby);
			}

			public class HEPBRIDGETILE
			{
				public static LocString NAME = UI.FormatAsLink("Radbolt Joint Plate", "HEPBRIDGETILE");

				public static LocString DESC = "Allows Radbolts to pass through walls.";

				public static LocString EFFECT = string.Concat(new string[]
				{
					"Receives ",
					UI.FormatAsLink("Radbolts", "RADIATION"),
					" from ",
					UI.FormatAsLink("Radbolt Generators", "HIGHENERGYPARTICLESPAWNER"),
					" and directs them through walls. All other materials and elements will be blocked from passage."
				});
			}

			public class ASTRONAUTTRAININGCENTER
			{
				public static LocString NAME = UI.FormatAsLink("Space Cadet Centrifuge", "ASTRONAUTTRAININGCENTER");

				public static LocString DESC = "Duplicants must complete astronaut training in order to pilot space rockets.";

				public static LocString EFFECT = string.Concat(new string[]
				{
					"Trains Duplicants to become ",
					UI.FormatAsLink("Astronaut", "ROCKETPILOTING1"),
					".\n\nDuplicants must possess the ",
					UI.FormatAsLink("Astronaut", "ROCKETPILOTING1"),
					" trait to receive training."
				});
			}

			public class HOTTUB
			{
				public static LocString NAME = UI.FormatAsLink("Hot Tub", "HOTTUB");

				public static LocString DESC = "Relaxes Duplicants with massaging jets of heated liquid.";

				public static LocString EFFECT = string.Concat(new string[]
				{
					"Requires ",
					UI.FormatAsLink("Pipes", "LIQUIDPIPING"),
					" to and from tub and ",
					UI.FormatAsLink("Power", "POWER"),
					" to run the jets.\n\nWater must be a comfortable temperature and will cool rapidly.\n\nIncreases Duplicant ",
					UI.FormatAsLink("Morale", "MORALE"),
					" and leaves them feeling deliciously warm."
				});

				public static LocString WATER_REQUIREMENT = "{element}: {amount}";

				public static LocString WATER_REQUIREMENT_TOOLTIP = "This building must be filled with {amount} {element} in order to function.";

				public static LocString TEMPERATURE_REQUIREMENT = "Minimum {element} Temperature: {temperature}";

				public static LocString TEMPERATURE_REQUIREMENT_TOOLTIP = "The Hot Tub will only be usable if supplied with {temperature} {element}. If the {element} gets too cold, the Hot Tub will drain and require refilling with {element}.";
			}

			public class SODAFOUNTAIN
			{
				public static LocString NAME = UI.FormatAsLink("Soda Fountain", "SODAFOUNTAIN");

				public static LocString DESC = "Sparkling water puts a twinkle in a Duplicant's eye.";

				public static LocString EFFECT = string.Concat(new string[]
				{
					"Creates soda from ",
					UI.FormatAsLink("Water", "WATER"),
					" and ",
					UI.FormatAsLink("Carbon Dioxide", "CARBONDIOXIDE"),
					".\n\nConsuming soda water increases Duplicant ",
					UI.FormatAsLink("Morale", "MORALE"),
					"."
				});
			}

			public class UNCONSTRUCTEDROCKETMODULE
			{
				public static LocString NAME = "Empty Rocket Module";

				public static LocString DESC = "Something useful could be put here someday";

				public static LocString EFFECT = "Can be changed into a different rocket module";
			}

			public class MILKFATSEPARATOR
			{
				public static LocString NAME = UI.FormatAsLink("Brackwax Gleaner", "MILKFATSEPARATOR");

				public static LocString DESC = "Duplicants can slather up with brackwax to increase their travel speed in transit tubes.";

				public static LocString EFFECT = string.Concat(new string[]
				{
					"Refines ",
					ELEMENTS.MILK.NAME,
					" into ",
					ELEMENTS.BRINE.NAME,
					" and ",
					ELEMENTS.MILKFAT.NAME,
					", and emits ",
					ELEMENTS.CARBONDIOXIDE.NAME,
					"."
				});
			}

			public class MILKFEEDER
			{
				public static LocString NAME = UI.FormatAsLink("Critter Fountain", "MILKFEEDER");

				public static LocString DESC = "It's easier to tolerate overcrowding when you're all hopped up on brackene.";

				public static LocString EFFECT = string.Concat(new string[]
				{
					"Dispenses ",
					ELEMENTS.MILK.NAME,
					" to a wide variety of ",
					UI.CODEX.CATEGORYNAMES.CREATURES,
					".\n\nAccessing the fountain significantly improves ",
					UI.CODEX.CATEGORYNAMES.CREATURES,
					"' moods."
				});
			}

			public class MILKINGSTATION
			{
				public static LocString NAME = UI.FormatAsLink("Milking Station", "MILKINGSTATION");

				public static LocString DESC = "The harvested liquid is basically the equivalent of soda for critters.";

				public static LocString EFFECT = string.Concat(new string[]
				{
					"Allows Duplicants with the ",
					UI.FormatAsLink("Critter Ranching II", "RANCHING2"),
					" skill to milk ",
					UI.FormatAsLink("Gassy Moos", "MOO"),
					" for ",
					ELEMENTS.MILK.NAME,
					".\n\n",
					ELEMENTS.MILK.NAME,
					" can be used to refill the ",
					BUILDINGS.PREFABS.MILKFEEDER.NAME,
					"."
				});
			}

			public class MODULARLAUNCHPADPORT
			{
				public static LocString NAME = UI.FormatAsLink("Rocket Port", "MODULARLAUNCHPADPORTSOLID");

				public static LocString NAME_PLURAL = UI.FormatAsLink("Rocket Ports", "MODULARLAUNCHPADPORTSOLID");
			}

			public class MODULARLAUNCHPADPORTGAS
			{
				public static LocString NAME = UI.FormatAsLink("Gas Rocket Port Loader", "MODULARLAUNCHPADPORTGAS");

				public static LocString DESC = "Rockets must be landed to load or unload resources.";

				public static LocString EFFECT = string.Concat(new string[]
				{
					"Loads ",
					UI.FormatAsLink("Gases", "ELEMENTS_GAS"),
					" to the storage of a linked rocket.\n\nAutomatically links when built to the side of a ",
					BUILDINGS.PREFABS.LAUNCHPAD.NAME,
					" or another ",
					BUILDINGS.PREFABS.MODULARLAUNCHPADPORT.NAME,
					".\n\nUses the gas filters set on the rocket's cargo bays."
				});
			}

			public class MODULARLAUNCHPADPORTBRIDGE
			{
				public static LocString NAME = UI.FormatAsLink("Rocket Port Extension", "MODULARLAUNCHPADPORTBRIDGE");

				public static LocString DESC = "Allows rocket platforms to be built farther apart.";

				public static LocString EFFECT = string.Concat(new string[]
				{
					"Automatically links when built to the side of a ",
					BUILDINGS.PREFABS.LAUNCHPAD.NAME,
					" or any ",
					BUILDINGS.PREFABS.MODULARLAUNCHPADPORT.NAME,
					"."
				});
			}

			public class MODULARLAUNCHPADPORTLIQUID
			{
				public static LocString NAME = UI.FormatAsLink("Liquid Rocket Port Loader", "MODULARLAUNCHPADPORTLIQUID");

				public static LocString DESC = "Rockets must be landed to load or unload resources.";

				public static LocString EFFECT = string.Concat(new string[]
				{
					"Loads ",
					UI.FormatAsLink("Liquids", "ELEMENTS_LIQUID"),
					" to the storage of a linked rocket.\n\nAutomatically links when built to the side of a ",
					BUILDINGS.PREFABS.LAUNCHPAD.NAME,
					" or another ",
					BUILDINGS.PREFABS.MODULARLAUNCHPADPORT.NAME,
					".\n\nUses the liquid filters set on the rocket's cargo bays."
				});
			}

			public class MODULARLAUNCHPADPORTSOLID
			{
				public static LocString NAME = UI.FormatAsLink("Solid Rocket Port Loader", "MODULARLAUNCHPADPORTSOLID");

				public static LocString DESC = "Rockets must be landed to load or unload resources.";

				public static LocString EFFECT = string.Concat(new string[]
				{
					"Loads ",
					UI.FormatAsLink("Solids", "ELEMENTS_SOLID"),
					" to the storage of a linked rocket.\n\nAutomatically links when built to the side of a ",
					BUILDINGS.PREFABS.LAUNCHPAD.NAME,
					" or another ",
					BUILDINGS.PREFABS.MODULARLAUNCHPADPORT.NAME,
					".\n\nUses the solid material filters set on the rocket's cargo bays."
				});
			}

			public class MODULARLAUNCHPADPORTGASUNLOADER
			{
				public static LocString NAME = UI.FormatAsLink("Gas Rocket Port Unloader", "MODULARLAUNCHPADPORTGASUNLOADER");

				public static LocString DESC = "Rockets must be landed to load or unload resources.";

				public static LocString EFFECT = string.Concat(new string[]
				{
					"Unloads ",
					UI.FormatAsLink("Gases", "ELEMENTS_GAS"),
					" from the storage of a linked rocket.\n\nAutomatically links when built to the side of a ",
					BUILDINGS.PREFABS.LAUNCHPAD.NAME,
					" or another ",
					BUILDINGS.PREFABS.MODULARLAUNCHPADPORT.NAME,
					".\n\nUses the gas filters set on this unloader."
				});
			}

			public class MODULARLAUNCHPADPORTLIQUIDUNLOADER
			{
				public static LocString NAME = UI.FormatAsLink("Liquid Rocket Port Unloader", "MODULARLAUNCHPADPORTLIQUIDUNLOADER");

				public static LocString DESC = "Rockets must be landed to load or unload resources.";

				public static LocString EFFECT = string.Concat(new string[]
				{
					"Unloads ",
					UI.FormatAsLink("Liquids", "ELEMENTS_LIQUID"),
					" from the storage of a linked rocket.\n\nAutomatically links when built to the side of a ",
					BUILDINGS.PREFABS.LAUNCHPAD.NAME,
					" or another ",
					BUILDINGS.PREFABS.MODULARLAUNCHPADPORT.NAME,
					".\n\nUses the liquid filters set on this unloader."
				});
			}

			public class MODULARLAUNCHPADPORTSOLIDUNLOADER
			{
				public static LocString NAME = UI.FormatAsLink("Solid Rocket Port Unloader", "MODULARLAUNCHPADPORTSOLIDUNLOADER");

				public static LocString DESC = "Rockets must be landed to load or unload resources.";

				public static LocString EFFECT = string.Concat(new string[]
				{
					"Unloads ",
					UI.FormatAsLink("Solids", "ELEMENTS_SOLID"),
					" from the storage of a linked rocket.\n\nAutomatically links when built to the side of a ",
					BUILDINGS.PREFABS.LAUNCHPAD.NAME,
					" or another ",
					BUILDINGS.PREFABS.MODULARLAUNCHPADPORT.NAME,
					".\n\nUses the solid material filters set on this unloader."
				});
			}

			public class STICKERBOMB
			{
				public static LocString NAME = UI.FormatAsLink("Sticker Bomb", "STICKERBOMB");

				public static LocString DESC = "Surprise decor sneak attacks a Duplicant's gloomy day.";
			}

			public class HEATCOMPRESSOR
			{
				public static LocString NAME = UI.FormatAsLink("Liquid Heatquilizer", "HEATCOMPRESSOR");

				public static LocString DESC = "\"Room temperature\" is relative, really.";

				public static LocString EFFECT = string.Concat(new string[]
				{
					"Heats or cools ",
					UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID"),
					" to match ambient ",
					UI.FormatAsLink("Air Temperature", "HEAT"),
					"."
				});
			}

			public class PARTYCAKE
			{
				public static LocString NAME = UI.FormatAsLink("Triple Decker Cake", "PARTYCAKE");

				public static LocString DESC = "Any way you slice it, that's a good looking cake.";

				public static LocString EFFECT = string.Concat(new string[]
				{
					"Increases ",
					UI.FormatAsLink("Decor", "DECOR"),
					", contributing to ",
					UI.FormatAsLink("Morale", "MORALE"),
					".\n\nAdds a ",
					UI.FormatAsLink("Morale", "MORALE"),
					" bonus to Duplicants' parties."
				});
			}

			public class RAILGUN
			{
				public static LocString NAME = UI.FormatAsLink("Interplanetary Launcher", "RAILGUN");

				public static LocString DESC = "It's tempting to climb inside but trust me... don't.";

				public static LocString EFFECT = string.Concat(new string[]
				{
					"Launches ",
					UI.FormatAsLink("Interplanetary Payloads", "RAILGUNPAYLOAD"),
					" between Planetoids.\n\nPayloads can contain ",
					UI.FormatAsLink("Solid", "ELEMENTS_SOLID"),
					", ",
					UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID"),
					", or ",
					UI.FormatAsLink("Gas", "ELEMENTS_GAS"),
					" materials.\n\nCannot transport Duplicants."
				});

				public static LocString SIDESCREEN_HEP_REQUIRED = "Launch cost: {current} / {required} radbolts";

				public static LocString LOGIC_PORT = "Launch Toggle";

				public static LocString LOGIC_PORT_ACTIVE = UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + ": Enable payload launching.";

				public static LocString LOGIC_PORT_INACTIVE = UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby) + ": Disable payload launching.";
			}

			public class RAILGUNPAYLOADOPENER
			{
				public static LocString NAME = UI.FormatAsLink("Payload Opener", "RAILGUNPAYLOADOPENER");

				public static LocString DESC = "Payload openers can be hooked up to conveyors, plumbing and ventilation for improved sorting.";

				public static LocString EFFECT = string.Concat(new string[]
				{
					"Unpacks ",
					UI.FormatAsLink("Interplanetary Payloads", "RAILGUNPAYLOAD"),
					" delivered by Duplicants.\n\nAutomatically separates ",
					UI.FormatAsLink("Solid", "ELEMENTS_SOLID"),
					", ",
					UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID"),
					", and ",
					UI.FormatAsLink("Gas", "ELEMENTS_GAS"),
					" materials and distributes them to the appropriate systems."
				});
			}

			public class LANDINGBEACON
			{
				public static LocString NAME = UI.FormatAsLink("Targeting Beacon", "LANDINGBEACON");

				public static LocString DESC = "Microtarget where your " + UI.FormatAsLink("Interplanetary Payload", "RAILGUNPAYLOAD") + " lands on a Planetoid surface.";

				public static LocString EFFECT = string.Concat(new string[]
				{
					"Guides ",
					UI.FormatAsLink("Interplanetary Payloads", "RAILGUNPAYLOAD"),
					" and ",
					UI.FormatAsLink("Orbital Cargo Modules", "ORBITALCARGOMODULE"),
					" to land nearby.\n\n",
					UI.FormatAsLink("Interplanetary Payloads", "RAILGUNPAYLOAD"),
					" must be launched from a ",
					UI.FormatAsLink("Interplanetary Launcher", "RAILGUN"),
					"."
				});
			}

			public class DIAMONDPRESS
			{
				public static LocString NAME = UI.FormatAsLink("Diamond Press", "DIAMONDPRESS");

				public static LocString DESC = "Crushes refined carbon into diamond.";

				public static LocString EFFECT = string.Concat(new string[]
				{
					"Uses ",
					UI.FormatAsLink("Power", "POWER"),
					" and ",
					UI.FormatAsLink("Radbolts", "RADIATION"),
					" to crush ",
					UI.FormatAsLink("Refined Carbon", "REFINEDCARBON"),
					" into ",
					UI.FormatAsLink("Diamond", "DIAMOND"),
					".\n\nDuplicants will not fabricate items unless recipes are queued and ",
					UI.FormatAsLink("Refined Carbon", "REFINEDCARBON"),
					" has been discovered."
				});

				public static LocString REFINED_CARBON_RECIPE_DESCRIPTION = "Converts {1} to {0}";
			}

			public class ESCAPEPOD
			{
				public static LocString NAME = UI.FormatAsLink("Escape Pod", "ESCAPEPOD");

				public static LocString DESC = "Delivers a Duplicant from a stranded rocket to the nearest Planetoid.";
			}

			public class ROCKETINTERIORLIQUIDOUTPUTPORT
			{
				public static LocString NAME = UI.FormatAsLink("Liquid Spacefarer Output Port", "ROCKETINTERIORLIQUIDOUTPUTPORT");

				public static LocString DESC = "A direct attachment to the input port on the exterior of a rocket.";

				public static LocString EFFECT = "Allows a direct conduit connection into the " + UI.FormatAsLink("Spacefarer Module", "HABITATMODULEMEDIUM") + " of a rocket.";
			}

			public class ROCKETINTERIORLIQUIDINPUTPORT
			{
				public static LocString NAME = UI.FormatAsLink("Liquid Spacefarer Input Port", "ROCKETINTERIORLIQUIDINPUTPORT");

				public static LocString DESC = "A direct attachment to the output port on the exterior of a rocket.";

				public static LocString EFFECT = string.Concat(new string[]
				{
					"Allows a direct conduit connection out of the ",
					UI.FormatAsLink("Spacefarer Module", "HABITATMODULEMEDIUM"),
					" of a rocket.\nCan be used to vent ",
					UI.FormatAsLink("Liquids", "ELEMENTS_LIQUID"),
					" to space during flight."
				});
			}

			public class ROCKETINTERIORGASOUTPUTPORT
			{
				public static LocString NAME = UI.FormatAsLink("Gas Spacefarer Output Port", "ROCKETINTERIORGASOUTPUTPORT");

				public static LocString DESC = "A direct attachment to the input port on the exterior of a rocket.";

				public static LocString EFFECT = "Allows a direct conduit connection into the " + UI.FormatAsLink("Spacefarer Module", "HABITATMODULEMEDIUM") + " of a rocket.";
			}

			public class ROCKETINTERIORGASINPUTPORT
			{
				public static LocString NAME = UI.FormatAsLink("Gas Spacefarer Input Port", "ROCKETINTERIORGASINPUTPORT");

				public static LocString DESC = "A direct attachment leading to the output port on the exterior of the rocket.";

				public static LocString EFFECT = string.Concat(new string[]
				{
					"Allows a direct conduit connection out of the ",
					UI.FormatAsLink("Spacefarer Module", "HABITATMODULEMEDIUM"),
					" of the rocket.\nCan be used to vent ",
					UI.FormatAsLink("Gasses", "ELEMENTS_GAS"),
					" to space during flight."
				});
			}

			public class MISSILELAUNCHER
			{
				public static LocString NAME = UI.FormatAsLink("Meteor Blaster", "MISSILELAUNCHER");

				public static LocString DESC = "Some meteors drop harvestable resources when they're blown to smithereens.";

				public static LocString EFFECT = "Fires explosive projectiles at incoming space objects to defend the colony from impact-related damage.\n\nProjectiles must be crafted at a " + UI.FormatAsLink("Blastshot Maker", "MISSILEFABRICATOR") + ".\n\nRange: 16 tiles horizontally, 32 tiles vertically.";

				public static LocString TARGET_SELECTION_HEADER = "Short Range Target Selection";

				public class BODY
				{
					public static LocString CONTAINER1 = "Fires " + UI.FormatAsLink("Blastshot", "MISSILELAUNCHER") + " shells at meteor showers to defend the colony from impact-related damage.\n\nRange: 16 tiles horizontally, 32 tiles vertically.\n\nMeteors that have been blown to smithereens leave behind no harvestable resources.";
				}
			}

			public class CRITTERCONDO
			{
				public static LocString NAME = UI.FormatAsLink("Critter Condo", "CRITTERCONDO");

				public static LocString DESC = "It's nice to have nice things.";

				public static LocString EFFECT = "Provides a comfortable lounge area that boosts " + UI.FormatAsLink("Critter", "CREATURES") + " happiness.";
			}

			public class UNDERWATERCRITTERCONDO
			{
				public static LocString NAME = UI.FormatAsLink("Water Fort", "UNDERWATERCRITTERCONDO");

				public static LocString DESC = "Even wild critters are happier after they've had a little R&R.";

				public static LocString EFFECT = string.Concat(new string[]
				{
					"A fancy respite area for adult ",
					UI.FormatAsLink("Pokeshells", "CRABSPECIES"),
					" and ",
					UI.FormatAsLink("Pacu", "PACUSPECIES"),
					"."
				});
			}

			public class AIRBORNECRITTERCONDO
			{
				public static LocString NAME = UI.FormatAsLink("Airborne Critter Condo", "AIRBORNECRITTERCONDO");

				public static LocString DESC = "Triggers natural nesting instincts and improves critters' moods.";

				public static LocString EFFECT = string.Concat(new string[]
				{
					"A hanging respite area for adult ",
					UI.FormatAsLink("Pufts", "PUFT"),
					", ",
					UI.FormatAsLink("Gassy Moos", "MOOSPECIES"),
					" and ",
					UI.FormatAsLink("Shine Bugs", "LIGHTBUG"),
					"."
				});
			}

			public class MASSIVEHEATSINK
			{
				public static LocString NAME = UI.FormatAsLink("Anti Entropy Thermo-Nullifier", "MASSIVEHEATSINK");

				public static LocString DESC = "";

				public static LocString EFFECT = string.Concat(new string[]
				{
					"A self-sustaining machine powered by what appears to be refined ",
					UI.FormatAsLink("Neutronium", "UNOBTANIUM"),
					".\n\nAbsorbs and neutralizes ",
					UI.FormatAsLink("Heat", "HEAT"),
					" energy when provided with piped ",
					UI.FormatAsLink("Hydrogen Gas", "HYDROGEN"),
					"."
				});
			}

			public class MEGABRAINTANK
			{
				public static LocString NAME = UI.FormatAsLink("Somnium Synthesizer", "MEGABRAINTANK");

				public static LocString DESC = "";

				public static LocString EFFECT = string.Concat(new string[]
				{
					"An organic multi-cortex repository and processing system fuelled by ",
					UI.FormatAsLink("Oxygen", "OXYGEN"),
					".\n\nAnalyzes ",
					UI.FormatAsLink("Dream Journals", "DREAMJOURNAL"),
					" produced by Duplicants wearing ",
					UI.FormatAsLink("Pajamas", "SLEEP_CLINIC_PAJAMAS"),
					".\n\nProvides a sustainable boost to Duplicant skills and abilities throughout the colony."
				});
			}

			public class GRAVITASCREATUREMANIPULATOR
			{
				public static LocString NAME = UI.FormatAsLink("Critter Flux-O-Matic", "GRAVITASCREATUREMANIPULATOR");

				public static LocString DESC = "";

				public static LocString EFFECT = "An experimental DNA manipulator.\n\nAnalyzes " + UI.FormatAsLink("Critters", "CREATURES") + " to transform base morphs into random variants of their species.";
			}

			public class FACILITYBACKWALLWINDOW
			{
				public static LocString NAME = "Window";

				public static LocString DESC = "";

				public static LocString EFFECT = "A tall, thin window.";
			}

			public class POIBUNKEREXTERIORDOOR
			{
				public static LocString NAME = "Security Door";

				public static LocString EFFECT = "A strong door with a sophisticated genetic lock.";

				public static LocString DESC = "";
			}

			public class POIDOORINTERNAL
			{
				public static LocString NAME = "Security Door";

				public static LocString EFFECT = "A strong door with a sophisticated genetic lock.";

				public static LocString DESC = "";
			}

			public class POIFACILITYDOOR
			{
				public static LocString NAME = "Lobby Doors";

				public static LocString EFFECT = "Large double doors that were once the main entrance to a large facility.";

				public static LocString DESC = "";
			}

			public class POIDLC2SHOWROOMDOOR
			{
				public static LocString NAME = "Showroom Doors";

				public static LocString EFFECT = "Large double doors identical to those you might find at the main entrance to a large facility.";

				public static LocString DESC = "";
			}

			public class VENDINGMACHINE
			{
				public static LocString NAME = "Vending Machine";

				public static LocString DESC = "A pristine " + UI.FormatAsLink("Nutrient Bar", "FIELDRATION") + " dispenser.";
			}

			public class GENESHUFFLER
			{
				public static LocString NAME = "Neural Vacillator";

				public static LocString DESC = "A massive synthetic brain, suspended in saline solution.\n\nThere is a chair attached to the device with room for one person.";
			}

			public class PROPTALLPLANT
			{
				public static LocString NAME = "Potted Plant";

				public static LocString DESC = "Looking closely, it appears to be fake.";
			}

			public class PROPTABLE
			{
				public static LocString NAME = "Table";

				public static LocString DESC = "A table and some chairs.";
			}

			public class PROPDESK
			{
				public static LocString NAME = "Computer Desk";

				public static LocString DESC = "An intact office desk, decorated with several personal belongings and a barely functioning computer.";
			}

			public class PROPFACILITYCHAIR
			{
				public static LocString NAME = "Lobby Chair";

				public static LocString DESC = "A chair where visitors can comfortably wait before their appointments.";
			}

			public class PROPFACILITYCOUCH
			{
				public static LocString NAME = "Lobby Couch";

				public static LocString DESC = "A couch where visitors can comfortably wait before their appointments.";
			}

			public class PROPFACILITYDESK
			{
				public static LocString NAME = "Director's Desk";

				public static LocString DESC = "A spotless desk filled with impeccably organized office supplies.\n\nA photo peeks out from beneath the desk pad, depicting two beaming young women in caps and gowns.\n\nThe photo is quite old.";
			}

			public class PROPFACILITYTABLE
			{
				public static LocString NAME = "Coffee Table";

				public static LocString DESC = "A low coffee table that may have once held old science magazines.";
			}

			public class PROPFACILITYSTATUE
			{
				public static LocString NAME = "Gravitas Monument";

				public static LocString DESC = "A large, modern sculpture that sits in the center of the lobby.\n\nIt's an artistic cross between an hourglass shape and a double helix.";
			}

			public class PROPFACILITYCHANDELIER
			{
				public static LocString NAME = "Chandelier";

				public static LocString DESC = "A large chandelier that hangs from the ceiling.\n\nIt does not appear to function.";
			}

			public class PROPFACILITYGLOBEDROORS
			{
				public static LocString NAME = "Filing Cabinet";

				public static LocString DESC = "A filing cabinet for storing hard copy employee records.\n\nThe contents have been shredded.";
			}

			public class PROPFACILITYDISPLAY1
			{
				public static LocString NAME = "Electronic Display";

				public static LocString DESC = "An electronic display projecting the blueprint of a familiar device.\n\nIt looks like a Printing Pod.";
			}

			public class PROPFACILITYDISPLAY2
			{
				public static LocString NAME = "Electronic Display";

				public static LocString DESC = "An electronic display projecting the blueprint of a familiar device.\n\nIt looks like a Mining Gun.";
			}

			public class PROPFACILITYDISPLAY3
			{
				public static LocString NAME = "Electronic Display";

				public static LocString DESC = "An electronic display projecting the blueprint of a strange device.\n\nPerhaps these displays were used to entice visitors.";
			}

			public class PROPFACILITYTALLPLANT
			{
				public static LocString NAME = "Office Plant";

				public static LocString DESC = "It's survived the vacuum of space by virtue of being plastic.";
			}

			public class PROPFACILITYLAMP
			{
				public static LocString NAME = "Light Fixture";

				public static LocString DESC = "A long light fixture that hangs from the ceiling.\n\nIt does not appear to function.";
			}

			public class PROPFACILITYWALLDEGREE
			{
				public static LocString NAME = "Doctorate Degree";

				public static LocString DESC = "Certification in Applied Physics, awarded in recognition of one \"Jacquelyn A. Stern\".";
			}

			public class PROPFACILITYPAINTING
			{
				public static LocString NAME = "Landscape Portrait";

				public static LocString DESC = "A painting featuring a copse of fir trees and a magnificent mountain range on the horizon.\n\nThe air in the room prickles with the sensation that I'm not meant to be here.";
			}

			public class PROPRECEPTIONDESK
			{
				public static LocString NAME = "Reception Desk";

				public static LocString DESC = "A full coffee cup and a note abandoned mid sentence sit behind the desk.\n\nIt gives me an eerie feeling, as if the receptionist has stepped out and will return any moment.";
			}

			public class PROPELEVATOR
			{
				public static LocString NAME = "Broken Elevator";

				public static LocString DESC = "Out of service.\n\nThe buttons inside indicate it went down more than a dozen floors at one point in time.";
			}

			public class SETLOCKER
			{
				public static LocString NAME = "Locker";

				public static LocString DESC = "A basic metal locker.\n\nIt contains an assortment of personal effects.";
			}

			public class PROPEXOSETLOCKER
			{
				public static LocString NAME = "Off-site Locker";

				public static LocString DESC = "A locker made with ultra-lightweight textiles.\n\nIt contains an assortment of personal effects.";
			}

			public class MISSILESETLOCKER
			{
				public static LocString NAME = "Explosives Locker";

				public static LocString DESC = "A locker that once belonged to an explosives engineer.\n\nThere's an " + UI.FormatAsLink("Intracosmic Blastshot", "MISSILELAUNCHER") + " in it.";
			}

			public class PROPGRAVITASSMALLSEEDLOCKER
			{
				public static LocString NAME = "Wall Cabinet";

				public static LocString DESC = "A small glass cabinet.\n\nThere's a biohazard symbol on it.";
			}

			public class PROPLIGHT
			{
				public static LocString NAME = "Light Fixture";

				public static LocString DESC = "An elegant ceiling lamp, slightly worse for wear.";
			}

			public class PROPLADDER
			{
				public static LocString NAME = "Ladder";

				public static LocString DESC = "A hard plastic ladder.";
			}

			public class PROPSKELETON
			{
				public static LocString NAME = "Model Skeleton";

				public static LocString DESC = "A detailed anatomical model.\n\nIt appears to be made of resin.";
			}

			public class PROPSURFACESATELLITE1
			{
				public static LocString NAME = "Crashed Satellite";

				public static LocString DESC = "All that remains of a once peacefully orbiting satellite.";
			}

			public class PROPSURFACESATELLITE2
			{
				public static LocString NAME = "Wrecked Satellite";

				public static LocString DESC = "All that remains of a once peacefully orbiting satellite.";
			}

			public class PROPSURFACESATELLITE3
			{
				public static LocString NAME = "Crushed Satellite";

				public static LocString DESC = "All that remains of a once peacefully orbiting satellite.";
			}

			public class PROPCLOCK
			{
				public static LocString NAME = "Clock";

				public static LocString DESC = "A simple wall clock.\n\nIt is no longer ticking.";
			}

			public class PROPGRAVITASDECORATIVEWINDOW
			{
				public static LocString NAME = "Window";

				public static LocString DESC = "A tall, thin window which once pointed to a courtyard.";
			}

			public class PROPGRAVITASLABWINDOW
			{
				public static LocString NAME = "Lab Window";

				public static LocString DESC = "";

				public static LocString EFFECT = "A lab window. Formerly a portal to the outside world.";
			}

			public class PROPGRAVITASLABWINDOWHORIZONTAL
			{
				public static LocString NAME = "Lab Window";

				public static LocString DESC = "";

				public static LocString EFFECT = "A lab window.\n\nSomeone once stared out of this, contemplating the results of an experiment.";
			}

			public class PROPGRAVITASLABWALL
			{
				public static LocString NAME = "Lab Wall";

				public static LocString DESC = "";

				public static LocString EFFECT = "A regular wall that once existed in a working lab.";
			}

			public class GRAVITASCONTAINER
			{
				public static LocString NAME = "Pajama Cubby";

				public static LocString DESC = "";

				public static LocString EFFECT = "A clothing storage unit.\n\nIt contains ultra-soft sleepwear.";
			}

			public class GRAVITASLABLIGHT
			{
				public static LocString NAME = "LED Light";

				public static LocString DESC = "";

				public static LocString EFFECT = "An overhead light therapy lamp designed to soothe the minds.";
			}

			public class GRAVITASDOOR
			{
				public static LocString NAME = "Gravitas Door";

				public static LocString DESC = "";

				public static LocString EFFECT = "An office door to an office that no longer exists.";
			}

			public class PROPGRAVITASWALL
			{
				public static LocString NAME = "Wall";

				public static LocString DESC = "";

				public static LocString EFFECT = "The wall of a once-great scientific facility.";
			}

			public class PROPGRAVITASWALLPURPLE
			{
				public static LocString NAME = "Wall";

				public static LocString DESC = "";

				public static LocString EFFECT = "The wall of an ambitious research and development department.";
			}

			public class PROPGRAVITASWALLPURPLEWHITEDIAGONAL
			{
				public static LocString NAME = "Wall";

				public static LocString DESC = "";

				public static LocString EFFECT = "The wall of an ambitious research and development department.";
			}

			public class PROPGRAVITASDISPLAY4
			{
				public static LocString NAME = "Electronic Display";

				public static LocString DESC = "An electronic display projecting the blueprint of a robotic device.\n\nIt looks like a ceiling robot.";
			}

			public class PROPDLC2DISPLAY1
			{
				public static LocString NAME = "Electronic Display";

				public static LocString DESC = "An electronic display projecting the blueprint of an engineering project.\n\nIt looks like a pump of some kind.";
			}

			public class PROPGRAVITASCEILINGROBOT
			{
				public static LocString NAME = "Ceiling Robot";

				public static LocString DESC = "Non-functioning robotic arms that once assisted lab technicians.";
			}

			public class PROPGRAVITASFLOORROBOT
			{
				public static LocString NAME = "Robotic Arm";

				public static LocString DESC = "The grasping robotic claw designed to assist technicians in a lab.";
			}

			public class PROPGRAVITASJAR1
			{
				public static LocString NAME = "Big Brain Jar";

				public static LocString DESC = "An abnormally large brain floating in embalming liquid to prevent decomposition.";
			}

			public class PROPGRAVITASCREATUREPOSTER
			{
				public static LocString NAME = "Anatomy Poster";

				public static LocString DESC = "An anatomical illustration of the very first " + UI.FormatAsLink("Hatch", "HATCH") + " ever produced.\n\nWhile the ratio of egg sac to brain may appear outlandish, it is in fact to scale.";
			}

			public class PROPGRAVITASDESKPODIUM
			{
				public static LocString NAME = "Computer Podium";

				public static LocString DESC = "A clutter-proof desk to minimize distractions.\n\nThere appears to be something stored in the computer.";
			}

			public class PROPGRAVITASFIRSTAIDKIT
			{
				public static LocString NAME = "First Aid Kit";

				public static LocString DESC = "It looks like it's been used a lot.";
			}

			public class PROPGRAVITASHANDSCANNER
			{
				public static LocString NAME = "Hand Scanner";

				public static LocString DESC = "A sophisticated security device.\n\nIt appears to use a method other than fingerprints to verify an individual's identity.";
			}

			public class PROPGRAVITASLABTABLE
			{
				public static LocString NAME = "Lab Desk";

				public static LocString DESC = "The quaint research desk of a departed lab technician.\n\nPerhaps the computer stores something of interest.";
			}

			public class PROPGRAVITASROBTICTABLE
			{
				public static LocString NAME = "Robotics Research Desk";

				public static LocString DESC = "The work space of an extinct robotics technician who left behind some unfinished prototypes.";
			}

			public class PROPDLC2GEOTHERMALCART
			{
				public static LocString NAME = "Service Cart";

				public static LocString DESC = "Maintenance equipment that once flushed debris out of complex mechanisms.\n\nOne of the wheels is squeaky.";
			}

			public class PROPGRAVITASSHELF
			{
				public static LocString NAME = "Shelf";

				public static LocString DESC = "A shelf holding jars just out of reach for a short person.";
			}

			public class PROPGRAVITASTOOLSHELF
			{
				public static LocString NAME = "Tool Rack";

				public static LocString DESC = "A wall-mounted rack for storing and displaying useful tools at a not-so-useful height.";
			}

			public class PROPGRAVITASTOOLCRATE
			{
				public static LocString NAME = "Tool Crate";

				public static LocString DESC = "A packing crate intended for safety equipment.\n\nIt has been repurposed for tool storage.";
			}

			public class PROPGRAVITASFIREEXTINGUISHER
			{
				public static LocString NAME = "Broken Fire Extinguisher";

				public static LocString DESC = "Essential lab equipment.\n\nThe inspection tag indicates it has long expired.";
			}

			public class PROPGRAVITASJAR2
			{
				public static LocString NAME = "Sample Jar";

				public static LocString DESC = "The corpse of a proto-hatch creature meticulously preserved in a jar.";
			}

			public class PROPEXOSHELFLONG
			{
				public static LocString NAME = "Long Prefab Shelf";

				public static LocString DESC = "A shelf made out of flat-packed pieces that can be assembled in various ways.\n\nThis is the long way.";
			}

			public class PROPEXOSHELSHORT
			{
				public static LocString NAME = "Prefab Shelf";

				public static LocString DESC = "A shelf made out of flat-packed pieces that can be assembled in various ways.\n\nIt looks nice, actually.";
			}

			public class PROPHUMANMURPHYBED
			{
				public static LocString NAME = "Murphy Bed";

				public static LocString DESC = "A bed that folds into the wall, for small live/work spaces.\n\nThis is the display model.";
			}

			public class PROPHUMANCHESTERFIELDSOFA
			{
				public static LocString NAME = "Showroom Couch";

				public static LocString DESC = "A luxurious couch where potential residents can comfortably nap and dream of home.";
			}

			public class PROPHUMANCHESTERFIELDCHAIR
			{
				public static LocString NAME = "Showroom Chair";

				public static LocString DESC = "A luxurious chair where future generations can comfortably sit and dream of home.";
			}

			public class WARPCONDUITRECEIVER
			{
				public static LocString NAME = "Supply Teleporter Output";

				public static LocString DESC = "The tubes at the back disappear into nowhere.";

				public static LocString EFFECT = string.Concat(new string[]
				{
					"A machine capable of teleporting ",
					UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID"),
					", ",
					UI.FormatAsLink("Solid", "ELEMENTS_SOLID"),
					", and ",
					UI.FormatAsLink("Gas", "ELEMENTS_GAS"),
					" resources to another asteroid.\n\nIt can be activated by a Duplicant with the ",
					UI.FormatAsLink("Field Research", "RESEARCHING2"),
					" skill.\n\nThis is the receiving side."
				});
			}

			public class WARPCONDUITSENDER
			{
				public static LocString NAME = "Supply Teleporter Input";

				public static LocString DESC = "The tubes at the back disappear into nowhere.";

				public static LocString EFFECT = string.Concat(new string[]
				{
					"A machine capable of teleporting ",
					UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID"),
					", ",
					UI.FormatAsLink("Solid", "ELEMENTS_SOLID"),
					", and ",
					UI.FormatAsLink("Gas", "ELEMENTS_GAS"),
					" resources to another asteroid.\n\nIt can be activated by a Duplicant with the ",
					UI.FormatAsLink("Field Research", "RESEARCHING2"),
					" skill.\n\nThis is the transmitting side."
				});
			}

			public class WARPPORTAL
			{
				public static LocString NAME = "Teleporter Transmitter";

				public static LocString DESC = "The functional remnants of an intricate teleportation system.\n\nThis is the outgoing side, and has one pre-programmed destination.";
			}

			public class WARPRECEIVER
			{
				public static LocString NAME = "Teleporter Receiver";

				public static LocString DESC = "The functional remnants of an intricate teleportation system.\n\nThis is the incoming side.";
			}

			public class TEMPORALTEAROPENER
			{
				public static LocString NAME = "Temporal Tear Opener";

				public static LocString DESC = "Infinite possibilities, with a complimentary side of meteor showers.";

				public static LocString EFFECT = "A powerful mechanism capable of tearing through the fabric of reality.";

				public class SIDESCREEN
				{
					public static LocString TEXT = "Fire!";

					public static LocString TOOLTIP = "The big red button.";
				}
			}

			public class LONELYMINIONHOUSE
			{
				public static LocString NAME = UI.FormatAsLink("Gravitas Shipping Container", "LONELYMINIONHOUSE");

				public static LocString DESC = "Its occupant has been alone for so long, he's forgotten what friendship feels like.";

				public static LocString EFFECT = "A large transport unit from the facility's sub-sub-basement.\n\nIt has been modified into a crude yet functional temporary shelter.";
			}

			public class LONELYMINIONHOUSE_COMPLETE
			{
				public static LocString NAME = UI.FormatAsLink("Gravitas Shipping Container", "LONELYMINIONHOUSE_COMPLETE");

				public static LocString DESC = "Someone lived inside it for a while.";

				public static LocString EFFECT = "A super-spacious container for the " + UI.FormatAsLink("Solid Materials", "ELEMENTS_SOLID") + " of your choosing.";
			}

			public class LONELYMAILBOX
			{
				public static LocString NAME = "Mailbox";

				public static LocString DESC = "There's nothing quite like receiving homemade gifts in the mail.";

				public static LocString EFFECT = "Displays a single edible object.";
			}

			public class PLASTICFLOWERS
			{
				public static LocString NAME = "Plastic Flowers";

				public static LocString DESCRIPTION = "Maintenance-free blooms that will outlive us all.";

				public static LocString LORE_DLC2 = "Manufactured by Home Staging Heroes Ltd. as commissioned by the Gravitas Facility, to <i>\"Make Space Feel More Like Home.\"</i>\n\nThis bouquet is designed to smell like freshly baked cookies.";
			}

			public class FOUNTAINPEN
			{
				public static LocString NAME = "Fountain Pen";

				public static LocString DESCRIPTION = "Cuts through red tape better than a sword ever could.";

				public static LocString LORE_DLC2 = "The handcrafted gold nib features a triangular logo with the letters V and I inside.\n\nIts owner was too proud to report it stolen, and would be shocked to learn of its whereabouts.";
			}

			public class PROPCLOTHESHANGER
			{
				public static LocString NAME = "Coat Rack";

				public static LocString DESC = "Holds one " + EQUIPMENT.PREFABS.WARM_VEST.NAME + ".\n\nIt'd be silly not to use it.";
			}

			public class PROPCERESPOSTERA
			{
				public static LocString NAME = "Travel Poster";

				public static LocString DESC = "A poster promoting a local tourist attraction.\n\nActual scenery may vary.";
			}

			public class PROPCERESPOSTERB
			{
				public static LocString NAME = "Travel Poster";

				public static LocString DESC = "A poster promoting local wildlife.\n\nThe first in an unfinished series.";
			}

			public class PROPCERESPOSTERLARGE
			{
				public static LocString NAME = "Acoustic Art Panel";

				public static LocString DESC = "A sound-absorbing panel that makes small-space living more bearable.\n\nThe artwork features a  power source.";
			}

			public class CHLORINATOR
			{
				public static LocString NAME = UI.FormatAsLink("Bleach Stone Hopper", "CHLORINATOR");

				public static LocString DESC = "Bleach stone is useful for sanitation and geotuning.";

				public static LocString EFFECT = string.Concat(new string[]
				{
					"Uses ",
					ELEMENTS.SALT.NAME,
					" and ",
					ELEMENTS.GOLD.NAME,
					" to produce ",
					ELEMENTS.BLEACHSTONE.NAME,
					"."
				});
			}

			public class MILKPRESS
			{
				public static LocString NAME = UI.FormatAsLink("Plant Pulverizer", "MILKPRESS");

				public static LocString DESC = "For Duplicants who are too squeamish to milk critters.";

				public static LocString EFFECT = string.Concat(new string[]
				{
					"Crushes organic materials to extract liquids such as ",
					ELEMENTS.MILK.NAME,
					" or ",
					ELEMENTS.PHYTOOIL.NAME,
					".\n\n",
					ELEMENTS.MILK.NAME,
					" can be used to refill the ",
					BUILDINGS.PREFABS.MILKFEEDER.NAME,
					"."
				});

				public static LocString WHEAT_MILK_RECIPE_DESCRIPTION = "Converts {0} to {1}";

				public static LocString VINEFRUIT_JAM_RECIPE_DESCRIPTION = "Converts {0} to {1}";

				public static LocString NUT_MILK_RECIPE_DESCRIPTION = "Converts {0} to {1}";

				public static LocString PHYTO_OIL_RECIPE_DESCRIPTION = "Converts {0} to {1} and {2}";

				public static LocString KELP_TO_PHYTO_OIL_RECIPE_DESCRIPTION = "Converts {0} to {1}";

				public static LocString DEWDRIPPER_MILK_RECIPE_DESCRIPTION = "Converts {0} to {1}";

				public static LocString RESIN_FROM_AMBER_RECIPE_DESCRIPTION = "Converts {0} into {1}, {2}, and a small amount of {3}";
			}

			public class FOODDEHYDRATOR
			{
				public static LocString NAME = UI.FormatAsLink("Dehydrator", "FOODDEHYDRATOR");

				public static LocString DESC = "Some of the eliminated liquid inevitably ends up on the floor.";

				public static LocString EFFECT = string.Concat(new string[]
				{
					"Uses low, even heat to eliminate moisture from eligible ",
					UI.FormatAsLink("Foods", "FOOD"),
					" and render them shelf-stable.\n\nDehydrated meals must be processed at the ",
					UI.FormatAsLink("Rehydrator", "FOODREHYDRATOR"),
					" before they can be eaten."
				});

				public static LocString RECIPE_NAME = "Dried {0}";

				public static LocString RESULT_DESCRIPTION = "Dehydrated portions of {0} do not require refrigeration.";
			}

			public class FOODREHYDRATOR
			{
				public static LocString NAME = UI.FormatAsLink("Rehydrator", "FOODREHYDRATOR");

				public static LocString DESC = "Rehydrated food is nutritious and only slightly less delicious.";

				public static LocString EFFECT = "Restores moisture to convert shelf-stable packaged meals into edible " + UI.FormatAsLink("Food", "FOOD") + ".";
			}

			public class GEOTHERMALCONTROLLER
			{
				public static LocString NAME = UI.FormatAsLink("Geothermal Heat Pump", "GEOTHERMALCONTROLLER");

				public static LocString DESC = "What comes out depends very much on the initial temperature of what goes in.";

				public static LocString EFFECT = string.Concat(new string[]
				{
					"Uses ",
					UI.FormatAsLink("Heat", "HEAT"),
					" from the planet's core to dramatically increase the temperature of ",
					UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID"),
					" inputs.\n\nMaterials will be emitted at connected Geo Vents."
				});
			}

			public class GEOTHERMALVENT
			{
				public static LocString NAME = UI.FormatAsLink("Geo Vent", "GEOTHERMALVENT");

				public static LocString NAME_FMT = UI.FormatAsLink("Geo Vent C-{ID}", "GEOTHERMALVENT");

				public static LocString DESC = "Geo vents must finish their current emission before accepting new materials.";

				public static LocString EFFECT = "Emits high-" + UI.FormatAsLink("temperature", "HEAT") + " materials received from the Geothermal Heat Pump.";

				public static LocString BLOCKED_DESC = string.Concat(new string[]
				{
					"Blocked geo vents can be cleared by pumping in ",
					UI.FormatAsLink("liquids", "ELEMENTS_LIQUID"),
					" that are hot enough to melt ",
					UI.FormatAsLink("Lead", "LEAD"),
					"."
				});

				public static LocString LOGIC_PORT = "Material Content Monitor";

				public static LocString LOGIC_PORT_ACTIVE = "Sends a " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + " when geo vent has materials to emit";

				public static LocString LOGIC_PORT_INACTIVE = "Otherwise, sends a " + UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby);
			}
		}

		public static class DAMAGESOURCES
		{
			public static LocString NOTIFICATION_TOOLTIP = "A {0} sustained damage from {1}";

			public static LocString CONDUIT_CONTENTS_FROZE = "pipe contents becoming too cold";

			public static LocString CONDUIT_CONTENTS_BOILED = "pipe contents becoming too hot";

			public static LocString BUILDING_OVERHEATED = "overheating";

			public static LocString CORROSIVE_ELEMENT = "corrosive element";

			public static LocString BAD_INPUT_ELEMENT = "receiving an incorrect substance";

			public static LocString MINION_DESTRUCTION = "an angry Duplicant. Rude!";

			public static LocString LIQUID_PRESSURE = "neighboring liquid pressure";

			public static LocString CIRCUIT_OVERLOADED = "an overloaded circuit";

			public static LocString LOGIC_CIRCUIT_OVERLOADED = "an overloaded logic circuit";

			public static LocString MICROMETEORITE = "micrometeorite";

			public static LocString COMET = "falling space rocks";

			public static LocString ROCKET = "rocket engine";
		}

		public static class AUTODISINFECTABLE
		{
			public static class ENABLE_AUTODISINFECT
			{
				public static LocString NAME = "Enable Disinfect";

				public static LocString TOOLTIP = "Automatically disinfect this building when it becomes contaminated";
			}

			public static class DISABLE_AUTODISINFECT
			{
				public static LocString NAME = "Disable Disinfect";

				public static LocString TOOLTIP = "Do not automatically disinfect this building";
			}

			public static class NO_DISEASE
			{
				public static LocString TOOLTIP = "This building is clean";
			}
		}

		public static class DISINFECTABLE
		{
			public static class ENABLE_DISINFECT
			{
				public static LocString NAME = "Disinfect";

				public static LocString TOOLTIP = "Mark this building for disinfection";
			}

			public static class DISABLE_DISINFECT
			{
				public static LocString NAME = "Cancel Disinfect";

				public static LocString TOOLTIP = "Cancel this disinfect order";
			}

			public static class NO_DISEASE
			{
				public static LocString TOOLTIP = "This building is already clean";
			}
		}

		public static class REPAIRABLE
		{
			public static class ENABLE_AUTOREPAIR
			{
				public static LocString NAME = "Enable Autorepair";

				public static LocString TOOLTIP = "Automatically repair this building when damaged";
			}

			public static class DISABLE_AUTOREPAIR
			{
				public static LocString NAME = "Disable Autorepair";

				public static LocString TOOLTIP = "Only repair this building when ordered";
			}
		}

		public static class AUTOMATABLE
		{
			public static class ENABLE_AUTOMATIONONLY
			{
				public static LocString NAME = "Disable Manual";

				public static LocString TOOLTIP = "This building's storage may be accessed by Auto-Sweepers only\n\nDuplicants will not be permitted to add or remove materials from this building";
			}

			public static class DISABLE_AUTOMATIONONLY
			{
				public static LocString NAME = "Enable Manual";

				public static LocString TOOLTIP = "This building's storage may be accessed by both Duplicants and Auto-Sweeper buildings";
			}
		}
	}
}
