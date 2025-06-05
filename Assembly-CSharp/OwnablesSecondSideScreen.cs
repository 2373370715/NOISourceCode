using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x02001FFA RID: 8186
public class OwnablesSecondSideScreen : KScreen
{
	// Token: 0x17000B11 RID: 2833
	// (get) Token: 0x0600ACFE RID: 44286 RVA: 0x00114D78 File Offset: 0x00112F78
	// (set) Token: 0x0600ACFD RID: 44285 RVA: 0x00114D6F File Offset: 0x00112F6F
	public AssignableSlotInstance Slot { get; private set; }

	// Token: 0x17000B12 RID: 2834
	// (get) Token: 0x0600AD00 RID: 44288 RVA: 0x00114D89 File Offset: 0x00112F89
	// (set) Token: 0x0600ACFF RID: 44287 RVA: 0x00114D80 File Offset: 0x00112F80
	public IAssignableIdentity OwnerIdentity { get; private set; }

	// Token: 0x17000B13 RID: 2835
	// (get) Token: 0x0600AD01 RID: 44289 RVA: 0x00114D91 File Offset: 0x00112F91
	public AssignableSlot SlotType
	{
		get
		{
			if (this.Slot != null)
			{
				return this.Slot.slot;
			}
			return null;
		}
	}

	// Token: 0x17000B14 RID: 2836
	// (get) Token: 0x0600AD02 RID: 44290 RVA: 0x00114DA8 File Offset: 0x00112FA8
	public Assignable CurrentSlotItem
	{
		get
		{
			if (!this.HasItem)
			{
				return null;
			}
			return this.Slot.assignable;
		}
	}

	// Token: 0x17000B15 RID: 2837
	// (get) Token: 0x0600AD03 RID: 44291 RVA: 0x00114DBF File Offset: 0x00112FBF
	public bool HasItem
	{
		get
		{
			return this.Slot != null && this.Slot.IsAssigned();
		}
	}

