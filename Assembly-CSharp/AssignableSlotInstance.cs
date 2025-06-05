using System;
using UnityEngine;

// Token: 0x02000C64 RID: 3172
public abstract class AssignableSlotInstance
{
	// Token: 0x170002B5 RID: 693
	// (get) Token: 0x06003C1F RID: 15391 RVA: 0x000CB3F6 File Offset: 0x000C95F6
	// (set) Token: 0x06003C20 RID: 15392 RVA: 0x000CB3FE File Offset: 0x000C95FE
	public Assignables assignables { get; private set; }

	// Token: 0x170002B6 RID: 694
	// (get) Token: 0x06003C21 RID: 15393 RVA: 0x000CB407 File Offset: 0x000C9607
	public GameObject gameObject
	{
		get
		{
			return this.assignables.gameObject;
		}
	}

	// Token: 0x06003C22 RID: 15394 RVA: 0x000CB414 File Offset: 0x000C9614
	public AssignableSlotInstance(Assignables assignables, AssignableSlot slot) : this(slot.Id, assignables, slot)
	{
	}

	// Token: 0x06003C23 RID: 15395 RVA: 0x000CB424 File Offset: 0x000C9624
	public AssignableSlotInstance(string id, Assignables assignables, AssignableSlot slot)
	{
		this.ID = id;
		this.slot = slot;
		this.assignables = assignables;
	}

	// Token: 0x06003C24 RID: 15396 RVA: 0x000CB441 File Offset: 0x000C9641
	public void Assign(Assignable assignable)
	{
		if (this.assignable == assignable)
		{
			return;
		}
		this.Unassign(false);
		this.assignable = assignable;
		this.assignables.Trigger(-1585839766, this);
	}

	// Token: 0x06003C25 RID: 15397 RVA: 0x0023B070 File Offset: 0x00239270
	public virtual void Unassign(bool trigger_event = true)
	{
		if (this.unassigning)
		{
			return;
		}
		if (this.IsAssigned())
		{
			this.unassigning = true;
			this.assignable.Unassign();
			if (trigger_event)
			{
				this.assignables.Trigger(-1585839766, this);
			}
			this.assignable = null;
			this.unassigning = false;
		}
	}

	// Token: 0x06003C26 RID: 15398 RVA: 0x000CB471 File Offset: 0x000C9671
	public bool IsAssigned()
	{
		return this.assignable != null;
	}

	// Token: 0x06003C27 RID: 15399 RVA: 0x000CB47F File Offset: 0x000C967F
	public bool IsUnassigning()
	{
		return this.unassigning;
	}

	// Token: 0x040029D9 RID: 10713
	public string ID;

	// Token: 0x040029DA RID: 10714
	public AssignableSlot slot;

	// Token: 0x040029DB RID: 10715
	public Assignable assignable;

	// Token: 0x040029DD RID: 10717
	private bool unassigning;
}
