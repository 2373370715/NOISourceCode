using System;

// Token: 0x0200110E RID: 4366
public interface IConduitConsumer
{
	// Token: 0x1700054B RID: 1355
	// (get) Token: 0x06005944 RID: 22852
	Storage Storage { get; }

	// Token: 0x1700054C RID: 1356
	// (get) Token: 0x06005945 RID: 22853
	ConduitType ConduitType { get; }
}
