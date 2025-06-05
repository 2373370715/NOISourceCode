using System;
using Klei;
using Klei.AI;
using TUNING;
using UnityEngine;

// Token: 0x0200150C RID: 5388
[AddComponentMenu("KMonoBehaviour/Workable/MechanicalSurfboardWorkable")]
public class MechanicalSurfboardWorkable : Workable, IWorkerPrioritizable
{
	// Token: 0x06007015 RID: 28693 RVA: 0x000B09A4 File Offset: 0x000AEBA4
	private MechanicalSurfboardWorkable()
	{
		base.SetReportType(ReportManager.ReportType.PersonalTime);
	}

	// Token: 0x06007016 RID: 28694 RVA: 0x000EDDF6 File Offset: 0x000EBFF6
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.showProgressBar = true;
		this.resetProgressOnStop = true;
		this.synchronizeAnims = true;
		base.SetWorkTime(30f);
		this.surfboard = base.GetComponent<MechanicalSurfboard>();
	}

	// Token: 0x06007017 RID: 28695 RVA: 0x000EDE2A File Offset: 0x000EC02A
	protected override void OnStartWork(WorkerBase worker)
	{
		this.operational.SetActive(true, false);
		worker.GetComponent<Effects>().Add("MechanicalSurfing", false);
	}

	// Token: 0x06007018 RID: 28696 RVA: 0x00302F64 File Offset: 0x00301164
	public override Workable.AnimInfo GetAnim(WorkerBase worker)
	{
		Workable.AnimInfo result = default(Workable.AnimInfo);
		AttributeInstance attributeInstance = worker.GetAttributes().Get(Db.Get().Attributes.Athletics);
		if (attributeInstance.GetTotalValue() <= 7f)
		{
			result.overrideAnims = new KAnimFile[]
			{
				Assets.GetAnim(this.surfboard.interactAnims[0])
			};
		}
		else if (attributeInstance.GetTotalValue() <= 15f)
		{
			result.overrideAnims = new KAnimFile[]
			{
				Assets.GetAnim(this.surfboard.interactAnims[1])
			};
		}
		else
		{
			result.overrideAnims = new KAnimFile[]
			{
				Assets.GetAnim(this.surfboard.interactAnims[2])
			};
		}
		return result;
	}

	// Token: 0x06007019 RID: 28697 RVA: 0x00303028 File Offset: 0x00301228
	protected override bool OnWorkTick(WorkerBase worker, float dt)
	{
		Building component = base.GetComponent<Building>();
		MechanicalSurfboard component2 = base.GetComponent<MechanicalSurfboard>();
		int widthInCells = component.Def.WidthInCells;
		int minInclusive = -(widthInCells - 1) / 2;
		int maxExclusive = widthInCells / 2;
		int x = UnityEngine.Random.Range(minInclusive, maxExclusive);
		float amount = component2.waterSpillRateKG * dt;
		float base_mass;
		SimUtil.DiseaseInfo diseaseInfo;
		float temperature;
		base.GetComponent<Storage>().ConsumeAndGetDisease(SimHashes.Water.CreateTag(), amount, out base_mass, out diseaseInfo, out temperature);
		int cell = Grid.OffsetCell(Grid.PosToCell(base.gameObject), new CellOffset(x, 0));
		ushort elementIndex = ElementLoader.GetElementIndex(SimHashes.Water);
		FallingWater.instance.AddParticle(cell, elementIndex, base_mass, temperature, diseaseInfo.idx, diseaseInfo.count, true, false, false, false);
		return false;
	}

	// Token: 0x0600701A RID: 28698 RVA: 0x003030D0 File Offset: 0x003012D0
	protected override void OnCompleteWork(WorkerBase worker)
	{
		Effects component = worker.GetComponent<Effects>();
		if (!string.IsNullOrEmpty(this.surfboard.specificEffect))
		{
			component.Add(this.surfboard.specificEffect, true);
		}
		if (!string.IsNullOrEmpty(this.surfboard.trackingEffect))
		{
			component.Add(this.surfboard.trackingEffect, true);
		}
	}

	// Token: 0x0600701B RID: 28699 RVA: 0x000EDE4B File Offset: 0x000EC04B
	protected override void OnStopWork(WorkerBase worker)
	{
		this.operational.SetActive(false, false);
		worker.GetComponent<Effects>().Remove("MechanicalSurfing");
	}

	// Token: 0x0600701C RID: 28700 RVA: 0x00303130 File Offset: 0x00301330
	public bool GetWorkerPriority(WorkerBase worker, out int priority)
	{
		priority = this.basePriority;
		Effects component = worker.GetComponent<Effects>();
		if (!string.IsNullOrEmpty(this.surfboard.trackingEffect) && component.HasEffect(this.surfboard.trackingEffect))
		{
			priority = 0;
			return false;
		}
		if (!string.IsNullOrEmpty(this.surfboard.specificEffect) && component.HasEffect(this.surfboard.specificEffect))
		{
			priority = RELAXATION.PRIORITY.RECENTLY_USED;
		}
		return true;
	}

	// Token: 0x04005428 RID: 21544
	[MyCmpReq]
	private Operational operational;

	// Token: 0x04005429 RID: 21545
	public int basePriority;

	// Token: 0x0400542A RID: 21546
	private MechanicalSurfboard surfboard;
}
