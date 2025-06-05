using System;
using Klei.AI;
using STRINGS;

// Token: 0x02001C2F RID: 7215
public class AsPercentAmountDisplayer : IAmountDisplayer
{
	// Token: 0x170009C1 RID: 2497
	// (get) Token: 0x06009627 RID: 38439 RVA: 0x00106419 File Offset: 0x00104619
	public IAttributeFormatter Formatter
	{
		get
		{
			return this.formatter;
		}
	}

	// Token: 0x170009C2 RID: 2498
	// (get) Token: 0x06009628 RID: 38440 RVA: 0x00106421 File Offset: 0x00104621
	// (set) Token: 0x06009629 RID: 38441 RVA: 0x0010642E File Offset: 0x0010462E
	public GameUtil.TimeSlice DeltaTimeSlice
	{
		get
		{
			return this.formatter.DeltaTimeSlice;
		}
		set
		{
			this.formatter.DeltaTimeSlice = value;
		}
	}

	// Token: 0x0600962A RID: 38442 RVA: 0x0010643C File Offset: 0x0010463C
	public AsPercentAmountDisplayer(GameUtil.TimeSlice deltaTimeSlice)
	{
		this.formatter = new StandardAttributeFormatter(GameUtil.UnitClass.Percent, deltaTimeSlice);
	}

	// Token: 0x0600962B RID: 38443 RVA: 0x00106451 File Offset: 0x00104651
	public string GetValueString(Amount master, AmountInstance instance)
	{
		return this.formatter.GetFormattedValue(this.ToPercent(instance.value, instance), GameUtil.TimeSlice.None);
	}

	// Token: 0x0600962C RID: 38444 RVA: 0x0010646C File Offset: 0x0010466C
	public virtual string GetDescription(Amount master, AmountInstance instance)
	{
		return string.Format("{0}: {1}", master.Name, this.formatter.GetFormattedValue(this.ToPercent(instance.value, instance), GameUtil.TimeSlice.None));
	}

	// Token: 0x0600962D RID: 38445 RVA: 0x00106497 File Offset: 0x00104697
	public virtual string GetTooltipDescription(Amount master, AmountInstance instance)
	{
		return string.Format(master.description, this.formatter.GetFormattedValue(instance.value, GameUtil.TimeSlice.None));
	}

	// Token: 0x0600962E RID: 38446 RVA: 0x003AB258 File Offset: 0x003A9458
	public virtual string GetTooltip(Amount master, AmountInstance instance)
	{
		string text = string.Format(master.description, this.formatter.GetFormattedValue(instance.value, GameUtil.TimeSlice.None));
		text += "\n\n";
		if (this.formatter.DeltaTimeSlice == GameUtil.TimeSlice.PerCycle)
		{
			text += string.Format(UI.CHANGEPERCYCLE, this.formatter.GetFormattedValue(this.ToPercent(instance.deltaAttribute.GetTotalDisplayValue(), instance), GameUtil.TimeSlice.PerCycle));
		}
		else
		{
			text += string.Format(UI.CHANGEPERSECOND, this.formatter.GetFormattedValue(this.ToPercent(instance.deltaAttribute.GetTotalDisplayValue(), instance), GameUtil.TimeSlice.PerSecond));
		}
		for (int num = 0; num != instance.deltaAttribute.Modifiers.Count; num++)
		{
			AttributeModifier attributeModifier = instance.deltaAttribute.Modifiers[num];
			float modifierContribution = instance.deltaAttribute.GetModifierContribution(attributeModifier);
			text = text + "\n" + string.Format(UI.MODIFIER_ITEM_TEMPLATE, attributeModifier.GetDescription(), this.formatter.GetFormattedValue(this.ToPercent(modifierContribution, instance), this.formatter.DeltaTimeSlice));
		}
		return text;
	}

	// Token: 0x0600962F RID: 38447 RVA: 0x001064B6 File Offset: 0x001046B6
	public string GetFormattedAttribute(AttributeInstance instance)
	{
		return this.formatter.GetFormattedAttribute(instance);
	}

	// Token: 0x06009630 RID: 38448 RVA: 0x001064C4 File Offset: 0x001046C4
	public string GetFormattedModifier(AttributeModifier modifier)
	{
		if (modifier.IsMultiplier)
		{
			return GameUtil.GetFormattedPercent(modifier.Value * 100f, GameUtil.TimeSlice.None);
		}
		return this.formatter.GetFormattedModifier(modifier);
	}

	// Token: 0x06009631 RID: 38449 RVA: 0x001064ED File Offset: 0x001046ED
	public string GetFormattedValue(float value, GameUtil.TimeSlice timeSlice)
	{
		return this.formatter.GetFormattedValue(value, timeSlice);
	}

	// Token: 0x06009632 RID: 38450 RVA: 0x001064FC File Offset: 0x001046FC
	protected float ToPercent(float value, AmountInstance instance)
	{
		return 100f * value / instance.GetMax();
	}

	// Token: 0x040074C2 RID: 29890
	protected StandardAttributeFormatter formatter;
}
