using System;
using STRINGS;

namespace Database
{
	// Token: 0x020021FA RID: 8698
	public class ClearBlockedGeothermalVent : VictoryColonyAchievementRequirement
	{
		// Token: 0x0600B93C RID: 47420 RVA: 0x0011BE2E File Offset: 0x0011A02E
		public override string Description()
		{
			return this.GetProgress(this.Success());
		}

		// Token: 0x0600B93D RID: 47421 RVA: 0x0011BEDA File Offset: 0x0011A0DA
		public override string Name()
		{
			return COLONY_ACHIEVEMENTS.ACTIVATEGEOTHERMALPLANT.REQUIREMENTS.UNBLOCK_VENT_TITLE;
		}

		// Token: 0x0600B93E RID: 47422 RVA: 0x0011BEE6 File Offset: 0x0011A0E6
		public override bool Success()
		{
			return SaveGame.Instance.ColonyAchievementTracker.GeothermalClearedEntombedVent;
		}

		// Token: 0x0600B93F RID: 47423 RVA: 0x0011BEF7 File Offset: 0x0011A0F7
		public override string GetProgress(bool complete)
		{
			return COLONY_ACHIEVEMENTS.ACTIVATEGEOTHERMALPLANT.REQUIREMENTS.UNBLOCK_VENT_DESCRIPTION;
		}
	}
}
