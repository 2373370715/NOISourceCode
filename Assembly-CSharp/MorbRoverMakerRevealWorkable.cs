using System;

// Token: 0x020004D3 RID: 1235
public class MorbRoverMakerRevealWorkable : Workable
{
	// Token: 0x0600153A RID: 5434 RVA: 0x0019DE44 File Offset: 0x0019C044
	protected override void OnPrefabInit()
	{
		this.workAnims = new HashedString[]
		{
			"reveal_working_pre",
			"reveal_working_loop"
		};
		this.workingPstComplete = new HashedString[]
		{
			"reveal_working_pst"
		};
		this.workingPstFailed = new HashedString[]
		{
			"reveal_working_pst"
		};
		base.OnPrefabInit();
		this.workingStatusItem = Db.Get().BuildingStatusItems.MorbRoverMakerBuildingRevealed;
		base.SetWorkerStatusItem(Db.Get().DuplicantStatusItems.MorbRoverMakerWorkingOnRevealing);
		this.overrideAnims = new KAnimFile[]
		{
			Assets.GetAnim("anim_interacts_gravitas_morb_tank_kanim")
		};
		this.lightEfficiencyBonus = true;
		this.synchronizeAnims = true;
		base.SetWorkTime(15f);
	}

	// Token: 0x0600153B RID: 5435 RVA: 0x000AF929 File Offset: 0x000ADB29
	protected override void OnStartWork(WorkerBase worker)
	{
		base.OnStartWork(worker);
	}

	// Token: 0x04000E94 RID: 3732
	public const string WORKABLE_PRE_ANIM_NAME = "reveal_working_pre";

	// Token: 0x04000E95 RID: 3733
	public const string WORKABLE_LOOP_ANIM_NAME = "reveal_working_loop";

	// Token: 0x04000E96 RID: 3734
	public const string WORKABLE_PST_ANIM_NAME = "reveal_working_pst";
}
