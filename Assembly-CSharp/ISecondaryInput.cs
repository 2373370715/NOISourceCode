using System;

// Token: 0x02000D2F RID: 3375
public interface ISecondaryInput
{
	// Token: 0x06004142 RID: 16706
	bool HasSecondaryConduitType(ConduitType type);

	// Token: 0x06004143 RID: 16707
	CellOffset GetSecondaryConduitOffset(ConduitType type);
}
