using System;

// Token: 0x02001234 RID: 4660
public interface IWiltCause
{
	// Token: 0x170005A1 RID: 1441
	// (get) Token: 0x06005E97 RID: 24215
	string WiltStateString { get; }

	// Token: 0x170005A2 RID: 1442
	// (get) Token: 0x06005E98 RID: 24216
	WiltCondition.Condition[] Conditions { get; }
}
