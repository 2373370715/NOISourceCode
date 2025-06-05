using System;
using System.Collections.Generic;
using FMOD.Studio;
using STRINGS;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x02001B3C RID: 6972
public class ManagementMenu : KIconToggleMenu
{
	// Token: 0x0600922D RID: 37421 RVA: 0x00104018 File Offset: 0x00102218
	public static void DestroyInstance()
	{
		ManagementMenu.Instance = null;
	}

	// Token: 0x0600922E RID: 37422 RVA: 0x00104020 File Offset: 0x00102220
	public override float GetSortKey()
	{
		return 21f;
	}

	// Token: 0x0600922F RID: 37423 RVA: 0x00391458 File Offset: 0x0038F658
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		ManagementMenu.Instance = this;
		this.notificationDisplayer.onNotificationsChanged += this.OnNotificationsChanged;
		CodexCache.CodexCacheInit();
		ScheduledUIInstantiation component = GameScreenManager.Instance.GetComponent<ScheduledUIInstantiation>();
		this.starmapScreen = component.GetInstantiatedObject<StarmapScreen>();
		this.clusterMapScreen = component.GetInstantiatedObject<ClusterMapScreen>();
		this.skillsScreen = component.GetInstantiatedObject<SkillsScreen>();
		this.researchScreen = component.GetInstantiatedObject<ResearchScreen>();
		this.fullscreenUIs = new ManagementMenu.ManagementMenuToggleInfo[]
		{
			this.researchInfo,
			this.skillsInfo,
			this.starmapInfo,
			this.clusterMapInfo
		};
		base.Subscribe(Game.Instance.gameObject, 288942073, new Action<object>(this.OnUIClear));
		this.consumablesInfo = new ManagementMenu.ManagementMenuToggleInfo(UI.CONSUMABLES, "OverviewUI_consumables_icon", null, global::Action.ManageConsumables, UI.TOOLTIPS.MANAGEMENTMENU_CONSUMABLES, "");
		this.AddToggleTooltip(this.consumablesInfo, null);
		this.vitalsInfo = new ManagementMenu.ManagementMenuToggleInfo(UI.VITALS, "OverviewUI_vitals_icon", null, global::Action.ManageVitals, UI.TOOLTIPS.MANAGEMENTMENU_VITALS, "");
		this.AddToggleTooltip(this.vitalsInfo, null);
		this.researchInfo = new ManagementMenu.ManagementMenuToggleInfo(UI.RESEARCH, "OverviewUI_research_nav_icon", null, global::Action.ManageResearch, UI.TOOLTIPS.MANAGEMENTMENU_RESEARCH, "");
		this.AddToggleTooltipForResearch(this.researchInfo, UI.TOOLTIPS.MANAGEMENTMENU_REQUIRES_RESEARCH);
		this.researchInfo.prefabOverride = this.researchButtonPrefab;
		this.jobsInfo = new ManagementMenu.ManagementMenuToggleInfo(UI.JOBS, "OverviewUI_priority_icon", null, global::Action.ManagePriorities, UI.TOOLTIPS.MANAGEMENTMENU_JOBS, "");
		this.AddToggleTooltip(this.jobsInfo, null);
		this.skillsInfo = new ManagementMenu.ManagementMenuToggleInfo(UI.SKILLS, "OverviewUI_jobs_icon", null, global::Action.ManageSkills, UI.TOOLTIPS.MANAGEMENTMENU_SKILLS, "");
		this.AddToggleTooltip(this.skillsInfo, UI.TOOLTIPS.MANAGEMENTMENU_REQUIRES_SKILL_STATION);
		this.starmapInfo = new ManagementMenu.ManagementMenuToggleInfo(UI.STARMAP.MANAGEMENT_BUTTON, "OverviewUI_starmap_icon", null, global::Action.ManageStarmap, UI.TOOLTIPS.MANAGEMENTMENU_STARMAP, "");
		this.AddToggleTooltip(this.starmapInfo, UI.TOOLTIPS.MANAGEMENTMENU_REQUIRES_TELESCOPE);
		this.clusterMapInfo = new ManagementMenu.ManagementMenuToggleInfo(UI.STARMAP.MANAGEMENT_BUTTON, "OverviewUI_starmap_icon", null, global::Action.ManageStarmap, UI.TOOLTIPS.MANAGEMENTMENU_STARMAP, "");
		this.AddToggleTooltip(this.clusterMapInfo, null);
		this.scheduleInfo = new ManagementMenu.ManagementMenuToggleInfo(UI.SCHEDULE, "OverviewUI_schedule2_icon", null, global::Action.ManageSchedule, UI.TOOLTIPS.MANAGEMENTMENU_SCHEDULE, "");
		this.AddToggleTooltip(this.scheduleInfo, null);
		this.reportsInfo = new ManagementMenu.ManagementMenuToggleInfo(UI.REPORT, "OverviewUI_reports_icon", null, global::Action.ManageReport, UI.TOOLTIPS.MANAGEMENTMENU_DAILYREPORT, "");
		this.AddToggleTooltip(this.reportsInfo, null);
		this.reportsInfo.prefabOverride = this.smallPrefab;
		this.codexInfo = new ManagementMenu.ManagementMenuToggleInfo(UI.CODEX.MANAGEMENT_BUTTON, "OverviewUI_database_icon", null, global::Action.ManageDatabase, UI.TOOLTIPS.MANAGEMENTMENU_CODEX, "");
		this.AddToggleTooltip(this.codexInfo, null);
		this.codexInfo.prefabOverride = this.smallPrefab;
		this.ScreenInfoMatch.Add(this.consumablesInfo, new ManagementMenu.ScreenData
		{
			screen = this.consumablesScreen,
			tabIdx = 3,
			toggleInfo = this.consumablesInfo,
			cancelHandler = null
		});
		this.ScreenInfoMatch.Add(this.vitalsInfo, new ManagementMenu.ScreenData
		{
			screen = this.vitalsScreen,
			tabIdx = 2,
			toggleInfo = this.vitalsInfo,
			cancelHandler = null
		});
		this.ScreenInfoMatch.Add(this.reportsInfo, new ManagementMenu.ScreenData
		{
			screen = this.reportsScreen,
			tabIdx = 4,
			toggleInfo = this.reportsInfo,
			cancelHandler = null
		});
		this.ScreenInfoMatch.Add(this.jobsInfo, new ManagementMenu.ScreenData
		{
			screen = this.jobsScreen,
			tabIdx = 1,
			toggleInfo = this.jobsInfo,
			cancelHandler = null
		});
		this.ScreenInfoMatch.Add(this.skillsInfo, new ManagementMenu.ScreenData
		{
			screen = this.skillsScreen,
			tabIdx = 0,
			toggleInfo = this.skillsInfo,
			cancelHandler = null
		});
		this.ScreenInfoMatch.Add(this.codexInfo, new ManagementMenu.ScreenData
		{
			screen = this.codexScreen,
			tabIdx = 6,
			toggleInfo = this.codexInfo,
			cancelHandler = null
		});
		this.ScreenInfoMatch.Add(this.scheduleInfo, new ManagementMenu.ScreenData
		{
			screen = this.scheduleScreen,
			tabIdx = 7,
			toggleInfo = this.scheduleInfo,
			cancelHandler = null
		});
		if (DlcManager.FeatureClusterSpaceEnabled())
		{
			this.ScreenInfoMatch.Add(this.clusterMapInfo, new ManagementMenu.ScreenData
			{
				screen = this.clusterMapScreen,
				tabIdx = 7,
				toggleInfo = this.clusterMapInfo,
				cancelHandler = new Func<bool>(this.clusterMapScreen.TryHandleCancel)
			});
		}
		else
		{
			this.ScreenInfoMatch.Add(this.starmapInfo, new ManagementMenu.ScreenData
			{
				screen = this.starmapScreen,
				tabIdx = 7,
				toggleInfo = this.starmapInfo,
				cancelHandler = null
			});
		}
		this.ScreenInfoMatch.Add(this.researchInfo, new ManagementMenu.ScreenData
		{
			screen = this.researchScreen,
			tabIdx = 5,
			toggleInfo = this.researchInfo,
			cancelHandler = null
		});
		List<KIconToggleMenu.ToggleInfo> list = new List<KIconToggleMenu.ToggleInfo>();
		list.Add(this.vitalsInfo);
		list.Add(this.consumablesInfo);
		list.Add(this.jobsInfo);
		list.Add(this.scheduleInfo);
		list.Add(this.skillsInfo);
		list.Add(this.researchInfo);
		if (DlcManager.FeatureClusterSpaceEnabled())
		{
			list.Add(this.clusterMapInfo);
		}
		else
		{
			list.Add(this.starmapInfo);
		}
		list.Add(this.reportsInfo);
		list.Add(this.codexInfo);
		base.Setup(list);
		base.onSelect += this.OnButtonClick;
		this.PauseMenuButton.onClick += this.OnPauseMenuClicked;
		this.PauseMenuButton.transform.SetAsLastSibling();
		this.PauseMenuButton.GetComponent<ToolTip>().toolTip = GameUtil.ReplaceHotkeyString(UI.TOOLTIPS.MANAGEMENTMENU_PAUSEMENU, global::Action.Escape);
		KInputManager.InputChange.AddListener(new UnityAction(this.OnInputChanged));
		Components.ResearchCenters.OnAdd += new Action<IResearchCenter>(this.CheckResearch);
		Components.ResearchCenters.OnRemove += new Action<IResearchCenter>(this.CheckResearch);
		Components.RoleStations.OnAdd += new Action<RoleStation>(this.CheckSkills);
		Components.RoleStations.OnRemove += new Action<RoleStation>(this.CheckSkills);
		Game.Instance.Subscribe(-809948329, new Action<object>(this.CheckResearch));
		Game.Instance.Subscribe(-809948329, new Action<object>(this.CheckSkills));
		Game.Instance.Subscribe(445618876, new Action<object>(this.OnResolutionChanged));
		if (!DlcManager.FeatureClusterSpaceEnabled())
		{
			Components.Telescopes.OnAdd += new Action<Telescope>(this.CheckStarmap);
			Components.Telescopes.OnRemove += new Action<Telescope>(this.CheckStarmap);
		}
		this.CheckResearch(null);
		this.CheckSkills(null);
		if (!DlcManager.FeatureClusterSpaceEnabled())
		{
			this.CheckStarmap(null);
		}
		this.researchInfo.toggle.soundPlayer.AcceptClickCondition = (() => this.ResearchAvailable() || this.activeScreen == this.ScreenInfoMatch[ManagementMenu.Instance.researchInfo]);
		foreach (KToggle ktoggle in this.toggles)
		{
			ktoggle.soundPlayer.toggle_widget_sound_events[0].PlaySound = false;
			ktoggle.soundPlayer.toggle_widget_sound_events[1].PlaySound = false;
		}
		this.OnResolutionChanged(null);
	}

	// Token: 0x06009230 RID: 37424 RVA: 0x00104027 File Offset: 0x00102227
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.mutuallyExclusiveScreens.Add(AllResourcesScreen.Instance);
		this.mutuallyExclusiveScreens.Add(AllDiagnosticsScreen.Instance);
		this.OnNotificationsChanged();
	}

	// Token: 0x06009231 RID: 37425 RVA: 0x00104055 File Offset: 0x00102255
	protected override void OnForcedCleanUp()
	{
		KInputManager.InputChange.RemoveListener(new UnityAction(this.OnInputChanged));
		base.OnForcedCleanUp();
	}

	// Token: 0x06009232 RID: 37426 RVA: 0x00391CA0 File Offset: 0x0038FEA0
	private void OnInputChanged()
	{
		this.PauseMenuButton.GetComponent<ToolTip>().toolTip = GameUtil.ReplaceHotkeyString(UI.TOOLTIPS.MANAGEMENTMENU_PAUSEMENU, global::Action.Escape);
		this.consumablesInfo.tooltip = GameUtil.ReplaceHotkeyString(UI.TOOLTIPS.MANAGEMENTMENU_CONSUMABLES, this.consumablesInfo.hotKey);
		this.vitalsInfo.tooltip = GameUtil.ReplaceHotkeyString(UI.TOOLTIPS.MANAGEMENTMENU_VITALS, this.vitalsInfo.hotKey);
		this.researchInfo.tooltip = GameUtil.ReplaceHotkeyString(UI.TOOLTIPS.MANAGEMENTMENU_RESEARCH, this.researchInfo.hotKey);
		this.jobsInfo.tooltip = GameUtil.ReplaceHotkeyString(UI.TOOLTIPS.MANAGEMENTMENU_JOBS, this.jobsInfo.hotKey);
		this.skillsInfo.tooltip = GameUtil.ReplaceHotkeyString(UI.TOOLTIPS.MANAGEMENTMENU_SKILLS, this.skillsInfo.hotKey);
		this.starmapInfo.tooltip = GameUtil.ReplaceHotkeyString(UI.TOOLTIPS.MANAGEMENTMENU_STARMAP, this.starmapInfo.hotKey);
		this.clusterMapInfo.tooltip = GameUtil.ReplaceHotkeyString(UI.TOOLTIPS.MANAGEMENTMENU_STARMAP, this.clusterMapInfo.hotKey);
		this.scheduleInfo.tooltip = GameUtil.ReplaceHotkeyString(UI.TOOLTIPS.MANAGEMENTMENU_SCHEDULE, this.scheduleInfo.hotKey);
		this.reportsInfo.tooltip = GameUtil.ReplaceHotkeyString(UI.TOOLTIPS.MANAGEMENTMENU_DAILYREPORT, this.reportsInfo.hotKey);
		this.codexInfo.tooltip = GameUtil.ReplaceHotkeyString(UI.TOOLTIPS.MANAGEMENTMENU_CODEX, this.codexInfo.hotKey);
	}

	// Token: 0x06009233 RID: 37427 RVA: 0x00391E40 File Offset: 0x00390040
	private void OnResolutionChanged(object data = null)
	{
		bool flag = (float)Screen.width < 1300f;
		foreach (KToggle ktoggle in this.toggles)
		{
			HierarchyReferences component = ktoggle.GetComponent<HierarchyReferences>();
			if (!(component == null))
			{
				RectTransform reference = component.GetReference<RectTransform>("TextContainer");
				if (!(reference == null))
				{
					reference.gameObject.SetActive(!flag);
				}
			}
		}
	}

	// Token: 0x06009234 RID: 37428 RVA: 0x00391ECC File Offset: 0x003900CC
	private void OnNotificationsChanged()
	{
		foreach (KeyValuePair<ManagementMenu.ManagementMenuToggleInfo, ManagementMenu.ScreenData> keyValuePair in this.ScreenInfoMatch)
		{
			keyValuePair.Key.SetNotificationDisplay(false, false, null, this.noAlertColorStyle);
		}
	}

	// Token: 0x06009235 RID: 37429 RVA: 0x00104073 File Offset: 0x00102273
	private ToolTip.ComplexTooltipDelegate CreateToggleTooltip(ManagementMenu.ManagementMenuToggleInfo toggleInfo, string disabledTooltip = null)
	{
		return delegate()
		{
			List<global::Tuple<string, TextStyleSetting>> list = new List<global::Tuple<string, TextStyleSetting>>();
			if (disabledTooltip != null && !toggleInfo.toggle.interactable)
			{
				list.Add(new global::Tuple<string, TextStyleSetting>(disabledTooltip, ToolTipScreen.Instance.defaultTooltipBodyStyle));
				return list;
			}
			if (toggleInfo.tooltipHeader != null)
			{
				list.Add(new global::Tuple<string, TextStyleSetting>(toggleInfo.tooltipHeader, ToolTipScreen.Instance.defaultTooltipHeaderStyle));
			}
			list.Add(new global::Tuple<string, TextStyleSetting>(toggleInfo.tooltip, ToolTipScreen.Instance.defaultTooltipBodyStyle));
			return list;
		};
	}

	// Token: 0x06009236 RID: 37430 RVA: 0x00104093 File Offset: 0x00102293
	private void AddToggleTooltip(ManagementMenu.ManagementMenuToggleInfo toggleInfo, string disabledTooltip = null)
	{
		toggleInfo.getTooltipText = this.CreateToggleTooltip(toggleInfo, disabledTooltip);
	}

	// Token: 0x06009237 RID: 37431 RVA: 0x00391F30 File Offset: 0x00390130
	private void AddToggleTooltipForResearch(ManagementMenu.ManagementMenuToggleInfo toggleInfo, string disabledTooltip = null)
	{
		toggleInfo.getTooltipText = delegate()
		{
			List<global::Tuple<string, TextStyleSetting>> list = new List<global::Tuple<string, TextStyleSetting>>();
			TechInstance activeResearch = Research.Instance.GetActiveResearch();
			string a = (activeResearch == null) ? UI.TOOLTIPS.MANAGEMENTMENU_RESEARCH_NO_RESEARCH : string.Format(UI.TOOLTIPS.MANAGEMENTMENU_RESEARCH_CARD_NAME, activeResearch.tech.Name);
			list.Add(new global::Tuple<string, TextStyleSetting>(a, ToolTipScreen.Instance.defaultTooltipHeaderStyle));
			if (activeResearch != null)
			{
				string text = "";
				for (int i = 0; i < activeResearch.tech.unlockedItems.Count; i++)
				{
					TechItem techItem = activeResearch.tech.unlockedItems[i];
					text = text + "\n" + string.Format(UI.TOOLTIPS.MANAGEMENTMENU_RESEARCH_ITEM_LINE, techItem.Name);
				}
				list.Add(new global::Tuple<string, TextStyleSetting>(text, ToolTipScreen.Instance.defaultTooltipBodyStyle));
			}
			if (disabledTooltip != null && !toggleInfo.toggle.interactable)
			{
				list.Add(new global::Tuple<string, TextStyleSetting>(disabledTooltip, ToolTipScreen.Instance.defaultTooltipBodyStyle));
				return list;
			}
			if (toggleInfo.tooltipHeader != null)
			{
				list.Add(new global::Tuple<string, TextStyleSetting>(toggleInfo.tooltipHeader, ToolTipScreen.Instance.defaultTooltipHeaderStyle));
			}
			list.Add(new global::Tuple<string, TextStyleSetting>("\n" + toggleInfo.tooltip, ToolTipScreen.Instance.defaultTooltipBodyStyle));
			return list;
		};
	}

	// Token: 0x06009238 RID: 37432 RVA: 0x00391F68 File Offset: 0x00390168
	public bool IsFullscreenUIActive()
	{
		if (this.activeScreen == null)
		{
			return false;
		}
		foreach (ManagementMenu.ManagementMenuToggleInfo managementMenuToggleInfo in this.fullscreenUIs)
		{
			if (this.activeScreen.toggleInfo == managementMenuToggleInfo)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06009239 RID: 37433 RVA: 0x001040A3 File Offset: 0x001022A3
	private void OnPauseMenuClicked()
	{
		PauseScreen.Instance.Show(true);
		this.PauseMenuButton.isOn = false;
	}

	// Token: 0x0600923A RID: 37434 RVA: 0x001040BC File Offset: 0x001022BC
	public void Refresh()
	{
		this.CheckResearch(null);
		this.CheckSkills(null);
		this.CheckStarmap(null);
	}

	// Token: 0x0600923B RID: 37435 RVA: 0x00391FAC File Offset: 0x003901AC
	public void CheckResearch(object o)
	{
		if (this.researchInfo.toggle == null)
		{
			return;
		}
		bool flag = Components.ResearchCenters.Count <= 0 && !DebugHandler.InstantBuildMode && !Game.Instance.SandboxModeActive;
		bool active = !flag && this.activeScreen != null && this.activeScreen.toggleInfo == this.researchInfo;
		this.ConfigureToggle(this.researchInfo.toggle, flag, active);
	}

	// Token: 0x0600923C RID: 37436 RVA: 0x00392028 File Offset: 0x00390228
	public void CheckSkills(object o = null)
	{
		if (this.skillsInfo.toggle == null)
		{
			return;
		}
		bool disabled = Components.RoleStations.Count <= 0 && !DebugHandler.InstantBuildMode && !Game.Instance.SandboxModeActive;
		bool active = this.activeScreen != null && this.activeScreen.toggleInfo == this.skillsInfo;
		this.ConfigureToggle(this.skillsInfo.toggle, disabled, active);
	}

	// Token: 0x0600923D RID: 37437 RVA: 0x003920A0 File Offset: 0x003902A0
	public void CheckStarmap(object o = null)
	{
		if (this.starmapInfo.toggle == null)
		{
			return;
		}
		bool disabled = Components.Telescopes.Count <= 0 && !DebugHandler.InstantBuildMode && !Game.Instance.SandboxModeActive;
		bool active = this.activeScreen != null && this.activeScreen.toggleInfo == this.starmapInfo;
		this.ConfigureToggle(this.starmapInfo.toggle, disabled, active);
	}

	// Token: 0x0600923E RID: 37438 RVA: 0x001040D3 File Offset: 0x001022D3
	private void ConfigureToggle(KToggle toggle, bool disabled, bool active)
	{
		toggle.interactable = !disabled;
		if (disabled)
		{
			toggle.GetComponentInChildren<ImageToggleState>().SetDisabled();
			return;
		}
		toggle.GetComponentInChildren<ImageToggleState>().SetActiveState(active);
	}

	// Token: 0x0600923F RID: 37439 RVA: 0x001040FA File Offset: 0x001022FA
	public override void OnKeyDown(KButtonEvent e)
	{
		if (this.activeScreen != null && e.TryConsume(global::Action.Escape))
		{
			this.ToggleIfCancelUnhandled(this.activeScreen);
		}
		if (!e.Consumed)
		{
			base.OnKeyDown(e);
		}
	}

	// Token: 0x06009240 RID: 37440 RVA: 0x00104128 File Offset: 0x00102328
	public override void OnKeyUp(KButtonEvent e)
	{
		if (this.activeScreen != null && PlayerController.Instance.ConsumeIfNotDragging(e, global::Action.MouseRight))
		{
			this.ToggleIfCancelUnhandled(this.activeScreen);
		}
		if (!e.Consumed)
		{
			base.OnKeyUp(e);
		}
	}

	// Token: 0x06009241 RID: 37441 RVA: 0x0010415B File Offset: 0x0010235B
	private void ToggleIfCancelUnhandled(ManagementMenu.ScreenData screenData)
	{
		if (screenData.cancelHandler == null || !screenData.cancelHandler())
		{
			this.ToggleScreen(screenData);
		}
	}

	// Token: 0x06009242 RID: 37442 RVA: 0x00104179 File Offset: 0x00102379
	private bool ResearchAvailable()
	{
		return Components.ResearchCenters.Count > 0 || DebugHandler.InstantBuildMode || Game.Instance.SandboxModeActive;
	}

	// Token: 0x06009243 RID: 37443 RVA: 0x0010419B File Offset: 0x0010239B
	private bool SkillsAvailable()
	{
		return Components.RoleStations.Count > 0 || DebugHandler.InstantBuildMode || Game.Instance.SandboxModeActive;
	}

	// Token: 0x06009244 RID: 37444 RVA: 0x001041BD File Offset: 0x001023BD
	public static bool StarmapAvailable()
	{
		return Components.Telescopes.Count > 0 || DebugHandler.InstantBuildMode || Game.Instance.SandboxModeActive;
	}

	// Token: 0x06009245 RID: 37445 RVA: 0x001041DF File Offset: 0x001023DF
	public void CloseAll()
	{
		if (this.activeScreen == null)
		{
			return;
		}
		if (this.activeScreen.toggleInfo != null)
		{
			this.ToggleScreen(this.activeScreen);
		}
		this.CloseActive();
		this.ClearSelection();
	}

	// Token: 0x06009246 RID: 37446 RVA: 0x0010420F File Offset: 0x0010240F
	private void OnUIClear(object data)
	{
		this.CloseAll();
	}

	// Token: 0x06009247 RID: 37447 RVA: 0x00392118 File Offset: 0x00390318
	public void ToggleScreen(ManagementMenu.ScreenData screenData)
	{
		if (screenData == null)
		{
			return;
		}
		if (screenData.toggleInfo == this.researchInfo && !this.ResearchAvailable())
		{
			this.CheckResearch(null);
			this.CloseActive();
			return;
		}
		if (screenData.toggleInfo == this.skillsInfo && !this.SkillsAvailable())
		{
			this.CheckSkills(null);
			this.CloseActive();
			return;
		}
		if (screenData.toggleInfo == this.starmapInfo && !ManagementMenu.StarmapAvailable())
		{
			this.CheckStarmap(null);
			this.CloseActive();
			return;
		}
		if (screenData.toggleInfo.toggle.gameObject.GetComponentInChildren<ImageToggleState>().IsDisabled)
		{
			return;
		}
		if (this.activeScreen != null)
		{
			this.activeScreen.toggleInfo.toggle.isOn = false;
			this.activeScreen.toggleInfo.toggle.gameObject.GetComponentInChildren<ImageToggleState>().SetInactive();
		}
		if (this.activeScreen != screenData)
		{
			OverlayScreen.Instance.ToggleOverlay(OverlayModes.None.ID, true);
			if (this.activeScreen != null)
			{
				this.activeScreen.toggleInfo.toggle.ActivateFlourish(false);
			}
			KMonoBehaviour.PlaySound(GlobalAssets.GetSound("HUD_Click_Open", false));
			AudioMixer.instance.Start(AudioMixerSnapshots.Get().MenuOpenMigrated);
			screenData.toggleInfo.toggle.ActivateFlourish(true);
			screenData.toggleInfo.toggle.gameObject.GetComponentInChildren<ImageToggleState>().SetActive();
			this.CloseActive();
			this.activeScreen = screenData;
			if (!this.activeScreen.screen.IsActive())
			{
				this.activeScreen.screen.Activate();
			}
			this.activeScreen.screen.Show(true);
			foreach (ManagementMenuNotification managementMenuNotification in this.notificationDisplayer.GetNotificationsForAction(screenData.toggleInfo.hotKey))
			{
				if (managementMenuNotification.customClickCallback != null)
				{
					managementMenuNotification.customClickCallback(managementMenuNotification.customClickData);
					break;
				}
			}
			using (List<KScreen>.Enumerator enumerator2 = this.mutuallyExclusiveScreens.GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					KScreen kscreen = enumerator2.Current;
					kscreen.Show(false);
				}
				return;
			}
		}
		this.activeScreen.screen.Show(false);
		KMonoBehaviour.PlaySound(GlobalAssets.GetSound("HUD_Click_Close", false));
		AudioMixer.instance.Stop(AudioMixerSnapshots.Get().MenuOpenMigrated, STOP_MODE.ALLOWFADEOUT);
		this.activeScreen.toggleInfo.toggle.ActivateFlourish(false);
		this.activeScreen = null;
		screenData.toggleInfo.toggle.gameObject.GetComponentInChildren<ImageToggleState>().SetInactive();
	}

	// Token: 0x06009248 RID: 37448 RVA: 0x00104217 File Offset: 0x00102417
	public void OnButtonClick(KIconToggleMenu.ToggleInfo toggle_info)
	{
		this.ToggleScreen(this.ScreenInfoMatch[(ManagementMenu.ManagementMenuToggleInfo)toggle_info]);
	}

	// Token: 0x06009249 RID: 37449 RVA: 0x00104230 File Offset: 0x00102430
	private void CloseActive()
	{
		if (this.activeScreen != null)
		{
			this.activeScreen.toggleInfo.toggle.isOn = false;
			this.activeScreen.screen.Show(false);
			this.activeScreen = null;
		}
	}

	// Token: 0x0600924A RID: 37450 RVA: 0x003923D0 File Offset: 0x003905D0
	public void ToggleResearch()
	{
		if ((this.ResearchAvailable() || this.activeScreen == this.ScreenInfoMatch[ManagementMenu.Instance.researchInfo]) && this.researchInfo != null)
		{
			this.ToggleScreen(this.ScreenInfoMatch[ManagementMenu.Instance.researchInfo]);
		}
	}

	// Token: 0x0600924B RID: 37451 RVA: 0x00104268 File Offset: 0x00102468
	public void ToggleCodex()
	{
		this.ToggleScreen(this.ScreenInfoMatch[ManagementMenu.Instance.codexInfo]);
	}

	// Token: 0x0600924C RID: 37452 RVA: 0x00392428 File Offset: 0x00390628
	public void OpenCodexToLockId(string lockId, bool focusContent = false)
	{
		string entryForLock = CodexCache.GetEntryForLock(lockId);
		if (entryForLock == null)
		{
			DebugUtil.LogWarningArgs(new object[]
			{
				"Could not open codex to lockId \"" + lockId + "\", couldn't find an entry that contained that lockId"
			});
			return;
		}
		ContentContainer contentContainer = null;
		if (focusContent)
		{
			CodexEntry codexEntry = CodexCache.FindEntry(entryForLock);
			int num = 0;
			while (contentContainer == null && num < codexEntry.contentContainers.Count)
			{
				if (!(codexEntry.contentContainers[num].lockID != lockId))
				{
					contentContainer = codexEntry.contentContainers[num];
				}
				num++;
			}
		}
		this.OpenCodexToEntry(entryForLock, contentContainer);
	}

	// Token: 0x0600924D RID: 37453 RVA: 0x003924B4 File Offset: 0x003906B4
	public void OpenCodexToEntry(string id, ContentContainer targetContainer = null)
	{
		if (!this.codexScreen.gameObject.activeInHierarchy)
		{
			this.ToggleCodex();
		}
		this.codexScreen.ChangeArticle(id, false, default(Vector3), CodexScreen.HistoryDirection.NewArticle);
		this.codexScreen.FocusContainer(targetContainer);
	}

	// Token: 0x0600924E RID: 37454 RVA: 0x003924FC File Offset: 0x003906FC
	public void ToggleSkills()
	{
		if ((this.SkillsAvailable() || this.activeScreen == this.ScreenInfoMatch[ManagementMenu.Instance.skillsInfo]) && this.skillsInfo != null)
		{
			this.ToggleScreen(this.ScreenInfoMatch[ManagementMenu.Instance.skillsInfo]);
		}
	}

	// Token: 0x0600924F RID: 37455 RVA: 0x00104285 File Offset: 0x00102485
	public void ToggleStarmap()
	{
		if (this.starmapInfo != null)
		{
			this.ToggleScreen(this.ScreenInfoMatch[ManagementMenu.Instance.starmapInfo]);
		}
	}

	// Token: 0x06009250 RID: 37456 RVA: 0x001042AA File Offset: 0x001024AA
	public void ToggleClusterMap()
	{
		this.ToggleScreen(this.ScreenInfoMatch[ManagementMenu.Instance.clusterMapInfo]);
	}

	// Token: 0x06009251 RID: 37457 RVA: 0x001042C7 File Offset: 0x001024C7
	public void TogglePriorities()
	{
		this.ToggleScreen(this.ScreenInfoMatch[ManagementMenu.Instance.jobsInfo]);
	}

	// Token: 0x06009252 RID: 37458 RVA: 0x00392554 File Offset: 0x00390754
	public void OpenReports(int day)
	{
		if (this.activeScreen != this.ScreenInfoMatch[ManagementMenu.Instance.reportsInfo])
		{
			this.ToggleScreen(this.ScreenInfoMatch[ManagementMenu.Instance.reportsInfo]);
		}
		ReportScreen.Instance.ShowReport(day);
	}

	// Token: 0x06009253 RID: 37459 RVA: 0x003925A4 File Offset: 0x003907A4
	public void OpenResearch(string zoomToTech = null)
	{
		if (this.activeScreen != this.ScreenInfoMatch[ManagementMenu.Instance.researchInfo])
		{
			this.ToggleScreen(this.ScreenInfoMatch[ManagementMenu.Instance.researchInfo]);
		}
		if (zoomToTech != null)
		{
			UIScheduler.Instance.Schedule("ResearchCameraFocus", 0.25f, delegate(object data)
			{
				this.researchScreen.ZoomToTech(zoomToTech, true);
			}, null, null);
		}
	}

	// Token: 0x06009254 RID: 37460 RVA: 0x001042E4 File Offset: 0x001024E4
	public void OpenStarmap()
	{
		if (this.activeScreen != this.ScreenInfoMatch[ManagementMenu.Instance.starmapInfo])
		{
			this.ToggleScreen(this.ScreenInfoMatch[ManagementMenu.Instance.starmapInfo]);
		}
	}

	// Token: 0x06009255 RID: 37461 RVA: 0x0010431E File Offset: 0x0010251E
	public void OpenClusterMap()
	{
		if (this.activeScreen != this.ScreenInfoMatch[ManagementMenu.Instance.clusterMapInfo])
		{
			this.ToggleScreen(this.ScreenInfoMatch[ManagementMenu.Instance.clusterMapInfo]);
		}
	}

	// Token: 0x06009256 RID: 37462 RVA: 0x00104358 File Offset: 0x00102558
	public void CloseClusterMap()
	{
		if (this.activeScreen == this.ScreenInfoMatch[ManagementMenu.Instance.clusterMapInfo])
		{
			this.ToggleScreen(this.ScreenInfoMatch[ManagementMenu.Instance.clusterMapInfo]);
		}
	}

	// Token: 0x06009257 RID: 37463 RVA: 0x00392628 File Offset: 0x00390828
	public void OpenSkills(MinionIdentity minionIdentity)
	{
		this.skillsScreen.CurrentlySelectedMinion = minionIdentity;
		if (this.activeScreen != this.ScreenInfoMatch[ManagementMenu.Instance.skillsInfo])
		{
			this.ToggleScreen(this.ScreenInfoMatch[ManagementMenu.Instance.skillsInfo]);
		}
	}

	// Token: 0x06009258 RID: 37464 RVA: 0x00104392 File Offset: 0x00102592
	public bool IsScreenOpen(KScreen screen)
	{
		return this.activeScreen != null && this.activeScreen.screen == screen;
	}

	// Token: 0x04006EBD RID: 28349
	private const float UI_WIDTH_COMPRESS_THRESHOLD = 1300f;

	// Token: 0x04006EBE RID: 28350
	[MyCmpReq]
	public ManagementMenuNotificationDisplayer notificationDisplayer;

	// Token: 0x04006EBF RID: 28351
	public static ManagementMenu Instance;

	// Token: 0x04006EC0 RID: 28352
	[Header("Management Menu Specific")]
	[SerializeField]
	private KToggle smallPrefab;

	// Token: 0x04006EC1 RID: 28353
	[SerializeField]
	private KToggle researchButtonPrefab;

	// Token: 0x04006EC2 RID: 28354
	public KToggle PauseMenuButton;

	// Token: 0x04006EC3 RID: 28355
	[Header("Top Right Screen References")]
	public JobsTableScreen jobsScreen;

	// Token: 0x04006EC4 RID: 28356
	public VitalsTableScreen vitalsScreen;

	// Token: 0x04006EC5 RID: 28357
	public ScheduleScreen scheduleScreen;

	// Token: 0x04006EC6 RID: 28358
	public ReportScreen reportsScreen;

	// Token: 0x04006EC7 RID: 28359
	public CodexScreen codexScreen;

	// Token: 0x04006EC8 RID: 28360
	public ConsumablesTableScreen consumablesScreen;

	// Token: 0x04006EC9 RID: 28361
	private StarmapScreen starmapScreen;

	// Token: 0x04006ECA RID: 28362
	private ClusterMapScreen clusterMapScreen;

	// Token: 0x04006ECB RID: 28363
	private SkillsScreen skillsScreen;

	// Token: 0x04006ECC RID: 28364
	private ResearchScreen researchScreen;

	// Token: 0x04006ECD RID: 28365
	[Header("Notification Styles")]
	public ColorStyleSetting noAlertColorStyle;

	// Token: 0x04006ECE RID: 28366
	public List<ColorStyleSetting> alertColorStyle;

	// Token: 0x04006ECF RID: 28367
	public List<TextStyleSetting> alertTextStyle;

	// Token: 0x04006ED0 RID: 28368
	private ManagementMenu.ManagementMenuToggleInfo jobsInfo;

	// Token: 0x04006ED1 RID: 28369
	private ManagementMenu.ManagementMenuToggleInfo consumablesInfo;

	// Token: 0x04006ED2 RID: 28370
	private ManagementMenu.ManagementMenuToggleInfo scheduleInfo;

	// Token: 0x04006ED3 RID: 28371
	private ManagementMenu.ManagementMenuToggleInfo vitalsInfo;

	// Token: 0x04006ED4 RID: 28372
	private ManagementMenu.ManagementMenuToggleInfo reportsInfo;

	// Token: 0x04006ED5 RID: 28373
	private ManagementMenu.ManagementMenuToggleInfo researchInfo;

	// Token: 0x04006ED6 RID: 28374
	private ManagementMenu.ManagementMenuToggleInfo codexInfo;

	// Token: 0x04006ED7 RID: 28375
	private ManagementMenu.ManagementMenuToggleInfo starmapInfo;

	// Token: 0x04006ED8 RID: 28376
	private ManagementMenu.ManagementMenuToggleInfo clusterMapInfo;

	// Token: 0x04006ED9 RID: 28377
	private ManagementMenu.ManagementMenuToggleInfo skillsInfo;

	// Token: 0x04006EDA RID: 28378
	private ManagementMenu.ManagementMenuToggleInfo[] fullscreenUIs;

	// Token: 0x04006EDB RID: 28379
	private Dictionary<ManagementMenu.ManagementMenuToggleInfo, ManagementMenu.ScreenData> ScreenInfoMatch = new Dictionary<ManagementMenu.ManagementMenuToggleInfo, ManagementMenu.ScreenData>();

	// Token: 0x04006EDC RID: 28380
	private ManagementMenu.ScreenData activeScreen;

	// Token: 0x04006EDD RID: 28381
	private KButton activeButton;

	// Token: 0x04006EDE RID: 28382
	private string skillsTooltip;

	// Token: 0x04006EDF RID: 28383
	private string skillsTooltipDisabled;

	// Token: 0x04006EE0 RID: 28384
	private string researchTooltip;

	// Token: 0x04006EE1 RID: 28385
	private string researchTooltipDisabled;

	// Token: 0x04006EE2 RID: 28386
	private string starmapTooltip;

	// Token: 0x04006EE3 RID: 28387
	private string starmapTooltipDisabled;

	// Token: 0x04006EE4 RID: 28388
	private string clusterMapTooltip;

	// Token: 0x04006EE5 RID: 28389
	private string clusterMapTooltipDisabled;

	// Token: 0x04006EE6 RID: 28390
	private List<KScreen> mutuallyExclusiveScreens = new List<KScreen>();

	// Token: 0x02001B3D RID: 6973
	public class ScreenData
	{
		// Token: 0x04006EE7 RID: 28391
		public KScreen screen;

		// Token: 0x04006EE8 RID: 28392
		public ManagementMenu.ManagementMenuToggleInfo toggleInfo;

		// Token: 0x04006EE9 RID: 28393
		public Func<bool> cancelHandler;

		// Token: 0x04006EEA RID: 28394
		public int tabIdx;
	}

	// Token: 0x02001B3E RID: 6974
	public class ManagementMenuToggleInfo : KIconToggleMenu.ToggleInfo
	{
		// Token: 0x0600925C RID: 37468 RVA: 0x001043F6 File Offset: 0x001025F6
		public ManagementMenuToggleInfo(string text, string icon, object user_data = null, global::Action hotkey = global::Action.NumActions, string tooltip = "", string tooltip_header = "") : base(text, icon, user_data, hotkey, tooltip, tooltip_header)
		{
			this.tooltip = GameUtil.ReplaceHotkeyString(this.tooltip, this.hotKey);
		}

		// Token: 0x0600925D RID: 37469 RVA: 0x0039267C File Offset: 0x0039087C
		public void SetNotificationDisplay(bool showAlertImage, bool showGlow, ColorStyleSetting buttonColorStyle, ColorStyleSetting alertColorStyle)
		{
			ImageToggleState component = this.toggle.GetComponent<ImageToggleState>();
			if (component != null)
			{
				if (buttonColorStyle != null)
				{
					component.SetColorStyle(buttonColorStyle);
				}
				else
				{
					component.SetColorStyle(this.originalButtonSetting);
				}
			}
			if (this.alertImage != null)
			{
				this.alertImage.gameObject.SetActive(showAlertImage);
				this.alertImage.SetColorStyle(alertColorStyle);
			}
			if (this.glowImage != null)
			{
				this.glowImage.gameObject.SetActive(showGlow);
				if (buttonColorStyle != null)
				{
					this.glowImage.SetColorStyle(buttonColorStyle);
				}
			}
		}

		// Token: 0x0600925E RID: 37470 RVA: 0x0039271C File Offset: 0x0039091C
		public override void SetToggle(KToggle toggle)
		{
			base.SetToggle(toggle);
			ImageToggleState component = toggle.GetComponent<ImageToggleState>();
			if (component != null)
			{
				this.originalButtonSetting = component.colorStyleSetting;
			}
			HierarchyReferences component2 = toggle.GetComponent<HierarchyReferences>();
			if (component2 != null)
			{
				this.alertImage = component2.GetReference<ImageToggleState>("AlertImage");
				this.glowImage = component2.GetReference<ImageToggleState>("GlowImage");
			}
		}

		// Token: 0x04006EEB RID: 28395
		public ImageToggleState alertImage;

		// Token: 0x04006EEC RID: 28396
		public ImageToggleState glowImage;

		// Token: 0x04006EED RID: 28397
		private ColorStyleSetting originalButtonSetting;
	}
}
