using System;

namespace STRINGS
{
	// Token: 0x020034A1 RID: 13473
	public class RESEARCH
	{
		// Token: 0x020034A2 RID: 13474
		public class MESSAGING
		{
			// Token: 0x0400D307 RID: 54023
			public static LocString NORESEARCHSELECTED = "No research selected";

			// Token: 0x0400D308 RID: 54024
			public static LocString RESEARCHTYPEREQUIRED = "{0} required";

			// Token: 0x0400D309 RID: 54025
			public static LocString RESEARCHTYPEALSOREQUIRED = "{0} also required";

			// Token: 0x0400D30A RID: 54026
			public static LocString NO_RESEARCHER_SKILL = "No Researchers assigned";

			// Token: 0x0400D30B RID: 54027
			public static LocString NO_RESEARCHER_SKILL_TOOLTIP = "The selected research focus requires {ResearchType} to complete\n\nOpen the " + UI.FormatAsManagementMenu("Skills Panel", global::Action.ManageSkills) + " and teach a Duplicant the {ResearchType} Skill to use this building";

			// Token: 0x0400D30C RID: 54028
			public static LocString MISSING_RESEARCH_STATION = "Missing Research Station";

			// Token: 0x0400D30D RID: 54029
			public static LocString MISSING_RESEARCH_STATION_TOOLTIP = "The selected research focus requires a {0} to perform\n\nOpen the " + UI.FormatAsBuildMenuTab("Stations Tab", global::Action.Plan10) + " of the Build Menu to construct one";

			// Token: 0x020034A3 RID: 13475
			public static class DLC
			{
				// Token: 0x0400D30E RID: 54030
				public static LocString EXPANSION1 = string.Concat(new string[]
				{
					UI.PRE_KEYWORD,
					"\n\n<i>",
					UI.DLC1.NAME,
					"</i>",
					UI.PST_KEYWORD,
					" DLC Content"
				});

				// Token: 0x0400D30F RID: 54031
				public static LocString DLC_CONTENT = "\n<i>{0}</i> DLC Content";
			}
		}

		// Token: 0x020034A4 RID: 13476
		public class TYPES
		{
			// Token: 0x0400D310 RID: 54032
			public static LocString MISSINGRECIPEDESC = "Missing Recipe Description";

			// Token: 0x020034A5 RID: 13477
			public class ALPHA
			{
				// Token: 0x0400D311 RID: 54033
				public static LocString NAME = "Novice Research";

				// Token: 0x0400D312 RID: 54034
				public static LocString DESC = UI.FormatAsLink("Novice Research", "RESEARCH") + " is required to unlock basic technologies.\nIt can be conducted at a " + UI.FormatAsLink("Research Station", "RESEARCHCENTER") + ".";

				// Token: 0x0400D313 RID: 54035
				public static LocString RECIPEDESC = "Unlocks rudimentary technologies.";
			}

			// Token: 0x020034A6 RID: 13478
			public class BETA
			{
				// Token: 0x0400D314 RID: 54036
				public static LocString NAME = "Advanced Research";

				// Token: 0x0400D315 RID: 54037
				public static LocString DESC = UI.FormatAsLink("Advanced Research", "RESEARCH") + " is required to unlock improved technologies.\nIt can be conducted at a " + UI.FormatAsLink("Super Computer", "ADVANCEDRESEARCHCENTER") + ".";

				// Token: 0x0400D316 RID: 54038
				public static LocString RECIPEDESC = "Unlocks improved technologies.";
			}

			// Token: 0x020034A7 RID: 13479
			public class GAMMA
			{
				// Token: 0x0400D317 RID: 54039
				public static LocString NAME = "Interstellar Research";

				// Token: 0x0400D318 RID: 54040
				public static LocString DESC = UI.FormatAsLink("Interstellar Research", "RESEARCH") + " is required to unlock space technologies.\nIt can be conducted at a " + UI.FormatAsLink("Virtual Planetarium", "COSMICRESEARCHCENTER") + ".";

				// Token: 0x0400D319 RID: 54041
				public static LocString RECIPEDESC = "Unlocks cutting-edge technologies.";
			}

			// Token: 0x020034A8 RID: 13480
			public class DELTA
			{
				// Token: 0x0400D31A RID: 54042
				public static LocString NAME = "Applied Sciences Research";

				// Token: 0x0400D31B RID: 54043
				public static LocString DESC = UI.FormatAsLink("Applied Sciences Research", "RESEARCH") + " is required to unlock materials science technologies.\nIt can be conducted at a " + UI.FormatAsLink("Materials Study Terminal", "NUCLEARRESEARCHCENTER") + ".";

				// Token: 0x0400D31C RID: 54044
				public static LocString RECIPEDESC = "Unlocks next wave technologies.";
			}

			// Token: 0x020034A9 RID: 13481
			public class ORBITAL
			{
				// Token: 0x0400D31D RID: 54045
				public static LocString NAME = "Data Analysis Research";

				// Token: 0x0400D31E RID: 54046
				public static LocString DESC = UI.FormatAsLink("Data Analysis Research", "RESEARCH") + " is required to unlock Data Analysis technologies.\nIt can be conducted at a " + UI.FormatAsLink("Orbital Data Collection Lab", "ORBITALRESEARCHCENTER") + ".";

				// Token: 0x0400D31F RID: 54047
				public static LocString RECIPEDESC = "Unlocks out-of-this-world technologies.";
			}
		}

		// Token: 0x020034AA RID: 13482
		public class OTHER_TECH_ITEMS
		{
			// Token: 0x020034AB RID: 13483
			public class AUTOMATION_OVERLAY
			{
				// Token: 0x0400D320 RID: 54048
				public static LocString NAME = UI.FormatAsOverlay("Automation Overlay");

				// Token: 0x0400D321 RID: 54049
				public static LocString DESC = "Enables access to the " + UI.FormatAsOverlay("Automation Overlay") + ".";
			}

			// Token: 0x020034AC RID: 13484
			public class SUITS_OVERLAY
			{
				// Token: 0x0400D322 RID: 54050
				public static LocString NAME = UI.FormatAsOverlay("Exosuit Overlay");

				// Token: 0x0400D323 RID: 54051
				public static LocString DESC = "Enables access to the " + UI.FormatAsOverlay("Exosuit Overlay") + ".";
			}

			// Token: 0x020034AD RID: 13485
			public class JET_SUIT
			{
				// Token: 0x0400D324 RID: 54052
				public static LocString NAME = UI.PRE_KEYWORD + "Jet Suit" + UI.PST_KEYWORD + " Pattern";

				// Token: 0x0400D325 RID: 54053
				public static LocString DESC = string.Concat(new string[]
				{
					"Enables fabrication of ",
					UI.PRE_KEYWORD,
					"Jet Suits",
					UI.PST_KEYWORD,
					" at the ",
					BUILDINGS.PREFABS.SUITFABRICATOR.NAME
				});
			}

			// Token: 0x020034AE RID: 13486
			public class OXYGEN_MASK
			{
				// Token: 0x0400D326 RID: 54054
				public static LocString NAME = UI.PRE_KEYWORD + "Oxygen Mask" + UI.PST_KEYWORD + " Pattern";

				// Token: 0x0400D327 RID: 54055
				public static LocString DESC = string.Concat(new string[]
				{
					"Enables fabrication of ",
					UI.PRE_KEYWORD,
					"Oxygen Masks",
					UI.PST_KEYWORD,
					" at the ",
					BUILDINGS.PREFABS.CRAFTINGTABLE.NAME
				});
			}

			// Token: 0x020034AF RID: 13487
			public class LEAD_SUIT
			{
				// Token: 0x0400D328 RID: 54056
				public static LocString NAME = UI.PRE_KEYWORD + "Lead Suit" + UI.PST_KEYWORD + " Pattern";

				// Token: 0x0400D329 RID: 54057
				public static LocString DESC = string.Concat(new string[]
				{
					"Enables fabrication of ",
					UI.PRE_KEYWORD,
					"Lead Suits",
					UI.PST_KEYWORD,
					" at the ",
					BUILDINGS.PREFABS.SUITFABRICATOR.NAME
				});
			}

			// Token: 0x020034B0 RID: 13488
			public class ATMO_SUIT
			{
				// Token: 0x0400D32A RID: 54058
				public static LocString NAME = UI.PRE_KEYWORD + "Atmo Suit" + UI.PST_KEYWORD + " Pattern";

				// Token: 0x0400D32B RID: 54059
				public static LocString DESC = string.Concat(new string[]
				{
					"Enables fabrication of ",
					UI.PRE_KEYWORD,
					"Atmo Suits",
					UI.PST_KEYWORD,
					" at the ",
					BUILDINGS.PREFABS.SUITFABRICATOR.NAME
				});
			}

