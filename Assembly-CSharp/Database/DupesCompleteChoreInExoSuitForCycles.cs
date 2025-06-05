using System;
using System.Collections.Generic;
using System.Linq;

namespace Database
{
	// Token: 0x0200221A RID: 8730
	public class DupesCompleteChoreInExoSuitForCycles : ColonyAchievementRequirement, AchievementRequirementSerialization_Deprecated
	{
		// Token: 0x0600B9C4 RID: 47556 RVA: 0x0011C306 File Offset: 0x0011A506
		public DupesCompleteChoreInExoSuitForCycles(int numCycles)
		{
			this.numCycles = numCycles;
		}

		// Token: 0x0600B9C5 RID: 47557 RVA: 0x00477FA4 File Offset: 0x004761A4
		public override bool Success()
		{
			Dictionary<int, List<int>> dupesCompleteChoresInSuits = SaveGame.Instance.ColonyAchievementTracker.dupesCompleteChoresInSuits;
			Dictionary<int, float> dictionary = new Dictionary<int, float>();
			foreach (MinionIdentity minionIdentity in Components.MinionIdentities.Items)
			{
				KPrefabID component = minionIdentity.GetComponent<KPrefabID>();
				if (!component.HasTag(GameTags.Dead))
				{
					dictionary.Add(component.InstanceID, minionIdentity.arrivalTime);
				}
			}
			int num = 0;
			int num2 = Math.Min(dupesCompleteChoresInSuits.Count, this.numCycles);
			for (int i = GameClock.Instance.GetCycle() - num2; i <= GameClock.Instance.GetCycle(); i++)
			{
				if (dupesCompleteChoresInSuits.ContainsKey(i))
				{
					List<int> list = dictionary.Keys.Except(dupesCompleteChoresInSuits[i]).ToList<int>();
					bool flag = true;
					foreach (int key in list)
					{
						if (dictionary[key] < (float)i)
						{
							flag = false;
							break;
						}
					}
					if (flag)
					{
						num++;
					}
					else if (i != GameClock.Instance.GetCycle())
					{
						num = 0;
					}
					this.currentCycleStreak = num;
					if (num >= this.numCycles)
					{
						this.currentCycleStreak = this.numCycles;
						return true;
					}
				}
				else
				{
					this.currentCycleStreak = Math.Max(this.currentCycleStreak, num);
					num = 0;
				}
			}
			return false;
		}

		// Token: 0x0600B9C6 RID: 47558 RVA: 0x0011C315 File Offset: 0x0011A515
		public void Deserialize(IReader reader)
		{
			this.numCycles = reader.ReadInt32();
		}

		// Token: 0x0600B9C7 RID: 47559 RVA: 0x00478134 File Offset: 0x00476334
		public int GetNumberOfDupesForCycle(int cycle)
		{
			int result = 0;
			Dictionary<int, List<int>> dupesCompleteChoresInSuits = SaveGame.Instance.ColonyAchievementTracker.dupesCompleteChoresInSuits;
			if (dupesCompleteChoresInSuits.ContainsKey(GameClock.Instance.GetCycle()))
			{
				result = dupesCompleteChoresInSuits[GameClock.Instance.GetCycle()].Count;
			}
			return result;
		}

		// Token: 0x0400979B RID: 38811
		public int currentCycleStreak;

		// Token: 0x0400979C RID: 38812
		public int numCycles;
	}
}
