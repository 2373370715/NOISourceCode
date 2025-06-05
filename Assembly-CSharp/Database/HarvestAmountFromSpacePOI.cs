using System;
using STRINGS;

namespace Database
{
	// Token: 0x02002221 RID: 8737
	public class HarvestAmountFromSpacePOI : ColonyAchievementRequirement
	{
		// Token: 0x0600B9DC RID: 47580 RVA: 0x0011C3FE File Offset: 0x0011A5FE
		public HarvestAmountFromSpacePOI(float amountToHarvest)
		{
			this.amountToHarvest = amountToHarvest;
		}

		// Token: 0x0600B9DD RID: 47581 RVA: 0x0011C40D File Offset: 0x0011A60D
		public override string GetProgress(bool complete)
		{
			return string.Format(COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.MINE_SPACE_POI, SaveGame.Instance.ColonyAchievementTracker.totalMaterialsHarvestFromPOI, this.amountToHarvest);
		}

		// Token: 0x0600B9DE RID: 47582 RVA: 0x0011C43D File Offset: 0x0011A63D
		public override bool Success()
		{
			return SaveGame.Instance.ColonyAchievementTracker.totalMaterialsHarvestFromPOI > this.amountToHarvest;
		}

		// Token: 0x0400979E RID: 38814
		private float amountToHarvest;
	}
}
