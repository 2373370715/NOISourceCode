using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000C65 RID: 3173
[AddComponentMenu("KMonoBehaviour/scripts/Assignables")]
public class Assignables : KMonoBehaviour
{
	// Token: 0x170002B7 RID: 695
	// (get) Token: 0x06003C28 RID: 15400 RVA: 0x000CB487 File Offset: 0x000C9687
	public List<AssignableSlotInstance> Slots
	{
		get
		{
			return this.slots;
		}
	}

	// Token: 0x06003C29 RID: 15401 RVA: 0x0023B0C4 File Offset: 0x002392C4
	protected IAssignableIdentity GetAssignableIdentity()
	{
		MinionIdentity component = base.GetComponent<MinionIdentity>();
		if (component != null)
		{
			return component.assignableProxy.Get();
		}
		return base.GetComponent<MinionAssignablesProxy>();
	}

	// Token: 0x06003C2A RID: 15402 RVA: 0x000CB48F File Offset: 0x000C968F
	protected override void OnSpawn()
	{
		base.OnSpawn();
		GameUtil.SubscribeToTags<Assignables>(this, Assignables.OnDeadTagAddedDelegate, true);
	}

	// Token: 0x06003C2B RID: 15403 RVA: 0x0023B0F4 File Offset: 0x002392F4
	private void OnDeath(object data)
	{
		foreach (AssignableSlotInstance assignableSlotInstance in this.slots)
		{
			assignableSlotInstance.Unassign(true);
		}
	}

	// Token: 0x06003C2C RID: 15404 RVA: 0x000CB4A3 File Offset: 0x000C96A3
	public void Add(AssignableSlotInstance slot_instance)
	{
		this.slots.Add(slot_instance);
	}

	// Token: 0x06003C2D RID: 15405 RVA: 0x0023B148 File Offset: 0x00239348
	public Assignable GetAssignable(AssignableSlot slot)
	{
		AssignableSlotInstance slot2 = this.GetSlot(slot);
		if (slot2 == null)
		{
			return null;
		}
		return slot2.assignable;
	}

	// Token: 0x06003C2E RID: 15406 RVA: 0x0023B168 File Offset: 0x00239368
	public AssignableSlotInstance GetSlot(AssignableSlot slot)
	{
		global::Debug.Assert(this.slots.Count > 0, "GetSlot called with no slots configured");
		if (slot == null)
		{
			return null;
		}
		foreach (AssignableSlotInstance assignableSlotInstance in this.slots)
		{
			if (assignableSlotInstance.slot == slot)
			{
				return assignableSlotInstance;
			}
		}
		return null;
	}

	// Token: 0x06003C2F RID: 15407 RVA: 0x0023B1E4 File Offset: 0x002393E4
	public AssignableSlotInstance[] GetSlots(AssignableSlot slot)
	{
		global::Debug.Assert(this.slots.Count > 0, "GetSlot called with no slots configured");
		if (slot == null)
		{
			return null;
		}
		List<AssignableSlotInstance> list = this.slots.FindAll((AssignableSlotInstance s) => s.slot == slot);
		if (list != null && list.Count > 0)
		{
			return list.ToArray();
		}
		return null;
	}

	// Token: 0x06003C30 RID: 15408 RVA: 0x0023B24C File Offset: 0x0023944C
	public Assignable AutoAssignSlot(AssignableSlot slot)
	{
		Assignable assignable = this.GetAssignable(slot);
		if (assignable != null)
		{
			return assignable;
		}
		GameObject targetGameObject = base.GetComponent<MinionAssignablesProxy>().GetTargetGameObject();
		if (targetGameObject == null)
		{
			global::Debug.LogWarning("AutoAssignSlot failed, proxy game object was null.");
			return null;
		}
		Navigator component = targetGameObject.GetComponent<Navigator>();
		IAssignableIdentity assignableIdentity = this.GetAssignableIdentity();
		int num = int.MaxValue;
		foreach (Assignable assignable2 in Game.Instance.assignmentManager)
		{
			if (!(assignable2 == null) && !assignable2.IsAssigned() && assignable2.slot == slot && assignable2.CanAutoAssignTo(assignableIdentity))
			{
				int navigationCost = assignable2.GetNavigationCost(component);
				if (navigationCost != -1 && navigationCost < num)
				{
					num = navigationCost;
					assignable = assignable2;
				}
			}
		}
		if (assignable != null)
		{
			assignable.Assign(assignableIdentity);
		}
		return assignable;
	}

	// Token: 0x06003C31 RID: 15409 RVA: 0x0023B33C File Offset: 0x0023953C
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		foreach (AssignableSlotInstance assignableSlotInstance in this.slots)
		{
			assignableSlotInstance.Unassign(true);
		}
	}

	// Token: 0x040029DE RID: 10718
	protected List<AssignableSlotInstance> slots = new List<AssignableSlotInstance>();

	// Token: 0x040029DF RID: 10719
	private static readonly EventSystem.IntraObjectHandler<Assignables> OnDeadTagAddedDelegate = GameUtil.CreateHasTagHandler<Assignables>(GameTags.Dead, delegate(Assignables component, object data)
	{
		component.OnDeath(data);
	});
}
