using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Database;
using STRINGS;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02001D8E RID: 7566
public class KleiInventoryScreen : KModalScreen
{
	// Token: 0x17000A57 RID: 2647
	// (get) Token: 0x06009DF8 RID: 40440 RVA: 0x0010B379 File Offset: 0x00109579
	// (set) Token: 0x06009DF9 RID: 40441 RVA: 0x0010B381 File Offset: 0x00109581
	private PermitResource SelectedPermit { get; set; }

	// Token: 0x17000A58 RID: 2648
	// (get) Token: 0x06009DFA RID: 40442 RVA: 0x0010B38A File Offset: 0x0010958A
	// (set) Token: 0x06009DFB RID: 40443 RVA: 0x0010B392 File Offset: 0x00109592
	private string SelectedCategoryId { get; set; }

	// Token: 0x06009DFC RID: 40444 RVA: 0x003D9398 File Offset: 0x003D7598
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.closeButton.onClick += delegate()
		{
			this.Show(false);
		};
		base.ConsumeMouseScroll = true;
		this.galleryGridLayouter = new GridLayouter
		{
			minCellSize = 64f,
			maxCellSize = 96f,
			targetGridLayouts = new List<GridLayoutGroup>()
		};
		this.galleryGridLayouter.overrideParentForSizeReference = this.galleryGridContent;
		InventoryOrganization.Initialize();
	}

	// Token: 0x06009DFD RID: 40445 RVA: 0x0010B39B File Offset: 0x0010959B
	public override void OnKeyDown(KButtonEvent e)
	{
		if (e.TryConsume(global::Action.Escape) || e.TryConsume(global::Action.MouseRight))
		{
			this.Show(false);
		}
		base.OnKeyDown(e);
	}

	// Token: 0x06009DFE RID: 40446 RVA: 0x000CF8A0 File Offset: 0x000CDAA0
	public override float GetSortKey()
	{
		return 20f;
	}

	// Token: 0x06009DFF RID: 40447 RVA: 0x0010B1EA File Offset: 0x001093EA
	protected override void OnActivate()
	{
		this.OnShow(true);
	}

	// Token: 0x06009E00 RID: 40448 RVA: 0x0010B3BD File Offset: 0x001095BD
	protected override void OnShow(bool show)
	{
		base.OnShow(show);
		if (show)
		{
			this.InitConfig();
			this.ToggleDoublesOnly(0);
			this.ClearSearch();
		}
	}

	// Token: 0x06009E01 RID: 40449 RVA: 0x003D940C File Offset: 0x003D760C
	private void ToggleDoublesOnly(int newState)
	{
		this.showFilterState = newState;
		this.doublesOnlyToggle.ChangeState(this.showFilterState);
		this.doublesOnlyToggle.GetComponentInChildren<LocText>().text = this.showFilterState.ToString() + "+";
		string simpleTooltip = "";
		switch (this.showFilterState)
		{
		case 0:
			simpleTooltip = UI.KLEI_INVENTORY_SCREEN.TOOLTIP_VIEW_ALL_ITEMS;
			break;
		case 1:
			simpleTooltip = UI.KLEI_INVENTORY_SCREEN.TOOLTIP_VIEW_OWNED_ONLY;
			break;
		case 2:
			simpleTooltip = UI.KLEI_INVENTORY_SCREEN.TOOLTIP_VIEW_DOUBLES_ONLY;
			break;
		}
		ToolTip component = this.doublesOnlyToggle.GetComponent<ToolTip>();
		component.SetSimpleTooltip(simpleTooltip);
		component.refreshWhileHovering = true;
		component.forceRefresh = true;
		this.RefreshGallery();
	}

	// Token: 0x06009E02 RID: 40450 RVA: 0x003D94C4 File Offset: 0x003D76C4
	private void InitConfig()
	{
		if (this.initConfigComplete)
		{
			return;
		}
		this.initConfigComplete = true;
		this.galleryGridLayouter.RequestGridResize();
		this.categoryListContent.GetComponent<RectTransform>().offsetMax = new Vector2(0f, 0f);
		this.PopulateCategories();
		this.PopulateGallery();
		this.SelectCategory("BUILDINGS");
		this.searchField.onValueChanged.RemoveAllListeners();
		this.searchField.onValueChanged.AddListener(delegate(string value)
		{
			this.RefreshGallery();
		});
		this.clearSearchButton.ClearOnClick();
		this.clearSearchButton.onClick += this.ClearSearch;
		MultiToggle multiToggle = this.doublesOnlyToggle;
		multiToggle.onClick = (System.Action)Delegate.Combine(multiToggle.onClick, new System.Action(delegate()
		{
			int newState = (this.showFilterState + 1) % 3;
			this.ToggleDoublesOnly(newState);
		}));
	}

	// Token: 0x06009E03 RID: 40451 RVA: 0x0010B3DC File Offset: 0x001095DC
	protected override void OnCmpEnable()
	{
		base.OnCmpEnable();
		this.ToggleDoublesOnly(0);
		this.ClearSearch();
		if (!this.initConfigComplete)
		{
			this.InitConfig();
		}
		this.RefreshUI();
		KleiItemsStatusRefresher.AddOrGetListener(this).OnRefreshUI(delegate
		{
			this.RefreshUI();
		});
	}

	// Token: 0x06009E04 RID: 40452 RVA: 0x0010B41C File Offset: 0x0010961C
	private void ClearSearch()
	{
		this.searchField.text = "";
		this.searchField.placeholder.GetComponent<TextMeshProUGUI>().text = UI.KLEI_INVENTORY_SCREEN.SEARCH_PLACEHOLDER;
		this.RefreshGallery();
	}

	// Token: 0x06009E05 RID: 40453 RVA: 0x0010B453 File Offset: 0x00109653
	private void Update()
	{
		this.galleryGridLayouter.CheckIfShouldResizeGrid();
	}

	// Token: 0x06009E06 RID: 40454 RVA: 0x003D9598 File Offset: 0x003D7798
	private void RefreshUI()
	{
		this.IS_ONLINE = ThreadedHttps<KleiAccount>.Instance.HasValidTicket();
		this.RefreshCategories();
		this.RefreshGallery();
		if (this.SelectedCategoryId.IsNullOrWhiteSpace())
		{
			this.SelectCategory("BUILDINGS");
		}
		this.RefreshDetails();
		this.RefreshBarterPanel();
	}

	// Token: 0x06009E07 RID: 40455 RVA: 0x0010B460 File Offset: 0x00109660
	private GameObject GetAvailableGridButton()
	{
		if (this.recycledGalleryGridButtons.Count == 0)
		{
			return Util.KInstantiateUI(this.gridItemPrefab, this.galleryGridContent.gameObject, true);
		}
		GameObject result = this.recycledGalleryGridButtons[0];
		this.recycledGalleryGridButtons.RemoveAt(0);
		return result;
	}

	// Token: 0x06009E08 RID: 40456 RVA: 0x0010B49F File Offset: 0x0010969F
	private void RecycleGalleryGridButton(GameObject button)
	{
		button.GetComponent<MultiToggle>().onClick = null;
		this.recycledGalleryGridButtons.Add(button);
	}

	// Token: 0x06009E09 RID: 40457 RVA: 0x003D95E8 File Offset: 0x003D77E8
	public void PopulateCategories()
	{
		foreach (KeyValuePair<string, MultiToggle> keyValuePair in this.categoryToggles)
		{
			UnityEngine.Object.Destroy(keyValuePair.Value.gameObject);
		}
		this.categoryToggles.Clear();
		foreach (KeyValuePair<string, List<string>> keyValuePair2 in InventoryOrganization.categoryIdToSubcategoryIdsMap)
		{
			string categoryId2;
			List<string> list;
			keyValuePair2.Deconstruct(out categoryId2, out list);
			string categoryId = categoryId2;
			GameObject gameObject = Util.KInstantiateUI(this.categoryRowPrefab, this.categoryListContent.gameObject, true);
			HierarchyReferences component = gameObject.GetComponent<HierarchyReferences>();
			component.GetReference<LocText>("Label").SetText(InventoryOrganization.GetCategoryName(categoryId));
			component.GetReference<Image>("Icon").sprite = InventoryOrganization.categoryIdToIconMap[categoryId];
			MultiToggle component2 = gameObject.GetComponent<MultiToggle>();
			MultiToggle multiToggle = component2;
			multiToggle.onEnter = (System.Action)Delegate.Combine(multiToggle.onEnter, new System.Action(this.OnMouseOverToggle));
			component2.onClick = delegate()
			{
				this.SelectCategory(categoryId);
			};
			this.categoryToggles.Add(categoryId, component2);
			this.SetCatogoryClickUISound(categoryId, component2);
		}
	}

	// Token: 0x06009E0A RID: 40458 RVA: 0x003D9768 File Offset: 0x003D7968
	public void PopulateGallery()
	{
		foreach (KeyValuePair<PermitResource, MultiToggle> keyValuePair in this.galleryGridButtons)
		{
			this.RecycleGalleryGridButton(keyValuePair.Value.gameObject);
		}
		this.galleryGridButtons.Clear();
		this.galleryGridLayouter.ImmediateSizeGridToScreenResolution();
		foreach (PermitResource permitResource in Db.Get().Permits.resources)
		{
			if (!permitResource.Id.StartsWith("visonly_"))
			{
				this.AddItemToGallery(permitResource);
			}
		}
		this.subcategories.Sort((KleiInventoryUISubcategory a, KleiInventoryUISubcategory b) => InventoryOrganization.subcategoryIdToPresentationDataMap[a.subcategoryID].sortKey.CompareTo(InventoryOrganization.subcategoryIdToPresentationDataMap[b.subcategoryID].sortKey));
		foreach (KleiInventoryUISubcategory kleiInventoryUISubcategory in this.subcategories)
		{
			kleiInventoryUISubcategory.gameObject.transform.SetAsLastSibling();
		}
		this.CollectSubcategoryGridLayouts();
		this.CloseSubcategory("UNCATEGORIZED");
	}

	// Token: 0x06009E0B RID: 40459 RVA: 0x003D98C0 File Offset: 0x003D7AC0
	private void CloseSubcategory(string subcategoryID)
	{
		KleiInventoryUISubcategory kleiInventoryUISubcategory = this.subcategories.Find((KleiInventoryUISubcategory match) => match.subcategoryID == subcategoryID);
		if (kleiInventoryUISubcategory != null)
		{
			kleiInventoryUISubcategory.ToggleOpen(false);
		}
	}

	// Token: 0x06009E0C RID: 40460 RVA: 0x003D9904 File Offset: 0x003D7B04
	private void AddItemToSubcategoryUIContainer(GameObject itemButton, string subcategoryId)
	{
		KleiInventoryUISubcategory kleiInventoryUISubcategory = this.subcategories.Find((KleiInventoryUISubcategory match) => match.subcategoryID == subcategoryId);
		if (kleiInventoryUISubcategory == null)
		{
			kleiInventoryUISubcategory = Util.KInstantiateUI(this.subcategoryPrefab, this.galleryGridContent.gameObject, true).GetComponent<KleiInventoryUISubcategory>();
			kleiInventoryUISubcategory.subcategoryID = subcategoryId;
			this.subcategories.Add(kleiInventoryUISubcategory);
			kleiInventoryUISubcategory.SetIdentity(InventoryOrganization.GetSubcategoryName(subcategoryId), InventoryOrganization.subcategoryIdToPresentationDataMap[subcategoryId].icon);
		}
		itemButton.transform.SetParent(kleiInventoryUISubcategory.gridLayout.transform);
	}

	// Token: 0x06009E0D RID: 40461 RVA: 0x003D99B0 File Offset: 0x003D7BB0
	private void CollectSubcategoryGridLayouts()
	{
		this.galleryGridLayouter.OnSizeGridComplete = null;
		foreach (KleiInventoryUISubcategory kleiInventoryUISubcategory in this.subcategories)
		{
			this.galleryGridLayouter.targetGridLayouts.Add(kleiInventoryUISubcategory.gridLayout);
			GridLayouter gridLayouter = this.galleryGridLayouter;
			gridLayouter.OnSizeGridComplete = (System.Action)Delegate.Combine(gridLayouter.OnSizeGridComplete, new System.Action(kleiInventoryUISubcategory.RefreshDisplay));
		}
		this.galleryGridLayouter.RequestGridResize();
	}

	// Token: 0x06009E0E RID: 40462 RVA: 0x003D9A50 File Offset: 0x003D7C50
	private void AddItemToGallery(PermitResource permit)
	{
		if (this.galleryGridButtons.ContainsKey(permit))
		{
			return;
		}
		PermitPresentationInfo permitPresentationInfo = permit.GetPermitPresentationInfo();
		GameObject availableGridButton = this.GetAvailableGridButton();
		this.AddItemToSubcategoryUIContainer(availableGridButton, InventoryOrganization.GetPermitSubcategory(permit));
		HierarchyReferences component = availableGridButton.GetComponent<HierarchyReferences>();
		Image reference = component.GetReference<Image>("Icon");
		LocText reference2 = component.GetReference<LocText>("OwnedCountLabel");
		Image reference3 = component.GetReference<Image>("IsUnownedOverlay");
		Image reference4 = component.GetReference<Image>("DlcBanner");
		MultiToggle component2 = availableGridButton.GetComponent<MultiToggle>();
		reference.sprite = permitPresentationInfo.sprite;
		if (permit.IsOwnableOnServer())
		{
			int ownedCount = PermitItems.GetOwnedCount(permit);
			reference2.text = UI.KLEI_INVENTORY_SCREEN.ITEM_PLAYER_OWNED_AMOUNT_ICON.Replace("{OwnedCount}", ownedCount.ToString());
			reference2.gameObject.SetActive(ownedCount > 0);
			reference3.gameObject.SetActive(ownedCount <= 0);
		}
		else
		{
			reference2.gameObject.SetActive(false);
			reference3.gameObject.SetActive(false);
		}
		string dlcIdFrom = permit.GetDlcIdFrom();
		if (DlcManager.IsDlcId(dlcIdFrom))
		{
			reference4.gameObject.SetActive(true);
			reference4.color = DlcManager.GetDlcBannerColor(dlcIdFrom);
		}
		else
		{
			reference4.gameObject.SetActive(false);
		}
		MultiToggle multiToggle = component2;
		multiToggle.onEnter = (System.Action)Delegate.Combine(multiToggle.onEnter, new System.Action(this.OnMouseOverToggle));
		component2.onClick = delegate()
		{
			this.SelectItem(permit);
		};
		this.galleryGridButtons.Add(permit, component2);
		this.SetItemClickUISound(permit, component2);
		KleiItemsUI.ConfigureTooltipOn(availableGridButton, KleiItemsUI.GetTooltipStringFor(permit));
	}

	// Token: 0x06009E0F RID: 40463 RVA: 0x0010B4B9 File Offset: 0x001096B9
	public void SelectCategory(string categoryId)
	{
		if (InventoryOrganization.categoryIdToIsEmptyMap[categoryId])
		{
			return;
		}
		this.SelectedCategoryId = categoryId;
		this.galleryHeaderLabel.SetText(InventoryOrganization.GetCategoryName(categoryId));
		this.RefreshCategories();
		this.SelectDefaultCategoryItem();
	}

	// Token: 0x06009E10 RID: 40464 RVA: 0x003D9C1C File Offset: 0x003D7E1C
	private void SelectDefaultCategoryItem()
	{
		foreach (KeyValuePair<PermitResource, MultiToggle> keyValuePair in this.galleryGridButtons)
		{
			if (InventoryOrganization.categoryIdToSubcategoryIdsMap[this.SelectedCategoryId].Contains(InventoryOrganization.GetPermitSubcategory(keyValuePair.Key)))
			{
				this.SelectItem(keyValuePair.Key);
				return;
			}
		}
		this.SelectItem(null);
	}

	// Token: 0x06009E11 RID: 40465 RVA: 0x0010B4ED File Offset: 0x001096ED
	public void SelectItem(PermitResource permit)
	{
		this.SelectedPermit = permit;
		this.RefreshGallery();
		this.RefreshDetails();
		this.RefreshBarterPanel();
	}

	// Token: 0x06009E12 RID: 40466 RVA: 0x003D9CA4 File Offset: 0x003D7EA4
	private void RefreshGallery()
	{
		string value = this.searchField.text.ToUpper();
		foreach (KeyValuePair<PermitResource, MultiToggle> keyValuePair in this.galleryGridButtons)
		{
			PermitResource permitResource;
			MultiToggle multiToggle;
			keyValuePair.Deconstruct(out permitResource, out multiToggle);
			PermitResource permitResource2 = permitResource;
			MultiToggle multiToggle2 = multiToggle;
			string permitSubcategory = InventoryOrganization.GetPermitSubcategory(permitResource2);
			bool flag = permitSubcategory == "UNCATEGORIZED" || InventoryOrganization.categoryIdToSubcategoryIdsMap[this.SelectedCategoryId].Contains(permitSubcategory);
			flag = (flag && (permitResource2.Name.ToUpper().Contains(value) || permitResource2.Id.ToUpper().Contains(value) || permitResource2.Description.ToUpper().Contains(value)));
			multiToggle2.ChangeState((permitResource2 == this.SelectedPermit) ? 1 : 0);
			HierarchyReferences component = multiToggle2.gameObject.GetComponent<HierarchyReferences>();
			LocText reference = component.GetReference<LocText>("OwnedCountLabel");
			Image reference2 = component.GetReference<Image>("IsUnownedOverlay");
			if (permitResource2.IsOwnableOnServer())
			{
				int ownedCount = PermitItems.GetOwnedCount(permitResource2);
				reference.text = UI.KLEI_INVENTORY_SCREEN.ITEM_PLAYER_OWNED_AMOUNT_ICON.Replace("{OwnedCount}", ownedCount.ToString());
				reference.gameObject.SetActive(ownedCount > 0);
				reference2.gameObject.SetActive(ownedCount <= 0);
				if (this.showFilterState == 2 && ownedCount < 2)
				{
					flag = false;
				}
				else if (this.showFilterState == 1 && ownedCount == 0)
				{
					flag = false;
				}
			}
			else if (!permitResource2.IsUnlocked())
			{
				reference.gameObject.SetActive(false);
				reference2.gameObject.SetActive(true);
				if (this.showFilterState != 0)
				{
					flag = false;
				}
			}
			else
			{
				reference.gameObject.SetActive(false);
				reference2.gameObject.SetActive(false);
				if (this.showFilterState == 2)
				{
					flag = false;
				}
			}
			if (multiToggle2.gameObject.activeSelf != flag)
			{
				multiToggle2.gameObject.SetActive(flag);
			}
		}
		foreach (KleiInventoryUISubcategory kleiInventoryUISubcategory in this.subcategories)
		{
			kleiInventoryUISubcategory.RefreshDisplay();
		}
	}

	// Token: 0x06009E13 RID: 40467 RVA: 0x003D9F08 File Offset: 0x003D8108
	private void RefreshCategories()
	{
		foreach (KeyValuePair<string, MultiToggle> keyValuePair in this.categoryToggles)
		{
			keyValuePair.Value.ChangeState((keyValuePair.Key == this.SelectedCategoryId) ? 1 : 0);
			if (InventoryOrganization.categoryIdToIsEmptyMap[keyValuePair.Key])
			{
				keyValuePair.Value.ChangeState(2);
			}
			else
			{
				keyValuePair.Value.ChangeState((keyValuePair.Key == this.SelectedCategoryId) ? 1 : 0);
			}
		}
	}

	// Token: 0x06009E14 RID: 40468 RVA: 0x003D9FC0 File Offset: 0x003D81C0
	private void RefreshDetails()
	{
		PermitResource selectedPermit = this.SelectedPermit;
		PermitPresentationInfo permitPresentationInfo = selectedPermit.GetPermitPresentationInfo();
		this.permitVis.ConfigureWith(selectedPermit);
		this.selectionDetailsScrollRect.rectTransform().anchorMin = new Vector2(0f, 0f);
		this.selectionDetailsScrollRect.rectTransform().anchorMax = new Vector2(1f, 1f);
		this.selectionDetailsScrollRect.rectTransform().sizeDelta = new Vector2(-24f, 0f);
		this.selectionDetailsScrollRect.rectTransform().anchoredPosition = Vector2.zero;
		this.selectionDetailsScrollRect.content.rectTransform().sizeDelta = new Vector2(0f, this.selectionDetailsScrollRect.content.rectTransform().sizeDelta.y);
		this.selectionDetailsScrollRectScrollBarContainer.anchorMin = new Vector2(1f, 0f);
		this.selectionDetailsScrollRectScrollBarContainer.anchorMax = new Vector2(1f, 1f);
		this.selectionDetailsScrollRectScrollBarContainer.sizeDelta = new Vector2(24f, 0f);
		this.selectionDetailsScrollRectScrollBarContainer.anchoredPosition = Vector2.zero;
		this.selectionHeaderLabel.SetText(selectedPermit.Name);
		this.selectionNameLabel.SetText(selectedPermit.Name);
		this.selectionDescriptionLabel.gameObject.SetActive(!string.IsNullOrWhiteSpace(selectedPermit.Description));
		this.selectionDescriptionLabel.SetText(selectedPermit.Description);
		this.selectionFacadeForLabel.gameObject.SetActive(!string.IsNullOrWhiteSpace(permitPresentationInfo.facadeFor));
		this.selectionFacadeForLabel.SetText(permitPresentationInfo.facadeFor);
		string dlcIdFrom = selectedPermit.GetDlcIdFrom();
		if (DlcManager.IsDlcId(dlcIdFrom))
		{
			this.selectionRarityDetailsLabel.gameObject.SetActive(false);
			this.selectionOwnedCount.gameObject.SetActive(false);
			this.selectionCollectionLabel.gameObject.SetActive(true);
			if (selectedPermit.Rarity == PermitRarity.UniversalLocked)
			{
				this.selectionCollectionLabel.SetText(UI.KLEI_INVENTORY_SCREEN.COLLECTION_COMING_SOON.Replace("{Collection}", DlcManager.GetDlcTitle(dlcIdFrom)));
				return;
			}
			this.selectionCollectionLabel.SetText(UI.KLEI_INVENTORY_SCREEN.COLLECTION.Replace("{Collection}", DlcManager.GetDlcTitle(dlcIdFrom)));
			return;
		}
		else
		{
			this.selectionCollectionLabel.gameObject.SetActive(false);
			string text = UI.KLEI_INVENTORY_SCREEN.ITEM_RARITY_DETAILS.Replace("{RarityName}", selectedPermit.Rarity.GetLocStringName());
			this.selectionRarityDetailsLabel.gameObject.SetActive(!string.IsNullOrWhiteSpace(text));
			this.selectionRarityDetailsLabel.SetText(text);
			this.selectionOwnedCount.gameObject.SetActive(true);
			if (!selectedPermit.IsOwnableOnServer())
			{
				this.selectionOwnedCount.SetText(UI.KLEI_INVENTORY_SCREEN.ITEM_PLAYER_UNLOCKED_BUT_UNOWNABLE);
				return;
			}
			int ownedCount = PermitItems.GetOwnedCount(selectedPermit);
			if (ownedCount > 0)
			{
				this.selectionOwnedCount.SetText(UI.KLEI_INVENTORY_SCREEN.ITEM_PLAYER_OWNED_AMOUNT.Replace("{OwnedCount}", ownedCount.ToString()));
				return;
			}
			this.selectionOwnedCount.SetText(KleiItemsUI.WrapWithColor(UI.KLEI_INVENTORY_SCREEN.ITEM_PLAYER_OWN_NONE, KleiItemsUI.TEXT_COLOR__PERMIT_NOT_OWNED));
			return;
		}
	}

	// Token: 0x06009E15 RID: 40469 RVA: 0x003DA2D0 File Offset: 0x003D84D0
	private KleiInventoryScreen.PermitPrintabilityState GetPermitPrintabilityState(PermitResource permit)
	{
		if (!this.IS_ONLINE)
		{
			return KleiInventoryScreen.PermitPrintabilityState.UserOffline;
		}
		ulong num;
		ulong num2;
		PermitItems.TryGetBarterPrice(this.SelectedPermit.Id, out num, out num2);
		if (num == 0UL)
		{
			if (permit.Rarity == PermitRarity.Universal || permit.Rarity == PermitRarity.UniversalLocked || permit.Rarity == PermitRarity.Loyalty || permit.Rarity == PermitRarity.Unknown)
			{
				return KleiInventoryScreen.PermitPrintabilityState.NotForSale;
			}
			return KleiInventoryScreen.PermitPrintabilityState.NotForSaleYet;
		}
		else
		{
			if (PermitItems.GetOwnedCount(permit) > 0)
			{
				return KleiInventoryScreen.PermitPrintabilityState.AlreadyOwned;
			}
			if (KleiItems.GetFilamentAmount() < num)
			{
				return KleiInventoryScreen.PermitPrintabilityState.TooExpensive;
			}
			return KleiInventoryScreen.PermitPrintabilityState.Printable;
		}
	}

	// Token: 0x06009E16 RID: 40470 RVA: 0x003DA344 File Offset: 0x003D8544
	private void RefreshBarterPanel()
	{
		this.barterBuyButton.ClearOnClick();
		this.barterSellButton.ClearOnClick();
		this.barterBuyButton.isInteractable = this.IS_ONLINE;
		this.barterSellButton.isInteractable = this.IS_ONLINE;
		HierarchyReferences component = this.barterBuyButton.GetComponent<HierarchyReferences>();
		HierarchyReferences component2 = this.barterSellButton.GetComponent<HierarchyReferences>();
		new Color(1f, 0.69411767f, 0.69411767f);
		Color color = new Color(0.6f, 0.9529412f, 0.5019608f);
		LocText reference = component.GetReference<LocText>("CostLabel");
		LocText reference2 = component2.GetReference<LocText>("CostLabel");
		this.barterPanelBG.color = (this.IS_ONLINE ? Util.ColorFromHex("575D6F") : Util.ColorFromHex("6F6F6F"));
		this.filamentWalletSection.gameObject.SetActive(this.IS_ONLINE);
		this.barterOfflineLabel.gameObject.SetActive(!this.IS_ONLINE);
		ulong filamentAmount = KleiItems.GetFilamentAmount();
		this.filamentWalletSection.GetComponent<ToolTip>().SetSimpleTooltip((filamentAmount > 1UL) ? string.Format(UI.KLEI_INVENTORY_SCREEN.BARTERING.WALLET_PLURAL_TOOLTIP, filamentAmount) : string.Format(UI.KLEI_INVENTORY_SCREEN.BARTERING.WALLET_TOOLTIP, filamentAmount));
		KleiInventoryScreen.PermitPrintabilityState permitPrintabilityState = this.GetPermitPrintabilityState(this.SelectedPermit);
		if (!this.IS_ONLINE)
		{
			component.GetReference<LocText>("CostLabel").SetText("");
			reference2.SetText("");
			reference2.color = Color.white;
			this.barterBuyButton.GetComponent<ToolTip>().SetSimpleTooltip(UI.KLEI_INVENTORY_SCREEN.BARTERING.TOOLTIP_ACTION_INVALID_OFFLINE);
			this.barterSellButton.GetComponent<ToolTip>().SetSimpleTooltip(UI.KLEI_INVENTORY_SCREEN.BARTERING.TOOLTIP_ACTION_INVALID_OFFLINE);
			return;
		}
		ulong num;
		ulong num2;
		PermitItems.TryGetBarterPrice(this.SelectedPermit.Id, out num, out num2);
		this.filamentWalletSection.GetComponentInChildren<LocText>().SetText(KleiItems.GetFilamentAmount().ToString());
		switch (permitPrintabilityState)
		{
		case KleiInventoryScreen.PermitPrintabilityState.Printable:
			this.barterBuyButton.isInteractable = true;
			this.barterBuyButton.GetComponent<ToolTip>().SetSimpleTooltip(string.Format(UI.KLEI_INVENTORY_SCREEN.BARTERING.TOOLTIP_BUY_ACTIVE, num.ToString()));
			reference.SetText("-" + num.ToString());
			this.barterBuyButton.onClick += delegate()
			{
				GameObject gameObject = Util.KInstantiateUI(this.barterConfirmationScreenPrefab, LockerNavigator.Instance.gameObject, false);
				gameObject.rectTransform().sizeDelta = Vector2.zero;
				gameObject.GetComponent<BarterConfirmationScreen>().Present(this.SelectedPermit, true);
			};
			break;
		case KleiInventoryScreen.PermitPrintabilityState.AlreadyOwned:
			this.barterBuyButton.isInteractable = false;
			this.barterBuyButton.GetComponent<ToolTip>().SetSimpleTooltip(UI.KLEI_INVENTORY_SCREEN.BARTERING.TOOLTIP_UNBUYABLE_ALREADY_OWNED);
			reference.SetText("-" + num.ToString());
			break;
		case KleiInventoryScreen.PermitPrintabilityState.TooExpensive:
			this.barterBuyButton.isInteractable = false;
			this.barterBuyButton.GetComponent<ToolTip>().SetSimpleTooltip(UI.KLEI_INVENTORY_SCREEN.BARTERING.TOOLTIP_BUY_CANT_AFFORD.text);
			reference.SetText("-" + num.ToString());
			break;
		case KleiInventoryScreen.PermitPrintabilityState.NotForSale:
			this.barterBuyButton.isInteractable = false;
			this.barterBuyButton.GetComponent<ToolTip>().SetSimpleTooltip(UI.KLEI_INVENTORY_SCREEN.BARTERING.TOOLTIP_UNBUYABLE);
			reference.SetText("");
			break;
		case KleiInventoryScreen.PermitPrintabilityState.NotForSaleYet:
			this.barterBuyButton.isInteractable = false;
			this.barterBuyButton.GetComponent<ToolTip>().SetSimpleTooltip(UI.KLEI_INVENTORY_SCREEN.BARTERING.TOOLTIP_UNBUYABLE_BETA);
			reference.SetText("");
			break;
		}
		if (num2 == 0UL)
		{
			this.barterSellButton.isInteractable = false;
			this.barterSellButton.GetComponent<ToolTip>().SetSimpleTooltip(UI.KLEI_INVENTORY_SCREEN.BARTERING.TOOLTIP_UNSELLABLE);
			reference2.SetText("");
			reference2.color = Color.white;
			return;
		}
		bool flag = PermitItems.GetOwnedCount(this.SelectedPermit) > 0;
		this.barterSellButton.isInteractable = flag;
		this.barterSellButton.GetComponent<ToolTip>().SetSimpleTooltip(flag ? string.Format(UI.KLEI_INVENTORY_SCREEN.BARTERING.TOOLTIP_SELL_ACTIVE, num2.ToString()) : UI.KLEI_INVENTORY_SCREEN.BARTERING.TOOLTIP_NONE_TO_SELL.text);
		if (flag)
		{
			reference2.color = color;
			reference2.SetText("+" + num2.ToString());
		}
		else
		{
			reference2.color = Color.white;
			reference2.SetText("+" + num2.ToString());
		}
		this.barterSellButton.onClick += delegate()
		{
			GameObject gameObject = Util.KInstantiateUI(this.barterConfirmationScreenPrefab, LockerNavigator.Instance.gameObject, false);
			gameObject.rectTransform().sizeDelta = Vector2.zero;
			gameObject.GetComponent<BarterConfirmationScreen>().Present(this.SelectedPermit, false);
		};
	}

	// Token: 0x06009E17 RID: 40471 RVA: 0x003DA79C File Offset: 0x003D899C
	private void SetCatogoryClickUISound(string categoryID, MultiToggle toggle)
	{
		if (!this.categoryToggles.ContainsKey(categoryID))
		{
			toggle.states[1].on_click_override_sound_path = "";
			toggle.states[0].on_click_override_sound_path = "";
			return;
		}
		toggle.states[1].on_click_override_sound_path = "General_Category_Click";
		toggle.states[0].on_click_override_sound_path = "General_Category_Click";
	}

	// Token: 0x06009E18 RID: 40472 RVA: 0x003DA810 File Offset: 0x003D8A10
	private void SetItemClickUISound(PermitResource permit, MultiToggle toggle)
	{
		string facadeItemSoundName = KleiInventoryScreen.GetFacadeItemSoundName(permit);
		toggle.states[1].on_click_override_sound_path = facadeItemSoundName + "_Click";
		toggle.states[1].sound_parameter_name = "Unlocked";
		toggle.states[1].sound_parameter_value = (permit.IsUnlocked() ? 1f : 0f);
		toggle.states[1].has_sound_parameter = true;
		toggle.states[0].on_click_override_sound_path = facadeItemSoundName + "_Click";
		toggle.states[0].sound_parameter_name = "Unlocked";
		toggle.states[0].sound_parameter_value = (permit.IsUnlocked() ? 1f : 0f);
		toggle.states[0].has_sound_parameter = true;
	}

	// Token: 0x06009E19 RID: 40473 RVA: 0x003DA8F8 File Offset: 0x003D8AF8
	public static string GetFacadeItemSoundName(PermitResource permit)
	{
		if (permit == null)
		{
			return "HUD";
		}
		switch (permit.Category)
		{
		case PermitCategory.DupeTops:
			return "tops";
		case PermitCategory.DupeBottoms:
			return "bottoms";
		case PermitCategory.DupeGloves:
			return "gloves";
		case PermitCategory.DupeShoes:
			return "shoes";
		case PermitCategory.DupeHats:
			return "hats";
		case PermitCategory.AtmoSuitHelmet:
			return "atmosuit_helmet";
		case PermitCategory.AtmoSuitBody:
			return "tops";
		case PermitCategory.AtmoSuitGloves:
			return "gloves";
		case PermitCategory.AtmoSuitBelt:
			return "belt";
		case PermitCategory.AtmoSuitShoes:
			return "shoes";
		}
		if (permit.Category == PermitCategory.Building)
		{
			BuildingDef buildingDef = KleiPermitVisUtil.GetBuildingDef(permit);
			if (buildingDef == null)
			{
				return "HUD";
			}
			string prefabID = buildingDef.PrefabID;
			uint num = <PrivateImplementationDetails>.ComputeStringHash(prefabID);
			if (num <= 1938276536U)
			{
				if (num <= 1036100273U)
				{
					if (num <= 297556592U)
					{
						if (num <= 228062815U)
						{
							if (num != 38823703U)
							{
								if (num != 112031228U)
								{
									if (num != 228062815U)
									{
										goto IL_87F;
									}
									if (!(prefabID == "LuxuryBed"))
									{
										goto IL_87F;
									}
									string id = permit.Id;
									if (id == "LuxuryBed_boat")
									{
										return "elegantbed_boat";
									}
									if (!(id == "LuxuryBed_bouncy"))
									{
										return "elegantbed";
									}
									return "elegantbed_bouncy";
								}
								else
								{
									if (!(prefabID == "LogicGateDemultiplexer"))
									{
										goto IL_87F;
									}
									goto IL_84F;
								}
							}
							else
							{
								if (!(prefabID == "LogicGateXOR"))
								{
									goto IL_87F;
								}
								goto IL_84F;
							}
						}
						else if (num != 228549509U)
						{
							if (num != 296872528U)
							{
								if (num != 297556592U)
								{
									goto IL_87F;
								}
								if (!(prefabID == "LogicRibbonBridge"))
								{
									goto IL_87F;
								}
								goto IL_84F;
							}
							else if (!(prefabID == "ItemPedestal"))
							{
								goto IL_87F;
							}
						}
						else
						{
							if (!(prefabID == "WashSink"))
							{
								goto IL_87F;
							}
							return "sink";
						}
					}
					else if (num <= 595816591U)
					{
						if (num != 301047391U)
						{
							if (num != 585850236U)
							{
								if (num != 595816591U)
								{
									goto IL_87F;
								}
								if (!(prefabID == "FlowerVase"))
								{
									goto IL_87F;
								}
								goto IL_7B7;
							}
							else if (!(prefabID == "GravitasPedestal"))
							{
								goto IL_87F;
							}
						}
						else
						{
							if (!(prefabID == "WireRefined"))
							{
								goto IL_87F;
							}
							goto IL_837;
						}
					}
					else if (num != 674245745U)
					{
						if (num != 781890915U)
						{
							if (num != 1036100273U)
							{
								goto IL_87F;
							}
							if (!(prefabID == "WireRefinedBridgeHighWattage"))
							{
								goto IL_87F;
							}
							goto IL_837;
						}
						else
						{
							if (!(prefabID == "LogicGateNOT"))
							{
								goto IL_87F;
							}
							goto IL_84F;
						}
					}
					else
					{
						if (!(prefabID == "CraftingTable"))
						{
							goto IL_87F;
						}
						return "craftingstation";
					}
					return "sculpture";
				}
				if (num <= 1526604543U)
				{
					if (num <= 1232204109U)
					{
						if (num != 1038415088U)
						{
							if (num != 1089791339U)
							{
								if (num != 1232204109U)
								{
									goto IL_87F;
								}
								if (!(prefabID == "WireBridge"))
								{
									goto IL_87F;
								}
								goto IL_837;
							}
							else
							{
								if (!(prefabID == "Refrigerator"))
								{
									goto IL_87F;
								}
								return "refrigerator";
							}
						}
						else
						{
							if (!(prefabID == "LogicGateFILTER"))
							{
								goto IL_87F;
							}
							goto IL_84F;
						}
					}
					else if (num != 1269853127U)
					{
						if (num != 1398532937U)
						{
							if (num != 1526604543U)
							{
								goto IL_87F;
							}
							if (!(prefabID == "StorageLockerSmart"))
							{
								goto IL_87F;
							}
							return "storagelockersmart";
						}
						else
						{
							if (!(prefabID == "LogicGateMultiplexer"))
							{
								goto IL_87F;
							}
							goto IL_84F;
						}
					}
					else
					{
						if (!(prefabID == "AdvancedResearchCenter"))
						{
							goto IL_87F;
						}
						return "advancedresearchcenter";
					}
				}
				else if (num <= 1734850496U)
				{
					if (num != 1607642960U)
					{
						if (num != 1633134164U)
						{
							if (num != 1734850496U)
							{
								goto IL_87F;
							}
							if (!(prefabID == "RockCrusher"))
							{
								goto IL_87F;
							}
							return "rockrefinery";
						}
						else
						{
							if (!(prefabID == "CeilingLight"))
							{
								goto IL_87F;
							}
							goto IL_7FB;
						}
					}
					else
					{
						if (!(prefabID == "FlushToilet"))
						{
							goto IL_87F;
						}
						return "flushtoilate";
					}
				}
				else if (num != 1815117387U)
				{
					if (num != 1908704479U)
					{
						if (num != 1938276536U)
						{
							goto IL_87F;
						}
						if (!(prefabID == "Wire"))
						{
							goto IL_87F;
						}
						goto IL_837;
					}
					else
					{
						if (!(prefabID == "LogicGateAND"))
						{
							goto IL_87F;
						}
						goto IL_84F;
					}
				}
				else
				{
					if (!(prefabID == "LogicGateOR"))
					{
						goto IL_87F;
					}
					goto IL_84F;
				}
			}
			else if (num <= 3132083755U)
			{
				if (num <= 2406622476U)
				{
					if (num <= 2041738741U)
					{
						if (num != 1943253450U)
						{
							if (num != 2028863301U)
							{
								if (num != 2041738741U)
								{
									goto IL_87F;
								}
								if (!(prefabID == "CookingStation"))
								{
									goto IL_87F;
								}
								return "grill";
							}
							else if (!(prefabID == "FlowerVaseHanging"))
							{
								goto IL_87F;
							}
						}
						else
						{
							if (!(prefabID == "WaterCooler"))
							{
								goto IL_87F;
							}
							return "watercooler";
						}
					}
					else if (num != 2076384603U)
					{
						if (num != 2402859370U)
						{
							if (num != 2406622476U)
							{
								goto IL_87F;
							}
							if (!(prefabID == "WireBridgeHighWattage"))
							{
								goto IL_87F;
							}
							goto IL_837;
						}
						else
						{
							if (!(prefabID == "StorageLocker"))
							{
								goto IL_87F;
							}
							return "storagelocker";
						}
					}
					else
					{
						if (!(prefabID == "GasReservoir"))
						{
							goto IL_87F;
						}
						return "gasstorage";
					}
				}
				else if (num <= 2818521706U)
				{
					if (num != 2691468069U)
					{
						if (num != 2722382738U)
						{
							if (num != 2818521706U)
							{
								goto IL_87F;
							}
							if (!(prefabID == "GourmetCookingStation"))
							{
								goto IL_87F;
							}
							return "gasrange";
						}
						else
						{
							if (!(prefabID == "PlanterBox"))
							{
								goto IL_87F;
							}
							return "planterbox";
						}
					}
					else
					{
						if (!(prefabID == "ResearchCenter"))
						{
							goto IL_87F;
						}
						return "researchcenter";
					}
				}
				else if (num != 2899744071U)
				{
					if (num != 3048425356U)
					{
						if (num != 3132083755U)
						{
							goto IL_87F;
						}
						if (!(prefabID == "FlowerVaseWall"))
						{
							goto IL_87F;
						}
					}
					else
					{
						if (!(prefabID == "Bed"))
						{
							goto IL_87F;
						}
						return "bed";
					}
				}
				else
				{
					if (!(prefabID == "ExteriorWall"))
					{
						goto IL_87F;
					}
					return "wall";
				}
			}
			else if (num <= 3562718686U)
			{
				if (num <= 3371266309U)
				{
					if (num != 3228988836U)
					{
						if (num != 3347778080U)
						{
							if (num != 3371266309U)
							{
								goto IL_87F;
							}
							if (!(prefabID == "LogicRibbon"))
							{
								goto IL_87F;
							}
							goto IL_84F;
						}
						else
						{
							if (!(prefabID == "LogicGateBUFFER"))
							{
								goto IL_87F;
							}
							goto IL_84F;
						}
					}
					else
					{
						if (!(prefabID == "LogicWire"))
						{
							goto IL_87F;
						}
						goto IL_84F;
					}
				}
				else if (num != 3422134480U)
				{
					if (num != 3534553076U)
					{
						if (num != 3562718686U)
						{
							goto IL_87F;
						}
						if (!(prefabID == "Headquarters"))
						{
							goto IL_87F;
						}
						return "headquarters";
					}
					else
					{
						if (!(prefabID == "MassageTable"))
						{
							goto IL_87F;
						}
						return "massagetable";
					}
				}
				else
				{
					if (!(prefabID == "MicrobeMusher"))
					{
						goto IL_87F;
					}
					return "microbemusher";
				}
			}
			else if (num <= 3873680366U)
			{
				if (num != 3681463987U)
				{
					if (num != 3716494409U)
					{
						if (num != 3873680366U)
						{
							goto IL_87F;
						}
						if (!(prefabID == "WireRefinedBridge"))
						{
							goto IL_87F;
						}
						goto IL_837;
					}
					else
					{
						if (!(prefabID == "HighWattageWire"))
						{
							goto IL_87F;
						}
						goto IL_837;
					}
				}
				else
				{
					if (!(prefabID == "FloorLamp"))
					{
						goto IL_87F;
					}
					goto IL_7FB;
				}
			}
			else if (num <= 3958671086U)
			{
				if (num != 3903452895U)
				{
					if (num != 3958671086U)
					{
						goto IL_87F;
					}
					if (!(prefabID == "FlowerVaseHangingFancy"))
					{
						goto IL_87F;
					}
				}
				else
				{
					if (!(prefabID == "EggCracker"))
					{
						goto IL_87F;
					}
					return "eggcracker";
				}
			}
			else if (num != 4217645425U)
			{
				if (num != 4243975822U)
				{
					goto IL_87F;
				}
				if (!(prefabID == "WireRefinedHighWattage"))
				{
					goto IL_87F;
				}
				goto IL_837;
			}
			else
			{
				if (!(prefabID == "LogicWireBridge"))
				{
					goto IL_87F;
				}
				goto IL_84F;
			}
			IL_7B7:
			return "flowervase";
			IL_7FB:
			return "ceilingLight";
			IL_837:
			return "wire";
			IL_84F:
			return "logicwire";
		}
		IL_87F:
		if (permit.Category == PermitCategory.Artwork)
		{
			BuildingDef buildingDef2 = KleiPermitVisUtil.GetBuildingDef(permit);
			if (buildingDef2 == null)
			{
				return "HUD";
			}
			if (KleiInventoryScreen.<GetFacadeItemSoundName>g__Has|76_0<Sculpture>(buildingDef2))
			{
				string prefabID = buildingDef2.PrefabID;
				if (prefabID == "IceSculpture")
				{
					return "icesculpture";
				}
				if (!(prefabID == "WoodSculpture"))
				{
					return "sculpture";
				}
				return "woodsculpture";
			}
			else
			{
				if (KleiInventoryScreen.<GetFacadeItemSoundName>g__Has|76_0<Painting>(buildingDef2))
				{
					return "painting";
				}
				if (KleiInventoryScreen.<GetFacadeItemSoundName>g__Has|76_0<MonumentPart>(buildingDef2))
				{
					return "HUD";
				}
			}
		}
		if (permit.Category == PermitCategory.JoyResponse && permit is BalloonArtistFacadeResource)
		{
			return "balloon";
		}
		return "HUD";
	}

	// Token: 0x06009E1A RID: 40474 RVA: 0x0010AC6E File Offset: 0x00108E6E
	private void OnMouseOverToggle()
	{
		KFMOD.PlayUISound(GlobalAssets.GetSound("HUD_Mouseover", false));
	}

	// Token: 0x06009E22 RID: 40482 RVA: 0x0010B5BE File Offset: 0x001097BE
	[CompilerGenerated]
	internal static bool <GetFacadeItemSoundName>g__Has|76_0<T>(BuildingDef buildingDef) where T : Component
	{
		return !buildingDef.BuildingComplete.GetComponent<T>().IsNullOrDestroyed();
	}

	// Token: 0x04007C0A RID: 31754
	[Header("Header")]
	[SerializeField]
	private KButton closeButton;

	// Token: 0x04007C0B RID: 31755
	[Header("CategoryColumn")]
	[SerializeField]
	private RectTransform categoryListContent;

	// Token: 0x04007C0C RID: 31756
	[SerializeField]
	private GameObject categoryRowPrefab;

	// Token: 0x04007C0D RID: 31757
	private Dictionary<string, MultiToggle> categoryToggles = new Dictionary<string, MultiToggle>();

	// Token: 0x04007C0E RID: 31758
	[Header("ItemGalleryColumn")]
	[SerializeField]
	private LocText galleryHeaderLabel;

	// Token: 0x04007C0F RID: 31759
	[SerializeField]
	private RectTransform galleryGridContent;

	// Token: 0x04007C10 RID: 31760
	[SerializeField]
	private GameObject gridItemPrefab;

	// Token: 0x04007C11 RID: 31761
	[SerializeField]
	private GameObject subcategoryPrefab;

	// Token: 0x04007C12 RID: 31762
	[SerializeField]
	private GameObject itemDummyPrefab;

	// Token: 0x04007C13 RID: 31763
	[Header("GalleryFilters")]
	[SerializeField]
	private KInputTextField searchField;

	// Token: 0x04007C14 RID: 31764
	[SerializeField]
	private KButton clearSearchButton;

	// Token: 0x04007C15 RID: 31765
	[SerializeField]
	private MultiToggle doublesOnlyToggle;

	// Token: 0x04007C16 RID: 31766
	public const int FILTER_SHOW_ALL = 0;

	// Token: 0x04007C17 RID: 31767
	public const int FILTER_SHOW_OWNED_ONLY = 1;

	// Token: 0x04007C18 RID: 31768
	public const int FILTER_SHOW_DOUBLES_ONLY = 2;

	// Token: 0x04007C19 RID: 31769
	private int showFilterState;

	// Token: 0x04007C1A RID: 31770
	[Header("BarterSection")]
	[SerializeField]
	private Image barterPanelBG;

	// Token: 0x04007C1B RID: 31771
	[SerializeField]
	private KButton barterBuyButton;

	// Token: 0x04007C1C RID: 31772
	[SerializeField]
	private KButton barterSellButton;

	// Token: 0x04007C1D RID: 31773
	[SerializeField]
	private GameObject barterConfirmationScreenPrefab;

	// Token: 0x04007C1E RID: 31774
	[SerializeField]
	private GameObject filamentWalletSection;

	// Token: 0x04007C1F RID: 31775
	[SerializeField]
	private GameObject barterOfflineLabel;

	// Token: 0x04007C20 RID: 31776
	private Dictionary<PermitResource, MultiToggle> galleryGridButtons = new Dictionary<PermitResource, MultiToggle>();

	// Token: 0x04007C21 RID: 31777
	private List<KleiInventoryUISubcategory> subcategories = new List<KleiInventoryUISubcategory>();

	// Token: 0x04007C22 RID: 31778
	private List<GameObject> recycledGalleryGridButtons = new List<GameObject>();

	// Token: 0x04007C23 RID: 31779
	private GridLayouter galleryGridLayouter;

	// Token: 0x04007C24 RID: 31780
	[Header("SelectionDetailsColumn")]
	[SerializeField]
	private LocText selectionHeaderLabel;

	// Token: 0x04007C25 RID: 31781
	[SerializeField]
	private KleiPermitDioramaVis permitVis;

	// Token: 0x04007C26 RID: 31782
	[SerializeField]
	private KScrollRect selectionDetailsScrollRect;

	// Token: 0x04007C27 RID: 31783
	[SerializeField]
	private RectTransform selectionDetailsScrollRectScrollBarContainer;

	// Token: 0x04007C28 RID: 31784
	[SerializeField]
	private LocText selectionNameLabel;

	// Token: 0x04007C29 RID: 31785
	[SerializeField]
	private LocText selectionDescriptionLabel;

	// Token: 0x04007C2A RID: 31786
	[SerializeField]
	private LocText selectionFacadeForLabel;

	// Token: 0x04007C2B RID: 31787
	[SerializeField]
	private LocText selectionCollectionLabel;

	// Token: 0x04007C2C RID: 31788
	[SerializeField]
	private LocText selectionRarityDetailsLabel;

	// Token: 0x04007C2D RID: 31789
	[SerializeField]
	private LocText selectionOwnedCount;

	// Token: 0x04007C2F RID: 31791
	private bool IS_ONLINE;

	// Token: 0x04007C30 RID: 31792
	private bool initConfigComplete;

	// Token: 0x02001D8F RID: 7567
	private enum PermitPrintabilityState
	{
		// Token: 0x04007C33 RID: 31795
		Printable,
		// Token: 0x04007C34 RID: 31796
		AlreadyOwned,
		// Token: 0x04007C35 RID: 31797
		TooExpensive,
		// Token: 0x04007C36 RID: 31798
		NotForSale,
		// Token: 0x04007C37 RID: 31799
		NotForSaleYet,
		// Token: 0x04007C38 RID: 31800
		UserOffline
	}

	// Token: 0x02001D90 RID: 7568
	private enum MultiToggleState
	{
		// Token: 0x04007C3A RID: 31802
		Default,
		// Token: 0x04007C3B RID: 31803
		Selected,
		// Token: 0x04007C3C RID: 31804
		NonInteractable
	}
}
