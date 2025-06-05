using System;

namespace Klei.AI.DiseaseGrowthRules
{
	// Token: 0x02003D05 RID: 15621
	public class ElementExposureRule : ExposureRule
	{
		// Token: 0x0600EFF1 RID: 61425 RVA: 0x001457A6 File Offset: 0x001439A6
		public ElementExposureRule(SimHashes element)
		{
			this.element = element;
		}

		// Token: 0x0600EFF2 RID: 61426 RVA: 0x001457B5 File Offset: 0x001439B5
		public override bool Test(Element e)
		{
			return e.id == this.element;
		}

		// Token: 0x0600EFF3 RID: 61427 RVA: 0x001457C5 File Offset: 0x001439C5
		public override string Name()
		{
			return ElementLoader.FindElementByHash(this.element).name;
		}

		// Token: 0x0400EB7F RID: 60287
		public SimHashes element;
	}
}
