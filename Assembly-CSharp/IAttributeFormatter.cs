using System;
using System.Collections.Generic;
using Klei.AI;

// Token: 0x02001C3F RID: 7231
public interface IAttributeFormatter
{
	// Token: 0x170009C3 RID: 2499
	// (get) Token: 0x0600964F RID: 38479
	// (set) Token: 0x06009650 RID: 38480
	GameUtil.TimeSlice DeltaTimeSlice { get; set; }

	// Token: 0x06009651 RID: 38481
	string GetFormattedAttribute(AttributeInstance instance);

	// Token: 0x06009652 RID: 38482
	string GetFormattedModifier(AttributeModifier modifier);

	// Token: 0x06009653 RID: 38483
	string GetFormattedValue(float value, GameUtil.TimeSlice timeSlice);

	// Token: 0x06009654 RID: 38484
	string GetTooltip(Klei.AI.Attribute master, AttributeInstance instance);

	// Token: 0x06009655 RID: 38485
	string GetTooltip(Klei.AI.Attribute master, List<AttributeModifier> modifiers, AttributeConverters converters);
}
