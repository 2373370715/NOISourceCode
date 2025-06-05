using System;
using System.Collections.Generic;
using System.IO;

namespace Klei.AI.DiseaseGrowthRules
{
	// Token: 0x02003CFD RID: 15613
	public struct ElemGrowthInfo
	{
		// Token: 0x0600EFD6 RID: 61398 RVA: 0x004EB5CC File Offset: 0x004E97CC
		public void Write(BinaryWriter writer)
		{
			writer.Write(this.underPopulationDeathRate);
			writer.Write(this.populationHalfLife);
			writer.Write(this.overPopulationHalfLife);
			writer.Write(this.diffusionScale);
			writer.Write(this.minCountPerKG);
			writer.Write(this.maxCountPerKG);
			writer.Write(this.minDiffusionCount);
			writer.Write(this.minDiffusionInfestationTickCount);
		}

		// Token: 0x0600EFD7 RID: 61399 RVA: 0x004EB63C File Offset: 0x004E983C
		public static void SetBulk(ElemGrowthInfo[] info, Func<Element, bool> test, ElemGrowthInfo settings)
		{
			List<Element> elements = ElementLoader.elements;
			for (int i = 0; i < elements.Count; i++)
			{
				if (test(elements[i]))
				{
					info[i] = settings;
				}
			}
		}

		// Token: 0x0600EFD8 RID: 61400 RVA: 0x004EB678 File Offset: 0x004E9878
		public float CalculateDiseaseCountDelta(int disease_count, float kg, float dt)
		{
			float num = this.minCountPerKG * kg;
			float num2 = this.maxCountPerKG * kg;
			float result;
			if (num <= (float)disease_count && (float)disease_count <= num2)
			{
				result = (Disease.HalfLifeToGrowthRate(this.populationHalfLife, dt) - 1f) * (float)disease_count;
			}
			else if ((float)disease_count < num)
			{
				result = -this.underPopulationDeathRate * dt;
			}
			else
			{
				result = (Disease.HalfLifeToGrowthRate(this.overPopulationHalfLife, dt) - 1f) * (float)disease_count;
			}
			return result;
		}

		// Token: 0x0400EB61 RID: 60257
		public float underPopulationDeathRate;

		// Token: 0x0400EB62 RID: 60258
		public float populationHalfLife;

		// Token: 0x0400EB63 RID: 60259
		public float overPopulationHalfLife;

		// Token: 0x0400EB64 RID: 60260
		public float diffusionScale;

		// Token: 0x0400EB65 RID: 60261
		public float minCountPerKG;

		// Token: 0x0400EB66 RID: 60262
		public float maxCountPerKG;

		// Token: 0x0400EB67 RID: 60263
		public int minDiffusionCount;

		// Token: 0x0400EB68 RID: 60264
		public byte minDiffusionInfestationTickCount;
	}
}
