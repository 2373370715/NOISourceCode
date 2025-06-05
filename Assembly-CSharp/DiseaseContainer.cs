using System;
using UnityEngine;

// Token: 0x0200127C RID: 4732
public struct DiseaseContainer
{
	// Token: 0x06006093 RID: 24723 RVA: 0x002BC700 File Offset: 0x002BA900
	public DiseaseContainer(GameObject go, ushort elemIdx)
	{
		this.elemIdx = elemIdx;
		this.isContainer = (go.GetComponent<IUserControlledCapacity>() != null && go.GetComponent<Storage>() != null);
		Conduit component = go.GetComponent<Conduit>();
		if (component != null)
		{
			this.conduitType = component.type;
		}
		else
		{
			this.conduitType = ConduitType.None;
		}
		this.controller = go.GetComponent<KBatchedAnimController>();
		this.overpopulationCount = 1;
		this.instanceGrowthRate = 1f;
		this.accumulatedError = 0f;
		this.visualDiseaseProvider = null;
		this.autoDisinfectable = go.GetComponent<AutoDisinfectable>();
		if (this.autoDisinfectable != null)
		{
			AutoDisinfectableManager.Instance.AddAutoDisinfectable(this.autoDisinfectable);
		}
	}

	// Token: 0x06006094 RID: 24724 RVA: 0x000E3573 File Offset: 0x000E1773
	public void Clear()
	{
		this.controller = null;
	}

	// Token: 0x04004506 RID: 17670
	public AutoDisinfectable autoDisinfectable;

	// Token: 0x04004507 RID: 17671
	public ushort elemIdx;

	// Token: 0x04004508 RID: 17672
	public bool isContainer;

	// Token: 0x04004509 RID: 17673
	public ConduitType conduitType;

	// Token: 0x0400450A RID: 17674
	public KBatchedAnimController controller;

	// Token: 0x0400450B RID: 17675
	public GameObject visualDiseaseProvider;

	// Token: 0x0400450C RID: 17676
	public int overpopulationCount;

	// Token: 0x0400450D RID: 17677
	public float instanceGrowthRate;

	// Token: 0x0400450E RID: 17678
	public float accumulatedError;
}
