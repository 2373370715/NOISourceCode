using System;

namespace STRINGS
{
	// Token: 0x020030A6 RID: 12454
	public class GAMEPLAY_EVENTS
	{
		// Token: 0x0400C90A RID: 51466
		public static LocString CANCELED = "{0} Canceled";

		// Token: 0x0400C90B RID: 51467
		public static LocString CANCELED_TOOLTIP = "The {0} event was canceled";

		// Token: 0x0400C90C RID: 51468
		public static LocString DEFAULT_OPTION_NAME = "OK";

		// Token: 0x0400C90D RID: 51469
		public static LocString DEFAULT_OPTION_CONSIDER_NAME = "Let me think about it";

		// Token: 0x0400C90E RID: 51470
		public static LocString CHAIN_EVENT_TOOLTIP = "This event is a chain event";

		// Token: 0x0400C90F RID: 51471
		public static LocString BONUS_EVENT_DESCRIPTION = "{effects} for {duration}";

		// Token: 0x020030A7 RID: 12455
		public class LOCATIONS
		{
			// Token: 0x0400C910 RID: 51472
			public static LocString NONE_AVAILABLE = "No location currently available";

			// Token: 0x0400C911 RID: 51473
			public static LocString SUN = "The Sun";

			// Token: 0x0400C912 RID: 51474
			public static LocString SURFACE = "Planetary Surface";

			// Token: 0x0400C913 RID: 51475
			public static LocString PRINTING_POD = BUILDINGS.PREFABS.HEADQUARTERS.NAME;

			// Token: 0x0400C914 RID: 51476
			public static LocString COLONY_WIDE = "Colonywide";
		}

		// Token: 0x020030A8 RID: 12456
		public class TIMES
		{
			// Token: 0x0400C915 RID: 51477
			public static LocString NOW = "Right now";

			// Token: 0x0400C916 RID: 51478
			public static LocString IN_CYCLES = "In {0} cycles";

			// Token: 0x0400C917 RID: 51479
			public static LocString UNKNOWN = "Sometime";
		}

		// Token: 0x020030A9 RID: 12457
		public class EVENT_TYPES
		{
			// Token: 0x020030AA RID: 12458
			public class PARTY
			{
				// Token: 0x0400C918 RID: 51480
				public static LocString NAME = "Party";

				// Token: 0x0400C919 RID: 51481
				public static LocString DESCRIPTION = "THIS EVENT IS NOT WORKING\n{host} is throwing a birthday party for {dupe}. Make sure there is an available " + ROOMS.TYPES.REC_ROOM.NAME + " for the party.\n\nSocial events are good for Duplicant morale. Rejecting this party will hurt {host} and {dupe}'s fragile ego.";

				// Token: 0x0400C91A RID: 51482
				public static LocString CANCELED_NO_ROOM_TITLE = "Party Canceled";

				// Token: 0x0400C91B RID: 51483
				public static LocString CANCELED_NO_ROOM_DESCRIPTION = "The party was canceled because no " + ROOMS.TYPES.REC_ROOM.NAME + " was available.";

				// Token: 0x0400C91C RID: 51484
				public static LocString UNDERWAY = "Party Happening";

				// Token: 0x0400C91D RID: 51485
				public static LocString UNDERWAY_TOOLTIP = "There's a party going on";

				// Token: 0x0400C91E RID: 51486
				public static LocString ACCEPT_OPTION_NAME = "Allow the party to happen";

				// Token: 0x0400C91F RID: 51487
				public static LocString ACCEPT_OPTION_DESC = "Party goers will get {goodEffect}";

				// Token: 0x0400C920 RID: 51488
				public static LocString ACCEPT_OPTION_INVALID_TOOLTIP = "A cake must be built for this event to take place.";

				// Token: 0x0400C921 RID: 51489
				public static LocString REJECT_OPTION_NAME = "Cancel the party";

				// Token: 0x0400C922 RID: 51490
				public static LocString REJECT_OPTION_DESC = "{host} and {dupe} gain {badEffect}";
			}

			// Token: 0x020030AB RID: 12459
			public class ECLIPSE
			{
				// Token: 0x0400C923 RID: 51491
				public static LocString NAME = "Eclipse";

				// Token: 0x0400C924 RID: 51492
				public static LocString DESCRIPTION = "A celestial object has obscured the sunlight";
			}

