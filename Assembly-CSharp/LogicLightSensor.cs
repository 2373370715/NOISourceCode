using System;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x02000E8F RID: 3727
[SerializationConfig(MemberSerialization.OptIn)]
public class LogicLightSensor : Switch, ISaveLoadable, IThresholdSwitch, ISim200ms
{
	// Token: 0x060049AD RID: 18861 RVA: 0x00268718 File Offset: 0x00266918
	private void OnCopySettings(object data)
	{
		LogicLightSensor component = ((GameObject)data).GetComponent<LogicLightSensor>();
		if (component != null)
		{
			this.Threshold = component.Threshold;
			this.ActivateAboveThreshold = component.ActivateAboveThreshold;
		}
	}

	// Token: 0x060049AE RID: 18862 RVA: 0x000D439A File Offset: 0x000D259A
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.Subscribe<LogicLightSensor>(-905833192, LogicLightSensor.OnCopySettingsDelegate);
	}

	// Token: 0x060049AF RID: 18863 RVA: 0x000D43B3 File Offset: 0x000D25B3
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.OnToggle += this.OnSwitchToggled;
		this.UpdateVisualState(true);
		this.UpdateLogicCircuit();
		this.wasOn = this.switchedOn;
	}

	// Token: 0x060049B0 RID: 18864 RVA: 0x00268754 File Offset: 0x00266954
	public void Sim200ms(float dt)
	{
		if (this.simUpdateCounter < 4)
		{
			this.levels[this.simUpdateCounter] = (float)Grid.LightIntensity[Grid.PosToCell(this)];
			this.simUpdateCounter++;
			return;
		}
		this.simUpdateCounter = 0;
		this.averageBrightness = 0f;
		for (int i = 0; i < 4; i++)
		{
			this.averageBrightness += this.levels[i];
		}
		this.averageBrightness /= 4f;
		if (this.activateOnBrighterThan)
		{
			if ((this.averageBrightness > this.thresholdBrightness && !base.IsSwitchedOn) || (this.averageBrightness < this.thresholdBrightness && base.IsSwitchedOn))
			{
				this.Toggle();
				return;
			}
		}
		else if ((this.averageBrightness > this.thresholdBrightness && base.IsSwitchedOn) || (this.averageBrightness < this.thresholdBrightness && !base.IsSwitchedOn))
		{
			this.Toggle();
		}
	}

	// Token: 0x060049B1 RID: 18865 RVA: 0x000D43E6 File Offset: 0x000D25E6
	private void OnSwitchToggled(bool toggled_on)
	{
		this.UpdateVisualState(false);
		this.UpdateLogicCircuit();
	}

	// Token: 0x060049B2 RID: 18866 RVA: 0x000CEE6A File Offset: 0x000CD06A
	private void UpdateLogicCircuit()
	{
		base.GetComponent<LogicPorts>().SendSignal(LogicSwitch.PORT_ID, this.switchedOn ? 1 : 0);
	}

	// Token: 0x060049B3 RID: 18867 RVA: 0x0026884C File Offset: 0x00266A4C
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

	// Token: 0x170003BA RID: 954
	// (get) Token: 0x060049B4 RID: 18868 RVA: 0x000D43F5 File Offset: 0x000D25F5
	// (set) Token: 0x060049B5 RID: 18869 RVA: 0x000D43FD File Offset: 0x000D25FD
	public float Threshold
	{
		get
		{
			return this.thresholdBrightness;
		}
		set
		{
			this.thresholdBrightness = value;
		}
	}

	// Token: 0x170003BB RID: 955
	// (get) Token: 0x060049B6 RID: 18870 RVA: 0x000D4406 File Offset: 0x000D2606
	// (set) Token: 0x060049B7 RID: 18871 RVA: 0x000D440E File Offset: 0x000D260E
	public bool ActivateAboveThreshold
	{
		get
		{
			return this.activateOnBrighterThan;
		}
		set
		{
			this.activateOnBrighterThan = value;
		}
	}

	// Token: 0x170003BC RID: 956
	// (get) Token: 0x060049B8 RID: 18872 RVA: 0x000D4417 File Offset: 0x000D2617
	public float CurrentValue
	{
		get
		{
			return this.averageBrightness;
		}
	}

	// Token: 0x170003BD RID: 957
	// (get) Token: 0x060049B9 RID: 18873 RVA: 0x000D441F File Offset: 0x000D261F
	public float RangeMin
	{
		get
		{
			return this.minBrightness;
		}
	}

	// Token: 0x170003BE RID: 958
	// (get) Token: 0x060049BA RID: 18874 RVA: 0x000D4427 File Offset: 0x000D2627
	public float RangeMax
	{
		get
		{
			return this.maxBrightness;
		}
	}

	// Token: 0x060049BB RID: 18875 RVA: 0x000D442F File Offset: 0x000D262F
	public float GetRangeMinInputField()
	{
		return this.RangeMin;
	}

	// Token: 0x060049BC RID: 18876 RVA: 0x000D4437 File Offset: 0x000D2637
	public float GetRangeMaxInputField()
	{
		return this.RangeMax;
	}

	// Token: 0x170003BF RID: 959
	// (get) Token: 0x060049BD RID: 18877 RVA: 0x000D443F File Offset: 0x000D263F
	public LocString Title
	{
		get
		{
			return UI.UISIDESCREENS.BRIGHTNESSSWITCHSIDESCREEN.TITLE;
		}
	}

	// Token: 0x170003C0 RID: 960
	// (get) Token: 0x060049BE RID: 18878 RVA: 0x000D4446 File Offset: 0x000D2646
	public LocString ThresholdValueName
	{
		get
		{
			return UI.UISIDESCREENS.THRESHOLD_SWITCH_SIDESCREEN.BRIGHTNESS;
		}
	}

	// Token: 0x170003C1 RID: 961
	// (get) Token: 0x060049BF RID: 18879 RVA: 0x000D444D File Offset: 0x000D264D
	public string AboveToolTip
	{
		get
		{
			return UI.UISIDESCREENS.THRESHOLD_SWITCH_SIDESCREEN.BRIGHTNESS_TOOLTIP_ABOVE;
		}
	}

	// Token: 0x170003C2 RID: 962
	// (get) Token: 0x060049C0 RID: 18880 RVA: 0x000D4459 File Offset: 0x000D2659
	public string BelowToolTip
	{
		get
		{
			return UI.UISIDESCREENS.THRESHOLD_SWITCH_SIDESCREEN.BRIGHTNESS_TOOLTIP_BELOW;
		}
	}

	// Token: 0x060049C1 RID: 18881 RVA: 0x000D4465 File Offset: 0x000D2665
	public string Format(float value, bool units)
	{
		if (units)
		{
			return GameUtil.GetFormattedLux((int)value);
		}
		return string.Format("{0}", (int)value);
	}

	// Token: 0x060049C2 RID: 18882 RVA: 0x000CEFDB File Offset: 0x000CD1DB
	public float ProcessedSliderValue(float input)
	{
		return Mathf.Round(input);
	}

	// Token: 0x060049C3 RID: 18883 RVA: 0x000B64D6 File Offset: 0x000B46D6
	public float ProcessedInputValue(float input)
	{
		return input;
	}

	// Token: 0x060049C4 RID: 18884 RVA: 0x000D4483 File Offset: 0x000D2683
	public LocString ThresholdValueUnits()
	{
		return UI.UNITSUFFIXES.LIGHT.LUX;
	}

	// Token: 0x170003C3 RID: 963
	// (get) Token: 0x060049C5 RID: 18885 RVA: 0x000B1628 File Offset: 0x000AF828
	public ThresholdScreenLayoutType LayoutType
	{
		get
		{
			return ThresholdScreenLayoutType.SliderBar;
		}
	}

	// Token: 0x170003C4 RID: 964
	// (get) Token: 0x060049C6 RID: 18886 RVA: 0x000AA7E7 File Offset: 0x000A89E7
	public int IncrementScale
	{
		get
		{
			return 1;
		}
	}

	// Token: 0x170003C5 RID: 965
	// (get) Token: 0x060049C7 RID: 18887 RVA: 0x000D448A File Offset: 0x000D268A
	public NonLinearSlider.Range[] GetRanges
	{
		get
		{
			return NonLinearSlider.GetDefaultRange(this.RangeMax);
		}
	}

	// Token: 0x060049C8 RID: 18888 RVA: 0x0026473C File Offset: 0x0026293C
	protected override void UpdateSwitchStatus()
	{
		StatusItem status_item = this.switchedOn ? Db.Get().BuildingStatusItems.LogicSensorStatusActive : Db.Get().BuildingStatusItems.LogicSensorStatusInactive;
		base.GetComponent<KSelectable>().SetStatusItem(Db.Get().StatusItemCategories.Power, status_item, null);
	}

	// Token: 0x040033F0 RID: 13296
	private int simUpdateCounter;

	// Token: 0x040033F1 RID: 13297
	[Serialize]
	public float thresholdBrightness = 280f;

	// Token: 0x040033F2 RID: 13298
	[Serialize]
	public bool activateOnBrighterThan = true;

	// Token: 0x040033F3 RID: 13299
	public float minBrightness;

	// Token: 0x040033F4 RID: 13300
	public float maxBrightness = 15000f;

	// Token: 0x040033F5 RID: 13301
	private const int NumFrameDelay = 4;

	// Token: 0x040033F6 RID: 13302
	private float[] levels = new float[4];

	// Token: 0x040033F7 RID: 13303
	private float averageBrightness;

	// Token: 0x040033F8 RID: 13304
	private bool wasOn;

	// Token: 0x040033F9 RID: 13305
	[MyCmpAdd]
	private CopyBuildingSettings copyBuildingSettings;

	// Token: 0x040033FA RID: 13306
	private static readonly EventSystem.IntraObjectHandler<LogicLightSensor> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<LogicLightSensor>(delegate(LogicLightSensor component, object data)
	{
		component.OnCopySettings(data);
	});
}
