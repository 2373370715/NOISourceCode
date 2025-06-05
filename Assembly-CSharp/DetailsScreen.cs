using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using FMOD.Studio;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02001D01 RID: 7425
public class DetailsScreen : KTabMenu
{
	// Token: 0x06009AFB RID: 39675 RVA: 0x00109497 File Offset: 0x00107697
	public static void DestroyInstance()
	{
		DetailsScreen.Instance = null;
	}

	// Token: 0x17000A35 RID: 2613
	// (get) Token: 0x06009AFC RID: 39676 RVA: 0x0010949F File Offset: 0x0010769F
	// (set) Token: 0x06009AFD RID: 39677 RVA: 0x001094A7 File Offset: 0x001076A7
	public GameObject target { get; private set; }

	// Token: 0x06009AFE RID: 39678 RVA: 0x003CA490 File Offset: 0x003C8690
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.SortScreenOrder();
		base.ConsumeMouseScroll = true;
		global::Debug.Assert(DetailsScreen.Instance == null);
		DetailsScreen.Instance = this;
		this.InitiateSidescreenTabs();
		this.DeactivateSideContent();
		this.Show(false);
		base.Subscribe(Game.Instance.gameObject, -1503271301, new Action<object>(this.OnSelectObject));
		this.tabHeader.Init();
	}

	// Token: 0x06009AFF RID: 39679 RVA: 0x003CA508 File Offset: 0x003C8708
	public bool CanObjectDisplayTabOfType(GameObject obj, DetailsScreen.SidescreenTabTypes type)
	{
		for (int i = 0; i < this.sidescreenTabs.Length; i++)
		{
			DetailsScreen.SidescreenTab sidescreenTab = this.sidescreenTabs[i];
			if (sidescreenTab.type == type)
			{
				return sidescreenTab.ValidateTarget(obj);
			}
		}
		return false;
	}

	// Token: 0x06009B00 RID: 39680 RVA: 0x003CA544 File Offset: 0x003C8744
	public DetailsScreen.SidescreenTab GetTabOfType(DetailsScreen.SidescreenTabTypes type)
	{
		for (int i = 0; i < this.sidescreenTabs.Length; i++)
		{
			DetailsScreen.SidescreenTab sidescreenTab = this.sidescreenTabs[i];
			if (sidescreenTab.type == type)
			{
				return sidescreenTab;
			}
		}
		return null;
	}

	// Token: 0x06009B01 RID: 39681 RVA: 0x003CA57C File Offset: 0x003C877C
	public void InitiateSidescreenTabs()
	{
		for (int i = 0; i < this.sidescreenTabs.Length; i++)
		{
			DetailsScreen.SidescreenTab sidescreenTab = this.sidescreenTabs[i];
			sidescreenTab.Initiate(this.original_tab, this.original_tab_body, delegate(DetailsScreen.SidescreenTab _tab)
			{
				this.SelectSideScreenTab(_tab.type);
			});
			switch (sidescreenTab.type)
			{
			case DetailsScreen.SidescreenTabTypes.Errands:
				sidescreenTab.ValidateTargetCallback = ((GameObject target, DetailsScreen.SidescreenTab _tab) => target.GetComponent<MinionIdentity>() != null);
				break;
			case DetailsScreen.SidescreenTabTypes.Material:
				sidescreenTab.ValidateTargetCallback = delegate(GameObject target, DetailsScreen.SidescreenTab _tab)
				{
					Reconstructable component = target.GetComponent<Reconstructable>();
					return component != null && component.AllowReconstruct;
				};
				break;
			case DetailsScreen.SidescreenTabTypes.Blueprints:
				sidescreenTab.ValidateTargetCallback = delegate(GameObject target, DetailsScreen.SidescreenTab _tab)
				{
					UnityEngine.Object component = target.GetComponent<MinionIdentity>();
					BuildingFacade component2 = target.GetComponent<BuildingFacade>();
					return component != null || component2 != null;
				};
				break;
			}
		}
	}

	// Token: 0x06009B02 RID: 39682 RVA: 0x003CA65C File Offset: 0x003C885C
	private void OnSelectObject(object data)
	{
		if (data == null)
		{
			this.previouslyActiveTab = -1;
			this.SelectSideScreenTab(DetailsScreen.SidescreenTabTypes.Config);
			return;
		}
		KPrefabID component = ((GameObject)data).GetComponent<KPrefabID>();
		if (!(component == null) && !(this.previousTargetID != component.PrefabID()))
		{
			this.SelectSideScreenTab(this.selectedSidescreenTabID);
			return;
		}
		if (component != null && component.GetComponent<MinionIdentity>())
		{
			this.SelectSideScreenTab(DetailsScreen.SidescreenTabTypes.Errands);
			return;
		}
		this.SelectSideScreenTab(DetailsScreen.SidescreenTabTypes.Config);
	}

	// Token: 0x06009B03 RID: 39683 RVA: 0x003CA6D8 File Offset: 0x003C88D8
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.CodexEntryButton.onClick += this.CodexEntryButton_OnClick;
		this.PinResourceButton.onClick += this.PinResourceButton_OnClick;
		this.CloseButton.onClick += this.DeselectAndClose;
		this.TabTitle.OnNameChanged += this.OnNameChanged;
		this.TabTitle.OnStartedEditing += this.OnStartedEditing;
		this.sideScreen2.SetActive(false);
		base.Subscribe<DetailsScreen>(-1514841199, DetailsScreen.OnRefreshDataDelegate);
	}

	// Token: 0x06009B04 RID: 39684 RVA: 0x001094B0 File Offset: 0x001076B0
	private void OnStartedEditing()
	{
		base.isEditing = true;
		KScreenManager.Instance.RefreshStack();
	}

	// Token: 0x06009B05 RID: 39685 RVA: 0x003CA77C File Offset: 0x003C897C
	private void OnNameChanged(string newName)
	{
		base.isEditing = false;
		if (string.IsNullOrEmpty(newName))
		{
			return;
		}
		MinionIdentity component = this.target.GetComponent<MinionIdentity>();
		UserNameable component2 = this.target.GetComponent<UserNameable>();
		ClustercraftExteriorDoor component3 = this.target.GetComponent<ClustercraftExteriorDoor>();
		CommandModule component4 = this.target.GetComponent<CommandModule>();
		if (component != null)
		{
			component.SetName(newName);
		}
		else if (component4 != null)
		{
			SpacecraftManager.instance.GetSpacecraftFromLaunchConditionManager(component4.GetComponent<LaunchConditionManager>()).SetRocketName(newName);
		}
		else if (component3 != null)
		{
			component3.GetTargetWorld().GetComponent<UserNameable>().SetName(newName);
		}
		else if (component2 != null)
		{
			component2.SetName(newName);
		}
		this.TabTitle.UpdateRenameTooltip(this.target);
	}

	// Token: 0x06009B06 RID: 39686 RVA: 0x001094C3 File Offset: 0x001076C3
	protected override void OnDeactivate()
	{
		if (this.target != null && this.setRocketTitleHandle != -1)
		{
			this.target.Unsubscribe(this.setRocketTitleHandle);
		}
		this.setRocketTitleHandle = -1;
		this.DeactivateSideContent();
		base.OnDeactivate();
	}

	// Token: 0x06009B07 RID: 39687 RVA: 0x00109500 File Offset: 0x00107700
	protected override void OnShow(bool show)
	{
		if (!show)
		{
			this.DeactivateSideContent();
		}
		else
		{
			this.MaskSideContent(false);
			AudioMixer.instance.Start(AudioMixerSnapshots.Get().MenuOpenHalfEffect);
		}
		base.OnShow(show);
	}

	// Token: 0x06009B08 RID: 39688 RVA: 0x00109530 File Offset: 0x00107730
	protected override void OnCmpDisable()
	{
		this.DeactivateSideContent();
		base.OnCmpDisable();
	}

	// Token: 0x06009B09 RID: 39689 RVA: 0x0010953E File Offset: 0x0010773E
	public override void OnKeyUp(KButtonEvent e)
	{
		if (!base.isEditing && this.target != null && PlayerController.Instance.ConsumeIfNotDragging(e, global::Action.MouseRight))
		{
			this.DeselectAndClose();
		}
	}

	// Token: 0x06009B0A RID: 39690 RVA: 0x003CA83C File Offset: 0x003C8A3C
	private static Component GetComponent(GameObject go, string name)
	{
		Type type = Type.GetType(name);
		Component component;
		if (type != null)
		{
			component = go.GetComponent(type);
		}
		else
		{
			component = go.GetComponent(name);
		}
		return component;
	}

	// Token: 0x06009B0B RID: 39691 RVA: 0x003CA870 File Offset: 0x003C8A70
	private static bool IsExcludedPrefabTag(GameObject go, Tag[] excluded_tags)
	{
		if (excluded_tags == null || excluded_tags.Length == 0)
		{
			return false;
		}
		bool result = false;
		KPrefabID component = go.GetComponent<KPrefabID>();
		foreach (Tag b in excluded_tags)
		{
			if (component.PrefabTag == b)
			{
				result = true;
				break;
			}
		}
		return result;
	}

	// Token: 0x06009B0C RID: 39692 RVA: 0x003CA8B8 File Offset: 0x003C8AB8
	private string CodexEntryButton_GetCodexId()
	{
		string text = "";
		global::Debug.Assert(this.target != null, "Details Screen has no target");
		KSelectable component = this.target.GetComponent<KSelectable>();
		DebugUtil.AssertArgs(component != null, new object[]
		{
			"Details Screen target is not a KSelectable",
			this.target
		});
		CellSelectionObject component2 = component.GetComponent<CellSelectionObject>();
		BuildingUnderConstruction component3 = component.GetComponent<BuildingUnderConstruction>();
		CreatureBrain component4 = component.GetComponent<CreatureBrain>();
		PlantableSeed component5 = component.GetComponent<PlantableSeed>();
		BudUprootedMonitor component6 = component.GetComponent<BudUprootedMonitor>();
		if (component2 != null)
		{
			text = CodexCache.FormatLinkID(component2.element.id.ToString());
		}
		else if (component3 != null)
		{
			text = CodexCache.FormatLinkID(component3.Def.PrefabID);
		}
		else if (component4 != null)
		{
			text = CodexCache.FormatLinkID(component.PrefabID().ToString());
			text = text.Replace("BABY", "");
		}
		else if (component5 != null)
		{
			text = CodexCache.FormatLinkID(component.PrefabID().ToString());
			text = text.Replace("SEED", "");
		}
		else if (component6 != null)
		{
			if (component6.parentObject.Get() != null)
			{
				text = CodexCache.FormatLinkID(component6.parentObject.Get().PrefabID().ToString());
			}
			else if (component6.GetComponent<TreeBud>() != null)
			{
				text = CodexCache.FormatLinkID(component6.GetComponent<TreeBud>().buddingTrunk.Get().PrefabID().ToString());
			}
		}
		else
		{
			text = UI.ExtractLinkID(component.GetProperName());
			if (string.IsNullOrEmpty(text))
			{
				text = CodexCache.FormatLinkID(component.PrefabID().ToString());
			}
		}
		if (CodexCache.entries.ContainsKey(text) || CodexCache.FindSubEntry(text) != null)
		{
			return text;
		}
		return "";
	}

	// Token: 0x06009B0D RID: 39693 RVA: 0x003CAAC4 File Offset: 0x003C8CC4
	private void CodexEntryButton_Refresh()
	{
		string a = this.CodexEntryButton_GetCodexId();
		this.CodexEntryButton.isInteractable = (a != "");
		this.CodexEntryButton.GetComponent<ToolTip>().SetSimpleTooltip(this.CodexEntryButton.isInteractable ? UI.TOOLTIPS.OPEN_CODEX_ENTRY : UI.TOOLTIPS.NO_CODEX_ENTRY);
	}

	// Token: 0x06009B0E RID: 39694 RVA: 0x003CAB1C File Offset: 0x003C8D1C
	public void CodexEntryButton_OnClick()
	{
		string text = this.CodexEntryButton_GetCodexId();
		if (text != "")
		{
			ManagementMenu.Instance.OpenCodexToEntry(text, null);
		}
	}

	// Token: 0x06009B0F RID: 39695 RVA: 0x003CAB4C File Offset: 0x003C8D4C
	private bool PinResourceButton_TryGetResourceTagAndProperName(out Tag targetTag, out string targetProperName)
	{
		KPrefabID component = this.target.GetComponent<KPrefabID>();
		if (component != null && DetailsScreen.<PinResourceButton_TryGetResourceTagAndProperName>g__ShouldUse|51_0(component.PrefabTag))
		{
			targetTag = component.PrefabTag;
			targetProperName = component.GetProperName();
			return true;
		}
		CellSelectionObject component2 = this.target.GetComponent<CellSelectionObject>();
		if (component2 != null && DetailsScreen.<PinResourceButton_TryGetResourceTagAndProperName>g__ShouldUse|51_0(component2.element.tag))
		{
			targetTag = component2.element.tag;
			targetProperName = component2.GetProperName();
			return true;
		}
		targetTag = null;
		targetProperName = null;
		return false;
	}

	// Token: 0x06009B10 RID: 39696 RVA: 0x003CABE4 File Offset: 0x003C8DE4
	private void PinResourceButton_Refresh()
	{
		Tag tag;
		string arg;
		if (this.PinResourceButton_TryGetResourceTagAndProperName(out tag, out arg))
		{
			ClusterManager.Instance.activeWorld.worldInventory.pinnedResources.Contains(tag);
			GameUtil.MeasureUnit measureUnit;
			if (!AllResourcesScreen.Instance.units.TryGetValue(tag, out measureUnit))
			{
				measureUnit = GameUtil.MeasureUnit.quantity;
			}
			string arg2;
			switch (measureUnit)
			{
			case GameUtil.MeasureUnit.mass:
				arg2 = GameUtil.GetFormattedMass(ClusterManager.Instance.activeWorld.worldInventory.GetAmount(tag, false), GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}");
				break;
			case GameUtil.MeasureUnit.kcal:
				arg2 = GameUtil.GetFormattedCalories(WorldResourceAmountTracker<RationTracker>.Get().CountAmountForItemWithID(tag.Name, ClusterManager.Instance.activeWorld.worldInventory, true), GameUtil.TimeSlice.None, true);
				break;
			case GameUtil.MeasureUnit.quantity:
				arg2 = GameUtil.GetFormattedUnits(ClusterManager.Instance.activeWorld.worldInventory.GetAmount(tag, false), GameUtil.TimeSlice.None, true, "");
				break;
			default:
				arg2 = "";
				break;
			}
			this.PinResourceButton.gameObject.SetActive(true);
			this.PinResourceButton.GetComponent<ToolTip>().SetSimpleTooltip(string.Format(UI.TOOLTIPS.OPEN_RESOURCE_INFO, arg2, arg));
			return;
		}
		this.PinResourceButton.gameObject.SetActive(false);
	}

	// Token: 0x06009B11 RID: 39697 RVA: 0x003CAD08 File Offset: 0x003C8F08
	public void PinResourceButton_OnClick()
	{
		Tag tag;
		string text;
		if (this.PinResourceButton_TryGetResourceTagAndProperName(out tag, out text))
		{
			AllResourcesScreen.Instance.SetFilter(UI.StripLinkFormatting(text));
			AllResourcesScreen.Instance.Show(true);
		}
	}

	// Token: 0x06009B12 RID: 39698 RVA: 0x003CAD3C File Offset: 0x003C8F3C
	public void OnRefreshData(object obj)
	{
		this.RefreshTitle();
		for (int i = 0; i < this.tabs.Count; i++)
		{
			if (this.tabs[i].gameObject.activeInHierarchy)
			{
				this.tabs[i].Trigger(-1514841199, obj);
			}
		}
	}

	// Token: 0x06009B13 RID: 39699 RVA: 0x003CAD94 File Offset: 0x003C8F94
	public void Refresh(GameObject go)
	{
		if (this.screens == null)
		{
			return;
		}
		if (this.target != go)
		{
			if (this.setRocketTitleHandle != -1)
			{
				this.target.Unsubscribe(this.setRocketTitleHandle);
				this.setRocketTitleHandle = -1;
			}
			if (this.target != null)
			{
				if (this.target.GetComponent<KPrefabID>() != null)
				{
					this.previousTargetID = this.target.GetComponent<KPrefabID>().PrefabID();
				}
				else
				{
					this.previousTargetID = null;
				}
			}
		}
		this.target = go;
		this.sortedSideScreens.Clear();
		CellSelectionObject component = this.target.GetComponent<CellSelectionObject>();
		if (component)
		{
			component.OnObjectSelected(null);
		}
		this.UpdateTitle();
		this.tabHeader.RefreshTabDisplayForTarget(this.target);
		if (this.sideScreens != null && this.sideScreens.Count > 0)
		{
			bool flag = false;
			foreach (DetailsScreen.SideScreenRef sideScreenRef in this.sideScreens)
			{
				if (!sideScreenRef.screenPrefab.IsValidForTarget(this.target))
				{
					if (sideScreenRef.screenInstance != null && sideScreenRef.screenInstance.gameObject.activeSelf)
					{
						sideScreenRef.screenInstance.gameObject.SetActive(false);
					}
				}
				else
				{
					flag = true;
					if (sideScreenRef.screenInstance == null)
					{
						DetailsScreen.SidescreenTab tabOfType = this.GetTabOfType(sideScreenRef.tab);
						sideScreenRef.screenInstance = global::Util.KInstantiateUI<SideScreenContent>(sideScreenRef.screenPrefab.gameObject, tabOfType.bodyInstance, false);
					}
					if (!this.sideScreen.activeSelf)
					{
						this.sideScreen.SetActive(true);
					}
					sideScreenRef.screenInstance.SetTarget(this.target);
					sideScreenRef.screenInstance.Show(true);
					int sideScreenSortOrder = sideScreenRef.screenInstance.GetSideScreenSortOrder();
					this.sortedSideScreens.Add(new KeyValuePair<DetailsScreen.SideScreenRef, int>(sideScreenRef, sideScreenSortOrder));
				}
			}
			if (!flag)
			{
				if (!this.CanObjectDisplayTabOfType(this.target, DetailsScreen.SidescreenTabTypes.Material) && !this.CanObjectDisplayTabOfType(this.target, DetailsScreen.SidescreenTabTypes.Blueprints))
				{
					this.sideScreen.SetActive(false);
				}
				else
				{
					this.sideScreen.SetActive(true);
				}
			}
		}
		this.sortedSideScreens.Sort(delegate(KeyValuePair<DetailsScreen.SideScreenRef, int> x, KeyValuePair<DetailsScreen.SideScreenRef, int> y)
		{
			if (x.Value <= y.Value)
			{
				return 1;
			}
			return -1;
		});
		for (int i = 0; i < this.sortedSideScreens.Count; i++)
		{
			this.sortedSideScreens[i].Key.screenInstance.transform.SetSiblingIndex(i);
		}
		for (int j = 0; j < this.sidescreenTabs.Length; j++)
		{
			DetailsScreen.SidescreenTab tab = this.sidescreenTabs[j];
			tab.RepositionTitle();
			KeyValuePair<DetailsScreen.SideScreenRef, int> keyValuePair = this.sortedSideScreens.Find((KeyValuePair<DetailsScreen.SideScreenRef, int> t) => t.Key.tab == tab.type);
			tab.SetNoConfigMessageVisibility(keyValuePair.Key == null);
		}
		this.RefreshTitle();
	}

	// Token: 0x06009B14 RID: 39700 RVA: 0x003CB0C0 File Offset: 0x003C92C0
	public void RefreshTitle()
	{
		for (int i = 0; i < this.sidescreenTabs.Length; i++)
		{
			DetailsScreen.SidescreenTab tab = this.sidescreenTabs[i];
			if (tab.IsVisible)
			{
				KeyValuePair<DetailsScreen.SideScreenRef, int> keyValuePair = this.sortedSideScreens.Find((KeyValuePair<DetailsScreen.SideScreenRef, int> match) => match.Key.tab == tab.type);
				if (keyValuePair.Key != null)
				{
					tab.SetTitleVisibility(true);
					tab.SetTitle(keyValuePair.Key.screenInstance.GetTitle());
				}
				else
				{
					tab.SetTitle(UI.UISIDESCREENS.NOCONFIG.TITLE);
					tab.SetTitleVisibility(tab.type == DetailsScreen.SidescreenTabTypes.Config || tab.type == DetailsScreen.SidescreenTabTypes.Errands);
				}
			}
		}
	}

	// Token: 0x06009B15 RID: 39701 RVA: 0x0010956A File Offset: 0x0010776A
	private void SelectSideScreenTab(DetailsScreen.SidescreenTabTypes tabID)
	{
		this.selectedSidescreenTabID = tabID;
		this.RefreshSideScreenTabs();
	}

	// Token: 0x06009B16 RID: 39702 RVA: 0x003CB194 File Offset: 0x003C9394
	private void RefreshSideScreenTabs()
	{
		int num = 1;
		for (int i = 0; i < this.sidescreenTabs.Length; i++)
		{
			DetailsScreen.SidescreenTab sidescreenTab = this.sidescreenTabs[i];
			bool flag = sidescreenTab.ValidateTarget(this.target);
			sidescreenTab.SetVisible(flag);
			sidescreenTab.SetSelected(this.selectedSidescreenTabID == sidescreenTab.type);
			num += (flag ? 1 : 0);
		}
		this.RefreshTitle();
		DetailsScreen.SidescreenTabTypes sidescreenTabTypes = this.selectedSidescreenTabID;
		if (sidescreenTabTypes != DetailsScreen.SidescreenTabTypes.Material)
		{
			if (sidescreenTabTypes == DetailsScreen.SidescreenTabTypes.Blueprints)
			{
				CosmeticsPanel reference = this.GetTabOfType(DetailsScreen.SidescreenTabTypes.Blueprints).bodyInstance.GetComponent<HierarchyReferences>().GetReference<CosmeticsPanel>("CosmeticsPanel");
				reference.SetTarget(this.target);
				reference.Refresh();
			}
		}
		else
		{
			this.GetTabOfType(DetailsScreen.SidescreenTabTypes.Material).bodyInstance.GetComponentInChildren<DetailsScreenMaterialPanel>().SetTarget(this.target);
		}
		this.sidescreenTabHeader.SetActive(num > 1);
	}

	// Token: 0x06009B17 RID: 39703 RVA: 0x003CB264 File Offset: 0x003C9464
	public KScreen SetSecondarySideScreen(KScreen secondaryPrefab, string title)
	{
		this.ClearSecondarySideScreen();
		if (this.instantiatedSecondarySideScreens.ContainsKey(secondaryPrefab))
		{
			this.activeSideScreen2 = this.instantiatedSecondarySideScreens[secondaryPrefab];
			this.activeSideScreen2.gameObject.SetActive(true);
		}
		else
		{
			this.activeSideScreen2 = KScreenManager.Instance.InstantiateScreen(secondaryPrefab.gameObject, this.sideScreen2ContentBody);
			this.activeSideScreen2.Activate();
			this.instantiatedSecondarySideScreens.Add(secondaryPrefab, this.activeSideScreen2);
		}
		this.sideScreen2Title.text = title;
		this.sideScreen2.SetActive(true);
		return this.activeSideScreen2;
	}

	// Token: 0x06009B18 RID: 39704 RVA: 0x00109579 File Offset: 0x00107779
	public void ClearSecondarySideScreen()
	{
		if (this.activeSideScreen2 != null)
		{
			this.activeSideScreen2.gameObject.SetActive(false);
			this.activeSideScreen2 = null;
		}
		this.sideScreen2.SetActive(false);
	}

	// Token: 0x06009B19 RID: 39705 RVA: 0x003CB304 File Offset: 0x003C9504
	public void DeactivateSideContent()
	{
		if (SideDetailsScreen.Instance != null && SideDetailsScreen.Instance.gameObject.activeInHierarchy)
		{
			SideDetailsScreen.Instance.Show(false);
		}
		if (this.sideScreens != null && this.sideScreens.Count > 0)
		{
			this.sideScreens.ForEach(delegate(DetailsScreen.SideScreenRef scn)
			{
				if (scn.screenInstance != null)
				{
					scn.screenInstance.ClearTarget();
					scn.screenInstance.Show(false);
				}
			});
		}
		DetailsScreen.SidescreenTab tabOfType = this.GetTabOfType(DetailsScreen.SidescreenTabTypes.Material);
		DetailsScreen.SidescreenTab tabOfType2 = this.GetTabOfType(DetailsScreen.SidescreenTabTypes.Blueprints);
		tabOfType.bodyInstance.GetComponentInChildren<DetailsScreenMaterialPanel>().SetTarget(null);
		tabOfType2.bodyInstance.GetComponentInChildren<CosmeticsPanel>().SetTarget(null);
		AudioMixer.instance.Stop(AudioMixerSnapshots.Get().MenuOpenHalfEffect, STOP_MODE.ALLOWFADEOUT);
		this.sideScreen.SetActive(false);
	}

	// Token: 0x06009B1A RID: 39706 RVA: 0x001095AD File Offset: 0x001077AD
	public void MaskSideContent(bool hide)
	{
		if (hide)
		{
			this.sideScreen.transform.localScale = Vector3.zero;
			return;
		}
		this.sideScreen.transform.localScale = Vector3.one;
	}

	// Token: 0x06009B1B RID: 39707 RVA: 0x003CB3CC File Offset: 0x003C95CC
	public void DeselectAndClose()
	{
		if (base.gameObject.activeInHierarchy)
		{
			KMonoBehaviour.PlaySound(GlobalAssets.GetSound("Back", false));
		}
		if (this.GetActiveTab() != null)
		{
			this.GetActiveTab().SetTarget(null);
		}
		SelectTool.Instance.Select(null, false);
		ClusterMapSelectTool.Instance.Select(null, false);
		if (this.target == null)
		{
			return;
		}
		this.target = null;
		this.previousTargetID = null;
		this.DeactivateSideContent();
		this.Show(false);
	}

	// Token: 0x06009B1C RID: 39708 RVA: 0x001095DD File Offset: 0x001077DD
	private void SortScreenOrder()
	{
		Array.Sort<DetailsScreen.Screens>(this.screens, (DetailsScreen.Screens x, DetailsScreen.Screens y) => x.displayOrderPriority.CompareTo(y.displayOrderPriority));
	}

	// Token: 0x06009B1D RID: 39709 RVA: 0x003CB458 File Offset: 0x003C9658
	public void UpdatePortrait(GameObject target)
	{
		KSelectable component = target.GetComponent<KSelectable>();
		if (component == null)
		{
			return;
		}
		this.TabTitle.portrait.ClearPortrait();
		Building component2 = component.GetComponent<Building>();
		if (component2)
		{
			Sprite uisprite = component2.Def.GetUISprite("ui", false);
			if (uisprite != null)
			{
				this.TabTitle.portrait.SetPortrait(uisprite);
				return;
			}
		}
		if (target.GetComponent<MinionIdentity>())
		{
			this.TabTitle.SetPortrait(component.gameObject);
			return;
		}
		Edible component3 = target.GetComponent<Edible>();
		if (component3 != null)
		{
			Sprite uispriteFromMultiObjectAnim = Def.GetUISpriteFromMultiObjectAnim(component3.GetComponent<KBatchedAnimController>().AnimFiles[0], "ui", false, "");
			this.TabTitle.portrait.SetPortrait(uispriteFromMultiObjectAnim);
			return;
		}
		PrimaryElement component4 = target.GetComponent<PrimaryElement>();
		if (component4 != null)
		{
			this.TabTitle.portrait.SetPortrait(Def.GetUISpriteFromMultiObjectAnim(ElementLoader.FindElementByHash(component4.ElementID).substance.anim, "ui", false, ""));
			return;
		}
		CellSelectionObject component5 = target.GetComponent<CellSelectionObject>();
		if (component5 != null)
		{
			string animName = component5.element.IsSolid ? "ui" : component5.element.substance.name;
			Sprite uispriteFromMultiObjectAnim2 = Def.GetUISpriteFromMultiObjectAnim(component5.element.substance.anim, animName, false, "");
			this.TabTitle.portrait.SetPortrait(uispriteFromMultiObjectAnim2);
			return;
		}
	}

	// Token: 0x06009B1E RID: 39710 RVA: 0x00109609 File Offset: 0x00107809
	public bool CompareTargetWith(GameObject compare)
	{
		return this.target == compare;
	}

	// Token: 0x06009B1F RID: 39711 RVA: 0x003CB5DC File Offset: 0x003C97DC
	public void UpdateTitle()
	{
		this.CodexEntryButton_Refresh();
		this.PinResourceButton_Refresh();
		this.TabTitle.SetTitle(this.target.GetProperName());
		if (this.TabTitle != null)
		{
			this.TabTitle.SetTitle(this.target.GetProperName());
			MinionIdentity minionIdentity = null;
			UserNameable x = null;
			ClustercraftExteriorDoor clustercraftExteriorDoor = null;
			CommandModule commandModule = null;
			if (this.target != null)
			{
				minionIdentity = this.target.gameObject.GetComponent<MinionIdentity>();
				x = this.target.gameObject.GetComponent<UserNameable>();
				clustercraftExteriorDoor = this.target.gameObject.GetComponent<ClustercraftExteriorDoor>();
				commandModule = this.target.gameObject.GetComponent<CommandModule>();
			}
			if (minionIdentity != null)
			{
				this.TabTitle.SetSubText(minionIdentity.GetComponent<MinionResume>().GetSkillsSubtitle(), "");
				this.TabTitle.SetUserEditable(true);
			}
			else if (x != null)
			{
				this.TabTitle.SetSubText("", "");
				this.TabTitle.SetUserEditable(true);
			}
			else if (commandModule != null)
			{
				this.TrySetRocketTitle(commandModule);
			}
			else if (clustercraftExteriorDoor != null)
			{
				this.TrySetRocketTitle(clustercraftExteriorDoor);
			}
			else
			{
				this.TabTitle.SetSubText("", "");
				this.TabTitle.SetUserEditable(false);
			}
			this.TabTitle.UpdateRenameTooltip(this.target);
		}
	}

	// Token: 0x06009B20 RID: 39712 RVA: 0x003CB740 File Offset: 0x003C9940
	private void TrySetRocketTitle(ClustercraftExteriorDoor clusterCraftDoor)
	{
		if (clusterCraftDoor2.HasTargetWorld())
		{
			WorldContainer targetWorld = clusterCraftDoor2.GetTargetWorld();
			this.TabTitle.SetTitle(targetWorld.GetComponent<ClusterGridEntity>().Name);
			this.TabTitle.SetUserEditable(true);
			this.TabTitle.SetSubText(this.target.GetProperName(), "");
			this.setRocketTitleHandle = -1;
			return;
		}
		if (this.setRocketTitleHandle == -1)
		{
			this.setRocketTitleHandle = this.target.Subscribe(-71801987, delegate(object clusterCraftDoor)
			{
				this.OnRefreshData(null);
				this.target.Unsubscribe(this.setRocketTitleHandle);
				this.setRocketTitleHandle = -1;
			});
		}
	}

	// Token: 0x06009B21 RID: 39713 RVA: 0x003CB7CC File Offset: 0x003C99CC
	private void TrySetRocketTitle(CommandModule commandModule)
	{
		if (commandModule != null)
		{
			this.TabTitle.SetTitle(SpacecraftManager.instance.GetSpacecraftFromLaunchConditionManager(commandModule.GetComponent<LaunchConditionManager>()).GetRocketName());
			this.TabTitle.SetUserEditable(true);
		}
		this.TabTitle.SetSubText(this.target.GetProperName(), "");
	}

	// Token: 0x06009B22 RID: 39714 RVA: 0x00109617 File Offset: 0x00107817
	public TargetPanel GetActiveTab()
	{
		return this.tabHeader.ActivePanel;
	}

	// Token: 0x06009B26 RID: 39718 RVA: 0x003CB82C File Offset: 0x003C9A2C
	[CompilerGenerated]
	internal static bool <PinResourceButton_TryGetResourceTagAndProperName>g__ShouldUse|51_0(Tag targetTag)
	{
		foreach (Tag tag in GameTags.MaterialCategories)
		{
			if (DiscoveredResources.Instance.GetDiscoveredResourcesFromTag(tag).Contains(targetTag))
			{
				return true;
			}
		}
		foreach (Tag tag2 in GameTags.CalorieCategories)
		{
			if (DiscoveredResources.Instance.GetDiscoveredResourcesFromTag(tag2).Contains(targetTag))
			{
				return true;
			}
		}
		foreach (Tag tag3 in GameTags.UnitCategories)
		{
			if (DiscoveredResources.Instance.GetDiscoveredResourcesFromTag(tag3).Contains(targetTag))
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x04007913 RID: 30995
	public static DetailsScreen Instance;

	// Token: 0x04007914 RID: 30996
	[SerializeField]
	private KButton CodexEntryButton;

	// Token: 0x04007915 RID: 30997
	[SerializeField]
	private KButton PinResourceButton;

	// Token: 0x04007916 RID: 30998
	[Header("Panels")]
	public Transform UserMenuPanel;

	// Token: 0x04007917 RID: 30999
	[Header("Name Editing (disabled)")]
	[SerializeField]
	private KButton CloseButton;

	// Token: 0x04007918 RID: 31000
	[Header("Tabs")]
	[SerializeField]
	private DetailTabHeader tabHeader;

	// Token: 0x04007919 RID: 31001
	[SerializeField]
	private EditableTitleBar TabTitle;

	// Token: 0x0400791A RID: 31002
	[SerializeField]
	private DetailsScreen.Screens[] screens;

	// Token: 0x0400791B RID: 31003
	[SerializeField]
	private GameObject tabHeaderContainer;

	// Token: 0x0400791C RID: 31004
	[Header("Side Screen Tabs")]
	[SerializeField]
	private DetailsScreen.SidescreenTab[] sidescreenTabs;

	// Token: 0x0400791D RID: 31005
	[SerializeField]
	private GameObject sidescreenTabHeader;

	// Token: 0x0400791E RID: 31006
	[SerializeField]
	private GameObject original_tab;

	// Token: 0x0400791F RID: 31007
	[SerializeField]
	private GameObject original_tab_body;

	// Token: 0x04007920 RID: 31008
	[Header("Side Screens")]
	[SerializeField]
	private GameObject sideScreen;

	// Token: 0x04007921 RID: 31009
	[SerializeField]
	private List<DetailsScreen.SideScreenRef> sideScreens;

	// Token: 0x04007922 RID: 31010
	[SerializeField]
	private LayoutElement tabBodyLayoutElement;

	// Token: 0x04007923 RID: 31011
	[Header("Secondary Side Screens")]
	[SerializeField]
	private GameObject sideScreen2ContentBody;

	// Token: 0x04007924 RID: 31012
	[SerializeField]
	private GameObject sideScreen2;

	// Token: 0x04007925 RID: 31013
	[SerializeField]
	private LocText sideScreen2Title;

	// Token: 0x04007926 RID: 31014
	private KScreen activeSideScreen2;

	// Token: 0x04007928 RID: 31016
	private Tag previousTargetID = null;

	// Token: 0x04007929 RID: 31017
	private bool HasActivated;

	// Token: 0x0400792A RID: 31018
	private DetailsScreen.SidescreenTabTypes selectedSidescreenTabID;

	// Token: 0x0400792B RID: 31019
	private Dictionary<KScreen, KScreen> instantiatedSecondarySideScreens = new Dictionary<KScreen, KScreen>();

	// Token: 0x0400792C RID: 31020
	private static readonly EventSystem.IntraObjectHandler<DetailsScreen> OnRefreshDataDelegate = new EventSystem.IntraObjectHandler<DetailsScreen>(delegate(DetailsScreen component, object data)
	{
		component.OnRefreshData(data);
	});

	// Token: 0x0400792D RID: 31021
	private List<KeyValuePair<DetailsScreen.SideScreenRef, int>> sortedSideScreens = new List<KeyValuePair<DetailsScreen.SideScreenRef, int>>();

	// Token: 0x0400792E RID: 31022
	private int setRocketTitleHandle = -1;

	// Token: 0x02001D02 RID: 7426
	[Serializable]
	private struct Screens
	{
		// Token: 0x0400792F RID: 31023
		public string name;

		// Token: 0x04007930 RID: 31024
		public string displayName;

		// Token: 0x04007931 RID: 31025
		public string tooltip;

		// Token: 0x04007932 RID: 31026
		public Sprite icon;

		// Token: 0x04007933 RID: 31027
		public TargetPanel screen;

		// Token: 0x04007934 RID: 31028
		public int displayOrderPriority;

		// Token: 0x04007935 RID: 31029
		public bool hideWhenDead;

		// Token: 0x04007936 RID: 31030
		public HashedString focusInViewMode;

		// Token: 0x04007937 RID: 31031
		[HideInInspector]
		public int tabIdx;
	}

	// Token: 0x02001D03 RID: 7427
	public enum SidescreenTabTypes
	{
		// Token: 0x04007939 RID: 31033
		Config,
		// Token: 0x0400793A RID: 31034
		Errands,
		// Token: 0x0400793B RID: 31035
		Material,
		// Token: 0x0400793C RID: 31036
		Blueprints
	}

	// Token: 0x02001D04 RID: 7428
	[Serializable]
	public class SidescreenTab
	{
		// Token: 0x06009B28 RID: 39720 RVA: 0x001096A0 File Offset: 0x001078A0
		private void OnTabClicked()
		{
			System.Action onClicked = this.OnClicked;
			if (onClicked == null)
			{
				return;
			}
			onClicked();
		}

		// Token: 0x17000A36 RID: 2614
		// (get) Token: 0x06009B2A RID: 39722 RVA: 0x001096BB File Offset: 0x001078BB
		// (set) Token: 0x06009B29 RID: 39721 RVA: 0x001096B2 File Offset: 0x001078B2
		public bool IsVisible { get; private set; }

		// Token: 0x17000A37 RID: 2615
		// (get) Token: 0x06009B2C RID: 39724 RVA: 0x001096CC File Offset: 0x001078CC
		// (set) Token: 0x06009B2B RID: 39723 RVA: 0x001096C3 File Offset: 0x001078C3
		public bool IsSelected { get; private set; }

		// Token: 0x06009B2D RID: 39725 RVA: 0x003CB928 File Offset: 0x003C9B28
		public void Initiate(GameObject originalTabInstance, GameObject originalBodyInstance, Action<DetailsScreen.SidescreenTab> on_tab_clicked_callback)
		{
			if (on_tab_clicked_callback != null)
			{
				this.OnClicked = delegate()
				{
					on_tab_clicked_callback(this);
				};
			}
			originalBodyInstance.gameObject.SetActive(false);
			if (this.OverrideBody == null)
			{
				this.bodyInstance = UnityEngine.Object.Instantiate<GameObject>(originalBodyInstance);
				this.bodyInstance.name = this.type.ToString() + " Tab - body instance";
				this.bodyInstance.SetActive(true);
				this.bodyInstance.transform.SetParent(originalBodyInstance.transform.parent, false);
			}
			else
			{
				this.bodyInstance = this.OverrideBody;
			}
			this.bodyReferences = this.bodyInstance.GetComponent<HierarchyReferences>();
			originalTabInstance.gameObject.SetActive(false);
			if (this.tabInstance == null)
			{
				this.tabInstance = UnityEngine.Object.Instantiate<GameObject>(originalTabInstance.gameObject).GetComponent<MultiToggle>();
				this.tabInstance.name = this.type.ToString() + " Tab Instance";
				this.tabInstance.gameObject.SetActive(true);
				this.tabInstance.transform.SetParent(originalTabInstance.transform.parent, false);
				MultiToggle multiToggle = this.tabInstance;
				multiToggle.onClick = (System.Action)Delegate.Combine(multiToggle.onClick, new System.Action(this.OnTabClicked));
				HierarchyReferences component = this.tabInstance.GetComponent<HierarchyReferences>();
				component.GetReference<LocText>("label").SetText(Strings.Get(this.Title_Key));
				component.GetReference<Image>("icon").sprite = this.Icon;
				this.tabInstance.GetComponent<ToolTip>().SetSimpleTooltip(Strings.Get(this.Tooltip_Key));
			}
		}

		// Token: 0x06009B2E RID: 39726 RVA: 0x001096D4 File Offset: 0x001078D4
		public void SetSelected(bool isSelected)
		{
			this.IsSelected = isSelected;
			this.tabInstance.ChangeState(isSelected ? 1 : 0);
			this.bodyInstance.SetActive(isSelected);
		}

		// Token: 0x06009B2F RID: 39727 RVA: 0x001096FB File Offset: 0x001078FB
		public void SetTitle(string title)
		{
			if (this.bodyReferences != null && this.bodyReferences.HasReference("TitleLabel"))
			{
				this.bodyReferences.GetReference<LocText>("TitleLabel").SetText(title);
			}
		}

		// Token: 0x06009B30 RID: 39728 RVA: 0x00109733 File Offset: 0x00107933
		public void SetTitleVisibility(bool visible)
		{
			if (this.bodyReferences != null && this.bodyReferences.HasReference("Title"))
			{
				this.bodyReferences.GetReference("Title").gameObject.SetActive(visible);
			}
		}

		// Token: 0x06009B31 RID: 39729 RVA: 0x00109770 File Offset: 0x00107970
		public void SetNoConfigMessageVisibility(bool visible)
		{
			if (this.bodyReferences != null && this.bodyReferences.HasReference("NoConfigMessage"))
			{
				this.bodyReferences.GetReference("NoConfigMessage").gameObject.SetActive(visible);
			}
		}

		// Token: 0x06009B32 RID: 39730 RVA: 0x003CBB04 File Offset: 0x003C9D04
		public void RepositionTitle()
		{
			if (this.bodyReferences != null && this.bodyReferences.GetReference("Title") != null)
			{
				this.bodyReferences.GetReference("Title").transform.SetSiblingIndex(0);
			}
		}

		// Token: 0x06009B33 RID: 39731 RVA: 0x001097AD File Offset: 0x001079AD
		public void SetVisible(bool visible)
		{
			this.IsVisible = visible;
			this.tabInstance.gameObject.SetActive(visible);
			this.bodyInstance.SetActive(this.IsSelected && this.IsVisible);
		}

		// Token: 0x06009B34 RID: 39732 RVA: 0x001097E3 File Offset: 0x001079E3
		public bool ValidateTarget(GameObject target)
		{
			return !(target == null) && (this.ValidateTargetCallback == null || this.ValidateTargetCallback(target, this));
		}

		// Token: 0x0400793D RID: 31037
		public DetailsScreen.SidescreenTabTypes type;

		// Token: 0x0400793E RID: 31038
		public string Title_Key;

		// Token: 0x0400793F RID: 31039
		public string Tooltip_Key;

		// Token: 0x04007940 RID: 31040
		public Sprite Icon;

		// Token: 0x04007941 RID: 31041
		public GameObject OverrideBody;

		// Token: 0x04007942 RID: 31042
		public Func<GameObject, DetailsScreen.SidescreenTab, bool> ValidateTargetCallback;

		// Token: 0x04007943 RID: 31043
		public System.Action OnClicked;

		// Token: 0x04007946 RID: 31046
		[NonSerialized]
		public MultiToggle tabInstance;

		// Token: 0x04007947 RID: 31047
		[NonSerialized]
		public GameObject bodyInstance;

		// Token: 0x04007948 RID: 31048
		private HierarchyReferences bodyReferences;

		// Token: 0x04007949 RID: 31049
		private const string bodyRef_Title = "Title";

		// Token: 0x0400794A RID: 31050
		private const string bodyRef_TitleLabel = "TitleLabel";

		// Token: 0x0400794B RID: 31051
		private const string bodyRef_NoConfigMessage = "NoConfigMessage";
	}

	// Token: 0x02001D06 RID: 7430
	[Serializable]
	public class SideScreenRef
	{
		// Token: 0x0400794E RID: 31054
		public string name;

		// Token: 0x0400794F RID: 31055
		public SideScreenContent screenPrefab;

		// Token: 0x04007950 RID: 31056
		public Vector2 offset;

		// Token: 0x04007951 RID: 31057
		public DetailsScreen.SidescreenTabTypes tab;

		// Token: 0x04007952 RID: 31058
		[HideInInspector]
		public SideScreenContent screenInstance;
	}
}
