using System;

namespace STRINGS
{
	// Token: 0x02003958 RID: 14680
	public class ITEMS
	{
		// Token: 0x02003959 RID: 14681
		public class PILLS
		{
			// Token: 0x0200395A RID: 14682
			public class PLACEBO
			{
				// Token: 0x0400DEDF RID: 57055
				public static LocString NAME = "Placebo";

				// Token: 0x0400DEE0 RID: 57056
				public static LocString DESC = "A general, all-purpose " + UI.FormatAsLink("Medicine", "MEDICINE") + ".\n\nThe less one knows about it, the better it works.";

				// Token: 0x0400DEE1 RID: 57057
				public static LocString RECIPEDESC = "All-purpose " + UI.FormatAsLink("Medicine", "MEDICINE") + ".";
			}

			// Token: 0x0200395B RID: 14683
			public class BASICBOOSTER
			{
				// Token: 0x0400DEE2 RID: 57058
				public static LocString NAME = "Vitamin Chews";

				// Token: 0x0400DEE3 RID: 57059
				public static LocString DESC = "Minorly reduces the chance of becoming sick.";

				// Token: 0x0400DEE4 RID: 57060
				public static LocString RECIPEDESC = string.Concat(new string[]
				{
					"A supplement that minorly reduces the chance of contracting a ",
					UI.PRE_KEYWORD,
					"Germ",
					UI.PST_KEYWORD,
					"-based ",
					UI.FormatAsLink("Disease", "DISEASE"),
					".\n\nMust be taken daily."
				});
			}

			// Token: 0x0200395C RID: 14684
			public class INTERMEDIATEBOOSTER
			{
				// Token: 0x0400DEE5 RID: 57061
				public static LocString NAME = "Immuno Booster";

				// Token: 0x0400DEE6 RID: 57062
				public static LocString DESC = "Significantly reduces the chance of becoming sick.";

				// Token: 0x0400DEE7 RID: 57063
				public static LocString RECIPEDESC = string.Concat(new string[]
				{
					"A supplement that significantly reduces the chance of contracting a ",
					UI.PRE_KEYWORD,
					"Germ",
					UI.PST_KEYWORD,
					"-based ",
					UI.FormatAsLink("Disease", "DISEASE"),
					".\n\nMust be taken daily."
				});
			}

			// Token: 0x0200395D RID: 14685
			public class ANTIHISTAMINE
			{
				// Token: 0x0400DEE8 RID: 57064
				public static LocString NAME = "Allergy Medication";

				// Token: 0x0400DEE9 RID: 57065
				public static LocString DESC = "Suppresses and prevents allergic reactions.";

				// Token: 0x0400DEEA RID: 57066
				public static LocString RECIPEDESC = "A strong antihistamine Duplicants can take to halt an allergic reaction. " + ITEMS.PILLS.ANTIHISTAMINE.NAME + " will also prevent further reactions from occurring for a short time after ingestion.";
			}

			// Token: 0x0200395E RID: 14686
			public class BASICCURE
			{
				// Token: 0x0400DEEB RID: 57067
				public static LocString NAME = "Curative Tablet";

				// Token: 0x0400DEEC RID: 57068
				public static LocString DESC = "A simple, easy-to-take remedy for minor germ-based diseases.";

				// Token: 0x0400DEED RID: 57069
				public static LocString RECIPEDESC = string.Concat(new string[]
				{
					"Duplicants can take this to cure themselves of minor ",
					UI.PRE_KEYWORD,
					"Germ",
					UI.PST_KEYWORD,
					"-based ",
					UI.FormatAsLink("Diseases", "DISEASE"),
					".\n\nCurative Tablets are very effective against ",
					UI.FormatAsLink("Food Poisoning", "FOODSICKNESS"),
					"."
				});
			}

			// Token: 0x0200395F RID: 14687
			public class INTERMEDIATECURE
			{
				// Token: 0x0400DEEE RID: 57070
				public static LocString NAME = "Medical Pack";

				// Token: 0x0400DEEF RID: 57071
				public static LocString DESC = "A doctor-administered cure for moderate ailments.";

				// Token: 0x0400DEF0 RID: 57072
				public static LocString RECIPEDESC = string.Concat(new string[]
				{
					"A doctor-administered cure for moderate ",
					UI.FormatAsLink("Diseases", "DISEASE"),
					". ",
					ITEMS.PILLS.INTERMEDIATECURE.NAME,
					"s are very effective against ",
					UI.FormatAsLink("Slimelung", "SLIMESICKNESS"),
					".\n\nMust be administered by a Duplicant with the ",
					DUPLICANTS.ROLES.MEDIC.NAME,
					" Skill."
				});
			}

			// Token: 0x02003960 RID: 14688
			public class ADVANCEDCURE
			{
				// Token: 0x0400DEF1 RID: 57073
				public static LocString NAME = "Serum Vial";

				// Token: 0x0400DEF2 RID: 57074
				public static LocString DESC = "A doctor-administered cure for severe ailments.";

				// Token: 0x0400DEF3 RID: 57075
				public static LocString RECIPEDESC = string.Concat(new string[]
				{
					"An extremely powerful medication created to treat severe ",
					UI.FormatAsLink("Diseases", "DISEASE"),
					". ",
					ITEMS.PILLS.ADVANCEDCURE.NAME,
					" is very effective against ",
					UI.FormatAsLink("Zombie Spores", "ZOMBIESPORES"),
					".\n\nMust be administered by a Duplicant with the ",
					DUPLICANTS.ROLES.SENIOR_MEDIC.NAME,
					" Skill."
				});
			}

			// Token: 0x02003961 RID: 14689
			public class BASICRADPILL
			{
				// Token: 0x0400DEF4 RID: 57076
				public static LocString NAME = "Basic Rad Pill";

				// Token: 0x0400DEF5 RID: 57077
				public static LocString DESC = "Increases a Duplicant's natural radiation absorption rate.";

				// Token: 0x0400DEF6 RID: 57078
				public static LocString RECIPEDESC = "A supplement that speeds up the rate at which a Duplicant body absorbs radiation, allowing them to manage increased radiation exposure.\n\nMust be taken daily.";
			}

			// Token: 0x02003962 RID: 14690
			public class INTERMEDIATERADPILL
			{
				// Token: 0x0400DEF7 RID: 57079
				public static LocString NAME = "Intermediate Rad Pill";

				// Token: 0x0400DEF8 RID: 57080
				public static LocString DESC = "Increases a Duplicant's natural radiation absorption rate.";

				// Token: 0x0400DEF9 RID: 57081
				public static LocString RECIPEDESC = "A supplement that speeds up the rate at which a Duplicant body absorbs radiation, allowing them to manage increased radiation exposure.\n\nMust be taken daily.";
			}
		}

		// Token: 0x02003963 RID: 14691
		public class LUBRICATIONSTICK
		{
			// Token: 0x0400DEFA RID: 57082
			public static LocString NAME = UI.FormatAsLink("Gear Balm", "LUBRICATIONSTICK");

			// Token: 0x0400DEFB RID: 57083
			public static LocString SUBHEADER = "Mechanical Lubricant";

			// Token: 0x0400DEFC RID: 57084
			public static LocString DESC = string.Concat(new string[]
			{
				"Provides a small amount of lubricating ",
				UI.FormatAsLink("Gear Oil", "LUBRICATINGOIL"),
				".\n\nCan be produced at the ",
				BUILDINGS.PREFABS.APOTHECARY.NAME,
				"."
			});

			// Token: 0x0400DEFD RID: 57085
			public static LocString RECIPEDESC = "A self-administered mechanical lubricant for Duplicants with bionic parts.";
		}

		// Token: 0x02003964 RID: 14692
		public class BIONIC_BOOSTERS
		{
			// Token: 0x0400DEFE RID: 57086
			public static LocString FABRICATION_SOURCE = "This booster can be manufactured at the {0}.";

			// Token: 0x02003965 RID: 14693
			public class BOOSTER_DIG1
			{
				// Token: 0x0400DEFF RID: 57087
				public static LocString NAME = UI.FormatAsLink("Digging Booster", "BOOSTER_DIG1");

				// Token: 0x0400DF00 RID: 57088
				public static LocString DESC = "Grants a Bionic Duplicant the skill required to dig hard things.";
			}

			// Token: 0x02003966 RID: 14694
			public class BOOSTER_DIG2
			{
				// Token: 0x0400DF01 RID: 57089
				public static LocString NAME = UI.FormatAsLink("Extreme Digging Booster", "BOOSTER_DIG2");

				// Token: 0x0400DF02 RID: 57090
				public static LocString DESC = "Grants a Bionic Duplicant the digging skill required to get through anything.";
			}

			// Token: 0x02003967 RID: 14695
			public class BOOSTER_CONSTRUCT1
			{
				// Token: 0x0400DF03 RID: 57091
				public static LocString NAME = UI.FormatAsLink("Construction Booster", "BOOSTER_CONSTRUCT1");

				// Token: 0x0400DF04 RID: 57092
				public static LocString DESC = "Grants a Bionic Duplicant the ability to build fast, and demolish buildings that others cannot.";
			}

			// Token: 0x02003968 RID: 14696
			public class BOOSTER_FARM1
			{
				// Token: 0x0400DF05 RID: 57093
				public static LocString NAME = UI.FormatAsLink("Crop Tending Booster", "BOOSTER_FARM1");

				// Token: 0x0400DF06 RID: 57094
				public static LocString DESC = "Grants a Bionic Duplicant unparalleled farming and botanical analysis skills.";
			}

			// Token: 0x02003969 RID: 14697
			public class BOOSTER_RANCH1
			{
				// Token: 0x0400DF07 RID: 57095
				public static LocString NAME = UI.FormatAsLink("Ranching Booster", "BOOSTER_RANCH1");

				// Token: 0x0400DF08 RID: 57096
				public static LocString DESC = "Grants a Bionic Duplicant the skills required to care for " + UI.FormatAsLink("Critters", "CREATURES") + " in every way.";
			}

			// Token: 0x0200396A RID: 14698
			public class BOOSTER_COOK1
			{
				// Token: 0x0400DF09 RID: 57097
				public static LocString NAME = UI.FormatAsLink("Grilling Booster", "BOOSTER_COOK1");

				// Token: 0x0400DF0A RID: 57098
				public static LocString DESC = "Grants a Bionic Duplicant deliciously professional culinary skills.";
			}

			// Token: 0x0200396B RID: 14699
			public class BOOSTER_ART1
			{
				// Token: 0x0400DF0B RID: 57099
				public static LocString NAME = UI.FormatAsLink("Masterworks Art Booster", "BOOSTER_ART1");

				// Token: 0x0400DF0C RID: 57100
				public static LocString DESC = "Grants a Bionic Duplicant flawless decorating skills.";
			}

			// Token: 0x0200396C RID: 14700
			public class BOOSTER_RESEARCH1
			{
				// Token: 0x0400DF0D RID: 57101
				public static LocString NAME = UI.FormatAsLink("Researching Booster", "BOOSTER_RESEARCH1");

				// Token: 0x0400DF0E RID: 57102
				public static LocString DESC = "Grants a Bionic Duplicant the expertise required to study " + UI.FormatAsLink("geysers", "GEYSERS") + " and other advanced topics.";
			}

