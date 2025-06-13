using System;

namespace Klei.AI
{
	public class FertilityModifier : Resource
	{
		public FertilityModifier(string id, Tag targetTag, string name, string description, Func<string, string> tooltipCB, FertilityModifier.FertilityModFn applyFunction) : base(id, name)
		{
			this.Description = description;
			this.TargetTag = targetTag;
			this.TooltipCB = tooltipCB;
			this.ApplyFunction = applyFunction;
		}

		public string GetTooltip()
		{
			if (this.TooltipCB != null)
			{
				return this.TooltipCB(this.Description);
			}
			return this.Description;
		}

		public string Description;

		public Tag TargetTag;

		public Func<string, string> TooltipCB;

		public FertilityModifier.FertilityModFn ApplyFunction;

Invoke) Token: 0x0600EF0C RID: 61196
		public delegate void FertilityModFn(FertilityMonitor.Instance inst, Tag eggTag);
	}
}
