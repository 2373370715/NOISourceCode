using System;
using Klei.AI;

// Token: 0x02001C2D RID: 7213
public interface IAmountDisplayer
{
	// Token: 0x06009619 RID: 38425
	string GetValueString(Amount master, AmountInstance instance);

	// Token: 0x0600961A RID: 38426
	string GetDescription(Amount master, AmountInstance instance);

	// Token: 0x0600961B RID: 38427
	string GetTooltip(Amount master, AmountInstance instance);

	// Token: 0x170009BE RID: 2494
	// (get) Token: 0x0600961C RID: 38428
	IAttributeFormatter Formatter { get; }
}
