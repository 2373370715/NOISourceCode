using System;

// Token: 0x0200200D RID: 8205
public interface IPlayerControlledToggle
{
	// Token: 0x0600AD93 RID: 44435
	void ToggledByPlayer();

	// Token: 0x0600AD94 RID: 44436
	bool ToggledOn();

	// Token: 0x0600AD95 RID: 44437
	KSelectable GetSelectable();

	// Token: 0x17000B23 RID: 2851
	// (get) Token: 0x0600AD96 RID: 44438
	string SideScreenTitleKey { get; }

	// Token: 0x17000B24 RID: 2852
	// (get) Token: 0x0600AD97 RID: 44439
	// (set) Token: 0x0600AD98 RID: 44440
	bool ToggleRequested { get; set; }
}