			// Token: 0x020030AC RID: 12460
			public class SOLAR_FLARE
			{
				// Token: 0x0400C925 RID: 51493
				public static LocString NAME = "Solar Storm";

				// Token: 0x0400C926 RID: 51494
				public static LocString DESCRIPTION = "A solar flare is headed this way";
			}

			// Token: 0x020030AD RID: 12461
			public class CREATURE_SPAWN
			{
				// Token: 0x0400C927 RID: 51495
				public static LocString NAME = "Critter Infestation";

				// Token: 0x0400C928 RID: 51496
				public static LocString DESCRIPTION = "There was a massive influx of destructive critters";
			}

			// Token: 0x020030AE RID: 12462
			public class SATELLITE_CRASH
			{
				// Token: 0x0400C929 RID: 51497
				public static LocString NAME = "Satellite Crash";

				// Token: 0x0400C92A RID: 51498
				public static LocString DESCRIPTION = "Mysterious space junk has crashed into the surface.\n\nIt may contain useful resources or information, but it may also be dangerous. Approach with caution.";
			}

			// Token: 0x020030AF RID: 12463
			public class FOOD_FIGHT
			{
				// Token: 0x0400C92B RID: 51499
				public static LocString NAME = "Food Fight";

				// Token: 0x0400C92C RID: 51500
				public static LocString DESCRIPTION = "Duplicants will throw food at each other for recreation\n\nIt may be wasteful, but everyone who participates will benefit from a major stress reduction.";

				// Token: 0x0400C92D RID: 51501
				public static LocString UNDERWAY = "Food Fight";

				// Token: 0x0400C92E RID: 51502
				public static LocString UNDERWAY_TOOLTIP = "There is a food fight happening now";

				// Token: 0x0400C92F RID: 51503
				public static LocString ACCEPT_OPTION_NAME = "Duplicants start preparing to fight.";

				// Token: 0x0400C930 RID: 51504
				public static LocString ACCEPT_OPTION_DETAILS = "(Plus morale)";

				// Token: 0x0400C931 RID: 51505
				public static LocString REJECT_OPTION_NAME = "No food fight today";

				// Token: 0x0400C932 RID: 51506
				public static LocString REJECT_OPTION_DETAILS = "Sadface";
			}

			// Token: 0x020030B0 RID: 12464
			public class PLANT_BLIGHT
			{
				// Token: 0x0400C933 RID: 51507
				public static LocString NAME = "Plant Blight: {plant}";

				// Token: 0x0400C934 RID: 51508
				public static LocString DESCRIPTION = "Our {plant} crops have been afflicted by a fungal sickness!\n\nI must get the Duplicants to uproot and compost the sick plants to save our farms.";

				// Token: 0x0400C935 RID: 51509
				public static LocString SUCCESS = "Blight Managed: {plant}";

				// Token: 0x0400C936 RID: 51510
				public static LocString SUCCESS_TOOLTIP = "All the blighted {plant} plants have been dealt with, halting the infection.";
			}

			// Token: 0x020030B1 RID: 12465
			public class CRYOFRIEND
			{
				// Token: 0x0400C937 RID: 51511
				public static LocString NAME = "New Event: A Frozen Friend";

				// Token: 0x0400C938 RID: 51512
				public static LocString DESCRIPTION = string.Concat(new string[]
				{
					"{dupe} has made an amazing discovery! A barely working ",
					BUILDINGS.PREFABS.CRYOTANK.NAME,
					" has been uncovered containing a {friend} inside in a frozen state.\n\n{dupe} was successful in thawing {friend} and this encounter has filled both Duplicants with a sense of hope, something they will desperately need to keep their ",
					UI.FormatAsLink("Morale", "MORALE"),
					" up when facing the dangers ahead."
				});

				// Token: 0x0400C939 RID: 51513
				public static LocString BUTTON = "{friend} is thawed!";
			}

			// Token: 0x020030B2 RID: 12466
			public class WARPWORLDREVEAL
			{
				// Token: 0x0400C93A RID: 51514
				public static LocString NAME = "New Event: Personnel Teleporter";

				// Token: 0x0400C93B RID: 51515
				public static LocString DESCRIPTION = "I've discovered a functioning teleportation device with a pre-programmed destination.\n\nIt appears to go to another " + UI.CLUSTERMAP.PLANETOID + ", and I'm fairly certain there's a return device on the other end.\n\nI could send a Duplicant through safely if I desired.";

