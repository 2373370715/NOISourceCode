using System;

// Token: 0x02001627 RID: 5671
[Serializable]
public struct SicknessExposureInfo
{
	// Token: 0x06007560 RID: 30048 RVA: 0x000F19CE File Offset: 0x000EFBCE
	public SicknessExposureInfo(string id, string infection_source_info)
	{
		this.sicknessID = id;
		this.sourceInfo = infection_source_info;
	}

	// Token: 0x0400582D RID: 22573
	public string sicknessID;

	// Token: 0x0400582E RID: 22574
	public string sourceInfo;
}
