using System;

namespace TUNING
{
	// Token: 0x02002289 RID: 8841
	public class FIXEDTRAITS
	{
		// Token: 0x0200228A RID: 8842
		public class NORTHERNLIGHTS
		{
			// Token: 0x04009A47 RID: 39495
			public static int NONE = 0;

			// Token: 0x04009A48 RID: 39496
			public static int ENABLED = 1;

			// Token: 0x04009A49 RID: 39497
			public static int DEFAULT_VALUE = FIXEDTRAITS.NORTHERNLIGHTS.NONE;

			// Token: 0x0200228B RID: 8843
			public class NAME
			{
				// Token: 0x04009A4A RID: 39498
				public static string NONE = "northernLightsNone";

				// Token: 0x04009A4B RID: 39499
				public static string ENABLED = "northernLightsOn";

				// Token: 0x04009A4C RID: 39500
				public static string DEFAULT = FIXEDTRAITS.NORTHERNLIGHTS.NAME.NONE;
			}
		}

		// Token: 0x0200228C RID: 8844
		public class SUNLIGHT
		{
			// Token: 0x04009A4D RID: 39501
			public static int DEFAULT_SPACED_OUT_SUNLIGHT = 40000;

			// Token: 0x04009A4E RID: 39502
			public static int NONE = 0;

			// Token: 0x04009A4F RID: 39503
			public static int VERY_VERY_LOW = (int)((float)FIXEDTRAITS.SUNLIGHT.DEFAULT_SPACED_OUT_SUNLIGHT * 0.25f);

			// Token: 0x04009A50 RID: 39504
			public static int VERY_LOW = (int)((float)FIXEDTRAITS.SUNLIGHT.DEFAULT_SPACED_OUT_SUNLIGHT * 0.5f);

			// Token: 0x04009A51 RID: 39505
			public static int LOW = (int)((float)FIXEDTRAITS.SUNLIGHT.DEFAULT_SPACED_OUT_SUNLIGHT * 0.75f);

			// Token: 0x04009A52 RID: 39506
			public static int MED_LOW = (int)((float)FIXEDTRAITS.SUNLIGHT.DEFAULT_SPACED_OUT_SUNLIGHT * 0.875f);

			// Token: 0x04009A53 RID: 39507
			public static int MED = FIXEDTRAITS.SUNLIGHT.DEFAULT_SPACED_OUT_SUNLIGHT;

			// Token: 0x04009A54 RID: 39508
			public static int MED_HIGH = (int)((float)FIXEDTRAITS.SUNLIGHT.DEFAULT_SPACED_OUT_SUNLIGHT * 1.25f);

			// Token: 0x04009A55 RID: 39509
			public static int HIGH = (int)((float)FIXEDTRAITS.SUNLIGHT.DEFAULT_SPACED_OUT_SUNLIGHT * 1.5f);

			// Token: 0x04009A56 RID: 39510
			public static int VERY_HIGH = FIXEDTRAITS.SUNLIGHT.DEFAULT_SPACED_OUT_SUNLIGHT * 2;

			// Token: 0x04009A57 RID: 39511
			public static int VERY_VERY_HIGH = (int)((float)FIXEDTRAITS.SUNLIGHT.DEFAULT_SPACED_OUT_SUNLIGHT * 2.5f);

			// Token: 0x04009A58 RID: 39512
			public static int VERY_VERY_VERY_HIGH = FIXEDTRAITS.SUNLIGHT.DEFAULT_SPACED_OUT_SUNLIGHT * 3;

			// Token: 0x04009A59 RID: 39513
			public static int DEFAULT_VALUE = FIXEDTRAITS.SUNLIGHT.VERY_HIGH;

			// Token: 0x0200228D RID: 8845
			public class NAME
			{
				// Token: 0x04009A5A RID: 39514
				public static string NONE = "sunlightNone";

				// Token: 0x04009A5B RID: 39515
				public static string VERY_VERY_LOW = "sunlightVeryVeryLow";

				// Token: 0x04009A5C RID: 39516
				public static string VERY_LOW = "sunlightVeryLow";

				// Token: 0x04009A5D RID: 39517
				public static string LOW = "sunlightLow";

				// Token: 0x04009A5E RID: 39518
				public static string MED_LOW = "sunlightMedLow";

				// Token: 0x04009A5F RID: 39519
				public static string MED = "sunlightMed";

