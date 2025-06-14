﻿using System;

namespace STRINGS
{

	public class ITEMS
	{

		public class PILLS
		{

			public class PLACEBO
			{

				public static LocString NAME = "Placebo";


				public static LocString DESC = "A general, all-purpose " + UI.FormatAsLink("Medicine", "MEDICINE") + ".\n\nThe less one knows about it, the better it works.";


				public static LocString RECIPEDESC = "All-purpose " + UI.FormatAsLink("Medicine", "MEDICINE") + ".";
			}


			public class BASICBOOSTER
			{

				public static LocString NAME = UI.FormatAsLink("Vitamin Chews", "BASICBOOSTER");


				public static LocString DESC = "Minorly reduces the chance of becoming sick.";


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


			public class INTERMEDIATEBOOSTER
			{

				public static LocString NAME = UI.FormatAsLink("Immuno Booster", "INTERMEDIATEBOOSTER");


				public static LocString DESC = "Significantly reduces the chance of becoming sick.";


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


			public class ANTIHISTAMINE
			{

				public static LocString NAME = UI.FormatAsLink("Allergy Medication", "ANTIHISTAMINE");


				public static LocString DESC = "Suppresses and prevents allergic reactions.";


				public static LocString RECIPEDESC = "A strong antihistamine Duplicants can take to halt an allergic reaction. " + ITEMS.PILLS.ANTIHISTAMINE.NAME + " will also prevent further reactions from occurring for a short time after ingestion.";
			}


			public class BASICCURE
			{

				public static LocString NAME = UI.FormatAsLink("Curative Tablet", "BASICCURE");


				public static LocString DESC = "A simple, easy-to-take remedy for minor germ-based diseases.";


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


			public class INTERMEDIATECURE
			{

				public static LocString NAME = UI.FormatAsLink("Medical Pack", "INTERMEDIATECURE");


				public static LocString DESC = "A doctor-administered cure for moderate ailments.";


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


			public class ADVANCEDCURE
			{

				public static LocString NAME = UI.FormatAsLink("Serum Vial", "ADVANCEDCURE");


				public static LocString DESC = "A doctor-administered cure for severe ailments.";


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


			public class BASICRADPILL
			{

				public static LocString NAME = UI.FormatAsLink("Basic Rad Pill", "BASICRADPILL");


				public static LocString DESC = "Increases a Duplicant's natural radiation absorption rate.";


				public static LocString RECIPEDESC = "A supplement that speeds up the rate at which a Duplicant body absorbs radiation, allowing them to manage increased radiation exposure.\n\nMust be taken daily.";
			}


			public class INTERMEDIATERADPILL
			{

				public static LocString NAME = UI.FormatAsLink("Intermediate Rad Pill", "INTERMEDIATERADPILL");


				public static LocString DESC = "Increases a Duplicant's natural radiation absorption rate.";


				public static LocString RECIPEDESC = "A supplement that speeds up the rate at which a Duplicant body absorbs radiation, allowing them to manage increased radiation exposure.\n\nMust be taken daily.";
			}
		}


		public class LUBRICATIONSTICK
		{

			public static LocString NAME = UI.FormatAsLink("Gear Balm", "LUBRICATIONSTICK");


			public static LocString SUBHEADER = "Mechanical Lubricant";


			public static LocString DESC = string.Concat(new string[]
			{
				"Provides a small amount of lubricating ",
				UI.FormatAsLink("Gear Oil", "LUBRICATINGOIL"),
				".\n\nCan be produced at the ",
				BUILDINGS.PREFABS.APOTHECARY.NAME,
				"."
			});


			public static LocString RECIPEDESC = "A self-administered mechanical lubricant for Duplicants with bionic parts.";
		}


		public class TALLOWLUBRICATIONSTICK
		{

			public static LocString NAME = UI.FormatAsLink("Tallow Gear Balm", "TALLOWLUBRICATIONSTICK");


			public static LocString SUBHEADER = "Mechanical Lubricant";


			public static LocString DESC = string.Concat(new string[]
			{
				"Provides a small amount of extra-silky lubricating ",
				UI.FormatAsLink("Gear Oil", "LUBRICATINGOIL"),
				".\n\nCan be produced at the ",
				BUILDINGS.PREFABS.APOTHECARY.NAME,
				"."
			});


			public static LocString RECIPEDESC = "An advanced self-administered mechanical lubricant for Duplicants with bionic parts.";
		}


		public class BIONIC_BOOSTERS
		{

			public static LocString FABRICATION_SOURCE = "This booster can be manufactured at the {0}.";


			public class BOOSTER_DIG1
			{

				public static LocString NAME = UI.FormatAsLink("Digging Booster", "BOOSTER_DIG1");


				public static LocString DESC = "Grants a Bionic Duplicant the skill required to dig hard things.";
			}


			public class BOOSTER_DIG2
			{

				public static LocString NAME = UI.FormatAsLink("Extreme Digging Booster", "BOOSTER_DIG2");


				public static LocString DESC = "Grants a Bionic Duplicant the digging skill required to get through anything.";
			}


			public class BOOSTER_CONSTRUCT1
			{

				public static LocString NAME = UI.FormatAsLink("Construction Booster", "BOOSTER_CONSTRUCT1");


				public static LocString DESC = "Grants a Bionic Duplicant the ability to build fast, and demolish buildings that others cannot.";
			}


			public class BOOSTER_FARM1
			{

				public static LocString NAME = UI.FormatAsLink("Crop Tending Booster", "BOOSTER_FARM1");


				public static LocString DESC = "Grants a Bionic Duplicant unparalleled farming and botanical analysis skills.";
			}


			public class BOOSTER_RANCH1
			{

				public static LocString NAME = UI.FormatAsLink("Ranching Booster", "BOOSTER_RANCH1");


				public static LocString DESC = "Grants a Bionic Duplicant the skills required to care for " + UI.FormatAsLink("Critters", "CREATURES") + " in every way.";
			}


			public class BOOSTER_COOK1
			{

				public static LocString NAME = UI.FormatAsLink("Grilling Booster", "BOOSTER_COOK1");


				public static LocString DESC = "Grants a Bionic Duplicant deliciously professional culinary skills.";
			}


			public class BOOSTER_ART1
			{

				public static LocString NAME = UI.FormatAsLink("Masterworks Art Booster", "BOOSTER_ART1");


				public static LocString DESC = "Grants a Bionic Duplicant flawless decorating skills.";
			}


			public class BOOSTER_RESEARCH1
			{

				public static LocString NAME = UI.FormatAsLink("Researching Booster", "BOOSTER_RESEARCH1");


				public static LocString DESC = "Grants a Bionic Duplicant the expertise required to study " + UI.FormatAsLink("geysers", "GEYSERS") + " and other advanced topics.";
			}


			public class BOOSTER_RESEARCH2
			{

				public static LocString NAME = UI.FormatAsLink("Astronomy Booster", "BOOSTER_RESEARCH2");


				public static LocString DESC = "Grants a Bionic Duplicant a keen grasp of science and usage of space-research buildings.";
			}


			public class BOOSTER_RESEARCH3
			{

				public static LocString NAME = UI.FormatAsLink("Applied Sciences Booster", "BOOSTER_RESEARCH3");


				public static LocString DESC = "Grants a Bionic Duplicant a deeply pragmatic approach to scientific research.";
			}


			public class BOOSTER_PILOT1
			{

				public static LocString NAME = UI.FormatAsLink("Piloting Booster", "BOOSTER_PILOT1");


				public static LocString DESC = "Grants a Bionic Duplicant the expertise required to explore the skies in person.";
			}


