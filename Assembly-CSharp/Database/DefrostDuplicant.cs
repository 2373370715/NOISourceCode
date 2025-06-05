using System;
using STRINGS;

namespace Database
{
	// Token: 0x0200221E RID: 8734
	public class DefrostDuplicant : ColonyAchievementRequirement
	{
		// Token: 0x0600B9D3 RID: 47571 RVA: 0x0011C384 File Offset: 0x0011A584
		public override string GetProgress(bool complete)
		{
			return COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.DEFROST_DUPLICANT;
		}

		// Token: 0x0600B9D4 RID: 47572 RVA: 0x0011C390 File Offset: 0x0011A590
		public override bool Success()
		{
			return SaveGame.Instance.ColonyAchievementTracker.defrostedDuplicant;
		}
	}
}