				// Token: 0x0400C93C RID: 51516
				public static LocString BUTTON = "See Destination";
			}

			// Token: 0x020030B3 RID: 12467
			public class ARTIFACT_REVEAL
			{
				// Token: 0x0400C93D RID: 51517
				public static LocString NAME = "New Event: Artifact Analyzed";

				// Token: 0x0400C93E RID: 51518
				public static LocString DESCRIPTION = "An artifact from a past civilization was analyzed.\n\n{desc}";

				// Token: 0x0400C93F RID: 51519
				public static LocString BUTTON = "Close";
			}
		}

		// Token: 0x020030B4 RID: 12468
		public class BONUS
		{
			// Token: 0x020030B5 RID: 12469
			public class BONUSDREAM1
			{
				// Token: 0x0400C940 RID: 51520
				public static LocString NAME = "Good Dream";

				// Token: 0x0400C941 RID: 51521
				public static LocString DESCRIPTION = "I've observed many improvements to {dupe}'s demeanor today. Analysis indicates unusually high amounts of dopamine in their system. There's a good chance this is due to an exceptionally good dream and analysis indicates that current sleeping conditions may have contributed to this occurrence.\n\nFurther improvements to sleeping conditions may have additional positive effects to the " + UI.FormatAsLink("Morale", "MORALE") + " of {dupe} and other Duplicants.";

				// Token: 0x0400C942 RID: 51522
				public static LocString CHAIN_TOOLTIP = "Improving the living conditions of {dupe} will lead to more good dreams.";
			}

			// Token: 0x020030B6 RID: 12470
			public class BONUSDREAM2
			{
				// Token: 0x0400C943 RID: 51523
				public static LocString NAME = "Really Good Dream";

				// Token: 0x0400C944 RID: 51524
				public static LocString DESCRIPTION = "{dupe} had another really good dream and the resulting release of dopamine has made this Duplicant energetic and full of possibilities! This is an encouraging byproduct of improving the living conditions of the colony.\n\nBased on these observations, building a better sleeping area for my Duplicants will have a similar effect on their " + UI.FormatAsLink("Morale", "MORALE") + ".";
			}

			// Token: 0x020030B7 RID: 12471
			public class BONUSDREAM3
			{
				// Token: 0x0400C945 RID: 51525
				public static LocString NAME = "Great Dream";

				// Token: 0x0400C946 RID: 51526
				public static LocString DESCRIPTION = "I have detected a distinct spring in {dupe}'s step today. There is a good chance that this Duplicant had another great dream last night. Such incidents are further indications that working on the care and comfort of the colony is not a waste of time.\n\nI do wonder though: What do Duplicants dream of?";
			}

			// Token: 0x020030B8 RID: 12472
			public class BONUSDREAM4
			{
				// Token: 0x0400C947 RID: 51527
				public static LocString NAME = "Amazing Dream";

				// Token: 0x0400C948 RID: 51528
				public static LocString DESCRIPTION = "{dupe}'s dream last night must have been simply amazing! Their dopamine levels are at an all time high. Based on these results, it can be safely assumed that improving the living conditions of my Duplicants will reduce " + UI.FormatAsLink("Stress", "STRESS") + " and have similar positive effects on their well-being.\n\nObservations such as this are an integral and enjoyable part of science. When I see my Duplicants happy, I can't help but share in some of their joy.";
			}

			// Token: 0x020030B9 RID: 12473
			public class BONUSTOILET1
			{
				// Token: 0x0400C949 RID: 51529
				public static LocString NAME = "Small Comforts";

				// Token: 0x0400C94A RID: 51530
				public static LocString DESCRIPTION = string.Concat(new string[]
				{
					"{dupe} recently visited an Outhouse and appears to have appreciated the small comforts based on the marked increase to their ",
					UI.FormatAsLink("Morale", "MORALE"),
					".\n\nHigh ",
					UI.FormatAsLink("Morale", "MORALE"),
					" has been linked to a better work ethic and greater enthusiasm for complex jobs, which are essential in building a successful new colony."
				});
			}

			// Token: 0x020030BA RID: 12474
			public class BONUSTOILET2
			{
				// Token: 0x0400C94B RID: 51531
				public static LocString NAME = "Greater Comforts";

