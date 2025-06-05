using System;
using STRINGS;

namespace Database
{
	// Token: 0x02002186 RID: 8582
	public class BalloonArtistFacadeResource : PermitResource
	{
		// Token: 0x17000BDD RID: 3037
		// (get) Token: 0x0600B69D RID: 46749 RVA: 0x0011AF3C File Offset: 0x0011913C
		// (set) Token: 0x0600B69E RID: 46750 RVA: 0x0011AF44 File Offset: 0x00119144
		public string animFilename { get; private set; }

		// Token: 0x17000BDE RID: 3038
		// (get) Token: 0x0600B69F RID: 46751 RVA: 0x0011AF4D File Offset: 0x0011914D
		// (set) Token: 0x0600B6A0 RID: 46752 RVA: 0x0011AF55 File Offset: 0x00119155
		public KAnimFile AnimFile { get; private set; }

		// Token: 0x0600B6A1 RID: 46753 RVA: 0x00458AF8 File Offset: 0x00456CF8
		public BalloonArtistFacadeResource(string id, string name, string desc, PermitRarity rarity, string animFile, BalloonArtistFacadeType balloonFacadeType, string[] requiredDlcIds = null, string[] forbiddenDlcIds = null) : base(id, name, desc, PermitCategory.JoyResponse, rarity, requiredDlcIds, forbiddenDlcIds)
		{
			this.AnimFile = Assets.GetAnim(animFile);
			this.animFilename = animFile;
			this.balloonFacadeType = balloonFacadeType;
			Db.Get().Accessories.AddAccessories(id, this.AnimFile);
			this.balloonOverrideSymbolIDs = this.GetBalloonOverrideSymbolIDs();
			Debug.Assert(this.balloonOverrideSymbolIDs.Length != 0);
		}

		// Token: 0x0600B6A2 RID: 46754 RVA: 0x00458B6C File Offset: 0x00456D6C
		public override PermitPresentationInfo GetPermitPresentationInfo()
		{
			PermitPresentationInfo result = default(PermitPresentationInfo);
			result.sprite = Def.GetUISpriteFromMultiObjectAnim(this.AnimFile, "ui", false, "");
			result.SetFacadeForText(UI.KLEI_INVENTORY_SCREEN.BALLOON_ARTIST_FACADE_FOR);
			return result;
		}

		// Token: 0x0600B6A3 RID: 46755 RVA: 0x00458BB0 File Offset: 0x00456DB0
		public BalloonOverrideSymbol GetNextOverride()
		{
			int num = this.nextSymbolIndex;
			this.nextSymbolIndex = (this.nextSymbolIndex + 1) % this.balloonOverrideSymbolIDs.Length;
			return new BalloonOverrideSymbol(this.animFilename, this.balloonOverrideSymbolIDs[num]);
		}

		// Token: 0x0600B6A4 RID: 46756 RVA: 0x0011AF5E File Offset: 0x0011915E
		public BalloonOverrideSymbolIter GetSymbolIter()
		{
			return new BalloonOverrideSymbolIter(this);
		}

		// Token: 0x0600B6A5 RID: 46757 RVA: 0x0011AF6B File Offset: 0x0011916B
		public BalloonOverrideSymbol GetOverrideAt(int index)
		{
			return new BalloonOverrideSymbol(this.animFilename, this.balloonOverrideSymbolIDs[index]);
		}

		// Token: 0x0600B6A6 RID: 46758 RVA: 0x00458BF0 File Offset: 0x00456DF0
		private string[] GetBalloonOverrideSymbolIDs()
		{
			KAnim.Build build = this.AnimFile.GetData().build;
			BalloonArtistFacadeType balloonArtistFacadeType = this.balloonFacadeType;
			string[] result;
			if (balloonArtistFacadeType != BalloonArtistFacadeType.Single)
			{
				if (balloonArtistFacadeType != BalloonArtistFacadeType.ThreeSet)
				{
					throw new NotImplementedException();
				}
				result = new string[]
				{
					"body1",
					"body2",
					"body3"
				};
			}
			else
			{
				result = new string[]
				{
					"body"
				};
			}
			return result;
		}

		// Token: 0x040090C9 RID: 37065
		private BalloonArtistFacadeType balloonFacadeType;

		// Token: 0x040090CA RID: 37066
		public readonly string[] balloonOverrideSymbolIDs;

		// Token: 0x040090CB RID: 37067
		public int nextSymbolIndex;
	}
}
