using System;

namespace KMod
{
	// Token: 0x0200225F RID: 8799
	public enum EventType
	{
		// Token: 0x0400993B RID: 39227
		LoadError,
		// Token: 0x0400993C RID: 39228
		NotFound,
		// Token: 0x0400993D RID: 39229
		InstallInfoInaccessible,
		// Token: 0x0400993E RID: 39230
		OutOfOrder,
		// Token: 0x0400993F RID: 39231
		ExpectedActive,
		// Token: 0x04009940 RID: 39232
		ExpectedInactive,
		// Token: 0x04009941 RID: 39233
		ActiveDuringCrash,
		// Token: 0x04009942 RID: 39234
		InstallFailed,
		// Token: 0x04009943 RID: 39235
		Installed,
		// Token: 0x04009944 RID: 39236
		Uninstalled,
		// Token: 0x04009945 RID: 39237
		VersionUpdate,
		// Token: 0x04009946 RID: 39238
		AvailableContentChanged,
		// Token: 0x04009947 RID: 39239
		RestartRequested,
		// Token: 0x04009948 RID: 39240
		BadWorldGen,
		// Token: 0x04009949 RID: 39241
		Deactivated,
		// Token: 0x0400994A RID: 39242
		DisabledEarlyAccess,
		// Token: 0x0400994B RID: 39243
		DownloadFailed
	}
}