			// Token: 0x0200396D RID: 14701
			public class BOOSTER_RESEARCH2
			{
				// Token: 0x0400DF0F RID: 57103
				public static LocString NAME = UI.FormatAsLink("Astronomy Booster", "BOOSTER_RESEARCH2");

				// Token: 0x0400DF10 RID: 57104
				public static LocString DESC = "Grants a Bionic Duplicant a keen grasp of science and usage of space-research buildings.";
			}

			// Token: 0x0200396E RID: 14702
			public class BOOSTER_RESEARCH3
			{
				// Token: 0x0400DF11 RID: 57105
				public static LocString NAME = UI.FormatAsLink("Applied Sciences Booster", "BOOSTER_RESEARCH3");

				// Token: 0x0400DF12 RID: 57106
				public static LocString DESC = "Grants a Bionic Duplicant a deeply pragmatic approach to scientific research.";
			}

			// Token: 0x0200396F RID: 14703
			public class BOOSTER_PILOT1
			{
				// Token: 0x0400DF13 RID: 57107
				public static LocString NAME = UI.FormatAsLink("Piloting Booster", "BOOSTER_PILOT1");

				// Token: 0x0400DF14 RID: 57108
				public static LocString DESC = "Grants a Bionic Duplicant the expertise required to explore the skies in person.";
			}

			// Token: 0x02003970 RID: 14704
			public class BOOSTER_PILOTVANILLA1
			{
				// Token: 0x0400DF15 RID: 57109
				public static LocString NAME = UI.FormatAsLink("Rocketry Booster", "BOOSTER_PILOTVANILLA1");

				// Token: 0x0400DF16 RID: 57110
				public static LocString DESC = "Grants a Bionic Duplicant the expertise required to command a rocket.";
			}

			// Token: 0x02003971 RID: 14705
			public class BOOSTER_SUITS1
			{
				// Token: 0x0400DF17 RID: 57111
				public static LocString NAME = UI.FormatAsLink("Suit Training Booster", "BOOSTER_SUITS1");

				// Token: 0x0400DF18 RID: 57112
				public static LocString DESC = "Enables a Bionic Duplicant to maximize durability of equipped " + UI.FormatAsLink("Exosuits", "EQUIPMENT") + " and maintain their runspeed.";
			}

			// Token: 0x02003972 RID: 14706
			public class BOOSTER_CARRY1
			{
				// Token: 0x0400DF19 RID: 57113
				public static LocString NAME = UI.FormatAsLink("Strength Booster", "BOOSTER_CARRY1");

				// Token: 0x0400DF1A RID: 57114
				public static LocString DESC = "Grants a Bionic Duplicant increased carrying capacity and athletic prowess.";
			}

			// Token: 0x02003973 RID: 14707
			public class BOOSTER_OP1
			{
				// Token: 0x0400DF1B RID: 57115
				public static LocString NAME = UI.FormatAsLink("Electrical Engineering Booster", "BOOSTER_OP1");

				// Token: 0x0400DF1C RID: 57116
				public static LocString DESC = "Grants a Bionic Duplicant the skills requried to tinker and solder to their heart's content.";
			}

			// Token: 0x02003974 RID: 14708
			public class BOOSTER_OP2
			{
				// Token: 0x0400DF1D RID: 57117
				public static LocString NAME = UI.FormatAsLink("Mechatronics Engineering Booster", "BOOSTER_OP2");

				// Token: 0x0400DF1E RID: 57118
				public static LocString DESC = "Grants a Bionic Duplicant complete mastery of engineering skills.";
			}

			// Token: 0x02003975 RID: 14709
			public class BOOSTER_MEDICINE1
			{
				// Token: 0x0400DF1F RID: 57119
				public static LocString NAME = UI.FormatAsLink("Advanced Medical Booster", "BOOSTER_MEDICINE1");

				// Token: 0x0400DF20 RID: 57120
				public static LocString DESC = "Grants a Bionic Duplicant the ability to perform all doctoring errands.";
			}

			// Token: 0x02003976 RID: 14710
			public class BOOSTER_TIDY1
			{
				// Token: 0x0400DF21 RID: 57121
				public static LocString NAME = UI.FormatAsLink("Tidying Booster", "BOOSTER_TIDY1");

				// Token: 0x0400DF22 RID: 57122
				public static LocString DESC = "Grants a Bionic Duplicant the full range of tidying skills, including blasting unwanted meteors out of the sky.";
			}
		}

		// Token: 0x02003977 RID: 14711
		public class FOOD
		{
			// Token: 0x0400DF23 RID: 57123
			public static LocString COMPOST = "Compost";

			// Token: 0x02003978 RID: 14712
			public class FOODSPLAT
			{
				// Token: 0x0400DF24 RID: 57124
				public static LocString NAME = "Food Splatter";

				// Token: 0x0400DF25 RID: 57125
				public static LocString DESC = "Food smeared on the wall from a recent Food Fight";
			}

			// Token: 0x02003979 RID: 14713
			public class BURGER
			{
				// Token: 0x0400DF26 RID: 57126
				public static LocString NAME = UI.FormatAsLink("Frost Burger", "BURGER");

				// Token: 0x0400DF27 RID: 57127
				public static LocString DESC = string.Concat(new string[]
				{
					UI.FormatAsLink("Meat", "MEAT"),
					" and ",
					UI.FormatAsLink("Lettuce", "LETTUCE"),
					" on a chilled ",
					UI.FormatAsLink("Frost Bun", "COLDWHEATBREAD"),
					".\n\nIt's the only burger best served cold."
				});

				// Token: 0x0400DF28 RID: 57128
				public static LocString RECIPEDESC = string.Concat(new string[]
				{
					UI.FormatAsLink("Meat", "MEAT"),
					" and ",
					UI.FormatAsLink("Lettuce", "LETTUCE"),
					" on a chilled ",
					UI.FormatAsLink("Frost Bun", "COLDWHEATBREAD"),
					"."
				});

				// Token: 0x0200397A RID: 14714
				public class DEHYDRATED
				{
					// Token: 0x0400DF29 RID: 57129
					public static LocString NAME = "Dried Frost Burger";

					// Token: 0x0400DF2A RID: 57130
					public static LocString DESC = string.Concat(new string[]
					{
						"A dehydrated ",
						UI.FormatAsLink("Frost Burger", "BURGER"),
						" ration. It must be rehydrated in order to be considered ",
						UI.FormatAsLink("Food", "FOOD"),
						".\n\nDry rations have no expiry date."
					});
				}
			}

			// Token: 0x0200397B RID: 14715
			public class FIELDRATION
			{
				// Token: 0x0400DF2B RID: 57131
				public static LocString NAME = UI.FormatAsLink("Nutrient Bar", "FIELDRATION");

				// Token: 0x0400DF2C RID: 57132
				public static LocString DESC = "A nourishing nutrient paste, sandwiched between thin wafer layers.";
			}

			// Token: 0x0200397C RID: 14716
			public class MUSHBAR
			{
				// Token: 0x0400DF2D RID: 57133
				public static LocString NAME = UI.FormatAsLink("Mush Bar", "MUSHBAR");

				// Token: 0x0400DF2E RID: 57134
				public static LocString DESC = "An edible, putrefied mudslop.\n\nMush Bars are preferable to starvation, but only just barely.";

				// Token: 0x0400DF2F RID: 57135
				public static LocString RECIPEDESC = "An edible, putrefied mudslop.\n\n" + ITEMS.FOOD.MUSHBAR.NAME + "s are preferable to starvation, but only just barely.";
			}

			// Token: 0x0200397D RID: 14717
			public class MUSHROOMWRAP
			{
				// Token: 0x0400DF30 RID: 57136
				public static LocString NAME = UI.FormatAsLink("Mushroom Wrap", "MUSHROOMWRAP");

				// Token: 0x0400DF31 RID: 57137
				public static LocString DESC = string.Concat(new string[]
				{
					"Flavorful ",
					UI.FormatAsLink("Mushrooms", "MUSHROOM"),
					" wrapped in ",
					UI.FormatAsLink("Lettuce", "LETTUCE"),
					".\n\nIt has an earthy flavor punctuated by a refreshing crunch."
				});

				// Token: 0x0400DF32 RID: 57138
				public static LocString RECIPEDESC = string.Concat(new string[]
				{
					"Flavorful ",
					UI.FormatAsLink("Mushrooms", "MUSHROOM"),
					" wrapped in ",
					UI.FormatAsLink("Lettuce", "LETTUCE"),
					"."
				});

				// Token: 0x0200397E RID: 14718
				public class DEHYDRATED
				{
					// Token: 0x0400DF33 RID: 57139
					public static LocString NAME = "Dried Mushroom Wrap";

					// Token: 0x0400DF34 RID: 57140
					public static LocString DESC = string.Concat(new string[]
					{
						"A dehydrated ",
						UI.FormatAsLink("Mushroom Wrap", "MUSHROOMWRAP"),
						" ration. It must be rehydrated in order to be considered ",
						UI.FormatAsLink("Food", "FOOD"),
						".\n\nDry rations have no expiry date."
					});
				}
			}

			// Token: 0x0200397F RID: 14719
			public class MICROWAVEDLETTUCE
			{
				// Token: 0x0400DF35 RID: 57141
				public static LocString NAME = UI.FormatAsLink("Microwaved Lettuce", "MICROWAVEDLETTUCE");

				// Token: 0x0400DF36 RID: 57142
				public static LocString DESC = UI.FormatAsLink("Lettuce", "LETTUCE") + " scrumptiously wilted in the " + BUILDINGS.PREFABS.GAMMARAYOVEN.NAME + ".";

				// Token: 0x0400DF37 RID: 57143
				public static LocString RECIPEDESC = UI.FormatAsLink("Lettuce", "LETTUCE") + " scrumptiously wilted in the " + BUILDINGS.PREFABS.GAMMARAYOVEN.NAME + ".";
			}

			// Token: 0x02003980 RID: 14720
			public class GAMMAMUSH
			{
				// Token: 0x0400DF38 RID: 57144
				public static LocString NAME = UI.FormatAsLink("Gamma Mush", "GAMMAMUSH");

				// Token: 0x0400DF39 RID: 57145
				public static LocString DESC = "A disturbingly delicious mixture of irradiated dirt and water.";

				// Token: 0x0400DF3A RID: 57146
				public static LocString RECIPEDESC = UI.FormatAsLink("Mush Fry", "FRIEDMUSHBAR") + " reheated in a " + BUILDINGS.PREFABS.GAMMARAYOVEN.NAME + ".";
			}

			// Token: 0x02003981 RID: 14721
			public class FRUITCAKE
			{
				// Token: 0x0400DF3B RID: 57147
				public static LocString NAME = UI.FormatAsLink("Berry Sludge", "FRUITCAKE");

				// Token: 0x0400DF3C RID: 57148
				public static LocString DESC = "A mashed up " + UI.FormatAsLink("Bristle Berry", "PRICKLEFRUIT") + " sludge with an exceptionally long shelf life.\n\nIts aggressive, overbearing sweetness can leave the tongue feeling temporarily numb.";

				// Token: 0x0400DF3D RID: 57149
				public static LocString RECIPEDESC = "A mashed up " + UI.FormatAsLink("Bristle Berry", "PRICKLEFRUIT") + " sludge with an exceptionally long shelf life.";
			}

