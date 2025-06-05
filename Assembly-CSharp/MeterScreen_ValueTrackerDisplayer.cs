using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

// Token: 0x02001E6D RID: 7789
public abstract class MeterScreen_ValueTrackerDisplayer : KMonoBehaviour
{
	// Token: 0x0600A328 RID: 41768 RVA: 0x0010E7A3 File Offset: 0x0010C9A3
	protected override void OnSpawn()
	{
		this.Tooltip.OnToolTip = new Func<string>(this.OnTooltip);
		base.OnSpawn();
	}

	// Token: 0x0600A329 RID: 41769 RVA: 0x0010E7C3 File Offset: 0x0010C9C3
	public void Refresh()
	{
		this.RefreshWorldMinionIdentities();
		this.InternalRefresh();
	}

	// Token: 0x0600A32A RID: 41770
	protected abstract void InternalRefresh();

	// Token: 0x0600A32B RID: 41771
	protected abstract string OnTooltip();

	// Token: 0x0600A32C RID: 41772 RVA: 0x000AA038 File Offset: 0x000A8238
	public virtual void OnClick(BaseEventData base_ev_data)
	{
	}

	// Token: 0x0600A32D RID: 41773 RVA: 0x003EF394 File Offset: 0x003ED594
	private void RefreshWorldMinionIdentities()
	{
		this.worldLiveMinionIdentities = new List<MinionIdentity>(from x in Components.LiveMinionIdentities.GetWorldItems(ClusterManager.Instance.activeWorldId, false)
		where !x.IsNullOrDestroyed()
		select x);
	}

	// Token: 0x0600A32E RID: 41774 RVA: 0x0010E7D1 File Offset: 0x0010C9D1
	protected virtual List<MinionIdentity> GetWorldMinionIdentities()
	{
		if (this.worldLiveMinionIdentities == null)
		{
			this.RefreshWorldMinionIdentities();
		}
		if (this.minionListCustomSortOperation != null)
		{
			this.worldLiveMinionIdentities = this.minionListCustomSortOperation(this.worldLiveMinionIdentities);
		}
		return this.worldLiveMinionIdentities;
	}

	// Token: 0x0600A32F RID: 41775 RVA: 0x003EF3E8 File Offset: 0x003ED5E8
	protected virtual List<MinionIdentity> GetAllMinionsFromAllWorlds()
	{
		List<MinionIdentity> list = new List<MinionIdentity>(from x in Components.LiveMinionIdentities.Items
		where !x.IsNullOrDestroyed()
		select x);
		if (this.minionListCustomSortOperation != null)
		{
			this.worldLiveMinionIdentities = this.minionListCustomSortOperation(list);
		}
		return list;
	}

	// Token: 0x04007F90 RID: 32656
	public LocText Label;

	// Token: 0x04007F91 RID: 32657
	public ToolTip Tooltip;

	// Token: 0x04007F92 RID: 32658
	public GameObject diagnosticGraph;

	// Token: 0x04007F93 RID: 32659
	public TextStyleSetting ToolTipStyle_Header;

	// Token: 0x04007F94 RID: 32660
	public TextStyleSetting ToolTipStyle_Property;

	// Token: 0x04007F95 RID: 32661
	protected Func<List<MinionIdentity>, List<MinionIdentity>> minionListCustomSortOperation;

	// Token: 0x04007F96 RID: 32662
	private List<MinionIdentity> worldLiveMinionIdentities;
}
