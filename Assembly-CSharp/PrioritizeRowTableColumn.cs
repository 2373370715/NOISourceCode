using System;
using UnityEngine;

// Token: 0x02001E17 RID: 7703
public class PrioritizeRowTableColumn : TableColumn
{
	// Token: 0x0600A0DC RID: 41180 RVA: 0x0010D4B1 File Offset: 0x0010B6B1
	public PrioritizeRowTableColumn(object user_data, Action<object, int> on_change_priority, Func<object, int, string> on_hover_widget) : base(null, null, null, null, null, false, "")
	{
		this.userData = user_data;
		this.onChangePriority = on_change_priority;
		this.onHoverWidget = on_hover_widget;
	}

	// Token: 0x0600A0DD RID: 41181 RVA: 0x0010D4D9 File Offset: 0x0010B6D9
	public override GameObject GetMinionWidget(GameObject parent)
	{
		return this.GetWidget(parent);
	}

	// Token: 0x0600A0DE RID: 41182 RVA: 0x0010D4D9 File Offset: 0x0010B6D9
	public override GameObject GetDefaultWidget(GameObject parent)
	{
		return this.GetWidget(parent);
	}

	// Token: 0x0600A0DF RID: 41183 RVA: 0x0010D4E2 File Offset: 0x0010B6E2
	public override GameObject GetHeaderWidget(GameObject parent)
	{
		return Util.KInstantiateUI(Assets.UIPrefabs.TableScreenWidgets.PrioritizeRowHeaderWidget, parent, true);
	}

	// Token: 0x0600A0E0 RID: 41184 RVA: 0x003E5258 File Offset: 0x003E3458
	private GameObject GetWidget(GameObject parent)
	{
		GameObject gameObject = Util.KInstantiateUI(Assets.UIPrefabs.TableScreenWidgets.PrioritizeRowWidget, parent, true);
		HierarchyReferences component = gameObject.GetComponent<HierarchyReferences>();
		this.ConfigureButton(component, "UpButton", 1, gameObject);
		this.ConfigureButton(component, "DownButton", -1, gameObject);
		return gameObject;
	}

	// Token: 0x0600A0E1 RID: 41185 RVA: 0x003E52A0 File Offset: 0x003E34A0
	private void ConfigureButton(HierarchyReferences refs, string ref_id, int delta, GameObject widget_go)
	{
		KButton kbutton = refs.GetReference(ref_id) as KButton;
		kbutton.onClick += delegate()
		{
			this.onChangePriority(widget_go, delta);
		};
		ToolTip component = kbutton.GetComponent<ToolTip>();
		if (component != null)
		{
			component.OnToolTip = (() => this.onHoverWidget(widget_go, delta));
		}
	}

	// Token: 0x04007E63 RID: 32355
	public object userData;

	// Token: 0x04007E64 RID: 32356
	private Action<object, int> onChangePriority;

	// Token: 0x04007E65 RID: 32357
	private Func<object, int, string> onHoverWidget;
}
