using System;
using System.Collections.Generic;
using Klei.AI;
using UnityEngine;
using UnityEngine.EventSystems;

// Token: 0x02001E6C RID: 7788
public abstract class MeterScreen_VTD_DuplicantIterator : MeterScreen_ValueTrackerDisplayer
{
	// Token: 0x0600A323 RID: 41763 RVA: 0x003EF288 File Offset: 0x003ED488
	protected virtual void UpdateDisplayInfo(BaseEventData base_ev_data, IList<MinionIdentity> minions)
	{
		PointerEventData pointerEventData = base_ev_data as PointerEventData;
		if (pointerEventData == null)
		{
			return;
		}
		List<MinionIdentity> worldMinionIdentities = this.GetWorldMinionIdentities();
		PointerEventData.InputButton button = pointerEventData.button;
		if (button != PointerEventData.InputButton.Left)
		{
			if (button != PointerEventData.InputButton.Right)
			{
				return;
			}
			this.lastSelectedDuplicantIndex = -1;
		}
		else
		{
			if (worldMinionIdentities.Count < this.lastSelectedDuplicantIndex)
			{
				this.lastSelectedDuplicantIndex = -1;
			}
			if (worldMinionIdentities.Count > 0)
			{
				this.lastSelectedDuplicantIndex = (this.lastSelectedDuplicantIndex + 1) % worldMinionIdentities.Count;
				MinionIdentity minionIdentity = minions[this.lastSelectedDuplicantIndex];
				SelectTool.Instance.SelectAndFocus(minionIdentity.transform.GetPosition(), minionIdentity.GetComponent<KSelectable>(), Vector3.zero);
				return;
			}
		}
	}

	// Token: 0x0600A324 RID: 41764 RVA: 0x003EF320 File Offset: 0x003ED520
	public override void OnClick(BaseEventData base_ev_data)
	{
		List<MinionIdentity> worldMinionIdentities = this.GetWorldMinionIdentities();
		this.UpdateDisplayInfo(base_ev_data, worldMinionIdentities);
		this.OnTooltip();
		this.Tooltip.forceRefresh = true;
	}

	// Token: 0x0600A325 RID: 41765 RVA: 0x0010E75B File Offset: 0x0010C95B
	protected void AddToolTipLine(string str, bool selected)
	{
		if (selected)
		{
			this.Tooltip.AddMultiStringTooltip("<color=#F0B310FF>" + str + "</color>", this.ToolTipStyle_Property);
			return;
		}
		this.Tooltip.AddMultiStringTooltip(str, this.ToolTipStyle_Property);
	}

	// Token: 0x0600A326 RID: 41766 RVA: 0x003EF350 File Offset: 0x003ED550
	protected void AddToolTipAmountPercentLine(AmountInstance amount, MinionIdentity id, bool selected)
	{
		string str = id.GetComponent<KSelectable>().GetName() + ":  " + Mathf.Round(amount.value).ToString() + "%";
		this.AddToolTipLine(str, selected);
	}

	// Token: 0x04007F8F RID: 32655
	protected int lastSelectedDuplicantIndex = -1;
}
