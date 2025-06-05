using System;
using UnityEngine;

// Token: 0x02001E08 RID: 7688
public class ButtonLabelColumn : LabelTableColumn
{
	// Token: 0x0600A0AB RID: 41131 RVA: 0x0010D198 File Offset: 0x0010B398
	public ButtonLabelColumn(Action<IAssignableIdentity, GameObject> on_load_action, Func<IAssignableIdentity, GameObject, string> get_value_action, Action<GameObject> on_click_action, Action<GameObject> on_double_click_action, Comparison<IAssignableIdentity> sort_comparison, Action<IAssignableIdentity, GameObject, ToolTip> on_tooltip, Action<IAssignableIdentity, GameObject, ToolTip> on_sort_tooltip, bool whiteText = false) : base(on_load_action, get_value_action, sort_comparison, on_tooltip, on_sort_tooltip, 128, false)
	{
		this.on_click_action = on_click_action;
		this.on_double_click_action = on_double_click_action;
		this.whiteText = whiteText;
	}

	// Token: 0x0600A0AC RID: 41132 RVA: 0x003E4C20 File Offset: 0x003E2E20
	public override GameObject GetDefaultWidget(GameObject parent)
	{
		GameObject widget_go = Util.KInstantiateUI(this.whiteText ? Assets.UIPrefabs.TableScreenWidgets.ButtonLabelWhite : Assets.UIPrefabs.TableScreenWidgets.ButtonLabel, parent, true);
		if (this.on_click_action != null)
		{
			widget_go.GetComponent<KButton>().onClick += delegate()
			{
				this.on_click_action(widget_go);
			};
		}
		if (this.on_double_click_action != null)
		{
			widget_go.GetComponent<KButton>().onDoubleClick += delegate()
			{
				this.on_double_click_action(widget_go);
			};
		}
		return widget_go;
	}

	// Token: 0x0600A0AD RID: 41133 RVA: 0x0010D1C5 File Offset: 0x0010B3C5
	public override GameObject GetHeaderWidget(GameObject parent)
	{
		return base.GetHeaderWidget(parent);
	}

	// Token: 0x0600A0AE RID: 41134 RVA: 0x003E4CC0 File Offset: 0x003E2EC0
	public override GameObject GetMinionWidget(GameObject parent)
	{
		GameObject widget_go = Util.KInstantiateUI(this.whiteText ? Assets.UIPrefabs.TableScreenWidgets.ButtonLabelWhite : Assets.UIPrefabs.TableScreenWidgets.ButtonLabel, parent, true);
		ToolTip tt = widget_go.GetComponent<ToolTip>();
		tt.OnToolTip = (() => this.GetTooltip(tt));
		if (this.on_click_action != null)
		{
			widget_go.GetComponent<KButton>().onClick += delegate()
			{
				this.on_click_action(widget_go);
			};
		}
		if (this.on_double_click_action != null)
		{
			widget_go.GetComponent<KButton>().onDoubleClick += delegate()
			{
				this.on_double_click_action(widget_go);
			};
		}
		return widget_go;
	}

	// Token: 0x04007E3B RID: 32315
	private Action<GameObject> on_click_action;

	// Token: 0x04007E3C RID: 32316
	private Action<GameObject> on_double_click_action;

	// Token: 0x04007E3D RID: 32317
	private bool whiteText;
}
