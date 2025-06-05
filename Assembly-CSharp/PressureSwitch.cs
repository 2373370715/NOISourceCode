using System;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x02000F60 RID: 3936
[SerializationConfig(MemberSerialization.OptIn)]
public class PressureSwitch : CircuitSwitch, ISaveLoadable, IThresholdSwitch, ISim200ms
{
	// Token: 0x06004EDE RID: 20190 RVA: 0x00277ADC File Offset: 0x00275CDC
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

	// Token: 0x06004EDF RID: 20191 RVA: 0x0026473C File Offset: 0x0026293C
	protected override void UpdateSwitchStatus()
	{
		StatusItem status_item = this.switchedOn ? Db.Get().BuildingStatusItems.LogicSensorStatusActive : Db.Get().BuildingStatusItems.LogicSensorStatusInactive;
		base.GetComponent<KSelectable>().SetStatusItem(Db.Get().StatusItemCategories.Power, status_item, null);
	}

	// Token: 0x17000455 RID: 1109
	// (get) Token: 0x06004EE0 RID: 20192 RVA: 0x000D7B25 File Offset: 0x000D5D25
	// (set) Token: 0x06004EE1 RID: 20193 RVA: 0x000D7B2D File Offset: 0x000D5D2D
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

	// Token: 0x17000456 RID: 1110
	// (get) Token: 0x06004EE2 RID: 20194 RVA: 0x000D7B36 File Offset: 0x000D5D36
	// (set) Token: 0x06004EE3 RID: 20195 RVA: 0x000D7B3E File Offset: 0x000D5D3E
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

	// Token: 0x17000457 RID: 1111
	// (get) Token: 0x06004EE4 RID: 20196 RVA: 0x00277BA4 File Offset: 0x00275DA4
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

	// Token: 0x17000458 RID: 1112
	// (get) Token: 0x06004EE5 RID: 20197 RVA: 0x000D7B47 File Offset: 0x000D5D47
	public float RangeMin
	{
		get
		{
			return this.rangeMin;
		}
	}

	// Token: 0x17000459 RID: 1113
	// (get) Token: 0x06004EE6 RID: 20198 RVA: 0x000D7B4F File Offset: 0x000D5D4F
	public float RangeMax
	{
		get
		{
			return this.rangeMax;
		}
	}

	// Token: 0x06004EE7 RID: 20199 RVA: 0x000D7B57 File Offset: 0x000D5D57
	public float GetRangeMinInputField()
	{
		if (this.desiredState != Element.State.Gas)
		{
			return this.rangeMin;
		}
		return this.rangeMin * 1000f;
	}

	// Token: 0x06004EE8 RID: 20200 RVA: 0x000D7B75 File Offset: 0x000D5D75
	public float GetRangeMaxInputField()
	{
		if (this.desiredState != Element.State.Gas)
		{
			return this.rangeMax;
		}
		return this.rangeMax * 1000f;
	}

	// Token: 0x1700045A RID: 1114
	// (get) Token: 0x06004EE9 RID: 20201 RVA: 0x000D454A File Offset: 0x000D274A
	public LocString Title
	{
		get
		{
			return UI.UISIDESCREENS.THRESHOLD_SWITCH_SIDESCREEN.TITLE;
		}
	}

	// Token: 0x1700045B RID: 1115
	// (get) Token: 0x06004EEA RID: 20202 RVA: 0x000D4599 File Offset: 0x000D2799
	public LocString ThresholdValueName
	{
		get
		{
			return UI.UISIDESCREENS.THRESHOLD_SWITCH_SIDESCREEN.PRESSURE;
		}
	}

	// Token: 0x1700045C RID: 1116
	// (get) Token: 0x06004EEB RID: 20203 RVA: 0x000D45A0 File Offset: 0x000D27A0
	public string AboveToolTip
	{
		get
		{
			return UI.UISIDESCREENS.THRESHOLD_SWITCH_SIDESCREEN.PRESSURE_TOOLTIP_ABOVE;
		}
	}

	// Token: 0x1700045D RID: 1117
	// (get) Token: 0x06004EEC RID: 20204 RVA: 0x000D45AC File Offset: 0x000D27AC
	public string BelowToolTip
	{
		get
		{
			return UI.UISIDESCREENS.THRESHOLD_SWITCH_SIDESCREEN.PRESSURE_TOOLTIP_BELOW;
		}
	}

	// Token: 0x06004EED RID: 20205 RVA: 0x00277BD8 File Offset: 0x00275DD8
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

	// Token: 0x06004EEE RID: 20206 RVA: 0x000D7B93 File Offset: 0x000D5D93
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

	// Token: 0x06004EEF RID: 20207 RVA: 0x000D7BBD File Offset: 0x000D5DBD
	public float ProcessedInputValue(float input)
	{
		if (this.desiredState == Element.State.Gas)
		{
			input /= 1000f;
		}
		return input;
	}

	// Token: 0x06004EF0 RID: 20208 RVA: 0x000D7BD2 File Offset: 0x000D5DD2
	public LocString ThresholdValueUnits()
	{
		return GameUtil.GetCurrentMassUnit(this.desiredState == Element.State.Gas);
	}

	// Token: 0x1700045E RID: 1118
	// (get) Token: 0x06004EF1 RID: 20209 RVA: 0x000B1628 File Offset: 0x000AF828
	public ThresholdScreenLayoutType LayoutType
	{
		get
		{
			return ThresholdScreenLayoutType.SliderBar;
		}
	}

	// Token: 0x1700045F RID: 1119
	// (get) Token: 0x06004EF2 RID: 20210 RVA: 0x000AA7E7 File Offset: 0x000A89E7
	public int IncrementScale
	{
		get
		{
			return 1;
		}
	}

	// Token: 0x17000460 RID: 1120
	// (get) Token: 0x06004EF3 RID: 20211 RVA: 0x000D7BE2 File Offset: 0x000D5DE2
	public NonLinearSlider.Range[] GetRanges
	{
		get
		{
			return NonLinearSlider.GetDefaultRange(this.RangeMax);
		}
	}

	// Token: 0x0400375B RID: 14171
	[SerializeField]
	[Serialize]
	private float threshold;

	// Token: 0x0400375C RID: 14172
	[SerializeField]
	[Serialize]
	private bool activateAboveThreshold = true;

	// Token: 0x0400375D RID: 14173
	public float rangeMin;

	// Token: 0x0400375E RID: 14174
	public float rangeMax = 1f;

	// Token: 0x0400375F RID: 14175
	public Element.State desiredState = Element.State.Gas;

	// Token: 0x04003760 RID: 14176
	private const int WINDOW_SIZE = 8;

	// Token: 0x04003761 RID: 14177
	private float[] samples = new float[8];

	// Token: 0x04003762 RID: 14178
	private int sampleIdx;
}
