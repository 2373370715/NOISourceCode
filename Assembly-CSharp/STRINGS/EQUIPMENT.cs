using System;

namespace STRINGS
{
	// Token: 0x02002936 RID: 10550
	public class EQUIPMENT
	{
		// Token: 0x02002937 RID: 10551
		public class PREFABS
		{
			// Token: 0x02002938 RID: 10552
			public class OXYGEN_MASK
			{
				// Token: 0x0400AEE8 RID: 44776
				public static LocString NAME = UI.FormatAsLink("Oxygen Mask", "OXYGEN_MASK");

				// Token: 0x0400AEE9 RID: 44777
				public static LocString DESC = "Ensures my Duplicants can breathe easy... for a little while, anyways.";

				// Token: 0x0400AEEA RID: 44778
				public static LocString EFFECT = "Supplies Duplicants with <style=\"oxygen\">Oxygen</style> in toxic and low breathability environments.\n\nMust be refilled with oxygen at an " + UI.FormatAsLink("Oxygen Mask Dock", "OXYGENMASKLOCKER") + " when depleted.";

				// Token: 0x0400AEEB RID: 44779
				public static LocString RECIPE_DESC = "Supplies Duplicants with <style=\"oxygen\">Oxygen</style> in toxic and low breathability environments.";

				// Token: 0x0400AEEC RID: 44780
				public static LocString GENERICNAME = "Suit";

				// Token: 0x0400AEED RID: 44781
				public static LocString WORN_NAME = UI.FormatAsLink("Worn Oxygen Mask", "OXYGEN_MASK");

				// Token: 0x0400AEEE RID: 44782
				public static LocString WORN_DESC = string.Concat(new string[]
				{
					"A worn out ",
					UI.FormatAsLink("Oxygen Mask", "OXYGEN_MASK"),
					".\n\nMasks can be repaired at a ",
					UI.FormatAsLink("Crafting Station", "CRAFTINGTABLE"),
					"."
				});
			}

			// Token: 0x02002939 RID: 10553
			public class ATMO_SUIT
			{
				// Token: 0x0400AEEF RID: 44783
				public static LocString NAME = UI.FormatAsLink("Atmo Suit", "ATMO_SUIT");

				// Token: 0x0400AEF0 RID: 44784
				public static LocString DESC = "Ensures my Duplicants can breathe easy, anytime, anywhere.";

				// Token: 0x0400AEF1 RID: 44785
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Supplies Duplicants with ",
					UI.FormatAsLink("Oxygen", "OXYGEN"),
					" in toxic and low breathability environments, and protects against extreme temperatures.\n\nMust be refilled with oxygen at an ",
					UI.FormatAsLink("Atmo Suit Dock", "SUITLOCKER"),
					" when depleted."
				});

				// Token: 0x0400AEF2 RID: 44786
				public static LocString RECIPE_DESC = "Supplies Duplicants with " + UI.FormatAsLink("Oxygen", "OXYGEN") + " in toxic and low breathability environments.";

				// Token: 0x0400AEF3 RID: 44787
				public static LocString GENERICNAME = "Suit";

				// Token: 0x0400AEF4 RID: 44788
				public static LocString WORN_NAME = UI.FormatAsLink("Worn Atmo Suit", "ATMO_SUIT");

				// Token: 0x0400AEF5 RID: 44789
				public static LocString WORN_DESC = string.Concat(new string[]
				{
					"A worn out ",
					UI.FormatAsLink("Atmo Suit", "ATMO_SUIT"),
					".\n\nSuits can be repaired at an ",
					UI.FormatAsLink("Exosuit Forge", "SUITFABRICATOR"),
					"."
				});

				// Token: 0x0400AEF6 RID: 44790
				public static LocString REPAIR_WORN_RECIPE_NAME = "Repair " + EQUIPMENT.PREFABS.ATMO_SUIT.NAME;

				// Token: 0x0400AEF7 RID: 44791
				public static LocString REPAIR_WORN_DESC = "Restore a " + UI.FormatAsLink("Worn Atmo Suit", "ATMO_SUIT") + " to working order.";
			}

			// Token: 0x0200293A RID: 10554
			public class ATMO_SUIT_SET
			{
				// Token: 0x0200293B RID: 10555
				public class PUFT
				{
					// Token: 0x0400AEF8 RID: 44792
					public static LocString NAME = "Puft Atmo Suit";

					// Token: 0x0400AEF9 RID: 44793
					public static LocString DESC = "Critter-forward protective gear for the intrepid explorer!\nReleased for Klei Fest 2023.";
				}
			}

			// Token: 0x0200293C RID: 10556
			public class HOLIDAY_2023_CRATE
			{
				// Token: 0x0400AEFA RID: 44794
				public static LocString NAME = "Holiday Gift Crate";

				// Token: 0x0400AEFB RID: 44795
				public static LocString DESC = "An unaddressed package has been discovered near the Printing Pod. It exudes seasonal cheer, and trace amounts of Neutronium have been detected.";
			}

			// Token: 0x0200293D RID: 10557
			public class ATMO_SUIT_HELMET
			{
				// Token: 0x0400AEFC RID: 44796
				public static LocString NAME = "Default Atmo Helmet";

				// Token: 0x0400AEFD RID: 44797
				public static LocString DESC = "Default helmet for atmo suits.";

				// Token: 0x0200293E RID: 10558
				public class FACADES
				{
					// Token: 0x0200293F RID: 10559
					public class SPARKLE_RED
					{
						// Token: 0x0400AEFE RID: 44798
						public static LocString NAME = "Red Glitter Atmo Helmet";

						// Token: 0x0400AEFF RID: 44799
						public static LocString DESC = "Protective gear at its sparkliest.";
					}

					// Token: 0x02002940 RID: 10560
					public class SPARKLE_GREEN
					{
						// Token: 0x0400AF00 RID: 44800
						public static LocString NAME = "Green Glitter Atmo Helmet";

						// Token: 0x0400AF01 RID: 44801
						public static LocString DESC = "Protective gear at its sparkliest.";
					}

					// Token: 0x02002941 RID: 10561
					public class SPARKLE_BLUE
					{
						// Token: 0x0400AF02 RID: 44802
						public static LocString NAME = "Blue Glitter Atmo Helmet";

						// Token: 0x0400AF03 RID: 44803
						public static LocString DESC = "Protective gear at its sparkliest.";
					}

					// Token: 0x02002942 RID: 10562
					public class SPARKLE_PURPLE
					{
						// Token: 0x0400AF04 RID: 44804
						public static LocString NAME = "Violet Glitter Atmo Helmet";

						// Token: 0x0400AF05 RID: 44805
						public static LocString DESC = "Protective gear at its sparkliest.";
					}

					// Token: 0x02002943 RID: 10563
					public class LIMONE
					{
						// Token: 0x0400AF06 RID: 44806
						public static LocString NAME = "Citrus Atmo Helmet";

						// Token: 0x0400AF07 RID: 44807
						public static LocString DESC = "Fresh, fruity and full of breathable air.";
					}

					// Token: 0x02002944 RID: 10564
					public class PUFT
					{
						// Token: 0x0400AF08 RID: 44808
						public static LocString NAME = "Puft Atmo Helmet";

						// Token: 0x0400AF09 RID: 44809
						public static LocString DESC = "Convincing enough to fool most Pufts and even a few Duplicants.\nReleased for Klei Fest 2023.";
					}

					// Token: 0x02002945 RID: 10565
					public class CLUBSHIRT_PURPLE
					{
						// Token: 0x0400AF0A RID: 44810
						public static LocString NAME = "Eggplant Atmo Helmet";

						// Token: 0x0400AF0B RID: 44811
						public static LocString DESC = "It is neither an egg, nor a plant. But it <i>is</i> a functional helmet.";
					}

					// Token: 0x02002946 RID: 10566
					public class TRIANGLES_TURQ
					{
						// Token: 0x0400AF0C RID: 44812
						public static LocString NAME = "Confetti Atmo Helmet";

						// Token: 0x0400AF0D RID: 44813
						public static LocString DESC = "Doubles as a party hat.";
					}

					// Token: 0x02002947 RID: 10567
					public class CUMMERBUND_RED
					{
						// Token: 0x0400AF0E RID: 44814
						public static LocString NAME = "Blastoff Atmo Helmet";

						// Token: 0x0400AF0F RID: 44815
						public static LocString DESC = "Red means go!";
					}

					// Token: 0x02002948 RID: 10568
					public class WORKOUT_LAVENDER
					{
						// Token: 0x0400AF10 RID: 44816
						public static LocString NAME = "Pink Punch Atmo Helmet";

						// Token: 0x0400AF11 RID: 44817
						public static LocString DESC = "Unapologetically ostentatious.";
					}

					// Token: 0x02002949 RID: 10569
					public class CANTALOUPE
					{
						// Token: 0x0400AF12 RID: 44818
						public static LocString NAME = "Rocketmelon Atmo Helmet";

						// Token: 0x0400AF13 RID: 44819
						public static LocString DESC = "A melon for your melon.";
					}

					// Token: 0x0200294A RID: 10570
					public class MONDRIAN_BLUE_RED_YELLOW
					{
						// Token: 0x0400AF14 RID: 44820
						public static LocString NAME = "Cubist Atmo Helmet";

						// Token: 0x0400AF15 RID: 44821
						public static LocString DESC = "Abstract geometrics are both hip <i>and</i> square.";
					}

					// Token: 0x0200294B RID: 10571
					public class OVERALLS_RED
					{
						// Token: 0x0400AF16 RID: 44822
						public static LocString NAME = "Spiffy Atmo Helmet";

						// Token: 0x0400AF17 RID: 44823
						public static LocString DESC = "The twin antennae serve as an early warning system for low ceilings.";
					}
				}
			}

			// Token: 0x0200294C RID: 10572
			public class ATMO_SUIT_BODY
			{
				// Token: 0x0400AF18 RID: 44824
				public static LocString NAME = "Default Atmo Uniform";

				// Token: 0x0400AF19 RID: 44825
				public static LocString DESC = "Default top and bottom of an atmo suit.";

				// Token: 0x0200294D RID: 10573
				public class FACADES
				{
					// Token: 0x0200294E RID: 10574
					public class SPARKLE_RED
					{
						// Token: 0x0400AF1A RID: 44826
						public static LocString NAME = "Red Glitter Atmo Suit";

						// Token: 0x0400AF1B RID: 44827
						public static LocString DESC = "Protects the wearer from hostile environments <i>and</i> drab fashion.";
					}

					// Token: 0x0200294F RID: 10575
					public class SPARKLE_GREEN
					{
						// Token: 0x0400AF1C RID: 44828
						public static LocString NAME = "Green Glitter Atmo Suit";

						// Token: 0x0400AF1D RID: 44829
						public static LocString DESC = "Protects the wearer from hostile environments <i>and</i> drab fashion.";
					}

					// Token: 0x02002950 RID: 10576
					public class SPARKLE_BLUE
					{
						// Token: 0x0400AF1E RID: 44830
						public static LocString NAME = "Blue Glitter Atmo Suit";

						// Token: 0x0400AF1F RID: 44831
						public static LocString DESC = "Protects the wearer from hostile environments <i>and</i> drab fashion.";
					}

					// Token: 0x02002951 RID: 10577
					public class SPARKLE_LAVENDER
					{
						// Token: 0x0400AF20 RID: 44832
						public static LocString NAME = "Violet Glitter Atmo Suit";

						// Token: 0x0400AF21 RID: 44833
						public static LocString DESC = "Protects the wearer from hostile environments <i>and</i> drab fashion.";
					}

					// Token: 0x02002952 RID: 10578
					public class LIMONE
					{
						// Token: 0x0400AF22 RID: 44834
						public static LocString NAME = "Citrus Atmo Suit";

						// Token: 0x0400AF23 RID: 44835
						public static LocString DESC = "Perfect for summery, atmospheric excursions.";
					}

					// Token: 0x02002953 RID: 10579
					public class PUFT
					{
						// Token: 0x0400AF24 RID: 44836
						public static LocString NAME = "Puft Atmo Suit";

						// Token: 0x0400AF25 RID: 44837
						public static LocString DESC = "Warning: prolonged wear may result in feelings of Puft-up pride.\nReleased for Klei Fest 2023.";
					}

					// Token: 0x02002954 RID: 10580
					public class BASIC_PURPLE
					{
						// Token: 0x0400AF26 RID: 44838
						public static LocString NAME = "Crisp Eggplant Atmo Suit";

						// Token: 0x0400AF27 RID: 44839
						public static LocString DESC = "It really emphasizes wide shoulders.";
					}

					// Token: 0x02002955 RID: 10581
					public class PRINT_TRIANGLES_TURQ
					{
						// Token: 0x0400AF28 RID: 44840
						public static LocString NAME = "Confetti Atmo Suit";

						// Token: 0x0400AF29 RID: 44841
						public static LocString DESC = "It puts the \"fun\" in \"perfunctory nods to personnel individuality\"!";
					}

					// Token: 0x02002956 RID: 10582
					public class BASIC_NEON_PINK
					{
						// Token: 0x0400AF2A RID: 44842
						public static LocString NAME = "Crisp Neon Pink Atmo Suit";

						// Token: 0x0400AF2B RID: 44843
						public static LocString DESC = "The neck is a little snug.";
					}

					// Token: 0x02002957 RID: 10583
					public class MULTI_RED_BLACK
					{
						// Token: 0x0400AF2C RID: 44844
						public static LocString NAME = "Red-bellied Atmo Suit";

						// Token: 0x0400AF2D RID: 44845
						public static LocString DESC = "It really highlights the midsection.";
					}

					// Token: 0x02002958 RID: 10584
					public class CANTALOUPE
					{
						// Token: 0x0400AF2E RID: 44846
						public static LocString NAME = "Rocketmelon Atmo Suit";

						// Token: 0x0400AF2F RID: 44847
						public static LocString DESC = "It starts to smell ripe pretty quickly.";
					}

					// Token: 0x02002959 RID: 10585
					public class MULTI_BLUE_GREY_BLACK
					{
						// Token: 0x0400AF30 RID: 44848
						public static LocString NAME = "Swagger Atmo Suit";

						// Token: 0x0400AF31 RID: 44849
						public static LocString DESC = "Engineered to resemble stonewashed denim and black leather.";
					}

					// Token: 0x0200295A RID: 10586
					public class MULTI_BLUE_YELLOW_RED
					{
						// Token: 0x0400AF32 RID: 44850
						public static LocString NAME = "Fundamental Stripe Atmo Suit";

						// Token: 0x0400AF33 RID: 44851
						public static LocString DESC = "Designed by the Primary Colors Appreciation Society.";
					}
				}
			}

			// Token: 0x0200295B RID: 10587
			public class ATMO_SUIT_GLOVES
			{
				// Token: 0x0400AF34 RID: 44852
				public static LocString NAME = "Default Atmo Gloves";

				// Token: 0x0400AF35 RID: 44853
				public static LocString DESC = "Default atmo suit gloves.";

				// Token: 0x0200295C RID: 10588
				public class FACADES
				{
					// Token: 0x0200295D RID: 10589
					public class SPARKLE_RED
					{
						// Token: 0x0400AF36 RID: 44854
						public static LocString NAME = "Red Glitter Atmo Gloves";

						// Token: 0x0400AF37 RID: 44855
						public static LocString DESC = "Sparkly red gloves for hostile environments.";
					}

					// Token: 0x0200295E RID: 10590
					public class SPARKLE_GREEN
					{
						// Token: 0x0400AF38 RID: 44856
						public static LocString NAME = "Green Glitter Atmo Gloves";

						// Token: 0x0400AF39 RID: 44857
						public static LocString DESC = "Sparkly green gloves for hostile environments.";
					}

					// Token: 0x0200295F RID: 10591
					public class SPARKLE_BLUE
					{
						// Token: 0x0400AF3A RID: 44858
						public static LocString NAME = "Blue Glitter Atmo Gloves";

						// Token: 0x0400AF3B RID: 44859
						public static LocString DESC = "Sparkly blue gloves for hostile environments.";
					}

					// Token: 0x02002960 RID: 10592
					public class SPARKLE_LAVENDER
					{
						// Token: 0x0400AF3C RID: 44860
						public static LocString NAME = "Violet Glitter Atmo Gloves";

