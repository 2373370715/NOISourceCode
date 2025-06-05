using System;
using System.Collections.Generic;

// Token: 0x02001FC3 RID: 8131
public interface IDispenser
{
	// Token: 0x0600ABE2 RID: 44002
	List<Tag> DispensedItems();

	// Token: 0x0600ABE3 RID: 44003
	Tag SelectedItem();

	// Token: 0x0600ABE4 RID: 44004
	void SelectItem(Tag tag);

	// Token: 0x0600ABE5 RID: 44005
	void OnOrderDispense();

	// Token: 0x0600ABE6 RID: 44006
	void OnCancelDispense();

	// Token: 0x0600ABE7 RID: 44007
	bool HasOpenChore();

	// Token: 0x14000037 RID: 55
	// (add) Token: 0x0600ABE8 RID: 44008
	// (remove) Token: 0x0600ABE9 RID: 44009
	event System.Action OnStopWorkEvent;
}
