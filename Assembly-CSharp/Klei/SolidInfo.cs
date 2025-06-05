using System;

namespace Klei
{
	// Token: 0x02003C40 RID: 15424
	public struct SolidInfo
	{
		// Token: 0x0600EC56 RID: 60502 RVA: 0x00143216 File Offset: 0x00141416
		public SolidInfo(int cellIdx, bool isSolid)
		{
			this.cellIdx = cellIdx;
			this.isSolid = isSolid;
		}

		// Token: 0x0400E889 RID: 59529
		public int cellIdx;

		// Token: 0x0400E88A RID: 59530
		public bool isSolid;
	}
}
