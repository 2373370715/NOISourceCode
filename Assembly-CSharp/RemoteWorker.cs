using System;
using Klei.AI;
using UnityEngine;

// Token: 0x020017A9 RID: 6057
public class RemoteWorker : StandardWorker
{
	// Token: 0x06007C7C RID: 31868 RVA: 0x0032DB28 File Offset: 0x0032BD28
	public override Attributes GetAttributes()
	{
		RemoteWorkerDock homeDepot = this.remoteWorkerSM.HomeDepot;
		WorkerBase workerBase = ((homeDepot != null) ? homeDepot.GetActiveTerminalWorker() : null) ?? null;
		if (workerBase != null)
		{
			return workerBase.GetAttributes();
		}
		return null;
	}

	// Token: 0x06007C7D RID: 31869 RVA: 0x0032DB64 File Offset: 0x0032BD64
	public override AttributeConverterInstance GetAttributeConverter(string id)
	{
		RemoteWorkerDock homeDepot = this.remoteWorkerSM.HomeDepot;
		WorkerBase workerBase = ((homeDepot != null) ? homeDepot.GetActiveTerminalWorker() : null) ?? null;
		if (workerBase != null)
		{
			return workerBase.GetAttributeConverter(id);
		}
		return null;
	}

	// Token: 0x06007C7E RID: 31870 RVA: 0x000F65AF File Offset: 0x000F47AF
	protected override void TryPlayingIdle()
	{
		if (this.remoteWorkerSM.Docked)
		{
			base.GetComponent<KAnimControllerBase>().Play("in_dock_idle", KAnim.PlayMode.Once, 1f, 0f);
			return;
		}
		base.TryPlayingIdle();
	}

	// Token: 0x06007C7F RID: 31871 RVA: 0x0032DBA0 File Offset: 0x0032BDA0
	protected override void InternalStopWork(Workable target_workable, bool is_aborted)
	{
		base.InternalStopWork(target_workable, is_aborted);
		Vector3 position = base.transform.GetPosition();
		RemoteWorkerSM remoteWorkerSM = this.remoteWorkerSM;
		position.z = Grid.GetLayerZ((remoteWorkerSM != null && remoteWorkerSM.Docked) ? Grid.SceneLayer.BuildingUse : Grid.SceneLayer.Move);
		base.transform.SetPosition(position);
	}

	// Token: 0x04005DBD RID: 23997
	[MyCmpGet]
	private RemoteWorkerSM remoteWorkerSM;
}
