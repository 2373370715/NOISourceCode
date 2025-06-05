using System;
using UnityEngine;

namespace TUNING
{
	// Token: 0x02002303 RID: 8963
	public class ROCKETRY
	{
		// Token: 0x0600BC3D RID: 48189 RVA: 0x0011D967 File Offset: 0x0011BB67
		public static float MassFromPenaltyPercentage(float penaltyPercentage = 0.5f)
		{
			return -(1f / Mathf.Pow(penaltyPercentage - 1f, 5f));
		}

		// Token: 0x0600BC3E RID: 48190 RVA: 0x0048D790 File Offset: 0x0048B990
		public static float CalculateMassWithPenalty(float realMass)
		{
			float b = Mathf.Pow(realMass / ROCKETRY.MASS_PENALTY_DIVISOR, ROCKETRY.MASS_PENALTY_EXPONENT);
			return Mathf.Max(realMass, b);
		}

		// Token: 0x04009E11 RID: 40465
		public static float MISSION_DURATION_SCALE = 1800f;

		// Token: 0x04009E12 RID: 40466
		public static float MASS_PENALTY_EXPONENT = 3.2f;

		// Token: 0x04009E13 RID: 40467
		public static float MASS_PENALTY_DIVISOR = 300f;

		// Token: 0x04009E14 RID: 40468
		public const float SELF_DESTRUCT_REFUND_FACTOR = 0.5f;

		// Token: 0x04009E15 RID: 40469
		public static float CARGO_CAPACITY_SCALE = 10f;

		// Token: 0x04009E16 RID: 40470
		public static float LIQUID_CARGO_BAY_CLUSTER_CAPACITY = 2700f;

		// Token: 0x04009E17 RID: 40471
		public static float SOLID_CARGO_BAY_CLUSTER_CAPACITY = 2700f;

		// Token: 0x04009E18 RID: 40472
		public static float GAS_CARGO_BAY_CLUSTER_CAPACITY = 1100f;

		// Token: 0x04009E19 RID: 40473
		public const float ENTITIES_CARGO_BAY_CLUSTER_CAPACITY = 100f;

		// Token: 0x04009E1A RID: 40474
		public static Vector2I ROCKET_INTERIOR_SIZE = new Vector2I(32, 32);

		// Token: 0x02002304 RID: 8964
		public class DESTINATION_RESEARCH
		{
			// Token: 0x04009E1B RID: 40475
			public static int EVERGREEN = 10;

			// Token: 0x04009E1C RID: 40476
			public static int BASIC = 50;

			// Token: 0x04009E1D RID: 40477
			public static int HIGH = 150;
		}

		// Token: 0x02002305 RID: 8965
		public class DESTINATION_ANALYSIS
		{
			// Token: 0x04009E1E RID: 40478
			public static int DISCOVERED = 50;

			// Token: 0x04009E1F RID: 40479
			public static int COMPLETE = 100;

			// Token: 0x04009E20 RID: 40480
			public static float DEFAULT_CYCLES_PER_DISCOVERY = 0.5f;
		}

		// Token: 0x02002306 RID: 8966
		public class DESTINATION_THRUST_COSTS
		{
			// Token: 0x04009E21 RID: 40481
			public static int LOW = 3;

			// Token: 0x04009E22 RID: 40482
			public static int MID = 5;

			// Token: 0x04009E23 RID: 40483
			public static int HIGH = 7;

			// Token: 0x04009E24 RID: 40484
			public static int VERY_HIGH = 9;
		}

		// Token: 0x02002307 RID: 8967
		public class CLUSTER_FOW
		{
			// Token: 0x04009E25 RID: 40485
			public static float POINTS_TO_REVEAL = 100f;

			// Token: 0x04009E26 RID: 40486
			public static float DEFAULT_CYCLES_PER_REVEAL = 0.5f;
		}

		// Token: 0x02002308 RID: 8968
		public class ENGINE_EFFICIENCY
		{
			// Token: 0x04009E27 RID: 40487
			public static float WEAK = 20f;

			// Token: 0x04009E28 RID: 40488
			public static float MEDIUM = 40f;

			// Token: 0x04009E29 RID: 40489
			public static float STRONG = 60f;

			// Token: 0x04009E2A RID: 40490
			public static float BOOSTER = 30f;
		}

		// Token: 0x02002309 RID: 8969
		public class ROCKET_HEIGHT
		{
			// Token: 0x04009E2B RID: 40491
			public static int VERY_SHORT = 10;

