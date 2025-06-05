using System;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x020009DB RID: 2523
[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/Workable/Clearable")]
public class Clearable : Workable, ISaveLoadable, IRender1000ms
{
	// Token: 0x06002DAC RID: 11692 RVA: 0x001FEE08 File Offset: 0x001FD008
	protected override void OnPrefabInit()
	{
		base.Subscribe<Clearable>(2127324410, Clearable.OnCancelDelegate);
		base.Subscribe<Clearable>(856640610, Clearable.OnStoreDelegate);
		base.Subscribe<Clearable>(-2064133523, Clearable.OnAbsorbDelegate);
		base.Subscribe<Clearable>(493375141, Clearable.OnRefreshUserMenuDelegate);
		base.Subscribe<Clearable>(-1617557748, Clearable.OnEquippedDelegate);
		this.workerStatusItem = Db.Get().DuplicantStatusItems.Clearing;
		this.simRenderLoadBalance = true;
		this.autoRegisterSimRender = false;
	}

	// Token: 0x06002DAD RID: 11693 RVA: 0x001FEE90 File Offset: 0x001FD090
	protected override void OnSpawn()
	{
		base.OnSpawn();
		if (this.isMarkedForClear)
		{
			if (this.pickupable.KPrefabID.HasTag(GameTags.Stored))
			{
				if (!base.transform.parent.GetComponent<Storage>().allowClearable)
				{
					this.isMarkedForClear = false;
				}
				else
				{
					this.MarkForClear(true, true);
				}
			}
			else
			{
				this.MarkForClear(true, false);
			}
		}
		this.RefreshClearableStatus(true);
	}

	// Token: 0x06002DAE RID: 11694 RVA: 0x000C20B1 File Offset: 0x000C02B1
	private void OnStore(object data)
	{
		this.CancelClearing();
	}

	// Token: 0x06002DAF RID: 11695 RVA: 0x001FEEFC File Offset: 0x001FD0FC
	private void OnCancel(object data)
	{
		for (ObjectLayerListItem objectLayerListItem = this.pickupable.objectLayerListItem; objectLayerListItem != null; objectLayerListItem = objectLayerListItem.nextItem)
		{
			if (objectLayerListItem.gameObject != null)
			{
				objectLayerListItem.gameObject.GetComponent<Clearable>().CancelClearing();
			}
		}
	}

	// Token: 0x06002DB0 RID: 11696 RVA: 0x001FEF40 File Offset: 0x001FD140
	public void CancelClearing()
	{
		if (this.isMarkedForClear)
		{
			this.isMarkedForClear = false;
			base.GetComponent<KPrefabID>().RemoveTag(GameTags.Garbage);
			Prioritizable.RemoveRef(base.gameObject);
			if (this.clearHandle.IsValid())
			{
				GlobalChoreProvider.Instance.UnregisterClearable(this.clearHandle);
				this.clearHandle.Clear();
			}
			this.RefreshClearableStatus(true);
			SimAndRenderScheduler.instance.Remove(this);
		}
	}

	// Token: 0x06002DB1 RID: 11697 RVA: 0x001FEFB4 File Offset: 0x001FD1B4
	public void MarkForClear(bool restoringFromSave = false, bool allowWhenStored = false)
	{
		if (!this.isClearable)
		{
			return;
		}
		if ((!this.isMarkedForClear || restoringFromSave) && !this.pickupable.IsEntombed && !this.clearHandle.IsValid() && (!this.HasTag(GameTags.Stored) || allowWhenStored))
		{
			Prioritizable.AddRef(base.gameObject);
			this.pickupable.KPrefabID.AddTag(GameTags.Garbage, false);
			this.isMarkedForClear = true;
			this.clearHandle = GlobalChoreProvider.Instance.RegisterClearable(this);
			this.RefreshClearableStatus(true);
			SimAndRenderScheduler.instance.Add(this, this.simRenderLoadBalance);
		}
	}

	// Token: 0x06002DB2 RID: 11698 RVA: 0x000C20B9 File Offset: 0x000C02B9
	private void OnClickClear()
	{
		this.MarkForClear(false, false);
	}

	// Token: 0x06002DB3 RID: 11699 RVA: 0x000C20B1 File Offset: 0x000C02B1
	private void OnClickCancel()
	{
		this.CancelClearing();
	}

	// Token: 0x06002DB4 RID: 11700 RVA: 0x000C20B1 File Offset: 0x000C02B1
	private void OnEquipped(object data)
	{
		this.CancelClearing();
	}

	// Token: 0x06002DB5 RID: 11701 RVA: 0x000C20C3 File Offset: 0x000C02C3
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		if (this.clearHandle.IsValid())
		{
			GlobalChoreProvider.Instance.UnregisterClearable(this.clearHandle);
			this.clearHandle.Clear();
		}
	}

	// Token: 0x06002DB6 RID: 11702 RVA: 0x001FF054 File Offset: 0x001FD254
	private void OnRefreshUserMenu(object data)
	{
		if (!this.isClearable || base.GetComponent<Health>() != null || this.pickupable.KPrefabID.HasTag(GameTags.Stored) || this.pickupable.KPrefabID.HasTag(GameTags.MarkedForMove))
		{
			return;
		}
		KIconButtonMenu.ButtonInfo button = this.isMarkedForClear ? new KIconButtonMenu.ButtonInfo("action_move_to_storage", UI.USERMENUACTIONS.CLEAR.NAME_OFF, new System.Action(this.OnClickCancel), global::Action.NumActions, null, null, null, UI.USERMENUACTIONS.CLEAR.TOOLTIP_OFF, true) : new KIconButtonMenu.ButtonInfo("action_move_to_storage", UI.USERMENUACTIONS.CLEAR.NAME, new System.Action(this.OnClickClear), global::Action.NumActions, null, null, null, UI.USERMENUACTIONS.CLEAR.TOOLTIP, true);
		Game.Instance.userMenu.AddButton(base.gameObject, button, 1f);
	}

	// Token: 0x06002DB7 RID: 11703 RVA: 0x001FF134 File Offset: 0x001FD334
	private void OnAbsorb(object data)
	{
		Pickupable pickupable = (Pickupable)data;
		if (pickupable != null)
		{
			Clearable component = pickupable.GetComponent<Clearable>();
			if (component != null && component.isMarkedForClear)
			{
				this.MarkForClear(false, false);
			}
		}
	}

	// Token: 0x06002DB8 RID: 11704 RVA: 0x000C20F3 File Offset: 0x000C02F3
	public void Render1000ms(float dt)
	{
		this.RefreshClearableStatus(false);
	}

	// Token: 0x06002DB9 RID: 11705 RVA: 0x001FF174 File Offset: 0x001FD374
	public void RefreshClearableStatus(bool force_update)
	{
		if (force_update || this.isMarkedForClear)
		{
			bool show = false;
			bool show2 = false;
			if (this.isMarkedForClear)
			{
				show2 = !(show = GlobalChoreProvider.Instance.ClearableHasDestination(this.pickupable));
			}
			this.pendingClearGuid = this.selectable.ToggleStatusItem(Db.Get().MiscStatusItems.PendingClear, this.pendingClearGuid, show, this);
			this.pendingClearNoStorageGuid = this.selectable.ToggleStatusItem(Db.Get().MiscStatusItems.PendingClearNoStorage, this.pendingClearNoStorageGuid, show2, this);
		}
	}

	// Token: 0x04001F4C RID: 8012
	[MyCmpReq]
	private Pickupable pickupable;

	// Token: 0x04001F4D RID: 8013
	[MyCmpReq]
	private KSelectable selectable;

	// Token: 0x04001F4E RID: 8014
	[Serialize]
	private bool isMarkedForClear;

	// Token: 0x04001F4F RID: 8015
	private HandleVector<int>.Handle clearHandle;

	// Token: 0x04001F50 RID: 8016
	public bool isClearable = true;

	// Token: 0x04001F51 RID: 8017
	private Guid pendingClearGuid;

	// Token: 0x04001F52 RID: 8018
	private Guid pendingClearNoStorageGuid;

	// Token: 0x04001F53 RID: 8019
	private static readonly EventSystem.IntraObjectHandler<Clearable> OnCancelDelegate = new EventSystem.IntraObjectHandler<Clearable>(delegate(Clearable component, object data)
	{
		component.OnCancel(data);
	});

	// Token: 0x04001F54 RID: 8020
	private static readonly EventSystem.IntraObjectHandler<Clearable> OnStoreDelegate = new EventSystem.IntraObjectHandler<Clearable>(delegate(Clearable component, object data)
	{
		component.OnStore(data);
	});

	// Token: 0x04001F55 RID: 8021
	private static readonly EventSystem.IntraObjectHandler<Clearable> OnAbsorbDelegate = new EventSystem.IntraObjectHandler<Clearable>(delegate(Clearable component, object data)
	{
		component.OnAbsorb(data);
	});

	// Token: 0x04001F56 RID: 8022
	private static readonly EventSystem.IntraObjectHandler<Clearable> OnRefreshUserMenuDelegate = new EventSystem.IntraObjectHandler<Clearable>(delegate(Clearable component, object data)
	{
		component.OnRefreshUserMenu(data);
	});

	// Token: 0x04001F57 RID: 8023
	private static readonly EventSystem.IntraObjectHandler<Clearable> OnEquippedDelegate = new EventSystem.IntraObjectHandler<Clearable>(delegate(Clearable component, object data)
	{
		component.OnEquipped(data);
	});
}
