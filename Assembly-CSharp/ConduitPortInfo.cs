using System;

// Token: 0x02000D34 RID: 3380
[Serializable]
public class ConduitPortInfo
{
	// Token: 0x06004172 RID: 16754 RVA: 0x000CECF6 File Offset: 0x000CCEF6
	public ConduitPortInfo(ConduitType type, CellOffset offset)
	{
		this.conduitType = type;
		this.offset = offset;
	}

	// Token: 0x04002D42 RID: 11586
	public ConduitType conduitType;

	// Token: 0x04002D43 RID: 11587
	public CellOffset offset;
}
