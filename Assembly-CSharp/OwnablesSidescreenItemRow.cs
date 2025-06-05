using System;
using STRINGS;
using UnityEngine;

// Token: 0x02002005 RID: 8197
public class OwnablesSidescreenItemRow : KMonoBehaviour
{
	// Token: 0x17000B19 RID: 2841
	// (get) Token: 0x0600AD4D RID: 44365 RVA: 0x0011519D File Offset: 0x0011339D
	// (set) Token: 0x0600AD4C RID: 44364 RVA: 0x00115194 File Offset: 0x00113394
	public bool IsLocked { get; private set; }

	// Token: 0x17000B1A RID: 2842
	// (get) Token: 0x0600AD4E RID: 44366 RVA: 0x001151A5 File Offset: 0x001133A5
	public bool SlotIsAssigned
	{
		get
		{
			return this.Slot != null && this.SlotInstance != null && !this.SlotInstance.IsUnassigning() && this.SlotInstance.IsAssigned();
		}
	}

	// Token: 0x17000B1B RID: 2843
	// (get) Token: 0x0600AD50 RID: 44368 RVA: 0x001151DA File Offset: 0x001133DA
	// (set) Token: 0x0600AD4F RID: 44367 RVA: 0x001151D1 File Offset: 0x001133D1
	public AssignableSlotInstance SlotInstance { get; private set; }

	// Token: 0x17000B1C RID: 2844
	// (get) Token: 0x0600AD52 RID: 44370 RVA: 0x001151EB File Offset: 0x001133EB
	// (set) Token: 0x0600AD51 RID: 44369 RVA: 0x001151E2 File Offset: 0x001133E2
	public AssignableSlot Slot { get; private set; }

	// Token: 0x17000B1D RID: 2845
	// (get) Token: 0x0600AD54 RID: 44372 RVA: 0x001151FC File Offset: 0x001133FC
	// (set) Token: 0x0600AD53 RID: 44371 RVA: 0x001151F3 File Offset: 0x001133F3
	public Assignables Owner { get; private set; }

