using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using KSerialization;
using UnityEngine;

// Token: 0x02000C62 RID: 3170
public abstract class Assignable : KMonoBehaviour, ISaveLoadable
{
	// Token: 0x170002B3 RID: 691
	// (get) Token: 0x06003C01 RID: 15361 RVA: 0x000CB2EC File Offset: 0x000C94EC
	public AssignableSlot slot
	{
		get
		{
			if (this._slot == null)
			{
				this._slot = Db.Get().AssignableSlots.Get(this.slotID);
			}
			return this._slot;
		}
	}

	// Token: 0x170002B4 RID: 692
	// (get) Token: 0x06003C02 RID: 15362 RVA: 0x000CB317 File Offset: 0x000C9517
	public bool CanBeAssigned
	{
		get
		{
			return this.canBeAssigned;
		}
	}

	// Token: 0x14000011 RID: 17
	// (add) Token: 0x06003C03 RID: 15363 RVA: 0x0023A930 File Offset: 0x00238B30
	// (remove) Token: 0x06003C04 RID: 15364 RVA: 0x0023A968 File Offset: 0x00238B68
	public event Action<IAssignableIdentity> OnAssign;

	// Token: 0x06003C05 RID: 15365 RVA: 0x000AA038 File Offset: 0x000A8238
	[OnDeserialized]
	internal void OnDeserialized()
	{
	}

	// Token: 0x06003C06 RID: 15366 RVA: 0x0023A9A0 File Offset: 0x00238BA0
	private void RestoreAssignee()
	{
		IAssignableIdentity savedAssignee = this.GetSavedAssignee();
		if (savedAssignee != null)
		{
			AssignableSlotInstance savedSlotInstance = this.GetSavedSlotInstance(savedAssignee);
			this.Assign(savedAssignee, savedSlotInstance);
		}
	}

	// Token: 0x06003C07 RID: 15367 RVA: 0x0023A9C8 File Offset: 0x00238BC8
	private AssignableSlotInstance GetSavedSlotInstance(IAssignableIdentity savedAsignee)
	{
		if ((savedAsignee != null && savedAsignee is MinionIdentity) || savedAsignee is StoredMinionIdentity || savedAsignee is MinionAssignablesProxy)
		{
			Ownables soleOwner = savedAsignee.GetSoleOwner();
			if (soleOwner != null)
			{
				AssignableSlotInstance[] slots = soleOwner.GetSlots(this.slot);
				if (slots != null)
				{
					AssignableSlotInstance assignableSlotInstance = slots.FindFirst((AssignableSlotInstance i) => i.ID == this.assignee_slotInstanceID);
					if (assignableSlotInstance != null)
					{
						return assignableSlotInstance;
					}
				}
			}
			Equipment component = soleOwner.GetComponent<Equipment>();
			if (component != null)
			{
				AssignableSlotInstance[] slots2 = component.GetSlots(this.slot);
				if (slots2 != null)
				{
					AssignableSlotInstance assignableSlotInstance2 = slots2.FindFirst((AssignableSlotInstance i) => i.ID == this.assignee_slotInstanceID);
					if (assignableSlotInstance2 != null)
					{
						return assignableSlotInstance2;
					}
				}
			}
		}
		return null;
	}

	// Token: 0x06003C08 RID: 15368 RVA: 0x0023AA68 File Offset: 0x00238C68
	private IAssignableIdentity GetSavedAssignee()
	{
		if (this.assignee_identityRef.Get() != null)
		{
			return this.assignee_identityRef.Get().GetComponent<IAssignableIdentity>();
		}
		if (!string.IsNullOrEmpty(this.assignee_groupID))
		{
			return Game.Instance.assignmentManager.assignment_groups[this.assignee_groupID];
		}
		return null;
	}

