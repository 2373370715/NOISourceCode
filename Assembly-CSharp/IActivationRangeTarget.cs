using System;

// Token: 0x02001F85 RID: 8069
public interface IActivationRangeTarget
{
	// Token: 0x17000AE2 RID: 2786
	// (get) Token: 0x0600AA64 RID: 43620
	// (set) Token: 0x0600AA65 RID: 43621
	float ActivateValue { get; set; }

	// Token: 0x17000AE3 RID: 2787
	// (get) Token: 0x0600AA66 RID: 43622
	// (set) Token: 0x0600AA67 RID: 43623
	float DeactivateValue { get; set; }

	// Token: 0x17000AE4 RID: 2788
	// (get) Token: 0x0600AA68 RID: 43624
	float MinValue { get; }

	// Token: 0x17000AE5 RID: 2789
	// (get) Token: 0x0600AA69 RID: 43625
	float MaxValue { get; }

	// Token: 0x17000AE6 RID: 2790
	// (get) Token: 0x0600AA6A RID: 43626
	bool UseWholeNumbers { get; }

	// Token: 0x17000AE7 RID: 2791
	// (get) Token: 0x0600AA6B RID: 43627
	string ActivationRangeTitleText { get; }

	// Token: 0x17000AE8 RID: 2792
	// (get) Token: 0x0600AA6C RID: 43628
	string ActivateSliderLabelText { get; }

	// Token: 0x17000AE9 RID: 2793
	// (get) Token: 0x0600AA6D RID: 43629
	string DeactivateSliderLabelText { get; }

	// Token: 0x17000AEA RID: 2794
	// (get) Token: 0x0600AA6E RID: 43630
	string ActivateTooltip { get; }

	// Token: 0x17000AEB RID: 2795
	// (get) Token: 0x0600AA6F RID: 43631
	string DeactivateTooltip { get; }
}
