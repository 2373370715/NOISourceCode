using System;

namespace Database
{
	// Token: 0x02002187 RID: 8583
	public readonly struct BalloonOverrideSymbol
	{
		// Token: 0x0600B6A7 RID: 46759 RVA: 0x00458C58 File Offset: 0x00456E58
		public BalloonOverrideSymbol(string animFileID, string animFileSymbolID)
		{
			if (string.IsNullOrEmpty(animFileID) || string.IsNullOrEmpty(animFileSymbolID))
			{
				this = default(BalloonOverrideSymbol);
				return;
			}
			this.animFileID = animFileID;
			this.animFileSymbolID = animFileSymbolID;
			this.animFile = Assets.GetAnim(animFileID);
			this.symbol = this.animFile.Value.GetData().build.GetSymbol(animFileSymbolID);
		}

		// Token: 0x0600B6A8 RID: 46760 RVA: 0x0011AF80 File Offset: 0x00119180
		public void ApplyTo(BalloonArtist.Instance artist)
		{
			artist.SetBalloonSymbolOverride(this);
		}

		// Token: 0x0600B6A9 RID: 46761 RVA: 0x0011AF8E File Offset: 0x0011918E
		public void ApplyTo(BalloonFX.Instance balloon)
		{
			balloon.SetBalloonSymbolOverride(this);
		}

		// Token: 0x040090CC RID: 37068
		public readonly Option<KAnim.Build.Symbol> symbol;

		// Token: 0x040090CD RID: 37069
		public readonly Option<KAnimFile> animFile;

		// Token: 0x040090CE RID: 37070
		public readonly string animFileID;

		// Token: 0x040090CF RID: 37071
		public readonly string animFileSymbolID;
	}
}
