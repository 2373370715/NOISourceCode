using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02001FA7 RID: 8103
public class ClusterDestinationSideScreen : SideScreenContent
{
	// Token: 0x17000AFA RID: 2810
	// (get) Token: 0x0600AB42 RID: 43842 RVA: 0x00113C22 File Offset: 0x00111E22
	// (set) Token: 0x0600AB43 RID: 43843 RVA: 0x00113C2A File Offset: 0x00111E2A
	private ClusterDestinationSelector targetSelector { get; set; }

	// Token: 0x17000AFB RID: 2811
	// (get) Token: 0x0600AB44 RID: 43844 RVA: 0x00113C33 File Offset: 0x00111E33
	// (set) Token: 0x0600AB45 RID: 43845 RVA: 0x00113C3B File Offset: 0x00111E3B
	private RocketClusterDestinationSelector targetRocketSelector { get; set; }

	// Token: 0x0600AB46 RID: 43846 RVA: 0x004185D4 File Offset: 0x004167D4
	protected override void OnSpawn()
	{
		this.changeDestinationButton.onClick += this.OnClickChangeDestination;
		this.clearDestinationButton.onClick += this.OnClickClearDestination;
		this.launchPadDropDown.targetDropDownContainer = GameScreenManager.Instance.ssOverlayCanvas;
		this.launchPadDropDown.CustomizeEmptyRow(UI.UISIDESCREENS.CLUSTERDESTINATIONSIDESCREEN.FIRSTAVAILABLE, null);
		this.repeatButton.onClick += this.OnRepeatClicked;
	}

	// Token: 0x0600AB47 RID: 43847 RVA: 0x0011387D File Offset: 0x00111A7D
	public override int GetSideScreenSortOrder()
	{
		return 300;
	}

	// Token: 0x0600AB48 RID: 43848 RVA: 0x00418654 File Offset: 0x00416854
	protected override void OnShow(bool show)
	{
		base.OnShow(show);
		if (show)
		{
			this.Refresh(null);
			this.m_refreshHandle = this.targetSelector.Subscribe(543433792, delegate(object data)
			{
				this.Refresh(null);
			});
			return;
		}
		if (this.m_refreshHandle != -1)
		{
			this.targetSelector.Unsubscribe(this.m_refreshHandle);
			this.m_refreshHandle = -1;
			this.launchPadDropDown.Close();
		}
	}

	// Token: 0x0600AB49 RID: 43849 RVA: 0x004186C4 File Offset: 0x004168C4
	public override bool IsValidForTarget(GameObject target)
	{
		ClusterDestinationSelector component = target.GetComponent<ClusterDestinationSelector>();
		return (component != null && component.assignable) || (target.GetComponent<RocketModule>() != null && target.HasTag(GameTags.LaunchButtonRocketModule)) || (target.GetComponent<RocketControlStation>() != null && target.GetComponent<RocketControlStation>().GetMyWorld().GetComponent<Clustercraft>().Status != Clustercraft.CraftStatus.Launching);
	}

	// Token: 0x0600AB4A RID: 43850 RVA: 0x00418734 File Offset: 0x00416934
	public override void SetTarget(GameObject target)
	{
		this.targetSelector = target.GetComponent<ClusterDestinationSelector>();
		if (this.targetSelector == null)
		{
			if (target.GetComponent<RocketModuleCluster>() != null)
			{
				this.targetSelector = target.GetComponent<RocketModuleCluster>().CraftInterface.GetClusterDestinationSelector();
			}
			else if (target.GetComponent<RocketControlStation>() != null)
			{
				this.targetSelector = target.GetMyWorld().GetComponent<Clustercraft>().ModuleInterface.GetClusterDestinationSelector();
			}
		}
		this.targetRocketSelector = (this.targetSelector as RocketClusterDestinationSelector);
	}

	// Token: 0x0600AB4B RID: 43851 RVA: 0x004187BC File Offset: 0x004169BC
	private void Refresh(object data = null)
	{
		if (!this.targetSelector.IsAtDestination())
		{
			Sprite sprite;
			string str;
			string text;
			ClusterGrid.Instance.GetLocationDescription(this.targetSelector.GetDestination(), out sprite, out str, out text);
			this.destinationImage.sprite = sprite;
			this.destinationLabel.text = UI.UISIDESCREENS.CLUSTERDESTINATIONSIDESCREEN.TITLE + ": " + str;
			this.clearDestinationButton.isInteractable = true;
		}
		else
		{
			this.destinationImage.sprite = Assets.GetSprite("hex_unknown");
			this.destinationLabel.text = UI.UISIDESCREENS.CLUSTERDESTINATIONSIDESCREEN.TITLE + ": " + UI.SPACEDESTINATIONS.NONE.NAME;
			this.clearDestinationButton.isInteractable = false;
		}
		if (this.targetRocketSelector != null)
		{
			List<LaunchPad> launchPadsForDestination = LaunchPad.GetLaunchPadsForDestination(this.targetRocketSelector.GetDestination());
			this.launchPadDropDown.gameObject.SetActive(true);
			this.repeatButton.gameObject.SetActive(true);
			this.launchPadDropDown.Initialize(launchPadsForDestination, new Action<IListableOption, object>(this.OnLaunchPadEntryClick), new Func<IListableOption, IListableOption, object, int>(this.PadDropDownSort), new Action<DropDownEntry, object>(this.PadDropDownEntryRefreshAction), true, this.targetRocketSelector);
			if (!this.targetRocketSelector.IsAtDestination() && launchPadsForDestination.Count > 0)
			{
				this.launchPadDropDown.openButton.isInteractable = true;
				LaunchPad destinationPad = this.targetRocketSelector.GetDestinationPad();
				if (destinationPad != null)
				{
					this.launchPadDropDown.selectedLabel.text = destinationPad.GetProperName();
				}
				else
				{
					this.launchPadDropDown.selectedLabel.text = UI.UISIDESCREENS.CLUSTERDESTINATIONSIDESCREEN.FIRSTAVAILABLE;
				}
			}
			else
			{
				this.launchPadDropDown.selectedLabel.text = UI.UISIDESCREENS.CLUSTERDESTINATIONSIDESCREEN.FIRSTAVAILABLE;
				this.launchPadDropDown.openButton.isInteractable = false;
			}
			this.StyleRepeatButton();
		}
		else
		{
			this.launchPadDropDown.gameObject.SetActive(false);
			this.repeatButton.gameObject.SetActive(false);
		}
		this.StyleChangeDestinationButton();
	}

