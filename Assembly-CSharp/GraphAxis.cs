using System;

// Token: 0x02001D40 RID: 7488
[Serializable]
public struct GraphAxis
{
	// Token: 0x17000A4E RID: 2638
	// (get) Token: 0x06009C72 RID: 40050 RVA: 0x0010A34B File Offset: 0x0010854B
	public float range
	{
		get
		{
			return this.max_value - this.min_value;
		}
	}

	// Token: 0x04007A76 RID: 31350
	public string name;

	// Token: 0x04007A77 RID: 31351
	public float min_value;

	// Token: 0x04007A78 RID: 31352
	public float max_value;

	// Token: 0x04007A79 RID: 31353
	public float guide_frequency;
}
