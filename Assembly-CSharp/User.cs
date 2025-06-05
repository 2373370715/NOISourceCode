using System;
using UnityEngine;

// Token: 0x020007AE RID: 1966
[AddComponentMenu("KMonoBehaviour/scripts/User")]
public class User : KMonoBehaviour
{
	// Token: 0x060022D7 RID: 8919 RVA: 0x000BB159 File Offset: 0x000B9359
	public void OnStateMachineStop(string reason, StateMachine.Status status)
	{
		if (status == StateMachine.Status.Success)
		{
			base.Trigger(58624316, null);
			return;
		}
		base.Trigger(1572098533, null);
	}
}
