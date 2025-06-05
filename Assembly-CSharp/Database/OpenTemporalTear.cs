using System;
using STRINGS;

namespace Database
{
	// Token: 0x020021FB RID: 8699
	public class OpenTemporalTear : VictoryColonyAchievementRequirement
	{
		// Token: 0x0600B941 RID: 47425 RVA: 0x0011BF03 File Offset: 0x0011A103
		public override string GetProgress(bool complete)
		{
			return COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.OPEN_TEMPORAL_TEAR;
		}

		// Token: 0x0600B942 RID: 47426 RVA: 0x0011BE2E File Offset: 0x0011A02E
		public override string Description()
		{
			return this.GetProgress(this.Success());
		}

		// Token: 0x0600B943 RID: 47427 RVA: 0x0011BF0F File Offset: 0x0011A10F
		public override bool Success()
		{
			return ClusterManager.Instance.GetComponent<ClusterPOIManager>().IsTemporalTearOpen();
		}

		// Token: 0x0600B944 RID: 47428 RVA: 0x0011BF20 File Offset: 0x0011A120
		public override string Name()
		{
			return COLONY_ACHIEVEMENTS.DISTANT_PLANET_REACHED.REQUIREMENTS.OPEN_TEMPORAL_TEAR;
		}
	}
}
