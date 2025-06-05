using System;
using System.Collections.Generic;
using Klei.AI;

// Token: 0x0200124A RID: 4682
public class DefragmentationZone : Workable
{
	// Token: 0x06005F47 RID: 24391 RVA: 0x002B4B00 File Offset: 0x002B2D00
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.SetReportType(ReportManager.ReportType.PersonalTime);
		this.showProgressBar = false;
		this.workerStatusItem = null;
		this.synchronizeAnims = false;
		this.triggerWorkReactions = false;
		this.lightEfficiencyBonus = false;
		this.approachable = base.GetComponent<IApproachable>();
		this.workAnims = new HashedString[]
		{
			"microchip_bed_pre",
			"microchip_bed_loop"
		};
		this.workingPstComplete = new HashedString[]
		{
			"microchip_bed_pst"
		};
		this.workingPstFailed = new HashedString[]
		{
			"microchip_bed_pst"
		};
	}

	// Token: 0x06005F48 RID: 24392 RVA: 0x000E2A85 File Offset: 0x000E0C85
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.SetWorkTime(float.PositiveInfinity);
		this.OnWorkableEventCB = (Action<Workable, Workable.WorkableEvent>)Delegate.Combine(this.OnWorkableEventCB, new Action<Workable, Workable.WorkableEvent>(this.OnWorkableEvent));
	}

	// Token: 0x06005F49 RID: 24393 RVA: 0x000E2ABA File Offset: 0x000E0CBA
	private void OnWorkableEvent(Workable workable, Workable.WorkableEvent workable_event)
	{
		if (workable_event == Workable.WorkableEvent.WorkStarted)
		{
			this.AddRoomEffects();
		}
	}

	// Token: 0x06005F4A RID: 24394 RVA: 0x002B4BB4 File Offset: 0x002B2DB4
	private void AddRoomEffects()
	{
		if (base.worker == null)
		{
			return;
		}
		Room roomOfGameObject = Game.Instance.roomProber.GetRoomOfGameObject(base.gameObject);
		if (roomOfGameObject == null)
		{
			return;
		}
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

	// Token: 0x06005F4B RID: 24395 RVA: 0x000B1628 File Offset: 0x000AF828
	public override bool InstantlyFinish(WorkerBase worker)
	{
		return false;
	}

	// Token: 0x0400440E RID: 17422
	private const float BEDROOM_EFFECTS_DURATION_OVERRIDE = 1800f;

	// Token: 0x0400440F RID: 17423
	[MyCmpGet]
	public Assignable assignable;

	// Token: 0x04004410 RID: 17424
	public IApproachable approachable;
}
