using System;

// Token: 0x0200203D RID: 8253
public interface ISliderControl
{
	// Token: 0x17000B3F RID: 2879
	// (get) Token: 0x0600AF0D RID: 44813
	string SliderTitleKey { get; }

	// Token: 0x17000B40 RID: 2880
	// (get) Token: 0x0600AF0E RID: 44814
	string SliderUnits { get; }

	// Token: 0x0600AF0F RID: 44815
	int SliderDecimalPlaces(int index);

	// Token: 0x0600AF10 RID: 44816
	float GetSliderMin(int index);

	// Token: 0x0600AF11 RID: 44817
	float GetSliderMax(int index);

	// Token: 0x0600AF12 RID: 44818
	float GetSliderValue(int index);

	// Token: 0x0600AF13 RID: 44819
	void SetSliderValue(float percent, int index);

	// Token: 0x0600AF14 RID: 44820
	string GetSliderTooltipKey(int index);

	// Token: 0x0600AF15 RID: 44821
	string GetSliderTooltip(int index);
}
