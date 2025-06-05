using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02002035 RID: 8245
public class SingleItemSelectionSideScreen : SingleItemSelectionSideScreenBase
{
	// Token: 0x0600AEC8 RID: 44744 RVA: 0x0011621B File Offset: 0x0011441B
	public override bool IsValidForTarget(GameObject target)
	{
		return target.GetSMI<StorageTile.Instance>() != null && target.GetComponent<TreeFilterable>() != null;
	}

	// Token: 0x0600AEC9 RID: 44745 RVA: 0x00116233 File Offset: 0x00114433
	private Tag GetTargetCurrentSelectedTag()
	{
		if (this.CurrentTarget != null)
		{
			return this.CurrentTarget.TargetTag;
		}
		return this.INVALID_OPTION_TAG;
	}

	// Token: 0x0600AECA RID: 44746 RVA: 0x004283C0 File Offset: 0x004265C0
	public override void SetTarget(GameObject target)
	{
		base.SetTarget(target);
		this.CurrentTarget = target.GetSMI<StorageTile.Instance>();
		if (this.CurrentTarget != null)
		{
			Dictionary<Tag, HashSet<Tag>> dictionary = new Dictionary<Tag, HashSet<Tag>>();
			foreach (Tag tag in new HashSet<Tag>(this.CurrentTarget.GetComponent<Storage>().storageFilters))
			{
				HashSet<Tag> discoveredResourcesFromTag = DiscoveredResources.Instance.GetDiscoveredResourcesFromTag(tag);
				if (discoveredResourcesFromTag != null && discoveredResourcesFromTag.Count > 0)
				{
					dictionary.Add(tag, discoveredResourcesFromTag);
				}
			}
			this.SetData(dictionary);
			SingleItemSelectionSideScreenBase.Category category = null;
			if (!this.categories.TryGetValue(this.INVALID_OPTION_TAG, out category))
			{
				category = base.GetCategoryWithItem(this.INVALID_OPTION_TAG, false);
			}
			if (category != null)
			{
				category.SetProihibedState(true);
			}
			this.CreateNoneOption();
			Tag targetCurrentSelectedTag = this.GetTargetCurrentSelectedTag();
			if (targetCurrentSelectedTag != this.INVALID_OPTION_TAG)
			{
				this.SetSelectedItem(targetCurrentSelectedTag);
				base.GetCategoryWithItem(targetCurrentSelectedTag, false).SetUnfoldedState(SingleItemSelectionSideScreenBase.Category.UnfoldedStates.Unfolded);
			}
			else
			{
				this.SetSelectedItem(this.noneOptionRow);
			}
			this.selectedItemLabel.SetItem(targetCurrentSelectedTag);
		}
	}

	// Token: 0x0600AECB RID: 44747 RVA: 0x0011624F File Offset: 0x0011444F
	private void CreateNoneOption()
	{
		if (this.noneOptionRow == null)
		{
			this.noneOptionRow = this.GetOrCreateItemRow(this.INVALID_OPTION_TAG);
		}
		this.noneOptionRow.transform.SetAsFirstSibling();
	}

	// Token: 0x0600AECC RID: 44748 RVA: 0x004284E8 File Offset: 0x004266E8
	public override void ItemRowClicked(SingleItemSelectionRow rowClicked)
	{
		base.ItemRowClicked(rowClicked);
		this.selectedItemLabel.SetItem(rowClicked.tag);
		Tag targetCurrentSelectedTag = this.GetTargetCurrentSelectedTag();
		if (this.CurrentTarget != null && targetCurrentSelectedTag != rowClicked.tag)
		{
			this.CurrentTarget.SetTargetItem(rowClicked.tag);
		}
	}

	// Token: 0x04008982 RID: 35202
	[SerializeField]
	private SingleItemSelectionSideScreen_SelectedItemSection selectedItemLabel;

	// Token: 0x04008983 RID: 35203
	private StorageTile.Instance CurrentTarget;

	// Token: 0x04008984 RID: 35204
	private SingleItemSelectionRow noneOptionRow;

	// Token: 0x04008985 RID: 35205
	private Tag INVALID_OPTION_TAG = GameTags.Void;
}
