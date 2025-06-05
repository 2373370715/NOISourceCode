using System;
using STRINGS;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x02002047 RID: 8263
public class TemperatureSwitchSideScreen : SideScreenContent, IRender200ms
{
	// Token: 0x0600AF4F RID: 44879 RVA: 0x00429D48 File Offset: 0x00427F48
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.coolerToggle.onClick += delegate()
		{
			this.OnConditionButtonClicked(false);
		};
		this.warmerToggle.onClick += delegate()
		{
			this.OnConditionButtonClicked(true);
		};
		LocText component = this.coolerToggle.transform.GetChild(0).GetComponent<LocText>();
		TMP_Text component2 = this.warmerToggle.transform.GetChild(0).GetComponent<LocText>();
		component.SetText(UI.UISIDESCREENS.TEMPERATURESWITCHSIDESCREEN.COLDER_BUTTON);
		component2.SetText(UI.UISIDESCREENS.TEMPERATURESWITCHSIDESCREEN.WARMER_BUTTON);
		Slider.SliderEvent sliderEvent = new Slider.SliderEvent();
		sliderEvent.AddListener(new UnityAction<float>(this.OnTargetTemperatureChanged));
		this.targetTemperatureSlider.onValueChanged = sliderEvent;
	}

	// Token: 0x0600AF50 RID: 44880 RVA: 0x00116859 File Offset: 0x00114A59
	public void Render200ms(float dt)
	{
		if (this.targetTemperatureSwitch == null)
		{
			return;
		}
		this.UpdateLabels();
	}

	// Token: 0x0600AF51 RID: 44881 RVA: 0x00116870 File Offset: 0x00114A70
	public override bool IsValidForTarget(GameObject target)
	{
		return target.GetComponent<TemperatureControlledSwitch>() != null;
	}

	// Token: 0x0600AF52 RID: 44882 RVA: 0x00429DFC File Offset: 0x00427FFC
	public override void SetTarget(GameObject target)
	{
		if (target == null)
		{
			global::Debug.LogError("Invalid gameObject received");
			return;
		}
		this.targetTemperatureSwitch = target.GetComponent<TemperatureControlledSwitch>();
		if (this.targetTemperatureSwitch == null)
		{
			global::Debug.LogError("The gameObject received does not contain a TimedSwitch component");
			return;
		}
		this.UpdateLabels();
		this.UpdateTargetTemperatureLabel();
		this.OnConditionButtonClicked(this.targetTemperatureSwitch.activateOnWarmerThan);
	}

	// Token: 0x0600AF53 RID: 44883 RVA: 0x0011687E File Offset: 0x00114A7E
	private void OnTargetTemperatureChanged(float new_value)
	{
		this.targetTemperatureSwitch.thresholdTemperature = new_value;
		this.UpdateTargetTemperatureLabel();
	}

	// Token: 0x0600AF54 RID: 44884 RVA: 0x00429E60 File Offset: 0x00428060
	private void OnConditionButtonClicked(bool isWarmer)
	{
		this.targetTemperatureSwitch.activateOnWarmerThan = isWarmer;
		if (isWarmer)
		{
			this.coolerToggle.isOn = false;
			this.warmerToggle.isOn = true;
			this.coolerToggle.GetComponent<ImageToggleState>().SetState(ImageToggleState.State.Inactive);
			this.warmerToggle.GetComponent<ImageToggleState>().SetState(ImageToggleState.State.Active);
			return;
		}
		this.coolerToggle.isOn = true;
		this.warmerToggle.isOn = false;
		this.coolerToggle.GetComponent<ImageToggleState>().SetState(ImageToggleState.State.Active);
		this.warmerToggle.GetComponent<ImageToggleState>().SetState(ImageToggleState.State.Inactive);
	}

	// Token: 0x0600AF55 RID: 44885 RVA: 0x00116892 File Offset: 0x00114A92
	private void UpdateTargetTemperatureLabel()
	{
		this.targetTemperature.text = GameUtil.GetFormattedTemperature(this.targetTemperatureSwitch.thresholdTemperature, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false);
	}

	// Token: 0x0600AF56 RID: 44886 RVA: 0x001168B3 File Offset: 0x00114AB3
	private void UpdateLabels()
	{
		this.currentTemperature.text = string.Format(UI.UISIDESCREENS.TEMPERATURESWITCHSIDESCREEN.CURRENT_TEMPERATURE, GameUtil.GetFormattedTemperature(this.targetTemperatureSwitch.GetTemperature(), GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false));
	}

	// Token: 0x040089C9 RID: 35273
	private TemperatureControlledSwitch targetTemperatureSwitch;

	// Token: 0x040089CA RID: 35274
	[SerializeField]
	private LocText currentTemperature;

	// Token: 0x040089CB RID: 35275
	[SerializeField]
	private LocText targetTemperature;

	// Token: 0x040089CC RID: 35276
	[SerializeField]
	private KToggle coolerToggle;

	// Token: 0x040089CD RID: 35277
	[SerializeField]
	private KToggle warmerToggle;

	// Token: 0x040089CE RID: 35278
	[SerializeField]
	private KSlider targetTemperatureSlider;
}
