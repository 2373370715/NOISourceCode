using System;
using Klei.AI;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x02000D40 RID: 3392
[SerializationConfig(MemberSerialization.OptIn)]
public class ConduitDiseaseSensor : ConduitThresholdSensor, IThresholdSwitch
{
	// Token: 0x060041AD RID: 16813 RVA: 0x0024CE94 File Offset: 0x0024B094
	protected override void UpdateVisualState(bool force = false)
	{
		if (this.wasOn != this.switchedOn || force)
		{
			this.wasOn = this.switchedOn;
			if (this.switchedOn)
			{
				this.animController.Play(ConduitSensor.ON_ANIMS, KAnim.PlayMode.Loop);
				int num;
				int num2;
				bool flag;
				this.GetContentsDisease(out num, out num2, out flag);
				Color32 c = Color.white;
				if (num != 255)
				{
					Disease disease = Db.Get().Diseases[num];
					c = GlobalAssets.Instance.colorSet.GetColorByName(disease.overlayColourName);
				}
				this.animController.SetSymbolTint(ConduitDiseaseSensor.TINT_SYMBOL, c);
				return;
			}
			this.animController.Play(ConduitSensor.OFF_ANIMS, KAnim.PlayMode.Once);
		}
	}

	// Token: 0x060041AE RID: 16814 RVA: 0x0024CF54 File Offset: 0x0024B154
	private void GetContentsDisease(out int diseaseIdx, out int diseaseCount, out bool hasMass)
	{
		int cell = Grid.PosToCell(this);
		if (this.conduitType == ConduitType.Liquid || this.conduitType == ConduitType.Gas)
		{
			ConduitFlow.ConduitContents contents = Conduit.GetFlowManager(this.conduitType).GetContents(cell);
			diseaseIdx = (int)contents.diseaseIdx;
			diseaseCount = contents.diseaseCount;
			hasMass = (contents.mass > 0f);
			return;
		}
		SolidConduitFlow flowManager = SolidConduit.GetFlowManager();
		SolidConduitFlow.ConduitContents contents2 = flowManager.GetContents(cell);
		Pickupable pickupable = flowManager.GetPickupable(contents2.pickupableHandle);
		if (pickupable != null && pickupable.PrimaryElement.Mass > 0f)
		{
			diseaseIdx = (int)pickupable.PrimaryElement.DiseaseIdx;
			diseaseCount = pickupable.PrimaryElement.DiseaseCount;
			hasMass = true;
			return;
		}
		diseaseIdx = 0;
		diseaseCount = 0;
		hasMass = false;
	}

	// Token: 0x1700032A RID: 810
	// (get) Token: 0x060041AF RID: 16815 RVA: 0x0024D008 File Offset: 0x0024B208
	public override float CurrentValue
	{
		get
		{
			int num;
			int num2;
			bool flag;
			this.GetContentsDisease(out num, out num2, out flag);
			if (flag)
			{
				this.lastValue = (float)num2;
			}
			return this.lastValue;
		}
	}

	// Token: 0x1700032B RID: 811
	// (get) Token: 0x060041B0 RID: 16816 RVA: 0x000C18F8 File Offset: 0x000BFAF8
	public float RangeMin
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x1700032C RID: 812
	// (get) Token: 0x060041B1 RID: 16817 RVA: 0x000CEF18 File Offset: 0x000CD118
	public float RangeMax
	{
		get
		{
			return 100000f;
		}
	}

	// Token: 0x060041B2 RID: 16818 RVA: 0x000C18F8 File Offset: 0x000BFAF8
	public float GetRangeMinInputField()
	{
		return 0f;
	}

	// Token: 0x060041B3 RID: 16819 RVA: 0x000CEF18 File Offset: 0x000CD118
	public float GetRangeMaxInputField()
	{
		return 100000f;
	}

	// Token: 0x1700032D RID: 813
	// (get) Token: 0x060041B4 RID: 16820 RVA: 0x000CEF1F File Offset: 0x000CD11F
	public LocString Title
	{
		get
		{
			return UI.UISIDESCREENS.THRESHOLD_SWITCH_SIDESCREEN.DISEASE_TITLE;
		}
	}

	// Token: 0x1700032E RID: 814
	// (get) Token: 0x060041B5 RID: 16821 RVA: 0x000CEF26 File Offset: 0x000CD126
	public LocString ThresholdValueName
	{
		get
		{
			return UI.UISIDESCREENS.THRESHOLD_SWITCH_SIDESCREEN.CONTENT_DISEASE;
		}
	}

	// Token: 0x1700032F RID: 815
	// (get) Token: 0x060041B6 RID: 16822 RVA: 0x000CEF2D File Offset: 0x000CD12D
	public string AboveToolTip
	{
		get
		{
			return UI.UISIDESCREENS.THRESHOLD_SWITCH_SIDESCREEN.DISEASE_TOOLTIP_ABOVE;
		}
	}

	// Token: 0x17000330 RID: 816
	// (get) Token: 0x060041B7 RID: 16823 RVA: 0x000CEF39 File Offset: 0x000CD139
	public string BelowToolTip
	{
		get
		{
			return UI.UISIDESCREENS.THRESHOLD_SWITCH_SIDESCREEN.DISEASE_TOOLTIP_BELOW;
		}
	}

	// Token: 0x060041B8 RID: 16824 RVA: 0x000CEF45 File Offset: 0x000CD145
	public string Format(float value, bool units)
	{
		return GameUtil.GetFormattedInt((float)((int)value), GameUtil.TimeSlice.None);
	}

	// Token: 0x060041B9 RID: 16825 RVA: 0x000B64D6 File Offset: 0x000B46D6
	public float ProcessedSliderValue(float input)
	{
		return input;
	}

	// Token: 0x060041BA RID: 16826 RVA: 0x000B64D6 File Offset: 0x000B46D6
	public float ProcessedInputValue(float input)
	{
		return input;
	}

	// Token: 0x060041BB RID: 16827 RVA: 0x000CEF50 File Offset: 0x000CD150
	public LocString ThresholdValueUnits()
	{
		return UI.UISIDESCREENS.THRESHOLD_SWITCH_SIDESCREEN.DISEASE_UNITS;
	}

	// Token: 0x17000331 RID: 817
	// (get) Token: 0x060041BC RID: 16828 RVA: 0x000B1628 File Offset: 0x000AF828
	public ThresholdScreenLayoutType LayoutType
	{
		get
		{
			return ThresholdScreenLayoutType.SliderBar;
		}
	}

	// Token: 0x17000332 RID: 818
	// (get) Token: 0x060041BD RID: 16829 RVA: 0x000AA7E7 File Offset: 0x000A89E7
	public int IncrementScale
	{
		get
		{
			return 1;
		}
	}

	// Token: 0x17000333 RID: 819
	// (get) Token: 0x060041BE RID: 16830 RVA: 0x000CEF57 File Offset: 0x000CD157
	public NonLinearSlider.Range[] GetRanges
	{
		get
		{
			return NonLinearSlider.GetDefaultRange(this.RangeMax);
		}
	}

	// Token: 0x04002D5F RID: 11615
	private const float rangeMin = 0f;

	// Token: 0x04002D60 RID: 11616
	private const float rangeMax = 100000f;

	// Token: 0x04002D61 RID: 11617
	[Serialize]
	private float lastValue;

	// Token: 0x04002D62 RID: 11618
	private static readonly HashedString TINT_SYMBOL = "germs";
}
