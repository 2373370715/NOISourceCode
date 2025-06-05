using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02001E0B RID: 7691
public class NumericDropDownTableColumn : TableColumn
{
	// Token: 0x0600A0B6 RID: 41142 RVA: 0x0010D241 File Offset: 0x0010B441
	public NumericDropDownTableColumn(object user_data, List<TMP_Dropdown.OptionData> options, Action<IAssignableIdentity, GameObject> on_load_action, Action<GameObject, int> set_value_action, Comparison<IAssignableIdentity> sort_comparer, NumericDropDownTableColumn.ToolTipCallbacks callbacks, Func<bool> revealed = null) : base(on_load_action, sort_comparer, callbacks.headerTooltip, callbacks.headerSortTooltip, revealed, false, "")
	{
		this.userData = user_data;
		this.set_value_action = set_value_action;
		this.options = options;
		this.callbacks = callbacks;
	}

	// Token: 0x0600A0B7 RID: 41143 RVA: 0x0010D280 File Offset: 0x0010B480
	public override GameObject GetMinionWidget(GameObject parent)
	{
		return this.GetWidget(parent);
	}

	// Token: 0x0600A0B8 RID: 41144 RVA: 0x0010D280 File Offset: 0x0010B480
	public override GameObject GetDefaultWidget(GameObject parent)
	{
		return this.GetWidget(parent);
	}

	// Token: 0x0600A0B9 RID: 41145 RVA: 0x003E4D88 File Offset: 0x003E2F88
	private GameObject GetWidget(GameObject parent)
	{
		GameObject widget_go = Util.KInstantiateUI(Assets.UIPrefabs.TableScreenWidgets.NumericDropDown, parent, true);
		TMP_Dropdown componentInChildren = widget_go.transform.GetComponentInChildren<TMP_Dropdown>();
		componentInChildren.options = this.options;
		componentInChildren.onValueChanged.AddListener(delegate(int new_value)
		{
			this.set_value_action(widget_go, new_value);
		});
		ToolTip tt = widget_go.transform.GetComponentInChildren<ToolTip>();
		if (tt != null)
		{
			tt.OnToolTip = (() => this.GetTooltip(tt));
		}
		return widget_go;
	}

	// Token: 0x0600A0BA RID: 41146 RVA: 0x003E4E34 File Offset: 0x003E3034
	public override GameObject GetHeaderWidget(GameObject parent)
	{
		NumericDropDownTableColumn.<>c__DisplayClass9_0 CS$<>8__locals1 = new NumericDropDownTableColumn.<>c__DisplayClass9_0();
		CS$<>8__locals1.<>4__this = this;
		CS$<>8__locals1.widget_go = Util.KInstantiateUI(Assets.UIPrefabs.TableScreenWidgets.DropDownHeader, parent, true);
		HierarchyReferences component = CS$<>8__locals1.widget_go.GetComponent<HierarchyReferences>();
		Component reference = component.GetReference("Label");
		MultiToggle componentInChildren = reference.GetComponentInChildren<MultiToggle>(true);
		this.column_sort_toggle = componentInChildren;
		MultiToggle multiToggle = componentInChildren;
		multiToggle.onClick = (System.Action)Delegate.Combine(multiToggle.onClick, new System.Action(delegate()
		{
			CS$<>8__locals1.<>4__this.screen.SetSortComparison(CS$<>8__locals1.<>4__this.sort_comparer, CS$<>8__locals1.<>4__this);
			CS$<>8__locals1.<>4__this.screen.SortRows();
		}));
		ToolTip tt2 = reference.GetComponent<ToolTip>();
		tt2.enabled = true;
		tt2.OnToolTip = delegate()
		{
			CS$<>8__locals1.<>4__this.callbacks.headerTooltip(null, CS$<>8__locals1.widget_go, tt2);
			return "";
		};
		ToolTip tt3 = componentInChildren.transform.GetComponent<ToolTip>();
		tt3.OnToolTip = delegate()
		{
			CS$<>8__locals1.<>4__this.callbacks.headerSortTooltip(null, CS$<>8__locals1.widget_go, tt3);
			return "";
		};
		Component reference2 = component.GetReference("DropDown");
		TMP_Dropdown componentInChildren2 = reference2.GetComponentInChildren<TMP_Dropdown>();
		componentInChildren2.options = this.options;
		componentInChildren2.onValueChanged.AddListener(delegate(int new_value)
		{
			CS$<>8__locals1.<>4__this.set_value_action(CS$<>8__locals1.widget_go, new_value);
		});
		ToolTip tt = reference2.GetComponent<ToolTip>();
		tt.OnToolTip = delegate()
		{
			CS$<>8__locals1.<>4__this.callbacks.headerDropdownTooltip(null, CS$<>8__locals1.widget_go, tt);
			return "";
		};
		LayoutElement component2 = CS$<>8__locals1.widget_go.GetComponentInChildren<LocText>().GetComponent<LayoutElement>();
		component2.preferredWidth = (component2.minWidth = 83f);
		return CS$<>8__locals1.widget_go;
	}

	// Token: 0x04007E43 RID: 32323
	public object userData;

	// Token: 0x04007E44 RID: 32324
	private NumericDropDownTableColumn.ToolTipCallbacks callbacks;

	// Token: 0x04007E45 RID: 32325
	private Action<GameObject, int> set_value_action;

	// Token: 0x04007E46 RID: 32326
	private List<TMP_Dropdown.OptionData> options;

	// Token: 0x02001E0C RID: 7692
	public class ToolTipCallbacks
	{
		// Token: 0x04007E47 RID: 32327
		public Action<IAssignableIdentity, GameObject, ToolTip> headerTooltip;

		// Token: 0x04007E48 RID: 32328
		public Action<IAssignableIdentity, GameObject, ToolTip> headerSortTooltip;

		// Token: 0x04007E49 RID: 32329
		public Action<IAssignableIdentity, GameObject, ToolTip> headerDropdownTooltip;
	}
}
