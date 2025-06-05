using System;
using Klei.AI;
using TUNING;
using UnityEngine;

// Token: 0x02001A7F RID: 6783
[AddComponentMenu("KMonoBehaviour/Workable/VerticalWindTunnelWorkable")]
public class VerticalWindTunnelWorkable : Workable, IWorkerPrioritizable
{
	// Token: 0x06008D74 RID: 36212 RVA: 0x000B09A4 File Offset: 0x000AEBA4
	private VerticalWindTunnelWorkable()
	{
		base.SetReportType(ReportManager.ReportType.PersonalTime);
	}

	// Token: 0x06008D75 RID: 36213 RVA: 0x003764C0 File Offset: 0x003746C0
	public override Workable.AnimInfo GetAnim(WorkerBase worker)
	{
		Workable.AnimInfo anim = base.GetAnim(worker);
		anim.smi = new WindTunnelWorkerStateMachine.StatesInstance(worker, this);
		return anim;
	}

	// Token: 0x06008D76 RID: 36214 RVA: 0x00100E76 File Offset: 0x000FF076
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.synchronizeAnims = false;
		this.showProgressBar = true;
		this.resetProgressOnStop = true;
		base.SetWorkTime(90f);
	}

	// Token: 0x06008D77 RID: 36215 RVA: 0x00100E9E File Offset: 0x000FF09E
	protected override void OnStartWork(WorkerBase worker)
	{
		base.OnStartWork(worker);
		worker.GetComponent<Effects>().Add("VerticalWindTunnelFlying", false);
	}

	// Token: 0x06008D78 RID: 36216 RVA: 0x00100EB9 File Offset: 0x000FF0B9
	protected override void OnStopWork(WorkerBase worker)
	{
		base.OnStopWork(worker);
		worker.GetComponent<Effects>().Remove("VerticalWindTunnelFlying");
	}

	// Token: 0x06008D79 RID: 36217 RVA: 0x00100ED2 File Offset: 0x000FF0D2
	protected override void OnCompleteWork(WorkerBase worker)
	{
		Effects component = worker.GetComponent<Effects>();
		component.Add(this.windTunnel.trackingEffect, true);
		component.Add(this.windTunnel.specificEffect, true);
	}

	// Token: 0x06008D7A RID: 36218 RVA: 0x003764E4 File Offset: 0x003746E4
	public bool GetWorkerPriority(WorkerBase worker, out int priority)
	{
		priority = this.windTunnel.basePriority;
		Effects component = worker.GetComponent<Effects>();
		if (component.HasEffect(this.windTunnel.trackingEffect))
		{
			priority = 0;
			return false;
		}
		if (component.HasEffect(this.windTunnel.specificEffect))
		{
			priority = RELAXATION.PRIORITY.RECENTLY_USED;
		}
		return true;
	}

	// Token: 0x04006ABB RID: 27323
	public VerticalWindTunnel windTunnel;

	// Token: 0x04006ABC RID: 27324
	public HashedString overrideAnim;

	// Token: 0x04006ABD RID: 27325
	public string[] preAnims;

	// Token: 0x04006ABE RID: 27326
	public string loopAnim;

	// Token: 0x04006ABF RID: 27327
	public string[] pstAnims;
}
