using System;
using STRINGS;

namespace Database
{
	// Token: 0x020021F2 RID: 8690
	public class FractionalCycleNumber : ColonyAchievementRequirement, AchievementRequirementSerialization_Deprecated
	{
		// Token: 0x0600B911 RID: 47377 RVA: 0x0011BD28 File Offset: 0x00119F28
		public FractionalCycleNumber(float fractionalCycleNumber)
		{
			this.fractionalCycleNumber = fractionalCycleNumber;
		}

		// Token: 0x0600B912 RID: 47378 RVA: 0x00476004 File Offset: 0x00474204
		public override bool Success()
		{
			int num = (int)this.fractionalCycleNumber;
			float num2 = this.fractionalCycleNumber - (float)num;
			return (float)(GameClock.Instance.GetCycle() + 1) > this.fractionalCycleNumber || (GameClock.Instance.GetCycle() + 1 == num && GameClock.Instance.GetCurrentCycleAsPercentage() >= num2);
		}

		// Token: 0x0600B913 RID: 47379 RVA: 0x0011BD37 File Offset: 0x00119F37
		public void Deserialize(IReader reader)
		{
			this.fractionalCycleNumber = reader.ReadSingle();
		}

		// Token: 0x0600B914 RID: 47380 RVA: 0x0047605C File Offset: 0x0047425C
		public override string GetProgress(bool complete)
		{
			float num = (float)GameClock.Instance.GetCycle() + GameClock.Instance.GetCurrentCycleAsPercentage();
			return string.Format(COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.FRACTIONAL_CYCLE, complete ? this.fractionalCycleNumber : num, this.fractionalCycleNumber);
		}

		// Token: 0x0400976E RID: 38766
		private float fractionalCycleNumber;
	}
}
