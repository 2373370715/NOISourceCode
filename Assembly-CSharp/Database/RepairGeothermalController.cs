using System;
using STRINGS;

namespace Database
{
	// Token: 0x020021F8 RID: 8696
	public class RepairGeothermalController : VictoryColonyAchievementRequirement
	{
		// Token: 0x0600B932 RID: 47410 RVA: 0x0011BE2E File Offset: 0x0011A02E
		public override string Description()
		{
			return this.GetProgress(this.Success());
		}

		// Token: 0x0600B933 RID: 47411 RVA: 0x0011BE99 File Offset: 0x0011A099
		public override string GetProgress(bool complete)
		{
			return COLONY_ACHIEVEMENTS.ACTIVATEGEOTHERMALPLANT.REQUIREMENTS.REPAIR_CONTROLLER_DESCRIPTION;
		}

		// Token: 0x0600B934 RID: 47412 RVA: 0x0011BEA5 File Offset: 0x0011A0A5
		public override string Name()
		{
			return COLONY_ACHIEVEMENTS.ACTIVATEGEOTHERMALPLANT.REQUIREMENTS.REPAIR_CONTROLLER_TITLE;
		}

		// Token: 0x0600B935 RID: 47413 RVA: 0x000E84FB File Offset: 0x000E66FB
		public override bool Success()
		{
			return SaveGame.Instance.ColonyAchievementTracker.GeothermalControllerRepaired;
		}
	}
}
