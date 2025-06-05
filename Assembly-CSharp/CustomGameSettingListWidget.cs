using System;
using Klei.CustomSettings;
using UnityEngine;

// Token: 0x02001CDE RID: 7390
public class CustomGameSettingListWidget : CustomGameSettingWidget
{
	// Token: 0x06009A17 RID: 39447 RVA: 0x00108A7B File Offset: 0x00106C7B
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.CycleLeft.onClick += this.DoCycleLeft;
		this.CycleRight.onClick += this.DoCycleRight;
	}

	// Token: 0x06009A18 RID: 39448 RVA: 0x00108AB1 File Offset: 0x00106CB1
	public void Initialize(ListSettingConfig config, Func<SettingConfig, SettingLevel> getCallback, Func<ListSettingConfig, int, SettingLevel> cycleCallback)
	{
		this.config = config;
		this.Label.text = config.label;
		this.ToolTip.toolTip = config.tooltip;
		this.getCallback = getCallback;
		this.cycleCallback = cycleCallback;
	}

	// Token: 0x06009A19 RID: 39449 RVA: 0x003C62C0 File Offset: 0x003C44C0
	public override void Refresh()
	{
		base.Refresh();
		SettingLevel settingLevel = this.getCallback(this.config);
		this.ValueLabel.text = settingLevel.label;
		this.ValueToolTip.toolTip = settingLevel.tooltip;
		this.CycleLeft.isInteractable = !this.config.IsFirstLevel(settingLevel.id);
		this.CycleRight.isInteractable = !this.config.IsLastLevel(settingLevel.id);
	}

	// Token: 0x06009A1A RID: 39450 RVA: 0x00108AEA File Offset: 0x00106CEA
	private void DoCycleLeft()
	{
		this.cycleCallback(this.config, -1);
		base.Notify();
	}

	// Token: 0x06009A1B RID: 39451 RVA: 0x00108B05 File Offset: 0x00106D05
	private void DoCycleRight()
	{
		this.cycleCallback(this.config, 1);
		base.Notify();
	}

	// Token: 0x0400783B RID: 30779
	[SerializeField]
	private LocText Label;

	// Token: 0x0400783C RID: 30780
	[SerializeField]
	private ToolTip ToolTip;

	// Token: 0x0400783D RID: 30781
	[SerializeField]
	private LocText ValueLabel;

	// Token: 0x0400783E RID: 30782
	[SerializeField]
	private ToolTip ValueToolTip;

	// Token: 0x0400783F RID: 30783
	[SerializeField]
	private KButton CycleLeft;

	// Token: 0x04007840 RID: 30784
	[SerializeField]
	private KButton CycleRight;

	// Token: 0x04007841 RID: 30785
	private ListSettingConfig config;

	// Token: 0x04007842 RID: 30786
	protected Func<ListSettingConfig, int, SettingLevel> cycleCallback;

	// Token: 0x04007843 RID: 30787
	protected Func<SettingConfig, SettingLevel> getCallback;
}
