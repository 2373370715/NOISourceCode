using System;
using UnityEngine;

// Token: 0x02000D1F RID: 3359
[AddComponentMenu("KMonoBehaviour/Workable/CommandModuleWorkable")]
public class CommandModuleWorkable : Workable
{
	// Token: 0x0600409B RID: 16539 RVA: 0x0024957C File Offset: 0x0024777C
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.SetOffsets(CommandModuleWorkable.entryOffsets);
		this.synchronizeAnims = false;
		this.overrideAnims = new KAnimFile[]
		{
			Assets.GetAnim("anim_interacts_incubator_kanim")
		};
		base.SetWorkTime(float.PositiveInfinity);
		this.showProgressBar = false;
	}

	// Token: 0x0600409C RID: 16540 RVA: 0x000AF929 File Offset: 0x000ADB29
	protected override void OnStartWork(WorkerBase worker)
	{
		base.OnStartWork(worker);
	}

	// Token: 0x0600409D RID: 16541 RVA: 0x002495D4 File Offset: 0x002477D4
	protected override bool OnWorkTick(WorkerBase worker, float dt)
	{
		if (!(worker != null))
		{
			return base.OnWorkTick(worker, dt);
		}
		if (DlcManager.IsExpansion1Active())
		{
			GameObject gameObject = worker.gameObject;
			base.CompleteWork(worker);
			base.GetComponent<ClustercraftExteriorDoor>().FerryMinion(gameObject);
			return true;
		}
		GameObject gameObject2 = worker.gameObject;
		base.CompleteWork(worker);
		base.GetComponent<MinionStorage>().SerializeMinion(gameObject2);
		return true;
	}

	// Token: 0x0600409E RID: 16542 RVA: 0x000CE5A0 File Offset: 0x000CC7A0
	protected override void OnStopWork(WorkerBase worker)
	{
		base.OnStopWork(worker);
	}

	// Token: 0x0600409F RID: 16543 RVA: 0x000AA038 File Offset: 0x000A8238
	protected override void OnCompleteWork(WorkerBase worker)
	{
	}

	// Token: 0x04002CBF RID: 11455
	private static CellOffset[] entryOffsets = new CellOffset[]
	{
		new CellOffset(0, 0),
		new CellOffset(0, 1),
		new CellOffset(0, 2),
		new CellOffset(0, 3),
		new CellOffset(0, 4)
	};
}
