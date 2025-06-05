using System;
using UnityEngine;

// Token: 0x02000B3E RID: 2878
[AddComponentMenu("KMonoBehaviour/scripts/SkyVisibilityVisualizer")]
public class SkyVisibilityVisualizer : KMonoBehaviour
{
	// Token: 0x06003568 RID: 13672 RVA: 0x000C747C File Offset: 0x000C567C
	private static bool HasSkyVisibility(int cell)
	{
		return Grid.ExposedToSunlight[cell] >= 1;
	}

	// Token: 0x040024DC RID: 9436
	public Vector2I OriginOffset = new Vector2I(0, 0);

	// Token: 0x040024DD RID: 9437
	public bool TwoWideOrgin;

	// Token: 0x040024DE RID: 9438
	public int RangeMin;

	// Token: 0x040024DF RID: 9439
	public int RangeMax;

	// Token: 0x040024E0 RID: 9440
	public int ScanVerticalStep;

	// Token: 0x040024E1 RID: 9441
	public bool SkipOnModuleInteriors;

	// Token: 0x040024E2 RID: 9442
	public bool AllOrNothingVisibility;

	// Token: 0x040024E3 RID: 9443
	public Func<int, bool> SkyVisibilityCb = new Func<int, bool>(SkyVisibilityVisualizer.HasSkyVisibility);
}
