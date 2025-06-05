using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02001FCE RID: 8142
public class FilterSideScreen : SingleItemSelectionSideScreenBase
{
	// Token: 0x0600AC07 RID: 44039 RVA: 0x0041BD78 File Offset: 0x00419F78
	public override bool IsValidForTarget(GameObject target)
	{
		bool flag;
		if (this.isLogicFilter)
		{
			flag = (target.GetComponent<ConduitElementSensor>() != null || target.GetComponent<LogicElementSensor>() != null);
		}
		else
		{
			flag = (target.GetComponent<ElementFilter>() != null || target.GetComponent<RocketConduitStorageAccess>() != null || target.GetComponent<DevPump>() != null);
		}
		return flag && target.GetComponent<Filterable>() != null;
	}

	// Token: 0x0600AC08 RID: 44040 RVA: 0x0041BDEC File Offset: 0x00419FEC
	public override void SetTarget(GameObject target)
	{
		base.SetTarget(target);
		this.targetFilterable = target.GetComponent<Filterable>();
		if (this.targetFilterable == null)
		{
			return;
		}
		switch (this.targetFilterable.filterElementState)
		{
		case Filterable.ElementState.Solid:
			this.everythingElseHeaderLabel.text = UI.UISIDESCREENS.FILTERSIDESCREEN.UNFILTEREDELEMENTS.SOLID;
			goto IL_87;
		case Filterable.ElementState.Gas:
			this.everythingElseHeaderLabel.text = UI.UISIDESCREENS.FILTERSIDESCREEN.UNFILTEREDELEMENTS.GAS;
			goto IL_87;
		}
		this.everythingElseHeaderLabel.text = UI.UISIDESCREENS.FILTERSIDESCREEN.UNFILTEREDELEMENTS.LIQUID;
		IL_87:
		this.Configure(this.targetFilterable);
		this.SetFilterTag(this.targetFilterable.SelectedTag);
	}

	// Token: 0x0600AC09 RID: 44041 RVA: 0x001143F7 File Offset: 0x001125F7
	public override void ItemRowClicked(SingleItemSelectionRow rowClicked)
	{
		this.SetFilterTag(rowClicked.tag);
		base.ItemRowClicked(rowClicked);
	}

	// Token: 0x0600AC0A RID: 44042 RVA: 0x0041BEA0 File Offset: 0x0041A0A0
	private void Configure(Filterable filterable)
	{
		Dictionary<Tag, HashSet<Tag>> tagOptions = filterable.GetTagOptions();
		Tag tag = GameTags.Void;
		foreach (Tag tag2 in tagOptions.Keys)
		{
			using (HashSet<Tag>.Enumerator enumerator2 = tagOptions[tag2].GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					if (enumerator2.Current == filterable.SelectedTag)
					{
						tag = tag2;
						break;
					}
				}
			}
		}
		this.SetData(tagOptions);
		SingleItemSelectionSideScreenBase.Category category = null;
		if (this.categories.TryGetValue(GameTags.Void, out category))
		{
			category.SetProihibedState(true);
		}
		if (tag != GameTags.Void)
		{
			this.categories[tag].SetUnfoldedState(SingleItemSelectionSideScreenBase.Category.UnfoldedStates.Unfolded);
		}
		if (this.voidRow == null)
		{
			this.voidRow = this.GetOrCreateItemRow(GameTags.Void);
		}
		this.voidRow.transform.SetAsFirstSibling();
		if (filterable.SelectedTag != GameTags.Void)
		{
			this.SetSelectedItem(filterable.SelectedTag);
		}
		else
		{
			this.SetSelectedItem(this.voidRow);
		}
		this.RefreshUI();
	}

	// Token: 0x0600AC0B RID: 44043 RVA: 0x0011440C File Offset: 0x0011260C
	private void SetFilterTag(Tag tag)
	{
		if (this.targetFilterable == null)
		{
			return;
		}
		if (tag.IsValid)
		{
			this.targetFilterable.SelectedTag = tag;
		}
		this.RefreshUI();
	}

	// Token: 0x0600AC0C RID: 44044 RVA: 0x0041BFF0 File Offset: 0x0041A1F0
	private void RefreshUI()
	{
		LocString loc_string;
		switch (this.targetFilterable.filterElementState)
		{
		case Filterable.ElementState.Solid:
			loc_string = UI.UISIDESCREENS.FILTERSIDESCREEN.FILTEREDELEMENT.SOLID;
			goto IL_38;
		case Filterable.ElementState.Gas:
			loc_string = UI.UISIDESCREENS.FILTERSIDESCREEN.FILTEREDELEMENT.GAS;
			goto IL_38;
		}
		loc_string = UI.UISIDESCREENS.FILTERSIDESCREEN.FILTEREDELEMENT.LIQUID;
		IL_38:
		this.currentSelectionLabel.text = string.Format(loc_string, UI.UISIDESCREENS.FILTERSIDESCREEN.NOELEMENTSELECTED);
		if (base.CurrentSelectedItem == null || base.CurrentSelectedItem.tag != this.targetFilterable.SelectedTag)
		{
			this.SetSelectedItem(this.targetFilterable.SelectedTag);
		}
		if (this.targetFilterable.SelectedTag != GameTags.Void)
		{
			this.currentSelectionLabel.text = string.Format(loc_string, this.targetFilterable.SelectedTag.ProperName());
			return;
		}
		this.currentSelectionLabel.text = UI.UISIDESCREENS.FILTERSIDESCREEN.NO_SELECTION;
	}

	// Token: 0x04008774 RID: 34676
	public HierarchyReferences categoryFoldoutPrefab;

	// Token: 0x04008775 RID: 34677
	public RectTransform elementEntryContainer;

	// Token: 0x04008776 RID: 34678
	public Image outputIcon;

	// Token: 0x04008777 RID: 34679
	public Image everythingElseIcon;

	// Token: 0x04008778 RID: 34680
	public LocText outputElementHeaderLabel;

	// Token: 0x04008779 RID: 34681
	public LocText everythingElseHeaderLabel;

	// Token: 0x0400877A RID: 34682
	public LocText selectElementHeaderLabel;

	// Token: 0x0400877B RID: 34683
	public LocText currentSelectionLabel;

	// Token: 0x0400877C RID: 34684
	private SingleItemSelectionRow voidRow;

	// Token: 0x0400877D RID: 34685
	public bool isLogicFilter;

	// Token: 0x0400877E RID: 34686
	private Filterable targetFilterable;
}
