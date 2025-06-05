using System;

namespace Database
{
	// Token: 0x020021A0 RID: 8608
	public class Dreams : ResourceSet<Dream>
	{
		// Token: 0x0600B7BC RID: 47036 RVA: 0x0011B33E File Offset: 0x0011953E
		public Dreams(ResourceSet parent) : base("Dreams", parent)
		{
			this.CommonDream = new Dream("CommonDream", this, "dream_tear_swirly_kanim", new string[]
			{
				"dreamIcon_journal"
			});
		}

		// Token: 0x04009403 RID: 37891
		public Dream CommonDream;
	}
}
