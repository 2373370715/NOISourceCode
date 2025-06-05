using System;
using STRINGS;

// Token: 0x02000D72 RID: 3442
public class DevRadiationEmitter : KMonoBehaviour, ISingleSliderControl, ISliderControl
{
	// Token: 0x060042C2 RID: 17090 RVA: 0x000CF98B File Offset: 0x000CDB8B
	protected override void OnSpawn()
	{
		base.OnSpawn();
		if (this.radiationEmitter != null)
		{
			this.radiationEmitter.SetEmitting(true);
		}
	}

	// Token: 0x1700034C RID: 844
	// (get) Token: 0x060042C3 RID: 17091 RVA: 0x000CF9AD File Offset: 0x000CDBAD
	public string SliderTitleKey
	{
		get
		{
			return BUILDINGS.PREFABS.DEVRADIATIONGENERATOR.NAME;
		}
	}

	// Token: 0x1700034D RID: 845
	// (get) Token: 0x060042C4 RID: 17092 RVA: 0x000CF9B9 File Offset: 0x000CDBB9
	public string SliderUnits
	{
		get
		{
			return UI.UNITSUFFIXES.RADIATION.RADS;
		}
	}

	// Token: 0x060042C5 RID: 17093 RVA: 0x000CF9C5 File Offset: 0x000CDBC5
	public float GetSliderMax(int index)
	{
		return 5000f;
	}

	// Token: 0x060042C6 RID: 17094 RVA: 0x000C18F8 File Offset: 0x000BFAF8
	public float GetSliderMin(int index)
	{
		return 0f;
	}

	// Token: 0x060042C7 RID: 17095 RVA: 0x000CBEB9 File Offset: 0x000CA0B9
	public string GetSliderTooltip(int index)
	{
		return "";
	}

	// Token: 0x060042C8 RID: 17096 RVA: 0x000CBEB9 File Offset: 0x000CA0B9
	public string GetSliderTooltipKey(int index)
	{
		return "";
	}

	// Token: 0x060042C9 RID: 17097 RVA: 0x000CF9CC File Offset: 0x000CDBCC
	public float GetSliderValue(int index)
	{
		return this.radiationEmitter.emitRads;
	}

	// Token: 0x060042CA RID: 17098 RVA: 0x000CF9D9 File Offset: 0x000CDBD9
	public void SetSliderValue(float value, int index)
	{
		this.radiationEmitter.emitRads = value;
		this.radiationEmitter.Refresh();
	}

	// Token: 0x060042CB RID: 17099 RVA: 0x000B1628 File Offset: 0x000AF828
	public int SliderDecimalPlaces(int index)
	{
		return 0;
	}

	// Token: 0x04002E03 RID: 11779
	[MyCmpReq]
	private RadiationEmitter radiationEmitter;
}
