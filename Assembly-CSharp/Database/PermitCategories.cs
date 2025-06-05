using System;
using System.Collections.Generic;
using STRINGS;

namespace Database
{
	// Token: 0x020021B7 RID: 8631
	public static class PermitCategories
	{
		// Token: 0x0600B843 RID: 47171 RVA: 0x0011B602 File Offset: 0x00119802
		public static string GetDisplayName(PermitCategory category)
		{
			return PermitCategories.CategoryInfos[category].displayName;
		}

		// Token: 0x0600B844 RID: 47172 RVA: 0x0011B614 File Offset: 0x00119814
		public static string GetUppercaseDisplayName(PermitCategory category)
		{
			return PermitCategories.CategoryInfos[category].displayName.ToUpper();
		}

		// Token: 0x0600B845 RID: 47173 RVA: 0x0011B62B File Offset: 0x0011982B
		public static string GetIconName(PermitCategory category)
		{
			return PermitCategories.CategoryInfos[category].iconName;
		}

		// Token: 0x0600B846 RID: 47174 RVA: 0x0046DF90 File Offset: 0x0046C190
		public static PermitCategory GetCategoryForId(string id)
		{
			try
			{
				return (PermitCategory)Enum.Parse(typeof(PermitCategory), id);
			}
			catch (ArgumentException)
			{
				Debug.LogError(id + " is not a valid PermitCategory.");
			}
			return PermitCategory.Equipment;
		}

		// Token: 0x0600B847 RID: 47175 RVA: 0x0011B63D File Offset: 0x0011983D
		public static Option<ClothingOutfitUtility.OutfitType> GetOutfitTypeFor(PermitCategory permitCategory)
		{
			return PermitCategories.CategoryInfos[permitCategory].outfitType;
		}

