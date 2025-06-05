using System;
using STRINGS;

namespace Database
{
	// Token: 0x02002218 RID: 8728
	public class BlockedCometWithBunkerDoor : ColonyAchievementRequirement, AchievementRequirementSerialization_Deprecated
	{
		// Token: 0x0600B9BE RID: 47550 RVA: 0x0011C2B9 File Offset: 0x0011A4B9
		public override bool Success()
		{
			return Game.Instance.savedInfo.blockedCometWithBunkerDoor;
		}

		// Token: 0x0600B9BF RID: 47551 RVA: 0x000AA038 File Offset: 0x000A8238
		public void Deserialize(IReader reader)
		{
		}

		// Token: 0x0600B9C0 RID: 47552 RVA: 0x0011C2CA File Offset: 0x0011A4CA
		public override string GetProgress(bool complete)
		{
			return COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.BLOCKED_A_COMET;
		}
	}
}
