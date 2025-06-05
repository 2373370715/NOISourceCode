using System;

namespace Database
{
	// Token: 0x02002195 RID: 8597
	public class ClothingItems : ResourceSet<ClothingItemResource>
	{
		// Token: 0x0600B75F RID: 46943 RVA: 0x00463EB8 File Offset: 0x004620B8
		public ClothingItems(ResourceSet parent) : base("ClothingItems", parent)
		{
			base.Initialize();
			foreach (ClothingItemInfo clothingItemInfo in Blueprints.Get().all.clothingItems)
			{
				this.Add(clothingItemInfo.id, clothingItemInfo.name, clothingItemInfo.desc, clothingItemInfo.outfitType, clothingItemInfo.category, clothingItemInfo.rarity, clothingItemInfo.animFile, clothingItemInfo.GetRequiredDlcIds(), clothingItemInfo.GetForbiddenDlcIds());
			}
		}

		// Token: 0x0600B760 RID: 46944 RVA: 0x00463F5C File Offset: 0x0046215C
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

		// Token: 0x0600B761 RID: 46945 RVA: 0x00463FB0 File Offset: 0x004621B0
		public void Add(string id, string name, string desc, ClothingOutfitUtility.OutfitType outfitType, PermitCategory category, PermitRarity rarity, string animFile, string[] requiredDlcIds = null, string[] forbiddenDlcIds = null)
		{
			ClothingItemResource item = new ClothingItemResource(id, name, desc, outfitType, category, rarity, animFile, requiredDlcIds, forbiddenDlcIds);
			this.resources.Add(item);
		}
	}
}
