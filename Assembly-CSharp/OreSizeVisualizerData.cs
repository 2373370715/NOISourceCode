using System;
using UnityEngine;

// Token: 0x020016D6 RID: 5846
public struct OreSizeVisualizerData
{
	// Token: 0x060078A2 RID: 30882 RVA: 0x000F3CE2 File Offset: 0x000F1EE2
	public OreSizeVisualizerData(GameObject go)
	{
		this.primaryElement = go.GetComponent<PrimaryElement>();
		this.onMassChangedCB = null;
	}

	// Token: 0x04005A9E RID: 23198
	public PrimaryElement primaryElement;

	// Token: 0x04005A9F RID: 23199
	public Action<object> onMassChangedCB;
}