			public class BOOSTER_PILOTVANILLA1
			{

				public static LocString NAME = UI.FormatAsLink("Rocketry Booster", "BOOSTER_PILOTVANILLA1");


				public static LocString DESC = "Grants a Bionic Duplicant the expertise required to command a rocket.";
			}


			public class BOOSTER_SUITS1
			{

				public static LocString NAME = UI.FormatAsLink("Suit Training Booster", "BOOSTER_SUITS1");


				public static LocString DESC = "Enables a Bionic Duplicant to maximize durability of equipped " + UI.FormatAsLink("Exosuits", "EQUIPMENT") + " and maintain their runspeed.";
			}


			public class BOOSTER_CARRY1
			{

				public static LocString NAME = UI.FormatAsLink("Strength Booster", "BOOSTER_CARRY1");


				public static LocString DESC = "Grants a Bionic Duplicant increased carrying capacity and athletic prowess.";
			}


			public class BOOSTER_OP1
			{

				public static LocString NAME = UI.FormatAsLink("Electrical Engineering Booster", "BOOSTER_OP1");


				public static LocString DESC = "Grants a Bionic Duplicant the skills requried to tinker and solder to their heart's content.";
			}


			public class BOOSTER_OP2
			{

				public static LocString NAME = UI.FormatAsLink("Mechatronics Engineering Booster", "BOOSTER_OP2");


				public static LocString DESC = "Grants a Bionic Duplicant complete mastery of engineering skills.";
			}


			public class BOOSTER_MEDICINE1
			{

				public static LocString NAME = UI.FormatAsLink("Advanced Medical Booster", "BOOSTER_MEDICINE1");


				public static LocString DESC = "Grants a Bionic Duplicant the ability to perform all doctoring errands.";
			}


			public class BOOSTER_TIDY1
			{

				public static LocString NAME = UI.FormatAsLink("Tidying Booster", "BOOSTER_TIDY1");


				public static LocString DESC = "Grants a Bionic Duplicant the full range of tidying skills, including blasting unwanted meteors out of the sky.";
			}
		}


		public class FOOD
		{

			public static LocString COMPOST = "Compost";


			public class FOODSPLAT
			{

				public static LocString NAME = "Food Splatter";


				public static LocString DESC = "Food smeared on the wall from a recent Food Fight";
			}


			public class BURGER
			{

				public static LocString NAME = UI.FormatAsLink("Frost Burger", "BURGER");


				public static LocString DESC = string.Concat(new string[]
				{
					UI.FormatAsLink("Meat", "MEAT"),
					" and ",
					UI.FormatAsLink("Lettuce", "LETTUCE"),
					" on a chilled ",
					UI.FormatAsLink("Frost Bun", "COLDWHEATBREAD"),
					".\n\nIt's the only burger best served cold."
				});


				public static LocString RECIPEDESC = string.Concat(new string[]
				{
					UI.FormatAsLink("Meat", "MEAT"),
					" and ",
					UI.FormatAsLink("Lettuce", "LETTUCE"),
					" on a chilled ",
					UI.FormatAsLink("Frost Bun", "COLDWHEATBREAD"),
					"."
				});


				public class DEHYDRATED
				{

					public static LocString NAME = "Dried Frost Burger";


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


			public class FIELDRATION
			{

				public static LocString NAME = UI.FormatAsLink("Nutrient Bar", "FIELDRATION");


				public static LocString DESC = "A nourishing nutrient paste, sandwiched between thin wafer layers.";
			}


			public class MUSHBAR
			{

				public static LocString NAME = UI.FormatAsLink("Mush Bar", "MUSHBAR");


				public static LocString DESC = "An edible, putrefied mudslop.\n\nMush Bars are preferable to starvation, but only just barely.";


				public static LocString RECIPEDESC = "An edible, putrefied mudslop.\n\n" + ITEMS.FOOD.MUSHBAR.NAME + "s are preferable to starvation, but only just barely.";
			}


			public class MUSHROOMWRAP
			{

				public static LocString NAME = UI.FormatAsLink("Mushroom Wrap", "MUSHROOMWRAP");


				public static LocString DESC = string.Concat(new string[]
				{
					"Flavorful ",
					UI.FormatAsLink("Mushrooms", "MUSHROOM"),
					" wrapped in ",
					UI.FormatAsLink("Lettuce", "LETTUCE"),
					".\n\nIt has an earthy flavor punctuated by a refreshing crunch."
				});


				public static LocString RECIPEDESC = string.Concat(new string[]
				{
					"Flavorful ",
					UI.FormatAsLink("Mushrooms", "MUSHROOM"),
					" wrapped in ",
					UI.FormatAsLink("Lettuce", "LETTUCE"),
					"."
				});


				public class DEHYDRATED
				{

					public static LocString NAME = "Dried Mushroom Wrap";


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


			public class MICROWAVEDLETTUCE
			{

				public static LocString NAME = UI.FormatAsLink("Microwaved Lettuce", "MICROWAVEDLETTUCE");


				public static LocString DESC = UI.FormatAsLink("Lettuce", "LETTUCE") + " scrumptiously wilted in the " + BUILDINGS.PREFABS.GAMMARAYOVEN.NAME + ".";


				public static LocString RECIPEDESC = UI.FormatAsLink("Lettuce", "LETTUCE") + " scrumptiously wilted in the " + BUILDINGS.PREFABS.GAMMARAYOVEN.NAME + ".";
			}


			public class GAMMAMUSH
			{

				public static LocString NAME = UI.FormatAsLink("Gamma Mush", "GAMMAMUSH");


				public static LocString DESC = "A disturbingly delicious mixture of irradiated dirt and water.";


				public static LocString RECIPEDESC = UI.FormatAsLink("Mush Fry", "FRIEDMUSHBAR") + " reheated in a " + BUILDINGS.PREFABS.GAMMARAYOVEN.NAME + ".";
			}


			public class FRUITCAKE
			{

				public static LocString NAME = UI.FormatAsLink("Berry Sludge", "FRUITCAKE");


				public static LocString DESC = "A mashed up " + UI.FormatAsLink("Bristle Berry", "PRICKLEFRUIT") + " sludge with an exceptionally long shelf life.\n\nIts aggressive, overbearing sweetness can leave the tongue feeling temporarily numb.";


				public static LocString RECIPEDESC = "A mashed up " + UI.FormatAsLink("Bristle Berry", "PRICKLEFRUIT") + " sludge with an exceptionally long shelf life.";
			}


			public class POPCORN
			{

				public static LocString NAME = UI.FormatAsLink("Popcorn", "POPCORN");


				public static LocString DESC = UI.FormatAsLink("Sleet Wheat Grain", "COLDWHEATSEED") + " popped in a " + BUILDINGS.PREFABS.GAMMARAYOVEN.NAME + ".\n\nCompletely devoid of any fancy flavorings.";


				public static LocString RECIPEDESC = "Gamma-radiated " + UI.FormatAsLink("Sleet Wheat Grain", "COLDWHEATSEED") + ".";
			}


			public class SUSHI
			{

				public static LocString NAME = UI.FormatAsLink("Sushi", "SUSHI");


				public static LocString DESC = string.Concat(new string[]
				{
					"Raw ",
					UI.FormatAsLink("Pacu Fillet", "FISHMEAT"),
					" wrapped with fresh ",
					UI.FormatAsLink("Lettuce", "LETTUCE"),
					".\n\nWhile the salt of the lettuce may initially overpower the flavor, a keen palate can discern the subtle sweetness of the fillet beneath."
				});


