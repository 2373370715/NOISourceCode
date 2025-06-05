using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Database;
using STRINGS;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x02001EC4 RID: 7876
public class OutfitBrowserScreen : KMonoBehaviour
{
	// Token: 0x0600A53F RID: 42303 RVA: 0x003F87DC File Offset: 0x003F69DC
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.galleryGridItemPool = new UIPrefabLocalPool(this.gridItemPrefab, this.galleryGridContent.gameObject);
		this.gridLayouter = new GridLayouter
		{
			minCellSize = 112f,
			maxCellSize = 144f,
			targetGridLayouts = this.galleryGridContent.GetComponents<GridLayoutGroup>().ToList<GridLayoutGroup>()
		};
		this.categoriesAndSearchBar.InitializeWith(this);
		this.pickOutfitButton.onClick += this.OnClickPickOutfit;
		this.editOutfitButton.onClick += delegate()
		{
			if (this.state.SelectedOutfitOpt.IsNone())
			{
				return;
			}
			new OutfitDesignerScreenConfig(this.state.SelectedOutfitOpt.Unwrap(), this.Config.minionPersonality, this.Config.targetMinionInstance, new Action<ClothingOutfitTarget>(this.OnOutfitDesignerWritesToOutfitTarget)).ApplyAndOpenScreen();
		};
		this.renameOutfitButton.onClick += delegate()
		{
			ClothingOutfitTarget selectedOutfit = this.state.SelectedOutfitOpt.Unwrap();
			OutfitBrowserScreen.MakeRenamePopup(this.inputFieldPrefab, selectedOutfit, () => selectedOutfit.ReadName(), delegate(string new_name)
			{
				selectedOutfit.WriteName(new_name);
				this.Configure(this.Config.WithOutfit(selectedOutfit));
			});
		};
		this.deleteOutfitButton.onClick += delegate()
		{
			ClothingOutfitTarget selectedOutfit = this.state.SelectedOutfitOpt.Unwrap();
			OutfitBrowserScreen.MakeDeletePopup(selectedOutfit, delegate
			{
				selectedOutfit.Delete();
				this.Configure(this.Config.WithOutfit(Option.None));
			});
		};
	}

	// Token: 0x17000A98 RID: 2712
	// (get) Token: 0x0600A540 RID: 42304 RVA: 0x0010FADD File Offset: 0x0010DCDD
	// (set) Token: 0x0600A541 RID: 42305 RVA: 0x0010FAE5 File Offset: 0x0010DCE5
	public OutfitBrowserScreenConfig Config { get; private set; }

	// Token: 0x0600A542 RID: 42306 RVA: 0x003F88AC File Offset: 0x003F6AAC
	protected override void OnCmpEnable()
	{
		if (this.isFirstDisplay)
		{
			this.isFirstDisplay = false;
			this.dioramaMinionOrMannequin.TrySpawn();
			this.FirstTimeSetup();
			this.postponeConfiguration = false;
			this.Configure(this.Config);
		}
		KleiItemsStatusRefresher.AddOrGetListener(this).OnRefreshUI(delegate
		{
			this.RefreshGallery();
			this.outfitDescriptionPanel.Refresh(this.state.SelectedOutfitOpt, ClothingOutfitUtility.OutfitType.Clothing, this.Config.minionPersonality);
		});
	}

	// Token: 0x0600A543 RID: 42307 RVA: 0x0010FAEE File Offset: 0x0010DCEE
	private void FirstTimeSetup()
	{
		this.state.OnCurrentOutfitTypeChanged += delegate()
		{
			this.PopulateGallery();
			OutfitBrowserScreenConfig config = this.Config;
			Option<ClothingOutfitTarget> selectedOutfitOpt;
			if (!config.minionPersonality.HasValue)
			{
				config = this.Config;
				if (!config.selectedTarget.HasValue)
				{
					selectedOutfitOpt = ClothingOutfitTarget.GetRandom(this.state.CurrentOutfitType);
					goto IL_4F;
				}
			}
			selectedOutfitOpt = this.Config.selectedTarget;
			IL_4F:
			if (selectedOutfitOpt.IsSome() && selectedOutfitOpt.Unwrap().DoesExist())
			{
				this.state.SelectedOutfitOpt = selectedOutfitOpt;
				return;
			}
			this.state.SelectedOutfitOpt = Option.None;
		};
		this.state.OnSelectedOutfitOptChanged += delegate()
		{
			if (this.state.SelectedOutfitOpt.IsSome())
			{
				this.selectionHeaderLabel.text = this.state.SelectedOutfitOpt.Unwrap().ReadName();
			}
			else
			{
				this.selectionHeaderLabel.text = UI.OUTFIT_NAME.NONE;
			}
			this.dioramaMinionOrMannequin.current.SetOutfit(this.state.CurrentOutfitType, this.state.SelectedOutfitOpt);
			this.dioramaMinionOrMannequin.current.ReactToFullOutfitChange();
			this.outfitDescriptionPanel.Refresh(this.state.SelectedOutfitOpt, this.state.CurrentOutfitType, this.Config.minionPersonality);
			this.dioramaBG.sprite = KleiPermitDioramaVis.GetDioramaBackground(this.state.CurrentOutfitType);
			this.pickOutfitButton.gameObject.SetActive(this.Config.isPickingOutfitForDupe);
			OutfitBrowserScreenConfig config = this.Config;
			if (config.minionPersonality.IsSome())
			{
				this.pickOutfitButton.isInteractable = (!this.state.SelectedOutfitOpt.IsSome() || !this.state.SelectedOutfitOpt.Unwrap().DoesContainLockedItems());
				GameObject gameObject = this.pickOutfitButton.gameObject;
				Option<string> tooltipText;
				if (!this.pickOutfitButton.isInteractable)
				{
					LocString tooltip_PICK_OUTFIT_ERROR_LOCKED = UI.OUTFIT_BROWSER_SCREEN.TOOLTIP_PICK_OUTFIT_ERROR_LOCKED;
					string search = "{MinionName}";
					config = this.Config;
					tooltipText = Option.Some<string>(tooltip_PICK_OUTFIT_ERROR_LOCKED.Replace(search, config.GetMinionName()));
				}
				else
				{
					tooltipText = Option.None;
				}
				KleiItemsUI.ConfigureTooltipOn(gameObject, tooltipText);
			}
			this.editOutfitButton.isInteractable = this.state.SelectedOutfitOpt.IsSome();
			this.renameOutfitButton.isInteractable = (this.state.SelectedOutfitOpt.IsSome() && this.state.SelectedOutfitOpt.Unwrap().CanWriteName);
			KleiItemsUI.ConfigureTooltipOn(this.renameOutfitButton.gameObject, this.renameOutfitButton.isInteractable ? UI.OUTFIT_BROWSER_SCREEN.TOOLTIP_RENAME_OUTFIT : UI.OUTFIT_BROWSER_SCREEN.TOOLTIP_RENAME_OUTFIT_ERROR_READONLY);
			this.deleteOutfitButton.isInteractable = (this.state.SelectedOutfitOpt.IsSome() && this.state.SelectedOutfitOpt.Unwrap().CanDelete);
			KleiItemsUI.ConfigureTooltipOn(this.deleteOutfitButton.gameObject, this.deleteOutfitButton.isInteractable ? UI.OUTFIT_BROWSER_SCREEN.TOOLTIP_DELETE_OUTFIT : UI.OUTFIT_BROWSER_SCREEN.TOOLTIP_DELETE_OUTFIT_ERROR_READONLY);
			this.state.OnSelectedOutfitOptChanged += this.RefreshGallery;
			this.state.OnFilterChanged += this.RefreshGallery;
			this.state.OnCurrentOutfitTypeChanged += this.RefreshGallery;
			this.RefreshGallery();
		};
	}

	// Token: 0x0600A544 RID: 42308 RVA: 0x003F8904 File Offset: 0x003F6B04
	public void Configure(OutfitBrowserScreenConfig config)
	{
		this.Config = config;
		if (this.postponeConfiguration)
		{
			return;
		}
		this.dioramaMinionOrMannequin.SetFrom(config.minionPersonality);
		if (config.targetMinionInstance.HasValue)
		{
			this.galleryHeaderLabel.text = UI.OUTFIT_BROWSER_SCREEN.COLUMN_HEADERS.MINION_GALLERY_HEADER.Replace("{MinionName}", config.targetMinionInstance.Value.GetProperName());
		}
		else if (config.minionPersonality.HasValue)
		{
			this.galleryHeaderLabel.text = UI.OUTFIT_BROWSER_SCREEN.COLUMN_HEADERS.MINION_GALLERY_HEADER.Replace("{MinionName}", config.minionPersonality.Value.Name);
		}
		else
		{
			this.galleryHeaderLabel.text = UI.OUTFIT_BROWSER_SCREEN.COLUMN_HEADERS.GALLERY_HEADER;
		}
		this.state.CurrentOutfitType = config.onlyShowOutfitType.UnwrapOr(this.lastShownOutfitType.UnwrapOr(ClothingOutfitUtility.OutfitType.Clothing, null), null);
		if (base.gameObject.activeInHierarchy)
		{
			base.gameObject.SetActive(false);
			base.gameObject.SetActive(true);
		}
	}

	// Token: 0x0600A545 RID: 42309 RVA: 0x0010FB1E File Offset: 0x0010DD1E
	private void RefreshGallery()
	{
		if (this.RefreshGalleryFn != null)
		{
			this.RefreshGalleryFn();
		}
	}

	// Token: 0x0600A546 RID: 42310 RVA: 0x003F8A08 File Offset: 0x003F6C08
	private void PopulateGallery()
	{
		this.outfits.Clear();
		this.galleryGridItemPool.ReturnAll();
		this.RefreshGalleryFn = null;
		if (this.Config.isPickingOutfitForDupe)
		{
			this.<PopulateGallery>g__AddGridIconForTarget|35_0(Option.None);
		}
		OutfitBrowserScreenConfig config = this.Config;
		if (config.targetMinionInstance.HasValue)
		{
			ClothingOutfitUtility.OutfitType currentOutfitType = this.state.CurrentOutfitType;
			config = this.Config;
			this.<PopulateGallery>g__AddGridIconForTarget|35_0(ClothingOutfitTarget.FromMinion(currentOutfitType, config.targetMinionInstance.Value));
		}
		foreach (ClothingOutfitTarget value in from outfit in ClothingOutfitTarget.GetAllTemplates()
		where outfit.OutfitType == this.state.CurrentOutfitType
		select outfit)
		{
			this.<PopulateGallery>g__AddGridIconForTarget|35_0(value);
		}
		this.addButtonGridItem.transform.SetAsLastSibling();
		this.addButtonGridItem.SetActive(true);
		this.addButtonGridItem.GetComponent<MultiToggle>().onClick = delegate()
		{
			new OutfitDesignerScreenConfig(ClothingOutfitTarget.ForNewTemplateOutfit(this.state.CurrentOutfitType), this.Config.minionPersonality, this.Config.targetMinionInstance, new Action<ClothingOutfitTarget>(this.OnOutfitDesignerWritesToOutfitTarget)).ApplyAndOpenScreen();
		};
		this.RefreshGallery();
	}

	// Token: 0x0600A547 RID: 42311 RVA: 0x003F8B28 File Offset: 0x003F6D28
	private void OnOutfitDesignerWritesToOutfitTarget(ClothingOutfitTarget outfit)
	{
		this.Configure(this.Config.WithOutfit(outfit));
	}

	// Token: 0x0600A548 RID: 42312 RVA: 0x0010FB33 File Offset: 0x0010DD33
	private void Update()
	{
		this.gridLayouter.CheckIfShouldResizeGrid();
	}

	// Token: 0x0600A549 RID: 42313 RVA: 0x003F8B50 File Offset: 0x003F6D50
	private void OnClickPickOutfit()
	{
		OutfitBrowserScreenConfig config = this.Config;
		if (config.targetMinionInstance.IsSome())
		{
			config = this.Config;
			WearableAccessorizer component = config.targetMinionInstance.Unwrap().GetComponent<WearableAccessorizer>();
			ClothingOutfitUtility.OutfitType currentOutfitType = this.state.CurrentOutfitType;
			Option<ClothingOutfitTarget> selectedOutfitOpt = this.state.SelectedOutfitOpt;
			component.ApplyClothingItems(currentOutfitType, selectedOutfitOpt.AndThen<IEnumerable<ClothingItemResource>>((ClothingOutfitTarget outfit) => outfit.ReadItemValues()).UnwrapOr(ClothingOutfitTarget.NO_ITEM_VALUES, null));
		}
		else
		{
			config = this.Config;
			if (config.minionPersonality.IsSome())
			{
				config = this.Config;
				Personality value = config.minionPersonality.Value;
				ClothingOutfitUtility.OutfitType currentOutfitType2 = this.state.CurrentOutfitType;
				Option<ClothingOutfitTarget> selectedOutfitOpt = this.state.SelectedOutfitOpt;
				value.SetSelectedTemplateOutfitId(currentOutfitType2, selectedOutfitOpt.AndThen<string>((ClothingOutfitTarget o) => o.OutfitId));
			}
		}
		LockerNavigator.Instance.PopScreen();
	}

	// Token: 0x0600A54A RID: 42314 RVA: 0x003F8C54 File Offset: 0x003F6E54
	public static void MakeDeletePopup(ClothingOutfitTarget sourceTarget, System.Action deleteFn)
	{
		Action<InfoDialogScreen> <>9__1;
		LockerNavigator.Instance.ShowDialogPopup(delegate(InfoDialogScreen dialog)
		{
			InfoDialogScreen infoDialogScreen = dialog.SetHeader(UI.OUTFIT_BROWSER_SCREEN.DELETE_WARNING_POPUP.HEADER.Replace("{OutfitName}", sourceTarget.ReadName())).AddPlainText(UI.OUTFIT_BROWSER_SCREEN.DELETE_WARNING_POPUP.BODY.Replace("{OutfitName}", sourceTarget.ReadName()));
			string text = UI.OUTFIT_BROWSER_SCREEN.DELETE_WARNING_POPUP.BUTTON_YES_DELETE;
			Action<InfoDialogScreen> action;
			if ((action = <>9__1) == null)
			{
				action = (<>9__1 = delegate(InfoDialogScreen d)
				{
					deleteFn();
					d.Deactivate();
				});
			}
			infoDialogScreen.AddOption(text, action, true).AddOption(UI.OUTFIT_BROWSER_SCREEN.DELETE_WARNING_POPUP.BUTTON_DONT_DELETE, delegate(InfoDialogScreen d)
			{
				d.Deactivate();
			}, false);
		});
	}

	// Token: 0x0600A54B RID: 42315 RVA: 0x003F8C8C File Offset: 0x003F6E8C
	public static void MakeRenamePopup(KInputTextField inputFieldPrefab, ClothingOutfitTarget sourceTarget, Func<string> readName, Action<string> writeName)
	{
		KInputTextField inputField;
		InfoScreenPlainText errorText;
		KButton okButton;
		LocText okButtonText;
		LockerNavigator.Instance.ShowDialogPopup(delegate(InfoDialogScreen dialog)
		{
			dialog.SetHeader(UI.OUTFIT_BROWSER_SCREEN.RENAME_POPUP.HEADER).AddUI<KInputTextField>(inputFieldPrefab, out inputField).AddSpacer(8f).AddUI<InfoScreenPlainText>(dialog.GetPlainTextPrefab(), out errorText).AddOption(true, out okButton, out okButtonText).AddOption(UI.CONFIRMDIALOG.CANCEL, delegate(InfoDialogScreen d)
			{
				d.Deactivate();
			}, false);
			inputField.onValueChanged.AddListener(new UnityAction<string>(base.<MakeRenamePopup>g__Refresh|1));
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
				writeName(inputField.text);
				dialog.Deactivate();
			};
			base.<MakeRenamePopup>g__Refresh|1(readName());
		});
	}

	// Token: 0x0600A54C RID: 42316 RVA: 0x003F8CD4 File Offset: 0x003F6ED4
	private void SetButtonClickUISound(Option<ClothingOutfitTarget> target, MultiToggle toggle)
	{
		if (!target.HasValue)
		{
			toggle.states[1].on_click_override_sound_path = "HUD_Click";
			toggle.states[0].on_click_override_sound_path = "HUD_Click";
			return;
		}
		bool flag = !target.Value.DoesContainLockedItems();
		toggle.states[1].on_click_override_sound_path = "ClothingItem_Click";
		toggle.states[1].sound_parameter_name = "Unlocked";
		toggle.states[1].sound_parameter_value = (flag ? 1f : 0f);
		toggle.states[1].has_sound_parameter = true;
		toggle.states[0].on_click_override_sound_path = "ClothingItem_Click";
		toggle.states[0].sound_parameter_name = "Unlocked";
		toggle.states[0].sound_parameter_value = (flag ? 1f : 0f);
		toggle.states[0].has_sound_parameter = true;
	}

	// Token: 0x0600A54D RID: 42317 RVA: 0x0010AC6E File Offset: 0x00108E6E
	private void OnMouseOverToggle()
	{
		KFMOD.PlayUISound(GlobalAssets.GetSound("HUD_Mouseover", false));
	}

	// Token: 0x0600A555 RID: 42325 RVA: 0x003F926C File Offset: 0x003F746C
	[CompilerGenerated]
	private void <PopulateGallery>g__AddGridIconForTarget|35_0(Option<ClothingOutfitTarget> target)
	{
		GameObject spawn = this.galleryGridItemPool.Borrow();
		GameObject gameObject = spawn.transform.GetChild(1).gameObject;
		GameObject isUnownedOverlayGO = spawn.transform.GetChild(2).gameObject;
		GameObject dlcBannerGO = spawn.transform.GetChild(3).gameObject;
		gameObject.SetActive(true);
		bool shouldShowOutfitWithDefaultItems = target.IsNone() || this.state.CurrentOutfitType == ClothingOutfitUtility.OutfitType.AtmoSuit;
		UIMannequin componentInChildren = gameObject.GetComponentInChildren<UIMannequin>();
		this.dioramaMinionOrMannequin.mannequin.shouldShowOutfitWithDefaultItems = shouldShowOutfitWithDefaultItems;
		componentInChildren.shouldShowOutfitWithDefaultItems = shouldShowOutfitWithDefaultItems;
		componentInChildren.personalityToUseForDefaultClothing = this.Config.minionPersonality;
		componentInChildren.SetOutfit(this.state.CurrentOutfitType, target);
		RectTransform component = gameObject.GetComponent<RectTransform>();
		float x;
		float num;
		float num2;
		float y;
		switch (this.state.CurrentOutfitType)
		{
		case ClothingOutfitUtility.OutfitType.Clothing:
			x = 8f;
			num = 8f;
			num2 = 8f;
			y = 8f;
			break;
		case ClothingOutfitUtility.OutfitType.JoyResponse:
			throw new NotSupportedException();
		case ClothingOutfitUtility.OutfitType.AtmoSuit:
			x = 24f;
			num = 16f;
			num2 = 32f;
			y = 8f;
			break;
		default:
			throw new NotImplementedException();
		}
		component.offsetMin = new Vector2(x, y);
		component.offsetMax = new Vector2(-num, -num2);
		MultiToggle button = spawn.GetComponent<MultiToggle>();
		MultiToggle button2 = button;
		button2.onEnter = (System.Action)Delegate.Combine(button2.onEnter, new System.Action(this.OnMouseOverToggle));
		button.onClick = delegate()
		{
			this.state.SelectedOutfitOpt = target;
		};
		this.RefreshGalleryFn = (System.Action)Delegate.Combine(this.RefreshGalleryFn, new System.Action(delegate()
		{
			button.ChangeState((target == this.state.SelectedOutfitOpt) ? 1 : 0);
			if (string.IsNullOrWhiteSpace(this.state.Filter) || target.IsNone())
			{
				spawn.SetActive(true);
			}
			else
			{
				spawn.SetActive(target.Unwrap().ReadName().ToLower().Contains(this.state.Filter.ToLower()));
			}
			if (!target.HasValue)
			{
				KleiItemsUI.ConfigureTooltipOn(spawn, KleiItemsUI.WrapAsToolTipTitle(KleiItemsUI.GetNoneOutfitName(this.state.CurrentOutfitType)));
				isUnownedOverlayGO.SetActive(false);
			}
			else
			{
				KleiItemsUI.ConfigureTooltipOn(spawn, KleiItemsUI.WrapAsToolTipTitle(target.Value.ReadName()));
				isUnownedOverlayGO.SetActive(target.Value.DoesContainLockedItems());
			}
			if (target.IsSome())
			{
				ClothingOutfitTarget.Implementation impl = target.Unwrap().impl;
				if (impl is ClothingOutfitTarget.DatabaseAuthoredTemplate)
				{
					ClothingOutfitTarget.DatabaseAuthoredTemplate databaseAuthoredTemplate = (ClothingOutfitTarget.DatabaseAuthoredTemplate)impl;
					string dlcIdFrom = databaseAuthoredTemplate.resource.GetDlcIdFrom();
					if (DlcManager.IsDlcId(dlcIdFrom))
					{
						dlcBannerGO.GetComponent<Image>().color = DlcManager.GetDlcBannerColor(dlcIdFrom);
						dlcBannerGO.SetActive(true);
						return;
					}
					dlcBannerGO.SetActive(false);
					return;
				}
			}
			dlcBannerGO.SetActive(false);
		}));
		this.SetButtonClickUISound(target, button);
	}

	// Token: 0x04008138 RID: 33080
	[Header("ItemGalleryColumn")]
	[SerializeField]
	private LocText galleryHeaderLabel;

	// Token: 0x04008139 RID: 33081
	[SerializeField]
	private OutfitBrowserScreen_CategoriesAndSearchBar categoriesAndSearchBar;

	// Token: 0x0400813A RID: 33082
	[SerializeField]
	private RectTransform galleryGridContent;

	// Token: 0x0400813B RID: 33083
	[SerializeField]
	private GameObject gridItemPrefab;

	// Token: 0x0400813C RID: 33084
	[SerializeField]
	private GameObject addButtonGridItem;

	// Token: 0x0400813D RID: 33085
	private UIPrefabLocalPool galleryGridItemPool;

	// Token: 0x0400813E RID: 33086
	private GridLayouter gridLayouter;

	// Token: 0x0400813F RID: 33087
	[Header("SelectionDetailsColumn")]
	[SerializeField]
	private LocText selectionHeaderLabel;

	// Token: 0x04008140 RID: 33088
	[SerializeField]
	private UIMinionOrMannequin dioramaMinionOrMannequin;

	// Token: 0x04008141 RID: 33089
	[SerializeField]
	private Image dioramaBG;

	// Token: 0x04008142 RID: 33090
	[SerializeField]
	private OutfitDescriptionPanel outfitDescriptionPanel;

	// Token: 0x04008143 RID: 33091
	[SerializeField]
	private KButton pickOutfitButton;

	// Token: 0x04008144 RID: 33092
	[SerializeField]
	private KButton editOutfitButton;

	// Token: 0x04008145 RID: 33093
	[SerializeField]
	private KButton renameOutfitButton;

	// Token: 0x04008146 RID: 33094
	[SerializeField]
	private KButton deleteOutfitButton;

	// Token: 0x04008147 RID: 33095
	[Header("Misc")]
	[SerializeField]
	private KInputTextField inputFieldPrefab;

	// Token: 0x04008148 RID: 33096
	[SerializeField]
	public ColorStyleSetting selectedCategoryStyle;

	// Token: 0x04008149 RID: 33097
	[SerializeField]
	public ColorStyleSetting notSelectedCategoryStyle;

	// Token: 0x0400814A RID: 33098
	public OutfitBrowserScreen.State state = new OutfitBrowserScreen.State();

	// Token: 0x0400814B RID: 33099
	public Option<ClothingOutfitUtility.OutfitType> lastShownOutfitType = Option.None;

	// Token: 0x0400814C RID: 33100
	private Dictionary<string, MultiToggle> outfits = new Dictionary<string, MultiToggle>();

	// Token: 0x0400814E RID: 33102
	private bool postponeConfiguration = true;

	// Token: 0x0400814F RID: 33103
	private bool isFirstDisplay = true;

	// Token: 0x04008150 RID: 33104
	private System.Action RefreshGalleryFn;

	// Token: 0x02001EC5 RID: 7877
	public class State
	{
		// Token: 0x14000034 RID: 52
		// (add) Token: 0x0600A558 RID: 42328 RVA: 0x003F94B4 File Offset: 0x003F76B4
		// (remove) Token: 0x0600A559 RID: 42329 RVA: 0x003F94EC File Offset: 0x003F76EC
		public event System.Action OnSelectedOutfitOptChanged;

		// Token: 0x17000A99 RID: 2713
		// (get) Token: 0x0600A55A RID: 42330 RVA: 0x0010FBBC File Offset: 0x0010DDBC
		// (set) Token: 0x0600A55B RID: 42331 RVA: 0x0010FBC4 File Offset: 0x0010DDC4
		public Option<ClothingOutfitTarget> SelectedOutfitOpt
		{
			get
			{
				return this.m_selectedOutfitOpt;
			}
			set
			{
				this.m_selectedOutfitOpt = value;
				if (this.OnSelectedOutfitOptChanged != null)
				{
					this.OnSelectedOutfitOptChanged();
				}
			}
		}

		// Token: 0x14000035 RID: 53
		// (add) Token: 0x0600A55C RID: 42332 RVA: 0x003F9524 File Offset: 0x003F7724
		// (remove) Token: 0x0600A55D RID: 42333 RVA: 0x003F955C File Offset: 0x003F775C
		public event System.Action OnCurrentOutfitTypeChanged;

		// Token: 0x17000A9A RID: 2714
		// (get) Token: 0x0600A55E RID: 42334 RVA: 0x0010FBE0 File Offset: 0x0010DDE0
		// (set) Token: 0x0600A55F RID: 42335 RVA: 0x0010FBE8 File Offset: 0x0010DDE8
		public ClothingOutfitUtility.OutfitType CurrentOutfitType
		{
			get
			{
				return this.m_currentOutfitType;
			}
			set
			{
				this.m_currentOutfitType = value;
				if (this.OnCurrentOutfitTypeChanged != null)
				{
					this.OnCurrentOutfitTypeChanged();
				}
			}
		}

		// Token: 0x14000036 RID: 54
		// (add) Token: 0x0600A560 RID: 42336 RVA: 0x003F9594 File Offset: 0x003F7794
		// (remove) Token: 0x0600A561 RID: 42337 RVA: 0x003F95CC File Offset: 0x003F77CC
		public event System.Action OnFilterChanged;

		// Token: 0x17000A9B RID: 2715
		// (get) Token: 0x0600A562 RID: 42338 RVA: 0x0010FC04 File Offset: 0x0010DE04
		// (set) Token: 0x0600A563 RID: 42339 RVA: 0x0010FC0C File Offset: 0x0010DE0C
		public string Filter
		{
			get
			{
				return this.m_filter;
			}
			set
			{
				this.m_filter = value;
				if (this.OnFilterChanged != null)
				{
					this.OnFilterChanged();
				}
			}
		}

		// Token: 0x04008151 RID: 33105
		private Option<ClothingOutfitTarget> m_selectedOutfitOpt;

		// Token: 0x04008152 RID: 33106
		private ClothingOutfitUtility.OutfitType m_currentOutfitType;

		// Token: 0x04008153 RID: 33107
		private string m_filter;
	}

	// Token: 0x02001EC6 RID: 7878
	private enum MultiToggleState
	{
		// Token: 0x04008158 RID: 33112
		Default,
		// Token: 0x04008159 RID: 33113
		Selected,
		// Token: 0x0400815A RID: 33114
		NonInteractable
	}
}
