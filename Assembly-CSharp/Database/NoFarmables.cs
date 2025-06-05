using System;
using System.Collections.Generic;
using STRINGS;

namespace Database
{
	// Token: 0x02002206 RID: 8710
	public class NoFarmables : ColonyAchievementRequirement, AchievementRequirementSerialization_Deprecated
	{
		// Token: 0x0600B973 RID: 47475 RVA: 0x00476D84 File Offset: 0x00474F84
		public override bool Success()
		{
			foreach (WorldContainer worldContainer in ClusterManager.Instance.WorldContainers)
			{
				foreach (PlantablePlot plantablePlot in Components.PlantablePlots.GetItems(worldContainer.id))
				{
					if (plantablePlot.Occupant != null)
					{
						using (IEnumerator<Tag> enumerator3 = plantablePlot.possibleDepositObjectTags.GetEnumerator())
						{
							while (enumerator3.MoveNext())
							{
								if (enumerator3.Current != GameTags.DecorSeed)
								{
									return false;
								}
							}
						}
					}
				}
			}
			return true;
		}

		// Token: 0x0600B974 RID: 47476 RVA: 0x0011BCD7 File Offset: 0x00119ED7
		public override bool Fail()
		{
			return !this.Success();
		}

		// Token: 0x0600B975 RID: 47477 RVA: 0x000AA038 File Offset: 0x000A8238
		public void Deserialize(IReader reader)
		{
		}

		// Token: 0x0600B976 RID: 47478 RVA: 0x0011C094 File Offset: 0x0011A294
		public override string GetProgress(bool complete)
		{
			return COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.NO_FARM_TILES;
		}
	}
}
