﻿using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Database
{
	[DebuggerDisplay("{Id}")]
	public class SpaceDestinationType : Resource
	{
		public int maxiumMass { get; private set; }

		public int minimumMass { get; private set; }

		public float replishmentPerCycle
		{
			get
			{
				return 1000f / (float)this.cyclesToRecover;
			}
		}

		public float replishmentPerSim1000ms
		{
			get
			{
				return 1000f / ((float)this.cyclesToRecover * 600f);
			}
		}

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

		public const float MASS_TO_RECOVER = 1000f;

		public string typeName;

		public string description;

		public int iconSize = 128;

		public string spriteName;

		public Dictionary<SimHashes, MathUtil.MinMax> elementTable;

		public Dictionary<string, int> recoverableEntities;

		public ArtifactDropRate artifactDropTable;

		public bool visitable;

		public int cyclesToRecover;
	}
}
