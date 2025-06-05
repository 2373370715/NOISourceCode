using System;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x02000EA3 RID: 3747
[SerializationConfig(MemberSerialization.OptIn)]
public class LogicTemperatureSensor : Switch, ISaveLoadable, IThresholdSwitch, ISim200ms
{
	// Token: 0x170003F2 RID: 1010
	// (get) Token: 0x06004A98 RID: 19096 RVA: 0x0026A0D8 File Offset: 0x002682D8
	public float StructureTemperature
	{
		get
		{
			return GameComps.StructureTemperatures.GetPayload(this.structureTemperature).Temperature;
		}
	}

	// Token: 0x06004A99 RID: 19097 RVA: 0x000D4B86 File Offset: 0x000D2D86
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.Subscribe<LogicTemperatureSensor>(-905833192, LogicTemperatureSensor.OnCopySettingsDelegate);
	}

	// Token: 0x06004A9A RID: 19098 RVA: 0x0026A100 File Offset: 0x00268300
	private void OnCopySettings(object data)
	{
		LogicTemperatureSensor component = ((GameObject)data).GetComponent<LogicTemperatureSensor>();
		if (component != null)
		{
			this.Threshold = component.Threshold;
			this.ActivateAboveThreshold = component.ActivateAboveThreshold;
		}
	}

	// Token: 0x06004A9B RID: 19099 RVA: 0x0026A13C File Offset: 0x0026833C
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.structureTemperature = GameComps.StructureTemperatures.GetHandle(base.gameObject);
		base.OnToggle += this.OnSwitchToggled;
		this.UpdateVisualState(true);
		this.UpdateLogicCircuit();
		this.wasOn = this.switchedOn;
	}

	// Token: 0x06004A9C RID: 19100 RVA: 0x0026A190 File Offset: 0x00268390
	public void Sim200ms(float dt)
	{
		if (this.simUpdateCounter < 8 && !this.dirty)
		{
			int i = Grid.PosToCell(this);
			if (Grid.Mass[i] > 0f)
			{
				this.temperatures[this.simUpdateCounter] = Grid.Temperature[i];
				this.simUpdateCounter++;
			}
			return;
		}
		this.simUpdateCounter = 0;
		this.dirty = false;
		this.averageTemp = 0f;
		for (int j = 0; j < 8; j++)
		{
			this.averageTemp += this.temperatures[j];
		}
		this.averageTemp /= 8f;
		if (this.activateOnWarmerThan)
		{
			if ((this.averageTemp > this.thresholdTemperature && !base.IsSwitchedOn) || (this.averageTemp <= this.thresholdTemperature && base.IsSwitchedOn))
			{
				this.Toggle();
				return;
			}
		}
		else if ((this.averageTemp >= this.thresholdTemperature && base.IsSwitchedOn) || (this.averageTemp < this.thresholdTemperature && !base.IsSwitchedOn))
		{
			this.Toggle();
		}
	}

	// Token: 0x06004A9D RID: 19101 RVA: 0x000D4B9F File Offset: 0x000D2D9F
	public float GetTemperature()
	{
		return this.averageTemp;
	}

	// Token: 0x06004A9E RID: 19102 RVA: 0x000D4BA7 File Offset: 0x000D2DA7
	private void OnSwitchToggled(bool toggled_on)
	{
		this.UpdateVisualState(false);
		this.UpdateLogicCircuit();
	}

	// Token: 0x06004A9F RID: 19103 RVA: 0x000CEE6A File Offset: 0x000CD06A
	private void UpdateLogicCircuit()
	{
		base.GetComponent<LogicPorts>().SendSignal(LogicSwitch.PORT_ID, this.switchedOn ? 1 : 0);
	}

	// Token: 0x06004AA0 RID: 19104 RVA: 0x0026A2A8 File Offset: 0x002684A8
	private void UpdateVisualState(bool force = false)
	{
		if (this.wasOn != this.switchedOn || force)
		{
			this.wasOn = this.switchedOn;
			KBatchedAnimController component = base.GetComponent<KBatchedAnimController>();
			component.Play(this.switchedOn ? "on_pre" : "on_pst", KAnim.PlayMode.Once, 1f, 0f);
			component.Queue(this.switchedOn ? "on" : "off", KAnim.PlayMode.Once, 1f, 0f);
		}
	}

	// Token: 0x06004AA1 RID: 19105 RVA: 0x0026473C File Offset: 0x0026293C
	protected override void UpdateSwitchStatus()
	{
		StatusItem status_item = this.switchedOn ? Db.Get().BuildingStatusItems.LogicSensorStatusActive : Db.Get().BuildingStatusItems.LogicSensorStatusInactive;
		base.GetComponent<KSelectable>().SetStatusItem(Db.Get().StatusItemCategories.Power, status_item, null);
	}

	// Token: 0x170003F3 RID: 1011
	// (get) Token: 0x06004AA2 RID: 19106 RVA: 0x000D4BB6 File Offset: 0x000D2DB6
	// (set) Token: 0x06004AA3 RID: 19107 RVA: 0x000D4BBE File Offset: 0x000D2DBE
	public float Threshold
	{
		get
		{
			return this.thresholdTemperature;
		}
		set
		{
			this.thresholdTemperature = value;
			this.dirty = true;
		}
	}

	// Token: 0x170003F4 RID: 1012
	// (get) Token: 0x06004AA4 RID: 19108 RVA: 0x000D4BCE File Offset: 0x000D2DCE
	// (set) Token: 0x06004AA5 RID: 19109 RVA: 0x000D4BD6 File Offset: 0x000D2DD6
	public bool ActivateAboveThreshold
	{
		get
		{
			return this.activateOnWarmerThan;
		}
		set
		{
			this.activateOnWarmerThan = value;
			this.dirty = true;
		}
	}

	// Token: 0x170003F5 RID: 1013
	// (get) Token: 0x06004AA6 RID: 19110 RVA: 0x000D4BE6 File Offset: 0x000D2DE6
	public float CurrentValue
	{
		get
		{
			return this.GetTemperature();
		}
	}

	// Token: 0x170003F6 RID: 1014
	// (get) Token: 0x06004AA7 RID: 19111 RVA: 0x000D4BEE File Offset: 0x000D2DEE
	public float RangeMin
	{
		get
		{
			return this.minTemp;
		}
	}

	// Token: 0x170003F7 RID: 1015
	// (get) Token: 0x06004AA8 RID: 19112 RVA: 0x000D4BF6 File Offset: 0x000D2DF6
	public float RangeMax
	{
		get
		{
			return this.maxTemp;
		}
	}

	// Token: 0x06004AA9 RID: 19113 RVA: 0x000D4BFE File Offset: 0x000D2DFE
	public float GetRangeMinInputField()
	{
		return GameUtil.GetConvertedTemperature(this.RangeMin, false);
	}

	// Token: 0x06004AAA RID: 19114 RVA: 0x000D4C0C File Offset: 0x000D2E0C
	public float GetRangeMaxInputField()
	{
		return GameUtil.GetConvertedTemperature(this.RangeMax, false);
	}

	// Token: 0x170003F8 RID: 1016
	// (get) Token: 0x06004AAB RID: 19115 RVA: 0x000CEFA9 File Offset: 0x000CD1A9
	public LocString Title
	{
		get
		{
			return UI.UISIDESCREENS.TEMPERATURESWITCHSIDESCREEN.TITLE;
		}
	}

	// Token: 0x170003F9 RID: 1017
	// (get) Token: 0x06004AAC RID: 19116 RVA: 0x000D4C1A File Offset: 0x000D2E1A
	public LocString ThresholdValueName
	{
		get
		{
			return UI.UISIDESCREENS.THRESHOLD_SWITCH_SIDESCREEN.TEMPERATURE;
		}
	}

	// Token: 0x170003FA RID: 1018
	// (get) Token: 0x06004AAD RID: 19117 RVA: 0x000D4C21 File Offset: 0x000D2E21
	public string AboveToolTip
	{
		get
		{
			return UI.UISIDESCREENS.THRESHOLD_SWITCH_SIDESCREEN.TEMPERATURE_TOOLTIP_ABOVE;
		}
	}

	// Token: 0x170003FB RID: 1019
	// (get) Token: 0x06004AAE RID: 19118 RVA: 0x000D4C2D File Offset: 0x000D2E2D
	public string BelowToolTip
	{
		get
		{
			return UI.UISIDESCREENS.THRESHOLD_SWITCH_SIDESCREEN.TEMPERATURE_TOOLTIP_BELOW;
		}
	}

	// Token: 0x06004AAF RID: 19119 RVA: 0x000D4C39 File Offset: 0x000D2E39
	public string Format(float value, bool units)
	{
		return GameUtil.GetFormattedTemperature(value, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, units, true);
	}

	// Token: 0x06004AB0 RID: 19120 RVA: 0x000CEFDB File Offset: 0x000CD1DB
	public float ProcessedSliderValue(float input)
	{
		return Mathf.Round(input);
	}

	// Token: 0x06004AB1 RID: 19121 RVA: 0x000CEFE3 File Offset: 0x000CD1E3
	public float ProcessedInputValue(float input)
	{
		return GameUtil.GetTemperatureConvertedToKelvin(input);
	}

	// Token: 0x06004AB2 RID: 19122 RVA: 0x0024D0FC File Offset: 0x0024B2FC
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

	// Token: 0x170003FC RID: 1020
	// (get) Token: 0x06004AB3 RID: 19123 RVA: 0x000B1628 File Offset: 0x000AF828
	public ThresholdScreenLayoutType LayoutType
	{
		get
		{
			return ThresholdScreenLayoutType.SliderBar;
		}
	}

	// Token: 0x170003FD RID: 1021
	// (get) Token: 0x06004AB4 RID: 19124 RVA: 0x000AA7E7 File Offset: 0x000A89E7
	public int IncrementScale
	{
		get
		{
			return 1;
		}
	}

	// Token: 0x170003FE RID: 1022
	// (get) Token: 0x06004AB5 RID: 19125 RVA: 0x0024D13C File Offset: 0x0024B33C
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

	// Token: 0x04003464 RID: 13412
	private HandleVector<int>.Handle structureTemperature;

	// Token: 0x04003465 RID: 13413
	private int simUpdateCounter;

	// Token: 0x04003466 RID: 13414
	[Serialize]
	public float thresholdTemperature = 280f;

	// Token: 0x04003467 RID: 13415
	[Serialize]
	public bool activateOnWarmerThan;

	// Token: 0x04003468 RID: 13416
	[Serialize]
	private bool dirty = true;

	// Token: 0x04003469 RID: 13417
	public float minTemp;

	// Token: 0x0400346A RID: 13418
	public float maxTemp = 373.15f;

	// Token: 0x0400346B RID: 13419
	private const int NumFrameDelay = 8;

	// Token: 0x0400346C RID: 13420
	private float[] temperatures = new float[8];

	// Token: 0x0400346D RID: 13421
	private float averageTemp;

	// Token: 0x0400346E RID: 13422
	private bool wasOn;

	// Token: 0x0400346F RID: 13423
	[MyCmpAdd]
	private CopyBuildingSettings copyBuildingSettings;

	// Token: 0x04003470 RID: 13424
	private static readonly EventSystem.IntraObjectHandler<LogicTemperatureSensor> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<LogicTemperatureSensor>(delegate(LogicTemperatureSensor component, object data)
	{
		component.OnCopySettings(data);
	});
}
