using System;

namespace Database
{
	// Token: 0x02002184 RID: 8580
	public class BalloonArtistFacades : ResourceSet<BalloonArtistFacadeResource>
	{
		// Token: 0x0600B699 RID: 46745 RVA: 0x00458A14 File Offset: 0x00456C14
		public BalloonArtistFacades(ResourceSet parent) : base("BalloonArtistFacades", parent)
		{
			foreach (BalloonArtistFacadeInfo balloonArtistFacadeInfo in Blueprints.Get().all.balloonArtistFacades)
			{
				this.Add(balloonArtistFacadeInfo.id, balloonArtistFacadeInfo.name, balloonArtistFacadeInfo.desc, balloonArtistFacadeInfo.rarity, balloonArtistFacadeInfo.animFile, balloonArtistFacadeInfo.balloonFacadeType, balloonArtistFacadeInfo.GetRequiredDlcIds(), balloonArtistFacadeInfo.GetForbiddenDlcIds());
			}
		}

		// Token: 0x0600B69A RID: 46746 RVA: 0x00458AAC File Offset: 0x00456CAC
		[Obsolete("Please use Add(...) with required/forbidden")]
		public void Add(string id, string name, string desc, PermitRarity rarity, string animFile, BalloonArtistFacadeType balloonFacadeType)
		{
			this.Add(id, name, desc, rarity, animFile, balloonFacadeType, null, null);
		}

		// Token: 0x0600B69B RID: 46747 RVA: 0x00458AAC File Offset: 0x00456CAC
		[Obsolete("Please use Add(...) with required/forbidden")]
		public void Add(string id, string name, string desc, PermitRarity rarity, string animFile, BalloonArtistFacadeType balloonFacadeType, string[] dlcIds)
		{
			this.Add(id, name, desc, rarity, animFile, balloonFacadeType, null, null);
		}

		// Token: 0x0600B69C RID: 46748 RVA: 0x00458ACC File Offset: 0x00456CCC
		public void Add(string id, string name, string desc, PermitRarity rarity, string animFile, BalloonArtistFacadeType balloonFacadeType, string[] requiredDlcIds, string[] forbiddenDlcIds)
		{
			BalloonArtistFacadeResource item = new BalloonArtistFacadeResource(id, name, desc, rarity, animFile, balloonFacadeType, requiredDlcIds, forbiddenDlcIds);
			this.resources.Add(item);
		}
	}
}
