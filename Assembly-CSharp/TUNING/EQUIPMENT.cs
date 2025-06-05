using System;

namespace TUNING
{
	// Token: 0x020022F8 RID: 8952
	public class EQUIPMENT
	{
		// Token: 0x020022F9 RID: 8953
		public class TOYS
		{
			// Token: 0x04009DC0 RID: 40384
			public static string SLOT = "Toy";

			// Token: 0x04009DC1 RID: 40385
			public static float BALLOON_MASS = 1f;
		}

		// Token: 0x020022FA RID: 8954
		public class ATTRIBUTE_MOD_IDS
		{
			// Token: 0x04009DC2 RID: 40386
			public static string DECOR = "Decor";

			// Token: 0x04009DC3 RID: 40387
			public static string INSULATION = "Insulation";

			// Token: 0x04009DC4 RID: 40388
			public static string ATHLETICS = "Athletics";

			// Token: 0x04009DC5 RID: 40389
			public static string DIGGING = "Digging";

			// Token: 0x04009DC6 RID: 40390
			public static string MAX_UNDERWATER_TRAVELCOST = "MaxUnderwaterTravelCost";

			// Token: 0x04009DC7 RID: 40391
			public static string THERMAL_CONDUCTIVITY_BARRIER = "ThermalConductivityBarrier";
		}

		// Token: 0x020022FB RID: 8955
		public class TOOLS
		{
			// Token: 0x04009DC8 RID: 40392
			public static string TOOLSLOT = "Multitool";

			// Token: 0x04009DC9 RID: 40393
			public static string TOOLFABRICATOR = "MultitoolWorkbench";

			// Token: 0x04009DCA RID: 40394
			public static string TOOL_ANIM = "constructor_gun_kanim";
		}

		// Token: 0x020022FC RID: 8956
		public class CLOTHING
		{
			// Token: 0x04009DCB RID: 40395
			public static string SLOT = "Outfit";
		}

		// Token: 0x020022FD RID: 8957
		public class SUITS
		{
			// Token: 0x04009DCC RID: 40396
			public static string SLOT = "Suit";

			// Token: 0x04009DCD RID: 40397
			public static string FABRICATOR = "SuitFabricator";

			// Token: 0x04009DCE RID: 40398
			public static string ANIM = "clothing_kanim";

			// Token: 0x04009DCF RID: 40399
			public static string SNAPON = "snapTo_neck";

			// Token: 0x04009DD0 RID: 40400
			public static float SUIT_DURABILITY_SKILL_BONUS = 0.25f;

			// Token: 0x04009DD1 RID: 40401
			public static int OXYMASK_FABTIME = 20;

			// Token: 0x04009DD2 RID: 40402
			public static int ATMOSUIT_FABTIME = 40;

			// Token: 0x04009DD3 RID: 40403
			public static int ATMOSUIT_INSULATION = 50;

			// Token: 0x04009DD4 RID: 40404
			public static int ATMOSUIT_ATHLETICS = -6;

			// Token: 0x04009DD5 RID: 40405
			public static float ATMOSUIT_THERMAL_CONDUCTIVITY_BARRIER = 0.2f;

			// Token: 0x04009DD6 RID: 40406
			public static int ATMOSUIT_DIGGING = 10;

			// Token: 0x04009DD7 RID: 40407
			public static int ATMOSUIT_CONSTRUCTION = 10;

			// Token: 0x04009DD8 RID: 40408
			public static float ATMOSUIT_BLADDER = -0.18333334f;

			// Token: 0x04009DD9 RID: 40409
			public static int ATMOSUIT_MASS = 200;

			// Token: 0x04009DDA RID: 40410
			public static int ATMOSUIT_SCALDING = 1000;

			// Token: 0x04009DDB RID: 40411
			public static int ATMOSUIT_SCOLDING = -1000;

			// Token: 0x04009DDC RID: 40412
			public static float ATMOSUIT_DECAY = -0.1f;

			// Token: 0x04009DDD RID: 40413
			public static float LEADSUIT_THERMAL_CONDUCTIVITY_BARRIER = 0.3f;

			// Token: 0x04009DDE RID: 40414
			public static int LEADSUIT_SCALDING = 1000;

			// Token: 0x04009DDF RID: 40415
			public static int LEADSUIT_SCOLDING = -1000;

			// Token: 0x04009DE0 RID: 40416
			public static int LEADSUIT_INSULATION = 50;

			// Token: 0x04009DE1 RID: 40417
			public static int LEADSUIT_STRENGTH = 10;

			// Token: 0x04009DE2 RID: 40418
			public static int LEADSUIT_ATHLETICS = -8;

			// Token: 0x04009DE3 RID: 40419
			public static float LEADSUIT_RADIATION_SHIELDING = 0.66f;

