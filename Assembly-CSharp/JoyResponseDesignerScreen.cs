using System;
using System.Linq;
using System.Runtime.CompilerServices;
using Database;
using STRINGS;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02001D66 RID: 7526
public class JoyResponseDesignerScreen : KMonoBehaviour
{
	// Token: 0x17000A56 RID: 2646
	// (get) Token: 0x06009D1F RID: 40223 RVA: 0x0010AB94 File Offset: 0x00108D94
	// (set) Token: 0x06009D20 RID: 40224 RVA: 0x0010AB9C File Offset: 0x00108D9C
	public JoyResponseScreenConfig Config { get; private set; }

	// Token: 0x06009D21 RID: 40225 RVA: 0x003D6CB0 File Offset: 0x003D4EB0
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		global::Debug.Assert(this.categoryRowPrefab.transform.parent == this.categoryListContent.transform);
		global::Debug.Assert(this.galleryItemPrefab.transform.parent == this.galleryGridContent.transform);
		this.categoryRowPrefab.SetActive(false);
		this.galleryItemPrefab.SetActive(false);
		this.galleryGridLayouter = new GridLayouter
		{
			minCellSize = 64f,
			maxCellSize = 96f,
			targetGridLayouts = this.galleryGridContent.GetComponents<GridLayoutGroup>().ToList<GridLayoutGroup>()
		};
		this.categoryRowPool = new UIPrefabLocalPool(this.categoryRowPrefab, this.categoryListContent.gameObject);
		this.galleryGridItemPool = new UIPrefabLocalPool(this.galleryItemPrefab, this.galleryGridContent.gameObject);
		JoyResponseDesignerScreen.JoyResponseCategory[] array = new JoyResponseDesignerScreen.JoyResponseCategory[1];
		int num = 0;
		JoyResponseDesignerScreen.JoyResponseCategory joyResponseCategory = new JoyResponseDesignerScreen.JoyResponseCategory();
		joyResponseCategory.displayName = UI.KLEI_INVENTORY_SCREEN.CATEGORIES.JOY_RESPONSES.BALLOON_ARTIST;
		joyResponseCategory.icon = Assets.GetSprite("icon_inventory_balloonartist");
		JoyResponseDesignerScreen.GalleryItem[] items = (from r in Db.Get().Permits.BalloonArtistFacades.resources
		select JoyResponseDesignerScreen.GalleryItem.Of(r)).Prepend(JoyResponseDesignerScreen.GalleryItem.Of(Option.None)).ToArray<JoyResponseDesignerScreen.GalleryItem.BalloonArtistFacadeTarget>();
		joyResponseCategory.items = items;
		array[num] = joyResponseCategory;
		this.joyResponseCategories = array;
		this.dioramaVis.ConfigureSetup();
	}

	// Token: 0x06009D22 RID: 40226 RVA: 0x0010ABA5 File Offset: 0x00108DA5
	private void Update()
	{
		this.galleryGridLayouter.CheckIfShouldResizeGrid();
	}

	// Token: 0x06009D23 RID: 40227 RVA: 0x0010ABB2 File Offset: 0x00108DB2
	protected override void OnSpawn()
	{
		this.postponeConfiguration = false;
		if (this.Config.isValid)
		{
			this.Configure(this.Config);
			return;
		}
		throw new InvalidOperationException("Cannot open up JoyResponseDesignerScreen without a target personality or minion instance");
	}

	// Token: 0x06009D24 RID: 40228 RVA: 0x0010ABDF File Offset: 0x00108DDF
	protected override void OnCmpEnable()
	{
		base.OnCmpEnable();
		KleiItemsStatusRefresher.AddOrGetListener(this).OnRefreshUI(delegate
		{
			this.Configure(this.Config);
		});
	}

	// Token: 0x06009D25 RID: 40229 RVA: 0x003D6E34 File Offset: 0x003D5034
	public void Configure(JoyResponseScreenConfig config)
	{
		this.Config = config;
		if (this.postponeConfiguration)
		{
			return;
		}
		this.RegisterPreventScreenPop();
		this.primaryButton.ClearOnClick();
		TMP_Text componentInChildren = this.primaryButton.GetComponentInChildren<LocText>();
		LocString button_APPLY_TO_MINION = UI.JOY_RESPONSE_DESIGNER_SCREEN.BUTTON_APPLY_TO_MINION;
		string search = "{MinionName}";
		JoyResponseScreenConfig config2 = this.Config;
		componentInChildren.SetText(button_APPLY_TO_MINION.Replace(search, config2.target.GetMinionName()));
		this.primaryButton.onClick += delegate()
		{
			Option<PermitResource> permitResource = this.selectedGalleryItem.GetPermitResource();
			if (permitResource.IsSome())
			{
				string str = "Save selected balloon ";
				string name = this.selectedGalleryItem.GetName();
				string str2 = " for ";
				JoyResponseScreenConfig config3 = this.Config;
				global::Debug.Log(str + name + str2 + config3.target.GetMinionName());
				if (this.CanSaveSelection())
				{
					config3 = this.Config;
					config3.target.WriteFacadeId(permitResource.Unwrap().Id);
				}
			}
			else
			{
				string str3 = "Save selected balloon ";
				string name2 = this.selectedGalleryItem.GetName();
				string str4 = " for ";
				JoyResponseScreenConfig config3 = this.Config;
				global::Debug.Log(str3 + name2 + str4 + config3.target.GetMinionName());
				config3 = this.Config;
				config3.target.WriteFacadeId(Option.None);
			}
			LockerNavigator.Instance.PopScreen();
		};
		this.PopulateCategories();
		this.PopulateGallery();
		this.PopulatePreview();
		config2 = this.Config;
		if (config2.initalSelectedItem.IsSome())
		{
			config2 = this.Config;
			this.SelectGalleryItem(config2.initalSelectedItem.Unwrap());
		}
	}

	// Token: 0x06009D26 RID: 40230 RVA: 0x003D6EEC File Offset: 0x003D50EC
	private bool CanSaveSelection()
	{
		return this.GetSaveSelectionError().IsNone();
	}

	// Token: 0x06009D27 RID: 40231 RVA: 0x003D6F08 File Offset: 0x003D5108
	private Option<string> GetSaveSelectionError()
	{
		if (!this.selectedGalleryItem.IsUnlocked())
		{
			return Option.Some<string>(UI.JOY_RESPONSE_DESIGNER_SCREEN.TOOLTIP_PICK_JOY_RESPONSE_ERROR_LOCKED.Replace("{MinionName}", this.Config.target.GetMinionName()));
		}
		return Option.None;
	}

	// Token: 0x06009D28 RID: 40232 RVA: 0x0010ABFE File Offset: 0x00108DFE
	private void RefreshCategories()
	{
		if (this.RefreshCategoriesFn != null)
		{
			this.RefreshCategoriesFn();
		}
	}

	// Token: 0x06009D29 RID: 40233 RVA: 0x003D6F54 File Offset: 0x003D5154
	public void PopulateCategories()
	{
		this.RefreshCategoriesFn = null;
		this.categoryRowPool.ReturnAll();
		JoyResponseDesignerScreen.JoyResponseCategory[] array = this.joyResponseCategories;
		for (int i = 0; i < array.Length; i++)
		{
			JoyResponseDesignerScreen.<>c__DisplayClass28_0 CS$<>8__locals1 = new JoyResponseDesignerScreen.<>c__DisplayClass28_0();
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.category = array[i];
			GameObject gameObject = this.categoryRowPool.Borrow();
			HierarchyReferences component = gameObject.GetComponent<HierarchyReferences>();
			component.GetReference<LocText>("Label").SetText(CS$<>8__locals1.category.displayName);
			component.GetReference<Image>("Icon").sprite = CS$<>8__locals1.category.icon;
			MultiToggle toggle = gameObject.GetComponent<MultiToggle>();
			MultiToggle toggle2 = toggle;
			toggle2.onEnter = (System.Action)Delegate.Combine(toggle2.onEnter, new System.Action(this.OnMouseOverToggle));
			toggle.onClick = delegate()
			{
				CS$<>8__locals1.<>4__this.SelectCategory(CS$<>8__locals1.category);
			};
			this.RefreshCategoriesFn = (System.Action)Delegate.Combine(this.RefreshCategoriesFn, new System.Action(delegate()
			{
				toggle.ChangeState((CS$<>8__locals1.category == CS$<>8__locals1.<>4__this.selectedCategoryOpt) ? 1 : 0);
			}));
			this.SetCatogoryClickUISound(CS$<>8__locals1.category, toggle);
		}
		this.SelectCategory(this.joyResponseCategories[0]);
	}

	// Token: 0x06009D2A RID: 40234 RVA: 0x0010AC13 File Offset: 0x00108E13
	public void SelectCategory(JoyResponseDesignerScreen.JoyResponseCategory category)
	{
		this.selectedCategoryOpt = category;
		this.galleryHeaderLabel.text = category.displayName;
		this.RefreshCategories();
		this.PopulateGallery();
		this.RefreshPreview();
	}

	// Token: 0x06009D2B RID: 40235 RVA: 0x000AA038 File Offset: 0x000A8238
	private void SetCatogoryClickUISound(JoyResponseDesignerScreen.JoyResponseCategory category, MultiToggle toggle)
	{
	}

	// Token: 0x06009D2C RID: 40236 RVA: 0x0010AC44 File Offset: 0x00108E44
	private void RefreshGallery()
	{
		if (this.RefreshGalleryFn != null)
		{
			this.RefreshGalleryFn();
		}
	}

	// Token: 0x06009D2D RID: 40237 RVA: 0x003D709C File Offset: 0x003D529C
	public void PopulateGallery()
	{
		this.RefreshGalleryFn = null;
		this.galleryGridItemPool.ReturnAll();
		if (this.selectedCategoryOpt.IsNone())
		{
			return;
		}
		JoyResponseDesignerScreen.JoyResponseCategory joyResponseCategory = this.selectedCategoryOpt.Unwrap();
		foreach (JoyResponseDesignerScreen.GalleryItem item in joyResponseCategory.items)
		{
			this.<PopulateGallery>g__AddGridIcon|36_0(item);
		}
		this.SelectGalleryItem(joyResponseCategory.items[0]);
	}

	// Token: 0x06009D2E RID: 40238 RVA: 0x0010AC59 File Offset: 0x00108E59
	public void SelectGalleryItem(JoyResponseDesignerScreen.GalleryItem item)
	{
		this.selectedGalleryItem = item;
		this.RefreshGallery();
		this.RefreshPreview();
	}

	// Token: 0x06009D2F RID: 40239 RVA: 0x0010AC6E File Offset: 0x00108E6E
	private void OnMouseOverToggle()
	{
		KFMOD.PlayUISound(GlobalAssets.GetSound("HUD_Mouseover", false));
	}

	// Token: 0x06009D30 RID: 40240 RVA: 0x0010AC80 File Offset: 0x00108E80
	public void RefreshPreview()
	{
		if (this.RefreshPreviewFn != null)
		{
			this.RefreshPreviewFn();
		}
	}

	// Token: 0x06009D31 RID: 40241 RVA: 0x0010AC95 File Offset: 0x00108E95
	public void PopulatePreview()
	{
		this.RefreshPreviewFn = (System.Action)Delegate.Combine(this.RefreshPreviewFn, new System.Action(delegate()
		{
			JoyResponseDesignerScreen.GalleryItem.BalloonArtistFacadeTarget balloonArtistFacadeTarget = this.selectedGalleryItem as JoyResponseDesignerScreen.GalleryItem.BalloonArtistFacadeTarget;
			if (balloonArtistFacadeTarget == null)
			{
				throw new NotImplementedException();
			}
			Option<PermitResource> permitResource = balloonArtistFacadeTarget.GetPermitResource();
			this.selectionHeaderLabel.SetText(balloonArtistFacadeTarget.GetName());
			KleiPermitDioramaVis_JoyResponseBalloon kleiPermitDioramaVis_JoyResponseBalloon = this.dioramaVis;
			JoyResponseScreenConfig config = this.Config;
			kleiPermitDioramaVis_JoyResponseBalloon.SetMinion(config.target.GetPersonality());
			this.dioramaVis.ConfigureWith(balloonArtistFacadeTarget.permit);
			OutfitDescriptionPanel outfitDescriptionPanel = this.outfitDescriptionPanel;
			PermitResource permitResource2 = permitResource.UnwrapOr(null, null);
			ClothingOutfitUtility.OutfitType outfitType = ClothingOutfitUtility.OutfitType.JoyResponse;
			config = this.Config;
			outfitDescriptionPanel.Refresh(permitResource2, outfitType, config.target.GetPersonality());
			Option<string> saveSelectionError = this.GetSaveSelectionError();
			if (saveSelectionError.IsSome())
			{
				this.primaryButton.isInteractable = false;
				this.primaryButton.gameObject.AddOrGet<ToolTip>().SetSimpleTooltip(saveSelectionError.Unwrap());
				return;
			}
			this.primaryButton.isInteractable = true;
			this.primaryButton.gameObject.AddOrGet<ToolTip>().ClearMultiStringTooltip();
		}));
		this.RefreshPreview();
	}

	// Token: 0x06009D32 RID: 40242 RVA: 0x0010ACBF File Offset: 0x00108EBF
	private void RegisterPreventScreenPop()
	{
		this.UnregisterPreventScreenPop();
		this.preventScreenPopFn = delegate()
		{
			if (this.Config.target.ReadFacadeId() != this.selectedGalleryItem.GetPermitResource().AndThen<string>((PermitResource r) => r.Id))
			{
				this.RegisterPreventScreenPop();
				JoyResponseDesignerScreen.MakeSaveWarningPopup(this.Config.target, delegate
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

	// Token: 0x06009D33 RID: 40243 RVA: 0x0010ACEE File Offset: 0x00108EEE
	private void UnregisterPreventScreenPop()
	{
		if (this.preventScreenPopFn != null)
		{
			LockerNavigator.Instance.preventScreenPop.Remove(this.preventScreenPopFn);
			this.preventScreenPopFn = null;
		}
	}

	// Token: 0x06009D34 RID: 40244 RVA: 0x003D7104 File Offset: 0x003D5304
	public static void MakeSaveWarningPopup(JoyResponseOutfitTarget target, System.Action discardChangesFn)
	{
		Action<InfoDialogScreen> <>9__1;
		LockerNavigator.Instance.ShowDialogPopup(delegate(InfoDialogScreen dialog)
		{
			InfoDialogScreen infoDialogScreen = dialog.SetHeader(UI.JOY_RESPONSE_DESIGNER_SCREEN.CHANGES_NOT_SAVED_WARNING_POPUP.HEADER.Replace("{MinionName}", target.GetMinionName())).AddPlainText(UI.OUTFIT_DESIGNER_SCREEN.CHANGES_NOT_SAVED_WARNING_POPUP.BODY);
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

	// Token: 0x06009D38 RID: 40248 RVA: 0x003D7218 File Offset: 0x003D5418
	[CompilerGenerated]
	private void <PopulateGallery>g__AddGridIcon|36_0(JoyResponseDesignerScreen.GalleryItem item)
	{
		GameObject gameObject = this.galleryGridItemPool.Borrow();
		HierarchyReferences component = gameObject.GetComponent<HierarchyReferences>();
		component.GetReference<Image>("Icon").sprite = item.GetIcon();
		component.GetReference<Image>("IsUnownedOverlay").gameObject.SetActive(!item.IsUnlocked());
		Option<PermitResource> permitResource = item.GetPermitResource();
		if (permitResource.IsSome())
		{
			KleiItemsUI.ConfigureTooltipOn(gameObject, KleiItemsUI.GetTooltipStringFor(permitResource.Unwrap()));
		}
		else
		{
			KleiItemsUI.ConfigureTooltipOn(gameObject, KleiItemsUI.GetNoneTooltipStringFor(PermitCategory.JoyResponse));
		}
		MultiToggle toggle = gameObject.GetComponent<MultiToggle>();
		MultiToggle toggle3 = toggle;
		toggle3.onEnter = (System.Action)Delegate.Combine(toggle3.onEnter, new System.Action(this.OnMouseOverToggle));
		MultiToggle toggle2 = toggle;
		toggle2.onClick = (System.Action)Delegate.Combine(toggle2.onClick, new System.Action(delegate()
		{
			this.SelectGalleryItem(item);
		}));
		this.RefreshGalleryFn = (System.Action)Delegate.Combine(this.RefreshGalleryFn, new System.Action(delegate()
		{
			toggle.ChangeState((item == this.selectedGalleryItem) ? 1 : 0);
		}));
	}

	// Token: 0x04007B5B RID: 31579
	[Header("CategoryColumn")]
	[SerializeField]
	private RectTransform categoryListContent;

	// Token: 0x04007B5C RID: 31580
	[SerializeField]
	private GameObject categoryRowPrefab;

	// Token: 0x04007B5D RID: 31581
	[Header("GalleryColumn")]
	[SerializeField]
	private LocText galleryHeaderLabel;

	// Token: 0x04007B5E RID: 31582
	[SerializeField]
	private RectTransform galleryGridContent;

	// Token: 0x04007B5F RID: 31583
	[SerializeField]
	private GameObject galleryItemPrefab;

	// Token: 0x04007B60 RID: 31584
	[Header("SelectionDetailsColumn")]
	[SerializeField]
	private LocText selectionHeaderLabel;

	// Token: 0x04007B61 RID: 31585
	[SerializeField]
	private KleiPermitDioramaVis_JoyResponseBalloon dioramaVis;

	// Token: 0x04007B62 RID: 31586
	[SerializeField]
	private OutfitDescriptionPanel outfitDescriptionPanel;

	// Token: 0x04007B63 RID: 31587
	[SerializeField]
	private KButton primaryButton;

	// Token: 0x04007B65 RID: 31589
	public JoyResponseDesignerScreen.JoyResponseCategory[] joyResponseCategories;

	// Token: 0x04007B66 RID: 31590
	private bool postponeConfiguration = true;

	// Token: 0x04007B67 RID: 31591
	private Option<JoyResponseDesignerScreen.JoyResponseCategory> selectedCategoryOpt;

	// Token: 0x04007B68 RID: 31592
	private UIPrefabLocalPool categoryRowPool;

	// Token: 0x04007B69 RID: 31593
	private System.Action RefreshCategoriesFn;

	// Token: 0x04007B6A RID: 31594
	private JoyResponseDesignerScreen.GalleryItem selectedGalleryItem;

	// Token: 0x04007B6B RID: 31595
	private UIPrefabLocalPool galleryGridItemPool;

	// Token: 0x04007B6C RID: 31596
	private GridLayouter galleryGridLayouter;

	// Token: 0x04007B6D RID: 31597
	private System.Action RefreshGalleryFn;

	// Token: 0x04007B6E RID: 31598
	public System.Action RefreshPreviewFn;

	// Token: 0x04007B6F RID: 31599
	private Func<bool> preventScreenPopFn;

	// Token: 0x02001D67 RID: 7527
	public class JoyResponseCategory
	{
		// Token: 0x04007B70 RID: 31600
		public string displayName;

		// Token: 0x04007B71 RID: 31601
		public Sprite icon;

		// Token: 0x04007B72 RID: 31602
		public JoyResponseDesignerScreen.GalleryItem[] items;
	}

	// Token: 0x02001D68 RID: 7528
	private enum MultiToggleState
	{
		// Token: 0x04007B74 RID: 31604
		Default,
		// Token: 0x04007B75 RID: 31605
		Selected
	}

	// Token: 0x02001D69 RID: 7529
	public abstract class GalleryItem : IEquatable<JoyResponseDesignerScreen.GalleryItem>
	{
		// Token: 0x06009D3D RID: 40253
		public abstract string GetName();

		// Token: 0x06009D3E RID: 40254
		public abstract Sprite GetIcon();

		// Token: 0x06009D3F RID: 40255
		public abstract string GetUniqueId();

		// Token: 0x06009D40 RID: 40256
		public abstract bool IsUnlocked();

		// Token: 0x06009D41 RID: 40257
		public abstract Option<PermitResource> GetPermitResource();

		// Token: 0x06009D42 RID: 40258 RVA: 0x003D74B8 File Offset: 0x003D56B8
		public override bool Equals(object obj)
		{
			JoyResponseDesignerScreen.GalleryItem galleryItem = obj as JoyResponseDesignerScreen.GalleryItem;
			return galleryItem != null && this.Equals(galleryItem);
		}

		// Token: 0x06009D43 RID: 40259 RVA: 0x0010AD45 File Offset: 0x00108F45
		public bool Equals(JoyResponseDesignerScreen.GalleryItem other)
		{
			return this.GetHashCode() == other.GetHashCode();
		}

		// Token: 0x06009D44 RID: 40260 RVA: 0x0010AD55 File Offset: 0x00108F55
		public override int GetHashCode()
		{
			return Hash.SDBMLower(this.GetUniqueId());
		}

		// Token: 0x06009D45 RID: 40261 RVA: 0x0010AD62 File Offset: 0x00108F62
		public override string ToString()
		{
			return this.GetUniqueId();
		}

		// Token: 0x06009D46 RID: 40262 RVA: 0x0010AD6A File Offset: 0x00108F6A
		public static JoyResponseDesignerScreen.GalleryItem.BalloonArtistFacadeTarget Of(Option<BalloonArtistFacadeResource> permit)
		{
			return new JoyResponseDesignerScreen.GalleryItem.BalloonArtistFacadeTarget
			{
				permit = permit
			};
		}

		// Token: 0x02001D6A RID: 7530
		public class BalloonArtistFacadeTarget : JoyResponseDesignerScreen.GalleryItem
		{
			// Token: 0x06009D48 RID: 40264 RVA: 0x003D74D8 File Offset: 0x003D56D8
			public override Sprite GetIcon()
			{
				return this.permit.AndThen<Sprite>((BalloonArtistFacadeResource p) => p.GetPermitPresentationInfo().sprite).UnwrapOrElse(() => KleiItemsUI.GetNoneBalloonArtistIcon(), null);
			}

			// Token: 0x06009D49 RID: 40265 RVA: 0x003D7538 File Offset: 0x003D5738
			public override string GetName()
			{
				return this.permit.AndThen<string>((BalloonArtistFacadeResource p) => p.Name).UnwrapOrElse(() => KleiItemsUI.GetNoneClothingItemStrings(PermitCategory.JoyResponse).Item1, null);
			}

			// Token: 0x06009D4A RID: 40266 RVA: 0x003D7598 File Offset: 0x003D5798
			public override string GetUniqueId()
			{
				return "balloon_artist_facade::" + this.permit.AndThen<string>((BalloonArtistFacadeResource p) => p.Id).UnwrapOr("<none>", null);
			}

			// Token: 0x06009D4B RID: 40267 RVA: 0x0010AD78 File Offset: 0x00108F78
			public override Option<PermitResource> GetPermitResource()
			{
				return this.permit.AndThen<PermitResource>((BalloonArtistFacadeResource p) => p);
			}

			// Token: 0x06009D4C RID: 40268 RVA: 0x003D75E8 File Offset: 0x003D57E8
			public override bool IsUnlocked()
			{
				return this.GetPermitResource().AndThen<bool>((PermitResource p) => p.IsUnlocked()).UnwrapOr(true, null);
			}

			// Token: 0x04007B76 RID: 31606
			public Option<BalloonArtistFacadeResource> permit;
		}
	}
}