				// Token: 0x04009A60 RID: 39520
				public static string MED_HIGH = "sunlightMedHigh";

				// Token: 0x04009A61 RID: 39521
				public static string HIGH = "sunlightHigh";

				// Token: 0x04009A62 RID: 39522
				public static string VERY_HIGH = "sunlightVeryHigh";

				// Token: 0x04009A63 RID: 39523
				public static string VERY_VERY_HIGH = "sunlightVeryVeryHigh";

				// Token: 0x04009A64 RID: 39524
				public static string VERY_VERY_VERY_HIGH = "sunlightVeryVeryVeryHigh";

				// Token: 0x04009A65 RID: 39525
				public static string DEFAULT = FIXEDTRAITS.SUNLIGHT.NAME.VERY_HIGH;
			}
		}

		// Token: 0x0200228E RID: 8846
		public class COSMICRADIATION
		{
			// Token: 0x04009A66 RID: 39526
			public static int BASELINE = 250;

			// Token: 0x04009A67 RID: 39527
			public static int NONE = 0;

			// Token: 0x04009A68 RID: 39528
			public static int VERY_VERY_LOW = (int)((float)FIXEDTRAITS.COSMICRADIATION.BASELINE * 0.25f);

			// Token: 0x04009A69 RID: 39529
			public static int VERY_LOW = (int)((float)FIXEDTRAITS.COSMICRADIATION.BASELINE * 0.5f);

			// Token: 0x04009A6A RID: 39530
			public static int LOW = (int)((float)FIXEDTRAITS.COSMICRADIATION.BASELINE * 0.75f);

			// Token: 0x04009A6B RID: 39531
			public static int MED_LOW = (int)((float)FIXEDTRAITS.COSMICRADIATION.BASELINE * 0.875f);

			// Token: 0x04009A6C RID: 39532
			public static int MED = FIXEDTRAITS.COSMICRADIATION.BASELINE;

			// Token: 0x04009A6D RID: 39533
			public static int MED_HIGH = (int)((float)FIXEDTRAITS.COSMICRADIATION.BASELINE * 1.25f);

			// Token: 0x04009A6E RID: 39534
			public static int HIGH = (int)((float)FIXEDTRAITS.COSMICRADIATION.BASELINE * 1.5f);

			// Token: 0x04009A6F RID: 39535
			public static int VERY_HIGH = FIXEDTRAITS.COSMICRADIATION.BASELINE * 2;

			// Token: 0x04009A70 RID: 39536
			public static int VERY_VERY_HIGH = FIXEDTRAITS.COSMICRADIATION.BASELINE * 3;

			// Token: 0x04009A71 RID: 39537
			public static int DEFAULT_VALUE = FIXEDTRAITS.COSMICRADIATION.MED;

			// Token: 0x04009A72 RID: 39538
			public static float TELESCOPE_RADIATION_SHIELDING = 0.5f;

			// Token: 0x0200228F RID: 8847
			public class NAME
			{
				// Token: 0x04009A73 RID: 39539
				public static string NONE = "cosmicRadiationNone";

				// Token: 0x04009A74 RID: 39540
				public static string VERY_VERY_LOW = "cosmicRadiationVeryVeryLow";

				// Token: 0x04009A75 RID: 39541
				public static string VERY_LOW = "cosmicRadiationVeryLow";

				// Token: 0x04009A76 RID: 39542
				public static string LOW = "cosmicRadiationLow";

				// Token: 0x04009A77 RID: 39543
				public static string MED_LOW = "cosmicRadiationMedLow";

				// Token: 0x04009A78 RID: 39544
				public static string MED = "cosmicRadiationMed";

				// Token: 0x04009A79 RID: 39545
				public static string MED_HIGH = "cosmicRadiationMedHigh";

				// Token: 0x04009A7A RID: 39546
				public static string HIGH = "cosmicRadiationHigh";

				// Token: 0x04009A7B RID: 39547
				public static string VERY_HIGH = "cosmicRadiationVeryHigh";

				// Token: 0x04009A7C RID: 39548
				public static string VERY_VERY_HIGH = "cosmicRadiationVeryVeryHigh";

				// Token: 0x04009A7D RID: 39549
				public static string DEFAULT = FIXEDTRAITS.COSMICRADIATION.NAME.MED;
			}
		}
	}
}
