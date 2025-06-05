using System;
using System.Runtime.CompilerServices;
using Database;
using STRINGS;
using UnityEngine;

// Token: 0x02001DA7 RID: 7591
public static class KleiItemsUI
{
	// Token: 0x06009E94 RID: 40596 RVA: 0x0010BA66 File Offset: 0x00109C66
	public static string WrapAsToolTipTitle(string text)
	{
		return "<b><style=\"KLink\">" + text + "</style></b>";
	}

	// Token: 0x06009E95 RID: 40597 RVA: 0x0010BA78 File Offset: 0x00109C78
	public static string WrapWithColor(string text, Color color)
	{
		return string.Concat(new string[]
		{
			"<color=#",
			color.ToHexString(),
			">",
			text,
			"</color>"
		});
	}

	// Token: 0x06009E96 RID: 40598 RVA: 0x0010BAAA File Offset: 0x00109CAA
	public static Sprite GetNoneClothingItemIcon(PermitCategory category, Option<Personality> personality)
	{
		return KleiItemsUI.GetNoneIconForCategory(category, personality);
	}

	// Token: 0x06009E97 RID: 40599 RVA: 0x0010BAB3 File Offset: 0x00109CB3
	public static Sprite GetNoneBalloonArtistIcon()
	{
		return KleiItemsUI.GetNoneIconForCategory(PermitCategory.JoyResponse, null);
	}

	// Token: 0x06009E98 RID: 40600 RVA: 0x0010BAC2 File Offset: 0x00109CC2
	private static Sprite GetNoneIconForCategory(PermitCategory category, Option<Personality> personality)
	{
		return Assets.GetSprite(KleiItemsUI.<GetNoneIconForCategory>g__GetIconName|5_0(category, personality));
	}

	// Token: 0x06009E99 RID: 40601 RVA: 0x003DCB0C File Offset: 0x003DAD0C
	public static string GetNoneOutfitName(ClothingOutfitUtility.OutfitType outfitType)
	{
		switch (outfitType)
		{
		case ClothingOutfitUtility.OutfitType.Clothing:
			return UI.OUTFIT_NAME.NONE;
		case ClothingOutfitUtility.OutfitType.JoyResponse:
			return UI.OUTFIT_NAME.NONE_JOY_RESPONSE;
		case ClothingOutfitUtility.OutfitType.AtmoSuit:
			return UI.OUTFIT_NAME.NONE_ATMO_SUIT;
		default:
			DebugUtil.DevAssert(false, string.Format("Couldn't find \"no item\" string for outfit {0}", outfitType), null);
			return "-";
		}
	}

