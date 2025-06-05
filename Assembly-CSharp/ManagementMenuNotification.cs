using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000AE1 RID: 2785
public class ManagementMenuNotification : Notification
{
	// Token: 0x17000216 RID: 534
	// (get) Token: 0x0600333F RID: 13119 RVA: 0x000C5E0D File Offset: 0x000C400D
	// (set) Token: 0x06003340 RID: 13120 RVA: 0x000C5E15 File Offset: 0x000C4015
	public bool hasBeenViewed { get; private set; }

	// Token: 0x17000217 RID: 535
	// (get) Token: 0x06003341 RID: 13121 RVA: 0x000C5E1E File Offset: 0x000C401E
	// (set) Token: 0x06003342 RID: 13122 RVA: 0x000C5E26 File Offset: 0x000C4026
	public string highlightTarget { get; set; }

	// Token: 0x06003343 RID: 13123 RVA: 0x0021360C File Offset: 0x0021180C
	public ManagementMenuNotification(global::Action targetMenu, NotificationValence valence, string highlightTarget, string title, NotificationType type, Func<List<Notification>, object, string> tooltip = null, object tooltip_data = null, bool expires = true, float delay = 0f, Notification.ClickCallback custom_click_callback = null, object custom_click_data = null, Transform click_focus = null, bool volume_attenuation = true) : base(title, type, tooltip, tooltip_data, expires, delay, custom_click_callback, custom_click_data, click_focus, volume_attenuation, false, false)
	{
		this.targetMenu = targetMenu;
		this.valence = valence;
		this.highlightTarget = highlightTarget;
	}

	// Token: 0x06003344 RID: 13124 RVA: 0x000C5E2F File Offset: 0x000C402F
	public void View()
	{
		this.hasBeenViewed = true;
		ManagementMenu.Instance.notificationDisplayer.NotificationWasViewed(this);
	}

	// Token: 0x0400231B RID: 8987
	public global::Action targetMenu;

	// Token: 0x0400231C RID: 8988
	public NotificationValence valence;
}
