using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x02001CE9 RID: 7401
public class DateTime : KScreen
{
	// Token: 0x06009A51 RID: 39505 RVA: 0x00108D79 File Offset: 0x00106F79
	public static void DestroyInstance()
	{
		global::DateTime.Instance = null;
	}

	// Token: 0x06009A52 RID: 39506 RVA: 0x00108D81 File Offset: 0x00106F81
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		global::DateTime.Instance = this;
		this.milestoneEffect.gameObject.SetActive(false);
	}

	// Token: 0x06009A53 RID: 39507 RVA: 0x003C69C8 File Offset: 0x003C4BC8
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.tooltip.OnComplexToolTip = new ToolTip.ComplexTooltipDelegate(this.BuildTooltip);
		Game.Instance.Subscribe(2070437606, new Action<object>(this.OnMilestoneDayReached));
		Game.Instance.Subscribe(-720092972, new Action<object>(this.OnMilestoneDayApproaching));
	}

	// Token: 0x06009A54 RID: 39508 RVA: 0x003C6A2C File Offset: 0x003C4C2C
	private List<global::Tuple<string, TextStyleSetting>> BuildTooltip()
	{
		List<global::Tuple<string, TextStyleSetting>> colonyToolTip = SaveGame.Instance.GetColonyToolTip();
		if (TimeOfDay.IsMilestoneApproaching)
		{
			colonyToolTip.Add(new global::Tuple<string, TextStyleSetting>(" ", null));
			colonyToolTip.Add(new global::Tuple<string, TextStyleSetting>(UI.ASTEROIDCLOCK.MILESTONE_TITLE.text, ToolTipScreen.Instance.defaultTooltipHeaderStyle));
			colonyToolTip.Add(new global::Tuple<string, TextStyleSetting>(UI.ASTEROIDCLOCK.MILESTONE_DESCRIPTION.text.Replace("{0}", (GameClock.Instance.GetCycle() + 2).ToString()), ToolTipScreen.Instance.defaultTooltipBodyStyle));
		}
		return colonyToolTip;
	}

	// Token: 0x06009A55 RID: 39509 RVA: 0x00108DA0 File Offset: 0x00106FA0
	private void Update()
	{
		if (GameClock.Instance != null && this.displayedDayCount != GameUtil.GetCurrentCycle())
		{
			this.text.text = this.Days();
			this.displayedDayCount = GameUtil.GetCurrentCycle();
		}
	}

	// Token: 0x06009A56 RID: 39510 RVA: 0x00108DD8 File Offset: 0x00106FD8
	private void OnMilestoneDayApproaching(object data)
	{
		int num = (int)data;
		this.milestoneEffect.gameObject.SetActive(true);
		this.milestoneEffect.Play("100fx_pre", KAnim.PlayMode.Loop, 1f, 0f);
	}

	// Token: 0x06009A57 RID: 39511 RVA: 0x00108E12 File Offset: 0x00107012
	private void OnMilestoneDayReached(object data)
	{
		int num = (int)data;
		this.milestoneEffect.gameObject.SetActive(true);
		this.milestoneEffect.Play("100fx", KAnim.PlayMode.Once, 1f, 0f);
	}

	// Token: 0x06009A58 RID: 39512 RVA: 0x003C6ABC File Offset: 0x003C4CBC
	private string Days()
	{
		return GameUtil.GetCurrentCycle().ToString();
	}

	// Token: 0x0400786F RID: 30831
	public static global::DateTime Instance;

	// Token: 0x04007870 RID: 30832
	private const string MILESTONE_ANTICIPATION_ANIMATION_NAME = "100fx_pre";

	// Token: 0x04007871 RID: 30833
	private const string MILESTONE_ANIMATION_NAME = "100fx";

	// Token: 0x04007872 RID: 30834
	public LocText day;

	// Token: 0x04007873 RID: 30835
	private int displayedDayCount = -1;

	// Token: 0x04007874 RID: 30836
	[SerializeField]
	private KBatchedAnimController milestoneEffect;

	// Token: 0x04007875 RID: 30837
	[SerializeField]
	private LocText text;

	// Token: 0x04007876 RID: 30838
	[SerializeField]
	private ToolTip tooltip;

	// Token: 0x04007877 RID: 30839
	[SerializeField]
	private TextStyleSetting tooltipstyle_Days;

	// Token: 0x04007878 RID: 30840
	[SerializeField]
	private TextStyleSetting tooltipstyle_Playtime;

	// Token: 0x04007879 RID: 30841
	[SerializeField]
	public KToggle scheduleToggle;
}
