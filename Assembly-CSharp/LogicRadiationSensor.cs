using System;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x02000E99 RID: 3737
[SerializationConfig(MemberSerialization.OptIn)]
public class LogicRadiationSensor : Switch, ISaveLoadable, IThresholdSwitch, ISim200ms
{
	// Token: 0x06004A26 RID: 18982 RVA: 0x000D4860 File Offset: 0x000D2A60
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.Subscribe<LogicRadiationSensor>(-905833192, LogicRadiationSensor.OnCopySettingsDelegate);
	}

	// Token: 0x06004A27 RID: 18983 RVA: 0x002692F0 File Offset: 0x002674F0
	private void OnCopySettings(object data)
	{
		LogicRadiationSensor component = ((GameObject)data).GetComponent<LogicRadiationSensor>();
		if (component != null)
		{
			this.Threshold = component.Threshold;
			this.ActivateAboveThreshold = component.ActivateAboveThreshold;
		}
	}

	// Token: 0x06004A28 RID: 18984 RVA: 0x000D4879 File Offset: 0x000D2A79
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.OnToggle += this.OnSwitchToggled;
		this.UpdateVisualState(true);
		this.UpdateLogicCircuit();
		this.wasOn = this.switchedOn;
	}

	// Token: 0x06004A29 RID: 18985 RVA: 0x0026932C File Offset: 0x0026752C
	public void Sim200ms(float dt)
	{
		if (this.simUpdateCounter < 8 && !this.dirty)
		{
			int i = Grid.PosToCell(this);
			this.radHistory[this.simUpdateCounter] = Grid.Radiation[i];
			this.simUpdateCounter++;
			return;
		}
		this.simUpdateCounter = 0;
		this.dirty = false;
		this.averageRads = 0f;
		for (int j = 0; j < 8; j++)
		{
			this.averageRads += this.radHistory[j];
		}
		this.averageRads /= 8f;
		if (this.activateOnWarmerThan)
		{
			if ((this.averageRads > this.thresholdRads && !base.IsSwitchedOn) || (this.averageRads <= this.thresholdRads && base.IsSwitchedOn))
			{
				this.Toggle();
				return;
			}
		}
		else if ((this.averageRads >= this.thresholdRads && base.IsSwitchedOn) || (this.averageRads < this.thresholdRads && !base.IsSwitchedOn))
		{
			this.Toggle();
		}
	}

	// Token: 0x06004A2A RID: 18986 RVA: 0x000D48AC File Offset: 0x000D2AAC
	public float GetAverageRads()
	{
		return this.averageRads;
	}

	// Token: 0x06004A2B RID: 18987 RVA: 0x000D48B4 File Offset: 0x000D2AB4
	private void OnSwitchToggled(bool toggled_on)
	{
		this.UpdateVisualState(false);
		this.UpdateLogicCircuit();
	}

	// Token: 0x06004A2C RID: 18988 RVA: 0x000CEE6A File Offset: 0x000CD06A
	private void UpdateLogicCircuit()
	{
		base.GetComponent<LogicPorts>().SendSignal(LogicSwitch.PORT_ID, this.switchedOn ? 1 : 0);
	}

	// Token: 0x06004A2D RID: 18989 RVA: 0x00269434 File Offset: 0x00267634
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

	// Token: 0x06004A2E RID: 18990 RVA: 0x0026473C File Offset: 0x0026293C
	protected override void UpdateSwitchStatus()
	{
		StatusItem status_item = this.switchedOn ? Db.Get().BuildingStatusItems.LogicSensorStatusActive : Db.Get().BuildingStatusItems.LogicSensorStatusInactive;
		base.GetComponent<KSelectable>().SetStatusItem(Db.Get().StatusItemCategories.Power, status_item, null);
	}

	// Token: 0x170003DE RID: 990
	// (get) Token: 0x06004A2F RID: 18991 RVA: 0x000D48C3 File Offset: 0x000D2AC3
	// (set) Token: 0x06004A30 RID: 18992 RVA: 0x000D48CB File Offset: 0x000D2ACB
	public float Threshold
	{
		get
		{
			return this.thresholdRads;
		}
		set
		{
			this.thresholdRads = value;
			this.dirty = true;
		}
	}

	// Token: 0x170003DF RID: 991
	// (get) Token: 0x06004A31 RID: 18993 RVA: 0x000D48DB File Offset: 0x000D2ADB
	// (set) Token: 0x06004A32 RID: 18994 RVA: 0x000D48E3 File Offset: 0x000D2AE3
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

	// Token: 0x170003E0 RID: 992
	// (get) Token: 0x06004A33 RID: 18995 RVA: 0x000D48F3 File Offset: 0x000D2AF3
	public float CurrentValue
	{
		get
		{
			return this.GetAverageRads();
		}
	}

	// Token: 0x170003E1 RID: 993
	// (get) Token: 0x06004A34 RID: 18996 RVA: 0x000D48FB File Offset: 0x000D2AFB
	public float RangeMin
	{
		get
		{
			return this.minRads;
		}
	}

	// Token: 0x170003E2 RID: 994
	// (get) Token: 0x06004A35 RID: 18997 RVA: 0x000D4903 File Offset: 0x000D2B03
	public float RangeMax
	{
		get
		{
			return this.maxRads;
		}
	}

	// Token: 0x06004A36 RID: 18998 RVA: 0x000D490B File Offset: 0x000D2B0B
	public float GetRangeMinInputField()
	{
		return GameUtil.GetConvertedTemperature(this.RangeMin, false);
	}

	// Token: 0x06004A37 RID: 18999 RVA: 0x000D4919 File Offset: 0x000D2B19
	public float GetRangeMaxInputField()
	{
		return GameUtil.GetConvertedTemperature(this.RangeMax, false);
	}

	// Token: 0x170003E3 RID: 995
	// (get) Token: 0x06004A38 RID: 19000 RVA: 0x000D4927 File Offset: 0x000D2B27
	public LocString Title
	{
		get
		{
			return UI.UISIDESCREENS.RADIATIONSWITCHSIDESCREEN.TITLE;
		}
	}

	// Token: 0x170003E4 RID: 996
	// (get) Token: 0x06004A39 RID: 19001 RVA: 0x000D492E File Offset: 0x000D2B2E
	public LocString ThresholdValueName
	{
		get
		{
			return UI.UISIDESCREENS.THRESHOLD_SWITCH_SIDESCREEN.RADIATION;
		}
	}

	// Token: 0x170003E5 RID: 997
	// (get) Token: 0x06004A3A RID: 19002 RVA: 0x000D4935 File Offset: 0x000D2B35
	public string AboveToolTip
	{
		get
		{
			return UI.UISIDESCREENS.THRESHOLD_SWITCH_SIDESCREEN.RADIATION_TOOLTIP_ABOVE;
		}
	}

	// Token: 0x170003E6 RID: 998
	// (get) Token: 0x06004A3B RID: 19003 RVA: 0x000D4941 File Offset: 0x000D2B41
	public string BelowToolTip
	{
		get
		{
			return UI.UISIDESCREENS.THRESHOLD_SWITCH_SIDESCREEN.RADIATION_TOOLTIP_BELOW;
		}
	}

	// Token: 0x06004A3C RID: 19004 RVA: 0x000C85F0 File Offset: 0x000C67F0
	public string Format(float value, bool units)
	{
		return GameUtil.GetFormattedRads(value, GameUtil.TimeSlice.None);
	}

	// Token: 0x06004A3D RID: 19005 RVA: 0x000CEFDB File Offset: 0x000CD1DB
	public float ProcessedSliderValue(float input)
	{
		return Mathf.Round(input);
	}

	// Token: 0x06004A3E RID: 19006 RVA: 0x000B64D6 File Offset: 0x000B46D6
	public float ProcessedInputValue(float input)
	{
		return input;
	}

	// Token: 0x06004A3F RID: 19007 RVA: 0x000D39DC File Offset: 0x000D1BDC
	public LocString ThresholdValueUnits()
	{
		return "";
	}

	// Token: 0x170003E7 RID: 999
	// (get) Token: 0x06004A40 RID: 19008 RVA: 0x000B1628 File Offset: 0x000AF828
	public ThresholdScreenLayoutType LayoutType
	{
		get
		{
			return ThresholdScreenLayoutType.SliderBar;
		}
	}

	// Token: 0x170003E8 RID: 1000
	// (get) Token: 0x06004A41 RID: 19009 RVA: 0x000AA7E7 File Offset: 0x000A89E7
	public int IncrementScale
	{
		get
		{
			return 1;
		}
	}

	// Token: 0x170003E9 RID: 1001
	// (get) Token: 0x06004A42 RID: 19010 RVA: 0x002694BC File Offset: 0x002676BC
	public NonLinearSlider.Range[] GetRanges
	{
		get
		{
			return new NonLinearSlider.Range[]
			{
				new NonLinearSlider.Range(50f, 200f),
				new NonLinearSlider.Range(25f, 1000f),
				new NonLinearSlider.Range(25f, 5000f)
			};
		}
	}

	// Token: 0x04003429 RID: 13353
	private int simUpdateCounter;

	// Token: 0x0400342A RID: 13354
	[Serialize]
	public float thresholdRads = 280f;

	// Token: 0x0400342B RID: 13355
	[Serialize]
	public bool activateOnWarmerThan;

	// Token: 0x0400342C RID: 13356
	[Serialize]
	private bool dirty = true;

	// Token: 0x0400342D RID: 13357
	public float minRads;

	// Token: 0x0400342E RID: 13358
	public float maxRads = 5000f;

	// Token: 0x0400342F RID: 13359
	private const int NumFrameDelay = 8;

	// Token: 0x04003430 RID: 13360
	private float[] radHistory = new float[8];

	// Token: 0x04003431 RID: 13361
	private float averageRads;

	// Token: 0x04003432 RID: 13362
	private bool wasOn;

	// Token: 0x04003433 RID: 13363
	[MyCmpAdd]
	private CopyBuildingSettings copyBuildingSettings;

	// Token: 0x04003434 RID: 13364
	private static readonly EventSystem.IntraObjectHandler<LogicRadiationSensor> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<LogicRadiationSensor>(delegate(LogicRadiationSensor component, object data)
	{
		component.OnCopySettings(data);
	});
}
