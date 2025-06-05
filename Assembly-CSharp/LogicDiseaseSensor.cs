using System;
using Klei.AI;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x02000E77 RID: 3703
[SerializationConfig(MemberSerialization.OptIn)]
public class LogicDiseaseSensor : Switch, ISaveLoadable, IThresholdSwitch, ISim200ms
{
	// Token: 0x060048BD RID: 18621 RVA: 0x000D3A43 File Offset: 0x000D1C43
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.Subscribe<LogicDiseaseSensor>(-905833192, LogicDiseaseSensor.OnCopySettingsDelegate);
	}

	// Token: 0x060048BE RID: 18622 RVA: 0x00264DD8 File Offset: 0x00262FD8
	private void OnCopySettings(object data)
	{
		LogicDiseaseSensor component = ((GameObject)data).GetComponent<LogicDiseaseSensor>();
		if (component != null)
		{
			this.Threshold = component.Threshold;
			this.ActivateAboveThreshold = component.ActivateAboveThreshold;
		}
	}

	// Token: 0x060048BF RID: 18623 RVA: 0x000D3A5C File Offset: 0x000D1C5C
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.animController = base.GetComponent<KBatchedAnimController>();
		base.OnToggle += this.OnSwitchToggled;
		this.UpdateLogicCircuit();
		this.UpdateVisualState(true);
		this.wasOn = this.switchedOn;
	}

	// Token: 0x060048C0 RID: 18624 RVA: 0x00264E14 File Offset: 0x00263014
	public void Sim200ms(float dt)
	{
		if (this.sampleIdx < 8)
		{
			int i = Grid.PosToCell(this);
			if (Grid.Mass[i] > 0f)
			{
				this.samples[this.sampleIdx] = Grid.DiseaseCount[i];
				this.sampleIdx++;
			}
			return;
		}
		this.sampleIdx = 0;
		float currentValue = this.CurrentValue;
		if (this.activateAboveThreshold)
		{
			if ((currentValue > this.threshold && !base.IsSwitchedOn) || (currentValue <= this.threshold && base.IsSwitchedOn))
			{
				this.Toggle();
			}
		}
		else if ((currentValue > this.threshold && base.IsSwitchedOn) || (currentValue <= this.threshold && !base.IsSwitchedOn))
		{
			this.Toggle();
		}
		this.animController.SetSymbolVisiblity(LogicDiseaseSensor.TINT_SYMBOL, currentValue > 0f);
	}

	// Token: 0x060048C1 RID: 18625 RVA: 0x000D3A9B File Offset: 0x000D1C9B
	private void OnSwitchToggled(bool toggled_on)
	{
		this.UpdateLogicCircuit();
		this.UpdateVisualState(false);
	}

	// Token: 0x1700038C RID: 908
	// (get) Token: 0x060048C2 RID: 18626 RVA: 0x000D3AAA File Offset: 0x000D1CAA
	// (set) Token: 0x060048C3 RID: 18627 RVA: 0x000D3AB2 File Offset: 0x000D1CB2
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

	// Token: 0x1700038D RID: 909
	// (get) Token: 0x060048C4 RID: 18628 RVA: 0x000D3ABB File Offset: 0x000D1CBB
	// (set) Token: 0x060048C5 RID: 18629 RVA: 0x000D3AC3 File Offset: 0x000D1CC3
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

	// Token: 0x1700038E RID: 910
	// (get) Token: 0x060048C6 RID: 18630 RVA: 0x00264EF0 File Offset: 0x002630F0
	public float CurrentValue
	{
		get
		{
			float num = 0f;
			for (int i = 0; i < 8; i++)
			{
				num += (float)this.samples[i];
			}
			return num / 8f;
		}
	}

	// Token: 0x1700038F RID: 911
	// (get) Token: 0x060048C7 RID: 18631 RVA: 0x000C18F8 File Offset: 0x000BFAF8
	public float RangeMin
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000390 RID: 912
	// (get) Token: 0x060048C8 RID: 18632 RVA: 0x000CEF18 File Offset: 0x000CD118
	public float RangeMax
	{
		get
		{
			return 100000f;
		}
	}

	// Token: 0x060048C9 RID: 18633 RVA: 0x000C18F8 File Offset: 0x000BFAF8
	public float GetRangeMinInputField()
	{
		return 0f;
	}

	// Token: 0x060048CA RID: 18634 RVA: 0x000CEF18 File Offset: 0x000CD118
	public float GetRangeMaxInputField()
	{
		return 100000f;
	}

	// Token: 0x17000391 RID: 913
	// (get) Token: 0x060048CB RID: 18635 RVA: 0x000D3ACC File Offset: 0x000D1CCC
	public LocString ThresholdValueName
	{
		get
		{
			return UI.UISIDESCREENS.THRESHOLD_SWITCH_SIDESCREEN.DISEASE;
		}
	}

	// Token: 0x17000392 RID: 914
	// (get) Token: 0x060048CC RID: 18636 RVA: 0x000CEF2D File Offset: 0x000CD12D
	public string AboveToolTip
	{
		get
		{
			return UI.UISIDESCREENS.THRESHOLD_SWITCH_SIDESCREEN.DISEASE_TOOLTIP_ABOVE;
		}
	}

	// Token: 0x17000393 RID: 915
	// (get) Token: 0x060048CD RID: 18637 RVA: 0x000CEF39 File Offset: 0x000CD139
	public string BelowToolTip
	{
		get
		{
			return UI.UISIDESCREENS.THRESHOLD_SWITCH_SIDESCREEN.DISEASE_TOOLTIP_BELOW;
		}
	}

	// Token: 0x060048CE RID: 18638 RVA: 0x000CEF45 File Offset: 0x000CD145
	public string Format(float value, bool units)
	{
		return GameUtil.GetFormattedInt((float)((int)value), GameUtil.TimeSlice.None);
	}

	// Token: 0x060048CF RID: 18639 RVA: 0x000B64D6 File Offset: 0x000B46D6
	public float ProcessedSliderValue(float input)
	{
		return input;
	}

	// Token: 0x060048D0 RID: 18640 RVA: 0x000B64D6 File Offset: 0x000B46D6
	public float ProcessedInputValue(float input)
	{
		return input;
	}

	// Token: 0x060048D1 RID: 18641 RVA: 0x000CEF50 File Offset: 0x000CD150
	public LocString ThresholdValueUnits()
	{
		return UI.UISIDESCREENS.THRESHOLD_SWITCH_SIDESCREEN.DISEASE_UNITS;
	}

	// Token: 0x17000394 RID: 916
	// (get) Token: 0x060048D2 RID: 18642 RVA: 0x000B1628 File Offset: 0x000AF828
	public ThresholdScreenLayoutType LayoutType
	{
		get
		{
			return ThresholdScreenLayoutType.SliderBar;
		}
	}

	// Token: 0x17000395 RID: 917
	// (get) Token: 0x060048D3 RID: 18643 RVA: 0x000D3AD3 File Offset: 0x000D1CD3
	public int IncrementScale
	{
		get
		{
			return 100;
		}
	}

	// Token: 0x17000396 RID: 918
	// (get) Token: 0x060048D4 RID: 18644 RVA: 0x000D3AD7 File Offset: 0x000D1CD7
	public NonLinearSlider.Range[] GetRanges
	{
		get
		{
			return NonLinearSlider.GetDefaultRange(this.RangeMax);
		}
	}

	// Token: 0x060048D5 RID: 18645 RVA: 0x000CEE6A File Offset: 0x000CD06A
	private void UpdateLogicCircuit()
	{
		base.GetComponent<LogicPorts>().SendSignal(LogicSwitch.PORT_ID, this.switchedOn ? 1 : 0);
	}

	// Token: 0x060048D6 RID: 18646 RVA: 0x00264F24 File Offset: 0x00263124
	private void UpdateVisualState(bool force = false)
	{
		if (this.wasOn != this.switchedOn || force)
		{
			this.wasOn = this.switchedOn;
			if (this.switchedOn)
			{
				this.animController.Play(LogicDiseaseSensor.ON_ANIMS, KAnim.PlayMode.Loop);
				int i = Grid.PosToCell(this);
				byte b = Grid.DiseaseIdx[i];
				Color32 c = Color.white;
				if (b != 255)
				{
					Disease disease = Db.Get().Diseases[(int)b];
					c = GlobalAssets.Instance.colorSet.GetColorByName(disease.overlayColourName);
				}
				this.animController.SetSymbolTint(LogicDiseaseSensor.TINT_SYMBOL, c);
				return;
			}
			this.animController.Play(LogicDiseaseSensor.OFF_ANIMS, KAnim.PlayMode.Once);
		}
	}

	// Token: 0x060048D7 RID: 18647 RVA: 0x0026473C File Offset: 0x0026293C
	protected override void UpdateSwitchStatus()
	{
		StatusItem status_item = this.switchedOn ? Db.Get().BuildingStatusItems.LogicSensorStatusActive : Db.Get().BuildingStatusItems.LogicSensorStatusInactive;
		base.GetComponent<KSelectable>().SetStatusItem(Db.Get().StatusItemCategories.Power, status_item, null);
	}

	// Token: 0x17000397 RID: 919
	// (get) Token: 0x060048D8 RID: 18648 RVA: 0x000CEF1F File Offset: 0x000CD11F
	public LocString Title
	{
		get
		{
			return UI.UISIDESCREENS.THRESHOLD_SWITCH_SIDESCREEN.DISEASE_TITLE;
		}
	}

	// Token: 0x0400330B RID: 13067
	[SerializeField]
	[Serialize]
	private float threshold;

	// Token: 0x0400330C RID: 13068
	[SerializeField]
	[Serialize]
	private bool activateAboveThreshold = true;

	// Token: 0x0400330D RID: 13069
	private KBatchedAnimController animController;

	// Token: 0x0400330E RID: 13070
	private bool wasOn;

	// Token: 0x0400330F RID: 13071
	private const float rangeMin = 0f;

	// Token: 0x04003310 RID: 13072
	private const float rangeMax = 100000f;

	// Token: 0x04003311 RID: 13073
	private const int WINDOW_SIZE = 8;

	// Token: 0x04003312 RID: 13074
	private int[] samples = new int[8];

	// Token: 0x04003313 RID: 13075
	private int sampleIdx;

	// Token: 0x04003314 RID: 13076
	[MyCmpAdd]
	private CopyBuildingSettings copyBuildingSettings;

	// Token: 0x04003315 RID: 13077
	private static readonly EventSystem.IntraObjectHandler<LogicDiseaseSensor> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<LogicDiseaseSensor>(delegate(LogicDiseaseSensor component, object data)
	{
		component.OnCopySettings(data);
	});

	// Token: 0x04003316 RID: 13078
	private static readonly HashedString[] ON_ANIMS = new HashedString[]
	{
		"on_pre",
		"on_loop"
	};

	// Token: 0x04003317 RID: 13079
	private static readonly HashedString[] OFF_ANIMS = new HashedString[]
	{
		"on_pst",
		"off"
	};

	// Token: 0x04003318 RID: 13080
	private static readonly HashedString TINT_SYMBOL = "germs";
}
