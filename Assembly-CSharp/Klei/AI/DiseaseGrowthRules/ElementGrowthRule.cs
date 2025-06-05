using System;

namespace Klei.AI.DiseaseGrowthRules
{
	// Token: 0x02003D00 RID: 15616
	public class ElementGrowthRule : GrowthRule
	{
		// Token: 0x0600EFE0 RID: 61408 RVA: 0x0014571E File Offset: 0x0014391E
		public ElementGrowthRule(SimHashes element)
		{
			this.element = element;
		}

		// Token: 0x0600EFE1 RID: 61409 RVA: 0x0014572D File Offset: 0x0014392D
		public override bool Test(Element e)
		{
			return e.id == this.element;
		}

		// Token: 0x0600EFE2 RID: 61410 RVA: 0x0014573D File Offset: 0x0014393D
		public override string Name()
		{
			return ElementLoader.FindElementByHash(this.element).name;
		}

		// Token: 0x0400EB72 RID: 60274
		public SimHashes element;
	}
}
