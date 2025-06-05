using System;
using TUNING;

namespace STRINGS
{
	// Token: 0x020030C8 RID: 12488
	public class ROOMS
	{
		// Token: 0x020030C9 RID: 12489
		public class CATEGORY
		{
			// Token: 0x020030CA RID: 12490
			public class NONE
			{
				// Token: 0x0400C967 RID: 51559
				public static LocString NAME = "None";
			}

			// Token: 0x020030CB RID: 12491
			public class FOOD
			{
				// Token: 0x0400C968 RID: 51560
				public static LocString NAME = "Dining";
			}

			// Token: 0x020030CC RID: 12492
			public class SLEEP
			{
				// Token: 0x0400C969 RID: 51561
				public static LocString NAME = "Sleep";
			}

			// Token: 0x020030CD RID: 12493
			public class RECREATION
			{
				// Token: 0x0400C96A RID: 51562
				public static LocString NAME = "Recreation";
			}

			// Token: 0x020030CE RID: 12494
			public class BATHROOM
			{
				// Token: 0x0400C96B RID: 51563
				public static LocString NAME = "Washroom";
			}

			// Token: 0x020030CF RID: 12495
			public class BIONIC
			{
				// Token: 0x0400C96C RID: 51564
				public static LocString NAME = "";
			}

			// Token: 0x020030D0 RID: 12496
			public class HOSPITAL
			{
				// Token: 0x0400C96D RID: 51565
				public static LocString NAME = "Medical";
			}

			// Token: 0x020030D1 RID: 12497
			public class INDUSTRIAL
			{
				// Token: 0x0400C96E RID: 51566
				public static LocString NAME = "Industrial";
			}

			// Token: 0x020030D2 RID: 12498
			public class AGRICULTURAL
			{
				// Token: 0x0400C96F RID: 51567
				public static LocString NAME = "Agriculture";
			}

			// Token: 0x020030D3 RID: 12499
			public class PARK
			{
				// Token: 0x0400C970 RID: 51568
				public static LocString NAME = "Parks";
			}

			// Token: 0x020030D4 RID: 12500
			public class SCIENCE
			{
				// Token: 0x0400C971 RID: 51569
				public static LocString NAME = "Science";
			}
		}

		// Token: 0x020030D5 RID: 12501
		public class TYPES
		{
			// Token: 0x0400C972 RID: 51570
			public static LocString CONFLICTED = "Conflicted Room";

			// Token: 0x020030D6 RID: 12502
			public class NEUTRAL
			{
				// Token: 0x0400C973 RID: 51571
				public static LocString NAME = "Miscellaneous Room";

				// Token: 0x0400C974 RID: 51572
				public static LocString DESCRIPTION = "An enclosed space with plenty of potential and no dedicated use.";

				// Token: 0x0400C975 RID: 51573
				public static LocString EFFECT = "- No effect";

				// Token: 0x0400C976 RID: 51574
				public static LocString TOOLTIP = "This area has walls and doors but no dedicated use";
			}

			// Token: 0x020030D7 RID: 12503
			public class LATRINE
			{
				// Token: 0x0400C977 RID: 51575
				public static LocString NAME = "Latrine";

				// Token: 0x0400C978 RID: 51576
				public static LocString DESCRIPTION = "It's a step up from doing one's business in full view of the rest of the colony.\n\nUsing a toilet in an enclosed room will improve Duplicants' Morale.";

				// Token: 0x0400C979 RID: 51577
				public static LocString EFFECT = "- Morale bonus";

				// Token: 0x0400C97A RID: 51578
				public static LocString TOOLTIP = "Using a toilet in an enclosed room will improve Duplicants' Morale";
			}

			// Token: 0x020030D8 RID: 12504
			public class BIONICUPKEEP
			{
				// Token: 0x0400C97B RID: 51579
				public static LocString NAME = "";

				// Token: 0x0400C97C RID: 51580
				public static LocString DESCRIPTION = "";

				// Token: 0x0400C97D RID: 51581
				public static LocString EFFECT = "";

				// Token: 0x0400C97E RID: 51582
				public static LocString TOOLTIP = "";
			}

			// Token: 0x020030D9 RID: 12505
			public class PLUMBEDBATHROOM
			{
				// Token: 0x0400C97F RID: 51583
				public static LocString NAME = "Washroom";

				// Token: 0x0400C980 RID: 51584
				public static LocString DESCRIPTION = "A sanctuary of personal hygiene.\n\nUsing a fully plumbed Washroom will improve Duplicants' Morale.";

				// Token: 0x0400C981 RID: 51585
				public static LocString EFFECT = "- Morale bonus";

				// Token: 0x0400C982 RID: 51586
				public static LocString TOOLTIP = "Using a fully plumbed Washroom will improve Duplicants' Morale";
			}

			// Token: 0x020030DA RID: 12506
			public class BARRACKS
			{
				// Token: 0x0400C983 RID: 51587
				public static LocString NAME = "Barracks";

				// Token: 0x0400C984 RID: 51588
				public static LocString DESCRIPTION = "A basic communal sleeping area for up-and-coming colonies.\n\nSleeping in Barracks will improve Duplicants' Morale.";

				// Token: 0x0400C985 RID: 51589
				public static LocString EFFECT = "- Morale bonus";

				// Token: 0x0400C986 RID: 51590
				public static LocString TOOLTIP = "Sleeping in Barracks will improve Duplicants' Morale";
			}

			// Token: 0x020030DB RID: 12507
			public class BEDROOM
			{
				// Token: 0x0400C987 RID: 51591
				public static LocString NAME = "Luxury Barracks";

				// Token: 0x0400C988 RID: 51592
				public static LocString DESCRIPTION = "An upscale communal sleeping area full of things that greatly enhance quality of rest for occupants.\n\nSleeping in a Luxury Barracks will improve Duplicants' Morale.";

				// Token: 0x0400C989 RID: 51593
				public static LocString EFFECT = "- Morale bonus";

				// Token: 0x0400C98A RID: 51594
				public static LocString TOOLTIP = "Sleeping in a Luxury Barracks will improve Duplicants' Morale";
			}

			// Token: 0x020030DC RID: 12508
			public class PRIVATE_BEDROOM
			{
				// Token: 0x0400C98B RID: 51595
				public static LocString NAME = "Private Bedroom";

				// Token: 0x0400C98C RID: 51596
				public static LocString DESCRIPTION = "A comfortable, roommate-free retreat where tired Duplicants can get uninterrupted rest.\n\nSleeping in a Private Bedroom will greatly improve Duplicants' Morale.";

				// Token: 0x0400C98D RID: 51597
				public static LocString EFFECT = "- Morale bonus";