				public static LocString RECIPEDESC = string.Concat(new string[]
				{
					"Raw ",
					UI.FormatAsLink("Pacu Fillet", "FISHMEAT"),
					" wrapped with fresh ",
					UI.FormatAsLink("Lettuce", "LETTUCE"),
					"."
				});
			}


			public class HATCHEGG
			{

				public static LocString NAME = CREATURES.SPECIES.HATCH.EGG_NAME;


				public static LocString DESC = string.Concat(new string[]
				{
					"An egg laid by a ",
					UI.FormatAsLink("Hatch", "HATCH"),
					".\n\nIf incubated, it will hatch into a ",
					UI.FormatAsLink("Hatchling", "HATCH"),
					"."
				});


				public static LocString RECIPEDESC = "An egg laid by a " + UI.FormatAsLink("Hatch", "HATCH") + ".";
			}


			public class DRECKOEGG
			{

				public static LocString NAME = CREATURES.SPECIES.DRECKO.EGG_NAME;


				public static LocString DESC = string.Concat(new string[]
				{
					"An egg laid by a ",
					UI.FormatAsLink("Drecko", "DRECKO"),
					".\n\nIf incubated, it will hatch into a new ",
					UI.FormatAsLink("Drecklet", "DRECKO"),
					"."
				});


				public static LocString RECIPEDESC = "An egg laid by a " + UI.FormatAsLink("Drecko", "DRECKO") + ".";
			}


			public class LIGHTBUGEGG
			{

				public static LocString NAME = CREATURES.SPECIES.LIGHTBUG.EGG_NAME;


				public static LocString DESC = string.Concat(new string[]
				{
					"An egg laid by a ",
					UI.FormatAsLink("Shine Bug", "LIGHTBUG"),
					".\n\nIf incubated, it will hatch into a ",
					UI.FormatAsLink("Shine Nymph", "LIGHTBUG"),
					"."
				});


				public static LocString RECIPEDESC = "An egg laid by a " + UI.FormatAsLink("Shine Bug", "LIGHTBUG") + ".";
			}


			public class LETTUCE
			{

				public static LocString NAME = UI.FormatAsLink("Lettuce", "LETTUCE");


				public static LocString DESC = "Crunchy, slightly salty leaves from a " + UI.FormatAsLink("Waterweed", "SEALETTUCE") + " plant.";


				public static LocString RECIPEDESC = "Edible roughage from a " + UI.FormatAsLink("Waterweed", "SEALETTUCE") + ".";
			}


			public class PASTA
			{

				public static LocString NAME = UI.FormatAsLink("Pasta", "PASTA");


				public static LocString DESC = "pasta made from egg and wheat";


				public static LocString RECIPEDESC = "pasta made from egg and wheat";
			}


			public class PANCAKES
			{

				public static LocString NAME = UI.FormatAsLink("Soufflé Pancakes", "PANCAKES");


				public static LocString DESC = string.Concat(new string[]
				{
					"Sweet discs made from ",
					UI.FormatAsLink("Raw Egg", "RAWEGG"),
					" and ",
					UI.FormatAsLink("Sleet Wheat Grain", "COLDWHEATSEED"),
					".\n\nThey're so thick!"
				});


				public static LocString RECIPEDESC = string.Concat(new string[]
				{
					"Sweet discs made from ",
					UI.FormatAsLink("Raw Egg", "RAWEGG"),
					" and ",
					UI.FormatAsLink("Sleet Wheat Grain", "COLDWHEATSEED"),
					"."
				});
			}


			public class OILFLOATEREGG
			{

				public static LocString NAME = CREATURES.SPECIES.OILFLOATER.EGG_NAME;


				public static LocString DESC = string.Concat(new string[]
				{
					"An egg laid by a ",
					UI.FormatAsLink("Slickster", "OILFLOATER"),
					".\n\nIf incubated, it will hatch into a ",
					UI.FormatAsLink("Slickster Larva", "OILFLOATER"),
					"."
				});


				public static LocString RECIPEDESC = "An egg laid by a " + UI.FormatAsLink("Slickster", "OILFLOATER") + ".";
			}


			public class PUFTEGG
			{

				public static LocString NAME = CREATURES.SPECIES.PUFT.EGG_NAME;


				public static LocString DESC = string.Concat(new string[]
				{
					"An egg laid by a ",
					UI.FormatAsLink("Puft", "PUFT"),
					".\n\nIf incubated, it will hatch into a ",
					UI.FormatAsLink("Puftlet", "PUFT"),
					"."
				});


				public static LocString RECIPEDESC = "An egg laid by a " + CREATURES.SPECIES.PUFT.NAME + ".";
			}


			public class PREHISTORICPACUFILLET
			{

				public static LocString NAME = UI.FormatAsLink("Jawbo Fillet", "PREHISTORICPACUFILLET");


				public static LocString DESC = "An uncooked fillet from a very dead " + CREATURES.SPECIES.PREHISTORICPACU.NAME + ". It has a silky texture.";
			}


			public class FISHMEAT
			{

				public static LocString NAME = UI.FormatAsLink("Pacu Fillet", "FISHMEAT");


				public static LocString DESC = "An uncooked fillet from a very dead " + CREATURES.SPECIES.PACU.NAME + ". Yum!";
			}


			public class MEAT
			{

				public static LocString NAME = UI.FormatAsLink("Meat", "MEAT");


				public static LocString DESC = "Uncooked meat from a very dead critter. Yum!";
			}


			public class DINOSAURMEAT
			{

				public static LocString NAME = UI.FormatAsLink("Tough Meat", "DINOSAURMEAT");


				public static LocString DESC = "Uncooked meat from a very dead critter.\n\nIt's inedible until cooked in the " + BUILDINGS.PREFABS.SMOKER.NAME + ".";
			}


			public class SMOKEDDINOSAURMEAT
			{

				public static LocString NAME = UI.FormatAsLink("Tender Brisket", "SMOKEDDINOSAURMEAT");


				public static LocString DESC = "A cooked stack of tough meat that's been marinated and slow-smoked to tender perfection.";


				public static LocString RECIPEDESC = "A stack of tender, slow-smoked meat.";
			}


			public class SMOKEDFISH
			{

				public static LocString NAME = UI.FormatAsLink("Smoked Fish", "SMOKEDFISH");


				public static LocString DESC = "A buttery smoked fish fillet.\n\nIt flakes nicely when pulled apart with a fork.";


				public static LocString RECIPEDESC = "A buttery smoked fish fillet.";
			}


			public class SMOKEDVEGETABLES
			{

				public static LocString NAME = UI.FormatAsLink("Veggie Poppers", "SMOKEDVEGETABLES");


				public static LocString DESC = "Crisp vegetables stuffed with herbs and smoked for hours.";


				public static LocString RECIPEDESC = "Crisp vegetables stuffed with herbs.";
			}


			public class PLANTMEAT
			{

				public static LocString NAME = UI.FormatAsLink("Plant Meat", "PLANTMEAT");


				public static LocString DESC = "Planty plant meat from a plant. How nice!";
			}


			public class SHELLFISHMEAT
			{

				public static LocString NAME = UI.FormatAsLink("Raw Shellfish", "SHELLFISHMEAT");


				public static LocString DESC = "An uncooked chunk of very dead " + CREATURES.SPECIES.CRAB.VARIANT_FRESH_WATER.NAME + ". Yum!";
			}


			public class MUSHROOM
			{

				public static LocString NAME = UI.FormatAsLink("Mushroom", "MUSHROOM");


				public static LocString DESC = "An edible, flavorless fungus that grew in the dark.";
			}


			public class COOKEDFISH
			{

