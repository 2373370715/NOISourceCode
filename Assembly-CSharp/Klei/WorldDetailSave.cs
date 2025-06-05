using System;
using System.Collections.Generic;
using Delaunay.Geo;
using KSerialization;
using ProcGen;
using ProcGenGame;

namespace Klei
{
	// Token: 0x02003C45 RID: 15429
	public class WorldDetailSave
	{
		// Token: 0x0600EC5C RID: 60508 RVA: 0x00143255 File Offset: 0x00141455
		public WorldDetailSave()
		{
			this.overworldCells = new List<WorldDetailSave.OverworldCell>();
		}

		// Token: 0x0400E8A4 RID: 59556
		public List<WorldDetailSave.OverworldCell> overworldCells;

		// Token: 0x0400E8A5 RID: 59557
		public int globalWorldSeed;

		// Token: 0x0400E8A6 RID: 59558
		public int globalWorldLayoutSeed;

		// Token: 0x0400E8A7 RID: 59559
		public int globalTerrainSeed;

		// Token: 0x0400E8A8 RID: 59560
		public int globalNoiseSeed;

		// Token: 0x02003C46 RID: 15430
		[SerializationConfig(MemberSerialization.OptOut)]
		public class OverworldCell
		{
			// Token: 0x0600EC5D RID: 60509 RVA: 0x000AA024 File Offset: 0x000A8224
			public OverworldCell()
			{
			}

			// Token: 0x0600EC5E RID: 60510 RVA: 0x00143268 File Offset: 0x00141468
			public OverworldCell(SubWorld.ZoneType zoneType, TerrainCell tc)
			{
				this.poly = tc.poly;
				this.tags = tc.node.tags;
				this.zoneType = zoneType;
			}

			// Token: 0x0400E8A9 RID: 59561
			public Polygon poly;

			// Token: 0x0400E8AA RID: 59562
			public TagSet tags;

			// Token: 0x0400E8AB RID: 59563
			public SubWorld.ZoneType zoneType;
		}
	}
}
