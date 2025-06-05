using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x02001F8E RID: 8078
public class AssignableSideScreen : SideScreenContent
{
	// Token: 0x17000AEC RID: 2796
	// (get) Token: 0x0600AAA2 RID: 43682 RVA: 0x00113523 File Offset: 0x00111723
	// (set) Token: 0x0600AAA3 RID: 43683 RVA: 0x0011352B File Offset: 0x0011172B
	public Assignable targetAssignable { get; private set; }

	// Token: 0x0600AAA4 RID: 43684 RVA: 0x00113534 File Offset: 0x00111734
	public override string GetTitle()
	{
		if (this.targetAssignable != null)
		{
			return string.Format(base.GetTitle(), this.targetAssignable.GetProperName());
		}
		return base.GetTitle();
	}

	// Token: 0x0600AAA5 RID: 43685 RVA: 0x00416064 File Offset: 0x00414264
	protected override void OnSpawn()
	{
		base.OnSpawn();
		MultiToggle multiToggle = this.dupeSortingToggle;
		multiToggle.onClick = (System.Action)Delegate.Combine(multiToggle.onClick, new System.Action(delegate()
		{
			this.SortByName(true);
		}));
		MultiToggle multiToggle2 = this.generalSortingToggle;
		multiToggle2.onClick = (System.Action)Delegate.Combine(multiToggle2.onClick, new System.Action(delegate()
		{
			this.SortByAssignment(true);
		}));
		base.Subscribe(Game.Instance.gameObject, 875045922, new Action<object>(this.OnRefreshData));
	}

	// Token: 0x0600AAA6 RID: 43686 RVA: 0x00113561 File Offset: 0x00111761
	private void OnRefreshData(object obj)
	{
		this.SetTarget(this.targetAssignable.gameObject);
	}

	// Token: 0x0600AAA7 RID: 43687 RVA: 0x004160E8 File Offset: 0x004142E8
	public override void ClearTarget()
	{
		if (this.targetAssignableSubscriptionHandle != -1 && this.targetAssignable != null)
		{
			this.targetAssignable.Unsubscribe(this.targetAssignableSubscriptionHandle);
			this.targetAssignableSubscriptionHandle = -1;
		}
		this.targetAssignable = null;
		Components.LiveMinionIdentities.OnAdd -= this.OnMinionIdentitiesChanged;
		Components.LiveMinionIdentities.OnRemove -= this.OnMinionIdentitiesChanged;
		base.ClearTarget();
	}

	// Token: 0x0600AAA8 RID: 43688 RVA: 0x00113574 File Offset: 0x00111774
	public override bool IsValidForTarget(GameObject target)
	{
		return target.GetComponent<Assignable>() != null && target.GetComponent<Assignable>().CanBeAssigned && target.GetComponent<AssignmentGroupController>() == null;
	}

	// Token: 0x0600AAA9 RID: 43689 RVA: 0x00416160 File Offset: 0x00414360
	public override void SetTarget(GameObject target)
	{
		Components.LiveMinionIdentities.OnAdd += this.OnMinionIdentitiesChanged;
		Components.LiveMinionIdentities.OnRemove += this.OnMinionIdentitiesChanged;
		if (this.targetAssignableSubscriptionHandle != -1 && this.targetAssignable != null)
		{
			this.targetAssignable.Unsubscribe(this.targetAssignableSubscriptionHandle);
		}
		this.targetAssignable = target.GetComponent<Assignable>();
		if (this.targetAssignable == null)
		{
			global::Debug.LogError(string.Format("{0} selected has no Assignable component.", target.GetProperName()));
			return;
		}
		if (this.rowPool == null)
		{
			this.rowPool = new UIPool<AssignableSideScreenRow>(this.rowPrefab);
		}
		base.gameObject.SetActive(true);
		this.identityList = new List<MinionAssignablesProxy>(Components.MinionAssignablesProxy.Items);
		this.dupeSortingToggle.ChangeState(0);
		this.generalSortingToggle.ChangeState(0);
		this.activeSortToggle = null;
		this.activeSortFunction = null;
		if (!this.targetAssignable.CanBeAssigned)
		{
			this.HideScreen(true);
		}
		else
		{
			this.HideScreen(false);
		}
		this.targetAssignableSubscriptionHandle = this.targetAssignable.Subscribe(684616645, new Action<object>(this.OnAssigneeChanged));
		this.Refresh(this.identityList);
		this.SortByAssignment(false);
	}

