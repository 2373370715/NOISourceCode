using System;
using STRINGS;

namespace Database
{
	// Token: 0x02002228 RID: 8744
	public class AllTheCircuitsCompleteCheck : ColonyAchievementRequirement
	{
		// Token: 0x0600B9F1 RID: 47601 RVA: 0x0011C540 File Offset: 0x0011A740
		public override string GetProgress(bool complete)
		{
			return string.Format(COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.MVB_DESCRIPTION, 8);
		}

		// Token: 0x0600B9F2 RID: 47602 RVA: 0x0011C557 File Offset: 0x0011A757
		public override bool Success()
		{
			return SaveGame.Instance.ColonyAchievementTracker.fullyBoostedBionic;
		}
	}
}
