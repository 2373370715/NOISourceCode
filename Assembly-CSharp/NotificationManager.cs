using System;
using System.Collections.Generic;

// Token: 0x02001EB9 RID: 7865
public class NotificationManager : KMonoBehaviour
{
	// Token: 0x17000A97 RID: 2711
	// (get) Token: 0x0600A503 RID: 42243 RVA: 0x0010F837 File Offset: 0x0010DA37
	// (set) Token: 0x0600A504 RID: 42244 RVA: 0x0010F83E File Offset: 0x0010DA3E
	public static NotificationManager Instance { get; private set; }

	// Token: 0x14000032 RID: 50
	// (add) Token: 0x0600A505 RID: 42245 RVA: 0x003F80A0 File Offset: 0x003F62A0
	// (remove) Token: 0x0600A506 RID: 42246 RVA: 0x003F80D8 File Offset: 0x003F62D8
	public event Action<Notification> notificationAdded;

	// Token: 0x14000033 RID: 51
	// (add) Token: 0x0600A507 RID: 42247 RVA: 0x003F8110 File Offset: 0x003F6310
	// (remove) Token: 0x0600A508 RID: 42248 RVA: 0x003F8148 File Offset: 0x003F6348
	public event Action<Notification> notificationRemoved;

	// Token: 0x0600A509 RID: 42249 RVA: 0x0010F846 File Offset: 0x0010DA46
	protected override void OnPrefabInit()
	{
		Debug.Assert(NotificationManager.Instance == null);
		NotificationManager.Instance = this;
	}

	// Token: 0x0600A50A RID: 42250 RVA: 0x0010F85E File Offset: 0x0010DA5E
	protected override void OnForcedCleanUp()
	{
		NotificationManager.Instance = null;
	}

	// Token: 0x0600A50B RID: 42251 RVA: 0x0010F866 File Offset: 0x0010DA66
	public void AddNotification(Notification notification)
	{
		this.pendingNotifications.Add(notification);
		if (NotificationScreen.Instance != null)
		{
			NotificationScreen.Instance.AddPendingNotification(notification);
		}
	}

	// Token: 0x0600A50C RID: 42252 RVA: 0x003F8180 File Offset: 0x003F6380
	public void RemoveNotification(Notification notification)
	{
		this.pendingNotifications.Remove(notification);
		if (NotificationScreen.Instance != null)
		{
			NotificationScreen.Instance.RemovePendingNotification(notification);
		}
		if (this.notifications.Remove(notification))
		{
			this.notificationRemoved(notification);
		}
	}

	// Token: 0x0600A50D RID: 42253 RVA: 0x003F81CC File Offset: 0x003F63CC
	private void Update()
	{
		int i = 0;
		while (i < this.pendingNotifications.Count)
		{
			if (this.pendingNotifications[i].IsReady())
			{
				this.DoAddNotification(this.pendingNotifications[i]);
				this.pendingNotifications.RemoveAt(i);
			}
			else
			{
				i++;
			}
		}
	}

	// Token: 0x0600A50E RID: 42254 RVA: 0x0010F88C File Offset: 0x0010DA8C
	private void DoAddNotification(Notification notification)
	{
		this.notifications.Add(notification);
		if (this.notificationAdded != null)
		{
			this.notificationAdded(notification);
		}
	}

	// Token: 0x0400810D RID: 33037
	private List<Notification> pendingNotifications = new List<Notification>();

	// Token: 0x0400810E RID: 33038
	private List<Notification> notifications = new List<Notification>();
}
