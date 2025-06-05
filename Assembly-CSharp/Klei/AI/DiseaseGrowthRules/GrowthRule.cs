using System;
using System.Collections.Generic;

namespace Klei.AI.DiseaseGrowthRules
{
	// Token: 0x02003CFE RID: 15614
	public class GrowthRule
	{
		// Token: 0x0600EFD9 RID: 61401 RVA: 0x004EB6E4 File Offset: 0x004E98E4
		public void Apply(ElemGrowthInfo[] infoList)
		{
			List<Element> elements = ElementLoader.elements;
			for (int i = 0; i < elements.Count; i++)
			{
				Element element = elements[i];
				if (element.id != SimHashes.Vacuum && this.Test(element))
				{
					ElemGrowthInfo elemGrowthInfo = infoList[i];
					if (this.underPopulationDeathRate != null)
					{
						elemGrowthInfo.underPopulationDeathRate = this.underPopulationDeathRate.Value;
					}
					if (this.populationHalfLife != null)
					{
						elemGrowthInfo.populationHalfLife = this.populationHalfLife.Value;
					}
					if (this.overPopulationHalfLife != null)
					{
						elemGrowthInfo.overPopulationHalfLife = this.overPopulationHalfLife.Value;
					}
					if (this.diffusionScale != null)
					{
						elemGrowthInfo.diffusionScale = this.diffusionScale.Value;
					}
					if (this.minCountPerKG != null)
					{
						elemGrowthInfo.minCountPerKG = this.minCountPerKG.Value;
					}
					if (this.maxCountPerKG != null)
					{
						elemGrowthInfo.maxCountPerKG = this.maxCountPerKG.Value;
					}
					if (this.minDiffusionCount != null)
					{
						elemGrowthInfo.minDiffusionCount = this.minDiffusionCount.Value;
					}
					if (this.minDiffusionInfestationTickCount != null)
					{
						elemGrowthInfo.minDiffusionInfestationTickCount = this.minDiffusionInfestationTickCount.Value;
					}
					infoList[i] = elemGrowthInfo;
				}
			}
		}

		// Token: 0x0600EFDA RID: 61402 RVA: 0x000AA7E7 File Offset: 0x000A89E7
		public virtual bool Test(Element e)
		{
			return true;
		}

		// Token: 0x0600EFDB RID: 61403 RVA: 0x000AA765 File Offset: 0x000A8965
		public virtual string Name()
		{
			return null;
		}

		// Token: 0x0400EB69 RID: 60265
		public float? underPopulationDeathRate;

		// Token: 0x0400EB6A RID: 60266
		public float? populationHalfLife;

		// Token: 0x0400EB6B RID: 60267
		public float? overPopulationHalfLife;

		// Token: 0x0400EB6C RID: 60268
		public float? diffusionScale;

		// Token: 0x0400EB6D RID: 60269
		public float? minCountPerKG;

		// Token: 0x0400EB6E RID: 60270
		public float? maxCountPerKG;

		// Token: 0x0400EB6F RID: 60271
		public int? minDiffusionCount;

		// Token: 0x0400EB70 RID: 60272
		public byte? minDiffusionInfestationTickCount;
	}
}
