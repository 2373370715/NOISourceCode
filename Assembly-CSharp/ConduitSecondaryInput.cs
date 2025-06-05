using System;
using UnityEngine;

// Token: 0x02000D35 RID: 3381
[AddComponentMenu("KMonoBehaviour/scripts/ConduitSecondaryInput")]
public class ConduitSecondaryInput : KMonoBehaviour, ISecondaryInput
{
	// Token: 0x06004173 RID: 16755 RVA: 0x000CED0C File Offset: 0x000CCF0C
	public bool HasSecondaryConduitType(ConduitType type)
	{
		return this.portInfo.conduitType == type;
	}

	// Token: 0x06004174 RID: 16756 RVA: 0x000CED1C File Offset: 0x000CCF1C
	public CellOffset GetSecondaryConduitOffset(ConduitType type)
	{
		if (this.portInfo.conduitType == type)
		{
			return this.portInfo.offset;
		}
		return CellOffset.none;
	}

	// Token: 0x04002D44 RID: 11588
	[SerializeField]
	public ConduitPortInfo portInfo;
}
