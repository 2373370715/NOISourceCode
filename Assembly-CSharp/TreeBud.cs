using System;
using KSerialization;
using UnityEngine;

// Token: 0x02001758 RID: 5976
[AddComponentMenu("KMonoBehaviour/scripts/TreeBud")]
public class TreeBud : KMonoBehaviour
{
	// Token: 0x06007AD6 RID: 31446 RVA: 0x00327798 File Offset: 0x00325998
	protected override void OnSpawn()
	{
		base.OnSpawn();
		PlantBranch.Instance smi = base.gameObject.GetSMI<PlantBranch.Instance>();
		if (smi != null && !smi.IsRunning())
		{
			smi.StartSM();
		}
	}

	// Token: 0x06007AD7 RID: 31447 RVA: 0x000F5581 File Offset: 0x000F3781
	public BuddingTrunk GetAndForgetOldTrunk()
	{
		BuddingTrunk result = (this.buddingTrunk == null) ? null : this.buddingTrunk.Get();
		this.buddingTrunk = null;
		return result;
	}

	// Token: 0x04005C85 RID: 23685
	[Serialize]
	public Ref<BuddingTrunk> buddingTrunk;
}
