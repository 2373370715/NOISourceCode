using System;
using STRINGS;

namespace Database
{
	// Token: 0x02002222 RID: 8738
	public class LandOnAllWorlds : ColonyAchievementRequirement
	{
		// Token: 0x0600B9DF RID: 47583 RVA: 0x004782B8 File Offset: 0x004764B8
		public override string GetProgress(bool complete)
		{
			int num = 0;
			int num2 = 0;
			foreach (WorldContainer worldContainer in ClusterManager.Instance.WorldContainers)
			{
				if (!worldContainer.IsModuleInterior)
				{
					num++;
					if (worldContainer.IsDupeVisited || worldContainer.IsRoverVisted)
					{
						num2++;
					}
				}
			}
			return string.Format(COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.LAND_DUPES_ON_ALL_WORLDS, num2, num);
		}

		// Token: 0x0600B9E0 RID: 47584 RVA: 0x00478348 File Offset: 0x00476548
		public override bool Success()
		{
			foreach (WorldContainer worldContainer in ClusterManager.Instance.WorldContainers)
			{
				if (!worldContainer.IsModuleInterior && !worldContainer.IsDupeVisited && !worldContainer.IsRoverVisted)
				{
					return false;
				}
			}
			return true;
		}
	}
}
