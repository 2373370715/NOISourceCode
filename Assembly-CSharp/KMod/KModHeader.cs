using System;

namespace KMod
{
	// Token: 0x0200223F RID: 8767
	public class KModHeader
	{
		// Token: 0x17000BFF RID: 3071
		// (get) Token: 0x0600BA33 RID: 47667 RVA: 0x0011C766 File Offset: 0x0011A966
		// (set) Token: 0x0600BA34 RID: 47668 RVA: 0x0011C76E File Offset: 0x0011A96E
		public string staticID { get; set; }

		// Token: 0x17000C00 RID: 3072
		// (get) Token: 0x0600BA35 RID: 47669 RVA: 0x0011C777 File Offset: 0x0011A977
		// (set) Token: 0x0600BA36 RID: 47670 RVA: 0x0011C77F File Offset: 0x0011A97F
		public string title { get; set; }

		// Token: 0x17000C01 RID: 3073
		// (get) Token: 0x0600BA37 RID: 47671 RVA: 0x0011C788 File Offset: 0x0011A988
		// (set) Token: 0x0600BA38 RID: 47672 RVA: 0x0011C790 File Offset: 0x0011A990
		public string description { get; set; }
	}
}
