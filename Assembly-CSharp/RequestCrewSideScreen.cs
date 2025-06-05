using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x0200201E RID: 8222
public class RequestCrewSideScreen : SideScreenContent
{
	// Token: 0x0600AE06 RID: 44550 RVA: 0x00425048 File Offset: 0x00423248
	protected override void OnSpawn()
	{
		this.changeCrewButton.onClick += this.OnChangeCrewButtonPressed;
		this.crewReleaseButton.onClick += this.CrewRelease;
		this.crewRequestButton.onClick += this.CrewRequest;
		this.toggleMap.Add(this.crewReleaseButton, PassengerRocketModule.RequestCrewState.Release);
		this.toggleMap.Add(this.crewRequestButton, PassengerRocketModule.RequestCrewState.Request);
		this.Refresh();
	}

	// Token: 0x0600AE07 RID: 44551 RVA: 0x000D3AD3 File Offset: 0x000D1CD3
	public override int GetSideScreenSortOrder()
	{
		return 100;
	}

	// Token: 0x0600AE08 RID: 44552 RVA: 0x004250C4 File Offset: 0x004232C4
	public override bool IsValidForTarget(GameObject target)
	{
		PassengerRocketModule component = target.GetComponent<PassengerRocketModule>();
		RocketControlStation component2 = target.GetComponent<RocketControlStation>();
		if (component != null)
		{
			return component.GetMyWorld() != null;
		}
		if (component2 != null)
		{
			RocketControlStation.StatesInstance smi = component2.GetSMI<RocketControlStation.StatesInstance>();
			return !smi.sm.IsInFlight(smi) && !smi.sm.IsLaunching(smi);
		}
		return false;
	}

	// Token: 0x0600AE09 RID: 44553 RVA: 0x0011590D File Offset: 0x00113B0D
	public override void SetTarget(GameObject target)
	{
		if (target.GetComponent<RocketControlStation>() != null)
		{
			this.rocketModule = target.GetMyWorld().GetComponent<Clustercraft>().ModuleInterface.GetPassengerModule();
		}
		else
		{
			this.rocketModule = target.GetComponent<PassengerRocketModule>();
		}
		this.Refresh();
	}

	// Token: 0x0600AE0A RID: 44554 RVA: 0x0011594C File Offset: 0x00113B4C
	private void Refresh()
	{
		this.RefreshRequestButtons();
	}

	// Token: 0x0600AE0B RID: 44555 RVA: 0x00115954 File Offset: 0x00113B54
	private void CrewRelease()
	{
		this.rocketModule.RequestCrewBoard(PassengerRocketModule.RequestCrewState.Release);
		this.RefreshRequestButtons();
	}

	// Token: 0x0600AE0C RID: 44556 RVA: 0x00115968 File Offset: 0x00113B68
	private void CrewRequest()
	{
		this.rocketModule.RequestCrewBoard(PassengerRocketModule.RequestCrewState.Request);
		this.RefreshRequestButtons();
	}

	// Token: 0x0600AE0D RID: 44557 RVA: 0x00425128 File Offset: 0x00423328
	private void RefreshRequestButtons()
	{
		foreach (KeyValuePair<KToggle, PassengerRocketModule.RequestCrewState> keyValuePair in this.toggleMap)
		{
			this.RefreshRequestButton(keyValuePair.Key);
		}
	}

	// Token: 0x0600AE0E RID: 44558 RVA: 0x00425184 File Offset: 0x00423384
	private void RefreshRequestButton(KToggle button)
	{
		ImageToggleState[] componentsInChildren;
		if (this.toggleMap[button] == this.rocketModule.PassengersRequested)
		{
			button.isOn = true;
			componentsInChildren = button.GetComponentsInChildren<ImageToggleState>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].SetActive();
			}
			button.GetComponent<ImageToggleStateThrobber>().enabled = false;
			return;
		}
		button.isOn = false;
		componentsInChildren = button.GetComponentsInChildren<ImageToggleState>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].SetInactive();
		}
		button.GetComponent<ImageToggleStateThrobber>().enabled = false;
	}

	// Token: 0x0600AE0F RID: 44559 RVA: 0x0042520C File Offset: 0x0042340C
	private void OnChangeCrewButtonPressed()
	{
		if (this.activeChangeCrewSideScreen == null)
		{
			this.activeChangeCrewSideScreen = (AssignmentGroupControllerSideScreen)DetailsScreen.Instance.SetSecondarySideScreen(this.changeCrewSideScreenPrefab, UI.UISIDESCREENS.ASSIGNMENTGROUPCONTROLLER.TITLE);
			this.activeChangeCrewSideScreen.SetTarget(this.rocketModule.gameObject);
			return;
		}
		DetailsScreen.Instance.ClearSecondarySideScreen();
		this.activeChangeCrewSideScreen = null;
	}

	// Token: 0x0600AE10 RID: 44560 RVA: 0x0011597C File Offset: 0x00113B7C
	protected override void OnShow(bool show)
	{
		base.OnShow(show);
		if (!show)
		{
			DetailsScreen.Instance.ClearSecondarySideScreen();
			this.activeChangeCrewSideScreen = null;
		}
	}

	// Token: 0x040088FB RID: 35067
	private PassengerRocketModule rocketModule;

	// Token: 0x040088FC RID: 35068
	public KToggle crewReleaseButton;

	// Token: 0x040088FD RID: 35069
	public KToggle crewRequestButton;

	// Token: 0x040088FE RID: 35070
	private Dictionary<KToggle, PassengerRocketModule.RequestCrewState> toggleMap = new Dictionary<KToggle, PassengerRocketModule.RequestCrewState>();

	// Token: 0x040088FF RID: 35071
	public KButton changeCrewButton;

	// Token: 0x04008900 RID: 35072
	public KScreen changeCrewSideScreenPrefab;

	// Token: 0x04008901 RID: 35073
	private AssignmentGroupControllerSideScreen activeChangeCrewSideScreen;
}
