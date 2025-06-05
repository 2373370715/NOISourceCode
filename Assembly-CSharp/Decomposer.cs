using System;
using UnityEngine;

// Token: 0x02001248 RID: 4680
[AddComponentMenu("KMonoBehaviour/scripts/Decomposer")]
public class Decomposer : KMonoBehaviour
{
	// Token: 0x06005F33 RID: 24371 RVA: 0x002B4318 File Offset: 0x002B2518
	protected override void OnSpawn()
	{
		base.OnSpawn();
		StateMachineController component = base.GetComponent<StateMachineController>();
		if (component == null)
		{
			return;
		}
		DecompositionMonitor.Instance instance = new DecompositionMonitor.Instance(this, null, 1f, false);
		component.AddStateMachineInstance(instance);
		instance.StartSM();
		instance.dirtyWaterMaxRange = 3;
	}
}
