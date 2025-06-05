using System;
using System.Collections.Generic;
using STRINGS;

namespace Database
{
	// Token: 0x0200220E RID: 8718
	public class AtLeastOneBuildingForEachDupe : ColonyAchievementRequirement, AchievementRequirementSerialization_Deprecated
	{
		// Token: 0x0600B994 RID: 47508 RVA: 0x0011C19F File Offset: 0x0011A39F
		public AtLeastOneBuildingForEachDupe(List<Tag> validBuildingTypes)
		{
			this.validBuildingTypes = validBuildingTypes;
		}

		// Token: 0x0600B995 RID: 47509 RVA: 0x00477580 File Offset: 0x00475780
		public override bool Success()
		{
			if (Components.LiveMinionIdentities.Items.Count <= 0)
			{
				return false;
			}
			int num = 0;
			foreach (IBasicBuilding basicBuilding in Components.BasicBuildings.Items)
			{
				Tag prefabTag = basicBuilding.transform.GetComponent<KPrefabID>().PrefabTag;
				if (this.validBuildingTypes.Contains(prefabTag))
				{
					num++;
					if (prefabTag == "FlushToilet" || prefabTag == "Outhouse")
					{
						return true;
					}
				}
			}
			return num >= Components.LiveMinionIdentities.Items.Count;
		}

		// Token: 0x0600B996 RID: 47510 RVA: 0x000B1628 File Offset: 0x000AF828
		public override bool Fail()
		{
			return false;
		}

		// Token: 0x0600B997 RID: 47511 RVA: 0x00477648 File Offset: 0x00475848
		public void Deserialize(IReader reader)
		{
			int num = reader.ReadInt32();
			this.validBuildingTypes = new List<Tag>(num);
			for (int i = 0; i < num; i++)
			{
				string name = reader.ReadKleiString();
				this.validBuildingTypes.Add(new Tag(name));
			}
		}

		// Token: 0x0600B998 RID: 47512 RVA: 0x0047768C File Offset: 0x0047588C
		public override string GetProgress(bool complete)
		{
			if (this.validBuildingTypes.Contains("FlushToilet"))
			{
				return COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.BUILT_ONE_TOILET;
			}
			if (complete)
			{
				return COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.BUILT_ONE_BED_PER_DUPLICANT;
			}
			int num = 0;
			foreach (IBasicBuilding basicBuilding in Components.BasicBuildings.Items)
			{
				KPrefabID component = basicBuilding.transform.GetComponent<KPrefabID>();
				if (this.validBuildingTypes.Contains(component.PrefabTag))
				{
					num++;
				}
			}
			return string.Format(COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.BUILING_BEDS, complete ? Components.LiveMinionIdentities.Items.Count : num, Components.LiveMinionIdentities.Items.Count);
		}

		// Token: 0x0400978D RID: 38797
		private List<Tag> validBuildingTypes = new List<Tag>();
	}
}
