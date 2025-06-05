using System;
using System.Collections.Generic;
using KSerialization;
using UnityEngine;

// Token: 0x02000DAF RID: 3503
public class FlatTagFilterable : KMonoBehaviour
{
	// Token: 0x06004415 RID: 17429 RVA: 0x000D06E6 File Offset: 0x000CE8E6
	protected override void OnSpawn()
	{
		base.OnSpawn();
		TreeFilterable component = base.GetComponent<TreeFilterable>();
		component.filterByStorageCategoriesOnSpawn = false;
		component.UpdateFilters(new HashSet<Tag>(this.selectedTags));
		base.Subscribe(-905833192, new Action<object>(this.OnCopySettings));
	}

	// Token: 0x06004416 RID: 17430 RVA: 0x00255414 File Offset: 0x00253614
	public void SelectTag(Tag tag, bool state)
	{
		global::Debug.Assert(this.tagOptions.Contains(tag), "The tag " + tag.Name + " is not valid for this filterable - it must be added to tagOptions");
		if (state)
		{
			if (!this.selectedTags.Contains(tag))
			{
				this.selectedTags.Add(tag);
			}
		}
		else if (this.selectedTags.Contains(tag))
		{
			this.selectedTags.Remove(tag);
		}
		base.GetComponent<TreeFilterable>().UpdateFilters(new HashSet<Tag>(this.selectedTags));
	}

	// Token: 0x06004417 RID: 17431 RVA: 0x000D0723 File Offset: 0x000CE923
	public void ToggleTag(Tag tag)
	{
		this.SelectTag(tag, !this.selectedTags.Contains(tag));
	}

	// Token: 0x06004418 RID: 17432 RVA: 0x000D073B File Offset: 0x000CE93B
	public string GetHeaderText()
	{
		return this.headerText;
	}

	// Token: 0x06004419 RID: 17433 RVA: 0x00255498 File Offset: 0x00253698
	private void OnCopySettings(object data)
	{
		GameObject gameObject = (GameObject)data;
		if (base.GetComponent<KPrefabID>().PrefabID() != gameObject.GetComponent<KPrefabID>().PrefabID())
		{
			return;
		}
		this.selectedTags.Clear();
		foreach (Tag tag in gameObject.GetComponent<FlatTagFilterable>().selectedTags)
		{
			this.SelectTag(tag, true);
		}
		base.GetComponent<TreeFilterable>().UpdateFilters(new HashSet<Tag>(this.selectedTags));
	}

	// Token: 0x04002F2B RID: 12075
	[Serialize]
	public List<Tag> selectedTags = new List<Tag>();

	// Token: 0x04002F2C RID: 12076
	public List<Tag> tagOptions = new List<Tag>();

	// Token: 0x04002F2D RID: 12077
	public string headerText;

	// Token: 0x04002F2E RID: 12078
	public bool displayOnlyDiscoveredTags = true;
}