			// Token: 0x02003982 RID: 14722
			public class POPCORN
			{
				// Token: 0x0400DF3E RID: 57150
				public static LocString NAME = UI.FormatAsLink("Popcorn", "POPCORN");

				// Token: 0x0400DF3F RID: 57151
				public static LocString DESC = UI.FormatAsLink("Sleet Wheat Grain", "COLDWHEATSEED") + " popped in a " + BUILDINGS.PREFABS.GAMMARAYOVEN.NAME + ".\n\nCompletely devoid of any fancy flavorings.";

				// Token: 0x0400DF40 RID: 57152
				public static LocString RECIPEDESC = "Gamma-radiated " + UI.FormatAsLink("Sleet Wheat Grain", "COLDWHEATSEED") + ".";
			}

			// Token: 0x02003983 RID: 14723
			public class SUSHI
			{
				// Token: 0x0400DF41 RID: 57153
				public static LocString NAME = UI.FormatAsLink("Sushi", "SUSHI");

				// Token: 0x0400DF42 RID: 57154
				public static LocString DESC = string.Concat(new string[]
				{
					"Raw ",
					UI.FormatAsLink("Pacu Fillet", "FISHMEAT"),
					" wrapped with fresh ",
					UI.FormatAsLink("Lettuce", "LETTUCE"),
					".\n\nWhile the salt of the lettuce may initially overpower the flavor, a keen palate can discern the subtle sweetness of the fillet beneath."
				});

				// Token: 0x0400DF43 RID: 57155
				public static LocString RECIPEDESC = string.Concat(new string[]
				{
					"Raw ",
					UI.FormatAsLink("Pacu Fillet", "FISHMEAT"),
					" wrapped with fresh ",
					UI.FormatAsLink("Lettuce", "LETTUCE"),
					"."
				});
			}

			// Token: 0x02003984 RID: 14724
			public class HATCHEGG
			{
				// Token: 0x0400DF44 RID: 57156
				public static LocString NAME = CREATURES.SPECIES.HATCH.EGG_NAME;

				// Token: 0x0400DF45 RID: 57157
				public static LocString DESC = string.Concat(new string[]
				{
					"An egg laid by a ",
					UI.FormatAsLink("Hatch", "HATCH"),
					".\n\nIf incubated, it will hatch into a ",
					UI.FormatAsLink("Hatchling", "HATCH"),
					"."
				});

				// Token: 0x0400DF46 RID: 57158
				public static LocString RECIPEDESC = "An egg laid by a " + UI.FormatAsLink("Hatch", "HATCH") + ".";
			}

			// Token: 0x02003985 RID: 14725
			public class DRECKOEGG
			{
				// Token: 0x0400DF47 RID: 57159
				public static LocString NAME = CREATURES.SPECIES.DRECKO.EGG_NAME;

				// Token: 0x0400DF48 RID: 57160
				public static LocString DESC = string.Concat(new string[]
				{
					"An egg laid by a ",
					UI.FormatAsLink("Drecko", "DRECKO"),
					".\n\nIf incubated, it will hatch into a new ",
					UI.FormatAsLink("Drecklet", "DRECKO"),
					"."
				});

				// Token: 0x0400DF49 RID: 57161
				public static LocString RECIPEDESC = "An egg laid by a " + UI.FormatAsLink("Drecko", "DRECKO") + ".";
			}

			// Token: 0x02003986 RID: 14726
			public class LIGHTBUGEGG
			{
				// Token: 0x0400DF4A RID: 57162
				public static LocString NAME = CREATURES.SPECIES.LIGHTBUG.EGG_NAME;

				// Token: 0x0400DF4B RID: 57163
				public static LocString DESC = string.Concat(new string[]
				{
					"An egg laid by a ",
					UI.FormatAsLink("Shine Bug", "LIGHTBUG"),
					".\n\nIf incubated, it will hatch into a ",
					UI.FormatAsLink("Shine Nymph", "LIGHTBUG"),
					"."
				});

				// Token: 0x0400DF4C RID: 57164
				public static LocString RECIPEDESC = "An egg laid by a " + UI.FormatAsLink("Shine Bug", "LIGHTBUG") + ".";
			}

			// Token: 0x02003987 RID: 14727
			public class LETTUCE
			{
				// Token: 0x0400DF4D RID: 57165
				public static LocString NAME = UI.FormatAsLink("Lettuce", "LETTUCE");

				// Token: 0x0400DF4E RID: 57166
				public static LocString DESC = "Crunchy, slightly salty leaves from a " + UI.FormatAsLink("Waterweed", "SEALETTUCE") + " plant.";

				// Token: 0x0400DF4F RID: 57167
				public static LocString RECIPEDESC = "Edible roughage from a " + UI.FormatAsLink("Waterweed", "SEALETTUCE") + ".";
			}

			// Token: 0x02003988 RID: 14728
			public class PASTA
			{
				// Token: 0x0400DF50 RID: 57168
				public static LocString NAME = UI.FormatAsLink("Pasta", "PASTA");

				// Token: 0x0400DF51 RID: 57169
				public static LocString DESC = "pasta made from egg and wheat";

				// Token: 0x0400DF52 RID: 57170
				public static LocString RECIPEDESC = "pasta made from egg and wheat";
			}

			// Token: 0x02003989 RID: 14729
			public class PANCAKES
			{
				// Token: 0x0400DF53 RID: 57171
				public static LocString NAME = UI.FormatAsLink("Soufflé Pancakes", "PANCAKES");

				// Token: 0x0400DF54 RID: 57172
				public static LocString DESC = string.Concat(new string[]
				{
					"Sweet discs made from ",
					UI.FormatAsLink("Raw Egg", "RAWEGG"),
					" and ",
					UI.FormatAsLink("Sleet Wheat Grain", "COLDWHEATSEED"),
					".\n\nThey're so thick!"
				});

				// Token: 0x0400DF55 RID: 57173
				public static LocString RECIPEDESC = string.Concat(new string[]
				{
					"Sweet discs made from ",
					UI.FormatAsLink("Raw Egg", "RAWEGG"),
					" and ",
					UI.FormatAsLink("Sleet Wheat Grain", "COLDWHEATSEED"),
					"."
				});
			}

			// Token: 0x0200398A RID: 14730
			public class OILFLOATEREGG
			{
				// Token: 0x0400DF56 RID: 57174
				public static LocString NAME = CREATURES.SPECIES.OILFLOATER.EGG_NAME;

				// Token: 0x0400DF57 RID: 57175
				public static LocString DESC = string.Concat(new string[]
				{
					"An egg laid by a ",
					UI.FormatAsLink("Slickster", "OILFLOATER"),
					".\n\nIf incubated, it will hatch into a ",
					UI.FormatAsLink("Slickster Larva", "OILFLOATER"),
					"."
				});

				// Token: 0x0400DF58 RID: 57176
				public static LocString RECIPEDESC = "An egg laid by a " + UI.FormatAsLink("Slickster", "OILFLOATER") + ".";
			}

			// Token: 0x0200398B RID: 14731
			public class PUFTEGG
			{
				// Token: 0x0400DF59 RID: 57177
				public static LocString NAME = CREATURES.SPECIES.PUFT.EGG_NAME;

				// Token: 0x0400DF5A RID: 57178
				public static LocString DESC = string.Concat(new string[]
				{
					"An egg laid by a ",
					UI.FormatAsLink("Puft", "PUFT"),
					".\n\nIf incubated, it will hatch into a ",
					UI.FormatAsLink("Puftlet", "PUFT"),
					"."
				});

				// Token: 0x0400DF5B RID: 57179
				public static LocString RECIPEDESC = "An egg laid by a " + CREATURES.SPECIES.PUFT.NAME + ".";
			}

			// Token: 0x0200398C RID: 14732
			public class FISHMEAT
			{
				// Token: 0x0400DF5C RID: 57180
				public static LocString NAME = UI.FormatAsLink("Pacu Fillet", "FISHMEAT");

				// Token: 0x0400DF5D RID: 57181
				public static LocString DESC = "An uncooked fillet from a very dead " + CREATURES.SPECIES.PACU.NAME + ". Yum!";
			}

			// Token: 0x0200398D RID: 14733
			public class MEAT
			{
				// Token: 0x0400DF5E RID: 57182
				public static LocString NAME = UI.FormatAsLink("Meat", "MEAT");

				// Token: 0x0400DF5F RID: 57183
				public static LocString DESC = "Uncooked meat from a very dead critter. Yum!";
			}

			// Token: 0x0200398E RID: 14734
			public class PLANTMEAT
			{
				// Token: 0x0400DF60 RID: 57184
				public static LocString NAME = UI.FormatAsLink("Plant Meat", "PLANTMEAT");

				// Token: 0x0400DF61 RID: 57185
				public static LocString DESC = "Planty plant meat from a plant. How nice!";
			}

			// Token: 0x0200398F RID: 14735
			public class SHELLFISHMEAT
			{
				// Token: 0x0400DF62 RID: 57186
				public static LocString NAME = UI.FormatAsLink("Raw Shellfish", "SHELLFISHMEAT");

				// Token: 0x0400DF63 RID: 57187
				public static LocString DESC = "An uncooked chunk of very dead " + CREATURES.SPECIES.CRAB.VARIANT_FRESH_WATER.NAME + ". Yum!";
			}

			// Token: 0x02003990 RID: 14736
			public class MUSHROOM
			{
				// Token: 0x0400DF64 RID: 57188
				public static LocString NAME = UI.FormatAsLink("Mushroom", "MUSHROOM");

				// Token: 0x0400DF65 RID: 57189
				public static LocString DESC = "An edible, flavorless fungus that grew in the dark.";
			}

			// Token: 0x02003991 RID: 14737
			public class COOKEDFISH
			{
				// Token: 0x0400DF66 RID: 57190
				public static LocString NAME = UI.FormatAsLink("Cooked Seafood", "COOKEDFISH");

				// Token: 0x0400DF67 RID: 57191
				public static LocString DESC = "A cooked piece of freshly caught aquatic critter.\n\nUnsurprisingly, it tastes a bit fishy.";

				// Token: 0x0400DF68 RID: 57192
				public static LocString RECIPEDESC = "A cooked piece of freshly caught aquatic critter.";
			}

			// Token: 0x02003992 RID: 14738
			public class COOKEDMEAT
			{
				// Token: 0x0400DF69 RID: 57193
				public static LocString NAME = UI.FormatAsLink("Barbeque", "COOKEDMEAT");

				// Token: 0x0400DF6A RID: 57194
				public static LocString DESC = "The cooked meat of a defeated critter.\n\nIt has a delightful smoky aftertaste.";

				// Token: 0x0400DF6B RID: 57195
				public static LocString RECIPEDESC = "The cooked meat of a defeated critter.";
			}

			// Token: 0x02003993 RID: 14739
			public class FRIESCARROT
			{
				// Token: 0x0400DF6C RID: 57196
				public static LocString NAME = UI.FormatAsLink("Squash Fries", "FRIESCARROT");

				// Token: 0x0400DF6D RID: 57197
				public static LocString DESC = "Irresistibly crunchy.\n\nBest eaten hot.";

