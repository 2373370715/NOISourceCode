using System;
using STRINGS;

namespace Database
{
	// Token: 0x020021F0 RID: 8688
	public class CycleNumber : VictoryColonyAchievementRequirement, AchievementRequirementSerialization_Deprecated
	{
		// Token: 0x0600B906 RID: 47366 RVA: 0x0011BC09 File Offset: 0x00119E09
		public override string Name()
		{
			return string.Format(COLONY_ACHIEVEMENTS.THRIVING.REQUIREMENTS.MINIMUM_CYCLE, this.cycleNumber);
		}

		// Token: 0x0600B907 RID: 47367 RVA: 0x0011BC25 File Offset: 0x00119E25
		public override string Description()
		{
			return string.Format(COLONY_ACHIEVEMENTS.THRIVING.REQUIREMENTS.MINIMUM_CYCLE_DESCRIPTION, this.cycleNumber);
		}

		// Token: 0x0600B908 RID: 47368 RVA: 0x0011BC41 File Offset: 0x00119E41
		public CycleNumber(int cycleNumber = 100)
		{
			this.cycleNumber = cycleNumber;
		}

		// Token: 0x0600B909 RID: 47369 RVA: 0x0011BC50 File Offset: 0x00119E50
		public override bool Success()
		{
			return GameClock.Instance.GetCycle() + 1 >= this.cycleNumber;
		}

		// Token: 0x0600B90A RID: 47370 RVA: 0x0011BC69 File Offset: 0x00119E69
		public void Deserialize(IReader reader)
		{
			this.cycleNumber = reader.ReadInt32();
		}

		// Token: 0x0600B90B RID: 47371 RVA: 0x0011BC77 File Offset: 0x00119E77
		public override string GetProgress(bool complete)
		{
			return string.Format(COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.CYCLE_NUMBER, complete ? this.cycleNumber : (GameClock.Instance.GetCycle() + 1), this.cycleNumber);
		}

		// Token: 0x0400976C RID: 38764
		private int cycleNumber;
	}
}