				public static LocString NAME = UI.FormatAsLink("Cooked Seafood", "COOKEDFISH");


				public static LocString DESC = "A cooked piece of freshly caught aquatic critter.\n\nUnsurprisingly, it tastes a bit fishy.";


				public static LocString RECIPEDESC = "A cooked piece of freshly caught aquatic critter.";
			}


			public class COOKEDMEAT
			{

				public static LocString NAME = UI.FormatAsLink("Barbeque", "COOKEDMEAT");


				public static LocString DESC = "The cooked meat of a defeated critter.\n\nIt has a delightful smoky aftertaste.";


				public static LocString RECIPEDESC = "The cooked meat of a defeated critter.";
			}


			public class FRIESCARROT
			{

				public static LocString NAME = UI.FormatAsLink("Squash Fries", "FRIESCARROT");


				public static LocString DESC = "Irresistibly crunchy.\n\nBest eaten hot.";


				public static LocString RECIPEDESC = string.Concat(new string[]
				{
					"Crunchy sticks of ",
					UI.FormatAsLink("Plume Squash", "CARROT"),
					" deep-fried in ",
					UI.FormatAsLink("Tallow", "TALLOW"),
					"."
				});
			}


			public class DEEPFRIEDFISH
			{

				public static LocString NAME = UI.FormatAsLink("Fish Taco", "DEEPFRIEDFISH");


				public static LocString DESC = "Deep-fried fish cradled in a crunchy fin.";


				public static LocString RECIPEDESC = UI.FormatAsLink("Pacu Fillet", "FISHMEAT") + " lightly battered and deep-fried in " + UI.FormatAsLink("Tallow", "TALLOW") + ".";
			}


			public class DEEPFRIEDSHELLFISH
			{

				public static LocString NAME = UI.FormatAsLink("Shellfish Tempura", "DEEPFRIEDSHELLFISH");


				public static LocString DESC = "A crispy deep-fried critter claw.";


				public static LocString RECIPEDESC = string.Concat(new string[]
				{
					"A tender chunk of battered ",
					UI.FormatAsLink("Raw Shellfish", "SHELLFISHMEAT"),
					" deep-fried in ",
					UI.FormatAsLink("Tallow", "TALLOW"),
					"."
				});
			}


			public class DEEPFRIEDMEAT
			{

				public static LocString NAME = UI.FormatAsLink("Deep Fried Steak", "DEEPFRIEDMEAT");


				public static LocString DESC = "A juicy slab of meat with a crunchy deep-fried upper layer.";


				public static LocString RECIPEDESC = string.Concat(new string[]
				{
					"A juicy slab of ",
					UI.FormatAsLink("Raw Meat", "MEAT"),
					" deep-fried in ",
					UI.FormatAsLink("Tallow", "TALLOW"),
					"."
				});
			}


			public class DEEPFRIEDNOSH
			{

				public static LocString NAME = UI.FormatAsLink("Nosh Noms", "DEEPFRIEDNOSH");


				public static LocString DESC = "A snackable handful of crunchy beans.";


				public static LocString RECIPEDESC = string.Concat(new string[]
				{
					"A crunchy stack of ",
					UI.FormatAsLink("Nosh Beans", "BEANPLANTSEED"),
					" deep-fried in ",
					UI.FormatAsLink("Tallow", "TALLOW"),
					"."
				});
			}


			public class PICKLEDMEAL
			{

				public static LocString NAME = UI.FormatAsLink("Pickled Meal", "PICKLEDMEAL");


				public static LocString DESC = "Meal Lice preserved in vinegar.\n\nIt's a rarely acquired taste.";


				public static LocString RECIPEDESC = ITEMS.FOOD.BASICPLANTFOOD.NAME + " regrettably preserved in vinegar.";
			}


			public class FRIEDMUSHBAR
			{

				public static LocString NAME = UI.FormatAsLink("Mush Fry", "FRIEDMUSHBAR");


				public static LocString DESC = "Pan-fried, solidified mudslop.\n\nThe inside is almost completely uncooked, despite the crunch on the outside.";


				public static LocString RECIPEDESC = "Pan-fried, solidified mudslop.";
			}


			public class RAWEGG
			{

				public static LocString NAME = UI.FormatAsLink("Raw Egg", "RAWEGG");


				public static LocString DESC = "A raw Egg that has been cracked open for use in " + UI.FormatAsLink("Food", "FOOD") + " preparation.\n\nIt will never hatch.";


				public static LocString RECIPEDESC = "A raw egg that has been cracked open for use in " + UI.FormatAsLink("Food", "FOOD") + " preparation.";
			}


			public class COOKEDEGG
			{

				public static LocString NAME = UI.FormatAsLink("Omelette", "COOKEDEGG");


				public static LocString DESC = "Fluffed and folded Egg innards.\n\nIt turns out you do, in fact, have to break a few eggs to make it.";


				public static LocString RECIPEDESC = "Fluffed and folded egg innards.";
			}


			public class FRIEDMUSHROOM
			{

				public static LocString NAME = UI.FormatAsLink("Fried Mushroom", "FRIEDMUSHROOM");


				public static LocString DESC = "A pan-fried dish made with a fruiting " + UI.FormatAsLink("Dusk Cap", "MUSHROOM") + ".\n\nIt has a thick, savory flavor with subtle earthy undertones.";


				public static LocString RECIPEDESC = "A pan-fried dish made with a fruiting " + UI.FormatAsLink("Dusk Cap", "MUSHROOM") + ".";
			}


			public class COOKEDPIKEAPPLE
			{

				public static LocString NAME = UI.FormatAsLink("Pikeapple Skewer", "COOKEDPIKEAPPLE");


				public static LocString DESC = "Grilling a " + UI.FormatAsLink("Pikeapple", "HARDSKINBERRY") + " softens its spikes, making it slighly less awkward to eat.\n\nIt does not diminish the smell.";


				public static LocString RECIPEDESC = "A grilled dish made with a fruiting " + UI.FormatAsLink("Pikeapple", "HARDSKINBERRY") + ".";
			}


			public class PRICKLEFRUIT
			{

				public static LocString NAME = UI.FormatAsLink("Bristle Berry", "PRICKLEFRUIT");


				public static LocString DESC = "A sweet, mostly pleasant-tasting fruit covered in prickly barbs.";
			}


			public class GRILLEDPRICKLEFRUIT
			{

				public static LocString NAME = UI.FormatAsLink("Gristle Berry", "GRILLEDPRICKLEFRUIT");


				public static LocString DESC = "The grilled bud of a " + UI.FormatAsLink("Bristle Berry", "PRICKLEFRUIT") + ".\n\nHeat unlocked an exquisite taste in the fruit, though the burnt spines leave something to be desired.";


				public static LocString RECIPEDESC = "The grilled bud of a " + UI.FormatAsLink("Bristle Berry", "PRICKLEFRUIT") + ".";
			}


			public class SWAMPFRUIT
			{

				public static LocString NAME = UI.FormatAsLink("Bog Jelly", "SWAMPFRUIT");


				public static LocString DESC = "A fruit with an outer film that contains chewy gelatinous cubes.";
			}


			public class SWAMPDELIGHTS
			{

				public static LocString NAME = UI.FormatAsLink("Swampy Delights", "SWAMPDELIGHTS");


				public static LocString DESC = "Dried gelatinous cubes from a " + UI.FormatAsLink("Bog Jelly", "SWAMPFRUIT") + ".\n\nEach cube has a wonderfully chewy texture and is lightly coated in a delicate powder.";


				public static LocString RECIPEDESC = "Dried gelatinous cubes from a " + UI.FormatAsLink("Bog Jelly", "SWAMPFRUIT") + ".";
			}


