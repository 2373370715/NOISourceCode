using System;
using Klei.CustomSettings;
using ProcGen;
using STRINGS;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x02001CC1 RID: 7361
public class ColonyDestinationSelectScreen : NewGameFlowScreen
{
	// Token: 0x06009982 RID: 39298 RVA: 0x003C3748 File Offset: 0x003C1948
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.backButton.onClick += this.BackClicked;
		this.customizeButton.onClick += this.CustomizeClicked;
		this.launchButton.onClick += this.LaunchClicked;
		this.shuffleButton.onClick += this.ShuffleClicked;
		this.storyTraitShuffleButton.onClick += this.StoryTraitShuffleClicked;
		this.storyTraitShuffleButton.gameObject.SetActive(Db.Get().Stories.Count > 5);
		this.destinationMapPanel.OnAsteroidClicked += this.OnAsteroidClicked;
		KInputTextField kinputTextField = this.coordinate;
		kinputTextField.onFocus = (System.Action)Delegate.Combine(kinputTextField.onFocus, new System.Action(this.CoordinateEditStarted));
		this.coordinate.onEndEdit.AddListener(new UnityAction<string>(this.CoordinateEditFinished));
		if (this.locationIcons != null)
		{
			bool cloudSavesAvailable = SaveLoader.GetCloudSavesAvailable();
			this.locationIcons.gameObject.SetActive(cloudSavesAvailable);
		}
		this.random = new KRandom();
	}

	// Token: 0x06009983 RID: 39299 RVA: 0x003C387C File Offset: 0x003C1A7C
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.RefreshCloudSavePref();
		this.RefreshCloudLocalIcon();
		this.newGameSettingsPanel.Init();
		this.newGameSettingsPanel.SetCloseAction(new System.Action(this.CustomizeClose));
		this.destinationMapPanel.Init();
		this.mixingPanel.Init();
		this.ShuffleClicked();
		this.RefreshMenuTabs();
		for (int i = 0; i < this.menuTabs.Length; i++)
		{
			int target = i;
			this.menuTabs[i].onClick = delegate()
			{
				this.selectedMenuTabIdx = target;
				this.RefreshMenuTabs();
			};
		}
		this.ResizeLayout();
		this.storyContentPanel.Init();
		this.storyContentPanel.SelectRandomStories(5, 5, true);
		this.storyContentPanel.SelectDefault();
		this.RefreshStoryLabel();
		this.RefreshRowsAndDescriptions();
		CustomGameSettings.Instance.OnQualitySettingChanged += this.QualitySettingChanged;
		CustomGameSettings.Instance.OnStorySettingChanged += this.QualitySettingChanged;
		CustomGameSettings.Instance.OnMixingSettingChanged += this.QualitySettingChanged;
		this.coordinate.text = CustomGameSettings.Instance.GetSettingsCoordinate();
	}

	// Token: 0x06009984 RID: 39300 RVA: 0x003C39AC File Offset: 0x003C1BAC
	private void ResizeLayout()
	{
		Vector2 sizeDelta = this.destinationProperties.clusterDetailsButton.rectTransform().sizeDelta;
		this.destinationProperties.clusterDetailsButton.rectTransform().sizeDelta = new Vector2(sizeDelta.x, (float)(DlcManager.FeatureClusterSpaceEnabled() ? 164 : 76));
		Vector2 sizeDelta2 = this.worldsScrollPanel.rectTransform().sizeDelta;
		Vector2 anchoredPosition = this.worldsScrollPanel.rectTransform().anchoredPosition;
		if (!DlcManager.FeatureClusterSpaceEnabled())
		{
			this.worldsScrollPanel.rectTransform().anchoredPosition = new Vector2(anchoredPosition.x, anchoredPosition.y + 88f);
		}
		float num = (float)(DlcManager.FeatureClusterSpaceEnabled() ? 436 : 524);
		LayoutRebuilder.ForceRebuildLayoutImmediate(base.gameObject.rectTransform());
		num = Mathf.Min(num, this.destinationInfoPanel.sizeDelta.y - (float)(DlcManager.FeatureClusterSpaceEnabled() ? 164 : 76) - 22f);
		this.worldsScrollPanel.rectTransform().sizeDelta = new Vector2(sizeDelta2.x, num);
		this.storyScrollPanel.rectTransform().sizeDelta = new Vector2(sizeDelta2.x, num);
		this.mixingScrollPanel.rectTransform().sizeDelta = new Vector2(sizeDelta2.x, num);
		this.gameSettingsScrollPanel.rectTransform().sizeDelta = new Vector2(sizeDelta2.x, num);
	}

	// Token: 0x06009985 RID: 39301 RVA: 0x003C3B14 File Offset: 0x003C1D14
	protected override void OnCleanUp()
	{
		CustomGameSettings.Instance.OnQualitySettingChanged -= this.QualitySettingChanged;
		CustomGameSettings.Instance.OnStorySettingChanged -= this.QualitySettingChanged;
		this.newGameSettingsPanel.Uninit();
		this.destinationMapPanel.Uninit();
		this.mixingPanel.Uninit();
		this.storyContentPanel.Cleanup();
		base.OnCleanUp();
	}

	// Token: 0x06009986 RID: 39302 RVA: 0x003C3B80 File Offset: 0x003C1D80
	private void RefreshCloudLocalIcon()
	{
		if (this.locationIcons == null)
		{
			return;
		}
		if (!SaveLoader.GetCloudSavesAvailable())
		{
			return;
		}
		HierarchyReferences component = this.locationIcons.GetComponent<HierarchyReferences>();
		LocText component2 = component.GetReference<RectTransform>("LocationText").GetComponent<LocText>();
		KButton component3 = component.GetReference<RectTransform>("CloudButton").GetComponent<KButton>();
		KButton component4 = component.GetReference<RectTransform>("LocalButton").GetComponent<KButton>();
		ToolTip component5 = component3.GetComponent<ToolTip>();
		ToolTip component6 = component4.GetComponent<ToolTip>();
		component5.toolTip = string.Format("{0}\n{1}", UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.SAVETOCLOUD.TOOLTIP, UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.SAVETOCLOUD.TOOLTIP_EXTRA);
		component6.toolTip = string.Format("{0}\n{1}", UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.SAVETOCLOUD.TOOLTIP_LOCAL, UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.SAVETOCLOUD.TOOLTIP_EXTRA);
		bool flag = CustomGameSettings.Instance.GetCurrentQualitySetting(CustomGameSettingConfigs.SaveToCloud).id == "Enabled";
		component2.text = (flag ? UI.FRONTEND.LOADSCREEN.CLOUD_SAVE : UI.FRONTEND.LOADSCREEN.LOCAL_SAVE);
		component3.gameObject.SetActive(flag);
		component3.ClearOnClick();
		if (flag)
		{
			component3.onClick += delegate()
			{
				CustomGameSettings.Instance.SetQualitySetting(CustomGameSettingConfigs.SaveToCloud, "Disabled");
				this.RefreshCloudLocalIcon();
			};
		}
		component4.gameObject.SetActive(!flag);
		component4.ClearOnClick();
		if (!flag)
		{
			component4.onClick += delegate()
			{
				CustomGameSettings.Instance.SetQualitySetting(CustomGameSettingConfigs.SaveToCloud, "Enabled");
				this.RefreshCloudLocalIcon();
			};
		}
	}

	// Token: 0x06009987 RID: 39303 RVA: 0x003C3CB4 File Offset: 0x003C1EB4
	private void RefreshCloudSavePref()
	{
		if (!SaveLoader.GetCloudSavesAvailable())
		{
			return;
		}
		string cloudSavesDefaultPref = SaveLoader.GetCloudSavesDefaultPref();
		CustomGameSettings.Instance.SetQualitySetting(CustomGameSettingConfigs.SaveToCloud, cloudSavesDefaultPref);
	}

	// Token: 0x06009988 RID: 39304 RVA: 0x001083BB File Offset: 0x001065BB
	private void BackClicked()
	{
		this.newGameSettingsPanel.Cancel();
		base.NavigateBackward();
	}

	// Token: 0x06009989 RID: 39305 RVA: 0x001083CE File Offset: 0x001065CE
	private void CustomizeClicked()
	{
		this.newGameSettingsPanel.Refresh();
		this.customSettings.SetActive(true);
	}

	// Token: 0x0600998A RID: 39306 RVA: 0x001083E7 File Offset: 0x001065E7
	private void CustomizeClose()
	{
		this.customSettings.SetActive(false);
	}

	// Token: 0x0600998B RID: 39307 RVA: 0x001083F5 File Offset: 0x001065F5
	private void LaunchClicked()
	{
		CustomGameSettings.Instance.RemoveInvalidMixingSettings();
		base.NavigateForward();
	}

	// Token: 0x0600998C RID: 39308 RVA: 0x003C3CE0 File Offset: 0x003C1EE0
	private void RefreshMenuTabs()
	{
		for (int i = 0; i < this.menuTabs.Length; i++)
		{
			this.menuTabs[i].ChangeState((i == this.selectedMenuTabIdx) ? 1 : 0);
			LocText componentInChildren = this.menuTabs[i].GetComponentInChildren<LocText>();
			HierarchyReferences component = this.menuTabs[i].GetComponent<HierarchyReferences>();
			if (componentInChildren != null)
			{
				componentInChildren.color = ((i == this.selectedMenuTabIdx) ? Color.white : Color.grey);
			}
			if (component != null)
			{
				Image reference = component.GetReference<Image>("Icon");
				if (reference != null)
				{
					reference.color = ((i == this.selectedMenuTabIdx) ? Color.white : Color.grey);
				}
			}
		}
		this.destinationInfoPanel.gameObject.SetActive(this.selectedMenuTabIdx == 1);
		this.storyInfoPanel.gameObject.SetActive(this.selectedMenuTabIdx == 2);
		this.mixingSettingsPanel.gameObject.SetActive(this.selectedMenuTabIdx == 3);
		this.gameSettingsPanel.gameObject.SetActive(this.selectedMenuTabIdx == 4);
		int num = this.selectedMenuTabIdx;
		if (num != 1)
		{
			if (num == 2)
			{
				this.destinationDetailsHeader.SetParent(this.destinationDetailsParent_Story);
			}
		}
		else
		{
			this.destinationDetailsHeader.SetParent(this.destinationDetailsParent_Asteroid);
		}
		this.destinationDetailsHeader.SetAsFirstSibling();
	}

	// Token: 0x0600998D RID: 39309 RVA: 0x003C3E40 File Offset: 0x003C2040
	private void ShuffleClicked()
	{
		ClusterLayout currentClusterLayout = CustomGameSettings.Instance.GetCurrentClusterLayout();
		int num = this.random.Next();
		if (currentClusterLayout != null && currentClusterLayout.fixedCoordinate != -1)
		{
			num = currentClusterLayout.fixedCoordinate;
		}
		this.newGameSettingsPanel.SetSetting(CustomGameSettingConfigs.WorldgenSeed, num.ToString(), true);
	}

	// Token: 0x0600998E RID: 39310 RVA: 0x00108407 File Offset: 0x00106607
	private void StoryTraitShuffleClicked()
	{
		this.storyContentPanel.SelectRandomStories(5, 5, false);
	}

	// Token: 0x0600998F RID: 39311 RVA: 0x003C3E90 File Offset: 0x003C2090
	private void CoordinateChanged(string text)
	{
		string[] array = CustomGameSettings.ParseSettingCoordinate(text);
		if (array.Length < 4 || array.Length > 6)
		{
			return;
		}
		int num;
		if (!int.TryParse(array[2], out num))
		{
			return;
		}
		ClusterLayout clusterLayout = null;
		foreach (string name in SettingsCache.GetClusterNames())
		{
			ClusterLayout clusterData = SettingsCache.clusterLayouts.GetClusterData(name);
			if (clusterData.coordinatePrefix == array[1])
			{
				clusterLayout = clusterData;
			}
		}
		if (clusterLayout != null)
		{
			this.newGameSettingsPanel.SetSetting(CustomGameSettingConfigs.ClusterLayout, clusterLayout.filePath, true);
		}
		this.newGameSettingsPanel.SetSetting(CustomGameSettingConfigs.WorldgenSeed, array[2], true);
		this.newGameSettingsPanel.ConsumeSettingsCode(array[3]);
		string code = (array.Length >= 5) ? array[4] : "0";
		this.newGameSettingsPanel.ConsumeStoryTraitsCode(code);
		string code2 = (array.Length >= 6) ? array[5] : "0";
		this.newGameSettingsPanel.ConsumeMixingSettingsCode(code2);
	}

	// Token: 0x06009990 RID: 39312 RVA: 0x00108417 File Offset: 0x00106617
	private void CoordinateEditStarted()
	{
		this.isEditingCoordinate = true;
	}

	// Token: 0x06009991 RID: 39313 RVA: 0x00108420 File Offset: 0x00106620
	private void CoordinateEditFinished(string text)
	{
		this.CoordinateChanged(text);
		this.isEditingCoordinate = false;
		this.coordinate.text = CustomGameSettings.Instance.GetSettingsCoordinate();
	}

	// Token: 0x06009992 RID: 39314 RVA: 0x003C3F98 File Offset: 0x003C2198
	private void QualitySettingChanged(SettingConfig config, SettingLevel level)
	{
		if (config == CustomGameSettingConfigs.SaveToCloud)
		{
			this.RefreshCloudLocalIcon();
		}
		if (!this.destinationDetailsHeader.IsNullOrDestroyed())
		{
			if (!this.isEditingCoordinate && !this.coordinate.IsNullOrDestroyed())
			{
				this.coordinate.text = CustomGameSettings.Instance.GetSettingsCoordinate();
			}
			this.RefreshRowsAndDescriptions();
		}
	}

	// Token: 0x06009993 RID: 39315 RVA: 0x003C3FF0 File Offset: 0x003C21F0
	public void RefreshRowsAndDescriptions()
	{
		string setting = this.newGameSettingsPanel.GetSetting(CustomGameSettingConfigs.ClusterLayout);
		int seed;
		int.TryParse(this.newGameSettingsPanel.GetSetting(CustomGameSettingConfigs.WorldgenSeed), out seed);
		int fixedCoordinate = CustomGameSettings.Instance.GetCurrentClusterLayout().fixedCoordinate;
		if (fixedCoordinate != -1)
		{
			this.newGameSettingsPanel.SetSetting(CustomGameSettingConfigs.WorldgenSeed, fixedCoordinate.ToString(), false);
			seed = fixedCoordinate;
			this.shuffleButton.isInteractable = false;
			this.shuffleButton.GetComponent<ToolTip>().SetSimpleTooltip(UI.FRONTEND.COLONYDESTINATIONSCREEN.SHUFFLETOOLTIP_DISABLED);
		}
		else
		{
			this.coordinate.interactable = true;
			this.shuffleButton.isInteractable = true;
			this.shuffleButton.GetComponent<ToolTip>().SetSimpleTooltip(UI.FRONTEND.COLONYDESTINATIONSCREEN.SHUFFLETOOLTIP);
		}
		ColonyDestinationAsteroidBeltData cluster;
		try
		{
			cluster = this.destinationMapPanel.SelectCluster(setting, seed);
		}
		catch
		{
			string defaultAsteroid = this.destinationMapPanel.GetDefaultAsteroid();
			this.newGameSettingsPanel.SetSetting(CustomGameSettingConfigs.ClusterLayout, defaultAsteroid, true);
			cluster = this.destinationMapPanel.SelectCluster(defaultAsteroid, seed);
		}
		if (DlcManager.IsContentSubscribed("EXPANSION1_ID"))
		{
			this.destinationProperties.EnableClusterLocationLabels(true);
			this.destinationProperties.RefreshAsteroidLines(cluster, this.selectedLocationProperties, this.storyContentPanel.GetActiveStories());
			this.destinationProperties.EnableClusterDetails(true);
			this.destinationProperties.SetClusterDetailLabels(cluster);
			this.selectedLocationProperties.headerLabel.SetText(UI.FRONTEND.COLONYDESTINATIONSCREEN.SELECTED_CLUSTER_TRAITS_HEADER);
			this.destinationProperties.clusterDetailsButton.onClick = delegate()
			{
				this.destinationProperties.SelectWholeClusterDetails(cluster, this.selectedLocationProperties, this.storyContentPanel.GetActiveStories());
			};
		}
		else
		{
			this.destinationProperties.EnableClusterDetails(false);
			this.destinationProperties.EnableClusterLocationLabels(false);
			this.destinationProperties.SetParameterDescriptors(cluster.GetParamDescriptors());
			this.selectedLocationProperties.SetTraitDescriptors(cluster.GetTraitDescriptors(), this.storyContentPanel.GetActiveStories(), true);
		}
		this.RefreshStoryLabel();
	}

	// Token: 0x06009994 RID: 39316 RVA: 0x00108445 File Offset: 0x00106645
	public void RefreshStoryLabel()
	{
		this.storyTraitsDestinationDetailsLabel.SetText(this.storyContentPanel.GetTraitsString(false));
		this.storyTraitsDestinationDetailsLabel.GetComponent<ToolTip>().SetSimpleTooltip(this.storyContentPanel.GetTraitsString(true));
	}

	// Token: 0x06009995 RID: 39317 RVA: 0x0010847A File Offset: 0x0010667A
	private void OnAsteroidClicked(ColonyDestinationAsteroidBeltData cluster)
	{
		this.newGameSettingsPanel.SetSetting(CustomGameSettingConfigs.ClusterLayout, cluster.beltPath, true);
		this.ShuffleClicked();
	}

	// Token: 0x06009996 RID: 39318 RVA: 0x003C4200 File Offset: 0x003C2400
	public override void OnKeyDown(KButtonEvent e)
	{
		if (this.isEditingCoordinate)
		{
			return;
		}
		if (!e.Consumed && e.TryConsume(global::Action.PanLeft))
		{
			this.destinationMapPanel.ScrollLeft();
		}
		else if (!e.Consumed && e.TryConsume(global::Action.PanRight))
		{
			this.destinationMapPanel.ScrollRight();
		}
		else if (this.customSettings.activeSelf && !e.Consumed && (e.TryConsume(global::Action.Escape) || e.TryConsume(global::Action.MouseRight)))
		{
			this.CustomizeClose();
		}
		base.OnKeyDown(e);
	}

	// Token: 0x04007758 RID: 30552
	[SerializeField]
	private GameObject destinationMap;

	// Token: 0x04007759 RID: 30553
	[SerializeField]
	private GameObject customSettings;

	// Token: 0x0400775A RID: 30554
	[Header("Menu")]
	[SerializeField]
	private MultiToggle[] menuTabs;

	// Token: 0x0400775B RID: 30555
	private int selectedMenuTabIdx = 1;

	// Token: 0x0400775C RID: 30556
	[Header("Buttons")]
	[SerializeField]
	private KButton backButton;

	// Token: 0x0400775D RID: 30557
	[SerializeField]
	private KButton customizeButton;

	// Token: 0x0400775E RID: 30558
	[SerializeField]
	private KButton launchButton;

	// Token: 0x0400775F RID: 30559
	[SerializeField]
	private KButton shuffleButton;

	// Token: 0x04007760 RID: 30560
	[SerializeField]
	private KButton storyTraitShuffleButton;

	// Token: 0x04007761 RID: 30561
	[Header("Scroll Panels")]
	[SerializeField]
	private RectTransform worldsScrollPanel;

	// Token: 0x04007762 RID: 30562
	[SerializeField]
	private RectTransform storyScrollPanel;

	// Token: 0x04007763 RID: 30563
	[SerializeField]
	private RectTransform mixingScrollPanel;

	// Token: 0x04007764 RID: 30564
	[SerializeField]
	private RectTransform gameSettingsScrollPanel;

	// Token: 0x04007765 RID: 30565
	[Header("Panels")]
	[SerializeField]
	private RectTransform destinationDetailsHeader;

	// Token: 0x04007766 RID: 30566
	[SerializeField]
	private RectTransform destinationInfoPanel;

	// Token: 0x04007767 RID: 30567
	[SerializeField]
	private RectTransform storyInfoPanel;

	// Token: 0x04007768 RID: 30568
	[SerializeField]
	private RectTransform mixingSettingsPanel;

	// Token: 0x04007769 RID: 30569
	[SerializeField]
	private RectTransform gameSettingsPanel;

	// Token: 0x0400776A RID: 30570
	[Header("References")]
	[SerializeField]
	private RectTransform destinationDetailsParent_Asteroid;

	// Token: 0x0400776B RID: 30571
	[SerializeField]
	private RectTransform destinationDetailsParent_Story;

	// Token: 0x0400776C RID: 30572
	[SerializeField]
	private LocText storyTraitsDestinationDetailsLabel;

	// Token: 0x0400776D RID: 30573
	[SerializeField]
	private HierarchyReferences locationIcons;

	// Token: 0x0400776E RID: 30574
	[SerializeField]
	private KInputTextField coordinate;

	// Token: 0x0400776F RID: 30575
	[SerializeField]
	private StoryContentPanel storyContentPanel;

	// Token: 0x04007770 RID: 30576
	[SerializeField]
	private AsteroidDescriptorPanel destinationProperties;

	// Token: 0x04007771 RID: 30577
	[SerializeField]
	private AsteroidDescriptorPanel selectedLocationProperties;

	// Token: 0x04007772 RID: 30578
	private const int DESTINATION_HEADER_BUTTON_HEIGHT_CLUSTER = 164;

	// Token: 0x04007773 RID: 30579
	private const int DESTINATION_HEADER_BUTTON_HEIGHT_BASE = 76;

	// Token: 0x04007774 RID: 30580
	private const int WORLDS_SCROLL_PANEL_HEIGHT_CLUSTER = 436;

	// Token: 0x04007775 RID: 30581
	private const int WORLDS_SCROLL_PANEL_HEIGHT_BASE = 524;

	// Token: 0x04007776 RID: 30582
	[SerializeField]
	private NewGameSettingsPanel newGameSettingsPanel;

	// Token: 0x04007777 RID: 30583
	[MyCmpReq]
	private DestinationSelectPanel destinationMapPanel;

	// Token: 0x04007778 RID: 30584
	[SerializeField]
	private MixingContentPanel mixingPanel;

	// Token: 0x04007779 RID: 30585
	private KRandom random;

	// Token: 0x0400777A RID: 30586
	private bool isEditingCoordinate;
}
