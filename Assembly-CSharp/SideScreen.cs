using System;
using UnityEngine;

// Token: 0x02002030 RID: 8240
public class SideScreen : KScreen
{
	// Token: 0x0600AEA1 RID: 44705 RVA: 0x00116034 File Offset: 0x00114234
	public void SetContent(SideScreenContent sideScreenContent, GameObject target)
	{
		if (sideScreenContent.transform.parent != this.contentBody.transform)
		{
			sideScreenContent.transform.SetParent(this.contentBody.transform);
		}
		sideScreenContent.SetTarget(target);
	}

	// Token: 0x0400896E RID: 35182
	[SerializeField]
	private GameObject contentBody;
}
