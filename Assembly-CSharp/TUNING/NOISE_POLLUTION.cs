using System;

namespace TUNING
{
	// Token: 0x020022A6 RID: 8870
	public class NOISE_POLLUTION
	{
		// Token: 0x04009AF1 RID: 39665
		public static readonly EffectorValues NONE = new EffectorValues
		{
			amount = 0,
			radius = 0
		};

		// Token: 0x04009AF2 RID: 39666
		public static readonly EffectorValues CONE_OF_SILENCE = new EffectorValues
		{
			amount = -120,
			radius = 5
		};

		// Token: 0x04009AF3 RID: 39667
		public static float DUPLICANT_TIME_THRESHOLD = 3f;

		// Token: 0x020022A7 RID: 8871
		public class LENGTHS
		{
			// Token: 0x04009AF4 RID: 39668
			public static float VERYSHORT = 0.25f;

			// Token: 0x04009AF5 RID: 39669
			public static float SHORT = 0.5f;

			// Token: 0x04009AF6 RID: 39670
			public static float NORMAL = 1f;

			// Token: 0x04009AF7 RID: 39671
			public static float LONG = 1.5f;

			// Token: 0x04009AF8 RID: 39672
			public static float VERYLONG = 2f;
		}

		// Token: 0x020022A8 RID: 8872
		public class NOISY
		{
			// Token: 0x04009AF9 RID: 39673
			public static readonly EffectorValues TIER0 = new EffectorValues
			{
				amount = 45,
				radius = 10
			};

			// Token: 0x04009AFA RID: 39674
			public static readonly EffectorValues TIER1 = new EffectorValues
			{
				amount = 55,
				radius = 10
			};

			// Token: 0x04009AFB RID: 39675
			public static readonly EffectorValues TIER2 = new EffectorValues
			{
				amount = 65,
				radius = 10
			};

			// Token: 0x04009AFC RID: 39676
			public static readonly EffectorValues TIER3 = new EffectorValues
			{
				amount = 75,
				radius = 15
			};

			// Token: 0x04009AFD RID: 39677
			public static readonly EffectorValues TIER4 = new EffectorValues
			{
				amount = 90,
				radius = 15
			};

			// Token: 0x04009AFE RID: 39678
			public static readonly EffectorValues TIER5 = new EffectorValues
			{
				amount = 105,
				radius = 20
			};

			// Token: 0x04009AFF RID: 39679
			public static readonly EffectorValues TIER6 = new EffectorValues
			{
				amount = 125,
				radius = 20
			};
		}

		// Token: 0x020022A9 RID: 8873
		public class CREATURES
		{
			// Token: 0x04009B00 RID: 39680
			public static readonly EffectorValues TIER0 = new EffectorValues
			{
				amount = 30,
				radius = 5
			};

			// Token: 0x04009B01 RID: 39681
			public static readonly EffectorValues TIER1 = new EffectorValues
			{
				amount = 35,
				radius = 5
			};

			// Token: 0x04009B02 RID: 39682
			public static readonly EffectorValues TIER2 = new EffectorValues
			{
				amount = 45,
				radius = 5
			};

			// Token: 0x04009B03 RID: 39683
			public static readonly EffectorValues TIER3 = new EffectorValues
			{
				amount = 55,
				radius = 5
			};

			// Token: 0x04009B04 RID: 39684
			public static readonly EffectorValues TIER4 = new EffectorValues
			{
				amount = 65,
				radius = 5
			};

			// Token: 0x04009B05 RID: 39685
			public static readonly EffectorValues TIER5 = new EffectorValues
			{
				amount = 75,
				radius = 5
			};

			// Token: 0x04009B06 RID: 39686
			public static readonly EffectorValues TIER6 = new EffectorValues
			{
				amount = 90,
				radius = 10
			};

			// Token: 0x04009B07 RID: 39687
			public static readonly EffectorValues TIER7 = new EffectorValues
			{
				amount = 105,
				radius = 10
			};
		}

		// Token: 0x020022AA RID: 8874
		public class DAMPEN
		{
			// Token: 0x04009B08 RID: 39688
			public static readonly EffectorValues TIER0 = new EffectorValues
			{
				amount = -5,
				radius = 1
			};

			// Token: 0x04009B09 RID: 39689
			public static readonly EffectorValues TIER1 = new EffectorValues
			{
				amount = -10,
				radius = 2
			};

			// Token: 0x04009B0A RID: 39690
			public static readonly EffectorValues TIER2 = new EffectorValues
			{
				amount = -15,
				radius = 3
			};

			// Token: 0x04009B0B RID: 39691
			public static readonly EffectorValues TIER3 = new EffectorValues
			{
				amount = -20,
				radius = 4
			};

			// Token: 0x04009B0C RID: 39692
			public static readonly EffectorValues TIER4 = new EffectorValues
			{
				amount = -20,
				radius = 5
			};

			// Token: 0x04009B0D RID: 39693
			public static readonly EffectorValues TIER5 = new EffectorValues
			{
				amount = -25,
				radius = 6
			};
		}
	}
}
