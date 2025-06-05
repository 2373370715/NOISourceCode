using System;
using KSerialization;
using UnityEngine;

// Token: 0x02001708 RID: 5896
[AddComponentMenu("KMonoBehaviour/scripts/BuddingTrunk")]
public class BuddingTrunk : KMonoBehaviour
{
	// Token: 0x0600796F RID: 31087 RVA: 0x0032302C File Offset: 0x0032122C
	protected override void OnSpawn()
	{
		base.OnSpawn();
		PlantBranchGrower.Instance smi = base.gameObject.GetSMI<PlantBranchGrower.Instance>();
		if (smi != null && !smi.IsRunning())
		{
			smi.StartSM();
		}
	}

	// Token: 0x06007970 RID: 31088 RVA: 0x0032305C File Offset: 0x0032125C
	public KPrefabID[] GetAndForgetOldSerializedBranches()
	{
		KPrefabID[] array = null;
		if (this.buds != null)
		{
			array = new KPrefabID[this.buds.Length];
			for (int i = 0; i < this.buds.Length; i++)
			{
				HarvestDesignatable harvestDesignatable = (this.buds[i] == null) ? null : this.buds[i].Get();
				array[i] = ((harvestDesignatable == null) ? null : harvestDesignatable.GetComponent<KPrefabID>());
			}
		}
		this.buds = null;
		return array;
	}

	// Token: 0x04005B37 RID: 23351
	[Serialize]
	private Ref<HarvestDesignatable>[] buds;
}
