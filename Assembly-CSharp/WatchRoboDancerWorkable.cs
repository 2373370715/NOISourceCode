using System;
using Klei.AI;
using TUNING;
using UnityEngine;

// Token: 0x0200107E RID: 4222
public class WatchRoboDancerWorkable : Workable, IWorkerPrioritizable
{
	// Token: 0x060055CA RID: 21962 RVA: 0x0028DE44 File Offset: 0x0028C044
	private WatchRoboDancerWorkable()
	{
		base.SetReportType(ReportManager.ReportType.PersonalTime);
	}

	// Token: 0x060055CB RID: 21963 RVA: 0x0028DEAC File Offset: 0x0028C0AC
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.synchronizeAnims = false;
		this.showProgressBar = true;
		this.resetProgressOnStop = true;
		this.workerStatusItem = Db.Get().DuplicantStatusItems.WatchRoboDancerWorkable;
		base.SetWorkTime(30f);
		this.showProgressBar = false;
	}

	// Token: 0x060055CC RID: 21964 RVA: 0x0028DEFC File Offset: 0x0028C0FC
	protected override void OnCompleteWork(WorkerBase worker)
	{
		Effects component = worker.GetComponent<Effects>();
		if (!string.IsNullOrEmpty(WatchRoboDancerWorkable.TRACKING_EFFECT))
		{
			component.Add(WatchRoboDancerWorkable.TRACKING_EFFECT, true);
		}
		if (!string.IsNullOrEmpty(WatchRoboDancerWorkable.SPECIFIC_EFFECT))
		{
			component.Add(WatchRoboDancerWorkable.SPECIFIC_EFFECT, true);
		}
	}

	// Token: 0x060055CD RID: 21965 RVA: 0x0028DF44 File Offset: 0x0028C144
	public bool GetWorkerPriority(WorkerBase worker, out int priority)
	{
		priority = this.basePriority;
		Effects component = worker.GetComponent<Effects>();
		if (!string.IsNullOrEmpty(WatchRoboDancerWorkable.TRACKING_EFFECT) && component.HasEffect(WatchRoboDancerWorkable.TRACKING_EFFECT))
		{
			priority = 0;
			return false;
		}
		if (!string.IsNullOrEmpty(WatchRoboDancerWorkable.SPECIFIC_EFFECT) && component.HasEffect(WatchRoboDancerWorkable.SPECIFIC_EFFECT))
		{
			priority = RELAXATION.PRIORITY.RECENTLY_USED;
		}
		return true;
	}

	// Token: 0x060055CE RID: 21966 RVA: 0x000DC6CD File Offset: 0x000DA8CD
	protected override void OnStartWork(WorkerBase worker)
	{
		worker.GetComponent<Effects>().Add("Dancing", false);
	}

	// Token: 0x060055CF RID: 21967 RVA: 0x000DC6E1 File Offset: 0x000DA8E1
	protected override bool OnWorkTick(WorkerBase worker, float dt)
	{
		worker.GetComponent<Facing>().Face(this.owner.transform.position.x);
		return base.OnWorkTick(worker, dt);
	}

	// Token: 0x060055D0 RID: 21968 RVA: 0x000DC70B File Offset: 0x000DA90B
	protected override void OnStopWork(WorkerBase worker)
	{
		worker.GetComponent<Effects>().Remove("Dancing");
		ChoreHelpers.DestroyLocator(base.gameObject);
	}

	// Token: 0x060055D1 RID: 21969 RVA: 0x0028DFA0 File Offset: 0x0028C1A0
	public override Workable.AnimInfo GetAnim(WorkerBase worker)
	{
		int num = UnityEngine.Random.Range(0, this.workerOverrideAnims.Length);
		this.overrideAnims = this.workerOverrideAnims[num];
		return base.GetAnim(worker);
	}

	// Token: 0x04003CBA RID: 15546
	public GameObject owner;

	// Token: 0x04003CBB RID: 15547
	public int basePriority = RELAXATION.PRIORITY.TIER3;

	// Token: 0x04003CBC RID: 15548
	public static string SPECIFIC_EFFECT = "SawRoboDancer";

	// Token: 0x04003CBD RID: 15549
	public static string TRACKING_EFFECT = "RecentlySawRoboDancer";

	// Token: 0x04003CBE RID: 15550
	public KAnimFile[][] workerOverrideAnims = new KAnimFile[][]
	{
		new KAnimFile[]
		{
			Assets.GetAnim("anim_interacts_robotdance_kanim")
		},
		new KAnimFile[]
		{
			Assets.GetAnim("anim_interacts_robotdance1_kanim")
		}
	};
}