						// Token: 0x0400AF3D RID: 44861
						public static LocString DESC = "Sparkly violet gloves for hostile environments.";
					}

					// Token: 0x02002961 RID: 10593
					public class LIMONE
					{
						// Token: 0x0400AF3E RID: 44862
						public static LocString NAME = "Citrus Atmo Gloves";

						// Token: 0x0400AF3F RID: 44863
						public static LocString DESC = "Lime-inspired gloves brighten up hostile environments.";
					}

					// Token: 0x02002962 RID: 10594
					public class PUFT
					{
						// Token: 0x0400AF40 RID: 44864
						public static LocString NAME = "Puft Atmo Gloves";

						// Token: 0x0400AF41 RID: 44865
						public static LocString DESC = "A little Puft-love for delicate extremities.\nReleased for Klei Fest 2023.";
					}

					// Token: 0x02002963 RID: 10595
					public class GOLD
					{
						// Token: 0x0400AF42 RID: 44866
						public static LocString NAME = "Gold Atmo Gloves";

						// Token: 0x0400AF43 RID: 44867
						public static LocString DESC = "A golden touch! Without all the Midas-type baggage.";
					}

					// Token: 0x02002964 RID: 10596
					public class PURPLE
					{
						// Token: 0x0400AF44 RID: 44868
						public static LocString NAME = "Eggplant Atmo Gloves";

						// Token: 0x0400AF45 RID: 44869
						public static LocString DESC = "Fab purple gloves for hostile environments.";
					}

					// Token: 0x02002965 RID: 10597
					public class WHITE
					{
						// Token: 0x0400AF46 RID: 44870
						public static LocString NAME = "White Atmo Gloves";

						// Token: 0x0400AF47 RID: 44871
						public static LocString DESC = "For the Duplicant who never gets their hands dirty.";
					}

					// Token: 0x02002966 RID: 10598
					public class STRIPES_LAVENDER
					{
						// Token: 0x0400AF48 RID: 44872
						public static LocString NAME = "Wildberry Atmo Gloves";

						// Token: 0x0400AF49 RID: 44873
						public static LocString DESC = "Functional finger-protectors with fruity flair.";
					}

					// Token: 0x02002967 RID: 10599
					public class CANTALOUPE
					{
						// Token: 0x0400AF4A RID: 44874
						public static LocString NAME = "Rocketmelon Atmo Gloves";

						// Token: 0x0400AF4B RID: 44875
						public static LocString DESC = "It takes eighteen melon rinds to make a single glove.";
					}

					// Token: 0x02002968 RID: 10600
					public class BROWN
					{
						// Token: 0x0400AF4C RID: 44876
						public static LocString NAME = "Leather Atmo Gloves";

						// Token: 0x0400AF4D RID: 44877
						public static LocString DESC = "They creak rather loudly during the break-in period.";
					}
				}
			}

			// Token: 0x02002969 RID: 10601
			public class ATMO_SUIT_BELT
			{
				// Token: 0x0400AF4E RID: 44878
				public static LocString NAME = "Default Atmo Belt";

				// Token: 0x0400AF4F RID: 44879
				public static LocString DESC = "Default belt for atmo suits.";

				// Token: 0x0200296A RID: 10602
				public class FACADES
				{
					// Token: 0x0200296B RID: 10603
					public class SPARKLE_RED
					{
						// Token: 0x0400AF50 RID: 44880
						public static LocString NAME = "Red Glitter Atmo Belt";

						// Token: 0x0400AF51 RID: 44881
						public static LocString DESC = "It's red! It's shiny! It keeps atmo suit pants on!";
					}

					// Token: 0x0200296C RID: 10604
					public class SPARKLE_GREEN
					{
						// Token: 0x0400AF52 RID: 44882
						public static LocString NAME = "Green Glitter Atmo Belt";

						// Token: 0x0400AF53 RID: 44883
						public static LocString DESC = "It's green! It's shiny! It keeps atmo suit pants on!";
					}

					// Token: 0x0200296D RID: 10605
					public class SPARKLE_BLUE
					{
						// Token: 0x0400AF54 RID: 44884
						public static LocString NAME = "Blue Glitter Atmo Belt";

						// Token: 0x0400AF55 RID: 44885
						public static LocString DESC = "It's blue! It's shiny! It keeps atmo suit pants on!";
					}

					// Token: 0x0200296E RID: 10606
					public class SPARKLE_LAVENDER
					{
						// Token: 0x0400AF56 RID: 44886
						public static LocString NAME = "Violet Glitter Atmo Belt";

						// Token: 0x0400AF57 RID: 44887
						public static LocString DESC = "It's violet! It's shiny! It keeps atmo suit pants on!";
					}

					// Token: 0x0200296F RID: 10607
					public class LIMONE
					{
						// Token: 0x0400AF58 RID: 44888
						public static LocString NAME = "Citrus Atmo Belt";

						// Token: 0x0400AF59 RID: 44889
						public static LocString DESC = "This lime-hued belt really pulls an atmo suit together.";
					}

					// Token: 0x02002970 RID: 10608
					public class PUFT
					{
						// Token: 0x0400AF5A RID: 44890
						public static LocString NAME = "Puft Atmo Belt";

						// Token: 0x0400AF5B RID: 44891
						public static LocString DESC = "If critters wore belts...\nReleased for Klei Fest 2023.";
					}

					// Token: 0x02002971 RID: 10609
					public class TWOTONE_PURPLE
					{
						// Token: 0x0400AF5C RID: 44892
						public static LocString NAME = "Eggplant Atmo Belt";

						// Token: 0x0400AF5D RID: 44893
						public static LocString DESC = "In the more pretentious space-fashion circles, it's known as \"aubergine.\"";
					}

					// Token: 0x02002972 RID: 10610
					public class BASIC_GOLD
					{
						// Token: 0x0400AF5E RID: 44894
						public static LocString NAME = "Gold Atmo Belt";

						// Token: 0x0400AF5F RID: 44895
						public static LocString DESC = "Better to be overdressed than underdressed.";
					}

					// Token: 0x02002973 RID: 10611
					public class BASIC_GREY
					{
						// Token: 0x0400AF60 RID: 44896
						public static LocString NAME = "Slate Atmo Belt";

						// Token: 0x0400AF61 RID: 44897
						public static LocString DESC = "Slick and understated space style.";
					}

					// Token: 0x02002974 RID: 10612
					public class BASIC_NEON_PINK
					{
						// Token: 0x0400AF62 RID: 44898
						public static LocString NAME = "Neon Pink Atmo Belt";

						// Token: 0x0400AF63 RID: 44899
						public static LocString DESC = "Visible from several planetoids away.";
					}

					// Token: 0x02002975 RID: 10613
					public class CANTALOUPE
					{
						// Token: 0x0400AF64 RID: 44900
						public static LocString NAME = "Rocketmelon Atmo Belt";

						// Token: 0x0400AF65 RID: 44901
						public static LocString DESC = "A tribute to the <i>cucumis melo cantalupensis</i>.";
					}

					// Token: 0x02002976 RID: 10614
					public class TWOTONE_BROWN
					{
						// Token: 0x0400AF66 RID: 44902
						public static LocString NAME = "Leather Atmo Belt";

						// Token: 0x0400AF67 RID: 44903
						public static LocString DESC = "Crafted from the tanned hide of a thick-skinned critter.";
					}
				}
			}

			// Token: 0x02002977 RID: 10615
			public class ATMO_SUIT_SHOES
			{
				// Token: 0x0400AF68 RID: 44904
				public static LocString NAME = "Default Atmo Boots";

				// Token: 0x0400AF69 RID: 44905
				public static LocString DESC = "Default footwear for atmo suits.";

				// Token: 0x02002978 RID: 10616
				public class FACADES
				{
					// Token: 0x02002979 RID: 10617
					public class LIMONE
					{
						// Token: 0x0400AF6A RID: 44906
						public static LocString NAME = "Citrus Atmo Boots";

						// Token: 0x0400AF6B RID: 44907
						public static LocString DESC = "Cheery boots for stomping around in hostile environments.";
					}

					// Token: 0x0200297A RID: 10618
					public class PUFT
					{
						// Token: 0x0400AF6C RID: 44908
						public static LocString NAME = "Puft Atmo Boots";

						// Token: 0x0400AF6D RID: 44909
						public static LocString DESC = "These boots were made for puft-ing.\nReleased for Klei Fest 2023.";
					}

					// Token: 0x0200297B RID: 10619
					public class SPARKLE_BLACK
					{
						// Token: 0x0400AF6E RID: 44910
						public static LocString NAME = "Black Glitter Atmo Boots";

						// Token: 0x0400AF6F RID: 44911
						public static LocString DESC = "A timeless color, with a little pizzazz.";
					}

					// Token: 0x0200297C RID: 10620
					public class BASIC_BLACK
					{
						// Token: 0x0400AF70 RID: 44912
						public static LocString NAME = "Stealth Atmo Boots";

						// Token: 0x0400AF71 RID: 44913
						public static LocString DESC = "They attract no attention at all.";
					}

					// Token: 0x0200297D RID: 10621
					public class BASIC_PURPLE
					{
						// Token: 0x0400AF72 RID: 44914
						public static LocString NAME = "Eggplant Atmo Boots";

						// Token: 0x0400AF73 RID: 44915
						public static LocString DESC = "Purple boots for stomping around in hostile environments.";
					}

					// Token: 0x0200297E RID: 10622
					public class BASIC_LAVENDER
					{
						// Token: 0x0400AF74 RID: 44916
						public static LocString NAME = "Lavender Atmo Boots";

						// Token: 0x0400AF75 RID: 44917
						public static LocString DESC = "Soothing space booties for tired feet.";
					}

					// Token: 0x0200297F RID: 10623
					public class CANTALOUPE
					{
						// Token: 0x0400AF76 RID: 44918
						public static LocString NAME = "Rocketmelon Atmo Boots";

						// Token: 0x0400AF77 RID: 44919
						public static LocString DESC = "Keeps feet safe (and juicy) in hostile environments.";
					}
				}
			}

			// Token: 0x02002980 RID: 10624
			public class AQUA_SUIT
			{
				// Token: 0x0400AF78 RID: 44920
				public static LocString NAME = UI.FormatAsLink("Aqua Suit", "AQUA_SUIT");

				// Token: 0x0400AF79 RID: 44921
				public static LocString DESC = "Because breathing underwater is better than... not.";

