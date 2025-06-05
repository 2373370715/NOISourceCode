using System;

namespace TUNING
{
	// Token: 0x020022AB RID: 8875
	public class ROBOTS
	{
		// Token: 0x020022AC RID: 8876
		public class SCOUTBOT
		{
			// Token: 0x04009B0E RID: 39694
			public static float CARRY_CAPACITY = DUPLICANTSTATS.STANDARD.BaseStats.CARRY_CAPACITY;

			// Token: 0x04009B0F RID: 39695
			public static readonly float DIGGING = 1f;

			// Token: 0x04009B10 RID: 39696
			public static readonly float CONSTRUCTION = 1f;

			// Token: 0x04009B11 RID: 39697
			public static readonly float ATHLETICS = 1f;

			// Token: 0x04009B12 RID: 39698
			public static readonly float HIT_POINTS = 100f;

			// Token: 0x04009B13 RID: 39699
			public static readonly float BATTERY_DEPLETION_RATE = 30f;

			// Token: 0x04009B14 RID: 39700
			public static readonly float BATTERY_CAPACITY = ROBOTS.SCOUTBOT.BATTERY_DEPLETION_RATE * 10f * 600f;
		}

		// Token: 0x020022AD RID: 8877
		public class MORBBOT
		{
			// Token: 0x04009B15 RID: 39701
			public static float CARRY_CAPACITY = DUPLICANTSTATS.STANDARD.BaseStats.CARRY_CAPACITY * 2f;

			// Token: 0x04009B16 RID: 39702
			public const float DIGGING = 1f;

			// Token: 0x04009B17 RID: 39703
			public const float CONSTRUCTION = 1f;

			// Token: 0x04009B18 RID: 39704
			public const float ATHLETICS = 3f;

			// Token: 0x04009B19 RID: 39705
			public static readonly float HIT_POINTS = 100f;

			// Token: 0x04009B1A RID: 39706
			public const float LIFETIME = 6000f;

			// Token: 0x04009B1B RID: 39707
			public const float BATTERY_DEPLETION_RATE = 30f;

			// Token: 0x04009B1C RID: 39708
			public const float BATTERY_CAPACITY = 180000f;

			// Token: 0x04009B1D RID: 39709
			public const float DECONSTRUCTION_WORK_TIME = 10f;
		}

		// Token: 0x020022AE RID: 8878
		public class FETCHDRONE
		{
			// Token: 0x04009B1E RID: 39710
			public static float CARRY_CAPACITY = DUPLICANTSTATS.STANDARD.BaseStats.CARRY_CAPACITY * 2f;

			// Token: 0x04009B1F RID: 39711
			public static readonly float HIT_POINTS = 100f;

			// Token: 0x04009B20 RID: 39712
			public const float BATTERY_DEPLETION_RATE = 50f;
		}
	}
}
