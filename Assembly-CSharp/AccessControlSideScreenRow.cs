using System;
using STRINGS;
using UnityEngine;

// Token: 0x02001F84 RID: 8068
public class AccessControlSideScreenRow : AccessControlSideScreenDoor
{
	// Token: 0x0600AA5F RID: 43615 RVA: 0x00113177 File Offset: 0x00111377
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.defaultButton.onValueChanged += this.OnDefaultButtonChanged;
	}

	// Token: 0x0600AA60 RID: 43616 RVA: 0x00113196 File Offset: 0x00111396
	private void OnDefaultButtonChanged(bool state)
	{
		this.UpdateButtonStates(!state);
		if (this.defaultClickedCallback != null)
		{
			this.defaultClickedCallback(this.targetIdentity, !state);
		}
	}

	// Token: 0x0600AA61 RID: 43617 RVA: 0x00415064 File Offset: 0x00413264
	protected override void UpdateButtonStates(bool isDefault)
	{
		base.UpdateButtonStates(isDefault);
		this.defaultButton.GetComponent<ToolTip>().SetSimpleTooltip(isDefault ? UI.UISIDESCREENS.ACCESS_CONTROL_SIDE_SCREEN.SET_TO_CUSTOM : UI.UISIDESCREENS.ACCESS_CONTROL_SIDE_SCREEN.SET_TO_DEFAULT);
		this.defaultControls.SetActive(isDefault);
		this.customControls.SetActive(!isDefault);
	}

	// Token: 0x0600AA62 RID: 43618 RVA: 0x004150B8 File Offset: 0x004132B8
	public void SetMinionContent(MinionAssignablesProxy identity, AccessControl.Permission permission, bool isDefault, Action<MinionAssignablesProxy, AccessControl.Permission> onPermissionChange, Action<MinionAssignablesProxy, bool> onDefaultClick)
	{
		base.SetContent(permission, onPermissionChange);
		if (identity == null)
		{
			global::Debug.LogError("Invalid data received.");
			return;
		}
		if (this.portraitInstance == null)
		{
			this.portraitInstance = Util.KInstantiateUI<CrewPortrait>(this.crewPortraitPrefab.gameObject, this.defaultButton.gameObject, false);
			this.portraitInstance.SetAlpha(1f);
		}
		this.targetIdentity = identity;
		this.portraitInstance.SetIdentityObject(identity, false);
		this.portraitInstance.SetSubTitle(isDefault ? UI.UISIDESCREENS.ACCESS_CONTROL_SIDE_SCREEN.USING_DEFAULT : UI.UISIDESCREENS.ACCESS_CONTROL_SIDE_SCREEN.USING_CUSTOM);
		this.defaultClickedCallback = null;
		this.defaultButton.isOn = !isDefault;
		this.defaultClickedCallback = onDefaultClick;
	}

	// Token: 0x0400861F RID: 34335
	[SerializeField]
	private CrewPortrait crewPortraitPrefab;

	// Token: 0x04008620 RID: 34336
	private CrewPortrait portraitInstance;

	// Token: 0x04008621 RID: 34337
	public KToggle defaultButton;

	// Token: 0x04008622 RID: 34338
	public GameObject defaultControls;

	// Token: 0x04008623 RID: 34339
	public GameObject customControls;

	// Token: 0x04008624 RID: 34340
	private Action<MinionAssignablesProxy, bool> defaultClickedCallback;
}
