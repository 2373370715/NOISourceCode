using System;
using System.Linq;
using System.Runtime.CompilerServices;
using Database;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02001E77 RID: 7799
public class MinionBrowserScreen : KMonoBehaviour
{
	// Token: 0x0600A352 RID: 41810 RVA: 0x003EF814 File Offset: 0x003EDA14
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.gridLayouter = new GridLayouter
		{
			minCellSize = 112f,
			maxCellSize = 144f,
			targetGridLayouts = this.galleryGridContent.GetComponents<GridLayoutGroup>().ToList<GridLayoutGroup>()
		};
		this.galleryGridItemPool = new UIPrefabLocalPool(this.gridItemPrefab, this.galleryGridContent.gameObject);
	}

	// Token: 0x0600A353 RID: 41811 RVA: 0x003EF87C File Offset: 0x003EDA7C
	protected override void OnCmpEnable()
	{
		if (this.isFirstDisplay)
		{
			this.isFirstDisplay = false;
			this.PopulateGallery();
			this.RefreshPreview();
			this.cycler.Initialize(this.CreateCycleOptions());
			this.editButton.onClick += delegate()
			{
				if (this.OnEditClickedFn != null)
				{
					this.OnEditClickedFn();
				}
			};
			this.changeOutfitButton.onClick += this.OnClickChangeOutfit;
		}
		else
		{
			this.RefreshGallery();
			this.RefreshPreview();
		}
		KleiItemsStatusRefresher.AddOrGetListener(this).OnRefreshUI(delegate
		{
			this.RefreshGallery();
			this.RefreshPreview();
		});
	}

	// Token: 0x0600A354 RID: 41812 RVA: 0x0010E952 File Offset: 0x0010CB52
	private void Update()
	{
		this.gridLayouter.CheckIfShouldResizeGrid();
	}

	// Token: 0x0600A355 RID: 41813 RVA: 0x003EF908 File Offset: 0x003EDB08
	protected override void OnSpawn()
	{
		this.postponeConfiguration = false;
		if (this.Config.isValid)
		{
			this.Configure(this.Config);
			return;
		}
		this.Configure(MinionBrowserScreenConfig.Personalities(default(Option<Personality>)));
	}

	// Token: 0x17000A8E RID: 2702
	// (get) Token: 0x0600A356 RID: 41814 RVA: 0x0010E95F File Offset: 0x0010CB5F
	// (set) Token: 0x0600A357 RID: 41815 RVA: 0x0010E967 File Offset: 0x0010CB67
	public MinionBrowserScreenConfig Config { get; private set; }

	// Token: 0x0600A358 RID: 41816 RVA: 0x0010E970 File Offset: 0x0010CB70
	public void Configure(MinionBrowserScreenConfig config)
	{
		this.Config = config;
		if (this.postponeConfiguration)
		{
			return;
		}
		this.PopulateGallery();
		this.RefreshPreview();
	}

	// Token: 0x0600A359 RID: 41817 RVA: 0x0010E98E File Offset: 0x0010CB8E
	private void RefreshGallery()
	{
		if (this.RefreshGalleryFn != null)
		{
			this.RefreshGalleryFn();
		}
	}

	// Token: 0x0600A35A RID: 41818 RVA: 0x003EF94C File Offset: 0x003EDB4C
	public void PopulateGallery()
	{
		this.RefreshGalleryFn = null;
		this.galleryGridItemPool.ReturnAll();
		foreach (MinionBrowserScreen.GridItem item in this.Config.items)
		{
			this.<PopulateGallery>g__AddGridIcon|32_0(item);
		}
		this.RefreshGallery();
		this.SelectMinion(this.Config.defaultSelectedItem.Unwrap());
	}

	// Token: 0x0600A35B RID: 41819 RVA: 0x003EF9B0 File Offset: 0x003EDBB0
	private void SelectMinion(MinionBrowserScreen.GridItem item)
	{
		this.selectedGridItem = item;
		this.RefreshGallery();
		this.RefreshPreview();
		this.UIMinion.GetMinionVoice().PlaySoundUI("voice_land");
	}

	// Token: 0x0600A35C RID: 41820 RVA: 0x003EF9E8 File Offset: 0x003EDBE8
	public void RefreshPreview()
	{
		this.UIMinion.SetMinion(this.selectedGridItem.GetPersonality());
		this.UIMinion.ReactToPersonalityChange();
		this.detailsHeaderText.SetText(this.selectedGridItem.GetName());
		this.detailHeaderIcon.sprite = this.selectedGridItem.GetIcon();
		this.RefreshOutfitDescription();
		this.RefreshPreviewButtonsInteractable();
		this.SetDioramaBG();
	}

	// Token: 0x0600A35D RID: 41821 RVA: 0x0010E9A3 File Offset: 0x0010CBA3
	private void RefreshOutfitDescription()
	{
		if (this.RefreshOutfitDescriptionFn != null)
		{
			this.RefreshOutfitDescriptionFn();
		}
	}

	// Token: 0x0600A35E RID: 41822 RVA: 0x003EFA54 File Offset: 0x003EDC54
	private void OnClickChangeOutfit()
	{
		if (this.selectedOutfitType.IsNone())
		{
			return;
		}
		OutfitBrowserScreenConfig.Minion(this.selectedOutfitType.Unwrap(), this.selectedGridItem).WithOutfit(this.selectedOutfit).ApplyAndOpenScreen();
	}

	// Token: 0x0600A35F RID: 41823 RVA: 0x003EFA9C File Offset: 0x003EDC9C
	private void RefreshPreviewButtonsInteractable()
	{
		this.editButton.isInteractable = true;
		if (this.currentOutfitType == ClothingOutfitUtility.OutfitType.JoyResponse)
		{
			Option<string> joyResponseEditError = this.GetJoyResponseEditError();
			if (joyResponseEditError.IsSome())
			{
				this.editButton.isInteractable = false;
				this.editButton.gameObject.AddOrGet<ToolTip>().SetSimpleTooltip(joyResponseEditError.Unwrap());
				return;
			}
			this.editButton.isInteractable = true;
			this.editButton.gameObject.AddOrGet<ToolTip>().ClearMultiStringTooltip();
		}
	}

	// Token: 0x0600A360 RID: 41824 RVA: 0x0010E9B8 File Offset: 0x0010CBB8
	private void SetDioramaBG()
	{
		this.dioramaBGImage.sprite = KleiPermitDioramaVis.GetDioramaBackground(this.currentOutfitType);
	}

	// Token: 0x0600A361 RID: 41825 RVA: 0x003EFB18 File Offset: 0x003EDD18
	private Option<string> GetJoyResponseEditError()
	{
		string joyTrait = this.selectedGridItem.GetPersonality().joyTrait;
		if (!(joyTrait == "BalloonArtist"))
		{
			return Option.Some<string>(UI.JOY_RESPONSE_DESIGNER_SCREEN.TOOLTIP_NO_FACADES_FOR_JOY_TRAIT.Replace("{JoyResponseType}", Db.Get().traits.Get(joyTrait).Name));
		}
		return Option.None;
	}

	// Token: 0x0600A362 RID: 41826 RVA: 0x003EFB78 File Offset: 0x003EDD78
	public void SetEditingOutfitType(ClothingOutfitUtility.OutfitType outfitType)
	{
		this.currentOutfitType = outfitType;
		switch (outfitType)
		{
		case ClothingOutfitUtility.OutfitType.Clothing:
			this.editButtonText.text = UI.MINION_BROWSER_SCREEN.BUTTON_EDIT_OUTFIT_ITEMS;
			this.changeOutfitButton.gameObject.SetActive(true);
			break;
		case ClothingOutfitUtility.OutfitType.JoyResponse:
			this.editButtonText.text = UI.MINION_BROWSER_SCREEN.BUTTON_EDIT_JOY_RESPONSE;
			this.changeOutfitButton.gameObject.SetActive(false);
			break;
		case ClothingOutfitUtility.OutfitType.AtmoSuit:
			this.editButtonText.text = UI.MINION_BROWSER_SCREEN.BUTTON_EDIT_ATMO_SUIT_OUTFIT_ITEMS;
			this.changeOutfitButton.gameObject.SetActive(true);
			break;
		default:
			throw new NotImplementedException();
		}
		this.RefreshPreviewButtonsInteractable();
		this.OnEditClickedFn = delegate()
		{
			switch (outfitType)
			{
			case ClothingOutfitUtility.OutfitType.Clothing:
			case ClothingOutfitUtility.OutfitType.AtmoSuit:
				OutfitDesignerScreenConfig.Minion(this.selectedOutfit.IsSome() ? this.selectedOutfit.Unwrap() : ClothingOutfitTarget.ForNewTemplateOutfit(outfitType), this.selectedGridItem).ApplyAndOpenScreen();
				return;
			case ClothingOutfitUtility.OutfitType.JoyResponse:
			{
				JoyResponseScreenConfig joyResponseScreenConfig = JoyResponseScreenConfig.From(this.selectedGridItem);
				joyResponseScreenConfig = joyResponseScreenConfig.WithInitialSelection(this.selectedGridItem.GetJoyResponseOutfitTarget().ReadFacadeId().AndThen<BalloonArtistFacadeResource>((string id) => Db.Get().Permits.BalloonArtistFacades.Get(id)));
				joyResponseScreenConfig.ApplyAndOpenScreen();
				return;
			}
			default:
				throw new NotImplementedException();
			}
		};
		this.RefreshOutfitDescriptionFn = delegate()
		{
			switch (outfitType)
			{
			case ClothingOutfitUtility.OutfitType.Clothing:
			case ClothingOutfitUtility.OutfitType.AtmoSuit:
				this.selectedOutfit = this.selectedGridItem.GetClothingOutfitTarget(outfitType);
				this.UIMinion.SetOutfit(outfitType, this.selectedOutfit);
				this.outfitDescriptionPanel.Refresh(this.selectedOutfit, outfitType, this.selectedGridItem.GetPersonality());
				return;
			case ClothingOutfitUtility.OutfitType.JoyResponse:
			{
				this.selectedOutfit = this.selectedGridItem.GetClothingOutfitTarget(ClothingOutfitUtility.OutfitType.Clothing);
				this.UIMinion.SetOutfit(ClothingOutfitUtility.OutfitType.Clothing, this.selectedOutfit);
				string text = this.selectedGridItem.GetJoyResponseOutfitTarget().ReadFacadeId().UnwrapOr(null, null);
				this.outfitDescriptionPanel.Refresh((text != null) ? Db.Get().Permits.Get(text) : null, outfitType, this.selectedGridItem.GetPersonality());
				return;
			}
			default:
				throw new NotImplementedException();
			}
		};
		this.RefreshOutfitDescription();
	}

	// Token: 0x0600A363 RID: 41827 RVA: 0x003EFC70 File Offset: 0x003EDE70
	private MinionBrowserScreen.CyclerUI.OnSelectedFn[] CreateCycleOptions()
	{
		MinionBrowserScreen.CyclerUI.OnSelectedFn[] array = new MinionBrowserScreen.CyclerUI.OnSelectedFn[3];
		for (int i = 0; i < 3; i++)
		{
			ClothingOutfitUtility.OutfitType outfitType = (ClothingOutfitUtility.OutfitType)i;
			array[i] = delegate()
			{
				this.selectedOutfitType = Option.Some<ClothingOutfitUtility.OutfitType>(outfitType);
				this.cycler.SetLabel(outfitType.GetName());
				this.SetEditingOutfitType(outfitType);
				this.RefreshPreview();
			};
		}
		return array;
	}

	// Token: 0x0600A364 RID: 41828 RVA: 0x0010AC6E File Offset: 0x00108E6E
	private void OnMouseOverToggle()
	{
		KFMOD.PlayUISound(GlobalAssets.GetSound("HUD_Mouseover", false));
	}

	// Token: 0x0600A368 RID: 41832 RVA: 0x003EFCB4 File Offset: 0x003EDEB4
	[CompilerGenerated]
	private void <PopulateGallery>g__AddGridIcon|32_0(MinionBrowserScreen.GridItem item)
	{
		GameObject gameObject = this.galleryGridItemPool.Borrow();
		gameObject.GetComponent<HierarchyReferences>().GetReference<Image>("Icon").sprite = item.GetIcon();
		gameObject.GetComponent<HierarchyReferences>().GetReference<LocText>("Label").SetText(item.GetName());
		string requiredDlcId = item.GetPersonality().requiredDlcId;
		ToolTip component = gameObject.GetComponent<ToolTip>();
		Image component2 = gameObject.transform.Find("DlcBanner").GetComponent<Image>();
		if (DlcManager.IsDlcId(requiredDlcId))
		{
			component2.gameObject.SetActive(true);
			component2.color = DlcManager.GetDlcBannerColor(requiredDlcId);
			component.SetSimpleTooltip(string.Format(UI.MINION_BROWSER_SCREEN.TOOLTIP_FROM_DLC, DlcManager.GetDlcTitle(requiredDlcId)));
		}
		else
		{
			component2.gameObject.SetActive(false);
			component.ClearMultiStringTooltip();
		}
		MultiToggle toggle = gameObject.GetComponent<MultiToggle>();
		MultiToggle toggle3 = toggle;
		toggle3.onEnter = (System.Action)Delegate.Combine(toggle3.onEnter, new System.Action(this.OnMouseOverToggle));
		MultiToggle toggle2 = toggle;
		toggle2.onClick = (System.Action)Delegate.Combine(toggle2.onClick, new System.Action(delegate()
		{
			this.SelectMinion(item);
		}));
		this.RefreshGalleryFn = (System.Action)Delegate.Combine(this.RefreshGalleryFn, new System.Action(delegate()
		{
			toggle.ChangeState((item == this.selectedGridItem) ? 1 : 0);
		}));
	}

	// Token: 0x04007FAD RID: 32685
	[Header("ItemGalleryColumn")]
	[SerializeField]
	private RectTransform galleryGridContent;

	// Token: 0x04007FAE RID: 32686
	[SerializeField]
	private GameObject gridItemPrefab;

	// Token: 0x04007FAF RID: 32687
	private GridLayouter gridLayouter;

	// Token: 0x04007FB0 RID: 32688
	[Header("SelectionDetailsColumn")]
	[SerializeField]
	private KleiPermitDioramaVis permitVis;

	// Token: 0x04007FB1 RID: 32689
	[SerializeField]
	private UIMinion UIMinion;

	// Token: 0x04007FB2 RID: 32690
	[SerializeField]
	private LocText detailsHeaderText;

	// Token: 0x04007FB3 RID: 32691
	[SerializeField]
	private Image detailHeaderIcon;

	// Token: 0x04007FB4 RID: 32692
	[SerializeField]
	private OutfitDescriptionPanel outfitDescriptionPanel;

	// Token: 0x04007FB5 RID: 32693
	[SerializeField]
	private MinionBrowserScreen.CyclerUI cycler;

	// Token: 0x04007FB6 RID: 32694
	[SerializeField]
	private KButton editButton;

	// Token: 0x04007FB7 RID: 32695
	[SerializeField]
	private LocText editButtonText;

	// Token: 0x04007FB8 RID: 32696
	[SerializeField]
	private KButton changeOutfitButton;

	// Token: 0x04007FB9 RID: 32697
	private Option<ClothingOutfitUtility.OutfitType> selectedOutfitType;

	// Token: 0x04007FBA RID: 32698
	private Option<ClothingOutfitTarget> selectedOutfit;

	// Token: 0x04007FBB RID: 32699
	[Header("Diorama Backgrounds")]
	[SerializeField]
	private Image dioramaBGImage;

	// Token: 0x04007FBC RID: 32700
	private MinionBrowserScreen.GridItem selectedGridItem;

	// Token: 0x04007FBD RID: 32701
	private System.Action OnEditClickedFn;

	// Token: 0x04007FBE RID: 32702
	private bool isFirstDisplay = true;

	// Token: 0x04007FC0 RID: 32704
	private bool postponeConfiguration = true;

	// Token: 0x04007FC1 RID: 32705
	private UIPrefabLocalPool galleryGridItemPool;

	// Token: 0x04007FC2 RID: 32706
	private System.Action RefreshGalleryFn;

	// Token: 0x04007FC3 RID: 32707
	private System.Action RefreshOutfitDescriptionFn;

	// Token: 0x04007FC4 RID: 32708
	private ClothingOutfitUtility.OutfitType currentOutfitType;

	// Token: 0x02001E78 RID: 7800
	private enum MultiToggleState
	{
		// Token: 0x04007FC6 RID: 32710
		Default,
		// Token: 0x04007FC7 RID: 32711
		Selected,
		// Token: 0x04007FC8 RID: 32712
		NonInteractable
	}

	// Token: 0x02001E79 RID: 7801
	[Serializable]
	public class CyclerUI
	{
		// Token: 0x0600A369 RID: 41833 RVA: 0x0010EA09 File Offset: 0x0010CC09
		public void Initialize(MinionBrowserScreen.CyclerUI.OnSelectedFn[] cycleOptions)
		{
			this.cyclePrevButton.onClick += this.CyclePrev;
			this.cycleNextButton.onClick += this.CycleNext;
			this.SetCycleOptions(cycleOptions);
		}

		// Token: 0x0600A36A RID: 41834 RVA: 0x0010EA40 File Offset: 0x0010CC40
		public void SetCycleOptions(MinionBrowserScreen.CyclerUI.OnSelectedFn[] cycleOptions)
		{
			DebugUtil.Assert(cycleOptions != null);
			DebugUtil.Assert(cycleOptions.Length != 0);
			this.cycleOptions = cycleOptions;
			this.GoTo(0);
		}

		// Token: 0x0600A36B RID: 41835 RVA: 0x003EFE20 File Offset: 0x003EE020
		public void GoTo(int wrappingIndex)
		{
			if (this.cycleOptions == null || this.cycleOptions.Length == 0)
			{
				return;
			}
			while (wrappingIndex < 0)
			{
				wrappingIndex += this.cycleOptions.Length;
			}
			while (wrappingIndex >= this.cycleOptions.Length)
			{
				wrappingIndex -= this.cycleOptions.Length;
			}
			this.selectedIndex = wrappingIndex;
			this.cycleOptions[this.selectedIndex]();
		}

		// Token: 0x0600A36C RID: 41836 RVA: 0x0010EA63 File Offset: 0x0010CC63
		public void CyclePrev()
		{
			this.GoTo(this.selectedIndex - 1);
		}

		// Token: 0x0600A36D RID: 41837 RVA: 0x0010EA73 File Offset: 0x0010CC73
		public void CycleNext()
		{
			this.GoTo(this.selectedIndex + 1);
		}

		// Token: 0x0600A36E RID: 41838 RVA: 0x0010EA83 File Offset: 0x0010CC83
		public void SetLabel(string text)
		{
			this.currentLabel.text = text;
		}

		// Token: 0x04007FC9 RID: 32713
		[SerializeField]
		public KButton cyclePrevButton;

		// Token: 0x04007FCA RID: 32714
		[SerializeField]
		public KButton cycleNextButton;

		// Token: 0x04007FCB RID: 32715
		[SerializeField]
		public LocText currentLabel;

		// Token: 0x04007FCC RID: 32716
		[NonSerialized]
		private int selectedIndex = -1;

		// Token: 0x04007FCD RID: 32717
		[NonSerialized]
		private MinionBrowserScreen.CyclerUI.OnSelectedFn[] cycleOptions;

		// Token: 0x02001E7A RID: 7802
		// (Invoke) Token: 0x0600A371 RID: 41841
		public delegate void OnSelectedFn();
	}

	// Token: 0x02001E7B RID: 7803
	public abstract class GridItem : IEquatable<MinionBrowserScreen.GridItem>
	{
		// Token: 0x0600A374 RID: 41844
		public abstract string GetName();

		// Token: 0x0600A375 RID: 41845
		public abstract Sprite GetIcon();

		// Token: 0x0600A376 RID: 41846
		public abstract string GetUniqueId();

		// Token: 0x0600A377 RID: 41847
		public abstract Personality GetPersonality();

		// Token: 0x0600A378 RID: 41848
		public abstract Option<ClothingOutfitTarget> GetClothingOutfitTarget(ClothingOutfitUtility.OutfitType outfitType);

		// Token: 0x0600A379 RID: 41849
		public abstract JoyResponseOutfitTarget GetJoyResponseOutfitTarget();

		// Token: 0x0600A37A RID: 41850 RVA: 0x003EFE84 File Offset: 0x003EE084
		public override bool Equals(object obj)
		{
			MinionBrowserScreen.GridItem gridItem = obj as MinionBrowserScreen.GridItem;
			return gridItem != null && this.Equals(gridItem);
		}

		// Token: 0x0600A37B RID: 41851 RVA: 0x0010AD45 File Offset: 0x00108F45
		public bool Equals(MinionBrowserScreen.GridItem other)
		{
			return this.GetHashCode() == other.GetHashCode();
		}

		// Token: 0x0600A37C RID: 41852 RVA: 0x0010EAA0 File Offset: 0x0010CCA0
		public override int GetHashCode()
		{
			return Hash.SDBMLower(this.GetUniqueId());
		}

		// Token: 0x0600A37D RID: 41853 RVA: 0x0010EAAD File Offset: 0x0010CCAD
		public override string ToString()
		{
			return this.GetUniqueId();
		}

		// Token: 0x0600A37E RID: 41854 RVA: 0x003EFEA4 File Offset: 0x003EE0A4
		public static MinionBrowserScreen.GridItem.MinionInstanceTarget Of(GameObject minionInstance)
		{
			MinionIdentity component = minionInstance.GetComponent<MinionIdentity>();
			return new MinionBrowserScreen.GridItem.MinionInstanceTarget
			{
				minionInstance = minionInstance,
				minionIdentity = component,
				personality = Db.Get().Personalities.Get(component.personalityResourceId)
			};
		}

		// Token: 0x0600A37F RID: 41855 RVA: 0x0010EAB5 File Offset: 0x0010CCB5
		public static MinionBrowserScreen.GridItem.PersonalityTarget Of(Personality personality)
		{
			return new MinionBrowserScreen.GridItem.PersonalityTarget
			{
				personality = personality
			};
		}

		// Token: 0x02001E7C RID: 7804
		public class MinionInstanceTarget : MinionBrowserScreen.GridItem
		{
			// Token: 0x0600A381 RID: 41857 RVA: 0x0010EAC3 File Offset: 0x0010CCC3
			public override Sprite GetIcon()
			{
				return this.personality.GetMiniIcon();
			}

			// Token: 0x0600A382 RID: 41858 RVA: 0x0010EAD0 File Offset: 0x0010CCD0
			public override string GetName()
			{
				return this.minionIdentity.GetProperName();
			}

			// Token: 0x0600A383 RID: 41859 RVA: 0x003EFEE8 File Offset: 0x003EE0E8
			public override string GetUniqueId()
			{
				return "minion_instance_id::" + this.minionInstance.GetInstanceID().ToString();
			}

			// Token: 0x0600A384 RID: 41860 RVA: 0x0010EADD File Offset: 0x0010CCDD
			public override Personality GetPersonality()
			{
				return this.personality;
			}

			// Token: 0x0600A385 RID: 41861 RVA: 0x0010EAE5 File Offset: 0x0010CCE5
			public override Option<ClothingOutfitTarget> GetClothingOutfitTarget(ClothingOutfitUtility.OutfitType outfitType)
			{
				return ClothingOutfitTarget.FromMinion(outfitType, this.minionInstance);
			}

			// Token: 0x0600A386 RID: 41862 RVA: 0x0010EAF8 File Offset: 0x0010CCF8
			public override JoyResponseOutfitTarget GetJoyResponseOutfitTarget()
			{
				return JoyResponseOutfitTarget.FromMinion(this.minionInstance);
			}

			// Token: 0x04007FCE RID: 32718
			public GameObject minionInstance;

			// Token: 0x04007FCF RID: 32719
			public MinionIdentity minionIdentity;

			// Token: 0x04007FD0 RID: 32720
			public Personality personality;
		}

		// Token: 0x02001E7D RID: 7805
		public class PersonalityTarget : MinionBrowserScreen.GridItem
		{
			// Token: 0x0600A388 RID: 41864 RVA: 0x0010EB0D File Offset: 0x0010CD0D
			public override Sprite GetIcon()
			{
				return this.personality.GetMiniIcon();
			}

			// Token: 0x0600A389 RID: 41865 RVA: 0x0010EB1A File Offset: 0x0010CD1A
			public override string GetName()
			{
				return this.personality.Name;
			}

			// Token: 0x0600A38A RID: 41866 RVA: 0x0010EB27 File Offset: 0x0010CD27
			public override string GetUniqueId()
			{
				return "personality::" + this.personality.nameStringKey;
			}

			// Token: 0x0600A38B RID: 41867 RVA: 0x0010EB3E File Offset: 0x0010CD3E
			public override Personality GetPersonality()
			{
				return this.personality;
			}

			// Token: 0x0600A38C RID: 41868 RVA: 0x0010EB46 File Offset: 0x0010CD46
			public override Option<ClothingOutfitTarget> GetClothingOutfitTarget(ClothingOutfitUtility.OutfitType outfitType)
			{
				return ClothingOutfitTarget.TryFromTemplateId(this.personality.GetSelectedTemplateOutfitId(outfitType));
			}

			// Token: 0x0600A38D RID: 41869 RVA: 0x0010EB59 File Offset: 0x0010CD59
			public override JoyResponseOutfitTarget GetJoyResponseOutfitTarget()
			{
				return JoyResponseOutfitTarget.FromPersonality(this.personality);
			}

			// Token: 0x04007FD1 RID: 32721
			public Personality personality;
		}
	}
}
