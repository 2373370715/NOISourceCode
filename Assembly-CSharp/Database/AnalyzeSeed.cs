using System;
using STRINGS;

namespace Database
{
	// Token: 0x02002220 RID: 8736
	public class AnalyzeSeed : ColonyAchievementRequirement
	{
		// Token: 0x0600B9D9 RID: 47577 RVA: 0x0011C3AD File Offset: 0x0011A5AD
		public AnalyzeSeed(string seedname)
		{
			this.seedName = seedname;
		}

		// Token: 0x0600B9DA RID: 47578 RVA: 0x0011C3BC File Offset: 0x0011A5BC
		public override string GetProgress(bool complete)
		{
			return string.Format(COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.ANALYZE_SEED, this.seedName.ProperName());
		}

		// Token: 0x0600B9DB RID: 47579 RVA: 0x0011C3DD File Offset: 0x0011A5DD
		public override bool Success()
		{
			return SaveGame.Instance.ColonyAchievementTracker.analyzedSeeds.Contains(this.seedName);
		}

		// Token: 0x0400979D RID: 38813
		private string seedName;
	}
}
