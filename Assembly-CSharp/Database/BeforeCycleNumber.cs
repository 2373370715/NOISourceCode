using System;
using STRINGS;
using UnityEngine;

namespace Database
{
	// Token: 0x020021F1 RID: 8689
	public class BeforeCycleNumber : ColonyAchievementRequirement, AchievementRequirementSerialization_Deprecated
	{
		// Token: 0x0600B90C RID: 47372 RVA: 0x0011BCAF File Offset: 0x00119EAF
		public BeforeCycleNumber(int cycleNumber = 100)
		{
			this.cycleNumber = cycleNumber;
		}

		// Token: 0x0600B90D RID: 47373 RVA: 0x0011BCBE File Offset: 0x00119EBE
		public override bool Success()
		{
			return GameClock.Instance.GetCycle() + 1 <= this.cycleNumber;
		}

		// Token: 0x0600B90E RID: 47374 RVA: 0x0011BCD7 File Offset: 0x00119ED7
		public override bool Fail()
		{
			return !this.Success();
		}

		// Token: 0x0600B90F RID: 47375 RVA: 0x0011BCE2 File Offset: 0x00119EE2
		public void Deserialize(IReader reader)
		{
			this.cycleNumber = reader.ReadInt32();
		}

		// Token: 0x0600B910 RID: 47376 RVA: 0x0011BCF0 File Offset: 0x00119EF0
		public override string GetProgress(bool complete)
		{
			return string.Format(COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.REMAINING_CYCLES, Mathf.Max(this.cycleNumber - GameClock.Instance.GetCycle(), 0), this.cycleNumber);
		}

		// Token: 0x0400976D RID: 38765
		private int cycleNumber;
	}
}
