using System;
using Klei.AI;
using STRINGS;

// Token: 0x02001C3B RID: 7227
public class MaturityDisplayer : AsPercentAmountDisplayer
{
	// Token: 0x06009646 RID: 38470 RVA: 0x001065E5 File Offset: 0x001047E5
	public MaturityDisplayer() : base(GameUtil.TimeSlice.PerCycle)
	{
		this.formatter = new MaturityDisplayer.MaturityAttributeFormatter();
	}

	// Token: 0x06009647 RID: 38471 RVA: 0x003ABD48 File Offset: 0x003A9F48
	public override string GetTooltipDescription(Amount master, AmountInstance instance)
	{
		string text = base.GetTooltipDescription(master, instance);
		Growing component = instance.gameObject.GetComponent<Growing>();
		if (component.IsGrowing())
		{
			float seconds = (instance.GetMax() - instance.value) / instance.GetDelta();
			if (component != null && component.IsGrowing())
			{
				text += string.Format(CREATURES.STATS.MATURITY.TOOLTIP_GROWING_CROP, GameUtil.GetFormattedCycles(seconds, "F1", false), GameUtil.GetFormattedCycles(component.TimeUntilNextHarvest(), "F1", false));
			}
			else
			{
				text += string.Format(CREATURES.STATS.MATURITY.TOOLTIP_GROWING, GameUtil.GetFormattedCycles(seconds, "F1", false));
			}
		}
		else if (component.ReachedNextHarvest())
		{
			text += CREATURES.STATS.MATURITY.TOOLTIP_GROWN;
		}
		else
		{
			text += CREATURES.STATS.MATURITY.TOOLTIP_STALLED;
		}
		return text;
	}

	// Token: 0x06009648 RID: 38472 RVA: 0x003ABE20 File Offset: 0x003AA020
	public override string GetDescription(Amount master, AmountInstance instance)
	{
		Growing component = instance.gameObject.GetComponent<Growing>();
		if (component != null && component.IsGrowing())
		{
			return string.Format(CREATURES.STATS.MATURITY.AMOUNT_DESC_FMT, master.Name, this.formatter.GetFormattedValue(base.ToPercent(instance.value, instance), GameUtil.TimeSlice.None), GameUtil.GetFormattedCycles(component.TimeUntilNextHarvest(), "F1", false));
		}
		return base.GetDescription(master, instance);
	}

	// Token: 0x02001C3C RID: 7228
	public class MaturityAttributeFormatter : StandardAttributeFormatter
	{
		// Token: 0x06009649 RID: 38473 RVA: 0x001065F9 File Offset: 0x001047F9
		public MaturityAttributeFormatter() : base(GameUtil.UnitClass.Percent, GameUtil.TimeSlice.None)
		{
		}

		// Token: 0x0600964A RID: 38474 RVA: 0x003ABE94 File Offset: 0x003AA094
		public override string GetFormattedModifier(AttributeModifier modifier)
		{
			float num = modifier.Value;
			GameUtil.TimeSlice timeSlice = base.DeltaTimeSlice;
			if (modifier.IsMultiplier)
			{
				num *= 100f;
				timeSlice = GameUtil.TimeSlice.None;
			}
			return this.GetFormattedValue(num, timeSlice);
		}
	}
}
