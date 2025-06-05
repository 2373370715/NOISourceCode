using System;
using UnityEngine;

// Token: 0x020009B7 RID: 2487
[SkipSaveFileSerialization]
[AddComponentMenu("KMonoBehaviour/scripts/Approachable")]
public class Approachable : KMonoBehaviour, IApproachable
{
	// Token: 0x06002C95 RID: 11413 RVA: 0x000C14FA File Offset: 0x000BF6FA
	public CellOffset[] GetOffsets()
	{
		return OffsetGroups.Use;
	}

	// Token: 0x06002C96 RID: 11414 RVA: 0x000C1501 File Offset: 0x000BF701
	public int GetCell()
	{
		return Grid.PosToCell(this);
	}
}
