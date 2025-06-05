using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02001F87 RID: 8071
public class AlarmSideScreen : SideScreenContent
{
	// Token: 0x0600AA7B RID: 43643 RVA: 0x0041564C File Offset: 0x0041384C
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.nameInputField.onEndEdit += this.OnEndEditName;
		this.nameInputField.field.characterLimit = 30;
		this.tooltipInputField.onEndEdit += this.OnEndEditTooltip;
		this.tooltipInputField.field.characterLimit = 90;
		this.pauseToggle.onClick += this.TogglePause;
		this.zoomToggle.onClick += this.ToggleZoom;
		this.InitializeToggles();
	}

	// Token: 0x0600AA7C RID: 43644 RVA: 0x00113202 File Offset: 0x00111402
	private void OnEndEditName()
	{
		this.targetAlarm.notificationName = this.nameInputField.field.text;
		this.UpdateNotification(true);
	}

	// Token: 0x0600AA7D RID: 43645 RVA: 0x00113226 File Offset: 0x00111426
	private void OnEndEditTooltip()
	{
		this.targetAlarm.notificationTooltip = this.tooltipInputField.field.text;
		this.UpdateNotification(true);
	}

	// Token: 0x0600AA7E RID: 43646 RVA: 0x0011324A File Offset: 0x0011144A
	private void TogglePause()
	{
		this.targetAlarm.pauseOnNotify = !this.targetAlarm.pauseOnNotify;
		this.pauseCheckmark.enabled = this.targetAlarm.pauseOnNotify;
		this.UpdateNotification(true);
	}

	// Token: 0x0600AA7F RID: 43647 RVA: 0x00113282 File Offset: 0x00111482
	private void ToggleZoom()
	{
		this.targetAlarm.zoomOnNotify = !this.targetAlarm.zoomOnNotify;
		this.zoomCheckmark.enabled = this.targetAlarm.zoomOnNotify;
		this.UpdateNotification(true);
	}

	// Token: 0x0600AA80 RID: 43648 RVA: 0x001132BA File Offset: 0x001114BA
	private void SelectType(NotificationType type)
	{
		this.targetAlarm.notificationType = type;
		this.UpdateNotification(true);
		this.RefreshToggles();
	}

	// Token: 0x0600AA81 RID: 43649 RVA: 0x004156E8 File Offset: 0x004138E8
	private void InitializeToggles()
	{
		if (this.toggles_by_type.Count == 0)
		{
			using (List<NotificationType>.Enumerator enumerator = this.validTypes.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					NotificationType type = enumerator.Current;
					GameObject gameObject = Util.KInstantiateUI(this.typeButtonPrefab, this.typeButtonPrefab.transform.parent.gameObject, true);
					gameObject.name = "TypeButton: " + type.ToString();
					HierarchyReferences component = gameObject.GetComponent<HierarchyReferences>();
					Color notificationBGColour = NotificationScreen.Instance.GetNotificationBGColour(type);
					Color notificationColour = NotificationScreen.Instance.GetNotificationColour(type);
					notificationBGColour.a = 1f;
					notificationColour.a = 1f;
					component.GetReference<KImage>("bg").color = notificationBGColour;
					component.GetReference<KImage>("icon").color = notificationColour;
					component.GetReference<KImage>("icon").sprite = NotificationScreen.Instance.GetNotificationIcon(type);
					ToolTip component2 = gameObject.GetComponent<ToolTip>();
					NotificationType type2 = type;
					if (type2 != NotificationType.Bad)
					{
						if (type2 != NotificationType.Neutral)
						{
							if (type2 == NotificationType.DuplicantThreatening)
							{
								component2.SetSimpleTooltip(UI.UISIDESCREENS.LOGICALARMSIDESCREEN.TOOLTIPS.DUPLICANT_THREATENING);
							}
						}
						else
						{
							component2.SetSimpleTooltip(UI.UISIDESCREENS.LOGICALARMSIDESCREEN.TOOLTIPS.NEUTRAL);
						}
					}
					else
					{
						component2.SetSimpleTooltip(UI.UISIDESCREENS.LOGICALARMSIDESCREEN.TOOLTIPS.BAD);
					}
					if (!this.toggles_by_type.ContainsKey(type))
					{
						this.toggles_by_type.Add(type, gameObject.GetComponent<MultiToggle>());
					}
					this.toggles_by_type[type].onClick = delegate()
					{
						this.SelectType(type);
					};
					for (int i = 0; i < this.toggles_by_type[type].states.Length; i++)
					{
						this.toggles_by_type[type].states[i].on_click_override_sound_path = NotificationScreen.Instance.GetNotificationSound(type);
					}
				}
			}
		}
	}

	// Token: 0x0600AA82 RID: 43650 RVA: 0x0041592C File Offset: 0x00413B2C
	private void RefreshToggles()
	{
		this.InitializeToggles();
		foreach (KeyValuePair<NotificationType, MultiToggle> keyValuePair in this.toggles_by_type)
		{
			if (this.targetAlarm.notificationType == keyValuePair.Key)
			{
				keyValuePair.Value.ChangeState(0);
			}
			else
			{
				keyValuePair.Value.ChangeState(1);
			}
		}
	}

	// Token: 0x0600AA83 RID: 43651 RVA: 0x001132D5 File Offset: 0x001114D5
	public override bool IsValidForTarget(GameObject target)
	{
		return target.GetComponent<LogicAlarm>() != null;
	}

	// Token: 0x0600AA84 RID: 43652 RVA: 0x001132E3 File Offset: 0x001114E3
	public override void SetTarget(GameObject target)
	{
		base.SetTarget(target);
		this.targetAlarm = target.GetComponent<LogicAlarm>();
		this.RefreshToggles();
		this.UpdateVisuals();
	}

	// Token: 0x0600AA85 RID: 43653 RVA: 0x00113304 File Offset: 0x00111504
	private void UpdateNotification(bool clear)
	{
		this.targetAlarm.UpdateNotification(clear);
	}

	// Token: 0x0600AA86 RID: 43654 RVA: 0x004159B0 File Offset: 0x00413BB0
	private void UpdateVisuals()
	{
		this.nameInputField.SetDisplayValue(this.targetAlarm.notificationName);
		this.tooltipInputField.SetDisplayValue(this.targetAlarm.notificationTooltip);
		this.pauseCheckmark.enabled = this.targetAlarm.pauseOnNotify;
		this.zoomCheckmark.enabled = this.targetAlarm.zoomOnNotify;
	}

	// Token: 0x0400862C RID: 34348
	public LogicAlarm targetAlarm;

	// Token: 0x0400862D RID: 34349
	[SerializeField]
	private KInputField nameInputField;

	// Token: 0x0400862E RID: 34350
	[SerializeField]
	private KInputField tooltipInputField;

	// Token: 0x0400862F RID: 34351
	[SerializeField]
	private KToggle pauseToggle;

	// Token: 0x04008630 RID: 34352
	[SerializeField]
	private Image pauseCheckmark;

	// Token: 0x04008631 RID: 34353
	[SerializeField]
	private KToggle zoomToggle;

	// Token: 0x04008632 RID: 34354
	[SerializeField]
	private Image zoomCheckmark;

	// Token: 0x04008633 RID: 34355
	[SerializeField]
	private GameObject typeButtonPrefab;

	// Token: 0x04008634 RID: 34356
	private List<NotificationType> validTypes = new List<NotificationType>
	{
		NotificationType.Bad,
		NotificationType.Neutral,
		NotificationType.DuplicantThreatening
	};

	// Token: 0x04008635 RID: 34357
	private Dictionary<NotificationType, MultiToggle> toggles_by_type = new Dictionary<NotificationType, MultiToggle>();
}
