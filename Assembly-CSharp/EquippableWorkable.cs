using System;
using KSerialization;
using UnityEngine;

// Token: 0x020012F9 RID: 4857
[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/Workable/EquippableWorkable")]
public class EquippableWorkable : Workable, ISaveLoadable
{
	// Token: 0x06006394 RID: 25492 RVA: 0x002C8E10 File Offset: 0x002C7010
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.workerStatusItem = Db.Get().DuplicantStatusItems.Equipping;
		this.overrideAnims = new KAnimFile[]
		{
			Assets.GetAnim("anim_equip_clothing_kanim")
		};
		this.synchronizeAnims = false;
	}

	// Token: 0x06006395 RID: 25493 RVA: 0x000E576A File Offset: 0x000E396A
	public global::QualityLevel GetQuality()
	{
		return this.quality;
	}

	// Token: 0x06006396 RID: 25494 RVA: 0x000E5772 File Offset: 0x000E3972
	public void SetQuality(global::QualityLevel level)
	{
		this.quality = level;
	}

	// Token: 0x06006397 RID: 25495 RVA: 0x000E577B File Offset: 0x000E397B
	protected override void OnSpawn()
	{
		base.SetWorkTime(1.5f);
		this.equippable.OnAssign += this.RefreshChore;
	}

	// Token: 0x06006398 RID: 25496 RVA: 0x002C8E60 File Offset: 0x002C7060
	private void CreateChore()
	{
		global::Debug.Assert(this.chore == null, "chore should be null");
		this.chore = new EquipChore(this);
		Chore chore = this.chore;
		chore.onExit = (Action<Chore>)Delegate.Combine(chore.onExit, new Action<Chore>(this.OnChoreExit));
	}

	// Token: 0x06006399 RID: 25497 RVA: 0x000E579F File Offset: 0x000E399F
	private void OnChoreExit(Chore chore)
	{
		if (!chore.isComplete)
		{
			this.RefreshChore(this.currentTarget);
		}
	}

	// Token: 0x0600639A RID: 25498 RVA: 0x000E57B5 File Offset: 0x000E39B5
	public void CancelChore(string reason = "")
	{
		if (this.chore != null)
		{
			this.chore.Cancel(reason);
			Prioritizable.RemoveRef(this.equippable.gameObject);
			this.chore = null;
		}
	}

	// Token: 0x0600639B RID: 25499 RVA: 0x000E57E2 File Offset: 0x000E39E2
	private void RefreshChore(IAssignableIdentity target)
	{
		if (this.chore != null)
		{
			this.CancelChore("Equipment Reassigned");
		}
		this.currentTarget = target;
		if (target != null && !target.GetSoleOwner().GetComponent<Equipment>().IsEquipped(this.equippable))
		{
			this.CreateChore();
		}
	}

	// Token: 0x0600639C RID: 25500 RVA: 0x002C8EB4 File Offset: 0x002C70B4
	protected override void OnCompleteWork(WorkerBase worker)
	{
		if (this.equippable.assignee != null)
		{
			Ownables soleOwner = this.equippable.assignee.GetSoleOwner();
			if (soleOwner)
			{
				soleOwner.GetComponent<Equipment>().Equip(this.equippable);
				Prioritizable.RemoveRef(this.equippable.gameObject);
				this.chore = null;
			}
		}
	}

	// Token: 0x0600639D RID: 25501 RVA: 0x000E581F File Offset: 0x000E3A1F
	protected override void OnStopWork(WorkerBase worker)
	{
		this.workTimeRemaining = this.GetWorkTime();
		base.OnStopWork(worker);
	}

	// Token: 0x04004762 RID: 18274
	[MyCmpReq]
	private Equippable equippable;

	// Token: 0x04004763 RID: 18275
	private Chore chore;

	// Token: 0x04004764 RID: 18276
	private IAssignableIdentity currentTarget;

	// Token: 0x04004765 RID: 18277
	private global::QualityLevel quality;
}
