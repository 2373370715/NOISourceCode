using System;

// Token: 0x02000DF2 RID: 3570
public class ToggleGeothermalVentConnection : Toggleable
{
	// Token: 0x060045B2 RID: 17842 RVA: 0x0025AA7C File Offset: 0x00258C7C
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.SetWorkTime(10f);
		this.overrideAnims = new KAnimFile[]
		{
			Assets.GetAnim(GeothermalVentConfig.TOGGLE_ANIM_OVERRIDE)
		};
		this.workAnims = new HashedString[]
		{
			GeothermalVentConfig.TOGGLE_ANIMATION
		};
		this.workingPstComplete = null;
		this.workingPstFailed = null;
		this.workLayer = Grid.SceneLayer.Front;
		this.synchronizeAnims = false;
		this.workAnimPlayMode = KAnim.PlayMode.Once;
		base.SetOffsets(new CellOffset[]
		{
			CellOffset.none
		});
	}

	// Token: 0x060045B3 RID: 17843 RVA: 0x0025AB0C File Offset: 0x00258D0C
	protected override void OnStartWork(WorkerBase worker)
	{
		base.OnStartWork(worker);
		this.buildingAnimController.Play(GeothermalVentConfig.TOGGLE_ANIMATION, KAnim.PlayMode.Once, 1f, 0f);
		if (this.workerFacing == null || this.workerFacing.gameObject != worker.gameObject)
		{
			this.workerFacing = worker.GetComponent<Facing>();
		}
	}

	// Token: 0x060045B4 RID: 17844 RVA: 0x000D173A File Offset: 0x000CF93A
	protected override bool OnWorkTick(WorkerBase worker, float dt)
	{
		if (this.workerFacing != null)
		{
			this.workerFacing.Face(this.workerFacing.transform.GetLocalPosition().x + 0.5f);
		}
		return base.OnWorkTick(worker, dt);
	}

	// Token: 0x04003089 RID: 12425
	[MyCmpGet]
	private KBatchedAnimController buildingAnimController;

	// Token: 0x0400308A RID: 12426
	private Facing workerFacing;
}
