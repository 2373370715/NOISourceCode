using System;

// Token: 0x02001CB6 RID: 7350
public class ElementUsage
{
	// Token: 0x0600993C RID: 39228 RVA: 0x001080AD File Offset: 0x001062AD
	public ElementUsage(Tag tag, float amount, bool continuous) : this(tag, amount, continuous, null)
	{
	}

	// Token: 0x0600993D RID: 39229 RVA: 0x001080B9 File Offset: 0x001062B9
	public ElementUsage(Tag tag, float amount, bool continuous, Func<Tag, float, bool, string> customFormating)
	{
		this.tag = tag;
		this.amount = amount;
		this.continuous = continuous;
		this.customFormating = customFormating;
	}

	// Token: 0x0400772F RID: 30511
	public Tag tag;

	// Token: 0x04007730 RID: 30512
	public float amount;

	// Token: 0x04007731 RID: 30513
	public bool continuous;

	// Token: 0x04007732 RID: 30514
	public Func<Tag, float, bool, string> customFormating;
}
