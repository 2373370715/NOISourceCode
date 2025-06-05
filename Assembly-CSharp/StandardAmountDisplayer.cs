using System;
using Klei.AI;
using STRINGS;

// Token: 0x02001C2E RID: 7214
public class StandardAmountDisplayer : IAmountDisplayer
{
	// Token: 0x170009BF RID: 2495
	// (get) Token: 0x0600961D RID: 38429 RVA: 0x00106389 File Offset: 0x00104589
	public IAttributeFormatter Formatter
	{
		get
		{
			return this.formatter;
		}
	}

	// Token: 0x170009C0 RID: 2496
	// (get) Token: 0x0600961E RID: 38430 RVA: 0x00106391 File Offset: 0x00104591
	// (set) Token: 0x0600961F RID: 38431 RVA: 0x0010639E File Offset: 0x0010459E
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

	// Token: 0x06009620 RID: 38432 RVA: 0x001063AC File Offset: 0x001045AC
	public StandardAmountDisplayer(GameUtil.UnitClass unitClass, GameUtil.TimeSlice deltaTimeSlice, StandardAttributeFormatter formatter = null, GameUtil.IdentityDescriptorTense tense = GameUtil.IdentityDescriptorTense.Normal)
	{
		this.tense = tense;
		if (formatter != null)
		{
			this.formatter = formatter;
			return;
		}
		this.formatter = new StandardAttributeFormatter(unitClass, deltaTimeSlice);
	}

	// Token: 0x06009621 RID: 38433 RVA: 0x003AB0A0 File Offset: 0x003A92A0
	public virtual string GetValueString(Amount master, AmountInstance instance)
	{
		if (!master.showMax)
		{
			return this.formatter.GetFormattedValue(instance.value, GameUtil.TimeSlice.None);
		}
		return string.Format("{0} / {1}", this.formatter.GetFormattedValue(instance.value, GameUtil.TimeSlice.None), this.formatter.GetFormattedValue(instance.GetMax(), GameUtil.TimeSlice.None));
	}

	// Token: 0x06009622 RID: 38434 RVA: 0x001063D4 File Offset: 0x001045D4
	public virtual string GetDescription(Amount master, AmountInstance instance)
	{
		return string.Format("{0}: {1}", master.Name, this.GetValueString(master, instance));
	}

	// Token: 0x06009623 RID: 38435 RVA: 0x003AB0F8 File Offset: 0x003A92F8
	public virtual string GetTooltip(Amount master, AmountInstance instance)
	{
		string text = "";
		if (master.description.IndexOf("{1}") > -1)
		{
			text += string.Format(master.description, this.formatter.GetFormattedValue(instance.value, GameUtil.TimeSlice.None), GameUtil.GetIdentityDescriptor(instance.gameObject, this.tense));
		}
		else
		{
			text += string.Format(master.description, this.formatter.GetFormattedValue(instance.value, GameUtil.TimeSlice.None));
		}
		text += "\n\n";
		if (this.formatter.DeltaTimeSlice == GameUtil.TimeSlice.PerCycle)
		{
			text += string.Format(UI.CHANGEPERCYCLE, this.formatter.GetFormattedValue(instance.deltaAttribute.GetTotalDisplayValue(), GameUtil.TimeSlice.PerCycle));
		}
		else if (this.formatter.DeltaTimeSlice == GameUtil.TimeSlice.PerSecond)
		{
			text += string.Format(UI.CHANGEPERSECOND, this.formatter.GetFormattedValue(instance.deltaAttribute.GetTotalDisplayValue(), GameUtil.TimeSlice.PerSecond));
		}
		for (int num = 0; num != instance.deltaAttribute.Modifiers.Count; num++)
		{
			AttributeModifier attributeModifier = instance.deltaAttribute.Modifiers[num];
			text = text + "\n" + string.Format(UI.MODIFIER_ITEM_TEMPLATE, attributeModifier.GetDescription(), this.formatter.GetFormattedModifier(attributeModifier));
		}
		return text;
	}

	// Token: 0x06009624 RID: 38436 RVA: 0x001063EE File Offset: 0x001045EE
	public string GetFormattedAttribute(AttributeInstance instance)
	{
		return this.formatter.GetFormattedAttribute(instance);
	}

	// Token: 0x06009625 RID: 38437 RVA: 0x001063FC File Offset: 0x001045FC
	public string GetFormattedModifier(AttributeModifier modifier)
	{
		return this.formatter.GetFormattedModifier(modifier);
	}

	// Token: 0x06009626 RID: 38438 RVA: 0x0010640A File Offset: 0x0010460A
	public string GetFormattedValue(float value, GameUtil.TimeSlice time_slice)
	{
		return this.formatter.GetFormattedValue(value, time_slice);
	}

	// Token: 0x040074C0 RID: 29888
	protected StandardAttributeFormatter formatter;

	// Token: 0x040074C1 RID: 29889
	public GameUtil.IdentityDescriptorTense tense;
}
