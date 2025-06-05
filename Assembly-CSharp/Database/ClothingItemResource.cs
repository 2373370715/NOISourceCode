using System;
using STRINGS;

namespace Database
{
	// Token: 0x02002197 RID: 8599
	public class ClothingItemResource : PermitResource
	{
		// Token: 0x17000BDF RID: 3039
		// (get) Token: 0x0600B764 RID: 46948 RVA: 0x0011B1C0 File Offset: 0x001193C0
		// (set) Token: 0x0600B765 RID: 46949 RVA: 0x0011B1C8 File Offset: 0x001193C8
		public string animFilename { get; private set; }

		// Token: 0x17000BE0 RID: 3040
		// (get) Token: 0x0600B766 RID: 46950 RVA: 0x0011B1D1 File Offset: 0x001193D1
		// (set) Token: 0x0600B767 RID: 46951 RVA: 0x0011B1D9 File Offset: 0x001193D9
		public KAnimFile AnimFile { get; private set; }

		// Token: 0x17000BE1 RID: 3041
		// (get) Token: 0x0600B768 RID: 46952 RVA: 0x0011B1E2 File Offset: 0x001193E2
		// (set) Token: 0x0600B769 RID: 46953 RVA: 0x0011B1EA File Offset: 0x001193EA
		public ClothingOutfitUtility.OutfitType outfitType { get; private set; }

		// Token: 0x0600B76A RID: 46954 RVA: 0x0011B1F3 File Offset: 0x001193F3
		public ClothingItemResource(string id, string name, string desc, ClothingOutfitUtility.OutfitType outfitType, PermitCategory category, PermitRarity rarity, string animFile, string[] requiredDlcIds = null, string[] forbiddenDlcIds = null) : base(id, name, desc, category, rarity, requiredDlcIds, forbiddenDlcIds)
		{
			this.AnimFile = Assets.GetAnim(animFile);
			this.animFilename = animFile;
			this.outfitType = outfitType;
		}

		// Token: 0x0600B76B RID: 46955 RVA: 0x00463FE0 File Offset: 0x004621E0
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
