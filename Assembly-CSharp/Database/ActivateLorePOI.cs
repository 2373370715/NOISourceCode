using System;
using STRINGS;

namespace Database
{
	// Token: 0x02002212 RID: 8722
	public class ActivateLorePOI : ColonyAchievementRequirement, AchievementRequirementSerialization_Deprecated
	{
		// Token: 0x0600B9A5 RID: 47525 RVA: 0x000AA038 File Offset: 0x000A8238
		public void Deserialize(IReader reader)
		{
		}

		// Token: 0x0600B9A6 RID: 47526 RVA: 0x00477AA4 File Offset: 0x00475CA4
		public override bool Success()
		{
			foreach (BuildingComplete buildingComplete in Components.TemplateBuildings.Items)
			{
				if (!(buildingComplete == null))
				{
					Unsealable component = buildingComplete.GetComponent<Unsealable>();
					if (component != null && component.unsealed)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x0600B9A7 RID: 47527 RVA: 0x0011C1E7 File Offset: 0x0011A3E7
		public override string GetProgress(bool complete)
		{
			return COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.INVESTIGATE_A_POI;
		}
	}
}
