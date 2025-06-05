using System;
using System.Collections.Generic;
using System.Linq;
using Database;
using STRINGS;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x02001ED5 RID: 7893
public class OutfitDesignerScreen : KMonoBehaviour
{
	// Token: 0x17000A9C RID: 2716
	// (get) Token: 0x0600A598 RID: 42392 RVA: 0x0010FE22 File Offset: 0x0010E022
	// (set) Token: 0x0600A599 RID: 42393 RVA: 0x0010FE2A File Offset: 0x0010E02A
	public OutfitDesignerScreenConfig Config { get; private set; }

	// Token: 0x17000A9D RID: 2717
	// (get) Token: 0x0600A59A RID: 42394 RVA: 0x0010FE33 File Offset: 0x0010E033
	// (set) Token: 0x0600A59B RID: 42395 RVA: 0x0010FE3B File Offset: 0x0010E03B
	public PermitResource SelectedPermit { get; private set; }

	// Token: 0x17000A9E RID: 2718
	// (get) Token: 0x0600A59C RID: 42396 RVA: 0x0010FE44 File Offset: 0x0010E044
	// (set) Token: 0x0600A59D RID: 42397 RVA: 0x0010FE4C File Offset: 0x0010E04C
	public PermitCategory SelectedCategory { get; private set; }

	// Token: 0x17000A9F RID: 2719
	// (get) Token: 0x0600A59E RID: 42398 RVA: 0x0010FE55 File Offset: 0x0010E055
	// (set) Token: 0x0600A59F RID: 42399 RVA: 0x0010FE5D File Offset: 0x0010E05D
	public OutfitDesignerScreen_OutfitState outfitState { get; private set; }

