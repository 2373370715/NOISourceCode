using System;
using UnityEngine;

// Token: 0x02001444 RID: 5188
public interface ITelepadDeliverableContainer
{
	// Token: 0x06006A5F RID: 27231
	void SelectDeliverable();

	// Token: 0x06006A60 RID: 27232
	void DeselectDeliverable();

	// Token: 0x06006A61 RID: 27233
	GameObject GetGameObject();
}