	// Token: 0x06009E9A RID: 40602 RVA: 0x003DCB6C File Offset: 0x003DAD6C
	[return: TupleElementNames(new string[]
	{
		"name",
		"desc"
	})]
	public static ValueTuple<string, string> GetNoneClothingItemStrings(PermitCategory category)
	{
		switch (category)
		{
		case PermitCategory.DupeTops:
			return new ValueTuple<string, string>(EQUIPMENT.PREFABS.CLOTHING_TOPS.NAME, EQUIPMENT.PREFABS.CLOTHING_TOPS.DESC);
		case PermitCategory.DupeBottoms:
			return new ValueTuple<string, string>(EQUIPMENT.PREFABS.CLOTHING_BOTTOMS.NAME, EQUIPMENT.PREFABS.CLOTHING_BOTTOMS.DESC);
		case PermitCategory.DupeGloves:
			return new ValueTuple<string, string>(EQUIPMENT.PREFABS.CLOTHING_GLOVES.NAME, EQUIPMENT.PREFABS.CLOTHING_GLOVES.DESC);
		case PermitCategory.DupeShoes:
			return new ValueTuple<string, string>(EQUIPMENT.PREFABS.CLOTHING_SHOES.NAME, EQUIPMENT.PREFABS.CLOTHING_SHOES.DESC);
		case PermitCategory.DupeHats:
			return new ValueTuple<string, string>(EQUIPMENT.PREFABS.CLOTHING_HATS.NAME, EQUIPMENT.PREFABS.CLOTHING_HATS.DESC);
		case PermitCategory.DupeAccessories:
			return new ValueTuple<string, string>(EQUIPMENT.PREFABS.CLOTHING_ACCESORIES.NAME, EQUIPMENT.PREFABS.CLOTHING_ACCESORIES.DESC);
		case PermitCategory.AtmoSuitHelmet:
			return new ValueTuple<string, string>(EQUIPMENT.PREFABS.ATMO_SUIT_HELMET.NAME, EQUIPMENT.PREFABS.ATMO_SUIT_HELMET.DESC);
		case PermitCategory.AtmoSuitBody:
			return new ValueTuple<string, string>(EQUIPMENT.PREFABS.ATMO_SUIT_BODY.NAME, EQUIPMENT.PREFABS.ATMO_SUIT_BODY.DESC);
		case PermitCategory.AtmoSuitGloves:
			return new ValueTuple<string, string>(EQUIPMENT.PREFABS.ATMO_SUIT_GLOVES.NAME, EQUIPMENT.PREFABS.ATMO_SUIT_GLOVES.DESC);
		case PermitCategory.AtmoSuitBelt:
			return new ValueTuple<string, string>(EQUIPMENT.PREFABS.ATMO_SUIT_BELT.NAME, EQUIPMENT.PREFABS.ATMO_SUIT_BELT.DESC);
		case PermitCategory.AtmoSuitShoes:
			return new ValueTuple<string, string>(EQUIPMENT.PREFABS.ATMO_SUIT_SHOES.NAME, EQUIPMENT.PREFABS.ATMO_SUIT_SHOES.DESC);
		case PermitCategory.JoyResponse:
			return new ValueTuple<string, string>(UI.OUTFIT_DESCRIPTION.NO_JOY_RESPONSE_NAME, UI.OUTFIT_DESCRIPTION.NO_JOY_RESPONSE_DESC);
		}
		DebugUtil.DevAssert(false, string.Format("Couldn't find \"no item\" string for category {0}", category), null);
		return new ValueTuple<string, string>("-", "-");
	}

	// Token: 0x06009E9B RID: 40603 RVA: 0x0010BAD5 File Offset: 0x00109CD5
	public static void ConfigureTooltipOn(GameObject gameObject, Option<LocString> tooltipText = default(Option<LocString>))
	{
		KleiItemsUI.ConfigureTooltipOn(gameObject, tooltipText.HasValue ? Option.Some<string>(tooltipText.Value) : Option.None);
	}

	// Token: 0x06009E9C RID: 40604 RVA: 0x003DCD28 File Offset: 0x003DAF28
	public static void ConfigureTooltipOn(GameObject gameObject, Option<string> tooltipText = default(Option<string>))
	{
		ToolTip toolTip = gameObject.GetComponent<ToolTip>();
		if (toolTip.IsNullOrDestroyed())
		{
			toolTip = gameObject.AddComponent<ToolTip>();
			toolTip.tooltipPivot = new Vector2(0.5f, 1f);
			if (gameObject.GetComponent<KButton>())
			{
				toolTip.tooltipPositionOffset = new Vector2(0f, 22f);
			}
			else
			{
				toolTip.tooltipPositionOffset = new Vector2(0f, 0f);
			}
			toolTip.parentPositionAnchor = new Vector2(0.5f, 0f);
			toolTip.toolTipPosition = ToolTip.TooltipPosition.Custom;
		}
		if (!tooltipText.HasValue)
		{
			toolTip.ClearMultiStringTooltip();
			return;
		}
		toolTip.SetSimpleTooltip(tooltipText.Value);
	}

