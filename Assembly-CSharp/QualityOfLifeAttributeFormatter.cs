using System;
using Klei.AI;
using STRINGS;

// Token: 0x02001C44 RID: 7236
public class QualityOfLifeAttributeFormatter : StandardAttributeFormatter
{
	// Token: 0x06009669 RID: 38505 RVA: 0x00106676 File Offset: 0x00104876
	public QualityOfLifeAttributeFormatter() : base(GameUtil.UnitClass.SimpleInteger, GameUtil.TimeSlice.None)
	{
	}

	// Token: 0x0600966A RID: 38506 RVA: 0x003AC56C File Offset: 0x003AA76C
	public override string GetFormattedAttribute(AttributeInstance instance)
	{
		AttributeInstance attributeInstance = Db.Get().Attributes.QualityOfLifeExpectation.Lookup(instance.gameObject);
		return string.Format(DUPLICANTS.ATTRIBUTES.QUALITYOFLIFE.DESC_FORMAT, this.GetFormattedValue(instance.GetTotalDisplayValue(), GameUtil.TimeSlice.None), this.GetFormattedValue(attributeInstance.GetTotalDisplayValue(), GameUtil.TimeSlice.None));
	}

	// Token: 0x0600966B RID: 38507 RVA: 0x003AC5C0 File Offset: 0x003AA7C0
	public override string GetTooltip(Klei.AI.Attribute master, AttributeInstance instance)
	{
		string text = base.GetTooltip(master, instance);
		AttributeInstance attributeInstance = Db.Get().Attributes.QualityOfLifeExpectation.Lookup(instance.gameObject);
		text = text + "\n\n" + string.Format(DUPLICANTS.ATTRIBUTES.QUALITYOFLIFE.TOOLTIP_EXPECTATION, this.GetFormattedValue(attributeInstance.GetTotalDisplayValue(), GameUtil.TimeSlice.None));
		if (instance.GetTotalDisplayValue() - attributeInstance.GetTotalDisplayValue() >= 0f)
		{
			text = text + "\n\n" + DUPLICANTS.ATTRIBUTES.QUALITYOFLIFE.TOOLTIP_EXPECTATION_OVER;
		}
		else
		{
			text = text + "\n\n" + DUPLICANTS.ATTRIBUTES.QUALITYOFLIFE.TOOLTIP_EXPECTATION_UNDER;
		}
		return text;
	}
}
