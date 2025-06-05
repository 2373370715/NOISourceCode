using System;
using KSerialization;

namespace ProcGenGame
{
	// Token: 0x02002106 RID: 8454
	[SerializationConfig(MemberSerialization.OptOut)]
	public struct Neighbors
	{
		// Token: 0x0600B3F0 RID: 46064 RVA: 0x001197BC File Offset: 0x001179BC
		public Neighbors(TerrainCell a, TerrainCell b)
		{
			Debug.Assert(a != null && b != null, "NULL Neighbor");
			this.n0 = a;
			this.n1 = b;
		}

		// Token: 0x04008E77 RID: 36471
		public TerrainCell n0;

		// Token: 0x04008E78 RID: 36472
		public TerrainCell n1;
	}
}
