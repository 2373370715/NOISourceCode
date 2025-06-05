using System;

namespace STRINGS
{
	// Token: 0x020039EE RID: 14830
	public class ROBOTS
	{
		// Token: 0x0400E042 RID: 57410
		public static LocString CATEGORY_NAME = "Robots";

		// Token: 0x020039EF RID: 14831
		public class STATS
		{
			// Token: 0x020039F0 RID: 14832
			public class INTERNALBATTERY
			{
				// Token: 0x0400E043 RID: 57411
				public static LocString NAME = "Rechargeable Battery";

				// Token: 0x0400E044 RID: 57412
				public static LocString TOOLTIP = "When this bot's battery runs out it must temporarily stop working to go recharge";
			}

			// Token: 0x020039F1 RID: 14833
			public class INTERNALCHEMICALBATTERY
			{
				// Token: 0x0400E045 RID: 57413
				public static LocString NAME = "Chemical Battery";

				// Token: 0x0400E046 RID: 57414
				public static LocString TOOLTIP = "This bot will shut down permanently when its battery runs out";
			}

			// Token: 0x020039F2 RID: 14834
			public class INTERNALBIOBATTERY
			{
				// Token: 0x0400E047 RID: 57415
				public static LocString NAME = "Biofuel";

				// Token: 0x0400E048 RID: 57416
				public static LocString TOOLTIP = "This bot will shut down permanently when its biofuel runs out";
			}

			// Token: 0x020039F3 RID: 14835
			public class INTERNALELECTROBANK
			{
				// Token: 0x0400E049 RID: 57417
				public static LocString NAME = "Power Bank";