				// Token: 0x0400DF6E RID: 57198
				public static LocString RECIPEDESC = string.Concat(new string[]
				{
					"Crunchy sticks of ",
					UI.FormatAsLink("Plume Squash", "CARROT"),
					" deep-fried in ",
					UI.FormatAsLink("Tallow", "TALLOW"),
					"."
				});
			}

			// Token: 0x02003994 RID: 14740
			public class DEEPFRIEDFISH
			{
				// Token: 0x0400DF6F RID: 57199
				public static LocString NAME = UI.FormatAsLink("Fish Taco", "DEEPFRIEDFISH");

				// Token: 0x0400DF70 RID: 57200
				public static LocString DESC = "Deep-fried fish cradled in a crunchy fin.";

				// Token: 0x0400DF71 RID: 57201
				public static LocString RECIPEDESC = UI.FormatAsLink("Pacu Fillet", "FISHMEAT") + " lightly battered and deep-fried in " + UI.FormatAsLink("Tallow", "TALLOW") + ".";
			}

			// Token: 0x02003995 RID: 14741
			public class DEEPFRIEDSHELLFISH
			{
				// Token: 0x0400DF72 RID: 57202
				public static LocString NAME = UI.FormatAsLink("Shellfish Tempura", "DEEPFRIEDSHELLFISH");

				// Token: 0x0400DF73 RID: 57203
				public static LocString DESC = "A crispy deep-fried critter claw.";

				// Token: 0x0400DF74 RID: 57204
				public static LocString RECIPEDESC = string.Concat(new string[]
				{
					"A tender chunk of battered ",
					UI.FormatAsLink("Raw Shellfish", "SHELLFISHMEAT"),
					" deep-fried in ",
					UI.FormatAsLink("Tallow", "TALLOW"),
					"."
				});
			}

			// Token: 0x02003996 RID: 14742
			public class DEEPFRIEDMEAT
			{
				// Token: 0x0400DF75 RID: 57205
				public static LocString NAME = UI.FormatAsLink("Deep Fried Steak", "DEEPFRIEDMEAT");

				// Token: 0x0400DF76 RID: 57206
				public static LocString DESC = "A juicy slab of meat with a crunchy deep-fried upper layer.";

				// Token: 0x0400DF77 RID: 57207
				public static LocString RECIPEDESC = string.Concat(new string[]
				{
					"A juicy slab of ",
					UI.FormatAsLink("Raw Meat", "MEAT"),
					" deep-fried in ",
					UI.FormatAsLink("Tallow", "TALLOW"),
					"."
				});
			}

			// Token: 0x02003997 RID: 14743
			public class DEEPFRIEDNOSH
			{
				// Token: 0x0400DF78 RID: 57208
				public static LocString NAME = UI.FormatAsLink("Nosh Noms", "DEEPFRIEDNOSH");

				// Token: 0x0400DF79 RID: 57209
				public static LocString DESC = "A snackable handful of crunchy beans.";

				// Token: 0x0400DF7A RID: 57210
				public static LocString RECIPEDESC = string.Concat(new string[]
				{
					"A crunchy stack of ",
					UI.FormatAsLink("Nosh Beans", "BEANPLANTSEED"),
					" deep-fried in ",
					UI.FormatAsLink("Tallow", "TALLOW"),
					"."
				});
			}

			// Token: 0x02003998 RID: 14744
			public class PICKLEDMEAL
			{
				// Token: 0x0400DF7B RID: 57211
				public static LocString NAME = UI.FormatAsLink("Pickled Meal", "PICKLEDMEAL");

				// Token: 0x0400DF7C RID: 57212
				public static LocString DESC = "Meal Lice preserved in vinegar.\n\nIt's a rarely acquired taste.";

				// Token: 0x0400DF7D RID: 57213
				public static LocString RECIPEDESC = ITEMS.FOOD.BASICPLANTFOOD.NAME + " regrettably preserved in vinegar.";
			}

			// Token: 0x02003999 RID: 14745
			public class FRIEDMUSHBAR
			{
				// Token: 0x0400DF7E RID: 57214
				public static LocString NAME = UI.FormatAsLink("Mush Fry", "FRIEDMUSHBAR");

				// Token: 0x0400DF7F RID: 57215
				public static LocString DESC = "Pan-fried, solidified mudslop.\n\nThe inside is almost completely uncooked, despite the crunch on the outside.";

				// Token: 0x0400DF80 RID: 57216
				public static LocString RECIPEDESC = "Pan-fried, solidified mudslop.";
			}

			// Token: 0x0200399A RID: 14746
			public class RAWEGG
			{
				// Token: 0x0400DF81 RID: 57217
				public static LocString NAME = UI.FormatAsLink("Raw Egg", "RAWEGG");

				// Token: 0x0400DF82 RID: 57218
				public static LocString DESC = "A raw Egg that has been cracked open for use in " + UI.FormatAsLink("Food", "FOOD") + " preparation.\n\nIt will never hatch.";

				// Token: 0x0400DF83 RID: 57219
				public static LocString RECIPEDESC = "A raw egg that has been cracked open for use in " + UI.FormatAsLink("Food", "FOOD") + " preparation.";
			}

			// Token: 0x0200399B RID: 14747
			public class COOKEDEGG
			{
				// Token: 0x0400DF84 RID: 57220
				public static LocString NAME = UI.FormatAsLink("Omelette", "COOKEDEGG");

				// Token: 0x0400DF85 RID: 57221
				public static LocString DESC = "Fluffed and folded Egg innards.\n\nIt turns out you do, in fact, have to break a few eggs to make it.";

				// Token: 0x0400DF86 RID: 57222
				public static LocString RECIPEDESC = "Fluffed and folded egg innards.";
			}

			// Token: 0x0200399C RID: 14748
			public class FRIEDMUSHROOM
			{
				// Token: 0x0400DF87 RID: 57223
				public static LocString NAME = UI.FormatAsLink("Fried Mushroom", "FRIEDMUSHROOM");

				// Token: 0x0400DF88 RID: 57224
				public static LocString DESC = "A pan-fried dish made with a fruiting " + UI.FormatAsLink("Dusk Cap", "MUSHROOM") + ".\n\nIt has a thick, savory flavor with subtle earthy undertones.";

				// Token: 0x0400DF89 RID: 57225
				public static LocString RECIPEDESC = "A pan-fried dish made with a fruiting " + UI.FormatAsLink("Dusk Cap", "MUSHROOM") + ".";
			}

			// Token: 0x0200399D RID: 14749
			public class COOKEDPIKEAPPLE
			{
				// Token: 0x0400DF8A RID: 57226
				public static LocString NAME = UI.FormatAsLink("Pikeapple Skewer", "COOKEDPIKEAPPLE");

				// Token: 0x0400DF8B RID: 57227
				public static LocString DESC = "Grilling a " + UI.FormatAsLink("Pikeapple", "HARDSKINBERRY") + " softens its spikes, making it slighly less awkward to eat.\n\nIt does not diminish the smell.";

				// Token: 0x0400DF8C RID: 57228
				public static LocString RECIPEDESC = "A grilled dish made with a fruiting " + UI.FormatAsLink("Pikeapple", "HARDSKINBERRY") + ".";
			}

			// Token: 0x0200399E RID: 14750
			public class PRICKLEFRUIT
			{
				// Token: 0x0400DF8D RID: 57229
				public static LocString NAME = UI.FormatAsLink("Bristle Berry", "PRICKLEFRUIT");

				// Token: 0x0400DF8E RID: 57230
				public static LocString DESC = "A sweet, mostly pleasant-tasting fruit covered in prickly barbs.";
			}

			// Token: 0x0200399F RID: 14751
			public class GRILLEDPRICKLEFRUIT
			{
				// Token: 0x0400DF8F RID: 57231
				public static LocString NAME = UI.FormatAsLink("Gristle Berry", "GRILLEDPRICKLEFRUIT");

				// Token: 0x0400DF90 RID: 57232
				public static LocString DESC = "The grilled bud of a " + UI.FormatAsLink("Bristle Berry", "PRICKLEFRUIT") + ".\n\nHeat unlocked an exquisite taste in the fruit, though the burnt spines leave something to be desired.";

				// Token: 0x0400DF91 RID: 57233
				public static LocString RECIPEDESC = "The grilled bud of a " + UI.FormatAsLink("Bristle Berry", "PRICKLEFRUIT") + ".";
			}

			// Token: 0x020039A0 RID: 14752
			public class SWAMPFRUIT
			{
				// Token: 0x0400DF92 RID: 57234
				public static LocString NAME = UI.FormatAsLink("Bog Jelly", "SWAMPFRUIT");

				// Token: 0x0400DF93 RID: 57235
				public static LocString DESC = "A fruit with an outer film that contains chewy gelatinous cubes.";
			}

			// Token: 0x020039A1 RID: 14753
			public class SWAMPDELIGHTS
			{
				// Token: 0x0400DF94 RID: 57236
				public static LocString NAME = UI.FormatAsLink("Swampy Delights", "SWAMPDELIGHTS");

				// Token: 0x0400DF95 RID: 57237
				public static LocString DESC = "Dried gelatinous cubes from a " + UI.FormatAsLink("Bog Jelly", "SWAMPFRUIT") + ".\n\nEach cube has a wonderfully chewy texture and is lightly coated in a delicate powder.";

				// Token: 0x0400DF96 RID: 57238
				public static LocString RECIPEDESC = "Dried gelatinous cubes from a " + UI.FormatAsLink("Bog Jelly", "SWAMPFRUIT") + ".";
			}

			// Token: 0x020039A2 RID: 14754
			public class WORMBASICFRUIT
			{
				// Token: 0x0400DF97 RID: 57239
				public static LocString NAME = UI.FormatAsLink("Spindly Grubfruit", "WORMBASICFRUIT");

				// Token: 0x0400DF98 RID: 57240
				public static LocString DESC = "A " + UI.FormatAsLink("Grubfruit", "WORMSUPERFRUIT") + " that failed to develop properly.\n\nIt is nonetheless edible, and vaguely tasty.";
			}

			// Token: 0x020039A3 RID: 14755
			public class WORMBASICFOOD
			{
				// Token: 0x0400DF99 RID: 57241
				public static LocString NAME = UI.FormatAsLink("Roast Grubfruit Nut", "WORMBASICFOOD");

				// Token: 0x0400DF9A RID: 57242
				public static LocString DESC = "Slow roasted " + UI.FormatAsLink("Spindly Grubfruit", "WORMBASICFRUIT") + ".\n\nIt has a smoky aroma and tastes of coziness.";

				// Token: 0x0400DF9B RID: 57243
				public static LocString RECIPEDESC = "Slow roasted " + UI.FormatAsLink("Spindly Grubfruit", "WORMBASICFRUIT") + ".";
			}

			// Token: 0x020039A4 RID: 14756
			public class WORMSUPERFRUIT
			{
				// Token: 0x0400DF9C RID: 57244
				public static LocString NAME = UI.FormatAsLink("Grubfruit", "WORMSUPERFRUIT");

				// Token: 0x0400DF9D RID: 57245
				public static LocString DESC = "A plump, healthy fruit with a honey-like taste.";
			}

			// Token: 0x020039A5 RID: 14757
			public class WORMSUPERFOOD
			{
				// Token: 0x0400DF9E RID: 57246
				public static LocString NAME = UI.FormatAsLink("Grubfruit Preserve", "WORMSUPERFOOD");

