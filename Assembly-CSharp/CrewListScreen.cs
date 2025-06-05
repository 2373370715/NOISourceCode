using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02001DF2 RID: 7666
public class CrewListScreen<EntryType> : KScreen where EntryType : CrewListEntry
{
	// Token: 0x0600A040 RID: 41024 RVA: 0x0010CCAD File Offset: 0x0010AEAD
	protected override void OnActivate()
	{
		base.OnActivate();
		this.ClearEntries();
		this.SpawnEntries();
		this.PositionColumnTitles();
		if (this.autoColumn)
		{
			this.UpdateColumnTitles();
		}
		base.ConsumeMouseScroll = true;
	}

	// Token: 0x0600A041 RID: 41025 RVA: 0x0010CCDC File Offset: 0x0010AEDC
	protected override void OnCmpEnable()
	{
		if (this.autoColumn)
		{
			this.UpdateColumnTitles();
		}
		this.Reconstruct();
	}

	// Token: 0x0600A042 RID: 41026 RVA: 0x003E30A4 File Offset: 0x003E12A4
	private void ClearEntries()
	{
		for (int i = this.EntryObjects.Count - 1; i > -1; i--)
		{
			Util.KDestroyGameObject(this.EntryObjects[i]);
		}
		this.EntryObjects.Clear();
	}

	// Token: 0x0600A043 RID: 41027 RVA: 0x0010CCF2 File Offset: 0x0010AEF2
	protected void RefreshCrewPortraitContent()
	{
		this.EntryObjects.ForEach(delegate(EntryType eo)
		{
			eo.RefreshCrewPortraitContent();
		});
	}

	// Token: 0x0600A044 RID: 41028 RVA: 0x0010CD1E File Offset: 0x0010AF1E
	protected virtual void SpawnEntries()
	{
		if (this.EntryObjects.Count != 0)
		{
			this.ClearEntries();
		}
	}

	// Token: 0x0600A045 RID: 41029 RVA: 0x003E30EC File Offset: 0x003E12EC
	public override void ScreenUpdate(bool topLevel)
	{
		base.ScreenUpdate(topLevel);
		if (this.autoColumn)
		{
			this.UpdateColumnTitles();
		}
		bool flag = false;
		List<MinionIdentity> liveIdentities = new List<MinionIdentity>(Components.LiveMinionIdentities.Items);
		if (this.EntryObjects.Count != liveIdentities.Count || this.EntryObjects.FindAll((EntryType o) => liveIdentities.Contains(o.Identity)).Count != this.EntryObjects.Count)
		{
			flag = true;
		}
		if (flag)
		{
			this.Reconstruct();
		}
		this.UpdateScroll();
	}

	// Token: 0x0600A046 RID: 41030 RVA: 0x0010CD33 File Offset: 0x0010AF33
	public void Reconstruct()
	{
		this.ClearEntries();
		this.SpawnEntries();
	}

	// Token: 0x0600A047 RID: 41031 RVA: 0x003E3180 File Offset: 0x003E1380
	private void UpdateScroll()
	{
		if (this.PanelScrollbar)
		{
			if (this.EntryObjects.Count <= this.maxEntriesBeforeScroll)
			{
				this.PanelScrollbar.value = Mathf.Lerp(this.PanelScrollbar.value, 1f, 10f);
				this.PanelScrollbar.gameObject.SetActive(false);
				return;
			}
			this.PanelScrollbar.gameObject.SetActive(true);
		}
	}

	// Token: 0x0600A048 RID: 41032 RVA: 0x003E31F8 File Offset: 0x003E13F8
	private void SetHeadersActive(bool state)
	{
		for (int i = 0; i < this.ColumnTitlesContainer.childCount; i++)
		{
			this.ColumnTitlesContainer.GetChild(i).gameObject.SetActive(state);
		}
	}

	// Token: 0x0600A049 RID: 41033 RVA: 0x003E3234 File Offset: 0x003E1434
	protected virtual void PositionColumnTitles()
	{
		if (this.ColumnTitlesContainer == null)
		{
			return;
		}
		if (this.EntryObjects.Count <= 0)
		{
			this.SetHeadersActive(false);
			return;
		}
		this.SetHeadersActive(true);
		int childCount = this.EntryObjects[0].transform.childCount;
		for (int i = 0; i < childCount; i++)
		{
			OverviewColumnIdentity component = this.EntryObjects[0].transform.GetChild(i).GetComponent<OverviewColumnIdentity>();
			if (component != null)
			{
				GameObject gameObject = Util.KInstantiate(this.Prefab_ColumnTitle, null, null);
				gameObject.name = component.Column_DisplayName;
				LocText componentInChildren = gameObject.GetComponentInChildren<LocText>();
				gameObject.transform.SetParent(this.ColumnTitlesContainer);
				componentInChildren.text = (component.StringLookup ? Strings.Get(component.Column_DisplayName) : component.Column_DisplayName);
				gameObject.GetComponent<ToolTip>().toolTip = string.Format(UI.TOOLTIPS.SORTCOLUMN, componentInChildren.text);
				gameObject.rectTransform().anchoredPosition = new Vector2(component.rectTransform().anchoredPosition.x, 0f);
				OverviewColumnIdentity overviewColumnIdentity = gameObject.GetComponent<OverviewColumnIdentity>();
				if (overviewColumnIdentity == null)
				{
					overviewColumnIdentity = gameObject.AddComponent<OverviewColumnIdentity>();
				}
				overviewColumnIdentity.Column_DisplayName = component.Column_DisplayName;
				overviewColumnIdentity.columnID = component.columnID;
				overviewColumnIdentity.xPivot = component.xPivot;
				overviewColumnIdentity.Sortable = component.Sortable;
				if (overviewColumnIdentity.Sortable)
				{
					overviewColumnIdentity.GetComponentInChildren<ImageToggleState>(true).gameObject.SetActive(true);
				}
			}
		}
		this.UpdateColumnTitles();
		this.sortToggleGroup = base.gameObject.AddComponent<ToggleGroup>();
		this.sortToggleGroup.allowSwitchOff = true;
	}