		// Token: 0x040095FE RID: 38398
		private static Dictionary<PermitCategory, PermitCategories.CategoryInfo> CategoryInfos = new Dictionary<PermitCategory, PermitCategories.CategoryInfo>
		{
			{
				PermitCategory.Equipment,
				new PermitCategories.CategoryInfo(UI.KLEI_INVENTORY_SCREEN.CATEGORIES.EQUIPMENT, "icon_inventory_equipment", Option.None)
			},
			{
				PermitCategory.DupeTops,
				new PermitCategories.CategoryInfo(UI.KLEI_INVENTORY_SCREEN.CATEGORIES.DUPE_TOPS, "icon_inventory_tops", ClothingOutfitUtility.OutfitType.Clothing)
			},
			{
				PermitCategory.DupeBottoms,
				new PermitCategories.CategoryInfo(UI.KLEI_INVENTORY_SCREEN.CATEGORIES.DUPE_BOTTOMS, "icon_inventory_bottoms", ClothingOutfitUtility.OutfitType.Clothing)
			},
			{
				PermitCategory.DupeGloves,
				new PermitCategories.CategoryInfo(UI.KLEI_INVENTORY_SCREEN.CATEGORIES.DUPE_GLOVES, "icon_inventory_gloves", ClothingOutfitUtility.OutfitType.Clothing)
			},
			{
				PermitCategory.DupeShoes,
				new PermitCategories.CategoryInfo(UI.KLEI_INVENTORY_SCREEN.CATEGORIES.DUPE_SHOES, "icon_inventory_shoes", ClothingOutfitUtility.OutfitType.Clothing)
			},
			{
				PermitCategory.DupeHats,
				new PermitCategories.CategoryInfo(UI.KLEI_INVENTORY_SCREEN.CATEGORIES.DUPE_HATS, "icon_inventory_hats", ClothingOutfitUtility.OutfitType.Clothing)
			},
			{
				PermitCategory.DupeAccessories,
				new PermitCategories.CategoryInfo(UI.KLEI_INVENTORY_SCREEN.CATEGORIES.DUPE_ACCESSORIES, "icon_inventory_accessories", ClothingOutfitUtility.OutfitType.Clothing)
			},
			{
				PermitCategory.AtmoSuitHelmet,
				new PermitCategories.CategoryInfo(UI.KLEI_INVENTORY_SCREEN.CATEGORIES.ATMO_SUIT_HELMET, "icon_inventory_atmosuit_helmet", ClothingOutfitUtility.OutfitType.AtmoSuit)
			},
			{
				PermitCategory.AtmoSuitBody,
				new PermitCategories.CategoryInfo(UI.KLEI_INVENTORY_SCREEN.CATEGORIES.ATMO_SUIT_BODY, "icon_inventory_atmosuit_body", ClothingOutfitUtility.OutfitType.AtmoSuit)
			},
			{
				PermitCategory.AtmoSuitGloves,
				new PermitCategories.CategoryInfo(UI.KLEI_INVENTORY_SCREEN.CATEGORIES.ATMO_SUIT_GLOVES, "icon_inventory_atmosuit_gloves", ClothingOutfitUtility.OutfitType.AtmoSuit)
			},
			{
				PermitCategory.AtmoSuitBelt,
				new PermitCategories.CategoryInfo(UI.KLEI_INVENTORY_SCREEN.CATEGORIES.ATMO_SUIT_BELT, "icon_inventory_atmosuit_belt", ClothingOutfitUtility.OutfitType.AtmoSuit)
			},
			{
				PermitCategory.AtmoSuitShoes,
				new PermitCategories.CategoryInfo(UI.KLEI_INVENTORY_SCREEN.CATEGORIES.ATMO_SUIT_SHOES, "icon_inventory_atmosuit_boots", ClothingOutfitUtility.OutfitType.AtmoSuit)
			},
			{
				PermitCategory.Building,
				new PermitCategories.CategoryInfo(UI.KLEI_INVENTORY_SCREEN.CATEGORIES.BUILDINGS, "icon_inventory_buildings", Option.None)
			},
			{
				PermitCategory.Critter,
				new PermitCategories.CategoryInfo(UI.KLEI_INVENTORY_SCREEN.CATEGORIES.CRITTERS, "icon_inventory_critters", Option.None)
			},
			{
				PermitCategory.Sweepy,
				new PermitCategories.CategoryInfo(UI.KLEI_INVENTORY_SCREEN.CATEGORIES.SWEEPYS, "icon_inventory_sweepys", Option.None)
			},
			{
				PermitCategory.Duplicant,
				new PermitCategories.CategoryInfo(UI.KLEI_INVENTORY_SCREEN.CATEGORIES.DUPLICANTS, "icon_inventory_duplicants", Option.None)
			},
			{
				PermitCategory.Artwork,
				new PermitCategories.CategoryInfo(UI.KLEI_INVENTORY_SCREEN.CATEGORIES.ARTWORKS, "icon_inventory_artworks", Option.None)
			},
			{
				PermitCategory.JoyResponse,
				new PermitCategories.CategoryInfo(UI.KLEI_INVENTORY_SCREEN.CATEGORIES.JOY_RESPONSE, "icon_inventory_joyresponses", ClothingOutfitUtility.OutfitType.JoyResponse)
			}
		};

		// Token: 0x020021B8 RID: 8632
		private class CategoryInfo
		{
			// Token: 0x0600B849 RID: 47177 RVA: 0x0011B64F File Offset: 0x0011984F
			public CategoryInfo(string displayName, string iconName, Option<ClothingOutfitUtility.OutfitType> outfitType)
			{
				this.displayName = displayName;
				this.iconName = iconName;
				this.outfitType = outfitType;
			}

			// Token: 0x040095FF RID: 38399
			public string displayName;

			// Token: 0x04009600 RID: 38400
			public string iconName;

			// Token: 0x04009601 RID: 38401
			public Option<ClothingOutfitUtility.OutfitType> outfitType;
		}
	}
}