			// Token: 0x020034B1 RID: 13489
			public class BETA_RESEARCH_POINT
			{
				// Token: 0x0400D32C RID: 54060
				public static LocString NAME = UI.PRE_KEYWORD + "Advanced Research" + UI.PST_KEYWORD + " Capability";

				// Token: 0x0400D32D RID: 54061
				public static LocString DESC = string.Concat(new string[]
				{
					"Allows ",
					UI.PRE_KEYWORD,
					"Advanced Research",
					UI.PST_KEYWORD,
					" points to be accumulated, unlocking higher technology tiers."
				});
			}

			// Token: 0x020034B2 RID: 13490
			public class GAMMA_RESEARCH_POINT
			{
				// Token: 0x0400D32E RID: 54062
				public static LocString NAME = UI.PRE_KEYWORD + "Interstellar Research" + UI.PST_KEYWORD + " Capability";

				// Token: 0x0400D32F RID: 54063
				public static LocString DESC = string.Concat(new string[]
				{
					"Allows ",
					UI.PRE_KEYWORD,
					"Interstellar Research",
					UI.PST_KEYWORD,
					" points to be accumulated, unlocking higher technology tiers."
				});
			}

			// Token: 0x020034B3 RID: 13491
			public class DELTA_RESEARCH_POINT
			{
				// Token: 0x0400D330 RID: 54064
				public static LocString NAME = UI.PRE_KEYWORD + "Materials Science Research" + UI.PST_KEYWORD + " Capability";

				// Token: 0x0400D331 RID: 54065
				public static LocString DESC = string.Concat(new string[]
				{
					"Allows ",
					UI.PRE_KEYWORD,
					"Materials Science Research",
					UI.PST_KEYWORD,
					" points to be accumulated, unlocking higher technology tiers."
				});
			}

			// Token: 0x020034B4 RID: 13492
			public class ORBITAL_RESEARCH_POINT
			{
				// Token: 0x0400D332 RID: 54066
				public static LocString NAME = UI.PRE_KEYWORD + "Data Analysis Research" + UI.PST_KEYWORD + " Capability";

				// Token: 0x0400D333 RID: 54067
				public static LocString DESC = string.Concat(new string[]
				{
					"Allows ",
					UI.PRE_KEYWORD,
					"Data Analysis Research",
					UI.PST_KEYWORD,
					" points to be accumulated, unlocking higher technology tiers."
				});
			}

			// Token: 0x020034B5 RID: 13493
			public class CONVEYOR_OVERLAY
			{
				// Token: 0x0400D334 RID: 54068
				public static LocString NAME = UI.FormatAsOverlay("Conveyor Overlay");

				// Token: 0x0400D335 RID: 54069
				public static LocString DESC = "Enables access to the " + UI.FormatAsOverlay("Conveyor Overlay") + ".";
			}

			// Token: 0x020034B6 RID: 13494
			public class LUBRICATION_STICK
			{
				// Token: 0x0400D336 RID: 54070
				public static LocString NAME = UI.PRE_KEYWORD + "Gear Balm" + UI.PST_KEYWORD + " Pattern";

				// Token: 0x0400D337 RID: 54071
				public static LocString DESC = string.Concat(new string[]
				{
					"Enables fabrication of ",
					UI.PRE_KEYWORD,
					"Gear Balm",
					UI.PST_KEYWORD,
					" at the ",
					BUILDINGS.PREFABS.APOTHECARY.NAME
				});
			}

			// Token: 0x020034B7 RID: 13495
			public class DISPOSABLE_ELECTROBANK_METAL_ORE
			{
				// Token: 0x0400D338 RID: 54072
				public static LocString NAME = UI.PRE_KEYWORD + "Metal Power Bank" + UI.PST_KEYWORD + " Pattern";

				// Token: 0x0400D339 RID: 54073
				public static LocString DESC = string.Concat(new string[]
				{
					"Enables fabrication of ",
					UI.PRE_KEYWORD,
					"Metal Power Banks",
					UI.PST_KEYWORD,
					" at the ",
					BUILDINGS.PREFABS.CRAFTINGTABLE.NAME
				});
			}

			// Token: 0x020034B8 RID: 13496
			public class DISPOSABLE_ELECTROBANK_URANIUM_ORE
			{
				// Token: 0x0400D33A RID: 54074
				public static LocString NAME = UI.PRE_KEYWORD + "Uranium Ore Power Bank" + UI.PST_KEYWORD + " Pattern";

				// Token: 0x0400D33B RID: 54075
				public static LocString DESC = string.Concat(new string[]
				{
					"Enables fabrication of ",
					UI.PRE_KEYWORD,
					"Uranium Ore Power Banks",
					UI.PST_KEYWORD,
					" at the ",
					BUILDINGS.PREFABS.CRAFTINGTABLE.NAME
				});
			}

			// Token: 0x020034B9 RID: 13497
			public class ELECTROBANK
			{
				// Token: 0x0400D33C RID: 54076
				public static LocString NAME = UI.PRE_KEYWORD + "Eco Power Bank" + UI.PST_KEYWORD + " Pattern";

				// Token: 0x0400D33D RID: 54077
				public static LocString DESC = string.Concat(new string[]
				{
					"Enables fabrication of ",
					UI.PRE_KEYWORD,
					"Eco Power Banks",
					UI.PST_KEYWORD,
					" at the ",
					BUILDINGS.PREFABS.ADVANCEDCRAFTINGTABLE.NAME
				});
			}

			// Token: 0x020034BA RID: 13498
			public class SELFCHARGINGELECTROBANK
			{
				// Token: 0x0400D33E RID: 54078
				public static LocString NAME = UI.PRE_KEYWORD + "Atomic Power Bank" + UI.PST_KEYWORD + " Pattern";

				// Token: 0x0400D33F RID: 54079
				public static LocString DESC = string.Concat(new string[]
				{
					"Enables fabrication of ",
					UI.PRE_KEYWORD,
					"Atomic Power Bank",
					UI.PST_KEYWORD,
					" at the ",
					BUILDINGS.PREFABS.SUPERMATERIALREFINERY.NAME
				});
			}

			// Token: 0x020034BB RID: 13499
			public class FETCHDRONE
			{
				// Token: 0x0400D340 RID: 54080
				public static LocString NAME = UI.PRE_KEYWORD + "Flydo" + UI.PST_KEYWORD + " Pattern";

				// Token: 0x0400D341 RID: 54081
				public static LocString DESC = string.Concat(new string[]
				{
					"Enables fabrication of ",
					UI.PRE_KEYWORD,
					"Flydo",
					UI.PST_KEYWORD,
					" at the ",
					BUILDINGS.PREFABS.ADVANCEDCRAFTINGTABLE.NAME
				});
			}

			// Token: 0x020034BC RID: 13500
			public class PILOTINGBOOSTER
			{
				// Token: 0x0400D342 RID: 54082
				public static LocString NAME = UI.PRE_KEYWORD + "Rocketry Booster" + UI.PST_KEYWORD + " Pattern";

				// Token: 0x0400D343 RID: 54083
				public static LocString DESC = string.Concat(new string[]
				{
					"Enables fabrication of ",
					UI.PRE_KEYWORD,
					"Rocketry Boosters",
					UI.PST_KEYWORD,
					" for Bionic Duplicants at the ",
					BUILDINGS.PREFABS.ADVANCEDCRAFTINGTABLE.NAME
				});
			}

			// Token: 0x020034BD RID: 13501
			public class CONSTRUCTIONBOOSTER
			{
				// Token: 0x0400D344 RID: 54084
				public static LocString NAME = UI.PRE_KEYWORD + "Building Booster" + UI.PST_KEYWORD + " Pattern";

				// Token: 0x0400D345 RID: 54085
				public static LocString DESC = string.Concat(new string[]
				{
					"Enables fabrication of ",
					UI.PRE_KEYWORD,
					"Building Boosters",
					UI.PST_KEYWORD,
					" for Bionic Duplicants at the ",
					BUILDINGS.PREFABS.ADVANCEDCRAFTINGTABLE.NAME
				});
			}

			// Token: 0x020034BE RID: 13502
			public class EXCAVATIONBOOSTER
			{
				// Token: 0x0400D346 RID: 54086
				public static LocString NAME = UI.PRE_KEYWORD + "Digging Booster" + UI.PST_KEYWORD + " Pattern";

				// Token: 0x0400D347 RID: 54087
				public static LocString DESC = string.Concat(new string[]
				{
					"Enables fabrication of ",
					UI.PRE_KEYWORD,
					"Digging Boosters",
					UI.PST_KEYWORD,
					" for Bionic Duplicants at the ",
					BUILDINGS.PREFABS.ADVANCEDCRAFTINGTABLE.NAME
				});
			}

			// Token: 0x020034BF RID: 13503
			public class EXPLORERBOOSTER
			{
				// Token: 0x0400D348 RID: 54088
				public static LocString NAME = UI.PRE_KEYWORD + "Dowsing Booster" + UI.PST_KEYWORD + " Pattern";