	// Token: 0x0600AAAA RID: 43690 RVA: 0x0011359F File Offset: 0x0011179F
	private void OnMinionIdentitiesChanged(MinionIdentity change)
	{
		this.identityList = new List<MinionAssignablesProxy>(Components.MinionAssignablesProxy.Items);
		this.Refresh(this.identityList);
	}

	// Token: 0x0600AAAB RID: 43691 RVA: 0x004162A4 File Offset: 0x004144A4
	private void OnAssigneeChanged(object data = null)
	{
		foreach (KeyValuePair<IAssignableIdentity, AssignableSideScreenRow> keyValuePair in this.identityRowMap)
		{
			keyValuePair.Value.Refresh(null);
		}
	}

	// Token: 0x0600AAAC RID: 43692 RVA: 0x00416300 File Offset: 0x00414500
	private void Refresh(List<MinionAssignablesProxy> identities)
	{
		this.ClearContent();
		this.currentOwnerText.text = string.Format(UI.UISIDESCREENS.ASSIGNABLESIDESCREEN.UNASSIGNED, Array.Empty<object>());
		if (this.targetAssignable == null)
		{
			return;
		}
		if (this.targetAssignable.GetComponent<Equippable>() == null && !this.targetAssignable.HasTag(GameTags.NotRoomAssignable))
		{
			Room roomOfGameObject = Game.Instance.roomProber.GetRoomOfGameObject(this.targetAssignable.gameObject);
			if (roomOfGameObject != null)
			{
				RoomType roomType = roomOfGameObject.roomType;
				if (roomType.primary_constraint != null && !roomType.primary_constraint.building_criteria(this.targetAssignable.GetComponent<KPrefabID>()))
				{
					AssignableSideScreenRow freeElement = this.rowPool.GetFreeElement(this.rowGroup, true);
					freeElement.sideScreen = this;
					this.identityRowMap.Add(roomOfGameObject, freeElement);
					freeElement.SetContent(roomOfGameObject, new Action<IAssignableIdentity>(this.OnRowClicked), this);
					return;
				}
			}
		}
		if (this.targetAssignable.canBePublic)
		{
			AssignableSideScreenRow freeElement2 = this.rowPool.GetFreeElement(this.rowGroup, true);
			freeElement2.sideScreen = this;
			freeElement2.transform.SetAsFirstSibling();
			this.identityRowMap.Add(Game.Instance.assignmentManager.assignment_groups["public"], freeElement2);
			freeElement2.SetContent(Game.Instance.assignmentManager.assignment_groups["public"], new Action<IAssignableIdentity>(this.OnRowClicked), this);
		}
		foreach (MinionAssignablesProxy minionAssignablesProxy in identities)
		{
			AssignableSideScreenRow freeElement3 = this.rowPool.GetFreeElement(this.rowGroup, true);
			freeElement3.sideScreen = this;
			this.identityRowMap.Add(minionAssignablesProxy, freeElement3);
			freeElement3.SetContent(minionAssignablesProxy, new Action<IAssignableIdentity>(this.OnRowClicked), this);
		}
		this.ExecuteSort(this.activeSortFunction);
	}

	// Token: 0x0600AAAD RID: 43693 RVA: 0x001135C2 File Offset: 0x001117C2
	private void SortByName(bool reselect)
	{
		this.SelectSortToggle(this.dupeSortingToggle, reselect);
		this.ExecuteSort((IAssignableIdentity i1, IAssignableIdentity i2) => i1.GetProperName().CompareTo(i2.GetProperName()) * (this.sortReversed ? -1 : 1));
	}

	// Token: 0x0600AAAE RID: 43694 RVA: 0x00416500 File Offset: 0x00414700
	private void SortByAssignment(bool reselect)
	{
		this.SelectSortToggle(this.generalSortingToggle, reselect);
		Comparison<IAssignableIdentity> sortFunction = delegate(IAssignableIdentity i1, IAssignableIdentity i2)
		{
			int num = this.targetAssignable.CanAssignTo(i1).CompareTo(this.targetAssignable.CanAssignTo(i2));
			if (num != 0)
			{
				return num * -1;
			}
			num = this.identityRowMap[i1].currentState.CompareTo(this.identityRowMap[i2].currentState);
			if (num != 0)
			{
				return num * (this.sortReversed ? -1 : 1);
			}
			return i1.GetProperName().CompareTo(i2.GetProperName());
		};
		this.ExecuteSort(sortFunction);
	}

