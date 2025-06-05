using System;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200204D RID: 8269
public class TimerSideScreen : SideScreenContent, IRenderEveryTick
{
	// Token: 0x0600AF8A RID: 44938 RVA: 0x00116B93 File Offset: 0x00114D93
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.labelHeaderOnDuration.text = UI.UISIDESCREENS.TIMER_SIDE_SCREEN.ON;
		this.labelHeaderOffDuration.text = UI.UISIDESCREENS.TIMER_SIDE_SCREEN.OFF;
	}

	// Token: 0x0600AF8B RID: 44939 RVA: 0x0042A888 File Offset: 0x00428A88
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.modeButton.onClick += delegate()
		{
			this.ToggleMode();
		};
		this.resetButton.onClick += this.ResetTimer;
		this.onDurationNumberInput.onEndEdit += delegate()
		{
			this.UpdateDurationValueFromTextInput(this.onDurationNumberInput.currentValue, this.onDurationSlider);
		};
		this.offDurationNumberInput.onEndEdit += delegate()
		{
			this.UpdateDurationValueFromTextInput(this.offDurationNumberInput.currentValue, this.offDurationSlider);
		};
		this.onDurationSlider.wholeNumbers = false;
		this.offDurationSlider.wholeNumbers = false;
	}

	// Token: 0x0600AF8C RID: 44940 RVA: 0x00116BC5 File Offset: 0x00114DC5
	public override bool IsValidForTarget(GameObject target)
	{
		return target.GetComponent<LogicTimerSensor>() != null;
	}

	// Token: 0x0600AF8D RID: 44941 RVA: 0x0042A910 File Offset: 0x00428B10
	public override void SetTarget(GameObject target)
	{
		this.greenActiveZone.color = GlobalAssets.Instance.colorSet.logicOnSidescreen;
		this.redActiveZone.color = GlobalAssets.Instance.colorSet.logicOffSidescreen;
		base.SetTarget(target);
		this.targetTimedSwitch = target.GetComponent<LogicTimerSensor>();
		this.onDurationSlider.onValueChanged.RemoveAllListeners();
		this.offDurationSlider.onValueChanged.RemoveAllListeners();
		this.cyclesMode = this.targetTimedSwitch.displayCyclesMode;
		this.UpdateVisualsForNewTarget();
		this.ReconfigureRingVisuals();
		this.onDurationSlider.onValueChanged.AddListener(delegate(float value)
		{
			this.ChangeSetting();
		});
		this.offDurationSlider.onValueChanged.AddListener(delegate(float value)
		{
			this.ChangeSetting();
		});
	}

	// Token: 0x0600AF8E RID: 44942 RVA: 0x0042A9E4 File Offset: 0x00428BE4
	private void UpdateVisualsForNewTarget()
	{
		float onDuration = this.targetTimedSwitch.onDuration;
		float offDuration = this.targetTimedSwitch.offDuration;
		bool displayCyclesMode = this.targetTimedSwitch.displayCyclesMode;
		if (displayCyclesMode)
		{
			this.onDurationSlider.minValue = this.minCycles;
			this.onDurationNumberInput.minValue = this.onDurationSlider.minValue;
			this.onDurationSlider.maxValue = this.maxCycles;
			this.onDurationNumberInput.maxValue = this.onDurationSlider.maxValue;
			this.onDurationNumberInput.decimalPlaces = 2;
			this.offDurationSlider.minValue = this.minCycles;
			this.offDurationNumberInput.minValue = this.offDurationSlider.minValue;
			this.offDurationSlider.maxValue = this.maxCycles;
			this.offDurationNumberInput.maxValue = this.offDurationSlider.maxValue;
			this.offDurationNumberInput.decimalPlaces = 2;
			this.onDurationSlider.value = onDuration / 600f;
			this.offDurationSlider.value = offDuration / 600f;
			this.onDurationNumberInput.SetAmount(onDuration / 600f);
			this.offDurationNumberInput.SetAmount(offDuration / 600f);
		}
		else
		{
			this.onDurationSlider.minValue = this.minSeconds;
			this.onDurationNumberInput.minValue = this.onDurationSlider.minValue;
			this.onDurationSlider.maxValue = this.maxSeconds;
			this.onDurationNumberInput.maxValue = this.onDurationSlider.maxValue;
			this.onDurationNumberInput.decimalPlaces = 1;
			this.offDurationSlider.minValue = this.minSeconds;
			this.offDurationNumberInput.minValue = this.offDurationSlider.minValue;
			this.offDurationSlider.maxValue = this.maxSeconds;
			this.offDurationNumberInput.maxValue = this.offDurationSlider.maxValue;
			this.offDurationNumberInput.decimalPlaces = 1;
			this.onDurationSlider.value = onDuration;
			this.offDurationSlider.value = offDuration;
			this.onDurationNumberInput.SetAmount(onDuration);
			this.offDurationNumberInput.SetAmount(offDuration);
		}
		this.modeButton.GetComponentInChildren<LocText>().text = (displayCyclesMode ? UI.UISIDESCREENS.TIMER_SIDE_SCREEN.MODE_LABEL_CYCLES : UI.UISIDESCREENS.TIMER_SIDE_SCREEN.MODE_LABEL_SECONDS);
	}

	// Token: 0x0600AF8F RID: 44943 RVA: 0x0042AC24 File Offset: 0x00428E24
	private void ToggleMode()
	{
		this.cyclesMode = !this.cyclesMode;
		this.targetTimedSwitch.displayCyclesMode = this.cyclesMode;
		float num = this.onDurationSlider.value;
		float num2 = this.offDurationSlider.value;
		if (this.cyclesMode)
		{
			num = this.onDurationSlider.value / 600f;
			num2 = this.offDurationSlider.value / 600f;
		}
		else
		{
			num = this.onDurationSlider.value * 600f;
			num2 = this.offDurationSlider.value * 600f;
		}
		this.onDurationSlider.minValue = (this.cyclesMode ? this.minCycles : this.minSeconds);
		this.onDurationNumberInput.minValue = this.onDurationSlider.minValue;
		this.onDurationSlider.maxValue = (this.cyclesMode ? this.maxCycles : this.maxSeconds);
		this.onDurationNumberInput.maxValue = this.onDurationSlider.maxValue;
		this.onDurationNumberInput.decimalPlaces = (this.cyclesMode ? 2 : 1);
		this.offDurationSlider.minValue = (this.cyclesMode ? this.minCycles : this.minSeconds);
		this.offDurationNumberInput.minValue = this.offDurationSlider.minValue;
		this.offDurationSlider.maxValue = (this.cyclesMode ? this.maxCycles : this.maxSeconds);
		this.offDurationNumberInput.maxValue = this.offDurationSlider.maxValue;
		this.offDurationNumberInput.decimalPlaces = (this.cyclesMode ? 2 : 1);
		this.onDurationSlider.value = num;
		this.offDurationSlider.value = num2;
		this.onDurationNumberInput.SetAmount(num);
		this.offDurationNumberInput.SetAmount(num2);
		this.modeButton.GetComponentInChildren<LocText>().text = (this.cyclesMode ? UI.UISIDESCREENS.TIMER_SIDE_SCREEN.MODE_LABEL_CYCLES : UI.UISIDESCREENS.TIMER_SIDE_SCREEN.MODE_LABEL_SECONDS);
	}

	// Token: 0x0600AF90 RID: 44944 RVA: 0x0042AE20 File Offset: 0x00429020
	private void ChangeSetting()
	{
		this.targetTimedSwitch.onDuration = (this.cyclesMode ? (this.onDurationSlider.value * 600f) : this.onDurationSlider.value);
		this.targetTimedSwitch.offDuration = (this.cyclesMode ? (this.offDurationSlider.value * 600f) : this.offDurationSlider.value);
		this.ReconfigureRingVisuals();
		this.onDurationNumberInput.SetDisplayValue(this.cyclesMode ? (this.targetTimedSwitch.onDuration / 600f).ToString("F2") : this.targetTimedSwitch.onDuration.ToString());
		this.offDurationNumberInput.SetDisplayValue(this.cyclesMode ? (this.targetTimedSwitch.offDuration / 600f).ToString("F2") : this.targetTimedSwitch.offDuration.ToString());
		this.onDurationSlider.SetTooltipText(string.Format(UI.UISIDESCREENS.TIMER_SIDE_SCREEN.GREEN_DURATION_TOOLTIP, this.cyclesMode ? GameUtil.GetFormattedCycles(this.targetTimedSwitch.onDuration, "F2", false) : GameUtil.GetFormattedTime(this.targetTimedSwitch.onDuration, "F0")));
		this.offDurationSlider.SetTooltipText(string.Format(UI.UISIDESCREENS.TIMER_SIDE_SCREEN.RED_DURATION_TOOLTIP, this.cyclesMode ? GameUtil.GetFormattedCycles(this.targetTimedSwitch.offDuration, "F2", false) : GameUtil.GetFormattedTime(this.targetTimedSwitch.offDuration, "F0")));
		if (this.phaseLength == 0f)
		{
			this.timeLeft.text = UI.UISIDESCREENS.TIMER_SIDE_SCREEN.DISABLED;
			if (this.targetTimedSwitch.IsSwitchedOn)
			{
				this.greenActiveZone.fillAmount = 1f;
				this.redActiveZone.fillAmount = 0f;
			}
			else
			{
				this.greenActiveZone.fillAmount = 0f;
				this.redActiveZone.fillAmount = 1f;
			}
			this.targetTimedSwitch.timeElapsedInCurrentState = 0f;
			this.currentTimeMarker.rotation = Quaternion.identity;
			this.currentTimeMarker.Rotate(0f, 0f, 0f);
		}
	}

	// Token: 0x0600AF91 RID: 44945 RVA: 0x0042B068 File Offset: 0x00429268
	private void ReconfigureRingVisuals()
	{
		this.phaseLength = this.targetTimedSwitch.onDuration + this.targetTimedSwitch.offDuration;
		this.greenActiveZone.fillAmount = this.targetTimedSwitch.onDuration / this.phaseLength;
		this.redActiveZone.fillAmount = this.targetTimedSwitch.offDuration / this.phaseLength;
	}

	// Token: 0x0600AF92 RID: 44946 RVA: 0x0042B0CC File Offset: 0x004292CC
	public void RenderEveryTick(float dt)
	{
		if (this.phaseLength == 0f)
		{
			return;
		}
		float timeElapsedInCurrentState = this.targetTimedSwitch.timeElapsedInCurrentState;
		if (this.cyclesMode)
		{
			this.timeLeft.text = string.Format(UI.UISIDESCREENS.TIMER_SIDE_SCREEN.CURRENT_TIME, GameUtil.GetFormattedCycles(timeElapsedInCurrentState, "F2", false), GameUtil.GetFormattedCycles(this.targetTimedSwitch.IsSwitchedOn ? this.targetTimedSwitch.onDuration : this.targetTimedSwitch.offDuration, "F2", false));
		}
		else
		{
			this.timeLeft.text = string.Format(UI.UISIDESCREENS.TIMER_SIDE_SCREEN.CURRENT_TIME, GameUtil.GetFormattedTime(timeElapsedInCurrentState, "F1"), GameUtil.GetFormattedTime(this.targetTimedSwitch.IsSwitchedOn ? this.targetTimedSwitch.onDuration : this.targetTimedSwitch.offDuration, "F1"));
		}
		this.currentTimeMarker.rotation = Quaternion.identity;
		if (this.targetTimedSwitch.IsSwitchedOn)
		{
			this.currentTimeMarker.Rotate(0f, 0f, this.targetTimedSwitch.timeElapsedInCurrentState / this.phaseLength * -360f);
			return;
		}
		this.currentTimeMarker.Rotate(0f, 0f, (this.targetTimedSwitch.onDuration + this.targetTimedSwitch.timeElapsedInCurrentState) / this.phaseLength * -360f);
	}

	// Token: 0x0600AF93 RID: 44947 RVA: 0x0042B22C File Offset: 0x0042942C
	private void UpdateDurationValueFromTextInput(float newValue, KSlider slider)
	{
		if (newValue < slider.minValue)
		{
			newValue = slider.minValue;
		}
		if (newValue > slider.maxValue)
		{
			newValue = slider.maxValue;
		}
		slider.value = newValue;
		NonLinearSlider nonLinearSlider = slider as NonLinearSlider;
		if (nonLinearSlider != null)
		{
			slider.value = nonLinearSlider.GetPercentageFromValue(newValue);
			return;
		}
		slider.value = newValue;
	}

	// Token: 0x0600AF94 RID: 44948 RVA: 0x00116BD3 File Offset: 0x00114DD3
	private void ResetTimer()
	{
		this.targetTimedSwitch.ResetTimer();
	}

	// Token: 0x040089EF RID: 35311
	public Image greenActiveZone;

	// Token: 0x040089F0 RID: 35312
	public Image redActiveZone;

	// Token: 0x040089F1 RID: 35313
	private LogicTimerSensor targetTimedSwitch;

	// Token: 0x040089F2 RID: 35314
	public KToggle modeButton;

	// Token: 0x040089F3 RID: 35315
	public KButton resetButton;

	// Token: 0x040089F4 RID: 35316
	public KSlider onDurationSlider;

	// Token: 0x040089F5 RID: 35317
	[SerializeField]
	private KNumberInputField onDurationNumberInput;

	// Token: 0x040089F6 RID: 35318
	public KSlider offDurationSlider;

	// Token: 0x040089F7 RID: 35319
	[SerializeField]
	private KNumberInputField offDurationNumberInput;

	// Token: 0x040089F8 RID: 35320
	public RectTransform endIndicator;

	// Token: 0x040089F9 RID: 35321
	public RectTransform currentTimeMarker;

	// Token: 0x040089FA RID: 35322
	public LocText labelHeaderOnDuration;

	// Token: 0x040089FB RID: 35323
	public LocText labelHeaderOffDuration;

	// Token: 0x040089FC RID: 35324
	public LocText labelValueOnDuration;

	// Token: 0x040089FD RID: 35325
	public LocText labelValueOffDuration;

	// Token: 0x040089FE RID: 35326
	public LocText timeLeft;

	// Token: 0x040089FF RID: 35327
	public float phaseLength;

	// Token: 0x04008A00 RID: 35328
	private bool cyclesMode;

	// Token: 0x04008A01 RID: 35329
	[SerializeField]
	private float minSeconds;

	// Token: 0x04008A02 RID: 35330
	[SerializeField]
	private float maxSeconds = 600f;

	// Token: 0x04008A03 RID: 35331
	[SerializeField]
	private float minCycles;

	// Token: 0x04008A04 RID: 35332
	[SerializeField]
	private float maxCycles = 10f;

	// Token: 0x04008A05 RID: 35333
	private const int CYCLEMODE_DECIMALS = 2;

	// Token: 0x04008A06 RID: 35334
	private const int SECONDSMODE_DECIMALS = 1;
}
