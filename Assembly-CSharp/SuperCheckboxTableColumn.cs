using System;
using UnityEngine;

// Token: 0x02001E01 RID: 7681
public class SuperCheckboxTableColumn : CheckboxTableColumn
{
	// Token: 0x0600A094 RID: 41108 RVA: 0x003E4884 File Offset: 0x003E2A84
	public SuperCheckboxTableColumn(CheckboxTableColumn[] columns_affected, Action<IAssignableIdentity, GameObject> on_load_action, Func<IAssignableIdentity, GameObject, TableScreen.ResultValues> get_value_action, Action<GameObject> on_press_action, Action<GameObject, TableScreen.ResultValues> set_value_action, Comparison<IAssignableIdentity> sort_comparison, Action<IAssignableIdentity, GameObject, ToolTip> on_tooltip) : base(on_load_action, get_value_action, on_press_action, set_value_action, sort_comparison, on_tooltip, null, null)
	{
		this.columns_affected = columns_affected;
	}

	// Token: 0x0600A095 RID: 41109 RVA: 0x003E48C0 File Offset: 0x003E2AC0
	public override GameObject GetDefaultWidget(GameObject parent)
	{
		GameObject widget_go = Util.KInstantiateUI(this.prefab_super_checkbox, parent, true);
		if (widget_go.GetComponent<ToolTip>() != null)
		{
			widget_go.GetComponent<ToolTip>().OnToolTip = (() => this.GetTooltip(widget_go.GetComponent<ToolTip>()));
		}
		MultiToggle component = widget_go.GetComponent<MultiToggle>();
		component.onClick = (System.Action)Delegate.Combine(component.onClick, new System.Action(delegate()
		{
			this.on_press_action(widget_go);
		}));
		return widget_go;
	}

	// Token: 0x0600A096 RID: 41110 RVA: 0x003E4950 File Offset: 0x003E2B50
	public override GameObject GetHeaderWidget(GameObject parent)
	{
		GameObject widget_go = Util.KInstantiateUI(this.prefab_super_checkbox, parent, true);
		if (widget_go.GetComponent<ToolTip>() != null)
		{
			widget_go.GetComponent<ToolTip>().OnToolTip = (() => this.GetTooltip(widget_go.GetComponent<ToolTip>()));
		}
		MultiToggle component = widget_go.GetComponent<MultiToggle>();
		component.onClick = (System.Action)Delegate.Combine(component.onClick, new System.Action(delegate()
		{
			this.on_press_action(widget_go);
		}));
		return widget_go;
	}

	// Token: 0x0600A097 RID: 41111 RVA: 0x003E49E0 File Offset: 0x003E2BE0
	public override GameObject GetMinionWidget(GameObject parent)
	{
		GameObject widget_go = Util.KInstantiateUI(this.prefab_super_checkbox, parent, true);
		if (widget_go.GetComponent<ToolTip>() != null)
		{
			widget_go.GetComponent<ToolTip>().OnToolTip = (() => this.GetTooltip(widget_go.GetComponent<ToolTip>()));
		}
		MultiToggle component = widget_go.GetComponent<MultiToggle>();
		component.onClick = (System.Action)Delegate.Combine(component.onClick, new System.Action(delegate()
		{
			this.on_press_action(widget_go);
		}));
		return widget_go;
	}

	// Token: 0x04007E2C RID: 32300
	public GameObject prefab_super_checkbox = Assets.UIPrefabs.TableScreenWidgets.SuperCheckbox_Horizontal;

	// Token: 0x04007E2D RID: 32301
	public CheckboxTableColumn[] columns_affected;
}
