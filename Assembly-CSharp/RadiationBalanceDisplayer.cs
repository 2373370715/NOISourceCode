﻿using System;
using System.Text;
using Klei.AI;
using STRINGS;
using UnityEngine;

public class RadiationBalanceDisplayer : StandardAmountDisplayer
{
	public RadiationBalanceDisplayer() : base(GameUtil.UnitClass.SimpleFloat, GameUtil.TimeSlice.PerCycle, null, GameUtil.IdentityDescriptorTense.Normal)
	{
		this.formatter = new RadiationBalanceDisplayer.RadiationAttributeFormatter();
	}

	public override string GetValueString(Amount master, AmountInstance instance)
	{
		return base.GetValueString(master, instance) + UI.UNITSUFFIXES.RADIATION.RADS;
	}

	public override string GetTooltip(Amount master, AmountInstance instance)
	{
		StringBuilder stringBuilder = GlobalStringBuilderPool.Alloc();
		if (instance.gameObject.GetSMI<RadiationMonitor.Instance>() != null)
		{
			int num = Grid.PosToCell(instance.gameObject);
			if (Grid.IsValidCell(num))
			{
				stringBuilder.Append(DUPLICANTS.STATS.RADIATIONBALANCE.TOOLTIP_CURRENT_BALANCE);
			}
			stringBuilder.Append("\n\n");
			float num2 = Mathf.Clamp01(1f - Db.Get().Attributes.RadiationResistance.Lookup(instance.gameObject).GetTotalValue());
			stringBuilder.AppendFormat(DUPLICANTS.STATS.RADIATIONBALANCE.CURRENT_EXPOSURE, Mathf.RoundToInt(Grid.Radiation[num] * num2));
			stringBuilder.Append("\n");
			stringBuilder.AppendFormat(DUPLICANTS.STATS.RADIATIONBALANCE.CURRENT_REJUVENATION, Mathf.RoundToInt(Db.Get().Attributes.RadiationRecovery.Lookup(instance.gameObject).GetTotalValue() * 600f));
		}
		return GlobalStringBuilderPool.ReturnAndFree(stringBuilder);
	}

	public class RadiationAttributeFormatter : StandardAttributeFormatter
	{
		public RadiationAttributeFormatter() : base(GameUtil.UnitClass.SimpleFloat, GameUtil.TimeSlice.PerCycle)
		{
		}
	}
}
