using System;
using System.Collections.Generic;

// Token: 0x02001EB6 RID: 7862
public abstract class NotificationDisplayer : KMonoBehaviour
{
	// Token: 0x0600A4ED RID: 42221 RVA: 0x0010F6B1 File Offset: 0x0010D8B1
	protected override void OnSpawn()
	{
		this.displayedNotifications = new List<Notification>();
		NotificationManager.Instance.notificationAdded += this.NotificationAdded;
		NotificationManager.Instance.notificationRemoved += this.NotificationRemoved;
	}

	// Token: 0x0600A4EE RID: 42222 RVA: 0x0010F6EA File Offset: 0x0010D8EA
	public void NotificationAdded(Notification notification)
	{
		if (this.ShouldDisplayNotification(notification))
		{
			this.displayedNotifications.Add(notification);
			this.OnNotificationAdded(notification);
		}
	}

	// Token: 0x0600A4EF RID: 42223
	protected abstract void OnNotificationAdded(Notification notification);

	// Token: 0x0600A4F0 RID: 42224 RVA: 0x0010F708 File Offset: 0x0010D908
	public void NotificationRemoved(Notification notification)
	{
		if (this.displayedNotifications.Contains(notification))
		{
			this.displayedNotifications.Remove(notification);
			this.OnNotificationRemoved(notification);
		}
	}

	// Token: 0x0600A4F1 RID: 42225
	protected abstract void OnNotificationRemoved(Notification notification);

	// Token: 0x0600A4F2 RID: 42226
	protected abstract bool ShouldDisplayNotification(Notification notification);

	// Token: 0x04008103 RID: 33027
	protected List<Notification> displayedNotifications;
}
