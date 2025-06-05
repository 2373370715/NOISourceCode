using System;

namespace Database
{
	// Token: 0x020021DB RID: 8667
	public class DbStickerBomb : PermitResource
	{
		// Token: 0x0600B8B7 RID: 47287 RVA: 0x0011B916 File Offset: 0x00119B16
		public DbStickerBomb(string id, string name, string desc, PermitRarity rarity, string animfilename, string sticker, string[] requiredDlcIds, string[] forbiddenDlcIds) : base(id, name, desc, PermitCategory.Artwork, rarity, requiredDlcIds, forbiddenDlcIds)
		{
			this.id = id;
			this.sticker = sticker;
			this.animFile = Assets.GetAnim(animfilename);
		}

		// Token: 0x0600B8B8 RID: 47288 RVA: 0x00472414 File Offset: 0x00470614
		public override PermitPresentationInfo GetPermitPresentationInfo()
		{
			return new PermitPresentationInfo
			{
				sprite = Def.GetUISpriteFromMultiObjectAnim(this.animFile, string.Format("{0}_{1}", "idle_sticker", this.sticker), false, string.Format("{0}_{1}", "sticker", this.sticker))
			};
		}

		// Token: 0x040096F6 RID: 38646
		public string id;

		// Token: 0x040096F7 RID: 38647
		public string sticker;

		// Token: 0x040096F8 RID: 38648
		public KAnimFile animFile;

		// Token: 0x040096F9 RID: 38649
		private const string stickerAnimPrefix = "idle_sticker";

		// Token: 0x040096FA RID: 38650
		private const string stickerSymbolPrefix = "sticker";
	}
}