				// Token: 0x0400D349 RID: 54089
				public static LocString DESC = string.Concat(new string[]
				{
					"Enables fabrication of ",
					UI.PRE_KEYWORD,
					"Dowsing Boosters",
					UI.PST_KEYWORD,
					" for Bionic Duplicants at the ",
					BUILDINGS.PREFABS.ADVANCEDCRAFTINGTABLE.NAME
				});
			}

			// Token: 0x020034C0 RID: 13504
			public class MACHINERYBOOSTER
			{
				// Token: 0x0400D34A RID: 54090
				public static LocString NAME = UI.PRE_KEYWORD + "Operating Booster" + UI.PST_KEYWORD + " Pattern";

				// Token: 0x0400D34B RID: 54091
				public static LocString DESC = string.Concat(new string[]
				{
					"Enables fabrication of ",
					UI.PRE_KEYWORD,
					"Operating Boosters",
					UI.PST_KEYWORD,
					" for Bionic Duplicants at the ",
					BUILDINGS.PREFABS.ADVANCEDCRAFTINGTABLE.NAME
				});
			}

			// Token: 0x020034C1 RID: 13505
			public class ATHLETICSBOOSTER
			{
				// Token: 0x0400D34C RID: 54092
				public static LocString NAME = UI.PRE_KEYWORD + "Athletics Booster" + UI.PST_KEYWORD + " Pattern";

				// Token: 0x0400D34D RID: 54093
				public static LocString DESC = string.Concat(new string[]
				{
					"Enables fabrication of ",
					UI.PRE_KEYWORD,
					"Athletics Boosters",
					UI.PST_KEYWORD,
					" for Bionic Duplicants at the ",
					BUILDINGS.PREFABS.ADVANCEDCRAFTINGTABLE.NAME
				});
			}

			// Token: 0x020034C2 RID: 13506
			public class SCIENCEBOOSTER
			{
				// Token: 0x0400D34E RID: 54094
				public static LocString NAME = UI.PRE_KEYWORD + "Researching Booster" + UI.PST_KEYWORD + " Pattern";

				// Token: 0x0400D34F RID: 54095
				public static LocString DESC = string.Concat(new string[]
				{
					"Enables fabrication of ",
					UI.PRE_KEYWORD,
					"Researching Boosters",
					UI.PST_KEYWORD,
					" for Bionic Duplicants at the ",
					BUILDINGS.PREFABS.ADVANCEDCRAFTINGTABLE.NAME
				});
			}

			// Token: 0x020034C3 RID: 13507
			public class COOKINGBOOSTER
			{
				// Token: 0x0400D350 RID: 54096
				public static LocString NAME = UI.PRE_KEYWORD + "Cooking Booster" + UI.PST_KEYWORD + " Pattern";

				// Token: 0x0400D351 RID: 54097
				public static LocString DESC = string.Concat(new string[]
				{
					"Enables fabrication of ",
					UI.PRE_KEYWORD,
					"Cooking Boosters",
					UI.PST_KEYWORD,
					" for Bionic Duplicants at the ",
					BUILDINGS.PREFABS.ADVANCEDCRAFTINGTABLE.NAME
				});
			}

			// Token: 0x020034C4 RID: 13508
			public class MEDICINEBOOSTER
			{
				// Token: 0x0400D352 RID: 54098
				public static LocString NAME = UI.PRE_KEYWORD + "Doctoring Booster" + UI.PST_KEYWORD + " Pattern";

				// Token: 0x0400D353 RID: 54099
				public static LocString DESC = string.Concat(new string[]
				{
					"Enables fabrication of ",
					UI.PRE_KEYWORD,
					"Doctoring Boosters",
					UI.PST_KEYWORD,
					" for Bionic Duplicants at the ",
					BUILDINGS.PREFABS.ADVANCEDCRAFTINGTABLE.NAME
				});
			}

			// Token: 0x020034C5 RID: 13509
			public class STRENGTHBOOSTER
			{
				// Token: 0x0400D354 RID: 54100
				public static LocString NAME = UI.PRE_KEYWORD + "Strength Booster" + UI.PST_KEYWORD + " Pattern";

				// Token: 0x0400D355 RID: 54101
				public static LocString DESC = string.Concat(new string[]
				{
					"Enables fabrication of ",
					UI.PRE_KEYWORD,
					"Strength Boosters",
					UI.PST_KEYWORD,
					" for Bionic Duplicants at the ",
					BUILDINGS.PREFABS.ADVANCEDCRAFTINGTABLE.NAME
				});
			}

			// Token: 0x020034C6 RID: 13510
			public class CREATIVITYBOOSTER
			{
				// Token: 0x0400D356 RID: 54102
				public static LocString NAME = UI.PRE_KEYWORD + "Decorating Booster" + UI.PST_KEYWORD + " Pattern";

				// Token: 0x0400D357 RID: 54103
				public static LocString DESC = string.Concat(new string[]
				{
					"Enables fabrication of ",
					UI.PRE_KEYWORD,
					"Decorating Boosters",
					UI.PST_KEYWORD,
					" for Bionic Duplicants at the ",
					BUILDINGS.PREFABS.ADVANCEDCRAFTINGTABLE.NAME
				});
			}

			// Token: 0x020034C7 RID: 13511
			public class AGRICULTUREBOOSTER
			{
				// Token: 0x0400D358 RID: 54104
				public static LocString NAME = UI.PRE_KEYWORD + "Farming Booster" + UI.PST_KEYWORD + " Pattern";

				// Token: 0x0400D359 RID: 54105
				public static LocString DESC = string.Concat(new string[]
				{
					"Enables fabrication of ",
					UI.PRE_KEYWORD,
					"Farming Boosters",
					UI.PST_KEYWORD,
					" for Bionic Duplicants at the ",
					BUILDINGS.PREFABS.ADVANCEDCRAFTINGTABLE.NAME
				});
			}

			// Token: 0x020034C8 RID: 13512
			public class HUSBANDRYBOOSTER
			{
				// Token: 0x0400D35A RID: 54106
				public static LocString NAME = UI.PRE_KEYWORD + "Ranching Booster" + UI.PST_KEYWORD + " Pattern";

				// Token: 0x0400D35B RID: 54107
				public static LocString DESC = string.Concat(new string[]
				{
					"Enables fabrication of ",
					UI.PRE_KEYWORD,
					"Ranching Boosters",
					UI.PST_KEYWORD,
					" for Bionic Duplicants at the ",
					BUILDINGS.PREFABS.ADVANCEDCRAFTINGTABLE.NAME
				});
			}
		}

		// Token: 0x020034C9 RID: 13513
		public class TREES
		{
			// Token: 0x0400D35C RID: 54108
			public static LocString TITLE_FOOD = "Food";

			// Token: 0x0400D35D RID: 54109
			public static LocString TITLE_POWER = "Power";

			// Token: 0x0400D35E RID: 54110
			public static LocString TITLE_SOLIDS = "Solid Material";

			// Token: 0x0400D35F RID: 54111
			public static LocString TITLE_COLONYDEVELOPMENT = "Colony Development";

			// Token: 0x0400D360 RID: 54112
			public static LocString TITLE_RADIATIONTECH = "Radiation Technologies";

			// Token: 0x0400D361 RID: 54113
			public static LocString TITLE_MEDICINE = "Medicine";

			// Token: 0x0400D362 RID: 54114
			public static LocString TITLE_LIQUIDS = "Liquids";

			// Token: 0x0400D363 RID: 54115
			public static LocString TITLE_GASES = "Gases";

			// Token: 0x0400D364 RID: 54116
			public static LocString TITLE_SUITS = "Exosuits";

			// Token: 0x0400D365 RID: 54117
			public static LocString TITLE_DECOR = "Decor";

			// Token: 0x0400D366 RID: 54118
			public static LocString TITLE_COMPUTERS = "Computers";

			// Token: 0x0400D367 RID: 54119
			public static LocString TITLE_ROCKETS = "Rocketry";
		}

		// Token: 0x020034CA RID: 13514
		public class TECHS
		{
			// Token: 0x020034CB RID: 13515
			public class JOBS
			{
				// Token: 0x0400D368 RID: 54120
				public static LocString NAME = UI.FormatAsLink("Employment", "JOBS");

				// Token: 0x0400D369 RID: 54121
				public static LocString DESC = "Exchange the skill points earned by Duplicants for new traits and abilities.";
			}

			// Token: 0x020034CC RID: 13516
			public class IMPROVEDOXYGEN
			{
				// Token: 0x0400D36A RID: 54122
				public static LocString NAME = UI.FormatAsLink("Air Systems", "IMPROVEDOXYGEN");

				// Token: 0x0400D36B RID: 54123
				public static LocString DESC = "Maintain clean, breathable air in the colony.";
			}

			// Token: 0x020034CD RID: 13517
			public class FARMINGTECH
			{
				// Token: 0x0400D36C RID: 54124
				public static LocString NAME = UI.FormatAsLink("Basic Farming", "FARMINGTECH");

