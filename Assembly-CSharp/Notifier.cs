using System;
using UnityEngine;

// Token: 0x02000AE5 RID: 2789
[SkipSaveFileSerialization]
[AddComponentMenu("KMonoBehaviour/scripts/Notifier")]
public class Notifier : KMonoBehaviour
{
	// Token: 0x06003363 RID: 13155 RVA: 0x000C5F45 File Offset: 0x000C4145
	protected override void OnPrefabInit()
	{
		Components.Notifiers.Add(this);
	}

	// Token: 0x06003364 RID: 13156 RVA: 0x000C5F52 File Offset: 0x000C4152
	protected override void OnCleanUp()
	{
		Components.Notifiers.Remove(this);
	}

	// Token: 0x06003365 RID: 13157 RVA: 0x002138EC File Offset: 0x00211AEC
	public void Add(Notification notification, string suffix = "")
	{
		if (KScreenManager.Instance == null)
		{
			return;
		}
		if (this.DisableNotifications)
		{
			return;
		}
		if (DebugHandler.NotificationsDisabled)
		{
			return;
		}
		DebugUtil.DevAssert(notification != null, "Trying to add null notification. It's safe to continue playing, the notification won't be displayed.", null);
		if (notification == null)
		{
			return;
		}
		if (notification.Notifier == null)
		{
			if (this.Selectable != null)
			{
				notification.NotifierName = "• " + this.Selectable.GetName() + suffix;
			}
			else
			{
				notification.NotifierName = "• " + base.name + suffix;
			}
			notification.Notifier = this;
			if (this.AutoClickFocus && notification.clickFocus == null)
			{
				notification.clickFocus = base.transform;
			}
			NotificationManager.Instance.AddNotification(notification);
			notification.GameTime = Time.time;
		}
		else
		{
			DebugUtil.Assert(notification.Notifier == this);
		}
		notification.Time = KTime.Instance.UnscaledGameTime;
	}

	// Token: 0x06003366 RID: 13158 RVA: 0x000C5F5F File Offset: 0x000C415F
	public void Remove(Notification notification)
	{
		if (notification == null)
		{
			return;
		}
		if (notification.Notifier != null)
		{
			notification.Notifier = null;
		}
		if (NotificationManager.Instance != null)
		{
			NotificationManager.Instance.RemoveNotification(notification);
		}
	}

	// Token: 0x04002332 RID: 9010
	[MyCmpGet]
	private KSelectable Selectable;

	// Token: 0x04002333 RID: 9011
	public bool DisableNotifications;

	// Token: 0x04002334 RID: 9012
	public bool AutoClickFocus = true;
}