				// Token: 0x0400C94C RID: 51532
				public static LocString DESCRIPTION = "{dupe} used a Lavatory and analysis shows a decided improvement to this Duplicant's " + UI.FormatAsLink("Morale", "MORALE") + ".\n\nAs my colony grows and expands, it's important not to ignore the benefits of giving my Duplicants a pleasant place to relieve themselves.";
			}

			// Token: 0x020030BB RID: 12475
			public class BONUSTOILET3
			{
				// Token: 0x0400C94D RID: 51533
				public static LocString NAME = "Small Luxury";

				// Token: 0x0400C94E RID: 51534
				public static LocString DESCRIPTION = string.Concat(new string[]
				{
					"{dupe} visited a ",
					ROOMS.TYPES.LATRINE.NAME,
					" and experienced luxury unlike they anything this Duplicant had previously experienced as analysis has revealed yet another boost to their ",
					UI.FormatAsLink("Morale", "MORALE"),
					".\n\nIt is unclear whether this development is a result of increased hygiene or whether there is something else inherently about working plumbing which would improve ",
					UI.FormatAsLink("Morale", "MORALE"),
					" in this way. Further analysis is needed."
				});
			}

			// Token: 0x020030BC RID: 12476
			public class BONUSTOILET4
			{
				// Token: 0x0400C94F RID: 51535
				public static LocString NAME = "Greater Luxury";

				// Token: 0x0400C950 RID: 51536
				public static LocString DESCRIPTION = "{dupe} visited a Washroom and the experience has left this Duplicant with significantly improved " + UI.FormatAsLink("Morale", "MORALE") + ". Analysis indicates this improvement should continue for many cycles.\n\nThe relationship of my Duplicants and their surroundings is an interesting aspect of colony life. I should continue to watch future developments in this department closely.";
			}

			// Token: 0x020030BD RID: 12477
			public class BONUSRESEARCH
			{
				// Token: 0x0400C951 RID: 51537
				public static LocString NAME = "Inspired Learner";

				// Token: 0x0400C952 RID: 51538
				public static LocString DESCRIPTION = string.Concat(new string[]
				{
					"Analysis indicates that the appearance of a ",
					UI.PRE_KEYWORD,
					"Research Station",
					UI.PST_KEYWORD,
					" has inspired {dupe} and heightened their brain activity on a cellular level.\n\nBrain stimulation is important if my Duplicants are going to adapt and innovate in their increasingly harsh environment."
				});
			}

			// Token: 0x020030BE RID: 12478
			public class BONUSDIGGING1
			{
				// Token: 0x0400C953 RID: 51539
				public static LocString NAME = "Hot Diggity!";

				// Token: 0x0400C954 RID: 51540
				public static LocString DESCRIPTION = "Some interesting data has revealed that {dupe} has had a marked increase in physical abilities, an increase that cannot entirely be attributed to the usual improvements that occur after regular physical activity.\n\nBased on previous observations this Duplicant's positive associations with digging appear to account for this additional physical boost.\n\nThis would mean the personal preferences of my Duplicants are directly correlated to how hard they work. How interesting...";
			}

			// Token: 0x020030BF RID: 12479
			public class BONUSSTORAGE
			{
				// Token: 0x0400C955 RID: 51541
				public static LocString NAME = "Something in Store";

				// Token: 0x0400C956 RID: 51542
				public static LocString DESCRIPTION = "Data indicates that {dupe}'s activity in storing something in a Storage Bin has led to an increase in this Duplicant's physical strength as well as an overall improvement to their general demeanor.\n\nThere have been many studies connecting organization with an increase in well-being. It is possible this explains {dupe}'s " + UI.FormatAsLink("Morale", "MORALE") + " improvements.";
			}

			// Token: 0x020030C0 RID: 12480
			public class BONUSBUILDER
			{
				// Token: 0x0400C957 RID: 51543
				public static LocString NAME = "Accomplished Builder";

				// Token: 0x0400C958 RID: 51544
				public static LocString DESCRIPTION = "{dupe} has been hard at work building many structures crucial to the future of the colony. It seems this activity has improved this Duplicant's budding construction and mechanical skills beyond what my models predicted.\n\nWhether this increase in ability is due to them learning new skills or simply gaining self-confidence I cannot say, but this unexpected development is a welcome surprise development.";
			}

