using System;
using System.Collections.Generic;
using STRINGS;

namespace Database
{
	// Token: 0x02002225 RID: 8741
	public class SurviveARocketWithMinimumMorale : ColonyAchievementRequirement
	{
		// Token: 0x0600B9E8 RID: 47592 RVA: 0x0011C4CC File Offset: 0x0011A6CC
		public SurviveARocketWithMinimumMorale(float minimumMorale, int numberOfCycles)
		{
			this.minimumMorale = minimumMorale;
			this.numberOfCycles = numberOfCycles;
		}

		// Token: 0x0600B9E9 RID: 47593 RVA: 0x0011C4E2 File Offset: 0x0011A6E2
		public override string GetProgress(bool complete)
		{
			if (complete)
			{
				return string.Format(COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.SURVIVE_SPACE_COMPLETE, this.minimumMorale, this.numberOfCycles);
			}
			return base.GetProgress(complete);
		}

		// Token: 0x0600B9EA RID: 47594 RVA: 0x004783B8 File Offset: 0x004765B8
		public override bool Success()
		{
			foreach (KeyValuePair<int, int> keyValuePair in SaveGame.Instance.ColonyAchievementTracker.cyclesRocketDupeMoraleAboveRequirement)
			{
				if (keyValuePair.Value >= this.numberOfCycles)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x040097A0 RID: 38816
		public float minimumMorale;

		// Token: 0x040097A1 RID: 38817
		public int numberOfCycles;
	}
}
