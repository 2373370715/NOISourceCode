using System;
using System.Collections.Generic;
using STRINGS;

namespace Database
{
	// Token: 0x02002200 RID: 8704
	public class ResearchComplete : ColonyAchievementRequirement, AchievementRequirementSerialization_Deprecated
	{
		// Token: 0x0600B95A RID: 47450 RVA: 0x0031D160 File Offset: 0x0031B360
		public override bool Success()
		{
			using (List<Tech>.Enumerator enumerator = Db.Get().Techs.resources.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (!enumerator.Current.IsComplete())
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x0600B95B RID: 47451 RVA: 0x000AA038 File Offset: 0x000A8238
		public void Deserialize(IReader reader)
		{
		}

		// Token: 0x0600B95C RID: 47452 RVA: 0x00476580 File Offset: 0x00474780
		public override string GetProgress(bool complete)
		{
			if (complete)
			{
				return string.Format(COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.TECH_RESEARCHED, Db.Get().Techs.resources.Count, Db.Get().Techs.resources.Count);
			}
			int num = 0;
			using (List<Tech>.Enumerator enumerator = Db.Get().Techs.resources.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.IsComplete())
					{
						num++;
					}
				}
			}
			return string.Format(COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.TECH_RESEARCHED, num, Db.Get().Techs.resources.Count);
		}
	}
}
