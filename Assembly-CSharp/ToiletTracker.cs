using System;

// Token: 0x02000B7A RID: 2938
public class ToiletTracker : WorldTracker
{
	// Token: 0x06003730 RID: 14128 RVA: 0x000C84F8 File Offset: 0x000C66F8
	public ToiletTracker(int worldID) : base(worldID)
	{
	}

	// Token: 0x06003731 RID: 14129 RVA: 0x000AFECA File Offset: 0x000AE0CA
	public override void UpdateData()
	{
		throw new NotImplementedException();
	}

	// Token: 0x06003732 RID: 14130 RVA: 0x000C6C93 File Offset: 0x000C4E93
	public override string FormatValueString(float value)
	{
		return value.ToString();
	}
}