				// Token: 0x0400D36D RID: 54125
				public static LocString DESC = "Learn the introductory principles of " + UI.FormatAsLink("Plant", "PLANTS") + " domestication.";
			}

			// Token: 0x020034CE RID: 13518
			public class AGRICULTURE
			{
				// Token: 0x0400D36E RID: 54126
				public static LocString NAME = UI.FormatAsLink("Agriculture", "AGRICULTURE");

				// Token: 0x0400D36F RID: 54127
				public static LocString DESC = "Master the agricultural art of crop raising.";
			}

			// Token: 0x020034CF RID: 13519
			public class RANCHING
			{
				// Token: 0x0400D370 RID: 54128
				public static LocString NAME = UI.FormatAsLink("Ranching", "RANCHING");

				// Token: 0x0400D371 RID: 54129
				public static LocString DESC = "Tame and care for wild critters.";
			}

			// Token: 0x020034D0 RID: 13520
			public class ANIMALCONTROL
			{
				// Token: 0x0400D372 RID: 54130
				public static LocString NAME = UI.FormatAsLink("Animal Control", "ANIMALCONTROL");

				// Token: 0x0400D373 RID: 54131
				public static LocString DESC = "Useful techniques to manage critter populations in the colony.";
			}

			// Token: 0x020034D1 RID: 13521
			public class ANIMALCOMFORT
			{
				// Token: 0x0400D374 RID: 54132
				public static LocString NAME = UI.FormatAsLink("Creature Comforts", "ANIMALCOMFORT");

				// Token: 0x0400D375 RID: 54133
				public static LocString DESC = "Strategies for maximizing critters' quality of life.";
			}

			// Token: 0x020034D2 RID: 13522
			public class DAIRYOPERATION
			{
				// Token: 0x0400D376 RID: 54134
				public static LocString NAME = UI.FormatAsLink("Brackene Flow", "DAIRYOPERATION");

				// Token: 0x0400D377 RID: 54135
				public static LocString DESC = "Advanced production, processing and distribution of this fluid resource.";
			}

			// Token: 0x020034D3 RID: 13523
			public class FOODREPURPOSING
			{
				// Token: 0x0400D378 RID: 54136
				public static LocString NAME = UI.FormatAsLink("Food Repurposing", "FOODREPURPOSING");

				// Token: 0x0400D379 RID: 54137
				public static LocString DESC = string.Concat(new string[]
				{
					"Blend that leftover ",
					UI.FormatAsLink("Food", "FOOD"),
					" into a ",
					UI.FormatAsLink("Morale", "MORALE"),
					"-boosting slurry."
				});
			}

			// Token: 0x020034D4 RID: 13524
			public class FINEDINING
			{
				// Token: 0x0400D37A RID: 54138
				public static LocString NAME = UI.FormatAsLink("Meal Preparation", "FINEDINING");

				// Token: 0x0400D37B RID: 54139
				public static LocString DESC = "Prepare more nutritious " + UI.FormatAsLink("Food", "FOOD") + " and store it longer before spoiling.";
			}

			// Token: 0x020034D5 RID: 13525
			public class FINERDINING
			{
				// Token: 0x0400D37C RID: 54140
				public static LocString NAME = UI.FormatAsLink("Gourmet Meal Preparation", "FINERDINING");

				// Token: 0x0400D37D RID: 54141
				public static LocString DESC = "Raise colony Morale by cooking the most delicious, high-quality " + UI.FormatAsLink("Foods", "FOOD") + ".";
			}

			// Token: 0x020034D6 RID: 13526
			public class GASPIPING
			{
				// Token: 0x0400D37E RID: 54142
				public static LocString NAME = UI.FormatAsLink("Ventilation", "GASPIPING");

				// Token: 0x0400D37F RID: 54143
				public static LocString DESC = "Rudimentary technologies for installing " + UI.FormatAsLink("Gas", "ELEMENTS_GAS") + " infrastructure.";
			}

			// Token: 0x020034D7 RID: 13527
			public class IMPROVEDGASPIPING
			{
				// Token: 0x0400D380 RID: 54144
				public static LocString NAME = UI.FormatAsLink("Improved Ventilation", "IMPROVEDGASPIPING");

				// Token: 0x0400D381 RID: 54145
				public static LocString DESC = UI.FormatAsLink("Gas", "ELEMENTS_GAS") + " infrastructure capable of withstanding more intense conditions, such as " + UI.FormatAsLink("Heat", "Heat") + " and pressure.";
			}

			// Token: 0x020034D8 RID: 13528
			public class FLOWREDIRECTION
			{
				// Token: 0x0400D382 RID: 54146
				public static LocString NAME = UI.FormatAsLink("Flow Redirection", "FLOWREDIRECTION");

				// Token: 0x0400D383 RID: 54147
				public static LocString DESC = UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID") + " management for " + UI.FormatAsLink("Morale", "MORALE") + " and industry.";
			}

			// Token: 0x020034D9 RID: 13529
			public class LIQUIDDISTRIBUTION
			{
				// Token: 0x0400D384 RID: 54148
				public static LocString NAME = UI.FormatAsLink("Liquid Distribution", "LIQUIDDISTRIBUTION");

				// Token: 0x0400D385 RID: 54149
				public static LocString DESC = "Advanced fittings ensure that " + UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID") + " resources get where they need to go.";
			}

			// Token: 0x020034DA RID: 13530
			public class TEMPERATUREMODULATION
			{
				// Token: 0x0400D386 RID: 54150
				public static LocString NAME = UI.FormatAsLink("Temperature Modulation", "TEMPERATUREMODULATION");

				// Token: 0x0400D387 RID: 54151
				public static LocString DESC = "Precise " + UI.FormatAsLink("Temperature", "HEAT") + " altering technologies to keep my colony at the perfect Kelvin.";
			}

			// Token: 0x020034DB RID: 13531
			public class HVAC
			{
				// Token: 0x0400D388 RID: 54152
				public static LocString NAME = UI.FormatAsLink("HVAC", "HVAC");

				// Token: 0x0400D389 RID: 54153
				public static LocString DESC = string.Concat(new string[]
				{
					"Regulate ",
					UI.FormatAsLink("Temperature", "HEAT"),
					" in the colony for ",
					UI.FormatAsLink("Plant", "PLANTS"),
					" cultivation and Duplicant comfort."
				});
			}

			// Token: 0x020034DC RID: 13532
			public class GASDISTRIBUTION
			{
				// Token: 0x0400D38A RID: 54154
				public static LocString NAME = UI.FormatAsLink("Gas Distribution", "GASDISTRIBUTION");

				// Token: 0x0400D38B RID: 54155
				public static LocString DESC = "Design building hookups to get " + UI.FormatAsLink("Gas", "ELEMENTS_GAS") + " resources circulating properly.";
			}

			// Token: 0x020034DD RID: 13533
			public class LIQUIDTEMPERATURE
			{
				// Token: 0x0400D38C RID: 54156
				public static LocString NAME = UI.FormatAsLink("Liquid Tuning", "LIQUIDTEMPERATURE");

				// Token: 0x0400D38D RID: 54157
				public static LocString DESC = string.Concat(new string[]
				{
					"Easily manipulate ",
					UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID"),
					" ",
					UI.FormatAsLink("Heat", "Temperatures"),
					" with these temperature regulating technologies."
				});
			}

			// Token: 0x020034DE RID: 13534
			public class INSULATION
			{
				// Token: 0x0400D38E RID: 54158
				public static LocString NAME = UI.FormatAsLink("Insulation", "INSULATION");

				// Token: 0x0400D38F RID: 54159
				public static LocString DESC = "Improve " + UI.FormatAsLink("Heat", "Heat") + " distribution within the colony and guard buildings from extreme temperatures.";
			}

			// Token: 0x020034DF RID: 13535
			public class PRESSUREMANAGEMENT
			{
				// Token: 0x0400D390 RID: 54160
				public static LocString NAME = UI.FormatAsLink("Pressure Management", "PRESSUREMANAGEMENT");

				// Token: 0x0400D391 RID: 54161
				public static LocString DESC = "Unlock technologies to manage colony pressure and atmosphere.";
			}

			// Token: 0x020034E0 RID: 13536
			public class PORTABLEGASSES
			{
				// Token: 0x0400D392 RID: 54162
				public static LocString NAME = UI.FormatAsLink("Portable Gases", "PORTABLEGASSES");

				// Token: 0x0400D393 RID: 54163
				public static LocString DESC = "Unlock technologies to easily move gases around your colony.";
			}

			// Token: 0x020034E1 RID: 13537
			public class DIRECTEDAIRSTREAMS
			{
				// Token: 0x0400D394 RID: 54164
				public static LocString NAME = UI.FormatAsLink("Decontamination", "DIRECTEDAIRSTREAMS");