			public class WORMBASICFRUIT
			{

				public static LocString NAME = UI.FormatAsLink("Spindly Grubfruit", "WORMBASICFRUIT");


				public static LocString DESC = "A " + UI.FormatAsLink("Grubfruit", "WORMSUPERFRUIT") + " that failed to develop properly.\n\nIt is nonetheless edible, and vaguely tasty.";
			}


			public class WORMBASICFOOD
			{

				public static LocString NAME = UI.FormatAsLink("Roast Grubfruit Nut", "WORMBASICFOOD");


				public static LocString DESC = "Slow roasted " + UI.FormatAsLink("Spindly Grubfruit", "WORMBASICFRUIT") + ".\n\nIt has a smoky aroma and tastes of coziness.";


				public static LocString RECIPEDESC = "Slow roasted " + UI.FormatAsLink("Spindly Grubfruit", "WORMBASICFRUIT") + ".";
			}


			public class WORMSUPERFRUIT
			{

				public static LocString NAME = UI.FormatAsLink("Grubfruit", "WORMSUPERFRUIT");


				public static LocString DESC = "A plump, healthy fruit with a honey-like taste.";
			}


			public class WORMSUPERFOOD
			{

				public static LocString NAME = UI.FormatAsLink("Grubfruit Preserve", "WORMSUPERFOOD");


				public static LocString DESC = string.Concat(new string[]
				{
					"A long lasting ",
					UI.FormatAsLink("Grubfruit", "WORMSUPERFRUIT"),
					" jam preserved in ",
					UI.FormatAsLink("Sucrose", "SUCROSE"),
					".\n\nThe thick, goopy jam retains the shape of the jar when poured out, but the sweet taste can't be matched."
				});


				public static LocString RECIPEDESC = string.Concat(new string[]
				{
					"A long lasting ",
					UI.FormatAsLink("Grubfruit", "WORMSUPERFRUIT"),
					" jam preserved in ",
					UI.FormatAsLink("Sucrose", "SUCROSE"),
					"."
				});
			}


			public class VINEFRUITJAM
			{

				public static LocString NAME = UI.FormatAsLink("", "VINEFRUITJAM");


				public static LocString DESC = "";


				public static LocString RECIPEDESC = "";
			}


			public class BERRYPIE
			{

				public static LocString NAME = UI.FormatAsLink("Mixed Berry Pie", "BERRYPIE");


				public static LocString DESC = string.Concat(new string[]
				{
					"A pie made primarily of ",
					UI.FormatAsLink("Grubfruit", "WORMSUPERFRUIT"),
					" and ",
					UI.FormatAsLink("Gristle Berries", "PRICKLEFRUIT"),
					".\n\nThe mixture of berries creates a fragrant, colorful filling that packs a sweet punch."
				});


				public static LocString RECIPEDESC = string.Concat(new string[]
				{
					"A pie made primarily of ",
					UI.FormatAsLink("Grubfruit", "WORMSUPERFRUIT"),
					" and ",
					UI.FormatAsLink("Gristle Berries", "PRICKLEFRUIT"),
					"."
				});


				public class DEHYDRATED
				{

					public static LocString NAME = "Dried Berry Pie";


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


			public class COLDWHEATBREAD
			{

				public static LocString NAME = UI.FormatAsLink("Frost Bun", "COLDWHEATBREAD");


				public static LocString DESC = "A simple bun baked from " + UI.FormatAsLink("Sleet Wheat Grain", "COLDWHEATSEED") + ".\n\nEach bite leaves a mild cooling sensation in one's mouth, even when the bun itself is warm.";


				public static LocString RECIPEDESC = "A simple bun baked from " + UI.FormatAsLink("Sleet Wheat Grain", "COLDWHEATSEED") + " grain.";
			}


			public class BEAN
			{

				public static LocString NAME = UI.FormatAsLink("Nosh Bean", "BEAN");


				public static LocString DESC = "The crisp bean of a " + UI.FormatAsLink("Nosh Sprout", "BEAN_PLANT") + ".\n\nEach bite tastes refreshingly natural and wholesome.";
			}


			public class SPICENUT
			{

				public static LocString NAME = UI.FormatAsLink("Pincha Peppernut", "SPICENUT");


				public static LocString DESC = "The flavorful nut of a " + UI.FormatAsLink("Pincha Pepperplant", "SPICE_VINE") + ".\n\nThe bitter outer rind hides a rich, peppery core that is useful in cooking.";
			}


			public class VINEFRUIT
			{

				public static LocString NAME = UI.FormatAsLink("Ovagro Fig", "VINEFRUIT");


				public static LocString DESC = "These fruit from an " + UI.FormatAsLink("Ovagro Vine", "VINEMOTHER") + ".\n\nIt's fun to squeeze as many as possible in a single mouthful.";
			}


			public class SPICEBREAD
			{

				public static LocString NAME = UI.FormatAsLink("Pepper Bread", "SPICEBREAD");


				public static LocString DESC = "A loaf of bread, lightly spiced with " + UI.FormatAsLink("Pincha Peppernut", "SPICENUT") + " for a mild bite.\n\nThere's a simple joy to be had in pulling it apart in one's fingers.";


				public static LocString RECIPEDESC = "A loaf of bread, lightly spiced with " + UI.FormatAsLink("Pincha Peppernut", "SPICENUT") + " for a mild bite.";


				public class DEHYDRATED
				{

					public static LocString NAME = "Dried Pepper Bread";


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


			public class SURFANDTURF
			{

				public static LocString NAME = UI.FormatAsLink("Surf'n'Turf", "SURFANDTURF");


				public static LocString DESC = string.Concat(new string[]
				{
					"A bit of ",
					UI.FormatAsLink("Meat", "MEAT"),
					" from the land and ",
					UI.FormatAsLink("Cooked Seafood", "COOKEDFISH"),
					" from the sea.\n\nIt's hearty and satisfying."
				});


				public static LocString RECIPEDESC = string.Concat(new string[]
				{
					"A bit of ",
					UI.FormatAsLink("Meat", "MEAT"),
					" from the land and ",
					UI.FormatAsLink("Cooked Seafood", "COOKEDFISH"),
					" from the sea."
				});


				public class DEHYDRATED
				{

					public static LocString NAME = "Dried Surf'n'Turf";


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


			public class TOFU
			{

				public static LocString NAME = UI.FormatAsLink("Tofu", "TOFU");


				public static LocString DESC = "A bland curd made from " + UI.FormatAsLink("Nosh Beans", "BEANPLANTSEED") + ".\n\nIt has an unusual but pleasant consistency.";


				public static LocString RECIPEDESC = "A bland curd made from " + UI.FormatAsLink("Nosh Beans", "BEANPLANTSEED") + ".";
			}


			public class SPICYTOFU
			{

				public static LocString NAME = UI.FormatAsLink("Spicy Tofu", "SPICYTOFU");


				public static LocString DESC = ITEMS.FOOD.TOFU.NAME + " marinated in a flavorful " + UI.FormatAsLink("Pincha Peppernut", "SPICENUT") + " sauce.\n\nIt packs a delightful punch.";


				public static LocString RECIPEDESC = ITEMS.FOOD.TOFU.NAME + " marinated in a flavorful " + UI.FormatAsLink("Pincha Peppernut", "SPICENUT") + " sauce.";


				public class DEHYDRATED
				{

					public static LocString NAME = "Dried Spicy Tofu";


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


			public class CURRY
			{

				public static LocString NAME = UI.FormatAsLink("Curried Beans", "CURRY");


