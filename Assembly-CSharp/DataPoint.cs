using System;

// Token: 0x02000B89 RID: 2953
public struct DataPoint
{
	// Token: 0x06003767 RID: 14183 RVA: 0x000C8691 File Offset: 0x000C6891
	public DataPoint(float start, float end, float value)
	{
		this.periodStart = start;
		this.periodEnd = end;
		this.periodValue = value;
	}

	// Token: 0x0400261D RID: 9757
	public float periodStart;

	// Token: 0x0400261E RID: 9758
	public float periodEnd;

	// Token: 0x0400261F RID: 9759
	public float periodValue;
}