				// Token: 0x0400D395 RID: 54165
				public static LocString DESC = "Instruments to help reduce " + UI.FormatAsLink("Germ", "DISEASE") + " spread within the base.";
			}

			// Token: 0x020034E2 RID: 13538
			public class LIQUIDFILTERING
			{
				// Token: 0x0400D396 RID: 54166
				public static LocString NAME = UI.FormatAsLink("Liquid-Based Refinement Processes", "LIQUIDFILTERING");

				// Token: 0x0400D397 RID: 54167
				public static LocString DESC = "Use pumped " + UI.FormatAsLink("Liquids", "ELEMENTS_LIQUID") + " to filter out unwanted elements.";
			}

			// Token: 0x020034E3 RID: 13539
			public class LIQUIDPIPING
			{
				// Token: 0x0400D398 RID: 54168
				public static LocString NAME = UI.FormatAsLink("Plumbing", "LIQUIDPIPING");

				// Token: 0x0400D399 RID: 54169
				public static LocString DESC = "Rudimentary technologies for installing " + UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID") + " infrastructure.";
			}

			// Token: 0x020034E4 RID: 13540
			public class IMPROVEDLIQUIDPIPING
			{
				// Token: 0x0400D39A RID: 54170
				public static LocString NAME = UI.FormatAsLink("Improved Plumbing", "IMPROVEDLIQUIDPIPING");

				// Token: 0x0400D39B RID: 54171
				public static LocString DESC = UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID") + " infrastructure capable of withstanding more intense conditions, such as " + UI.FormatAsLink("Heat", "Heat") + " and pressure.";
			}

			// Token: 0x020034E5 RID: 13541
			public class PRECISIONPLUMBING
			{
				// Token: 0x0400D39C RID: 54172
				public static LocString NAME = UI.FormatAsLink("Advanced Caffeination", "PRECISIONPLUMBING");

				// Token: 0x0400D39D RID: 54173
				public static LocString DESC = "Let Duplicants relax after a long day of subterranean digging with a shot of warm beanjuice.";
			}

			// Token: 0x020034E6 RID: 13542
			public class SANITATIONSCIENCES
			{
				// Token: 0x0400D39E RID: 54174
				public static LocString NAME = UI.FormatAsLink("Sanitation", "SANITATIONSCIENCES");

				// Token: 0x0400D39F RID: 54175
				public static LocString DESC = "Make daily ablutions less of a hassle.";
			}

			// Token: 0x020034E7 RID: 13543
			public class ADVANCEDSANITATION
			{
				// Token: 0x0400D3A0 RID: 54176
				public static LocString NAME = UI.FormatAsLink("Advanced Sanitation", "ADVANCEDSANITATION");

				// Token: 0x0400D3A1 RID: 54177
				public static LocString DESC = "Clean up dirty Duplicants.";
			}

			// Token: 0x020034E8 RID: 13544
			public class MEDICINEI
			{
				// Token: 0x0400D3A2 RID: 54178
				public static LocString NAME = UI.FormatAsLink("Pharmacology", "MEDICINEI");

				// Token: 0x0400D3A3 RID: 54179
				public static LocString DESC = "Compound natural cures to fight the most common " + UI.FormatAsLink("Sicknesses", "SICKNESSES") + " that plague Duplicants.";
			}

			// Token: 0x020034E9 RID: 13545
			public class MEDICINEII
			{
				// Token: 0x0400D3A4 RID: 54180
				public static LocString NAME = UI.FormatAsLink("Medical Equipment", "MEDICINEII");

				// Token: 0x0400D3A5 RID: 54181
				public static LocString DESC = "The basic necessities doctors need to facilitate patient care.";
			}

			// Token: 0x020034EA RID: 13546
			public class MEDICINEIII
			{
				// Token: 0x0400D3A6 RID: 54182
				public static LocString NAME = UI.FormatAsLink("Pathogen Diagnostics", "MEDICINEIII");

				// Token: 0x0400D3A7 RID: 54183
				public static LocString DESC = "Stop Germs at the source using special medical automation technology.";
			}

			// Token: 0x020034EB RID: 13547
			public class MEDICINEIV
			{
				// Token: 0x0400D3A8 RID: 54184
				public static LocString NAME = UI.FormatAsLink("Micro-Targeted Medicine", "MEDICINEIV");

				// Token: 0x0400D3A9 RID: 54185
				public static LocString DESC = "State of the art equipment to conquer the most stubborn of illnesses.";
			}

			// Token: 0x020034EC RID: 13548
			public class ADVANCEDFILTRATION
			{
				// Token: 0x0400D3AA RID: 54186
				public static LocString NAME = UI.FormatAsLink("Filtration", "ADVANCEDFILTRATION");

				// Token: 0x0400D3AB RID: 54187
				public static LocString DESC = string.Concat(new string[]
				{
					"Basic technologies for filtering ",
					UI.FormatAsLink("Liquids", "ELEMENTS_LIQUID"),
					" and ",
					UI.FormatAsLink("Gases", "ELEMENTS_GAS"),
					"."
				});
			}

			// Token: 0x020034ED RID: 13549
			public class POWERREGULATION
			{
				// Token: 0x0400D3AC RID: 54188
				public static LocString NAME = UI.FormatAsLink("Power Regulation", "POWERREGULATION");

				// Token: 0x0400D3AD RID: 54189
				public static LocString DESC = "Prevent wasted " + UI.FormatAsLink("Power", "POWER") + " with improved electrical tools.";
			}

			// Token: 0x020034EE RID: 13550
			public class COMBUSTION
			{
				// Token: 0x0400D3AE RID: 54190
				public static LocString NAME = UI.FormatAsLink("Internal Combustion", "COMBUSTION");

				// Token: 0x0400D3AF RID: 54191
				public static LocString DESC = "Fuel-powered generators for crude yet powerful " + UI.FormatAsLink("Power", "POWER") + " production.";
			}

			// Token: 0x020034EF RID: 13551
			public class IMPROVEDCOMBUSTION
			{
				// Token: 0x0400D3B0 RID: 54192
				public static LocString NAME = UI.FormatAsLink("Fossil Fuels", "IMPROVEDCOMBUSTION");

				// Token: 0x0400D3B1 RID: 54193
				public static LocString DESC = "Burn dirty fuels for exceptional " + UI.FormatAsLink("Power", "POWER") + " production.";
			}

			// Token: 0x020034F0 RID: 13552
			public class INTERIORDECOR
			{
				// Token: 0x0400D3B2 RID: 54194
				public static LocString NAME = UI.FormatAsLink("Interior Decor", "INTERIORDECOR");

				// Token: 0x0400D3B3 RID: 54195
				public static LocString DESC = UI.FormatAsLink("Decor", "DECOR") + " boosting items to counteract the gloom of underground living.";
			}

			// Token: 0x020034F1 RID: 13553
			public class ARTISTRY
			{
				// Token: 0x0400D3B4 RID: 54196
				public static LocString NAME = UI.FormatAsLink("Artistic Expression", "ARTISTRY");

				// Token: 0x0400D3B5 RID: 54197
				public static LocString DESC = "Majorly improve " + UI.FormatAsLink("Decor", "DECOR") + " by giving Duplicants the tools of artistic and emotional expression.";
			}

			// Token: 0x020034F2 RID: 13554
			public class CLOTHING
			{
				// Token: 0x0400D3B6 RID: 54198
				public static LocString NAME = UI.FormatAsLink("Textile Production", "CLOTHING");

				// Token: 0x0400D3B7 RID: 54199
				public static LocString DESC = "Bring Duplicants the " + UI.FormatAsLink("Morale", "MORALE") + " boosting benefits of soft, cushy fabrics.";
			}

			// Token: 0x020034F3 RID: 13555
			public class ACOUSTICS
			{
				// Token: 0x0400D3B8 RID: 54200
				public static LocString NAME = UI.FormatAsLink("Sound Amplifiers", "ACOUSTICS");

				// Token: 0x0400D3B9 RID: 54201
				public static LocString DESC = "Precise control of the audio spectrum allows Duplicants to get funky.";
			}

			// Token: 0x020034F4 RID: 13556
			public class SPACEPOWER
			{
				// Token: 0x0400D3BA RID: 54202
				public static LocString NAME = UI.FormatAsLink("Space Power", "SPACEPOWER");

				// Token: 0x0400D3BB RID: 54203
				public static LocString DESC = "It's like power... in space!";
			}

			// Token: 0x020034F5 RID: 13557
			public class AMPLIFIERS
			{
				// Token: 0x0400D3BC RID: 54204
				public static LocString NAME = UI.FormatAsLink("Power Amplifiers", "AMPLIFIERS");

				// Token: 0x0400D3BD RID: 54205
				public static LocString DESC = "Further increased efficacy of " + UI.FormatAsLink("Power", "POWER") + " management to prevent those wasted joules.";
			}

