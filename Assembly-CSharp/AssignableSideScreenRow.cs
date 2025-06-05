using System;
using STRINGS;
using UnityEngine;

// Token: 0x02001F8F RID: 8079
[AddComponentMenu("KMonoBehaviour/scripts/AssignableSideScreenRow")]
public class AssignableSideScreenRow : KMonoBehaviour
{
	// Token: 0x0600AABC RID: 43708 RVA: 0x00416704 File Offset: 0x00414904
	public void Refresh(object data = null)
	{
		if (!this.sideScreen.targetAssignable.CanAssignTo(this.targetIdentity))
		{
			this.currentState = AssignableSideScreenRow.AssignableState.Disabled;
			this.assignmentText.text = UI.UISIDESCREENS.ASSIGNABLESIDESCREEN.DISABLED;
		}
		else if (this.sideScreen.targetAssignable.assignee == this.targetIdentity)
		{
			this.currentState = AssignableSideScreenRow.AssignableState.Selected;
			this.assignmentText.text = UI.UISIDESCREENS.ASSIGNABLESIDESCREEN.ASSIGNED;
		}
		else
		{
			bool flag = false;
			KMonoBehaviour kmonoBehaviour = this.targetIdentity as KMonoBehaviour;
			if (kmonoBehaviour != null)
			{
				Ownables component = kmonoBehaviour.GetComponent<Ownables>();
				if (component != null)
				{
					AssignableSlotInstance[] slots = component.GetSlots(this.sideScreen.targetAssignable.slot);
					if (slots != null && slots.Length != 0)
					{
						AssignableSlotInstance assignableSlotInstance = slots.FindFirst((AssignableSlotInstance s) => !s.IsAssigned());
						if (assignableSlotInstance == null)
						{
							assignableSlotInstance = slots[0];
						}
						if (assignableSlotInstance != null && assignableSlotInstance.IsAssigned())
						{
							this.currentState = AssignableSideScreenRow.AssignableState.AssignedToOther;
							this.assignmentText.text = assignableSlotInstance.assignable.GetProperName();
							flag = true;
						}
					}
				}
				Equipment component2 = kmonoBehaviour.GetComponent<Equipment>();
				if (component2 != null)
				{
					AssignableSlotInstance[] slots2 = component2.GetSlots(this.sideScreen.targetAssignable.slot);
					if (slots2 != null && slots2.Length != 0)
					{
						AssignableSlotInstance assignableSlotInstance2 = slots2.FindFirst((AssignableSlotInstance s) => !s.IsAssigned());
						if (assignableSlotInstance2 == null)
						{
							assignableSlotInstance2 = slots2[0];
						}
						if (assignableSlotInstance2 != null && assignableSlotInstance2.IsAssigned())
						{
							this.currentState = AssignableSideScreenRow.AssignableState.AssignedToOther;
							this.assignmentText.text = assignableSlotInstance2.assignable.GetProperName();
							flag = true;
						}
					}
				}
			}
			if (!flag)
			{
				this.currentState = AssignableSideScreenRow.AssignableState.Unassigned;
				this.assignmentText.text = UI.UISIDESCREENS.ASSIGNABLESIDESCREEN.UNASSIGNED;
			}
		}
		this.toggle.ChangeState((int)this.currentState);
	}

	// Token: 0x0600AABD RID: 43709 RVA: 0x001136E6 File Offset: 0x001118E6
	protected override void OnCleanUp()
	{
		if (this.refreshHandle == -1)
		{
			Game.Instance.Unsubscribe(this.refreshHandle);
		}
		base.OnCleanUp();
	}

	// Token: 0x0600AABE RID: 43710 RVA: 0x004168F0 File Offset: 0x00414AF0
	public void SetContent(IAssignableIdentity identity_object, Action<IAssignableIdentity> selectionCallback, AssignableSideScreen assignableSideScreen)
	{
		if (this.refreshHandle == -1)
		{
			Game.Instance.Unsubscribe(this.refreshHandle);
		}
		this.refreshHandle = Game.Instance.Subscribe(-2146166042, delegate(object o)
		{
			if (this != null && this.gameObject != null && this.gameObject.activeInHierarchy)
			{
				this.Refresh(null);
			}
		});
		this.toggle = base.GetComponent<MultiToggle>();
		this.sideScreen = assignableSideScreen;
		this.targetIdentity = identity_object;
		if (this.portraitInstance == null)
		{
			this.portraitInstance = Util.KInstantiateUI<CrewPortrait>(this.crewPortraitPrefab.gameObject, base.gameObject, false);
			this.portraitInstance.transform.SetSiblingIndex(1);
			this.portraitInstance.SetAlpha(1f);
		}
		this.toggle.onClick = delegate()
		{
			selectionCallback(this.targetIdentity);
		};
		this.portraitInstance.SetIdentityObject(identity_object, false);
		base.GetComponent<ToolTip>().OnToolTip = new Func<string>(this.GetTooltip);
		this.Refresh(null);
	}

	// Token: 0x0600AABF RID: 43711 RVA: 0x004169F4 File Offset: 0x00414BF4
	private string GetTooltip()
	{
		ToolTip component = base.GetComponent<ToolTip>();
		component.ClearMultiStringTooltip();
		if (this.sideScreen.targetAssignable.customAssignablesUITooltipFunc != null)
		{
			return this.sideScreen.targetAssignable.customAssignablesUITooltipFunc(this.targetIdentity.GetSoleOwner());
		}
		if (this.targetIdentity != null && !this.targetIdentity.IsNull())
		{
			AssignableSideScreenRow.AssignableState assignableState = this.currentState;
			if (assignableState != AssignableSideScreenRow.AssignableState.Selected)
			{
				if (assignableState != AssignableSideScreenRow.AssignableState.Disabled)
				{
					component.AddMultiStringTooltip(string.Format(UI.UISIDESCREENS.ASSIGNABLESIDESCREEN.ASSIGN_TO_TOOLTIP, this.targetIdentity.GetProperName()), null);
				}
				else
				{
					component.AddMultiStringTooltip(string.Format(UI.UISIDESCREENS.ASSIGNABLESIDESCREEN.DISABLED_TOOLTIP, this.targetIdentity.GetProperName()), null);
				}
			}
			else
			{
				component.AddMultiStringTooltip(string.Format(UI.UISIDESCREENS.ASSIGNABLESIDESCREEN.UNASSIGN_TOOLTIP, this.targetIdentity.GetProperName()), null);
			}
		}
		return "";
	}

	// Token: 0x0400865A RID: 34394
	[SerializeField]
	private CrewPortrait crewPortraitPrefab;

	// Token: 0x0400865B RID: 34395
	[SerializeField]
	private LocText assignmentText;

	// Token: 0x0400865C RID: 34396
	public AssignableSideScreen sideScreen;

	// Token: 0x0400865D RID: 34397
	private CrewPortrait portraitInstance;

	// Token: 0x0400865E RID: 34398
	[MyCmpReq]
	private MultiToggle toggle;

	// Token: 0x0400865F RID: 34399
	public IAssignableIdentity targetIdentity;

	// Token: 0x04008660 RID: 34400
	public AssignableSideScreenRow.AssignableState currentState;

	// Token: 0x04008661 RID: 34401
	private int refreshHandle = -1;

	// Token: 0x02001F90 RID: 8080
	public enum AssignableState
	{
		// Token: 0x04008663 RID: 34403
		Selected,
		// Token: 0x04008664 RID: 34404
		AssignedToOther,
		// Token: 0x04008665 RID: 34405
		Unassigned,
		// Token: 0x04008666 RID: 34406
		Disabled
	}
}
