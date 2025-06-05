using System;
using UnityEngine;

// Token: 0x02001036 RID: 4150
public interface IUsable
{
	// Token: 0x0600541B RID: 21531
	bool IsUsable();

	// Token: 0x170004D5 RID: 1237
	// (get) Token: 0x0600541C RID: 21532
	Transform transform { get; }
}