	// Token: 0x0600A04A RID: 41034 RVA: 0x003E33F8 File Offset: 0x003E15F8
	protected void SortByName(bool reverse)
	{
		List<EntryType> list = new List<EntryType>(this.EntryObjects);
		list.Sort(delegate(EntryType a, EntryType b)
		{
			string text = a.Identity.GetProperName() + a.gameObject.GetInstanceID().ToString();
			string strB = b.Identity.GetProperName() + b.gameObject.GetInstanceID().ToString();
			return text.CompareTo(strB);
		});
		this.ReorderEntries(list, reverse);
	}

	// Token: 0x0600A04B RID: 41035 RVA: 0x003E3440 File Offset: 0x003E1640
	protected void UpdateColumnTitles()
	{
		if (this.EntryObjects.Count <= 0 || !this.EntryObjects[0].gameObject.activeSelf)
		{
			this.SetHeadersActive(false);
			return;
		}
		this.SetHeadersActive(true);
		for (int i = 0; i < this.ColumnTitlesContainer.childCount; i++)
		{
			RectTransform rectTransform = this.ColumnTitlesContainer.GetChild(i).rectTransform();
			for (int j = 0; j < this.EntryObjects[0].transform.childCount; j++)
			{
				OverviewColumnIdentity component = this.EntryObjects[0].transform.GetChild(j).GetComponent<OverviewColumnIdentity>();
				if (component != null && component.Column_DisplayName == rectTransform.name)
				{
					rectTransform.pivot = new Vector2(component.xPivot, rectTransform.pivot.y);
					rectTransform.anchoredPosition = new Vector2(component.rectTransform().anchoredPosition.x + this.columnTitleHorizontalOffset, 0f);
					rectTransform.sizeDelta = new Vector2(component.rectTransform().sizeDelta.x, rectTransform.sizeDelta.y);
					if (rectTransform.anchoredPosition.x == 0f)
					{
						rectTransform.gameObject.SetActive(false);
					}
					else
					{
						rectTransform.gameObject.SetActive(true);
					}
				}
			}
		}
	}

	// Token: 0x0600A04C RID: 41036 RVA: 0x003E35BC File Offset: 0x003E17BC
	protected void ReorderEntries(List<EntryType> sortedEntries, bool reverse)
	{
		for (int i = 0; i < sortedEntries.Count; i++)
		{
			if (reverse)
			{
				sortedEntries[i].transform.SetSiblingIndex(sortedEntries.Count - 1 - i);
			}
			else
			{
				sortedEntries[i].transform.SetSiblingIndex(i);
			}
		}
	}

	// Token: 0x04007DE8 RID: 32232
	public GameObject Prefab_CrewEntry;

	// Token: 0x04007DE9 RID: 32233
	public List<EntryType> EntryObjects = new List<EntryType>();

	// Token: 0x04007DEA RID: 32234
	public Transform ScrollRectTransform;

	// Token: 0x04007DEB RID: 32235
	public Transform EntriesPanelTransform;

	// Token: 0x04007DEC RID: 32236
	protected Vector2 EntryRectSize = new Vector2(750f, 64f);

	// Token: 0x04007DED RID: 32237
	public int maxEntriesBeforeScroll = 5;

	// Token: 0x04007DEE RID: 32238
	public Scrollbar PanelScrollbar;

	// Token: 0x04007DEF RID: 32239
	protected ToggleGroup sortToggleGroup;

	// Token: 0x04007DF0 RID: 32240
	protected Toggle lastSortToggle;

	// Token: 0x04007DF1 RID: 32241
	protected bool lastSortReversed;

	// Token: 0x04007DF2 RID: 32242
	public GameObject Prefab_ColumnTitle;

	// Token: 0x04007DF3 RID: 32243
	public Transform ColumnTitlesContainer;

	// Token: 0x04007DF4 RID: 32244
	public bool autoColumn;

	// Token: 0x04007DF5 RID: 32245
	public float columnTitleHorizontalOffset;
}
