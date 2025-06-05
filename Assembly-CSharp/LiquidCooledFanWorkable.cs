using System;
using UnityEngine;

// Token: 0x02000E5D RID: 3677
[AddComponentMenu("KMonoBehaviour/Workable/LiquidCooledFanWorkable")]
public class LiquidCooledFanWorkable : Workable
{
	// Token: 0x060047E2 RID: 18402 RVA: 0x000D27A7 File Offset: 0x000D09A7
	private LiquidCooledFanWorkable()
	{
		this.showProgressBar = false;
	}

	// Token: 0x060047E3 RID: 18403 RVA: 0x000D30AA File Offset: 0x000D12AA
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.workerStatusItem = null;
	}

	// Token: 0x060047E4 RID: 18404 RVA: 0x000D30B9 File Offset: 0x000D12B9
	protected override void OnSpawn()
	{
		GameScheduler.Instance.Schedule("InsulationTutorial", 2f, delegate(object obj)
		{
			Tutorial.Instance.TutorialMessage(Tutorial.TutorialMessages.TM_Insulation, true);
		}, null, null);
		base.OnSpawn();
	}

	// Token: 0x060047E5 RID: 18405 RVA: 0x000D30F7 File Offset: 0x000D12F7
	protected override void OnStartWork(WorkerBase worker)
	{
		this.operational.SetActive(true, false);
	}

	// Token: 0x060047E6 RID: 18406 RVA: 0x000D3106 File Offset: 0x000D1306
	protected override void OnStopWork(WorkerBase worker)
	{
		this.operational.SetActive(false, false);
	}

	// Token: 0x060047E7 RID: 18407 RVA: 0x000D3106 File Offset: 0x000D1306
	protected override void OnCompleteWork(WorkerBase worker)
	{
		this.operational.SetActive(false, false);
	}

	// Token: 0x0400326A RID: 12906
	[MyCmpGet]
	private Operational operational;
}
