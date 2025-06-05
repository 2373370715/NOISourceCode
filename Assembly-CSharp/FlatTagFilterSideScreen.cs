using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02001FD0 RID: 8144
public class FlatTagFilterSideScreen : SideScreenContent
{
	// Token: 0x0600AC11 RID: 44049 RVA: 0x00114475 File Offset: 0x00112675
	public override bool IsValidForTarget(GameObject target)
	{
		return target.GetComponent<FlatTagFilterable>() != null;
	}

	// Token: 0x0600AC12 RID: 44050 RVA: 0x00114483 File Offset: 0x00112683
	public override void SetTarget(GameObject target)
	{
		base.SetTarget(target);
		this.tagFilterable = target.GetComponent<FlatTagFilterable>();
		this.Build();
	}

	// Token: 0x0600AC13 RID: 44051 RVA: 0x0041C0E0 File Offset: 0x0041A2E0
	private void Build()
	{
		this.headerLabel.SetText(this.tagFilterable.GetHeaderText());
		foreach (KeyValuePair<Tag, GameObject> keyValuePair in this.rows)
		{
			Util.KDestroyGameObject(keyValuePair.Value);
		}
		this.rows.Clear();
		foreach (Tag tag in this.tagFilterable.tagOptions)
		{
			GameObject gameObject = Util.KInstantiateUI(this.rowPrefab, this.listContainer, false);
			gameObject.gameObject.name = tag.ProperName();
			this.rows.Add(tag, gameObject);
		}
		this.Refresh();
	}

	// Token: 0x0600AC14 RID: 44052 RVA: 0x0041C1D4 File Offset: 0x0041A3D4
	private void Refresh()
	{
		using (Dictionary<Tag, GameObject>.Enumerator enumerator = this.rows.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				KeyValuePair<Tag, GameObject> kvp = enumerator.Current;
				kvp.Value.GetComponent<HierarchyReferences>().GetReference<LocText>("Label").SetText(kvp.Key.ProperNameStripLink());
				kvp.Value.GetComponent<HierarchyReferences>().GetReference<Image>("Icon").sprite = Def.GetUISprite(kvp.Key, "ui", false).first;
				kvp.Value.GetComponent<HierarchyReferences>().GetReference<Image>("Icon").color = Def.GetUISprite(kvp.Key, "ui", false).second;
				kvp.Value.GetComponent<HierarchyReferences>().GetReference<MultiToggle>("Toggle").onClick = delegate()
				{
					this.tagFilterable.ToggleTag(kvp.Key);
					this.Refresh();
				};
				kvp.Value.GetComponent<HierarchyReferences>().GetReference<MultiToggle>("Toggle").ChangeState(this.tagFilterable.selectedTags.Contains(kvp.Key) ? 1 : 0);
				kvp.Value.SetActive(!this.tagFilterable.displayOnlyDiscoveredTags || DiscoveredResources.Instance.IsDiscovered(kvp.Key));
			}
		}
	}

	// Token: 0x0600AC15 RID: 44053 RVA: 0x0011449E File Offset: 0x0011269E
	public override string GetTitle()
	{
		return this.tagFilterable.gameObject.GetProperName();
	}

	// Token: 0x0400877F RID: 34687
	private FlatTagFilterable tagFilterable;

	// Token: 0x04008780 RID: 34688
	[SerializeField]
	private GameObject rowPrefab;

	// Token: 0x04008781 RID: 34689
	[SerializeField]
	private GameObject listContainer;

	// Token: 0x04008782 RID: 34690
	[SerializeField]
	private LocText headerLabel;

	// Token: 0x04008783 RID: 34691
	private Dictionary<Tag, GameObject> rows = new Dictionary<Tag, GameObject>();
}
