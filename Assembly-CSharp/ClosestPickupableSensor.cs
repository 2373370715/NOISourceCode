using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200084C RID: 2124
public abstract class ClosestPickupableSensor<T> : Sensor where T : Component
{
	// Token: 0x06002577 RID: 9591 RVA: 0x000BCED4 File Offset: 0x000BB0D4
	public ClosestPickupableSensor(Sensors sensors, Tag itemSearchTag, bool shouldStartActive) : base(sensors, shouldStartActive)
	{
		this.navigator = base.GetComponent<Navigator>();
		this.consumableConsumer = base.GetComponent<ConsumableConsumer>();
		this.storage = base.GetComponent<Storage>();
		this.itemSearchTag = itemSearchTag;
	}

	// Token: 0x06002578 RID: 9592 RVA: 0x000BCF14 File Offset: 0x000BB114
	public T GetItem()
	{
		return this.item;
	}

	// Token: 0x06002579 RID: 9593 RVA: 0x000BCF1C File Offset: 0x000BB11C
	public int GetItemNavCost()
	{
		if (!(this.item == null))
		{
			return this.itemNavCost;
		}
		return int.MaxValue;
	}

	// Token: 0x0600257A RID: 9594 RVA: 0x000BCF3D File Offset: 0x000BB13D
	public virtual HashSet<Tag> GetForbbidenTags()
	{
		if (!(this.consumableConsumer == null))
		{
			return this.consumableConsumer.forbiddenTagSet;
		}
		return new HashSet<Tag>(0);
	}

	// Token: 0x0600257B RID: 9595 RVA: 0x001D9738 File Offset: 0x001D7938
	public override void Update()
	{
		HashSet<Tag> forbbidenTags = this.GetForbbidenTags();
		int maxValue = int.MaxValue;
		Pickupable pickupable = this.FindClosestPickupable(this.storage, forbbidenTags, out maxValue, this.itemSearchTag, this.requiredTags);
		bool flag = this.itemInReachButNotPermitted;
		T t = default(T);
		bool flag2 = false;
		if (pickupable != null)
		{
			t = pickupable.GetComponent<T>();
			flag2 = true;
			flag = false;
		}
		else
		{
			int num;
			flag = (this.FindClosestPickupable(this.storage, new HashSet<Tag>(), out num, this.itemSearchTag, this.requiredTags) != null);
		}
		if (t != this.item || this.isThereAnyItemAvailable != flag2)
		{
			this.item = t;
			this.itemNavCost = maxValue;
			this.isThereAnyItemAvailable = flag2;
			this.itemInReachButNotPermitted = flag;
			this.ItemChanged();
		}
	}

	// Token: 0x0600257C RID: 9596 RVA: 0x001D9808 File Offset: 0x001D7A08
	public Pickupable FindClosestPickupable(Storage destination, HashSet<Tag> exclude_tags, out int cost, Tag categoryTag, Tag[] otherRequiredTags = null)
	{
		ICollection<Pickupable> pickupables = base.gameObject.GetMyWorld().worldInventory.GetPickupables(categoryTag, false);
		if (pickupables == null)
		{
			cost = int.MaxValue;
			return null;
		}
		if (otherRequiredTags == null)
		{
			otherRequiredTags = new Tag[]
			{
				categoryTag
			};
		}
		Pickupable result = null;
		int num = int.MaxValue;
		foreach (Pickupable pickupable in pickupables)
		{
			if (FetchManager.IsFetchablePickup_Exclude(pickupable.KPrefabID, pickupable.storage, pickupable.UnreservedAmount, exclude_tags, otherRequiredTags, destination))
			{
				int navigationCost = pickupable.GetNavigationCost(this.navigator, pickupable.cachedCell);
				if (navigationCost != -1 && navigationCost < num)
				{
					result = pickupable;
					num = navigationCost;
				}
			}
		}
		cost = num;
		return result;
	}

	// Token: 0x0600257D RID: 9597 RVA: 0x000BCF5F File Offset: 0x000BB15F
	public virtual void ItemChanged()
	{
		Action<T> onItemChanged = this.OnItemChanged;
		if (onItemChanged == null)
		{
			return;
		}
		onItemChanged(this.item);
	}

	// Token: 0x040019CE RID: 6606
	public Action<T> OnItemChanged;

	// Token: 0x040019CF RID: 6607
	protected T item;

	// Token: 0x040019D0 RID: 6608
	protected int itemNavCost = int.MaxValue;

	// Token: 0x040019D1 RID: 6609
	protected Tag itemSearchTag;

	// Token: 0x040019D2 RID: 6610
	protected Tag[] requiredTags;

	// Token: 0x040019D3 RID: 6611
	protected bool isThereAnyItemAvailable;

	// Token: 0x040019D4 RID: 6612
	protected bool itemInReachButNotPermitted;

	// Token: 0x040019D5 RID: 6613
	private Navigator navigator;

	// Token: 0x040019D6 RID: 6614
	protected ConsumableConsumer consumableConsumer;

	// Token: 0x040019D7 RID: 6615
	private Storage storage;
}
