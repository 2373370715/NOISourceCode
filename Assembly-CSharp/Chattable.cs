using System;
using UnityEngine;

// Token: 0x020009DA RID: 2522
[AddComponentMenu("KMonoBehaviour/scripts/Chattable")]
public class Chattable : KMonoBehaviour, IApproachable
{
	// Token: 0x06002DA9 RID: 11689 RVA: 0x000C20AA File Offset: 0x000C02AA
	public CellOffset[] GetOffsets()
	{
		return OffsetGroups.Chat;
	}

	// Token: 0x06002DAA RID: 11690 RVA: 0x000C1501 File Offset: 0x000BF701
	public int GetCell()
	{
		return Grid.PosToCell(this);
	}
}
