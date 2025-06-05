using System;

namespace TUNING
{
	// Token: 0x020022CD RID: 8909
	public class DISEASE
	{
		// Token: 0x04009C04 RID: 39940
		public const int COUNT_SCALER = 1000;

		// Token: 0x04009C05 RID: 39941
		public const int GENERIC_EMIT_COUNT = 100000;

		// Token: 0x04009C06 RID: 39942
		public const float GENERIC_EMIT_INTERVAL = 5f;

		// Token: 0x04009C07 RID: 39943
		public const float GENERIC_INFECTION_RADIUS = 1.5f;

		// Token: 0x04009C08 RID: 39944
		public const float GENERIC_INFECTION_INTERVAL = 5f;

		// Token: 0x04009C09 RID: 39945
		public const float STINKY_EMIT_MASS = 0.0025000002f;

		// Token: 0x04009C0A RID: 39946
		public const float STINKY_EMIT_INTERVAL = 2.5f;

		// Token: 0x04009C0B RID: 39947
		public const float STORAGE_TRANSFER_RATE = 0.05f;

		// Token: 0x04009C0C RID: 39948
		public const float WORKABLE_TRANSFER_RATE = 0.33f;

		// Token: 0x04009C0D RID: 39949
		public const float LADDER_TRANSFER_RATE = 0.005f;

		// Token: 0x04009C0E RID: 39950
		public const float INTERNAL_GERM_DEATH_MULTIPLIER = -0.00066666666f;

		// Token: 0x04009C0F RID: 39951
		public const float INTERNAL_GERM_DEATH_ADDEND = -0.8333333f;

		// Token: 0x04009C10 RID: 39952
		public const float MINIMUM_IMMUNE_DAMAGE = 0.00016666666f;

		// Token: 0x020022CE RID: 8910
		public class DURATION
		{
			// Token: 0x04009C11 RID: 39953
			public const float LONG = 10800f;

			// Token: 0x04009C12 RID: 39954
			public const float LONGISH = 4620f;

			// Token: 0x04009C13 RID: 39955
			public const float NORMAL = 2220f;

			// Token: 0x04009C14 RID: 39956
			public const float SHORT = 1020f;

			// Token: 0x04009C15 RID: 39957
			public const float TEMPORARY = 180f;

			// Token: 0x04009C16 RID: 39958
			public const float VERY_BRIEF = 60f;
		}

		// Token: 0x020022CF RID: 8911
		public class IMMUNE_ATTACK_STRENGTH_PERCENT
		{
			// Token: 0x04009C17 RID: 39959
			public const float SLOW_3 = 0.00025f;

			// Token: 0x04009C18 RID: 39960
			public const float SLOW_2 = 0.0005f;

			// Token: 0x04009C19 RID: 39961
			public const float SLOW_1 = 0.00125f;

			// Token: 0x04009C1A RID: 39962
			public const float NORMAL = 0.005f;

			// Token: 0x04009C1B RID: 39963
			public const float FAST_1 = 0.0125f;

			// Token: 0x04009C1C RID: 39964
			public const float FAST_2 = 0.05f;

			// Token: 0x04009C1D RID: 39965
			public const float FAST_3 = 0.125f;
		}

		// Token: 0x020022D0 RID: 8912
		public class RADIATION_KILL_RATE
		{
			// Token: 0x04009C1E RID: 39966
			public const float NO_EFFECT = 0f;

			// Token: 0x04009C1F RID: 39967
			public const float SLOW = 1f;

			// Token: 0x04009C20 RID: 39968
			public const float NORMAL = 2.5f;

			// Token: 0x04009C21 RID: 39969
			public const float FAST = 5f;
		}

		// Token: 0x020022D1 RID: 8913
		public static class GROWTH_FACTOR
		{
			// Token: 0x04009C22 RID: 39970
			public const float NONE = float.PositiveInfinity;

			// Token: 0x04009C23 RID: 39971
			public const float DEATH_1 = 12000f;

			// Token: 0x04009C24 RID: 39972
			public const float DEATH_2 = 6000f;

			// Token: 0x04009C25 RID: 39973
			public const float DEATH_3 = 3000f;

			// Token: 0x04009C26 RID: 39974
			public const float DEATH_4 = 1200f;

			// Token: 0x04009C27 RID: 39975
			public const float DEATH_5 = 300f;

			// Token: 0x04009C28 RID: 39976
			public const float DEATH_MAX = 10f;

			// Token: 0x04009C29 RID: 39977
			public const float DEATH_INSTANT = 0f;

			// Token: 0x04009C2A RID: 39978
			public const float GROWTH_1 = -12000f;

			// Token: 0x04009C2B RID: 39979
			public const float GROWTH_2 = -6000f;

			// Token: 0x04009C2C RID: 39980
			public const float GROWTH_3 = -3000f;

			// Token: 0x04009C2D RID: 39981
			public const float GROWTH_4 = -1200f;

			// Token: 0x04009C2E RID: 39982
			public const float GROWTH_5 = -600f;

			// Token: 0x04009C2F RID: 39983
			public const float GROWTH_6 = -300f;

			// Token: 0x04009C30 RID: 39984
			public const float GROWTH_7 = -150f;
		}

		// Token: 0x020022D2 RID: 8914
		public static class UNDERPOPULATION_DEATH_RATE
		{
			// Token: 0x04009C31 RID: 39985
			public const float NONE = 0f;

			// Token: 0x04009C32 RID: 39986
			private const float BASE_NUM_TO_KILL = 400f;

			// Token: 0x04009C33 RID: 39987
			public const float SLOW = 0.6666667f;

			// Token: 0x04009C34 RID: 39988
			public const float FAST = 2.6666667f;
		}
	}
}
