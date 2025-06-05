using System;
using System.Collections.Generic;
using Klei.AI;
using UnityEngine;

// Token: 0x0200037B RID: 891
public class GunkEmptierWorkable : Workable
{
	// Token: 0x06000E38 RID: 3640 RVA: 0x000B09A4 File Offset: 0x000AEBA4
	private GunkEmptierWorkable()
	{
		base.SetReportType(ReportManager.ReportType.PersonalTime);
	}

	// Token: 0x06000E39 RID: 3641 RVA: 0x001821E4 File Offset: 0x001803E4
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.showProgressBar = true;
		this.resetProgressOnStop = true;
		this.overrideAnims = new KAnimFile[]
		{
			Assets.GetAnim("anim_interacts_gunkdump_kanim")
		};
		this.attributeConverter = Db.Get().AttributeConverters.ToiletSpeed;
		this.storage = base.GetComponent<Storage>();
		base.SetWorkTime(8.5f);
	}

	// Token: 0x06000E3A RID: 3642 RVA: 0x00182250 File Offset: 0x00180450
	protected override bool OnWorkTick(WorkerBase worker, float dt)
	{
		float mass = Mathf.Min(new float[]
		{
			dt / this.workTime * GunkMonitor.GUNK_CAPACITY,
			this.gunkMonitor.CurrentGunkMass,
			this.storage.RemainingCapacity()
		});
		this.gunkMonitor.ExpellGunk(mass, this.storage);
		return base.OnWorkTick(worker, dt);
	}

	// Token: 0x06000E3B RID: 3643 RVA: 0x001822B0 File Offset: 0x001804B0
	protected override void OnStartWork(WorkerBase worker)
	{
		base.OnStartWork(worker);
		this.gunkMonitor = worker.GetSMI<GunkMonitor.Instance>();
		if (Sim.IsRadiationEnabled() && worker.GetAmounts().Get(Db.Get().Amounts.RadiationBalance).value > 0f)
		{
			worker.gameObject.GetComponent<KSelectable>().AddStatusItem(Db.Get().DuplicantStatusItems.ExpellingRads, null);
		}
		this.TriggerRoomEffects();
	}

	// Token: 0x06000E3C RID: 3644 RVA: 0x00182324 File Offset: 0x00180524
	private void TriggerRoomEffects()
	{
		Room roomOfGameObject = Game.Instance.roomProber.GetRoomOfGameObject(base.gameObject);
		if (roomOfGameObject != null)
		{
			RoomType roomType = roomOfGameObject.roomType;
			List<EffectInstance> list = null;
			roomType.TriggerRoomEffects(base.GetComponent<KPrefabID>(), base.worker.GetComponent<Effects>(), out list);
			if (list != null)
			{
				foreach (EffectInstance effectInstance in list)
				{
					effectInstance.timeRemaining = 1800f;
				}
			}
		}
	}

	// Token: 0x06000E3D RID: 3645 RVA: 0x000B09B4 File Offset: 0x000AEBB4
	protected override void OnCompleteWork(WorkerBase worker)
	{
		if (this.gunkMonitor != null)
		{
			this.gunkMonitor.ExpellAllGunk(this.storage);
		}
		this.gunkMonitor = null;
		base.OnCompleteWork(worker);
	}

	// Token: 0x06000E3E RID: 3646 RVA: 0x000B09DD File Offset: 0x000AEBDD
	protected override void OnStopWork(WorkerBase worker)
	{
		this.RemoveExpellingRadStatusItem();
		base.OnStopWork(worker);
	}

	// Token: 0x06000E3F RID: 3647 RVA: 0x000B09EC File Offset: 0x000AEBEC
	protected override void OnAbortWork(WorkerBase worker)
	{
		this.RemoveExpellingRadStatusItem();
		base.OnAbortWork(worker);
		this.gunkMonitor = null;
	}

	// Token: 0x06000E40 RID: 3648 RVA: 0x000B0A02 File Offset: 0x000AEC02
	private void RemoveExpellingRadStatusItem()
	{
		if (Sim.IsRadiationEnabled())
		{
			base.worker.gameObject.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().DuplicantStatusItems.ExpellingRads, false);
		}
	}

	// Token: 0x04000A80 RID: 2688
	private const float BATHROOM_EFFECTS_DURATION_OVERRIDE = 1800f;

	// Token: 0x04000A81 RID: 2689
	private Storage storage;

	// Token: 0x04000A82 RID: 2690
	private GunkMonitor.Instance gunkMonitor;
}
