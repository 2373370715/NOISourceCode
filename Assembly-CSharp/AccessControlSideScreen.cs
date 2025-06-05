using System;
using System.Collections.Generic;
using System.Linq;
using STRINGS;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x02001F80 RID: 8064
public class AccessControlSideScreen : SideScreenContent
{
	// Token: 0x0600AA3F RID: 43583 RVA: 0x00112F97 File Offset: 0x00111197
	public override string GetTitle()
	{
		if (this.target != null)
		{
			return string.Format(base.GetTitle(), this.target.GetProperName());
		}
		return base.GetTitle();
	}

	// Token: 0x0600AA40 RID: 43584 RVA: 0x004148CC File Offset: 0x00412ACC
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.sortByNameToggle.onValueChanged.AddListener(delegate(bool reverse_sort)
		{
			this.SortEntries(reverse_sort, new Comparison<MinionAssignablesProxy>(AccessControlSideScreen.MinionIdentitySort.CompareByName));
		});
		this.sortByRoleToggle.onValueChanged.AddListener(delegate(bool reverse_sort)
		{
			this.SortEntries(reverse_sort, new Comparison<MinionAssignablesProxy>(AccessControlSideScreen.MinionIdentitySort.CompareByRole));
		});
		this.sortByPermissionToggle.onValueChanged.AddListener(new UnityAction<bool>(this.SortByPermission));
	}

	// Token: 0x0600AA41 RID: 43585 RVA: 0x00112FC4 File Offset: 0x001111C4
	public override bool IsValidForTarget(GameObject target)
	{
		return target.GetComponent<AccessControl>() != null && target.GetComponent<AccessControl>().controlEnabled;
	}

	// Token: 0x0600AA42 RID: 43586 RVA: 0x00414934 File Offset: 0x00412B34
	public override void SetTarget(GameObject target)
	{
		if (this.target != null)
		{
			this.ClearTarget();
		}
		this.target = target.GetComponent<AccessControl>();
		this.doorTarget = target.GetComponent<Door>();
		if (this.target == null)
		{
			return;
		}
		target.Subscribe(1734268753, new Action<object>(this.OnDoorStateChanged));
		target.Subscribe(-1525636549, new Action<object>(this.OnAccessControlChanged));
		if (this.rowPool == null)
		{
			this.rowPool = new UIPool<AccessControlSideScreenRow>(this.rowPrefab);
		}
		base.gameObject.SetActive(true);
		this.identityList = new List<MinionAssignablesProxy>(Components.MinionAssignablesProxy.Items);
		this.Refresh(this.identityList, true);
	}

	// Token: 0x0600AA43 RID: 43587 RVA: 0x004149F4 File Offset: 0x00412BF4
	public override void ClearTarget()
	{
		base.ClearTarget();
		if (this.target != null)
		{
			this.target.Unsubscribe(1734268753, new Action<object>(this.OnDoorStateChanged));
			this.target.Unsubscribe(-1525636549, new Action<object>(this.OnAccessControlChanged));
		}
	}

	// Token: 0x0600AA44 RID: 43588 RVA: 0x00414A50 File Offset: 0x00412C50
	private void Refresh(List<MinionAssignablesProxy> identities, bool rebuild)
	{
		Rotatable component = this.target.GetComponent<Rotatable>();
		bool rotated = component != null && component.IsRotated;
		this.defaultsRow.SetRotated(rotated);
		this.defaultsRow.SetContent(this.target.DefaultPermission, new Action<MinionAssignablesProxy, AccessControl.Permission>(this.OnDefaultPermissionChanged));
		if (rebuild)
		{
			this.ClearContent();
		}
		foreach (MinionAssignablesProxy minionAssignablesProxy in identities)
		{
			AccessControlSideScreenRow accessControlSideScreenRow;
			if (rebuild)
			{
				accessControlSideScreenRow = this.rowPool.GetFreeElement(this.rowGroup, true);
				this.identityRowMap.Add(minionAssignablesProxy, accessControlSideScreenRow);
			}
			else
			{
				accessControlSideScreenRow = this.identityRowMap[minionAssignablesProxy];
			}
			AccessControl.Permission setPermission = this.target.GetSetPermission(minionAssignablesProxy);
			bool isDefault = this.target.IsDefaultPermission(minionAssignablesProxy);
			accessControlSideScreenRow.SetRotated(rotated);
			accessControlSideScreenRow.SetMinionContent(minionAssignablesProxy, setPermission, isDefault, new Action<MinionAssignablesProxy, AccessControl.Permission>(this.OnPermissionChanged), new Action<MinionAssignablesProxy, bool>(this.OnPermissionDefault));
		}
		this.RefreshOnline();
		this.ContentContainer.SetActive(this.target.controlEnabled);
	}

	// Token: 0x0600AA45 RID: 43589 RVA: 0x00414B8C File Offset: 0x00412D8C
	private void RefreshOnline()
	{
		bool flag = this.target.Online && (this.doorTarget == null || this.doorTarget.CurrentState == Door.ControlState.Auto);
		this.disabledOverlay.SetActive(!flag);
		this.headerBG.ColorState = (flag ? KImage.ColorSelector.Active : KImage.ColorSelector.Inactive);
	}

	// Token: 0x0600AA46 RID: 43590 RVA: 0x00112FE1 File Offset: 0x001111E1
	private void SortByPermission(bool state)
	{
		this.ExecuteSort<int>(this.sortByPermissionToggle, state, delegate(MinionAssignablesProxy identity)
		{
			if (!this.target.IsDefaultPermission(identity))
			{
				return (int)this.target.GetSetPermission(identity);
			}
			return -1;
		}, false);
	}

	// Token: 0x0600AA47 RID: 43591 RVA: 0x00414BEC File Offset: 0x00412DEC
	private void ExecuteSort<T>(Toggle toggle, bool state, Func<MinionAssignablesProxy, T> sortFunction, bool refresh = false)
	{
		toggle.GetComponent<ImageToggleState>().SetActiveState(state);
		if (!state)
		{
			return;
		}
		this.identityList = (state ? this.identityList.OrderBy(sortFunction).ToList<MinionAssignablesProxy>() : this.identityList.OrderByDescending(sortFunction).ToList<MinionAssignablesProxy>());
		if (refresh)
		{
			this.Refresh(this.identityList, false);
			return;
		}
		for (int i = 0; i < this.identityList.Count; i++)
		{
			if (this.identityRowMap.ContainsKey(this.identityList[i]))
			{
				this.identityRowMap[this.identityList[i]].transform.SetSiblingIndex(i);
			}
		}
	}

	// Token: 0x0600AA48 RID: 43592 RVA: 0x00414C9C File Offset: 0x00412E9C
	private void SortEntries(bool reverse_sort, Comparison<MinionAssignablesProxy> compare)
	{
		this.identityList.Sort(compare);
		if (reverse_sort)
		{
			this.identityList.Reverse();
		}
		for (int i = 0; i < this.identityList.Count; i++)
		{
			if (this.identityRowMap.ContainsKey(this.identityList[i]))
			{
				this.identityRowMap[this.identityList[i]].transform.SetSiblingIndex(i);
			}
		}
	}

	// Token: 0x0600AA49 RID: 43593 RVA: 0x00112FFD File Offset: 0x001111FD
	private void ClearContent()
	{
		if (this.rowPool != null)
		{
			this.rowPool.ClearAll();
		}
		this.identityRowMap.Clear();
	}

	// Token: 0x0600AA4A RID: 43594 RVA: 0x00414D14 File Offset: 0x00412F14
	private void OnDefaultPermissionChanged(MinionAssignablesProxy identity, AccessControl.Permission permission)
	{
		this.target.DefaultPermission = permission;
		this.Refresh(this.identityList, false);
		foreach (MinionAssignablesProxy key in this.identityList)
		{
			if (this.target.IsDefaultPermission(key))
			{
				this.target.ClearPermission(key);
			}
		}
	}

	// Token: 0x0600AA4B RID: 43595 RVA: 0x0011301D File Offset: 0x0011121D
	private void OnPermissionChanged(MinionAssignablesProxy identity, AccessControl.Permission permission)
	{
		this.target.SetPermission(identity, permission);
	}

	// Token: 0x0600AA4C RID: 43596 RVA: 0x0011302C File Offset: 0x0011122C
	private void OnPermissionDefault(MinionAssignablesProxy identity, bool isDefault)
	{
		if (isDefault)
		{
			this.target.ClearPermission(identity);
		}
		else
		{
			this.target.SetPermission(identity, this.target.DefaultPermission);
		}
		this.Refresh(this.identityList, false);
	}

	// Token: 0x0600AA4D RID: 43597 RVA: 0x00113063 File Offset: 0x00111263
	private void OnAccessControlChanged(object data)
	{
		this.RefreshOnline();
	}

	// Token: 0x0600AA4E RID: 43598 RVA: 0x00113063 File Offset: 0x00111263
	private void OnDoorStateChanged(object data)
	{
		this.RefreshOnline();
	}

	// Token: 0x0600AA4F RID: 43599 RVA: 0x00414D94 File Offset: 0x00412F94
	private void OnSelectSortFunc(IListableOption role, object data)
	{
		if (role != null)
		{
			foreach (AccessControlSideScreen.MinionIdentitySort.SortInfo sortInfo in AccessControlSideScreen.MinionIdentitySort.SortInfos)
			{
				if (sortInfo.name == role.GetProperName())
				{
					this.sortInfo = sortInfo;
					this.identityList.Sort(this.sortInfo.compare);
					for (int j = 0; j < this.identityList.Count; j++)
					{
						if (this.identityRowMap.ContainsKey(this.identityList[j]))
						{
							this.identityRowMap[this.identityList[j]].transform.SetSiblingIndex(j);
						}
					}
					return;
				}
			}
		}
	}

	// Token: 0x04008609 RID: 34313
	[SerializeField]
	private AccessControlSideScreenRow rowPrefab;

	// Token: 0x0400860A RID: 34314
	[SerializeField]
	private GameObject rowGroup;

	// Token: 0x0400860B RID: 34315
	[SerializeField]
	private AccessControlSideScreenDoor defaultsRow;

	// Token: 0x0400860C RID: 34316
	[SerializeField]
	private Toggle sortByNameToggle;

	// Token: 0x0400860D RID: 34317
	[SerializeField]
	private Toggle sortByPermissionToggle;

	// Token: 0x0400860E RID: 34318
	[SerializeField]
	private Toggle sortByRoleToggle;

	// Token: 0x0400860F RID: 34319
	[SerializeField]
	private GameObject disabledOverlay;

	// Token: 0x04008610 RID: 34320
	[SerializeField]
	private KImage headerBG;

	// Token: 0x04008611 RID: 34321
	private AccessControl target;

	// Token: 0x04008612 RID: 34322
	private Door doorTarget;

	// Token: 0x04008613 RID: 34323
	private UIPool<AccessControlSideScreenRow> rowPool;

	// Token: 0x04008614 RID: 34324
	private AccessControlSideScreen.MinionIdentitySort.SortInfo sortInfo = AccessControlSideScreen.MinionIdentitySort.SortInfos[0];

	// Token: 0x04008615 RID: 34325
	private Dictionary<MinionAssignablesProxy, AccessControlSideScreenRow> identityRowMap = new Dictionary<MinionAssignablesProxy, AccessControlSideScreenRow>();

	// Token: 0x04008616 RID: 34326
	private List<MinionAssignablesProxy> identityList = new List<MinionAssignablesProxy>();

	// Token: 0x02001F81 RID: 8065
	private static class MinionIdentitySort
	{
		// Token: 0x0600AA54 RID: 43604 RVA: 0x001130DE File Offset: 0x001112DE
		public static int CompareByName(MinionAssignablesProxy a, MinionAssignablesProxy b)
		{
			return a.GetProperName().CompareTo(b.GetProperName());
		}

		// Token: 0x0600AA55 RID: 43605 RVA: 0x00414E4C File Offset: 0x0041304C
		public static int CompareByRole(MinionAssignablesProxy a, MinionAssignablesProxy b)
		{
			global::Debug.Assert(a, "a was null");
			global::Debug.Assert(b, "b was null");
			GameObject targetGameObject = a.GetTargetGameObject();
			GameObject targetGameObject2 = b.GetTargetGameObject();
			MinionResume minionResume = targetGameObject ? targetGameObject.GetComponent<MinionResume>() : null;
			MinionResume minionResume2 = targetGameObject2 ? targetGameObject2.GetComponent<MinionResume>() : null;
			if (minionResume2 == null)
			{
				return 1;
			}
			if (minionResume == null)
			{
				return -1;
			}
			int num = minionResume.CurrentRole.CompareTo(minionResume2.CurrentRole);
			if (num != 0)
			{
				return num;
			}
			return AccessControlSideScreen.MinionIdentitySort.CompareByName(a, b);
		}

		// Token: 0x04008617 RID: 34327
		public static readonly AccessControlSideScreen.MinionIdentitySort.SortInfo[] SortInfos = new AccessControlSideScreen.MinionIdentitySort.SortInfo[]
		{
			new AccessControlSideScreen.MinionIdentitySort.SortInfo
			{
				name = UI.MINION_IDENTITY_SORT.NAME,
				compare = new Comparison<MinionAssignablesProxy>(AccessControlSideScreen.MinionIdentitySort.CompareByName)
			},
			new AccessControlSideScreen.MinionIdentitySort.SortInfo
			{
				name = UI.MINION_IDENTITY_SORT.ROLE,
				compare = new Comparison<MinionAssignablesProxy>(AccessControlSideScreen.MinionIdentitySort.CompareByRole)
			}
		};

		// Token: 0x02001F82 RID: 8066
		public class SortInfo : IListableOption
		{
			// Token: 0x0600AA57 RID: 43607 RVA: 0x001130F1 File Offset: 0x001112F1
			public string GetProperName()
			{
				return this.name;
			}

			// Token: 0x04008618 RID: 34328
			public LocString name;

			// Token: 0x04008619 RID: 34329
			public Comparison<MinionAssignablesProxy> compare;
		}
	}
}