	// Token: 0x0600A5A0 RID: 42400 RVA: 0x003FA5E8 File Offset: 0x003F87E8
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		global::Debug.Assert(this.categoryRowPrefab.transform.parent == this.categoryListContent.transform);
		global::Debug.Assert(this.gridItemPrefab.transform.parent == this.galleryGridContent.transform);
		global::Debug.Assert(this.subcategoryUiPrefab.transform.parent == this.galleryGridContent.transform);
		this.categoryRowPrefab.SetActive(false);
		this.gridItemPrefab.SetActive(false);
		this.galleryGridLayouter = new GridLayouter
		{
			minCellSize = 64f,
			maxCellSize = 96f,
			targetGridLayouts = this.galleryGridContent.GetComponents<GridLayoutGroup>().ToList<GridLayoutGroup>()
		};
		this.galleryGridLayouter.overrideParentForSizeReference = this.galleryGridContent;
		this.categoryRowPool = new UIPrefabLocalPool(this.categoryRowPrefab, this.categoryListContent.gameObject);
		this.galleryGridItemPool = new UIPrefabLocalPool(this.gridItemPrefab, this.galleryGridContent.gameObject);
		this.subcategoryUiPool = new UIPrefabLocalPool(this.subcategoryUiPrefab, this.galleryGridContent.gameObject);
		if (OutfitDesignerScreen.outfitTypeToCategoriesDict == null)
		{
			Dictionary<ClothingOutfitUtility.OutfitType, PermitCategory[]> dictionary = new Dictionary<ClothingOutfitUtility.OutfitType, PermitCategory[]>();
			dictionary[ClothingOutfitUtility.OutfitType.Clothing] = ClothingOutfitUtility.PERMIT_CATEGORIES_FOR_CLOTHING;
			dictionary[ClothingOutfitUtility.OutfitType.AtmoSuit] = ClothingOutfitUtility.PERMIT_CATEGORIES_FOR_ATMO_SUITS;
			OutfitDesignerScreen.outfitTypeToCategoriesDict = dictionary;
		}
		InventoryOrganization.Initialize();
	}

	// Token: 0x0600A5A1 RID: 42401 RVA: 0x0010FE66 File Offset: 0x0010E066
	private void Update()
	{
		this.galleryGridLayouter.CheckIfShouldResizeGrid();
	}

	// Token: 0x0600A5A2 RID: 42402 RVA: 0x0010FE73 File Offset: 0x0010E073
	protected override void OnSpawn()
	{
		this.postponeConfiguration = false;
		this.minionOrMannequin.TrySpawn();
		if (!this.Config.isValid)
		{
			throw new NotSupportedException("Cannot open OutfitDesignerScreen without a config. Make sure to call Configure() before enabling the screen");
		}
		this.Configure(this.Config);
	}

	// Token: 0x0600A5A3 RID: 42403 RVA: 0x0010FEAC File Offset: 0x0010E0AC
	protected override void OnCmpEnable()
	{
		base.OnCmpEnable();
		KleiItemsStatusRefresher.AddOrGetListener(this).OnRefreshUI(delegate
		{
			this.RefreshCategories();
			this.RefreshGallery();
			this.RefreshOutfitState();
		});
	}

	// Token: 0x0600A5A4 RID: 42404 RVA: 0x0010FECB File Offset: 0x0010E0CB
	protected override void OnCmpDisable()
	{
		base.OnCmpDisable();
		this.UnregisterPreventScreenPop();
	}

	// Token: 0x0600A5A5 RID: 42405 RVA: 0x0010FED9 File Offset: 0x0010E0D9
	private void UpdateSaveButtons()
	{
		if (this.updateSaveButtonsFn != null)
		{
			this.updateSaveButtonsFn();
		}
	}

	// Token: 0x0600A5A6 RID: 42406 RVA: 0x003FA74C File Offset: 0x003F894C
	public void Configure(OutfitDesignerScreenConfig config)
	{
		this.Config = config;
		if (config.targetMinionInstance.HasValue)
		{
			this.outfitState = OutfitDesignerScreen_OutfitState.ForMinionInstance(this.Config.sourceTarget, config.targetMinionInstance.Value);
		}
		else
		{
			this.outfitState = OutfitDesignerScreen_OutfitState.ForTemplateOutfit(this.Config.sourceTarget);
		}
		if (this.postponeConfiguration)
		{
			return;
		}
		this.RegisterPreventScreenPop();
		this.minionOrMannequin.SetFrom(config.minionPersonality).SpawnedAvatar.GetComponent<WearableAccessorizer>();
		using (ListPool<ClothingItemResource, OutfitDesignerScreen>.PooledList pooledList = PoolsFor<OutfitDesignerScreen>.AllocateList<ClothingItemResource>())
		{
			this.outfitState.AddItemValuesTo(pooledList);
			this.minionOrMannequin.SetFrom(config.minionPersonality).SetOutfit(config.sourceTarget.OutfitType, pooledList);
		}
		this.PopulateCategories();
		this.SelectCategory(OutfitDesignerScreen.outfitTypeToCategoriesDict[this.outfitState.outfitType][0]);
		this.galleryGridLayouter.RequestGridResize();
		this.RefreshOutfitState();
		OutfitDesignerScreenConfig config2 = this.Config;
		if (config2.targetMinionInstance.HasValue)
		{
			this.updateSaveButtonsFn = null;
			this.primaryButton.ClearOnClick();
			TMP_Text componentInChildren = this.primaryButton.GetComponentInChildren<LocText>();
			LocString button_APPLY_TO_MINION = UI.OUTFIT_DESIGNER_SCREEN.MINION_INSTANCE.BUTTON_APPLY_TO_MINION;
			string search = "{MinionName}";
			config2 = this.Config;
			componentInChildren.SetText(button_APPLY_TO_MINION.Replace(search, config2.targetMinionInstance.Value.GetProperName()));
			this.primaryButton.onClick += delegate()
			{
				OutfitDesignerScreenConfig config3 = this.Config;
				ClothingOutfitUtility.OutfitType outfitType = config3.sourceTarget.OutfitType;
				config3 = this.Config;
				ClothingOutfitTarget obj = ClothingOutfitTarget.FromMinion(outfitType, config3.targetMinionInstance.Value);
				config3 = this.Config;
				obj.WriteItems(config3.sourceTarget.OutfitType, this.outfitState.GetItems());
				if (this.Config.onWriteToOutfitTargetFn != null)
				{
					this.Config.onWriteToOutfitTargetFn(obj);
				}
				LockerNavigator.Instance.PopScreen();
			};
			this.secondaryButton.ClearOnClick();
			this.secondaryButton.GetComponentInChildren<LocText>().SetText(UI.OUTFIT_DESIGNER_SCREEN.MINION_INSTANCE.BUTTON_APPLY_TO_TEMPLATE);
			this.secondaryButton.onClick += delegate()
			{
				OutfitDesignerScreen.MakeApplyToTemplatePopup(this.inputFieldPrefab, this.outfitState, this.Config.targetMinionInstance.Value, this.Config.outfitTemplate, this.Config.onWriteToOutfitTargetFn);
			};
			this.updateSaveButtonsFn = (System.Action)Delegate.Combine(this.updateSaveButtonsFn, new System.Action(delegate()
			{
				if (this.outfitState.DoesContainLockedItems())
				{
					this.primaryButton.isInteractable = false;
					this.primaryButton.gameObject.AddOrGet<ToolTip>().SetSimpleTooltip(UI.OUTFIT_DESIGNER_SCREEN.OUTFIT_TEMPLATE.TOOLTIP_SAVE_ERROR_LOCKED);
					this.secondaryButton.isInteractable = false;
					this.secondaryButton.gameObject.AddOrGet<ToolTip>().SetSimpleTooltip(UI.OUTFIT_DESIGNER_SCREEN.OUTFIT_TEMPLATE.TOOLTIP_SAVE_ERROR_LOCKED);
					return;
				}
				this.primaryButton.isInteractable = true;
				this.primaryButton.gameObject.AddOrGet<ToolTip>().ClearMultiStringTooltip();
				this.secondaryButton.isInteractable = true;
				this.secondaryButton.gameObject.AddOrGet<ToolTip>().ClearMultiStringTooltip();
			}));
		}
		else
		{
			config2 = this.Config;
			if (!config2.outfitTemplate.HasValue)
			{
				throw new NotSupportedException();
			}
			this.updateSaveButtonsFn = null;
			this.primaryButton.ClearOnClick();
			this.primaryButton.GetComponentInChildren<LocText>().SetText(UI.OUTFIT_DESIGNER_SCREEN.OUTFIT_TEMPLATE.BUTTON_SAVE);
			this.primaryButton.onClick += delegate()
			{
				this.outfitState.destinationTarget.WriteName(this.outfitState.name);
				this.outfitState.destinationTarget.WriteItems(this.outfitState.outfitType, this.outfitState.GetItems());
				OutfitDesignerScreenConfig config3 = this.Config;
				if (config3.minionPersonality.HasValue)
				{
					config3 = this.Config;
					config3.minionPersonality.Value.SetSelectedTemplateOutfitId(this.outfitState.destinationTarget.OutfitType, this.outfitState.destinationTarget.OutfitId);
				}
				if (this.Config.onWriteToOutfitTargetFn != null)
				{
					this.Config.onWriteToOutfitTargetFn(this.outfitState.destinationTarget);
				}
				LockerNavigator.Instance.PopScreen();
			};
			this.secondaryButton.ClearOnClick();
			this.secondaryButton.GetComponentInChildren<LocText>().SetText(UI.OUTFIT_DESIGNER_SCREEN.OUTFIT_TEMPLATE.BUTTON_COPY);
			this.secondaryButton.onClick += delegate()
			{
				OutfitDesignerScreen.MakeCopyPopup(this, this.inputFieldPrefab, this.outfitState, this.Config.outfitTemplate.Value, this.Config.minionPersonality, this.Config.onWriteToOutfitTargetFn);
			};
			this.updateSaveButtonsFn = (System.Action)Delegate.Combine(this.updateSaveButtonsFn, new System.Action(delegate()
			{
				if (!this.outfitState.destinationTarget.CanWriteItems)
				{
					this.primaryButton.isInteractable = false;
					this.primaryButton.gameObject.AddOrGet<ToolTip>().SetSimpleTooltip(UI.OUTFIT_DESIGNER_SCREEN.OUTFIT_TEMPLATE.TOOLTIP_SAVE_ERROR_READONLY);
					if (this.outfitState.DoesContainLockedItems())
					{
						this.secondaryButton.isInteractable = false;
						this.secondaryButton.gameObject.AddOrGet<ToolTip>().SetSimpleTooltip(UI.OUTFIT_DESIGNER_SCREEN.OUTFIT_TEMPLATE.TOOLTIP_SAVE_ERROR_LOCKED);
						return;
					}
					this.secondaryButton.isInteractable = true;
					this.secondaryButton.gameObject.AddOrGet<ToolTip>().ClearMultiStringTooltip();
					return;
				}
				else
				{
					if (this.outfitState.DoesContainLockedItems())
					{
						this.primaryButton.isInteractable = false;
						this.primaryButton.gameObject.AddOrGet<ToolTip>().SetSimpleTooltip(UI.OUTFIT_DESIGNER_SCREEN.OUTFIT_TEMPLATE.TOOLTIP_SAVE_ERROR_LOCKED);
						this.secondaryButton.isInteractable = false;
						this.secondaryButton.gameObject.AddOrGet<ToolTip>().SetSimpleTooltip(UI.OUTFIT_DESIGNER_SCREEN.OUTFIT_TEMPLATE.TOOLTIP_SAVE_ERROR_LOCKED);
						return;
					}
					this.primaryButton.isInteractable = true;
					this.primaryButton.gameObject.AddOrGet<ToolTip>().ClearMultiStringTooltip();
					this.secondaryButton.isInteractable = true;
					this.secondaryButton.gameObject.AddOrGet<ToolTip>().ClearMultiStringTooltip();
					return;
				}
			}));
		}
		this.UpdateSaveButtons();
	}

	// Token: 0x0600A5A7 RID: 42407 RVA: 0x0010FEEE File Offset: 0x0010E0EE
	private void RefreshOutfitState()
	{
		this.selectionHeaderLabel.text = this.outfitState.name;
		this.outfitDescriptionPanel.Refresh(this.outfitState, this.Config.minionPersonality);
		this.UpdateSaveButtons();
	}

	// Token: 0x0600A5A8 RID: 42408 RVA: 0x0010FF28 File Offset: 0x0010E128
	private void RefreshCategories()
	{
		if (this.RefreshCategoriesFn != null)
		{
			this.RefreshCategoriesFn();
		}
	}

	// Token: 0x0600A5A9 RID: 42409 RVA: 0x003FA9FC File Offset: 0x003F8BFC
	public void PopulateCategories()
	{
		this.RefreshCategoriesFn = null;
		this.categoryRowPool.ReturnAll();
		PermitCategory[] array = OutfitDesignerScreen.outfitTypeToCategoriesDict[this.outfitState.outfitType];
		for (int i = 0; i < array.Length; i++)
		{
			OutfitDesignerScreen.<>c__DisplayClass47_0 CS$<>8__locals1 = new OutfitDesignerScreen.<>c__DisplayClass47_0();
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.permitCategory = array[i];
			GameObject gameObject = this.categoryRowPool.Borrow();
			HierarchyReferences component = gameObject.GetComponent<HierarchyReferences>();
			component.GetReference<LocText>("Label").SetText(PermitCategories.GetUppercaseDisplayName(CS$<>8__locals1.permitCategory));
			component.GetReference<Image>("Icon").sprite = Assets.GetSprite(PermitCategories.GetIconName(CS$<>8__locals1.permitCategory));
			MultiToggle toggle = gameObject.GetComponent<MultiToggle>();
			MultiToggle toggle2 = toggle;
			toggle2.onEnter = (System.Action)Delegate.Combine(toggle2.onEnter, new System.Action(this.OnMouseOverToggle));
			toggle.onClick = delegate()
			{
				CS$<>8__locals1.<>4__this.SelectCategory(CS$<>8__locals1.permitCategory);
			};
			this.RefreshCategoriesFn = (System.Action)Delegate.Combine(this.RefreshCategoriesFn, new System.Action(delegate()
			{
				toggle.ChangeState((CS$<>8__locals1.permitCategory == CS$<>8__locals1.<>4__this.SelectedCategory) ? 1 : 0);
			}));
			this.SetCatogoryClickUISound(CS$<>8__locals1.permitCategory, toggle);
		}
	}

	// Token: 0x0600A5AA RID: 42410 RVA: 0x003FAB50 File Offset: 0x003F8D50
	public void SelectCategory(PermitCategory permitCategory)
	{
		this.SelectedCategory = permitCategory;
		this.galleryHeaderLabel.text = PermitCategories.GetDisplayName(permitCategory);
		this.RefreshCategories();
		this.PopulateGallery();
		Option<ClothingItemResource> itemForCategory = this.outfitState.GetItemForCategory(permitCategory);
		if (itemForCategory.HasValue)
		{
			this.SelectPermit(itemForCategory.Value);
			return;
		}
		this.SelectPermit(null);
	}

	// Token: 0x0600A5AB RID: 42411 RVA: 0x0010FF3D File Offset: 0x0010E13D
	private void RefreshGallery()
	{
		if (this.RefreshGalleryFn != null)
		{
			this.RefreshGalleryFn();
		}
	}

	// Token: 0x0600A5AC RID: 42412 RVA: 0x003FABAC File Offset: 0x003F8DAC
	public void PopulateGallery()
	{
		OutfitDesignerScreen.<>c__DisplayClass51_0 CS$<>8__locals1 = new OutfitDesignerScreen.<>c__DisplayClass51_0();
		CS$<>8__locals1.<>4__this = this;
		this.RefreshGalleryFn = null;
		this.galleryGridItemPool.ReturnAll();
		this.subcategoryUiPool.ReturnAll();
		this.galleryGridLayouter.targetGridLayouts.Clear();
		this.galleryGridLayouter.OnSizeGridComplete = null;
		CS$<>8__locals1.onFirstDisplayCategoryDecided = new Promise<KleiInventoryUISubcategory>();
		CS$<>8__locals1.<PopulateGallery>g__AddGridIconForPermit|0(null);
		foreach (ClothingItemResource clothingItemResource in Db.Get().Permits.ClothingItems.resources)
		{
			if (clothingItemResource.Category == this.SelectedCategory && clothingItemResource.outfitType == this.Config.sourceTarget.OutfitType && !clothingItemResource.Id.StartsWith("visonly_"))
			{
				CS$<>8__locals1.<PopulateGallery>g__AddGridIconForPermit|0(clothingItemResource);
			}
		}
		foreach (GameObject gameObject3 in this.subcategoryUiPool.GetBorrowedObjects().StableSort(Comparer<GameObject>.Create(delegate(GameObject a, GameObject b)
		{
			KleiInventoryUISubcategory component = a.GetComponent<KleiInventoryUISubcategory>();
			KleiInventoryUISubcategory component2 = b.GetComponent<KleiInventoryUISubcategory>();
			int sortKey = InventoryOrganization.subcategoryIdToPresentationDataMap[component.subcategoryID].sortKey;
			int sortKey2 = InventoryOrganization.subcategoryIdToPresentationDataMap[component2.subcategoryID].sortKey;
			return sortKey.CompareTo(sortKey2);
		})))
		{
			gameObject3.transform.SetAsLastSibling();
		}
		GameObject gameObject2 = this.subcategoryUiPool.GetBorrowedObjects().FirstOrDefault((GameObject gameObject) => gameObject.GetComponent<KleiInventoryUISubcategory>().IsOpen);
		if (gameObject2 != null)
		{
			CS$<>8__locals1.onFirstDisplayCategoryDecided.Resolve(gameObject2.GetComponent<KleiInventoryUISubcategory>());
		}
		this.galleryGridLayouter.RequestGridResize();
		this.RefreshGallery();
	}

	// Token: 0x0600A5AD RID: 42413 RVA: 0x0010FF52 File Offset: 0x0010E152
	public void SelectPermit(PermitResource permit)
	{
		this.SelectedPermit = permit;
		this.RefreshGallery();
		this.UpdateSelectedItemDetails();
		this.UpdateSaveButtons();
	}

	// Token: 0x0600A5AE RID: 42414 RVA: 0x003FAD70 File Offset: 0x003F8F70
	public void UpdateSelectedItemDetails()
	{
		Option<ClothingItemResource> item = Option.None;
		if (this.SelectedPermit != null)
		{
			ClothingItemResource clothingItemResource = this.SelectedPermit as ClothingItemResource;
			if (clothingItemResource != null)
			{
				item = clothingItemResource;
			}
		}
		this.outfitState.SetItemForCategory(this.SelectedCategory, item);
		this.minionOrMannequin.current.SetOutfit(this.outfitState);
		this.minionOrMannequin.current.ReactToClothingItemChange(this.SelectedCategory);
		this.outfitDescriptionPanel.Refresh(this.outfitState, this.Config.minionPersonality);
		this.dioramaBG.sprite = KleiPermitDioramaVis.GetDioramaBackground(this.SelectedCategory);
	}

	// Token: 0x0600A5AF RID: 42415 RVA: 0x0010FF6D File Offset: 0x0010E16D
	private void RegisterPreventScreenPop()
	{
		this.UnregisterPreventScreenPop();
		this.preventScreenPopFn = delegate()
		{
			if (this.outfitState.IsDirty())
			{
				this.RegisterPreventScreenPop();
				OutfitDesignerScreen.MakeSaveWarningPopup(this.outfitState, delegate
				{
					this.UnregisterPreventScreenPop();
					LockerNavigator.Instance.PopScreen();
				});
				return true;
			}
			return false;
		};
		LockerNavigator.Instance.preventScreenPop.Add(this.preventScreenPopFn);
	}

	// Token: 0x0600A5B0 RID: 42416 RVA: 0x0010FF9C File Offset: 0x0010E19C
	private void UnregisterPreventScreenPop()
	{
		if (this.preventScreenPopFn != null)
		{
			LockerNavigator.Instance.preventScreenPop.Remove(this.preventScreenPopFn);
			this.preventScreenPopFn = null;
		}
	}

	// Token: 0x0600A5B1 RID: 42417 RVA: 0x003FAE18 File Offset: 0x003F9018
	public static void MakeSaveWarningPopup(OutfitDesignerScreen_OutfitState outfitState, System.Action discardChangesFn)
	{
		Action<InfoDialogScreen> <>9__1;
		LockerNavigator.Instance.ShowDialogPopup(delegate(InfoDialogScreen dialog)
		{
			InfoDialogScreen infoDialogScreen = dialog.SetHeader(UI.OUTFIT_DESIGNER_SCREEN.CHANGES_NOT_SAVED_WARNING_POPUP.HEADER.Replace("{OutfitName}", outfitState.name)).AddPlainText(UI.OUTFIT_DESIGNER_SCREEN.CHANGES_NOT_SAVED_WARNING_POPUP.BODY);
			string text = UI.OUTFIT_DESIGNER_SCREEN.CHANGES_NOT_SAVED_WARNING_POPUP.BUTTON_DISCARD;
			Action<InfoDialogScreen> action;
			if ((action = <>9__1) == null)
			{
				action = (<>9__1 = delegate(InfoDialogScreen d)
				{
					d.Deactivate();
					discardChangesFn();
				});
			}
			infoDialogScreen.AddOption(text, action, true).AddOption(UI.OUTFIT_DESIGNER_SCREEN.CHANGES_NOT_SAVED_WARNING_POPUP.BUTTON_RETURN, delegate(InfoDialogScreen d)
			{
				d.Deactivate();
			}, false);
		});
	}

	// Token: 0x0600A5B2 RID: 42418 RVA: 0x003FAE50 File Offset: 0x003F9050
	public static void MakeApplyToTemplatePopup(KInputTextField inputFieldPrefab, OutfitDesignerScreen_OutfitState outfitState, GameObject targetMinionInstance, Option<ClothingOutfitTarget> existingOutfitTemplate, Action<ClothingOutfitTarget> onWriteToOutfitTargetFn)
	{
		ClothingOutfitNameProposal proposal = default(ClothingOutfitNameProposal);
		Color errorTextColor = Util.ColorFromHex("F44A47");
		Color defaultTextColor = Util.ColorFromHex("FFFFFF");
		KInputTextField inputField;
		InfoScreenPlainText descLabel;
		KButton saveButton;
		LocText saveButtonText;
		LocText descLocText;
		LockerNavigator.Instance.ShowDialogPopup(delegate(InfoDialogScreen dialog)
		{
			dialog.SetHeader(UI.OUTFIT_DESIGNER_SCREEN.MINION_INSTANCE.APPLY_TEMPLATE_POPUP.HEADER.Replace("{OutfitName}", outfitState.name)).AddUI<KInputTextField>(inputFieldPrefab, out inputField).AddSpacer(8f).AddUI<InfoScreenPlainText>(dialog.GetPlainTextPrefab(), out descLabel).AddOption(true, out saveButton, out saveButtonText).AddDefaultCancel();
			descLocText = descLabel.gameObject.GetComponent<LocText>();
			descLocText.allowOverride = true;
			descLocText.alignment = TextAlignmentOptions.BottomLeft;
			descLocText.color = errorTextColor;
			descLocText.fontSize = 14f;
			descLabel.SetText("");
			inputField.onValueChanged.AddListener(new UnityAction<string>(base.<MakeApplyToTemplatePopup>g__Refresh|1));
			saveButton.onClick += delegate()
			{
				ClothingOutfitTarget clothingOutfitTarget = ClothingOutfitTarget.FromMinion(outfitState.outfitType, targetMinionInstance);
				ClothingOutfitNameProposal.Result result = proposal.result;
				ClothingOutfitTarget obj;
				if (result != ClothingOutfitNameProposal.Result.NewOutfit)
				{
					if (result != ClothingOutfitNameProposal.Result.SameOutfit)
					{
						throw new NotSupportedException(string.Format("Can't save outfit with name \"{0}\", failed with result: {1}", proposal.candidateName, proposal.result));
					}
					obj = existingOutfitTemplate.Value;
				}
				else
				{
					obj = ClothingOutfitTarget.ForNewTemplateOutfit(outfitState.outfitType, proposal.candidateName);
				}
				obj.WriteItems(outfitState.outfitType, outfitState.GetItems());
				clothingOutfitTarget.WriteItems(outfitState.outfitType, outfitState.GetItems());
				if (onWriteToOutfitTargetFn != null)
				{
					onWriteToOutfitTargetFn(obj);
				}
				dialog.Deactivate();
				LockerNavigator.Instance.PopScreen();
			};
			if (!existingOutfitTemplate.HasValue)
			{
				base.<MakeApplyToTemplatePopup>g__Refresh|1(outfitState.name);
				return;
			}
			if (existingOutfitTemplate.Value.CanWriteName && existingOutfitTemplate.Value.CanWriteItems)
			{
				base.<MakeApplyToTemplatePopup>g__Refresh|1(existingOutfitTemplate.Value.OutfitId);
				return;
			}
			base.<MakeApplyToTemplatePopup>g__Refresh|1(ClothingOutfitTarget.ForTemplateCopyOf(existingOutfitTemplate.Value).OutfitId);
		});
	}

	// Token: 0x0600A5B3 RID: 42419 RVA: 0x003FAECC File Offset: 0x003F90CC
	public static void MakeCopyPopup(OutfitDesignerScreen screen, KInputTextField inputFieldPrefab, OutfitDesignerScreen_OutfitState outfitState, ClothingOutfitTarget outfitTemplate, Option<Personality> minionPersonality, Action<ClothingOutfitTarget> onWriteToOutfitTargetFn)
	{
		ClothingOutfitNameProposal proposal = default(ClothingOutfitNameProposal);
		KInputTextField inputField;
		InfoScreenPlainText errorText;
		KButton okButton;
		LocText okButtonText;
		LockerNavigator.Instance.ShowDialogPopup(delegate(InfoDialogScreen dialog)
		{
			dialog.SetHeader(UI.OUTFIT_DESIGNER_SCREEN.COPY_POPUP.HEADER).AddUI<KInputTextField>(inputFieldPrefab, out inputField).AddSpacer(8f).AddUI<InfoScreenPlainText>(dialog.GetPlainTextPrefab(), out errorText).AddOption(true, out okButton, out okButtonText).AddOption(UI.CONFIRMDIALOG.CANCEL, delegate(InfoDialogScreen d)
			{
				d.Deactivate();
			}, false);
			inputField.onValueChanged.AddListener(new UnityAction<string>(base.<MakeCopyPopup>g__Refresh|1));
			errorText.gameObject.SetActive(false);
			LocText component = errorText.gameObject.GetComponent<LocText>();
			component.allowOverride = true;
			component.alignment = TextAlignmentOptions.BottomLeft;
			component.color = Util.ColorFromHex("F44A47");
			component.fontSize = 14f;
			errorText.SetText("");
			okButtonText.text = UI.CONFIRMDIALOG.OK;
			okButton.onClick += delegate()
			{
				if (proposal.result == ClothingOutfitNameProposal.Result.NewOutfit)
				{
					ClothingOutfitTarget clothingOutfitTarget = ClothingOutfitTarget.ForNewTemplateOutfit(outfitTemplate.OutfitType, proposal.candidateName);
					clothingOutfitTarget.WriteItems(outfitState.outfitType, outfitState.GetItems());
					if (minionPersonality.HasValue)
					{
						minionPersonality.Value.SetSelectedTemplateOutfitId(clothingOutfitTarget.OutfitType, clothingOutfitTarget.OutfitId);
					}
					if (onWriteToOutfitTargetFn != null)
					{
						onWriteToOutfitTargetFn(clothingOutfitTarget);
					}
					dialog.Deactivate();
					screen.Configure(screen.Config.WithOutfit(clothingOutfitTarget));
					return;
				}
				throw new NotSupportedException(string.Format("Can't save outfit with name \"{0}\", failed with result: {1}", proposal.candidateName, proposal.result));
			};
			base.<MakeCopyPopup>g__Refresh|1(ClothingOutfitTarget.ForTemplateCopyOf(outfitTemplate).OutfitId);
		});
	}

	// Token: 0x0600A5B4 RID: 42420 RVA: 0x003FAF30 File Offset: 0x003F9130
	private void SetCatogoryClickUISound(PermitCategory category, MultiToggle toggle)
	{
		toggle.states[1].on_click_override_sound_path = category.ToString() + "_Click";
		toggle.states[0].on_click_override_sound_path = category.ToString() + "_Click";
	}

	// Token: 0x0600A5B5 RID: 42421 RVA: 0x003FAF90 File Offset: 0x003F9190
	private void SetItemClickUISound(PermitResource permit, MultiToggle toggle)
	{
		if (permit == null)
		{
			toggle.states[1].on_click_override_sound_path = "HUD_Click";
			toggle.states[0].on_click_override_sound_path = "HUD_Click";
			return;
		}
		string clothingItemSoundName = OutfitDesignerScreen.GetClothingItemSoundName(permit);
		toggle.states[1].on_click_override_sound_path = clothingItemSoundName + "_Click";
		toggle.states[1].sound_parameter_name = "Unlocked";
		toggle.states[1].sound_parameter_value = (permit.IsUnlocked() ? 1f : 0f);
		toggle.states[1].has_sound_parameter = true;
		toggle.states[0].on_click_override_sound_path = clothingItemSoundName + "_Click";
		toggle.states[0].sound_parameter_name = "Unlocked";
		toggle.states[0].sound_parameter_value = (permit.IsUnlocked() ? 1f : 0f);
		toggle.states[0].has_sound_parameter = true;
	}

	// Token: 0x0600A5B6 RID: 42422 RVA: 0x003FB0A8 File Offset: 0x003F92A8
	public static string GetClothingItemSoundName(PermitResource permit)
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
		default:
			return "HUD";
		}
	}

	// Token: 0x0600A5B7 RID: 42423 RVA: 0x0010AC6E File Offset: 0x00108E6E
	private void OnMouseOverToggle()
	{
		KFMOD.PlayUISound(GlobalAssets.GetSound("HUD_Mouseover", false));
	}

	// Token: 0x0400819A RID: 33178
	[Header("CategoryColumn")]
	[SerializeField]
	private RectTransform categoryListContent;

	// Token: 0x0400819B RID: 33179
	[SerializeField]
	private GameObject categoryRowPrefab;

	// Token: 0x0400819C RID: 33180
	private UIPrefabLocalPool categoryRowPool;

	// Token: 0x0400819D RID: 33181
	[Header("ItemGalleryColumn")]
	[SerializeField]
	private LocText galleryHeaderLabel;

	// Token: 0x0400819E RID: 33182
	[SerializeField]
	private RectTransform galleryGridContent;

	// Token: 0x0400819F RID: 33183
	[SerializeField]
	private GameObject subcategoryUiPrefab;

	// Token: 0x040081A0 RID: 33184
	[SerializeField]
	private GameObject gridItemPrefab;

	// Token: 0x040081A1 RID: 33185
	private UIPrefabLocalPool subcategoryUiPool;

	// Token: 0x040081A2 RID: 33186
	private UIPrefabLocalPool galleryGridItemPool;

	// Token: 0x040081A3 RID: 33187
	private GridLayouter galleryGridLayouter;

	// Token: 0x040081A4 RID: 33188
	[Header("SelectionDetailsColumn")]
	[SerializeField]
	private LocText selectionHeaderLabel;

	// Token: 0x040081A5 RID: 33189
	[SerializeField]
	private UIMinionOrMannequin minionOrMannequin;

	// Token: 0x040081A6 RID: 33190
	[SerializeField]
	private Image dioramaBG;

	// Token: 0x040081A7 RID: 33191
	[SerializeField]
	private KButton primaryButton;

	// Token: 0x040081A8 RID: 33192
	[SerializeField]
	private KButton secondaryButton;

	// Token: 0x040081A9 RID: 33193
	[SerializeField]
	private OutfitDescriptionPanel outfitDescriptionPanel;

	// Token: 0x040081AA RID: 33194
	[SerializeField]
	private KInputTextField inputFieldPrefab;

	// Token: 0x040081AF RID: 33199
	public static Dictionary<ClothingOutfitUtility.OutfitType, PermitCategory[]> outfitTypeToCategoriesDict;

	// Token: 0x040081B0 RID: 33200
	private bool postponeConfiguration = true;

	// Token: 0x040081B1 RID: 33201
	private System.Action updateSaveButtonsFn;

	// Token: 0x040081B2 RID: 33202
	private System.Action RefreshCategoriesFn;

	// Token: 0x040081B3 RID: 33203
	private System.Action RefreshGalleryFn;

	// Token: 0x040081B4 RID: 33204
	private Func<bool> preventScreenPopFn;

	// Token: 0x02001ED6 RID: 7894
	private enum MultiToggleState
	{
		// Token: 0x040081B6 RID: 33206
		Default,
		// Token: 0x040081B7 RID: 33207
		Selected,
		// Token: 0x040081B8 RID: 33208
		NonInteractable
	}
}