				// Token: 0x0400DF9F RID: 57247
				public static LocString DESC = string.Concat(new string[]
				{
					"A long lasting ",
					UI.FormatAsLink("Grubfruit", "WORMSUPERFRUIT"),
					" jam preserved in ",
					UI.FormatAsLink("Sucrose", "SUCROSE"),
					".\n\nThe thick, goopy jam retains the shape of the jar when poured out, but the sweet taste can't be matched."
				});

				// Token: 0x0400DFA0 RID: 57248
				public static LocString RECIPEDESC = string.Concat(new string[]
				{
					"A long lasting ",
					UI.FormatAsLink("Grubfruit", "WORMSUPERFRUIT"),
					" jam preserved in ",
					UI.FormatAsLink("Sucrose", "SUCROSE"),
					"."
				});
			}

			// Token: 0x020039A6 RID: 14758
			public class BERRYPIE
			{
				// Token: 0x0400DFA1 RID: 57249
				public static LocString NAME = UI.FormatAsLink("Mixed Berry Pie", "BERRYPIE");

				// Token: 0x0400DFA2 RID: 57250
				public static LocString DESC = string.Concat(new string[]
				{
					"A pie made primarily of ",
					UI.FormatAsLink("Grubfruit", "WORMSUPERFRUIT"),
					" and ",
					UI.FormatAsLink("Gristle Berries", "PRICKLEFRUIT"),
					".\n\nThe mixture of berries creates a fragrant, colorful filling that packs a sweet punch."
				});

				// Token: 0x0400DFA3 RID: 57251
				public static LocString RECIPEDESC = string.Concat(new string[]
				{
					"A pie made primarily of ",
					UI.FormatAsLink("Grubfruit", "WORMSUPERFRUIT"),
					" and ",
					UI.FormatAsLink("Gristle Berries", "PRICKLEFRUIT"),
					"."
				});

				// Token: 0x020039A7 RID: 14759
				public class DEHYDRATED
				{
					// Token: 0x0400DFA4 RID: 57252
					public static LocString NAME = "Dried Berry Pie";

					// Token: 0x0400DFA5 RID: 57253
					public static LocString DESC = string.Concat(new string[]
					{
						"A dehydrated ",
						UI.FormatAsLink("Mixed Berry Pie", "BERRYPIE"),
						" ration. It must be rehydrated in order to be considered ",
						UI.FormatAsLink("Food", "FOOD"),
						".\n\nDry rations have no expiry date."
					});
				}
			}

			// Token: 0x020039A8 RID: 14760
			public class COLDWHEATBREAD
			{
				// Token: 0x0400DFA6 RID: 57254
				public static LocString NAME = UI.FormatAsLink("Frost Bun", "COLDWHEATBREAD");

				// Token: 0x0400DFA7 RID: 57255
				public static LocString DESC = "A simple bun baked from " + UI.FormatAsLink("Sleet Wheat Grain", "COLDWHEATSEED") + " grain.\n\nEach bite leaves a mild cooling sensation in one's mouth, even when the bun itself is warm.";

				// Token: 0x0400DFA8 RID: 57256
				public static LocString RECIPEDESC = "A simple bun baked from " + UI.FormatAsLink("Sleet Wheat Grain", "COLDWHEATSEED") + " grain.";
			}

			// Token: 0x020039A9 RID: 14761
			public class BEAN
			{
				// Token: 0x0400DFA9 RID: 57257
				public static LocString NAME = UI.FormatAsLink("Nosh Bean", "BEAN");

				// Token: 0x0400DFAA RID: 57258
				public static LocString DESC = "The crisp bean of a " + UI.FormatAsLink("Nosh Sprout", "BEAN_PLANT") + ".\n\nEach bite tastes refreshingly natural and wholesome.";
			}

			// Token: 0x020039AA RID: 14762
			public class SPICENUT
			{
				// Token: 0x0400DFAB RID: 57259
				public static LocString NAME = UI.FormatAsLink("Pincha Peppernut", "SPICENUT");

				// Token: 0x0400DFAC RID: 57260
				public static LocString DESC = "The flavorful nut of a " + UI.FormatAsLink("Pincha Pepperplant", "SPICE_VINE") + ".\n\nThe bitter outer rind hides a rich, peppery core that is useful in cooking.";
			}

			// Token: 0x020039AB RID: 14763
			public class SPICEBREAD
			{
				// Token: 0x0400DFAD RID: 57261
				public static LocString NAME = UI.FormatAsLink("Pepper Bread", "SPICEBREAD");

				// Token: 0x0400DFAE RID: 57262
				public static LocString DESC = "A loaf of bread, lightly spiced with " + UI.FormatAsLink("Pincha Peppernut", "SPICENUT") + " for a mild bite.\n\nThere's a simple joy to be had in pulling it apart in one's fingers.";

				// Token: 0x0400DFAF RID: 57263
				public static LocString RECIPEDESC = "A loaf of bread, lightly spiced with " + UI.FormatAsLink("Pincha Peppernut", "SPICENUT") + " for a mild bite.";

				// Token: 0x020039AC RID: 14764
				public class DEHYDRATED
				{
					// Token: 0x0400DFB0 RID: 57264
					public static LocString NAME = "Dried Pepper Bread";

					// Token: 0x0400DFB1 RID: 57265
					public static LocString DESC = string.Concat(new string[]
					{
						"A dehydrated ",
						UI.FormatAsLink("Pepper Bread", "SPICEBREAD"),
						" ration. It must be rehydrated in order to be considered ",
						UI.FormatAsLink("Food", "FOOD"),
						".\n\nDry rations have no expiry date."
					});
				}
			}

			// Token: 0x020039AD RID: 14765
			public class SURFANDTURF
			{
				// Token: 0x0400DFB2 RID: 57266
				public static LocString NAME = UI.FormatAsLink("Surf'n'Turf", "SURFANDTURF");

				// Token: 0x0400DFB3 RID: 57267
				public static LocString DESC = string.Concat(new string[]
				{
					"A bit of ",
					UI.FormatAsLink("Meat", "MEAT"),
					" from the land and ",
					UI.FormatAsLink("Cooked Seafood", "COOKEDFISH"),
					" from the sea.\n\nIt's hearty and satisfying."
				});

				// Token: 0x0400DFB4 RID: 57268
				public static LocString RECIPEDESC = string.Concat(new string[]
				{
					"A bit of ",
					UI.FormatAsLink("Meat", "MEAT"),
					" from the land and ",
					UI.FormatAsLink("Cooked Seafood", "COOKEDFISH"),
					" from the sea."
				});

				// Token: 0x020039AE RID: 14766
				public class DEHYDRATED
				{
					// Token: 0x0400DFB5 RID: 57269
					public static LocString NAME = "Dried Surf'n'Turf";

					// Token: 0x0400DFB6 RID: 57270
					public static LocString DESC = string.Concat(new string[]
					{
						"A dehydrated ",
						UI.FormatAsLink("Surf'n'Turf", "SURFANDTURF"),
						" ration. It must be rehydrated in order to be considered ",
						UI.FormatAsLink("Food", "FOOD"),
						".\n\nDry rations have no expiry date."
					});
				}
			}

			// Token: 0x020039AF RID: 14767
			public class TOFU
			{
				// Token: 0x0400DFB7 RID: 57271
				public static LocString NAME = UI.FormatAsLink("Tofu", "TOFU");

				// Token: 0x0400DFB8 RID: 57272
				public static LocString DESC = "A bland curd made from " + UI.FormatAsLink("Nosh Beans", "BEANPLANTSEED") + ".\n\nIt has an unusual but pleasant consistency.";

				// Token: 0x0400DFB9 RID: 57273
				public static LocString RECIPEDESC = "A bland curd made from " + UI.FormatAsLink("Nosh Beans", "BEANPLANTSEED") + ".";
			}

			// Token: 0x020039B0 RID: 14768
			public class SPICYTOFU
			{
				// Token: 0x0400DFBA RID: 57274
				public static LocString NAME = UI.FormatAsLink("Spicy Tofu", "SPICYTOFU");

				// Token: 0x0400DFBB RID: 57275
				public static LocString DESC = ITEMS.FOOD.TOFU.NAME + " marinated in a flavorful " + UI.FormatAsLink("Pincha Peppernut", "SPICENUT") + " sauce.\n\nIt packs a delightful punch.";

				// Token: 0x0400DFBC RID: 57276
				public static LocString RECIPEDESC = ITEMS.FOOD.TOFU.NAME + " marinated in a flavorful " + UI.FormatAsLink("Pincha Peppernut", "SPICENUT") + " sauce.";

				// Token: 0x020039B1 RID: 14769
				public class DEHYDRATED
				{
					// Token: 0x0400DFBD RID: 57277
					public static LocString NAME = "Dried Spicy Tofu";

					// Token: 0x0400DFBE RID: 57278
					public static LocString DESC = string.Concat(new string[]
					{
						"A dehydrated ",
						UI.FormatAsLink("Spicy Tofu", "SPICYTOFU"),
						" ration. It must be rehydrated in order to be considered ",
						UI.FormatAsLink("Food", "FOOD"),
						".\n\nDry rations have no expiry date."
					});
				}
			}

			// Token: 0x020039B2 RID: 14770
			public class CURRY
			{
				// Token: 0x0400DFBF RID: 57279
				public static LocString NAME = UI.FormatAsLink("Curried Beans", "CURRY");

				// Token: 0x0400DFC0 RID: 57280
				public static LocString DESC = string.Concat(new string[]
				{
					"Chewy ",
					UI.FormatAsLink("Nosh Beans", "BEANPLANTSEED"),
					" simmered with chunks of ",
					ITEMS.INGREDIENTS.GINGER.NAME,
					".\n\nIt's so spicy!"
				});

				// Token: 0x0400DFC1 RID: 57281
				public static LocString RECIPEDESC = string.Concat(new string[]
				{
					"Chewy ",
					UI.FormatAsLink("Nosh Beans", "BEANPLANTSEED"),
					" simmered with chunks of ",
					ITEMS.INGREDIENTS.GINGER.NAME,
					"."
				});

				// Token: 0x020039B3 RID: 14771
				public class DEHYDRATED
				{
					// Token: 0x0400DFC2 RID: 57282
					public static LocString NAME = "Dried Curried Beans";

					// Token: 0x0400DFC3 RID: 57283
					public static LocString DESC = string.Concat(new string[]
					{
						"A dehydrated ",
						UI.FormatAsLink("Curried Beans", "CURRY"),
						" ration. It must be rehydrated in order to be considered ",
						UI.FormatAsLink("Food", "FOOD"),
						".\n\nDry rations have no expiry date."
					});
				}
			}

			// Token: 0x020039B4 RID: 14772
			public class SALSA
			{
				// Token: 0x0400DFC4 RID: 57284
				public static LocString NAME = UI.FormatAsLink("Stuffed Berry", "SALSA");

				// Token: 0x0400DFC5 RID: 57285
				public static LocString DESC = "A baked " + UI.FormatAsLink("Bristle Berry", "PRICKLEFRUIT") + " stuffed with delectable spices and vibrantly flavored.";

				// Token: 0x0400DFC6 RID: 57286
				public static LocString RECIPEDESC = "A baked " + UI.FormatAsLink("Bristle Berry", "PRICKLEFRUIT") + " stuffed with delectable spices and vibrantly flavored.";

