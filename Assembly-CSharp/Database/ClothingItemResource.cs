﻿using System;
using STRINGS;

namespace Database
{
	public class ClothingItemResource : PermitResource
	{
		public string animFilename { get; private set; }

		public KAnimFile AnimFile { get; private set; }

		public ClothingOutfitUtility.OutfitType outfitType { get; private set; }

		public ClothingItemResource(string id, string name, string desc, ClothingOutfitUtility.OutfitType outfitType, PermitCategory category, PermitRarity rarity, string animFile, string[] requiredDlcIds = null, string[] forbiddenDlcIds = null) : base(id, name, desc, category, rarity, requiredDlcIds, forbiddenDlcIds)
		{
			this.AnimFile = Assets.GetAnim(animFile);
			this.animFilename = animFile;
			this.outfitType = outfitType;
		}

		public override PermitPresentationInfo GetPermitPresentationInfo()
		{
			PermitPresentationInfo result = default(PermitPresentationInfo);
			if (this.AnimFile == null)
			{
				Debug.LogError("Clothing kanim is missing from bundle: " + this.animFilename);
			}
			result.sprite = Def.GetUISpriteFromMultiObjectAnim(this.AnimFile, "ui", false, "");
			result.SetFacadeForText(UI.KLEI_INVENTORY_SCREEN.CLOTHING_ITEM_FACADE_FOR);
			return result;
		}
	}
}
