using System;

namespace TUNING
{
	// Token: 0x02002316 RID: 8982
	public class METEORS
	{
		// Token: 0x02002317 RID: 8983
		public class DIFFICULTY
		{
			// Token: 0x02002318 RID: 8984
			public class PEROID_MULTIPLIER
			{
				// Token: 0x04009E6C RID: 40556
				public const float INFREQUENT = 2f;

				// Token: 0x04009E6D RID: 40557
				public const float INTENSE = 1f;

				// Token: 0x04009E6E RID: 40558
				public const float DOOMED = 1f;
			}

			// Token: 0x02002319 RID: 8985
			public class SECONDS_PER_METEOR_MULTIPLIER
			{
				// Token: 0x04009E6F RID: 40559
				public const float INFREQUENT = 1.5f;

				// Token: 0x04009E70 RID: 40560
				public const float INTENSE = 0.8f;

				// Token: 0x04009E71 RID: 40561
				public const float DOOMED = 0.5f;
			}

			// Token: 0x0200231A RID: 8986
			public class BOMBARD_OFF_MULTIPLIER
			{
				// Token: 0x04009E72 RID: 40562
				public const float INFREQUENT = 1f;

				// Token: 0x04009E73 RID: 40563
				public const float INTENSE = 1f;

				// Token: 0x04009E74 RID: 40564
				public const float DOOMED = 0.5f;
			}

			// Token: 0x0200231B RID: 8987
			public class BOMBARD_ON_MULTIPLIER
			{
				// Token: 0x04009E75 RID: 40565
				public const float INFREQUENT = 1f;

				// Token: 0x04009E76 RID: 40566
				public const float INTENSE = 1f;

				// Token: 0x04009E77 RID: 40567
				public const float DOOMED = 1f;
			}

			// Token: 0x0200231C RID: 8988
			public class MASS_MULTIPLIER
			{
				// Token: 0x04009E78 RID: 40568
				public const float INFREQUENT = 1f;

				// Token: 0x04009E79 RID: 40569
				public const float INTENSE = 0.8f;

				// Token: 0x04009E7A RID: 40570
				public const float DOOMED = 0.5f;
			}
		}

		// Token: 0x0200231D RID: 8989
		public class IDENTIFY_DURATION
		{
			// Token: 0x04009E7B RID: 40571
			public const float TIER1 = 20f;
		}

		// Token: 0x0200231E RID: 8990
		public class PEROID
		{
			// Token: 0x04009E7C RID: 40572
			public const float TIER1 = 5f;

			// Token: 0x04009E7D RID: 40573
			public const float TIER2 = 10f;

			// Token: 0x04009E7E RID: 40574
			public const float TIER3 = 20f;

			// Token: 0x04009E7F RID: 40575
			public const float TIER4 = 30f;
		}

		// Token: 0x0200231F RID: 8991
		public class DURATION
		{
			// Token: 0x04009E80 RID: 40576
			public const float TIER0 = 1800f;

			// Token: 0x04009E81 RID: 40577
			public const float TIER1 = 3000f;

			// Token: 0x04009E82 RID: 40578
			public const float TIER2 = 4200f;

			// Token: 0x04009E83 RID: 40579
			public const float TIER3 = 6000f;
		}

		// Token: 0x02002320 RID: 8992
		public class DURATION_CLUSTER
		{
			// Token: 0x04009E84 RID: 40580
			public const float TIER0 = 75f;

			// Token: 0x04009E85 RID: 40581
			public const float TIER1 = 150f;

			// Token: 0x04009E86 RID: 40582
			public const float TIER2 = 300f;

			// Token: 0x04009E87 RID: 40583
			public const float TIER3 = 600f;

			// Token: 0x04009E88 RID: 40584
			public const float TIER4 = 1800f;

			// Token: 0x04009E89 RID: 40585
			public const float TIER5 = 3000f;
		}

		// Token: 0x02002321 RID: 8993
		public class TRAVEL_DURATION
		{
			// Token: 0x04009E8A RID: 40586
			public const float TIER0 = 600f;

			// Token: 0x04009E8B RID: 40587
			public const float TIER1 = 3000f;

			// Token: 0x04009E8C RID: 40588
			public const float TIER2 = 4500f;

			// Token: 0x04009E8D RID: 40589
			public const float TIER3 = 6000f;

			// Token: 0x04009E8E RID: 40590
			public const float TIER4 = 12000f;

			// Token: 0x04009E8F RID: 40591
			public const float TIER5 = 30000f;
		}

		// Token: 0x02002322 RID: 8994
		public class BOMBARDMENT_ON
		{
			// Token: 0x04009E90 RID: 40592
			public static MathUtil.MinMax NONE = new MathUtil.MinMax(1f, 1f);

			// Token: 0x04009E91 RID: 40593
			public static MathUtil.MinMax UNLIMITED = new MathUtil.MinMax(10000f, 10000f);

			// Token: 0x04009E92 RID: 40594
			public static MathUtil.MinMax CYCLE = new MathUtil.MinMax(600f, 600f);
		}

		// Token: 0x02002323 RID: 8995
		public class BOMBARDMENT_OFF
		{
			// Token: 0x04009E93 RID: 40595
			public static MathUtil.MinMax NONE = new MathUtil.MinMax(1f, 1f);
		}

		// Token: 0x02002324 RID: 8996
		public class TRAVELDURATION
		{
			// Token: 0x04009E94 RID: 40596
			public static float TIER0 = 0f;

			// Token: 0x04009E95 RID: 40597
			public static float TIER1 = 5f;

			// Token: 0x04009E96 RID: 40598
			public static float TIER2 = 10f;

			// Token: 0x04009E97 RID: 40599
			public static float TIER3 = 20f;

			// Token: 0x04009E98 RID: 40600
			public static float TIER4 = 30f;
		}
	}
}