			// Token: 0x04009DE4 RID: 40420
			public static int AQUASUIT_FABTIME = EQUIPMENT.SUITS.ATMOSUIT_FABTIME;

			// Token: 0x04009DE5 RID: 40421
			public static int AQUASUIT_INSULATION = 0;

			// Token: 0x04009DE6 RID: 40422
			public static int AQUASUIT_ATHLETICS = EQUIPMENT.SUITS.ATMOSUIT_ATHLETICS;

			// Token: 0x04009DE7 RID: 40423
			public static int AQUASUIT_MASS = EQUIPMENT.SUITS.ATMOSUIT_MASS;

			// Token: 0x04009DE8 RID: 40424
			public static int AQUASUIT_UNDERWATER_TRAVELCOST = 6;

			// Token: 0x04009DE9 RID: 40425
			public static int TEMPERATURESUIT_FABTIME = EQUIPMENT.SUITS.ATMOSUIT_FABTIME;

			// Token: 0x04009DEA RID: 40426
			public static float TEMPERATURESUIT_INSULATION = 0.2f;

			// Token: 0x04009DEB RID: 40427
			public static int TEMPERATURESUIT_ATHLETICS = EQUIPMENT.SUITS.ATMOSUIT_ATHLETICS;

			// Token: 0x04009DEC RID: 40428
			public static int TEMPERATURESUIT_MASS = EQUIPMENT.SUITS.ATMOSUIT_MASS;

			// Token: 0x04009DED RID: 40429
			public const int OXYGEN_MASK_MASS = 15;

			// Token: 0x04009DEE RID: 40430
			public static int OXYGEN_MASK_ATHLETICS = -2;

			// Token: 0x04009DEF RID: 40431
			public static float OXYGEN_MASK_DECAY = -0.2f;

			// Token: 0x04009DF0 RID: 40432
			public static float INDESTRUCTIBLE_DURABILITY_MOD = 0f;

			// Token: 0x04009DF1 RID: 40433
			public static float REINFORCED_DURABILITY_MOD = 0.5f;

			// Token: 0x04009DF2 RID: 40434
			public static float FLIMSY_DURABILITY_MOD = 1.5f;

			// Token: 0x04009DF3 RID: 40435
			public static float THREADBARE_DURABILITY_MOD = 2f;

			// Token: 0x04009DF4 RID: 40436
			public static float MINIMUM_USABLE_SUIT_CHARGE = 0.95f;
		}

		// Token: 0x020022FE RID: 8958
		public class VESTS
		{
			// Token: 0x04009DF5 RID: 40437
			public static string SLOT = "Suit";

			// Token: 0x04009DF6 RID: 40438
			public static string FABRICATOR = "ClothingFabricator";

			// Token: 0x04009DF7 RID: 40439
			public static string SNAPON0 = "snapTo_body";

			// Token: 0x04009DF8 RID: 40440
			public static string SNAPON1 = "snapTo_arm";

			// Token: 0x04009DF9 RID: 40441
			public static string WARM_VEST_ANIM0 = "body_shirt_hot_shearling_kanim";

			// Token: 0x04009DFA RID: 40442
			public static string WARM_VEST_ICON0 = "shirt_hot_shearling_kanim";

			// Token: 0x04009DFB RID: 40443
			public static float WARM_VEST_FABTIME = 180f;

			// Token: 0x04009DFC RID: 40444
			public static float WARM_VEST_INSULATION = 0.01f;

			// Token: 0x04009DFD RID: 40445
			public static int WARM_VEST_MASS = 4;

			// Token: 0x04009DFE RID: 40446
			public static float COOL_VEST_FABTIME = EQUIPMENT.VESTS.WARM_VEST_FABTIME;

			// Token: 0x04009DFF RID: 40447
			public static float COOL_VEST_INSULATION = 0.01f;

			// Token: 0x04009E00 RID: 40448
			public static int COOL_VEST_MASS = EQUIPMENT.VESTS.WARM_VEST_MASS;

			// Token: 0x04009E01 RID: 40449
			public static float FUNKY_VEST_FABTIME = EQUIPMENT.VESTS.WARM_VEST_FABTIME;

			// Token: 0x04009E02 RID: 40450
			public static float FUNKY_VEST_DECOR = 1f;

			// Token: 0x04009E03 RID: 40451
			public static int FUNKY_VEST_MASS = EQUIPMENT.VESTS.WARM_VEST_MASS;

			// Token: 0x04009E04 RID: 40452
			public static float CUSTOM_CLOTHING_FABTIME = 180f;

			// Token: 0x04009E05 RID: 40453
			public static float CUSTOM_ATMOSUIT_FABTIME = 15f;

			// Token: 0x04009E06 RID: 40454
			public static int CUSTOM_CLOTHING_MASS = EQUIPMENT.VESTS.WARM_VEST_MASS + 3;
		}
	}
}
