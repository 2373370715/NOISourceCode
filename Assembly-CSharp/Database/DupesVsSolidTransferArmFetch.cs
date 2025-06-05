using System;
using System.Collections.Generic;

namespace Database
{
	// Token: 0x02002219 RID: 8729
	public class DupesVsSolidTransferArmFetch : ColonyAchievementRequirement, AchievementRequirementSerialization_Deprecated
	{
		// Token: 0x0600B9C1 RID: 47553 RVA: 0x0011C2D6 File Offset: 0x0011A4D6
		public DupesVsSolidTransferArmFetch(float percentage, int numCycles)
		{
			this.percentage = percentage;
			this.numCycles = numCycles;
		}

		// Token: 0x0600B9C2 RID: 47554 RVA: 0x00477EE8 File Offset: 0x004760E8
		public override bool Success()
		{
			Dictionary<int, int> fetchDupeChoreDeliveries = SaveGame.Instance.ColonyAchievementTracker.fetchDupeChoreDeliveries;
			Dictionary<int, int> fetchAutomatedChoreDeliveries = SaveGame.Instance.ColonyAchievementTracker.fetchAutomatedChoreDeliveries;
			int num = 0;
			this.currentCycleCount = 0;
			for (int i = GameClock.Instance.GetCycle() - 1; i >= GameClock.Instance.GetCycle() - this.numCycles; i--)
			{
				if (fetchAutomatedChoreDeliveries.ContainsKey(i))
				{
					if (fetchDupeChoreDeliveries.ContainsKey(i) && (float)fetchDupeChoreDeliveries[i] >= (float)fetchAutomatedChoreDeliveries[i] * this.percentage)
					{
						break;
					}
					num++;
				}
				else if (fetchDupeChoreDeliveries.ContainsKey(i))
				{
					num = 0;
					break;
				}
			}
			this.currentCycleCount = Math.Max(this.currentCycleCount, num);
			return num >= this.numCycles;
		}

		// Token: 0x0600B9C3 RID: 47555 RVA: 0x0011C2EC File Offset: 0x0011A4EC
		public void Deserialize(IReader reader)
		{
			this.numCycles = reader.ReadInt32();
			this.percentage = reader.ReadSingle();
		}

		// Token: 0x04009797 RID: 38807
		public float percentage;

		// Token: 0x04009798 RID: 38808
		public int numCycles;

		// Token: 0x04009799 RID: 38809
		public int currentCycleCount;

		// Token: 0x0400979A RID: 38810
		public bool armsOutPerformingDupesThisCycle;
	}
}
