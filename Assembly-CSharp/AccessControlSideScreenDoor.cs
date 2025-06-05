using System;
using STRINGS;
using UnityEngine;

// Token: 0x02001F83 RID: 8067
[AddComponentMenu("KMonoBehaviour/scripts/AccessControlSideScreenDoor")]
public class AccessControlSideScreenDoor : KMonoBehaviour
{
	// Token: 0x0600AA59 RID: 43609 RVA: 0x001130FE File Offset: 0x001112FE
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.leftButton.onClick += this.OnPermissionButtonClicked;
		this.rightButton.onClick += this.OnPermissionButtonClicked;
	}

	// Token: 0x0600AA5A RID: 43610 RVA: 0x00414F48 File Offset: 0x00413148
	private void OnPermissionButtonClicked()
	{
		AccessControl.Permission arg;
		if (this.leftButton.isOn)
		{
			if (this.rightButton.isOn)
			{
				arg = AccessControl.Permission.Both;
			}
			else
			{
				arg = AccessControl.Permission.GoLeft;
			}
		}
		else if (this.rightButton.isOn)
		{
			arg = AccessControl.Permission.GoRight;
		}
		else
		{
			arg = AccessControl.Permission.Neither;
		}
		this.UpdateButtonStates(false);
		this.permissionChangedCallback(this.targetIdentity, arg);
	}

	// Token: 0x0600AA5B RID: 43611 RVA: 0x00414FA4 File Offset: 0x004131A4
	protected virtual void UpdateButtonStates(bool isDefault)
	{
		ToolTip component = this.leftButton.GetComponent<ToolTip>();
		ToolTip component2 = this.rightButton.GetComponent<ToolTip>();
		if (this.isUpDown)
		{
			component.SetSimpleTooltip(this.leftButton.isOn ? UI.UISIDESCREENS.ACCESS_CONTROL_SIDE_SCREEN.GO_UP_ENABLED : UI.UISIDESCREENS.ACCESS_CONTROL_SIDE_SCREEN.GO_UP_DISABLED);
			component2.SetSimpleTooltip(this.rightButton.isOn ? UI.UISIDESCREENS.ACCESS_CONTROL_SIDE_SCREEN.GO_DOWN_ENABLED : UI.UISIDESCREENS.ACCESS_CONTROL_SIDE_SCREEN.GO_DOWN_DISABLED);
			return;
		}
		component.SetSimpleTooltip(this.leftButton.isOn ? UI.UISIDESCREENS.ACCESS_CONTROL_SIDE_SCREEN.GO_LEFT_ENABLED : UI.UISIDESCREENS.ACCESS_CONTROL_SIDE_SCREEN.GO_LEFT_DISABLED);
		component2.SetSimpleTooltip(this.rightButton.isOn ? UI.UISIDESCREENS.ACCESS_CONTROL_SIDE_SCREEN.GO_RIGHT_ENABLED : UI.UISIDESCREENS.ACCESS_CONTROL_SIDE_SCREEN.GO_RIGHT_DISABLED);
	}

	// Token: 0x0600AA5C RID: 43612 RVA: 0x00113134 File Offset: 0x00111334
	public void SetRotated(bool rotated)
	{
		this.isUpDown = rotated;
	}

	// Token: 0x0600AA5D RID: 43613 RVA: 0x0011313D File Offset: 0x0011133D
	public void SetContent(AccessControl.Permission permission, Action<MinionAssignablesProxy, AccessControl.Permission> onPermissionChange)
	{
		this.permissionChangedCallback = onPermissionChange;
		this.leftButton.isOn = (permission == AccessControl.Permission.Both || permission == AccessControl.Permission.GoLeft);
		this.rightButton.isOn = (permission == AccessControl.Permission.Both || permission == AccessControl.Permission.GoRight);
		this.UpdateButtonStates(false);
	}

	// Token: 0x0400861A RID: 34330
	public KToggle leftButton;

	// Token: 0x0400861B RID: 34331
	public KToggle rightButton;

	// Token: 0x0400861C RID: 34332
	private Action<MinionAssignablesProxy, AccessControl.Permission> permissionChangedCallback;

	// Token: 0x0400861D RID: 34333
	private bool isUpDown;

	// Token: 0x0400861E RID: 34334
	protected MinionAssignablesProxy targetIdentity;
}
