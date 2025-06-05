using System;
using System.Collections.Generic;
using ProcGen;
using ProcGenGame;
using VoronoiTree;

namespace Klei
{
	// Token: 0x02003C43 RID: 15427
	public class Data
	{
		// Token: 0x0600EC5A RID: 60506 RVA: 0x004DD904 File Offset: 0x004DBB04
		public Data()
		{
			this.worldLayout = new WorldLayout(null, 0);
			this.terrainCells = new List<TerrainCell>();
			this.overworldCells = new List<TerrainCell>();
			this.rivers = new List<ProcGen.River>();
			this.gameSpawnData = new GameSpawnData();
			this.world = new Chunk();
			this.voronoiTree = new Tree(0);
		}

		// Token: 0x0400E892 RID: 59538
		public int globalWorldSeed;

		// Token: 0x0400E893 RID: 59539
		public int globalWorldLayoutSeed;

		// Token: 0x0400E894 RID: 59540
		public int globalTerrainSeed;

		// Token: 0x0400E895 RID: 59541
		public int globalNoiseSeed;

		// Token: 0x0400E896 RID: 59542
		public int chunkEdgeSize = 32;

		// Token: 0x0400E897 RID: 59543
		public WorldLayout worldLayout;

		// Token: 0x0400E898 RID: 59544
		public List<TerrainCell> terrainCells;

		// Token: 0x0400E899 RID: 59545
		public List<TerrainCell> overworldCells;

		// Token: 0x0400E89A RID: 59546
		public List<ProcGen.River> rivers;

		// Token: 0x0400E89B RID: 59547
		public GameSpawnData gameSpawnData;

		// Token: 0x0400E89C RID: 59548
		public Chunk world;

		// Token: 0x0400E89D RID: 59549
		public Tree voronoiTree;

		// Token: 0x0400E89E RID: 59550
		public AxialI clusterLocation;
	}
}
