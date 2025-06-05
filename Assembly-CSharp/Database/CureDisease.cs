using System;
using STRINGS;

namespace Database
{
	// Token: 0x02002214 RID: 8724
	public class CureDisease : ColonyAchievementRequirement, AchievementRequirementSerialization_Deprecated
	{
		// Token: 0x0600B9AD RID: 47533 RVA: 0x0011C219 File Offset: 0x0011A419
		public override bool Success()
		{
			return Game.Instance.savedInfo.curedDisease;
		}

		// Token: 0x0600B9AE RID: 47534 RVA: 0x000AA038 File Offset: 0x000A8238
		public void Deserialize(IReader reader)
		{
		}

		// Token: 0x0600B9AF RID: 47535 RVA: 0x0011C22A File Offset: 0x0011A42A
		public override string GetProgress(bool complete)
		{
			return COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.CURED_DISEASE;
		}
	}
}
