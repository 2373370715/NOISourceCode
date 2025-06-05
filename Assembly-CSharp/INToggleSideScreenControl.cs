using System;
using System.Collections.Generic;

// Token: 0x02001FF6 RID: 8182
public interface INToggleSideScreenControl
{
	// Token: 0x17000B0B RID: 2827
	// (get) Token: 0x0600ACED RID: 44269
	string SidescreenTitleKey { get; }

	// Token: 0x17000B0C RID: 2828
	// (get) Token: 0x0600ACEE RID: 44270
	List<LocString> Options { get; }

	// Token: 0x17000B0D RID: 2829
	// (get) Token: 0x0600ACEF RID: 44271
	List<LocString> Tooltips { get; }

	// Token: 0x17000B0E RID: 2830
	// (get) Token: 0x0600ACF0 RID: 44272
	string Description { get; }

	// Token: 0x17000B0F RID: 2831
	// (get) Token: 0x0600ACF1 RID: 44273
	int SelectedOption { get; }

	// Token: 0x17000B10 RID: 2832
	// (get) Token: 0x0600ACF2 RID: 44274
	int QueuedOption { get; }

	// Token: 0x0600ACF3 RID: 44275
	void QueueSelectedOption(int option);
}
