using System;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02001FFE RID: 8190
public class OwnablesSidescreen : SideScreenContent
{
	// Token: 0x0600AD26 RID: 44326 RVA: 0x00421010 File Offset: 0x0041F210
	private void DefineCategories()
	{
		if (this.categories == null)
		{
			OwnablesSidescreen.Category[] array = new OwnablesSidescreen.Category[2];
			array[0] = new OwnablesSidescreen.Category((IAssignableIdentity assignableIdentity) => (assignableIdentity as MinionIdentity).GetEquipment(), new OwnablesSidescreenCategoryRow.Data(UI.UISIDESCREENS.OWNABLESSIDESCREEN.CATEGORIES.SUITS, new OwnablesSidescreenCategoryRow.AssignableSlotData[]
			{
				new OwnablesSidescreenCategoryRow.AssignableSlotData(Db.Get().AssignableSlots.Suit, new Func<IAssignableIdentity, bool>(this.Always)),
				new OwnablesSidescreenCategoryRow.AssignableSlotData(Db.Get().AssignableSlots.Outfit, new Func<IAssignableIdentity, bool>(this.Always))
			}));
			array[1] = new OwnablesSidescreen.Category((IAssignableIdentity assignableIdentity) => assignableIdentity.GetSoleOwner(), new OwnablesSidescreenCategoryRow.Data(UI.UISIDESCREENS.OWNABLESSIDESCREEN.CATEGORIES.AMENITIES, new OwnablesSidescreenCategoryRow.AssignableSlotData[]
			{
				new OwnablesSidescreenCategoryRow.AssignableSlotData(Db.Get().AssignableSlots.Bed, new Func<IAssignableIdentity, bool>(this.Always)),
				new OwnablesSidescreenCategoryRow.AssignableSlotData(Db.Get().AssignableSlots.Toilet, new Func<IAssignableIdentity, bool>(this.Always)),
				new OwnablesSidescreenCategoryRow.AssignableSlotData(Db.Get().AssignableSlots.MessStation, this.HasAmount("Calories"))
			}));
			this.categories = array;
		}
	}

	// Token: 0x0600AD27 RID: 44327 RVA: 0x000AA7E7 File Offset: 0x000A89E7
	private bool Always(IAssignableIdentity identity)
	{
		return true;
	}

	// Token: 0x0600AD28 RID: 44328 RVA: 0x00114FAB File Offset: 0x001131AB
	private Func<IAssignableIdentity, bool> HasAmount(string amountID)
	{
		return delegate(IAssignableIdentity identity)
		{
			if (identity == null)
			{
				return false;
			}
			GameObject targetGameObject = identity.GetOwners()[0].GetComponent<MinionAssignablesProxy>().GetTargetGameObject();
			return Db.Get().Amounts.Get(amountID).Lookup(targetGameObject) != null;
		};
	}

	// Token: 0x0600AD29 RID: 44329 RVA: 0x00107377 File Offset: 0x00105577
	protected override void OnSpawn()
	{
		base.OnSpawn();
	}

	// Token: 0x0600AD2A RID: 44330 RVA: 0x00114FC4 File Offset: 0x001131C4
	private void ActivateSecondSidescreen(AssignableSlotInstance slot)
	{
		((OwnablesSecondSideScreen)DetailsScreen.Instance.SetSecondarySideScreen(this.selectedSlotScreenPrefab, slot.slot.Name)).SetSlot(slot);
		if (slot != null && this.OnSlotInstanceSelected != null)
		{
			this.OnSlotInstanceSelected(slot);
		}
	}

	// Token: 0x0600AD2B RID: 44331 RVA: 0x00115003 File Offset: 0x00113203
	private void DeactivateSecondScreen()
	{
		DetailsScreen.Instance.ClearSecondarySideScreen();
	}

