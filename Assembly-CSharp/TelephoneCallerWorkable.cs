using System;
using Klei.AI;
using TUNING;
using UnityEngine;

// Token: 0x02001A36 RID: 6710
[AddComponentMenu("KMonoBehaviour/Workable/TelephoneWorkable")]
public class TelephoneCallerWorkable : Workable, IWorkerPrioritizable
{
	// Token: 0x06008BC8 RID: 35784 RVA: 0x0036ED68 File Offset: 0x0036CF68
	private TelephoneCallerWorkable()
	{
		base.SetReportType(ReportManager.ReportType.PersonalTime);
		this.workingPstComplete = new HashedString[]
		{
			"on_pst"
		};
		this.workAnims = new HashedString[]
		{
			"on_pre",
			"on",
			"on_receiving",
			"on_pre_loop_receiving",
			"on_loop",
			"on_loop_pre"
		};
	}

	// Token: 0x06008BC9 RID: 35785 RVA: 0x0036EE14 File Offset: 0x0036D014
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.overrideAnims = new KAnimFile[]
		{
			Assets.GetAnim("anim_interacts_telephone_kanim")
		};
		this.showProgressBar = true;
		this.resetProgressOnStop = true;
		this.synchronizeAnims = true;
		base.SetWorkTime(40f);
		this.telephone = base.GetComponent<Telephone>();
	}

	// Token: 0x06008BCA RID: 35786 RVA: 0x000FFFA1 File Offset: 0x000FE1A1
	protected override void OnStartWork(WorkerBase worker)
	{
		this.operational.SetActive(true, false);
		this.telephone.isInUse = true;
	}

	// Token: 0x06008BCB RID: 35787 RVA: 0x0036EE74 File Offset: 0x0036D074
	protected override void OnCompleteWork(WorkerBase worker)
	{
		Effects component = worker.GetComponent<Effects>();
		if (this.telephone.HasTag(GameTags.LongDistanceCall))
		{
			if (!string.IsNullOrEmpty(this.telephone.longDistanceEffect))
			{
				component.Add(this.telephone.longDistanceEffect, true);
			}
		}
		else if (this.telephone.wasAnswered)
		{
			if (!string.IsNullOrEmpty(this.telephone.chatEffect))
			{
				component.Add(this.telephone.chatEffect, true);
			}
		}
		else if (!string.IsNullOrEmpty(this.telephone.babbleEffect))
		{
			component.Add(this.telephone.babbleEffect, true);
		}
		if (!string.IsNullOrEmpty(this.telephone.trackingEffect))
		{
			component.Add(this.telephone.trackingEffect, true);
		}
	}

	// Token: 0x06008BCC RID: 35788 RVA: 0x000FFFBC File Offset: 0x000FE1BC
	protected override void OnStopWork(WorkerBase worker)
	{
		this.operational.SetActive(false, false);
		this.telephone.HangUp();
	}

	// Token: 0x06008BCD RID: 35789 RVA: 0x0036EF40 File Offset: 0x0036D140
	public bool GetWorkerPriority(WorkerBase worker, out int priority)
	{
		priority = this.basePriority;
		Effects component = worker.GetComponent<Effects>();
		if (!string.IsNullOrEmpty(this.telephone.trackingEffect) && component.HasEffect(this.telephone.trackingEffect))
		{
			priority = 0;
			return false;
		}
		if (!string.IsNullOrEmpty(this.telephone.chatEffect) && component.HasEffect(this.telephone.chatEffect))
		{
			priority = RELAXATION.PRIORITY.RECENTLY_USED;
		}
		if (!string.IsNullOrEmpty(this.telephone.babbleEffect) && component.HasEffect(this.telephone.babbleEffect))
		{
			priority = RELAXATION.PRIORITY.RECENTLY_USED;
		}
		return true;
	}

	// Token: 0x0400697B RID: 27003
	[MyCmpReq]
	private Operational operational;

	// Token: 0x0400697C RID: 27004
	public int basePriority;

	// Token: 0x0400697D RID: 27005
	private Telephone telephone;
}