	// Token: 0x06009E9D RID: 40605 RVA: 0x003DCDD4 File Offset: 0x003DAFD4
	public static string GetTooltipStringFor(PermitResource permit)
	{
		string text = KleiItemsUI.WrapAsToolTipTitle(permit.Name);
		if (!string.IsNullOrWhiteSpace(permit.Description))
		{
			text = text + "\n" + permit.Description;
		}
		string dlcIdFrom = permit.GetDlcIdFrom();
		if (DlcManager.IsDlcId(dlcIdFrom))
		{
			if (permit.Rarity == PermitRarity.UniversalLocked)
			{
				text = text + "\n\n" + UI.KLEI_INVENTORY_SCREEN.COLLECTION_COMING_SOON.Replace("{Collection}", DlcManager.GetDlcTitle(dlcIdFrom));
			}
			else
			{
				text = text + "\n\n" + UI.KLEI_INVENTORY_SCREEN.COLLECTION.Replace("{Collection}", DlcManager.GetDlcTitle(dlcIdFrom));
			}
		}
		else
		{
			string text2 = UI.KLEI_INVENTORY_SCREEN.ITEM_RARITY_DETAILS.Replace("{RarityName}", permit.Rarity.GetLocStringName());
			if (!string.IsNullOrWhiteSpace(text2))
			{
				text = text + "\n\n" + text2;
			}
		}
		if (permit.IsOwnableOnServer() && PermitItems.GetOwnedCount(permit) <= 0)
		{
			text = text + "\n\n" + KleiItemsUI.WrapWithColor(UI.KLEI_INVENTORY_SCREEN.ITEM_PLAYER_OWN_NONE, KleiItemsUI.TEXT_COLOR__PERMIT_NOT_OWNED);
		}
		return text;
	}

	// Token: 0x06009E9E RID: 40606 RVA: 0x003DCECC File Offset: 0x003DB0CC
	public static string GetNoneTooltipStringFor(PermitCategory category)
	{
		ValueTuple<string, string> noneClothingItemStrings = KleiItemsUI.GetNoneClothingItemStrings(category);
		string item = noneClothingItemStrings.Item1;
		string item2 = noneClothingItemStrings.Item2;
		return KleiItemsUI.WrapAsToolTipTitle(item) + "\n" + item2;
	}

	// Token: 0x06009E9F RID: 40607 RVA: 0x0010BB03 File Offset: 0x00109D03
	public static Color GetColor(string input)
	{
		if (input[0] == '#')
		{
			return Util.ColorFromHex(input.Substring(1));
		}
		return Util.ColorFromHex(input);
	}

	// Token: 0x06009EA1 RID: 40609 RVA: 0x003DCF00 File Offset: 0x003DB100
	[CompilerGenerated]
	internal static string <GetNoneIconForCategory>g__GetIconName|5_0(PermitCategory category, Option<Personality> personality)
	{
		switch (category)
		{
		case PermitCategory.DupeTops:
			return "default_no_top";
		case PermitCategory.DupeBottoms:
			return "default_no_bottom";
		case PermitCategory.DupeGloves:
			return "default_no_gloves";
		case PermitCategory.DupeShoes:
			return "default_no_footwear";
		case PermitCategory.DupeHats:
			return "icon_inventory_hats";
		case PermitCategory.DupeAccessories:
			return "icon_inventory_accessories";
		case PermitCategory.AtmoSuitHelmet:
			return "icon_inventory_atmosuit_helmet";
		case PermitCategory.AtmoSuitBody:
			return "icon_inventory_atmosuit_body";
		case PermitCategory.AtmoSuitGloves:
			return "icon_inventory_atmosuit_gloves";
		case PermitCategory.AtmoSuitBelt:
			return "icon_inventory_atmosuit_belt";
		case PermitCategory.AtmoSuitShoes:
			return "icon_inventory_atmosuit_boots";
		case PermitCategory.Building:
			return "icon_inventory_buildings";
		case PermitCategory.Critter:
			return "icon_inventory_critters";
		case PermitCategory.Sweepy:
			return "icon_inventory_sweepys";
		case PermitCategory.Duplicant:
			return "icon_inventory_duplicants";
		case PermitCategory.Artwork:
			return "icon_inventory_artworks";
		case PermitCategory.JoyResponse:
			return "icon_inventory_joyresponses";
		default:
			return "NoTraits";
		}
	}

	// Token: 0x04007C95 RID: 31893
	public static readonly Color TEXT_COLOR__PERMIT_NOT_OWNED = KleiItemsUI.GetColor("#DD992F");
}
