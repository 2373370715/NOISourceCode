using System;
using UnityEngine;

namespace Database
{
	// Token: 0x02002188 RID: 8584
	public class BalloonOverrideSymbolIter
	{
		// Token: 0x0600B6AA RID: 46762 RVA: 0x00458CCC File Offset: 0x00456ECC
		public BalloonOverrideSymbolIter(Option<BalloonArtistFacadeResource> facade)
		{
			global::Debug.Assert(facade.IsNone() || facade.Unwrap().balloonOverrideSymbolIDs.Length != 0);
			this.facade = facade;
			if (facade.IsSome())
			{
				this.index = UnityEngine.Random.Range(0, facade.Unwrap().balloonOverrideSymbolIDs.Length);
			}
			this.Next();
		}

		// Token: 0x0600B6AB RID: 46763 RVA: 0x0011AF9C File Offset: 0x0011919C
		public BalloonOverrideSymbol Current()
		{
			return this.current;
		}

		// Token: 0x0600B6AC RID: 46764 RVA: 0x00458D34 File Offset: 0x00456F34
		public BalloonOverrideSymbol Next()
		{
			if (this.facade.IsSome())
			{
				BalloonArtistFacadeResource balloonArtistFacadeResource = this.facade.Unwrap();
				this.current = new BalloonOverrideSymbol(balloonArtistFacadeResource.animFilename, balloonArtistFacadeResource.balloonOverrideSymbolIDs[this.index]);
				this.index = (this.index + 1) % balloonArtistFacadeResource.balloonOverrideSymbolIDs.Length;
				return this.current;
			}
			return default(BalloonOverrideSymbol);
		}

		// Token: 0x040090D0 RID: 37072
		public readonly Option<BalloonArtistFacadeResource> facade;

		// Token: 0x040090D1 RID: 37073
		private BalloonOverrideSymbol current;

		// Token: 0x040090D2 RID: 37074
		private int index;
	}
}