				// Token: 0x020039B5 RID: 14773
				public class DEHYDRATED
				{
					// Token: 0x0400DFC7 RID: 57287
					public static LocString NAME = "Dried Stuffed Berry";

					// Token: 0x0400DFC8 RID: 57288
					public static LocString DESC = string.Concat(new string[]
					{
						"A dehydrated ",
						UI.FormatAsLink("Stuffed Berry", "SALSA"),
						" ration. It must be rehydrated in order to be considered ",
						UI.FormatAsLink("Food", "FOOD"),
						".\n\nDry rations have no expiry date."
					});
				}
			}

			// Token: 0x020039B6 RID: 14774
			public class HARDSKINBERRY
			{
				// Token: 0x0400DFC9 RID: 57289
				public static LocString NAME = UI.FormatAsLink("Pikeapple", "HARDSKINBERRY");

				// Token: 0x0400DFCA RID: 57290
				public static LocString DESC = "An edible fruit encased in a thorny husk.";
			}

			// Token: 0x020039B7 RID: 14775
			public class CARROT
			{
				// Token: 0x0400DFCB RID: 57291
				public static LocString NAME = UI.FormatAsLink("Plume Squash", "CARROT");

				// Token: 0x0400DFCC RID: 57292
				public static LocString DESC = "An edible tuber with an earthy, elegant flavor.";
			}

			// Token: 0x020039B8 RID: 14776
			public class PEMMICAN
			{
				// Token: 0x0400DFCD RID: 57293
				public static LocString NAME = UI.FormatAsLink("Pemmican", "PEMMICAN");

				// Token: 0x0400DFCE RID: 57294
				public static LocString DESC = UI.FormatAsLink("Meat", "MEAT") + " and " + UI.FormatAsLink("Tallow", "TALLOW") + " pounded into a calorie-dense brick with an exceptionally long shelf life.\n\nSurvival never tasted so good.";

				// Token: 0x0400DFCF RID: 57295
				public static LocString RECIPEDESC = UI.FormatAsLink("Meat", "MEAT") + " and " + UI.FormatAsLink("Tallow", "TALLOW") + " pounded into a nutrient-dense brick with an exceptionally long shelf life.";
			}

			// Token: 0x020039B9 RID: 14777
			public class BASICPLANTFOOD
			{
				// Token: 0x0400DFD0 RID: 57296
				public static LocString NAME = UI.FormatAsLink("Meal Lice", "BASICPLANTFOOD");

				// Token: 0x0400DFD1 RID: 57297
				public static LocString DESC = "A flavorless grain that almost never wiggles on its own.";
			}

			// Token: 0x020039BA RID: 14778
			public class BASICPLANTBAR
			{
				// Token: 0x0400DFD2 RID: 57298
				public static LocString NAME = UI.FormatAsLink("Liceloaf", "BASICPLANTBAR");

				// Token: 0x0400DFD3 RID: 57299
				public static LocString DESC = UI.FormatAsLink("Meal Lice", "BASICPLANTFOOD") + " compacted into a dense, immobile loaf.";

				// Token: 0x0400DFD4 RID: 57300
				public static LocString RECIPEDESC = UI.FormatAsLink("Meal Lice", "BASICPLANTFOOD") + " compacted into a dense, immobile loaf.";
			}

			// Token: 0x020039BB RID: 14779
			public class BASICFORAGEPLANT
			{
				// Token: 0x0400DFD5 RID: 57301
				public static LocString NAME = UI.FormatAsLink("Muckroot", "BASICFORAGEPLANT");

				// Token: 0x0400DFD6 RID: 57302
				public static LocString DESC = "A seedless fruit with an upsettingly bland aftertaste.\n\nIt cannot be replanted.\n\nDigging up Buried Objects may uncover a " + ITEMS.FOOD.BASICFORAGEPLANT.NAME + ".";
			}

			// Token: 0x020039BC RID: 14780
			public class FORESTFORAGEPLANT
			{
				// Token: 0x0400DFD7 RID: 57303
				public static LocString NAME = UI.FormatAsLink("Hexalent Fruit", "FORESTFORAGEPLANT");

				// Token: 0x0400DFD8 RID: 57304
				public static LocString DESC = "A seedless fruit with an unusual rubbery texture.\n\nIt cannot be replanted.\n\nHexalent fruit is much more calorie dense than Muckroot fruit.";
			}

			// Token: 0x020039BD RID: 14781
			public class SWAMPFORAGEPLANT
			{
				// Token: 0x0400DFD9 RID: 57305
				public static LocString NAME = UI.FormatAsLink("Swamp Chard Heart", "SWAMPFORAGEPLANT");

				// Token: 0x0400DFDA RID: 57306
				public static LocString DESC = "A seedless plant with a squishy, juicy center and an awful smell.\n\nIt cannot be replanted.";
			}

			// Token: 0x020039BE RID: 14782
			public class ICECAVESFORAGEPLANT
			{
				// Token: 0x0400DFDB RID: 57307
				public static LocString NAME = UI.FormatAsLink("Sherberry", "ICECAVESFORAGEPLANT");

				// Token: 0x0400DFDC RID: 57308
				public static LocString DESC = "A cold seedless fruit that triggers mild brain freeze.\n\nIt cannot be replanted.";
			}

			// Token: 0x020039BF RID: 14783
			public class ROTPILE
			{
				// Token: 0x0400DFDD RID: 57309
				public static LocString NAME = UI.FormatAsLink("Rot Pile", "COMPOST");

				// Token: 0x0400DFDE RID: 57310
				public static LocString DESC = string.Concat(new string[]
				{
					"An inedible glop of former foodstuff.\n\n",
					ITEMS.FOOD.ROTPILE.NAME,
					"s break down into ",
					UI.FormatAsLink("Polluted Dirt", "TOXICSAND"),
					" over time."
				});
			}

			// Token: 0x020039C0 RID: 14784
			public class COLDWHEATSEED
			{
				// Token: 0x0400DFDF RID: 57311
				public static LocString NAME = UI.FormatAsLink("Sleet Wheat Grain", "COLDWHEATSEED");

				// Token: 0x0400DFE0 RID: 57312
				public static LocString DESC = "An edible grain that leaves a cool taste on the tongue.";
			}

			// Token: 0x020039C1 RID: 14785
			public class BEANPLANTSEED
			{
				// Token: 0x0400DFE1 RID: 57313
				public static LocString NAME = UI.FormatAsLink("Nosh Bean", "BEANPLANTSEED");

				// Token: 0x0400DFE2 RID: 57314
				public static LocString DESC = "An inedible bean that can be processed into delicious foods.";
			}

			// Token: 0x020039C2 RID: 14786
			public class QUICHE
			{
				// Token: 0x0400DFE3 RID: 57315
				public static LocString NAME = UI.FormatAsLink("Mushroom Quiche", "QUICHE");

				// Token: 0x0400DFE4 RID: 57316
				public static LocString DESC = string.Concat(new string[]
				{
					UI.FormatAsLink("Omelette", "COOKEDEGG"),
					", ",
					UI.FormatAsLink("Fried Mushroom", "FRIEDMUSHROOM"),
					" and ",
					UI.FormatAsLink("Lettuce", "LETTUCE"),
					" piled onto a yummy crust.\n\nSomehow, it's both soggy <i>and</i> crispy."
				});

				// Token: 0x0400DFE5 RID: 57317
				public static LocString RECIPEDESC = string.Concat(new string[]
				{
					UI.FormatAsLink("Omelette", "COOKEDEGG"),
					", ",
					UI.FormatAsLink("Fried Mushroom", "FRIEDMUSHROOM"),
					" and ",
					UI.FormatAsLink("Lettuce", "LETTUCE"),
					" piled onto a yummy crust."
				});

				// Token: 0x020039C3 RID: 14787
				public class DEHYDRATED
				{
					// Token: 0x0400DFE6 RID: 57318
					public static LocString NAME = "Dried Mushroom Quiche";

					// Token: 0x0400DFE7 RID: 57319
					public static LocString DESC = string.Concat(new string[]
					{
						"A dehydrated ",
						UI.FormatAsLink("Mushroom Quiche", "QUICHE"),
						" ration. It must be rehydrated in order to be considered ",
						UI.FormatAsLink("Food", "FOOD"),
						".\n\nDry rations have no expiry date."
					});
				}
			}
		}

		// Token: 0x020039C4 RID: 14788
		public class INGREDIENTS
		{
			// Token: 0x020039C5 RID: 14789
			public class SWAMPLILYFLOWER
			{
				// Token: 0x0400DFE8 RID: 57320
				public static LocString NAME = UI.FormatAsLink("Balm Lily Flower", "SWAMPLILYFLOWER");

				// Token: 0x0400DFE9 RID: 57321
				public static LocString DESC = "A medicinal flower that soothes most minor maladies.\n\nIt is exceptionally fragrant.";
			}

			// Token: 0x020039C6 RID: 14790
			public class GINGER
			{
				// Token: 0x0400DFEA RID: 57322
				public static LocString NAME = UI.FormatAsLink("Tonic Root", "GINGERCONFIG");

				// Token: 0x0400DFEB RID: 57323
				public static LocString DESC = "A chewy, fibrous rhizome with a fiery aftertaste.";
			}
		}

		// Token: 0x020039C7 RID: 14791
		public class INDUSTRIAL_PRODUCTS
		{
			// Token: 0x020039C8 RID: 14792
			public class ELECTROBANK_URANIUM_ORE
			{
				// Token: 0x0400DFEC RID: 57324
				public static LocString NAME = UI.FormatAsLink("Uranium Ore Power Bank", "ELECTROBANK_URANIUM_ORE");

				// Token: 0x0400DFED RID: 57325
				public static LocString DESC = string.Concat(new string[]
				{
					"A disposable ",
					UI.FormatAsLink("Power Bank", "ELECTROBANK"),
					" made with ",
					UI.FormatAsLink("Uranium Ore", "URANIUMORE"),
					".\n\nIt can power buildings via ",
					UI.FormatAsLink("Large Dischargers", "LARGEELECTROBANKDISCHARGER"),
					" or ",
					UI.FormatAsLink("Compact Dischargers", "SMALLELECTROBANKDISCHARGER"),
					".\n\nDuplicants can produce new ",
					UI.FormatAsLink("Uranium Ore Power Banks", "ELECTROBANK"),
					" at the ",
					UI.FormatAsLink("Crafting Station", "CRAFTINGTABLE"),
					".\n\nMust be kept dry."
				});
			}

			// Token: 0x020039C9 RID: 14793
			public class ELECTROBANK_METAL_ORE
			{
				// Token: 0x0400DFEE RID: 57326
				public static LocString NAME = UI.FormatAsLink("Metal Power Bank", "ELECTROBANK_METAL_ORE");

				// Token: 0x0400DFEF RID: 57327
				public static LocString DESC = string.Concat(new string[]
				{
					"A disposable ",
					UI.FormatAsLink("Power Bank", "ELECTROBANK"),
					" made with ",
					UI.FormatAsLink("Metal Ore", "METAL"),
					".\n\nIt can power buildings via ",
					UI.FormatAsLink("Large Dischargers", "LARGEELECTROBANKDISCHARGER"),
					" or ",
					UI.FormatAsLink("Compact Dischargers", "SMALLELECTROBANKDISCHARGER"),
					".\n\nDuplicants can produce new ",
					UI.FormatAsLink("Metal Power Banks", "ELECTROBANK"),
					" at the ",
					UI.FormatAsLink("Crafting Station", "CRAFTINGTABLE"),
					".\n\nMust be kept dry."
				});
			}

