using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Database;

// Token: 0x020020AB RID: 8363
public static class UIMinionOrMannequinITargetExtensions
{
	// Token: 0x0600B253 RID: 45651 RVA: 0x001186DB File Offset: 0x001168DB
	public static void SetOutfit(this UIMinionOrMannequin.ITarget self, ClothingOutfitResource outfit)
	{
		self.SetOutfit(outfit.outfitType, from itemId in outfit.itemsInOutfit
		select Db.Get().Permits.ClothingItems.Get(itemId));
	}

	// Token: 0x0600B254 RID: 45652 RVA: 0x00118713 File Offset: 0x00116913
	public static void SetOutfit(this UIMinionOrMannequin.ITarget self, OutfitDesignerScreen_OutfitState outfit)
	{
		self.SetOutfit(outfit.outfitType, from itemId in outfit.GetItems()
		select Db.Get().Permits.ClothingItems.Get(itemId));
	}

	// Token: 0x0600B255 RID: 45653 RVA: 0x0011874B File Offset: 0x0011694B
	public static void SetOutfit(this UIMinionOrMannequin.ITarget self, ClothingOutfitTarget outfit)
	{
		self.SetOutfit(outfit.OutfitType, outfit.ReadItemValues());
	}

	// Token: 0x0600B256 RID: 45654 RVA: 0x00118761 File Offset: 0x00116961
	public static void SetOutfit(this UIMinionOrMannequin.ITarget self, ClothingOutfitUtility.OutfitType outfitType, Option<ClothingOutfitTarget> outfit)
	{
		if (outfit.HasValue)
		{
			self.SetOutfit(outfit.Value);
			return;
		}
		self.ClearOutfit(outfitType);
	}

	// Token: 0x0600B257 RID: 45655 RVA: 0x00118781 File Offset: 0x00116981
	public static void ClearOutfit(this UIMinionOrMannequin.ITarget self, ClothingOutfitUtility.OutfitType outfitType)
	{
		self.SetOutfit(outfitType, UIMinionOrMannequinITargetExtensions.EMPTY_OUTFIT);
	}

	// Token: 0x0600B258 RID: 45656 RVA: 0x0011878F File Offset: 0x0011698F
	public static void React(this UIMinionOrMannequin.ITarget self)
	{
		self.React(UIMinionOrMannequinReactSource.None);
	}

	// Token: 0x0600B259 RID: 45657 RVA: 0x00118798 File Offset: 0x00116998
	public static void ReactToClothingItemChange(this UIMinionOrMannequin.ITarget self, PermitCategory clothingChangedCategory)
	{
		self.React(UIMinionOrMannequinITargetExtensions.<ReactToClothingItemChange>g__GetSource|7_0(clothingChangedCategory));
	}

	// Token: 0x0600B25A RID: 45658 RVA: 0x001187A6 File Offset: 0x001169A6
	public static void ReactToPersonalityChange(this UIMinionOrMannequin.ITarget self)
	{
		self.React(UIMinionOrMannequinReactSource.OnPersonalityChanged);
	}

	// Token: 0x0600B25B RID: 45659 RVA: 0x001187AF File Offset: 0x001169AF
	public static void ReactToFullOutfitChange(this UIMinionOrMannequin.ITarget self)
	{
		self.React(UIMinionOrMannequinReactSource.OnWholeOutfitChanged);
	}

	// Token: 0x0600B25C RID: 45660 RVA: 0x0043D0BC File Offset: 0x0043B2BC
	public static IEnumerable<ClothingItemResource> GetOutfitWithDefaultItems(ClothingOutfitUtility.OutfitType outfitType, IEnumerable<ClothingItemResource> outfit)
	{
		switch (outfitType)
		{
		case ClothingOutfitUtility.OutfitType.Clothing:
			return outfit;
		case ClothingOutfitUtility.OutfitType.JoyResponse:
			throw new NotSupportedException();
		case ClothingOutfitUtility.OutfitType.AtmoSuit:
			using (DictionaryPool<PermitCategory, ClothingItemResource, UIMinionOrMannequin.ITarget>.PooledDictionary pooledDictionary = PoolsFor<UIMinionOrMannequin.ITarget>.AllocateDict<PermitCategory, ClothingItemResource>())
			{
				foreach (ClothingItemResource clothingItemResource in outfit)
				{
					DebugUtil.DevAssert(!pooledDictionary.ContainsKey(clothingItemResource.Category), "Duplicate item for category", null);
					pooledDictionary[clothingItemResource.Category] = clothingItemResource;
				}
				if (!pooledDictionary.ContainsKey(PermitCategory.AtmoSuitHelmet))
				{
					pooledDictionary[PermitCategory.AtmoSuitHelmet] = Db.Get().Permits.ClothingItems.Get("visonly_AtmoHelmetClear");
				}
				if (!pooledDictionary.ContainsKey(PermitCategory.AtmoSuitBody))
				{
					pooledDictionary[PermitCategory.AtmoSuitBody] = Db.Get().Permits.ClothingItems.Get("visonly_AtmoSuitBasicBlue");
				}
				if (!pooledDictionary.ContainsKey(PermitCategory.AtmoSuitGloves))
				{
					pooledDictionary[PermitCategory.AtmoSuitGloves] = Db.Get().Permits.ClothingItems.Get("visonly_AtmoGlovesBasicBlue");
				}
				if (!pooledDictionary.ContainsKey(PermitCategory.AtmoSuitBelt))
				{
					pooledDictionary[PermitCategory.AtmoSuitBelt] = Db.Get().Permits.ClothingItems.Get("visonly_AtmoBeltBasicBlue");
				}
				if (!pooledDictionary.ContainsKey(PermitCategory.AtmoSuitShoes))
				{
					pooledDictionary[PermitCategory.AtmoSuitShoes] = Db.Get().Permits.ClothingItems.Get("visonly_AtmoShoesBasicBlack");
				}
				return pooledDictionary.Values.ToArray<ClothingItemResource>();
			}
			break;
		}
		throw new NotImplementedException();
	}

	// Token: 0x0600B25E RID: 45662 RVA: 0x0043D260 File Offset: 0x0043B460
	[CompilerGenerated]
	internal static UIMinionOrMannequinReactSource <ReactToClothingItemChange>g__GetSource|7_0(PermitCategory clothingChangedCategory)
	{
		switch (clothingChangedCategory)
		{
		case PermitCategory.DupeTops:
		case PermitCategory.AtmoSuitBody:
		case PermitCategory.AtmoSuitBelt:
			return UIMinionOrMannequinReactSource.OnTopChanged;
		case PermitCategory.DupeBottoms:
			return UIMinionOrMannequinReactSource.OnBottomChanged;
		case PermitCategory.DupeGloves:
		case PermitCategory.AtmoSuitGloves:
			return UIMinionOrMannequinReactSource.OnGlovesChanged;
		case PermitCategory.DupeShoes:
		case PermitCategory.AtmoSuitShoes:
			return UIMinionOrMannequinReactSource.OnShoesChanged;
		case PermitCategory.DupeHats:
		case PermitCategory.AtmoSuitHelmet:
			return UIMinionOrMannequinReactSource.OnHatChanged;
		}
		DebugUtil.DevAssert(false, string.Format("Couldn't find a reaction for \"{0}\" clothing item category being changed", clothingChangedCategory), null);
		return UIMinionOrMannequinReactSource.None;
	}

	// Token: 0x04008CCF RID: 36047
	public static readonly ClothingItemResource[] EMPTY_OUTFIT = new ClothingItemResource[0];
}
