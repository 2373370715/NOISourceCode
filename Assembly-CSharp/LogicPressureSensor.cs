using System;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x02000E97 RID: 3735
[SerializationConfig(MemberSerialization.OptIn)]
public class LogicPressureSensor : Switch, ISaveLoadable, IThresholdSwitch, ISim200ms
{
	// Token: 0x06004A05 RID: 18949 RVA: 0x000D46DD File Offset: 0x000D28DD
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.Subscribe<LogicPressureSensor>(-905833192, LogicPressureSensor.OnCopySettingsDelegate);
	}

	// Token: 0x06004A06 RID: 18950 RVA: 0x00269104 File Offset: 0x00267304
	private void OnCopySettings(object data)
	{
		LogicPressureSensor component = ((GameObject)data).GetComponent<LogicPressureSensor>();
		if (component != null)
		{
			this.Threshold = component.Threshold;
			this.ActivateAboveThreshold = component.ActivateAboveThreshold;
		}
	}

	// Token: 0x06004A07 RID: 18951 RVA: 0x000D46F6 File Offset: 0x000D28F6
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.OnToggle += this.OnSwitchToggled;
		this.UpdateLogicCircuit();
		this.UpdateVisualState(true);
		this.wasOn = this.switchedOn;
	}

	// Token: 0x06004A08 RID: 18952 RVA: 0x00269140 File Offset: 0x00267340
	public void Sim200ms(float dt)
	{
		int num = Grid.PosToCell(this);
		if (this.sampleIdx < 8)
		{
			float num2 = Grid.Element[num].IsState(this.desiredState) ? Grid.Mass[num] : 0f;
			this.samples[this.sampleIdx] = num2;
			this.sampleIdx++;
			return;
		}
		this.sampleIdx = 0;
		float currentValue = this.CurrentValue;
		if (this.activateAboveThreshold)
		{
			if ((currentValue > this.threshold && !base.IsSwitchedOn) || (currentValue <= this.threshold && base.IsSwitchedOn))
			{
				this.Toggle();
				return;
			}
		}
		else if ((currentValue > this.threshold && base.IsSwitchedOn) || (currentValue <= this.threshold && !base.IsSwitchedOn))
		{
			this.Toggle();
		}
	}

	// Token: 0x06004A09 RID: 18953 RVA: 0x000D4729 File Offset: 0x000D2929
	private void OnSwitchToggled(bool toggled_on)
	{
		this.UpdateLogicCircuit();
		this.UpdateVisualState(false);
	}

	// Token: 0x170003D2 RID: 978
	// (get) Token: 0x06004A0A RID: 18954 RVA: 0x000D4738 File Offset: 0x000D2938
	// (set) Token: 0x06004A0B RID: 18955 RVA: 0x000D4740 File Offset: 0x000D2940
	public float Threshold
	{
		get
		{
			return this.threshold;
		}
		set
		{
			this.threshold = value;
		}
	}

	// Token: 0x170003D3 RID: 979
	// (get) Token: 0x06004A0C RID: 18956 RVA: 0x000D4749 File Offset: 0x000D2949
	// (set) Token: 0x06004A0D RID: 18957 RVA: 0x000D4751 File Offset: 0x000D2951
	public bool ActivateAboveThreshold
	{
		get
		{
			return this.activateAboveThreshold;
		}
		set
		{
			this.activateAboveThreshold = value;
		}
	}

	// Token: 0x170003D4 RID: 980
	// (get) Token: 0x06004A0E RID: 18958 RVA: 0x00269208 File Offset: 0x00267408
	public float CurrentValue
	{
		get
		{
			float num = 0f;
			for (int i = 0; i < 8; i++)
			{
				num += this.samples[i];
			}
			return num / 8f;
		}
	}

	// Token: 0x170003D5 RID: 981
	// (get) Token: 0x06004A0F RID: 18959 RVA: 0x000D475A File Offset: 0x000D295A
	public float RangeMin
	{
		get
		{
			return this.rangeMin;
		}
	}

	// Token: 0x170003D6 RID: 982
	// (get) Token: 0x06004A10 RID: 18960 RVA: 0x000D4762 File Offset: 0x000D2962
	public float RangeMax
	{
		get
		{
			return this.rangeMax;
		}
	}

	// Token: 0x06004A11 RID: 18961 RVA: 0x000D476A File Offset: 0x000D296A
	public float GetRangeMinInputField()
	{
		if (this.desiredState != Element.State.Gas)
		{
			return this.rangeMin;
		}
		return this.rangeMin * 1000f;
	}

	// Token: 0x06004A12 RID: 18962 RVA: 0x000D4788 File Offset: 0x000D2988
	public float GetRangeMaxInputField()
	{
		if (this.desiredState != Element.State.Gas)
		{
			return this.rangeMax;
		}
		return this.rangeMax * 1000f;
	}

	// Token: 0x170003D7 RID: 983
	// (get) Token: 0x06004A13 RID: 18963 RVA: 0x000D4599 File Offset: 0x000D2799
	public LocString ThresholdValueName
	{
		get
		{
			return UI.UISIDESCREENS.THRESHOLD_SWITCH_SIDESCREEN.PRESSURE;
		}
	}

	// Token: 0x170003D8 RID: 984
	// (get) Token: 0x06004A14 RID: 18964 RVA: 0x000D45A0 File Offset: 0x000D27A0
	public string AboveToolTip
	{
		get
		{
			return UI.UISIDESCREENS.THRESHOLD_SWITCH_SIDESCREEN.PRESSURE_TOOLTIP_ABOVE;
		}
	}

	// Token: 0x170003D9 RID: 985
	// (get) Token: 0x06004A15 RID: 18965 RVA: 0x000D45AC File Offset: 0x000D27AC
	public string BelowToolTip
	{
		get
		{
			return UI.UISIDESCREENS.THRESHOLD_SWITCH_SIDESCREEN.PRESSURE_TOOLTIP_BELOW;
		}
	}

	// Token: 0x06004A16 RID: 18966 RVA: 0x0026923C File Offset: 0x0026743C
	public string Format(float value, bool units)
	{
		GameUtil.MetricMassFormat massFormat;
		if (this.desiredState == Element.State.Gas)
		{
			massFormat = GameUtil.MetricMassFormat.Gram;
		}
		else
		{
			massFormat = GameUtil.MetricMassFormat.Kilogram;
		}
		return GameUtil.GetFormattedMass(value, GameUtil.TimeSlice.None, massFormat, units, "{0:0.#}");
	}

	// Token: 0x06004A17 RID: 18967 RVA: 0x000D47A6 File Offset: 0x000D29A6
	public float ProcessedSliderValue(float input)
	{
		if (this.desiredState == Element.State.Gas)
		{
			input = Mathf.Round(input * 1000f) / 1000f;
		}
		else
		{
			input = Mathf.Round(input);
		}
		return input;
	}

	// Token: 0x06004A18 RID: 18968 RVA: 0x000D47D0 File Offset: 0x000D29D0
	public float ProcessedInputValue(float input)
	{
		if (this.desiredState == Element.State.Gas)
		{
			input /= 1000f;
		}
		return input;
	}

	// Token: 0x06004A19 RID: 18969 RVA: 0x000D47E5 File Offset: 0x000D29E5
	public LocString ThresholdValueUnits()
	{
		return GameUtil.GetCurrentMassUnit(this.desiredState == Element.State.Gas);
	}

	// Token: 0x170003DA RID: 986
	// (get) Token: 0x06004A1A RID: 18970 RVA: 0x000D454A File Offset: 0x000D274A
	public LocString Title
	{
		get
		{
			return UI.UISIDESCREENS.THRESHOLD_SWITCH_SIDESCREEN.TITLE;
		}
	}

	// Token: 0x170003DB RID: 987
	// (get) Token: 0x06004A1B RID: 18971 RVA: 0x000B1628 File Offset: 0x000AF828
	public ThresholdScreenLayoutType LayoutType
	{
		get
		{
			return ThresholdScreenLayoutType.SliderBar;
		}
	}

	// Token: 0x170003DC RID: 988
	// (get) Token: 0x06004A1C RID: 18972 RVA: 0x000AA7E7 File Offset: 0x000A89E7
	public int IncrementScale
	{
		get
		{
			return 1;
		}
	}

	// Token: 0x170003DD RID: 989
	// (get) Token: 0x06004A1D RID: 18973 RVA: 0x000D47F5 File Offset: 0x000D29F5
	public NonLinearSlider.Range[] GetRanges
	{
		get
		{
			return NonLinearSlider.GetDefaultRange(this.RangeMax);
		}
	}

	// Token: 0x06004A1E RID: 18974 RVA: 0x000CEE6A File Offset: 0x000CD06A
	private void UpdateLogicCircuit()
	{
		base.GetComponent<LogicPorts>().SendSignal(LogicSwitch.PORT_ID, this.switchedOn ? 1 : 0);
	}

	// Token: 0x06004A1F RID: 18975 RVA: 0x00269268 File Offset: 0x00267468
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

	// Token: 0x06004A20 RID: 18976 RVA: 0x0026473C File Offset: 0x0026293C
	protected override void UpdateSwitchStatus()
	{
		StatusItem status_item = this.switchedOn ? Db.Get().BuildingStatusItems.LogicSensorStatusActive : Db.Get().BuildingStatusItems.LogicSensorStatusInactive;
		base.GetComponent<KSelectable>().SetStatusItem(Db.Get().StatusItemCategories.Power, status_item, null);
	}

	// Token: 0x0400341D RID: 13341
	[SerializeField]
	[Serialize]
	private float threshold;

	// Token: 0x0400341E RID: 13342
	[SerializeField]
	[Serialize]
	private bool activateAboveThreshold = true;

	// Token: 0x0400341F RID: 13343
	private bool wasOn;

	// Token: 0x04003420 RID: 13344
	public float rangeMin;

	// Token: 0x04003421 RID: 13345
	public float rangeMax = 1f;

	// Token: 0x04003422 RID: 13346
	public Element.State desiredState = Element.State.Gas;

	// Token: 0x04003423 RID: 13347
	private const int WINDOW_SIZE = 8;

	// Token: 0x04003424 RID: 13348
	private float[] samples = new float[8];

	// Token: 0x04003425 RID: 13349
	private int sampleIdx;

	// Token: 0x04003426 RID: 13350
	[MyCmpAdd]
	private CopyBuildingSettings copyBuildingSettings;

	// Token: 0x04003427 RID: 13351
	private static readonly EventSystem.IntraObjectHandler<LogicPressureSensor> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<LogicPressureSensor>(delegate(LogicPressureSensor component, object data)
	{
		component.OnCopySettings(data);
	});
}
