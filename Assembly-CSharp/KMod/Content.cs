using System;

namespace KMod
{
	// Token: 0x0200224E RID: 8782
	[Flags]
	public enum Content : byte
	{
		// Token: 0x040098DD RID: 39133
		LayerableFiles = 1,
		// Token: 0x040098DE RID: 39134
		Strings = 2,
		// Token: 0x040098DF RID: 39135
		DLL = 4,
		// Token: 0x040098E0 RID: 39136
		Translation = 8,
		// Token: 0x040098E1 RID: 39137
		Animation = 16
	}
}
