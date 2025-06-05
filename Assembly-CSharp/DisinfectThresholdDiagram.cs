using System;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02001B1A RID: 6938
public class DisinfectThresholdDiagram : MonoBehaviour
{
	// Token: 0x06009153 RID: 37203 RVA: 0x0038CF60 File Offset: 0x0038B160
	private void Start()
	{
		this.inputField.minValue = 0f;
		this.inputField.maxValue = (float)DisinfectThresholdDiagram.MAX_VALUE;
		this.inputField.currentValue = (float)SaveGame.Instance.minGermCountForDisinfect;
		this.inputField.SetDisplayValue(SaveGame.Instance.minGermCountForDisinfect.ToString());
		this.inputField.onEndEdit += delegate()
		{
			this.ReceiveValueFromInput(this.inputField.currentValue);
		};
		this.inputField.decimalPlaces = 1;
		this.inputField.Activate();
		this.slider.minValue = 0f;
		this.slider.maxValue = (float)(DisinfectThresholdDiagram.MAX_VALUE / DisinfectThresholdDiagram.SLIDER_CONVERSION);
		this.slider.wholeNumbers = true;
		this.slider.value = (float)(SaveGame.Instance.minGermCountForDisinfect / DisinfectThresholdDiagram.SLIDER_CONVERSION);
		this.slider.onReleaseHandle += this.OnReleaseHandle;
		this.slider.onDrag += delegate()
		{
			this.ReceiveValueFromSlider(this.slider.value);
		};
		this.slider.onPointerDown += delegate()
		{
			this.ReceiveValueFromSlider(this.slider.value);
		};
		this.slider.onMove += delegate()
		{
			this.ReceiveValueFromSlider(this.slider.value);
			this.OnReleaseHandle();
		};
		this.unitsLabel.SetText(UI.OVERLAYS.DISEASE.DISINFECT_THRESHOLD_DIAGRAM.UNITS);
		this.minLabel.SetText(UI.OVERLAYS.DISEASE.DISINFECT_THRESHOLD_DIAGRAM.MIN_LABEL);
		this.maxLabel.SetText(UI.OVERLAYS.DISEASE.DISINFECT_THRESHOLD_DIAGRAM.MAX_LABEL);
		this.thresholdPrefix.SetText(UI.OVERLAYS.DISEASE.DISINFECT_THRESHOLD_DIAGRAM.THRESHOLD_PREFIX);
		this.toolTip.OnToolTip = delegate()
		{
			this.toolTip.ClearMultiStringTooltip();
			if (SaveGame.Instance.enableAutoDisinfect)
			{
				this.toolTip.AddMultiStringTooltip(UI.OVERLAYS.DISEASE.DISINFECT_THRESHOLD_DIAGRAM.TOOLTIP.ToString().Replace("{NumberOfGerms}", SaveGame.Instance.minGermCountForDisinfect.ToString()), null);
			}
			else
			{
				this.toolTip.AddMultiStringTooltip(UI.OVERLAYS.DISEASE.DISINFECT_THRESHOLD_DIAGRAM.TOOLTIP_DISABLED.ToString(), null);
			}
			return "";
		};
		this.disabledImage.gameObject.SetActive(!SaveGame.Instance.enableAutoDisinfect);
		this.toggle.isOn = SaveGame.Instance.enableAutoDisinfect;
		this.toggle.onValueChanged += this.OnClickToggle;
	}

	// Token: 0x06009154 RID: 37204 RVA: 0x0038D14C File Offset: 0x0038B34C
	private void OnReleaseHandle()
	{
		float num = (float)((int)this.slider.value * DisinfectThresholdDiagram.SLIDER_CONVERSION);
		SaveGame.Instance.minGermCountForDisinfect = (int)num;
		this.inputField.SetDisplayValue(num.ToString());
	}

	// Token: 0x06009155 RID: 37205 RVA: 0x0038D18C File Offset: 0x0038B38C
	private void ReceiveValueFromSlider(float new_value)
	{
		SaveGame.Instance.minGermCountForDisinfect = (int)new_value * DisinfectThresholdDiagram.SLIDER_CONVERSION;
		this.inputField.SetDisplayValue((new_value * (float)DisinfectThresholdDiagram.SLIDER_CONVERSION).ToString());
	}

	// Token: 0x06009156 RID: 37206 RVA: 0x001036AB File Offset: 0x001018AB
	private void ReceiveValueFromInput(float new_value)
	{
		this.slider.value = new_value / (float)DisinfectThresholdDiagram.SLIDER_CONVERSION;
		SaveGame.Instance.minGermCountForDisinfect = (int)new_value;
	}

	// Token: 0x06009157 RID: 37207 RVA: 0x001036CC File Offset: 0x001018CC
	private void OnClickToggle(bool new_value)
	{
		SaveGame.Instance.enableAutoDisinfect = new_value;
		this.disabledImage.gameObject.SetActive(!SaveGame.Instance.enableAutoDisinfect);
	}

	// Token: 0x04006DFA RID: 28154
	[SerializeField]
	private KNumberInputField inputField;

	// Token: 0x04006DFB RID: 28155
	[SerializeField]
	private KSlider slider;

	// Token: 0x04006DFC RID: 28156
	[SerializeField]
	private LocText minLabel;

	// Token: 0x04006DFD RID: 28157
	[SerializeField]
	private LocText maxLabel;

	// Token: 0x04006DFE RID: 28158
	[SerializeField]
	private LocText unitsLabel;

	// Token: 0x04006DFF RID: 28159
	[SerializeField]
	private LocText thresholdPrefix;

	// Token: 0x04006E00 RID: 28160
	[SerializeField]
	private ToolTip toolTip;

	// Token: 0x04006E01 RID: 28161
	[SerializeField]
	private KToggle toggle;

	// Token: 0x04006E02 RID: 28162
	[SerializeField]
	private Image disabledImage;

	// Token: 0x04006E03 RID: 28163
	private static int MAX_VALUE = 1000000;

	// Token: 0x04006E04 RID: 28164
	private static int SLIDER_CONVERSION = 1000;
}
