using System;
using System.Collections;
using System.Collections.Generic;
using STRINGS;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02001E34 RID: 7732
public class TableScreen : ShowOptimizedKScreen
{
	// Token: 0x0600A1A4 RID: 41380 RVA: 0x0010D937 File Offset: 0x0010BB37
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.removeWorldHandle = ClusterManager.Instance.Subscribe(-1078710002, new Action<object>(this.RemoveWorldDivider));
	}

	// Token: 0x0600A1A5 RID: 41381 RVA: 0x003EA5DC File Offset: 0x003E87DC
	protected override void OnActivate()
	{
		base.OnActivate();
		this.title_bar.text = this.title;
		base.ConsumeMouseScroll = true;
		this.CloseButton.onClick += delegate()
		{
			ManagementMenu.Instance.CloseAll();
		};
		this.incubating = true;
		base.transform.rectTransform().localScale = Vector3.zero;
		Components.LiveMinionIdentities.OnAdd += delegate(MinionIdentity param)
		{
			this.MarkRowsDirty();
		};
		Components.LiveMinionIdentities.OnRemove += delegate(MinionIdentity param)
		{
			this.MarkRowsDirty();
		};
	}

	// Token: 0x0600A1A6 RID: 41382 RVA: 0x0010D960 File Offset: 0x0010BB60
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		if (this.removeWorldHandle != -1)
		{
			ClusterManager.Instance.Unsubscribe(this.removeWorldHandle);
		}
	}

	// Token: 0x0600A1A7 RID: 41383 RVA: 0x0010D981 File Offset: 0x0010BB81
	protected override void OnShow(bool show)
	{
		if (!show)
		{
			this.active_cascade_coroutine_count = 0;
			base.StopAllCoroutines();
			this.StopLoopingCascadeSound();
		}
		this.ZeroScrollers();
		base.OnShow(show);
		if (show)
		{
			this.MarkRowsDirty();
		}
	}

	// Token: 0x0600A1A8 RID: 41384 RVA: 0x003EA67C File Offset: 0x003E887C
	private void ZeroScrollers()
	{
		if (this.rows.Count > 0)
		{
			foreach (string scrollerID in this.column_scrollers)
			{
				foreach (TableRow tableRow in this.rows)
				{
					tableRow.GetScroller(scrollerID).transform.parent.GetComponent<ScrollRect>().horizontalNormalizedPosition = 0f;
				}
			}
			foreach (KeyValuePair<int, GameObject> keyValuePair in this.worldDividers)
			{
				ScrollRect componentInChildren = keyValuePair.Value.GetComponentInChildren<ScrollRect>();
				if (componentInChildren != null)
				{
					componentInChildren.horizontalNormalizedPosition = 0f;
				}
			}
		}
	}

	// Token: 0x0600A1A9 RID: 41385 RVA: 0x0010D9AF File Offset: 0x0010BBAF
	public bool CheckScrollersDirty()
	{
		return this.scrollersDirty;
	}

	// Token: 0x0600A1AA RID: 41386 RVA: 0x0010D9B7 File Offset: 0x0010BBB7
	public void SetScrollersDirty(float position)
	{
		this.targetScrollerPosition = position;
		this.scrollersDirty = true;
		this.PositionScrollers();
	}

	// Token: 0x0600A1AB RID: 41387 RVA: 0x003EA794 File Offset: 0x003E8994
	public void PositionScrollers()
	{
		foreach (TableRow tableRow in this.rows)
		{
			ScrollRect componentInChildren = tableRow.GetComponentInChildren<ScrollRect>();
			if (componentInChildren != null)
			{
				componentInChildren.horizontalNormalizedPosition = this.targetScrollerPosition;
			}
		}
		foreach (KeyValuePair<int, GameObject> keyValuePair in this.worldDividers)
		{
			if (keyValuePair.Value.activeInHierarchy)
			{
				ScrollRect componentInChildren2 = keyValuePair.Value.GetComponentInChildren<ScrollRect>();
				if (componentInChildren2 != null)
				{
					componentInChildren2.horizontalNormalizedPosition = this.targetScrollerPosition;
				}
			}
		}
		this.scrollersDirty = false;
	}

	// Token: 0x0600A1AC RID: 41388 RVA: 0x003EA870 File Offset: 0x003E8A70
	public override void ScreenUpdate(bool topLevel)
	{
		if (this.isHiddenButActive)
		{
			return;
		}
		base.ScreenUpdate(topLevel);
		if (this.incubating)
		{
			this.ZeroScrollers();
			base.transform.rectTransform().localScale = Vector3.one;
			this.incubating = false;
		}
		if (this.rows_dirty)
		{
			this.RefreshRows();
		}
		foreach (TableRow tableRow in this.rows)
		{
			tableRow.RefreshScrollers();
		}
		foreach (TableColumn tableColumn in this.columns.Values)
		{
			if (tableColumn.isDirty)
			{
				foreach (KeyValuePair<TableRow, GameObject> keyValuePair in tableColumn.widgets_by_row)
				{
					tableColumn.on_load_action(keyValuePair.Key.GetIdentity(), keyValuePair.Value);
					tableColumn.MarkClean();
				}
			}
		}
	}

	// Token: 0x0600A1AD RID: 41389 RVA: 0x0010D9CD File Offset: 0x0010BBCD
	protected void MarkRowsDirty()
	{
		this.rows_dirty = true;
	}

	// Token: 0x0600A1AE RID: 41390 RVA: 0x003EA9B0 File Offset: 0x003E8BB0
	protected virtual void RefreshRows()
	{
		this.ObsoleteRows();
		this.AddRow(null);
		if (this.has_default_duplicant_row)
		{
			this.AddDefaultRow();
		}
		for (int i = 0; i < Components.LiveMinionIdentities.Count; i++)
		{
			if (Components.LiveMinionIdentities[i] != null)
			{
				this.AddRow(Components.LiveMinionIdentities[i]);
			}
		}
		foreach (MinionStorage minionStorage in Components.MinionStorages.Items)
		{
			foreach (MinionStorage.Info info in minionStorage.GetStoredMinionInfo())
			{
				if (info.serializedMinion != null)
				{
					StoredMinionIdentity minion = info.serializedMinion.Get<StoredMinionIdentity>();
					this.AddRow(minion);
				}
			}
		}
		foreach (int worldId in ClusterManager.Instance.GetWorldIDsSorted())
		{
			this.AddWorldDivider(worldId);
		}
		this.AddWorldDivider(255);
		foreach (KeyValuePair<int, bool> keyValuePair in this.obsoleteWorldDividerStatus)
		{
			if (keyValuePair.Value)
			{
				this.RemoveWorldDivider(keyValuePair.Key);
			}
		}
		this.obsoleteWorldDividerStatus.Clear();
		foreach (KeyValuePair<int, GameObject> keyValuePair2 in this.worldDividers)
		{
			Component reference = keyValuePair2.Value.GetComponent<HierarchyReferences>().GetReference("NobodyRow");
			bool flag = true;
			foreach (object obj in Components.MinionAssignablesProxy)
			{
				MinionAssignablesProxy minionAssignablesProxy = (MinionAssignablesProxy)obj;
				if (minionAssignablesProxy != null && minionAssignablesProxy.GetTargetGameObject() != null)
				{
					WorldContainer myWorld = minionAssignablesProxy.GetTargetGameObject().GetMyWorld();
					if (myWorld != null && myWorld.id == keyValuePair2.Key)
					{
						flag = false;
						break;
					}
					if (myWorld == null && keyValuePair2.Key == 255)
					{
						flag = false;
						break;
					}
				}
			}
			reference.gameObject.SetActive(flag);
			WorldContainer world = ClusterManager.Instance.GetWorld(keyValuePair2.Key);
			bool flag2 = DlcManager.FeatureClusterSpaceEnabled() && (world == null || ClusterManager.Instance.GetWorld(keyValuePair2.Key).IsDiscovered);
			if (world == null && flag)
			{
				flag2 = false;
			}
			if (keyValuePair2.Value.activeSelf != flag2)
			{
				keyValuePair2.Value.SetActive(flag2);
			}
		}
		using (Dictionary<IAssignableIdentity, bool>.Enumerator enumerator7 = this.obsoleteMinionRowStatus.GetEnumerator())
		{
			while (enumerator7.MoveNext())
			{
				KeyValuePair<IAssignableIdentity, bool> kvp = enumerator7.Current;
				if (kvp.Value)
				{
					int index = this.rows.FindIndex((TableRow match) => match.GetIdentity() == kvp.Key);
					TableRow item = this.rows[index];
					this.rows[index].Clear();
					this.rows.RemoveAt(index);
					this.all_sortable_rows.Remove(item);
				}
			}
		}
		this.obsoleteMinionRowStatus.Clear();
		this.SortRows();
		this.rows_dirty = false;
	}

	// Token: 0x0600A1AF RID: 41391 RVA: 0x003EAE08 File Offset: 0x003E9008
	public virtual void SetSortComparison(Comparison<IAssignableIdentity> comparison, TableColumn sort_column)
	{
		if (comparison == null)
		{
			return;
		}
		if (this.active_sort_column != sort_column)
		{
			this.active_sort_column = sort_column;
			this.active_sort_method = comparison;
			this.sort_is_reversed = false;
			return;
		}
		if (this.sort_is_reversed)
		{
			this.sort_is_reversed = false;
			this.active_sort_method = null;
			this.active_sort_column = null;
			return;
		}
		this.sort_is_reversed = true;
	}

	// Token: 0x0600A1B0 RID: 41392 RVA: 0x003EAE60 File Offset: 0x003E9060
	public void SortRows()
	{
		foreach (TableColumn tableColumn in this.columns.Values)
		{
			if (!(tableColumn.column_sort_toggle == null))
			{
				if (tableColumn == this.active_sort_column)
				{
					if (this.sort_is_reversed)
					{
						tableColumn.column_sort_toggle.ChangeState(2);
					}
					else
					{
						tableColumn.column_sort_toggle.ChangeState(1);
					}
				}
				else
				{
					tableColumn.column_sort_toggle.ChangeState(0);
				}
			}
		}
		Dictionary<IAssignableIdentity, TableRow> dictionary = new Dictionary<IAssignableIdentity, TableRow>();
		foreach (TableRow tableRow in this.all_sortable_rows)
		{
			dictionary.Add(tableRow.GetIdentity(), tableRow);
		}
		Dictionary<int, List<IAssignableIdentity>> dictionary2 = new Dictionary<int, List<IAssignableIdentity>>();
		foreach (KeyValuePair<IAssignableIdentity, TableRow> keyValuePair in dictionary)
		{
			WorldContainer myWorld = keyValuePair.Key.GetSoleOwner().GetComponent<MinionAssignablesProxy>().GetTargetGameObject().GetComponent<KMonoBehaviour>().GetMyWorld();
			int key = 255;
			if (myWorld != null)
			{
				key = myWorld.id;
			}
			if (!dictionary2.ContainsKey(key))
			{
				dictionary2.Add(key, new List<IAssignableIdentity>());
			}
			dictionary2[key].Add(keyValuePair.Key);
		}
		this.all_sortable_rows.Clear();
		Dictionary<int, int> dictionary3 = new Dictionary<int, int>();
		int num = 0;
		int num2 = 0;
		foreach (KeyValuePair<int, List<IAssignableIdentity>> keyValuePair2 in dictionary2)
		{
			dictionary3.Add(keyValuePair2.Key, num);
			num++;
			List<IAssignableIdentity> list = new List<IAssignableIdentity>();
			foreach (IAssignableIdentity item in keyValuePair2.Value)
			{
				list.Add(item);
			}
			if (this.active_sort_method != null)
			{
				list.Sort(this.active_sort_method);
				if (this.sort_is_reversed)
				{
					list.Reverse();
				}
			}
			num += list.Count;
			num2 += list.Count;
			for (int i = 0; i < list.Count; i++)
			{
				this.all_sortable_rows.Add(dictionary[list[i]]);
			}
		}
		for (int j = 0; j < this.all_sortable_rows.Count; j++)
		{
			this.all_sortable_rows[j].gameObject.transform.SetSiblingIndex(j);
		}
		foreach (KeyValuePair<int, int> keyValuePair3 in dictionary3)
		{
			this.worldDividers[keyValuePair3.Key].transform.SetSiblingIndex(keyValuePair3.Value);
		}
		if (this.has_default_duplicant_row)
		{
			this.default_row.transform.SetAsFirstSibling();
		}
	}

	// Token: 0x0600A1B1 RID: 41393 RVA: 0x0010D9D6 File Offset: 0x0010BBD6
	protected int compare_rows_alphabetical(IAssignableIdentity a, IAssignableIdentity b)
	{
		if (a == null && b == null)
		{
			return 0;
		}
		if (a == null)
		{
			return -1;
		}
		if (b == null)
		{
			return 1;
		}
		return a.GetProperName().CompareTo(b.GetProperName());
	}

	// Token: 0x0600A1B2 RID: 41394 RVA: 0x000B1628 File Offset: 0x000AF828
	protected int default_sort(TableRow a, TableRow b)
	{
		return 0;
	}

	// Token: 0x0600A1B3 RID: 41395 RVA: 0x003EB1C0 File Offset: 0x003E93C0
	protected void ObsoleteRows()
	{
		for (int i = this.rows.Count - 1; i >= 0; i--)
		{
			IAssignableIdentity identity = this.rows[i].GetIdentity();
			if (identity != null)
			{
				this.obsoleteMinionRowStatus.Add(identity, true);
			}
		}
		foreach (KeyValuePair<int, GameObject> keyValuePair in this.worldDividers)
		{
			this.obsoleteWorldDividerStatus.Add(keyValuePair.Key, true);
		}
	}

	// Token: 0x0600A1B4 RID: 41396 RVA: 0x003EB25C File Offset: 0x003E945C
	protected void AddRow(IAssignableIdentity minion)
	{
		bool flag = minion == null;
		if (!flag && this.obsoleteMinionRowStatus.ContainsKey(minion))
		{
			this.obsoleteMinionRowStatus[minion] = false;
			this.rows.Find((TableRow match) => match.GetIdentity() == minion).RefreshColumns(this.columns);
			return;
		}
		if (flag && this.header_row != null)
		{
			this.header_row.GetComponent<TableRow>().RefreshColumns(this.columns);
			return;
		}
		GameObject gameObject = Util.KInstantiateUI(flag ? this.prefab_row_header : this.prefab_row_empty, (minion == null) ? this.header_content_transform.gameObject : this.scroll_content_transform.gameObject, true);
		TableRow component = gameObject.GetComponent<TableRow>();
		component.rowType = (flag ? TableRow.RowType.Header : ((minion as MinionIdentity != null) ? TableRow.RowType.Minion : TableRow.RowType.StoredMinon));
		this.rows.Add(component);
		component.ConfigureContent(minion, this.columns, this);
		if (!flag)
		{
			this.all_sortable_rows.Add(component);
			return;
		}
		this.header_row = gameObject;
	}

	// Token: 0x0600A1B5 RID: 41397 RVA: 0x003EB38C File Offset: 0x003E958C
	protected void AddDefaultRow()
	{
		if (this.default_row != null)
		{
			this.default_row.GetComponent<TableRow>().RefreshColumns(this.columns);
			return;
		}
		GameObject gameObject = Util.KInstantiateUI(this.prefab_row_empty, this.scroll_content_transform.gameObject, true);
		this.default_row = gameObject;
		TableRow component = gameObject.GetComponent<TableRow>();
		component.rowType = TableRow.RowType.Default;
		component.isDefault = true;
		this.rows.Add(component);
		component.ConfigureContent(null, this.columns, this);
	}

	// Token: 0x0600A1B6 RID: 41398 RVA: 0x003EB40C File Offset: 0x003E960C
	protected void AddWorldDivider(int worldId)
	{
		if (this.obsoleteWorldDividerStatus.ContainsKey(worldId) && this.obsoleteWorldDividerStatus[worldId])
		{
			this.obsoleteWorldDividerStatus[worldId] = false;
			return;
		}
		GameObject gameObject = Util.KInstantiateUI(this.prefab_world_divider, this.scroll_content_transform.gameObject, true);
		gameObject.GetComponentInChildren<Image>().color = ClusterManager.worldColors[worldId % ClusterManager.worldColors.Length];
		RectTransform component = gameObject.GetComponentInChildren<LocText>().GetComponent<RectTransform>();
		component.sizeDelta = new Vector2(150f, component.sizeDelta.y);
		WorldContainer world = ClusterManager.Instance.GetWorld(worldId);
		if (world != null)
		{
			ClusterGridEntity component2 = world.GetComponent<ClusterGridEntity>();
			string str = (component2 is Clustercraft) ? NAMEGEN.WORLD.SPACECRAFT_PREFIX : NAMEGEN.WORLD.PLANETOID_PREFIX;
			gameObject.GetComponentInChildren<LocText>().SetText(str + component2.Name);
			gameObject.GetComponentInChildren<ToolTip>().SetSimpleTooltip(string.Format(NAMEGEN.WORLD.WORLDDIVIDER_TOOLTIP, component2.Name));
			gameObject.GetComponent<HierarchyReferences>().GetReference<Image>("Icon").sprite = component2.GetUISprite();
		}
		else
		{
			gameObject.GetComponentInChildren<LocText>().SetText(NAMEGEN.WORLD.UNKNOWN_WORLD);
			gameObject.GetComponentInChildren<ToolTip>().SetSimpleTooltip("");
			gameObject.GetComponent<HierarchyReferences>().GetReference<Image>("Icon").sprite = Assets.GetSprite("hex_unknown");
		}
		this.worldDividers.Add(worldId, gameObject);
		gameObject.GetComponent<TableRow>().ConfigureAsWorldDivider(this.columns, this);
	}

	// Token: 0x0600A1B7 RID: 41399 RVA: 0x003EB594 File Offset: 0x003E9794
	protected void RemoveWorldDivider(object worldId)
	{
		if (this.worldDividers.ContainsKey((int)worldId))
		{
			this.rows.Remove(this.worldDividers[(int)worldId].GetComponent<TableRow>());
			Util.KDestroyGameObject(this.worldDividers[(int)worldId]);
			this.worldDividers.Remove((int)worldId);
		}
	}

	// Token: 0x0600A1B8 RID: 41400 RVA: 0x003EB600 File Offset: 0x003E9800
	protected TableRow GetWidgetRow(GameObject widget_go)
	{
		if (widget_go == null)
		{
			global::Debug.LogWarning("Widget is null");
			return null;
		}
		if (this.known_widget_rows.ContainsKey(widget_go))
		{
			return this.known_widget_rows[widget_go];
		}
		foreach (TableRow tableRow in this.rows)
		{
			if (tableRow.rowType != TableRow.RowType.WorldDivider && tableRow.ContainsWidget(widget_go))
			{
				this.known_widget_rows.Add(widget_go, tableRow);
				return tableRow;
			}
		}
		global::Debug.LogWarning("Row is null for widget: " + widget_go.name + " parent is " + widget_go.transform.parent.name);
		return null;
	}

	// Token: 0x0600A1B9 RID: 41401 RVA: 0x003EB6CC File Offset: 0x003E98CC
	protected void StartScrollableContent(string scrollablePanelID)
	{
		if (!this.column_scrollers.Contains(scrollablePanelID))
		{
			DividerColumn new_column = new DividerColumn(() => true, "");
			this.RegisterColumn("scroller_spacer_" + scrollablePanelID, new_column);
			this.column_scrollers.Add(scrollablePanelID);
		}
	}

	// Token: 0x0600A1BA RID: 41402 RVA: 0x003EB730 File Offset: 0x003E9930
	protected PortraitTableColumn AddPortraitColumn(string id, Action<IAssignableIdentity, GameObject> on_load_action, Comparison<IAssignableIdentity> sort_comparison, bool double_click_to_target = true)
	{
		PortraitTableColumn portraitTableColumn = new PortraitTableColumn(on_load_action, sort_comparison, double_click_to_target);
		if (this.RegisterColumn(id, portraitTableColumn))
		{
			return portraitTableColumn;
		}
		return null;
	}

	// Token: 0x0600A1BB RID: 41403 RVA: 0x003EB754 File Offset: 0x003E9954
	protected ButtonLabelColumn AddButtonLabelColumn(string id, Action<IAssignableIdentity, GameObject> on_load_action, Func<IAssignableIdentity, GameObject, string> get_value_action, Action<GameObject> on_click_action, Action<GameObject> on_double_click_action, Comparison<IAssignableIdentity> sort_comparison, Action<IAssignableIdentity, GameObject, ToolTip> on_tooltip, Action<IAssignableIdentity, GameObject, ToolTip> on_sort_tooltip, bool whiteText = false)
	{
		ButtonLabelColumn buttonLabelColumn = new ButtonLabelColumn(on_load_action, get_value_action, on_click_action, on_double_click_action, sort_comparison, on_tooltip, on_sort_tooltip, whiteText);
		if (this.RegisterColumn(id, buttonLabelColumn))
		{
			return buttonLabelColumn;
		}
		return null;
	}

	// Token: 0x0600A1BC RID: 41404 RVA: 0x003EB784 File Offset: 0x003E9984
	protected LabelTableColumn AddLabelColumn(string id, Action<IAssignableIdentity, GameObject> on_load_action, Func<IAssignableIdentity, GameObject, string> get_value_action, Comparison<IAssignableIdentity> sort_comparison, Action<IAssignableIdentity, GameObject, ToolTip> on_tooltip, Action<IAssignableIdentity, GameObject, ToolTip> on_sort_tooltip, int widget_width = 128, bool should_refresh_columns = false)
	{
		LabelTableColumn labelTableColumn = new LabelTableColumn(on_load_action, get_value_action, sort_comparison, on_tooltip, on_sort_tooltip, widget_width, should_refresh_columns);
		if (this.RegisterColumn(id, labelTableColumn))
		{
			return labelTableColumn;
		}
		return null;
	}

	// Token: 0x0600A1BD RID: 41405 RVA: 0x003EB7B0 File Offset: 0x003E99B0
	protected CheckboxTableColumn AddCheckboxColumn(string id, Action<IAssignableIdentity, GameObject> on_load_action, Func<IAssignableIdentity, GameObject, TableScreen.ResultValues> get_value_action, Action<GameObject> on_press_action, Action<GameObject, TableScreen.ResultValues> set_value_function, Comparison<IAssignableIdentity> sort_comparison, Action<IAssignableIdentity, GameObject, ToolTip> on_tooltip, Action<IAssignableIdentity, GameObject, ToolTip> on_sort_tooltip)
	{
		CheckboxTableColumn checkboxTableColumn = new CheckboxTableColumn(on_load_action, get_value_action, on_press_action, set_value_function, sort_comparison, on_tooltip, on_sort_tooltip, null);
		if (this.RegisterColumn(id, checkboxTableColumn))
		{
			return checkboxTableColumn;
		}
		return null;
	}

	// Token: 0x0600A1BE RID: 41406 RVA: 0x003EB7E0 File Offset: 0x003E99E0
	protected SuperCheckboxTableColumn AddSuperCheckboxColumn(string id, CheckboxTableColumn[] columns_affected, Action<IAssignableIdentity, GameObject> on_load_action, Func<IAssignableIdentity, GameObject, TableScreen.ResultValues> get_value_action, Action<GameObject> on_press_action, Action<GameObject, TableScreen.ResultValues> set_value_action, Comparison<IAssignableIdentity> sort_comparison, Action<IAssignableIdentity, GameObject, ToolTip> on_tooltip)
	{
		SuperCheckboxTableColumn superCheckboxTableColumn = new SuperCheckboxTableColumn(columns_affected, on_load_action, get_value_action, on_press_action, set_value_action, sort_comparison, on_tooltip);
		if (this.RegisterColumn(id, superCheckboxTableColumn))
		{
			foreach (CheckboxTableColumn checkboxTableColumn in columns_affected)
			{
				checkboxTableColumn.on_set_action = (Action<GameObject, TableScreen.ResultValues>)Delegate.Combine(checkboxTableColumn.on_set_action, new Action<GameObject, TableScreen.ResultValues>(superCheckboxTableColumn.MarkDirty));
			}
			superCheckboxTableColumn.MarkDirty(null, TableScreen.ResultValues.False);
			return superCheckboxTableColumn;
		}
		global::Debug.LogWarning("SuperCheckbox column registration failed");
		return null;
	}

	// Token: 0x0600A1BF RID: 41407 RVA: 0x003EB854 File Offset: 0x003E9A54
	protected NumericDropDownTableColumn AddNumericDropDownColumn(string id, object user_data, List<TMP_Dropdown.OptionData> options, Action<IAssignableIdentity, GameObject> on_load_action, Action<GameObject, int> set_value_action, Comparison<IAssignableIdentity> sort_comparison, NumericDropDownTableColumn.ToolTipCallbacks tooltip_callbacks)
	{
		NumericDropDownTableColumn numericDropDownTableColumn = new NumericDropDownTableColumn(user_data, options, on_load_action, set_value_action, sort_comparison, tooltip_callbacks, null);
		if (this.RegisterColumn(id, numericDropDownTableColumn))
		{
			return numericDropDownTableColumn;
		}
		return null;
	}

	// Token: 0x0600A1C0 RID: 41408 RVA: 0x0010D9FB File Offset: 0x0010BBFB
	protected bool RegisterColumn(string id, TableColumn new_column)
	{
		if (this.columns.ContainsKey(id))
		{
			global::Debug.LogWarning(string.Format("Column with id {0} already in dictionary", id));
			return false;
		}
		new_column.screen = this;
		this.columns.Add(id, new_column);
		this.MarkRowsDirty();
		return true;
	}

	// Token: 0x0600A1C1 RID: 41409 RVA: 0x003EB880 File Offset: 0x003E9A80
	protected TableColumn GetWidgetColumn(GameObject widget_go)
	{
		if (this.known_widget_columns.ContainsKey(widget_go))
		{
			return this.known_widget_columns[widget_go];
		}
		foreach (KeyValuePair<string, TableColumn> keyValuePair in this.columns)
		{
			if (keyValuePair.Value.ContainsWidget(widget_go))
			{
				this.known_widget_columns.Add(widget_go, keyValuePair.Value);
				return keyValuePair.Value;
			}
		}
		global::Debug.LogWarning("No column found for widget gameobject " + widget_go.name);
		return null;
	}

	// Token: 0x0600A1C2 RID: 41410 RVA: 0x003EB92C File Offset: 0x003E9B2C
	protected void on_load_portrait(IAssignableIdentity minion, GameObject widget_go)
	{
		TableRow widgetRow = this.GetWidgetRow(widget_go);
		CrewPortrait component = widget_go.GetComponent<CrewPortrait>();
		if (minion != null)
		{
			component.SetIdentityObject(minion, false);
			component.ForceRefresh();
			return;
		}
		component.targetImage.enabled = (widgetRow.rowType == TableRow.RowType.Default);
	}

	// Token: 0x0600A1C3 RID: 41411 RVA: 0x003EB970 File Offset: 0x003E9B70
	protected void on_load_name_label(IAssignableIdentity minion, GameObject widget_go)
	{
		TableRow widgetRow = this.GetWidgetRow(widget_go);
		LocText locText = null;
		HierarchyReferences component = widget_go.GetComponent<HierarchyReferences>();
		LocText locText2 = component.GetReference("Label") as LocText;
		if (component.HasReference("SubLabel"))
		{
			locText = (component.GetReference("SubLabel") as LocText);
		}
		if (minion != null)
		{
			locText2.text = (this.GetWidgetColumn(widget_go) as LabelTableColumn).get_value_action(minion, widget_go);
			if (locText != null)
			{
				MinionIdentity minionIdentity = minion as MinionIdentity;
				if (minionIdentity != null)
				{
					locText.text = minionIdentity.gameObject.GetComponent<MinionResume>().GetSkillsSubtitle();
				}
				else
				{
					locText.text = "";
				}
				locText.enableWordWrapping = false;
				return;
			}
		}
		else
		{
			if (widgetRow.isDefault)
			{
				locText2.text = UI.JOBSCREEN_DEFAULT;
				if (locText != null && locText.gameObject.activeSelf)
				{
					locText.gameObject.SetActive(false);
				}
			}
			else
			{
				locText2.text = UI.JOBSCREEN_EVERYONE;
			}
			if (locText != null)
			{
				locText.text = "";
			}
		}
	}

	// Token: 0x0600A1C4 RID: 41412 RVA: 0x0010DA38 File Offset: 0x0010BC38
	protected string get_value_name_label(IAssignableIdentity minion, GameObject widget_go)
	{
		return minion.GetProperName();
	}

	// Token: 0x0600A1C5 RID: 41413 RVA: 0x003EBA8C File Offset: 0x003E9C8C
	protected void on_load_value_checkbox_column_super(IAssignableIdentity minion, GameObject widget_go)
	{
		MultiToggle component = widget_go.GetComponent<MultiToggle>();
		TableRow.RowType rowType = this.GetWidgetRow(widget_go).rowType;
		if (rowType <= TableRow.RowType.Minion)
		{
			component.ChangeState((int)this.get_value_checkbox_column_super(minion, widget_go));
		}
	}

	// Token: 0x0600A1C6 RID: 41414 RVA: 0x003EBAC4 File Offset: 0x003E9CC4
	public virtual TableScreen.ResultValues get_value_checkbox_column_super(IAssignableIdentity minion, GameObject widget_go)
	{
		SuperCheckboxTableColumn superCheckboxTableColumn = this.GetWidgetColumn(widget_go) as SuperCheckboxTableColumn;
		TableRow widgetRow = this.GetWidgetRow(widget_go);
		bool flag = true;
		bool flag2 = true;
		bool flag3 = false;
		bool flag4 = false;
		foreach (CheckboxTableColumn checkboxTableColumn in superCheckboxTableColumn.columns_affected)
		{
			if (checkboxTableColumn.isRevealed)
			{
				switch (checkboxTableColumn.get_value_action(widgetRow.GetIdentity(), widgetRow.GetWidget(checkboxTableColumn)))
				{
				case TableScreen.ResultValues.False:
					flag2 = false;
					if (!flag)
					{
					}
					break;
				case TableScreen.ResultValues.Partial:
					flag4 = true;
					break;
				case TableScreen.ResultValues.True:
					flag4 = true;
					flag = false;
					if (!flag2)
					{
					}
					break;
				case TableScreen.ResultValues.ConditionalGroup:
					flag3 = true;
					flag2 = false;
					flag = false;
					break;
				}
			}
		}
		TableScreen.ResultValues result = TableScreen.ResultValues.Partial;
		if (flag3 && !flag4 && !flag2 && !flag)
		{
			result = TableScreen.ResultValues.ConditionalGroup;
		}
		else if (flag2)
		{
			result = TableScreen.ResultValues.True;
		}
		else if (flag)
		{
			result = TableScreen.ResultValues.False;
		}
		else if (flag4)
		{
			result = TableScreen.ResultValues.Partial;
		}
		return result;
	}

	// Token: 0x0600A1C7 RID: 41415 RVA: 0x003EBBA8 File Offset: 0x003E9DA8
	protected void set_value_checkbox_column_super(GameObject widget_go, TableScreen.ResultValues new_value)
	{
		SuperCheckboxTableColumn superCheckboxTableColumn = this.GetWidgetColumn(widget_go) as SuperCheckboxTableColumn;
		TableRow widgetRow = this.GetWidgetRow(widget_go);
		switch (widgetRow.rowType)
		{
		case TableRow.RowType.Header:
			base.StartCoroutine(this.CascadeSetRowCheckBoxes(superCheckboxTableColumn.columns_affected, this.default_row.GetComponent<TableRow>(), new_value, widget_go));
			base.StartCoroutine(this.CascadeSetColumnCheckBoxes(this.all_sortable_rows, superCheckboxTableColumn, new_value, widget_go));
			return;
		case TableRow.RowType.Default:
			base.StartCoroutine(this.CascadeSetRowCheckBoxes(superCheckboxTableColumn.columns_affected, widgetRow, new_value, widget_go));
			return;
		case TableRow.RowType.Minion:
			base.StartCoroutine(this.CascadeSetRowCheckBoxes(superCheckboxTableColumn.columns_affected, widgetRow, new_value, widget_go));
			return;
		default:
			return;
		}
	}

	// Token: 0x0600A1C8 RID: 41416 RVA: 0x0010DA40 File Offset: 0x0010BC40
	protected IEnumerator CascadeSetRowCheckBoxes(CheckboxTableColumn[] checkBoxToggleColumns, TableRow row, TableScreen.ResultValues state, GameObject ignore_widget = null)
	{
		if (this.active_cascade_coroutine_count == 0)
		{
			this.current_looping_sound = LoopingSoundManager.StartSound(this.cascade_sound_path, Vector3.zero, false, false);
		}
		this.active_cascade_coroutine_count++;
		int num;
		for (int i = 0; i < checkBoxToggleColumns.Length; i = num + 1)
		{
			if (checkBoxToggleColumns[i].widgets_by_row.ContainsKey(row))
			{
				GameObject gameObject = checkBoxToggleColumns[i].widgets_by_row[row];
				if (!(gameObject == ignore_widget) && checkBoxToggleColumns[i].isRevealed)
				{
					bool flag = false;
					switch ((this.GetWidgetColumn(gameObject) as CheckboxTableColumn).get_value_action(row.GetIdentity(), gameObject))
					{
					case TableScreen.ResultValues.False:
						flag = (state != TableScreen.ResultValues.False);
						break;
					case TableScreen.ResultValues.Partial:
					case TableScreen.ResultValues.ConditionalGroup:
						flag = true;
						break;
					case TableScreen.ResultValues.True:
						flag = (state != TableScreen.ResultValues.True);
						break;
					}
					if (flag)
					{
						(this.GetWidgetColumn(gameObject) as CheckboxTableColumn).on_set_action(gameObject, state);
						yield return null;
					}
				}
			}
			num = i;
		}
		this.active_cascade_coroutine_count--;
		if (this.active_cascade_coroutine_count <= 0)
		{
			this.StopLoopingCascadeSound();
		}
		yield break;
	}

	// Token: 0x0600A1C9 RID: 41417 RVA: 0x0010DA6C File Offset: 0x0010BC6C
	protected IEnumerator CascadeSetColumnCheckBoxes(List<TableRow> rows, CheckboxTableColumn checkBoxToggleColumn, TableScreen.ResultValues state, GameObject header_widget_go = null)
	{
		if (this.active_cascade_coroutine_count == 0)
		{
			this.current_looping_sound = LoopingSoundManager.StartSound(this.cascade_sound_path, Vector3.zero, false, true);
		}
		this.active_cascade_coroutine_count++;
		int num;
		for (int i = 0; i < rows.Count; i = num + 1)
		{
			GameObject widget = rows[i].GetWidget(checkBoxToggleColumn);
			if (!(widget == header_widget_go))
			{
				bool flag = false;
				switch ((this.GetWidgetColumn(widget) as CheckboxTableColumn).get_value_action(rows[i].GetIdentity(), widget))
				{
				case TableScreen.ResultValues.False:
					flag = (state != TableScreen.ResultValues.False);
					break;
				case TableScreen.ResultValues.Partial:
				case TableScreen.ResultValues.ConditionalGroup:
					flag = true;
					break;
				case TableScreen.ResultValues.True:
					flag = (state != TableScreen.ResultValues.True);
					break;
				}
				if (flag)
				{
					(this.GetWidgetColumn(widget) as CheckboxTableColumn).on_set_action(widget, state);
					yield return null;
				}
			}
			num = i;
		}
		if (header_widget_go != null)
		{
			(this.GetWidgetColumn(header_widget_go) as CheckboxTableColumn).on_load_action(null, header_widget_go);
		}
		this.active_cascade_coroutine_count--;
		if (this.active_cascade_coroutine_count <= 0)
		{
			this.StopLoopingCascadeSound();
		}
		yield break;
	}

	// Token: 0x0600A1CA RID: 41418 RVA: 0x0010DA98 File Offset: 0x0010BC98
	private void StopLoopingCascadeSound()
	{
		if (this.current_looping_sound.IsValid())
		{
			LoopingSoundManager.StopSound(this.current_looping_sound);
			this.current_looping_sound.Clear();
		}
	}

	// Token: 0x0600A1CB RID: 41419 RVA: 0x003EBC48 File Offset: 0x003E9E48
	protected void on_press_checkbox_column_super(GameObject widget_go)
	{
		SuperCheckboxTableColumn superCheckboxTableColumn = this.GetWidgetColumn(widget_go) as SuperCheckboxTableColumn;
		TableRow widgetRow = this.GetWidgetRow(widget_go);
		switch (this.get_value_checkbox_column_super(widgetRow.GetIdentity(), widget_go))
		{
		case TableScreen.ResultValues.False:
			superCheckboxTableColumn.on_set_action(widget_go, TableScreen.ResultValues.True);
			break;
		case TableScreen.ResultValues.Partial:
		case TableScreen.ResultValues.ConditionalGroup:
			superCheckboxTableColumn.on_set_action(widget_go, TableScreen.ResultValues.True);
			break;
		case TableScreen.ResultValues.True:
			superCheckboxTableColumn.on_set_action(widget_go, TableScreen.ResultValues.False);
			break;
		}
		superCheckboxTableColumn.on_load_action(widgetRow.GetIdentity(), widget_go);
	}

	// Token: 0x0600A1CC RID: 41420 RVA: 0x003EBCD0 File Offset: 0x003E9ED0
	protected void on_tooltip_sort_alphabetically(IAssignableIdentity minion, GameObject widget_go, ToolTip tooltip)
	{
		tooltip.ClearMultiStringTooltip();
		switch (this.GetWidgetRow(widget_go).rowType)
		{
		case TableRow.RowType.Header:
			tooltip.AddMultiStringTooltip(UI.TABLESCREENS.COLUMN_SORT_BY_NAME, null);
			break;
		case TableRow.RowType.Default:
		case TableRow.RowType.Minion:
			break;
		default:
			return;
		}
	}

	// Token: 0x04007EC1 RID: 32449
	protected string title;

	// Token: 0x04007EC2 RID: 32450
	protected bool has_default_duplicant_row = true;

	// Token: 0x04007EC3 RID: 32451
	protected bool useWorldDividers = true;

	// Token: 0x04007EC4 RID: 32452
	private bool rows_dirty;

	// Token: 0x04007EC5 RID: 32453
	protected Comparison<IAssignableIdentity> active_sort_method;

	// Token: 0x04007EC6 RID: 32454
	protected TableColumn active_sort_column;

	// Token: 0x04007EC7 RID: 32455
	protected bool sort_is_reversed;

	// Token: 0x04007EC8 RID: 32456
	private int active_cascade_coroutine_count;

	// Token: 0x04007EC9 RID: 32457
	private HandleVector<int>.Handle current_looping_sound = HandleVector<int>.InvalidHandle;

	// Token: 0x04007ECA RID: 32458
	private bool incubating;

	// Token: 0x04007ECB RID: 32459
	private int removeWorldHandle = -1;

	// Token: 0x04007ECC RID: 32460
	protected Dictionary<string, TableColumn> columns = new Dictionary<string, TableColumn>();

	// Token: 0x04007ECD RID: 32461
	public List<TableRow> rows = new List<TableRow>();

	// Token: 0x04007ECE RID: 32462
	public List<TableRow> all_sortable_rows = new List<TableRow>();

	// Token: 0x04007ECF RID: 32463
	public List<string> column_scrollers = new List<string>();

	// Token: 0x04007ED0 RID: 32464
	private Dictionary<GameObject, TableRow> known_widget_rows = new Dictionary<GameObject, TableRow>();

	// Token: 0x04007ED1 RID: 32465
	private Dictionary<GameObject, TableColumn> known_widget_columns = new Dictionary<GameObject, TableColumn>();

	// Token: 0x04007ED2 RID: 32466
	public GameObject prefab_row_empty;

	// Token: 0x04007ED3 RID: 32467
	public GameObject prefab_row_header;

	// Token: 0x04007ED4 RID: 32468
	public GameObject prefab_world_divider;

	// Token: 0x04007ED5 RID: 32469
	public GameObject prefab_scroller_border;

	// Token: 0x04007ED6 RID: 32470
	private string cascade_sound_path = GlobalAssets.GetSound("Placers_Unfurl_LP", false);

	// Token: 0x04007ED7 RID: 32471
	public KButton CloseButton;

	// Token: 0x04007ED8 RID: 32472
	[MyCmpGet]
	private VerticalLayoutGroup VLG;

	// Token: 0x04007ED9 RID: 32473
	protected GameObject header_row;

	// Token: 0x04007EDA RID: 32474
	protected GameObject default_row;

	// Token: 0x04007EDB RID: 32475
	public LocText title_bar;

	// Token: 0x04007EDC RID: 32476
	public Transform header_content_transform;

	// Token: 0x04007EDD RID: 32477
	public Transform scroll_content_transform;

	// Token: 0x04007EDE RID: 32478
	public Transform scroller_borders_transform;

	// Token: 0x04007EDF RID: 32479
	public Dictionary<int, GameObject> worldDividers = new Dictionary<int, GameObject>();

	// Token: 0x04007EE0 RID: 32480
	private bool scrollersDirty;

	// Token: 0x04007EE1 RID: 32481
	private float targetScrollerPosition;

	// Token: 0x04007EE2 RID: 32482
	private Dictionary<IAssignableIdentity, bool> obsoleteMinionRowStatus = new Dictionary<IAssignableIdentity, bool>();

	// Token: 0x04007EE3 RID: 32483
	private Dictionary<int, bool> obsoleteWorldDividerStatus = new Dictionary<int, bool>();

	// Token: 0x02001E35 RID: 7733
	public enum ResultValues
	{
		// Token: 0x04007EE5 RID: 32485
		False,
		// Token: 0x04007EE6 RID: 32486
		Partial,
		// Token: 0x04007EE7 RID: 32487
		True,
		// Token: 0x04007EE8 RID: 32488
		ConditionalGroup,
		// Token: 0x04007EE9 RID: 32489
		NotApplicable
	}
}