	// Token: 0x0600AD55 RID: 44373 RVA: 0x00115204 File Offset: 0x00113404
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		MultiToggle multiToggle = this.toggle;
		multiToggle.onClick = (System.Action)Delegate.Combine(multiToggle.onClick, new System.Action(this.OnRowClicked));
		this.SetSelectedVisualState(false);
	}

	// Token: 0x0600AD56 RID: 44374 RVA: 0x0011523A File Offset: 0x0011343A
	private void OnRowClicked()
	{
		Action<OwnablesSidescreenItemRow> onSlotRowClicked = this.OnSlotRowClicked;
		if (onSlotRowClicked == null)
		{
			return;
		}
		onSlotRowClicked(this);
	}

	// Token: 0x0600AD57 RID: 44375 RVA: 0x0011524D File Offset: 0x0011344D
	public void SetLockState(bool locked)
	{
		this.IsLocked = locked;
		this.Refresh();
	}

	// Token: 0x0600AD58 RID: 44376 RVA: 0x00421670 File Offset: 0x0041F870
	public void SetData(Assignables owner, AssignableSlot slot, bool IsLocked)
	{
		if (this.Owner != null)
		{
			this.ClearData();
		}
		this.Owner = owner;
		this.Slot = slot;
		this.SlotInstance = owner.GetSlot(slot);
		this.subscribe_IDX = this.Owner.Subscribe(-1585839766, delegate(object o)
		{
			this.Refresh();
		});
		this.SetLockState(IsLocked);
		if (!IsLocked)
		{
			this.Refresh();
		}
	}

	// Token: 0x0600AD59 RID: 44377 RVA: 0x004216E0 File Offset: 0x0041F8E0
	public void ClearData()
	{
		if (this.Owner != null && this.subscribe_IDX != -1)
		{
			this.Owner.Unsubscribe(this.subscribe_IDX);
		}
		this.Owner = null;
		this.Slot = null;
		this.SlotInstance = null;
		this.IsLocked = false;
		this.subscribe_IDX = -1;
		this.DisplayAsEmpty();
	}

	// Token: 0x0600AD5A RID: 44378 RVA: 0x0011525C File Offset: 0x0011345C
	private void Refresh()
	{
		if (this.IsNullOrDestroyed())
		{
			return;
		}
		if (this.IsLocked)
		{
			this.DisplayAsLocked();
			return;
		}
		if (!this.SlotIsAssigned)
		{
			this.DisplayAsEmpty();
			return;
		}
		this.DisplayAsOccupied();
	}

	// Token: 0x0600AD5B RID: 44379 RVA: 0x00421740 File Offset: 0x0041F940
	public void SetSelectedVisualState(bool shouldDisplayAsSelected)
	{
		int new_state_index = shouldDisplayAsSelected ? 1 : 0;
		this.toggle.ChangeState(new_state_index);
	}

	// Token: 0x0600AD5C RID: 44380 RVA: 0x00421764 File Offset: 0x0041F964
	private void DisplayAsOccupied()
	{
		Assignable assignable = this.SlotInstance.assignable;
		string properName = assignable.GetProperName();
		string text = this.Slot.Name + ": " + properName;
		this.textLabel.SetText(text);
		this.itemIcon.sprite = Def.GetUISprite(assignable.gameObject, "ui", false).first;
		this.itemIcon.gameObject.SetActive(true);
		this.lockedIcon.gameObject.SetActive(false);
		InfoDescription component = assignable.gameObject.GetComponent<InfoDescription>();
		string simpleTooltip = string.Format(UI.UISIDESCREENS.OWNABLESSIDESCREEN.TOOLTIPS.ITEM_ASSIGNED_GENERIC, properName);
		if (component != null && !string.IsNullOrEmpty(component.description))
		{
			simpleTooltip = string.Format(UI.UISIDESCREENS.OWNABLESSIDESCREEN.TOOLTIPS.ITEM_ASSIGNED, properName, component.description);
		}
		this.tooltip.SetSimpleTooltip(simpleTooltip);
	}

	// Token: 0x0600AD5D RID: 44381 RVA: 0x00421844 File Offset: 0x0041FA44
	private void DisplayAsEmpty()
	{
		this.textLabel.SetText(((this.Slot != null) ? (this.Slot.Name + ": ") : "") + OwnablesSidescreenItemRow.EMPTY_TEXT);
		this.lockedIcon.gameObject.SetActive(false);
		this.itemIcon.sprite = null;
		this.itemIcon.gameObject.SetActive(false);
		this.tooltip.SetSimpleTooltip((this.Slot != null) ? string.Format(UI.UISIDESCREENS.OWNABLESSIDESCREEN.TOOLTIPS.NO_ITEM_ASSIGNED, this.Slot.Name) : null);
	}

	// Token: 0x0600AD5E RID: 44382 RVA: 0x004218E8 File Offset: 0x0041FAE8
	private void DisplayAsLocked()
	{
		this.lockedIcon.gameObject.SetActive(true);
		this.itemIcon.sprite = null;
		this.itemIcon.gameObject.SetActive(false);
		this.textLabel.SetText(string.Format(UI.UISIDESCREENS.OWNABLESSIDESCREEN.NO_APPLICABLE, this.Slot.Name));
		this.tooltip.SetSimpleTooltip(string.Format(UI.UISIDESCREENS.OWNABLESSIDESCREEN.TOOLTIPS.NO_APPLICABLE, this.Slot.Name));
	}

	// Token: 0x0600AD5F RID: 44383 RVA: 0x0011528B File Offset: 0x0011348B
	protected override void OnCleanUp()
	{
		this.ClearData();
	}

	// Token: 0x04008865 RID: 34917
	private static string EMPTY_TEXT = UI.UISIDESCREENS.OWNABLESSIDESCREEN.NO_ITEM_ASSIGNED;

	// Token: 0x04008866 RID: 34918
	public KImage lockedIcon;

	// Token: 0x04008867 RID: 34919
	public KImage itemIcon;

	// Token: 0x04008868 RID: 34920
	public LocText textLabel;

	// Token: 0x04008869 RID: 34921
	public ToolTip tooltip;

	// Token: 0x0400886A RID: 34922
	[Header("Icon settings")]
	public KImage frameOuterBorder;

	// Token: 0x0400886B RID: 34923
	public Action<OwnablesSidescreenItemRow> OnSlotRowClicked;

	// Token: 0x04008870 RID: 34928
	public MultiToggle toggle;

	// Token: 0x04008871 RID: 34929
	private int subscribe_IDX = -1;
}
