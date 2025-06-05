using System;
using Klei;
using ProcGen;
using STRINGS;

namespace Database
{
	// Token: 0x0200220A RID: 8714
	public class BuildOutsideStartBiome : ColonyAchievementRequirement, AchievementRequirementSerialization_Deprecated
	{
		// Token: 0x0600B985 RID: 47493 RVA: 0x00477164 File Offset: 0x00475364
		public override bool Success()
		{
			WorldDetailSave clusterDetailSave = SaveLoader.Instance.clusterDetailSave;
			foreach (BuildingComplete buildingComplete in Components.BuildingCompletes.Items)
			{
				if (!buildingComplete.GetComponent<KPrefabID>().HasTag(GameTags.TemplateBuilding))
				{
					for (int i = 0; i < clusterDetailSave.overworldCells.Count; i++)
					{
						WorldDetailSave.OverworldCell overworldCell = clusterDetailSave.overworldCells[i];
						if (overworldCell.tags != null && !overworldCell.tags.Contains(WorldGenTags.StartWorld) && overworldCell.poly.PointInPolygon(buildingComplete.transform.GetPosition()))
						{
							Game.Instance.unlocks.Unlock("buildoutsidestartingbiome", true);
							return true;
						}
					}
				}
			}
			return false;
		}

		// Token: 0x0600B986 RID: 47494 RVA: 0x000AA038 File Offset: 0x000A8238
		public void Deserialize(IReader reader)
		{
		}

		// Token: 0x0600B987 RID: 47495 RVA: 0x0011C130 File Offset: 0x0011A330
		public override string GetProgress(bool complete)
		{
			return COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.BUILT_OUTSIDE_START;
		}
	}
}
