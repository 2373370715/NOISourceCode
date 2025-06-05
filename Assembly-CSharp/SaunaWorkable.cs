using System;
using Klei;
using Klei.AI;
using TUNING;
using UnityEngine;

// Token: 0x0200185F RID: 6239
[AddComponentMenu("KMonoBehaviour/Workable/SaunaWorkable")]
public class SaunaWorkable : Workable, IWorkerPrioritizable
{
	// Token: 0x06008095 RID: 32917 RVA: 0x000B09A4 File Offset: 0x000AEBA4
	private SaunaWorkable()
	{
		base.SetReportType(ReportManager.ReportType.PersonalTime);
	}

	// Token: 0x06008096 RID: 32918 RVA: 0x003415AC File Offset: 0x0033F7AC
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.overrideAnims = new KAnimFile[]
		{
			Assets.GetAnim("anim_interacts_sauna_kanim")
		};
		this.showProgressBar = true;
		this.resetProgressOnStop = true;
		this.synchronizeAnims = true;
		this.workLayer = Grid.SceneLayer.BuildingUse;
		base.SetWorkTime(30f);
		this.sauna = base.GetComponent<Sauna>();
	}

	// Token: 0x06008097 RID: 32919 RVA: 0x000F933F File Offset: 0x000F753F
	protected override void OnStartWork(WorkerBase worker)
	{
		base.OnStartWork(worker);
		this.operational.SetActive(true, false);
		worker.GetComponent<Effects>().Add("SaunaRelaxing", false);
	}

	// Token: 0x06008098 RID: 32920 RVA: 0x00341614 File Offset: 0x0033F814
	protected override void OnCompleteWork(WorkerBase worker)
	{
		Effects component = worker.GetComponent<Effects>();
		if (!string.IsNullOrEmpty(this.sauna.specificEffect))
		{
			component.Add(this.sauna.specificEffect, true);
		}
		if (!string.IsNullOrEmpty(this.sauna.trackingEffect))
		{
			component.Add(this.sauna.trackingEffect, true);
		}
		component.Add("WarmTouch", true).timeRemaining = 1800f;
		this.operational.SetActive(false, false);
	}

	// Token: 0x06008099 RID: 32921 RVA: 0x00341698 File Offset: 0x0033F898
	protected override void OnStopWork(WorkerBase worker)
	{
		this.operational.SetActive(false, false);
		worker.GetComponent<Effects>().Remove("SaunaRelaxing");
		Storage component = base.GetComponent<Storage>();
		float num;
		SimUtil.DiseaseInfo diseaseInfo;
		float num2;
		component.ConsumeAndGetDisease(SimHashes.Steam.CreateTag(), this.sauna.steamPerUseKG, out num, out diseaseInfo, out num2);
		component.AddLiquid(SimHashes.Water, this.sauna.steamPerUseKG, this.sauna.waterOutputTemp, diseaseInfo.idx, diseaseInfo.count, true, false);
	}

	// Token: 0x0600809A RID: 32922 RVA: 0x00341718 File Offset: 0x0033F918
	public bool GetWorkerPriority(WorkerBase worker, out int priority)
	{
		priority = this.basePriority;
		Effects component = worker.GetComponent<Effects>();
		if (!string.IsNullOrEmpty(this.sauna.trackingEffect) && component.HasEffect(this.sauna.trackingEffect))
		{
			priority = 0;
			return false;
		}
		if (!string.IsNullOrEmpty(this.sauna.specificEffect) && component.HasEffect(this.sauna.specificEffect))
		{
			priority = RELAXATION.PRIORITY.RECENTLY_USED;
		}
		return true;
	}

	// Token: 0x040061D2 RID: 25042
	[MyCmpReq]
	private Operational operational;

	// Token: 0x040061D3 RID: 25043
	public int basePriority;

	// Token: 0x040061D4 RID: 25044
	private Sauna sauna;
}
