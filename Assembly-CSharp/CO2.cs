using System;
using KSerialization;
using UnityEngine;

// Token: 0x02001090 RID: 4240
[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/scripts/CO2")]
public class CO2 : KMonoBehaviour
{
	// Token: 0x06005628 RID: 22056 RVA: 0x000DC983 File Offset: 0x000DAB83
	public void StartLoop()
	{
		KBatchedAnimController component = base.GetComponent<KBatchedAnimController>();
		component.Play("exhale_pre", KAnim.PlayMode.Once, 1f, 0f);
		component.Play("exhale_loop", KAnim.PlayMode.Loop, 1f, 0f);
	}

	// Token: 0x06005629 RID: 22057 RVA: 0x000DC9C0 File Offset: 0x000DABC0
	public void TriggerDestroy()
	{
		base.GetComponent<KBatchedAnimController>().Play("exhale_pst", KAnim.PlayMode.Once, 1f, 0f);
	}

	// Token: 0x04003CF3 RID: 15603
	[Serialize]
	[NonSerialized]
	public Vector3 velocity = Vector3.zero;

	// Token: 0x04003CF4 RID: 15604
	[Serialize]
	[NonSerialized]
	public float mass;

	// Token: 0x04003CF5 RID: 15605
	[Serialize]
	[NonSerialized]
	public float temperature;

	// Token: 0x04003CF6 RID: 15606
	[Serialize]
	[NonSerialized]
	public float lifetimeRemaining;
}
