using System;
using System.Collections.Generic;

namespace Klei
{
	// Token: 0x02003C44 RID: 15428
	public class WorldGenSave
	{
		// Token: 0x0600EC5B RID: 60507 RVA: 0x00143242 File Offset: 0x00141442
		public WorldGenSave()
		{
			this.data = new Data();
		}

		// Token: 0x0400E89F RID: 59551
		public Vector2I version;

		// Token: 0x0400E8A0 RID: 59552
		public Data data;

		// Token: 0x0400E8A1 RID: 59553
		public string worldID;

		// Token: 0x0400E8A2 RID: 59554
		public List<string> traitIDs;

		// Token: 0x0400E8A3 RID: 59555
		public List<string> storyTraitIDs;
	}
}
