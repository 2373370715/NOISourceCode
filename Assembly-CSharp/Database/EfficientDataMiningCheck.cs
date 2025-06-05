using System;
using STRINGS;

namespace Database
{
	// Token: 0x02002227 RID: 8743
	public class EfficientDataMiningCheck : ColonyAchievementRequirement
	{
		// Token: 0x0600B9EE RID: 47598 RVA: 0x0011C523 File Offset: 0x0011A723
		public override string GetProgress(bool complete)
		{
			return COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.DATA_DRIVEN_DESCRIPTION;
		}

		// Token: 0x0600B9EF RID: 47599 RVA: 0x0011C52F File Offset: 0x0011A72F
		public override bool Success()
		{
			return SaveGame.Instance.ColonyAchievementTracker.efficientlyGatheredData;
		}
	}
}