	// Token: 0x0600AD2C RID: 44332 RVA: 0x00421178 File Offset: 0x0041F378
	public override void SetTarget(GameObject target)
	{
		base.SetTarget(target);
		this.UnsubscribeFromLastTarget();
		this.lastSelectedSlot = null;
		this.DefineCategories();
		this.CreateCategoryRows();
		this.DeactivateSecondScreen();
		this.RefreshSelectedStatusOnRows();
		IAssignableIdentity component = target.GetComponent<IAssignableIdentity>();
		for (int i = 0; i < this.categoryRows.Length; i++)
		{
			Assignables owner = this.categories[i].getAssignablesFn(component);
			this.categoryRows[i].SetOwner(owner);
		}
		this.titleSection.SetActive(target.GetComponent<MinionIdentity>().model == BionicMinionConfig.MODEL);
		MinionIdentity minionIdentity = component as MinionIdentity;
		if (minionIdentity != null)
		{
			this.lastTarget = minionIdentity;
			this.minionDestroyedCallbackIDX = minionIdentity.gameObject.Subscribe(1502190696, new Action<object>(this.OnTargetDestroyed));
		}
	}

	// Token: 0x0600AD2D RID: 44333 RVA: 0x0011500F File Offset: 0x0011320F
	private void OnTargetDestroyed(object o)
	{
		this.ClearTarget();
	}

	// Token: 0x0600AD2E RID: 44334 RVA: 0x0042124C File Offset: 0x0041F44C
	public override void ClearTarget()
	{
		base.ClearTarget();
		this.lastSelectedSlot = null;
		this.RefreshSelectedStatusOnRows();
		for (int i = 0; i < this.categoryRows.Length; i++)
		{
			this.categoryRows[i].SetOwner(null);
		}
		this.DeactivateSecondScreen();
		this.UnsubscribeFromLastTarget();
	}

	// Token: 0x0600AD2F RID: 44335 RVA: 0x0042129C File Offset: 0x0041F49C
	private void CreateCategoryRows()
	{
		if (this.categoryRows == null)
		{
			this.originalCategoryRow.gameObject.SetActive(false);
			this.categoryRows = new OwnablesSidescreenCategoryRow[this.categories.Length];
			for (int i = 0; i < this.categories.Length; i++)
			{
				OwnablesSidescreenCategoryRow.Data data = this.categories[i].data;
				OwnablesSidescreenCategoryRow component = Util.KInstantiateUI(this.originalCategoryRow.gameObject, this.originalCategoryRow.transform.parent.gameObject, false).GetComponent<OwnablesSidescreenCategoryRow>();
				OwnablesSidescreenCategoryRow ownablesSidescreenCategoryRow = component;
				ownablesSidescreenCategoryRow.OnSlotRowClicked = (Action<OwnablesSidescreenItemRow>)Delegate.Combine(ownablesSidescreenCategoryRow.OnSlotRowClicked, new Action<OwnablesSidescreenItemRow>(this.OnSlotRowClicked));
				component.gameObject.SetActive(true);
				component.SetCategoryData(data);
				this.categoryRows[i] = component;
			}
			this.RefreshSelectedStatusOnRows();
		}
	}

	// Token: 0x0600AD30 RID: 44336 RVA: 0x00115017 File Offset: 0x00113217
	private void OnSlotRowClicked(OwnablesSidescreenItemRow slotRow)
	{
		if (slotRow.IsLocked || slotRow.SlotInstance == this.lastSelectedSlot)
		{
			this.SetSelectedSlot(null);
			return;
		}
		this.SetSelectedSlot(slotRow.SlotInstance);
	}

	// Token: 0x0600AD31 RID: 44337 RVA: 0x00421374 File Offset: 0x0041F574
	public void RefreshSelectedStatusOnRows()
	{
		if (this.categoryRows == null)
		{
			return;
		}
		for (int i = 0; i < this.categoryRows.Length; i++)
		{
			this.categoryRows[i].SetSelectedRow_VisualsOnly(this.lastSelectedSlot);
		}
	}

