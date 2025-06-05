using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02001C1A RID: 7194
public class DetailsPanelDrawer
{
	// Token: 0x0600959B RID: 38299 RVA: 0x00105F50 File Offset: 0x00104150
	public DetailsPanelDrawer(GameObject label_prefab, GameObject parent)
	{
		this.parent = parent;
		this.labelPrefab = label_prefab;
		this.stringformatter = new UIStringFormatter();
		this.floatFormatter = new UIFloatFormatter();
	}

	// Token: 0x0600959C RID: 38300 RVA: 0x003A6918 File Offset: 0x003A4B18
	public DetailsPanelDrawer NewLabel(string text)
	{
		DetailsPanelDrawer.Label label = default(DetailsPanelDrawer.Label);
		if (this.activeLabelCount >= this.labels.Count)
		{
			label.text = Util.KInstantiate(this.labelPrefab, this.parent, null).GetComponent<LocText>();
			label.tooltip = label.text.GetComponent<ToolTip>();
			label.text.transform.localScale = new Vector3(1f, 1f, 1f);
			this.labels.Add(label);
		}
		else
		{
			label = this.labels[this.activeLabelCount];
		}
		this.activeLabelCount++;
		label.text.text = text;
		label.tooltip.toolTip = "";
		label.tooltip.OnToolTip = null;
		label.text.gameObject.SetActive(true);
		return this;
	}

	// Token: 0x0600959D RID: 38301 RVA: 0x000BC493 File Offset: 0x000BA693
	public DetailsPanelDrawer BeginDrawing()
	{
		return this;
	}

	// Token: 0x0600959E RID: 38302 RVA: 0x000BC493 File Offset: 0x000BA693
	public DetailsPanelDrawer EndDrawing()
	{
		return this;
	}

	// Token: 0x0400746B RID: 29803
	private List<DetailsPanelDrawer.Label> labels = new List<DetailsPanelDrawer.Label>();

	// Token: 0x0400746C RID: 29804
	private int activeLabelCount;

	// Token: 0x0400746D RID: 29805
	private UIStringFormatter stringformatter;

	// Token: 0x0400746E RID: 29806
	private UIFloatFormatter floatFormatter;

	// Token: 0x0400746F RID: 29807
	private GameObject parent;

	// Token: 0x04007470 RID: 29808
	private GameObject labelPrefab;

	// Token: 0x02001C1B RID: 7195
	private struct Label
	{
		// Token: 0x04007471 RID: 29809
		public LocText text;

		// Token: 0x04007472 RID: 29810
		public ToolTip tooltip;
	}
}
