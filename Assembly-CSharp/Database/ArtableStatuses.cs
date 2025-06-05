using System;

namespace Database
{
	// Token: 0x02002179 RID: 8569
	public class ArtableStatuses : ResourceSet<ArtableStatusItem>
	{
		// Token: 0x0600B680 RID: 46720 RVA: 0x00457398 File Offset: 0x00455598
		public ArtableStatuses(ResourceSet parent) : base("ArtableStatuses", parent)
		{
			this.AwaitingArting = this.Add("AwaitingArting", ArtableStatuses.ArtableStatusType.AwaitingArting);
			this.LookingUgly = this.Add("LookingUgly", ArtableStatuses.ArtableStatusType.LookingUgly);
			this.LookingOkay = this.Add("LookingOkay", ArtableStatuses.ArtableStatusType.LookingOkay);
			this.LookingGreat = this.Add("LookingGreat", ArtableStatuses.ArtableStatusType.LookingGreat);
		}

		// Token: 0x0600B681 RID: 46721 RVA: 0x004573FC File Offset: 0x004555FC
		public ArtableStatusItem Add(string id, ArtableStatuses.ArtableStatusType statusType)
		{
			ArtableStatusItem artableStatusItem = new ArtableStatusItem(id, statusType);
			this.resources.Add(artableStatusItem);
			return artableStatusItem;
		}

		// Token: 0x04009061 RID: 36961
		public ArtableStatusItem AwaitingArting;

		// Token: 0x04009062 RID: 36962
		public ArtableStatusItem LookingUgly;

		// Token: 0x04009063 RID: 36963
		public ArtableStatusItem LookingOkay;

		// Token: 0x04009064 RID: 36964
		public ArtableStatusItem LookingGreat;

		// Token: 0x0200217A RID: 8570
		public enum ArtableStatusType
		{
			// Token: 0x04009066 RID: 36966
			AwaitingArting,
			// Token: 0x04009067 RID: 36967
			LookingUgly,
			// Token: 0x04009068 RID: 36968
			LookingOkay,
			// Token: 0x04009069 RID: 36969
			LookingGreat
		}
	}
}
