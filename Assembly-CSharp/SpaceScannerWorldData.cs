using System;
using System.Collections.Generic;
using Klei.AI;
using KSerialization;

// Token: 0x020019ED RID: 6637
[Serialize]
[SerializationConfig(MemberSerialization.OptIn)]
[Serializable]
public class SpaceScannerWorldData
{
	// Token: 0x06008A5C RID: 35420 RVA: 0x000FEFBA File Offset: 0x000FD1BA
	[Serialize]
	public SpaceScannerWorldData(int worldId)
	{
		this.worldId = worldId;
	}

	// Token: 0x06008A5D RID: 35421 RVA: 0x000FEFEA File Offset: 0x000FD1EA
	public WorldContainer GetWorld()
	{
		if (this.world == null)
		{
			this.world = ClusterManager.Instance.GetWorld(this.worldId);
		}
		return this.world;
	}

	// Token: 0x04006864 RID: 26724
	[NonSerialized]
	private WorldContainer world;

	// Token: 0x04006865 RID: 26725
	[Serialize]
	public int worldId;

	// Token: 0x04006866 RID: 26726
	[Serialize]
	public float networkQuality01;

	// Token: 0x04006867 RID: 26727
	[Serialize]
	public Dictionary<string, float> targetIdToRandomValue01Map = new Dictionary<string, float>();

	// Token: 0x04006868 RID: 26728
	[Serialize]
	public HashSet<string> targetIdsDetected = new HashSet<string>();

	// Token: 0x04006869 RID: 26729
	[NonSerialized]
	public SpaceScannerWorldData.Scratchpad scratchpad = new SpaceScannerWorldData.Scratchpad();

	// Token: 0x020019EE RID: 6638
	public class Scratchpad
	{
		// Token: 0x0400686A RID: 26730
		public List<ClusterTraveler> ballisticObjects = new List<ClusterTraveler>();

		// Token: 0x0400686B RID: 26731
		public HashSet<MeteorShowerEvent.StatesInstance> lastDetectedMeteorShowers = new HashSet<MeteorShowerEvent.StatesInstance>();

		// Token: 0x0400686C RID: 26732
		public HashSet<LaunchConditionManager> lastDetectedRocketsBaseGame = new HashSet<LaunchConditionManager>();

		// Token: 0x0400686D RID: 26733
		public HashSet<Clustercraft> lastDetectedRocketsDLC1 = new HashSet<Clustercraft>();
	}
}
