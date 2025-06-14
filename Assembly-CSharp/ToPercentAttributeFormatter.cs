﻿using System;
using Klei.AI;

public class ToPercentAttributeFormatter : StandardAttributeFormatter
{
	public ToPercentAttributeFormatter(float max, GameUtil.TimeSlice deltaTimeSlice = GameUtil.TimeSlice.None) : base(GameUtil.UnitClass.Percent, deltaTimeSlice)
	{
		this.max = max;
	}

	public override string GetFormattedAttribute(AttributeInstance instance)
	{
		return this.GetFormattedValue(instance.GetTotalDisplayValue(), base.DeltaTimeSlice);
	}

	public override string GetFormattedModifier(AttributeModifier modifier)
	{
		return this.GetFormattedValue(modifier.Value, base.DeltaTimeSlice);
	}

	public override string GetFormattedValue(float value, GameUtil.TimeSlice timeSlice)
	{
		return GameUtil.GetFormattedPercent(value / this.max * 100f, timeSlice);
	}

	public float max = 1f;
}
