using System;
using UnityEngine;

// Token: 0x020018FD RID: 6397
public static class SoundUtil
{
	// Token: 0x06008476 RID: 33910 RVA: 0x00352B98 File Offset: 0x00350D98
	public static float GetLiquidDepth(int cell)
	{
		float num = 0f;
		num += Grid.Mass[cell] * (Grid.Element[cell].IsLiquid ? 1f : 0f);
		int num2 = Grid.CellBelow(cell);
		if (Grid.IsValidCell(num2))
		{
			num += Grid.Mass[num2] * (Grid.Element[num2].IsLiquid ? 1f : 0f);
		}
		return Mathf.Min(num / 1000f, 1f);
	}

	// Token: 0x06008477 RID: 33911 RVA: 0x000FB8F4 File Offset: 0x000F9AF4
	public static float GetLiquidVolume(float mass)
	{
		return Mathf.Min(mass / 100f, 1f);
	}
}
