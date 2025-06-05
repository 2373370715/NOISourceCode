using System;

namespace Klei.AI.DiseaseGrowthRules
{
	// Token: 0x02003D01 RID: 15617
	public class TagGrowthRule : GrowthRule
	{
		// Token: 0x0600EFE3 RID: 61411 RVA: 0x0014574F File Offset: 0x0014394F
		public TagGrowthRule(Tag tag)
		{
			this.tag = tag;
		}

		// Token: 0x0600EFE4 RID: 61412 RVA: 0x0014575E File Offset: 0x0014395E
		public override bool Test(Element e)
		{
			return e.HasTag(this.tag);
		}

		// Token: 0x0600EFE5 RID: 61413 RVA: 0x0014576C File Offset: 0x0014396C
		public override string Name()
		{
			return this.tag.ProperName();
		}

		// Token: 0x0400EB73 RID: 60275
		public Tag tag;
	}
}