			// Token: 0x020034F6 RID: 13558
			public class LUXURY
			{
				// Token: 0x0400D3BE RID: 54206
				public static LocString NAME = UI.FormatAsLink("Home Luxuries", "LUXURY");

				// Token: 0x0400D3BF RID: 54207
				public static LocString DESC = "Luxury amenities for advanced " + UI.FormatAsLink("Stress", "STRESS") + " reduction.";
			}

			// Token: 0x020034F7 RID: 13559
			public class ENVIRONMENTALAPPRECIATION
			{
				// Token: 0x0400D3C0 RID: 54208
				public static LocString NAME = UI.FormatAsLink("Environmental Appreciation", "ENVIRONMENTALAPPRECIATION");

				// Token: 0x0400D3C1 RID: 54209
				public static LocString DESC = string.Concat(new string[]
				{
					"Improve ",
					UI.FormatAsLink("Morale", "MORALE"),
					" by lazing around in ",
					UI.FormatAsLink("Light", "LIGHT"),
					" with a high Lux value."
				});
			}

			// Token: 0x020034F8 RID: 13560
			public class FINEART
			{
				// Token: 0x0400D3C2 RID: 54210
				public static LocString NAME = UI.FormatAsLink("Fine Art", "FINEART");

				// Token: 0x0400D3C3 RID: 54211
				public static LocString DESC = "Broader options for artistic " + UI.FormatAsLink("Decor", "DECOR") + " improvements.";
			}

			// Token: 0x020034F9 RID: 13561
			public class REFRACTIVEDECOR
			{
				// Token: 0x0400D3C4 RID: 54212
				public static LocString NAME = UI.FormatAsLink("High Culture", "REFRACTIVEDECOR");

				// Token: 0x0400D3C5 RID: 54213
				public static LocString DESC = "New methods for working with extremely high quality art materials.";
			}

			// Token: 0x020034FA RID: 13562
			public class RENAISSANCEART
			{
				// Token: 0x0400D3C6 RID: 54214
				public static LocString NAME = UI.FormatAsLink("Renaissance Art", "RENAISSANCEART");

				// Token: 0x0400D3C7 RID: 54215
				public static LocString DESC = "The kind of art that culture legacies are made of.";
			}

			// Token: 0x020034FB RID: 13563
			public class GLASSFURNISHINGS
			{
				// Token: 0x0400D3C8 RID: 54216
				public static LocString NAME = UI.FormatAsLink("Glass Blowing", "GLASSFURNISHINGS");

				// Token: 0x0400D3C9 RID: 54217
				public static LocString DESC = "The decorative benefits of glass are both apparent and transparent.";
			}

			// Token: 0x020034FC RID: 13564
			public class SCREENS
			{
				// Token: 0x0400D3CA RID: 54218
				public static LocString NAME = UI.FormatAsLink("New Media", "SCREENS");

				// Token: 0x0400D3CB RID: 54219
				public static LocString DESC = "High tech displays with lots of pretty colors.";
			}

			// Token: 0x020034FD RID: 13565
			public class ADVANCEDPOWERREGULATION
			{
				// Token: 0x0400D3CC RID: 54220
				public static LocString NAME = UI.FormatAsLink("Advanced Power Regulation", "ADVANCEDPOWERREGULATION");

				// Token: 0x0400D3CD RID: 54221
				public static LocString DESC = "Circuit components required for large scale " + UI.FormatAsLink("Power", "POWER") + " management.";
			}

			// Token: 0x020034FE RID: 13566
			public class PLASTICS
			{
				// Token: 0x0400D3CE RID: 54222
				public static LocString NAME = UI.FormatAsLink("Plastic Manufacturing", "PLASTICS");

				// Token: 0x0400D3CF RID: 54223
				public static LocString DESC = "Stable, lightweight, durable. Plastics are useful for a wide array of applications.";
			}

			// Token: 0x020034FF RID: 13567
			public class SUITS
			{
				// Token: 0x0400D3D0 RID: 54224
				public static LocString NAME = UI.FormatAsLink("Hazard Protection", "SUITS");

				// Token: 0x0400D3D1 RID: 54225
				public static LocString DESC = "Vital gear for surviving in extreme conditions and environments.";
			}

			// Token: 0x02003500 RID: 13568
			public class DISTILLATION
			{
				// Token: 0x0400D3D2 RID: 54226
				public static LocString NAME = UI.FormatAsLink("Distillation", "DISTILLATION");

				// Token: 0x0400D3D3 RID: 54227
				public static LocString DESC = "Distill difficult mixtures down to their most useful parts.";
			}

			// Token: 0x02003501 RID: 13569
			public class CATALYTICS
			{
				// Token: 0x0400D3D4 RID: 54228
				public static LocString NAME = UI.FormatAsLink("Catalytics", "CATALYTICS");

				// Token: 0x0400D3D5 RID: 54229
				public static LocString DESC = "Advanced gas manipulation using unique catalysts.";
			}

			// Token: 0x02003502 RID: 13570
			public class ADVANCEDRESEARCH
			{
				// Token: 0x0400D3D6 RID: 54230
				public static LocString NAME = UI.FormatAsLink("Advanced Research", "ADVANCEDRESEARCH");

				// Token: 0x0400D3D7 RID: 54231
				public static LocString DESC = "The tools my colony needs to conduct more advanced, in-depth research.";
			}

			// Token: 0x02003503 RID: 13571
			public class SPACEPROGRAM
			{
				// Token: 0x0400D3D8 RID: 54232
				public static LocString NAME = UI.FormatAsLink("Space Program", "SPACEPROGRAM");

				// Token: 0x0400D3D9 RID: 54233
				public static LocString DESC = "The first steps in getting a Duplicant to space.";
			}

			// Token: 0x02003504 RID: 13572
			public class CRASHPLAN
			{
				// Token: 0x0400D3DA RID: 54234
				public static LocString NAME = UI.FormatAsLink("Crash Plan", "CRASHPLAN");

				// Token: 0x0400D3DB RID: 54235
				public static LocString DESC = "What goes up, must come down.";
			}

			// Token: 0x02003505 RID: 13573
			public class DURABLELIFESUPPORT
			{
				// Token: 0x0400D3DC RID: 54236
				public static LocString NAME = UI.FormatAsLink("Durable Life Support", "DURABLELIFESUPPORT");

				// Token: 0x0400D3DD RID: 54237
				public static LocString DESC = "Improved devices for extended missions into space.";
			}

			// Token: 0x02003506 RID: 13574
			public class ARTIFICIALFRIENDS
			{
				// Token: 0x0400D3DE RID: 54238
				public static LocString NAME = UI.FormatAsLink("Artificial Friends", "ARTIFICIALFRIENDS");

				// Token: 0x0400D3DF RID: 54239
				public static LocString DESC = "Sweeping advances in companion technology.";
			}

			// Token: 0x02003507 RID: 13575
			public class ROBOTICTOOLS
			{
				// Token: 0x0400D3E0 RID: 54240
				public static LocString NAME = UI.FormatAsLink("Robotic Tools", "ROBOTICTOOLS");

				// Token: 0x0400D3E1 RID: 54241
				public static LocString DESC = "The goal of every great civilization is to one day make itself obsolete.";
			}

			// Token: 0x02003508 RID: 13576
			public class LOGICCONTROL
			{
				// Token: 0x0400D3E2 RID: 54242
				public static LocString NAME = UI.FormatAsLink("Smart Home", "LOGICCONTROL");

				// Token: 0x0400D3E3 RID: 54243
				public static LocString DESC = "Switches that grant full control of building operations within the colony.";
			}

			// Token: 0x02003509 RID: 13577
			public class LOGICCIRCUITS
			{
				// Token: 0x0400D3E4 RID: 54244
				public static LocString NAME = UI.FormatAsLink("Advanced Automation", "LOGICCIRCUITS");

				// Token: 0x0400D3E5 RID: 54245
				public static LocString DESC = "The only limit to colony automation is my own imagination.";
			}

			// Token: 0x0200350A RID: 13578
			public class PARALLELAUTOMATION
			{
				// Token: 0x0400D3E6 RID: 54246
				public static LocString NAME = UI.FormatAsLink("Parallel Automation", "PARALLELAUTOMATION");

				// Token: 0x0400D3E7 RID: 54247
				public static LocString DESC = "Multi-wire automation at a fraction of the space.";
			}

			// Token: 0x0200350B RID: 13579
			public class MULTIPLEXING
			{
				// Token: 0x0400D3E8 RID: 54248
				public static LocString NAME = UI.FormatAsLink("Multiplexing", "MULTIPLEXING");

				// Token: 0x0400D3E9 RID: 54249
				public static LocString DESC = "More choices for Automation signal distribution.";
			}

			// Token: 0x0200350C RID: 13580
			public class VALVEMINIATURIZATION
			{
				// Token: 0x0400D3EA RID: 54250
				public static LocString NAME = UI.FormatAsLink("Valve Miniaturization", "VALVEMINIATURIZATION");

