using System;

// Token: 0x02002032 RID: 8242
public interface ICheckboxControl
{
	// Token: 0x17000B2F RID: 2863
	// (get) Token: 0x0600AEA9 RID: 44713
	string CheckboxTitleKey { get; }

	// Token: 0x17000B30 RID: 2864
	// (get) Token: 0x0600AEAA RID: 44714
	string CheckboxLabel { get; }

	// Token: 0x17000B31 RID: 2865
	// (get) Token: 0x0600AEAB RID: 44715
	string CheckboxTooltip { get; }

	// Token: 0x0600AEAC RID: 44716
	bool GetCheckboxValue();

	// Token: 0x0600AEAD RID: 44717
	void SetCheckboxValue(bool value);
}
