using System;
using Klei.AI;

// Token: 0x02001C42 RID: 7234
public class RadsPerCycleAttributeFormatter : StandardAttributeFormatter
{
	// Token: 0x06009662 RID: 38498 RVA: 0x0010664D File Offset: 0x0010484D
	public RadsPerCycleAttributeFormatter() : base(GameUtil.UnitClass.Radiation, GameUtil.TimeSlice.PerCycle)
	{
	}

	// Token: 0x06009663 RID: 38499 RVA: 0x00106657 File Offset: 0x00104857
	public override string GetFormattedAttribute(AttributeInstance instance)
	{
		return this.GetFormattedValue(instance.GetTotalDisplayValue(), GameUtil.TimeSlice.PerCycle);
	}

	// Token: 0x06009664 RID: 38500 RVA: 0x00106666 File Offset: 0x00104866
	public override string GetFormattedValue(float value, GameUtil.TimeSlice timeSlice)
	{
		return base.GetFormattedValue(value / 600f, timeSlice);
	}
}
