using System;
using System.Collections.Generic;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x02000B9C RID: 2972
[AddComponentMenu("KMonoBehaviour/scripts/Uncoverable")]
public class Uncoverable : KMonoBehaviour
{
	// Token: 0x1700026C RID: 620
	// (get) Token: 0x060037DE RID: 14302 RVA: 0x000C8AF7 File Offset: 0x000C6CF7
	public bool IsUncovered
	{
		get
		{
			return this.hasBeenUncovered;
		}
	}

	// Token: 0x060037DF RID: 14303 RVA: 0x00226BBC File Offset: 0x00224DBC
	private bool IsAnyCellShowing()
	{
		int rootCell = Grid.PosToCell(this);
		return !this.occupyArea.TestArea(rootCell, null, Uncoverable.IsCellBlockedDelegate);
	}

	// Token: 0x060037E0 RID: 14304 RVA: 0x000C8AFF File Offset: 0x000C6CFF
	private static bool IsCellBlocked(int cell, object data)
	{
		return Grid.Element[cell].IsSolid && !Grid.Foundation[cell];
	}

	// Token: 0x060037E1 RID: 14305 RVA: 0x000B74E6 File Offset: 0x000B56E6
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
	}

	// Token: 0x060037E2 RID: 14306 RVA: 0x00226BE8 File Offset: 0x00224DE8
	protected override void OnSpawn()
	{
		base.OnSpawn();
		if (this.IsAnyCellShowing())
		{
			this.hasBeenUncovered = true;
		}
		if (!this.hasBeenUncovered)
		{
			base.GetComponent<KSelectable>().IsSelectable = false;
			Extents extents = this.occupyArea.GetExtents();
			this.partitionerEntry = GameScenePartitioner.Instance.Add("Uncoverable.OnSpawn", base.gameObject, extents, GameScenePartitioner.Instance.solidChangedLayer, new Action<object>(this.OnSolidChanged));
		}
	}

	// Token: 0x060037E3 RID: 14307 RVA: 0x00226C5C File Offset: 0x00224E5C
	private void OnSolidChanged(object data)
	{
		if (this.IsAnyCellShowing() && !this.hasBeenUncovered && this.partitionerEntry.IsValid())
		{
			GameScenePartitioner.Instance.Free(ref this.partitionerEntry);
			this.hasBeenUncovered = true;
			base.GetComponent<KSelectable>().IsSelectable = true;
			Notification notification = new Notification(MISC.STATUSITEMS.BURIEDITEM.NOTIFICATION, NotificationType.Good, new Func<List<Notification>, object, string>(Uncoverable.OnNotificationToolTip), this, true, 0f, null, null, null, true, false, false);
			base.gameObject.AddOrGet<Notifier>().Add(notification, "");
		}
	}

	// Token: 0x060037E4 RID: 14308 RVA: 0x00226CEC File Offset: 0x00224EEC
	private static string OnNotificationToolTip(List<Notification> notifications, object data)
	{
		Uncoverable cmp = (Uncoverable)data;
		return MISC.STATUSITEMS.BURIEDITEM.NOTIFICATION_TOOLTIP.Replace("{Uncoverable}", cmp.GetProperName());
	}

	// Token: 0x060037E5 RID: 14309 RVA: 0x000C8B1F File Offset: 0x000C6D1F
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		GameScenePartitioner.Instance.Free(ref this.partitionerEntry);
	}

	// Token: 0x0400267C RID: 9852
	[MyCmpReq]
	private OccupyArea occupyArea;

	// Token: 0x0400267D RID: 9853
	[Serialize]
	private bool hasBeenUncovered;

	// Token: 0x0400267E RID: 9854
	private HandleVector<int>.Handle partitionerEntry;

	// Token: 0x0400267F RID: 9855
	private static readonly Func<int, object, bool> IsCellBlockedDelegate = (int cell, object data) => Uncoverable.IsCellBlocked(cell, data);
}
