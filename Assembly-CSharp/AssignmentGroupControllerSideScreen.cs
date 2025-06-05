using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x02001F93 RID: 8083
public class AssignmentGroupControllerSideScreen : KScreen
{
	// Token: 0x0600AAC8 RID: 43720 RVA: 0x00113745 File Offset: 0x00111945
	protected override void OnShow(bool show)
	{
		base.OnShow(show);
		if (show)
		{
			this.RefreshRows();
		}
	}

	// Token: 0x0600AAC9 RID: 43721 RVA: 0x00416B24 File Offset: 0x00414D24
	protected override void OnCmpDisable()
	{
		for (int i = 0; i < this.identityRowMap.Count; i++)
		{
			UnityEngine.Object.Destroy(this.identityRowMap[i]);
		}
		this.identityRowMap.Clear();
		base.OnCmpDisable();
	}

	// Token: 0x0600AACA RID: 43722 RVA: 0x00113757 File Offset: 0x00111957
	public void SetTarget(GameObject target)
	{
		this.target = target.GetComponent<AssignmentGroupController>();
		this.RefreshRows();
	}

	// Token: 0x0600AACB RID: 43723 RVA: 0x00416B6C File Offset: 0x00414D6C
	private void RefreshRows()
	{
		int num = 0;
		WorldContainer myWorld = this.target.GetMyWorld();
		ClustercraftExteriorDoor component = this.target.GetComponent<ClustercraftExteriorDoor>();
		if (component != null)
		{
			myWorld = component.GetInteriorDoor().GetMyWorld();
		}
		List<AssignmentGroupControllerSideScreen.RowSortHelper> list = new List<AssignmentGroupControllerSideScreen.RowSortHelper>();
		for (int i = 0; i < Components.MinionAssignablesProxy.Count; i++)
		{
			MinionAssignablesProxy minionAssignablesProxy = Components.MinionAssignablesProxy[i];
			GameObject targetGameObject = minionAssignablesProxy.GetTargetGameObject();
			WorldContainer myWorld2 = targetGameObject.GetMyWorld();
			if (!(targetGameObject == null) && !targetGameObject.HasTag(GameTags.Dead))
			{
				MinionResume component2 = minionAssignablesProxy.GetTargetGameObject().GetComponent<MinionResume>();
				bool isPilot = false;
				if (component2 != null && component2.HasPerk(Db.Get().SkillPerks.CanUseRocketControlStation))
				{
					isPilot = true;
				}
				bool isSameWorld = myWorld2.ParentWorldId == myWorld.ParentWorldId;
				list.Add(new AssignmentGroupControllerSideScreen.RowSortHelper
				{
					minion = minionAssignablesProxy,
					isPilot = isPilot,
					isSameWorld = isSameWorld
				});
			}
		}
		list.Sort(delegate(AssignmentGroupControllerSideScreen.RowSortHelper a, AssignmentGroupControllerSideScreen.RowSortHelper b)
		{
			int num2 = b.isSameWorld.CompareTo(a.isSameWorld);
			if (num2 != 0)
			{
				return num2;
			}
			return b.isPilot.CompareTo(a.isPilot);
		});
		foreach (AssignmentGroupControllerSideScreen.RowSortHelper rowSortHelper in list)
		{
			MinionAssignablesProxy minion = rowSortHelper.minion;
			GameObject gameObject;
			if (num >= this.identityRowMap.Count)
			{
				gameObject = Util.KInstantiateUI(this.minionRowPrefab, this.minionRowContainer, true);
				this.identityRowMap.Add(gameObject);
			}
			else
			{
				gameObject = this.identityRowMap[num];
				gameObject.SetActive(true);
			}
			num++;
			HierarchyReferences component3 = gameObject.GetComponent<HierarchyReferences>();
			MultiToggle toggle = component3.GetReference<MultiToggle>("Toggle");
			toggle.ChangeState(this.target.CheckMinionIsMember(minion) ? 1 : 0);
			component3.GetReference<CrewPortrait>("Portrait").SetIdentityObject(minion, false);
			LocText reference = component3.GetReference<LocText>("Label");
			LocText reference2 = component3.GetReference<LocText>("Designation");
			if (rowSortHelper.isSameWorld)
			{
				if (rowSortHelper.isPilot)
				{
					reference2.text = UI.UISIDESCREENS.ASSIGNMENTGROUPCONTROLLER.PILOT;
				}
				else
				{
					reference2.text = "";
				}
				reference.color = Color.black;
				reference2.color = Color.black;
			}
			else
			{
				reference.color = Color.grey;
				reference2.color = Color.grey;
				reference2.text = UI.UISIDESCREENS.ASSIGNMENTGROUPCONTROLLER.OFFWORLD;
				gameObject.transform.SetAsLastSibling();
			}
			toggle.onClick = delegate()
			{
				this.target.SetMember(minion, !this.target.CheckMinionIsMember(minion));
				toggle.ChangeState(this.target.CheckMinionIsMember(minion) ? 1 : 0);
				this.RefreshRows();
			};
			string simpleTooltip = this.UpdateToolTip(minion, !rowSortHelper.isSameWorld);
			toggle.GetComponent<ToolTip>().SetSimpleTooltip(simpleTooltip);
		}
		for (int j = num; j < this.identityRowMap.Count; j++)
		{
			this.identityRowMap[j].SetActive(false);
		}
		this.minionRowContainer.GetComponent<QuickLayout>().ForceUpdate();
	}

	// Token: 0x0600AACC RID: 43724 RVA: 0x00416ED0 File Offset: 0x004150D0
	private string UpdateToolTip(MinionAssignablesProxy minion, bool offworld)
	{
		string text = this.target.CheckMinionIsMember(minion) ? UI.UISIDESCREENS.ASSIGNMENTGROUPCONTROLLER.TOOLTIPS.UNASSIGN : UI.UISIDESCREENS.ASSIGNMENTGROUPCONTROLLER.TOOLTIPS.ASSIGN;
		if (offworld)
		{
			text = string.Concat(new string[]
			{
				text,
				"\n\n",
				UIConstants.ColorPrefixYellow,
				UI.UISIDESCREENS.ASSIGNMENTGROUPCONTROLLER.TOOLTIPS.DIFFERENT_WORLD,
				UIConstants.ColorSuffix
			});
		}
		return text;
	}

	// Token: 0x0400866C RID: 34412
	[SerializeField]
	private GameObject header;

	// Token: 0x0400866D RID: 34413
	[SerializeField]
	private GameObject minionRowPrefab;

	// Token: 0x0400866E RID: 34414
	[SerializeField]
	private GameObject footer;

	// Token: 0x0400866F RID: 34415
	[SerializeField]
	private GameObject minionRowContainer;

	// Token: 0x04008670 RID: 34416
	private AssignmentGroupController target;

	// Token: 0x04008671 RID: 34417
	private List<GameObject> identityRowMap = new List<GameObject>();

	// Token: 0x02001F94 RID: 8084
	private struct RowSortHelper
	{
		// Token: 0x04008672 RID: 34418
		public MinionAssignablesProxy minion;

		// Token: 0x04008673 RID: 34419
		public bool isPilot;

		// Token: 0x04008674 RID: 34420
		public bool isSameWorld;
	}
}