				// Token: 0x0400C98E RID: 51598
				public static LocString TOOLTIP = "Sleeping in a Private Bedroom will greatly improve Duplicants' Morale";
			}

			// Token: 0x020030DD RID: 12509
			public class MESSHALL
			{
				// Token: 0x0400C98F RID: 51599
				public static LocString NAME = "Mess Hall";

				// Token: 0x0400C990 RID: 51600
				public static LocString DESCRIPTION = "A simple dining room setup that's easy to improve upon.\n\nEating at a mess table in a Mess Hall will increase Duplicants' Morale.";

				// Token: 0x0400C991 RID: 51601
				public static LocString EFFECT = "- Morale bonus";

				// Token: 0x0400C992 RID: 51602
				public static LocString TOOLTIP = "Eating at a Mess Table in a Mess Hall will improve Duplicants' Morale";
			}

			// Token: 0x020030DE RID: 12510
			public class KITCHEN
			{
				// Token: 0x0400C993 RID: 51603
				public static LocString NAME = "Kitchen";

				// Token: 0x0400C994 RID: 51604
				public static LocString DESCRIPTION = "A cooking area equipped to take meals to the next level.\n\nAdding ingredients from a Spice Grinder to foods cooked on an Electric Grill or Gas Range provides a variety of positive benefits.";

				// Token: 0x0400C995 RID: 51605
				public static LocString EFFECT = "- Enables Spice Grinder use";

				// Token: 0x0400C996 RID: 51606
				public static LocString TOOLTIP = "Using a Spice Grinder in a Kitchen adds benefits to foods cooked on Electric Grill or Gas Range";
			}

			// Token: 0x020030DF RID: 12511
			public class GREATHALL
			{
				// Token: 0x0400C997 RID: 51607
				public static LocString NAME = "Great Hall";

				// Token: 0x0400C998 RID: 51608
				public static LocString DESCRIPTION = "A great place to eat, with great decor and great company. Great!\n\nEating in a Great Hall will significantly improve Duplicants' Morale.";

				// Token: 0x0400C999 RID: 51609
				public static LocString EFFECT = "- Morale bonus";

				// Token: 0x0400C99A RID: 51610
				public static LocString TOOLTIP = "Eating in a Great Hall will significantly improve Duplicants' Morale";
			}

			// Token: 0x020030E0 RID: 12512
			public class HOSPITAL
			{
				// Token: 0x0400C99B RID: 51611
				public static LocString NAME = "Hospital";

				// Token: 0x0400C99C RID: 51612
				public static LocString DESCRIPTION = "A dedicated medical facility that helps minimize recovery time.\n\nSick Duplicants assigned to medical buildings located within a Hospital are also less likely to spread Disease.";

				// Token: 0x0400C99D RID: 51613
				public static LocString EFFECT = "- Quarantine sick Duplicants";

				// Token: 0x0400C99E RID: 51614
				public static LocString TOOLTIP = "Sick Duplicants assigned to medical buildings located within a Hospital are less likely to spread Disease";
			}

			// Token: 0x020030E1 RID: 12513
			public class MASSAGE_CLINIC
			{
				// Token: 0x0400C99F RID: 51615
				public static LocString NAME = "Massage Clinic";

				// Token: 0x0400C9A0 RID: 51616
				public static LocString DESCRIPTION = "A soothing space with a very relaxing ambience, especially when well-decorated.\n\nReceiving massages at a Massage Clinic will significantly improve Stress reduction.";

				// Token: 0x0400C9A1 RID: 51617
				public static LocString EFFECT = "- Massage stress relief bonus";

				// Token: 0x0400C9A2 RID: 51618
				public static LocString TOOLTIP = "Receiving massages at a Massage Clinic will significantly improve Stress reduction";
			}

			// Token: 0x020030E2 RID: 12514
			public class POWER_PLANT
			{
				// Token: 0x0400C9A3 RID: 51619
				public static LocString NAME = "Power Plant";

				// Token: 0x0400C9A4 RID: 51620
				public static LocString DESCRIPTION = "The perfect place for Duplicants to flex their Electrical Engineering skills.\n\nHeavy-duty generators built within a Power Plant can be tuned up using microchips from power control stations to improve their " + UI.FormatAsLink("Power", "POWER") + " production.";

				// Token: 0x0400C9A5 RID: 51621
				public static LocString EFFECT = "- Enables " + ITEMS.INDUSTRIAL_PRODUCTS.POWER_STATION_TOOLS.NAME + " tune-ups on heavy-duty generators";

				// Token: 0x0400C9A6 RID: 51622
				public static LocString TOOLTIP = "Heavy-duty generators built in a Power Plant can be tuned up using microchips from Power Control Stations to improve their Power production";
			}

			// Token: 0x020030E3 RID: 12515
			public class MACHINE_SHOP
			{
				// Token: 0x0400C9A7 RID: 51623
				public static LocString NAME = "Machine Shop";

				// Token: 0x0400C9A8 RID: 51624
				public static LocString DESCRIPTION = "It smells like elbow grease.\n\nDuplicants working in a Machine Shop can maintain buildings and increase their production speed.";

				// Token: 0x0400C9A9 RID: 51625
				public static LocString EFFECT = "- Increased fabrication efficiency";

				// Token: 0x0400C9AA RID: 51626
				public static LocString TOOLTIP = "Duplicants working in a Machine Shop can maintain buildings and increase their production speed";
			}

			// Token: 0x020030E4 RID: 12516
			public class FARM
			{
				// Token: 0x0400C9AB RID: 51627
				public static LocString NAME = "Greenhouse";

				// Token: 0x0400C9AC RID: 51628
				public static LocString DESCRIPTION = "An enclosed agricultural space best utilized by Duplicants with Crop Tending skills.\n\nCrops grown within a Greenhouse can be tended with Farm Station fertilizer to increase their growth speed.";

				// Token: 0x0400C9AD RID: 51629
				public static LocString EFFECT = "- Enables Farm Station use";

				// Token: 0x0400C9AE RID: 51630
				public static LocString TOOLTIP = "Crops grown within a Greenhouse can be tended with Farm Station fertilizer to increase their growth speed";
			}

			// Token: 0x020030E5 RID: 12517
			public class CREATUREPEN
			{
				// Token: 0x0400C9AF RID: 51631
				public static LocString NAME = "Stable";

				// Token: 0x0400C9B0 RID: 51632
				public static LocString DESCRIPTION = "Critters don't mind it here, as long as things don't get too crowded.\n\nStabled critters can be tended to in order to improve their happiness, hasten their domestication and increase their production.\n\nEnables the use of Grooming Stations, Shearing Stations, Critter Condos, Critter Fountains and Milking Stations.";

				// Token: 0x0400C9B1 RID: 51633
				public static LocString EFFECT = "- Critter taming and mood bonus";

