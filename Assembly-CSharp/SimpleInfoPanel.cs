using System;
using UnityEngine;

// Token: 0x02001D00 RID: 7424
public class SimpleInfoPanel
{
	// Token: 0x06009AF9 RID: 39673 RVA: 0x00109488 File Offset: 0x00107688
	public SimpleInfoPanel(SimpleInfoScreen simpleInfoRoot)
	{
		this.simpleInfoRoot = simpleInfoRoot;
	}

	// Token: 0x06009AFA RID: 39674 RVA: 0x000AA038 File Offset: 0x000A8238
	public virtual void Refresh(CollapsibleDetailContentPanel panel, GameObject selectedTarget)
	{
	}

	// Token: 0x04007912 RID: 30994
	protected SimpleInfoScreen simpleInfoRoot;
}