	// Token: 0x06003C09 RID: 15369 RVA: 0x0023AAC4 File Offset: 0x00238CC4
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.RestoreAssignee();
		Components.AssignableItems.Add(this);
		Game.Instance.assignmentManager.Add(this);
		if (this.assignee == null && this.canBePublic)
		{
			this.Assign(Game.Instance.assignmentManager.assignment_groups["public"]);
		}
		this.assignmentPreconditions.Add(delegate(MinionAssignablesProxy proxy)
		{
			GameObject targetGameObject = proxy.GetTargetGameObject();
			return targetGameObject.GetComponent<KMonoBehaviour>().GetMyWorldId() == this.GetMyWorldId() || targetGameObject.IsMyParentWorld(base.gameObject);
		});
		this.autoassignmentPreconditions.Add(delegate(MinionAssignablesProxy proxy)
		{
			Operational component = base.GetComponent<Operational>();
			return !(component != null) || component.IsOperational;
		});
	}

	// Token: 0x06003C0A RID: 15370 RVA: 0x000CB31F File Offset: 0x000C951F
	protected override void OnCleanUp()
	{
		this.Unassign();
		Components.AssignableItems.Remove(this);
		Game.Instance.assignmentManager.Remove(this);
		base.OnCleanUp();
	}

	// Token: 0x06003C0B RID: 15371 RVA: 0x0023AB58 File Offset: 0x00238D58
	public bool CanAutoAssignTo(IAssignableIdentity identity)
	{
		MinionAssignablesProxy minionAssignablesProxy = identity as MinionAssignablesProxy;
		if (minionAssignablesProxy == null)
		{
			return true;
		}
		if (!this.CanAssignTo(minionAssignablesProxy))
		{
			return false;
		}
		using (List<Func<MinionAssignablesProxy, bool>>.Enumerator enumerator = this.autoassignmentPreconditions.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				if (!enumerator.Current(minionAssignablesProxy))
				{
					return false;
				}
			}
		}
		return true;
	}

	// Token: 0x06003C0C RID: 15372 RVA: 0x0023ABD0 File Offset: 0x00238DD0
	public bool CanAssignTo(IAssignableIdentity identity)
	{
		MinionAssignablesProxy minionAssignablesProxy = identity as MinionAssignablesProxy;
		if (minionAssignablesProxy == null)
		{
			return true;
		}
		using (List<Func<MinionAssignablesProxy, bool>>.Enumerator enumerator = this.assignmentPreconditions.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				if (!enumerator.Current(minionAssignablesProxy))
				{
					return false;
				}
			}
		}
		return true;
	}

	// Token: 0x06003C0D RID: 15373 RVA: 0x000CB348 File Offset: 0x000C9548
	public bool IsAssigned()
	{
		return this.assignee != null;
	}

	// Token: 0x06003C0E RID: 15374 RVA: 0x0023AC3C File Offset: 0x00238E3C
	public bool IsAssignedTo(IAssignableIdentity identity)
	{
		global::Debug.Assert(identity != null, "IsAssignedTo identity is null");
		Ownables soleOwner = identity.GetSoleOwner();
		global::Debug.Assert(soleOwner != null, "IsAssignedTo identity sole owner is null");
		if (this.assignee != null)
		{
			foreach (Ownables ownables in this.assignee.GetOwners())
			{
				global::Debug.Assert(ownables, "Assignable owners list contained null");
				if (ownables.gameObject == soleOwner.gameObject)
				{
					return true;
				}
			}
			return false;
		}
		return false;
	}

	// Token: 0x06003C0F RID: 15375 RVA: 0x000CB353 File Offset: 0x000C9553
	public virtual void Assign(IAssignableIdentity new_assignee)
	{
		this.Assign(new_assignee, null);
	}

	// Token: 0x06003C10 RID: 15376 RVA: 0x0023ACE4 File Offset: 0x00238EE4
	public virtual void Assign(IAssignableIdentity new_assignee, AssignableSlotInstance specificSlotInstance)
	{
		if (new_assignee == this.assignee)
		{
			return;
		}
		if (new_assignee is KMonoBehaviour)
		{
			if (!this.CanAssignTo(new_assignee))
			{
				return;
			}
			this.assignee_identityRef.Set((KMonoBehaviour)new_assignee);
			this.assignee_groupID = "";
		}
		else if (new_assignee is AssignmentGroup)
		{
			this.assignee_identityRef.Set(null);
			this.assignee_groupID = ((AssignmentGroup)new_assignee).id;
		}
		base.GetComponent<KPrefabID>().AddTag(GameTags.Assigned, false);
		this.assignee = new_assignee;
		this.assignee_slotInstanceID = null;
		if (this.slot != null && (new_assignee is MinionIdentity || new_assignee is StoredMinionIdentity || new_assignee is MinionAssignablesProxy))
		{
			if (specificSlotInstance == null)
			{
				Ownables soleOwner = new_assignee.GetSoleOwner();
				if (soleOwner != null)
				{
					AssignableSlotInstance slot = soleOwner.GetSlot(this.slot);
					if (slot != null)
					{
						this.assignee_slotInstanceID = slot.ID;
						slot.Assign(this);
					}
				}
				Equipment component = soleOwner.GetComponent<Equipment>();
				if (component != null)
				{
					AssignableSlotInstance slot2 = component.GetSlot(this.slot);
					if (slot2 != null)
					{
						this.assignee_slotInstanceID = slot2.ID;
						slot2.Assign(this);
					}
				}
			}
			else
			{
				this.assignee_slotInstanceID = specificSlotInstance.ID;
				specificSlotInstance.Assign(this);
			}
		}
		if (this.OnAssign != null)
		{
			this.OnAssign(new_assignee);
		}
		base.Trigger(684616645, new_assignee);
	}

	// Token: 0x06003C11 RID: 15377 RVA: 0x0023AE30 File Offset: 0x00239030
	public virtual void Unassign()
	{
		if (this.assignee == null)
		{
			return;
		}
		base.GetComponent<KPrefabID>().RemoveTag(GameTags.Assigned);
		if (this.slot != null)
		{
			Ownables soleOwner = this.assignee.GetSoleOwner();
			if (soleOwner)
			{
				AssignableSlotInstance[] slots = soleOwner.GetSlots(this.slot);
				AssignableSlotInstance assignableSlotInstance = (slots == null) ? null : slots.FindFirst((AssignableSlotInstance s) => s.assignable == this);
				if (assignableSlotInstance != null)
				{
					assignableSlotInstance.Unassign(true);
				}
				Equipment component = soleOwner.GetComponent<Equipment>();
				if (component != null)
				{
					AssignableSlotInstance[] slots2 = component.GetSlots(this.slot);
					assignableSlotInstance = ((slots2 == null) ? null : slots2.FindFirst((AssignableSlotInstance s) => s.assignable == this));
					if (assignableSlotInstance != null)
					{
						assignableSlotInstance.Unassign(true);
					}
				}
			}
		}
		this.assignee = null;
		if (this.canBePublic)
		{
			this.Assign(Game.Instance.assignmentManager.assignment_groups["public"]);
		}
		this.assignee_slotInstanceID = null;
		this.assignee_identityRef.Set(null);
		this.assignee_groupID = "";
		if (this.OnAssign != null)
		{
			this.OnAssign(null);
		}
		base.Trigger(684616645, null);
	}

	// Token: 0x06003C12 RID: 15378 RVA: 0x000CB35D File Offset: 0x000C955D
	public void SetCanBeAssigned(bool state)
	{
		this.canBeAssigned = state;
	}

	// Token: 0x06003C13 RID: 15379 RVA: 0x000CB366 File Offset: 0x000C9566
	public void AddAssignPrecondition(Func<MinionAssignablesProxy, bool> precondition)
	{
		this.assignmentPreconditions.Add(precondition);
	}

	// Token: 0x06003C14 RID: 15380 RVA: 0x000CB374 File Offset: 0x000C9574
	public void AddAutoassignPrecondition(Func<MinionAssignablesProxy, bool> precondition)
	{
		this.autoassignmentPreconditions.Add(precondition);
	}

	// Token: 0x06003C15 RID: 15381 RVA: 0x0023AF54 File Offset: 0x00239154
	public int GetNavigationCost(Navigator navigator)
	{
		int num = -1;
		int cell = Grid.PosToCell(this);
		IApproachable component = base.GetComponent<IApproachable>();
		CellOffset[] array = (component != null) ? component.GetOffsets() : new CellOffset[1];
		DebugUtil.DevAssert(navigator != null, "Navigator is mysteriously null", null);
		if (navigator == null)
		{
			return -1;
		}
		foreach (CellOffset offset in array)
		{
			int cell2 = Grid.OffsetCell(cell, offset);
			int navigationCost = navigator.GetNavigationCost(cell2);
			if (navigationCost != -1 && (num == -1 || navigationCost < num))
			{
				num = navigationCost;
			}
		}
		return num;
	}

	// Token: 0x040029CA RID: 10698
	public string slotID;

	// Token: 0x040029CB RID: 10699
	private AssignableSlot _slot;

	// Token: 0x040029CC RID: 10700
	public IAssignableIdentity assignee;

	// Token: 0x040029CD RID: 10701
	[Serialize]
	protected Ref<KMonoBehaviour> assignee_identityRef = new Ref<KMonoBehaviour>();

	// Token: 0x040029CE RID: 10702
	[Serialize]
	protected string assignee_slotInstanceID;

	// Token: 0x040029CF RID: 10703
	[Serialize]
	private string assignee_groupID = "";

	// Token: 0x040029D0 RID: 10704
	public AssignableSlot[] subSlots;

	// Token: 0x040029D1 RID: 10705
	public bool canBePublic;

	// Token: 0x040029D2 RID: 10706
	[Serialize]
	private bool canBeAssigned = true;

	// Token: 0x040029D3 RID: 10707
	private List<Func<MinionAssignablesProxy, bool>> autoassignmentPreconditions = new List<Func<MinionAssignablesProxy, bool>>();

	// Token: 0x040029D4 RID: 10708
	private List<Func<MinionAssignablesProxy, bool>> assignmentPreconditions = new List<Func<MinionAssignablesProxy, bool>>();

	// Token: 0x040029D5 RID: 10709
	public Func<Assignables, string> customAssignmentUITooltipFunc;

	// Token: 0x040029D6 RID: 10710
	public Func<Assignables, string> customAssignablesUITooltipFunc;
}
