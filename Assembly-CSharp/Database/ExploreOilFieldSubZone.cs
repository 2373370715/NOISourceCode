using System;
using STRINGS;

namespace Database
{
	// Token: 0x0200220C RID: 8716
	public class ExploreOilFieldSubZone : ColonyAchievementRequirement, AchievementRequirementSerialization_Deprecated
	{
		// Token: 0x0600B98D RID: 47501 RVA: 0x0011C152 File Offset: 0x0011A352
		public override bool Success()
		{
			return Game.Instance.savedInfo.discoveredOilField;
		}

		// Token: 0x0600B98E RID: 47502 RVA: 0x000AA038 File Offset: 0x000A8238
		public void Deserialize(IReader reader)
		{
		}

		// Token: 0x0600B98F RID: 47503 RVA: 0x0011C163 File Offset: 0x0011A363
		public override string GetProgress(bool complete)
		{
			return COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.ENTER_OIL_BIOME;
		}
	}
}
