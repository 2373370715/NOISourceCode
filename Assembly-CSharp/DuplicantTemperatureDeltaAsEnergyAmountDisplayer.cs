using System;
using Klei.AI;
using STRINGS;
using TUNING;

// Token: 0x02001C3D RID: 7229
public class DuplicantTemperatureDeltaAsEnergyAmountDisplayer : StandardAmountDisplayer
{
	// Token: 0x0600964B RID: 38475 RVA: 0x00106542 File Offset: 0x00104742
	public DuplicantTemperatureDeltaAsEnergyAmountDisplayer(GameUtil.UnitClass unitClass, GameUtil.TimeSlice timeSlice) : base(unitClass, timeSlice, null, GameUtil.IdentityDescriptorTense.Normal)
	{
	}

	// Token: 0x0600964C RID: 38476 RVA: 0x003ABECC File Offset: 0x003AA0CC
	public override string GetTooltip(Amount master, AmountInstance instance)
	{
		string text = string.Format(master.description, this.formatter.GetFormattedValue(instance.value, GameUtil.TimeSlice.None), this.formatter.GetFormattedValue(DUPLICANTSTATS.STANDARD.Temperature.Internal.IDEAL, GameUtil.TimeSlice.None));
		float num = ElementLoader.FindElementByHash(SimHashes.Creature).specificHeatCapacity * DUPLICANTSTATS.STANDARD.BaseStats.DEFAULT_MASS * 1000f;
		text += "\n\n";
		if (this.formatter.DeltaTimeSlice == GameUtil.TimeSlice.PerCycle)
		{
			text += string.Format(UI.CHANGEPERCYCLE, this.formatter.GetFormattedValue(instance.deltaAttribute.GetTotalDisplayValue(), GameUtil.TimeSlice.PerCycle));
		}
		else
		{
			text += string.Format(UI.CHANGEPERSECOND, this.formatter.GetFormattedValue(instance.deltaAttribute.GetTotalDisplayValue(), GameUtil.TimeSlice.PerSecond));
			text = text + "\n" + string.Format(UI.CHANGEPERSECOND, GameUtil.GetFormattedJoules(instance.deltaAttribute.GetTotalDisplayValue() * num, "F1", GameUtil.TimeSlice.None));
		}
		for (int num2 = 0; num2 != instance.deltaAttribute.Modifiers.Count; num2++)
		{
			AttributeModifier attributeModifier = instance.deltaAttribute.Modifiers[num2];
			text = text + "\n" + string.Format(UI.MODIFIER_ITEM_TEMPLATE, attributeModifier.GetDescription(), GameUtil.GetFormattedHeatEnergyRate(attributeModifier.Value * num * 1f, GameUtil.HeatEnergyFormatterUnit.Automatic));
		}
		return text;
	}
}