				// Token: 0x0400E04A RID: 57418
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"When this bot's ",
					UI.PRE_KEYWORD,
					"Power Bank",
					UI.PST_KEYWORD,
					" runs out, it will stop working until a fully charged one is delivered"
				});
			}
		}

		// Token: 0x020039F4 RID: 14836
		public class ATTRIBUTES
		{
			// Token: 0x020039F5 RID: 14837
			public class INTERNALBATTERYDELTA
			{
				// Token: 0x0400E04B RID: 57419
				public static LocString NAME = "Rechargeable Battery Drain";

				// Token: 0x0400E04C RID: 57420
				public static LocString TOOLTIP = "The rate at which battery life is depleted";
			}
		}

		// Token: 0x020039F6 RID: 14838
		public class STATUSITEMS
		{
			// Token: 0x020039F7 RID: 14839
			public class CANTREACHSTATION
			{
				// Token: 0x0400E04D RID: 57421
				public static LocString NAME = "Unreachable Dock";

				// Token: 0x0400E04E RID: 57422
				public static LocString DESC = "Obstacles are preventing {0} from heading home";

				// Token: 0x0400E04F RID: 57423
				public static LocString TOOLTIP = "Obstacles are preventing {0} from heading home";
			}

			// Token: 0x020039F8 RID: 14840
			public class MOVINGTOCHARGESTATION
			{
				// Token: 0x0400E050 RID: 57424
				public static LocString NAME = "Traveling to Dock";

				// Token: 0x0400E051 RID: 57425
				public static LocString DESC = "{0} is on its way home to recharge";

				// Token: 0x0400E052 RID: 57426
				public static LocString TOOLTIP = "{0} is on its way home to recharge";
			}

			// Token: 0x020039F9 RID: 14841
			public class LOWBATTERY
			{
				// Token: 0x0400E053 RID: 57427
				public static LocString NAME = "Low Battery";

				// Token: 0x0400E054 RID: 57428
				public static LocString DESC = "{0}'s battery is low and needs to recharge";

				// Token: 0x0400E055 RID: 57429
				public static LocString TOOLTIP = "{0}'s battery is low and needs to recharge";
			}

			// Token: 0x020039FA RID: 14842
			public class LOWBATTERYNOCHARGE
			{
				// Token: 0x0400E056 RID: 57430
				public static LocString NAME = "Low Battery";

				// Token: 0x0400E057 RID: 57431
				public static LocString DESC = "{0}'s battery is low\n\nThe internal battery cannot be recharged and this robot will cease functioning after it is depleted.";

				// Token: 0x0400E058 RID: 57432
				public static LocString TOOLTIP = "{0}'s battery is low\n\nThe internal battery cannot be recharged and this robot will cease functioning after it is depleted.";
			}

			// Token: 0x020039FB RID: 14843
			public class DEADBATTERY
			{
				// Token: 0x0400E059 RID: 57433
				public static LocString NAME = "Shut Down";

				// Token: 0x0400E05A RID: 57434
				public static LocString DESC = "RIP {0}\n\n{0}'s battery has been depleted and cannot be recharged";

				// Token: 0x0400E05B RID: 57435
				public static LocString TOOLTIP = "RIP {0}\n\n{0}'s battery has been depleted and cannot be recharged";
			}

			// Token: 0x020039FC RID: 14844
			public class DEADBATTERYFLYDO
			{
				// Token: 0x0400E05C RID: 57436
				public static LocString NAME = "Shut Down";

				// Token: 0x0400E05D RID: 57437
				public static LocString DESC = "{0}'s battery has been depleted\n\n{0} will resume function when a new battery has been delivered";

				// Token: 0x0400E05E RID: 57438
				public static LocString TOOLTIP = "{0}'s battery has been depleted\n\n{0} will resume function when a new battery has been delivered";
			}

			// Token: 0x020039FD RID: 14845
			public class DUSTBINFULL
			{
				// Token: 0x0400E05F RID: 57439
				public static LocString NAME = "Dust Bin Full";

				// Token: 0x0400E060 RID: 57440
				public static LocString DESC = "{0} must return to its dock to unload";

				// Token: 0x0400E061 RID: 57441
				public static LocString TOOLTIP = "{0} must return to its dock to unload";
			}

			// Token: 0x020039FE RID: 14846
			public class WORKING
			{
				// Token: 0x0400E062 RID: 57442
				public static LocString NAME = "Working";

				// Token: 0x0400E063 RID: 57443
				public static LocString DESC = "{0} is working diligently. Great job, {0}!";

				// Token: 0x0400E064 RID: 57444
				public static LocString TOOLTIP = "{0} is working diligently. Great job, {0}!";
			}

			// Token: 0x020039FF RID: 14847
			public class UNLOADINGSTORAGE
			{
				// Token: 0x0400E065 RID: 57445
				public static LocString NAME = "Unloading";

				// Token: 0x0400E066 RID: 57446
				public static LocString DESC = "{0} is emptying out its dust bin";

				// Token: 0x0400E067 RID: 57447
				public static LocString TOOLTIP = "{0} is emptying out its dust bin";
			}

			// Token: 0x02003A00 RID: 14848
			public class CHARGING
			{
				// Token: 0x0400E068 RID: 57448
				public static LocString NAME = "Charging";

				// Token: 0x0400E069 RID: 57449
				public static LocString DESC = "{0} is recharging its battery";

				// Token: 0x0400E06A RID: 57450
				public static LocString TOOLTIP = "{0} is recharging its battery";
			}

			// Token: 0x02003A01 RID: 14849
			public class REACTPOSITIVE
			{
				// Token: 0x0400E06B RID: 57451
				public static LocString NAME = "Happy Reaction";

				// Token: 0x0400E06C RID: 57452
				public static LocString DESC = "This bot saw something nice!";

				// Token: 0x0400E06D RID: 57453
				public static LocString TOOLTIP = "This bot saw something nice!";
			}

			// Token: 0x02003A02 RID: 14850
			public class REACTNEGATIVE
			{
				// Token: 0x0400E06E RID: 57454
				public static LocString NAME = "Bothered Reaction";

				// Token: 0x0400E06F RID: 57455
				public static LocString DESC = "This bot saw something upsetting";

				// Token: 0x0400E070 RID: 57456
				public static LocString TOOLTIP = "This bot saw something upsetting";
			}
		}

		// Token: 0x02003A03 RID: 14851
		public class MODELS
		{
			// Token: 0x02003A04 RID: 14852
			public class MORB
			{
				// Token: 0x0400E071 RID: 57457
				public static LocString NAME = UI.FormatAsLink("Biobot", "STORYTRAITMORBROVER");

				// Token: 0x0400E072 RID: 57458
				public static LocString DESC = "A Pathogen-Fueled Extravehicular Geo-Exploratory Guidebot (model Y), aka \"P.E.G.G.Y.\"\n\nIt can be assigned basic building tasks and digging duties in hazardous environments.";

				// Token: 0x0400E073 RID: 57459
				public static LocString CODEX_DESC = "The pathogen-fueled guidebot is designed to maximize a colony's chances of surviving in hostile environments by meeting three core outcomes:\n\n1. Filtration and removal of toxins from environment;\n2. Safe disposal of filtered toxins through conversion into usable biofuel;\n3. Creation of geo-exploration equipment for colony expansion with minimal colonist endangerment.\n\nThe elements aggregated during this process may result in the unintentional spread of contaminants. Specialized training required for safe handling.";
			}

			// Token: 0x02003A05 RID: 14853
			public class SCOUT
			{
				// Token: 0x0400E074 RID: 57460
				public static LocString NAME = "Rover";

				// Token: 0x0400E075 RID: 57461
				public static LocString DESC = "A curious bot that can remotely explore new " + UI.CLUSTERMAP.PLANETOID_KEYWORD + " locations.";
			}

			// Token: 0x02003A06 RID: 14854
			public class SWEEPBOT
			{
				// Token: 0x0400E076 RID: 57462
				public static LocString NAME = "Sweepy";

				// Token: 0x0400E077 RID: 57463
				public static LocString DESC = string.Concat(new string[]
				{
					"An automated sweeping robot.\n\nSweeps up ",
					UI.FormatAsLink("Solid", "ELEMENTS_SOLID"),
					" debris and ",
					UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID"),
					" spills and stores the material back in its ",
					UI.FormatAsLink("Sweepy Dock", "SWEEPBOTSTATION"),
					"."
				});
			}

			// Token: 0x02003A07 RID: 14855
			public class FLYDO
			{
				// Token: 0x0400E078 RID: 57464
				public static LocString NAME = "Flydo";

				// Token: 0x0400E079 RID: 57465
				public static LocString DESC = "A programmable delivery robot.\n\nPicks up " + UI.FormatAsLink("Solid", "ELEMENTS_SOLID") + " objects for delivery to selected destinations.";
			}
		}
	}
}
