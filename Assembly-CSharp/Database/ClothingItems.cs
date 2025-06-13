﻿using System;

namespace Database
{
	public class ClothingItems : ResourceSet<ClothingItemResource>
	{
		public ClothingItems(ResourceSet parent) : base("ClothingItems", parent)
		{
			base.Initialize();
			foreach (ClothingItemInfo clothingItemInfo in Blueprints.Get().all.clothingItems)
			{
				this.Add(clothingItemInfo.id, clothingItemInfo.name, clothingItemInfo.desc, clothingItemInfo.outfitType, clothingItemInfo.category, clothingItemInfo.rarity, clothingItemInfo.animFile, clothingItemInfo.GetRequiredDlcIds(), clothingItemInfo.GetForbiddenDlcIds());
			}
		}

		public ClothingItemResource TryResolveAccessoryResource(ResourceGuid AccessoryGuid)
		{
			if (AccessoryGuid.Guid != null)
			{
				string[] array = AccessoryGuid.Guid.Split('.', StringSplitOptions.None);
				if (array.Length != 0)
				{
					string symbol_name = array[array.Length - 1];
					return this.resources.Find((ClothingItemResource ci) => symbol_name.Contains(ci.Id));
				}
			}
			return null;
		}

		public void Add(string id, string name, string desc, ClothingOutfitUtility.OutfitType outfitType, PermitCategory category, PermitRarity rarity, string animFile, string[] requiredDlcIds = null, string[] forbiddenDlcIds = null)
		{
			ClothingItemResource item = new ClothingItemResource(id, name, desc, outfitType, category, rarity, animFile, requiredDlcIds, forbiddenDlcIds);
			this.resources.Add(item);
		}
	}
}
