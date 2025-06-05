using System;

namespace Database
{
	// Token: 0x020021D0 RID: 8656
	public class Shirts : ResourceSet<Shirt>
	{
		// Token: 0x0600B892 RID: 47250 RVA: 0x0011B803 File Offset: 0x00119A03
		public Shirts()
		{
			this.Hot00 = base.Add(new Shirt("body_shirt_hot_shearling"));
			this.Decor00 = base.Add(new Shirt("body_shirt_decor01"));
		}

		// Token: 0x04009695 RID: 38549
		public Shirt Hot00;

		// Token: 0x04009696 RID: 38550
		public Shirt Decor00;
	}
}