	// Token: 0x0600AAAF RID: 43695 RVA: 0x00416530 File Offset: 0x00414730
	private void SelectSortToggle(MultiToggle toggle, bool reselect)
	{
		this.dupeSortingToggle.ChangeState(0);
		this.generalSortingToggle.ChangeState(0);
		if (toggle != null)
		{
			if (reselect && this.activeSortToggle == toggle)
			{
				this.sortReversed = !this.sortReversed;
			}
			this.activeSortToggle = toggle;
		}
		this.activeSortToggle.ChangeState(this.sortReversed ? 2 : 1);
	}

	// Token: 0x0600AAB0 RID: 43696 RVA: 0x0041659C File Offset: 0x0041479C
	private void ExecuteSort(Comparison<IAssignableIdentity> sortFunction)
	{
		if (sortFunction != null)
		{
			List<IAssignableIdentity> list = new List<IAssignableIdentity>(this.identityRowMap.Keys);
			list.Sort(sortFunction);
			for (int i = 0; i < list.Count; i++)
			{
				this.identityRowMap[list[i]].transform.SetSiblingIndex(i);
			}
			this.activeSortFunction = sortFunction;
		}
	}

	// Token: 0x0600AAB1 RID: 43697 RVA: 0x004165FC File Offset: 0x004147FC
	private void ClearContent()
	{
		if (this.rowPool != null)
		{
			this.rowPool.DestroyAll();
		}
		foreach (KeyValuePair<IAssignableIdentity, AssignableSideScreenRow> keyValuePair in this.identityRowMap)
		{
			keyValuePair.Value.targetIdentity = null;
		}
		this.identityRowMap.Clear();
	}

	// Token: 0x0600AAB2 RID: 43698 RVA: 0x001135E3 File Offset: 0x001117E3
	private void HideScreen(bool hide)
	{
		if (hide)
		{
			base.transform.localScale = Vector3.zero;
			return;
		}
		if (base.transform.localScale != Vector3.one)
		{
			base.transform.localScale = Vector3.one;
		}
	}

	// Token: 0x0600AAB3 RID: 43699 RVA: 0x00113620 File Offset: 0x00111820
	private void OnRowClicked(IAssignableIdentity identity)
	{
		if (this.targetAssignable.assignee != identity)
		{
			this.ChangeAssignment(identity);
			return;
		}
		if (this.CanDeselect(identity))
		{
			this.ChangeAssignment(null);
		}
	}

	// Token: 0x0600AAB4 RID: 43700 RVA: 0x00113648 File Offset: 0x00111848
	private bool CanDeselect(IAssignableIdentity identity)
	{
		return identity is MinionAssignablesProxy;
	}

	// Token: 0x0600AAB5 RID: 43701 RVA: 0x00113653 File Offset: 0x00111853
	private void ChangeAssignment(IAssignableIdentity new_identity)
	{
		this.targetAssignable.Unassign();
		if (!new_identity.IsNullOrDestroyed())
		{
			this.targetAssignable.Assign(new_identity);
		}
	}

	// Token: 0x0600AAB6 RID: 43702 RVA: 0x00113674 File Offset: 0x00111874
	private void OnValidStateChanged(bool state)
	{
		if (base.gameObject.activeInHierarchy)
		{
			this.Refresh(this.identityList);
		}
	}

	// Token: 0x0400864D RID: 34381
	[SerializeField]
	private AssignableSideScreenRow rowPrefab;

	// Token: 0x0400864E RID: 34382
	[SerializeField]
	private GameObject rowGroup;

	// Token: 0x0400864F RID: 34383
	[SerializeField]
	private LocText currentOwnerText;

	// Token: 0x04008650 RID: 34384
	[SerializeField]
	private MultiToggle dupeSortingToggle;

	// Token: 0x04008651 RID: 34385
	[SerializeField]
	private MultiToggle generalSortingToggle;

	// Token: 0x04008652 RID: 34386
	private MultiToggle activeSortToggle;

	// Token: 0x04008653 RID: 34387
	private Comparison<IAssignableIdentity> activeSortFunction;

	// Token: 0x04008654 RID: 34388
	private bool sortReversed;

	// Token: 0x04008655 RID: 34389
	private int targetAssignableSubscriptionHandle = -1;

	// Token: 0x04008657 RID: 34391
	private UIPool<AssignableSideScreenRow> rowPool;

	// Token: 0x04008658 RID: 34392
	private Dictionary<IAssignableIdentity, AssignableSideScreenRow> identityRowMap = new Dictionary<IAssignableIdentity, AssignableSideScreenRow>();

	// Token: 0x04008659 RID: 34393
	private List<MinionAssignablesProxy> identityList = new List<MinionAssignablesProxy>();
}
