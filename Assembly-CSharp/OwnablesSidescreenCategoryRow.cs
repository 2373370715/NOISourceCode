using System;

// Token: 0x02002002 RID: 8194
public class OwnablesSidescreenCategoryRow : KMonoBehaviour
{
	// Token: 0x17000B18 RID: 2840
	// (get) Token: 0x0600AD40 RID: 44352 RVA: 0x00115125 File Offset: 0x00113325
	private AssignableSlot[] slots
	{
		get
		{
			return this.data.slots;
		}
	}

	// Token: 0x0600AD41 RID: 44353 RVA: 0x00115132 File Offset: 0x00113332
	public void SetCategoryData(OwnablesSidescreenCategoryRow.Data categoryData)
	{
		this.DeleteAllRows();
		this.data = categoryData;
		this.titleLabel.text = categoryData.name;
	}

	// Token: 0x0600AD42 RID: 44354 RVA: 0x00115152 File Offset: 0x00113352
	public void SetOwner(Assignables owner)
	{
		this.owner = owner;
		if (owner != null)
		{
			this.RecreateAllItemRows();
			return;
		}
		this.DeleteAllRows();
	}

	// Token: 0x0600AD43 RID: 44355 RVA: 0x0042144C File Offset: 0x0041F64C
	private void RecreateAllItemRows()
	{
		this.DeleteAllRows();
		this.itemRows = new OwnablesSidescreenItemRow[this.slots.Length];
		IAssignableIdentity component = this.owner.gameObject.GetComponent<IAssignableIdentity>();
		for (int i = 0; i < this.slots.Length; i++)
		{
			AssignableSlot slot = this.slots[i];
			this.itemRows[i] = this.CreateRow(slot, component);
		}
	}

	// Token: 0x0600AD44 RID: 44356 RVA: 0x004214B0 File Offset: 0x0041F6B0
	private OwnablesSidescreenItemRow CreateRow(AssignableSlot slot, IAssignableIdentity ownerIdentity)
	{
		this.originalItemRow.gameObject.SetActive(false);
		OwnablesSidescreenItemRow component = Util.KInstantiateUI(this.originalItemRow.gameObject, this.originalItemRow.transform.parent.gameObject, false).GetComponent<OwnablesSidescreenItemRow>();
		component.OnSlotRowClicked = (Action<OwnablesSidescreenItemRow>)Delegate.Combine(component.OnSlotRowClicked, new Action<OwnablesSidescreenItemRow>(this.OnRowClicked));
		component.gameObject.SetActive(true);
		component.SetData(this.owner, slot, !this.data.IsSlotApplicable(ownerIdentity, slot));
		return component;
	}

	// Token: 0x0600AD45 RID: 44357 RVA: 0x00115171 File Offset: 0x00113371
	private void OnRowClicked(OwnablesSidescreenItemRow row)
	{
		Action<OwnablesSidescreenItemRow> onSlotRowClicked = this.OnSlotRowClicked;
		if (onSlotRowClicked == null)
		{
			return;
		}
		onSlotRowClicked(row);
	}

	// Token: 0x0600AD46 RID: 44358 RVA: 0x00421544 File Offset: 0x0041F744
	private void DeleteAllRows()
	{
		this.originalItemRow.gameObject.SetActive(false);
		if (this.itemRows != null)
		{
			for (int i = 0; i < this.itemRows.Length; i++)
			{
				this.itemRows[i].ClearData();
				this.itemRows[i].DeleteObject();
			}
			this.itemRows = null;
		}
	}

	// Token: 0x0600AD47 RID: 44359 RVA: 0x004215A0 File Offset: 0x0041F7A0
	public void SetSelectedRow_VisualsOnly(AssignableSlotInstance slotInstance)
	{
		if (this.itemRows == null)
		{
			return;
		}
		for (int i = 0; i < this.itemRows.Length; i++)
		{
			OwnablesSidescreenItemRow ownablesSidescreenItemRow = this.itemRows[i];
			ownablesSidescreenItemRow.SetSelectedVisualState(ownablesSidescreenItemRow.SlotInstance == slotInstance);
		}
	}

	// Token: 0x0400885A RID: 34906
	public Action<OwnablesSidescreenItemRow> OnSlotRowClicked;

	// Token: 0x0400885B RID: 34907
	public LocText titleLabel;

	// Token: 0x0400885C RID: 34908
	public OwnablesSidescreenItemRow originalItemRow;

	// Token: 0x0400885D RID: 34909
	private Assignables owner;

	// Token: 0x0400885E RID: 34910
	private OwnablesSidescreenCategoryRow.Data data;

	// Token: 0x0400885F RID: 34911
	private OwnablesSidescreenItemRow[] itemRows;

	// Token: 0x02002003 RID: 8195
	public struct AssignableSlotData
	{
		// Token: 0x0600AD49 RID: 44361 RVA: 0x00115184 File Offset: 0x00113384
		public AssignableSlotData(AssignableSlot slot, Func<IAssignableIdentity, bool> isApplicableCallback)
		{
			this.slot = slot;
			this.IsApplicableCallback = isApplicableCallback;
		}

		// Token: 0x04008860 RID: 34912
		public AssignableSlot slot;

		// Token: 0x04008861 RID: 34913
		public Func<IAssignableIdentity, bool> IsApplicableCallback;
	}

	// Token: 0x02002004 RID: 8196
	public struct Data
	{
		// Token: 0x0600AD4A RID: 44362 RVA: 0x004215E0 File Offset: 0x0041F7E0
		public Data(string name, OwnablesSidescreenCategoryRow.AssignableSlotData[] slotsData)
		{
			this.name = name;
			this.slotsData = slotsData;
			this.slots = new AssignableSlot[slotsData.Length];
			for (int i = 0; i < slotsData.Length; i++)
			{
				this.slots[i] = slotsData[i].slot;
			}
		}

		// Token: 0x0600AD4B RID: 44363 RVA: 0x0042162C File Offset: 0x0041F82C
		public bool IsSlotApplicable(IAssignableIdentity identity, AssignableSlot slot)
		{
			for (int i = 0; i < this.slotsData.Length; i++)
			{
				OwnablesSidescreenCategoryRow.AssignableSlotData assignableSlotData = this.slotsData[i];
				if (assignableSlotData.slot == slot)
				{
					return assignableSlotData.IsApplicableCallback(identity);
				}
			}
			return false;
		}

		// Token: 0x04008862 RID: 34914
		public string name;

		// Token: 0x04008863 RID: 34915
		public AssignableSlot[] slots;

		// Token: 0x04008864 RID: 34916
		private OwnablesSidescreenCategoryRow.AssignableSlotData[] slotsData;
	}
}
