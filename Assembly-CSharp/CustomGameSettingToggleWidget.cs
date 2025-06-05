using System;
using Klei.CustomSettings;
using UnityEngine;

// Token: 0x02001CE0 RID: 7392
public class CustomGameSettingToggleWidget : CustomGameSettingWidget
{
	// Token: 0x06009A26 RID: 39462 RVA: 0x00108BA3 File Offset: 0x00106DA3
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		MultiToggle toggle = this.Toggle;
		toggle.onClick = (System.Action)Delegate.Combine(toggle.onClick, new System.Action(this.ToggleSetting));
	}

	// Token: 0x06009A27 RID: 39463 RVA: 0x00108BD2 File Offset: 0x00106DD2
	public void Initialize(ToggleSettingConfig config, Func<SettingConfig, SettingLevel> getCurrentSettingCallback, Func<ToggleSettingConfig, SettingLevel> toggleCallback)
	{
		this.config = config;
		this.Label.text = config.label;
		this.ToolTip.toolTip = config.tooltip;
		this.getCurrentSettingCallback = getCurrentSettingCallback;
		this.toggleCallback = toggleCallback;
	}

	// Token: 0x06009A28 RID: 39464 RVA: 0x003C6554 File Offset: 0x003C4754
	public override void Refresh()
	{
		base.Refresh();
		SettingLevel settingLevel = this.getCurrentSettingCallback(this.config);
		this.Toggle.ChangeState(this.config.IsOnLevel(settingLevel.id) ? 1 : 0);
		this.ToggleToolTip.toolTip = settingLevel.tooltip;
	}

	// Token: 0x06009A29 RID: 39465 RVA: 0x00108C0B File Offset: 0x00106E0B
	public void ToggleSetting()
	{
		this.toggleCallback(this.config);
		base.Notify();
	}

	// Token: 0x0400784D RID: 30797
	[SerializeField]
	private LocText Label;

	// Token: 0x0400784E RID: 30798
	[SerializeField]
	private ToolTip ToolTip;

	// Token: 0x0400784F RID: 30799
	[SerializeField]
	private MultiToggle Toggle;

	// Token: 0x04007850 RID: 30800
	[SerializeField]
	private ToolTip ToggleToolTip;

	// Token: 0x04007851 RID: 30801
	private ToggleSettingConfig config;

	// Token: 0x04007852 RID: 30802
	protected Func<SettingConfig, SettingLevel> getCurrentSettingCallback;

	// Token: 0x04007853 RID: 30803
	protected Func<ToggleSettingConfig, SettingLevel> toggleCallback;
}
