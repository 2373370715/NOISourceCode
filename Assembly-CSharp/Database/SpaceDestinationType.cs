using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Database
{
	// Token: 0x020021D2 RID: 8658
	[DebuggerDisplay("{Id}")]
	public class SpaceDestinationType : Resource
	{
		// Token: 0x17000BEA RID: 3050
		// (get) Token: 0x0600B895 RID: 47253 RVA: 0x0011B837 File Offset: 0x00119A37
		// (set) Token: 0x0600B896 RID: 47254 RVA: 0x0011B83F File Offset: 0x00119A3F
		public int maxiumMass { get; private set; }

		// Token: 0x17000BEB RID: 3051
		// (get) Token: 0x0600B897 RID: 47255 RVA: 0x0011B848 File Offset: 0x00119A48
		// (set) Token: 0x0600B898 RID: 47256 RVA: 0x0011B850 File Offset: 0x00119A50
		public int minimumMass { get; private set; }

		// Token: 0x17000BEC RID: 3052
		// (get) Token: 0x0600B899 RID: 47257 RVA: 0x0011B859 File Offset: 0x00119A59
		public float replishmentPerCycle
		{
			get
			{
				return 1000f / (float)this.cyclesToRecover;
			}
		}

		// Token: 0x17000BED RID: 3053
		// (get) Token: 0x0600B89A RID: 47258 RVA: 0x0011B868 File Offset: 0x00119A68
		public float replishmentPerSim1000ms
		{
			get
			{
				return 1000f / ((float)this.cyclesToRecover * 600f);
			}
		}

		// Token: 0x0600B89B RID: 47259 RVA: 0x00470E30 File Offset: 0x0046F030
		public SpaceDestinationType(string id, ResourceSet parent, string name, string description, int iconSize, string spriteName, Dictionary<SimHashes, MathUtil.MinMax> elementTable, Dictionary<string, int> recoverableEntities = null, ArtifactDropRate artifactDropRate = null, int max = 64000000, int min = 63994000, int cycles = 6, bool visitable = true) : base(id, parent, name)
		{
			this.typeName = name;
			this.description = description;
			this.iconSize = iconSize;
			this.spriteName = spriteName;
			this.elementTable = elementTable;
			this.recoverableEntities = recoverableEntities;
			this.artifactDropTable = artifactDropRate;
			this.maxiumMass = max;
			this.minimumMass = min;
			this.cyclesToRecover = cycles;
			this.visitable = visitable;
		}

		// Token: 0x0400969D RID: 38557
		public const float MASS_TO_RECOVER = 1000f;

		// Token: 0x0400969E RID: 38558
		public string typeName;

		// Token: 0x0400969F RID: 38559
		public string description;

		// Token: 0x040096A0 RID: 38560
		public int iconSize = 128;

		// Token: 0x040096A1 RID: 38561
		public string spriteName;

		// Token: 0x040096A2 RID: 38562
		public Dictionary<SimHashes, MathUtil.MinMax> elementTable;

		// Token: 0x040096A3 RID: 38563
		public Dictionary<string, int> recoverableEntities;

		// Token: 0x040096A4 RID: 38564
		public ArtifactDropRate artifactDropTable;

		// Token: 0x040096A5 RID: 38565
		public bool visitable;

		// Token: 0x040096A8 RID: 38568
		public int cyclesToRecover;
	}
}
