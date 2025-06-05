using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using FMOD.Studio;
using STRINGS;
using TUNING;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x02001EFE RID: 7934
public class PlanScreen : KIconToggleMenu
{
	// Token: 0x17000AB0 RID: 2736
	// (get) Token: 0x0600A693 RID: 42643 RVA: 0x001108CC File Offset: 0x0010EACC
	// (set) Token: 0x0600A694 RID: 42644 RVA: 0x001108D3 File Offset: 0x0010EAD3
	public static PlanScreen Instance { get; private set; }

	// Token: 0x0600A695 RID: 42645 RVA: 0x001108DB File Offset: 0x0010EADB
	public static void DestroyInstance()
	{
		PlanScreen.Instance = null;
	}

	// Token: 0x17000AB1 RID: 2737
	// (get) Token: 0x0600A696 RID: 42646 RVA: 0x001108E3 File Offset: 0x0010EAE3
	public static Dictionary<HashedString, string> IconNameMap
	{
		get
		{
			return PlanScreen.iconNameMap;
		}
	}

	// Token: 0x0600A697 RID: 42647 RVA: 0x00106BCE File Offset: 0x00104DCE
	private static HashedString CacheHashedString(string str)
	{
		return HashCache.Get().Add(str);
	}

	// Token: 0x17000AB2 RID: 2738
	// (get) Token: 0x0600A698 RID: 42648 RVA: 0x001108EA File Offset: 0x0010EAEA
	// (set) Token: 0x0600A699 RID: 42649 RVA: 0x001108F2 File Offset: 0x0010EAF2
	public ProductInfoScreen ProductInfoScreen { get; private set; }

	// Token: 0x17000AB3 RID: 2739
	// (get) Token: 0x0600A69A RID: 42650 RVA: 0x001108FB File Offset: 0x0010EAFB
	public KIconToggleMenu.ToggleInfo ActiveCategoryToggleInfo
	{
		get
		{
			return this.activeCategoryInfo;
		}
	}

	// Token: 0x17000AB4 RID: 2740
	// (get) Token: 0x0600A69B RID: 42651 RVA: 0x00110903 File Offset: 0x0010EB03
	// (set) Token: 0x0600A69C RID: 42652 RVA: 0x0011090B File Offset: 0x0010EB0B
	public GameObject SelectedBuildingGameObject { get; private set; }

	// Token: 0x0600A69D RID: 42653 RVA: 0x000D9BC8 File Offset: 0x000D7DC8
	public override float GetSortKey()
	{
		return 2f;
	}

	// Token: 0x0600A69E RID: 42654 RVA: 0x00110914 File Offset: 0x0010EB14
	public PlanScreen.RequirementsState GetBuildableState(BuildingDef def)
	{
		if (def == null)
		{
			return PlanScreen.RequirementsState.Materials;
		}
		return this._buildableStatesByID[def.PrefabID];
	}

	// Token: 0x0600A69F RID: 42655 RVA: 0x003FF5E8 File Offset: 0x003FD7E8
	private bool IsDefResearched(BuildingDef def)
	{
		bool result = false;
		if (!this._researchedDefs.TryGetValue(def, out result))
		{
			result = this.UpdateDefResearched(def);
		}
		return result;
	}

	// Token: 0x0600A6A0 RID: 42656 RVA: 0x003FF610 File Offset: 0x003FD810
	private bool UpdateDefResearched(BuildingDef def)
	{
		return this._researchedDefs[def] = Db.Get().TechItems.IsTechItemComplete(def.PrefabID);
	}

	// Token: 0x0600A6A1 RID: 42657 RVA: 0x003FF644 File Offset: 0x003FD844
	protected override void OnPrefabInit()
	{
		if (BuildMenu.UseHotkeyBuildMenu())
		{
			base.gameObject.SetActive(false);
		}
		else
		{
			base.OnPrefabInit();
			PlanScreen.Instance = this;
			this.ProductInfoScreen = global::Util.KInstantiateUI<ProductInfoScreen>(this.productInfoScreenPrefab, this.recipeInfoScreenParent, false);
			this.ProductInfoScreen.rectTransform().pivot = new Vector2(0f, 0f);
			this.ProductInfoScreen.rectTransform().SetLocalPosition(new Vector3(326f, 0f, 0f));
			this.ProductInfoScreen.onElementsFullySelected = new System.Action(this.OnRecipeElementsFullySelected);
			KInputManager.InputChange.AddListener(new UnityAction(this.RefreshToolTip));
			this.planScreenScrollRect = base.transform.parent.GetComponentInParent<KScrollRect>();
			Game.Instance.Subscribe(-107300940, new Action<object>(this.OnResearchComplete));
			Game.Instance.Subscribe(1174281782, new Action<object>(this.OnActiveToolChanged));
			Game.Instance.Subscribe(1557339983, new Action<object>(this.ForceUpdateAllCategoryToggles));
		}
		this.buildingGroupsRoot.gameObject.SetActive(false);
	}

