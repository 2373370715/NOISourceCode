using System;

// Token: 0x02000D2E RID: 3374
public interface ISecondaryOutput
{
	// Token: 0x06004140 RID: 16704
	bool HasSecondaryConduitType(ConduitType type);

	// Token: 0x06004141 RID: 16705
	CellOffset GetSecondaryConduitOffset(ConduitType type);
}
