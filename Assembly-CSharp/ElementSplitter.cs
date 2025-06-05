using System;
using UnityEngine;

// Token: 0x02000A73 RID: 2675
public struct ElementSplitter
{
	// Token: 0x0600309C RID: 12444 RVA: 0x000C40EF File Offset: 0x000C22EF
	public ElementSplitter(GameObject go)
	{
		this.primaryElement = go.GetComponent<PrimaryElement>();
		this.kPrefabID = go.GetComponent<KPrefabID>();
		this.onTakeCB = null;
		this.canAbsorbCB = null;
	}

	// Token: 0x04002159 RID: 8537
	public PrimaryElement primaryElement;

	// Token: 0x0400215A RID: 8538
	public Func<Pickupable, float, Pickupable> onTakeCB;

	// Token: 0x0400215B RID: 8539
	public Func<Pickupable, bool> canAbsorbCB;

	// Token: 0x0400215C RID: 8540
	public KPrefabID kPrefabID;
}