				// Token: 0x0400C9B2 RID: 51634
				public static LocString TOOLTIP = "A stable enables Grooming Station, Critter Condo, Critter Fountain, Shearing Station and Milking Station use";
			}

			// Token: 0x020030E6 RID: 12518
			public class REC_ROOM
			{
				// Token: 0x0400C9B3 RID: 51635
				public static LocString NAME = "Recreation Room";

				// Token: 0x0400C9B4 RID: 51636
				public static LocString DESCRIPTION = "Where Duplicants go to mingle with off-duty peers and indulge in a little R&R.\n\nScheduled Downtime will further improve Morale for Duplicants visiting a Recreation Room.";

				// Token: 0x0400C9B5 RID: 51637
				public static LocString EFFECT = "- Morale bonus";

				// Token: 0x0400C9B6 RID: 51638
				public static LocString TOOLTIP = "Scheduled Downtime will further improve Morale for Duplicants visiting a Recreation Room";
			}

			// Token: 0x020030E7 RID: 12519
			public class PARK
			{
				// Token: 0x0400C9B7 RID: 51639
				public static LocString NAME = "Park";

				// Token: 0x0400C9B8 RID: 51640
				public static LocString DESCRIPTION = "A little greenery goes a long way.\n\nPassing through natural spaces throughout the day will raise the Morale of Duplicants.";

				// Token: 0x0400C9B9 RID: 51641
				public static LocString EFFECT = "- Morale bonus";

				// Token: 0x0400C9BA RID: 51642
				public static LocString TOOLTIP = "Passing through natural spaces throughout the day will raise the Morale of Duplicants";
			}

			// Token: 0x020030E8 RID: 12520
			public class NATURERESERVE
			{
				// Token: 0x0400C9BB RID: 51643
				public static LocString NAME = "Nature Reserve";

				// Token: 0x0400C9BC RID: 51644
				public static LocString DESCRIPTION = "A lot of greenery goes an even longer way.\n\nPassing through a Nature Reserve will grant higher Morale bonuses to Duplicants than a Park.";

				// Token: 0x0400C9BD RID: 51645
				public static LocString EFFECT = "- Morale bonus";

				// Token: 0x0400C9BE RID: 51646
				public static LocString TOOLTIP = "A Nature Reserve will grant higher Morale bonuses to Duplicants than a Park";
			}

			// Token: 0x020030E9 RID: 12521
			public class LABORATORY
			{
				// Token: 0x0400C9BF RID: 51647
				public static LocString NAME = "Laboratory";

				// Token: 0x0400C9C0 RID: 51648
				public static LocString DESCRIPTION = "Where wild hypotheses meet rigorous scientific experimentation.\n\nScience stations built in a Laboratory function more efficiently.\n\nA Laboratory enables the use of the Geotuner and the Mission Control Station.";

				// Token: 0x0400C9C1 RID: 51649
				public static LocString EFFECT = "- Efficiency bonus";

				// Token: 0x0400C9C2 RID: 51650
				public static LocString TOOLTIP = "Science buildings built in a Laboratory function more efficiently\n\nA Laboratory enables Geotuner and Mission Control Station use";
			}

			// Token: 0x020030EA RID: 12522
			public class PRIVATE_BATHROOM
			{
				// Token: 0x0400C9C3 RID: 51651
				public static LocString NAME = "Private Bathroom";

				// Token: 0x0400C9C4 RID: 51652
				public static LocString DESCRIPTION = "Finally, a place to truly be alone with one's thoughts.\n\nDuplicants relieve even more Stress when using the toilet in a Private Bathroom than in a Latrine.";

				// Token: 0x0400C9C5 RID: 51653
				public static LocString EFFECT = "- Stress relief bonus";

				// Token: 0x0400C9C6 RID: 51654
				public static LocString TOOLTIP = "Duplicants relieve even more stress when using the toilet in a Private Bathroom than in a Latrine";
			}

			// Token: 0x020030EB RID: 12523
			public class BIONIC_UPKEEP
			{
				// Token: 0x0400C9C7 RID: 51655
				public static LocString NAME = "";

				// Token: 0x0400C9C8 RID: 51656
				public static LocString DESCRIPTION = "";

				// Token: 0x0400C9C9 RID: 51657
				public static LocString EFFECT = "";

				// Token: 0x0400C9CA RID: 51658
				public static LocString TOOLTIP = "";
			}
		}

		// Token: 0x020030EC RID: 12524
		public class CRITERIA
		{
			// Token: 0x0400C9CB RID: 51659
			public static LocString HEADER = "<b>Requirements:</b>";

			// Token: 0x0400C9CC RID: 51660
			public static LocString NEUTRAL_TYPE = "Enclosed by wall tile";

			// Token: 0x0400C9CD RID: 51661
			public static LocString POSSIBLE_TYPES_HEADER = "Possible Room Types";

			// Token: 0x0400C9CE RID: 51662
			public static LocString NO_TYPE_CONFLICTS = "Remove conflicting buildings";

			// Token: 0x0400C9CF RID: 51663
			public static LocString IN_CODE_ERROR = "String Key Not Found: {0}";

			// Token: 0x020030ED RID: 12525
			public class CRITERIA_FAILED
			{
				// Token: 0x0400C9D0 RID: 51664
				public static LocString MISSING_BUILDING = "Missing {0}";

				// Token: 0x0400C9D1 RID: 51665
				public static LocString FAILED = "{0}";
			}

			// Token: 0x020030EE RID: 12526
			public static class DECORATION
			{
				// Token: 0x0400C9D2 RID: 51666
				public static LocString NAME = UI.FormatAsLink("Decor item", "REQUIREMENTCLASSDECORATION");

				// Token: 0x0400C9D3 RID: 51667
				public static LocString CONFLICT_DESCRIPTION = ROOMS.CRITERIA.DECORATION.NAME;
			}

			// Token: 0x020030EF RID: 12527
			public class CEILING_HEIGHT
			{
				// Token: 0x0400C9D4 RID: 51668
				public static LocString NAME = "Minimum height: {0} tiles";

				// Token: 0x0400C9D5 RID: 51669
				public static LocString DESCRIPTION = "Must have a ceiling height of at least {0} tiles";

				// Token: 0x0400C9D6 RID: 51670
				public static LocString CONFLICT_DESCRIPTION = ROOMS.CRITERIA.CEILING_HEIGHT.NAME;
			}

			// Token: 0x020030F0 RID: 12528
			public class MINIMUM_SIZE
			{
				// Token: 0x0400C9D7 RID: 51671
				public static LocString NAME = "Minimum size: {0} tiles";

				// Token: 0x0400C9D8 RID: 51672
				public static LocString DESCRIPTION = "Must have an area of at least {0} tiles";

