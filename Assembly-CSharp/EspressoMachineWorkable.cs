using System;
using System.Collections.Generic;
using Klei;
using Klei.AI;
using TUNING;
using UnityEngine;

// Token: 0x02001302 RID: 4866
[AddComponentMenu("KMonoBehaviour/Workable/EspressoMachineWorkable")]
public class EspressoMachineWorkable : Workable, IWorkerPrioritizable
{
	// Token: 0x060063C6 RID: 25542 RVA: 0x000E5983 File Offset: 0x000E3B83
	private EspressoMachineWorkable()
	{
		base.SetReportType(ReportManager.ReportType.PersonalTime);
	}

	// Token: 0x060063C7 RID: 25543 RVA: 0x002C972C File Offset: 0x002C792C
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.overrideAnims = new KAnimFile[]
		{
			Assets.GetAnim("anim_interacts_espresso_machine_kanim")
		};
		this.showProgressBar = true;
		this.resetProgressOnStop = true;
		this.synchronizeAnims = false;
		base.SetWorkTime(30f);
	}

	// Token: 0x060063C8 RID: 25544 RVA: 0x002C9780 File Offset: 0x002C7980
	public override Workable.AnimInfo GetAnim(WorkerBase worker)
	{
		KAnimFile[] overrideAnims = null;
		if (this.workerTypeOverrideAnims.TryGetValue(worker.PrefabID(), out overrideAnims))
		{
			this.overrideAnims = overrideAnims;
		}
		return base.GetAnim(worker);
	}

	// Token: 0x060063C9 RID: 25545 RVA: 0x000E59A9 File Offset: 0x000E3BA9
	protected override void OnStartWork(WorkerBase worker)
	{
		this.operational.SetActive(true, false);
	}

	// Token: 0x060063CA RID: 25546 RVA: 0x002C97B4 File Offset: 0x002C79B4
	protected override void OnCompleteWork(WorkerBase worker)
	{
		Storage component = base.GetComponent<Storage>();
		float num;
		SimUtil.DiseaseInfo diseaseInfo;
		float num2;
		component.ConsumeAndGetDisease(GameTags.Water, EspressoMachine.WATER_MASS_PER_USE, out num, out diseaseInfo, out num2);
		SimUtil.DiseaseInfo diseaseInfo2;
		component.ConsumeAndGetDisease(EspressoMachine.INGREDIENT_TAG, EspressoMachine.INGREDIENT_MASS_PER_USE, out num, out diseaseInfo2, out num2);
		GermExposureMonitor.Instance smi = worker.GetSMI<GermExposureMonitor.Instance>();
		if (smi != null)
		{
			smi.TryInjectDisease(diseaseInfo.idx, diseaseInfo.count, GameTags.Water, Sickness.InfectionVector.Digestion);
			smi.TryInjectDisease(diseaseInfo2.idx, diseaseInfo2.count, EspressoMachine.INGREDIENT_TAG, Sickness.InfectionVector.Digestion);
		}
		Effects component2 = worker.GetComponent<Effects>();
		if (!string.IsNullOrEmpty(EspressoMachineWorkable.specificEffect))
		{
			component2.Add(EspressoMachineWorkable.specificEffect, true);
		}
		if (!string.IsNullOrEmpty(EspressoMachineWorkable.trackingEffect))
		{
			component2.Add(EspressoMachineWorkable.trackingEffect, true);
		}
	}

	// Token: 0x060063CB RID: 25547 RVA: 0x000E59B8 File Offset: 0x000E3BB8
	protected override void OnStopWork(WorkerBase worker)
	{
		this.operational.SetActive(false, false);
	}

	// Token: 0x060063CC RID: 25548 RVA: 0x002C986C File Offset: 0x002C7A6C
	public bool GetWorkerPriority(WorkerBase worker, out int priority)
	{
		priority = this.basePriority;
		Effects component = worker.GetComponent<Effects>();
		if (!string.IsNullOrEmpty(EspressoMachineWorkable.trackingEffect) && component.HasEffect(EspressoMachineWorkable.trackingEffect))
		{
			priority = 0;
			return false;
		}
		if (!string.IsNullOrEmpty(EspressoMachineWorkable.specificEffect) && component.HasEffect(EspressoMachineWorkable.specificEffect))
		{
			priority = RELAXATION.PRIORITY.RECENTLY_USED;
		}
		return true;
	}

	// Token: 0x04004781 RID: 18305
	public Dictionary<Tag, KAnimFile[]> workerTypeOverrideAnims = new Dictionary<Tag, KAnimFile[]>();

	// Token: 0x04004782 RID: 18306
	[MyCmpReq]
	private Operational operational;

	// Token: 0x04004783 RID: 18307
	public int basePriority = RELAXATION.PRIORITY.TIER5;

	// Token: 0x04004784 RID: 18308
	private static string specificEffect = "Espresso";

	// Token: 0x04004785 RID: 18309
	private static string trackingEffect = "RecentlyRecDrink";
}
