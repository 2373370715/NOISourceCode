using System;

// Token: 0x0200102B RID: 4139
public class TeleporterWorkableUse : Workable
{
	// Token: 0x060053B0 RID: 21424 RVA: 0x000C1333 File Offset: 0x000BF533
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
	}

	// Token: 0x060053B1 RID: 21425 RVA: 0x000DAF29 File Offset: 0x000D9129
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.SetWorkTime(5f);
		this.resetProgressOnStop = true;
	}

	// Token: 0x060053B2 RID: 21426 RVA: 0x00287544 File Offset: 0x00285744
	protected override void OnStartWork(WorkerBase worker)
	{
		Teleporter component = base.GetComponent<Teleporter>();
		Teleporter teleporter = component.FindTeleportTarget();
		component.SetTeleportTarget(teleporter);
		TeleportalPad.StatesInstance smi = teleporter.GetSMI<TeleportalPad.StatesInstance>();
		smi.sm.targetTeleporter.Trigger(smi);
	}

	// Token: 0x060053B3 RID: 21427 RVA: 0x0028757C File Offset: 0x0028577C
	protected override void OnStopWork(WorkerBase worker)
	{
		TeleportalPad.StatesInstance smi = this.GetSMI<TeleportalPad.StatesInstance>();
		smi.sm.doTeleport.Trigger(smi);
	}
}
