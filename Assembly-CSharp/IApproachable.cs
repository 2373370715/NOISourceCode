using System;
using UnityEngine;

// Token: 0x020009B6 RID: 2486
public interface IApproachable
{
	// Token: 0x06002C92 RID: 11410
	CellOffset[] GetOffsets();

	// Token: 0x06002C93 RID: 11411
	int GetCell();

	// Token: 0x1700019B RID: 411
	// (get) Token: 0x06002C94 RID: 11412
	Transform transform { get; }
}
