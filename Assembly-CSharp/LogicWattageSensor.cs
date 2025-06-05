using System;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x02000EA9 RID: 3753
[SerializationConfig(MemberSerialization.OptIn)]
public class LogicWattageSensor : Switch, ISaveLoadable, IThresholdSwitch, ISim200ms
{
	// Token: 0x06004AD6 RID: 19158 RVA: 0x000D4E0B File Offset: 0x000D300B
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.Subscribe<LogicWattageSensor>(-905833192, LogicWattageSensor.OnCopySettingsDelegate);
	}

	// Token: 0x06004AD7 RID: 19159 RVA: 0x0026A5B4 File Offset: 0x002687B4
	private void OnCopySettings(object data)
	{
		LogicWattageSensor component = ((GameObject)data).GetComponent<LogicWattageSensor>();
		if (component != null)
		{
			this.Threshold = component.Threshold;
			this.ActivateAboveThreshold = component.ActivateAboveThreshold;
		}
	}

	// Token: 0x06004AD8 RID: 19160 RVA: 0x000D4E24 File Offset: 0x000D3024
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.OnToggle += this.OnSwitchToggled;
		this.UpdateVisualState(true);
		this.UpdateLogicCircuit();
		this.wasOn = this.switchedOn;
	}

	// Token: 0x06004AD9 RID: 19161 RVA: 0x0026A5F0 File Offset: 0x002687F0
	public void Sim200ms(float dt)
	{
		float wattsUsedByCircuit = Game.Instance.circuitManager.GetWattsUsedByCircuit(Game.Instance.circuitManager.GetCircuitID(Grid.PosToCell(this)));
		if (wattsUsedByCircuit < 0f)
		{
			return;
		}
		this.currentWattage = wattsUsedByCircuit;
		if (this.activateOnHigherThan)
		{
			if ((this.currentWattage > this.thresholdWattage && !base.IsSwitchedOn) || (this.currentWattage <= this.thresholdWattage && base.IsSwitchedOn))
			{
				this.Toggle();
				return;
			}
		}
		else if ((this.currentWattage >= this.thresholdWattage && base.IsSwitchedOn) || (this.currentWattage < this.thresholdWattage && !base.IsSwitchedOn))
		{
			this.Toggle();
		}
	}

	// Token: 0x06004ADA RID: 19162 RVA: 0x000D4E57 File Offset: 0x000D3057
	public float GetWattageUsed()
	{
		return this.currentWattage;
	}

	// Token: 0x06004ADB RID: 19163 RVA: 0x000D4E5F File Offset: 0x000D305F
	private void OnSwitchToggled(bool toggled_on)
	{
		this.UpdateVisualState(false);
		this.UpdateLogicCircuit();
	}

	// Token: 0x06004ADC RID: 19164 RVA: 0x000CEE6A File Offset: 0x000CD06A
	private void UpdateLogicCircuit()
	{
		base.GetComponent<LogicPorts>().SendSignal(LogicSwitch.PORT_ID, this.switchedOn ? 1 : 0);
	}

	// Token: 0x06004ADD RID: 19165 RVA: 0x0026A6A0 File Offset: 0x002688A0
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

	// Token: 0x06004ADE RID: 19166 RVA: 0x0026473C File Offset: 0x0026293C
	protected override void UpdateSwitchStatus()
	{
		StatusItem status_item = this.switchedOn ? Db.Get().BuildingStatusItems.LogicSensorStatusActive : Db.Get().BuildingStatusItems.LogicSensorStatusInactive;
		base.GetComponent<KSelectable>().SetStatusItem(Db.Get().StatusItemCategories.Power, status_item, null);
	}

	// Token: 0x170003FF RID: 1023
	// (get) Token: 0x06004ADF RID: 19167 RVA: 0x000D4E6E File Offset: 0x000D306E
	// (set) Token: 0x06004AE0 RID: 19168 RVA: 0x000D4E76 File Offset: 0x000D3076
	public float Threshold
	{
		get
		{
			return this.thresholdWattage;
		}
		set
		{
			this.thresholdWattage = value;
			this.dirty = true;
		}
	}

	// Token: 0x17000400 RID: 1024
	// (get) Token: 0x06004AE1 RID: 19169 RVA: 0x000D4E86 File Offset: 0x000D3086
	// (set) Token: 0x06004AE2 RID: 19170 RVA: 0x000D4E8E File Offset: 0x000D308E
	public bool ActivateAboveThreshold
	{
		get
		{
			return this.activateOnHigherThan;
		}
		set
		{
			this.activateOnHigherThan = value;
			this.dirty = true;
		}
	}

	// Token: 0x17000401 RID: 1025
	// (get) Token: 0x06004AE3 RID: 19171 RVA: 0x000D4E9E File Offset: 0x000D309E
	public float CurrentValue
	{
		get
		{
			return this.GetWattageUsed();
		}
	}

	// Token: 0x17000402 RID: 1026
	// (get) Token: 0x06004AE4 RID: 19172 RVA: 0x000D4EA6 File Offset: 0x000D30A6
	public float RangeMin
	{
		get
		{
			return this.minWattage;
		}
	}

	// Token: 0x17000403 RID: 1027
	// (get) Token: 0x06004AE5 RID: 19173 RVA: 0x000D4EAE File Offset: 0x000D30AE
	public float RangeMax
	{
		get
		{
			return this.maxWattage;
		}
	}

	// Token: 0x06004AE6 RID: 19174 RVA: 0x000D4EA6 File Offset: 0x000D30A6
	public float GetRangeMinInputField()
	{
		return this.minWattage;
	}

	// Token: 0x06004AE7 RID: 19175 RVA: 0x000D4EAE File Offset: 0x000D30AE
	public float GetRangeMaxInputField()
	{
		return this.maxWattage;
	}

	// Token: 0x17000404 RID: 1028
	// (get) Token: 0x06004AE8 RID: 19176 RVA: 0x000D4EB6 File Offset: 0x000D30B6
	public LocString Title
	{
		get
		{
			return UI.UISIDESCREENS.WATTAGESWITCHSIDESCREEN.TITLE;
		}
	}

	// Token: 0x17000405 RID: 1029
	// (get) Token: 0x06004AE9 RID: 19177 RVA: 0x000D4EBD File Offset: 0x000D30BD
	public LocString ThresholdValueName
	{
		get
		{
			return UI.UISIDESCREENS.THRESHOLD_SWITCH_SIDESCREEN.WATTAGE;
		}
	}

	// Token: 0x17000406 RID: 1030
	// (get) Token: 0x06004AEA RID: 19178 RVA: 0x000D4EC4 File Offset: 0x000D30C4
	public string AboveToolTip
	{
		get
		{
			return UI.UISIDESCREENS.THRESHOLD_SWITCH_SIDESCREEN.WATTAGE_TOOLTIP_ABOVE;
		}
	}

	// Token: 0x17000407 RID: 1031
	// (get) Token: 0x06004AEB RID: 19179 RVA: 0x000D4ED0 File Offset: 0x000D30D0
	public string BelowToolTip
	{
		get
		{
			return UI.UISIDESCREENS.THRESHOLD_SWITCH_SIDESCREEN.WATTAGE_TOOLTIP_BELOW;
		}
	}

	// Token: 0x06004AEC RID: 19180 RVA: 0x000D4EDC File Offset: 0x000D30DC
	public string Format(float value, bool units)
	{
		return GameUtil.GetFormattedWattage(value, GameUtil.WattageFormatterUnit.Watts, units);
	}

	// Token: 0x06004AED RID: 19181 RVA: 0x000CEFDB File Offset: 0x000CD1DB
	public float ProcessedSliderValue(float input)
	{
		return Mathf.Round(input);
	}

	// Token: 0x06004AEE RID: 19182 RVA: 0x000B64D6 File Offset: 0x000B46D6
	public float ProcessedInputValue(float input)
	{
		return input;
	}

	// Token: 0x06004AEF RID: 19183 RVA: 0x000D4EE6 File Offset: 0x000D30E6
	public LocString ThresholdValueUnits()
	{
		return UI.UNITSUFFIXES.ELECTRICAL.WATT;
	}

	// Token: 0x17000408 RID: 1032
	// (get) Token: 0x06004AF0 RID: 19184 RVA: 0x000B1628 File Offset: 0x000AF828
	public ThresholdScreenLayoutType LayoutType
	{
		get
		{
			return ThresholdScreenLayoutType.SliderBar;
		}
	}

	// Token: 0x17000409 RID: 1033
	// (get) Token: 0x06004AF1 RID: 19185 RVA: 0x000AA7E7 File Offset: 0x000A89E7
	public int IncrementScale
	{
		get
		{
			return 1;
		}
	}

	// Token: 0x1700040A RID: 1034
	// (get) Token: 0x06004AF2 RID: 19186 RVA: 0x0026A728 File Offset: 0x00268928
	public NonLinearSlider.Range[] GetRanges
	{
		get
		{
			return new NonLinearSlider.Range[]
			{
				new NonLinearSlider.Range(5f, 5f),
				new NonLinearSlider.Range(35f, 1000f),
				new NonLinearSlider.Range(50f, 3000f),
				new NonLinearSlider.Range(10f, this.maxWattage)
			};
		}
	}

	// Token: 0x04003480 RID: 13440
	[Serialize]
	public float thresholdWattage;

	// Token: 0x04003481 RID: 13441
	[Serialize]
	public bool activateOnHigherThan;

	// Token: 0x04003482 RID: 13442
	[Serialize]
	public bool dirty = true;

	// Token: 0x04003483 RID: 13443
	private readonly float minWattage;

	// Token: 0x04003484 RID: 13444
	private readonly float maxWattage = 1.5f * Wire.GetMaxWattageAsFloat(Wire.WattageRating.Max50000);

	// Token: 0x04003485 RID: 13445
	private float currentWattage;

	// Token: 0x04003486 RID: 13446
	private bool wasOn;

	// Token: 0x04003487 RID: 13447
	[MyCmpAdd]
	private CopyBuildingSettings copyBuildingSettings;

	// Token: 0x04003488 RID: 13448
	private static readonly EventSystem.IntraObjectHandler<LogicWattageSensor> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<LogicWattageSensor>(delegate(LogicWattageSensor component, object data)
	{
		component.OnCopySettings(data);
	});
}
