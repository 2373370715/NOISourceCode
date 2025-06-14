﻿using System;
using System.Collections.Generic;
using STRINGS;

namespace Database
{
	public class ResearchComplete : ColonyAchievementRequirement, AchievementRequirementSerialization_Deprecated
	{
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

		public void Deserialize(IReader reader)
		{
		}

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