				// Token: 0x0400D3EB RID: 54251
				public static LocString DESC = "Smaller, more efficient pumps for those low-throughput situations.";
			}

			// Token: 0x0200350D RID: 13581
			public class HYDROCARBONPROPULSION
			{
				// Token: 0x0400D3EC RID: 54252
				public static LocString NAME = UI.FormatAsLink("Hydrocarbon Propulsion", "HYDROCARBONPROPULSION");

				// Token: 0x0400D3ED RID: 54253
				public static LocString DESC = "Low-range rocket engines with lots of smoke.";
			}

			// Token: 0x0200350E RID: 13582
			public class BETTERHYDROCARBONPROPULSION
			{
				// Token: 0x0400D3EE RID: 54254
				public static LocString NAME = UI.FormatAsLink("Improved Hydrocarbon Propulsion", "BETTERHYDROCARBONPROPULSION");

				// Token: 0x0400D3EF RID: 54255
				public static LocString DESC = "Mid-range rocket engines with lots of smoke.";
			}

			// Token: 0x0200350F RID: 13583
			public class PRETTYGOODCONDUCTORS
			{
				// Token: 0x0400D3F0 RID: 54256
				public static LocString NAME = UI.FormatAsLink("Low-Resistance Conductors", "PRETTYGOODCONDUCTORS");

				// Token: 0x0400D3F1 RID: 54257
				public static LocString DESC = "Pure-core wires that can handle more " + UI.FormatAsLink("Electrical", "POWER") + " current without overloading.";
			}

			// Token: 0x02003510 RID: 13584
			public class RENEWABLEENERGY
			{
				// Token: 0x0400D3F2 RID: 54258
				public static LocString NAME = UI.FormatAsLink("Renewable Energy", "RENEWABLEENERGY");

				// Token: 0x0400D3F3 RID: 54259
				public static LocString DESC = "Clean, sustainable " + UI.FormatAsLink("Power", "POWER") + " production that produces little to no waste.";
			}

			// Token: 0x02003511 RID: 13585
			public class BASICREFINEMENT
			{
				// Token: 0x0400D3F4 RID: 54260
				public static LocString NAME = UI.FormatAsLink("Brute-Force Refinement", "BASICREFINEMENT");

				// Token: 0x0400D3F5 RID: 54261
				public static LocString DESC = "Low-tech refinement methods for producing clay and renewable sources of sand.";
			}

			// Token: 0x02003512 RID: 13586
			public class REFINEDOBJECTS
			{
				// Token: 0x0400D3F6 RID: 54262
				public static LocString NAME = UI.FormatAsLink("Refined Renovations", "REFINEDOBJECTS");

				// Token: 0x0400D3F7 RID: 54263
				public static LocString DESC = "Improve base infrastructure with new objects crafted from " + UI.FormatAsLink("Refined Metals", "REFINEDMETAL") + ".";
			}

			// Token: 0x02003513 RID: 13587
			public class GENERICSENSORS
			{
				// Token: 0x0400D3F8 RID: 54264
				public static LocString NAME = UI.FormatAsLink("Generic Sensors", "GENERICSENSORS");

				// Token: 0x0400D3F9 RID: 54265
				public static LocString DESC = "Drive automation in a variety of new, inventive ways.";
			}

			// Token: 0x02003514 RID: 13588
			public class DUPETRAFFICCONTROL
			{
				// Token: 0x0400D3FA RID: 54266
				public static LocString NAME = UI.FormatAsLink("Computing", "DUPETRAFFICCONTROL");

				// Token: 0x0400D3FB RID: 54267
				public static LocString DESC = "Virtually extend the boundaries of Duplicant imagination.";
			}

			// Token: 0x02003515 RID: 13589
			public class ADVANCEDSCANNERS
			{
				// Token: 0x0400D3FC RID: 54268
				public static LocString NAME = UI.FormatAsLink("Sensitive Microimaging", "ADVANCEDSCANNERS");

				// Token: 0x0400D3FD RID: 54269
				public static LocString DESC = "Computerized systems do the looking, so Duplicants don't have to.";
			}

			// Token: 0x02003516 RID: 13590
			public class SMELTING
			{
				// Token: 0x0400D3FE RID: 54270
				public static LocString NAME = UI.FormatAsLink("Smelting", "SMELTING");

				// Token: 0x0400D3FF RID: 54271
				public static LocString DESC = "High temperatures facilitate the production of purer, special use metal resources.";
			}

			// Token: 0x02003517 RID: 13591
			public class TRAVELTUBES
			{
				// Token: 0x0400D400 RID: 54272
				public static LocString NAME = UI.FormatAsLink("Transit Tubes", "TRAVELTUBES");

				// Token: 0x0400D401 RID: 54273
				public static LocString DESC = "A wholly futuristic way to move Duplicants around the base.";
			}

			// Token: 0x02003518 RID: 13592
			public class SMARTSTORAGE
			{
				// Token: 0x0400D402 RID: 54274
				public static LocString NAME = UI.FormatAsLink("Smart Storage", "SMARTSTORAGE");

				// Token: 0x0400D403 RID: 54275
				public static LocString DESC = "Completely automate the storage of solid resources.";
			}

			// Token: 0x02003519 RID: 13593
			public class SOLIDTRANSPORT
			{
				// Token: 0x0400D404 RID: 54276
				public static LocString NAME = UI.FormatAsLink("Solid Transport", "SOLIDTRANSPORT");

				// Token: 0x0400D405 RID: 54277
				public static LocString DESC = "Free Duplicants from the drudgery of day-to-day material deliveries with new methods of automation.";
			}

			// Token: 0x0200351A RID: 13594
			public class SOLIDMANAGEMENT
			{
				// Token: 0x0400D406 RID: 54278
				public static LocString NAME = UI.FormatAsLink("Solid Management", "SOLIDMANAGEMENT");

				// Token: 0x0400D407 RID: 54279
				public static LocString DESC = "Make solid decisions in " + UI.FormatAsLink("Solid", "ELEMENTS_SOLID") + " sorting.";
			}

			// Token: 0x0200351B RID: 13595
			public class SOLIDDISTRIBUTION
			{
				// Token: 0x0400D408 RID: 54280
				public static LocString NAME = UI.FormatAsLink("Solid Distribution", "SOLIDDISTRIBUTION");

				// Token: 0x0400D409 RID: 54281
				public static LocString DESC = "Internal rocket hookups for " + UI.FormatAsLink("Solid", "ELEMENTS_SOLID") + " resources.";
			}

			// Token: 0x0200351C RID: 13596
			public class HIGHTEMPFORGING
			{
				// Token: 0x0400D40A RID: 54282
				public static LocString NAME = UI.FormatAsLink("Superheated Forging", "HIGHTEMPFORGING");

				// Token: 0x0400D40B RID: 54283
				public static LocString DESC = "Craft entirely new materials by harnessing the most extreme temperatures.";
			}

			// Token: 0x0200351D RID: 13597
			public class HIGHPRESSUREFORGING
			{
				// Token: 0x0400D40C RID: 54284
				public static LocString NAME = UI.FormatAsLink("Pressurized Forging", "HIGHPRESSUREFORGING");

				// Token: 0x0400D40D RID: 54285
				public static LocString DESC = "High pressure diamond forging.";
			}

			// Token: 0x0200351E RID: 13598
			public class RADIATIONPROTECTION
			{
				// Token: 0x0400D40E RID: 54286
				public static LocString NAME = UI.FormatAsLink("Radiation Protection", "RADIATIONPROTECTION");

				// Token: 0x0400D40F RID: 54287
				public static LocString DESC = "Shield Duplicants from dangerous amounts of radiation.";
			}

			// Token: 0x0200351F RID: 13599
			public class SKYDETECTORS
			{
				// Token: 0x0400D410 RID: 54288
				public static LocString NAME = UI.FormatAsLink("Celestial Detection", "SKYDETECTORS");

				// Token: 0x0400D411 RID: 54289
				public static LocString DESC = "Turn Duplicants' eyes to the skies and discover what undiscovered wonders await out there.";
			}

			// Token: 0x02003520 RID: 13600
			public class JETPACKS
			{
				// Token: 0x0400D412 RID: 54290
				public static LocString NAME = UI.FormatAsLink("Jetpacks", "JETPACKS");

				// Token: 0x0400D413 RID: 54291
				public static LocString DESC = "Objectively the most stylish way for Duplicants to get around.";
			}

			// Token: 0x02003521 RID: 13601
			public class BASICROCKETRY
			{
				// Token: 0x0400D414 RID: 54292
				public static LocString NAME = UI.FormatAsLink("Introductory Rocketry", "BASICROCKETRY");

				// Token: 0x0400D415 RID: 54293
				public static LocString DESC = "Everything required for launching the colony's very first space program.";
			}

