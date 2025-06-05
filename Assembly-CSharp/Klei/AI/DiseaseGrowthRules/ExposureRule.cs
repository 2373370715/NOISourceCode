using System;
using System.Collections.Generic;

namespace Klei.AI.DiseaseGrowthRules
{
	// Token: 0x02003D04 RID: 15620
	public class ExposureRule
	{
		// Token: 0x0600EFED RID: 61421 RVA: 0x004EB9CC File Offset: 0x004E9BCC
		public void Apply(ElemExposureInfo[] infoList)
		{
			List<Element> elements = ElementLoader.elements;
			for (int i = 0; i < elements.Count; i++)
			{
				if (this.Test(elements[i]))
				{
					ElemExposureInfo elemExposureInfo = infoList[i];
					if (this.populationHalfLife != null)
					{
						elemExposureInfo.populationHalfLife = this.populationHalfLife.Value;
					}
					infoList[i] = elemExposureInfo;
				}
			}
		}

		// Token: 0x0600EFEE RID: 61422 RVA: 0x000AA7E7 File Offset: 0x000A89E7
		public virtual bool Test(Element e)
		{
			return true;
		}

		// Token: 0x0600EFEF RID: 61423 RVA: 0x000AA765 File Offset: 0x000A8965
		public virtual string Name()
		{
			return null;
		}

		// Token: 0x0400EB7E RID: 60286
		public float? populationHalfLife;
	}
}
