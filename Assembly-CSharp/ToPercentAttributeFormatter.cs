using System;
using Klei.AI;

// Token: 0x02001C46 RID: 7238
public class ToPercentAttributeFormatter : StandardAttributeFormatter
{
	// Token: 0x0600966E RID: 38510 RVA: 0x001066B4 File Offset: 0x001048B4
	public ToPercentAttributeFormatter(float max, GameUtil.TimeSlice deltaTimeSlice = GameUtil.TimeSlice.None) : base(GameUtil.UnitClass.Percent, deltaTimeSlice)
	{
		this.max = max;
	}

	// Token: 0x0600966F RID: 38511 RVA: 0x001066D0 File Offset: 0x001048D0
	public override string GetFormattedAttribute(AttributeInstance instance)
	{
		return this.GetFormattedValue(instance.GetTotalDisplayValue(), base.DeltaTimeSlice);
	}

	// Token: 0x06009670 RID: 38512 RVA: 0x001066E4 File Offset: 0x001048E4
	public override string GetFormattedModifier(AttributeModifier modifier)
	{
		return this.GetFormattedValue(modifier.Value, base.DeltaTimeSlice);
	}

	// Token: 0x06009671 RID: 38513 RVA: 0x001066F8 File Offset: 0x001048F8
	public override string GetFormattedValue(float value, GameUtil.TimeSlice timeSlice)
	{
		return GameUtil.GetFormattedPercent(value / this.max * 100f, timeSlice);
	}

	// Token: 0x040074CD RID: 29901
	public float max = 1f;
}
