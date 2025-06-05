using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200128D RID: 4749
[AddComponentMenu("KMonoBehaviour/Workable/DropToUserCapacity")]
public class DropToUserCapacity : Workable
{
	// Token: 0x060060FE RID: 24830 RVA: 0x000C8314 File Offset: 0x000C6514
	protected DropToUserCapacity()
	{
		base.SetOffsetTable(OffsetGroups.InvertedStandardTable);
	}

	// Token: 0x060060FF RID: 24831 RVA: 0x002BE110 File Offset: 0x002BC310
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.workerStatusItem = Db.Get().DuplicantStatusItems.Emptying;
		base.Subscribe<DropToUserCapacity>(-945020481, DropToUserCapacity.OnStorageCapacityChangedHandler);
		base.Subscribe<DropToUserCapacity>(-1697596308, DropToUserCapacity.OnStorageChangedHandler);
		this.synchronizeAnims = false;
		base.SetWorkTime(0.1f);
	}

	// Token: 0x06006100 RID: 24832 RVA: 0x000E3993 File Offset: 0x000E1B93
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.UpdateChore();
	}

	// Token: 0x06006101 RID: 24833 RVA: 0x000E39A1 File Offset: 0x000E1BA1
	private Storage[] GetStorages()
	{
		if (this.storages == null)
		{
			this.storages = base.GetComponents<Storage>();
		}
		return this.storages;
	}

	// Token: 0x06006102 RID: 24834 RVA: 0x000E39BD File Offset: 0x000E1BBD
	private void OnStorageChanged(object data)
	{
		this.UpdateChore();
	}

	// Token: 0x06006103 RID: 24835 RVA: 0x002BE16C File Offset: 0x002BC36C
	public void UpdateChore()
	{
		IUserControlledCapacity component = base.GetComponent<IUserControlledCapacity>();
		if (component != null && component.AmountStored > component.UserMaxCapacity)
		{
			if (this.chore == null)
			{
				this.chore = new WorkChore<DropToUserCapacity>(Db.Get().ChoreTypes.EmptyStorage, this, null, true, null, null, null, true, null, false, false, null, false, true, true, PriorityScreen.PriorityClass.basic, 5, false, true);
				return;
			}
		}
		else if (this.chore != null)
		{
			this.chore.Cancel("Cancelled emptying");
			this.chore = null;
			base.GetComponent<KSelectable>().RemoveStatusItem(this.workerStatusItem, false);
			base.ShowProgressBar(false);
		}
	}

	// Token: 0x06006104 RID: 24836 RVA: 0x002BE200 File Offset: 0x002BC400
	protected override void OnCompleteWork(WorkerBase worker)
	{
		Storage component = base.GetComponent<Storage>();
		IUserControlledCapacity component2 = base.GetComponent<IUserControlledCapacity>();
		float num = Mathf.Max(0f, component2.AmountStored - component2.UserMaxCapacity);
		List<GameObject> list = new List<GameObject>(component.items);
		for (int i = 0; i < list.Count; i++)
		{
			Pickupable component3 = list[i].GetComponent<Pickupable>();
			if (component3.PrimaryElement.Mass > num)
			{
				component3.Take(num).transform.SetPosition(base.transform.GetPosition());
				return;
			}
			num -= component3.PrimaryElement.Mass;
			component.Drop(component3.gameObject, true);
		}
		this.chore = null;
	}

	// Token: 0x04004557 RID: 17751
	private Chore chore;

	// Token: 0x04004558 RID: 17752
	private bool showCmd;

	// Token: 0x04004559 RID: 17753
	private Storage[] storages;

	// Token: 0x0400455A RID: 17754
	private static readonly EventSystem.IntraObjectHandler<DropToUserCapacity> OnStorageCapacityChangedHandler = new EventSystem.IntraObjectHandler<DropToUserCapacity>(delegate(DropToUserCapacity component, object data)
	{
		component.OnStorageChanged(data);
	});

	// Token: 0x0400455B RID: 17755
	private static readonly EventSystem.IntraObjectHandler<DropToUserCapacity> OnStorageChangedHandler = new EventSystem.IntraObjectHandler<DropToUserCapacity>(delegate(DropToUserCapacity component, object data)
	{
		component.OnStorageChanged(data);
	});
}
