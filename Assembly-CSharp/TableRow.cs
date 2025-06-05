using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02001E2F RID: 7727
[AddComponentMenu("KMonoBehaviour/scripts/TableRow")]
public class TableRow : KMonoBehaviour
{
	// Token: 0x0600A191 RID: 41361 RVA: 0x003E9CF0 File Offset: 0x003E7EF0
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		if (this.selectMinionButton != null)
		{
			this.selectMinionButton.onClick += this.SelectMinion;
			this.selectMinionButton.onDoubleClick += this.SelectAndFocusMinion;
		}
	}

	// Token: 0x0600A192 RID: 41362 RVA: 0x0010D886 File Offset: 0x0010BA86
	public GameObject GetScroller(string scrollerID)
	{
		return this.scrollers[scrollerID];
	}

	// Token: 0x0600A193 RID: 41363 RVA: 0x0010D894 File Offset: 0x0010BA94
	public GameObject GetScrollerBorder(string scrolledID)
	{
		return this.scrollerBorders[scrolledID];
	}

	// Token: 0x0600A194 RID: 41364 RVA: 0x003E9D40 File Offset: 0x003E7F40
	public void SelectMinion()
	{
		MinionIdentity minionIdentity = this.minion as MinionIdentity;
		if (minionIdentity == null)
		{
			return;
		}
		SelectTool.Instance.Select(minionIdentity.GetComponent<KSelectable>(), false);
	}

	// Token: 0x0600A195 RID: 41365 RVA: 0x003E9D74 File Offset: 0x003E7F74
	public void SelectAndFocusMinion()
	{
		MinionIdentity minionIdentity = this.minion as MinionIdentity;
		if (minionIdentity == null)
		{
			return;
		}
		SelectTool.Instance.SelectAndFocus(minionIdentity.transform.GetPosition(), minionIdentity.GetComponent<KSelectable>(), new Vector3(8f, 0f, 0f));
	}

	// Token: 0x0600A196 RID: 41366 RVA: 0x003E9DC8 File Offset: 0x003E7FC8
	public void ConfigureAsWorldDivider(Dictionary<string, TableColumn> columns, TableScreen screen)
	{
		ScrollRect scroll_rect = base.gameObject.GetComponentInChildren<ScrollRect>();
		this.rowType = TableRow.RowType.WorldDivider;
		foreach (KeyValuePair<string, TableColumn> keyValuePair in columns)
		{
			if (keyValuePair.Value.scrollerID != "")
			{
				TableColumn value = keyValuePair.Value;
				break;
			}
		}
		scroll_rect.onValueChanged.AddListener(delegate(Vector2 <p0>)
		{
			if (screen.CheckScrollersDirty())
			{
				return;
			}
			screen.SetScrollersDirty(scroll_rect.horizontalNormalizedPosition);
		});
	}

	// Token: 0x0600A197 RID: 41367 RVA: 0x003E9E74 File Offset: 0x003E8074
	public void ConfigureContent(IAssignableIdentity minion, Dictionary<string, TableColumn> columns, TableScreen screen)
	{
		this.minion = minion;
		KImage componentInChildren = base.GetComponentInChildren<KImage>(true);
		componentInChildren.colorStyleSetting = ((minion == null) ? this.style_setting_default : this.style_setting_minion);
		componentInChildren.ColorState = KImage.ColorSelector.Inactive;
		CanvasGroup component = base.GetComponent<CanvasGroup>();
		if (component != null && minion as StoredMinionIdentity != null)
		{
			component.alpha = 0.6f;
		}
		foreach (KeyValuePair<string, TableColumn> keyValuePair in columns)
		{
			GameObject gameObject;
			if (minion == null)
			{
				if (this.isDefault)
				{
					gameObject = keyValuePair.Value.GetDefaultWidget(base.gameObject);
				}
				else
				{
					gameObject = keyValuePair.Value.GetHeaderWidget(base.gameObject);
				}
			}
			else
			{
				gameObject = keyValuePair.Value.GetMinionWidget(base.gameObject);
			}
			this.widgets.Add(keyValuePair.Value, gameObject);
			keyValuePair.Value.widgets_by_row.Add(this, gameObject);
			if (keyValuePair.Value.scrollerID != "")
			{
				foreach (string text in keyValuePair.Value.screen.column_scrollers)
				{
					if (!(text != keyValuePair.Value.scrollerID))
					{
						if (!this.scrollers.ContainsKey(text))
						{
							GameObject gameObject2 = Util.KInstantiateUI(this.scrollerPrefab, base.gameObject, true);
							ScrollRect scroll_rect = gameObject2.GetComponent<ScrollRect>();
							this.scrollbar = gameObject2.GetComponentInChildren<Scrollbar>();
							scroll_rect.horizontalScrollbar = this.scrollbar;
							scroll_rect.horizontalScrollbarVisibility = ScrollRect.ScrollbarVisibility.AutoHide;
							scroll_rect.onValueChanged.AddListener(delegate(Vector2 <p0>)
							{
								if (screen.CheckScrollersDirty())
								{
									return;
								}
								screen.SetScrollersDirty(scroll_rect.horizontalNormalizedPosition);
							});
							this.scrollers.Add(text, scroll_rect.content.gameObject);
							if (scroll_rect.content.transform.parent.Find("Border") != null)
							{
								this.scrollerBorders.Add(text, scroll_rect.content.transform.parent.Find("Border").gameObject);
							}
						}
						gameObject.transform.SetParent(this.scrollers[text].transform);
						this.scrollers[text].transform.parent.GetComponent<ScrollRect>().horizontalNormalizedPosition = 0f;
					}
				}
			}
		}
		this.RefreshColumns(columns);
		if (minion != null)
		{
			base.gameObject.name = minion.GetProperName();
		}
		else if (this.isDefault)
		{
			base.gameObject.name = "defaultRow";
		}
		if (this.selectMinionButton)
		{
			this.selectMinionButton.transform.SetAsLastSibling();
		}
		foreach (KeyValuePair<string, GameObject> keyValuePair2 in this.scrollerBorders)
		{
			RectTransform rectTransform = keyValuePair2.Value.rectTransform();
			float width = rectTransform.rect.width;
			keyValuePair2.Value.transform.SetParent(base.gameObject.transform);
			rectTransform.anchorMin = (rectTransform.anchorMax = new Vector2(0f, 1f));
			rectTransform.sizeDelta = new Vector2(width, rectTransform.sizeDelta.y);
			RectTransform rectTransform2 = this.scrollers[keyValuePair2.Key].transform.parent.rectTransform();
			Vector3 a = this.scrollers[keyValuePair2.Key].transform.parent.rectTransform().GetLocalPosition() - new Vector3(rectTransform2.sizeDelta.x / 2f, -1f * (rectTransform2.sizeDelta.y / 2f), 0f);
			a.y = 0f;
			rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, 374f);
			rectTransform.SetLocalPosition(a + Vector3.up * rectTransform.GetLocalPosition().y + Vector3.up * -rectTransform.anchoredPosition.y);
		}
	}

	// Token: 0x0600A198 RID: 41368 RVA: 0x003EA384 File Offset: 0x003E8584
	public void RefreshColumns(Dictionary<string, TableColumn> columns)
	{
		foreach (KeyValuePair<string, TableColumn> keyValuePair in columns)
		{
			if (keyValuePair.Value.on_load_action != null)
			{
				keyValuePair.Value.on_load_action(this.minion, keyValuePair.Value.widgets_by_row[this]);
			}
		}
	}

	// Token: 0x0600A199 RID: 41369 RVA: 0x003EA404 File Offset: 0x003E8604
	public void RefreshScrollers()
	{
		foreach (KeyValuePair<string, GameObject> keyValuePair in this.scrollers)
		{
			ScrollRect component = keyValuePair.Value.transform.parent.GetComponent<ScrollRect>();
			component.GetComponent<LayoutElement>().minWidth = Mathf.Min(768f, component.content.sizeDelta.x);
		}
		foreach (KeyValuePair<string, GameObject> keyValuePair2 in this.scrollerBorders)
		{
			RectTransform rectTransform = keyValuePair2.Value.rectTransform();
			rectTransform.sizeDelta = new Vector2(this.scrollers[keyValuePair2.Key].transform.parent.GetComponent<LayoutElement>().minWidth, rectTransform.sizeDelta.y);
		}
	}

	// Token: 0x0600A19A RID: 41370 RVA: 0x003EA514 File Offset: 0x003E8714
	public GameObject GetWidget(TableColumn column)
	{
		if (this.widgets.ContainsKey(column) && this.widgets[column] != null)
		{
			return this.widgets[column];
		}
		global::Debug.LogWarning("Widget is null or row does not contain widget for column " + ((column != null) ? column.ToString() : null));
		return null;
	}

	// Token: 0x0600A19B RID: 41371 RVA: 0x0010D8A2 File Offset: 0x0010BAA2
	public IAssignableIdentity GetIdentity()
	{
		return this.minion;
	}

	// Token: 0x0600A19C RID: 41372 RVA: 0x0010D8AA File Offset: 0x0010BAAA
	public bool ContainsWidget(GameObject widget)
	{
		return this.widgets.ContainsValue(widget);
	}

	// Token: 0x0600A19D RID: 41373 RVA: 0x003EA570 File Offset: 0x003E8770
	public void Clear()
	{
		foreach (KeyValuePair<TableColumn, GameObject> keyValuePair in this.widgets)
		{
			keyValuePair.Key.widgets_by_row.Remove(this);
		}
		UnityEngine.Object.Destroy(base.gameObject);
	}

	// Token: 0x04007EAB RID: 32427
	public TableRow.RowType rowType;

	// Token: 0x04007EAC RID: 32428
	private IAssignableIdentity minion;

	// Token: 0x04007EAD RID: 32429
	private Dictionary<TableColumn, GameObject> widgets = new Dictionary<TableColumn, GameObject>();

	// Token: 0x04007EAE RID: 32430
	private Dictionary<string, GameObject> scrollers = new Dictionary<string, GameObject>();

	// Token: 0x04007EAF RID: 32431
	private Dictionary<string, GameObject> scrollerBorders = new Dictionary<string, GameObject>();

	// Token: 0x04007EB0 RID: 32432
	public bool isDefault;

	// Token: 0x04007EB1 RID: 32433
	public KButton selectMinionButton;

	// Token: 0x04007EB2 RID: 32434
	[SerializeField]
	private ColorStyleSetting style_setting_default;

	// Token: 0x04007EB3 RID: 32435
	[SerializeField]
	private ColorStyleSetting style_setting_minion;

	// Token: 0x04007EB4 RID: 32436
	[SerializeField]
	private GameObject scrollerPrefab;

	// Token: 0x04007EB5 RID: 32437
	[SerializeField]
	private Scrollbar scrollbar;

	// Token: 0x02001E30 RID: 7728
	public enum RowType
	{
		// Token: 0x04007EB7 RID: 32439
		Header,
		// Token: 0x04007EB8 RID: 32440
		Default,
		// Token: 0x04007EB9 RID: 32441
		Minion,
		// Token: 0x04007EBA RID: 32442
		StoredMinon,
		// Token: 0x04007EBB RID: 32443
		WorldDivider
	}
}
