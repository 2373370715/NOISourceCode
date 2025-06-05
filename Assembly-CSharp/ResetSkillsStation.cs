using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x02000F95 RID: 3989
[AddComponentMenu("KMonoBehaviour/Workable/ResetSkillsStation")]
public class ResetSkillsStation : Workable
{
	// Token: 0x06005053 RID: 20563 RVA: 0x000D8BBC File Offset: 0x000D6DBC
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.lightEfficiencyBonus = false;
	}

	// Token: 0x06005054 RID: 20564 RVA: 0x000D8BCB File Offset: 0x000D6DCB
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.OnAssign(this.assignable.assignee);
		this.assignable.OnAssign += this.OnAssign;
	}

	// Token: 0x06005055 RID: 20565 RVA: 0x000D8BFB File Offset: 0x000D6DFB
	private void OnAssign(IAssignableIdentity obj)
	{
		if (obj != null)
		{
			this.CreateChore();
			return;
		}
		if (this.chore != null)
		{
			this.chore.Cancel("Unassigned");
			this.chore = null;
		}
	}

	// Token: 0x06005056 RID: 20566 RVA: 0x0027D1BC File Offset: 0x0027B3BC
	private void CreateChore()
	{
		this.chore = new WorkChore<ResetSkillsStation>(Db.Get().ChoreTypes.UnlearnSkill, this, null, true, null, null, null, false, null, true, true, null, false, true, false, PriorityScreen.PriorityClass.high, 5, false, true);
	}

	// Token: 0x06005057 RID: 20567 RVA: 0x000D8C26 File Offset: 0x000D6E26
	protected override void OnStartWork(WorkerBase worker)
	{
		base.OnStartWork(worker);
		base.GetComponent<Operational>().SetActive(true, false);
		base.GetComponent<KSelectable>().AddStatusItem(Db.Get().BuildingStatusItems.ComplexFabricatorTraining, this);
	}

	// Token: 0x06005058 RID: 20568 RVA: 0x0027D1F8 File Offset: 0x0027B3F8
	protected override void OnCompleteWork(WorkerBase worker)
	{
		base.OnCompleteWork(worker);
		this.assignable.Unassign();
		MinionResume component = worker.GetComponent<MinionResume>();
		if (component != null)
		{
			component.ResetSkillLevels(true);
			component.SetHats(component.CurrentHat, null);
			component.ApplyTargetHat();
			this.notification = new Notification(MISC.NOTIFICATIONS.RESETSKILL.NAME, NotificationType.Good, (List<Notification> notificationList, object data) => MISC.NOTIFICATIONS.RESETSKILL.TOOLTIP + notificationList.ReduceMessages(false), null, true, 0f, null, null, null, true, false, false);
			worker.GetComponent<Notifier>().Add(this.notification, "");
		}
	}

	// Token: 0x06005059 RID: 20569 RVA: 0x000D8C58 File Offset: 0x000D6E58
	protected override void OnStopWork(WorkerBase worker)
	{
		base.OnStopWork(worker);
		base.GetComponent<Operational>().SetActive(false, false);
		base.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().BuildingStatusItems.ComplexFabricatorTraining, this);
		this.chore = null;
	}

	// Token: 0x04003894 RID: 14484
	[MyCmpReq]
	public Assignable assignable;

	// Token: 0x04003895 RID: 14485
	private Notification notification;

	// Token: 0x04003896 RID: 14486
	private Chore chore;
}