				// Token: 0x0400C9D9 RID: 51673
				public static LocString CONFLICT_DESCRIPTION = ROOMS.CRITERIA.MINIMUM_SIZE.NAME;
			}

			// Token: 0x020030F1 RID: 12529
			public class MAXIMUM_SIZE
			{
				// Token: 0x0400C9DA RID: 51674
				public static LocString NAME = "Maximum size: {0} tiles";

				// Token: 0x0400C9DB RID: 51675
				public static LocString DESCRIPTION = "Must have an area no larger than {0} tiles";

				// Token: 0x0400C9DC RID: 51676
				public static LocString CONFLICT_DESCRIPTION = ROOMS.CRITERIA.MAXIMUM_SIZE.NAME;
			}

			// Token: 0x020030F2 RID: 12530
			public class INDUSTRIALMACHINERY
			{
				// Token: 0x0400C9DD RID: 51677
				public static LocString NAME = UI.FormatAsLink("Industrial machinery", "REQUIREMENTCLASSINDUSTRIALMACHINERY");

				// Token: 0x0400C9DE RID: 51678
				public static LocString CONFLICT_DESCRIPTION = ROOMS.CRITERIA.INDUSTRIALMACHINERY.NAME;
			}

			// Token: 0x020030F3 RID: 12531
			public class HAS_BED
			{
				// Token: 0x0400C9DF RID: 51679
				public static LocString NAME = "One or more " + UI.FormatAsLink("beds", "REQUIREMENTCLASSBEDTYPE");

				// Token: 0x0400C9E0 RID: 51680
				public static LocString DESCRIPTION = "Requires at least one Cot or Comfy Bed";

				// Token: 0x0400C9E1 RID: 51681
				public static LocString CONFLICT_DESCRIPTION = ROOMS.CRITERIA.HAS_BED.NAME;
			}

			// Token: 0x020030F4 RID: 12532
			public class HAS_LUXURY_BED
			{
				// Token: 0x0400C9E2 RID: 51682
				public static LocString NAME = "One or more " + UI.FormatAsLink("Comfy Beds", "LUXURYBED");

				// Token: 0x0400C9E3 RID: 51683
				public static LocString DESCRIPTION = "Requires at least one Comfy Bed";

				// Token: 0x0400C9E4 RID: 51684
				public static LocString CONFLICT_DESCRIPTION = ROOMS.CRITERIA.HAS_LUXURY_BED.NAME;
			}

			// Token: 0x020030F5 RID: 12533
			public class LUXURYBEDTYPE
			{
				// Token: 0x0400C9E5 RID: 51685
				public static LocString NAME = "Single " + UI.FormatAsLink("Comfy Bed", "LUXURYBED");

				// Token: 0x0400C9E6 RID: 51686
				public static LocString DESCRIPTION = "Must have no more than one Comfy Bed";

				// Token: 0x0400C9E7 RID: 51687
				public static LocString CONFLICT_DESCRIPTION = ROOMS.CRITERIA.LUXURYBEDTYPE.NAME;
			}

			// Token: 0x020030F6 RID: 12534
			public class BED_SINGLE
			{
				// Token: 0x0400C9E8 RID: 51688
				public static LocString NAME = "Single " + UI.FormatAsLink("beds", "REQUIREMENTCLASSBEDTYPE");

				// Token: 0x0400C9E9 RID: 51689
				public static LocString DESCRIPTION = "Must have no more than one Cot or Comfy Bed";

				// Token: 0x0400C9EA RID: 51690
				public static LocString CONFLICT_DESCRIPTION = ROOMS.CRITERIA.BED_SINGLE.NAME;
			}

			// Token: 0x020030F7 RID: 12535
			public class IS_BACKWALLED
			{
				// Token: 0x0400C9EB RID: 51691
				public static LocString NAME = "Has backwall tiles";

				// Token: 0x0400C9EC RID: 51692
				public static LocString DESCRIPTION = "Must be covered in backwall tiles";

				// Token: 0x0400C9ED RID: 51693
				public static LocString CONFLICT_DESCRIPTION = ROOMS.CRITERIA.IS_BACKWALLED.NAME;
			}

			// Token: 0x020030F8 RID: 12536
			public class NO_COTS
			{
				// Token: 0x0400C9EE RID: 51694
				public static LocString NAME = "No " + UI.FormatAsLink("Cots", "BED");

				// Token: 0x0400C9EF RID: 51695
				public static LocString DESCRIPTION = "Room cannot contain a Cot";

				// Token: 0x0400C9F0 RID: 51696
				public static LocString CONFLICT_DESCRIPTION = ROOMS.CRITERIA.NO_COTS.NAME;
			}

			// Token: 0x020030F9 RID: 12537
			public class NO_LUXURY_BEDS
			{
				// Token: 0x0400C9F1 RID: 51697
				public static LocString NAME = "No " + UI.FormatAsLink("Comfy Beds", "LUXURYBED");

				// Token: 0x0400C9F2 RID: 51698
				public static LocString DESCRIPTION = "Room cannot contain a Comfy Bed";

				// Token: 0x0400C9F3 RID: 51699
				public static LocString CONFLICT_DESCRIPTION = ROOMS.CRITERIA.NO_LUXURY_BEDS.NAME;
			}

			// Token: 0x020030FA RID: 12538
			public class BEDTYPE
			{
				// Token: 0x0400C9F4 RID: 51700
				public static LocString NAME = UI.FormatAsLink("Beds", "REQUIREMENTCLASSBEDTYPE");

				// Token: 0x0400C9F5 RID: 51701
				public static LocString DESCRIPTION = "Requires two or more Cots or Comfy Beds";

				// Token: 0x0400C9F6 RID: 51702
				public static LocString CONFLICT_DESCRIPTION = ROOMS.CRITERIA.BEDTYPE.NAME;
			}

			// Token: 0x020030FB RID: 12539
			public class BUILDING_DECOR_POSITIVE
			{
				// Token: 0x0400C9F7 RID: 51703
				public static LocString NAME = "Positive " + UI.FormatAsLink("decor", "REQUIREMENTCLASSDECORATION");

				// Token: 0x0400C9F8 RID: 51704
				public static LocString DESCRIPTION = "Requires at least one building with positive decor";

				// Token: 0x0400C9F9 RID: 51705
				public static LocString CONFLICT_DESCRIPTION = ROOMS.CRITERIA.BUILDING_DECOR_POSITIVE.NAME;
			}

			// Token: 0x020030FC RID: 12540
			public class DECORATIVE_ITEM
			{
				// Token: 0x0400C9FA RID: 51706
				public static LocString NAME = UI.FormatAsLink("Decor item", "REQUIREMENTCLASSDECORATION") + " ({0})";

