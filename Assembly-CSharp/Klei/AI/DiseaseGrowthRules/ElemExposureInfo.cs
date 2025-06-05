using System;
using System.Collections.Generic;
using System.IO;

namespace Klei.AI.DiseaseGrowthRules
{
	// Token: 0x02003D03 RID: 15619
	public struct ElemExposureInfo
	{
		// Token: 0x0600EFEA RID: 61418 RVA: 0x00145781 File Offset: 0x00143981
		public void Write(BinaryWriter writer)
		{
			writer.Write(this.populationHalfLife);
		}

		// Token: 0x0600EFEB RID: 61419 RVA: 0x004EB990 File Offset: 0x004E9B90
		public static void SetBulk(ElemExposureInfo[] info, Func<Element, bool> test, ElemExposureInfo settings)
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

		// Token: 0x0600EFEC RID: 61420 RVA: 0x0014578F File Offset: 0x0014398F
		public float CalculateExposureDiseaseCountDelta(int disease_count, float dt)
		{
			return (Disease.HalfLifeToGrowthRate(this.populationHalfLife, dt) - 1f) * (float)disease_count;
		}

		// Token: 0x0400EB7D RID: 60285
		public float populationHalfLife;
	}
}
