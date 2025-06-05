using System;

// Token: 0x02001F9C RID: 8092
public interface ISidescreenButtonControl
{
	// Token: 0x17000AEE RID: 2798
	// (get) Token: 0x0600AB03 RID: 43779
	string SidescreenButtonText { get; }

	// Token: 0x17000AEF RID: 2799
	// (get) Token: 0x0600AB04 RID: 43780
	string SidescreenButtonTooltip { get; }

	// Token: 0x0600AB05 RID: 43781
	void SetButtonTextOverride(ButtonMenuTextOverride textOverride);

	// Token: 0x0600AB06 RID: 43782
	bool SidescreenEnabled();

	// Token: 0x0600AB07 RID: 43783
	bool SidescreenButtonInteractable();

	// Token: 0x0600AB08 RID: 43784
	void OnSidescreenButtonPressed();

	// Token: 0x0600AB09 RID: 43785
	int HorizontalGroupID();

	// Token: 0x0600AB0A RID: 43786
	int ButtonSideScreenSortOrder();
}
