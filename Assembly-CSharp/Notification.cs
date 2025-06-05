using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

// Token: 0x02000AE2 RID: 2786
public class Notification
{
	// Token: 0x17000218 RID: 536
	// (get) Token: 0x06003345 RID: 13125 RVA: 0x000C5E48 File Offset: 0x000C4048
	// (set) Token: 0x06003346 RID: 13126 RVA: 0x000C5E50 File Offset: 0x000C4050
	public NotificationType Type { get; set; }

	// Token: 0x17000219 RID: 537
	// (get) Token: 0x06003347 RID: 13127 RVA: 0x000C5E59 File Offset: 0x000C4059
	// (set) Token: 0x06003348 RID: 13128 RVA: 0x000C5E61 File Offset: 0x000C4061
	public Notifier Notifier { get; set; }

	// Token: 0x1700021A RID: 538
	// (get) Token: 0x06003349 RID: 13129 RVA: 0x000C5E6A File Offset: 0x000C406A
	// (set) Token: 0x0600334A RID: 13130 RVA: 0x000C5E72 File Offset: 0x000C4072
	public Transform clickFocus { get; set; }

	// Token: 0x1700021B RID: 539
	// (get) Token: 0x0600334B RID: 13131 RVA: 0x000C5E7B File Offset: 0x000C407B
	// (set) Token: 0x0600334C RID: 13132 RVA: 0x000C5E83 File Offset: 0x000C4083
	public float Time { get; set; }

	// Token: 0x1700021C RID: 540
	// (get) Token: 0x0600334D RID: 13133 RVA: 0x000C5E8C File Offset: 0x000C408C
	// (set) Token: 0x0600334E RID: 13134 RVA: 0x000C5E94 File Offset: 0x000C4094
	public float GameTime { get; set; }

	// Token: 0x1700021D RID: 541
	// (get) Token: 0x0600334F RID: 13135 RVA: 0x000C5E9D File Offset: 0x000C409D
	// (set) Token: 0x06003350 RID: 13136 RVA: 0x000C5EA5 File Offset: 0x000C40A5
	public float Delay { get; set; }

	// Token: 0x1700021E RID: 542
	// (get) Token: 0x06003351 RID: 13137 RVA: 0x000C5EAE File Offset: 0x000C40AE
	// (set) Token: 0x06003352 RID: 13138 RVA: 0x000C5EB6 File Offset: 0x000C40B6
	public int Idx { get; set; }

	// Token: 0x1700021F RID: 543
	// (get) Token: 0x06003353 RID: 13139 RVA: 0x000C5EBF File Offset: 0x000C40BF
	// (set) Token: 0x06003354 RID: 13140 RVA: 0x000C5EC7 File Offset: 0x000C40C7
	public Func<List<Notification>, object, string> ToolTip { get; set; }

	// Token: 0x06003355 RID: 13141 RVA: 0x000C5ED0 File Offset: 0x000C40D0
	public bool IsReady()
	{
		return UnityEngine.Time.time >= this.GameTime + this.Delay;
	}

	// Token: 0x17000220 RID: 544
	// (get) Token: 0x06003356 RID: 13142 RVA: 0x000C5EE9 File Offset: 0x000C40E9
	// (set) Token: 0x06003357 RID: 13143 RVA: 0x000C5EF1 File Offset: 0x000C40F1
	public string titleText { get; private set; }

	// Token: 0x17000221 RID: 545
	// (get) Token: 0x06003358 RID: 13144 RVA: 0x000C5EFA File Offset: 0x000C40FA
	// (set) Token: 0x06003359 RID: 13145 RVA: 0x000C5F02 File Offset: 0x000C4102
	public string NotifierName
	{
		get
		{
			return this.notifierName;
		}
		set
		{
			this.notifierName = value;
			this.titleText = this.ReplaceTags(this.titleText);
		}
	}

	// Token: 0x0600335A RID: 13146 RVA: 0x0021364C File Offset: 0x0021184C
	public Notification(string title, NotificationType type, Func<List<Notification>, object, string> tooltip = null, object tooltip_data = null, bool expires = true, float delay = 0f, Notification.ClickCallback custom_click_callback = null, object custom_click_data = null, Transform click_focus = null, bool volume_attenuation = true, bool clear_on_click = false, bool show_dismiss_button = false)
	{
		this.titleText = title;
		this.Type = type;
		this.ToolTip = tooltip;
		this.tooltipData = tooltip_data;
		this.expires = expires;
		this.Delay = delay;
		this.customClickCallback = custom_click_callback;
		this.customClickData = custom_click_data;
		this.clickFocus = click_focus;
		this.volume_attenuation = volume_attenuation;
		this.clearOnClick = clear_on_click;
		this.showDismissButton = show_dismiss_button;
		int num = this.notificationIncrement;
		this.notificationIncrement = num + 1;
		this.Idx = num;
	}

	// Token: 0x0600335B RID: 13147 RVA: 0x000C5F1D File Offset: 0x000C411D
	public void Clear()
	{
		if (this.Notifier != null)
		{
			this.Notifier.Remove(this);
			return;
		}
		NotificationManager.Instance.RemoveNotification(this);
	}

	// Token: 0x0600335C RID: 13148 RVA: 0x002136E8 File Offset: 0x002118E8
	private string ReplaceTags(string text)
	{
		DebugUtil.Assert(text != null);
		int num = text.IndexOf('{');
		int num2 = text.IndexOf('}');
		if (0 <= num && num < num2)
		{
			StringBuilder stringBuilder = new StringBuilder();
			int num3 = 0;
			while (0 <= num)
			{
				string value = text.Substring(num3, num - num3);
				stringBuilder.Append(value);
				num2 = text.IndexOf('}', num);
				if (num >= num2)
				{
					break;
				}
				string tag = text.Substring(num + 1, num2 - num - 1);
				string tagDescription = this.GetTagDescription(tag);
				stringBuilder.Append(tagDescription);
				num3 = num2 + 1;
				num = text.IndexOf('{', num2);
			}
			stringBuilder.Append(text.Substring(num3, text.Length - num3));
			return stringBuilder.ToString();
		}
		return text;
	}

	// Token: 0x0600335D RID: 13149 RVA: 0x0021379C File Offset: 0x0021199C
	private string GetTagDescription(string tag)
	{
		string result;
		if (tag == "NotifierName")
		{
			result = this.notifierName;
		}
		else
		{
			result = "UNKNOWN TAG: " + tag;
		}
		return result;
	}

	// Token: 0x04002327 RID: 8999
	public object tooltipData;

	// Token: 0x04002328 RID: 9000
	public bool expires = true;

	// Token: 0x04002329 RID: 9001
	public bool playSound = true;

	// Token: 0x0400232A RID: 9002
	public bool volume_attenuation = true;

	// Token: 0x0400232B RID: 9003
	public Notification.ClickCallback customClickCallback;

	// Token: 0x0400232C RID: 9004
	public bool clearOnClick;

	// Token: 0x0400232D RID: 9005
	public bool showDismissButton;

	// Token: 0x0400232E RID: 9006
	public object customClickData;

	// Token: 0x0400232F RID: 9007
	private int notificationIncrement;

	// Token: 0x04002331 RID: 9009
	private string notifierName;

	// Token: 0x02000AE3 RID: 2787
	// (Invoke) Token: 0x0600335F RID: 13151
	public delegate void ClickCallback(object data);
}
