using System;
using System.Collections.Generic;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x02000E6C RID: 3692
[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/scripts/LogicAlarm")]
public class LogicAlarm : KMonoBehaviour, ISaveLoadable
{
	// Token: 0x06004849 RID: 18505 RVA: 0x000D34E1 File Offset: 0x000D16E1
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.Subscribe<LogicAlarm>(-905833192, LogicAlarm.OnCopySettingsDelegate);
	}

	// Token: 0x0600484A RID: 18506 RVA: 0x00263C0C File Offset: 0x00261E0C
	private void OnCopySettings(object data)
	{
		LogicAlarm component = ((GameObject)data).GetComponent<LogicAlarm>();
		if (component != null)
		{
			this.notificationName = component.notificationName;
			this.notificationType = component.notificationType;
			this.pauseOnNotify = component.pauseOnNotify;
			this.zoomOnNotify = component.zoomOnNotify;
			this.cooldown = component.cooldown;
			this.notificationTooltip = component.notificationTooltip;
		}
	}

	// Token: 0x0600484B RID: 18507 RVA: 0x00263C78 File Offset: 0x00261E78
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.notifier = base.gameObject.AddComponent<Notifier>();
		base.Subscribe<LogicAlarm>(-801688580, LogicAlarm.OnLogicValueChangedDelegate);
		if (string.IsNullOrEmpty(this.notificationName))
		{
			this.notificationName = UI.UISIDESCREENS.LOGICALARMSIDESCREEN.NAME_DEFAULT;
		}
		if (string.IsNullOrEmpty(this.notificationTooltip))
		{
			this.notificationTooltip = UI.UISIDESCREENS.LOGICALARMSIDESCREEN.TOOLTIP_DEFAULT;
		}
		this.UpdateVisualState();
		this.UpdateNotification(false);
	}

	// Token: 0x0600484C RID: 18508 RVA: 0x000D34FA File Offset: 0x000D16FA
	private void UpdateVisualState()
	{
		base.GetComponent<KBatchedAnimController>().Play(this.wasOn ? LogicAlarm.ON_ANIMS : LogicAlarm.OFF_ANIMS, KAnim.PlayMode.Once);
	}

	// Token: 0x0600484D RID: 18509 RVA: 0x00263CF4 File Offset: 0x00261EF4
	public void OnLogicValueChanged(object data)
	{
		LogicValueChanged logicValueChanged = (LogicValueChanged)data;
		if (logicValueChanged.portID != LogicAlarm.INPUT_PORT_ID)
		{
			return;
		}
		int newValue = logicValueChanged.newValue;
		if (LogicCircuitNetwork.IsBitActive(0, newValue))
		{
			if (!this.wasOn)
			{
				this.PushNotification();
				this.wasOn = true;
				if (this.pauseOnNotify && !SpeedControlScreen.Instance.IsPaused)
				{
					SpeedControlScreen.Instance.Pause(false, false);
				}
				if (this.zoomOnNotify)
				{
					GameUtil.FocusCameraOnWorld(base.gameObject.GetMyWorldId(), base.transform.GetPosition(), 8f, null, true);
				}
				this.UpdateVisualState();
				return;
			}
		}
		else if (this.wasOn)
		{
			this.wasOn = false;
			this.UpdateVisualState();
		}
	}

	// Token: 0x0600484E RID: 18510 RVA: 0x000D351C File Offset: 0x000D171C
	private void PushNotification()
	{
		this.notification.Clear();
		this.notifier.Add(this.notification, "");
	}

	// Token: 0x0600484F RID: 18511 RVA: 0x00263DA8 File Offset: 0x00261FA8
	public void UpdateNotification(bool clear)
	{
		if (this.notification != null && clear)
		{
			this.notification.Clear();
			this.lastNotificationCreated = null;
		}
		if (this.notification != this.lastNotificationCreated || this.lastNotificationCreated == null)
		{
			this.notification = this.CreateNotification();
		}
	}

	// Token: 0x06004850 RID: 18512 RVA: 0x00263DF8 File Offset: 0x00261FF8
	public Notification CreateNotification()
	{
		base.GetComponent<KSelectable>();
		Notification result = new Notification(this.notificationName, this.notificationType, (List<Notification> n, object d) => this.notificationTooltip, null, true, 0f, null, null, null, false, false, false);
		this.lastNotificationCreated = result;
		return result;
	}

	// Token: 0x040032BF RID: 12991
	[Serialize]
	public string notificationName;

	// Token: 0x040032C0 RID: 12992
	[Serialize]
	public string notificationTooltip;

	// Token: 0x040032C1 RID: 12993
	[Serialize]
	public NotificationType notificationType;

	// Token: 0x040032C2 RID: 12994
	[Serialize]
	public bool pauseOnNotify;

	// Token: 0x040032C3 RID: 12995
	[Serialize]
	public bool zoomOnNotify;

	// Token: 0x040032C4 RID: 12996
	[Serialize]
	public float cooldown;

	// Token: 0x040032C5 RID: 12997
	[MyCmpAdd]
	private CopyBuildingSettings copyBuildingSettings;

	// Token: 0x040032C6 RID: 12998
	private bool wasOn;

	// Token: 0x040032C7 RID: 12999
	private Notifier notifier;

	// Token: 0x040032C8 RID: 13000
	private Notification notification;

	// Token: 0x040032C9 RID: 13001
	private Notification lastNotificationCreated;

	// Token: 0x040032CA RID: 13002
	private static readonly EventSystem.IntraObjectHandler<LogicAlarm> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<LogicAlarm>(delegate(LogicAlarm component, object data)
	{
		component.OnCopySettings(data);
	});

	// Token: 0x040032CB RID: 13003
	private static readonly EventSystem.IntraObjectHandler<LogicAlarm> OnLogicValueChangedDelegate = new EventSystem.IntraObjectHandler<LogicAlarm>(delegate(LogicAlarm component, object data)
	{
		component.OnLogicValueChanged(data);
	});

	// Token: 0x040032CC RID: 13004
	public static readonly HashedString INPUT_PORT_ID = new HashedString("LogicAlarmInput");

	// Token: 0x040032CD RID: 13005
	protected static readonly HashedString[] ON_ANIMS = new HashedString[]
	{
		"on_pre",
		"on_loop"
	};

	// Token: 0x040032CE RID: 13006
	protected static readonly HashedString[] OFF_ANIMS = new HashedString[]
	{
		"on_pst",
		"off"
	};
}
