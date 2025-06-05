using System;
using STRINGS;

namespace Database
{
	// Token: 0x02002223 RID: 8739
	public class RadBoltTravelDistance : ColonyAchievementRequirement
	{
		// Token: 0x0600B9E2 RID: 47586 RVA: 0x0011C456 File Offset: 0x0011A656
		public RadBoltTravelDistance(int travelDistance)
		{
			this.travelDistance = travelDistance;
		}

		// Token: 0x0600B9E3 RID: 47587 RVA: 0x0011C465 File Offset: 0x0011A665
		public override string GetProgress(bool complete)
		{
			return string.Format(COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.RADBOLT_TRAVEL, SaveGame.Instance.ColonyAchievementTracker.radBoltTravelDistance, this.travelDistance);
		}

		// Token: 0x0600B9E4 RID: 47588 RVA: 0x0011C495 File Offset: 0x0011A695
		public override bool Success()
		{
			return SaveGame.Instance.ColonyAchievementTracker.radBoltTravelDistance > (float)this.travelDistance;
		}

		// Token: 0x0400979F RID: 38815
		private int travelDistance;
	}
}