				public static LocString DESC = string.Concat(new string[]
				{
					"Chewy ",
					UI.FormatAsLink("Nosh Beans", "BEANPLANTSEED"),
					" simmered with chunks of ",
					ITEMS.INGREDIENTS.GINGER.NAME,
					".\n\nIt's so spicy!"
				});


				public static LocString RECIPEDESC = string.Concat(new string[]
				{
					"Chewy ",
					UI.FormatAsLink("Nosh Beans", "BEANPLANTSEED"),
					" simmered with chunks of ",
					ITEMS.INGREDIENTS.GINGER.NAME,
					"."
				});


				public class DEHYDRATED
				{

					public static LocString NAME = "Dried Curried Beans";


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


			public class SALSA
			{

				public static LocString NAME = UI.FormatAsLink("Stuffed Berry", "SALSA");


				public static LocString DESC = "A baked " + UI.FormatAsLink("Bristle Berry", "PRICKLEFRUIT") + " stuffed with delectable spices and vibrantly flavored.";


				public static LocString RECIPEDESC = "A baked " + UI.FormatAsLink("Bristle Berry", "PRICKLEFRUIT") + " stuffed with delectable spices and vibrantly flavored.";


				public class DEHYDRATED
				{

					public static LocString NAME = "Dried Stuffed Berry";


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


			public class HARDSKINBERRY
			{

				public static LocString NAME = UI.FormatAsLink("Pikeapple", "HARDSKINBERRY");


				public static LocString DESC = "An edible fruit encased in a thorny husk.";
			}


			public class CARROT
			{

				public static LocString NAME = UI.FormatAsLink("Plume Squash", "CARROT");


				public static LocString DESC = "An edible tuber with an earthy, elegant flavor.";
			}


			public class FERNFOOD
			{

				public static LocString NAME = UI.FormatAsLink("Megafrond Grain", "FERNFOOD");


				public static LocString DESC = "An ancient grain that can be processed into " + UI.FormatAsLink("Food", "FOOD") + ".";
			}


			public class PEMMICAN
			{

				public static LocString NAME = UI.FormatAsLink("Pemmican", "PEMMICAN");


				public static LocString DESC = UI.FormatAsLink("Meat", "MEAT") + " and " + UI.FormatAsLink("Tallow", "TALLOW") + " pounded into a calorie-dense brick with an exceptionally long shelf life.\n\nSurvival never tasted so good.";


				public static LocString RECIPEDESC = UI.FormatAsLink("Meat", "MEAT") + " and " + UI.FormatAsLink("Tallow", "TALLOW") + " pounded into a nutrient-dense brick with an exceptionally long shelf life.";
			}


			public class BASICPLANTFOOD
			{

				public static LocString NAME = UI.FormatAsLink("Meal Lice", "BASICPLANTFOOD");


				public static LocString DESC = "A flavorless grain that almost never wiggles on its own.";
			}


			public class BASICPLANTBAR
			{

				public static LocString NAME = UI.FormatAsLink("Liceloaf", "BASICPLANTBAR");


				public static LocString DESC = UI.FormatAsLink("Meal Lice", "BASICPLANTFOOD") + " compacted into a dense, immobile loaf.";


				public static LocString RECIPEDESC = UI.FormatAsLink("Meal Lice", "BASICPLANTFOOD") + " compacted into a dense, immobile loaf.";
			}


			public class BASICFORAGEPLANT
			{

				public static LocString NAME = UI.FormatAsLink("Muckroot", "BASICFORAGEPLANT");


				public static LocString DESC = "A seedless fruit with an upsettingly bland aftertaste.\n\nIt cannot be replanted.\n\nDigging up Buried Objects may uncover a " + ITEMS.FOOD.BASICFORAGEPLANT.NAME + ".";
			}


			public class FORESTFORAGEPLANT
			{

				public static LocString NAME = UI.FormatAsLink("Hexalent Fruit", "FORESTFORAGEPLANT");


				public static LocString DESC = "A seedless fruit with an unusual rubbery texture.\n\nIt cannot be replanted.\n\nHexalent fruit is much more calorie dense than Muckroot fruit.";
			}


			public class SWAMPFORAGEPLANT
			{

				public static LocString NAME = UI.FormatAsLink("Swamp Chard Heart", "SWAMPFORAGEPLANT");


				public static LocString DESC = "A seedless plant with a squishy, juicy center and an awful smell.\n\nIt cannot be replanted.";
			}


			public class ICECAVESFORAGEPLANT
			{

				public static LocString NAME = UI.FormatAsLink("Sherberry", "ICECAVESFORAGEPLANT");


				public static LocString DESC = "A cold seedless fruit that triggers mild brain freeze.\n\nIt cannot be replanted.";
			}


			public class ROTPILE
			{

				public static LocString NAME = UI.FormatAsLink("Rot Pile", "COMPOST");


				public static LocString DESC = string.Concat(new string[]
				{
					"An inedible glop of former foodstuff.\n\n",
					ITEMS.FOOD.ROTPILE.NAME,
					"s break down into ",
					UI.FormatAsLink("Polluted Dirt", "TOXICSAND"),
					" over time."
				});
			}


			public class COLDWHEATSEED
			{

				public static LocString NAME = UI.FormatAsLink("Sleet Wheat Grain", "COLDWHEATSEED");


				public static LocString DESC = "An edible grain that leaves a cool taste on the tongue.";
			}


			public class BEANPLANTSEED
			{

				public static LocString NAME = UI.FormatAsLink("Nosh Bean", "BEANPLANTSEED");


				public static LocString DESC = "An inedible bean that can be processed into delicious foods.";
			}


			public class QUICHE
			{

				public static LocString NAME = UI.FormatAsLink("Mushroom Quiche", "QUICHE");


				public static LocString DESC = string.Concat(new string[]
				{
					UI.FormatAsLink("Omelette", "COOKEDEGG"),
					", ",
					UI.FormatAsLink("Fried Mushroom", "FRIEDMUSHROOM"),
					" and ",
					UI.FormatAsLink("Lettuce", "LETTUCE"),
					" piled onto a yummy crust.\n\nSomehow, it's both soggy <i>and</i> crispy."
				});


				public static LocString RECIPEDESC = string.Concat(new string[]
				{
					UI.FormatAsLink("Omelette", "COOKEDEGG"),
					", ",
					UI.FormatAsLink("Fried Mushroom", "FRIEDMUSHROOM"),
					" and ",
					UI.FormatAsLink("Lettuce", "LETTUCE"),
					" piled onto a yummy crust."
				});


				public class DEHYDRATED
				{

					public static LocString NAME = "Dried Mushroom Quiche";


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


			public class GARDENFOODPLANTFOOD
			{

				public static LocString NAME = UI.FormatAsLink("Sweatcorn", "GARDENFOODPLANTFOOD");


				public static LocString DESC = string.Concat(new string[]
				{
					"The sugary fruit of a ",
					UI.FormatAsLink("Sweatcorn Stalk", "GARDENFOODPLANT"),
					"\n\nSweatcorn is more calorie-dense than ",
					UI.FormatAsLink("Snac Fruit", "GARDENFORAGEPLANT"),
					"."
				});
			}


			public class GARDENFORAGEPLANT
			{

				public static LocString NAME = UI.FormatAsLink("Snac Fruit", "GARDENFORAGEPLANT");


				public static LocString DESC = "A seedless fruit that loses its flavor long before it is fully chewed.\n\nIt cannot be replanted.\n\nDigging up Buried Objects may uncover a " + ITEMS.FOOD.GARDENFORAGEPLANT.NAME + ".";
			}


