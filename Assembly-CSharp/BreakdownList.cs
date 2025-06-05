using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020020BC RID: 8380
[AddComponentMenu("KMonoBehaviour/scripts/BreakdownList")]
public class BreakdownList : KMonoBehaviour
{
	// Token: 0x0600B2B9 RID: 45753 RVA: 0x0043E520 File Offset: 0x0043C720
	public BreakdownListRow AddRow()
	{
		BreakdownListRow breakdownListRow;
		if (this.unusedListRows.Count > 0)
		{
			breakdownListRow = this.unusedListRows[0];
			this.unusedListRows.RemoveAt(0);
		}
		else
		{
			breakdownListRow = UnityEngine.Object.Instantiate<BreakdownListRow>(this.listRowTemplate);
		}
		breakdownListRow.gameObject.transform.SetParent(base.transform);
		breakdownListRow.gameObject.transform.SetAsLastSibling();
		this.listRows.Add(breakdownListRow);
		breakdownListRow.gameObject.SetActive(true);
		return breakdownListRow;
	}

	// Token: 0x0600B2BA RID: 45754 RVA: 0x00118B15 File Offset: 0x00116D15
	public GameObject AddCustomRow(GameObject newRow)
	{
		newRow.transform.SetParent(base.transform);
		newRow.gameObject.transform.SetAsLastSibling();
		this.customRows.Add(newRow);
		newRow.SetActive(true);
		return newRow;
	}

	// Token: 0x0600B2BB RID: 45755 RVA: 0x0043E5A4 File Offset: 0x0043C7A4
	public void ClearRows()
	{
		foreach (BreakdownListRow breakdownListRow in this.listRows)
		{
			this.unusedListRows.Add(breakdownListRow);
			breakdownListRow.gameObject.SetActive(false);
			breakdownListRow.ClearTooltip();
		}
		this.listRows.Clear();
		foreach (GameObject gameObject in this.customRows)
		{
			gameObject.SetActive(false);
		}
	}

	// Token: 0x0600B2BC RID: 45756 RVA: 0x00118B4C File Offset: 0x00116D4C
	public void SetTitle(string title)
	{
		this.headerTitle.text = title;
	}

	// Token: 0x0600B2BD RID: 45757 RVA: 0x00118B5A File Offset: 0x00116D5A
	public void SetDescription(string description)
	{
		if (description != null && description.Length >= 0)
		{
			this.infoTextLabel.gameObject.SetActive(true);
			this.infoTextLabel.text = description;
			return;
		}
		this.infoTextLabel.gameObject.SetActive(false);
	}

	// Token: 0x0600B2BE RID: 45758 RVA: 0x00118B97 File Offset: 0x00116D97
	public void SetIcon(Sprite icon)
	{
		this.headerIcon.sprite = icon;
	}

	// Token: 0x04008D1C RID: 36124
	public Image headerIcon;

	// Token: 0x04008D1D RID: 36125
	public Sprite headerIconSprite;

	// Token: 0x04008D1E RID: 36126
	public Image headerBar;

	// Token: 0x04008D1F RID: 36127
	public LocText headerTitle;

	// Token: 0x04008D20 RID: 36128
	public LocText headerValue;

	// Token: 0x04008D21 RID: 36129
	public LocText infoTextLabel;

	// Token: 0x04008D22 RID: 36130
	public BreakdownListRow listRowTemplate;

	// Token: 0x04008D23 RID: 36131
	private List<BreakdownListRow> listRows = new List<BreakdownListRow>();

	// Token: 0x04008D24 RID: 36132
	private List<BreakdownListRow> unusedListRows = new List<BreakdownListRow>();

	// Token: 0x04008D25 RID: 36133
	private List<GameObject> customRows = new List<GameObject>();
}
