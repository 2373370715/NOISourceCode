using System;
using UnityEngine;

// Token: 0x02000B29 RID: 2857
public interface IRottable
{
	// Token: 0x17000245 RID: 581
	// (get) Token: 0x06003500 RID: 13568
	GameObject gameObject { get; }

	// Token: 0x17000246 RID: 582
	// (get) Token: 0x06003501 RID: 13569
	float RotTemperature { get; }

	// Token: 0x17000247 RID: 583
	// (get) Token: 0x06003502 RID: 13570
	float PreserveTemperature { get; }
}