			public class BUTTERFLYPLANTSEED
			{

				public static LocString NAME = UI.FormatAsLink("Mimillet", "BUTTERFLYPLANT");


				public static LocString DESC = string.Concat(new string[]
				{
					"An inedible seed from a ",
					UI.FormatAsLink("Mimika Bud", "BUTTERFLYPLANT"),
					".\n\nIt can be sown to cultivate more plants, or processed into ",
					UI.FormatAsLink("Food", "FOOD"),
					".\n\nDigging up Buried Objects may uncover a Mimillet Seed."
				});


				public static LocString RECIPEDESC = "An inedible " + UI.FormatAsLink("Mimillet", "BUTTERFLYPLANT") + " seed.";
			}


			public class BUTTERFLYFOOD
			{

				public static LocString NAME = UI.FormatAsLink("Toasted Mimillet", "BUTTERFLYFOOD");


				public static LocString DESC = "A lightly toasted " + UI.FormatAsLink("Mimillet", "BUTTERFLYPLANT") + ".\n\nIt makes the tummy feel a bit fluttery.";


				public static LocString RECIPEDESC = "A lightly toasted " + UI.FormatAsLink("Mimillet", "BUTTERFLYPLANT") + ".";
			}
		}


		public class INGREDIENTS
		{

			public class SWAMPLILYFLOWER
			{

				public static LocString NAME = UI.FormatAsLink("Balm Lily Flower", "SWAMPLILYFLOWER");


				public static LocString DESC = "A medicinal flower that soothes most minor maladies.\n\nIt is exceptionally fragrant.";
			}


			public class GINGER
			{

				public static LocString NAME = UI.FormatAsLink("Tonic Root", "GINGERCONFIG");


				public static LocString DESC = "A chewy, fibrous rhizome with a fiery aftertaste.";
			}


			public class KELP
			{

				public static LocString NAME = UI.FormatAsLink("Seakomb Leaf", "KELP");


				public static LocString DESC = string.Concat(new string[]
				{
					"The leaf of a ",
					UI.FormatAsLink("Seakomb", "KELPPLANT"),
					".\n\nIt can be processed into ",
					UI.FormatAsLink("Phyto Oil", "PHYTOOIL"),
					" or used as an ingredient in ",
					UI.FormatAsLink("Allergy Medication", "ANTIHISTAMINE "),
					"."
				});
			}
		}


		public class INDUSTRIAL_PRODUCTS
		{

			public class ELECTROBANK_URANIUM_ORE
			{

				public static LocString NAME = UI.FormatAsLink("Uranium Ore Power Bank", "ELECTROBANK_URANIUM_ORE");


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


			public class ELECTROBANK_METAL_ORE
			{

				public static LocString NAME = UI.FormatAsLink("Metal Power Bank", "ELECTROBANK_METAL_ORE");


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


			public class ELECTROBANK_SELFCHARGING
			{

				public static LocString NAME = UI.FormatAsLink("Atomic Power Bank", "ELECTROBANK_SELFCHARGING");


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


			public class ELECTROBANK
			{

				public static LocString NAME = UI.FormatAsLink("Eco Power Bank", "ELECTROBANK");


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


			public class ELECTROBANK_EMPTY
			{

				public static LocString NAME = UI.FormatAsLink("Empty Eco Power Bank", "ELECTROBANK");


				public static LocString DESC = string.Concat(new string[]
				{
					"A depleted ",
					UI.FormatAsLink("Power Bank", "ELECTROBANK"),
					".\n\nIt must be recharged at a ",
					UI.FormatAsLink("Power Bank Charger", "ELECTROBANKCHARGER"),
					" before it can be reused."
				});
			}


			public class ELECTROBANK_GARBAGE
			{

				public static LocString NAME = UI.FormatAsLink("Power Bank Scrap", "ELECTROBANK");


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


			public class FUEL_BRICK
			{

				public static LocString NAME = "Fuel Brick";


				public static LocString DESC = "A densely compressed brick of combustible material.\n\nIt can be burned to produce a one-time burst of " + UI.FormatAsLink("Power", "POWER") + ".";
			}


			public class BASIC_FABRIC
			{

				public static LocString NAME = UI.FormatAsLink("Reed Fiber", "BASIC_FABRIC");


				public static LocString DESC = "A ball of raw cellulose used in the production of " + UI.FormatAsLink("Clothing", "EQUIPMENT") + " and textiles.";
			}


			public class FEATHER_FABRIC
			{

				public static LocString NAME = UI.FormatAsLink("Feather Fiber", "FEATHER_FABRIC");


				public static LocString DESC = "A stalk of raw keratin used in the production of " + UI.FormatAsLink("Clothing", "EQUIPMENT") + " and textiles.";
			}


			public class DEWDRIP
			{

				public static LocString NAME = UI.FormatAsLink("Dewdrip", "DEWDRIP");


				public static LocString DESC = string.Concat(new string[]
				{
					"A crystallized blob of ",
					UI.FormatAsLink("Brackene", "MILK"),
					" from the ",
					UI.FormatAsLink("Dew Dripper", "DEWDRIPPERPLANT"),
					"."
				});
			}


			public class TRAP_PARTS
			{

				public static LocString NAME = "Trap Components";


				public static LocString DESC = string.Concat(new string[]
				{
					"These components can be assembled into a ",
					BUILDINGS.PREFABS.CREATURETRAP.NAME,
					" and used to catch ",
					UI.FormatAsLink("Critters", "CREATURES"),
					"."
				});
			}


			public class POWER_STATION_TOOLS
			{

				public static LocString NAME = UI.FormatAsLink("Microchip", "POWER_STATION_TOOLS");


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


				public static LocString TINKER_REQUIREMENT_NAME = "Skill: " + DUPLICANTS.ROLES.POWER_TECHNICIAN.NAME;


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


				public static LocString TINKER_EFFECT_NAME = "Engie's Tune-Up: {0} {1}";


				public static LocString TINKER_EFFECT_TOOLTIP = string.Concat(new string[]
				{
					"Can be used to ",
					UI.PRE_KEYWORD,
					"Tune Up",
					UI.PST_KEYWORD,
					" a generator, increasing its {0} by <b>{1}</b>."
				});


				public static LocString RECIPE_DESCRIPTION = "Make " + ITEMS.INDUSTRIAL_PRODUCTS.POWER_STATION_TOOLS.NAME + " from {0}";
			}


			public class FARM_STATION_TOOLS
			{

				public static LocString NAME = UI.FormatAsLink("Micronutrient Fertilizer", "FARM_STATION_TOOLS");


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


			public class MACHINE_PARTS
			{

				public static LocString NAME = "Custom Parts";


				public static LocString DESC = string.Concat(new string[]
				{
					"Specialized Parts crafted by a professional engineer.\n\n",
					UI.PRE_KEYWORD,
					"Jerry Rig",
					UI.PST_KEYWORD,
					" machine buildings to increase their efficiency."
				});


				public static LocString TINKER_REQUIREMENT_NAME = "Job: " + DUPLICANTS.ROLES.MECHATRONIC_ENGINEER.NAME;


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


				public static LocString TINKER_EFFECT_NAME = "Engineer's Jerry Rig: {0} {1}";


				public static LocString TINKER_EFFECT_TOOLTIP = string.Concat(new string[]
				{
					"Can be used to ",
					UI.PRE_KEYWORD,
					"Jerry Rig",
					UI.PST_KEYWORD,
					" upgrades to a machine building, increasing its {0} by <b>{1}</b>."
				});
			}


			public class RESEARCH_DATABANK
			{