			// Token: 0x020030C1 RID: 12481
			public class BONUSOXYGEN
			{
				// Token: 0x0400C959 RID: 51545
				public static LocString NAME = "Fresh Air";

				// Token: 0x0400C95A RID: 51546
				public static LocString DESCRIPTION = "{dupe} is experiencing a sudden unexpected improvement to their physical prowess which appears to be a result of exposure to elevated levels of oxygen from passing by an Oxygen Diffuser.\n\nObservations such as this are important in documenting just how beneficial having access to oxygen is to my colony.";
			}

			// Token: 0x020030C2 RID: 12482
			public class BONUSALGAE
			{
				// Token: 0x0400C95B RID: 51547
				public static LocString NAME = "Fresh Algae Smell";

				// Token: 0x0400C95C RID: 51548
				public static LocString DESCRIPTION = "{dupe}'s recent proximity to an Algae Terrarium has left them feeling refreshed and exuberant and is correlated to an increase in their physical attributes. It is unclear whether these physical improvements came from the excess of oxygen or the invigorating smell of algae.\n\nIt's curious that I find myself nostalgic for the smell of algae growing in a lab. But how could this be...?";
			}

			// Token: 0x020030C3 RID: 12483
			public class BONUSGENERATOR
			{
				// Token: 0x0400C95D RID: 51549
				public static LocString NAME = "Exercised";

				// Token: 0x0400C95E RID: 51550
				public static LocString DESCRIPTION = "{dupe} ran in a Manual Generator and the physical activity appears to have given this Duplicant increased strength and sense of well-being.\n\nWhile not the primary reason for building Manual Generators, I am very pleased to see my Duplicants reaping the " + UI.FormatAsLink("Stress", "STRESS") + " relieving benefits to physical activity.";
			}

			// Token: 0x020030C4 RID: 12484
			public class BONUSDOOR
			{
				// Token: 0x0400C95F RID: 51551
				public static LocString NAME = "Open and Shut";

				// Token: 0x0400C960 RID: 51552
				public static LocString DESCRIPTION = string.Concat(new string[]
				{
					"The act of closing a door has apparently lead to a decrease in the ",
					UI.FormatAsLink("Stress", "STRESS"),
					" levels of {dupe}, as well as decreased the exposure of this Duplicant to harmful ",
					UI.FormatAsLink("Germs", "GERMS"),
					".\n\nWhile it may be more efficient to group all my Duplicants together in common sleeping quarters, it's important to remember the mental benefits to privacy and space to express their individuality."
				});
			}

			// Token: 0x020030C5 RID: 12485
			public class BONUSHITTHEBOOKS
			{
				// Token: 0x0400C961 RID: 51553
				public static LocString NAME = "Hit the Books";

				// Token: 0x0400C962 RID: 51554
				public static LocString DESCRIPTION = "{dupe}'s recent Research errand has resulted in a significant increase to this Duplicant's brain activity. The discovery of newly found knowledge has given {dupe} an invigorating jolt of excitement.\n\nI am all too familiar with this feeling.";
			}

			// Token: 0x020030C6 RID: 12486
			public class BONUSLITWORKSPACE
			{
				// Token: 0x0400C963 RID: 51555
				public static LocString NAME = "Lit-erally Great";

				// Token: 0x0400C964 RID: 51556
				public static LocString DESCRIPTION = "{dupe}'s recent time in a well-lit area has greatly improved this Duplicant's ability to work with, and on, machinery.\n\nThis supports the prevailing theory that a well-lit workspace has many benefits beyond just improving my Duplicant's ability to see.";
			}

			// Token: 0x020030C7 RID: 12487
			public class BONUSTALKER
			{
				// Token: 0x0400C965 RID: 51557
				public static LocString NAME = "Big Small Talker";

				// Token: 0x0400C966 RID: 51558
				public static LocString DESCRIPTION = "{dupe}'s recent conversation with another Duplicant shows a correlation to improved serotonin and " + UI.FormatAsLink("Morale", "MORALE") + " levels in this Duplicant. It is very possible that small talk with a co-worker, however short and seemingly insignificant, will make my Duplicant's feel connected to the colony as a whole.\n\nAs the colony gets bigger and more sophisticated, I must ensure that the opportunity for such connections continue, for the good of my Duplicants' mental well being.";
			}
		}
	}
}