	// Token: 0x0600AD32 RID: 44338 RVA: 0x00115043 File Offset: 0x00113243
	public void SetSelectedSlot(AssignableSlotInstance slot)
	{
		this.lastSelectedSlot = slot;
		if (slot != null)
		{
			this.ActivateSecondSidescreen(slot);
		}
		else
		{
			this.DeactivateSecondScreen();
		}
		this.RefreshSelectedStatusOnRows();
	}

	// Token: 0x0600AD33 RID: 44339 RVA: 0x004213B0 File Offset: 0x0041F5B0
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		if (this.categoryRows != null)
		{
			for (int i = 0; i < this.categoryRows.Length; i++)
			{
				if (this.categoryRows[i] != null)
				{
					this.categoryRows[i].SetOwner(null);
				}
			}
		}
		this.UnsubscribeFromLastTarget();
	}

	// Token: 0x0600AD34 RID: 44340 RVA: 0x00115064 File Offset: 0x00113264
	private void UnsubscribeFromLastTarget()
	{
		if (this.lastTarget != null && this.minionDestroyedCallbackIDX != -1)
		{
			this.lastTarget.Unsubscribe(this.minionDestroyedCallbackIDX);
		}
		this.minionDestroyedCallbackIDX = -1;
		this.lastTarget = null;
	}

	// Token: 0x0600AD35 RID: 44341 RVA: 0x0011509C File Offset: 0x0011329C
	public override bool IsValidForTarget(GameObject target)
	{
		return target.GetComponent<IAssignableIdentity>() != null;
	}

	// Token: 0x0600AD36 RID: 44342 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnValidate()
	{
	}

	// Token: 0x0600AD37 RID: 44343 RVA: 0x001150A7 File Offset: 0x001132A7
	private void SetScrollBarVisibility(bool isVisible)
	{
		this.scrollbarSection.gameObject.SetActive(isVisible);
		this.mainLayoutGroup.padding.right = (isVisible ? 20 : 0);
		this.scrollRect.enabled = isVisible;
	}

	// Token: 0x04008847 RID: 34887
	public OwnablesSecondSideScreen selectedSlotScreenPrefab;

	// Token: 0x04008848 RID: 34888
	public OwnablesSidescreenCategoryRow originalCategoryRow;

	// Token: 0x04008849 RID: 34889
	[Header("Editor Settings")]
	public bool usingSlider = true;

	// Token: 0x0400884A RID: 34890
	public GameObject titleSection;

	// Token: 0x0400884B RID: 34891
	public GameObject scrollbarSection;

	// Token: 0x0400884C RID: 34892
	public VerticalLayoutGroup mainLayoutGroup;

	// Token: 0x0400884D RID: 34893
	public KScrollRect scrollRect;

	// Token: 0x0400884E RID: 34894
	private OwnablesSidescreenCategoryRow[] categoryRows;

	// Token: 0x0400884F RID: 34895
	private AssignableSlotInstance lastSelectedSlot;

	// Token: 0x04008850 RID: 34896
	private OwnablesSidescreen.Category[] categories;

	// Token: 0x04008851 RID: 34897
	public Action<AssignableSlotInstance> OnSlotInstanceSelected;

	// Token: 0x04008852 RID: 34898
	private MinionIdentity lastTarget;

	// Token: 0x04008853 RID: 34899
	private int minionDestroyedCallbackIDX = -1;

	// Token: 0x02001FFF RID: 8191
	public struct Category
	{
		// Token: 0x0600AD39 RID: 44345 RVA: 0x001150F4 File Offset: 0x001132F4
		public Category(Func<IAssignableIdentity, Assignables> getAssignablesFn, OwnablesSidescreenCategoryRow.Data categoryData)
		{
			this.getAssignablesFn = getAssignablesFn;
			this.data = categoryData;
		}

		// Token: 0x04008854 RID: 34900
		public Func<IAssignableIdentity, Assignables> getAssignablesFn;

		// Token: 0x04008855 RID: 34901
		public OwnablesSidescreenCategoryRow.Data data;
	}
}
