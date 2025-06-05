using System;
using System.Collections.Generic;

// Token: 0x02001EB3 RID: 7859
public class ManagementMenuNotificationDisplayer : NotificationDisplayer
{
	// Token: 0x17000A96 RID: 2710
	// (get) Token: 0x0600A4D9 RID: 42201 RVA: 0x0010F5EC File Offset: 0x0010D7EC
	// (set) Token: 0x0600A4DA RID: 42202 RVA: 0x0010F5F4 File Offset: 0x0010D7F4
	public List<ManagementMenuNotification> displayedManagementMenuNotifications { get; private set; }

	// Token: 0x14000031 RID: 49
	// (add) Token: 0x0600A4DB RID: 42203 RVA: 0x003F7BE8 File Offset: 0x003F5DE8
	// (remove) Token: 0x0600A4DC RID: 42204 RVA: 0x003F7C20 File Offset: 0x003F5E20
	public event System.Action onNotificationsChanged;

	// Token: 0x0600A4DD RID: 42205 RVA: 0x0010F5FD File Offset: 0x0010D7FD
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.displayedManagementMenuNotifications = new List<ManagementMenuNotification>();
	}

	// Token: 0x0600A4DE RID: 42206 RVA: 0x0010F610 File Offset: 0x0010D810
	public void NotificationWasViewed(ManagementMenuNotification notification)
	{
		this.onNotificationsChanged();
	}

	// Token: 0x0600A4DF RID: 42207 RVA: 0x0010F61D File Offset: 0x0010D81D
	protected override void OnNotificationAdded(Notification notification)
	{
		this.displayedManagementMenuNotifications.Add(notification as ManagementMenuNotification);
		this.onNotificationsChanged();
	}

	// Token: 0x0600A4E0 RID: 42208 RVA: 0x0010F63B File Offset: 0x0010D83B
	protected override void OnNotificationRemoved(Notification notification)
	{
		this.displayedManagementMenuNotifications.Remove(notification as ManagementMenuNotification);
		this.onNotificationsChanged();
	}

	// Token: 0x0600A4E1 RID: 42209 RVA: 0x0010F65A File Offset: 0x0010D85A
	protected override bool ShouldDisplayNotification(Notification notification)
	{
		return notification is ManagementMenuNotification;
	}

	// Token: 0x0600A4E2 RID: 42210 RVA: 0x003F7C58 File Offset: 0x003F5E58
	public List<ManagementMenuNotification> GetNotificationsForAction(global::Action hotKey)
	{
		List<ManagementMenuNotification> list = new List<ManagementMenuNotification>();
		foreach (ManagementMenuNotification managementMenuNotification in this.displayedManagementMenuNotifications)
		{
			if (managementMenuNotification.targetMenu == hotKey)
			{
				list.Add(managementMenuNotification);
			}
		}
		return list;
	}
}
