using System;
using Klei.AI;

// Token: 0x02001C35 RID: 7221
public class CaloriesDisplayer : StandardAmountDisplayer
{
	// Token: 0x0600963C RID: 38460 RVA: 0x0010654E File Offset: 0x0010474E
	public CaloriesDisplayer() : base(GameUtil.UnitClass.Calories, GameUtil.TimeSlice.PerCycle, null, GameUtil.IdentityDescriptorTense.Normal)
	{
		this.formatter = new CaloriesDisplayer.CaloriesAttributeFormatter();
	}

	// Token: 0x02001C36 RID: 7222
	public class CaloriesAttributeFormatter : StandardAttributeFormatter
	{
		// Token: 0x0600963D RID: 38461 RVA: 0x00106565 File Offset: 0x00104765
		public CaloriesAttributeFormatter() : base(GameUtil.UnitClass.Calories, GameUtil.TimeSlice.PerCycle)
		{
		}

		// Token: 0x0600963E RID: 38462 RVA: 0x0010656F File Offset: 0x0010476F
		public override string GetFormattedModifier(AttributeModifier modifier)
		{
			if (modifier.IsMultiplier)
			{
				return GameUtil.GetFormattedPercent(-modifier.Value * 100f, GameUtil.TimeSlice.None);
			}
			return base.GetFormattedModifier(modifier);
		}
	}
}
