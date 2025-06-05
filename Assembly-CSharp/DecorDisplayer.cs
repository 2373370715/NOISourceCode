using System;
using Klei.AI;
using STRINGS;

// Token: 0x02001C39 RID: 7225
public class DecorDisplayer : StandardAmountDisplayer
{
	// Token: 0x06009643 RID: 38467 RVA: 0x001065CE File Offset: 0x001047CE
	public DecorDisplayer() : base(GameUtil.UnitClass.SimpleFloat, GameUtil.TimeSlice.PerCycle, null, GameUtil.IdentityDescriptorTense.Normal)
	{
		this.formatter = new DecorDisplayer.DecorAttributeFormatter();
	}

	// Token: 0x06009644 RID: 38468 RVA: 0x003ABC74 File Offset: 0x003A9E74
	public override string GetTooltip(Amount master, AmountInstance instance)
	{
		string text = string.Format(LocText.ParseText(master.description), this.formatter.GetFormattedValue(instance.value, GameUtil.TimeSlice.None));
		int cell = Grid.PosToCell(instance.gameObject);
		if (Grid.IsValidCell(cell))
		{
			text += string.Format(DUPLICANTS.STATS.DECOR.TOOLTIP_CURRENT, GameUtil.GetDecorAtCell(cell));
		}
		text += "\n";
		DecorMonitor.Instance smi = instance.gameObject.GetSMI<DecorMonitor.Instance>();
		if (smi != null)
		{
			text += string.Format(DUPLICANTS.STATS.DECOR.TOOLTIP_AVERAGE_TODAY, this.formatter.GetFormattedValue(smi.GetTodaysAverageDecor(), GameUtil.TimeSlice.None));
			text += string.Format(DUPLICANTS.STATS.DECOR.TOOLTIP_AVERAGE_YESTERDAY, this.formatter.GetFormattedValue(smi.GetYesterdaysAverageDecor(), GameUtil.TimeSlice.None));
		}
		return text;
	}

	// Token: 0x02001C3A RID: 7226
	public class DecorAttributeFormatter : StandardAttributeFormatter
	{
		// Token: 0x06009645 RID: 38469 RVA: 0x001065C4 File Offset: 0x001047C4
		public DecorAttributeFormatter() : base(GameUtil.UnitClass.SimpleFloat, GameUtil.TimeSlice.PerCycle)
		{
		}
	}
}
