using System;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02001ECE RID: 7886
[Serializable]
public class OutfitBrowserScreen_CategoriesAndSearchBar
{
	// Token: 0x0600A57B RID: 42363 RVA: 0x003F9B24 File Offset: 0x003F7D24
	public void InitializeWith(OutfitBrowserScreen outfitBrowserScreen)
	{
		this.outfitBrowserScreen = outfitBrowserScreen;
		this.clothingOutfitTypeButton = new OutfitBrowserScreen_CategoriesAndSearchBar.SelectOutfitTypeButton(outfitBrowserScreen, Util.KInstantiateUI(this.selectOutfitType_Prefab.gameObject, this.selectOutfitType_Prefab.transform.parent.gameObject, true));
		this.clothingOutfitTypeButton.button.onClick += delegate()
		{
			this.SetOutfitType(ClothingOutfitUtility.OutfitType.Clothing);
		};
		this.clothingOutfitTypeButton.icon.sprite = Assets.GetSprite("icon_inventory_equipment");
		KleiItemsUI.ConfigureTooltipOn(this.clothingOutfitTypeButton.button.gameObject, UI.OUTFIT_BROWSER_SCREEN.TOOLTIP_FILTER_BY_CLOTHING);
		this.atmosuitOutfitTypeButton = new OutfitBrowserScreen_CategoriesAndSearchBar.SelectOutfitTypeButton(outfitBrowserScreen, Util.KInstantiateUI(this.selectOutfitType_Prefab.gameObject, this.selectOutfitType_Prefab.transform.parent.gameObject, true));
		this.atmosuitOutfitTypeButton.button.onClick += delegate()
		{
			this.SetOutfitType(ClothingOutfitUtility.OutfitType.AtmoSuit);
		};
		this.atmosuitOutfitTypeButton.icon.sprite = Assets.GetSprite("icon_inventory_atmosuits");
		KleiItemsUI.ConfigureTooltipOn(this.atmosuitOutfitTypeButton.button.gameObject, UI.OUTFIT_BROWSER_SCREEN.TOOLTIP_FILTER_BY_ATMO_SUITS);
		this.searchTextField.onValueChanged.AddListener(delegate(string newFilter)
		{
			outfitBrowserScreen.state.Filter = newFilter;
		});
		this.searchTextField.transform.SetAsLastSibling();
		outfitBrowserScreen.state.OnCurrentOutfitTypeChanged += delegate()
		{
			if (outfitBrowserScreen.Config.onlyShowOutfitType.IsSome())
			{
				this.clothingOutfitTypeButton.root.gameObject.SetActive(false);
				this.atmosuitOutfitTypeButton.root.gameObject.SetActive(false);
				return;
			}
			this.clothingOutfitTypeButton.root.gameObject.SetActive(true);
			this.atmosuitOutfitTypeButton.root.gameObject.SetActive(true);
			this.clothingOutfitTypeButton.SetState(OutfitBrowserScreen_CategoriesAndSearchBar.SelectOutfitTypeButtonState.Unselected);
			this.atmosuitOutfitTypeButton.SetState(OutfitBrowserScreen_CategoriesAndSearchBar.SelectOutfitTypeButtonState.Unselected);
			ClothingOutfitUtility.OutfitType currentOutfitType = outfitBrowserScreen.state.CurrentOutfitType;
			if (currentOutfitType == ClothingOutfitUtility.OutfitType.Clothing)
			{
				this.clothingOutfitTypeButton.SetState(OutfitBrowserScreen_CategoriesAndSearchBar.SelectOutfitTypeButtonState.Selected);
				return;
			}
			if (currentOutfitType != ClothingOutfitUtility.OutfitType.AtmoSuit)
			{
				throw new NotImplementedException();
			}
			this.atmosuitOutfitTypeButton.SetState(OutfitBrowserScreen_CategoriesAndSearchBar.SelectOutfitTypeButtonState.Selected);
		};
	}

