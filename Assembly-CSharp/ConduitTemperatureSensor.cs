using System;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x02000D41 RID: 3393
[SerializationConfig(MemberSerialization.OptIn)]
public class ConduitTemperatureSensor : ConduitThresholdSensor, IThresholdSwitch
{
	// Token: 0x060041C1 RID: 16833 RVA: 0x0024D034 File Offset: 0x0024B234
	private void GetContentsTemperature(out float temperature, out bool hasMass)
	{
		int cell = Grid.PosToCell(this);
		if (this.conduitType == ConduitType.Liquid || this.conduitType == ConduitType.Gas)
		{
			ConduitFlow.ConduitContents contents = Conduit.GetFlowManager(this.conduitType).GetContents(cell);
			temperature = contents.temperature;
			hasMass = (contents.mass > 0f);
			return;
		}
		SolidConduitFlow flowManager = SolidConduit.GetFlowManager();
		SolidConduitFlow.ConduitContents contents2 = flowManager.GetContents(cell);
		Pickupable pickupable = flowManager.GetPickupable(contents2.pickupableHandle);
		if (pickupable != null && pickupable.PrimaryElement.Mass > 0f)
		{
			temperature = pickupable.PrimaryElement.Temperature;
			hasMass = true;
			return;
		}
		temperature = 0f;
		hasMass = false;
	}

	// Token: 0x17000334 RID: 820
	// (get) Token: 0x060041C2 RID: 16834 RVA: 0x0024D0D4 File Offset: 0x0024B2D4
	public override float CurrentValue
	{
		get
		{
			float num;
			bool flag;
			this.GetContentsTemperature(out num, out flag);
			if (flag)
			{
				this.lastValue = num;
			}
			return this.lastValue;
		}
	}

	// Token: 0x17000335 RID: 821
	// (get) Token: 0x060041C3 RID: 16835 RVA: 0x000CEF7D File Offset: 0x000CD17D
	public float RangeMin
	{
		get
		{
			return this.rangeMin;
		}
	}

	// Token: 0x17000336 RID: 822
	// (get) Token: 0x060041C4 RID: 16836 RVA: 0x000CEF85 File Offset: 0x000CD185
	public float RangeMax
	{
		get
		{
			return this.rangeMax;
		}
	}

	// Token: 0x060041C5 RID: 16837 RVA: 0x000CEF8D File Offset: 0x000CD18D
	public float GetRangeMinInputField()
	{
		return GameUtil.GetConvertedTemperature(this.RangeMin, false);
	}

	// Token: 0x060041C6 RID: 16838 RVA: 0x000CEF9B File Offset: 0x000CD19B
	public float GetRangeMaxInputField()
	{
		return GameUtil.GetConvertedTemperature(this.RangeMax, false);
	}

	// Token: 0x17000337 RID: 823
	// (get) Token: 0x060041C7 RID: 16839 RVA: 0x000CEFA9 File Offset: 0x000CD1A9
	public LocString Title
	{
		get
		{
			return UI.UISIDESCREENS.TEMPERATURESWITCHSIDESCREEN.TITLE;
		}
	}

	// Token: 0x17000338 RID: 824
	// (get) Token: 0x060041C8 RID: 16840 RVA: 0x000CEFB0 File Offset: 0x000CD1B0
	public LocString ThresholdValueName
	{
		get
		{
			return UI.UISIDESCREENS.THRESHOLD_SWITCH_SIDESCREEN.CONTENT_TEMPERATURE;
		}
	}

	// Token: 0x17000339 RID: 825
	// (get) Token: 0x060041C9 RID: 16841 RVA: 0x000CEFB7 File Offset: 0x000CD1B7
	public string AboveToolTip
	{
		get
		{
			return UI.UISIDESCREENS.THRESHOLD_SWITCH_SIDESCREEN.CONTENT_TEMPERATURE_TOOLTIP_ABOVE;
		}
	}

	// Token: 0x1700033A RID: 826
	// (get) Token: 0x060041CA RID: 16842 RVA: 0x000CEFC3 File Offset: 0x000CD1C3
	public string BelowToolTip
	{
		get
		{
			return UI.UISIDESCREENS.THRESHOLD_SWITCH_SIDESCREEN.CONTENT_TEMPERATURE_TOOLTIP_BELOW;
		}
	}

	// Token: 0x060041CB RID: 16843 RVA: 0x000CEFCF File Offset: 0x000CD1CF
	public string Format(float value, bool units)
	{
		return GameUtil.GetFormattedTemperature(value, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, units, false);
	}

	// Token: 0x060041CC RID: 16844 RVA: 0x000CEFDB File Offset: 0x000CD1DB
	public float ProcessedSliderValue(float input)
	{
		return Mathf.Round(input);
	}

	// Token: 0x060041CD RID: 16845 RVA: 0x000CEFE3 File Offset: 0x000CD1E3
	public float ProcessedInputValue(float input)
	{
		return GameUtil.GetTemperatureConvertedToKelvin(input);
	}

	// Token: 0x060041CE RID: 16846 RVA: 0x0024D0FC File Offset: 0x0024B2FC
	public LocString ThresholdValueUnits()
	{
		LocString result = null;
		switch (GameUtil.temperatureUnit)
		{
		case GameUtil.TemperatureUnit.Celsius:
			result = UI.UNITSUFFIXES.TEMPERATURE.CELSIUS;
			break;
		case GameUtil.TemperatureUnit.Fahrenheit:
			result = UI.UNITSUFFIXES.TEMPERATURE.FAHRENHEIT;
			break;
		case GameUtil.TemperatureUnit.Kelvin:
			result = UI.UNITSUFFIXES.TEMPERATURE.KELVIN;
			break;
		}
		return result;
	}

	// Token: 0x1700033B RID: 827
	// (get) Token: 0x060041CF RID: 16847 RVA: 0x000B1628 File Offset: 0x000AF828
	public ThresholdScreenLayoutType LayoutType
	{
		get
		{
			return ThresholdScreenLayoutType.SliderBar;
		}
	}

	// Token: 0x1700033C RID: 828
	// (get) Token: 0x060041D0 RID: 16848 RVA: 0x000AA7E7 File Offset: 0x000A89E7
	public int IncrementScale
	{
		get
		{
			return 1;
		}
	}

	// Token: 0x1700033D RID: 829
	// (get) Token: 0x060041D1 RID: 16849 RVA: 0x0024D13C File Offset: 0x0024B33C
	public NonLinearSlider.Range[] GetRanges
	{
		get
		{
			return new NonLinearSlider.Range[]
			{
				new NonLinearSlider.Range(25f, 260f),
				new NonLinearSlider.Range(50f, 400f),
				new NonLinearSlider.Range(12f, 1500f),
				new NonLinearSlider.Range(13f, 10000f)
			};
		}
	}

	// Token: 0x04002D63 RID: 11619
	public float rangeMin;

	// Token: 0x04002D64 RID: 11620
	public float rangeMax = 373.15f;

	// Token: 0x04002D65 RID: 11621
	[Serialize]
	private float lastValue;
}
