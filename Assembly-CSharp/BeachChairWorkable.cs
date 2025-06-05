using System;
using Klei.AI;
using TUNING;
using UnityEngine;

// Token: 0x02000C7A RID: 3194
[AddComponentMenu("KMonoBehaviour/Workable/BeachChairWorkable")]
public class BeachChairWorkable : Workable, IWorkerPrioritizable
{
	// Token: 0x06003CA4 RID: 15524 RVA: 0x0023CC34 File Offset: 0x0023AE34
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.SetReportType(ReportManager.ReportType.PersonalTime);
		this.overrideAnims = new KAnimFile[]
		{
			Assets.GetAnim("anim_interacts_beach_chair_kanim")
		};
		this.workAnims = null;
		this.workingPstComplete = null;
		this.workingPstFailed = null;
		this.showProgressBar = true;
		this.resetProgressOnStop = true;
		this.synchronizeAnims = false;
		this.lightEfficiencyBonus = false;
		base.SetWorkTime(150f);
		this.beachChair = base.GetComponent<BeachChair>();
	}

	// Token: 0x06003CA5 RID: 15525 RVA: 0x000CB9E2 File Offset: 0x000C9BE2
	protected override void OnStartWork(WorkerBase worker)
	{
		this.timeLit = 0f;
		this.beachChair.SetWorker(worker);
		this.operational.SetActive(true, false);
		worker.GetComponent<Effects>().Add("BeachChairRelaxing", false);
	}

	// Token: 0x06003CA6 RID: 15526 RVA: 0x0023CCB8 File Offset: 0x0023AEB8
	protected override bool OnWorkTick(WorkerBase worker, float dt)
	{
		int i = Grid.PosToCell(base.gameObject);
		bool flag = (float)Grid.LightIntensity[i] >= (float)BeachChairConfig.TAN_LUX - 1f;
		this.beachChair.SetLit(flag);
		if (flag)
		{
			base.GetComponent<LoopingSounds>().SetParameter(this.soundPath, this.BEACH_CHAIR_LIT_PARAMETER, 1f);
			this.timeLit += dt;
		}
		else
		{
			base.GetComponent<LoopingSounds>().SetParameter(this.soundPath, this.BEACH_CHAIR_LIT_PARAMETER, 0f);
		}
		return false;
	}

	// Token: 0x06003CA7 RID: 15527 RVA: 0x0023CD48 File Offset: 0x0023AF48
	protected override void OnCompleteWork(WorkerBase worker)
	{
		Effects component = worker.GetComponent<Effects>();
		if (this.timeLit / this.workTime >= 0.75f)
		{
			component.Add(this.beachChair.specificEffectLit, true);
			component.Remove(this.beachChair.specificEffectUnlit);
		}
		else
		{
			component.Add(this.beachChair.specificEffectUnlit, true);
			component.Remove(this.beachChair.specificEffectLit);
		}
		component.Add(this.beachChair.trackingEffect, true);
	}

	// Token: 0x06003CA8 RID: 15528 RVA: 0x000CBA1A File Offset: 0x000C9C1A
	protected override void OnStopWork(WorkerBase worker)
	{
		this.operational.SetActive(false, false);
		worker.GetComponent<Effects>().Remove("BeachChairRelaxing");
	}

	// Token: 0x06003CA9 RID: 15529 RVA: 0x0023CDD0 File Offset: 0x0023AFD0
	public bool GetWorkerPriority(WorkerBase worker, out int priority)
	{
		priority = this.basePriority;
		Effects component = worker.GetComponent<Effects>();
		if (component.HasEffect(this.beachChair.trackingEffect))
		{
			priority = 0;
			return false;
		}
		if (component.HasEffect(this.beachChair.specificEffectLit) || component.HasEffect(this.beachChair.specificEffectUnlit))
		{
			priority = RELAXATION.PRIORITY.RECENTLY_USED;
		}
		return true;
	}

	// Token: 0x04002A15 RID: 10773
	[MyCmpReq]
	private Operational operational;

	// Token: 0x04002A16 RID: 10774
	private float timeLit;

	// Token: 0x04002A17 RID: 10775
	public string soundPath = GlobalAssets.GetSound("BeachChair_music_lp", false);

	// Token: 0x04002A18 RID: 10776
	public HashedString BEACH_CHAIR_LIT_PARAMETER = "beachChair_lit";

	// Token: 0x04002A19 RID: 10777
	public int basePriority;

	// Token: 0x04002A1A RID: 10778
	private BeachChair beachChair;
}
