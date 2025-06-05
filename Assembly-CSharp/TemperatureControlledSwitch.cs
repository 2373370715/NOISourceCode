using System;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x0200102E RID: 4142
[SerializationConfig(MemberSerialization.OptIn)]
public class TemperatureControlledSwitch : CircuitSwitch, ISaveLoadable, IThresholdSwitch, ISim200ms
{
	// Token: 0x170004C6 RID: 1222
	// (get) Token: 0x060053CE RID: 21454 RVA: 0x00287B30 File Offset: 0x00285D30
	public float StructureTemperature
	{
		get
		{
			return GameComps.StructureTemperatures.GetPayload(this.structureTemperature).Temperature;
		}
	}

	// Token: 0x060053CF RID: 21455 RVA: 0x000DAFF0 File Offset: 0x000D91F0
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.structureTemperature = GameComps.StructureTemperatures.GetHandle(base.gameObject);
	}

	// Token: 0x060053D0 RID: 21456 RVA: 0x00287B58 File Offset: 0x00285D58
	public void Sim200ms(float dt)
	{
		if (this.simUpdateCounter < 8)
		{
			this.temperatures[this.simUpdateCounter] = Grid.Temperature[Grid.PosToCell(this)];
			this.simUpdateCounter++;
			return;
		}
		this.simUpdateCounter = 0;
		this.averageTemp = 0f;
		for (int i = 0; i < 8; i++)
		{
			this.averageTemp += this.temperatures[i];
		}
		this.averageTemp /= 8f;
		if (this.activateOnWarmerThan)
		{
			if ((this.averageTemp > this.thresholdTemperature && !base.IsSwitchedOn) || (this.averageTemp < this.thresholdTemperature && base.IsSwitchedOn))
			{
				this.Toggle();
				return;
			}
		}
		else if ((this.averageTemp > this.thresholdTemperature && base.IsSwitchedOn) || (this.averageTemp < this.thresholdTemperature && !base.IsSwitchedOn))
		{
			this.Toggle();
		}
	}

	// Token: 0x060053D1 RID: 21457 RVA: 0x000DB00E File Offset: 0x000D920E
	public float GetTemperature()
	{
		return this.averageTemp;
	}

	// Token: 0x170004C7 RID: 1223
	// (get) Token: 0x060053D2 RID: 21458 RVA: 0x000DB016 File Offset: 0x000D9216
	// (set) Token: 0x060053D3 RID: 21459 RVA: 0x000DB01E File Offset: 0x000D921E
	public float Threshold
	{
		get
		{
			return this.thresholdTemperature;
		}
		set
		{
			this.thresholdTemperature = value;
		}
	}

	// Token: 0x170004C8 RID: 1224
	// (get) Token: 0x060053D4 RID: 21460 RVA: 0x000DB027 File Offset: 0x000D9227
	// (set) Token: 0x060053D5 RID: 21461 RVA: 0x000DB02F File Offset: 0x000D922F
	public bool ActivateAboveThreshold
	{
		get
		{
			return this.activateOnWarmerThan;
		}
		set
		{
			this.activateOnWarmerThan = value;
		}
	}

	// Token: 0x170004C9 RID: 1225
	// (get) Token: 0x060053D6 RID: 21462 RVA: 0x000DB038 File Offset: 0x000D9238
	public float CurrentValue
	{
		get
		{
			return this.GetTemperature();
		}
	}

	// Token: 0x170004CA RID: 1226
	// (get) Token: 0x060053D7 RID: 21463 RVA: 0x000DB040 File Offset: 0x000D9240
	public float RangeMin
	{
		get
		{
			return this.minTemp;
		}
	}

	// Token: 0x170004CB RID: 1227
	// (get) Token: 0x060053D8 RID: 21464 RVA: 0x000DB048 File Offset: 0x000D9248
	public float RangeMax
	{
		get
		{
			return this.maxTemp;
		}
	}

	// Token: 0x060053D9 RID: 21465 RVA: 0x000DB050 File Offset: 0x000D9250
	public float GetRangeMinInputField()
	{
		return GameUtil.GetConvertedTemperature(this.RangeMin, false);
	}

	// Token: 0x060053DA RID: 21466 RVA: 0x000DB05E File Offset: 0x000D925E
	public float GetRangeMaxInputField()
	{
		return GameUtil.GetConvertedTemperature(this.RangeMax, false);
	}

	// Token: 0x170004CC RID: 1228
	// (get) Token: 0x060053DB RID: 21467 RVA: 0x000CEFA9 File Offset: 0x000CD1A9
	public LocString Title
	{
		get
		{
			return UI.UISIDESCREENS.TEMPERATURESWITCHSIDESCREEN.TITLE;
		}
	}

	// Token: 0x170004CD RID: 1229
	// (get) Token: 0x060053DC RID: 21468 RVA: 0x000D4C1A File Offset: 0x000D2E1A
	public LocString ThresholdValueName
	{
		get
		{
			return UI.UISIDESCREENS.THRESHOLD_SWITCH_SIDESCREEN.TEMPERATURE;
		}
	}

	// Token: 0x170004CE RID: 1230
	// (get) Token: 0x060053DD RID: 21469 RVA: 0x000D4C21 File Offset: 0x000D2E21
	public string AboveToolTip
	{
		get
		{
			return UI.UISIDESCREENS.THRESHOLD_SWITCH_SIDESCREEN.TEMPERATURE_TOOLTIP_ABOVE;
		}
	}

	// Token: 0x170004CF RID: 1231
	// (get) Token: 0x060053DE RID: 21470 RVA: 0x000D4C2D File Offset: 0x000D2E2D
	public string BelowToolTip
	{
		get
		{
			return UI.UISIDESCREENS.THRESHOLD_SWITCH_SIDESCREEN.TEMPERATURE_TOOLTIP_BELOW;
		}
	}

	// Token: 0x060053DF RID: 21471 RVA: 0x000CEFCF File Offset: 0x000CD1CF
	public string Format(float value, bool units)
	{
		return GameUtil.GetFormattedTemperature(value, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, units, false);
	}

	// Token: 0x060053E0 RID: 21472 RVA: 0x000CEFDB File Offset: 0x000CD1DB
	public float ProcessedSliderValue(float input)
	{
		return Mathf.Round(input);
	}

	// Token: 0x060053E1 RID: 21473 RVA: 0x000CEFE3 File Offset: 0x000CD1E3
	public float ProcessedInputValue(float input)
	{
		return GameUtil.GetTemperatureConvertedToKelvin(input);
	}

	// Token: 0x060053E2 RID: 21474 RVA: 0x0024D0FC File Offset: 0x0024B2FC
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

	// Token: 0x170004D0 RID: 1232
	// (get) Token: 0x060053E3 RID: 21475 RVA: 0x000AA7E7 File Offset: 0x000A89E7
	public ThresholdScreenLayoutType LayoutType
	{
		get
		{
			return ThresholdScreenLayoutType.InputField;
		}
	}

	// Token: 0x170004D1 RID: 1233
	// (get) Token: 0x060053E4 RID: 21476 RVA: 0x000AA7E7 File Offset: 0x000A89E7
	public int IncrementScale
	{
		get
		{
			return 1;
		}
	}

	// Token: 0x170004D2 RID: 1234
	// (get) Token: 0x060053E5 RID: 21477 RVA: 0x000DB06C File Offset: 0x000D926C
	public NonLinearSlider.Range[] GetRanges
	{
		get
		{
			return NonLinearSlider.GetDefaultRange(this.RangeMax);
		}
	}

	// Token: 0x04003B14 RID: 15124
	private HandleVector<int>.Handle structureTemperature;

	// Token: 0x04003B15 RID: 15125
	private int simUpdateCounter;

	// Token: 0x04003B16 RID: 15126
	[Serialize]
	public float thresholdTemperature = 280f;

	// Token: 0x04003B17 RID: 15127
	[Serialize]
	public bool activateOnWarmerThan;

	// Token: 0x04003B18 RID: 15128
	public float minTemp;

	// Token: 0x04003B19 RID: 15129
	public float maxTemp = 373.15f;

	// Token: 0x04003B1A RID: 15130
	private const int NumFrameDelay = 8;

	// Token: 0x04003B1B RID: 15131
	private float[] temperatures = new float[8];

	// Token: 0x04003B1C RID: 15132
	private float averageTemp;
}
