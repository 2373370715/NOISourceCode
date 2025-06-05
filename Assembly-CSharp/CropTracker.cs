using System;

// Token: 0x02000B78 RID: 2936
public class CropTracker : WorldTracker
{
	// Token: 0x0600372A RID: 14122 RVA: 0x000C84F8 File Offset: 0x000C66F8
	public CropTracker(int worldID) : base(worldID)
	{
	}

	// Token: 0x0600372B RID: 14123 RVA: 0x0022361C File Offset: 0x0022181C
	public override void UpdateData()
	{
		float num = 0f;
		foreach (PlantablePlot plantablePlot in Components.PlantablePlots.GetItems(base.WorldID))
		{
			if (!(plantablePlot.plant == null) && plantablePlot.HasDepositTag(GameTags.CropSeed) && !plantablePlot.plant.HasTag(GameTags.Wilting))
			{
				num += 1f;
			}
		}
		base.AddPoint(num);
	}

	// Token: 0x0600372C RID: 14124 RVA: 0x000C8552 File Offset: 0x000C6752
	public override string FormatValueString(float value)
	{
		return value.ToString() + "%";
	}
}
