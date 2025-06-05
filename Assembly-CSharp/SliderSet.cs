using System;
using UnityEngine;

// Token: 0x0200203E RID: 8254
[Serializable]
public class SliderSet
{
	// Token: 0x0600AF16 RID: 44822 RVA: 0x00428E7C File Offset: 0x0042707C
	public void SetupSlider(int index)
	{
		this.index = index;
		this.valueSlider.onReleaseHandle += delegate()
		{
			this.valueSlider.value = Mathf.Round(this.valueSlider.value * 10f) / 10f;
			this.ReceiveValueFromSlider();
		};
		this.valueSlider.onDrag += delegate()
		{
			this.ReceiveValueFromSlider();
		};
		this.valueSlider.onMove += delegate()
		{
			this.ReceiveValueFromSlider();
		};
		this.valueSlider.onPointerDown += delegate()
		{
			this.ReceiveValueFromSlider();
		};
		this.numberInput.onEndEdit += delegate()
		{
			this.ReceiveValueFromInput();
		};
	}

	// Token: 0x0600AF17 RID: 44823 RVA: 0x00428F04 File Offset: 0x00427104
	public void SetTarget(ISliderControl target, int index)
	{
		this.index = index;
		this.target = target;
		ToolTip component = this.valueSlider.handleRect.GetComponent<ToolTip>();
		if (component != null)
		{
			component.SetSimpleTooltip(target.GetSliderTooltip(index));
		}
		if (this.targetLabel != null)
		{
			this.targetLabel.text = ((target.SliderTitleKey != null) ? Strings.Get(target.SliderTitleKey) : "");
		}
		this.unitsLabel.text = target.SliderUnits;
		this.minLabel.text = target.GetSliderMin(index).ToString() + target.SliderUnits;
		this.maxLabel.text = target.GetSliderMax(index).ToString() + target.SliderUnits;
		this.numberInput.minValue = target.GetSliderMin(index);
		this.numberInput.maxValue = target.GetSliderMax(index);
		this.numberInput.decimalPlaces = target.SliderDecimalPlaces(index);
		this.numberInput.field.characterLimit = Mathf.FloorToInt(1f + Mathf.Log10(this.numberInput.maxValue + (float)this.numberInput.decimalPlaces));
		Vector2 sizeDelta = this.numberInput.GetComponent<RectTransform>().sizeDelta;
		sizeDelta.x = (float)((this.numberInput.field.characterLimit + 1) * 10);
		this.numberInput.GetComponent<RectTransform>().sizeDelta = sizeDelta;
		this.valueSlider.minValue = target.GetSliderMin(index);
		this.valueSlider.maxValue = target.GetSliderMax(index);
		this.valueSlider.value = target.GetSliderValue(index);
		this.SetValue(target.GetSliderValue(index));
		if (index == 0)
		{
			this.numberInput.Activate();
		}
	}

	// Token: 0x0600AF18 RID: 44824 RVA: 0x004290D8 File Offset: 0x004272D8
	private void ReceiveValueFromSlider()
	{
		float num = this.valueSlider.value;
		if (this.numberInput.decimalPlaces != -1)
		{
			float num2 = Mathf.Pow(10f, (float)this.numberInput.decimalPlaces);
			num = Mathf.Round(num * num2) / num2;
		}
		this.SetValue(num);
	}

	// Token: 0x0600AF19 RID: 44825 RVA: 0x00429128 File Offset: 0x00427328
	private void ReceiveValueFromInput()
	{
		float num = this.numberInput.currentValue;
		if (this.numberInput.decimalPlaces != -1)
		{
			float num2 = Mathf.Pow(10f, (float)this.numberInput.decimalPlaces);
			num = Mathf.Round(num * num2) / num2;
		}
		this.valueSlider.value = num;
		this.SetValue(num);
	}

	// Token: 0x0600AF1A RID: 44826 RVA: 0x00429184 File Offset: 0x00427384
	private void SetValue(float value)
	{
		float num = value;
		if (num > this.target.GetSliderMax(this.index))
		{
			num = this.target.GetSliderMax(this.index);
		}
		else if (num < this.target.GetSliderMin(this.index))
		{
			num = this.target.GetSliderMin(this.index);
		}
		this.UpdateLabel(num);
		this.target.SetSliderValue(num, this.index);
		ToolTip component = this.valueSlider.handleRect.GetComponent<ToolTip>();
		if (component != null)
		{
			component.SetSimpleTooltip(this.target.GetSliderTooltip(this.index));
		}
	}

	// Token: 0x0600AF1B RID: 44827 RVA: 0x0042922C File Offset: 0x0042742C
	private void UpdateLabel(float value)
	{
		float num = Mathf.Round(value * 10f) / 10f;
		this.numberInput.SetDisplayValue(num.ToString());
	}

	// Token: 0x0400899E RID: 35230
	public KSlider valueSlider;

	// Token: 0x0400899F RID: 35231
	public KNumberInputField numberInput;

	// Token: 0x040089A0 RID: 35232
	public LocText targetLabel;

	// Token: 0x040089A1 RID: 35233
	public LocText unitsLabel;

	// Token: 0x040089A2 RID: 35234
	public LocText minLabel;

	// Token: 0x040089A3 RID: 35235
	public LocText maxLabel;

	// Token: 0x040089A4 RID: 35236
	[NonSerialized]
	public int index;

	// Token: 0x040089A5 RID: 35237
	private ISliderControl target;
}