			// Token: 0x020039CA RID: 14794
			public class ELECTROBANK_SELFCHARGING
			{
				// Token: 0x0400DFF0 RID: 57328
				public static LocString NAME = UI.FormatAsLink("Atomic Power Bank", "ELECTROBANK_SELFCHARGING");

				// Token: 0x0400DFF1 RID: 57329
				public static LocString DESC = string.Concat(new string[]
				{
					"A self-charging ",
					UI.FormatAsLink("Power Bank", "ELECTROBANK"),
					" made with ",
					ELEMENTS.ENRICHEDURANIUM.NAME,
					".\n\nIt can power buildings via ",
					UI.FormatAsLink("Large Dischargers", "LARGEELECTROBANKDISCHARGER"),
					" or ",
					UI.FormatAsLink("Compact Dischargers", "SMALLELECTROBANKDISCHARGER"),
					".\n\nIts low ",
					UI.FormatAsLink("wattage", "POWER"),
					" and high ",
					UI.FormatAsLink("Radioactivity", "RADIATION"),
					" make it unsuitable for Bionic Duplicant use."
				});
			}

			// Token: 0x020039CB RID: 14795
			public class ELECTROBANK
			{
				// Token: 0x0400DFF2 RID: 57330
				public static LocString NAME = UI.FormatAsLink("Eco Power Bank", "ELECTROBANK");

				// Token: 0x0400DFF3 RID: 57331
				public static LocString DESC = string.Concat(new string[]
				{
					"A rechargeable ",
					UI.FormatAsLink("Power Bank", "ELECTROBANK"),
					".\n\nIt can power buildings via ",
					UI.FormatAsLink("Large Dischargers", "LARGEELECTROBANKDISCHARGER"),
					" or ",
					UI.FormatAsLink("Compact Dischargers", "SMALLELECTROBANKDISCHARGER"),
					".\n\nDuplicants can produce new ",
					UI.FormatAsLink("Eco Power Banks", "ELECTROBANK"),
					" at the ",
					UI.FormatAsLink("Soldering Station", "ADVANCEDCRAFTINGTABLE"),
					".\n\nMust be kept dry."
				});
			}

			// Token: 0x020039CC RID: 14796
			public class ELECTROBANK_EMPTY
			{
				// Token: 0x0400DFF4 RID: 57332
				public static LocString NAME = UI.FormatAsLink("Empty Eco Power Bank", "ELECTROBANK");

				// Token: 0x0400DFF5 RID: 57333
				public static LocString DESC = string.Concat(new string[]
				{
					"A depleted ",
					UI.FormatAsLink("Power Bank", "ELECTROBANK"),
					".\n\nIt must be recharged at a ",
					UI.FormatAsLink("Power Bank Charger", "ELECTROBANKCHARGER"),
					" before it can be reused."
				});
			}

			// Token: 0x020039CD RID: 14797
			public class ELECTROBANK_GARBAGE
			{
				// Token: 0x0400DFF6 RID: 57334
				public static LocString NAME = UI.FormatAsLink("Power Bank Scrap", "ELECTROBANK");

				// Token: 0x0400DFF7 RID: 57335
				public static LocString DESC = string.Concat(new string[]
				{
					"A ",
					UI.FormatAsLink("Power Bank", "ELECTROBANK"),
					" that has reached the end of its lifetime.\n\nIt can be salvaged for ",
					UI.FormatAsLink("Abyssalite", "KATAIRITE"),
					" at the ",
					UI.FormatAsLink("Rock Crusher", "ROCKCRUSHER"),
					"."
				});
			}

			// Token: 0x020039CE RID: 14798
			public class FUEL_BRICK
			{
				// Token: 0x0400DFF8 RID: 57336
				public static LocString NAME = "Fuel Brick";

				// Token: 0x0400DFF9 RID: 57337
				public static LocString DESC = "A densely compressed brick of combustible material.\n\nIt can be burned to produce a one-time burst of " + UI.FormatAsLink("Power", "POWER") + ".";
			}

			// Token: 0x020039CF RID: 14799
			public class BASIC_FABRIC
			{
				// Token: 0x0400DFFA RID: 57338
				public static LocString NAME = UI.FormatAsLink("Reed Fiber", "BASIC_FABRIC");

				// Token: 0x0400DFFB RID: 57339
				public static LocString DESC = "A ball of raw cellulose used in the production of " + UI.FormatAsLink("Clothing", "EQUIPMENT") + " and textiles.";
			}

			// Token: 0x020039D0 RID: 14800
			public class TRAP_PARTS
			{
				// Token: 0x0400DFFC RID: 57340
				public static LocString NAME = "Trap Components";

				// Token: 0x0400DFFD RID: 57341
				public static LocString DESC = string.Concat(new string[]
				{
					"These components can be assembled into a ",
					BUILDINGS.PREFABS.CREATURETRAP.NAME,
					" and used to catch ",
					UI.FormatAsLink("Critters", "CREATURES"),
					"."
				});
			}

			// Token: 0x020039D1 RID: 14801
			public class POWER_STATION_TOOLS
			{
				// Token: 0x0400DFFE RID: 57342
				public static LocString NAME = UI.FormatAsLink("Microchip", "POWER_STATION_TOOLS");

				// Token: 0x0400DFFF RID: 57343
				public static LocString DESC = string.Concat(new string[]
				{
					"A specialized ",
					ITEMS.INDUSTRIAL_PRODUCTS.POWER_STATION_TOOLS.NAME,
					" created by a professional engineer.\n\nTunes up ",
					UI.FormatAsLink("Generators", "REQUIREMENTCLASSGENERATORTYPE"),
					" to increase their ",
					UI.FormatAsLink("Power", "POWER"),
					" output.\n\nAlso used in the production of ",
					UI.FormatAsLink("Boosters", "BOOSTER"),
					" for Bionic Duplicants."
				});

				// Token: 0x0400E000 RID: 57344
				public static LocString TINKER_REQUIREMENT_NAME = "Skill: " + DUPLICANTS.ROLES.POWER_TECHNICIAN.NAME;

				// Token: 0x0400E001 RID: 57345
				public static LocString TINKER_REQUIREMENT_TOOLTIP = string.Concat(new string[]
				{
					"Can only be used by a Duplicant with ",
					DUPLICANTS.ROLES.POWER_TECHNICIAN.NAME,
					" to apply a ",
					UI.PRE_KEYWORD,
					"Tune Up",
					UI.PST_KEYWORD,
					"."
				});

				// Token: 0x0400E002 RID: 57346
				public static LocString TINKER_EFFECT_NAME = "Engie's Tune-Up: {0} {1}";

				// Token: 0x0400E003 RID: 57347
				public static LocString TINKER_EFFECT_TOOLTIP = string.Concat(new string[]
				{
					"Can be used to ",
					UI.PRE_KEYWORD,
					"Tune Up",
					UI.PST_KEYWORD,
					" a generator, increasing its {0} by <b>{1}</b>."
				});

				// Token: 0x0400E004 RID: 57348
				public static LocString RECIPE_DESCRIPTION = "Make " + ITEMS.INDUSTRIAL_PRODUCTS.POWER_STATION_TOOLS.NAME + " from {0}";
			}

			// Token: 0x020039D2 RID: 14802
			public class FARM_STATION_TOOLS
			{
				// Token: 0x0400E005 RID: 57349
				public static LocString NAME = "Micronutrient Fertilizer";

				// Token: 0x0400E006 RID: 57350
				public static LocString DESC = string.Concat(new string[]
				{
					"Specialized ",
					UI.FormatAsLink("Fertilizer", "FERTILIZER"),
					" mixed by a Duplicant with the ",
					DUPLICANTS.ROLES.FARMER.NAME,
					" Skill.\n\nIncreases the ",
					UI.PRE_KEYWORD,
					"Growth Rate",
					UI.PST_KEYWORD,
					" of one ",
					UI.FormatAsLink("Plant", "PLANTS"),
					"."
				});
			}

			// Token: 0x020039D3 RID: 14803
			public class MACHINE_PARTS
			{
				// Token: 0x0400E007 RID: 57351
				public static LocString NAME = "Custom Parts";

				// Token: 0x0400E008 RID: 57352
				public static LocString DESC = string.Concat(new string[]
				{
					"Specialized Parts crafted by a professional engineer.\n\n",
					UI.PRE_KEYWORD,
					"Jerry Rig",
					UI.PST_KEYWORD,
					" machine buildings to increase their efficiency."
				});

				// Token: 0x0400E009 RID: 57353
				public static LocString TINKER_REQUIREMENT_NAME = "Job: " + DUPLICANTS.ROLES.MECHATRONIC_ENGINEER.NAME;

				// Token: 0x0400E00A RID: 57354
				public static LocString TINKER_REQUIREMENT_TOOLTIP = string.Concat(new string[]
				{
					"Can only be used by a Duplicant with ",
					DUPLICANTS.ROLES.MECHATRONIC_ENGINEER.NAME,
					" to apply a ",
					UI.PRE_KEYWORD,
					"Jerry Rig",
					UI.PST_KEYWORD,
					"."
				});

				// Token: 0x0400E00B RID: 57355
				public static LocString TINKER_EFFECT_NAME = "Engineer's Jerry Rig: {0} {1}";

				// Token: 0x0400E00C RID: 57356
				public static LocString TINKER_EFFECT_TOOLTIP = string.Concat(new string[]
				{
					"Can be used to ",
					UI.PRE_KEYWORD,
					"Jerry Rig",
					UI.PST_KEYWORD,
					" upgrades to a machine building, increasing its {0} by <b>{1}</b>."
				});
			}

			// Token: 0x020039D4 RID: 14804
			public class RESEARCH_DATABANK
			{
				// Token: 0x0400E00D RID: 57357
				public static LocString NAME = UI.FormatAsLink("Data Bank", "DATABANK");

				// Token: 0x0400E00E RID: 57358
				public static LocString NAME_PLURAL = UI.FormatAsLink("Data Banks", "DATABANK");

				// Token: 0x0400E00F RID: 57359
				public static LocString DESC = "Raw data that can be processed into " + UI.FormatAsLink("Interstellar Research", "RESEARCH") + " points.";
			}

			// Token: 0x020039D5 RID: 14805
			public class ORBITAL_RESEARCH_DATABANK
			{
				// Token: 0x0400E010 RID: 57360
				public static LocString NAME = UI.FormatAsLink("Data Bank", "DATABANK");

				// Token: 0x0400E011 RID: 57361
				public static LocString NAME_PLURAL = UI.FormatAsLink("Data Banks", "DATABANK");

				// Token: 0x0400E012 RID: 57362
				public static LocString DESC = "Raw Data that can be processed into " + UI.FormatAsLink("Data Analysis Research", "RESEARCHDLC1") + " points.";