				// Token: 0x0400AF7A RID: 44922
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Supplies Duplicants with <style=\"oxygen\">Oxygen</style> in underwater environments.\n\nMust be refilled with ",
					UI.FormatAsLink("Oxygen", "OXYGEN"),
					" at an ",
					UI.FormatAsLink("Atmo Suit Dock", "SUITLOCKER"),
					" when depleted."
				});

				// Token: 0x0400AF7B RID: 44923
				public static LocString RECIPE_DESC = "Supplies Duplicants with <style=\"oxygen\">Oxygen</style> in underwater environments.";

				// Token: 0x0400AF7C RID: 44924
				public static LocString WORN_NAME = UI.FormatAsLink("Worn Lead Suit", "AQUA_SUIT");

				// Token: 0x0400AF7D RID: 44925
				public static LocString WORN_DESC = string.Concat(new string[]
				{
					"A worn out ",
					UI.FormatAsLink("Aqua Suit", "AQUA_SUIT"),
					".\n\nSuits can be repaired at a ",
					UI.FormatAsLink("Crafting Station", "CRAFTINGTABLE"),
					"."
				});
			}

			// Token: 0x02002981 RID: 10625
			public class TEMPERATURE_SUIT
			{
				// Token: 0x0400AF7E RID: 44926
				public static LocString NAME = UI.FormatAsLink("Thermo Suit", "TEMPERATURE_SUIT");

				// Token: 0x0400AF7F RID: 44927
				public static LocString DESC = "Keeps my Duplicants cool in case things heat up.";

				// Token: 0x0400AF80 RID: 44928
				public static LocString EFFECT = "Provides insulation in regions with extreme <style=\"heat\">Temperatures</style>.\n\nMust be powered at a Thermo Suit Dock when depleted.";

				// Token: 0x0400AF81 RID: 44929
				public static LocString RECIPE_DESC = "Provides insulation in regions with extreme <style=\"heat\">Temperatures</style>.";

				// Token: 0x0400AF82 RID: 44930
				public static LocString WORN_NAME = UI.FormatAsLink("Worn Lead Suit", "TEMPERATURE_SUIT");

				// Token: 0x0400AF83 RID: 44931
				public static LocString WORN_DESC = string.Concat(new string[]
				{
					"A worn out ",
					UI.FormatAsLink("Thermo Suit", "TEMPERATURE_SUIT"),
					".\n\nSuits can be repaired at a ",
					UI.FormatAsLink("Crafting Station", "CRAFTINGTABLE"),
					"."
				});
			}

			// Token: 0x02002982 RID: 10626
			public class JET_SUIT
			{
				// Token: 0x0400AF84 RID: 44932
				public static LocString NAME = UI.FormatAsLink("Jet Suit", "JET_SUIT");

				// Token: 0x0400AF85 RID: 44933
				public static LocString DESC = "Allows my Duplicants to take to the skies, for a time.";

				// Token: 0x0400AF86 RID: 44934
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Supplies Duplicants with ",
					UI.FormatAsLink("Oxygen", "OXYGEN"),
					" in toxic and low breathability environments.\n\nMust be refilled with ",
					UI.FormatAsLink("Oxygen", "OXYGEN"),
					" and ",
					UI.FormatAsLink("Petroleum", "PETROLEUM"),
					" at a ",
					UI.FormatAsLink("Jet Suit Dock", "JETSUITLOCKER"),
					" when depleted."
				});

				// Token: 0x0400AF87 RID: 44935
				public static LocString RECIPE_DESC = "Supplies Duplicants with " + UI.FormatAsLink("Oxygen", "OXYGEN") + " in toxic and low breathability environments.\n\nAllows Duplicant flight.";

				// Token: 0x0400AF88 RID: 44936
				public static LocString GENERICNAME = "Jet Suit";

				// Token: 0x0400AF89 RID: 44937
				public static LocString TANK_EFFECT_NAME = "Fuel Tank";

				// Token: 0x0400AF8A RID: 44938
				public static LocString WORN_NAME = UI.FormatAsLink("Worn Jet Suit", "JET_SUIT");

				// Token: 0x0400AF8B RID: 44939
				public static LocString WORN_DESC = string.Concat(new string[]
				{
					"A worn out ",
					UI.FormatAsLink("Jet Suit", "JET_SUIT"),
					".\n\nSuits can be repaired at an ",
					UI.FormatAsLink("Exosuit Forge", "SUITFABRICATOR"),
					"."
				});
			}

			// Token: 0x02002983 RID: 10627
			public class LEAD_SUIT
			{
				// Token: 0x0400AF8C RID: 44940
				public static LocString NAME = UI.FormatAsLink("Lead Suit", "LEAD_SUIT");

				// Token: 0x0400AF8D RID: 44941
				public static LocString DESC = "Because exposure to radiation doesn't grant Duplicants superpowers.";

				// Token: 0x0400AF8E RID: 44942
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Supplies Duplicants with ",
					UI.FormatAsLink("Oxygen", "OXYGEN"),
					" and protection in areas with ",
					UI.FormatAsLink("Radiation", "RADIATION"),
					".\n\nMust be refilled with ",
					UI.FormatAsLink("Oxygen", "OXYGEN"),
					" at a ",
					UI.FormatAsLink("Lead Suit Dock", "LEADSUITLOCKER"),
					" when depleted."
				});

				// Token: 0x0400AF8F RID: 44943
				public static LocString RECIPE_DESC = string.Concat(new string[]
				{
					"Supplies Duplicants with ",
					UI.FormatAsLink("Oxygen", "OXYGEN"),
					" in toxic and low breathability environments.\n\nProtects Duplicants from ",
					UI.FormatAsLink("Radiation", "RADIATION"),
					"."
				});

				// Token: 0x0400AF90 RID: 44944
				public static LocString GENERICNAME = "Lead Suit";

				// Token: 0x0400AF91 RID: 44945
				public static LocString BATTERY_EFFECT_NAME = "Suit Battery";

				// Token: 0x0400AF92 RID: 44946
				public static LocString SUIT_OUT_OF_BATTERIES = "Suit Batteries Empty";

				// Token: 0x0400AF93 RID: 44947
				public static LocString WORN_NAME = UI.FormatAsLink("Worn Lead Suit", "LEAD_SUIT");

				// Token: 0x0400AF94 RID: 44948
				public static LocString WORN_DESC = string.Concat(new string[]
				{
					"A worn out ",
					UI.FormatAsLink("Lead Suit", "LEAD_SUIT"),
					".\n\nSuits can be repaired at an ",
					UI.FormatAsLink("Exosuit Forge", "SUITFABRICATOR"),
					"."
				});
			}

			// Token: 0x02002984 RID: 10628
			public class COOL_VEST
			{
				// Token: 0x0400AF95 RID: 44949
				public static LocString NAME = UI.FormatAsLink("Cool Vest", "COOL_VEST");

				// Token: 0x0400AF96 RID: 44950
				public static LocString GENERICNAME = "Clothing";

				// Token: 0x0400AF97 RID: 44951
				public static LocString DESC = "Don't sweat it!";

				// Token: 0x0400AF98 RID: 44952
				public static LocString EFFECT = "Protects the wearer from <style=\"heat\">Heat</style> by decreasing insulation.";

				// Token: 0x0400AF99 RID: 44953
				public static LocString RECIPE_DESC = "Protects the wearer from <style=\"heat\">Heat</style> by decreasing insulation";
			}

			// Token: 0x02002985 RID: 10629
			public class WARM_VEST
			{
				// Token: 0x0400AF9A RID: 44954
				public static LocString NAME = UI.FormatAsLink("Warm Coat", "WARM_VEST");

				// Token: 0x0400AF9B RID: 44955
				public static LocString GENERICNAME = "Clothing";

				// Token: 0x0400AF9C RID: 44956
				public static LocString DESC = "Happiness is a warm Duplicant.";

				// Token: 0x0400AF9D RID: 44957
				public static LocString EFFECT = "Protects the wearer from <style=\"heat\">Cold</style> by increasing insulation.";

				// Token: 0x0400AF9E RID: 44958
				public static LocString RECIPE_DESC = "Protects the wearer from <style=\"heat\">Cold</style> by increasing insulation";
			}

			// Token: 0x02002986 RID: 10630
			public class FUNKY_VEST
			{
				// Token: 0x0400AF9F RID: 44959
				public static LocString NAME = UI.FormatAsLink("Snazzy Suit", "FUNKY_VEST");

				// Token: 0x0400AFA0 RID: 44960
				public static LocString GENERICNAME = "Clothing";

				// Token: 0x0400AFA1 RID: 44961
				public static LocString DESC = "This transforms my Duplicant into a walking beacon of charm and style.";

				// Token: 0x0400AFA2 RID: 44962
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Increases Decor in a small area effect around the wearer. Can be upgraded to ",
					UI.FormatAsLink("Primo Garb", "CUSTOMCLOTHING"),
					" at the ",
					UI.FormatAsLink("Clothing Refashionator", "CLOTHINGALTERATIONSTATION"),
					"."
				});

				// Token: 0x0400AFA3 RID: 44963
				public static LocString RECIPE_DESC = "Increases Decor in a small area effect around the wearer. Can be upgraded to " + UI.FormatAsLink("Primo Garb", "CUSTOMCLOTHING") + " at the " + UI.FormatAsLink("Clothing Refashionator", "CLOTHINGALTERATIONSTATION");
			}

			// Token: 0x02002987 RID: 10631
			public class CUSTOMCLOTHING
			{
				// Token: 0x0400AFA4 RID: 44964
				public static LocString NAME = UI.FormatAsLink("Primo Garb", "CUSTOMCLOTHING");

				// Token: 0x0400AFA5 RID: 44965
				public static LocString GENERICNAME = "Clothing";

				// Token: 0x0400AFA6 RID: 44966
				public static LocString DESC = "This transforms my Duplicant into a colony-inspiring fashion icon.";

				// Token: 0x0400AFA7 RID: 44967
				public static LocString EFFECT = "Increases Decor in a small area effect around the wearer.";

				// Token: 0x0400AFA8 RID: 44968
				public static LocString RECIPE_DESC = "Increases Decor in a small area effect around the wearer";

				// Token: 0x02002988 RID: 10632
				public class FACADES
				{
					// Token: 0x0400AFA9 RID: 44969
					public static LocString CLUBSHIRT = UI.FormatAsLink("Purple Polyester Suit", "CUSTOMCLOTHING");

					// Token: 0x0400AFAA RID: 44970
					public static LocString CUMMERBUND = UI.FormatAsLink("Classic Cummerbund", "CUSTOMCLOTHING");

					// Token: 0x0400AFAB RID: 44971
					public static LocString DECOR_02 = UI.FormatAsLink("Snazzier Red Suit", "CUSTOMCLOTHING");

					// Token: 0x0400AFAC RID: 44972
					public static LocString DECOR_03 = UI.FormatAsLink("Snazzier Blue Suit", "CUSTOMCLOTHING");

					// Token: 0x0400AFAD RID: 44973
					public static LocString DECOR_04 = UI.FormatAsLink("Snazzier Green Suit", "CUSTOMCLOTHING");

					// Token: 0x0400AFAE RID: 44974
					public static LocString DECOR_05 = UI.FormatAsLink("Snazzier Violet Suit", "CUSTOMCLOTHING");

					// Token: 0x0400AFAF RID: 44975
					public static LocString GAUDYSWEATER = UI.FormatAsLink("Pompom Knit Suit", "CUSTOMCLOTHING");

					// Token: 0x0400AFB0 RID: 44976
					public static LocString LIMONE = UI.FormatAsLink("Citrus Spandex Suit", "CUSTOMCLOTHING");

					// Token: 0x0400AFB1 RID: 44977
					public static LocString MONDRIAN = UI.FormatAsLink("Cubist Knit Suit", "CUSTOMCLOTHING");

					// Token: 0x0400AFB2 RID: 44978
					public static LocString OVERALLS = UI.FormatAsLink("Spiffy Overalls", "CUSTOMCLOTHING");

					// Token: 0x0400AFB3 RID: 44979
					public static LocString TRIANGLES = UI.FormatAsLink("Confetti Suit", "CUSTOMCLOTHING");

					// Token: 0x0400AFB4 RID: 44980
					public static LocString WORKOUT = UI.FormatAsLink("Pink Unitard", "CUSTOMCLOTHING");
				}
			}

			// Token: 0x02002989 RID: 10633
			public class CLOTHING_GLOVES
			{
				// Token: 0x0400AFB5 RID: 44981
				public static LocString NAME = "Default Gloves";

				// Token: 0x0400AFB6 RID: 44982
				public static LocString DESC = "The default gloves.";

				// Token: 0x0200298A RID: 10634
				public class FACADES
				{
					// Token: 0x0200298B RID: 10635
					public class BASIC_BLUE_MIDDLE
					{
						// Token: 0x0400AFB7 RID: 44983
						public static LocString NAME = "Basic Aqua Gloves";

						// Token: 0x0400AFB8 RID: 44984
						public static LocString DESC = "A good, solid pair of aqua-blue gloves that go with everything.";
					}

					// Token: 0x0200298C RID: 10636
					public class BASIC_YELLOW
					{
						// Token: 0x0400AFB9 RID: 44985
						public static LocString NAME = "Basic Yellow Gloves";

						// Token: 0x0400AFBA RID: 44986
						public static LocString DESC = "A good, solid pair of yellow gloves that go with everything.";
					}

					// Token: 0x0200298D RID: 10637
					public class BASIC_BLACK
					{
						// Token: 0x0400AFBB RID: 44987
						public static LocString NAME = "Basic Black Gloves";

						// Token: 0x0400AFBC RID: 44988
						public static LocString DESC = "A good, solid pair of black gloves that go with everything.";
					}

					// Token: 0x0200298E RID: 10638
					public class BASIC_PINK_ORCHID
					{
						// Token: 0x0400AFBD RID: 44989
						public static LocString NAME = "Basic Bubblegum Gloves";

						// Token: 0x0400AFBE RID: 44990
						public static LocString DESC = "A good, solid pair of bubblegum-pink gloves that go with everything.";
					}

					// Token: 0x0200298F RID: 10639
					public class BASIC_GREEN
					{
						// Token: 0x0400AFBF RID: 44991
						public static LocString NAME = "Basic Green Gloves";

						// Token: 0x0400AFC0 RID: 44992
						public static LocString DESC = "A good, solid pair of green gloves that go with everything.";
					}

					// Token: 0x02002990 RID: 10640
					public class BASIC_ORANGE
					{
						// Token: 0x0400AFC1 RID: 44993
						public static LocString NAME = "Basic Orange Gloves";

						// Token: 0x0400AFC2 RID: 44994
						public static LocString DESC = "A good, solid pair of orange gloves that go with everything.";
					}

					// Token: 0x02002991 RID: 10641
					public class BASIC_PURPLE
					{
						// Token: 0x0400AFC3 RID: 44995
						public static LocString NAME = "Basic Purple Gloves";

						// Token: 0x0400AFC4 RID: 44996
						public static LocString DESC = "A good, solid pair of purple gloves that go with everything.";
					}

					// Token: 0x02002992 RID: 10642
					public class BASIC_RED
					{
						// Token: 0x0400AFC5 RID: 44997
						public static LocString NAME = "Basic Red Gloves";

						// Token: 0x0400AFC6 RID: 44998
						public static LocString DESC = "A good, solid pair of red gloves that go with everything.";
					}

					// Token: 0x02002993 RID: 10643
					public class BASIC_WHITE
					{
						// Token: 0x0400AFC7 RID: 44999
						public static LocString NAME = "Basic White Gloves";

						// Token: 0x0400AFC8 RID: 45000
						public static LocString DESC = "A good, solid pair of white gloves that go with everything.";
					}

					// Token: 0x02002994 RID: 10644
					public class GLOVES_ATHLETIC_DEEPRED
					{
						// Token: 0x0400AFC9 RID: 45001
						public static LocString NAME = "Team Captain Sports Gloves";

						// Token: 0x0400AFCA RID: 45002
						public static LocString DESC = "Red-striped gloves for winning at any activity.";
					}

					// Token: 0x02002995 RID: 10645
					public class GLOVES_ATHLETIC_SATSUMA
					{
						// Token: 0x0400AFCB RID: 45003
						public static LocString NAME = "Superfan Sports Gloves";

						// Token: 0x0400AFCC RID: 45004
						public static LocString DESC = "Orange-striped gloves for enthusiastic athletes.";
					}

					// Token: 0x02002996 RID: 10646
					public class GLOVES_ATHLETIC_LEMON
					{
						// Token: 0x0400AFCD RID: 45005
						public static LocString NAME = "Hype Sports Gloves";

						// Token: 0x0400AFCE RID: 45006
						public static LocString DESC = "Yellow-striped gloves for athletes who seek to raise the bar.";
					}

					// Token: 0x02002997 RID: 10647
					public class GLOVES_ATHLETIC_KELLYGREEN
					{
						// Token: 0x0400AFCF RID: 45007
						public static LocString NAME = "Go Team Sports Gloves";

						// Token: 0x0400AFD0 RID: 45008
						public static LocString DESC = "Green-striped gloves for the perenially good sport.";
					}

					// Token: 0x02002998 RID: 10648
					public class GLOVES_ATHLETIC_COBALT
					{
						// Token: 0x0400AFD1 RID: 45009
						public static LocString NAME = "True Blue Sports Gloves";

						// Token: 0x0400AFD2 RID: 45010
						public static LocString DESC = "Blue-striped gloves perfect for shaking hands after the game.";
					}

					// Token: 0x02002999 RID: 10649
					public class GLOVES_ATHLETIC_FLAMINGO
					{
						// Token: 0x0400AFD3 RID: 45011
						public static LocString NAME = "Pep Rally Sports Gloves";

						// Token: 0x0400AFD4 RID: 45012
						public static LocString DESC = "Pink-striped glove designed to withstand countless high-fives.";
					}

					// Token: 0x0200299A RID: 10650
					public class GLOVES_ATHLETIC_CHARCOAL
					{
						// Token: 0x0400AFD5 RID: 45013
						public static LocString NAME = "Underdog Sports Gloves";

						// Token: 0x0400AFD6 RID: 45014
						public static LocString DESC = "The muted stripe minimizes distractions so its wearer can focus on trying very, very hard.";
					}

					// Token: 0x0200299B RID: 10651
					public class CUFFLESS_BLUEBERRY
					{
						// Token: 0x0400AFD7 RID: 45015
						public static LocString NAME = "Blueberry Glovelets";

						// Token: 0x0400AFD8 RID: 45016
						public static LocString DESC = "Wrist coverage is <i>so</i> overrated.";
					}

					// Token: 0x0200299C RID: 10652
					public class CUFFLESS_GRAPE
					{
						// Token: 0x0400AFD9 RID: 45017
						public static LocString NAME = "Grape Glovelets";

						// Token: 0x0400AFDA RID: 45018
						public static LocString DESC = "Wrist coverage is <i>so</i> overrated.";
					}

					// Token: 0x0200299D RID: 10653
					public class CUFFLESS_LEMON
					{
						// Token: 0x0400AFDB RID: 45019
						public static LocString NAME = "Lemon Glovelets";

						// Token: 0x0400AFDC RID: 45020
						public static LocString DESC = "Wrist coverage is <i>so</i> overrated.";
					}

					// Token: 0x0200299E RID: 10654
					public class CUFFLESS_LIME
					{
						// Token: 0x0400AFDD RID: 45021
						public static LocString NAME = "Lime Glovelets";

						// Token: 0x0400AFDE RID: 45022
						public static LocString DESC = "Wrist coverage is <i>so</i> overrated.";
					}

					// Token: 0x0200299F RID: 10655
					public class CUFFLESS_SATSUMA
					{
						// Token: 0x0400AFDF RID: 45023
						public static LocString NAME = "Satsuma Glovelets";

						// Token: 0x0400AFE0 RID: 45024
						public static LocString DESC = "Wrist coverage is <i>so</i> overrated.";
					}

					// Token: 0x020029A0 RID: 10656
					public class CUFFLESS_STRAWBERRY
					{
						// Token: 0x0400AFE1 RID: 45025
						public static LocString NAME = "Strawberry Glovelets";

						// Token: 0x0400AFE2 RID: 45026
						public static LocString DESC = "Wrist coverage is <i>so</i> overrated.";
					}

					// Token: 0x020029A1 RID: 10657
					public class CUFFLESS_WATERMELON
					{
						// Token: 0x0400AFE3 RID: 45027
						public static LocString NAME = "Watermelon Glovelets";

						// Token: 0x0400AFE4 RID: 45028
						public static LocString DESC = "Wrist coverage is <i>so</i> overrated.";
					}

					// Token: 0x020029A2 RID: 10658
					public class CIRCUIT_GREEN
					{
						// Token: 0x0400AFE5 RID: 45029
						public static LocString NAME = "LED Gloves";

						// Token: 0x0400AFE6 RID: 45030
						public static LocString DESC = "Great for gesticulating at parties.";
					}

					// Token: 0x020029A3 RID: 10659
					public class ATHLETE
					{
						// Token: 0x0400AFE7 RID: 45031
						public static LocString NAME = "Racing Gloves";

						// Token: 0x0400AFE8 RID: 45032
						public static LocString DESC = "Crafted for high-speed handshakes.";
					}

					// Token: 0x020029A4 RID: 10660
					public class BASIC_BROWN_KHAKI
					{
						// Token: 0x0400AFE9 RID: 45033
						public static LocString NAME = "Basic Khaki Gloves";

						// Token: 0x0400AFEA RID: 45034
						public static LocString DESC = "They don't show dirt.";
					}

					// Token: 0x020029A5 RID: 10661
					public class BASIC_BLUEGREY
					{
						// Token: 0x0400AFEB RID: 45035
						public static LocString NAME = "Basic Gunmetal Gloves";

						// Token: 0x0400AFEC RID: 45036
						public static LocString DESC = "A tough name for soft gloves.";
					}

					// Token: 0x020029A6 RID: 10662
					public class CUFFLESS_BLACK
					{
						// Token: 0x0400AFED RID: 45037
						public static LocString NAME = "Stealth Glovelets";

						// Token: 0x0400AFEE RID: 45038
						public static LocString DESC = "It's easy to forget they're even on.";
					}

					// Token: 0x020029A7 RID: 10663
					public class DENIM_BLUE
					{
						// Token: 0x0400AFEF RID: 45039
						public static LocString NAME = "Denim Gloves";

						// Token: 0x0400AFF0 RID: 45040
						public static LocString DESC = "They're not great for dexterity.";
					}

					// Token: 0x020029A8 RID: 10664
					public class BASIC_GREY
					{
						// Token: 0x0400AFF1 RID: 45041
						public static LocString NAME = "Basic Gray Gloves";

						// Token: 0x0400AFF2 RID: 45042
						public static LocString DESC = "A good, solid pair of gray gloves that go with everything.";
					}

					// Token: 0x020029A9 RID: 10665
					public class BASIC_PINKSALMON
					{
						// Token: 0x0400AFF3 RID: 45043
						public static LocString NAME = "Basic Coral Gloves";

						// Token: 0x0400AFF4 RID: 45044
						public static LocString DESC = "A good, solid pair of bright pink gloves that go with everything.";
					}

					// Token: 0x020029AA RID: 10666
					public class BASIC_TAN
					{
						// Token: 0x0400AFF5 RID: 45045
						public static LocString NAME = "Basic Tan Gloves";

						// Token: 0x0400AFF6 RID: 45046
						public static LocString DESC = "A good, solid pair of tan gloves that go with everything.";
					}

					// Token: 0x020029AB RID: 10667
					public class BALLERINA_PINK
					{
						// Token: 0x0400AFF7 RID: 45047
						public static LocString NAME = "Ballet Gloves";

						// Token: 0x0400AFF8 RID: 45048
						public static LocString DESC = "Wrist ruffles highlight the poetic movements of the phalanges.";
					}

					// Token: 0x020029AC RID: 10668
					public class FORMAL_WHITE
					{
						// Token: 0x0400AFF9 RID: 45049
						public static LocString NAME = "White Silk Gloves";

						// Token: 0x0400AFFA RID: 45050
						public static LocString DESC = "They're as soft as...well, silk.";
					}

					// Token: 0x020029AD RID: 10669
					public class LONG_WHITE
					{
						// Token: 0x0400AFFB RID: 45051
						public static LocString NAME = "White Evening Gloves";

						// Token: 0x0400AFFC RID: 45052
						public static LocString DESC = "Super-long gloves for super-formal occasions.";
					}

					// Token: 0x020029AE RID: 10670
					public class TWOTONE_CREAM_CHARCOAL
					{
						// Token: 0x0400AFFD RID: 45053
						public static LocString NAME = "Contrast Cuff Gloves";

						// Token: 0x0400AFFE RID: 45054
						public static LocString DESC = "For elegance so understated, it may go completely unnoticed.";
					}

					// Token: 0x020029AF RID: 10671
					public class SOCKSUIT_BEIGE
					{
						// Token: 0x0400AFFF RID: 45055
						public static LocString NAME = "Vintage Handsock";

						// Token: 0x0400B000 RID: 45056
						public static LocString DESC = "Designed by someone with cold hands and an excess of old socks.";
					}

					// Token: 0x020029B0 RID: 10672
					public class BASIC_SLATE
					{
						// Token: 0x0400B001 RID: 45057
						public static LocString NAME = "Basic Slate Gloves";

						// Token: 0x0400B002 RID: 45058
						public static LocString DESC = "A good, solid pair of slate gloves that go with everything.";
					}

					// Token: 0x020029B1 RID: 10673
					public class KNIT_GOLD
					{
						// Token: 0x0400B003 RID: 45059
						public static LocString NAME = "Gold Knit Gloves";

						// Token: 0x0400B004 RID: 45060
						public static LocString DESC = "Produces a pleasantly muffled \"whump\" when high-fiving.";
					}

					// Token: 0x020029B2 RID: 10674
					public class KNIT_MAGENTA
					{
						// Token: 0x0400B005 RID: 45061
						public static LocString NAME = "Magenta Knit Gloves";

						// Token: 0x0400B006 RID: 45062
						public static LocString DESC = "Produces a pleasantly muffled \"whump\" when high-fiving.";
					}

					// Token: 0x020029B3 RID: 10675
					public class SPARKLE_WHITE
					{
						// Token: 0x0400B007 RID: 45063
						public static LocString NAME = "White Glitter Gloves";

						// Token: 0x0400B008 RID: 45064
						public static LocString DESC = "Each sequin was attached using sealant borrowed from the rocketry department.";
					}

					// Token: 0x020029B4 RID: 10676
					public class GINCH_PINK_SALTROCK
					{
						// Token: 0x0400B009 RID: 45065
						public static LocString NAME = "Frilly Saltrock Gloves";

						// Token: 0x0400B00A RID: 45066
						public static LocString DESC = "Thick, soft pink gloves with added flounce.";
					}

					// Token: 0x020029B5 RID: 10677
					public class GINCH_PURPLE_DUSKY
					{
						// Token: 0x0400B00B RID: 45067
						public static LocString NAME = "Frilly Dusk Gloves";

						// Token: 0x0400B00C RID: 45068
						public static LocString DESC = "Thick, soft purple gloves with added flounce.";
					}

					// Token: 0x020029B6 RID: 10678
					public class GINCH_BLUE_BASIN
					{
						// Token: 0x0400B00D RID: 45069
						public static LocString NAME = "Frilly Basin Gloves";

						// Token: 0x0400B00E RID: 45070
						public static LocString DESC = "Thick, soft blue gloves with added flounce.";
					}

					// Token: 0x020029B7 RID: 10679
					public class GINCH_TEAL_BALMY
					{
						// Token: 0x0400B00F RID: 45071
						public static LocString NAME = "Frilly Balm Gloves";

						// Token: 0x0400B010 RID: 45072
						public static LocString DESC = "The soft teal fabric soothes hard-working hands.";
					}

					// Token: 0x020029B8 RID: 10680
					public class GINCH_GREEN_LIME
					{
						// Token: 0x0400B011 RID: 45073
						public static LocString NAME = "Frilly Leach Gloves";

						// Token: 0x0400B012 RID: 45074
						public static LocString DESC = "Thick, soft green gloves with added flounce.";
					}

					// Token: 0x020029B9 RID: 10681
					public class GINCH_YELLOW_YELLOWCAKE
					{
						// Token: 0x0400B013 RID: 45075
						public static LocString NAME = "Frilly Yellowcake Gloves";

						// Token: 0x0400B014 RID: 45076
						public static LocString DESC = "Thick, soft yellow gloves with added flounce.";
					}

					// Token: 0x020029BA RID: 10682
					public class GINCH_ORANGE_ATOMIC
					{
						// Token: 0x0400B015 RID: 45077
						public static LocString NAME = "Frilly Atomic Gloves";

						// Token: 0x0400B016 RID: 45078
						public static LocString DESC = "Thick, bright orange gloves with added flounce.";
					}

					// Token: 0x020029BB RID: 10683
					public class GINCH_RED_MAGMA
					{
						// Token: 0x0400B017 RID: 45079
						public static LocString NAME = "Frilly Magma Gloves";

						// Token: 0x0400B018 RID: 45080
						public static LocString DESC = "Thick, soft red gloves with added flounce.";
					}

					// Token: 0x020029BC RID: 10684
					public class GINCH_GREY_GREY
					{
						// Token: 0x0400B019 RID: 45081
						public static LocString NAME = "Frilly Slate Gloves";

						// Token: 0x0400B01A RID: 45082
						public static LocString DESC = "Thick, soft grey gloves with added flounce.";
					}

					// Token: 0x020029BD RID: 10685
					public class GINCH_GREY_CHARCOAL
					{
						// Token: 0x0400B01B RID: 45083
						public static LocString NAME = "Frilly Charcoal Gloves";

						// Token: 0x0400B01C RID: 45084
						public static LocString DESC = "Thick, soft dark grey gloves with added flounce.";
					}
				}
			}

			// Token: 0x020029BE RID: 10686
			public class CLOTHING_TOPS
			{
				// Token: 0x0400B01D RID: 45085
				public static LocString NAME = "Default Top";

				// Token: 0x0400B01E RID: 45086
				public static LocString DESC = "The default shirt.";

				// Token: 0x020029BF RID: 10687
				public class FACADES
				{
					// Token: 0x020029C0 RID: 10688
					public class BASIC_BLUE_MIDDLE
					{
						// Token: 0x0400B01F RID: 45087
						public static LocString NAME = "Basic Aqua Shirt";

						// Token: 0x0400B020 RID: 45088
						public static LocString DESC = "A nice aqua-blue shirt that goes with everything.";
					}

					// Token: 0x020029C1 RID: 10689
					public class BASIC_BLACK
					{
						// Token: 0x0400B021 RID: 45089
						public static LocString NAME = "Basic Black Shirt";

						// Token: 0x0400B022 RID: 45090
						public static LocString DESC = "A nice black shirt that goes with everything.";
					}

					// Token: 0x020029C2 RID: 10690
					public class BASIC_PINK_ORCHID
					{
						// Token: 0x0400B023 RID: 45091
						public static LocString NAME = "Basic Bubblegum Shirt";

						// Token: 0x0400B024 RID: 45092
						public static LocString DESC = "A nice bubblegum-pink shirt that goes with everything.";
					}

					// Token: 0x020029C3 RID: 10691
					public class BASIC_GREEN
					{
						// Token: 0x0400B025 RID: 45093
						public static LocString NAME = "Basic Green Shirt";

						// Token: 0x0400B026 RID: 45094
						public static LocString DESC = "A nice green shirt that goes with everything.";
					}

					// Token: 0x020029C4 RID: 10692
					public class BASIC_ORANGE
					{
						// Token: 0x0400B027 RID: 45095
						public static LocString NAME = "Basic Orange Shirt";

						// Token: 0x0400B028 RID: 45096
						public static LocString DESC = "A nice orange shirt that goes with everything.";
					}

					// Token: 0x020029C5 RID: 10693
					public class BASIC_PURPLE
					{
						// Token: 0x0400B029 RID: 45097
						public static LocString NAME = "Basic Purple Shirt";

						// Token: 0x0400B02A RID: 45098
						public static LocString DESC = "A nice purple shirt that goes with everything.";
					}

					// Token: 0x020029C6 RID: 10694
					public class BASIC_RED_BURNT
					{
						// Token: 0x0400B02B RID: 45099
						public static LocString NAME = "Basic Red Shirt";

						// Token: 0x0400B02C RID: 45100
						public static LocString DESC = "A nice red shirt that goes with everything.";
					}

					// Token: 0x020029C7 RID: 10695
					public class BASIC_WHITE
					{
						// Token: 0x0400B02D RID: 45101
						public static LocString NAME = "Basic White Shirt";

						// Token: 0x0400B02E RID: 45102
						public static LocString DESC = "A nice white shirt that goes with everything.";
					}

					// Token: 0x020029C8 RID: 10696
					public class BASIC_YELLOW
					{
						// Token: 0x0400B02F RID: 45103
						public static LocString NAME = "Basic Yellow Shirt";

						// Token: 0x0400B030 RID: 45104
						public static LocString DESC = "A nice yellow shirt that goes with everything.";
					}

					// Token: 0x020029C9 RID: 10697
					public class RAGLANTOP_DEEPRED
					{
						// Token: 0x0400B031 RID: 45105
						public static LocString NAME = "Team Captain T-shirt";

						// Token: 0x0400B032 RID: 45106
						public static LocString DESC = "A slightly sweat-stained tee for natural leaders.";
					}

					// Token: 0x020029CA RID: 10698
					public class RAGLANTOP_COBALT
					{
						// Token: 0x0400B033 RID: 45107
						public static LocString NAME = "True Blue T-shirt";

						// Token: 0x0400B034 RID: 45108
						public static LocString DESC = "A slightly sweat-stained tee for the real team players.";
					}

					// Token: 0x020029CB RID: 10699
					public class RAGLANTOP_FLAMINGO
					{
						// Token: 0x0400B035 RID: 45109
						public static LocString NAME = "Pep Rally T-shirt";

						// Token: 0x0400B036 RID: 45110
						public static LocString DESC = "A slightly sweat-stained tee to boost team spirits.";
					}

					// Token: 0x020029CC RID: 10700
					public class RAGLANTOP_KELLYGREEN
					{
						// Token: 0x0400B037 RID: 45111
						public static LocString NAME = "Go Team T-shirt";

						// Token: 0x0400B038 RID: 45112
						public static LocString DESC = "A slightly sweat-stained tee for cheering from the sidelines.";
					}

					// Token: 0x020029CD RID: 10701
					public class RAGLANTOP_CHARCOAL
					{
						// Token: 0x0400B039 RID: 45113
						public static LocString NAME = "Underdog T-shirt";

						// Token: 0x0400B03A RID: 45114
						public static LocString DESC = "For those who don't win a lot.";
					}

					// Token: 0x020029CE RID: 10702
					public class RAGLANTOP_LEMON
					{
						// Token: 0x0400B03B RID: 45115
						public static LocString NAME = "Hype T-shirt";

						// Token: 0x0400B03C RID: 45116
						public static LocString DESC = "A slightly sweat-stained tee to wear when talking a big game.";
					}

					// Token: 0x020029CF RID: 10703
					public class RAGLANTOP_SATSUMA
					{
						// Token: 0x0400B03D RID: 45117
						public static LocString NAME = "Superfan T-shirt";

						// Token: 0x0400B03E RID: 45118
						public static LocString DESC = "A slightly sweat-stained tee for the long-time supporter.";
					}

					// Token: 0x020029D0 RID: 10704
					public class JELLYPUFFJACKET_BLUEBERRY
					{
						// Token: 0x0400B03F RID: 45119
						public static LocString NAME = "Blueberry Jelly Jacket";

						// Token: 0x0400B040 RID: 45120
						public static LocString DESC = "It's best to keep jelly-filled puffer jackets away from sharp corners.";
					}

					// Token: 0x020029D1 RID: 10705
					public class JELLYPUFFJACKET_GRAPE
					{
						// Token: 0x0400B041 RID: 45121
						public static LocString NAME = "Grape Jelly Jacket";

						// Token: 0x0400B042 RID: 45122
						public static LocString DESC = "It's best to keep jelly-filled puffer jackets away from sharp corners.";
					}

					// Token: 0x020029D2 RID: 10706
					public class JELLYPUFFJACKET_LEMON
					{
						// Token: 0x0400B043 RID: 45123
						public static LocString NAME = "Lemon Jelly Jacket";

						// Token: 0x0400B044 RID: 45124
						public static LocString DESC = "It's best to keep jelly-filled puffer jackets away from sharp corners.";
					}

					// Token: 0x020029D3 RID: 10707
					public class JELLYPUFFJACKET_LIME
					{
						// Token: 0x0400B045 RID: 45125
						public static LocString NAME = "Lime Jelly Jacket";

						// Token: 0x0400B046 RID: 45126
						public static LocString DESC = "It's best to keep jelly-filled puffer jackets away from sharp corners.";
					}

					// Token: 0x020029D4 RID: 10708
					public class JELLYPUFFJACKET_SATSUMA
					{
						// Token: 0x0400B047 RID: 45127
						public static LocString NAME = "Satsuma Jelly Jacket";

						// Token: 0x0400B048 RID: 45128
						public static LocString DESC = "It's best to keep jelly-filled puffer jackets away from sharp corners.";
					}

					// Token: 0x020029D5 RID: 10709
					public class JELLYPUFFJACKET_STRAWBERRY
					{
						// Token: 0x0400B049 RID: 45129
						public static LocString NAME = "Strawberry Jelly Jacket";

						// Token: 0x0400B04A RID: 45130
						public static LocString DESC = "It's best to keep jelly-filled puffer jackets away from sharp corners.";
					}

					// Token: 0x020029D6 RID: 10710
					public class JELLYPUFFJACKET_WATERMELON
					{
						// Token: 0x0400B04B RID: 45131
						public static LocString NAME = "Watermelon Jelly Jacket";

						// Token: 0x0400B04C RID: 45132
						public static LocString DESC = "It's best to keep jelly-filled puffer jackets away from sharp corners.";
					}

					// Token: 0x020029D7 RID: 10711
					public class CIRCUIT_GREEN
					{
						// Token: 0x0400B04D RID: 45133
						public static LocString NAME = "LED Jacket";

						// Token: 0x0400B04E RID: 45134
						public static LocString DESC = "For dancing in the dark.";
					}

					// Token: 0x020029D8 RID: 10712
					public class TSHIRT_WHITE
					{
						// Token: 0x0400B04F RID: 45135
						public static LocString NAME = "Classic White Tee";

						// Token: 0x0400B050 RID: 45136
						public static LocString DESC = "It's practically begging for a big Bog Jelly stain down the front.";
					}

					// Token: 0x020029D9 RID: 10713
					public class TSHIRT_MAGENTA
					{
						// Token: 0x0400B051 RID: 45137
						public static LocString NAME = "Classic Magenta Tee";

						// Token: 0x0400B052 RID: 45138
						public static LocString DESC = "It will never chafe against delicate inner-elbow skin.";
					}

					// Token: 0x020029DA RID: 10714
					public class ATHLETE
					{
						// Token: 0x0400B053 RID: 45139
						public static LocString NAME = "Racing Jacket";

						// Token: 0x0400B054 RID: 45140
						public static LocString DESC = "The epitome of fast fashion.";
					}

					// Token: 0x020029DB RID: 10715
					public class DENIM_BLUE
					{
						// Token: 0x0400B055 RID: 45141
						public static LocString NAME = "Denim Jacket";

						// Token: 0x0400B056 RID: 45142
						public static LocString DESC = "The top half of a Canadian tuxedo.";
					}

					// Token: 0x020029DC RID: 10716
					public class GONCH_STRAWBERRY
					{
						// Token: 0x0400B057 RID: 45143
						public static LocString NAME = "Executive Undershirt";

						// Token: 0x0400B058 RID: 45144
						public static LocString DESC = "The breathable base layer every power suit needs.";
					}

					// Token: 0x020029DD RID: 10717
					public class GONCH_SATSUMA
					{
						// Token: 0x0400B059 RID: 45145
						public static LocString NAME = "Underling Undershirt";

						// Token: 0x0400B05A RID: 45146
						public static LocString DESC = "Extra-absorbent fabric in the underarms to mop up nervous sweat.";
					}

					// Token: 0x020029DE RID: 10718
					public class GONCH_LEMON
					{
						// Token: 0x0400B05B RID: 45147
						public static LocString NAME = "Groupthink Undershirt";

						// Token: 0x0400B05C RID: 45148
						public static LocString DESC = "Because the most popular choice is always the right choice.";
					}

					// Token: 0x020029DF RID: 10719
					public class GONCH_LIME
					{
						// Token: 0x0400B05D RID: 45149
						public static LocString NAME = "Stakeholder Undershirt";

						// Token: 0x0400B05E RID: 45150
						public static LocString DESC = "Soft against the skin, for those who have skin in the game.";
					}

					// Token: 0x020029E0 RID: 10720
					public class GONCH_BLUEBERRY
					{
						// Token: 0x0400B05F RID: 45151
						public static LocString NAME = "Admin Undershirt";

						// Token: 0x0400B060 RID: 45152
						public static LocString DESC = "Criminally underappreciated.";
					}

					// Token: 0x020029E1 RID: 10721
					public class GONCH_GRAPE
					{
						// Token: 0x0400B061 RID: 45153
						public static LocString NAME = "Buzzword Undershirt";

						// Token: 0x0400B062 RID: 45154
						public static LocString DESC = "A value-added vest for touching base and thinking outside the box using best practices ASAP.";
					}

					// Token: 0x020029E2 RID: 10722
					public class GONCH_WATERMELON
					{
						// Token: 0x0400B063 RID: 45155
						public static LocString NAME = "Synergy Undershirt";

						// Token: 0x0400B064 RID: 45156
						public static LocString DESC = "Asking for it by name often triggers dramatic eye-rolls from bystanders.";
					}

					// Token: 0x020029E3 RID: 10723
					public class NERD_BROWN
					{
						// Token: 0x0400B065 RID: 45157
						public static LocString NAME = "Research Shirt";

						// Token: 0x0400B066 RID: 45158
						public static LocString DESC = "Comes with a thoughtfully chewed-up ballpoint pen.";
					}

					// Token: 0x020029E4 RID: 10724
					public class GI_WHITE
					{
						// Token: 0x0400B067 RID: 45159
						public static LocString NAME = "Rebel Gi Jacket";

						// Token: 0x0400B068 RID: 45160
						public static LocString DESC = "The contrasting trim hides stains from messy post-sparring snacks.";
					}

					// Token: 0x020029E5 RID: 10725
					public class JACKET_SMOKING_BURGUNDY
					{
						// Token: 0x0400B069 RID: 45161
						public static LocString NAME = "Donor Jacket";

						// Token: 0x0400B06A RID: 45162
						public static LocString DESC = "Crafted from the softest, most philanthropic fibers.";
					}

					// Token: 0x020029E6 RID: 10726
					public class MECHANIC
					{
						// Token: 0x0400B06B RID: 45163
						public static LocString NAME = "Engineer Jacket";

						// Token: 0x0400B06C RID: 45164
						public static LocString DESC = "Designed to withstand the rigors of applied science.";
					}

					// Token: 0x020029E7 RID: 10727
					public class VELOUR_BLACK
					{
						// Token: 0x0400B06D RID: 45165
						public static LocString NAME = "PhD Velour Jacket";

						// Token: 0x0400B06E RID: 45166
						public static LocString DESC = "A formal jacket for those who are \"not that kind of doctor.\"";
					}

					// Token: 0x020029E8 RID: 10728
					public class VELOUR_BLUE
					{
						// Token: 0x0400B06F RID: 45167
						public static LocString NAME = "Shortwave Velour Jacket";

						// Token: 0x0400B070 RID: 45168
						public static LocString DESC = "A luxe, pettable jacket paired with a clip-on tie.";
					}

					// Token: 0x020029E9 RID: 10729
					public class VELOUR_PINK
					{
						// Token: 0x0400B071 RID: 45169
						public static LocString NAME = "Gamma Velour Jacket";

						// Token: 0x0400B072 RID: 45170
						public static LocString DESC = "Some scientists are less shy than others.";
					}

					// Token: 0x020029EA RID: 10730
					public class WAISTCOAT_PINSTRIPE_SLATE
					{
						// Token: 0x0400B073 RID: 45171
						public static LocString NAME = "Nobel Pinstripe Waistcoat";

						// Token: 0x0400B074 RID: 45172
						public static LocString DESC = "One must dress for the prize that one wishes to win.";
					}

					// Token: 0x020029EB RID: 10731
					public class WATER
					{
						// Token: 0x0400B075 RID: 45173
						public static LocString NAME = "HVAC Khaki Shirt";

						// Token: 0x0400B076 RID: 45174
						public static LocString DESC = "Designed to regulate temperature and humidity.";
					}

					// Token: 0x020029EC RID: 10732
					public class TWEED_PINK_ORCHID
					{
						// Token: 0x0400B077 RID: 45175
						public static LocString NAME = "Power Brunch Blazer";

						// Token: 0x0400B078 RID: 45176
						public static LocString DESC = "Winners never quit, quitters never win.";
					}

					// Token: 0x020029ED RID: 10733
					public class DRESS_SLEEVELESS_BOW_BW
					{
						// Token: 0x0400B079 RID: 45177
						public static LocString NAME = "PhD Dress";

						// Token: 0x0400B07A RID: 45178
						public static LocString DESC = "Ready for a post-thesis-defense party.";
					}

					// Token: 0x020029EE RID: 10734
					public class BODYSUIT_BALLERINA_PINK
					{
						// Token: 0x0400B07B RID: 45179
						public static LocString NAME = "Ballet Leotard";

						// Token: 0x0400B07C RID: 45180
						public static LocString DESC = "Lab-crafted fabric with a level of stretchiness that defies the laws of physics.";
					}

					// Token: 0x020029EF RID: 10735
					public class SOCKSUIT_BEIGE
					{
						// Token: 0x0400B07D RID: 45181
						public static LocString NAME = "Vintage Sockshirt";

						// Token: 0x0400B07E RID: 45182
						public static LocString DESC = "Like a sock for the torso. With sleeves.";
					}

					// Token: 0x020029F0 RID: 10736
					public class X_SPORCHID
					{
						// Token: 0x0400B07F RID: 45183
						public static LocString NAME = "Sporefest Sweater";

						// Token: 0x0400B080 RID: 45184
						public static LocString DESC = "This soft knit can be worn anytime, not just during Zombie Spore season.";
					}

					// Token: 0x020029F1 RID: 10737
					public class X1_PINCHAPEPPERNUTBELLS
					{
						// Token: 0x0400B081 RID: 45185
						public static LocString NAME = "Pinchabell Jacket";

						// Token: 0x0400B082 RID: 45186
						public static LocString DESC = "The peppernuts jingle just loudly enough to be distracting.";
					}

					// Token: 0x020029F2 RID: 10738
					public class POMPOM_SHINEBUGS_PINK_PEPPERNUT
					{
						// Token: 0x0400B083 RID: 45187
						public static LocString NAME = "Pom Bug Sweater";

						// Token: 0x0400B084 RID: 45188
						public static LocString DESC = "No Shine Bugs were harmed in the making of this sweater.";
					}

					// Token: 0x020029F3 RID: 10739
					public class SNOWFLAKE_BLUE
					{
						// Token: 0x0400B085 RID: 45189
						public static LocString NAME = "Crystal-Iced Sweater";

						// Token: 0x0400B086 RID: 45190
						public static LocString DESC = "Tiny imperfections in the front pattern ensure that no two are truly identical.";
					}

					// Token: 0x020029F4 RID: 10740
					public class PJ_CLOVERS_GLITCH_KELLY
					{
						// Token: 0x0400B087 RID: 45191
						public static LocString NAME = "Lucky Jammies";

						// Token: 0x0400B088 RID: 45192
						public static LocString DESC = "Even the most brilliant minds need a little extra luck sometimes.";
					}

					// Token: 0x020029F5 RID: 10741
					public class PJ_HEARTS_CHILLI_STRAWBERRY
					{
						// Token: 0x0400B089 RID: 45193
						public static LocString NAME = "Sweetheart Jammies";

						// Token: 0x0400B08A RID: 45194
						public static LocString DESC = "Plush chenille fabric and a drool-absorbent collar? This sleepsuit really <i>is</i> \"The One.\"";
					}

					// Token: 0x020029F6 RID: 10742
					public class BUILDER
					{
						// Token: 0x0400B08B RID: 45195
						public static LocString NAME = "Hi-Vis Jacket";

						// Token: 0x0400B08C RID: 45196
						public static LocString DESC = "Unmissable style for the safety-minded.";
					}

					// Token: 0x020029F7 RID: 10743
					public class FLORAL_PINK
					{
						// Token: 0x0400B08D RID: 45197
						public static LocString NAME = "Downtime Shirt";

						// Token: 0x0400B08E RID: 45198
						public static LocString DESC = "For maxing and relaxing when errands are too taxing.";
					}

					// Token: 0x020029F8 RID: 10744
					public class GINCH_PINK_SALTROCK
					{
						// Token: 0x0400B08F RID: 45199
						public static LocString NAME = "Frilly Saltrock Undershirt";

						// Token: 0x0400B090 RID: 45200
						public static LocString DESC = "A seamless pink undershirt with laser-cut ruffles.";
					}

					// Token: 0x020029F9 RID: 10745
					public class GINCH_PURPLE_DUSKY
					{
						// Token: 0x0400B091 RID: 45201
						public static LocString NAME = "Frilly Dusk Undershirt";

						// Token: 0x0400B092 RID: 45202
						public static LocString DESC = "A seamless purple undershirt with laser-cut ruffles.";
					}

					// Token: 0x020029FA RID: 10746
					public class GINCH_BLUE_BASIN
					{
						// Token: 0x0400B093 RID: 45203
						public static LocString NAME = "Frilly Basin Undershirt";

						// Token: 0x0400B094 RID: 45204
						public static LocString DESC = "A seamless blue undershirt with laser-cut ruffles.";
					}

					// Token: 0x020029FB RID: 10747
					public class GINCH_TEAL_BALMY
					{
						// Token: 0x0400B095 RID: 45205
						public static LocString NAME = "Frilly Balm Undershirt";

						// Token: 0x0400B096 RID: 45206
						public static LocString DESC = "A seamless teal undershirt with laser-cut ruffles.";
					}

					// Token: 0x020029FC RID: 10748
					public class GINCH_GREEN_LIME
					{
						// Token: 0x0400B097 RID: 45207
						public static LocString NAME = "Frilly Leach Undershirt";

						// Token: 0x0400B098 RID: 45208
						public static LocString DESC = "A seamless green undershirt with laser-cut ruffles.";
					}

					// Token: 0x020029FD RID: 10749
					public class GINCH_YELLOW_YELLOWCAKE
					{
						// Token: 0x0400B099 RID: 45209
						public static LocString NAME = "Frilly Yellowcake Undershirt";

						// Token: 0x0400B09A RID: 45210
						public static LocString DESC = "A seamless yellow undershirt with laser-cut ruffles.";
					}

					// Token: 0x020029FE RID: 10750
					public class GINCH_ORANGE_ATOMIC
					{
						// Token: 0x0400B09B RID: 45211
						public static LocString NAME = "Frilly Atomic Undershirt";

						// Token: 0x0400B09C RID: 45212
						public static LocString DESC = "A seamless orange undershirt with laser-cut ruffles.";
					}

					// Token: 0x020029FF RID: 10751
					public class GINCH_RED_MAGMA
					{
						// Token: 0x0400B09D RID: 45213
						public static LocString NAME = "Frilly Magma Undershirt";

						// Token: 0x0400B09E RID: 45214
						public static LocString DESC = "A seamless red undershirt with laser-cut ruffles.";
					}

					// Token: 0x02002A00 RID: 10752
					public class GINCH_GREY_GREY
					{
						// Token: 0x0400B09F RID: 45215
						public static LocString NAME = "Frilly Slate Undershirt";

						// Token: 0x0400B0A0 RID: 45216
						public static LocString DESC = "A seamless grey undershirt with laser-cut ruffles.";
					}

					// Token: 0x02002A01 RID: 10753
					public class GINCH_GREY_CHARCOAL
					{
						// Token: 0x0400B0A1 RID: 45217
						public static LocString NAME = "Frilly Charcoal Undershirt";

						// Token: 0x0400B0A2 RID: 45218
						public static LocString DESC = "A seamless dark grey undershirt with laser-cut ruffles.";
					}

					// Token: 0x02002A02 RID: 10754
					public class KNIT_POLKADOT_TURQ
					{
						// Token: 0x0400B0A3 RID: 45219
						public static LocString NAME = "Polka Dot Track Jacket";

						// Token: 0x0400B0A4 RID: 45220
						public static LocString DESC = "The dots are infused with odor-neutralizing enzymes!";
					}

					// Token: 0x02002A03 RID: 10755
					public class FLASHY
					{
						// Token: 0x0400B0A5 RID: 45221
						public static LocString NAME = "Superstar Jacket";

						// Token: 0x0400B0A6 RID: 45222
						public static LocString DESC = "Some of us were not made to be subtle.";
					}
				}
			}

			// Token: 0x02002A04 RID: 10756
			public class CLOTHING_BOTTOMS
			{
				// Token: 0x0400B0A7 RID: 45223
				public static LocString NAME = "Default Bottom";

				// Token: 0x0400B0A8 RID: 45224
				public static LocString DESC = "The default bottoms.";

				// Token: 0x02002A05 RID: 10757
				public class FACADES
				{
					// Token: 0x02002A06 RID: 10758
					public class BASIC_BLUE_MIDDLE
					{
						// Token: 0x0400B0A9 RID: 45225
						public static LocString NAME = "Basic Aqua Pants";

						// Token: 0x0400B0AA RID: 45226
						public static LocString DESC = "A clean pair of aqua-blue pants that go with everything.";
					}

					// Token: 0x02002A07 RID: 10759
					public class BASIC_PINK_ORCHID
					{
						// Token: 0x0400B0AB RID: 45227
						public static LocString NAME = "Basic Bubblegum Pants";

						// Token: 0x0400B0AC RID: 45228
						public static LocString DESC = "A clean pair of bubblegum-pink pants that go with everything.";
					}

					// Token: 0x02002A08 RID: 10760
					public class BASIC_GREEN
					{
						// Token: 0x0400B0AD RID: 45229
						public static LocString NAME = "Basic Green Pants";

						// Token: 0x0400B0AE RID: 45230
						public static LocString DESC = "A clean pair of green pants that go with everything.";
					}

					// Token: 0x02002A09 RID: 10761
					public class BASIC_ORANGE
					{
						// Token: 0x0400B0AF RID: 45231
						public static LocString NAME = "Basic Orange Pants";

						// Token: 0x0400B0B0 RID: 45232
						public static LocString DESC = "A clean pair of orange pants that go with everything.";
					}

					// Token: 0x02002A0A RID: 10762
					public class BASIC_PURPLE
					{
						// Token: 0x0400B0B1 RID: 45233
						public static LocString NAME = "Basic Purple Pants";

						// Token: 0x0400B0B2 RID: 45234
						public static LocString DESC = "A clean pair of purple pants that go with everything.";
					}

					// Token: 0x02002A0B RID: 10763
					public class BASIC_RED
					{
						// Token: 0x0400B0B3 RID: 45235
						public static LocString NAME = "Basic Red Pants";

						// Token: 0x0400B0B4 RID: 45236
						public static LocString DESC = "A clean pair of red pants that go with everything.";
					}

					// Token: 0x02002A0C RID: 10764
					public class BASIC_WHITE
					{
						// Token: 0x0400B0B5 RID: 45237
						public static LocString NAME = "Basic White Pants";

						// Token: 0x0400B0B6 RID: 45238
						public static LocString DESC = "A clean pair of white pants that go with everything.";
					}

					// Token: 0x02002A0D RID: 10765
					public class BASIC_YELLOW
					{
						// Token: 0x0400B0B7 RID: 45239
						public static LocString NAME = "Basic Yellow Pants";

						// Token: 0x0400B0B8 RID: 45240
						public static LocString DESC = "A clean pair of yellow pants that go with everything.";
					}

					// Token: 0x02002A0E RID: 10766
					public class BASIC_BLACK
					{
						// Token: 0x0400B0B9 RID: 45241
						public static LocString NAME = "Basic Black Pants";

						// Token: 0x0400B0BA RID: 45242
						public static LocString DESC = "A clean pair of black pants that go with everything.";
					}

					// Token: 0x02002A0F RID: 10767
					public class SHORTS_BASIC_DEEPRED
					{
						// Token: 0x0400B0BB RID: 45243
						public static LocString NAME = "Team Captain Shorts";

						// Token: 0x0400B0BC RID: 45244
						public static LocString DESC = "A fresh pair of shorts for natural leaders.";
					}

					// Token: 0x02002A10 RID: 10768
					public class SHORTS_BASIC_SATSUMA
					{
						// Token: 0x0400B0BD RID: 45245
						public static LocString NAME = "Superfan Shorts";

						// Token: 0x0400B0BE RID: 45246
						public static LocString DESC = "A fresh pair of shorts for long-time supporters of...shorts.";
					}

					// Token: 0x02002A11 RID: 10769
					public class SHORTS_BASIC_YELLOWCAKE
					{
						// Token: 0x0400B0BF RID: 45247
						public static LocString NAME = "Yellowcake Shorts";

						// Token: 0x0400B0C0 RID: 45248
						public static LocString DESC = "A fresh pair of uranium-powder-colored shorts that are definitely not radioactive. Probably.";
					}

					// Token: 0x02002A12 RID: 10770
					public class SHORTS_BASIC_KELLYGREEN
					{
						// Token: 0x0400B0C1 RID: 45249
						public static LocString NAME = "Go Team Shorts";

						// Token: 0x0400B0C2 RID: 45250
						public static LocString DESC = "A fresh pair of shorts for cheering from the sidelines.";
					}

					// Token: 0x02002A13 RID: 10771
					public class SHORTS_BASIC_BLUE_COBALT
					{
						// Token: 0x0400B0C3 RID: 45251
						public static LocString NAME = "True Blue Shorts";

						// Token: 0x0400B0C4 RID: 45252
						public static LocString DESC = "A fresh pair of shorts for the real team players.";
					}

					// Token: 0x02002A14 RID: 10772
					public class SHORTS_BASIC_PINK_FLAMINGO
					{
						// Token: 0x0400B0C5 RID: 45253
						public static LocString NAME = "Pep Rally Shorts";

						// Token: 0x0400B0C6 RID: 45254
						public static LocString DESC = "The peppiest pair of shorts this side of the asteroid.";
					}

					// Token: 0x02002A15 RID: 10773
					public class SHORTS_BASIC_CHARCOAL
					{
						// Token: 0x0400B0C7 RID: 45255
						public static LocString NAME = "Underdog Shorts";

						// Token: 0x0400B0C8 RID: 45256
						public static LocString DESC = "A fresh pair of shorts. They're cleaner than they look.";
					}

					// Token: 0x02002A16 RID: 10774
					public class CIRCUIT_GREEN
					{
						// Token: 0x0400B0C9 RID: 45257
						public static LocString NAME = "LED Pants";

						// Token: 0x0400B0CA RID: 45258
						public static LocString DESC = "These legs are lit.";
					}

					// Token: 0x02002A17 RID: 10775
					public class ATHLETE
					{
						// Token: 0x0400B0CB RID: 45259
						public static LocString NAME = "Racing Pants";

						// Token: 0x0400B0CC RID: 45260
						public static LocString DESC = "Fast, furious fashion.";
					}

					// Token: 0x02002A18 RID: 10776
					public class BASIC_LIGHTBROWN
					{
						// Token: 0x0400B0CD RID: 45261
						public static LocString NAME = "Basic Khaki Pants";

						// Token: 0x0400B0CE RID: 45262
						public static LocString DESC = "Transition effortlessly from subterranean day to subterranean night.";
					}

					// Token: 0x02002A19 RID: 10777
					public class BASIC_REDORANGE
					{
						// Token: 0x0400B0CF RID: 45263
						public static LocString NAME = "Basic Crimson Pants";

						// Token: 0x0400B0D0 RID: 45264
						public static LocString DESC = "Like red pants, but slightly fancier-sounding.";
					}

					// Token: 0x02002A1A RID: 10778
					public class GONCH_STRAWBERRY
					{
						// Token: 0x0400B0D1 RID: 45265
						public static LocString NAME = "Executive Briefs";

						// Token: 0x0400B0D2 RID: 45266
						public static LocString DESC = "Bossy (under)pants.";
					}

					// Token: 0x02002A1B RID: 10779
					public class GONCH_SATSUMA
					{
						// Token: 0x0400B0D3 RID: 45267
						public static LocString NAME = "Underling Briefs";

						// Token: 0x0400B0D4 RID: 45268
						public static LocString DESC = "The seams are already unraveling.";
					}

					// Token: 0x02002A1C RID: 10780
					public class GONCH_LEMON
					{
						// Token: 0x0400B0D5 RID: 45269
						public static LocString NAME = "Groupthink Briefs";

						// Token: 0x0400B0D6 RID: 45270
						public static LocString DESC = "All the cool people are wearing them.";
					}

					// Token: 0x02002A1D RID: 10781
					public class GONCH_LIME
					{
						// Token: 0x0400B0D7 RID: 45271
						public static LocString NAME = "Stakeholder Briefs";

						// Token: 0x0400B0D8 RID: 45272
						public static LocString DESC = "They're really invested in keeping the wearer comfortable.";
					}

					// Token: 0x02002A1E RID: 10782
					public class GONCH_BLUEBERRY
					{
						// Token: 0x0400B0D9 RID: 45273
						public static LocString NAME = "Admin Briefs";

						// Token: 0x0400B0DA RID: 45274
						public static LocString DESC = "The workhorse of the underwear world.";
					}

					// Token: 0x02002A1F RID: 10783
					public class GONCH_GRAPE
					{
						// Token: 0x0400B0DB RID: 45275
						public static LocString NAME = "Buzzword Briefs";

						// Token: 0x0400B0DC RID: 45276
						public static LocString DESC = "Underwear that works hard, plays hard, and gives 110% to maximize the \"bottom\" line.";
					}

					// Token: 0x02002A20 RID: 10784
					public class GONCH_WATERMELON
					{
						// Token: 0x0400B0DD RID: 45277
						public static LocString NAME = "Synergy Briefs";

						// Token: 0x0400B0DE RID: 45278
						public static LocString DESC = "Teamwork makes the dream work.";
					}

					// Token: 0x02002A21 RID: 10785
					public class DENIM_BLUE
					{
						// Token: 0x0400B0DF RID: 45279
						public static LocString NAME = "Jeans";

						// Token: 0x0400B0E0 RID: 45280
						public static LocString DESC = "The bottom half of a Canadian tuxedo.";
					}

					// Token: 0x02002A22 RID: 10786
					public class GI_WHITE
					{
						// Token: 0x0400B0E1 RID: 45281
						public static LocString NAME = "White Capris";

						// Token: 0x0400B0E2 RID: 45282
						public static LocString DESC = "The cropped length is ideal for wading through flooded hallways.";
					}

					// Token: 0x02002A23 RID: 10787
					public class NERD_BROWN
					{
						// Token: 0x0400B0E3 RID: 45283
						public static LocString NAME = "Research Pants";

						// Token: 0x0400B0E4 RID: 45284
						public static LocString DESC = "The pockets are full of illegible notes that didn't quite survive the wash.";
					}

					// Token: 0x02002A24 RID: 10788
					public class SKIRT_BASIC_BLUE_MIDDLE
					{
						// Token: 0x0400B0E5 RID: 45285
						public static LocString NAME = "Aqua Rayon Skirt";

						// Token: 0x0400B0E6 RID: 45286
						public static LocString DESC = "The tag says \"Dry Clean Only.\" There are no dry cleaners in space.";
					}

					// Token: 0x02002A25 RID: 10789
					public class SKIRT_BASIC_PURPLE
					{
						// Token: 0x0400B0E7 RID: 45287
						public static LocString NAME = "Purple Rayon Skirt";

						// Token: 0x0400B0E8 RID: 45288
						public static LocString DESC = "It's not the most breathable fabric, but it <i>is</i> a lovely shade of purple.";
					}

					// Token: 0x02002A26 RID: 10790
					public class SKIRT_BASIC_GREEN
					{
						// Token: 0x0400B0E9 RID: 45289
						public static LocString NAME = "Olive Rayon Skirt";

						// Token: 0x0400B0EA RID: 45290
						public static LocString DESC = "Designed not to get snagged on ladders.";
					}

					// Token: 0x02002A27 RID: 10791
					public class SKIRT_BASIC_ORANGE
					{
						// Token: 0x0400B0EB RID: 45291
						public static LocString NAME = "Apricot Rayon Skirt";

						// Token: 0x0400B0EC RID: 45292
						public static LocString DESC = "Ready for spontaneous workplace twirling.";
					}

					// Token: 0x02002A28 RID: 10792
					public class SKIRT_BASIC_PINK_ORCHID
					{
						// Token: 0x0400B0ED RID: 45293
						public static LocString NAME = "Bubblegum Rayon Skirt";

						// Token: 0x0400B0EE RID: 45294
						public static LocString DESC = "The bubblegum scent lasts 100 washes!";
					}

					// Token: 0x02002A29 RID: 10793
					public class SKIRT_BASIC_RED
					{
						// Token: 0x0400B0EF RID: 45295
						public static LocString NAME = "Garnet Rayon Skirt";

						// Token: 0x0400B0F0 RID: 45296
						public static LocString DESC = "It's business time.";
					}

					// Token: 0x02002A2A RID: 10794
					public class SKIRT_BASIC_YELLOW
					{
						// Token: 0x0400B0F1 RID: 45297
						public static LocString NAME = "Yellow Rayon Skirt";

						// Token: 0x0400B0F2 RID: 45298
						public static LocString DESC = "A formerly white skirt that has not aged well.";
					}

					// Token: 0x02002A2B RID: 10795
					public class SKIRT_BASIC_POLKADOT
					{
						// Token: 0x0400B0F3 RID: 45299
						public static LocString NAME = "Polka Dot Skirt";

						// Token: 0x0400B0F4 RID: 45300
						public static LocString DESC = "Polka dots are a way to infinity.";
					}

					// Token: 0x02002A2C RID: 10796
					public class SKIRT_BASIC_WATERMELON
					{
						// Token: 0x0400B0F5 RID: 45301
						public static LocString NAME = "Picnic Skirt";

						// Token: 0x0400B0F6 RID: 45302
						public static LocString DESC = "The seeds are spittable, but will bear no fruit.";
					}

					// Token: 0x02002A2D RID: 10797
					public class SKIRT_DENIM_BLUE
					{
						// Token: 0x0400B0F7 RID: 45303
						public static LocString NAME = "Denim Tux Skirt";

						// Token: 0x0400B0F8 RID: 45304
						public static LocString DESC = "Designed for the casual red carpet.";
					}

					// Token: 0x02002A2E RID: 10798
					public class SKIRT_LEOPARD_PRINT_BLUE_PINK
					{
						// Token: 0x0400B0F9 RID: 45305
						public static LocString NAME = "Disco Leopard Skirt";

						// Token: 0x0400B0FA RID: 45306
						public static LocString DESC = "A faux-fur party staple.";
					}

					// Token: 0x02002A2F RID: 10799
					public class SKIRT_SPARKLE_BLUE
					{
						// Token: 0x0400B0FB RID: 45307
						public static LocString NAME = "Blue Tinsel Skirt";

						// Token: 0x0400B0FC RID: 45308
						public static LocString DESC = "The tinsel is scratchy, but look how shiny!";
					}

					// Token: 0x02002A30 RID: 10800
					public class BASIC_ORANGE_SATSUMA
					{
						// Token: 0x0400B0FD RID: 45309
						public static LocString NAME = "Hi-Vis Pants";

						// Token: 0x0400B0FE RID: 45310
						public static LocString DESC = "They make the wearer feel truly seen.";
					}

					// Token: 0x02002A31 RID: 10801
					public class PINSTRIPE_SLATE
					{
						// Token: 0x0400B0FF RID: 45311
						public static LocString NAME = "Nobel Pinstripe Trousers";

						// Token: 0x0400B100 RID: 45312
						public static LocString DESC = "There's a waterproof pocket to keep acceptance speeches smudge-free.";
					}

					// Token: 0x02002A32 RID: 10802
					public class VELOUR_BLACK
					{
						// Token: 0x0400B101 RID: 45313
						public static LocString NAME = "Black Velour Trousers";

						// Token: 0x0400B102 RID: 45314
						public static LocString DESC = "Fuzzy, formal and finely cut.";
					}

					// Token: 0x02002A33 RID: 10803
					public class VELOUR_BLUE
					{
						// Token: 0x0400B103 RID: 45315
						public static LocString NAME = "Shortwave Velour Pants";

						// Token: 0x0400B104 RID: 45316
						public static LocString DESC = "Formal wear with a sensory side.";
					}

					// Token: 0x02002A34 RID: 10804
					public class VELOUR_PINK
					{
						// Token: 0x0400B105 RID: 45317
						public static LocString NAME = "Gamma Velour Pants";

						// Token: 0x0400B106 RID: 45318
						public static LocString DESC = "They're stretchy <i>and</i> flame retardant.";
					}

					// Token: 0x02002A35 RID: 10805
					public class SKIRT_BALLERINA_PINK
					{
						// Token: 0x0400B107 RID: 45319
						public static LocString NAME = "Ballet Tutu";

						// Token: 0x0400B108 RID: 45320
						public static LocString DESC = "A tulle skirt spun and assembled by an army of patent-pending nanobots.";
					}

					// Token: 0x02002A36 RID: 10806
					public class SKIRT_TWEED_PINK_ORCHID
					{
						// Token: 0x0400B109 RID: 45321
						public static LocString NAME = "Power Brunch Skirt";

						// Token: 0x0400B10A RID: 45322
						public static LocString DESC = "It has pockets!";
					}

					// Token: 0x02002A37 RID: 10807
					public class GINCH_PINK_GLUON
					{
						// Token: 0x0400B10B RID: 45323
						public static LocString NAME = "Gluon Shorties";

						// Token: 0x0400B10C RID: 45324
						public static LocString DESC = "Comfy pink short-shorts with a ruffled hem.";
					}

					// Token: 0x02002A38 RID: 10808
					public class GINCH_PURPLE_CORTEX
					{
						// Token: 0x0400B10D RID: 45325
						public static LocString NAME = "Cortex Shorties";

						// Token: 0x0400B10E RID: 45326
						public static LocString DESC = "Comfy purple short-shorts with a ruffled hem.";
					}

					// Token: 0x02002A39 RID: 10809
					public class GINCH_BLUE_FROSTY
					{
						// Token: 0x0400B10F RID: 45327
						public static LocString NAME = "Frosty Shorties";

						// Token: 0x0400B110 RID: 45328
						public static LocString DESC = "Icy blue short-shorts with a ruffled hem.";
					}

					// Token: 0x02002A3A RID: 10810
					public class GINCH_TEAL_LOCUS
					{
						// Token: 0x0400B111 RID: 45329
						public static LocString NAME = "Locus Shorties";

						// Token: 0x0400B112 RID: 45330
						public static LocString DESC = "Comfy teal short-shorts with a ruffled hem.";
					}

					// Token: 0x02002A3B RID: 10811
					public class GINCH_GREEN_GOOP
					{
						// Token: 0x0400B113 RID: 45331
						public static LocString NAME = "Goop Shorties";

						// Token: 0x0400B114 RID: 45332
						public static LocString DESC = "Short-shorts with a ruffled hem and one pocket full of melted snacks.";
					}

					// Token: 0x02002A3C RID: 10812
					public class GINCH_YELLOW_BILE
					{
						// Token: 0x0400B115 RID: 45333
						public static LocString NAME = "Bile Shorties";

						// Token: 0x0400B116 RID: 45334
						public static LocString DESC = "Ruffled short-shorts in a stomach-turning shade of yellow.";
					}

					// Token: 0x02002A3D RID: 10813
					public class GINCH_ORANGE_NYBBLE
					{
						// Token: 0x0400B117 RID: 45335
						public static LocString NAME = "Nybble Shorties";

						// Token: 0x0400B118 RID: 45336
						public static LocString DESC = "Comfy orange ruffled short-shorts for computer scientists.";
					}

					// Token: 0x02002A3E RID: 10814
					public class GINCH_RED_IRONBOW
					{
						// Token: 0x0400B119 RID: 45337
						public static LocString NAME = "Ironbow Shorties";

						// Token: 0x0400B11A RID: 45338
						public static LocString DESC = "Comfy red short-shorts with a ruffled hem.";
					}

					// Token: 0x02002A3F RID: 10815
					public class GINCH_GREY_PHLEGM
					{
						// Token: 0x0400B11B RID: 45339
						public static LocString NAME = "Phlegmy Shorties";

						// Token: 0x0400B11C RID: 45340
						public static LocString DESC = "Ruffled short-shorts in a rather sticky shade of light grey.";
					}

					// Token: 0x02002A40 RID: 10816
					public class GINCH_GREY_OBELUS
					{
						// Token: 0x0400B11D RID: 45341
						public static LocString NAME = "Obelus Shorties";

						// Token: 0x0400B11E RID: 45342
						public static LocString DESC = "Comfy grey short-shorts with a ruffled hem.";
					}

					// Token: 0x02002A41 RID: 10817
					public class KNIT_POLKADOT_TURQ
					{
						// Token: 0x0400B11F RID: 45343
						public static LocString NAME = "Polka Dot Track Pants";

						// Token: 0x0400B120 RID: 45344
						public static LocString DESC = "For clowning around during mandatory physical fitness week.";
					}

					// Token: 0x02002A42 RID: 10818
					public class GI_BELT_WHITE_BLACK
					{
						// Token: 0x0400B121 RID: 45345
						public static LocString NAME = "Rebel Gi Pants";

						// Token: 0x0400B122 RID: 45346
						public static LocString DESC = "Relaxed-fit pants designed for roundhouse kicks.";
					}

					// Token: 0x02002A43 RID: 10819
					public class BELT_KHAKI_TAN
					{
						// Token: 0x0400B123 RID: 45347
						public static LocString NAME = "HVAC Khaki Pants";

						// Token: 0x0400B124 RID: 45348
						public static LocString DESC = "Rip-resistant fabric makes crawling through ducts a breeze.";
					}
				}
			}

			// Token: 0x02002A44 RID: 10820
			public class CLOTHING_SHOES
			{
				// Token: 0x0400B125 RID: 45349
				public static LocString NAME = "Default Footwear";

				// Token: 0x0400B126 RID: 45350
				public static LocString DESC = "The default style of footwear.";

				// Token: 0x02002A45 RID: 10821
				public class FACADES
				{
					// Token: 0x02002A46 RID: 10822
					public class BASIC_BLUE_MIDDLE
					{
						// Token: 0x0400B127 RID: 45351
						public static LocString NAME = "Basic Aqua Shoes";

						// Token: 0x0400B128 RID: 45352
						public static LocString DESC = "A fresh pair of aqua-blue shoes that go with everything.";
					}

					// Token: 0x02002A47 RID: 10823
					public class BASIC_PINK_ORCHID
					{
						// Token: 0x0400B129 RID: 45353
						public static LocString NAME = "Basic Bubblegum Shoes";

						// Token: 0x0400B12A RID: 45354
						public static LocString DESC = "A fresh pair of bubblegum-pink shoes that go with everything.";
					}

					// Token: 0x02002A48 RID: 10824
					public class BASIC_GREEN
					{
						// Token: 0x0400B12B RID: 45355
						public static LocString NAME = "Basic Green Shoes";

						// Token: 0x0400B12C RID: 45356
						public static LocString DESC = "A fresh pair of green shoes that go with everything.";
					}

					// Token: 0x02002A49 RID: 10825
					public class BASIC_ORANGE
					{
						// Token: 0x0400B12D RID: 45357
						public static LocString NAME = "Basic Orange Shoes";

						// Token: 0x0400B12E RID: 45358
						public static LocString DESC = "A fresh pair of orange shoes that go with everything.";
					}

					// Token: 0x02002A4A RID: 10826
					public class BASIC_PURPLE
					{
						// Token: 0x0400B12F RID: 45359
						public static LocString NAME = "Basic Purple Shoes";

						// Token: 0x0400B130 RID: 45360
						public static LocString DESC = "A fresh pair of purple shoes that go with everything.";
					}

					// Token: 0x02002A4B RID: 10827
					public class BASIC_RED
					{
						// Token: 0x0400B131 RID: 45361
						public static LocString NAME = "Basic Red Shoes";

						// Token: 0x0400B132 RID: 45362
						public static LocString DESC = "A fresh pair of red shoes that go with everything.";
					}

					// Token: 0x02002A4C RID: 10828
					public class BASIC_WHITE
					{
						// Token: 0x0400B133 RID: 45363
						public static LocString NAME = "Basic White Shoes";

						// Token: 0x0400B134 RID: 45364
						public static LocString DESC = "A fresh pair of white shoes that go with everything.";
					}

					// Token: 0x02002A4D RID: 10829
					public class BASIC_YELLOW
					{
						// Token: 0x0400B135 RID: 45365
						public static LocString NAME = "Basic Yellow Shoes";

						// Token: 0x0400B136 RID: 45366
						public static LocString DESC = "A fresh pair of yellow shoes that go with everything.";
					}

					// Token: 0x02002A4E RID: 10830
					public class BASIC_BLACK
					{
						// Token: 0x0400B137 RID: 45367
						public static LocString NAME = "Basic Black Shoes";

						// Token: 0x0400B138 RID: 45368
						public static LocString DESC = "A fresh pair of black shoes that go with everything.";
					}

					// Token: 0x02002A4F RID: 10831
					public class BASIC_BLUEGREY
					{
						// Token: 0x0400B139 RID: 45369
						public static LocString NAME = "Basic Gunmetal Shoes";

						// Token: 0x0400B13A RID: 45370
						public static LocString DESC = "A fresh pair of pastel shoes that go with everything.";
					}

					// Token: 0x02002A50 RID: 10832
					public class BASIC_TAN
					{
						// Token: 0x0400B13B RID: 45371
						public static LocString NAME = "Basic Tan Shoes";

						// Token: 0x0400B13C RID: 45372
						public static LocString DESC = "They're remarkably unremarkable.";
					}

					// Token: 0x02002A51 RID: 10833
					public class SOCKS_ATHLETIC_DEEPRED
					{
						// Token: 0x0400B13D RID: 45373
						public static LocString NAME = "Team Captain Gym Socks";

						// Token: 0x0400B13E RID: 45374
						public static LocString DESC = "Breathable socks with sporty red stripes.";
					}

					// Token: 0x02002A52 RID: 10834
					public class SOCKS_ATHLETIC_SATSUMA
					{
						// Token: 0x0400B13F RID: 45375
						public static LocString NAME = "Superfan Gym Socks";

						// Token: 0x0400B140 RID: 45376
						public static LocString DESC = "Breathable socks with sporty orange stripes.";
					}

					// Token: 0x02002A53 RID: 10835
					public class SOCKS_ATHLETIC_LEMON
					{
						// Token: 0x0400B141 RID: 45377
						public static LocString NAME = "Hype Gym Socks";

						// Token: 0x0400B142 RID: 45378
						public static LocString DESC = "Breathable socks with sporty yellow stripes.";
					}

					// Token: 0x02002A54 RID: 10836
					public class SOCKS_ATHLETIC_KELLYGREEN
					{
						// Token: 0x0400B143 RID: 45379
						public static LocString NAME = "Go Team Gym Socks";

						// Token: 0x0400B144 RID: 45380
						public static LocString DESC = "Breathable socks with sporty green stripes.";
					}

					// Token: 0x02002A55 RID: 10837
					public class SOCKS_ATHLETIC_COBALT
					{
						// Token: 0x0400B145 RID: 45381
						public static LocString NAME = "True Blue Gym Socks";

						// Token: 0x0400B146 RID: 45382
						public static LocString DESC = "Breathable socks with sporty blue stripes.";
					}

					// Token: 0x02002A56 RID: 10838
					public class SOCKS_ATHLETIC_FLAMINGO
					{
						// Token: 0x0400B147 RID: 45383
						public static LocString NAME = "Pep Rally Gym Socks";

						// Token: 0x0400B148 RID: 45384
						public static LocString DESC = "Breathable socks with sporty pink stripes.";
					}

					// Token: 0x02002A57 RID: 10839
					public class SOCKS_ATHLETIC_CHARCOAL
					{
						// Token: 0x0400B149 RID: 45385
						public static LocString NAME = "Underdog Gym Socks";

						// Token: 0x0400B14A RID: 45386
						public static LocString DESC = "Breathable socks that do nothing whatsoever to eliminate foot odor.";
					}

					// Token: 0x02002A58 RID: 10840
					public class BASIC_GREY
					{
						// Token: 0x0400B14B RID: 45387
						public static LocString NAME = "Basic Gray Shoes";

						// Token: 0x0400B14C RID: 45388
						public static LocString DESC = "A fresh pair of gray shoes that go with everything.";
					}

					// Token: 0x02002A59 RID: 10841
					public class DENIM_BLUE
					{
						// Token: 0x0400B14D RID: 45389
						public static LocString NAME = "Denim Shoes";

						// Token: 0x0400B14E RID: 45390
						public static LocString DESC = "Not technically essential for a Canadian tuxedo, but why not?";
					}

					// Token: 0x02002A5A RID: 10842
					public class LEGWARMERS_STRAWBERRY
					{
						// Token: 0x0400B14F RID: 45391
						public static LocString NAME = "Slouchy Strawberry Socks";

						// Token: 0x0400B150 RID: 45392
						public static LocString DESC = "Freckly knitted socks that don't stay up.";
					}

					// Token: 0x02002A5B RID: 10843
					public class LEGWARMERS_SATSUMA
					{
						// Token: 0x0400B151 RID: 45393
						public static LocString NAME = "Slouchy Satsuma Socks";

						// Token: 0x0400B152 RID: 45394
						public static LocString DESC = "Sweet knitted socks for spontaneous dance segments.";
					}

					// Token: 0x02002A5C RID: 10844
					public class LEGWARMERS_LEMON
					{
						// Token: 0x0400B153 RID: 45395
						public static LocString NAME = "Slouchy Lemon Socks";

						// Token: 0x0400B154 RID: 45396
						public static LocString DESC = "Zesty knitted socks that don't stay up.";
					}

					// Token: 0x02002A5D RID: 10845
					public class LEGWARMERS_LIME
					{
						// Token: 0x0400B155 RID: 45397
						public static LocString NAME = "Slouchy Lime Socks";

						// Token: 0x0400B156 RID: 45398
						public static LocString DESC = "Juicy knitted socks that don't stay up.";
					}

					// Token: 0x02002A5E RID: 10846
					public class LEGWARMERS_BLUEBERRY
					{
						// Token: 0x0400B157 RID: 45399
						public static LocString NAME = "Slouchy Blueberry Socks";

						// Token: 0x0400B158 RID: 45400
						public static LocString DESC = "Knitted socks with a fun bobble-stitch texture.";
					}

					// Token: 0x02002A5F RID: 10847
					public class LEGWARMERS_GRAPE
					{
						// Token: 0x0400B159 RID: 45401
						public static LocString NAME = "Slouchy Grape Socks";

						// Token: 0x0400B15A RID: 45402
						public static LocString DESC = "These fabulous knitted socks that don't stay up are really raisin the bar.";
					}

					// Token: 0x02002A60 RID: 10848
					public class LEGWARMERS_WATERMELON
					{
						// Token: 0x0400B15B RID: 45403
						public static LocString NAME = "Slouchy Watermelon Socks";

						// Token: 0x0400B15C RID: 45404
						public static LocString DESC = "Summery knitted socks that don't stay up.";
					}

					// Token: 0x02002A61 RID: 10849
					public class BALLERINA_PINK
					{
						// Token: 0x0400B15D RID: 45405
						public static LocString NAME = "Ballet Shoes";

						// Token: 0x0400B15E RID: 45406
						public static LocString DESC = "There's no \"pointe\" in aiming for anything less than perfection.";
					}

					// Token: 0x02002A62 RID: 10850
					public class MARYJANE_SOCKS_BW
					{
						// Token: 0x0400B15F RID: 45407
						public static LocString NAME = "Frilly Sock Shoes";

						// Token: 0x0400B160 RID: 45408
						public static LocString DESC = "They add a little <i>je ne sais quoi</i> to everyday lab wear.";
					}

					// Token: 0x02002A63 RID: 10851
					public class CLASSICFLATS_CREAM_CHARCOAL
					{
						// Token: 0x0400B161 RID: 45409
						public static LocString NAME = "Dressy Shoes";

						// Token: 0x0400B162 RID: 45410
						public static LocString DESC = "An enduring style, for enduring endless small talk.";
					}

					// Token: 0x02002A64 RID: 10852
					public class VELOUR_BLUE
					{
						// Token: 0x0400B163 RID: 45411
						public static LocString NAME = "Shortwave Velour Shoes";

						// Token: 0x0400B164 RID: 45412
						public static LocString DESC = "Not the easiest to keep clean.";
					}

					// Token: 0x02002A65 RID: 10853
					public class VELOUR_PINK
					{
						// Token: 0x0400B165 RID: 45413
						public static LocString NAME = "Gamma Velour Shoes";

						// Token: 0x0400B166 RID: 45414
						public static LocString DESC = "Finally, a pair of work-appropriate fuzzy shoes.";
					}

					// Token: 0x02002A66 RID: 10854
					public class VELOUR_BLACK
					{
						// Token: 0x0400B167 RID: 45415
						public static LocString NAME = "Black Velour Shoes";

						// Token: 0x0400B168 RID: 45416
						public static LocString DESC = "Matching velour lining gently tickles feet with every step.";
					}

					// Token: 0x02002A67 RID: 10855
					public class FLASHY
					{
						// Token: 0x0400B169 RID: 45417
						public static LocString NAME = "Superstar Shoes";

						// Token: 0x0400B16A RID: 45418
						public static LocString DESC = "Why walk when you can <i>moon</i>walk?";
					}

					// Token: 0x02002A68 RID: 10856
					public class GINCH_PINK_SALTROCK
					{
						// Token: 0x0400B16B RID: 45419
						public static LocString NAME = "Frilly Saltrock Socks";

						// Token: 0x0400B16C RID: 45420
						public static LocString DESC = "Thick, soft pink socks with extra flounce.";
					}

					// Token: 0x02002A69 RID: 10857
					public class GINCH_PURPLE_DUSKY
					{
						// Token: 0x0400B16D RID: 45421
						public static LocString NAME = "Frilly Dusk Socks";

						// Token: 0x0400B16E RID: 45422
						public static LocString DESC = "Thick, soft purple socks with extra flounce.";
					}

					// Token: 0x02002A6A RID: 10858
					public class GINCH_BLUE_BASIN
					{
						// Token: 0x0400B16F RID: 45423
						public static LocString NAME = "Frilly Basin Socks";

						// Token: 0x0400B170 RID: 45424
						public static LocString DESC = "Thick, soft blue socks with extra flounce.";
					}

					// Token: 0x02002A6B RID: 10859
					public class GINCH_TEAL_BALMY
					{
						// Token: 0x0400B171 RID: 45425
						public static LocString NAME = "Frilly Balm Socks";

						// Token: 0x0400B172 RID: 45426
						public static LocString DESC = "Thick, soothing teal socks with extra flounce.";
					}

					// Token: 0x02002A6C RID: 10860
					public class GINCH_GREEN_LIME
					{
						// Token: 0x0400B173 RID: 45427
						public static LocString NAME = "Frilly Leach Socks";

						// Token: 0x0400B174 RID: 45428
						public static LocString DESC = "Thick, soft green socks with extra flounce.";
					}

					// Token: 0x02002A6D RID: 10861
					public class GINCH_YELLOW_YELLOWCAKE
					{
						// Token: 0x0400B175 RID: 45429
						public static LocString NAME = "Frilly Yellowcake Socks";

						// Token: 0x0400B176 RID: 45430
						public static LocString DESC = "Dangerously soft yellow socks with extra flounce.";
					}

					// Token: 0x02002A6E RID: 10862
					public class GINCH_ORANGE_ATOMIC
					{
						// Token: 0x0400B177 RID: 45431
						public static LocString NAME = "Frilly Atomic Socks";

						// Token: 0x0400B178 RID: 45432
						public static LocString DESC = "Thick, soft orange socks with extra flounce.";
					}

					// Token: 0x02002A6F RID: 10863
					public class GINCH_RED_MAGMA
					{
						// Token: 0x0400B179 RID: 45433
						public static LocString NAME = "Frilly Magma Socks";

						// Token: 0x0400B17A RID: 45434
						public static LocString DESC = "Thick, toasty red socks with extra flounce.";
					}

					// Token: 0x02002A70 RID: 10864
					public class GINCH_GREY_GREY
					{
						// Token: 0x0400B17B RID: 45435
						public static LocString NAME = "Frilly Slate Socks";

						// Token: 0x0400B17C RID: 45436
						public static LocString DESC = "Thick, soft grey socks with extra flounce.";
					}

					// Token: 0x02002A71 RID: 10865
					public class GINCH_GREY_CHARCOAL
					{
						// Token: 0x0400B17D RID: 45437
						public static LocString NAME = "Frilly Charcoal Socks";

						// Token: 0x0400B17E RID: 45438
						public static LocString DESC = "Thick, soft dark grey socks with extra flounce.";
					}
				}
			}

			// Token: 0x02002A72 RID: 10866
			public class CLOTHING_HATS
			{
				// Token: 0x0400B17F RID: 45439
				public static LocString NAME = "Default Headgear";

				// Token: 0x0400B180 RID: 45440
				public static LocString DESC = "<DESC>";

				// Token: 0x02002A73 RID: 10867
				public class FACADES
				{
				}
			}

			// Token: 0x02002A74 RID: 10868
			public class CLOTHING_ACCESORIES
			{
				// Token: 0x0400B181 RID: 45441
				public static LocString NAME = "Default Accessory";

				// Token: 0x0400B182 RID: 45442
				public static LocString DESC = "<DESC>";

				// Token: 0x02002A75 RID: 10869
				public class FACADES
				{
				}
			}

			// Token: 0x02002A76 RID: 10870
			public class OXYGEN_TANK
			{
				// Token: 0x0400B183 RID: 45443
				public static LocString NAME = UI.FormatAsLink("Oxygen Tank", "OXYGEN_TANK");

				// Token: 0x0400B184 RID: 45444
				public static LocString GENERICNAME = "Equipment";

				// Token: 0x0400B185 RID: 45445
				public static LocString DESC = "It's like a to-go bag for your lungs.";

				// Token: 0x0400B186 RID: 45446
				public static LocString EFFECT = "Allows Duplicants to breathe in hazardous environments.\n\nDoes not work when submerged in <style=\"liquid\">Liquid</style>.";

				// Token: 0x0400B187 RID: 45447
				public static LocString RECIPE_DESC = "Allows Duplicants to breathe in hazardous environments.\n\nDoes not work when submerged in <style=\"liquid\">Liquid</style>";
			}

			// Token: 0x02002A77 RID: 10871
			public class OXYGEN_TANK_UNDERWATER
			{
				// Token: 0x0400B188 RID: 45448
				public static LocString NAME = "Oxygen Rebreather";

				// Token: 0x0400B189 RID: 45449
				public static LocString GENERICNAME = "Equipment";

				// Token: 0x0400B18A RID: 45450
				public static LocString DESC = "";

				// Token: 0x0400B18B RID: 45451
				public static LocString EFFECT = "Allows Duplicants to breathe while submerged in <style=\"liquid\">Liquid</style>.\n\nDoes not work outside of liquid.";

				// Token: 0x0400B18C RID: 45452
				public static LocString RECIPE_DESC = "Allows Duplicants to breathe while submerged in <style=\"liquid\">Liquid</style>.\n\nDoes not work outside of liquid";
			}

			// Token: 0x02002A78 RID: 10872
			public class EQUIPPABLEBALLOON
			{
				// Token: 0x0400B18D RID: 45453
				public static LocString NAME = UI.FormatAsLink("Balloon Friend", "EQUIPPABLEBALLOON");

				// Token: 0x0400B18E RID: 45454
				public static LocString DESC = "A floating friend to reassure my Duplicants they are so very, very clever.";

				// Token: 0x0400B18F RID: 45455
				public static LocString EFFECT = "Gives Duplicants a boost in brain function.\n\nSupplied by Duplicants with the Balloon Artist " + UI.FormatAsLink("Overjoyed", "MORALE") + " response.";

				// Token: 0x0400B190 RID: 45456
				public static LocString RECIPE_DESC = "Gives Duplicants a boost in brain function.\n\nSupplied by Duplicants with the Balloon Artist " + UI.FormatAsLink("Overjoyed", "MORALE") + " response";

				// Token: 0x0400B191 RID: 45457
				public static LocString GENERICNAME = "Balloon Friend";

				// Token: 0x02002A79 RID: 10873
				public class FACADES
				{
					// Token: 0x02002A7A RID: 10874
					public class DEFAULT_BALLOON
					{
						// Token: 0x0400B192 RID: 45458
						public static LocString NAME = UI.FormatAsLink("Balloon Friend", "EQUIPPABLEBALLOON");

						// Token: 0x0400B193 RID: 45459
						public static LocString DESC = "A floating friend to reassure my Duplicants that they are so very, very clever.";
					}

					// Token: 0x02002A7B RID: 10875
					public class BALLOON_FIREENGINE_LONG_SPARKLES
					{
						// Token: 0x0400B194 RID: 45460
						public static LocString NAME = UI.FormatAsLink("Magma Glitter", "EQUIPPABLEBALLOON");

						// Token: 0x0400B195 RID: 45461
						public static LocString DESC = "They float <i>and</i> sparkle!";
					}

					// Token: 0x02002A7C RID: 10876
					public class BALLOON_YELLOW_LONG_SPARKLES
					{
						// Token: 0x0400B196 RID: 45462
						public static LocString NAME = UI.FormatAsLink("Lavatory Glitter", "EQUIPPABLEBALLOON");

						// Token: 0x0400B197 RID: 45463
						public static LocString DESC = "Sparkly balloons in an all-too-familiar hue.";
					}

					// Token: 0x02002A7D RID: 10877
					public class BALLOON_BLUE_LONG_SPARKLES
					{
						// Token: 0x0400B198 RID: 45464
						public static LocString NAME = UI.FormatAsLink("Wheezewort Glitter", "EQUIPPABLEBALLOON");

						// Token: 0x0400B199 RID: 45465
						public static LocString DESC = "They float <i>and</i> sparkle!";
					}

					// Token: 0x02002A7E RID: 10878
					public class BALLOON_GREEN_LONG_SPARKLES
					{
						// Token: 0x0400B19A RID: 45466
						public static LocString NAME = UI.FormatAsLink("Mush Bar Glitter", "EQUIPPABLEBALLOON");

						// Token: 0x0400B19B RID: 45467
						public static LocString DESC = "They float <i>and</i> sparkle!";
					}

					// Token: 0x02002A7F RID: 10879
					public class BALLOON_PINK_LONG_SPARKLES
					{
						// Token: 0x0400B19C RID: 45468
						public static LocString NAME = UI.FormatAsLink("Petal Glitter", "EQUIPPABLEBALLOON");

						// Token: 0x0400B19D RID: 45469
						public static LocString DESC = "They float <i>and</i> sparkle!";
					}

					// Token: 0x02002A80 RID: 10880
					public class BALLOON_PURPLE_LONG_SPARKLES
					{
						// Token: 0x0400B19E RID: 45470
						public static LocString NAME = UI.FormatAsLink("Dusky Glitter", "EQUIPPABLEBALLOON");

						// Token: 0x0400B19F RID: 45471
						public static LocString DESC = "They float <i>and</i> sparkle!";
					}

					// Token: 0x02002A81 RID: 10881
					public class BALLOON_BABY_PACU_EGG
					{
						// Token: 0x0400B1A0 RID: 45472
						public static LocString NAME = UI.FormatAsLink("Floatie Fish", "EQUIPPABLEBALLOON");

						// Token: 0x0400B1A1 RID: 45473
						public static LocString DESC = "They do not taste as good as the real thing.";
					}

					// Token: 0x02002A82 RID: 10882
					public class BALLOON_BABY_GLOSSY_DRECKO_EGG
					{
						// Token: 0x0400B1A2 RID: 45474
						public static LocString NAME = UI.FormatAsLink("Glossy Glee", "EQUIPPABLEBALLOON");

						// Token: 0x0400B1A3 RID: 45475
						public static LocString DESC = "A happy little trio of inflatable critters.";
					}

					// Token: 0x02002A83 RID: 10883
					public class BALLOON_BABY_HATCH_EGG
					{
						// Token: 0x0400B1A4 RID: 45476
						public static LocString NAME = UI.FormatAsLink("Helium Hatches", "EQUIPPABLEBALLOON");

						// Token: 0x0400B1A5 RID: 45477
						public static LocString DESC = "A happy little trio of inflatable critters.";
					}

					// Token: 0x02002A84 RID: 10884
					public class BALLOON_BABY_POKESHELL_EGG
					{
						// Token: 0x0400B1A6 RID: 45478
						public static LocString NAME = UI.FormatAsLink("Peppy Pokeshells", "EQUIPPABLEBALLOON");

						// Token: 0x0400B1A7 RID: 45479
						public static LocString DESC = "A happy little trio of inflatable critters.";
					}

					// Token: 0x02002A85 RID: 10885
					public class BALLOON_BABY_PUFT_EGG
					{
						// Token: 0x0400B1A8 RID: 45480
						public static LocString NAME = UI.FormatAsLink("Puffed-Up Pufts", "EQUIPPABLEBALLOON");

						// Token: 0x0400B1A9 RID: 45481
						public static LocString DESC = "A happy little trio of inflatable critters.";
					}

					// Token: 0x02002A86 RID: 10886
					public class BALLOON_BABY_SHOVOLE_EGG
					{
						// Token: 0x0400B1AA RID: 45482
						public static LocString NAME = UI.FormatAsLink("Voley Voley Voles", "EQUIPPABLEBALLOON");

						// Token: 0x0400B1AB RID: 45483
						public static LocString DESC = "A happy little trio of inflatable critters.";
					}

					// Token: 0x02002A87 RID: 10887
					public class BALLOON_BABY_PIP_EGG
					{
						// Token: 0x0400B1AC RID: 45484
						public static LocString NAME = UI.FormatAsLink("Pip Pip Hooray", "EQUIPPABLEBALLOON");

						// Token: 0x0400B1AD RID: 45485
						public static LocString DESC = "A happy little trio of inflatable critters.";
					}

					// Token: 0x02002A88 RID: 10888
					public class CANDY_BLUEBERRY
					{
						// Token: 0x0400B1AE RID: 45486
						public static LocString NAME = UI.FormatAsLink("Candied Blueberry", "EQUIPPABLEBALLOON");

						// Token: 0x0400B1AF RID: 45487
						public static LocString DESC = "A juicy bunch of blueberry-scented balloons.";
					}

					// Token: 0x02002A89 RID: 10889
					public class CANDY_GRAPE
					{
						// Token: 0x0400B1B0 RID: 45488
						public static LocString NAME = UI.FormatAsLink("Candied Grape", "EQUIPPABLEBALLOON");

						// Token: 0x0400B1B1 RID: 45489
						public static LocString DESC = "A juicy bunch of grape-scented balloons.";
					}

					// Token: 0x02002A8A RID: 10890
					public class CANDY_LEMON
					{
						// Token: 0x0400B1B2 RID: 45490
						public static LocString NAME = UI.FormatAsLink("Candied Lemon", "EQUIPPABLEBALLOON");

						// Token: 0x0400B1B3 RID: 45491
						public static LocString DESC = "A juicy lemon-scented bunch of balloons.";
					}

					// Token: 0x02002A8B RID: 10891
					public class CANDY_LIME
					{
						// Token: 0x0400B1B4 RID: 45492
						public static LocString NAME = UI.FormatAsLink("Candied Lime", "EQUIPPABLEBALLOON");

						// Token: 0x0400B1B5 RID: 45493
						public static LocString DESC = "A juicy lime-scented bunch of balloons.";
					}

					// Token: 0x02002A8C RID: 10892
					public class CANDY_ORANGE
					{
						// Token: 0x0400B1B6 RID: 45494
						public static LocString NAME = UI.FormatAsLink("Candied Satsuma", "EQUIPPABLEBALLOON");

						// Token: 0x0400B1B7 RID: 45495
						public static LocString DESC = "A juicy satsuma-scented bunch of balloons.";
					}

					// Token: 0x02002A8D RID: 10893
					public class CANDY_STRAWBERRY
					{
						// Token: 0x0400B1B8 RID: 45496
						public static LocString NAME = UI.FormatAsLink("Candied Strawberry", "EQUIPPABLEBALLOON");

						// Token: 0x0400B1B9 RID: 45497
						public static LocString DESC = "A juicy strawberry-scented bunch of balloons.";
					}

					// Token: 0x02002A8E RID: 10894
					public class CANDY_WATERMELON
					{
						// Token: 0x0400B1BA RID: 45498
						public static LocString NAME = UI.FormatAsLink("Candied Watermelon", "EQUIPPABLEBALLOON");

						// Token: 0x0400B1BB RID: 45499
						public static LocString DESC = "A juicy watermelon-scented bunch of balloons.";
					}

					// Token: 0x02002A8F RID: 10895
					public class HAND_GOLD
					{
						// Token: 0x0400B1BC RID: 45500
						public static LocString NAME = UI.FormatAsLink("Gold Fingers", "EQUIPPABLEBALLOON");

						// Token: 0x0400B1BD RID: 45501
						public static LocString DESC = "Inflatable gestures of encouragement.";
					}
				}
			}

			// Token: 0x02002A90 RID: 10896
			public class SLEEPCLINICPAJAMAS
			{
				// Token: 0x0400B1BE RID: 45502
				public static LocString NAME = UI.FormatAsLink("Pajamas", "SLEEP_CLINIC_PAJAMAS");

				// Token: 0x0400B1BF RID: 45503
				public static LocString GENERICNAME = "Clothing";

				// Token: 0x0400B1C0 RID: 45504
				public static LocString DESC = "A soft, fleecy ticket to dreamland.";

				// Token: 0x0400B1C1 RID: 45505
				public static LocString EFFECT = "Helps Duplicants fall asleep by reducing " + UI.FormatAsLink("Stamina", "HEALTH") + ".\n\nEnables the wearer to dream and produce Dream Journals.";

				// Token: 0x0400B1C2 RID: 45506
				public static LocString DESTROY_TOAST = "Ripped Pajamas";
			}
		}
	}
}