	// Token: 0x0600A57C RID: 42364 RVA: 0x0010FCAB File Offset: 0x0010DEAB
	public void SetOutfitType(ClothingOutfitUtility.OutfitType outfitType)
	{
		this.outfitBrowserScreen.state.CurrentOutfitType = outfitType;
	}

	// Token: 0x04008177 RID: 33143
	[NonSerialized]
	public OutfitBrowserScreen_CategoriesAndSearchBar.SelectOutfitTypeButton clothingOutfitTypeButton;

	// Token: 0x04008178 RID: 33144
	[NonSerialized]
	public OutfitBrowserScreen_CategoriesAndSearchBar.SelectOutfitTypeButton atmosuitOutfitTypeButton;

	// Token: 0x04008179 RID: 33145
	[NonSerialized]
	public OutfitBrowserScreen outfitBrowserScreen;

	// Token: 0x0400817A RID: 33146
	public KButton selectOutfitType_Prefab;

	// Token: 0x0400817B RID: 33147
	public KInputTextField searchTextField;

	// Token: 0x02001ECF RID: 7887
	public enum SelectOutfitTypeButtonState
	{
		// Token: 0x0400817D RID: 33149
		Disabled,
		// Token: 0x0400817E RID: 33150
		Unselected,
		// Token: 0x0400817F RID: 33151
		Selected
	}

	// Token: 0x02001ED0 RID: 7888
	public readonly struct SelectOutfitTypeButton
	{
		// Token: 0x0600A57E RID: 42366 RVA: 0x003F9CBC File Offset: 0x003F7EBC
		public SelectOutfitTypeButton(OutfitBrowserScreen outfitBrowserScreen, GameObject rootGameObject)
		{
			this.outfitBrowserScreen = outfitBrowserScreen;
			this.root = rootGameObject.GetComponent<RectTransform>();
			this.button = rootGameObject.GetComponent<KButton>();
			this.buttonImage = rootGameObject.GetComponent<KImage>();
			this.icon = this.root.GetChild(0).GetComponent<Image>();
			global::Debug.Assert(this.root != null);
			global::Debug.Assert(this.button != null);
			global::Debug.Assert(this.buttonImage != null);
			global::Debug.Assert(this.icon != null);
		}

		// Token: 0x0600A57F RID: 42367 RVA: 0x003F9D50 File Offset: 0x003F7F50
		public void SetState(OutfitBrowserScreen_CategoriesAndSearchBar.SelectOutfitTypeButtonState state)
		{
			switch (state)
			{
			case OutfitBrowserScreen_CategoriesAndSearchBar.SelectOutfitTypeButtonState.Disabled:
				this.button.isInteractable = false;
				this.buttonImage.colorStyleSetting = this.outfitBrowserScreen.notSelectedCategoryStyle;
				this.buttonImage.ApplyColorStyleSetting();
				return;
			case OutfitBrowserScreen_CategoriesAndSearchBar.SelectOutfitTypeButtonState.Unselected:
				this.button.isInteractable = true;
				this.buttonImage.colorStyleSetting = this.outfitBrowserScreen.notSelectedCategoryStyle;
				this.buttonImage.ApplyColorStyleSetting();
				return;
			case OutfitBrowserScreen_CategoriesAndSearchBar.SelectOutfitTypeButtonState.Selected:
				this.button.isInteractable = true;
				this.buttonImage.colorStyleSetting = this.outfitBrowserScreen.selectedCategoryStyle;
				this.buttonImage.ApplyColorStyleSetting();
				return;
			default:
				throw new NotImplementedException();
			}
		}

		// Token: 0x04008180 RID: 33152
		public readonly OutfitBrowserScreen outfitBrowserScreen;

		// Token: 0x04008181 RID: 33153
		public readonly RectTransform root;

		// Token: 0x04008182 RID: 33154
		public readonly KButton button;

		// Token: 0x04008183 RID: 33155
		public readonly KImage buttonImage;

		// Token: 0x04008184 RID: 33156
		public readonly Image icon;
	}
}