			// Token: 0x04009E2C RID: 40492
			public static int SHORT = 16;

			// Token: 0x04009E2D RID: 40493
			public static int MEDIUM = 20;

			// Token: 0x04009E2E RID: 40494
			public static int TALL = 25;

			// Token: 0x04009E2F RID: 40495
			public static int VERY_TALL = 35;

			// Token: 0x04009E30 RID: 40496
			public static int MAX_MODULE_STACK_HEIGHT = ROCKETRY.ROCKET_HEIGHT.VERY_TALL - 5;
		}

		// Token: 0x0200230A RID: 8970
		public class OXIDIZER_EFFICIENCY
		{
			// Token: 0x04009E31 RID: 40497
			public static float VERY_LOW = 0.334f;

			// Token: 0x04009E32 RID: 40498
			public static float LOW = 1f;

			// Token: 0x04009E33 RID: 40499
			public static float HIGH = 1.33f;
		}

		// Token: 0x0200230B RID: 8971
		public class DLC1_OXIDIZER_EFFICIENCY
		{
			// Token: 0x04009E34 RID: 40500
			public static float VERY_LOW = 1f;

			// Token: 0x04009E35 RID: 40501
			public static float LOW = 2f;

			// Token: 0x04009E36 RID: 40502
			public static float HIGH = 4f;
		}

		// Token: 0x0200230C RID: 8972
		public class CARGO_CONTAINER_MASS
		{
			// Token: 0x04009E37 RID: 40503
			public static float STATIC_MASS = 1000f;

			// Token: 0x04009E38 RID: 40504
			public static float PAYLOAD_MASS = 1000f;
		}

		// Token: 0x0200230D RID: 8973
		public class BURDEN
		{
			// Token: 0x04009E39 RID: 40505
			public static int INSIGNIFICANT = 1;

			// Token: 0x04009E3A RID: 40506
			public static int MINOR = 2;

			// Token: 0x04009E3B RID: 40507
			public static int MINOR_PLUS = 3;

			// Token: 0x04009E3C RID: 40508
			public static int MODERATE = 4;

			// Token: 0x04009E3D RID: 40509
			public static int MODERATE_PLUS = 5;

			// Token: 0x04009E3E RID: 40510
			public static int MAJOR = 6;

			// Token: 0x04009E3F RID: 40511
			public static int MAJOR_PLUS = 7;

			// Token: 0x04009E40 RID: 40512
			public static int MEGA = 9;

			// Token: 0x04009E41 RID: 40513
			public static int MONUMENTAL = 15;
		}

		// Token: 0x0200230E RID: 8974
		public class ENGINE_POWER
		{
			// Token: 0x04009E42 RID: 40514
			public static int EARLY_WEAK = 16;

			// Token: 0x04009E43 RID: 40515
			public static int EARLY_STRONG = 23;

			// Token: 0x04009E44 RID: 40516
			public static int MID_VERY_STRONG = 48;

			// Token: 0x04009E45 RID: 40517
			public static int MID_STRONG = 31;

			// Token: 0x04009E46 RID: 40518
			public static int MID_WEAK = 27;

			// Token: 0x04009E47 RID: 40519
			public static int LATE_STRONG = 34;

			// Token: 0x04009E48 RID: 40520
			public static int LATE_VERY_STRONG = 55;
		}

		// Token: 0x0200230F RID: 8975
		public class FUEL_COST_PER_DISTANCE
		{
			// Token: 0x04009E49 RID: 40521
			public static float VERY_LOW = 0.033333335f;

			// Token: 0x04009E4A RID: 40522
			public static float LOW = 0.0375f;

			// Token: 0x04009E4B RID: 40523
			public static float MEDIUM = 0.075f;

			// Token: 0x04009E4C RID: 40524
			public static float HIGH = 0.09375f;

			// Token: 0x04009E4D RID: 40525
			public static float VERY_HIGH = 0.15f;

			// Token: 0x04009E4E RID: 40526
			public static float GAS_VERY_LOW = 0.025f;

			// Token: 0x04009E4F RID: 40527
			public static float GAS_LOW = 0.027777778f;

			// Token: 0x04009E50 RID: 40528
			public static float GAS_HIGH = 0.041666668f;

			// Token: 0x04009E51 RID: 40529
			public static float PARTICLES = 0.33333334f;
		}
	}
}
