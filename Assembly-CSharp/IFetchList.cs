using System;
using System.Collections.Generic;

// Token: 0x02001327 RID: 4903
public interface IFetchList
{
	// Token: 0x1700063E RID: 1598
	// (get) Token: 0x06006453 RID: 25683
	Storage Destination { get; }

	// Token: 0x06006454 RID: 25684
	float GetMinimumAmount(Tag tag);

	// Token: 0x06006455 RID: 25685
	Dictionary<Tag, float> GetRemaining();

	// Token: 0x06006456 RID: 25686
	Dictionary<Tag, float> GetRemainingMinimum();
}
