using System;
using STRINGS;
using TMPro;
using UnityEngine;

// Token: 0x0200204A RID: 8266
public class ThresholdSwitchSideScreen : SideScreenContent, IRender200ms
{
	// Token: 0x0600AF63 RID: 44899 RVA: 0x0042A00C File Offset: 0x0042820C
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.aboveToggle.onClick += delegate()
		{
			this.OnConditionButtonClicked(true);
		};
		this.belowToggle.onClick += delegate()
		{
			this.OnConditionButtonClicked(false);
		};
		LocText component = this.aboveToggle.transform.GetChild(0).GetComponent<LocText>();
		TMP_Text component2 = this.belowToggle.transform.GetChild(0).GetComponent<LocText>();
		component.SetText(UI.UISIDESCREENS.THRESHOLD_SWITCH_SIDESCREEN.ABOVE_BUTTON);
		component2.SetText(UI.UISIDESCREENS.THRESHOLD_SWITCH_SIDESCREEN.BELOW_BUTTON);
		this.thresholdSlider.onDrag += delegate()
		{
			this.ReceiveValueFromSlider(this.thresholdSlider.GetValueForPercentage(GameUtil.GetRoundedTemperatureInKelvin(this.thresholdSlider.value)));
		};
		this.thresholdSlider.onPointerDown += delegate()
		{
			this.ReceiveValueFromSlider(this.thresholdSlider.GetValueForPercentage(GameUtil.GetRoundedTemperatureInKelvin(this.thresholdSlider.value)));
		};
		this.thresholdSlider.onMove += delegate()
		{
			this.ReceiveValueFromSlider(this.thresholdSlider.GetValueForPercentage(GameUtil.GetRoundedTemperatureInKelvin(this.thresholdSlider.value)));
		};
		this.numberInput.onEndEdit += delegate()
		{
			this.ReceiveValueFromInput(this.numberInput.currentValue);
		};
		this.numberInput.decimalPlaces = 1;
	}

	// Token: 0x0600AF64 RID: 44900 RVA: 0x0011692F File Offset: 0x00114B2F
	public void Render200ms(float dt)
	{
		if (this.target == null)
		{
			this.target = null;
			return;
		}
		this.UpdateLabels();
	}

	// Token: 0x0600AF65 RID: 44901 RVA: 0x0011694D File Offset: 0x00114B4D
	public override bool IsValidForTarget(GameObject target)
	{
		return target.GetComponent<IThresholdSwitch>() != null;
	}

	// Token: 0x0600AF66 RID: 44902 RVA: 0x0042A104 File Offset: 0x00428304
	public override void SetTarget(GameObject new_target)
	{
		this.target = null;
		if (new_target == null)
		{
			global::Debug.LogError("Invalid gameObject received");
			return;
		}
		this.target = new_target;
		this.thresholdSwitch = this.target.GetComponent<IThresholdSwitch>();
		if (this.thresholdSwitch == null)
		{
			this.target = null;
			global::Debug.LogError("The gameObject received does not contain a IThresholdSwitch component");
			return;
		}
		this.UpdateLabels();
		if (this.target.GetComponent<IThresholdSwitch>().LayoutType == ThresholdScreenLayoutType.SliderBar)
		{
			this.thresholdSlider.gameObject.SetActive(true);
			this.thresholdSlider.minValue = 0f;
			this.thresholdSlider.maxValue = 100f;
			this.thresholdSlider.SetRanges(this.thresholdSwitch.GetRanges);
			this.thresholdSlider.value = this.thresholdSlider.GetPercentageFromValue(this.thresholdSwitch.Threshold);
			this.thresholdSlider.GetComponentInChildren<ToolTip>();
		}
		else
		{
			this.thresholdSlider.gameObject.SetActive(false);
		}
		MultiToggle incrementMinorToggle = this.incrementMinor.GetComponent<MultiToggle>();
		incrementMinorToggle.onClick = delegate()
		{
			this.UpdateThresholdValue(this.thresholdSwitch.Threshold + (float)this.thresholdSwitch.IncrementScale);
			incrementMinorToggle.ChangeState(1);
		};
		incrementMinorToggle.onStopHold = delegate()
		{
			incrementMinorToggle.ChangeState(0);
		};
		MultiToggle incrementMajorToggle = this.incrementMajor.GetComponent<MultiToggle>();
		incrementMajorToggle.onClick = delegate()
		{
			this.UpdateThresholdValue(this.thresholdSwitch.Threshold + 10f * (float)this.thresholdSwitch.IncrementScale);
			incrementMajorToggle.ChangeState(1);
		};
		incrementMajorToggle.onStopHold = delegate()
		{
			incrementMajorToggle.ChangeState(0);
		};
		MultiToggle decrementMinorToggle = this.decrementMinor.GetComponent<MultiToggle>();
		decrementMinorToggle.onClick = delegate()
		{
			this.UpdateThresholdValue(this.thresholdSwitch.Threshold - (float)this.thresholdSwitch.IncrementScale);
			decrementMinorToggle.ChangeState(1);
		};
		decrementMinorToggle.onStopHold = delegate()
		{
			decrementMinorToggle.ChangeState(0);
		};
		MultiToggle decrementMajorToggle = this.decrementMajor.GetComponent<MultiToggle>();
		decrementMajorToggle.onClick = delegate()
		{
			this.UpdateThresholdValue(this.thresholdSwitch.Threshold - 10f * (float)this.thresholdSwitch.IncrementScale);
			decrementMajorToggle.ChangeState(1);
		};
		decrementMajorToggle.onStopHold = delegate()
		{
			decrementMajorToggle.ChangeState(0);
		};
		this.unitsLabel.text = this.thresholdSwitch.ThresholdValueUnits();
		this.numberInput.minValue = this.thresholdSwitch.GetRangeMinInputField();
		this.numberInput.maxValue = this.thresholdSwitch.GetRangeMaxInputField();
		this.numberInput.Activate();
		this.UpdateTargetThresholdLabel();
		this.OnConditionButtonClicked(this.thresholdSwitch.ActivateAboveThreshold);
	}

	// Token: 0x0600AF67 RID: 44903 RVA: 0x00116958 File Offset: 0x00114B58
	private void OnThresholdValueChanged(float new_value)
	{
		this.thresholdSwitch.Threshold = new_value;
		this.UpdateTargetThresholdLabel();
	}

	// Token: 0x0600AF68 RID: 44904 RVA: 0x0042A370 File Offset: 0x00428570
	private void OnConditionButtonClicked(bool activate_above_threshold)
	{
		this.thresholdSwitch.ActivateAboveThreshold = activate_above_threshold;
		if (activate_above_threshold)
		{
			this.belowToggle.isOn = true;
			this.aboveToggle.isOn = false;
			this.belowToggle.GetComponent<ImageToggleState>().SetState(ImageToggleState.State.Inactive);
			this.aboveToggle.GetComponent<ImageToggleState>().SetState(ImageToggleState.State.Active);
		}
		else
		{
			this.belowToggle.isOn = false;
			this.aboveToggle.isOn = true;
			this.belowToggle.GetComponent<ImageToggleState>().SetState(ImageToggleState.State.Active);
			this.aboveToggle.GetComponent<ImageToggleState>().SetState(ImageToggleState.State.Inactive);
		}
		this.UpdateTargetThresholdLabel();
	}

	// Token: 0x0600AF69 RID: 44905 RVA: 0x0042A408 File Offset: 0x00428608
	private void UpdateTargetThresholdLabel()
	{
		this.numberInput.SetDisplayValue(this.thresholdSwitch.Format(this.thresholdSwitch.Threshold, false) + this.thresholdSwitch.ThresholdValueUnits());
		if (this.thresholdSwitch.ActivateAboveThreshold)
		{
			this.thresholdSlider.GetComponentInChildren<ToolTip>().SetSimpleTooltip(string.Format(this.thresholdSwitch.AboveToolTip, this.thresholdSwitch.Format(this.thresholdSwitch.Threshold, true)));
			this.thresholdSlider.GetComponentInChildren<ToolTip>().tooltipPositionOffset = new Vector2(0f, 25f);
			return;
		}
		this.thresholdSlider.GetComponentInChildren<ToolTip>().SetSimpleTooltip(string.Format(this.thresholdSwitch.BelowToolTip, this.thresholdSwitch.Format(this.thresholdSwitch.Threshold, true)));
		this.thresholdSlider.GetComponentInChildren<ToolTip>().tooltipPositionOffset = new Vector2(0f, 25f);
	}

	// Token: 0x0600AF6A RID: 44906 RVA: 0x0011696C File Offset: 0x00114B6C
	private void ReceiveValueFromSlider(float newValue)
	{
		this.UpdateThresholdValue(this.thresholdSwitch.ProcessedSliderValue(newValue));
	}

	// Token: 0x0600AF6B RID: 44907 RVA: 0x00116980 File Offset: 0x00114B80
	private void ReceiveValueFromInput(float newValue)
	{
		this.UpdateThresholdValue(this.thresholdSwitch.ProcessedInputValue(newValue));
	}

	// Token: 0x0600AF6C RID: 44908 RVA: 0x0042A508 File Offset: 0x00428708
	private void UpdateThresholdValue(float newValue)
	{
		if (newValue < this.thresholdSwitch.RangeMin)
		{
			newValue = this.thresholdSwitch.RangeMin;
		}
		if (newValue > this.thresholdSwitch.RangeMax)
		{
			newValue = this.thresholdSwitch.RangeMax;
		}
		this.thresholdSwitch.Threshold = newValue;
		NonLinearSlider nonLinearSlider = this.thresholdSlider;
		if (nonLinearSlider != null)
		{
			this.thresholdSlider.value = nonLinearSlider.GetPercentageFromValue(newValue);
		}
		else
		{
			this.thresholdSlider.value = newValue;
		}
		this.UpdateTargetThresholdLabel();
	}

	// Token: 0x0600AF6D RID: 44909 RVA: 0x00116994 File Offset: 0x00114B94
	private void UpdateLabels()
	{
		this.currentValue.text = string.Format(UI.UISIDESCREENS.THRESHOLD_SWITCH_SIDESCREEN.CURRENT_VALUE, this.thresholdSwitch.ThresholdValueName, this.thresholdSwitch.Format(this.thresholdSwitch.CurrentValue, true));
	}

	// Token: 0x0600AF6E RID: 44910 RVA: 0x001169D2 File Offset: 0x00114BD2
	public override string GetTitle()
	{
		if (this.target != null)
		{
			return this.thresholdSwitch.Title;
		}
		return UI.UISIDESCREENS.THRESHOLD_SWITCH_SIDESCREEN.TITLE;
	}

	// Token: 0x040089D2 RID: 35282
	private GameObject target;

	// Token: 0x040089D3 RID: 35283
	private IThresholdSwitch thresholdSwitch;

	// Token: 0x040089D4 RID: 35284
	[SerializeField]
	private LocText currentValue;

	// Token: 0x040089D5 RID: 35285
	[SerializeField]
	private LocText thresholdValue;

	// Token: 0x040089D6 RID: 35286
	[SerializeField]
	private KToggle aboveToggle;

	// Token: 0x040089D7 RID: 35287
	[SerializeField]
	private KToggle belowToggle;

	// Token: 0x040089D8 RID: 35288
	[Header("Slider")]
	[SerializeField]
	private NonLinearSlider thresholdSlider;

	// Token: 0x040089D9 RID: 35289
	[Header("Number Input")]
	[SerializeField]
	private KNumberInputField numberInput;

	// Token: 0x040089DA RID: 35290
	[SerializeField]
	private LocText unitsLabel;

	// Token: 0x040089DB RID: 35291
	[Header("Increment Buttons")]
	[SerializeField]
	private GameObject incrementMinor;

	// Token: 0x040089DC RID: 35292
	[SerializeField]
	private GameObject incrementMajor;

	// Token: 0x040089DD RID: 35293
	[SerializeField]
	private GameObject decrementMinor;

	// Token: 0x040089DE RID: 35294
	[SerializeField]
	private GameObject decrementMajor;
}