				// Token: 0x0400C9FB RID: 51707
				public static LocString DESCRIPTION = "Requires {0} or more Decor items";

				// Token: 0x0400C9FC RID: 51708
				public static LocString CONFLICT_DESCRIPTION = ROOMS.CRITERIA.DECORATIVE_ITEM.NAME;
			}

			// Token: 0x020030FD RID: 12541
			public class DECOR20
			{
				// Token: 0x0600D684 RID: 54916 RVA: 0x004B6044 File Offset: 0x004B4244
				// Note: this type is marked as 'beforefieldinit'.
				static DECOR20()
				{
					string str = "Requires a decorative item with a minimum Decor value of ";
					int amount = BUILDINGS.DECOR.BONUS.TIER3.amount;
					ROOMS.CRITERIA.DECOR20.DESCRIPTION = str + amount.ToString();
					ROOMS.CRITERIA.DECOR20.CONFLICT_DESCRIPTION = ROOMS.CRITERIA.DECOR20.NAME;
				}

				// Token: 0x0400C9FD RID: 51709
				public static LocString NAME = UI.FormatAsLink("Fancy decor item", "REQUIREMENTCLASSDECORATION");

				// Token: 0x0400C9FE RID: 51710
				public static LocString DESCRIPTION;

				// Token: 0x0400C9FF RID: 51711
				public static LocString CONFLICT_DESCRIPTION;
			}

			// Token: 0x020030FE RID: 12542
			public class CLINIC
			{
				// Token: 0x0400CA00 RID: 51712
				public static LocString NAME = UI.FormatAsLink("Medical equipment", "REQUIREMENTCLASSCLINIC");

				// Token: 0x0400CA01 RID: 51713
				public static LocString DESCRIPTION = "Requires one or more Sick Bays or Disease Clinics";

				// Token: 0x0400CA02 RID: 51714
				public static LocString CONFLICT_DESCRIPTION = ROOMS.CRITERIA.CLINIC.NAME;
			}

			// Token: 0x020030FF RID: 12543
			public class POWERPLANT
			{
				// Token: 0x0400CA03 RID: 51715
				public static LocString NAME = UI.FormatAsLink("Heavy-Duty Generator", "REQUIREMENTCLASSGENERATORTYPE") + "\n    • Two or more " + UI.FormatAsLink("Power Buildings", "REQUIREMENTCLASSPOWERBUILDING");

				// Token: 0x0400CA04 RID: 51716
				public static LocString DESCRIPTION = "Requires a Heavy-Duty Generator and two or more Power Buildings";

				// Token: 0x0400CA05 RID: 51717
				public static LocString CONFLICT_DESCRIPTION = "Heavy-Duty Generator and two or more Power buildings";
			}

			// Token: 0x02003100 RID: 12544
			public class FARMSTATIONTYPE
			{
				// Token: 0x0400CA06 RID: 51718
				public static LocString NAME = UI.FormatAsLink("Farm Station", "FARMSTATION");

				// Token: 0x0400CA07 RID: 51719
				public static LocString DESCRIPTION = "Requires a single Farm Station";

				// Token: 0x0400CA08 RID: 51720
				public static LocString CONFLICT_DESCRIPTION = ROOMS.CRITERIA.FARMSTATIONTYPE.NAME;
			}

			// Token: 0x02003101 RID: 12545
			public class FARMBUILDING
			{
				// Token: 0x0400CA09 RID: 51721
				public static LocString NAME = UI.FormatAsLink("Farm Building", "FARMBUILDING");

				// Token: 0x0400CA0A RID: 51722
				public static LocString DESCRIPTION = "";

				// Token: 0x0400CA0B RID: 51723
				public static LocString CONFLICT_DESCRIPTION = ROOMS.CRITERIA.FARMBUILDING.NAME;
			}

			// Token: 0x02003102 RID: 12546
			public class CREATURE_FEEDER
			{
				// Token: 0x0400CA0C RID: 51724
				public static LocString NAME = UI.FormatAsLink("Critter Feeder", "CREATUREFEEDER");

				// Token: 0x0400CA0D RID: 51725
				public static LocString DESCRIPTION = "Requires a single Critter Feeder";

				// Token: 0x0400CA0E RID: 51726
				public static LocString CONFLICT_DESCRIPTION = ROOMS.CRITERIA.CREATURE_FEEDER.NAME;
			}

			// Token: 0x02003103 RID: 12547
			public class RANCHSTATIONTYPE
			{
				// Token: 0x0400CA0F RID: 51727
				public static LocString NAME = UI.FormatAsLink("Ranching building", "REQUIREMENTCLASSRANCHSTATIONTYPE");

				// Token: 0x0400CA10 RID: 51728
				public static LocString DESCRIPTION = "Requires a single Grooming Station, Critter Condo, Critter Fountain, Shearing Station or Milking Station";

				// Token: 0x0400CA11 RID: 51729
				public static LocString CONFLICT_DESCRIPTION = ROOMS.CRITERIA.RANCHSTATIONTYPE.NAME;
			}

			// Token: 0x02003104 RID: 12548
			public class SPICESTATION
			{
				// Token: 0x0400CA12 RID: 51730
				public static LocString NAME = UI.FormatAsLink("Spice Grinder", "SPICEGRINDER");

				// Token: 0x0400CA13 RID: 51731
				public static LocString DESCRIPTION = "Requires a single Spice Grinder";

				// Token: 0x0400CA14 RID: 51732
				public static LocString CONFLICT_DESCRIPTION = ROOMS.CRITERIA.SPICESTATION.NAME;
			}

			// Token: 0x02003105 RID: 12549
			public class COOKTOP
			{
				// Token: 0x0400CA15 RID: 51733
				public static LocString NAME = UI.FormatAsLink("Cooking station", "REQUIREMENTCLASSCOOKTOP");

				// Token: 0x0400CA16 RID: 51734
				public static LocString DESCRIPTION = "Requires a single Electric Grill or Gas Range";

				// Token: 0x0400CA17 RID: 51735
				public static LocString CONFLICT_DESCRIPTION = ROOMS.CRITERIA.COOKTOP.NAME;
			}

			// Token: 0x02003106 RID: 12550
			public class REFRIGERATOR
			{
				// Token: 0x0400CA18 RID: 51736
				public static LocString NAME = UI.FormatAsLink("Refrigerator", "REFRIGERATOR");

				// Token: 0x0400CA19 RID: 51737
				public static LocString DESCRIPTION = "Requires a single Refrigerator";

				// Token: 0x0400CA1A RID: 51738
				public static LocString CONFLICT_DESCRIPTION = ROOMS.CRITERIA.REFRIGERATOR.NAME;
			}

