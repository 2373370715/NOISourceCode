using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02001F1D RID: 7965
[AddComponentMenu("KMonoBehaviour/scripts/ReportScreenEntry")]
public class ReportScreenEntry : KMonoBehaviour
{
	// Token: 0x0600A78D RID: 42893 RVA: 0x0040536C File Offset: 0x0040356C
	public void SetMainEntry(ReportManager.ReportEntry entry, ReportManager.ReportGroup reportGroup)
	{
		if (this.mainRow == null)
		{
			this.mainRow = Util.KInstantiateUI(this.rowTemplate.gameObject, base.gameObject, true).GetComponent<ReportScreenEntryRow>();
			MultiToggle toggle = this.mainRow.toggle;
			toggle.onClick = (System.Action)Delegate.Combine(toggle.onClick, new System.Action(this.ToggleContext));
			MultiToggle componentInChildren = this.mainRow.name.GetComponentInChildren<MultiToggle>();
			componentInChildren.onClick = (System.Action)Delegate.Combine(componentInChildren.onClick, new System.Action(this.ToggleContext));
			MultiToggle componentInChildren2 = this.mainRow.added.GetComponentInChildren<MultiToggle>();
			componentInChildren2.onClick = (System.Action)Delegate.Combine(componentInChildren2.onClick, new System.Action(this.ToggleContext));
			MultiToggle componentInChildren3 = this.mainRow.removed.GetComponentInChildren<MultiToggle>();
			componentInChildren3.onClick = (System.Action)Delegate.Combine(componentInChildren3.onClick, new System.Action(this.ToggleContext));
			MultiToggle componentInChildren4 = this.mainRow.net.GetComponentInChildren<MultiToggle>();
			componentInChildren4.onClick = (System.Action)Delegate.Combine(componentInChildren4.onClick, new System.Action(this.ToggleContext));
		}
		this.mainRow.SetLine(entry, reportGroup);
		this.currentContextCount = entry.contextEntries.Count;
		for (int i = 0; i < entry.contextEntries.Count; i++)
		{
			if (i >= this.contextRows.Count)
			{
				ReportScreenEntryRow component = Util.KInstantiateUI(this.rowTemplate.gameObject, base.gameObject, false).GetComponent<ReportScreenEntryRow>();
				this.contextRows.Add(component);
			}
			this.contextRows[i].SetLine(entry.contextEntries[i], reportGroup);
		}
		this.UpdateVisibility();
	}

	// Token: 0x0600A78E RID: 42894 RVA: 0x00111320 File Offset: 0x0010F520
	private void ToggleContext()
	{
		this.mainRow.toggle.NextState();
		this.UpdateVisibility();
	}

	// Token: 0x0600A78F RID: 42895 RVA: 0x0040552C File Offset: 0x0040372C
	private void UpdateVisibility()
	{
		int i;
		for (i = 0; i < this.currentContextCount; i++)
		{
			this.contextRows[i].gameObject.SetActive(this.mainRow.toggle.CurrentState == 1);
		}
		while (i < this.contextRows.Count)
		{
			this.contextRows[i].gameObject.SetActive(false);
			i++;
		}
	}

	// Token: 0x0400837D RID: 33661
	[SerializeField]
	private ReportScreenEntryRow rowTemplate;

	// Token: 0x0400837E RID: 33662
	private ReportScreenEntryRow mainRow;

	// Token: 0x0400837F RID: 33663
	private List<ReportScreenEntryRow> contextRows = new List<ReportScreenEntryRow>();

	// Token: 0x04008380 RID: 33664
	private int currentContextCount;
}
