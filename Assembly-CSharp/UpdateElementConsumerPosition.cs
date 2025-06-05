using System;
using UnityEngine;

// Token: 0x0200025D RID: 605
[AddComponentMenu("KMonoBehaviour/scripts/UpdateElementConsumerPosition")]
public class UpdateElementConsumerPosition : KMonoBehaviour, ISim200ms
{
	// Token: 0x06000894 RID: 2196 RVA: 0x000AE4C1 File Offset: 0x000AC6C1
	public void Sim200ms(float dt)
	{
		base.GetComponent<ElementConsumer>().RefreshConsumptionRate();
	}
}
