using System;
using STRINGS;

namespace Database
{
	// Token: 0x020021F9 RID: 8697
	public class UseGeothermalPlant : VictoryColonyAchievementRequirement
	{
		// Token: 0x0600B937 RID: 47415 RVA: 0x0011BE2E File Offset: 0x0011A02E
		public override string Description()
		{
			return this.GetProgress(this.Success());
		}

		// Token: 0x0600B938 RID: 47416 RVA: 0x0011BEB1 File Offset: 0x0011A0B1
		public override string Name()
		{
			return COLONY_ACHIEVEMENTS.ACTIVATEGEOTHERMALPLANT.REQUIREMENTS.ACTIVATE_PLANT_TITLE;
		}

		// Token: 0x0600B939 RID: 47417 RVA: 0x0011BEBD File Offset: 0x0011A0BD
		public override bool Success()
		{
			return SaveGame.Instance.ColonyAchievementTracker.GeothermalControllerHasVented;
		}

		// Token: 0x0600B93A RID: 47418 RVA: 0x0011BECE File Offset: 0x0011A0CE
		public override string GetProgress(bool complete)
		{
			return COLONY_ACHIEVEMENTS.ACTIVATEGEOTHERMALPLANT.REQUIREMENTS.ACTIVATE_PLANT_DESCRIPTION;
		}
	}
}
