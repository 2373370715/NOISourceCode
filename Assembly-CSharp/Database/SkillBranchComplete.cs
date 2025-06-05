using System;
using System.Collections.Generic;
using STRINGS;

namespace Database
{
	// Token: 0x02002201 RID: 8705
	public class SkillBranchComplete : ColonyAchievementRequirement, AchievementRequirementSerialization_Deprecated
	{
		// Token: 0x0600B95E RID: 47454 RVA: 0x0011C00B File Offset: 0x0011A20B
		public SkillBranchComplete(List<Skill> skillsToMaster)
		{
			this.skillsToMaster = skillsToMaster;
		}

		// Token: 0x0600B95F RID: 47455 RVA: 0x00476654 File Offset: 0x00474854
		public override bool Success()
		{
			foreach (MinionResume minionResume in Components.MinionResumes.Items)
			{
				foreach (Skill skill in this.skillsToMaster)
				{
					if (minionResume.HasMasteredSkill(skill.Id))
					{
						if (!minionResume.HasBeenGrantedSkill(skill))
						{
							return true;
						}
						List<Skill> allPriorSkills = Db.Get().Skills.GetAllPriorSkills(skill);
						bool flag = true;
						foreach (Skill skill2 in allPriorSkills)
						{
							flag = (flag && minionResume.HasMasteredSkill(skill2.Id));
						}
						if (flag)
						{
							return true;
						}
					}
				}
			}
			return false;
		}

		// Token: 0x0600B960 RID: 47456 RVA: 0x00476774 File Offset: 0x00474974
		public void Deserialize(IReader reader)
		{
			this.skillsToMaster = new List<Skill>();
			int num = reader.ReadInt32();
			for (int i = 0; i < num; i++)
			{
				string id = reader.ReadKleiString();
				this.skillsToMaster.Add(Db.Get().Skills.Get(id));
			}
		}

		// Token: 0x0600B961 RID: 47457 RVA: 0x0011C01A File Offset: 0x0011A21A
		public override string GetProgress(bool complete)
		{
			return COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.SKILL_BRANCH;
		}

		// Token: 0x04009778 RID: 38776
		private List<Skill> skillsToMaster;
	}
}
