using System;

namespace Database
{
	// Token: 0x0200217B RID: 8571
	public class ArtableStatusItem : StatusItem
	{
		// Token: 0x0600B682 RID: 46722 RVA: 0x00457420 File Offset: 0x00455620
		public ArtableStatusItem(string id, ArtableStatuses.ArtableStatusType statusType) : base(id, "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022, null)
		{
			this.StatusType = statusType;
		}

		// Token: 0x0400906A RID: 36970
		public ArtableStatuses.ArtableStatusType StatusType;
	}
}
