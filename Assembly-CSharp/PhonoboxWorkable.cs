using System;
using Klei.AI;
using TUNING;
using UnityEngine;

// Token: 0x020016F4 RID: 5876
[AddComponentMenu("KMonoBehaviour/Workable/PhonoboxWorkable")]
public class PhonoboxWorkable : Workable, IWorkerPrioritizable
{
	// Token: 0x0600792D RID: 31021 RVA: 0x003224F8 File Offset: 0x003206F8
	private PhonoboxWorkable()
	{
		base.SetReportType(ReportManager.ReportType.PersonalTime);
	}

	// Token: 0x0600792E RID: 31022 RVA: 0x000F415F File Offset: 0x000F235F
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.synchronizeAnims = false;
		this.showProgressBar = true;
		this.resetProgressOnStop = true;
		base.SetWorkTime(15f);
	}

	// Token: 0x0600792F RID: 31023 RVA: 0x00322594 File Offset: 0x00320794
	protected override void OnCompleteWork(WorkerBase worker)
	{
		Effects component = worker.GetComponent<Effects>();
		if (!string.IsNullOrEmpty(this.trackingEffect))
		{
			component.Add(this.trackingEffect, true);
		}
		if (!string.IsNullOrEmpty(this.specificEffect))
		{
			component.Add(this.specificEffect, true);
		}
	}

	// Token: 0x06007930 RID: 31024 RVA: 0x003225E0 File Offset: 0x003207E0
	public bool GetWorkerPriority(WorkerBase worker, out int priority)
	{
		priority = this.basePriority;
		Effects component = worker.GetComponent<Effects>();
		if (!string.IsNullOrEmpty(this.trackingEffect) && component.HasEffect(this.trackingEffect))
		{
			priority = 0;
			return false;
		}
		if (!string.IsNullOrEmpty(this.specificEffect) && component.HasEffect(this.specificEffect))
		{
			priority = RELAXATION.PRIORITY.RECENTLY_USED;
		}
		return true;
	}

	// Token: 0x06007931 RID: 31025 RVA: 0x000F4187 File Offset: 0x000F2387
	protected override void OnStartWork(WorkerBase worker)
	{
		this.owner.AddWorker(worker);
		worker.GetComponent<Effects>().Add("Dancing", false);
	}

	// Token: 0x06007932 RID: 31026 RVA: 0x000F41A7 File Offset: 0x000F23A7
	protected override void OnStopWork(WorkerBase worker)
	{
		this.owner.RemoveWorker(worker);
		worker.GetComponent<Effects>().Remove("Dancing");
	}

	// Token: 0x06007933 RID: 31027 RVA: 0x00322640 File Offset: 0x00320840
	public override Workable.AnimInfo GetAnim(WorkerBase worker)
	{
		int num = UnityEngine.Random.Range(0, this.workerOverrideAnims.Length);
		this.overrideAnims = this.workerOverrideAnims[num];
		return base.GetAnim(worker);
	}

	// Token: 0x04005AFD RID: 23293
	public Phonobox owner;

	// Token: 0x04005AFE RID: 23294
	public int basePriority = RELAXATION.PRIORITY.TIER3;

	// Token: 0x04005AFF RID: 23295
	public string specificEffect = "Danced";

	// Token: 0x04005B00 RID: 23296
	public string trackingEffect = "RecentlyDanced";

	// Token: 0x04005B01 RID: 23297
	public KAnimFile[][] workerOverrideAnims = new KAnimFile[][]
	{
		new KAnimFile[]
		{
			Assets.GetAnim("anim_interacts_phonobox_danceone_kanim")
		},
		new KAnimFile[]
		{
			Assets.GetAnim("anim_interacts_phonobox_dancetwo_kanim")
		},
		new KAnimFile[]
		{
			Assets.GetAnim("anim_interacts_phonobox_dancethree_kanim")
		}
	};
}
