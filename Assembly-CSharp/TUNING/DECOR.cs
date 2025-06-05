using System;
using STRINGS;

namespace TUNING
{
	// Token: 0x020022A2 RID: 8866
	public class DECOR
	{
		// Token: 0x04009AD9 RID: 39641
		public static int LIT_BONUS = 15;

		// Token: 0x04009ADA RID: 39642
		public static readonly EffectorValues NONE = new EffectorValues
		{
			amount = 0,
			radius = 0
		};

		// Token: 0x020022A3 RID: 8867
		public class BONUS
		{
			// Token: 0x04009ADB RID: 39643
			public static readonly EffectorValues TIER0 = new EffectorValues
			{
				amount = 10,
				radius = 1
			};

			// Token: 0x04009ADC RID: 39644
			public static readonly EffectorValues TIER1 = new EffectorValues
			{
				amount = 15,
				radius = 2
			};

			// Token: 0x04009ADD RID: 39645
			public static readonly EffectorValues TIER2 = new EffectorValues
			{
				amount = 20,
				radius = 3
			};

			// Token: 0x04009ADE RID: 39646
			public static readonly EffectorValues TIER3 = new EffectorValues
			{
				amount = 25,
				radius = 4
			};

			// Token: 0x04009ADF RID: 39647
			public static readonly EffectorValues TIER4 = new EffectorValues
			{
				amount = 30,
				radius = 5
			};

			// Token: 0x04009AE0 RID: 39648
			public static readonly EffectorValues TIER5 = new EffectorValues
			{
				amount = 35,
				radius = 6
			};

			// Token: 0x04009AE1 RID: 39649
			public static readonly EffectorValues TIER6 = new EffectorValues
			{
				amount = 50,
				radius = 7
			};

			// Token: 0x04009AE2 RID: 39650
			public static readonly EffectorValues TIER7 = new EffectorValues
			{
				amount = 80,
				radius = 7
			};

			// Token: 0x04009AE3 RID: 39651
			public static readonly EffectorValues TIER8 = new EffectorValues
			{
				amount = 200,
				radius = 8
			};
		}

		// Token: 0x020022A4 RID: 8868
		public class PENALTY
		{
			// Token: 0x04009AE4 RID: 39652
			public static readonly EffectorValues TIER0 = new EffectorValues
			{
				amount = -5,
				radius = 1
			};

			// Token: 0x04009AE5 RID: 39653
			public static readonly EffectorValues TIER1 = new EffectorValues
			{
				amount = -10,
				radius = 2
			};

			// Token: 0x04009AE6 RID: 39654
			public static readonly EffectorValues TIER2 = new EffectorValues
			{
				amount = -15,
				radius = 3
			};

			// Token: 0x04009AE7 RID: 39655
			public static readonly EffectorValues TIER3 = new EffectorValues
			{
				amount = -20,
				radius = 4
			};

			// Token: 0x04009AE8 RID: 39656
			public static readonly EffectorValues TIER4 = new EffectorValues
			{
				amount = -20,
				radius = 5
			};

			// Token: 0x04009AE9 RID: 39657
			public static readonly EffectorValues TIER5 = new EffectorValues
			{
				amount = -25,
				radius = 6
			};
		}

		// Token: 0x020022A5 RID: 8869
		public class SPACEARTIFACT
		{
			// Token: 0x04009AEA RID: 39658
			public static readonly ArtifactTier TIER_NONE = new ArtifactTier(UI.SPACEARTIFACTS.ARTIFACTTIERS.TIER_NONE.key, DECOR.NONE, 0f);

			// Token: 0x04009AEB RID: 39659
			public static readonly ArtifactTier TIER0 = new ArtifactTier(UI.SPACEARTIFACTS.ARTIFACTTIERS.TIER0.key, DECOR.BONUS.TIER0, 0.25f);

			// Token: 0x04009AEC RID: 39660
			public static readonly ArtifactTier TIER1 = new ArtifactTier(UI.SPACEARTIFACTS.ARTIFACTTIERS.TIER1.key, DECOR.BONUS.TIER2, 0.4f);

			// Token: 0x04009AED RID: 39661
			public static readonly ArtifactTier TIER2 = new ArtifactTier(UI.SPACEARTIFACTS.ARTIFACTTIERS.TIER2.key, DECOR.BONUS.TIER4, 0.55f);

			// Token: 0x04009AEE RID: 39662
			public static readonly ArtifactTier TIER3 = new ArtifactTier(UI.SPACEARTIFACTS.ARTIFACTTIERS.TIER3.key, DECOR.BONUS.TIER5, 0.7f);

			// Token: 0x04009AEF RID: 39663
			public static readonly ArtifactTier TIER4 = new ArtifactTier(UI.SPACEARTIFACTS.ARTIFACTTIERS.TIER4.key, DECOR.BONUS.TIER6, 0.85f);

			// Token: 0x04009AF0 RID: 39664
			public static readonly ArtifactTier TIER5 = new ArtifactTier(UI.SPACEARTIFACTS.ARTIFACTTIERS.TIER5.key, DECOR.BONUS.TIER7, 1f);
		}
	}
}
