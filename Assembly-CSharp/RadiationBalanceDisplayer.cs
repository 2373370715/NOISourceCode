using System;
using Klei.AI;
using STRINGS;
using UnityEngine;

// Token: 0x02001C37 RID: 7223
public class RadiationBalanceDisplayer : StandardAmountDisplayer
{
	// Token: 0x0600963F RID: 38463 RVA: 0x00106594 File Offset: 0x00104794
	public RadiationBalanceDisplayer() : base(GameUtil.UnitClass.SimpleFloat, GameUtil.TimeSlice.PerCycle, null, GameUtil.IdentityDescriptorTense.Normal)
	{
		this.formatter = new RadiationBalanceDisplayer.RadiationAttributeFormatter();
	}

	// Token: 0x06009640 RID: 38464 RVA: 0x001065AB File Offset: 0x001047AB
	public override string GetValueString(Amount master, AmountInstance instance)
	{
		return base.GetValueString(master, instance) + UI.UNITSUFFIXES.RADIATION.RADS;
	}

	// Token: 0x06009641 RID: 38465 RVA: 0x003ABB74 File Offset: 0x003A9D74
	public override string GetTooltip(Amount master, AmountInstance instance)
	{
		string text = "";
		if (instance.gameObject.GetSMI<RadiationMonitor.Instance>() != null)
		{
			int num = Grid.PosToCell(instance.gameObject);
			if (Grid.IsValidCell(num))
			{
				text += DUPLICANTS.STATS.RADIATIONBALANCE.TOOLTIP_CURRENT_BALANCE;
			}
			text += "\n\n";
			float num2 = Mathf.Clamp01(1f - Db.Get().Attributes.RadiationResistance.Lookup(instance.gameObject).GetTotalValue());
			text += string.Format(DUPLICANTS.STATS.RADIATIONBALANCE.CURRENT_EXPOSURE, Mathf.RoundToInt(Grid.Radiation[num] * num2));
			text += "\n";
			text += string.Format(DUPLICANTS.STATS.RADIATIONBALANCE.CURRENT_REJUVENATION, Mathf.RoundToInt(Db.Get().Attributes.RadiationRecovery.Lookup(instance.gameObject).GetTotalValue() * 600f));
		}
		return text;
	}

	// Token: 0x02001C38 RID: 7224
	public class RadiationAttributeFormatter : StandardAttributeFormatter
	{
		// Token: 0x06009642 RID: 38466 RVA: 0x001065C4 File Offset: 0x001047C4
		public RadiationAttributeFormatter() : base(GameUtil.UnitClass.SimpleFloat, GameUtil.TimeSlice.PerCycle)
		{
		}
	}
}