				public static LocString NAME = UI.FormatAsLink("Data Bank", "DATABANK");


				public static LocString NAME_PLURAL = UI.FormatAsLink("Data Banks", "DATABANK");


				public static LocString DESC = "Raw data that can be processed into " + UI.FormatAsLink("Interstellar Research", "RESEARCH") + " points.";
			}


			public class ORBITAL_RESEARCH_DATABANK
			{

				public static LocString NAME = UI.FormatAsLink("Data Bank", "DATABANK");


				public static LocString NAME_PLURAL = UI.FormatAsLink("Data Banks", "DATABANK");


				public static LocString DESC = "Raw Data that can be processed into " + UI.FormatAsLink("Data Analysis Research", "RESEARCHDLC1") + " points.";


				public static LocString RECIPE_DESC = string.Concat(new string[]
				{
					"Data Banks of raw data generated from exploring, either by exploring new areas with Duplicants, or by using an ",
					UI.FormatAsLink("Orbital Data Collection Lab", "ORBITALRESEARCHCENTER"),
					".\n\nUsed by the ",
					UI.FormatAsLink("Virtual Planetarium", "DLC1COSMICRESEARCHCENTER"),
					" to conduct research."
				});
			}


			public class EGG_SHELL
			{

				public static LocString NAME = UI.FormatAsLink("Egg Shell", "EGG_SHELL");


				public static LocString DESC = "Can be crushed to produce " + UI.FormatAsLink("Lime", "LIME") + ".";
			}


			public class GOLD_BELLY_CROWN
			{

				public static LocString NAME = UI.FormatAsLink("Regal Bammoth Crest", "GOLD_BELLY_CROWN");


				public static LocString DESC = "Can be crushed to produce " + ELEMENTS.GOLDAMALGAM.NAME + ".";
			}


			public class CRAB_SHELL
			{

				public static LocString NAME = UI.FormatAsLink("Pokeshell Molt", "CRAB_SHELL");


				public static LocString DESC = "Can be crushed to produce " + UI.FormatAsLink("Lime", "LIME") + ".";


				public class VARIANT_WOOD
				{

					public static LocString NAME = UI.FormatAsLink("Oakshell Molt", "CRABWOODSHELL");


					public static LocString DESC = "Can be crushed to produce " + UI.FormatAsLink("Wood", "WOOD") + ".";
				}
			}


			public class BABY_CRAB_SHELL
			{

				public static LocString NAME = UI.FormatAsLink("Small Pokeshell Molt", "CRAB_SHELL");


				public static LocString DESC = "Can be crushed to produce " + UI.FormatAsLink("Lime", "LIME") + ".";


				public class VARIANT_WOOD
				{

					public static LocString NAME = UI.FormatAsLink("Small Oakshell Molt", "CRABWOODSHELL");


					public static LocString DESC = "Can be crushed to produce " + UI.FormatAsLink("Wood", "WOOD") + ".";
				}
			}


			public class WOOD
			{

				public static LocString NAME = UI.FormatAsLink("Wood", "WOOD");


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


			public class GENE_SHUFFLER_RECHARGE
			{

				public static LocString NAME = "Vacillator Recharge";


				public static LocString DESC = "Replenishes one charge to a depleted " + BUILDINGS.PREFABS.GENESHUFFLER.NAME + ".";
			}


			public class TABLE_SALT
			{

				public static LocString NAME = "Table Salt";


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


			public class REFINED_SUGAR
			{

				public static LocString NAME = "Refined Sugar";


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


			public class ICE_BELLY_POOP
			{

				public static LocString NAME = UI.FormatAsLink("Bammoth Patty", "ICE_BELLY_POOP");


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


		public class CARGO_CAPSULE
		{

			public static LocString NAME = "Care Package";


			public static LocString DESC = "A delivery system for recently printed resources.\n\nIt will dematerialize shortly.";
		}


		public class RAILGUNPAYLOAD
		{

			public static LocString NAME = UI.FormatAsLink("Interplanetary Payload", "RAILGUNPAYLOAD");


			public static LocString DESC = string.Concat(new string[]
			{
				"Contains resources packed for interstellar shipping.\n\nCan be launched by a ",
				BUILDINGS.PREFABS.RAILGUN.NAME,
				" or unpacked with a ",
				BUILDINGS.PREFABS.RAILGUNPAYLOADOPENER.NAME,
				"."
			});
		}


		public class MISSILE_BASIC
		{

			public static LocString NAME = UI.FormatAsLink("Blastshot", "MISSILELAUNCHER");


			public static LocString DESC = "An explosive projectile designed to defend against meteor showers.\n\nMust be launched by a " + UI.FormatAsLink("Meteor Blaster", "MISSILELAUNCHER") + ".";
		}


		public class MISSILE_LONGRANGE_VANILLADLC4
		{

			public static LocString NAME = UI.FormatAsLink("Intracosmic Blastshot", "MISSILELAUNCHER");


			public static LocString DESC = "A long-range explosive projectile that defends against distant space objects.\n\nMust be launched by " + UI.FormatAsLink("Meteor Blaster", "MISSILELAUNCHER") + ".";
		}


		public class MISSILE_LONGRANGE
		{

			public static LocString NAME = UI.FormatAsLink("Intracosmic Blastshot", "MISSILELAUNCHER");


			public static LocString DESC = "A long-range explosive projectile that defends against distant space objects.\n\nMust be launched by " + UI.FormatAsLink("Meteor Blaster", "MISSILELAUNCHER") + ".";
		}


		public class DEBRISPAYLOAD
		{

			public static LocString NAME = "Rocket Debris";


			public static LocString DESC = "Whatever is left over from a Rocket Self-Destruct can be recovered once it has crash-landed.";
		}


		public class RADIATION
		{

			public class HIGHENERGYPARITCLE
			{

				public static LocString NAME = "Radbolts";


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


		public class DREAMJOURNAL
		{

			public static LocString NAME = "Dream Journal";


			public static LocString DESC = string.Concat(new string[]
			{
				"A hand-scrawled account of ",
				UI.FormatAsLink("Pajama", "SLEEP_CLINIC_PAJAMAS"),
				"-induced dreams.\n\nCan be analyzed using a ",
				UI.FormatAsLink("Somnium Synthesizer", "MEGABRAINTANK"),
				"."
			});
		}


		public class DEHYDRATEDFOODPACKAGE
		{

			public static LocString NAME = "Dry Ration";


			public static LocString DESC = "A package of non-perishable dehydrated food.\n\nIt requires no refrigeration, but must be rehydrated before consumption.";


			public static LocString CONSUMED = "Ate Rehydrated Food";


			public static LocString CONTENTS = "Dried {0}";
		}


		public class SPICES
		{

			public class MACHINERY_SPICE
			{

				public static LocString NAME = UI.FormatAsLink("Machinist Spice", "MACHINERY_SPICE");


				public static LocString DESC = "Improves operating skills when ingested.";
			}


			public class PILOTING_SPICE
			{

				public static LocString NAME = UI.FormatAsLink("Rocketeer Spice", "PILOTING_SPICE");


				public static LocString DESC = "Provides a boost to piloting abilities.";
			}


			public class PRESERVING_SPICE
			{

				public static LocString NAME = UI.FormatAsLink("Freshener Spice", "PRESERVING_SPICE");


				public static LocString DESC = "Slows the decomposition of perishable foods.";
			}


			public class STRENGTH_SPICE
			{

				public static LocString NAME = UI.FormatAsLink("Brawny Spice", "STRENGTH_SPICE");


				public static LocString DESC = "Strengthens even the weakest of muscles.";
			}
		}
	}
}
