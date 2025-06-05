using System;
using STRINGS;

namespace Database
{
	// Token: 0x0200221B RID: 8731
	public class SentCraftIntoTemporalTear : VictoryColonyAchievementRequirement
	{
		// Token: 0x0600B9C8 RID: 47560 RVA: 0x0011C323 File Offset: 0x0011A523
		public override string Name()
		{
			return string.Format(COLONY_ACHIEVEMENTS.DISTANT_PLANET_REACHED.REQUIREMENTS.REACHED_SPACE_DESTINATION, UI.SPACEDESTINATIONS.WORMHOLE.NAME);
		}

		// Token: 0x0600B9C9 RID: 47561 RVA: 0x0011C339 File Offset: 0x0011A539
		public override string Description()
		{
			return string.Format(COLONY_ACHIEVEMENTS.DISTANT_PLANET_REACHED.REQUIREMENTS.REACHED_SPACE_DESTINATION_DESCRIPTION, UI.SPACEDESTINATIONS.WORMHOLE.NAME);
		}

		// Token: 0x0600B9CA RID: 47562 RVA: 0x0011C34F File Offset: 0x0011A54F
		public override string GetProgress(bool completed)
		{
			return COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.LAUNCHED_ROCKET_TO_WORMHOLE;
		}

		// Token: 0x0600B9CB RID: 47563 RVA: 0x0011C35B File Offset: 0x0011A55B
		public override bool Success()
		{
			return ClusterManager.Instance.GetClusterPOIManager().HasTemporalTearConsumedCraft();
		}
	}
}
