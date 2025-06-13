﻿using System;
using System.Text;
using Klei.AI;
using STRINGS;

public class DecorDisplayer : StandardAmountDisplayer
{
	public DecorDisplayer() : base(GameUtil.UnitClass.SimpleFloat, GameUtil.TimeSlice.PerCycle, null, GameUtil.IdentityDescriptorTense.Normal)
	{
		this.formatter = new DecorDisplayer.DecorAttributeFormatter();
	}

	public override string GetTooltip(Amount master, AmountInstance instance)
	{
		string format = LocText.ParseText(master.description);
		StringBuilder stringBuilder = GlobalStringBuilderPool.Alloc();
		stringBuilder.AppendFormat(format, this.formatter.GetFormattedValue(instance.value, GameUtil.TimeSlice.None));
		int cell = Grid.PosToCell(instance.gameObject);
		if (Grid.IsValidCell(cell))
		{
			stringBuilder.Append(string.Format(DUPLICANTS.STATS.DECOR.TOOLTIP_CURRENT, GameUtil.GetDecorAtCell(cell)));
		}
		stringBuilder.Append("\n");
		DecorMonitor.Instance smi = instance.gameObject.GetSMI<DecorMonitor.Instance>();
		if (smi != null)
		{
			stringBuilder.AppendFormat(DUPLICANTS.STATS.DECOR.TOOLTIP_AVERAGE_TODAY, this.formatter.GetFormattedValue(smi.GetTodaysAverageDecor(), GameUtil.TimeSlice.None));
			stringBuilder.AppendFormat(DUPLICANTS.STATS.DECOR.TOOLTIP_AVERAGE_YESTERDAY, this.formatter.GetFormattedValue(smi.GetYesterdaysAverageDecor(), GameUtil.TimeSlice.None));
		}
		return GlobalStringBuilderPool.ReturnAndFree(stringBuilder);
	}

	public class DecorAttributeFormatter : StandardAttributeFormatter
	{
		public DecorAttributeFormatter() : base(GameUtil.UnitClass.SimpleFloat, GameUtil.TimeSlice.PerCycle)
		{
		}
	}
}
