using System;
using UnityEngine;

// Token: 0x02000D36 RID: 3382
[AddComponentMenu("KMonoBehaviour/scripts/ConduitSecondaryOutput")]
public class ConduitSecondaryOutput : KMonoBehaviour, ISecondaryOutput
{
	// Token: 0x06004176 RID: 16758 RVA: 0x000CED3D File Offset: 0x000CCF3D
	public bool HasSecondaryConduitType(ConduitType type)
	{
		return this.portInfo.conduitType == type;
	}

	// Token: 0x06004177 RID: 16759 RVA: 0x000CED4D File Offset: 0x000CCF4D
	public CellOffset GetSecondaryConduitOffset(ConduitType type)
	{
		if (type == this.portInfo.conduitType)
		{
			return this.portInfo.offset;
		}
		return CellOffset.none;
	}

	// Token: 0x04002D45 RID: 11589
	[SerializeField]
	public ConduitPortInfo portInfo;
}
