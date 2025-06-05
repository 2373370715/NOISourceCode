using System;
using Klei.AI;
using TUNING;
using UnityEngine;

// Token: 0x02000C56 RID: 3158
[AddComponentMenu("KMonoBehaviour/Workable/ArcadeMachineWorkable")]
public class ArcadeMachineWorkable : Workable, IWorkerPrioritizable
{
	// Token: 0x06003BA2 RID: 15266 RVA: 0x000CAF39 File Offset: 0x000C9139
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.SetReportType(ReportManager.ReportType.PersonalTime);
		this.synchronizeAnims = false;
		this.showProgressBar = true;
		this.resetProgressOnStop = true;
		base.SetWorkTime(15f);
	}

	// Token: 0x06003BA3 RID: 15267 RVA: 0x000CAF69 File Offset: 0x000C9169
	protected override void OnStartWork(WorkerBase worker)
	{
		base.OnStartWork(worker);
		worker.GetComponent<Effects>().Add("ArcadePlaying", false);
	}

	// Token: 0x06003BA4 RID: 15268 RVA: 0x000CAF84 File Offset: 0x000C9184
	protected override void OnStopWork(WorkerBase worker)
	{
		base.OnStopWork(worker);
		worker.GetComponent<Effects>().Remove("ArcadePlaying");
	}

	// Token: 0x06003BA5 RID: 15269 RVA: 0x002394EC File Offset: 0x002376EC
	protected override void OnCompleteWork(WorkerBase worker)
	{
		Effects component = worker.GetComponent<Effects>();
		if (!string.IsNullOrEmpty(ArcadeMachineWorkable.trackingEffect))
		{
			component.Add(ArcadeMachineWorkable.trackingEffect, true);
		}
		if (!string.IsNullOrEmpty(ArcadeMachineWorkable.specificEffect))
		{
			component.Add(ArcadeMachineWorkable.specificEffect, true);
		}
	}

	// Token: 0x06003BA6 RID: 15270 RVA: 0x00239534 File Offset: 0x00237734
	public bool GetWorkerPriority(WorkerBase worker, out int priority)
	{
		priority = this.basePriority;
		Effects component = worker.GetComponent<Effects>();
		if (!string.IsNullOrEmpty(ArcadeMachineWorkable.trackingEffect) && component.HasEffect(ArcadeMachineWorkable.trackingEffect))
		{
			priority = 0;
			return false;
		}
		if (!string.IsNullOrEmpty(ArcadeMachineWorkable.specificEffect) && component.HasEffect(ArcadeMachineWorkable.specificEffect))
		{
			priority = RELAXATION.PRIORITY.RECENTLY_USED;
		}
		return true;
	}

	// Token: 0x0400295D RID: 10589
	public ArcadeMachine owner;

	// Token: 0x0400295E RID: 10590
	public int basePriority = RELAXATION.PRIORITY.TIER3;

	// Token: 0x0400295F RID: 10591
	private static string specificEffect = "PlayedArcade";

	// Token: 0x04002960 RID: 10592
	private static string trackingEffect = "RecentlyPlayedArcade";
}
