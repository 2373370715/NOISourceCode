using System;
using STRINGS;

namespace Database
{
	// Token: 0x02002208 RID: 8712
	public class EatXCalories : ColonyAchievementRequirement, AchievementRequirementSerialization_Deprecated
	{
		// Token: 0x0600B97C RID: 47484 RVA: 0x0011C0E2 File Offset: 0x0011A2E2
		public EatXCalories(int numCalories)
		{
			this.numCalories = numCalories;
		}

		// Token: 0x0600B97D RID: 47485 RVA: 0x0011C0F1 File Offset: 0x0011A2F1
		public override bool Success()
		{
			return WorldResourceAmountTracker<RationTracker>.Get().GetAmountConsumed() / 1000f > (float)this.numCalories;
		}

		// Token: 0x0600B97E RID: 47486 RVA: 0x0011C10C File Offset: 0x0011A30C
		public void Deserialize(IReader reader)
		{
			this.numCalories = reader.ReadInt32();
		}

		// Token: 0x0600B97F RID: 47487 RVA: 0x00476F24 File Offset: 0x00475124
		public override string GetProgress(bool complete)
		{
			return string.Format(COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.CONSUME_CALORIES, GameUtil.GetFormattedCalories(complete ? ((float)this.numCalories * 1000f) : WorldResourceAmountTracker<RationTracker>.Get().GetAmountConsumed(), GameUtil.TimeSlice.None, true), GameUtil.GetFormattedCalories((float)this.numCalories * 1000f, GameUtil.TimeSlice.None, true));
		}

		// Token: 0x04009786 RID: 38790
		private int numCalories;
	}
}