	// Token: 0x0600AD04 RID: 44292 RVA: 0x00114DD6 File Offset: 0x00112FD6
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.originalRow.gameObject.SetActive(false);
		MultiToggle multiToggle = this.noneRow;
		multiToggle.onClick = (System.Action)Delegate.Combine(multiToggle.onClick, new System.Action(this.OnNoneRowClicked));
	}

	// Token: 0x0600AD05 RID: 44293 RVA: 0x00114E16 File Offset: 0x00113016
	private void OnNoneRowClicked()
	{
		this.UnassignCurrentItem();
		this.RefreshNoneRow();
	}

	// Token: 0x0600AD06 RID: 44294 RVA: 0x00114E24 File Offset: 0x00113024
	protected override void OnCmpDisable()
	{
		this.SetSlot(null);
		base.OnCmpDisable();
	}

	// Token: 0x0600AD07 RID: 44295 RVA: 0x004206BC File Offset: 0x0041E8BC
	public void SetSlot(AssignableSlotInstance slot)
	{
		Components.AssignableItems.Unregister(new Action<Assignable>(this.OnNewItemAvailable), new Action<Assignable>(this.OnItemUnregistered));
		this.Slot = slot;
		this.OwnerIdentity = ((slot == null) ? null : slot.assignables.GetComponent<IAssignableIdentity>());
		if (this.Slot != null)
		{
			Components.AssignableItems.Register(new Action<Assignable>(this.OnNewItemAvailable), new Action<Assignable>(this.OnItemUnregistered));
		}
		this.RefreshItemListOptions(true);
	}

	// Token: 0x0600AD08 RID: 44296 RVA: 0x0042073C File Offset: 0x0041E93C
	public void SortRows()
	{
		if (this.itemRows != null)
		{
			this.itemRows.Sort((OwnablesSecondSideScreenRow a, OwnablesSecondSideScreenRow b) => string.Compare(UI.StripLinkFormatting(a.nameLabel.text), UI.StripLinkFormatting(b.nameLabel.text)) * -1);
			OwnablesSecondSideScreenRow ownablesSecondSideScreenRow = null;
			for (int i = 0; i < this.itemRows.Count; i++)
			{
				OwnablesSecondSideScreenRow ownablesSecondSideScreenRow2 = this.itemRows[i];
				if (ownablesSecondSideScreenRow2.item == null || ownablesSecondSideScreenRow2.item.IsAssigned())
				{
					if (ownablesSecondSideScreenRow == null && ownablesSecondSideScreenRow2 != null && ownablesSecondSideScreenRow2.item != null && ownablesSecondSideScreenRow2.item.IsAssigned() && ownablesSecondSideScreenRow2.item == this.CurrentSlotItem)
					{
						ownablesSecondSideScreenRow = ownablesSecondSideScreenRow2;
					}
					else
					{
						ownablesSecondSideScreenRow2.transform.SetAsLastSibling();
					}
				}
				else
				{
					ownablesSecondSideScreenRow2.transform.SetAsFirstSibling();
				}
			}
			if (ownablesSecondSideScreenRow != null)
			{
				ownablesSecondSideScreenRow.transform.SetAsFirstSibling();
			}
		}
		this.noneRow.transform.SetAsFirstSibling();
	}

	// Token: 0x0600AD09 RID: 44297 RVA: 0x00420844 File Offset: 0x0041EA44
	public void RefreshItemListOptions(bool sortRows = false)
	{
		GameObject gameObject = (this.OwnerIdentity == null) ? null : this.OwnerIdentity.GetOwners()[0].GetComponent<MinionAssignablesProxy>().GetTargetGameObject();
		int worldID = (this.OwnerIdentity == null) ? 255 : gameObject.GetMyWorldId();
		List<Assignable> list = null;
		int b = 0;
		bool showItemsAssignedToOthers = true;
		if (this.Slot != null && (this.Slot is EquipmentSlotInstance || this.Slot.ID.Contains("BionicUpgrade")))
		{
			showItemsAssignedToOthers = false;
		}
		if (worldID != 255)
		{
			list = Components.AssignableItems.Items.FindAll(delegate(Assignable i)
			{
				bool flag = i.slotID == this.SlotType.Id && i.CanAssignTo(this.OwnerIdentity);
				if (flag && i is Equippable)
				{
					Equippable equippable = i as Equippable;
					GameObject gameObject2 = equippable.gameObject;
					if (equippable.isEquipped)
					{
						gameObject2 = equippable.assignee.GetOwners()[0].GetComponent<MinionAssignablesProxy>().GetTargetGameObject();
					}
					flag = (flag && gameObject2.GetMyWorldId() == worldID);
				}
				bool flag2 = i.assignee != null && i.assignee.GetSoleOwner() == this.OwnerIdentity.GetSoleOwner();
				bool flag3 = flag2 && this.Slot.assignable == i;
				if (!showItemsAssignedToOthers)
				{
					if (i.assignee != null && !flag2)
					{
						flag = false;
					}
					if (flag2 && !flag3)
					{
						flag = false;
					}
				}
				return flag;
			});
			b = list.Count;
		}
		for (int j = 0; j < Mathf.Max(this.itemRows.Count, b); j++)
		{
			if (list != null && j < list.Count)
			{
				Assignable assignable = list[j];
				if (j >= this.itemRows.Count)
				{
					OwnablesSecondSideScreenRow item = this.CreateItemRow(assignable);
					this.itemRows.Add(item);
				}
				OwnablesSecondSideScreenRow ownablesSecondSideScreenRow = this.itemRows[j];
				ownablesSecondSideScreenRow.gameObject.SetActive(true);
				ownablesSecondSideScreenRow.SetData(this.Slot, assignable);
			}
			else
			{
				OwnablesSecondSideScreenRow ownablesSecondSideScreenRow2 = this.itemRows[j];
				ownablesSecondSideScreenRow2.ClearData();
				ownablesSecondSideScreenRow2.gameObject.SetActive(false);
			}
		}
		if (sortRows)
		{
			this.SortRows();
		}
		this.RefreshNoneRow();
	}

	// Token: 0x0600AD0A RID: 44298 RVA: 0x00114E33 File Offset: 0x00113033
	private void RefreshNoneRow()
	{
		this.noneRow.ChangeState(this.HasItem ? 0 : 1);
	}

	// Token: 0x0600AD0B RID: 44299 RVA: 0x004209C8 File Offset: 0x0041EBC8
	private OwnablesSecondSideScreenRow CreateItemRow(Assignable item)
	{
		OwnablesSecondSideScreenRow component = Util.KInstantiateUI(this.originalRow.gameObject, this.originalRow.transform.parent.gameObject, false).GetComponent<OwnablesSecondSideScreenRow>();
		component.OnRowClicked = (Action<OwnablesSecondSideScreenRow>)Delegate.Combine(component.OnRowClicked, new Action<OwnablesSecondSideScreenRow>(this.OnItemRowClicked));
		component.OnRowItemAssigneeChanged = (Action<OwnablesSecondSideScreenRow>)Delegate.Combine(component.OnRowItemAssigneeChanged, new Action<OwnablesSecondSideScreenRow>(this.OnItemRowAsigneeChanged));
		component.OnRowItemDestroyed = (Action<OwnablesSecondSideScreenRow>)Delegate.Combine(component.OnRowItemDestroyed, new Action<OwnablesSecondSideScreenRow>(this.OnItemDestroyed));
		return component;
	}

	// Token: 0x0600AD0C RID: 44300 RVA: 0x00114E4C File Offset: 0x0011304C
	private void OnItemDestroyed(OwnablesSecondSideScreenRow correspondingItemRow)
	{
		correspondingItemRow.ClearData();
		correspondingItemRow.gameObject.SetActive(false);
	}

	// Token: 0x0600AD0D RID: 44301 RVA: 0x00114E60 File Offset: 0x00113060
	private void OnItemRowAsigneeChanged(OwnablesSecondSideScreenRow correspondingItemRow)
	{
		correspondingItemRow.Refresh();
		this.RefreshNoneRow();
	}

	// Token: 0x0600AD0E RID: 44302 RVA: 0x00420A68 File Offset: 0x0041EC68
	private void OnItemRowClicked(OwnablesSecondSideScreenRow rowClicked)
	{
		Assignable item = rowClicked.item;
		bool flag = item.IsAssigned() && item.assignee is AssignmentGroup;
		bool flag2 = item.IsAssigned() && item.IsAssignedTo(this.OwnerIdentity) && !flag && this.Slot.IsAssigned() && this.Slot.assignable == item;
		if (item.IsAssigned())
		{
			item.Unassign();
		}
		if (!flag2)
		{
			item.Assign(this.OwnerIdentity, this.Slot);
		}
		rowClicked.Refresh();
		this.RefreshNoneRow();
	}

	// Token: 0x0600AD0F RID: 44303 RVA: 0x00114E6E File Offset: 0x0011306E
	private void UnassignCurrentItem()
	{
		if (this.Slot != null)
		{
			this.Slot.Unassign(true);
			this.RefreshItemListOptions(false);
		}
	}

	// Token: 0x0600AD10 RID: 44304 RVA: 0x00114E8B File Offset: 0x0011308B
	private void OnNewItemAvailable(Assignable item)
	{
		if (this.Slot != null && item.slotID == this.SlotType.Id)
		{
			this.RefreshItemListOptions(false);
		}
	}

	// Token: 0x0600AD11 RID: 44305 RVA: 0x00114E8B File Offset: 0x0011308B
	private void OnItemUnregistered(Assignable item)
	{
		if (this.Slot != null && item.slotID == this.SlotType.Id)
		{
			this.RefreshItemListOptions(false);
		}
	}

	// Token: 0x04008829 RID: 34857
	public MultiToggle noneRow;

	// Token: 0x0400882A RID: 34858
	public OwnablesSecondSideScreenRow originalRow;

	// Token: 0x0400882D RID: 34861
	public System.Action OnScreenDeactivated;

	// Token: 0x0400882E RID: 34862
	private List<OwnablesSecondSideScreenRow> itemRows = new List<OwnablesSecondSideScreenRow>();
}