			// Token: 0x02003107 RID: 12551
			public class RECBUILDING
			{
				// Token: 0x0400CA1B RID: 51739
				public static LocString NAME = UI.FormatAsLink("Recreational building", "REQUIREMENTCLASSRECBUILDING");

				// Token: 0x0400CA1C RID: 51740
				public static LocString DESCRIPTION = "Requires one or more recreational buildings";

				// Token: 0x0400CA1D RID: 51741
				public static LocString CONFLICT_DESCRIPTION = ROOMS.CRITERIA.RECBUILDING.NAME;
			}

			// Token: 0x02003108 RID: 12552
			public class PARK
			{
				// Token: 0x0400CA1E RID: 51742
				public static LocString NAME = UI.FormatAsLink("Park Sign", "PARKSIGN");

				// Token: 0x0400CA1F RID: 51743
				public static LocString DESCRIPTION = "Requires one or more Park Signs";

				// Token: 0x0400CA20 RID: 51744
				public static LocString CONFLICT_DESCRIPTION = ROOMS.CRITERIA.PARK.NAME;
			}

			// Token: 0x02003109 RID: 12553
			public class MACHINESHOPTYPE
			{
				// Token: 0x0400CA21 RID: 51745
				public static LocString NAME = "Mechanics Station";

				// Token: 0x0400CA22 RID: 51746
				public static LocString DESCRIPTION = "Requires requires one or more Mechanics Stations";

				// Token: 0x0400CA23 RID: 51747
				public static LocString CONFLICT_DESCRIPTION = ROOMS.CRITERIA.MACHINESHOPTYPE.NAME;
			}

			// Token: 0x0200310A RID: 12554
			public class FOOD_BOX
			{
				// Token: 0x0400CA24 RID: 51748
				public static LocString NAME = "Food storage";

				// Token: 0x0400CA25 RID: 51749
				public static LocString DESCRIPTION = "Requires one or more Ration Boxes or Refrigerators";

				// Token: 0x0400CA26 RID: 51750
				public static LocString CONFLICT_DESCRIPTION = ROOMS.CRITERIA.FOOD_BOX.NAME;
			}

			// Token: 0x0200310B RID: 12555
			public class LIGHTSOURCE
			{
				// Token: 0x0400CA27 RID: 51751
				public static LocString NAME = UI.FormatAsLink("Light source", "REQUIREMENTCLASSLIGHTSOURCE");

				// Token: 0x0400CA28 RID: 51752
				public static LocString DESCRIPTION = "Requires one or more light sources";

				// Token: 0x0400CA29 RID: 51753
				public static LocString CONFLICT_DESCRIPTION = ROOMS.CRITERIA.LIGHTSOURCE.NAME;
			}

			// Token: 0x0200310C RID: 12556
			public class DESTRESSINGBUILDING
			{
				// Token: 0x0400CA2A RID: 51754
				public static LocString NAME = UI.FormatAsLink("De-Stressing Building", "MASSAGETABLE");

				// Token: 0x0400CA2B RID: 51755
				public static LocString DESCRIPTION = "Requires one or more De-Stressing buildings";

				// Token: 0x0400CA2C RID: 51756
				public static LocString CONFLICT_DESCRIPTION = ROOMS.CRITERIA.DESTRESSINGBUILDING.NAME;
			}

			// Token: 0x0200310D RID: 12557
			public class MASSAGE_TABLE
			{
				// Token: 0x0400CA2D RID: 51757
				public static LocString NAME = UI.FormatAsLink("Massage Table", "MASSAGETABLE");

				// Token: 0x0400CA2E RID: 51758
				public static LocString DESCRIPTION = "Requires one or more Massage Tables";

				// Token: 0x0400CA2F RID: 51759
				public static LocString CONFLICT_DESCRIPTION = ROOMS.CRITERIA.MASSAGE_TABLE.NAME;
			}

			// Token: 0x0200310E RID: 12558
			public class MESSTABLE
			{
				// Token: 0x0400CA30 RID: 51760
				public static LocString NAME = UI.FormatAsLink("Mess Table", "DININGTABLE");

				// Token: 0x0400CA31 RID: 51761
				public static LocString DESCRIPTION = "Requires a single Mess Table";

				// Token: 0x0400CA32 RID: 51762
				public static LocString CONFLICT_DESCRIPTION = ROOMS.CRITERIA.MESSTABLE.NAME;
			}

			// Token: 0x0200310F RID: 12559
			public class NO_MESS_STATION
			{
				// Token: 0x0400CA33 RID: 51763
				public static LocString NAME = "No " + UI.FormatAsLink("Mess Table", "DININGTABLE");

				// Token: 0x0400CA34 RID: 51764
				public static LocString DESCRIPTION = "Cannot contain a Mess Table";

				// Token: 0x0400CA35 RID: 51765
				public static LocString CONFLICT_DESCRIPTION = ROOMS.CRITERIA.NO_MESS_STATION.NAME;
			}

			// Token: 0x02003110 RID: 12560
			public class MESS_STATION_MULTIPLE
			{
				// Token: 0x0400CA36 RID: 51766
				public static LocString NAME = UI.FormatAsLink("Mess Tables", "DININGTABLE");

				// Token: 0x0400CA37 RID: 51767
				public static LocString DESCRIPTION = "Requires two or more Mess Tables";

				// Token: 0x0400CA38 RID: 51768
				public static LocString CONFLICT_DESCRIPTION = ROOMS.CRITERIA.MESS_STATION_MULTIPLE.NAME;
			}

			// Token: 0x02003111 RID: 12561
			public class RESEARCH_STATION
			{
				// Token: 0x0400CA39 RID: 51769
				public static LocString NAME = UI.FormatAsLink("Research station", "REQUIREMENTCLASSRESEARCH_STATION");

				// Token: 0x0400CA3A RID: 51770
				public static LocString DESCRIPTION = "Requires one or more Research Stations or Super Computers";

				// Token: 0x0400CA3B RID: 51771
				public static LocString CONFLICT_DESCRIPTION = ROOMS.CRITERIA.RESEARCH_STATION.NAME;
			}

			// Token: 0x02003112 RID: 12562
			public class BIONICUPKEEP
			{
				// Token: 0x0400CA3C RID: 51772
				public static LocString NAME = UI.FormatAsLink("Bionic service station", "REQUIREMENTCLASSBIONICUPKEEP");

				// Token: 0x0400CA3D RID: 51773
				public static LocString DESCRIPTION = "Requires at least one Lubrication Station and one Gunk Extractor";

				// Token: 0x0400CA3E RID: 51774
				public static LocString CONFLICT_DESCRIPTION = ROOMS.CRITERIA.BIONICUPKEEP.NAME;
			}

