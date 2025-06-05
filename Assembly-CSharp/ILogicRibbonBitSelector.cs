using System;

// Token: 0x02001FE8 RID: 8168
public interface ILogicRibbonBitSelector
{
	// Token: 0x0600AC98 RID: 44184
	void SetBitSelection(int bit);

	// Token: 0x0600AC99 RID: 44185
	int GetBitSelection();

	// Token: 0x0600AC9A RID: 44186
	int GetBitDepth();

	// Token: 0x17000AFF RID: 2815
	// (get) Token: 0x0600AC9B RID: 44187
	string SideScreenTitle { get; }

	// Token: 0x17000B00 RID: 2816
	// (get) Token: 0x0600AC9C RID: 44188
	string SideScreenDescription { get; }

	// Token: 0x0600AC9D RID: 44189
	bool SideScreenDisplayWriterDescription();

	// Token: 0x0600AC9E RID: 44190
	bool SideScreenDisplayReaderDescription();

	// Token: 0x0600AC9F RID: 44191
	bool IsBitActive(int bit);

	// Token: 0x0600ACA0 RID: 44192
	int GetOutputValue();

	// Token: 0x0600ACA1 RID: 44193
	int GetInputValue();

	// Token: 0x0600ACA2 RID: 44194
	void UpdateVisuals();
}
