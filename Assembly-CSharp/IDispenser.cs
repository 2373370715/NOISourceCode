using System;
using System.Collections.Generic;

public interface IDispenser
{
	List<Tag> DispensedItems();

	Tag SelectedItem();

	void SelectItem(Tag tag);

	void OnOrderDispense();

	void OnCancelDispense();

	bool HasOpenChore();

add) Token: 0x0600ABE8 RID: 44008
remove) Token: 0x0600ABE9 RID: 44009
	event System.Action OnStopWorkEvent;
}
