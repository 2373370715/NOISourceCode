using System;

namespace Database
{
	// Token: 0x020021DC RID: 8668
	public class StickerBombs : ResourceSet<DbStickerBomb>
	{
		// Token: 0x0600B8B9 RID: 47289 RVA: 0x00472468 File Offset: 0x00470668
		public StickerBombs(ResourceSet parent) : base("StickerBombs", parent)
		{
			foreach (StickerBombFacadeInfo stickerBombFacadeInfo in Blueprints.Get().all.stickerBombFacades)
			{
				this.Add(stickerBombFacadeInfo.id, stickerBombFacadeInfo.name, stickerBombFacadeInfo.desc, stickerBombFacadeInfo.rarity, stickerBombFacadeInfo.animFile, stickerBombFacadeInfo.sticker, stickerBombFacadeInfo.requiredDlcIds, stickerBombFacadeInfo.GetForbiddenDlcIds());
			}
		}

		// Token: 0x0600B8BA RID: 47290 RVA: 0x00472500 File Offset: 0x00470700
		private DbStickerBomb Add(string id, string name, string desc, PermitRarity rarity, string animfilename, string symbolName, string[] requiredDlcIds, string[] forbiddenDlcIds)
		{
			DbStickerBomb dbStickerBomb = new DbStickerBomb(id, name, desc, rarity, animfilename, symbolName, requiredDlcIds, forbiddenDlcIds);
			this.resources.Add(dbStickerBomb);
			return dbStickerBomb;
		}

		// Token: 0x0600B8BB RID: 47291 RVA: 0x0011B94A File Offset: 0x00119B4A
		public DbStickerBomb GetRandomSticker()
		{
			return this.resources.GetRandom<DbStickerBomb>();
		}
	}
}
