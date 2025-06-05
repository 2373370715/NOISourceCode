using System;
using STRINGS;

namespace Database
{
	// Token: 0x02002224 RID: 8740
	public class HarvestAHiveWithoutBeingStung : ColonyAchievementRequirement
	{
		// Token: 0x0600B9E5 RID: 47589 RVA: 0x0011C4AF File Offset: 0x0011A6AF
		public override string GetProgress(bool complete)
		{
			return COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.HARVEST_HIVE;
		}

		// Token: 0x0600B9E6 RID: 47590 RVA: 0x0011C4BB File Offset: 0x0011A6BB
		public override bool Success()
		{
			return SaveGame.Instance.ColonyAchievementTracker.harvestAHiveWithoutGettingStung;
		}
	}
}
