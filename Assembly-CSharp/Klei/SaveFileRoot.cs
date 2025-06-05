using System;
using System.Collections.Generic;
using KMod;

namespace Klei
{
	// Token: 0x02003C42 RID: 15426
	internal class SaveFileRoot
	{
		// Token: 0x0600EC59 RID: 60505 RVA: 0x0014322F File Offset: 0x0014142F
		public SaveFileRoot()
		{
			this.streamed = new Dictionary<string, byte[]>();
		}

		// Token: 0x0400E88C RID: 59532
		public int WidthInCells;

		// Token: 0x0400E88D RID: 59533
		public int HeightInCells;

		// Token: 0x0400E88E RID: 59534
		public Dictionary<string, byte[]> streamed;

		// Token: 0x0400E88F RID: 59535
		public string clusterID;

		// Token: 0x0400E890 RID: 59536
		public List<ModInfo> requiredMods;

		// Token: 0x0400E891 RID: 59537
		public List<Label> active_mods;
	}
}
