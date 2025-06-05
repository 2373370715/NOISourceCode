using System;
using STRINGS;
using UnityEngine;

// Token: 0x02001FE5 RID: 8165
public class LimitValveSideScreen : SideScreenContent
{
	// Token: 0x0600AC7E RID: 44158 RVA: 0x0041E1A0 File Offset: 0x0041C3A0
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.resetButton.onClick += this.ResetCounter;
		this.limitSlider.onReleaseHandle += this.OnReleaseHandle;
		this.limitSlider.onDrag += delegate()
		{
			this.ReceiveValueFromSlider(this.limitSlider.value);
		};
		this.limitSlider.onPointerDown += delegate()
		{
			this.ReceiveValueFromSlider(this.limitSlider.value);
		};
		this.limitSlider.onMove += delegate()
		{
			this.ReceiveValueFromSlider(this.limitSlider.value);
			this.OnReleaseHandle();
		};
		this.numberInput.onEndEdit += delegate()
		{
			this.ReceiveValueFromInput(this.numberInput.currentValue);
		};
		this.numberInput.decimalPlaces = 3;
	}

	// Token: 0x0600AC7F RID: 44159 RVA: 0x0011499E File Offset: 0x00112B9E
	public void OnReleaseHandle()
	{
		this.targetLimitValve.Limit = this.targetLimit;
	}

	// Token: 0x0600AC80 RID: 44160 RVA: 0x001149B1 File Offset: 0x00112BB1
	public override bool IsValidForTarget(GameObject target)
	{
		return target.GetComponent<LimitValve>() != null;
	}

	// Token: 0x0600AC81 RID: 44161 RVA: 0x0041E24C File Offset: 0x0041C44C
	public override void SetTarget(GameObject target)
	{
		this.targetLimitValve = target.GetComponent<LimitValve>();
		if (this.targetLimitValve == null)
		{
			global::Debug.LogError("The target object does not have a LimitValve component.");
			return;
		}
		if (this.targetLimitValveSubHandle != -1)
		{
			base.Unsubscribe(this.targetLimitValveSubHandle);
		}
		this.targetLimitValveSubHandle = this.targetLimitValve.Subscribe(-1722241721, new Action<object>(this.UpdateAmountLabel));
		this.limitSlider.minValue = 0f;
		this.limitSlider.maxValue = 100f;
		this.limitSlider.SetRanges(this.targetLimitValve.GetRanges());
		this.limitSlider.value = this.limitSlider.GetPercentageFromValue(this.targetLimitValve.Limit);
		this.numberInput.minValue = 0f;
		this.numberInput.maxValue = this.targetLimitValve.maxLimitKg;
		this.numberInput.Activate();
		if (this.targetLimitValve.displayUnitsInsteadOfMass)
		{
			this.minLimitLabel.text = GameUtil.GetFormattedUnits(0f, GameUtil.TimeSlice.None, true, "");
			this.maxLimitLabel.text = GameUtil.GetFormattedUnits(this.targetLimitValve.maxLimitKg, GameUtil.TimeSlice.None, true, "");
			this.numberInput.SetDisplayValue(GameUtil.GetFormattedUnits(Mathf.Max(0f, this.targetLimitValve.Limit), GameUtil.TimeSlice.None, false, LimitValveSideScreen.FLOAT_FORMAT));
			this.unitsLabel.text = UI.UNITSUFFIXES.UNITS;
			this.toolTip.enabled = true;
			this.toolTip.SetSimpleTooltip(UI.UISIDESCREENS.LIMIT_VALVE_SIDE_SCREEN.SLIDER_TOOLTIP_UNITS);
		}
		else
		{
			this.minLimitLabel.text = GameUtil.GetFormattedMass(0f, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.Kilogram, true, "{0:0.#}");
			this.maxLimitLabel.text = GameUtil.GetFormattedMass(this.targetLimitValve.maxLimitKg, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.Kilogram, true, "{0:0.#}");
			this.numberInput.SetDisplayValue(GameUtil.GetFormattedMass(Mathf.Max(0f, this.targetLimitValve.Limit), GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.Kilogram, false, LimitValveSideScreen.FLOAT_FORMAT));
			this.unitsLabel.text = GameUtil.GetCurrentMassUnit(false);
			this.toolTip.enabled = false;
		}
		this.UpdateAmountLabel(null);
	}

	// Token: 0x0600AC82 RID: 44162 RVA: 0x0041E488 File Offset: 0x0041C688
	private void UpdateAmountLabel(object obj = null)
	{
		if (this.targetLimitValve.displayUnitsInsteadOfMass)
		{
			string formattedUnits = GameUtil.GetFormattedUnits(this.targetLimitValve.Amount, GameUtil.TimeSlice.None, true, LimitValveSideScreen.FLOAT_FORMAT);
			this.amountLabel.text = string.Format(UI.UISIDESCREENS.LIMIT_VALVE_SIDE_SCREEN.AMOUNT, formattedUnits);
			return;
		}
		string formattedMass = GameUtil.GetFormattedMass(this.targetLimitValve.Amount, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.Kilogram, true, LimitValveSideScreen.FLOAT_FORMAT);
		this.amountLabel.text = string.Format(UI.UISIDESCREENS.LIMIT_VALVE_SIDE_SCREEN.AMOUNT, formattedMass);
	}

	// Token: 0x0600AC83 RID: 44163 RVA: 0x001149BF File Offset: 0x00112BBF
	private void ResetCounter()
	{
		this.targetLimitValve.ResetAmount();
	}

	// Token: 0x0600AC84 RID: 44164 RVA: 0x0041E50C File Offset: 0x0041C70C
	private void ReceiveValueFromSlider(float sliderPercentage)
	{
		float num = this.limitSlider.GetValueForPercentage(sliderPercentage);
		num = (float)Mathf.RoundToInt(num);
		this.UpdateLimitValue(num);
	}

	// Token: 0x0600AC85 RID: 44165 RVA: 0x001149CC File Offset: 0x00112BCC
	private void ReceiveValueFromInput(float input)
	{
		this.UpdateLimitValue(input);
		this.targetLimitValve.Limit = this.targetLimit;
	}

	// Token: 0x0600AC86 RID: 44166 RVA: 0x0041E538 File Offset: 0x0041C738
	private void UpdateLimitValue(float newValue)
	{
		this.targetLimit = newValue;
		this.limitSlider.value = this.limitSlider.GetPercentageFromValue(newValue);
		if (this.targetLimitValve.displayUnitsInsteadOfMass)
		{
			this.numberInput.SetDisplayValue(GameUtil.GetFormattedUnits(newValue, GameUtil.TimeSlice.None, false, LimitValveSideScreen.FLOAT_FORMAT));
			return;
		}
		this.numberInput.SetDisplayValue(GameUtil.GetFormattedMass(newValue, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.Kilogram, false, LimitValveSideScreen.FLOAT_FORMAT));
	}

	// Token: 0x040087D2 RID: 34770
	public static readonly string FLOAT_FORMAT = "{0:0.#####}";

	// Token: 0x040087D3 RID: 34771
	private LimitValve targetLimitValve;

	// Token: 0x040087D4 RID: 34772
	[Header("State")]
	[SerializeField]
	private LocText amountLabel;

	// Token: 0x040087D5 RID: 34773
	[SerializeField]
	private KButton resetButton;

	// Token: 0x040087D6 RID: 34774
	[Header("Slider")]
	[SerializeField]
	private NonLinearSlider limitSlider;

	// Token: 0x040087D7 RID: 34775
	[SerializeField]
	private LocText minLimitLabel;

	// Token: 0x040087D8 RID: 34776
	[SerializeField]
	private LocText maxLimitLabel;

	// Token: 0x040087D9 RID: 34777
	[SerializeField]
	private ToolTip toolTip;

	// Token: 0x040087DA RID: 34778
	[Header("Input Field")]
	[SerializeField]
	private KNumberInputField numberInput;

	// Token: 0x040087DB RID: 34779
	[SerializeField]
	private LocText unitsLabel;

	// Token: 0x040087DC RID: 34780
	private float targetLimit;

	// Token: 0x040087DD RID: 34781
	private int targetLimitValveSubHandle = -1;
}
