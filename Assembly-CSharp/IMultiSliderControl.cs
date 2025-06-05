using System;

// Token: 0x02001FF4 RID: 8180
public interface IMultiSliderControl
{
	// Token: 0x17000B09 RID: 2825
	// (get) Token: 0x0600ACE6 RID: 44262
	string SidescreenTitleKey { get; }

	// Token: 0x0600ACE7 RID: 44263
	bool SidescreenEnabled();

	// Token: 0x17000B0A RID: 2826
	// (get) Token: 0x0600ACE8 RID: 44264
	ISliderControl[] sliderControls { get; }
}
