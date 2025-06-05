using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x02001BA1 RID: 7073
public class TemperatureOverlayThresholdAdjustmentWidget : KMonoBehaviour
{
	// Token: 0x0600949B RID: 38043 RVA: 0x001057C0 File Offset: 0x001039C0
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.scrollbar.onValueChanged.AddListener(new UnityAction<float>(this.OnValueChanged));
	}

	// Token: 0x0600949C RID: 38044 RVA: 0x003A0548 File Offset: 0x0039E748
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.scrollbar.size = TemperatureOverlayThresholdAdjustmentWidget.temperatureWindowSize / TemperatureOverlayThresholdAdjustmentWidget.maxTemperatureRange;
		this.scrollbar.value = this.KelvinToScrollPercentage(SaveGame.Instance.relativeTemperatureOverlaySliderValue);
		this.defaultButton.onClick += this.OnDefaultPressed;
	}

	// Token: 0x0600949D RID: 38045 RVA: 0x001057E4 File Offset: 0x001039E4
	private void OnValueChanged(float data)
	{
		this.SetUserConfig(data);
	}

	// Token: 0x0600949E RID: 38046 RVA: 0x001057ED File Offset: 0x001039ED
	private float KelvinToScrollPercentage(float kelvin)
	{
		kelvin -= TemperatureOverlayThresholdAdjustmentWidget.minimumSelectionTemperature;
		if (kelvin < 1f)
		{
			kelvin = 1f;
		}
		return Mathf.Clamp01(kelvin / TemperatureOverlayThresholdAdjustmentWidget.maxTemperatureRange);
	}

	// Token: 0x0600949F RID: 38047 RVA: 0x003A05A4 File Offset: 0x0039E7A4
	private void SetUserConfig(float scrollPercentage)
	{
		float num = TemperatureOverlayThresholdAdjustmentWidget.minimumSelectionTemperature + TemperatureOverlayThresholdAdjustmentWidget.maxTemperatureRange * scrollPercentage;
		float num2 = num - TemperatureOverlayThresholdAdjustmentWidget.temperatureWindowSize / 2f;
		float num3 = num + TemperatureOverlayThresholdAdjustmentWidget.temperatureWindowSize / 2f;
		SimDebugView.Instance.user_temperatureThresholds[0] = num2;
		SimDebugView.Instance.user_temperatureThresholds[1] = num3;
		this.scrollBarRangeCenterText.SetText(GameUtil.GetFormattedTemperature(num, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, true));
		this.scrollBarRangeLowText.SetText(GameUtil.GetFormattedTemperature((float)Mathf.RoundToInt(num2), GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, true));
		this.scrollBarRangeHighText.SetText(GameUtil.GetFormattedTemperature((float)Mathf.RoundToInt(num3), GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, true));
		SaveGame.Instance.relativeTemperatureOverlaySliderValue = num;
	}

	// Token: 0x060094A0 RID: 38048 RVA: 0x00105813 File Offset: 0x00103A13
	private void OnDefaultPressed()
	{
		this.scrollbar.value = this.KelvinToScrollPercentage(294.15f);
	}

	// Token: 0x040070BD RID: 28861
	public const float DEFAULT_TEMPERATURE = 294.15f;

	// Token: 0x040070BE RID: 28862
	[SerializeField]
	private Scrollbar scrollbar;

	// Token: 0x040070BF RID: 28863
	[SerializeField]
	private LocText scrollBarRangeLowText;

	// Token: 0x040070C0 RID: 28864
	[SerializeField]
	private LocText scrollBarRangeCenterText;

	// Token: 0x040070C1 RID: 28865
	[SerializeField]
	private LocText scrollBarRangeHighText;

	// Token: 0x040070C2 RID: 28866
	[SerializeField]
	private KButton defaultButton;

	// Token: 0x040070C3 RID: 28867
	private static float maxTemperatureRange = 700f;

	// Token: 0x040070C4 RID: 28868
	private static float temperatureWindowSize = 200f;

	// Token: 0x040070C5 RID: 28869
	private static float minimumSelectionTemperature = TemperatureOverlayThresholdAdjustmentWidget.temperatureWindowSize / 2f;
}
