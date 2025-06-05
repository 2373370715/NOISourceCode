using System;

namespace Klei
{
	// Token: 0x02003C47 RID: 15431
	public class SimSaveFileStructure
	{
		// Token: 0x0600EC5F RID: 60511 RVA: 0x00143294 File Offset: 0x00141494
		public SimSaveFileStructure()
		{
			this.worldDetail = new WorldDetailSave();
		}

		// Token: 0x0400E8AC RID: 59564
		public int WidthInCells;

		// Token: 0x0400E8AD RID: 59565
		public int HeightInCells;

		// Token: 0x0400E8AE RID: 59566
		public int x;

		// Token: 0x0400E8AF RID: 59567
		public int y;

		// Token: 0x0400E8B0 RID: 59568
		public byte[] Sim;

		// Token: 0x0400E8B1 RID: 59569
		public WorldDetailSave worldDetail;
	}
}
