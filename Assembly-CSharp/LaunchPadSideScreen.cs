using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x02001FE3 RID: 8163
public class LaunchPadSideScreen : SideScreenContent
{
	// Token: 0x0600AC72 RID: 44146 RVA: 0x001148F1 File Offset: 0x00112AF1
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.startNewRocketbutton.onClick += this.ClickStartNewRocket;
		this.devAutoRocketButton.onClick += this.ClickAutoRocket;
	}

	// Token: 0x0600AC73 RID: 44147 RVA: 0x00114927 File Offset: 0x00112B27
	protected override void OnShow(bool show)
	{
		base.OnShow(show);
		if (!show)
		{
			DetailsScreen.Instance.ClearSecondarySideScreen();
		}
	}

	// Token: 0x0600AC74 RID: 44148 RVA: 0x000D3AD3 File Offset: 0x000D1CD3
	public override int GetSideScreenSortOrder()
	{
		return 100;
	}

	// Token: 0x0600AC75 RID: 44149 RVA: 0x0011493D File Offset: 0x00112B3D
	public override bool IsValidForTarget(GameObject target)
	{
		return target.GetComponent<LaunchPad>() != null;
	}

	// Token: 0x0600AC76 RID: 44150 RVA: 0x0041DE00 File Offset: 0x0041C000
	public override void SetTarget(GameObject new_target)
	{
		if (new_target == null)
		{
			global::Debug.LogError("Invalid gameObject received");
			return;
		}
		if (this.refreshEventHandle != -1)
		{
			this.selectedPad.Unsubscribe(this.refreshEventHandle);
		}
		this.selectedPad = new_target.GetComponent<LaunchPad>();
		if (this.selectedPad == null)
		{
			global::Debug.LogError("The gameObject received does not contain a LaunchPad component");
			return;
		}
		this.refreshEventHandle = this.selectedPad.Subscribe(-887025858, new Action<object>(this.RefreshWaitingToLandList));
		this.RefreshRocketButton();
		this.RefreshWaitingToLandList(null);
	}

	// Token: 0x0600AC77 RID: 44151 RVA: 0x0041DE90 File Offset: 0x0041C090
	private void RefreshWaitingToLandList(object data = null)
	{
		for (int i = this.waitingToLandRows.Count - 1; i >= 0; i--)
		{
			Util.KDestroyGameObject(this.waitingToLandRows[i]);
		}
		this.waitingToLandRows.Clear();
		this.nothingWaitingRow.SetActive(true);
		AxialI myWorldLocation = this.selectedPad.GetMyWorldLocation();
		foreach (ClusterGridEntity clusterGridEntity in ClusterGrid.Instance.GetEntitiesInRange(myWorldLocation, 1))
		{
			Clustercraft craft = clusterGridEntity as Clustercraft;
			if (!(craft == null) && craft.Status == Clustercraft.CraftStatus.InFlight && (!craft.IsFlightInProgress() || !(craft.Destination != myWorldLocation)))
			{
				GameObject gameObject = Util.KInstantiateUI(this.landableRocketRowPrefab, this.landableRowContainer, true);
				gameObject.GetComponentInChildren<LocText>().text = craft.Name;
				this.waitingToLandRows.Add(gameObject);
				KButton componentInChildren = gameObject.GetComponentInChildren<KButton>();
				componentInChildren.GetComponentInChildren<LocText>().SetText((craft.ModuleInterface.GetClusterDestinationSelector().GetDestinationPad() == this.selectedPad) ? UI.UISIDESCREENS.LAUNCHPADSIDESCREEN.CANCEL_LAND_BUTTON : UI.UISIDESCREENS.LAUNCHPADSIDESCREEN.LAND_BUTTON);
				string simpleTooltip;
				componentInChildren.isInteractable = (craft.CanLandAtPad(this.selectedPad, out simpleTooltip) != Clustercraft.PadLandingStatus.CanNeverLand);
				if (!componentInChildren.isInteractable)
				{
					componentInChildren.GetComponent<ToolTip>().SetSimpleTooltip(simpleTooltip);
				}
				else
				{
					componentInChildren.GetComponent<ToolTip>().ClearMultiStringTooltip();
				}
				componentInChildren.onClick += delegate()
				{
					if (craft.ModuleInterface.GetClusterDestinationSelector().GetDestinationPad() == this.selectedPad)
					{
						craft.GetComponent<ClusterDestinationSelector>().SetDestination(craft.Location);
					}
					else
					{
						craft.LandAtPad(this.selectedPad);
					}
					this.RefreshWaitingToLandList(null);
				};
				this.nothingWaitingRow.SetActive(false);
			}
		}
	}

	// Token: 0x0600AC78 RID: 44152 RVA: 0x0011494B File Offset: 0x00112B4B
	private void ClickStartNewRocket()
	{
		((SelectModuleSideScreen)DetailsScreen.Instance.SetSecondarySideScreen(this.changeModuleSideScreen, UI.UISIDESCREENS.ROCKETMODULESIDESCREEN.CHANGEMODULEPANEL)).SetLaunchPad(this.selectedPad);
	}

	// Token: 0x0600AC79 RID: 44153 RVA: 0x0041E090 File Offset: 0x0041C290
	private void RefreshRocketButton()
	{
		bool isOperational = this.selectedPad.GetComponent<Operational>().IsOperational;
		this.startNewRocketbutton.isInteractable = (this.selectedPad.LandedRocket == null && isOperational);
		if (!isOperational)
		{
			this.startNewRocketbutton.GetComponent<ToolTip>().SetSimpleTooltip(UI.UISIDESCREENS.CLUSTERDESTINATIONSIDESCREEN.DROPDOWN_TOOLTIP_PAD_DISABLED);
		}
		else
		{
			this.startNewRocketbutton.GetComponent<ToolTip>().ClearMultiStringTooltip();
		}
		this.devAutoRocketButton.isInteractable = (this.selectedPad.LandedRocket == null);
		this.devAutoRocketButton.gameObject.SetActive(DebugHandler.InstantBuildMode);
	}

	// Token: 0x0600AC7A RID: 44154 RVA: 0x00114977 File Offset: 0x00112B77
	private void ClickAutoRocket()
	{
		AutoRocketUtility.StartAutoRocket(this.selectedPad);
	}

	// Token: 0x040087C4 RID: 34756
	public GameObject content;

	// Token: 0x040087C5 RID: 34757
	private LaunchPad selectedPad;

	// Token: 0x040087C6 RID: 34758
	public LocText DescriptionText;

	// Token: 0x040087C7 RID: 34759
	public GameObject landableRocketRowPrefab;

	// Token: 0x040087C8 RID: 34760
	public GameObject newRocketPanel;

	// Token: 0x040087C9 RID: 34761
	public KButton startNewRocketbutton;

	// Token: 0x040087CA RID: 34762
	public KButton devAutoRocketButton;

	// Token: 0x040087CB RID: 34763
	public GameObject landableRowContainer;

	// Token: 0x040087CC RID: 34764
	public GameObject nothingWaitingRow;

	// Token: 0x040087CD RID: 34765
	public KScreen changeModuleSideScreen;

	// Token: 0x040087CE RID: 34766
	private int refreshEventHandle = -1;

	// Token: 0x040087CF RID: 34767
	public List<GameObject> waitingToLandRows = new List<GameObject>();
}
