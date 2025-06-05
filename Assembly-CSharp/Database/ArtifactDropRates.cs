using System;
using TUNING;

namespace Database
{
	// Token: 0x02002181 RID: 8577
	public class ArtifactDropRates : ResourceSet<ArtifactDropRate>
	{
		// Token: 0x0600B693 RID: 46739 RVA: 0x0011AF28 File Offset: 0x00119128
		public ArtifactDropRates(ResourceSet parent) : base("ArtifactDropRates", parent)
		{
			this.CreateDropRates();
		}

		// Token: 0x0600B694 RID: 46740 RVA: 0x00457764 File Offset: 0x00455964
		private void CreateDropRates()
		{
			this.None = new ArtifactDropRate();
			this.None.AddItem(DECOR.SPACEARTIFACT.TIER_NONE, 1f);
			base.Add(this.None);
			this.Bad = new ArtifactDropRate();
			this.Bad.AddItem(DECOR.SPACEARTIFACT.TIER_NONE, 10f);
			this.Bad.AddItem(DECOR.SPACEARTIFACT.TIER0, 5f);
			this.Bad.AddItem(DECOR.SPACEARTIFACT.TIER1, 3f);
			this.Bad.AddItem(DECOR.SPACEARTIFACT.TIER2, 2f);
			base.Add(this.Bad);
			this.Mediocre = new ArtifactDropRate();
			this.Mediocre.AddItem(DECOR.SPACEARTIFACT.TIER_NONE, 10f);
			this.Mediocre.AddItem(DECOR.SPACEARTIFACT.TIER1, 5f);
			this.Mediocre.AddItem(DECOR.SPACEARTIFACT.TIER2, 3f);
			this.Mediocre.AddItem(DECOR.SPACEARTIFACT.TIER3, 2f);
			base.Add(this.Mediocre);
			this.Good = new ArtifactDropRate();
			this.Good.AddItem(DECOR.SPACEARTIFACT.TIER_NONE, 10f);
			this.Good.AddItem(DECOR.SPACEARTIFACT.TIER2, 5f);
			this.Good.AddItem(DECOR.SPACEARTIFACT.TIER3, 3f);
			this.Good.AddItem(DECOR.SPACEARTIFACT.TIER4, 2f);
			base.Add(this.Good);
			this.Great = new ArtifactDropRate();
			this.Great.AddItem(DECOR.SPACEARTIFACT.TIER_NONE, 10f);
			this.Great.AddItem(DECOR.SPACEARTIFACT.TIER3, 5f);
			this.Great.AddItem(DECOR.SPACEARTIFACT.TIER4, 3f);
			this.Great.AddItem(DECOR.SPACEARTIFACT.TIER5, 2f);
			base.Add(this.Great);
			this.Amazing = new ArtifactDropRate();
			this.Amazing.AddItem(DECOR.SPACEARTIFACT.TIER_NONE, 10f);
			this.Amazing.AddItem(DECOR.SPACEARTIFACT.TIER3, 3f);
			this.Amazing.AddItem(DECOR.SPACEARTIFACT.TIER4, 5f);
			this.Amazing.AddItem(DECOR.SPACEARTIFACT.TIER5, 2f);
			base.Add(this.Amazing);
			this.Perfect = new ArtifactDropRate();
			this.Perfect.AddItem(DECOR.SPACEARTIFACT.TIER_NONE, 10f);
			this.Perfect.AddItem(DECOR.SPACEARTIFACT.TIER4, 6f);
			this.Perfect.AddItem(DECOR.SPACEARTIFACT.TIER5, 4f);
			base.Add(this.Perfect);
		}

		// Token: 0x04009078 RID: 36984
		public ArtifactDropRate None;

		// Token: 0x04009079 RID: 36985
		public ArtifactDropRate Bad;

		// Token: 0x0400907A RID: 36986
		public ArtifactDropRate Mediocre;

		// Token: 0x0400907B RID: 36987
		public ArtifactDropRate Good;

		// Token: 0x0400907C RID: 36988
		public ArtifactDropRate Great;

		// Token: 0x0400907D RID: 36989
		public ArtifactDropRate Amazing;

		// Token: 0x0400907E RID: 36990
		public ArtifactDropRate Perfect;
	}
}