			// Token: 0x02003113 RID: 12563
			public class BIONIC_GUNKEMPTIER
			{
				// Token: 0x0400CA3F RID: 51775
				public static LocString NAME = UI.FormatAsLink("Gunk Extractor", "REQUIREMENTCLASSBIONIC_GUNKEMPTIER");

				// Token: 0x0400CA40 RID: 51776
				public static LocString DESCRIPTION = "Requires one or more Gunk Extractors";

				// Token: 0x0400CA41 RID: 51777
				public static LocString CONFLICT_DESCRIPTION = ROOMS.CRITERIA.BIONIC_GUNKEMPTIER.NAME;
			}

			// Token: 0x02003114 RID: 12564
			public class BIONIC_LUBRICATION
			{
				// Token: 0x0400CA42 RID: 51778
				public static LocString NAME = UI.FormatAsLink("Lubrication Station", "REQUIREMENTCLASSBIONIC_LUBRICATION");

				// Token: 0x0400CA43 RID: 51779
				public static LocString DESCRIPTION = "Requires one or more Lubrication Stations";

				// Token: 0x0400CA44 RID: 51780
				public static LocString CONFLICT_DESCRIPTION = ROOMS.CRITERIA.BIONIC_LUBRICATION.NAME;
			}

			// Token: 0x02003115 RID: 12565
			public class TOILETTYPE
			{
				// Token: 0x0400CA45 RID: 51781
				public static LocString NAME = UI.FormatAsLink("Toilet", "REQUIREMENTCLASSTOILETTYPE");

				// Token: 0x0400CA46 RID: 51782
				public static LocString DESCRIPTION = "Requires one or more Outhouses or Lavatories";

				// Token: 0x0400CA47 RID: 51783
				public static LocString CONFLICT_DESCRIPTION = ROOMS.CRITERIA.TOILETTYPE.NAME;
			}

			// Token: 0x02003116 RID: 12566
			public class FLUSHTOILETTYPE
			{
				// Token: 0x0400CA48 RID: 51784
				public static LocString NAME = UI.FormatAsLink("Flush Toilet", "REQUIREMENTCLASSFLUSHTOILETTYPE");

				// Token: 0x0400CA49 RID: 51785
				public static LocString DESCRIPTION = "Requires one or more Lavatories";

				// Token: 0x0400CA4A RID: 51786
				public static LocString CONFLICT_DESCRIPTION = ROOMS.CRITERIA.FLUSHTOILETTYPE.NAME;
			}

			// Token: 0x02003117 RID: 12567
			public class NO_OUTHOUSES
			{
				// Token: 0x0400CA4B RID: 51787
				public static LocString NAME = "No " + UI.FormatAsLink("Outhouses", "OUTHOUSE");

				// Token: 0x0400CA4C RID: 51788
				public static LocString DESCRIPTION = "Cannot contain basic Outhouses";

				// Token: 0x0400CA4D RID: 51789
				public static LocString CONFLICT_DESCRIPTION = ROOMS.CRITERIA.NO_OUTHOUSES.NAME;
			}

			// Token: 0x02003118 RID: 12568
			public class WASHSTATION
			{
				// Token: 0x0400CA4E RID: 51790
				public static LocString NAME = UI.FormatAsLink("Wash station", "REQUIREMENTCLASSWASHSTATION");

				// Token: 0x0400CA4F RID: 51791
				public static LocString DESCRIPTION = "Requires one or more Wash Basins, Sinks, Hand Sanitizers, or Showers";

				// Token: 0x0400CA50 RID: 51792
				public static LocString CONFLICT_DESCRIPTION = ROOMS.CRITERIA.WASHSTATION.NAME;
			}

			// Token: 0x02003119 RID: 12569
			public class ADVANCEDWASHSTATION
			{
				// Token: 0x0400CA51 RID: 51793
				public static LocString NAME = UI.FormatAsLink("Plumbed wash station", "REQUIREMENTCLASSWASHSTATION");

				// Token: 0x0400CA52 RID: 51794
				public static LocString DESCRIPTION = "Requires one or more Sinks, Hand Sanitizers, or Showers";

				// Token: 0x0400CA53 RID: 51795
				public static LocString CONFLICT_DESCRIPTION = ROOMS.CRITERIA.ADVANCEDWASHSTATION.NAME;
			}

			// Token: 0x0200311A RID: 12570
			public class NO_INDUSTRIAL_MACHINERY
			{
				// Token: 0x0400CA54 RID: 51796
				public static LocString NAME = "No " + UI.FormatAsLink("industrial machinery", "REQUIREMENTCLASSINDUSTRIALMACHINERY");

				// Token: 0x0400CA55 RID: 51797
				public static LocString DESCRIPTION = "Cannot contain any building labeled Industrial Machinery";

				// Token: 0x0400CA56 RID: 51798
				public static LocString CONFLICT_DESCRIPTION = ROOMS.CRITERIA.NO_INDUSTRIAL_MACHINERY.NAME;
			}

			// Token: 0x0200311B RID: 12571
			public class WILDANIMAL
			{
				// Token: 0x0400CA57 RID: 51799
				public static LocString NAME = "Wildlife";

				// Token: 0x0400CA58 RID: 51800
				public static LocString DESCRIPTION = "Requires at least one wild critter";

				// Token: 0x0400CA59 RID: 51801
				public static LocString CONFLICT_DESCRIPTION = ROOMS.CRITERIA.WILDANIMAL.NAME;
			}

			// Token: 0x0200311C RID: 12572
			public class WILDANIMALS
			{
				// Token: 0x0400CA5A RID: 51802
				public static LocString NAME = "More wildlife";

				// Token: 0x0400CA5B RID: 51803
				public static LocString DESCRIPTION = "Requires two or more wild critters";

				// Token: 0x0400CA5C RID: 51804
				public static LocString CONFLICT_DESCRIPTION = ROOMS.CRITERIA.WILDANIMALS.NAME;
			}

			// Token: 0x0200311D RID: 12573
			public class WILDPLANT
			{
				// Token: 0x0400CA5D RID: 51805
				public static LocString NAME = "Two wild plants";

				// Token: 0x0400CA5E RID: 51806
				public static LocString DESCRIPTION = "Requires two or more wild plants";

				// Token: 0x0400CA5F RID: 51807
				public static LocString CONFLICT_DESCRIPTION = ROOMS.CRITERIA.WILDPLANT.NAME;
			}

			// Token: 0x0200311E RID: 12574
			public class WILDPLANTS
			{
				// Token: 0x0400CA60 RID: 51808
				public static LocString NAME = "Four wild plants";

				// Token: 0x0400CA61 RID: 51809
				public static LocString DESCRIPTION = "Requires four or more wild plants";

				// Token: 0x0400CA62 RID: 51810
				public static LocString CONFLICT_DESCRIPTION = ROOMS.CRITERIA.WILDPLANTS.NAME;
			}

