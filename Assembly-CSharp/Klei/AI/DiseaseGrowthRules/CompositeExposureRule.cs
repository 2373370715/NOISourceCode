using System;

namespace Klei.AI.DiseaseGrowthRules
{
	// Token: 0x02003D06 RID: 15622
	public class CompositeExposureRule
	{
		// Token: 0x0600EFF4 RID: 61428 RVA: 0x001457D7 File Offset: 0x001439D7
		public string Name()
		{
			return this.name;
		}

		// Token: 0x0600EFF5 RID: 61429 RVA: 0x001457DF File Offset: 0x001439DF
		public void Overlay(ExposureRule rule)
		{
			if (rule.populationHalfLife != null)
			{
				this.populationHalfLife = rule.populationHalfLife.Value;
			}
			this.name = rule.Name();
		}

		// Token: 0x0600EFF6 RID: 61430 RVA: 0x0014580C File Offset: 0x00143A0C
		public float GetHalfLifeForCount(int count)
		{
			return this.populationHalfLife;
		}

		// Token: 0x0400EB80 RID: 60288
		public string name;

		// Token: 0x0400EB81 RID: 60289
		public float populationHalfLife;
	}
}
