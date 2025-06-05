using System;

namespace Klei.AI.DiseaseGrowthRules
{
	// Token: 0x02003D02 RID: 15618
	public class CompositeGrowthRule
	{
		// Token: 0x0600EFE6 RID: 61414 RVA: 0x00145779 File Offset: 0x00143979
		public string Name()
		{
			return this.name;
		}

		// Token: 0x0600EFE7 RID: 61415 RVA: 0x004EB840 File Offset: 0x004E9A40
		public void Overlay(GrowthRule rule)
		{
			if (rule.underPopulationDeathRate != null)
			{
				this.underPopulationDeathRate = rule.underPopulationDeathRate.Value;
			}
			if (rule.populationHalfLife != null)
			{
				this.populationHalfLife = rule.populationHalfLife.Value;
			}
			if (rule.overPopulationHalfLife != null)
			{
				this.overPopulationHalfLife = rule.overPopulationHalfLife.Value;
			}
			if (rule.diffusionScale != null)
			{
				this.diffusionScale = rule.diffusionScale.Value;
			}
			if (rule.minCountPerKG != null)
			{
				this.minCountPerKG = rule.minCountPerKG.Value;
			}
			if (rule.maxCountPerKG != null)
			{
				this.maxCountPerKG = rule.maxCountPerKG.Value;
			}
			if (rule.minDiffusionCount != null)
			{
				this.minDiffusionCount = rule.minDiffusionCount.Value;
			}
			if (rule.minDiffusionInfestationTickCount != null)
			{
				this.minDiffusionInfestationTickCount = rule.minDiffusionInfestationTickCount.Value;
			}
			this.name = rule.Name();
		}

		// Token: 0x0600EFE8 RID: 61416 RVA: 0x004EB950 File Offset: 0x004E9B50
		public float GetHalfLifeForCount(int count, float kg)
		{
			int num = (int)(this.minCountPerKG * kg);
			int num2 = (int)(this.maxCountPerKG * kg);
			if (count < num)
			{
				return this.populationHalfLife;
			}
			if (count < num2)
			{
				return this.populationHalfLife;
			}
			return this.overPopulationHalfLife;
		}

		// Token: 0x0400EB74 RID: 60276
		public string name;

		// Token: 0x0400EB75 RID: 60277
		public float underPopulationDeathRate;

		// Token: 0x0400EB76 RID: 60278
		public float populationHalfLife;

		// Token: 0x0400EB77 RID: 60279
		public float overPopulationHalfLife;

		// Token: 0x0400EB78 RID: 60280
		public float diffusionScale;

		// Token: 0x0400EB79 RID: 60281
		public float minCountPerKG;

		// Token: 0x0400EB7A RID: 60282
		public float maxCountPerKG;

		// Token: 0x0400EB7B RID: 60283
		public int minDiffusionCount;

		// Token: 0x0400EB7C RID: 60284
		public byte minDiffusionInfestationTickCount;
	}
}