			// Token: 0x0200311F RID: 12575
			public class SCIENCEBUILDING
			{
				// Token: 0x0400CA63 RID: 51811
				public static LocString NAME = UI.FormatAsLink("Science building", "REQUIREMENTCLASSSCIENCEBUILDING");

				// Token: 0x0400CA64 RID: 51812
				public static LocString DESCRIPTION = "Requires one or more science buildings";

				// Token: 0x0400CA65 RID: 51813
				public static LocString CONFLICT_DESCRIPTION = ROOMS.CRITERIA.SCIENCEBUILDING.NAME;
			}

			// Token: 0x02003120 RID: 12576
			public class SCIENCE_BUILDINGS
			{
				// Token: 0x0400CA66 RID: 51814
				public static LocString NAME = "Two " + UI.FormatAsLink("science buildings", "REQUIREMENTCLASSSCIENCEBUILDING");

				// Token: 0x0400CA67 RID: 51815
				public static LocString DESCRIPTION = "Requires two or more science buildings";

				// Token: 0x0400CA68 RID: 51816
				public static LocString CONFLICT_DESCRIPTION = ROOMS.CRITERIA.SCIENCE_BUILDINGS.NAME;
			}

			// Token: 0x02003121 RID: 12577
			public class ROCKETINTERIOR
			{
				// Token: 0x0400CA69 RID: 51817
				public static LocString NAME = UI.FormatAsLink("Rocket interior", "REQUIREMENTCLASSROCKETINTERIOR");

				// Token: 0x0400CA6A RID: 51818
				public static LocString DESCRIPTION = "Must be built inside a rocket";

				// Token: 0x0400CA6B RID: 51819
				public static LocString CONFLICT_DESCRIPTION = ROOMS.CRITERIA.ROCKETINTERIOR.NAME;
			}

			// Token: 0x02003122 RID: 12578
			public class WARMINGSTATION
			{
				// Token: 0x0400CA6C RID: 51820
				public static LocString NAME = UI.FormatAsLink("Warming station", "REQUIREMENTCLASSWARMINGSTATION");

				// Token: 0x0400CA6D RID: 51821
				public static LocString DESCRIPTION = "Raises the ambient temperature";

				// Token: 0x0400CA6E RID: 51822
				public static LocString CONFLICT_DESCRIPTION = ROOMS.CRITERIA.WARMINGSTATION.NAME;
			}

			// Token: 0x02003123 RID: 12579
			public class GENERATORTYPE
			{
				// Token: 0x0400CA6F RID: 51823
				public static LocString NAME = UI.FormatAsLink("Generator", "REQUIREMENTCLASSGENERATORTYPE");

				// Token: 0x0400CA70 RID: 51824
				public static LocString DESCRIPTION = "Generates electrical power";

				// Token: 0x0400CA71 RID: 51825
				public static LocString CONFLICT_DESCRIPTION = ROOMS.CRITERIA.GENERATORTYPE.NAME;
			}

			// Token: 0x02003124 RID: 12580
			public class HEAVYDUTYGENERATORTYPE
			{
				// Token: 0x0400CA72 RID: 51826
				public static LocString NAME = UI.FormatAsLink("Heavy-duty generator", "REQUIREMENTCLASSGENERATORTYPE");

				// Token: 0x0400CA73 RID: 51827
				public static LocString DESCRIPTION = "For big power needs";

				// Token: 0x0400CA74 RID: 51828
				public static LocString CONFLICT_DESCRIPTION = ROOMS.CRITERIA.HEAVYDUTYGENERATORTYPE.NAME;
			}

			// Token: 0x02003125 RID: 12581
			public class LIGHTDUTYGENERATORTYPE
			{
				// Token: 0x0400CA75 RID: 51829
				public static LocString NAME = UI.FormatAsLink("Basic generator", "REQUIREMENTCLASSGENERATORTYPE");

				// Token: 0x0400CA76 RID: 51830
				public static LocString DESCRIPTION = "For basic power needs";

				// Token: 0x0400CA77 RID: 51831
				public static LocString CONFLICT_DESCRIPTION = ROOMS.CRITERIA.LIGHTDUTYGENERATORTYPE.NAME;
			}

			// Token: 0x02003126 RID: 12582
			public class POWERBUILDING
			{
				// Token: 0x0400CA78 RID: 51832
				public static LocString NAME = UI.FormatAsLink("Power building", "REQUIREMENTCLASSPOWERBUILDING");

				// Token: 0x0400CA79 RID: 51833
				public static LocString DESCRIPTION = "Buildings that generate, store, or manage power";

				// Token: 0x0400CA7A RID: 51834
				public static LocString CONFLICT_DESCRIPTION = ROOMS.CRITERIA.POWERBUILDING.NAME;
			}
		}

		// Token: 0x02003127 RID: 12583
		public class DETAILS
		{
			// Token: 0x0400CA7B RID: 51835
			public static LocString HEADER = "Room Details";

			// Token: 0x02003128 RID: 12584
			public class ASSIGNED_TO
			{
				// Token: 0x0400CA7C RID: 51836
				public static LocString NAME = "<b>Assignments:</b>\n{0}";

				// Token: 0x0400CA7D RID: 51837
				public static LocString UNASSIGNED = "Unassigned";
			}

			// Token: 0x02003129 RID: 12585
			public class AVERAGE_TEMPERATURE
			{
				// Token: 0x0400CA7E RID: 51838
				public static LocString NAME = "Average temperature: {0}";
			}

			// Token: 0x0200312A RID: 12586
			public class AVERAGE_ATMO_MASS
			{
				// Token: 0x0400CA7F RID: 51839
				public static LocString NAME = "Average air pressure: {0}";
			}

			// Token: 0x0200312B RID: 12587
			public class SIZE
			{
				// Token: 0x0400CA80 RID: 51840
				public static LocString NAME = "Room size: {0} Tiles";
			}

			// Token: 0x0200312C RID: 12588
			public class BUILDING_COUNT
			{
				// Token: 0x0400CA81 RID: 51841
				public static LocString NAME = "Buildings: {0}";
			}

			// Token: 0x0200312D RID: 12589
			public class CREATURE_COUNT
			{
				// Token: 0x0400CA82 RID: 51842
				public static LocString NAME = "Critters: {0}";
			}

			// Token: 0x0200312E RID: 12590
			public class PLANT_COUNT
			{
				// Token: 0x0400CA83 RID: 51843
				public static LocString NAME = "Plants: {0}";
			}
		}

		// Token: 0x0200312F RID: 12591
		public class EFFECTS
		{
			// Token: 0x0400CA84 RID: 51844
			public static LocString HEADER = "<b>Effects:</b>";
		}
	}
}
