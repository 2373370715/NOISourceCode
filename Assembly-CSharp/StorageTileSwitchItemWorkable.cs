using System;

// Token: 0x02001002 RID: 4098
public class StorageTileSwitchItemWorkable : Workable
{
	// Token: 0x170004B0 RID: 1200
	// (get) Token: 0x060052A0 RID: 21152 RVA: 0x000DA492 File Offset: 0x000D8692
	// (set) Token: 0x0600529F RID: 21151 RVA: 0x000DA489 File Offset: 0x000D8689
	public int LastCellWorkerUsed { get; private set; } = -1;

	// Token: 0x060052A1 RID: 21153 RVA: 0x000DA49A File Offset: 0x000D869A
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.overrideAnims = new KAnimFile[]
		{
			Assets.GetAnim("anim_use_remote_kanim")
		};
		base.SetOffsetTable(OffsetGroups.InvertedStandardTable);
		this.faceTargetWhenWorking = true;
		this.synchronizeAnims = false;
	}

	// Token: 0x060052A2 RID: 21154 RVA: 0x000D1386 File Offset: 0x000CF586
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.SetWorkTime(3f);
	}

	// Token: 0x060052A3 RID: 21155 RVA: 0x000DA4D9 File Offset: 0x000D86D9
	protected override void OnCompleteWork(WorkerBase worker)
	{
		if (worker != null)
		{
			this.LastCellWorkerUsed = Grid.PosToCell(worker.transform.GetPosition());
		}
		base.OnCompleteWork(worker);
	}

	// Token: 0x04003A5B RID: 14939
	private const string animName = "anim_use_remote_kanim";
}
