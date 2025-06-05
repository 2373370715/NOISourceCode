using System;

// Token: 0x02001114 RID: 4372
public interface IConduitDispenser
{
	// Token: 0x17000559 RID: 1369
	// (get) Token: 0x06005968 RID: 22888
	Storage Storage { get; }

	// Token: 0x1700055A RID: 1370
	// (get) Token: 0x06005969 RID: 22889
	ConduitType ConduitType { get; }
}
