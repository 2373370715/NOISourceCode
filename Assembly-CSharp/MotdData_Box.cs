using System;
using UnityEngine;

// Token: 0x02001E99 RID: 7833
public class MotdData_Box
{
	// Token: 0x0600A433 RID: 42035 RVA: 0x003F46B4 File Offset: 0x003F28B4
	public bool ShouldDisplay()
	{
		long num = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
		return num >= this.startTime && this.finishTime >= num;
	}

	// Token: 0x0400805E RID: 32862
	public string category;

	// Token: 0x0400805F RID: 32863
	public string guid;

	// Token: 0x04008060 RID: 32864
	public long startTime;

	// Token: 0x04008061 RID: 32865
	public long finishTime;

	// Token: 0x04008062 RID: 32866
	public string title;

	// Token: 0x04008063 RID: 32867
	public string text;

	// Token: 0x04008064 RID: 32868
	public string image;

	// Token: 0x04008065 RID: 32869
	public string href;

	// Token: 0x04008066 RID: 32870
	public Texture2D resolvedImage;

	// Token: 0x04008067 RID: 32871
	public bool resolvedImageIsFromDisk;
}
