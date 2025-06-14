﻿using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Database;
using STRINGS;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class KleiInventoryScreen : KModalScreen
{
	private PermitResource SelectedPermit { get; set; }

	private string SelectedCategoryId { get; set; }

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

	public override void OnKeyDown(KButtonEvent e)
	{
		if (e.TryConsume(global::Action.Escape) || e.TryConsume(global::Action.MouseRight))
		{
			this.Show(false);
		}
		base.OnKeyDown(e);
	}

	public override float GetSortKey()
	{
		return 20f;
	}

	protected override void OnActivate()
	{
		this.OnShow(true);
	}

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

	private void ClearSearch()
	{
		this.searchField.text = "";
		this.searchField.placeholder.GetComponent<TextMeshProUGUI>().text = UI.KLEI_INVENTORY_SCREEN.SEARCH_PLACEHOLDER;
		this.RefreshGallery();
	}

	private void Update()
	{
		this.galleryGridLayouter.CheckIfShouldResizeGrid();
	}

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

	private void RecycleGalleryGridButton(GameObject button)
	{
		button.GetComponent<MultiToggle>().onClick = null;
		this.recycledGalleryGridButtons.Add(button);
	}

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

	private void CloseSubcategory(string subcategoryID)
	{
		KleiInventoryUISubcategory kleiInventoryUISubcategory = this.subcategories.Find((KleiInventoryUISubcategory match) => match.subcategoryID == subcategoryID);
		if (kleiInventoryUISubcategory != null)
		{
			kleiInventoryUISubcategory.ToggleOpen(false);
		}
	}

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

	public void SelectItem(PermitResource permit)
	{
		this.SelectedPermit = permit;
		this.RefreshGallery();
		this.RefreshDetails();
		this.RefreshBarterPanel();
	}

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

	private void OnMouseOverToggle()
	{
		KFMOD.PlayUISound(GlobalAssets.GetSound("HUD_Mouseover", false));
	}

	[CompilerGenerated]
	internal static bool <GetFacadeItemSoundName>g__Has|76_0<T>(BuildingDef buildingDef) where T : Component
	{
		return !buildingDef.BuildingComplete.GetComponent<T>().IsNullOrDestroyed();
	}

	[Header("Header")]
	[SerializeField]
	private KButton closeButton;

	[Header("CategoryColumn")]
	[SerializeField]
	private RectTransform categoryListContent;

	[SerializeField]
	private GameObject categoryRowPrefab;

	private Dictionary<string, MultiToggle> categoryToggles = new Dictionary<string, MultiToggle>();

	[Header("ItemGalleryColumn")]
	[SerializeField]
	private LocText galleryHeaderLabel;

	[SerializeField]
	private RectTransform galleryGridContent;

	[SerializeField]
	private GameObject gridItemPrefab;

	[SerializeField]
	private GameObject subcategoryPrefab;

	[SerializeField]
	private GameObject itemDummyPrefab;

	[Header("GalleryFilters")]
	[SerializeField]
	private KInputTextField searchField;

	[SerializeField]
	private KButton clearSearchButton;

	[SerializeField]
	private MultiToggle doublesOnlyToggle;

	public const int FILTER_SHOW_ALL = 0;

	public const int FILTER_SHOW_OWNED_ONLY = 1;

	public const int FILTER_SHOW_DOUBLES_ONLY = 2;

	private int showFilterState;

	[Header("BarterSection")]
	[SerializeField]
	private Image barterPanelBG;

	[SerializeField]
	private KButton barterBuyButton;

	[SerializeField]
	private KButton barterSellButton;

	[SerializeField]
	private GameObject barterConfirmationScreenPrefab;

	[SerializeField]
	private GameObject filamentWalletSection;

	[SerializeField]
	private GameObject barterOfflineLabel;

	private Dictionary<PermitResource, MultiToggle> galleryGridButtons = new Dictionary<PermitResource, MultiToggle>();

	private List<KleiInventoryUISubcategory> subcategories = new List<KleiInventoryUISubcategory>();

	private List<GameObject> recycledGalleryGridButtons = new List<GameObject>();

	private GridLayouter galleryGridLayouter;

	[Header("SelectionDetailsColumn")]
	[SerializeField]
	private LocText selectionHeaderLabel;

	[SerializeField]
	private KleiPermitDioramaVis permitVis;

	[SerializeField]
	private KScrollRect selectionDetailsScrollRect;

	[SerializeField]
	private RectTransform selectionDetailsScrollRectScrollBarContainer;

	[SerializeField]
	private LocText selectionNameLabel;

	[SerializeField]
	private LocText selectionDescriptionLabel;

	[SerializeField]
	private LocText selectionFacadeForLabel;

	[SerializeField]
	private LocText selectionCollectionLabel;

	[SerializeField]
	private LocText selectionRarityDetailsLabel;

	[SerializeField]
	private LocText selectionOwnedCount;

	private bool IS_ONLINE;

	private bool initConfigComplete;

	private enum PermitPrintabilityState
	{
		Printable,
		AlreadyOwned,
		TooExpensive,
		NotForSale,
		NotForSaleYet,
		UserOffline
	}

	private enum MultiToggleState
	{
		Default,
		Selected,
		NonInteractable
	}
}
