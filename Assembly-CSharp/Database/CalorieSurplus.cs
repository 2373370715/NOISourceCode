using System;
using STRINGS;

namespace Database
{
	// Token: 0x020021FF RID: 8703
	public class CalorieSurplus : ColonyAchievementRequirement, AchievementRequirementSerialization_Deprecated
	{
		// Token: 0x0600B955 RID: 47445 RVA: 0x0011BF93 File Offset: 0x0011A193
		public CalorieSurplus(float surplusAmount)
		{
			this.surplusAmount = (double)surplusAmount;
		}

		// Token: 0x0600B956 RID: 47446 RVA: 0x0011BFA3 File Offset: 0x0011A1A3
		public override bool Success()
		{
			return (double)(ClusterManager.Instance.CountAllRations() / 1000f) >= this.surplusAmount;
		}

		// Token: 0x0600B957 RID: 47447 RVA: 0x0011BCD7 File Offset: 0x00119ED7
		public override bool Fail()
		{
			return !this.Success();
		}

		// Token: 0x0600B958 RID: 47448 RVA: 0x0011BFC1 File Offset: 0x0011A1C1
		public void Deserialize(IReader reader)
		{
			this.surplusAmount = reader.ReadDouble();
		}

		// Token: 0x0600B959 RID: 47449 RVA: 0x0011BFCF File Offset: 0x0011A1CF
		public override string GetProgress(bool complete)
		{
			return string.Format(COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.CALORIE_SURPLUS, GameUtil.GetFormattedCalories(complete ? ((float)this.surplusAmount) : ClusterManager.Instance.CountAllRations(), GameUtil.TimeSlice.None, true), GameUtil.GetFormattedCalories((float)this.surplusAmount, GameUtil.TimeSlice.None, true));
		}

		// Token: 0x04009777 RID: 38775
		private double surplusAmount;
	}
}
