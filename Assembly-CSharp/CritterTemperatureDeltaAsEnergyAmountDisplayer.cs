using System;
using Klei.AI;
using STRINGS;

// Token: 0x02001C3E RID: 7230
public class CritterTemperatureDeltaAsEnergyAmountDisplayer : StandardAmountDisplayer
{
	// Token: 0x0600964D RID: 38477 RVA: 0x00106542 File Offset: 0x00104742
	public CritterTemperatureDeltaAsEnergyAmountDisplayer(GameUtil.UnitClass unitClass, GameUtil.TimeSlice timeSlice) : base(unitClass, timeSlice, null, GameUtil.IdentityDescriptorTense.Normal)
	{
	}

	// Token: 0x0600964E RID: 38478 RVA: 0x003AC04C File Offset: 0x003AA24C
	public override string GetTooltip(Amount master, AmountInstance instance)
	{
		CritterTemperatureMonitor.Def def = instance.gameObject.GetDef<CritterTemperatureMonitor.Def>();
		PrimaryElement component = instance.gameObject.GetComponent<PrimaryElement>();
		string text = string.Format(master.description, new object[]
		{
			this.formatter.GetFormattedValue(def.temperatureColdUncomfortable, GameUtil.TimeSlice.None),
			this.formatter.GetFormattedValue(def.temperatureHotUncomfortable, GameUtil.TimeSlice.None),
			this.formatter.GetFormattedValue(def.temperatureColdDeadly, GameUtil.TimeSlice.None),
			this.formatter.GetFormattedValue(def.temperatureHotDeadly, GameUtil.TimeSlice.None)
		});
		float num = ElementLoader.FindElementByHash(SimHashes.Creature).specificHeatCapacity * component.Mass * 1000f;
		if (this.formatter.DeltaTimeSlice == GameUtil.TimeSlice.PerCycle)
		{
			text += "\n\n";
			text += string.Format(UI.CHANGEPERCYCLE, this.formatter.GetFormattedValue(instance.deltaAttribute.GetTotalDisplayValue(), GameUtil.TimeSlice.PerCycle));
		}
		else if (instance.deltaAttribute.Modifiers.Count > 0)
		{
			text += "\n\n";
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