	// Token: 0x0600AB4C RID: 43852 RVA: 0x00113C44 File Offset: 0x00111E44
	private void OnClickChangeDestination()
	{
		if (this.targetSelector.assignable)
		{
			ClusterMapScreen.Instance.ShowInSelectDestinationMode(this.targetSelector);
		}
		this.StyleChangeDestinationButton();
	}

	// Token: 0x0600AB4D RID: 43853 RVA: 0x000AA038 File Offset: 0x000A8238
	private void StyleChangeDestinationButton()
	{
	}

	// Token: 0x0600AB4E RID: 43854 RVA: 0x00113C69 File Offset: 0x00111E69
	private void OnClickClearDestination()
	{
		this.targetSelector.SetDestination(this.targetSelector.GetMyWorldLocation());
	}

	// Token: 0x0600AB4F RID: 43855 RVA: 0x004189C4 File Offset: 0x00416BC4
	private void OnLaunchPadEntryClick(IListableOption option, object data)
	{
		LaunchPad destinationPad = (LaunchPad)option;
		this.targetRocketSelector.SetDestinationPad(destinationPad);
	}

	// Token: 0x0600AB50 RID: 43856 RVA: 0x004189E4 File Offset: 0x00416BE4
	private void PadDropDownEntryRefreshAction(DropDownEntry entry, object targetData)
	{
		LaunchPad launchPad = (LaunchPad)entry.entryData;
		Clustercraft component = this.targetRocketSelector.GetComponent<Clustercraft>();
		if (!(launchPad != null))
		{
			entry.button.isInteractable = true;
			entry.image.sprite = Assets.GetBuildingDef("LaunchPad").GetUISprite("ui", false);
			entry.tooltip.SetSimpleTooltip(UI.UISIDESCREENS.CLUSTERDESTINATIONSIDESCREEN.DROPDOWN_TOOLTIP_FIRST_AVAILABLE);
			return;
		}
		string simpleTooltip;
		if (component.CanLandAtPad(launchPad, out simpleTooltip) == Clustercraft.PadLandingStatus.CanNeverLand)
		{
			entry.button.isInteractable = false;
			entry.image.sprite = Assets.GetSprite("iconWarning");
			entry.tooltip.SetSimpleTooltip(simpleTooltip);
			return;
		}
		entry.button.isInteractable = true;
		entry.image.sprite = launchPad.GetComponent<Building>().Def.GetUISprite("ui", false);
		entry.tooltip.SetSimpleTooltip(string.Format(UI.UISIDESCREENS.CLUSTERDESTINATIONSIDESCREEN.DROPDOWN_TOOLTIP_VALID_SITE, launchPad.GetProperName()));
	}

	// Token: 0x0600AB51 RID: 43857 RVA: 0x000B1628 File Offset: 0x000AF828
	private int PadDropDownSort(IListableOption a, IListableOption b, object targetData)
	{
		return 0;
	}

	// Token: 0x0600AB52 RID: 43858 RVA: 0x00113C81 File Offset: 0x00111E81
	private void OnRepeatClicked()
	{
		this.targetRocketSelector.Repeat = !this.targetRocketSelector.Repeat;
		this.StyleRepeatButton();
	}

	// Token: 0x0600AB53 RID: 43859 RVA: 0x00113CA2 File Offset: 0x00111EA2
	private void StyleRepeatButton()
	{
		this.repeatButton.bgImage.colorStyleSetting = (this.targetRocketSelector.Repeat ? this.repeatOn : this.repeatOff);
		this.repeatButton.bgImage.ApplyColorStyleSetting();
	}

	// Token: 0x040086CF RID: 34511
	public Image destinationImage;

	// Token: 0x040086D0 RID: 34512
	public LocText destinationLabel;

	// Token: 0x040086D1 RID: 34513
	public KButton changeDestinationButton;

	// Token: 0x040086D2 RID: 34514
	public KButton clearDestinationButton;

	// Token: 0x040086D3 RID: 34515
	public DropDown launchPadDropDown;

	// Token: 0x040086D4 RID: 34516
	public KButton repeatButton;

	// Token: 0x040086D5 RID: 34517
	public ColorStyleSetting repeatOff;

	// Token: 0x040086D6 RID: 34518
	public ColorStyleSetting repeatOn;

	// Token: 0x040086D7 RID: 34519
	public ColorStyleSetting defaultButton;

	// Token: 0x040086D8 RID: 34520
	public ColorStyleSetting highlightButton;

	// Token: 0x040086DB RID: 34523
	private int m_refreshHandle = -1;
}
