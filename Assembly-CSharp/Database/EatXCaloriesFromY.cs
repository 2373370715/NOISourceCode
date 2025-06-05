using System;
using System.Collections.Generic;
using STRINGS;

namespace Database
{
	// Token: 0x02002207 RID: 8711
	public class EatXCaloriesFromY : ColonyAchievementRequirement, AchievementRequirementSerialization_Deprecated
	{
		// Token: 0x0600B978 RID: 47480 RVA: 0x0011C0A0 File Offset: 0x0011A2A0
		public EatXCaloriesFromY(int numCalories, List<string> fromFoodType)
		{
			this.numCalories = numCalories;
			this.fromFoodType = fromFoodType;
		}

		// Token: 0x0600B979 RID: 47481 RVA: 0x0011C0C1 File Offset: 0x0011A2C1
		public override bool Success()
		{
			return WorldResourceAmountTracker<RationTracker>.Get().GetAmountConsumedForIDs(this.fromFoodType) / 1000f > (float)this.numCalories;
		}

		// Token: 0x0600B97A RID: 47482 RVA: 0x00476E7C File Offset: 0x0047507C
		public void Deserialize(IReader reader)
		{
			this.numCalories = reader.ReadInt32();
			int num = reader.ReadInt32();
			this.fromFoodType = new List<string>(num);
			for (int i = 0; i < num; i++)
			{
				string item = reader.ReadKleiString();
				this.fromFoodType.Add(item);
			}
		}

		// Token: 0x0600B97B RID: 47483 RVA: 0x00476EC8 File Offset: 0x004750C8
		public override string GetProgress(bool complete)
		{
			return string.Format(COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.CALORIES_FROM_MEAT, GameUtil.GetFormattedCalories(complete ? ((float)this.numCalories * 1000f) : WorldResourceAmountTracker<RationTracker>.Get().GetAmountConsumedForIDs(this.fromFoodType), GameUtil.TimeSlice.None, true), GameUtil.GetFormattedCalories((float)this.numCalories * 1000f, GameUtil.TimeSlice.None, true));
		}

		// Token: 0x04009784 RID: 38788
		private int numCalories;

		// Token: 0x04009785 RID: 38789
		private List<string> fromFoodType = new List<string>();
	}
}
