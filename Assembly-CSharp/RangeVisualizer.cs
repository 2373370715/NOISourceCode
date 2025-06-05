using System;
using UnityEngine;

// Token: 0x02000B01 RID: 2817
[AddComponentMenu("KMonoBehaviour/scripts/RangeVisualizer")]
public class RangeVisualizer : KMonoBehaviour
{
	// Token: 0x040023C1 RID: 9153
	public Vector2I OriginOffset;

	// Token: 0x040023C2 RID: 9154
	public Vector2I RangeMin;

	// Token: 0x040023C3 RID: 9155
	public Vector2I RangeMax;

	// Token: 0x040023C4 RID: 9156
	public bool TestLineOfSight = true;

	// Token: 0x040023C5 RID: 9157
	public bool BlockingTileVisible;

	// Token: 0x040023C6 RID: 9158
	public Func<int, bool> BlockingVisibleCb;

	// Token: 0x040023C7 RID: 9159
	public Func<int, bool> BlockingCb = new Func<int, bool>(Grid.IsSolidCell);

	// Token: 0x040023C8 RID: 9160
	public bool AllowLineOfSightInvalidCells;
}
