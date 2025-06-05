using System;
using Klei.AI;
using TUNING;
using UnityEngine;

// Token: 0x0200143B RID: 5179
[AddComponentMenu("KMonoBehaviour/Workable/HotTubWorkable")]
public class HotTubWorkable : Workable, IWorkerPrioritizable
{
	// Token: 0x06006A3B RID: 27195 RVA: 0x000B09A4 File Offset: 0x000AEBA4
	private HotTubWorkable()
	{
		base.SetReportType(ReportManager.ReportType.PersonalTime);
	}

	// Token: 0x06006A3C RID: 27196 RVA: 0x000EA155 File Offset: 0x000E8355
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.synchronizeAnims = false;
		this.showProgressBar = true;
		this.resetProgressOnStop = true;
		this.faceTargetWhenWorking = true;
		base.SetWorkTime(90f);
	}

	// Token: 0x06006A3D RID: 27197 RVA: 0x002EBA2C File Offset: 0x002E9C2C
	public override Workable.AnimInfo GetAnim(WorkerBase worker)
	{
		Workable.AnimInfo anim = base.GetAnim(worker);
		anim.smi = new HotTubWorkerStateMachine.StatesInstance(worker);
		return anim;
	}

	// Token: 0x06006A3E RID: 27198 RVA: 0x000EA184 File Offset: 0x000E8384
	protected override void OnStartWork(WorkerBase worker)
	{
		this.faceLeft = (UnityEngine.Random.value > 0.5f);
		worker.GetComponent<Effects>().Add("HotTubRelaxing", false);
	}

	// Token: 0x06006A3F RID: 27199 RVA: 0x000EA1AE File Offset: 0x000E83AE
	protected override void OnStopWork(WorkerBase worker)
	{
		worker.GetComponent<Effects>().Remove("HotTubRelaxing");
	}

	// Token: 0x06006A40 RID: 27200 RVA: 0x000EA1C0 File Offset: 0x000E83C0
	public override Vector3 GetFacingTarget()
	{
		return base.transform.GetPosition() + (this.faceLeft ? Vector3.left : Vector3.right);
	}

	// Token: 0x06006A41 RID: 27201 RVA: 0x002EBA50 File Offset: 0x002E9C50
	protected override void OnCompleteWork(WorkerBase worker)
	{
		Effects component = worker.GetComponent<Effects>();
		if (!string.IsNullOrEmpty(this.hotTub.trackingEffect))
		{
			component.Add(this.hotTub.trackingEffect, true);
		}
		if (!string.IsNullOrEmpty(this.hotTub.specificEffect))
		{
			component.Add(this.hotTub.specificEffect, true);
		}
		component.Add("WarmTouch", true).timeRemaining = 1800f;
	}

	// Token: 0x06006A42 RID: 27202 RVA: 0x002EBAC4 File Offset: 0x002E9CC4
	public bool GetWorkerPriority(WorkerBase worker, out int priority)
	{
		priority = this.hotTub.basePriority;
		Effects component = worker.GetComponent<Effects>();
		if (!string.IsNullOrEmpty(this.hotTub.trackingEffect) && component.HasEffect(this.hotTub.trackingEffect))
		{
			priority = 0;
			return false;
		}
		if (!string.IsNullOrEmpty(this.hotTub.specificEffect) && component.HasEffect(this.hotTub.specificEffect))
		{
			priority = RELAXATION.PRIORITY.RECENTLY_USED;
		}
		return true;
	}

	// Token: 0x0400509D RID: 20637
	public HotTub hotTub;

	// Token: 0x0400509E RID: 20638
	private bool faceLeft;
}
