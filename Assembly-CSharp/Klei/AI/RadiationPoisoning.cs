using System;
using Klei.AI.DiseaseGrowthRules;

namespace Klei.AI
{
	// Token: 0x02003C72 RID: 15474
	public class RadiationPoisoning : Disease
	{
		// Token: 0x0600ED70 RID: 60784 RVA: 0x004E1BD8 File Offset: 0x004DFDD8
		public RadiationPoisoning(bool statsOnly) : base("RadiationSickness", 100f, Disease.RangeInfo.Idempotent(), Disease.RangeInfo.Idempotent(), Disease.RangeInfo.Idempotent(), Disease.RangeInfo.Idempotent(), 0f, statsOnly)
		{
		}

		// Token: 0x0600ED71 RID: 60785 RVA: 0x004E1C10 File Offset: 0x004DFE10
		protected override void PopulateElemGrowthInfo()
		{
			base.InitializeElemGrowthArray(ref this.elemGrowthInfo, Disease.DEFAULT_GROWTH_INFO);
			base.AddGrowthRule(new GrowthRule
			{
				underPopulationDeathRate = new float?(0f),
				minCountPerKG = new float?(0f),
				populationHalfLife = new float?(600f),
				maxCountPerKG = new float?(float.PositiveInfinity),
				overPopulationHalfLife = new float?(600f),
				minDiffusionCount = new int?(10000),
				diffusionScale = new float?(0f),
				minDiffusionInfestationTickCount = new byte?((byte)1)
			});
			base.InitializeElemExposureArray(ref this.elemExposureInfo, Disease.DEFAULT_EXPOSURE_INFO);
		}

		// Token: 0x0400E979 RID: 59769
		public const string ID = "RadiationSickness";
	}
}
