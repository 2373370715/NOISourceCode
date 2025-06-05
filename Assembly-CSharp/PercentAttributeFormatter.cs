using System;
using Klei.AI;

// Token: 0x02001C47 RID: 7239
public class PercentAttributeFormatter : StandardAttributeFormatter
{
	// Token: 0x06009672 RID: 38514 RVA: 0x001065F9 File Offset: 0x001047F9
	public PercentAttributeFormatter() : base(GameUtil.UnitClass.Percent, GameUtil.TimeSlice.None)
	{
	}

	// Token: 0x06009673 RID: 38515 RVA: 0x001066D0 File Offset: 0x001048D0
	public override string GetFormattedAttribute(AttributeInstance instance)
	{
		return this.GetFormattedValue(instance.GetTotalDisplayValue(), base.DeltaTimeSlice);
	}

	// Token: 0x06009674 RID: 38516 RVA: 0x001066E4 File Offset: 0x001048E4
	public override string GetFormattedModifier(AttributeModifier modifier)
	{
		return this.GetFormattedValue(modifier.Value, base.DeltaTimeSlice);
	}

	// Token: 0x06009675 RID: 38517 RVA: 0x0010670E File Offset: 0x0010490E
	public override string GetFormattedValue(float value, GameUtil.TimeSlice timeSlice)
	{
		return GameUtil.GetFormattedPercent(value * 100f, timeSlice);
	}
}
