using System;

// Token: 0x02001018 RID: 4120
public interface IThresholdSwitch
{
	// Token: 0x170004B6 RID: 1206
	// (get) Token: 0x0600533C RID: 21308
	// (set) Token: 0x0600533D RID: 21309
	float Threshold { get; set; }

	// Token: 0x170004B7 RID: 1207
	// (get) Token: 0x0600533E RID: 21310
	// (set) Token: 0x0600533F RID: 21311
	bool ActivateAboveThreshold { get; set; }

	// Token: 0x170004B8 RID: 1208
	// (get) Token: 0x06005340 RID: 21312
	float CurrentValue { get; }

	// Token: 0x170004B9 RID: 1209
	// (get) Token: 0x06005341 RID: 21313
	float RangeMin { get; }

	// Token: 0x170004BA RID: 1210
	// (get) Token: 0x06005342 RID: 21314
	float RangeMax { get; }

	// Token: 0x06005343 RID: 21315
	float GetRangeMinInputField();

	// Token: 0x06005344 RID: 21316
	float GetRangeMaxInputField();

	// Token: 0x170004BB RID: 1211
	// (get) Token: 0x06005345 RID: 21317
	LocString Title { get; }

	// Token: 0x170004BC RID: 1212
	// (get) Token: 0x06005346 RID: 21318
	LocString ThresholdValueName { get; }

	// Token: 0x06005347 RID: 21319
	LocString ThresholdValueUnits();

	// Token: 0x06005348 RID: 21320
	string Format(float value, bool units);

	// Token: 0x170004BD RID: 1213
	// (get) Token: 0x06005349 RID: 21321
	string AboveToolTip { get; }

	// Token: 0x170004BE RID: 1214
	// (get) Token: 0x0600534A RID: 21322
	string BelowToolTip { get; }

	// Token: 0x0600534B RID: 21323
	float ProcessedSliderValue(float input);

	// Token: 0x0600534C RID: 21324
	float ProcessedInputValue(float input);

	// Token: 0x170004BF RID: 1215
	// (get) Token: 0x0600534D RID: 21325
	ThresholdScreenLayoutType LayoutType { get; }

	// Token: 0x170004C0 RID: 1216
	// (get) Token: 0x0600534E RID: 21326
	int IncrementScale { get; }

	// Token: 0x170004C1 RID: 1217
	// (get) Token: 0x0600534F RID: 21327
	NonLinearSlider.Range[] GetRanges { get; }
}
