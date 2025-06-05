using System;
using System.Collections.Generic;

namespace Klei
{
	// Token: 0x02003C48 RID: 15432
	public class ClusterLayoutSave
	{
		// Token: 0x0600EC60 RID: 60512 RVA: 0x001432A7 File Offset: 0x001414A7
		public ClusterLayoutSave()
		{
			this.worlds = new List<ClusterLayoutSave.World>();
		}

		// Token: 0x0400E8B2 RID: 59570
		public string ID;

		// Token: 0x0400E8B3 RID: 59571
		public Vector2I version;

		// Token: 0x0400E8B4 RID: 59572
		public List<ClusterLayoutSave.World> worlds;

		// Token: 0x0400E8B5 RID: 59573
		public Vector2I size;

		// Token: 0x0400E8B6 RID: 59574
		public int currentWorldIdx;

		// Token: 0x0400E8B7 RID: 59575
		public int numRings;

		// Token: 0x0400E8B8 RID: 59576
		public Dictionary<ClusterLayoutSave.POIType, List<AxialI>> poiLocations = new Dictionary<ClusterLayoutSave.POIType, List<AxialI>>();

		// Token: 0x0400E8B9 RID: 59577
		public Dictionary<AxialI, string> poiPlacements = new Dictionary<AxialI, string>();

		// Token: 0x02003C49 RID: 15433
		public class World
		{
			// Token: 0x0400E8BA RID: 59578
			public Data data = new Data();

			// Token: 0x0400E8BB RID: 59579
			public string name = string.Empty;

			// Token: 0x0400E8BC RID: 59580
			public bool isDiscovered;

			// Token: 0x0400E8BD RID: 59581
			public List<string> traits = new List<string>();

			// Token: 0x0400E8BE RID: 59582
			public List<string> storyTraits = new List<string>();

			// Token: 0x0400E8BF RID: 59583
			public List<string> seasons = new List<string>();

			// Token: 0x0400E8C0 RID: 59584
			public List<string> generatedSubworlds = new List<string>();
		}

		// Token: 0x02003C4A RID: 15434
		public enum POIType
		{
			// Token: 0x0400E8C2 RID: 59586
			TemporalTear,
			// Token: 0x0400E8C3 RID: 59587
			ResearchDestination
		}
	}
}
