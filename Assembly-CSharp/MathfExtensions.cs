using System;

// Token: 0x020004C5 RID: 1221
public static class MathfExtensions
{
	// Token: 0x0600150A RID: 5386 RVA: 0x000B3B4B File Offset: 0x000B1D4B
	public static long Max(this long a, long b)
	{
		if (a < b)
		{
			return b;
		}
		return a;
	}

	// Token: 0x0600150B RID: 5387 RVA: 0x000B3B54 File Offset: 0x000B1D54
	public static long Min(this long a, long b)
	{
		if (a > b)
		{
			return b;
		}
		return a;
	}
}
