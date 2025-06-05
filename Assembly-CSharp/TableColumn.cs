using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02001E2C RID: 7724
public class TableColumn : IRender1000ms
{
	// Token: 0x17000A7D RID: 2685
	// (get) Token: 0x0600A17F RID: 41343 RVA: 0x0010D7DA File Offset: 0x0010B9DA
	public bool isRevealed
	{
		get
		{
			return this.revealed == null || this.revealed();
		}
	}

	// Token: 0x0600A180 RID: 41344 RVA: 0x003E9AF0 File Offset: 0x003E7CF0
	public TableColumn(Action<IAssignableIdentity, GameObject> on_load_action, Comparison<IAssignableIdentity> sort_comparison, Action<IAssignableIdentity, GameObject, ToolTip> on_tooltip = null, Action<IAssignableIdentity, GameObject, ToolTip> on_sort_tooltip = null, Func<bool> revealed = null, bool should_refresh_columns = false, string scrollerID = "")
	{
		this.on_load_action = on_load_action;
		this.sort_comparer = sort_comparison;
		this.on_tooltip = on_tooltip;
		this.on_sort_tooltip = on_sort_tooltip;
		this.revealed = revealed;
		this.scrollerID = scrollerID;
		if (should_refresh_columns)
		{
			SimAndRenderScheduler.instance.Add(this, false);
		}
	}

	// Token: 0x0600A181 RID: 41345 RVA: 0x003E9B4C File Offset: 0x003E7D4C
	protected string GetTooltip(ToolTip tool_tip_instance)
	{
		GameObject gameObject = tool_tip_instance.gameObject;
		HierarchyReferences component = tool_tip_instance.GetComponent<HierarchyReferences>();
		if (component != null && component.HasReference("Widget"))
		{
			gameObject = component.GetReference("Widget").gameObject;
		}
		TableRow tableRow = null;
		foreach (KeyValuePair<TableRow, GameObject> keyValuePair in this.widgets_by_row)
		{
			if (keyValuePair.Value == gameObject)
			{
				tableRow = keyValuePair.Key;
				break;
			}
		}
		if (tableRow != null && this.on_tooltip != null)
		{
			this.on_tooltip(tableRow.GetIdentity(), gameObject, tool_tip_instance);
		}
		return "";
	}

	// Token: 0x0600A182 RID: 41346 RVA: 0x003E9C14 File Offset: 0x003E7E14
	protected string GetSortTooltip(ToolTip sort_tooltip_instance)
	{
		GameObject gameObject = sort_tooltip_instance.transform.parent.gameObject;
		TableRow tableRow = null;
		foreach (KeyValuePair<TableRow, GameObject> keyValuePair in this.widgets_by_row)
		{
			if (keyValuePair.Value == gameObject)
			{
				tableRow = keyValuePair.Key;
				break;
			}
		}
		if (tableRow != null && this.on_sort_tooltip != null)
		{
			this.on_sort_tooltip(tableRow.GetIdentity(), gameObject, sort_tooltip_instance);
		}
		return "";
	}

	// Token: 0x17000A7E RID: 2686
	// (get) Token: 0x0600A183 RID: 41347 RVA: 0x0010D7F1 File Offset: 0x0010B9F1
	public bool isDirty
	{
		get
		{
			return this.dirty;
		}
	}

	// Token: 0x0600A184 RID: 41348 RVA: 0x0010D7F9 File Offset: 0x0010B9F9
	public bool ContainsWidget(GameObject widget)
	{
		return this.widgets_by_row.ContainsValue(widget);
	}

	// Token: 0x0600A185 RID: 41349 RVA: 0x0010D807 File Offset: 0x0010BA07
	public virtual GameObject GetMinionWidget(GameObject parent)
	{
		global::Debug.LogError("Table Column has no Widget prefab");
		return null;
	}

	// Token: 0x0600A186 RID: 41350 RVA: 0x0010D807 File Offset: 0x0010BA07
	public virtual GameObject GetHeaderWidget(GameObject parent)
	{
		global::Debug.LogError("Table Column has no Widget prefab");
		return null;
	}

	// Token: 0x0600A187 RID: 41351 RVA: 0x0010D807 File Offset: 0x0010BA07
	public virtual GameObject GetDefaultWidget(GameObject parent)
	{
		global::Debug.LogError("Table Column has no Widget prefab");
		return null;
	}

	// Token: 0x0600A188 RID: 41352 RVA: 0x0010D814 File Offset: 0x0010BA14
	public void Render1000ms(float dt)
	{
		this.MarkDirty(null, TableScreen.ResultValues.False);
	}

	// Token: 0x0600A189 RID: 41353 RVA: 0x0010D81E File Offset: 0x0010BA1E
	public void MarkDirty(GameObject triggering_obj = null, TableScreen.ResultValues triggering_object_state = TableScreen.ResultValues.False)
	{
		this.dirty = true;
	}

	// Token: 0x0600A18A RID: 41354 RVA: 0x0010D827 File Offset: 0x0010BA27
	public void MarkClean()
	{
		this.dirty = false;
	}

	// Token: 0x04007EA0 RID: 32416
	public Action<IAssignableIdentity, GameObject> on_load_action;

	// Token: 0x04007EA1 RID: 32417
	public Action<IAssignableIdentity, GameObject, ToolTip> on_tooltip;

	// Token: 0x04007EA2 RID: 32418
	public Action<IAssignableIdentity, GameObject, ToolTip> on_sort_tooltip;

	// Token: 0x04007EA3 RID: 32419
	public Comparison<IAssignableIdentity> sort_comparer;

	// Token: 0x04007EA4 RID: 32420
	public Dictionary<TableRow, GameObject> widgets_by_row = new Dictionary<TableRow, GameObject>();

	// Token: 0x04007EA5 RID: 32421
	public string scrollerID;

	// Token: 0x04007EA6 RID: 32422
	public TableScreen screen;

	// Token: 0x04007EA7 RID: 32423
	public MultiToggle column_sort_toggle;

	// Token: 0x04007EA8 RID: 32424
	private Func<bool> revealed;

	// Token: 0x04007EA9 RID: 32425
	protected bool dirty;
}