				// Token: 0x0400E013 RID: 57363
				public static LocString RECIPE_DESC = string.Concat(new string[]
				{
					"Data Banks of raw data generated from exploring, either by exploring new areas with Duplicants, or by using an ",
					UI.FormatAsLink("Orbital Data Collection Lab", "ORBITALRESEARCHCENTER"),
					".\n\nUsed by the ",
					UI.FormatAsLink("Virtual Planetarium", "DLC1COSMICRESEARCHCENTER"),
					" to conduct research."
				});
			}

			// Token: 0x020039D6 RID: 14806
			public class EGG_SHELL
			{
				// Token: 0x0400E014 RID: 57364
				public static LocString NAME = UI.FormatAsLink("Egg Shell", "EGG_SHELL");

				// Token: 0x0400E015 RID: 57365
				public static LocString DESC = "Can be crushed to produce " + UI.FormatAsLink("Lime", "LIME") + ".";
			}

			// Token: 0x020039D7 RID: 14807
			public class GOLD_BELLY_CROWN
			{
				// Token: 0x0400E016 RID: 57366
				public static LocString NAME = UI.FormatAsLink("Regal Bammoth Crest", "GOLD_BELLY_CROWN");

				// Token: 0x0400E017 RID: 57367
				public static LocString DESC = "Can be crushed to produce " + ELEMENTS.GOLDAMALGAM.NAME + ".";
			}

			// Token: 0x020039D8 RID: 14808
			public class CRAB_SHELL
			{
				// Token: 0x0400E018 RID: 57368
				public static LocString NAME = UI.FormatAsLink("Pokeshell Molt", "CRAB_SHELL");

				// Token: 0x0400E019 RID: 57369
				public static LocString DESC = "Can be crushed to produce " + UI.FormatAsLink("Lime", "LIME") + ".";

				// Token: 0x020039D9 RID: 14809
				public class VARIANT_WOOD
				{
					// Token: 0x0400E01A RID: 57370
					public static LocString NAME = UI.FormatAsLink("Oakshell Molt", "VARIANT_WOOD_SHELL");

					// Token: 0x0400E01B RID: 57371
					public static LocString DESC = "Can be crushed to produce " + UI.FormatAsLink("Wood", "WOOD") + ".";
				}
			}

			// Token: 0x020039DA RID: 14810
			public class BABY_CRAB_SHELL
			{
				// Token: 0x0400E01C RID: 57372
				public static LocString NAME = UI.FormatAsLink("Small Pokeshell Molt", "CRAB_SHELL");

				// Token: 0x0400E01D RID: 57373
				public static LocString DESC = "Can be crushed to produce " + UI.FormatAsLink("Lime", "LIME") + ".";

				// Token: 0x020039DB RID: 14811
				public class VARIANT_WOOD
				{
					// Token: 0x0400E01E RID: 57374
					public static LocString NAME = UI.FormatAsLink("Small Oakshell Molt", "VARIANT_WOOD_SHELL");

					// Token: 0x0400E01F RID: 57375
					public static LocString DESC = "Can be crushed to produce " + UI.FormatAsLink("Wood", "WOOD") + ".";
				}
			}

			// Token: 0x020039DC RID: 14812
			public class WOOD
			{
				// Token: 0x0400E020 RID: 57376
				public static LocString NAME = UI.FormatAsLink("Wood", "WOOD");

				// Token: 0x0400E021 RID: 57377
				public static LocString DESC = string.Concat(new string[]
				{
					"Natural resource harvested from certain ",
					UI.FormatAsLink("Critters", "CREATURES"),
					" and ",
					UI.FormatAsLink("Plants", "PLANTS"),
					".\n\nUsed in construction or ",
					UI.FormatAsLink("Heat", "HEAT"),
					" production."
				});
			}

			// Token: 0x020039DD RID: 14813
			public class GENE_SHUFFLER_RECHARGE
			{
				// Token: 0x0400E022 RID: 57378
				public static LocString NAME = "Vacillator Recharge";

				// Token: 0x0400E023 RID: 57379
				public static LocString DESC = "Replenishes one charge to a depleted " + BUILDINGS.PREFABS.GENESHUFFLER.NAME + ".";
			}

			// Token: 0x020039DE RID: 14814
			public class TABLE_SALT
			{
				// Token: 0x0400E024 RID: 57380
				public static LocString NAME = "Table Salt";

				// Token: 0x0400E025 RID: 57381
				public static LocString DESC = string.Concat(new string[]
				{
					"A seasoning that Duplicants can add to their ",
					UI.FormatAsLink("Food", "FOOD"),
					" to boost ",
					UI.FormatAsLink("Morale", "MORALE"),
					".\n\nDuplicants will automatically use Table Salt while sitting at a ",
					BUILDINGS.PREFABS.DININGTABLE.NAME,
					" during mealtime.\n\n<i>Only the finest grains are chosen.</i>"
				});
			}

			// Token: 0x020039DF RID: 14815
			public class REFINED_SUGAR
			{
				// Token: 0x0400E026 RID: 57382
				public static LocString NAME = "Refined Sugar";

				// Token: 0x0400E027 RID: 57383
				public static LocString DESC = string.Concat(new string[]
				{
					"A seasoning that Duplicants can add to their ",
					UI.FormatAsLink("Food", "FOOD"),
					" to boost ",
					UI.FormatAsLink("Morale", "MORALE"),
					".\n\nDuplicants will automatically use Refined Sugar while sitting at a ",
					BUILDINGS.PREFABS.DININGTABLE.NAME,
					" during mealtime.\n\n<i>Only the finest grains are chosen.</i>"
				});
			}

			// Token: 0x020039E0 RID: 14816
			public class ICE_BELLY_POOP
			{
				// Token: 0x0400E028 RID: 57384
				public static LocString NAME = UI.FormatAsLink("Bammoth Patty", "ICE_BELLY_POOP");

				// Token: 0x0400E029 RID: 57385
				public static LocString DESC = string.Concat(new string[]
				{
					"A little treat left behind by a very large critter.\n\nIt can be crushed to extract ",
					UI.FormatAsLink("Phosphorite", "PHOSPHORITE"),
					" and ",
					UI.FormatAsLink("Clay", "CLAY"),
					"."
				});
			}
		}

		// Token: 0x020039E1 RID: 14817
		public class CARGO_CAPSULE
		{
			// Token: 0x0400E02A RID: 57386
			public static LocString NAME = "Care Package";

			// Token: 0x0400E02B RID: 57387
			public static LocString DESC = "A delivery system for recently printed resources.\n\nIt will dematerialize shortly.";
		}

		// Token: 0x020039E2 RID: 14818
		public class RAILGUNPAYLOAD
		{
			// Token: 0x0400E02C RID: 57388
			public static LocString NAME = UI.FormatAsLink("Interplanetary Payload", "RAILGUNPAYLOAD");

			// Token: 0x0400E02D RID: 57389
			public static LocString DESC = string.Concat(new string[]
			{
				"Contains resources packed for interstellar shipping.\n\nCan be launched by a ",
				BUILDINGS.PREFABS.RAILGUN.NAME,
				" or unpacked with a ",
				BUILDINGS.PREFABS.RAILGUNPAYLOADOPENER.NAME,
				"."
			});
		}

		// Token: 0x020039E3 RID: 14819
		public class MISSILE_BASIC
		{
			// Token: 0x0400E02E RID: 57390
			public static LocString NAME = UI.FormatAsLink("Blastshot", "MISSILELAUNCHER");

			// Token: 0x0400E02F RID: 57391
			public static LocString DESC = "An explosive projectile designed to defend against meteor showers.\n\nMust be launched by a " + UI.FormatAsLink("Meteor Blaster", "MISSILELAUNCHER") + ".";
		}

		// Token: 0x020039E4 RID: 14820
		public class DEBRISPAYLOAD
		{
			// Token: 0x0400E030 RID: 57392
			public static LocString NAME = "Rocket Debris";

			// Token: 0x0400E031 RID: 57393
			public static LocString DESC = "Whatever is left over from a Rocket Self-Destruct can be recovered once it has crash-landed.";
		}

		// Token: 0x020039E5 RID: 14821
		public class RADIATION
		{
			// Token: 0x020039E6 RID: 14822
			public class HIGHENERGYPARITCLE
			{
				// Token: 0x0400E032 RID: 57394
				public static LocString NAME = "Radbolts";

				// Token: 0x0400E033 RID: 57395
				public static LocString DESC = string.Concat(new string[]
				{
					"A concentrated field of ",
					UI.FormatAsKeyWord("Radbolts"),
					" that can be largely redirected using a ",
					UI.FormatAsLink("Radbolt Reflector", "HIGHENERGYPARTICLEREDIRECTOR"),
					"."
				});
			}
		}

		// Token: 0x020039E7 RID: 14823
		public class DREAMJOURNAL
		{
			// Token: 0x0400E034 RID: 57396
			public static LocString NAME = "Dream Journal";

			// Token: 0x0400E035 RID: 57397
			public static LocString DESC = string.Concat(new string[]
			{
				"A hand-scrawled account of ",
				UI.FormatAsLink("Pajama", "SLEEP_CLINIC_PAJAMAS"),
				"-induced dreams.\n\nCan be analyzed using a ",
				UI.FormatAsLink("Somnium Synthesizer", "MEGABRAINTANK"),
				"."
			});
		}

		// Token: 0x020039E8 RID: 14824
		public class DEHYDRATEDFOODPACKAGE
		{
			// Token: 0x0400E036 RID: 57398
			public static LocString NAME = "Dry Ration";

			// Token: 0x0400E037 RID: 57399
			public static LocString DESC = "A package of non-perishable dehydrated food.\n\nIt requires no refrigeration, but must be rehydrated before consumption.";

			// Token: 0x0400E038 RID: 57400
			public static LocString CONSUMED = "Ate Rehydrated Food";

			// Token: 0x0400E039 RID: 57401
			public static LocString CONTENTS = "Dried {0}";
		}

		// Token: 0x020039E9 RID: 14825
		public class SPICES
		{
			// Token: 0x020039EA RID: 14826
			public class MACHINERY_SPICE
			{
				// Token: 0x0400E03A RID: 57402
				public static LocString NAME = UI.FormatAsLink("Machinist Spice", "MACHINERY_SPICE");

				// Token: 0x0400E03B RID: 57403
				public static LocString DESC = "Improves operating skills when ingested.";
			}

			// Token: 0x020039EB RID: 14827
			public class PILOTING_SPICE
			{
				// Token: 0x0400E03C RID: 57404
				public static LocString NAME = UI.FormatAsLink("Rocketeer Spice", "PILOTING_SPICE");

				// Token: 0x0400E03D RID: 57405
				public static LocString DESC = "Provides a boost to piloting abilities.";
			}

			// Token: 0x020039EC RID: 14828
			public class PRESERVING_SPICE
			{
				// Token: 0x0400E03E RID: 57406
				public static LocString NAME = UI.FormatAsLink("Freshener Spice", "PRESERVING_SPICE");

				// Token: 0x0400E03F RID: 57407
				public static LocString DESC = "Slows the decomposition of perishable foods.";
			}

			// Token: 0x020039ED RID: 14829
			public class STRENGTH_SPICE
			{
				// Token: 0x0400E040 RID: 57408
				public static LocString NAME = UI.FormatAsLink("Brawny Spice", "STRENGTH_SPICE");

				// Token: 0x0400E041 RID: 57409
				public static LocString DESC = "Strengthens even the weakest of muscles.";
			}
		}
	}
}
