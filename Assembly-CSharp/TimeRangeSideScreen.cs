using System;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200204C RID: 8268
public class TimeRangeSideScreen : SideScreenContent, IRender200ms
{
	// Token: 0x0600AF7F RID: 44927 RVA: 0x00116AF3 File Offset: 0x00114CF3
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.labelHeaderStart.text = UI.UISIDESCREENS.TIME_RANGE_SIDE_SCREEN.ON;
		this.labelHeaderDuration.text = UI.UISIDESCREENS.TIME_RANGE_SIDE_SCREEN.DURATION;
	}

	// Token: 0x0600AF80 RID: 44928 RVA: 0x00116B25 File Offset: 0x00114D25
	public override bool IsValidForTarget(GameObject target)
	{
		return target.GetComponent<LogicTimeOfDaySensor>() != null;
	}

	// Token: 0x0600AF81 RID: 44929 RVA: 0x0042A628 File Offset: 0x00428828
	public override void SetTarget(GameObject target)
	{
		this.imageActiveZone.color = GlobalAssets.Instance.colorSet.logicOnSidescreen;
		this.imageInactiveZone.color = GlobalAssets.Instance.colorSet.logicOffSidescreen;
		base.SetTarget(target);
		this.targetTimedSwitch = target.GetComponent<LogicTimeOfDaySensor>();
		this.duration.onValueChanged.RemoveAllListeners();
		this.startTime.onValueChanged.RemoveAllListeners();
		this.startTime.value = this.targetTimedSwitch.startTime;
		this.duration.value = this.targetTimedSwitch.duration;
		this.ChangeSetting();
		this.startTime.onValueChanged.AddListener(delegate(float value)
		{
			this.ChangeSetting();
		});
		this.duration.onValueChanged.AddListener(delegate(float value)
		{
			this.ChangeSetting();
		});
	}

	// Token: 0x0600AF82 RID: 44930 RVA: 0x0042A710 File Offset: 0x00428910
	private void ChangeSetting()
	{
		this.targetTimedSwitch.startTime = this.startTime.value;
		this.targetTimedSwitch.duration = this.duration.value;
		this.imageActiveZone.rectTransform.rotation = Quaternion.identity;
		this.imageActiveZone.rectTransform.Rotate(0f, 0f, this.NormalizedValueToDegrees(this.startTime.value));
		this.imageActiveZone.fillAmount = this.duration.value;
		this.labelValueStart.text = GameUtil.GetFormattedPercent(this.targetTimedSwitch.startTime * 100f, GameUtil.TimeSlice.None);
		this.labelValueDuration.text = GameUtil.GetFormattedPercent(this.targetTimedSwitch.duration * 100f, GameUtil.TimeSlice.None);
		this.endIndicator.rotation = Quaternion.identity;
		this.endIndicator.Rotate(0f, 0f, this.NormalizedValueToDegrees(this.startTime.value + this.duration.value));
		this.startTime.SetTooltipText(string.Format(UI.UISIDESCREENS.TIME_RANGE_SIDE_SCREEN.ON_TOOLTIP, GameUtil.GetFormattedPercent(this.targetTimedSwitch.startTime * 100f, GameUtil.TimeSlice.None)));
		this.duration.SetTooltipText(string.Format(UI.UISIDESCREENS.TIME_RANGE_SIDE_SCREEN.DURATION_TOOLTIP, GameUtil.GetFormattedPercent(this.targetTimedSwitch.duration * 100f, GameUtil.TimeSlice.None)));
	}

	// Token: 0x0600AF83 RID: 44931 RVA: 0x00116B33 File Offset: 0x00114D33
	public void Render200ms(float dt)
	{
		this.currentTimeMarker.rotation = Quaternion.identity;
		this.currentTimeMarker.Rotate(0f, 0f, this.NormalizedValueToDegrees(GameClock.Instance.GetCurrentCycleAsPercentage()));
	}

	// Token: 0x0600AF84 RID: 44932 RVA: 0x00116B6A File Offset: 0x00114D6A
	private float NormalizedValueToDegrees(float value)
	{
		return 360f * value;
	}

	// Token: 0x0600AF85 RID: 44933 RVA: 0x00116B73 File Offset: 0x00114D73
	private float SecondsToDegrees(float seconds)
	{
		return 360f * (seconds / 600f);
	}

	// Token: 0x0600AF86 RID: 44934 RVA: 0x00116B82 File Offset: 0x00114D82
	private float DegreesToNormalizedValue(float degrees)
	{
		return degrees / 360f;
	}

	// Token: 0x040089E4 RID: 35300
	public Image imageInactiveZone;

	// Token: 0x040089E5 RID: 35301
	public Image imageActiveZone;

	// Token: 0x040089E6 RID: 35302
	private LogicTimeOfDaySensor targetTimedSwitch;

	// Token: 0x040089E7 RID: 35303
	public KSlider startTime;

	// Token: 0x040089E8 RID: 35304
	public KSlider duration;

	// Token: 0x040089E9 RID: 35305
	public RectTransform endIndicator;

	// Token: 0x040089EA RID: 35306
	public LocText labelHeaderStart;

	// Token: 0x040089EB RID: 35307
	public LocText labelHeaderDuration;

	// Token: 0x040089EC RID: 35308
	public LocText labelValueStart;

	// Token: 0x040089ED RID: 35309
	public LocText labelValueDuration;

	// Token: 0x040089EE RID: 35310
	public RectTransform currentTimeMarker;
}
