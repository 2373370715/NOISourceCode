using System;
using System.Diagnostics;

namespace Database
{
	// Token: 0x020021D9 RID: 8665
	public class StatusItems : ResourceSet<StatusItem>
	{
		// Token: 0x0600B8B5 RID: 47285 RVA: 0x0011B90C File Offset: 0x00119B0C
		public StatusItems(string id, ResourceSet parent) : base(id, parent)
		{
		}

		// Token: 0x020021DA RID: 8666
		[DebuggerDisplay("{Id}")]
		public class StatusItemInfo : Resource
		{
			// Token: 0x040096E9 RID: 38633
			public string Type;

			// Token: 0x040096EA RID: 38634
			public string Tooltip;

			// Token: 0x040096EB RID: 38635
			public bool IsIconTinted;

			// Token: 0x040096EC RID: 38636
			public StatusItem.IconType IconType;

			// Token: 0x040096ED RID: 38637
			public string Icon;

			// Token: 0x040096EE RID: 38638
			public string SoundPath;

			// Token: 0x040096EF RID: 38639
			public bool ShouldNotify;

			// Token: 0x040096F0 RID: 38640
			public float NotificationDelay;

			// Token: 0x040096F1 RID: 38641
			public NotificationType NotificationType;

			// Token: 0x040096F2 RID: 38642
			public bool AllowMultiples;

			// Token: 0x040096F3 RID: 38643
			public string Effect;

			// Token: 0x040096F4 RID: 38644
			public HashedString Overlay;

			// Token: 0x040096F5 RID: 38645
			public HashedString SecondOverlay;
		}
	}
}
