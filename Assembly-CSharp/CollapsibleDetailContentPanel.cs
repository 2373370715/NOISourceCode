using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02001CF8 RID: 7416
[AddComponentMenu("KMonoBehaviour/scripts/CollapsibleDetailContentPanel")]
public class CollapsibleDetailContentPanel : KMonoBehaviour
{
	// Token: 0x06009AD2 RID: 39634 RVA: 0x003C9AD4 File Offset: 0x003C7CD4
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		MultiToggle multiToggle = this.collapseButton;
		multiToggle.onClick = (System.Action)Delegate.Combine(multiToggle.onClick, new System.Action(this.ToggleOpen));
		this.ArrowIcon.SetActive();
		this.log = new LoggerFSS("detailpanel", 35);
		this.labels = new Dictionary<string, CollapsibleDetailContentPanel.Label<DetailLabel>>();
		this.buttonLabels = new Dictionary<string, CollapsibleDetailContentPanel.Label<DetailLabelWithButton>>();
		this.Commit();
	}

	// Token: 0x06009AD3 RID: 39635 RVA: 0x0010935E File Offset: 0x0010755E
	public void SetTitle(string title)
	{
		this.HeaderLabel.text = title;
	}

	// Token: 0x06009AD4 RID: 39636 RVA: 0x003C9B48 File Offset: 0x003C7D48
	public void Commit()
	{
		int num = 0;
		foreach (CollapsibleDetailContentPanel.Label<DetailLabel> label in this.labels.Values)
		{
			if (label.used)
			{
				num++;
				if (!label.obj.gameObject.activeSelf)
				{
					label.obj.gameObject.SetActive(true);
				}
			}
			else if (!label.used && label.obj.gameObject.activeSelf)
			{
				label.obj.gameObject.SetActive(false);
			}
			label.used = false;
		}
		foreach (CollapsibleDetailContentPanel.Label<DetailLabelWithButton> label2 in this.buttonLabels.Values)
		{
			if (label2.used)
			{
				num++;
				if (!label2.obj.gameObject.activeSelf)
				{
					label2.obj.gameObject.SetActive(true);
				}
			}
			else if (!label2.used && label2.obj.gameObject.activeSelf)
			{
				label2.obj.gameObject.SetActive(false);
			}
			label2.used = false;
		}
		if (base.gameObject.activeSelf && num == 0)
		{
			base.gameObject.SetActive(false);
			return;
		}
		if (!base.gameObject.activeSelf && num > 0)
		{
			base.gameObject.SetActive(true);
		}
	}

	// Token: 0x06009AD5 RID: 39637 RVA: 0x003C9CE4 File Offset: 0x003C7EE4
	public void SetLabel(string id, string text, string tooltip)
	{
		CollapsibleDetailContentPanel.Label<DetailLabel> label;
		if (!this.labels.TryGetValue(id, out label))
		{
			label = new CollapsibleDetailContentPanel.Label<DetailLabel>
			{
				used = true,
				obj = Util.KInstantiateUI(this.labelTemplate.gameObject, this.Content.gameObject, false).GetComponent<DetailLabel>()
			};
			label.obj.gameObject.name = id;
			this.labels[id] = label;
		}
		label.obj.label.AllowLinks = true;
		label.obj.label.text = text;
		label.obj.toolTip.toolTip = tooltip;
		label.used = true;
	}

	// Token: 0x06009AD6 RID: 39638 RVA: 0x003C9D90 File Offset: 0x003C7F90
	public void SetLabelWithButton(string id, string text, string tooltip, System.Action buttonCb)
	{
		CollapsibleDetailContentPanel.Label<DetailLabelWithButton> label;
		if (!this.buttonLabels.TryGetValue(id, out label))
		{
			label = new CollapsibleDetailContentPanel.Label<DetailLabelWithButton>
			{
				used = true,
				obj = Util.KInstantiateUI(this.labelWithActionButtonTemplate.gameObject, this.Content.gameObject, false).GetComponent<DetailLabelWithButton>()
			};
			label.obj.gameObject.name = id;
			this.buttonLabels[id] = label;
		}
		label.obj.label.AllowLinks = false;
		label.obj.label.raycastTarget = false;
		label.obj.label.text = text;
		label.obj.toolTip.toolTip = tooltip;
		label.obj.button.ClearOnClick();
		label.obj.button.onClick += buttonCb;
		label.used = true;
	}

	// Token: 0x06009AD7 RID: 39639 RVA: 0x003C9E6C File Offset: 0x003C806C
	private void ToggleOpen()
	{
		bool flag = this.scalerMask.gameObject.activeSelf;
		flag = !flag;
		this.scalerMask.gameObject.SetActive(flag);
		if (flag)
		{
			this.ArrowIcon.SetActive();
			this.ForceLocTextsMeshRebuild();
			return;
		}
		this.ArrowIcon.SetInactive();
	}

	// Token: 0x06009AD8 RID: 39640 RVA: 0x003C9EC0 File Offset: 0x003C80C0
	public void ForceLocTextsMeshRebuild()
	{
		LocText[] componentsInChildren = base.GetComponentsInChildren<LocText>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].ForceMeshUpdate();
		}
	}

	// Token: 0x06009AD9 RID: 39641 RVA: 0x0010936C File Offset: 0x0010756C
	public void SetActive(bool active)
	{
		if (base.gameObject.activeSelf != active)
		{
			base.gameObject.SetActive(active);
		}
	}

	// Token: 0x040078E8 RID: 30952
	public ImageToggleState ArrowIcon;

	// Token: 0x040078E9 RID: 30953
	public LocText HeaderLabel;

	// Token: 0x040078EA RID: 30954
	public MultiToggle collapseButton;

	// Token: 0x040078EB RID: 30955
	public Transform Content;

	// Token: 0x040078EC RID: 30956
	public ScalerMask scalerMask;

	// Token: 0x040078ED RID: 30957
	[Space(10f)]
	public DetailLabel labelTemplate;

	// Token: 0x040078EE RID: 30958
	public DetailLabelWithButton labelWithActionButtonTemplate;

	// Token: 0x040078EF RID: 30959
	private Dictionary<string, CollapsibleDetailContentPanel.Label<DetailLabel>> labels;

	// Token: 0x040078F0 RID: 30960
	private Dictionary<string, CollapsibleDetailContentPanel.Label<DetailLabelWithButton>> buttonLabels;

	// Token: 0x040078F1 RID: 30961
	private LoggerFSS log;

	// Token: 0x02001CF9 RID: 7417
	private class Label<T>
	{
		// Token: 0x040078F2 RID: 30962
		public T obj;

		// Token: 0x040078F3 RID: 30963
		public bool used;
	}
}
