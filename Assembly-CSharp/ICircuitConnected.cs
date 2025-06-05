using System;

// Token: 0x020010AB RID: 4267
public interface ICircuitConnected
{
	// Token: 0x170004FE RID: 1278
	// (get) Token: 0x060056A1 RID: 22177
	bool IsVirtual { get; }

	// Token: 0x170004FF RID: 1279
	// (get) Token: 0x060056A2 RID: 22178
	int PowerCell { get; }

	// Token: 0x17000500 RID: 1280
	// (get) Token: 0x060056A3 RID: 22179
	object VirtualCircuitKey { get; }
}
