using System;
using System.Collections.Generic;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x0200128B RID: 4747
[AddComponentMenu("KMonoBehaviour/Workable/DropAllWorkable")]
public class DropAllWorkable : Workable
{
	// Token: 0x170005D0 RID: 1488
	// (get) Token: 0x060060ED RID: 24813 RVA: 0x000E38C3 File Offset: 0x000E1AC3
	// (set) Token: 0x060060EE RID: 24814 RVA: 0x000E38CB File Offset: 0x000E1ACB
	private Chore Chore
	{
		get
		{
			return this._chore;
		}
		set
		{
			this._chore = value;
			this.markedForDrop = (this._chore != null);
		}
	}

	// Token: 0x060060EF RID: 24815 RVA: 0x000E38E3 File Offset: 0x000E1AE3
	protected DropAllWorkable()
	{
		base.SetOffsetTable(OffsetGroups.InvertedStandardTable);
	}

	// Token: 0x060060F0 RID: 24816 RVA: 0x002BDD60 File Offset: 0x002BBF60
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.Subscribe<DropAllWorkable>(493375141, DropAllWorkable.OnRefreshUserMenuDelegate);
		base.Subscribe<DropAllWorkable>(-1697596308, DropAllWorkable.OnStorageChangeDelegate);
		this.workerStatusItem = Db.Get().DuplicantStatusItems.Emptying;
		this.synchronizeAnims = false;
		base.SetWorkTime(this.dropWorkTime);
		Prioritizable.AddRef(base.gameObject);
	}

	// Token: 0x060060F1 RID: 24817 RVA: 0x000E3901 File Offset: 0x000E1B01
	private Storage[] GetStorages()
	{
		if (this.storages == null)
		{
			this.storages = base.GetComponents<Storage>();
		}
		return this.storages;
	}

	// Token: 0x060060F2 RID: 24818 RVA: 0x000E391D File Offset: 0x000E1B1D
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.showCmd = this.GetNewShowCmd();
		if (this.markedForDrop)
		{
			this.DropAll();
		}
	}

	// Token: 0x060060F3 RID: 24819 RVA: 0x002BDDC8 File Offset: 0x002BBFC8
	public void DropAll()
	{
		if (DebugHandler.InstantBuildMode)
		{
			this.OnCompleteWork(null);
		}
		else if (this.Chore == null)
		{
			ChoreType chore_type = (!string.IsNullOrEmpty(this.choreTypeID)) ? Db.Get().ChoreTypes.Get(this.choreTypeID) : Db.Get().ChoreTypes.EmptyStorage;
			this.Chore = new WorkChore<DropAllWorkable>(chore_type, this, null, true, null, null, null, true, null, false, false, null, false, true, true, PriorityScreen.PriorityClass.basic, 5, false, true);
		}
		else
		{
			this.Chore.Cancel("Cancelled emptying");
			this.Chore = null;
			base.GetComponent<KSelectable>().RemoveStatusItem(this.workerStatusItem, false);
			base.ShowProgressBar(false);
		}
		this.RefreshStatusItem();
	}

	// Token: 0x060060F4 RID: 24820 RVA: 0x002BDE7C File Offset: 0x002BC07C
	protected override void OnCompleteWork(WorkerBase worker)
	{
		Storage[] array = this.GetStorages();
		for (int i = 0; i < array.Length; i++)
		{
			List<GameObject> list = new List<GameObject>(array[i].items);
			for (int j = 0; j < list.Count; j++)
			{
				GameObject gameObject = array[i].Drop(list[j], true);
				if (gameObject != null)
				{
					foreach (Tag tag in this.removeTags)
					{
						gameObject.RemoveTag(tag);
					}
					gameObject.Trigger(580035959, worker);
					if (this.resetTargetWorkableOnCompleteWork)
					{
						Pickupable component = gameObject.GetComponent<Pickupable>();
						component.targetWorkable = component;
						component.SetOffsetTable(OffsetGroups.InvertedStandardTable);
					}
				}
			}
		}
		this.Chore = null;
		this.RefreshStatusItem();
		base.Trigger(-1957399615, null);
	}

	// Token: 0x060060F5 RID: 24821 RVA: 0x002BDF78 File Offset: 0x002BC178
	private void OnRefreshUserMenu(object data)
	{
		if (this.showCmd)
		{
			KIconButtonMenu.ButtonInfo button = (this.Chore == null) ? new KIconButtonMenu.ButtonInfo("action_empty_contents", UI.USERMENUACTIONS.EMPTYSTORAGE.NAME, new System.Action(this.DropAll), global::Action.NumActions, null, null, null, UI.USERMENUACTIONS.EMPTYSTORAGE.TOOLTIP, true) : new KIconButtonMenu.ButtonInfo("action_empty_contents", UI.USERMENUACTIONS.EMPTYSTORAGE.NAME_OFF, new System.Action(this.DropAll), global::Action.NumActions, null, null, null, UI.USERMENUACTIONS.EMPTYSTORAGE.TOOLTIP_OFF, true);
			Game.Instance.userMenu.AddButton(base.gameObject, button, 1f);
		}
	}

	// Token: 0x060060F6 RID: 24822 RVA: 0x002BE01C File Offset: 0x002BC21C
	private bool GetNewShowCmd()
	{
		bool flag = false;
		Storage[] array = this.GetStorages();
		for (int i = 0; i < array.Length; i++)
		{
			flag = (flag || !array[i].IsEmpty());
		}
		return flag;
	}

	// Token: 0x060060F7 RID: 24823 RVA: 0x002BE054 File Offset: 0x002BC254
	private void OnStorageChange(object data)
	{
		bool newShowCmd = this.GetNewShowCmd();
		if (newShowCmd != this.showCmd)
		{
			this.showCmd = newShowCmd;
			Game.Instance.userMenu.Refresh(base.gameObject);
		}
	}

	// Token: 0x060060F8 RID: 24824 RVA: 0x002BE090 File Offset: 0x002BC290
	private void RefreshStatusItem()
	{
		if (this.Chore != null && this.statusItem == Guid.Empty)
		{
			KSelectable component = base.GetComponent<KSelectable>();
			this.statusItem = component.AddStatusItem(Db.Get().BuildingStatusItems.AwaitingEmptyBuilding, null);
			return;
		}
		if (this.Chore == null && this.statusItem != Guid.Empty)
		{
			KSelectable component2 = base.GetComponent<KSelectable>();
			this.statusItem = component2.RemoveStatusItem(this.statusItem, false);
		}
	}

	// Token: 0x0400454A RID: 17738
	[Serialize]
	private bool markedForDrop;

	// Token: 0x0400454B RID: 17739
	private Chore _chore;

	// Token: 0x0400454C RID: 17740
	private bool showCmd;

	// Token: 0x0400454D RID: 17741
	private Storage[] storages;

	// Token: 0x0400454E RID: 17742
	public float dropWorkTime = 0.1f;

	// Token: 0x0400454F RID: 17743
	public string choreTypeID;

	// Token: 0x04004550 RID: 17744
	[MyCmpAdd]
	private Prioritizable _prioritizable;

	// Token: 0x04004551 RID: 17745
	public List<Tag> removeTags;

	// Token: 0x04004552 RID: 17746
	public bool resetTargetWorkableOnCompleteWork;

	// Token: 0x04004553 RID: 17747
	private static readonly EventSystem.IntraObjectHandler<DropAllWorkable> OnRefreshUserMenuDelegate = new EventSystem.IntraObjectHandler<DropAllWorkable>(delegate(DropAllWorkable component, object data)
	{
		component.OnRefreshUserMenu(data);
	});

	// Token: 0x04004554 RID: 17748
	private static readonly EventSystem.IntraObjectHandler<DropAllWorkable> OnStorageChangeDelegate = new EventSystem.IntraObjectHandler<DropAllWorkable>(delegate(DropAllWorkable component, object data)
	{
		component.OnStorageChange(data);
	});

	// Token: 0x04004555 RID: 17749
	private Guid statusItem;
}