	// Token: 0x0600A6A2 RID: 42658 RVA: 0x003FF77C File Offset: 0x003FD97C
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.ConsumeMouseScroll = true;
		this.useSubCategoryLayout = (KPlayerPrefs.GetInt("usePlanScreenListView") == 1);
		this.initTime = KTime.Instance.UnscaledGameTime;
		foreach (BuildingDef buildingDef in Assets.BuildingDefs)
		{
			this._buildableStatesByID.Add(buildingDef.PrefabID, PlanScreen.RequirementsState.Materials);
		}
		if (BuildMenu.UseHotkeyBuildMenu())
		{
			base.gameObject.SetActive(false);
		}
		else
		{
			base.onSelect += this.OnClickCategory;
			this.Refresh();
			foreach (KToggle ktoggle in this.toggles)
			{
				ktoggle.group = base.GetComponent<ToggleGroup>();
			}
			this.RefreshBuildableStates(true);
			Game.Instance.Subscribe(288942073, new Action<object>(this.OnUIClear));
		}
		this.copyBuildingButton.GetComponent<MultiToggle>().onClick = delegate()
		{
			this.OnClickCopyBuilding();
		};
		this.RefreshCopyBuildingButton(null);
		Game.Instance.Subscribe(-1503271301, new Action<object>(this.RefreshCopyBuildingButton));
		Game.Instance.Subscribe(1983128072, delegate(object data)
		{
			this.CloseRecipe(false);
		});
		this.pointerEnterActions = (KScreen.PointerEnterActions)Delegate.Combine(this.pointerEnterActions, new KScreen.PointerEnterActions(this.PointerEnter));
		this.pointerExitActions = (KScreen.PointerExitActions)Delegate.Combine(this.pointerExitActions, new KScreen.PointerExitActions(this.PointerExit));
		this.copyBuildingButton.GetComponent<ToolTip>().SetSimpleTooltip(GameUtil.ReplaceHotkeyString(UI.COPY_BUILDING_TOOLTIP, global::Action.CopyBuilding));
		this.RefreshScale(null);
		this.refreshScaleHandle = Game.Instance.Subscribe(-442024484, new Action<object>(this.RefreshScale));
		this.CacheSearchCaches();
		this.BuildButtonList();
		this.gridViewButton.onClick += this.OnClickGridView;
		this.listViewButton.onClick += this.OnClickListView;
	}

	// Token: 0x0600A6A3 RID: 42659 RVA: 0x003FF9C4 File Offset: 0x003FDBC4
	private void RefreshScale(object data = null)
	{
		base.GetComponent<GridLayoutGroup>().cellSize = (ScreenResolutionMonitor.UsingGamepadUIMode() ? new Vector2(54f, 50f) : new Vector2(45f, 45f));
		this.toggles.ForEach(delegate(KToggle to)
		{
			to.GetComponentInChildren<LocText>().fontSize = (float)(ScreenResolutionMonitor.UsingGamepadUIMode() ? PlanScreen.fontSizeBigMode : PlanScreen.fontSizeStandardMode);
		});
		LayoutElement component = this.copyBuildingButton.GetComponent<LayoutElement>();
		component.minWidth = (float)(ScreenResolutionMonitor.UsingGamepadUIMode() ? 58 : 54);
		component.minHeight = (float)(ScreenResolutionMonitor.UsingGamepadUIMode() ? 58 : 54);
		base.gameObject.rectTransform().anchoredPosition = new Vector2(0f, (float)(ScreenResolutionMonitor.UsingGamepadUIMode() ? -68 : -74));
		this.adjacentPinnedButtons.GetComponent<HorizontalLayoutGroup>().padding.bottom = (ScreenResolutionMonitor.UsingGamepadUIMode() ? 14 : 6);
		Vector2 sizeDelta = this.buildingGroupsRoot.rectTransform().sizeDelta;
		Vector2 vector = ScreenResolutionMonitor.UsingGamepadUIMode() ? new Vector2(320f, sizeDelta.y) : new Vector2(264f, sizeDelta.y);
		this.buildingGroupsRoot.rectTransform().sizeDelta = vector;
		foreach (KeyValuePair<string, GameObject> keyValuePair in this.allSubCategoryObjects)
		{
			GridLayoutGroup componentInChildren = keyValuePair.Value.GetComponentInChildren<GridLayoutGroup>(true);
			if (this.useSubCategoryLayout)
			{
				componentInChildren.constraintCount = 1;
				componentInChildren.cellSize = new Vector2(vector.x - 24f, 36f);
			}
			else
			{
				componentInChildren.constraintCount = 3;
				componentInChildren.cellSize = (ScreenResolutionMonitor.UsingGamepadUIMode() ? PlanScreen.bigBuildingButtonSize : PlanScreen.standarduildingButtonSize);
			}
		}
		this.ProductInfoScreen.rectTransform().anchoredPosition = new Vector2(vector.x + 8f, this.ProductInfoScreen.rectTransform().anchoredPosition.y);
	}

	// Token: 0x0600A6A4 RID: 42660 RVA: 0x00110932 File Offset: 0x0010EB32
	protected override void OnForcedCleanUp()
	{
		KInputManager.InputChange.RemoveListener(new UnityAction(this.RefreshToolTip));
		base.OnForcedCleanUp();
	}

	// Token: 0x0600A6A5 RID: 42661 RVA: 0x00110950 File Offset: 0x0010EB50
	protected override void OnCleanUp()
	{
		if (Game.Instance != null)
		{
			Game.Instance.Unsubscribe(this.refreshScaleHandle);
		}
		base.OnCleanUp();
	}

	// Token: 0x0600A6A6 RID: 42662 RVA: 0x003FFBCC File Offset: 0x003FDDCC
	private void OnClickCopyBuilding()
	{
		if (!this.LastSelectedBuilding.IsNullOrDestroyed() && this.LastSelectedBuilding.gameObject.activeInHierarchy && (!this.lastSelectedBuilding.Def.DebugOnly || DebugHandler.InstantBuildMode))
		{
			PlanScreen.Instance.CopyBuildingOrder(this.LastSelectedBuilding);
			return;
		}
		if (this.lastSelectedBuildingDef != null && (!this.lastSelectedBuildingDef.DebugOnly || DebugHandler.InstantBuildMode))
		{
			PlanScreen.Instance.CopyBuildingOrder(this.lastSelectedBuildingDef, this.LastSelectedBuildingFacade);
		}
	}

	// Token: 0x0600A6A7 RID: 42663 RVA: 0x00110975 File Offset: 0x0010EB75
	private void OnClickListView()
	{
		this.useSubCategoryLayout = true;
		this.BuildButtonList();
		this.ConfigurePanelSize(null);
		this.RefreshScale(null);
		KPlayerPrefs.SetInt("usePlanScreenListView", 1);
	}

	// Token: 0x0600A6A8 RID: 42664 RVA: 0x0011099D File Offset: 0x0010EB9D
	private void OnClickGridView()
	{
		this.useSubCategoryLayout = false;
		this.BuildButtonList();
		this.ConfigurePanelSize(null);
		this.RefreshScale(null);
		KPlayerPrefs.SetInt("usePlanScreenListView", 0);
	}

	// Token: 0x17000AB5 RID: 2741
	// (get) Token: 0x0600A6A9 RID: 42665 RVA: 0x001109C5 File Offset: 0x0010EBC5
	// (set) Token: 0x0600A6AA RID: 42666 RVA: 0x003FFC5C File Offset: 0x003FDE5C
	private Building LastSelectedBuilding
	{
		get
		{
			return this.lastSelectedBuilding;
		}
		set
		{
			this.lastSelectedBuilding = value;
			if (this.lastSelectedBuilding != null)
			{
				this.lastSelectedBuildingDef = this.lastSelectedBuilding.Def;
				if (this.lastSelectedBuilding.gameObject.activeInHierarchy)
				{
					this.LastSelectedBuildingFacade = this.lastSelectedBuilding.GetComponent<BuildingFacade>().CurrentFacade;
				}
			}
		}
	}

	// Token: 0x17000AB6 RID: 2742
	// (get) Token: 0x0600A6AB RID: 42667 RVA: 0x001109CD File Offset: 0x0010EBCD
	// (set) Token: 0x0600A6AC RID: 42668 RVA: 0x001109D5 File Offset: 0x0010EBD5
	public string LastSelectedBuildingFacade
	{
		get
		{
			return this.lastSelectedBuildingFacade;
		}
		set
		{
			this.lastSelectedBuildingFacade = value;
		}
	}

	// Token: 0x0600A6AD RID: 42669 RVA: 0x003FFCB8 File Offset: 0x003FDEB8
	public void RefreshCopyBuildingButton(object data = null)
	{
		this.adjacentPinnedButtons.rectTransform().anchoredPosition = new Vector2(Mathf.Min(base.gameObject.rectTransform().sizeDelta.x, base.transform.parent.rectTransform().rect.width), 0f);
		MultiToggle component = this.copyBuildingButton.GetComponent<MultiToggle>();
		if (SelectTool.Instance != null && SelectTool.Instance.selected != null)
		{
			Building component2 = SelectTool.Instance.selected.GetComponent<Building>();
			if (component2 != null && component2.Def.ShouldShowInBuildMenu() && component2.Def.IsAvailable())
			{
				this.LastSelectedBuilding = component2;
			}
		}
		if (this.lastSelectedBuildingDef != null)
		{
			component.gameObject.SetActive(PlanScreen.Instance.gameObject.activeInHierarchy);
			Sprite sprite = this.lastSelectedBuildingDef.GetUISprite("ui", false);
			if (this.LastSelectedBuildingFacade != null && this.LastSelectedBuildingFacade != "DEFAULT_FACADE" && Db.Get().Permits.BuildingFacades.TryGet(this.LastSelectedBuildingFacade) != null)
			{
				sprite = Def.GetFacadeUISprite(this.LastSelectedBuildingFacade);
			}
			component.transform.Find("FG").GetComponent<Image>().sprite = sprite;
			component.transform.Find("FG").GetComponent<Image>().color = Color.white;
			component.ChangeState(1);
			return;
		}
		component.gameObject.SetActive(false);
		component.ChangeState(0);
	}

	// Token: 0x0600A6AE RID: 42670 RVA: 0x003FFE50 File Offset: 0x003FE050
	public void RefreshToolTip()
	{
		for (int i = 0; i < TUNING.BUILDINGS.PLANORDER.Count; i++)
		{
			PlanScreen.PlanInfo planInfo = TUNING.BUILDINGS.PLANORDER[i];
			if (Game.IsCorrectDlcActiveForCurrentSave(planInfo))
			{
				global::Action action = (i < 14) ? (global::Action.Plan1 + i) : global::Action.NumActions;
				string str = HashCache.Get().Get(planInfo.category).ToUpper();
				this.toggleInfo[i].tooltip = GameUtil.ReplaceHotkeyString(Strings.Get("STRINGS.UI.BUILDCATEGORIES." + str + ".TOOLTIP"), action);
			}
		}
		this.copyBuildingButton.GetComponent<ToolTip>().SetSimpleTooltip(GameUtil.ReplaceHotkeyString(UI.COPY_BUILDING_TOOLTIP, global::Action.CopyBuilding));
	}

	// Token: 0x0600A6AF RID: 42671 RVA: 0x003FFF08 File Offset: 0x003FE108
	public void Refresh()
	{
		List<KIconToggleMenu.ToggleInfo> list = new List<KIconToggleMenu.ToggleInfo>();
		if (this.tagCategoryMap == null)
		{
			int num = 0;
			this.tagCategoryMap = new Dictionary<Tag, HashedString>();
			this.tagOrderMap = new Dictionary<Tag, int>();
			if (TUNING.BUILDINGS.PLANORDER.Count > 15)
			{
				DebugUtil.LogWarningArgs(new object[]
				{
					"Insufficient keys to cover root plan menu",
					"Max of 14 keys supported but TUNING.BUILDINGS.PLANORDER has " + TUNING.BUILDINGS.PLANORDER.Count.ToString()
				});
			}
			this.toggleEntries.Clear();
			for (int i = 0; i < TUNING.BUILDINGS.PLANORDER.Count; i++)
			{
				PlanScreen.PlanInfo planInfo = TUNING.BUILDINGS.PLANORDER[i];
				if (Game.IsCorrectDlcActiveForCurrentSave(planInfo))
				{
					global::Action action = (i < 15) ? (global::Action.Plan1 + i) : global::Action.NumActions;
					string icon = PlanScreen.iconNameMap[planInfo.category];
					string str = HashCache.Get().Get(planInfo.category).ToUpper();
					KIconToggleMenu.ToggleInfo toggleInfo = new KIconToggleMenu.ToggleInfo(UI.StripLinkFormatting(Strings.Get("STRINGS.UI.BUILDCATEGORIES." + str + ".NAME")), icon, planInfo.category, action, GameUtil.ReplaceHotkeyString(Strings.Get("STRINGS.UI.BUILDCATEGORIES." + str + ".TOOLTIP"), action), "");
					list.Add(toggleInfo);
					PlanScreen.PopulateOrderInfo(planInfo.category, planInfo.buildingAndSubcategoryData, this.tagCategoryMap, this.tagOrderMap, ref num);
					List<BuildingDef> list2 = new List<BuildingDef>();
					foreach (BuildingDef buildingDef in Assets.BuildingDefs)
					{
						HashedString x;
						if (buildingDef.IsAvailable() && this.tagCategoryMap.TryGetValue(buildingDef.Tag, out x) && !(x != planInfo.category))
						{
							list2.Add(buildingDef);
						}
					}
					this.toggleEntries.Add(new PlanScreen.ToggleEntry(toggleInfo, planInfo.category, list2, planInfo.hideIfNotResearched));
				}
			}
			base.Setup(list);
			this.toggleBouncers.Clear();
			this.toggles.ForEach(delegate(KToggle to)
			{
				foreach (ImageToggleState imageToggleState in to.GetComponents<ImageToggleState>())
				{
					if (imageToggleState.TargetImage.sprite != null && imageToggleState.TargetImage.name == "FG" && !imageToggleState.useSprites)
					{
						imageToggleState.SetSprites(Assets.GetSprite(imageToggleState.TargetImage.sprite.name + "_disabled"), imageToggleState.TargetImage.sprite, imageToggleState.TargetImage.sprite, Assets.GetSprite(imageToggleState.TargetImage.sprite.name + "_disabled"));
					}
				}
				to.GetComponent<KToggle>().soundPlayer.Enabled = false;
				to.GetComponentInChildren<LocText>().fontSize = (float)(ScreenResolutionMonitor.UsingGamepadUIMode() ? PlanScreen.fontSizeBigMode : PlanScreen.fontSizeStandardMode);
				this.toggleBouncers.Add(to, to.GetComponent<Bouncer>());
			});
			for (int j = 0; j < this.toggleEntries.Count; j++)
			{
				PlanScreen.ToggleEntry toggleEntry = this.toggleEntries[j];
				toggleEntry.CollectToggleImages();
				this.toggleEntries[j] = toggleEntry;
			}
			this.ForceUpdateAllCategoryToggles(null);
		}
	}

	// Token: 0x0600A6B0 RID: 42672 RVA: 0x001109DE File Offset: 0x0010EBDE
	private void ForceUpdateAllCategoryToggles(object data = null)
	{
		this.forceUpdateAllCategoryToggles = true;
	}

	// Token: 0x0600A6B1 RID: 42673 RVA: 0x001109E7 File Offset: 0x0010EBE7
	public void ForceRefreshAllBuildingToggles()
	{
		this.forceRefreshAllBuildings = true;
	}

	// Token: 0x0600A6B2 RID: 42674 RVA: 0x00400198 File Offset: 0x003FE398
	public void CopyBuildingOrder(BuildingDef buildingDef, string facadeID)
	{
		foreach (PlanScreen.PlanInfo planInfo in TUNING.BUILDINGS.PLANORDER)
		{
			foreach (KeyValuePair<string, string> keyValuePair in planInfo.buildingAndSubcategoryData)
			{
				if (buildingDef.PrefabID == keyValuePair.Key)
				{
					this.OpenCategoryByName(HashCache.Get().Get(planInfo.category));
					this.OnSelectBuilding(this.activeCategoryBuildingToggles[buildingDef].gameObject, buildingDef, facadeID);
					this.ProductInfoScreen.ToggleExpandedInfo(true);
					break;
				}
			}
		}
	}

	// Token: 0x0600A6B3 RID: 42675 RVA: 0x00400278 File Offset: 0x003FE478
	public void CopyBuildingOrder(Building building)
	{
		this.CopyBuildingOrder(building.Def, building.GetComponent<BuildingFacade>().CurrentFacade);
		if (this.ProductInfoScreen.materialSelectionPanel == null)
		{
			DebugUtil.DevLogError(building.Def.name + " def likely needs to be marked def.ShowInBuildMenu = false");
			return;
		}
		this.ProductInfoScreen.materialSelectionPanel.SelectSourcesMaterials(building);
		Rotatable component = building.GetComponent<Rotatable>();
		if (component != null)
		{
			BuildTool.Instance.SetToolOrientation(component.GetOrientation());
		}
	}

	// Token: 0x0600A6B4 RID: 42676 RVA: 0x004002FC File Offset: 0x003FE4FC
	private static void PopulateOrderInfo(HashedString category, object data, Dictionary<Tag, HashedString> category_map, Dictionary<Tag, int> order_map, ref int building_index)
	{
		if (data.GetType() == typeof(PlanScreen.PlanInfo))
		{
			PlanScreen.PlanInfo planInfo = (PlanScreen.PlanInfo)data;
			PlanScreen.PopulateOrderInfo(planInfo.category, planInfo.buildingAndSubcategoryData, category_map, order_map, ref building_index);
			return;
		}
		foreach (KeyValuePair<string, string> keyValuePair in ((List<KeyValuePair<string, string>>)data))
		{
			Tag key = new Tag(keyValuePair.Key);
			category_map[key] = category;
			order_map[key] = building_index;
			building_index++;
		}
	}

	// Token: 0x0600A6B5 RID: 42677 RVA: 0x001109F0 File Offset: 0x0010EBF0
	protected override void OnCmpEnable()
	{
		this.Refresh();
		this.RefreshCopyBuildingButton(null);
	}

	// Token: 0x0600A6B6 RID: 42678 RVA: 0x001109FF File Offset: 0x0010EBFF
	protected override void OnCmpDisable()
	{
		this.ClearButtons();
	}

	// Token: 0x0600A6B7 RID: 42679 RVA: 0x004003A4 File Offset: 0x003FE5A4
	private void ClearButtons()
	{
		foreach (KeyValuePair<string, GameObject> keyValuePair in this.allSubCategoryObjects)
		{
		}
		foreach (KeyValuePair<string, PlanBuildingToggle> keyValuePair2 in this.allBuildingToggles)
		{
			keyValuePair2.Value.gameObject.SetActive(false);
		}
		this.activeCategoryBuildingToggles.Clear();
		this.copyBuildingButton.gameObject.SetActive(false);
		this.copyBuildingButton.GetComponent<MultiToggle>().ChangeState(0);
	}

	// Token: 0x0600A6B8 RID: 42680 RVA: 0x0040046C File Offset: 0x003FE66C
	public void OnSelectBuilding(GameObject button_go, BuildingDef def, string facadeID = null)
	{
		if (button_go == null)
		{
			global::Debug.Log("Button gameObject is null", base.gameObject);
			return;
		}
		if (button_go == this.SelectedBuildingGameObject)
		{
			this.CloseRecipe(true);
			return;
		}
		this.ignoreToolChangeMessages++;
		PlanBuildingToggle planBuildingToggle = null;
		if (this.currentlySelectedToggle != null)
		{
			planBuildingToggle = this.currentlySelectedToggle.GetComponent<PlanBuildingToggle>();
		}
		this.SelectedBuildingGameObject = button_go;
		this.currentlySelectedToggle = button_go.GetComponent<KToggle>();
		KMonoBehaviour.PlaySound(GlobalAssets.GetSound("HUD_Click", false));
		HashedString category = this.tagCategoryMap[def.Tag];
		PlanScreen.ToggleEntry toggleEntry;
		if (this.GetToggleEntryForCategory(category, out toggleEntry) && toggleEntry.pendingResearchAttentions.Contains(def.Tag))
		{
			toggleEntry.pendingResearchAttentions.Remove(def.Tag);
			button_go.GetComponent<PlanCategoryNotifications>().ToggleAttention(false);
			if (toggleEntry.pendingResearchAttentions.Count == 0)
			{
				toggleEntry.toggleInfo.toggle.GetComponent<PlanCategoryNotifications>().ToggleAttention(false);
			}
		}
		this.ProductInfoScreen.ClearProduct(false);
		if (planBuildingToggle != null)
		{
			planBuildingToggle.Refresh(BuildingGroupScreen.SearchIsEmpty ? null : new bool?(this.buildingDefSearchCaches[def.PrefabID].IsPassingScore()));
		}
		ToolMenu.Instance.ClearSelection();
		PrebuildTool.Instance.Activate(def, this.GetTooltipForBuildable(def));
		this.LastSelectedBuilding = def.BuildingComplete.GetComponent<Building>();
		this.RefreshCopyBuildingButton(null);
		this.ProductInfoScreen.Show(true);
		this.ProductInfoScreen.ConfigureScreen(def, facadeID);
		this.ignoreToolChangeMessages--;
	}

	// Token: 0x0600A6B9 RID: 42681 RVA: 0x0040060C File Offset: 0x003FE80C
	private void RefreshBuildableStates(bool force_update)
	{
		if (Assets.BuildingDefs == null || Assets.BuildingDefs.Count == 0)
		{
			return;
		}
		if (this.timeSinceNotificationPing < this.specialNotificationEmbellishDelay)
		{
			this.timeSinceNotificationPing += Time.unscaledDeltaTime;
		}
		if (this.timeSinceNotificationPing >= this.notificationPingExpire)
		{
			this.notificationPingCount = 0;
		}
		int num = 10;
		if (force_update)
		{
			num = Assets.BuildingDefs.Count;
			this.buildable_state_update_idx = 0;
		}
		ListPool<HashedString, PlanScreen>.PooledList pooledList = ListPool<HashedString, PlanScreen>.Allocate();
		for (int i = 0; i < num; i++)
		{
			this.buildable_state_update_idx = (this.buildable_state_update_idx + 1) % Assets.BuildingDefs.Count;
			BuildingDef buildingDef = Assets.BuildingDefs[this.buildable_state_update_idx];
			PlanScreen.RequirementsState buildableStateForDef = this.GetBuildableStateForDef(buildingDef);
			HashedString hashedString;
			if (this.tagCategoryMap.TryGetValue(buildingDef.Tag, out hashedString) && this._buildableStatesByID[buildingDef.PrefabID] != buildableStateForDef)
			{
				this._buildableStatesByID[buildingDef.PrefabID] = buildableStateForDef;
				if (this.ProductInfoScreen.currentDef == buildingDef)
				{
					this.ignoreToolChangeMessages++;
					this.ProductInfoScreen.ClearProduct(false);
					this.ProductInfoScreen.Show(true);
					this.ProductInfoScreen.ConfigureScreen(buildingDef);
					this.ignoreToolChangeMessages--;
				}
				if (buildableStateForDef == PlanScreen.RequirementsState.Complete)
				{
					foreach (KIconToggleMenu.ToggleInfo toggleInfo in this.toggleInfo)
					{
						if ((HashedString)toggleInfo.userData == hashedString)
						{
							Bouncer bouncer = this.toggleBouncers[toggleInfo.toggle];
							if (bouncer != null && !bouncer.IsBouncing() && !pooledList.Contains(hashedString))
							{
								pooledList.Add(hashedString);
								bouncer.Bounce();
								if (KTime.Instance.UnscaledGameTime - this.initTime > 1.5f)
								{
									if (this.timeSinceNotificationPing >= this.specialNotificationEmbellishDelay)
									{
										string sound = GlobalAssets.GetSound("NewBuildable_Embellishment", false);
										if (sound != null)
										{
											SoundEvent.EndOneShot(SoundEvent.BeginOneShot(sound, SoundListenerController.Instance.transform.GetPosition(), 1f, false));
										}
									}
									string sound2 = GlobalAssets.GetSound("NewBuildable", false);
									if (sound2 != null)
									{
										EventInstance instance = SoundEvent.BeginOneShot(sound2, SoundListenerController.Instance.transform.GetPosition(), 1f, false);
										instance.setParameterByName("playCount", (float)this.notificationPingCount, false);
										SoundEvent.EndOneShot(instance);
									}
								}
								this.timeSinceNotificationPing = 0f;
								this.notificationPingCount++;
							}
						}
					}
				}
			}
		}
		pooledList.Recycle();
	}

	// Token: 0x0600A6BA RID: 42682 RVA: 0x004008DC File Offset: 0x003FEADC
	private PlanScreen.RequirementsState GetBuildableStateForDef(BuildingDef def)
	{
		if (!def.IsAvailable())
		{
			return PlanScreen.RequirementsState.Invalid;
		}
		PlanScreen.RequirementsState result = PlanScreen.RequirementsState.Complete;
		KPrefabID component = def.BuildingComplete.GetComponent<KPrefabID>();
		if (!DebugHandler.InstantBuildMode && !Game.Instance.SandboxModeActive && !this.IsDefResearched(def))
		{
			result = PlanScreen.RequirementsState.Tech;
		}
		else if (component.HasTag(GameTags.Telepad) && ClusterUtil.ActiveWorldHasPrinter())
		{
			result = PlanScreen.RequirementsState.TelepadBuilt;
		}
		else if (component.HasTag(GameTags.RocketInteriorBuilding) && !ClusterUtil.ActiveWorldIsRocketInterior())
		{
			result = PlanScreen.RequirementsState.RocketInteriorOnly;
		}
		else if (component.HasTag(GameTags.NotRocketInteriorBuilding) && ClusterUtil.ActiveWorldIsRocketInterior())
		{
			result = PlanScreen.RequirementsState.RocketInteriorForbidden;
		}
		else if (component.HasTag(GameTags.UniquePerWorld) && BuildingInventory.Instance.BuildingCountForWorld_BAD_PERF(def.Tag, ClusterManager.Instance.activeWorldId) > 0)
		{
			result = PlanScreen.RequirementsState.UniquePerWorld;
		}
		else if (!DebugHandler.InstantBuildMode && !Game.Instance.SandboxModeActive && !ProductInfoScreen.MaterialsMet(def.CraftRecipe))
		{
			result = PlanScreen.RequirementsState.Materials;
		}
		return result;
	}

	// Token: 0x0600A6BB RID: 42683 RVA: 0x004009C0 File Offset: 0x003FEBC0
	private void SetCategoryButtonState()
	{
		this.nextCategoryToUpdateIDX = (this.nextCategoryToUpdateIDX + 1) % this.toggleEntries.Count;
		for (int i = 0; i < this.toggleEntries.Count; i++)
		{
			if (this.forceUpdateAllCategoryToggles || i == this.nextCategoryToUpdateIDX)
			{
				PlanScreen.ToggleEntry toggleEntry = this.toggleEntries[i];
				KIconToggleMenu.ToggleInfo toggleInfo = toggleEntry.toggleInfo;
				toggleInfo.toggle.ActivateFlourish(this.activeCategoryInfo != null && toggleInfo.userData == this.activeCategoryInfo.userData);
				bool flag = false;
				bool flag2 = true;
				if (DebugHandler.InstantBuildMode || Game.Instance.SandboxModeActive)
				{
					flag = true;
					flag2 = false;
				}
				else
				{
					foreach (BuildingDef def in toggleEntry.buildingDefs)
					{
						if (this.GetBuildableState(def) == PlanScreen.RequirementsState.Complete)
						{
							flag = true;
							flag2 = false;
							break;
						}
					}
					if (flag2 && toggleEntry.AreAnyRequiredTechItemsAvailable())
					{
						flag2 = false;
					}
				}
				this.CategoryInteractive[toggleInfo] = !flag2;
				GameObject gameObject = toggleInfo.toggle.fgImage.transform.Find("ResearchIcon").gameObject;
				if (!flag)
				{
					if (flag2 && toggleEntry.hideIfNotResearched)
					{
						toggleInfo.toggle.gameObject.SetActive(false);
					}
					else if (flag2)
					{
						toggleInfo.toggle.gameObject.SetActive(true);
						gameObject.gameObject.SetActive(true);
					}
					else
					{
						toggleInfo.toggle.gameObject.SetActive(true);
						gameObject.gameObject.SetActive(false);
					}
					ImageToggleState.State state = (this.activeCategoryInfo != null && toggleInfo.userData == this.activeCategoryInfo.userData) ? ImageToggleState.State.DisabledActive : ImageToggleState.State.Disabled;
					ImageToggleState[] toggleImages = toggleEntry.toggleImages;
					for (int j = 0; j < toggleImages.Length; j++)
					{
						toggleImages[j].SetState(state);
					}
				}
				else
				{
					toggleInfo.toggle.gameObject.SetActive(true);
					gameObject.gameObject.SetActive(false);
					ImageToggleState.State state2 = (this.activeCategoryInfo == null || toggleInfo.userData != this.activeCategoryInfo.userData) ? ImageToggleState.State.Inactive : ImageToggleState.State.Active;
					ImageToggleState[] toggleImages = toggleEntry.toggleImages;
					for (int j = 0; j < toggleImages.Length; j++)
					{
						toggleImages[j].SetState(state2);
					}
				}
			}
		}
		this.RefreshCopyBuildingButton(null);
		this.forceUpdateAllCategoryToggles = false;
	}

	// Token: 0x0600A6BC RID: 42684 RVA: 0x00400C2C File Offset: 0x003FEE2C
	private void DeactivateBuildTools()
	{
		InterfaceTool activeTool = PlayerController.Instance.ActiveTool;
		if (activeTool != null)
		{
			Type type = activeTool.GetType();
			if (type == typeof(BuildTool) || typeof(BaseUtilityBuildTool).IsAssignableFrom(type) || type == typeof(PrebuildTool))
			{
				activeTool.DeactivateTool(null);
				PlayerController.Instance.ActivateTool(SelectTool.Instance);
			}
		}
	}

	// Token: 0x0600A6BD RID: 42685 RVA: 0x00400CA0 File Offset: 0x003FEEA0
	public void CloseRecipe(bool playSound = false)
	{
		if (playSound)
		{
			KMonoBehaviour.PlaySound(GlobalAssets.GetSound("HUD_Click_Deselect", false));
		}
		if (PlayerController.Instance.ActiveTool is PrebuildTool || PlayerController.Instance.ActiveTool is BuildTool)
		{
			ToolMenu.Instance.ClearSelection();
		}
		this.DeactivateBuildTools();
		if (this.ProductInfoScreen != null)
		{
			this.ProductInfoScreen.ClearProduct(true);
		}
		if (this.activeCategoryInfo != null)
		{
			this.UpdateBuildingButtonList(this.activeCategoryInfo);
		}
		this.SelectedBuildingGameObject = null;
	}

	// Token: 0x0600A6BE RID: 42686 RVA: 0x00400D28 File Offset: 0x003FEF28
	public void SoftCloseRecipe()
	{
		this.ignoreToolChangeMessages++;
		if (PlayerController.Instance.ActiveTool is PrebuildTool || PlayerController.Instance.ActiveTool is BuildTool)
		{
			ToolMenu.Instance.ClearSelection();
		}
		this.DeactivateBuildTools();
		if (this.ProductInfoScreen != null)
		{
			this.ProductInfoScreen.ClearProduct(true);
		}
		this.currentlySelectedToggle = null;
		this.SelectedBuildingGameObject = null;
		this.ignoreToolChangeMessages--;
	}

	// Token: 0x0600A6BF RID: 42687 RVA: 0x00400DAC File Offset: 0x003FEFAC
	public void CloseCategoryPanel(bool playSound = true)
	{
		this.activeCategoryInfo = null;
		if (playSound)
		{
			KMonoBehaviour.PlaySound(GlobalAssets.GetSound("HUD_Click_Close", false));
		}
		this.buildingGroupsRoot.GetComponent<ExpandRevealUIContent>().Collapse(delegate(object s)
		{
			this.ClearButtons();
			this.buildingGroupsRoot.gameObject.SetActive(false);
			this.ForceUpdateAllCategoryToggles(null);
		});
		this.PlanCategoryLabel.text = "";
		this.ForceUpdateAllCategoryToggles(null);
	}

	// Token: 0x0600A6C0 RID: 42688 RVA: 0x00400E08 File Offset: 0x003FF008
	private void OnClickCategory(KIconToggleMenu.ToggleInfo toggle_info)
	{
		this.CloseRecipe(false);
		if (!this.CategoryInteractive.ContainsKey(toggle_info) || !this.CategoryInteractive[toggle_info])
		{
			this.CloseCategoryPanel(false);
			KMonoBehaviour.PlaySound(GlobalAssets.GetSound("Negative", false));
			return;
		}
		if (this.activeCategoryInfo == toggle_info)
		{
			this.CloseCategoryPanel(true);
		}
		else
		{
			this.OpenCategoryPanel(toggle_info, true);
		}
		this.ConfigurePanelSize(null);
		this.SetScrollPoint(0f);
	}

	// Token: 0x0600A6C1 RID: 42689 RVA: 0x00400E7C File Offset: 0x003FF07C
	private void OpenCategoryPanel(KIconToggleMenu.ToggleInfo toggle_info, bool play_sound = true)
	{
		HashedString hashedString = (HashedString)toggle_info.userData;
		if (BuildingGroupScreen.Instance != null)
		{
			BuildingGroupScreen.Instance.ClearSearch();
		}
		this.ClearButtons();
		this.buildingGroupsRoot.gameObject.SetActive(true);
		this.activeCategoryInfo = toggle_info;
		if (play_sound)
		{
			UISounds.PlaySound(UISounds.Sound.ClickObject);
		}
		this.BuildButtonList();
		this.UpdateBuildingButtonList(this.activeCategoryInfo);
		this.RefreshCategoryPanelTitle();
		this.ForceUpdateAllCategoryToggles(null);
		this.buildingGroupsRoot.GetComponent<ExpandRevealUIContent>().Expand(null);
	}

	// Token: 0x0600A6C2 RID: 42690 RVA: 0x00400F04 File Offset: 0x003FF104
	public void RefreshCategoryPanelTitle()
	{
		if (this.activeCategoryInfo != null)
		{
			this.PlanCategoryLabel.text = this.activeCategoryInfo.text.ToUpper();
		}
		if (!BuildingGroupScreen.SearchIsEmpty)
		{
			this.PlanCategoryLabel.text = UI.BUILDMENU.SEARCH_RESULTS_HEADER;
		}
	}

	// Token: 0x0600A6C3 RID: 42691 RVA: 0x00400F50 File Offset: 0x003FF150
	public void RefreshSearch()
	{
		if (BuildingGroupScreen.SearchIsEmpty)
		{
			using (Dictionary<string, SearchUtil.SubcategoryCache>.Enumerator enumerator = this.subcategorySearchCaches.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					KeyValuePair<string, SearchUtil.SubcategoryCache> keyValuePair = enumerator.Current;
					keyValuePair.Value.Reset();
				}
				goto IL_98;
			}
		}
		string searchStringUpper = BuildingGroupScreen.Instance.inputField.text.ToUpper().Trim();
		foreach (KeyValuePair<string, SearchUtil.SubcategoryCache> keyValuePair2 in this.subcategorySearchCaches)
		{
			keyValuePair2.Value.Bind(searchStringUpper);
		}
		IL_98:
		this.SortButtons();
		this.SortSubcategories();
		this.ForceRefreshAllBuildingToggles();
	}

	// Token: 0x0600A6C4 RID: 42692 RVA: 0x00401024 File Offset: 0x003FF224
	public void OpenCategoryByName(string category)
	{
		PlanScreen.ToggleEntry toggleEntry;
		if (this.GetToggleEntryForCategory(category, out toggleEntry))
		{
			this.OpenCategoryPanel(toggleEntry.toggleInfo, false);
			this.ConfigurePanelSize(null);
		}
	}

	// Token: 0x0600A6C5 RID: 42693 RVA: 0x00401058 File Offset: 0x003FF258
	private void UpdateBuildingButton(int i, bool checkScore)
	{
		KeyValuePair<string, PlanBuildingToggle> keyValuePair = this.allBuildingToggles.ElementAt(i);
		bool? passesSearchFilter = checkScore ? new bool?(this.buildingDefSearchCaches[keyValuePair.Key].IsPassingScore()) : null;
		if (keyValuePair.Value.Refresh(passesSearchFilter))
		{
			this.categoryPanelSizeNeedsRefresh = true;
		}
		keyValuePair.Value.SwitchViewMode(this.useSubCategoryLayout);
	}

	// Token: 0x0600A6C6 RID: 42694 RVA: 0x004010C8 File Offset: 0x003FF2C8
	private void UpdateBuildingButtonList(KIconToggleMenu.ToggleInfo toggle_info)
	{
		KToggle toggle = toggle_info.toggle;
		if (toggle == null)
		{
			foreach (KIconToggleMenu.ToggleInfo toggleInfo in this.toggleInfo)
			{
				if (toggleInfo.userData == toggle_info.userData)
				{
					toggle = toggleInfo.toggle;
					break;
				}
			}
		}
		if (toggle != null && this.allBuildingToggles.Count != 0)
		{
			bool checkScore = !BuildingGroupScreen.SearchIsEmpty;
			if (this.forceRefreshAllBuildings)
			{
				this.forceRefreshAllBuildings = false;
				for (int num = 0; num != this.allBuildingToggles.Count; num++)
				{
					this.UpdateBuildingButton(num, checkScore);
				}
			}
			else
			{
				for (int i = 0; i < this.maxToggleRefreshPerFrame; i++)
				{
					if (this.building_button_refresh_idx >= this.allBuildingToggles.Count)
					{
						this.building_button_refresh_idx = 0;
					}
					this.UpdateBuildingButton(this.building_button_refresh_idx, checkScore);
					this.building_button_refresh_idx++;
				}
			}
		}
		foreach (KeyValuePair<string, GameObject> keyValuePair in this.allSubCategoryObjects)
		{
			GridLayoutGroup componentInChildren = keyValuePair.Value.GetComponentInChildren<GridLayoutGroup>(true);
			if (!(componentInChildren == null))
			{
				int num2 = 0;
				for (int j = 0; j < componentInChildren.transform.childCount; j++)
				{
					if (componentInChildren.transform.GetChild(j).gameObject.activeSelf)
					{
						num2++;
					}
				}
				bool flag = num2 > 0;
				if (keyValuePair.Value.activeSelf != flag)
				{
					keyValuePair.Value.SetActive(flag);
				}
			}
		}
		if (this.categoryPanelSizeNeedsRefresh && this.building_button_refresh_idx >= this.activeCategoryBuildingToggles.Count)
		{
			this.categoryPanelSizeNeedsRefresh = false;
			this.ConfigurePanelSize(null);
		}
	}

	// Token: 0x0600A6C7 RID: 42695 RVA: 0x00110A07 File Offset: 0x0010EC07
	public override void ScreenUpdate(bool topLevel)
	{
		base.ScreenUpdate(topLevel);
		this.RefreshBuildableStates(false);
		this.SetCategoryButtonState();
		if (this.activeCategoryInfo != null)
		{
			this.UpdateBuildingButtonList(this.activeCategoryInfo);
		}
	}

	// Token: 0x0600A6C8 RID: 42696 RVA: 0x004012C0 File Offset: 0x003FF4C0
	private void CacheSearchCaches()
	{
		this.<CacheSearchCaches>g__ManifestSubcategoryCache|128_0("default", string.Empty);
		foreach (PlanScreen.PlanInfo planInfo in TUNING.BUILDINGS.PLANORDER)
		{
			foreach (KeyValuePair<string, string> keyValuePair in planInfo.buildingAndSubcategoryData)
			{
				BuildingDef buildingDef = Assets.GetBuildingDef(keyValuePair.Key);
				SearchUtil.BuildingDefCache buildingDefCache = null;
				if (buildingDef.IsAvailable() && buildingDef.ShouldShowInBuildMenu() && Game.IsCorrectDlcActiveForCurrentSave(buildingDef) && !this.buildingDefSearchCaches.TryGetValue(buildingDef.PrefabID, out buildingDefCache))
				{
					buildingDefCache = SearchUtil.MakeBuildingDefCache(buildingDef);
					this.buildingDefSearchCaches[buildingDef.PrefabID] = buildingDefCache;
				}
				SearchUtil.SubcategoryCache subcategoryCache = this.<CacheSearchCaches>g__ManifestSubcategoryCache|128_0(keyValuePair.Value, null);
				if (buildingDefCache != null)
				{
					subcategoryCache.buildingDefs.Add(buildingDefCache);
				}
			}
		}
	}

	// Token: 0x0600A6C9 RID: 42697 RVA: 0x004013DC File Offset: 0x003FF5DC
	private void CollectRequiredBuildingDefs(List<BuildingDef> defs)
	{
		foreach (PlanScreen.PlanInfo planInfo in TUNING.BUILDINGS.PLANORDER)
		{
			foreach (KeyValuePair<string, string> keyValuePair in planInfo.buildingAndSubcategoryData)
			{
				BuildingDef buildingDef = Assets.GetBuildingDef(keyValuePair.Key);
				if (buildingDef.IsAvailable() && buildingDef.ShouldShowInBuildMenu() && Game.IsCorrectDlcActiveForCurrentSave(buildingDef))
				{
					defs.Add(buildingDef);
				}
			}
		}
	}

	// Token: 0x0600A6CA RID: 42698 RVA: 0x00110A31 File Offset: 0x0010EC31
	private int CompareScores(global::Tuple<PlanBuildingToggle, string> a, global::Tuple<PlanBuildingToggle, string> b)
	{
		return this.buildingDefSearchCaches[a.second].CompareTo(this.buildingDefSearchCaches[b.second]);
	}

	// Token: 0x17000AB7 RID: 2743
	// (get) Token: 0x0600A6CB RID: 42699 RVA: 0x00110A5A File Offset: 0x0010EC5A
	private Comparer<global::Tuple<PlanBuildingToggle, string>> BuildingDefComparer
	{
		get
		{
			if (this.buildingDefComparer == null)
			{
				this.buildingDefComparer = Comparer<global::Tuple<PlanBuildingToggle, string>>.Create(new Comparison<global::Tuple<PlanBuildingToggle, string>>(this.CompareScores));
			}
			return this.buildingDefComparer;
		}
	}

	// Token: 0x0600A6CC RID: 42700 RVA: 0x0040148C File Offset: 0x003FF68C
	private void SortButtons()
	{
		ListPool<BuildingDef, PlanScreen>.PooledList pooledList = ListPool<BuildingDef, PlanScreen>.Allocate();
		this.CollectRequiredBuildingDefs(pooledList);
		ListPool<global::Tuple<PlanBuildingToggle, string>, PlanScreen>.PooledList pooledList2 = ListPool<global::Tuple<PlanBuildingToggle, string>, PlanScreen>.Allocate();
		foreach (BuildingDef buildingDef in pooledList)
		{
			global::Tuple<PlanBuildingToggle, string> tuple = new global::Tuple<PlanBuildingToggle, string>(this.allBuildingToggles[buildingDef.PrefabID], buildingDef.PrefabID);
			int num = pooledList2.BinarySearch(tuple, this.BuildingDefComparer);
			if (num < 0)
			{
				num = ~num;
			}
			while (num < pooledList2.Count && this.CompareScores(tuple, pooledList2[num]) == 0)
			{
				num++;
			}
			pooledList2.Insert(num, tuple);
		}
		pooledList.Recycle();
		foreach (global::Tuple<PlanBuildingToggle, string> tuple2 in pooledList2)
		{
			tuple2.first.transform.SetAsLastSibling();
		}
		pooledList2.Recycle();
	}

	// Token: 0x0600A6CD RID: 42701 RVA: 0x004015A0 File Offset: 0x003FF7A0
	private void SortSubcategories()
	{
		Comparer<global::Tuple<GameObject, string>> comparer = Comparer<global::Tuple<GameObject, string>>.Create(new Comparison<global::Tuple<GameObject, string>>(this.<SortSubcategories>g__CompareScores|135_0));
		ListPool<global::Tuple<GameObject, string>, PlanScreen>.PooledList pooledList = ListPool<global::Tuple<GameObject, string>, PlanScreen>.Allocate();
		foreach (string text in this.stableSubcategoryOrder)
		{
			global::Tuple<GameObject, string> tuple = new global::Tuple<GameObject, string>(this.allSubCategoryObjects[text], text);
			int num = pooledList.BinarySearch(tuple, comparer);
			if (num < 0)
			{
				num = ~num;
			}
			while (num < pooledList.Count && this.<SortSubcategories>g__CompareScores|135_0(tuple, pooledList[num]) == 0)
			{
				num++;
			}
			pooledList.Insert(num, tuple);
		}
		foreach (global::Tuple<GameObject, string> tuple2 in pooledList)
		{
			tuple2.first.transform.SetAsLastSibling();
		}
		pooledList.Recycle();
	}

	// Token: 0x0600A6CE RID: 42702 RVA: 0x004016A8 File Offset: 0x003FF8A8
	private void BuildButtonList()
	{
		this.activeCategoryBuildingToggles.Clear();
		this.CacheSearchCaches();
		DictionaryPool<string, HashedString, PlanScreen>.PooledDictionary pooledDictionary = DictionaryPool<string, HashedString, PlanScreen>.Allocate();
		DictionaryPool<string, List<BuildingDef>, PlanScreen>.PooledDictionary pooledDictionary2 = DictionaryPool<string, List<BuildingDef>, PlanScreen>.Allocate();
		if (!pooledDictionary2.ContainsKey("default"))
		{
			pooledDictionary2.Add("default", new List<BuildingDef>());
		}
		foreach (PlanScreen.PlanInfo planInfo in TUNING.BUILDINGS.PLANORDER)
		{
			foreach (KeyValuePair<string, string> keyValuePair in planInfo.buildingAndSubcategoryData)
			{
				BuildingDef buildingDef = Assets.GetBuildingDef(keyValuePair.Key);
				if (buildingDef.IsAvailable() && buildingDef.ShouldShowInBuildMenu() && Game.IsCorrectDlcActiveForCurrentSave(buildingDef))
				{
					pooledDictionary.Add(buildingDef.PrefabID, planInfo.category);
					if (!pooledDictionary2.ContainsKey(keyValuePair.Value))
					{
						pooledDictionary2.Add(keyValuePair.Value, new List<BuildingDef>());
					}
					pooledDictionary2[keyValuePair.Value].Add(buildingDef);
				}
			}
		}
		if (this.stableSubcategoryOrder.Count == 0)
		{
			foreach (PlanScreen.PlanInfo ptr in TUNING.BUILDINGS.PLANORDER)
			{
				this.<BuildButtonList>g__RegisterSubcategory|136_0("default");
				foreach (KeyValuePair<string, string> keyValuePair2 in ptr.buildingAndSubcategoryData)
				{
					this.<BuildButtonList>g__RegisterSubcategory|136_0(keyValuePair2.Value);
				}
			}
		}
		GameObject gameObject = this.allSubCategoryObjects["default"].GetComponent<HierarchyReferences>().GetReference<GridLayoutGroup>("Grid").gameObject;
		bool flag = !BuildingGroupScreen.SearchIsEmpty;
		foreach (string text in this.stableSubcategoryOrder)
		{
			List<BuildingDef> list;
			if (pooledDictionary2.TryGetValue(text, out list))
			{
				if (text == "default")
				{
					this.allSubCategoryObjects[text].SetActive(this.useSubCategoryLayout);
				}
				HierarchyReferences component = this.allSubCategoryObjects[text].GetComponent<HierarchyReferences>();
				GameObject parent;
				if (this.useSubCategoryLayout)
				{
					component.GetReference<RectTransform>("Header").gameObject.SetActive(true);
					parent = this.allSubCategoryObjects[text].GetComponent<HierarchyReferences>().GetReference<GridLayoutGroup>("Grid").gameObject;
					StringEntry entry;
					if (Strings.TryGet("STRINGS.UI.NEWBUILDCATEGORIES." + text.ToUpper() + ".BUILDMENUTITLE", out entry))
					{
						component.GetReference<LocText>("HeaderLabel").SetText(entry);
					}
				}
				else
				{
					component.GetReference<RectTransform>("Header").gameObject.SetActive(false);
					parent = gameObject;
				}
				foreach (BuildingDef buildingDef2 in list)
				{
					HashedString hashedString = pooledDictionary[buildingDef2.PrefabID];
					GameObject gameObject2 = this.CreateButton(buildingDef2, parent, hashedString, flag);
					PlanScreen.ToggleEntry toggleEntry;
					this.GetToggleEntryForCategory(hashedString, out toggleEntry);
					if (toggleEntry != null && toggleEntry.pendingResearchAttentions.Contains(buildingDef2.PrefabID))
					{
						gameObject2.GetComponent<PlanCategoryNotifications>().ToggleAttention(true);
					}
				}
			}
		}
		pooledDictionary2.Recycle();
		pooledDictionary.Recycle();
		if (flag)
		{
			this.RefreshSearch();
		}
		this.ForceRefreshAllBuildingToggles();
		this.RefreshScale(null);
	}

	// Token: 0x0600A6CF RID: 42703 RVA: 0x00401ACC File Offset: 0x003FFCCC
	public void ConfigurePanelSize(object data = null)
	{
		if (this.useSubCategoryLayout)
		{
			this.buildGrid_bg_rowHeight = 48f;
		}
		else
		{
			this.buildGrid_bg_rowHeight = (ScreenResolutionMonitor.UsingGamepadUIMode() ? PlanScreen.bigBuildingButtonSize.y : PlanScreen.standarduildingButtonSize.y);
		}
		GridLayoutGroup reference = this.subgroupPrefab.GetComponent<HierarchyReferences>().GetReference<GridLayoutGroup>("Grid");
		this.buildGrid_bg_rowHeight += reference.spacing.y;
		int num = 0;
		int num2 = 0;
		for (int i = 0; i < this.GroupsTransform.childCount; i++)
		{
			int num3 = 0;
			HierarchyReferences component = this.GroupsTransform.GetChild(i).GetComponent<HierarchyReferences>();
			if (!(component == null))
			{
				GridLayoutGroup reference2 = component.GetReference<GridLayoutGroup>("Grid");
				if (!(reference2 == null))
				{
					for (int j = 0; j < reference2.transform.childCount; j++)
					{
						if (reference2.transform.GetChild(j).gameObject.activeSelf)
						{
							num3++;
						}
					}
					if (num3 > 0)
					{
						num2 += 24;
					}
					num += num3 / reference2.constraintCount;
					if (num3 % reference2.constraintCount != 0)
					{
						num++;
					}
				}
			}
		}
		num2 = Math.Min(72, num2);
		this.noResultMessage.SetActive(num == 0);
		int num4 = num;
		int num5 = Math.Max(1, Screen.height / (int)this.buildGrid_bg_rowHeight - 3);
		num5 = Math.Min(num5, this.useSubCategoryLayout ? 12 : 6);
		if (BuildingGroupScreen.IsEditing || !BuildingGroupScreen.SearchIsEmpty)
		{
			num4 = Mathf.Min(num5, this.useSubCategoryLayout ? 8 : 4);
		}
		this.BuildingGroupContentsRect.GetComponent<ScrollRect>().verticalScrollbar.gameObject.SetActive(num4 >= num5 - 1);
		float num6 = this.buildGrid_bg_borderHeight + (float)num2 + 36f + (float)Mathf.Clamp(num4, 0, num5) * this.buildGrid_bg_rowHeight;
		if (BuildingGroupScreen.IsEditing || !BuildingGroupScreen.SearchIsEmpty)
		{
			num6 = Mathf.Max(num6, this.buildingGroupsRoot.sizeDelta.y);
		}
		this.buildingGroupsRoot.sizeDelta = new Vector2(this.buildGrid_bg_width, num6);
		this.RefreshScale(null);
	}

	// Token: 0x0600A6D0 RID: 42704 RVA: 0x00110A81 File Offset: 0x0010EC81
	private void SetScrollPoint(float targetY)
	{
		this.BuildingGroupContentsRect.anchoredPosition = new Vector2(this.BuildingGroupContentsRect.anchoredPosition.x, targetY);
	}

	// Token: 0x0600A6D1 RID: 42705 RVA: 0x00401CF4 File Offset: 0x003FFEF4
	private GameObject CreateButton(BuildingDef def, GameObject parent, HashedString plan_category, bool checkScore)
	{
		bool? passesSearchFilter = checkScore ? new bool?(this.buildingDefSearchCaches[def.PrefabID].IsPassingScore()) : null;
		PlanBuildingToggle componentInChildren;
		GameObject gameObject;
		if (this.allBuildingToggles.TryGetValue(def.PrefabID, out componentInChildren))
		{
			gameObject = componentInChildren.gameObject;
			componentInChildren.Refresh(passesSearchFilter);
		}
		else
		{
			gameObject = global::Util.KInstantiateUI(this.planButtonPrefab, parent, false);
			gameObject.name = UI.StripLinkFormatting(def.name) + " Group:" + plan_category.ToString();
			componentInChildren = gameObject.GetComponentInChildren<PlanBuildingToggle>();
			componentInChildren.Config(def, this, plan_category, passesSearchFilter);
			componentInChildren.soundPlayer.Enabled = false;
			componentInChildren.SwitchViewMode(this.useSubCategoryLayout);
			this.allBuildingToggles.Add(def.PrefabID, componentInChildren);
		}
		if (gameObject.transform.parent != parent)
		{
			gameObject.transform.SetParent(parent.transform);
		}
		this.activeCategoryBuildingToggles.Add(def, componentInChildren);
		return gameObject;
	}

	// Token: 0x0600A6D2 RID: 42706 RVA: 0x00110AA4 File Offset: 0x0010ECA4
	public static bool TechRequirementsMet(TechItem techItem)
	{
		return DebugHandler.InstantBuildMode || Game.Instance.SandboxModeActive || techItem == null || techItem.IsComplete();
	}

	// Token: 0x0600A6D3 RID: 42707 RVA: 0x00110AC4 File Offset: 0x0010ECC4
	private static bool TechRequirementsUpcoming(TechItem techItem)
	{
		return PlanScreen.TechRequirementsMet(techItem);
	}

	// Token: 0x0600A6D4 RID: 42708 RVA: 0x00401DF8 File Offset: 0x003FFFF8
	private bool GetToggleEntryForCategory(HashedString category, out PlanScreen.ToggleEntry toggleEntry)
	{
		toggleEntry = null;
		foreach (PlanScreen.ToggleEntry toggleEntry2 in this.toggleEntries)
		{
			if (toggleEntry2.planCategory == category)
			{
				toggleEntry = toggleEntry2;
				return true;
			}
		}
		return false;
	}

	// Token: 0x0600A6D5 RID: 42709 RVA: 0x00110ACC File Offset: 0x0010ECCC
	public bool IsDefBuildable(BuildingDef def)
	{
		return this.GetBuildableState(def) == PlanScreen.RequirementsState.Complete;
	}

	// Token: 0x0600A6D6 RID: 42710 RVA: 0x00401E60 File Offset: 0x00400060
	public string GetTooltipForBuildable(BuildingDef def)
	{
		PlanScreen.RequirementsState buildableState = this.GetBuildableState(def);
		return PlanScreen.GetTooltipForRequirementsState(def, buildableState);
	}

	// Token: 0x0600A6D7 RID: 42711 RVA: 0x00401E7C File Offset: 0x0040007C
	public static string GetTooltipForRequirementsState(BuildingDef def, PlanScreen.RequirementsState state)
	{
		TechItem techItem = Db.Get().TechItems.TryGet(def.PrefabID);
		string text = null;
		if (Game.Instance.SandboxModeActive)
		{
			text = UIConstants.ColorPrefixYellow + UI.SANDBOXTOOLS.SETTINGS.INSTANT_BUILD.NAME + UIConstants.ColorSuffix;
		}
		else if (DebugHandler.InstantBuildMode)
		{
			text = UIConstants.ColorPrefixYellow + UI.DEBUG_TOOLS.DEBUG_ACTIVE + UIConstants.ColorSuffix;
		}
		else
		{
			switch (state)
			{
			case PlanScreen.RequirementsState.Tech:
				text = string.Format(UI.PRODUCTINFO_REQUIRESRESEARCHDESC, techItem.ParentTech.Name);
				break;
			case PlanScreen.RequirementsState.Materials:
				text = UI.PRODUCTINFO_MISSINGRESOURCES_HOVER;
				foreach (Recipe.Ingredient ingredient in def.CraftRecipe.Ingredients)
				{
					string str = string.Format("{0}{1}: {2}", "• ", ingredient.tag.ProperName(), GameUtil.GetFormattedMass(ingredient.amount, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"));
					text = text + "\n" + str;
				}
				break;
			case PlanScreen.RequirementsState.TelepadBuilt:
				text = UI.PRODUCTINFO_UNIQUE_PER_WORLD;
				break;
			case PlanScreen.RequirementsState.UniquePerWorld:
				text = UI.PRODUCTINFO_UNIQUE_PER_WORLD;
				break;
			case PlanScreen.RequirementsState.RocketInteriorOnly:
				text = UI.PRODUCTINFO_ROCKET_INTERIOR;
				break;
			case PlanScreen.RequirementsState.RocketInteriorForbidden:
				text = UI.PRODUCTINFO_ROCKET_NOT_INTERIOR;
				break;
			}
		}
		return text;
	}

	// Token: 0x0600A6D8 RID: 42712 RVA: 0x00110AD8 File Offset: 0x0010ECD8
	private void PointerEnter(PointerEventData data)
	{
		this.planScreenScrollRect.mouseIsOver = true;
	}

	// Token: 0x0600A6D9 RID: 42713 RVA: 0x00110AE6 File Offset: 0x0010ECE6
	private void PointerExit(PointerEventData data)
	{
		this.planScreenScrollRect.mouseIsOver = false;
	}

	// Token: 0x0600A6DA RID: 42714 RVA: 0x00402008 File Offset: 0x00400208
	public override void OnKeyDown(KButtonEvent e)
	{
		if (e.Consumed)
		{
			return;
		}
		if (this.mouseOver && base.ConsumeMouseScroll)
		{
			if (KInputManager.currentControllerIsGamepad)
			{
				if (e.IsAction(global::Action.ZoomIn) || e.IsAction(global::Action.ZoomOut))
				{
					this.planScreenScrollRect.OnKeyDown(e);
				}
			}
			else if (!e.TryConsume(global::Action.ZoomIn))
			{
				e.TryConsume(global::Action.ZoomOut);
			}
		}
		if (e.IsAction(global::Action.CopyBuilding) && e.TryConsume(global::Action.CopyBuilding))
		{
			this.OnClickCopyBuilding();
		}
		if (this.toggles == null)
		{
			return;
		}
		if (!e.Consumed && this.activeCategoryInfo != null && e.TryConsume(global::Action.Escape))
		{
			this.OnClickCategory(this.activeCategoryInfo);
			SelectTool.Instance.Activate();
			this.ClearSelection();
			return;
		}
		if (!e.Consumed)
		{
			base.OnKeyDown(e);
		}
	}

	// Token: 0x0600A6DB RID: 42715 RVA: 0x004020D0 File Offset: 0x004002D0
	public override void OnKeyUp(KButtonEvent e)
	{
		if (this.mouseOver && base.ConsumeMouseScroll)
		{
			if (KInputManager.currentControllerIsGamepad)
			{
				if (e.IsAction(global::Action.ZoomIn) || e.IsAction(global::Action.ZoomOut))
				{
					this.planScreenScrollRect.OnKeyUp(e);
				}
			}
			else if (!e.TryConsume(global::Action.ZoomIn))
			{
				e.TryConsume(global::Action.ZoomOut);
			}
		}
		if (e.Consumed)
		{
			return;
		}
		if (this.SelectedBuildingGameObject != null && PlayerController.Instance.ConsumeIfNotDragging(e, global::Action.MouseRight))
		{
			this.CloseRecipe(false);
			KMonoBehaviour.PlaySound(GlobalAssets.GetSound("HUD_Click_Close", false));
		}
		else if (this.activeCategoryInfo != null && PlayerController.Instance.ConsumeIfNotDragging(e, global::Action.MouseRight))
		{
			this.OnUIClear(null);
		}
		if (!e.Consumed)
		{
			base.OnKeyUp(e);
		}
	}

	// Token: 0x0600A6DC RID: 42716 RVA: 0x00402190 File Offset: 0x00400390
	private void OnRecipeElementsFullySelected()
	{
		BuildingDef buildingDef = null;
		foreach (KeyValuePair<string, PlanBuildingToggle> keyValuePair in this.allBuildingToggles)
		{
			if (keyValuePair.Value == this.currentlySelectedToggle)
			{
				buildingDef = Assets.GetBuildingDef(keyValuePair.Key);
				break;
			}
		}
		DebugUtil.DevAssert(buildingDef, "def is null", null);
		if (buildingDef)
		{
			if (buildingDef.isKAnimTile && buildingDef.isUtility)
			{
				IList<Tag> getSelectedElementAsList = this.ProductInfoScreen.materialSelectionPanel.GetSelectedElementAsList;
				((buildingDef.BuildingComplete.GetComponent<Wire>() != null) ? WireBuildTool.Instance : UtilityBuildTool.Instance).Activate(buildingDef, getSelectedElementAsList, this.ProductInfoScreen.FacadeSelectionPanel.SelectedFacade);
				return;
			}
			BuildTool.Instance.Activate(buildingDef, this.ProductInfoScreen.materialSelectionPanel.GetSelectedElementAsList, this.ProductInfoScreen.FacadeSelectionPanel.SelectedFacade);
		}
	}

	// Token: 0x0600A6DD RID: 42717 RVA: 0x004022A0 File Offset: 0x004004A0
	public void OnResearchComplete(object tech)
	{
		if (tech is Tech)
		{
			using (List<TechItem>.Enumerator enumerator = ((Tech)tech).unlockedItems.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					TechItem techItem = enumerator.Current;
					BuildingDef buildingDef = Assets.GetBuildingDef(techItem.Id);
					this.AddResearchedBuildingCategory(buildingDef);
				}
				return;
			}
		}
		if (tech is BuildingDef)
		{
			BuildingDef def = tech as BuildingDef;
			this.AddResearchedBuildingCategory(def);
		}
	}

	// Token: 0x0600A6DE RID: 42718 RVA: 0x00402320 File Offset: 0x00400520
	private void AddResearchedBuildingCategory(BuildingDef def)
	{
		if (def != null && Game.IsCorrectDlcActiveForCurrentSave(def))
		{
			this.UpdateDefResearched(def);
			if (this.tagCategoryMap.ContainsKey(def.Tag))
			{
				HashedString category = this.tagCategoryMap[def.Tag];
				PlanScreen.ToggleEntry toggleEntry;
				if (this.GetToggleEntryForCategory(category, out toggleEntry))
				{
					toggleEntry.pendingResearchAttentions.Add(def.Tag);
					toggleEntry.toggleInfo.toggle.GetComponent<PlanCategoryNotifications>().ToggleAttention(true);
					toggleEntry.Refresh();
				}
			}
		}
	}

	// Token: 0x0600A6DF RID: 42719 RVA: 0x004023A4 File Offset: 0x004005A4
	private void OnUIClear(object data)
	{
		if (this.activeCategoryInfo != null)
		{
			this.selected = -1;
			this.OnClickCategory(this.activeCategoryInfo);
			SelectTool.Instance.Activate();
			PlayerController.Instance.ActivateTool(SelectTool.Instance);
			SelectTool.Instance.Select(null, true);
		}
	}

	// Token: 0x0600A6E0 RID: 42720 RVA: 0x004023F4 File Offset: 0x004005F4
	private void OnActiveToolChanged(object data)
	{
		if (data == null)
		{
			return;
		}
		if (this.ignoreToolChangeMessages > 0)
		{
			return;
		}
		Type type = data.GetType();
		if (!typeof(BuildTool).IsAssignableFrom(type) && !typeof(PrebuildTool).IsAssignableFrom(type) && !typeof(BaseUtilityBuildTool).IsAssignableFrom(type))
		{
			this.CloseRecipe(false);
			this.CloseCategoryPanel(false);
		}
	}

	// Token: 0x0600A6E1 RID: 42721 RVA: 0x00110AF4 File Offset: 0x0010ECF4
	public PrioritySetting GetBuildingPriority()
	{
		return this.ProductInfoScreen.materialSelectionPanel.PriorityScreen.GetLastSelectedPriority();
	}

	// Token: 0x0600A6E8 RID: 42728 RVA: 0x004027CC File Offset: 0x004009CC
	[CompilerGenerated]
	private SearchUtil.SubcategoryCache <CacheSearchCaches>g__ManifestSubcategoryCache|128_0(string subcategory, string _text = null)
	{
		SearchUtil.SubcategoryCache subcategoryCache;
		if (!this.subcategorySearchCaches.TryGetValue(subcategory, out subcategoryCache))
		{
			subcategoryCache = new SearchUtil.SubcategoryCache
			{
				subcategory = new SearchUtil.MatchCache
				{
					text = SearchUtil.Canonicalize(_text ?? subcategory)
				},
				buildingDefs = new HashSet<SearchUtil.BuildingDefCache>()
			};
			this.subcategorySearchCaches[subcategory] = subcategoryCache;
		}
		return subcategoryCache;
	}

	// Token: 0x0600A6E9 RID: 42729 RVA: 0x00110B3C File Offset: 0x0010ED3C
	[CompilerGenerated]
	private int <SortSubcategories>g__CompareScores|135_0(global::Tuple<GameObject, string> a, global::Tuple<GameObject, string> b)
	{
		return this.subcategorySearchCaches[a.second].CompareTo(this.subcategorySearchCaches[b.second]);
	}

	// Token: 0x0600A6EA RID: 42730 RVA: 0x00402824 File Offset: 0x00400A24
	[CompilerGenerated]
	private void <BuildButtonList>g__RegisterSubcategory|136_0(string subcategory)
	{
		if (this.allSubCategoryObjects.ContainsKey(subcategory))
		{
			return;
		}
		GameObject gameObject = global::Util.KInstantiateUI(this.subgroupPrefab, this.GroupsTransform.gameObject, true);
		this.stableSubcategoryOrder.Add(subcategory);
		this.allSubCategoryObjects[subcategory] = gameObject;
		gameObject.SetActive(false);
	}

	// Token: 0x04008270 RID: 33392
	[SerializeField]
	private GameObject planButtonPrefab;

	// Token: 0x04008271 RID: 33393
	[SerializeField]
	private GameObject recipeInfoScreenParent;

	// Token: 0x04008272 RID: 33394
	[SerializeField]
	private GameObject productInfoScreenPrefab;

	// Token: 0x04008273 RID: 33395
	[SerializeField]
	private GameObject copyBuildingButton;

	// Token: 0x04008274 RID: 33396
	[SerializeField]
	private KButton gridViewButton;

	// Token: 0x04008275 RID: 33397
	[SerializeField]
	private KButton listViewButton;

	// Token: 0x04008276 RID: 33398
	private bool useSubCategoryLayout;

	// Token: 0x04008277 RID: 33399
	private int refreshScaleHandle = -1;

	// Token: 0x04008278 RID: 33400
	[SerializeField]
	private GameObject adjacentPinnedButtons;

	// Token: 0x04008279 RID: 33401
	private static Dictionary<HashedString, string> iconNameMap = new Dictionary<HashedString, string>
	{
		{
			PlanScreen.CacheHashedString("Base"),
			"icon_category_base"
		},
		{
			PlanScreen.CacheHashedString("Oxygen"),
			"icon_category_oxygen"
		},
		{
			PlanScreen.CacheHashedString("Power"),
			"icon_category_electrical"
		},
		{
			PlanScreen.CacheHashedString("Food"),
			"icon_category_food"
		},
		{
			PlanScreen.CacheHashedString("Plumbing"),
			"icon_category_plumbing"
		},
		{
			PlanScreen.CacheHashedString("HVAC"),
			"icon_category_ventilation"
		},
		{
			PlanScreen.CacheHashedString("Refining"),
			"icon_category_refinery"
		},
		{
			PlanScreen.CacheHashedString("Medical"),
			"icon_category_medical"
		},
		{
			PlanScreen.CacheHashedString("Furniture"),
			"icon_category_furniture"
		},
		{
			PlanScreen.CacheHashedString("Equipment"),
			"icon_category_misc"
		},
		{
			PlanScreen.CacheHashedString("Utilities"),
			"icon_category_utilities"
		},
		{
			PlanScreen.CacheHashedString("Automation"),
			"icon_category_automation"
		},
		{
			PlanScreen.CacheHashedString("Conveyance"),
			"icon_category_shipping"
		},
		{
			PlanScreen.CacheHashedString("Rocketry"),
			"icon_category_rocketry"
		},
		{
			PlanScreen.CacheHashedString("HEP"),
			"icon_category_radiation"
		}
	};

	// Token: 0x0400827A RID: 33402
	private Dictionary<KIconToggleMenu.ToggleInfo, bool> CategoryInteractive = new Dictionary<KIconToggleMenu.ToggleInfo, bool>();

	// Token: 0x0400827C RID: 33404
	[SerializeField]
	public PlanScreen.BuildingToolTipSettings buildingToolTipSettings;

	// Token: 0x0400827D RID: 33405
	public PlanScreen.BuildingNameTextSetting buildingNameTextSettings;

	// Token: 0x0400827E RID: 33406
	private KIconToggleMenu.ToggleInfo activeCategoryInfo;

	// Token: 0x0400827F RID: 33407
	public Dictionary<BuildingDef, PlanBuildingToggle> activeCategoryBuildingToggles = new Dictionary<BuildingDef, PlanBuildingToggle>();

	// Token: 0x04008280 RID: 33408
	private float timeSinceNotificationPing;

	// Token: 0x04008281 RID: 33409
	private float notificationPingExpire = 0.5f;

	// Token: 0x04008282 RID: 33410
	private float specialNotificationEmbellishDelay = 8f;

	// Token: 0x04008283 RID: 33411
	private int notificationPingCount;

	// Token: 0x04008284 RID: 33412
	private Dictionary<KToggle, Bouncer> toggleBouncers = new Dictionary<KToggle, Bouncer>();

	// Token: 0x04008285 RID: 33413
	public const string DEFAULT_SUBCATEGORY_KEY = "default";

	// Token: 0x04008286 RID: 33414
	private Dictionary<string, GameObject> allSubCategoryObjects = new Dictionary<string, GameObject>();

	// Token: 0x04008287 RID: 33415
	private Dictionary<string, PlanBuildingToggle> allBuildingToggles = new Dictionary<string, PlanBuildingToggle>();

	// Token: 0x04008288 RID: 33416
	private readonly Dictionary<string, SearchUtil.BuildingDefCache> buildingDefSearchCaches = new Dictionary<string, SearchUtil.BuildingDefCache>();

	// Token: 0x04008289 RID: 33417
	private readonly Dictionary<string, SearchUtil.SubcategoryCache> subcategorySearchCaches = new Dictionary<string, SearchUtil.SubcategoryCache>();

	// Token: 0x0400828A RID: 33418
	private readonly List<string> stableSubcategoryOrder = new List<string>();

	// Token: 0x0400828B RID: 33419
	private static Vector2 bigBuildingButtonSize = new Vector2(98f, 123f);

	// Token: 0x0400828C RID: 33420
	private static Vector2 standarduildingButtonSize = PlanScreen.bigBuildingButtonSize * 0.8f;

	// Token: 0x0400828D RID: 33421
	public static int fontSizeBigMode = 16;

	// Token: 0x0400828E RID: 33422
	public static int fontSizeStandardMode = 14;

	// Token: 0x04008290 RID: 33424
	[SerializeField]
	private GameObject subgroupPrefab;

	// Token: 0x04008291 RID: 33425
	public Transform GroupsTransform;

	// Token: 0x04008292 RID: 33426
	public Sprite Overlay_NeedTech;

	// Token: 0x04008293 RID: 33427
	public RectTransform buildingGroupsRoot;

	// Token: 0x04008294 RID: 33428
	public RectTransform BuildButtonBGPanel;

	// Token: 0x04008295 RID: 33429
	public RectTransform BuildingGroupContentsRect;

	// Token: 0x04008296 RID: 33430
	public Sprite defaultBuildingIconSprite;

	// Token: 0x04008297 RID: 33431
	private KScrollRect planScreenScrollRect;

	// Token: 0x04008298 RID: 33432
	public Material defaultUIMaterial;

	// Token: 0x04008299 RID: 33433
	public Material desaturatedUIMaterial;

	// Token: 0x0400829A RID: 33434
	public LocText PlanCategoryLabel;

	// Token: 0x0400829B RID: 33435
	public GameObject noResultMessage;

	// Token: 0x0400829C RID: 33436
	private int nextCategoryToUpdateIDX = -1;

	// Token: 0x0400829D RID: 33437
	private bool forceUpdateAllCategoryToggles;

	// Token: 0x0400829E RID: 33438
	private bool forceRefreshAllBuildings = true;

	// Token: 0x0400829F RID: 33439
	private List<PlanScreen.ToggleEntry> toggleEntries = new List<PlanScreen.ToggleEntry>();

	// Token: 0x040082A0 RID: 33440
	private int ignoreToolChangeMessages;

	// Token: 0x040082A1 RID: 33441
	private Dictionary<string, PlanScreen.RequirementsState> _buildableStatesByID = new Dictionary<string, PlanScreen.RequirementsState>();

	// Token: 0x040082A2 RID: 33442
	private Dictionary<Def, bool> _researchedDefs = new Dictionary<Def, bool>();

	// Token: 0x040082A3 RID: 33443
	[SerializeField]
	private TextStyleSetting[] CategoryLabelTextStyles;

	// Token: 0x040082A4 RID: 33444
	private float initTime;

	// Token: 0x040082A5 RID: 33445
	private Dictionary<Tag, HashedString> tagCategoryMap;

	// Token: 0x040082A6 RID: 33446
	private Dictionary<Tag, int> tagOrderMap;

	// Token: 0x040082A7 RID: 33447
	private BuildingDef lastSelectedBuildingDef;

	// Token: 0x040082A8 RID: 33448
	private Building lastSelectedBuilding;

	// Token: 0x040082A9 RID: 33449
	private string lastSelectedBuildingFacade = "DEFAULT_FACADE";

	// Token: 0x040082AA RID: 33450
	private int buildable_state_update_idx;

	// Token: 0x040082AB RID: 33451
	private int building_button_refresh_idx;

	// Token: 0x040082AC RID: 33452
	private readonly int maxToggleRefreshPerFrame = 10;

	// Token: 0x040082AD RID: 33453
	private bool categoryPanelSizeNeedsRefresh;

	// Token: 0x040082AE RID: 33454
	private Comparer<global::Tuple<PlanBuildingToggle, string>> buildingDefComparer;

	// Token: 0x040082AF RID: 33455
	private float buildGrid_bg_width = 320f;

	// Token: 0x040082B0 RID: 33456
	private float buildGrid_bg_borderHeight = 48f;

	// Token: 0x040082B1 RID: 33457
	private const float BUILDGRID_SEARCHBAR_HEIGHT = 36f;

	// Token: 0x040082B2 RID: 33458
	private const int SUBCATEGORY_HEADER_HEIGHT = 24;

	// Token: 0x040082B3 RID: 33459
	private float buildGrid_bg_rowHeight;

	// Token: 0x02001EFF RID: 7935
	public struct PlanInfo : IHasDlcRestrictions
	{
		// Token: 0x0600A6EB RID: 42731 RVA: 0x00402878 File Offset: 0x00400A78
		public PlanInfo(HashedString category, bool hideIfNotResearched, List<string> listData, string[] requiredDlcIds = null, string[] forbiddenDlcIds = null)
		{
			List<KeyValuePair<string, string>> list = new List<KeyValuePair<string, string>>();
			foreach (string key in listData)
			{
				list.Add(new KeyValuePair<string, string>(key, TUNING.BUILDINGS.PLANSUBCATEGORYSORTING.ContainsKey(key) ? TUNING.BUILDINGS.PLANSUBCATEGORYSORTING[key] : "uncategorized"));
			}
			this.category = category;
			this.hideIfNotResearched = hideIfNotResearched;
			this.data = listData;
			this.buildingAndSubcategoryData = list;
			this.requiredDlcIds = requiredDlcIds;
			this.forbiddenDlcIds = forbiddenDlcIds;
		}

		// Token: 0x0600A6EC RID: 42732 RVA: 0x00110B65 File Offset: 0x0010ED65
		public string[] GetRequiredDlcIds()
		{
			return this.requiredDlcIds;
		}

		// Token: 0x0600A6ED RID: 42733 RVA: 0x00110B6D File Offset: 0x0010ED6D
		public string[] GetForbiddenDlcIds()
		{
			return this.forbiddenDlcIds;
		}

		// Token: 0x040082B4 RID: 33460
		public HashedString category;

		// Token: 0x040082B5 RID: 33461
		public bool hideIfNotResearched;

		// Token: 0x040082B6 RID: 33462
		[Obsolete("Modders: Use ModUtil.AddBuildingToPlanScreen")]
		public List<string> data;

		// Token: 0x040082B7 RID: 33463
		public List<KeyValuePair<string, string>> buildingAndSubcategoryData;

		// Token: 0x040082B8 RID: 33464
		private string[] requiredDlcIds;

		// Token: 0x040082B9 RID: 33465
		private string[] forbiddenDlcIds;
	}

	// Token: 0x02001F00 RID: 7936
	[Serializable]
	public struct BuildingToolTipSettings
	{
		// Token: 0x040082BA RID: 33466
		public TextStyleSetting BuildButtonName;

		// Token: 0x040082BB RID: 33467
		public TextStyleSetting BuildButtonDescription;

		// Token: 0x040082BC RID: 33468
		public TextStyleSetting MaterialRequirement;

		// Token: 0x040082BD RID: 33469
		public TextStyleSetting ResearchRequirement;
	}

	// Token: 0x02001F01 RID: 7937
	[Serializable]
	public struct BuildingNameTextSetting
	{
		// Token: 0x040082BE RID: 33470
		public TextStyleSetting ActiveSelected;

		// Token: 0x040082BF RID: 33471
		public TextStyleSetting ActiveDeselected;

		// Token: 0x040082C0 RID: 33472
		public TextStyleSetting InactiveSelected;

		// Token: 0x040082C1 RID: 33473
		public TextStyleSetting InactiveDeselected;
	}

	// Token: 0x02001F02 RID: 7938
	private class ToggleEntry
	{
		// Token: 0x0600A6EE RID: 42734 RVA: 0x0040291C File Offset: 0x00400B1C
		public ToggleEntry(KIconToggleMenu.ToggleInfo toggle_info, HashedString plan_category, List<BuildingDef> building_defs, bool hideIfNotResearched)
		{
			this.toggleInfo = toggle_info;
			this.planCategory = plan_category;
			building_defs.RemoveAll((BuildingDef def) => !Game.IsCorrectDlcActiveForCurrentSave(def));
			this.buildingDefs = building_defs;
			this.hideIfNotResearched = hideIfNotResearched;
			this.pendingResearchAttentions = new List<Tag>();
			this.requiredTechItems = new List<TechItem>();
			this.toggleImages = null;
			foreach (BuildingDef buildingDef in building_defs)
			{
				TechItem techItem = Db.Get().TechItems.TryGet(buildingDef.PrefabID);
				if (techItem == null)
				{
					this.requiredTechItems.Clear();
					break;
				}
				if (!this.requiredTechItems.Contains(techItem))
				{
					this.requiredTechItems.Add(techItem);
				}
			}
			this._areAnyRequiredTechItemsAvailable = false;
			this.Refresh();
		}

		// Token: 0x0600A6EF RID: 42735 RVA: 0x00110B75 File Offset: 0x0010ED75
		public bool AreAnyRequiredTechItemsAvailable()
		{
			return this._areAnyRequiredTechItemsAvailable;
		}

		// Token: 0x0600A6F0 RID: 42736 RVA: 0x00402A18 File Offset: 0x00400C18
		public void Refresh()
		{
			if (this._areAnyRequiredTechItemsAvailable)
			{
				return;
			}
			if (this.requiredTechItems.Count == 0)
			{
				this._areAnyRequiredTechItemsAvailable = true;
				return;
			}
			using (List<TechItem>.Enumerator enumerator = this.requiredTechItems.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (PlanScreen.TechRequirementsUpcoming(enumerator.Current))
					{
						this._areAnyRequiredTechItemsAvailable = true;
						break;
					}
				}
			}
		}

		// Token: 0x0600A6F1 RID: 42737 RVA: 0x00110B7D File Offset: 0x0010ED7D
		public void CollectToggleImages()
		{
			this.toggleImages = this.toggleInfo.toggle.gameObject.GetComponents<ImageToggleState>();
		}

		// Token: 0x040082C2 RID: 33474
		public KIconToggleMenu.ToggleInfo toggleInfo;

		// Token: 0x040082C3 RID: 33475
		public HashedString planCategory;

		// Token: 0x040082C4 RID: 33476
		public List<BuildingDef> buildingDefs;

		// Token: 0x040082C5 RID: 33477
		public List<Tag> pendingResearchAttentions;

		// Token: 0x040082C6 RID: 33478
		private List<TechItem> requiredTechItems;

		// Token: 0x040082C7 RID: 33479
		public ImageToggleState[] toggleImages;

		// Token: 0x040082C8 RID: 33480
		public bool hideIfNotResearched;

		// Token: 0x040082C9 RID: 33481
		private bool _areAnyRequiredTechItemsAvailable;
	}

	// Token: 0x02001F04 RID: 7940
	public enum RequirementsState
	{
		// Token: 0x040082CD RID: 33485
		Invalid,
		// Token: 0x040082CE RID: 33486
		Tech,
		// Token: 0x040082CF RID: 33487
		Materials,
		// Token: 0x040082D0 RID: 33488
		Complete,
		// Token: 0x040082D1 RID: 33489
		TelepadBuilt,
		// Token: 0x040082D2 RID: 33490
		UniquePerWorld,
		// Token: 0x040082D3 RID: 33491
		RocketInteriorOnly,
		// Token: 0x040082D4 RID: 33492
		RocketInteriorForbidden
	}
}
