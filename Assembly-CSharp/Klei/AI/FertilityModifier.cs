using System;

namespace Klei.AI
{
	// Token: 0x02003CDA RID: 15578
	public class FertilityModifier : Resource
	{
		// Token: 0x0600EF09 RID: 61193 RVA: 0x00144DC6 File Offset: 0x00142FC6
		public FertilityModifier(string id, Tag targetTag, string name, string description, Func<string, string> tooltipCB, FertilityModifier.FertilityModFn applyFunction) : base(id, name)
		{
			this.Description = description;
			this.TargetTag = targetTag;
			this.TooltipCB = tooltipCB;
			this.ApplyFunction = applyFunction;
		}

		// Token: 0x0600EF0A RID: 61194 RVA: 0x00144DEF File Offset: 0x00142FEF
		public string GetTooltip()
		{
			if (this.TooltipCB != null)
			{
				return this.TooltipCB(this.Description);
			}
			return this.Description;
		}

		// Token: 0x0400EAC7 RID: 60103
		public string Description;

		// Token: 0x0400EAC8 RID: 60104
		public Tag TargetTag;

		// Token: 0x0400EAC9 RID: 60105
		public Func<string, string> TooltipCB;

		// Token: 0x0400EACA RID: 60106
		public FertilityModifier.FertilityModFn ApplyFunction;

		// Token: 0x02003CDB RID: 15579
		// (Invoke) Token: 0x0600EF0C RID: 61196
		public delegate void FertilityModFn(FertilityMonitor.Instance inst, Tag eggTag);
	}
}
