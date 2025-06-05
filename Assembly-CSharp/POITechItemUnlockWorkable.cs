using System;

// Token: 0x020016E9 RID: 5865
public class POITechItemUnlockWorkable : Workable
{
	// Token: 0x060078FD RID: 30973 RVA: 0x00321A80 File Offset: 0x0031FC80
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.workerStatusItem = Db.Get().DuplicantStatusItems.ResearchingFromPOI;
		this.alwaysShowProgressBar = true;
		this.resetProgressOnStop = false;
		this.overrideAnims = new KAnimFile[]
		{
			Assets.GetAnim("anim_interacts_research_unlock_kanim")
		};
		this.synchronizeAnims = true;
	}

	// Token: 0x060078FE RID: 30974 RVA: 0x00321ADC File Offset: 0x0031FCDC
	protected override void OnCompleteWork(WorkerBase worker)
	{
		base.OnCompleteWork(worker);
		POITechItemUnlocks.Instance smi = this.GetSMI<POITechItemUnlocks.Instance>();
		smi.UnlockTechItems();
		smi.sm.pendingChore.Set(false, smi, false);
		base.gameObject.Trigger(1980521255, null);
		Prioritizable.RemoveRef(base.gameObject);
	}
}