			// Token: 0x02003522 RID: 13602
			public class ENGINESI
			{
				// Token: 0x0400D416 RID: 54294
				public static LocString NAME = UI.FormatAsLink("Solid Fuel Combustion", "ENGINESI");

				// Token: 0x0400D417 RID: 54295
				public static LocString DESC = "Rockets that fly further, longer.";
			}

			// Token: 0x02003523 RID: 13603
			public class ENGINESII
			{
				// Token: 0x0400D418 RID: 54296
				public static LocString NAME = UI.FormatAsLink("Hydrocarbon Combustion", "ENGINESII");

				// Token: 0x0400D419 RID: 54297
				public static LocString DESC = "Delve deeper into the vastness of space than ever before.";
			}

			// Token: 0x02003524 RID: 13604
			public class ENGINESIII
			{
				// Token: 0x0400D41A RID: 54298
				public static LocString NAME = UI.FormatAsLink("Cryofuel Combustion", "ENGINESIII");

				// Token: 0x0400D41B RID: 54299
				public static LocString DESC = "With this technology, the sky is your oyster. Go exploring!";
			}

			// Token: 0x02003525 RID: 13605
			public class CRYOFUELPROPULSION
			{
				// Token: 0x0400D41C RID: 54300
				public static LocString NAME = UI.FormatAsLink("Cryofuel Propulsion", "CRYOFUELPROPULSION");

				// Token: 0x0400D41D RID: 54301
				public static LocString DESC = "A semi-powerful engine to propel you further into the galaxy.";
			}

			// Token: 0x02003526 RID: 13606
			public class NUCLEARPROPULSION
			{
				// Token: 0x0400D41E RID: 54302
				public static LocString NAME = UI.FormatAsLink("Radbolt Propulsion", "NUCLEARPROPULSION");

				// Token: 0x0400D41F RID: 54303
				public static LocString DESC = "Radical technology to get you to the stars.";
			}

			// Token: 0x02003527 RID: 13607
			public class ADVANCEDRESOURCEEXTRACTION
			{
				// Token: 0x0400D420 RID: 54304
				public static LocString NAME = UI.FormatAsLink("Advanced Resource Extraction", "ADVANCEDRESOURCEEXTRACTION");

				// Token: 0x0400D421 RID: 54305
				public static LocString DESC = "Bring back souvieners from the stars.";
			}

			// Token: 0x02003528 RID: 13608
			public class CARGOI
			{
				// Token: 0x0400D422 RID: 54306
				public static LocString NAME = UI.FormatAsLink("Solid Cargo", "CARGOI");

				// Token: 0x0400D423 RID: 54307
				public static LocString DESC = "Make extra use of journeys into space by mining and storing useful resources.";
			}

			// Token: 0x02003529 RID: 13609
			public class CARGOII
			{
				// Token: 0x0400D424 RID: 54308
				public static LocString NAME = UI.FormatAsLink("Liquid and Gas Cargo", "CARGOII");

				// Token: 0x0400D425 RID: 54309
				public static LocString DESC = "Extract precious liquids and gases from the far reaches of space, and return with them to the colony.";
			}

			// Token: 0x0200352A RID: 13610
			public class CARGOIII
			{
				// Token: 0x0400D426 RID: 54310
				public static LocString NAME = UI.FormatAsLink("Unique Cargo", "CARGOIII");

				// Token: 0x0400D427 RID: 54311
				public static LocString DESC = "Allow Duplicants to take their friends to see the stars... or simply bring souvenirs back from their travels.";
			}

			// Token: 0x0200352B RID: 13611
			public class NOTIFICATIONSYSTEMS
			{
				// Token: 0x0400D428 RID: 54312
				public static LocString NAME = UI.FormatAsLink("Notification Systems", "NOTIFICATIONSYSTEMS");

				// Token: 0x0400D429 RID: 54313
				public static LocString DESC = "Get all the news you need to know about your complex colony.";
			}

			// Token: 0x0200352C RID: 13612
			public class NUCLEARREFINEMENT
			{
				// Token: 0x0400D42A RID: 54314
				public static LocString NAME = UI.FormatAsLink("Radiation Refinement", "NUCLEAR");

				// Token: 0x0400D42B RID: 54315
				public static LocString DESC = "Refine uranium and generate radiation.";
			}

			// Token: 0x0200352D RID: 13613
			public class NUCLEARRESEARCH
			{
				// Token: 0x0400D42C RID: 54316
				public static LocString NAME = UI.FormatAsLink("Materials Science Research", "NUCLEARRESEARCH");

				// Token: 0x0400D42D RID: 54317
				public static LocString DESC = "Harness sub-atomic particles to study the properties of matter.";
			}

			// Token: 0x0200352E RID: 13614
			public class ADVANCEDNUCLEARRESEARCH
			{
				// Token: 0x0400D42E RID: 54318
				public static LocString NAME = UI.FormatAsLink("More Materials Science Research", "ADVANCEDNUCLEARRESEARCH");

				// Token: 0x0400D42F RID: 54319
				public static LocString DESC = "Harness sub-atomic particles to study the properties of matter even more.";
			}

			// Token: 0x0200352F RID: 13615
			public class NUCLEARSTORAGE
			{
				// Token: 0x0400D430 RID: 54320
				public static LocString NAME = UI.FormatAsLink("Radbolt Containment", "NUCLEARSTORAGE");

				// Token: 0x0400D431 RID: 54321
				public static LocString DESC = "Build a quality cache of radbolts.";
			}

			// Token: 0x02003530 RID: 13616
			public class SOLIDSPACE
			{
				// Token: 0x0400D432 RID: 54322
				public static LocString NAME = UI.FormatAsLink("Solid Control", "SOLIDSPACE");

				// Token: 0x0400D433 RID: 54323
				public static LocString DESC = "Transport and sort " + UI.FormatAsLink("Solid", "ELEMENTS_SOLID") + " resources.";
			}

			// Token: 0x02003531 RID: 13617
			public class HIGHVELOCITYTRANSPORT
			{
				// Token: 0x0400D434 RID: 54324
				public static LocString NAME = UI.FormatAsLink("High Velocity Transport", "HIGHVELOCITY");

				// Token: 0x0400D435 RID: 54325
				public static LocString DESC = "Hurl things through space.";
			}

			// Token: 0x02003532 RID: 13618
			public class MONUMENTS
			{
				// Token: 0x0400D436 RID: 54326
				public static LocString NAME = UI.FormatAsLink("Monuments", "MONUMENTS");

				// Token: 0x0400D437 RID: 54327
				public static LocString DESC = "Monumental art projects.";
			}

			// Token: 0x02003533 RID: 13619
			public class BIOENGINEERING
			{
				// Token: 0x0400D438 RID: 54328
				public static LocString NAME = UI.FormatAsLink("Bioengineering", "BIOENGINEERING");

				// Token: 0x0400D439 RID: 54329
				public static LocString DESC = "Mutation station.";
			}

			// Token: 0x02003534 RID: 13620
			public class SPACECOMBUSTION
			{
				// Token: 0x0400D43A RID: 54330
				public static LocString NAME = UI.FormatAsLink("Advanced Combustion", "SPACECOMBUSTION");

				// Token: 0x0400D43B RID: 54331
				public static LocString DESC = "Sweet advancements in rocket engines.";
			}

			// Token: 0x02003535 RID: 13621
			public class HIGHVELOCITYDESTRUCTION
			{
				// Token: 0x0400D43C RID: 54332
				public static LocString NAME = UI.FormatAsLink("High Velocity Destruction", "HIGHVELOCITYDESTRUCTION");

				// Token: 0x0400D43D RID: 54333
				public static LocString DESC = "Mine the skies.";
			}

			// Token: 0x02003536 RID: 13622
			public class SPACEGAS
			{
				// Token: 0x0400D43E RID: 54334
				public static LocString NAME = UI.FormatAsLink("Advanced Gas Flow", "SPACEGAS");

				// Token: 0x0400D43F RID: 54335
				public static LocString DESC = UI.FormatAsLink("Gas", "ELEMENTS_GASSES") + " engines and transportation for rockets.";
			}

			// Token: 0x02003537 RID: 13623
			public class DATASCIENCE
			{
				// Token: 0x0400D440 RID: 54336
				public static LocString NAME = UI.FormatAsLink("Data Science", "DATASCIENCE");

				// Token: 0x0400D441 RID: 54337
				public static LocString DESC = "The science of making the data work for my Duplicants, instead of the other way around.";
			}

			// Token: 0x02003538 RID: 13624
			public class DATASCIENCEBASEGAME
			{
				// Token: 0x0400D442 RID: 54338
				public static LocString NAME = UI.FormatAsLink("Data Science", "DATASCIENCEBASEGAME");

				// Token: 0x0400D443 RID: 54339
				public static LocString DESC = "The science of making the data work for my Duplicants, instead of the other way around.";
			}
		}
	}
}
