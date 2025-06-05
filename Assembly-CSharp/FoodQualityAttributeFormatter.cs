using System;
using Klei.AI;

// Token: 0x02001C43 RID: 7235
public class FoodQualityAttributeFormatter : StandardAttributeFormatter
{
	// Token: 0x06009665 RID: 38501 RVA: 0x00106676 File Offset: 0x00104876
	public FoodQualityAttributeFormatter() : base(GameUtil.UnitClass.SimpleInteger, GameUtil.TimeSlice.None)
	{
	}

	// Token: 0x06009666 RID: 38502 RVA: 0x0010662A File Offset: 0x0010482A
	public override string GetFormattedAttribute(AttributeInstance instance)
	{
		return this.GetFormattedValue(instance.GetTotalDisplayValue(), GameUtil.TimeSlice.None);
	}

	// Token: 0x06009667 RID: 38503 RVA: 0x00106680 File Offset: 0x00104880
	public override string GetFormattedModifier(AttributeModifier modifier)
	{
		return GameUtil.GetFormattedInt(modifier.Value, GameUtil.TimeSlice.None);
	}

	// Token: 0x06009668 RID: 38504 RVA: 0x0010668E File Offset: 0x0010488E
	public override string GetFormattedValue(float value, GameUtil.TimeSlice timeSlice)
	{
		return Util.StripTextFormatting(GameUtil.GetFormattedFoodQuality((int)value));
	}
}
