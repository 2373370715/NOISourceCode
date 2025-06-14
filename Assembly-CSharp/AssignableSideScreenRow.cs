﻿using System;
using STRINGS;
using UnityEngine;

[AddComponentMenu("KMonoBehaviour/scripts/AssignableSideScreenRow")]
public class AssignableSideScreenRow : KMonoBehaviour
{
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

	protected override void OnCleanUp()
	{
		if (this.refreshHandle == -1)
		{
			Game.Instance.Unsubscribe(this.refreshHandle);
		}
		base.OnCleanUp();
	}

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

	[SerializeField]
	private CrewPortrait crewPortraitPrefab;

	[SerializeField]
	private LocText assignmentText;

	public AssignableSideScreen sideScreen;

	private CrewPortrait portraitInstance;

	[MyCmpReq]
	private MultiToggle toggle;

	public IAssignableIdentity targetIdentity;

	public AssignableSideScreenRow.AssignableState currentState;

	private int refreshHandle = -1;

	public enum AssignableState
	{
		Selected,
		AssignedToOther,
		Unassigned,
		Disabled
	}
}
