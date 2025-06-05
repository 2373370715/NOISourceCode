using System;
using System.Linq;
using UnityEngine;

namespace Database
{
	// Token: 0x0200219A RID: 8602
	public class ClothingOutfitResource : Resource, IHasDlcRestrictions
	{
		// Token: 0x17000BE2 RID: 3042
		// (get) Token: 0x0600B76F RID: 46959 RVA: 0x0011B23B File Offset: 0x0011943B
		// (set) Token: 0x0600B770 RID: 46960 RVA: 0x0011B243 File Offset: 0x00119443
		public string[] itemsInOutfit { get; private set; }

		// Token: 0x0600B771 RID: 46961 RVA: 0x0011B24C File Offset: 0x0011944C
		public ClothingOutfitResource(string id, string[] items_in_outfit, string name, ClothingOutfitUtility.OutfitType outfitType, string[] requiredDlcIds = null, string[] forbiddenDlcIds = null) : base(id, name)
		{
			this.itemsInOutfit = items_in_outfit;
			this.outfitType = outfitType;
			this.requiredDlcIds = requiredDlcIds;
			this.forbiddenDlcIds = forbiddenDlcIds;
		}

		// Token: 0x0600B772 RID: 46962 RVA: 0x0011B275 File Offset: 0x00119475
		public global::Tuple<Sprite, Color> GetUISprite()
		{
			Sprite sprite = Assets.GetSprite("unknown");
			return new global::Tuple<Sprite, Color>(sprite, (sprite != null) ? Color.white : Color.clear);
		}

		// Token: 0x0600B773 RID: 46963 RVA: 0x0011B2A0 File Offset: 0x001194A0
		public string GetDlcIdFrom()
		{
			if (this.requiredDlcIds == null)
			{
				return null;
			}
			return this.requiredDlcIds.Last<string>();
		}

		// Token: 0x0600B774 RID: 46964 RVA: 0x0011B2B7 File Offset: 0x001194B7
		public string[] GetRequiredDlcIds()
		{
			return this.requiredDlcIds;
		}

		// Token: 0x0600B775 RID: 46965 RVA: 0x0011B2BF File Offset: 0x001194BF
		public string[] GetForbiddenDlcIds()
		{
			return this.forbiddenDlcIds;
		}

		// Token: 0x0400935A RID: 37722
		public ClothingOutfitUtility.OutfitType outfitType;

		// Token: 0x0400935C RID: 37724
		public string[] requiredDlcIds;

		// Token: 0x0400935D RID: 37725
		public string[] forbiddenDlcIds;
	}
}
