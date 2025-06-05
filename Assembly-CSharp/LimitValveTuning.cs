using System;

// Token: 0x020003CD RID: 973
public class LimitValveTuning
{
	// Token: 0x06000FD5 RID: 4053 RVA: 0x000B13E3 File Offset: 0x000AF5E3
	public static NonLinearSlider.Range[] GetDefaultSlider()
	{
		return new NonLinearSlider.Range[]
		{
			new NonLinearSlider.Range(70f, 100f),
			new NonLinearSlider.Range(30f, 500f)
		};
	}

	// Token: 0x04000B6A RID: 2922
	public const float MAX_LIMIT = 500f;

	// Token: 0x04000B6B RID: 2923
	public const float DEFAULT_LIMIT = 100f;
}
