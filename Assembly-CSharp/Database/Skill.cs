using System;
using System.Collections.Generic;
using TUNING;

namespace Database
{
	// Token: 0x02002233 RID: 8755
	public class Skill : Resource, IHasDlcRestrictions
	{
		// Token: 0x0600BA17 RID: 47639 RVA: 0x0047BDE0 File Offset: 0x00479FE0
		public Skill(string id, string name, string description, int tier, string hat, string badge, string skillGroup, List<SkillPerk> perks = null, List<string> priorSkills = null, string requiredDuplicantModel = "Minion", string[] requiredDlcIds = null, string[] forbiddenDlcIds = null) : base(id, name)
		{
			this.description = description;
			this.requiredDlcIds = requiredDlcIds;
			this.forbiddenDlcIds = forbiddenDlcIds;
			this.tier = tier;
			this.hat = hat;
			this.badge = badge;
			this.skillGroup = skillGroup;
			this.perks = perks;
			if (this.perks == null)
			{
				this.perks = new List<SkillPerk>();
			}
			this.priorSkills = priorSkills;
			if (this.priorSkills == null)
			{
				this.priorSkills = new List<string>();
			}
			this.requiredDuplicantModel = requiredDuplicantModel;
		}

		// Token: 0x0600BA18 RID: 47640 RVA: 0x0047BE6C File Offset: 0x0047A06C
		[Obsolete]
		public Skill(string id, string name, string description, string dlcId, int tier, string hat, string badge, string skillGroup, List<SkillPerk> perks = null, List<string> priorSkills = null, string requiredDuplicantModel = "Minion") : this(id, name, description, tier, hat, badge, skillGroup, perks, priorSkills, requiredDuplicantModel, null, null)
		{
		}

		// Token: 0x0600BA19 RID: 47641 RVA: 0x0011C6CE File Offset: 0x0011A8CE
		public int GetMoraleExpectation()
		{
			return SKILLS.SKILL_TIER_MORALE_COST[this.tier];
		}

		// Token: 0x0600BA1A RID: 47642 RVA: 0x0011C6DC File Offset: 0x0011A8DC
		public bool GivesPerk(SkillPerk perk)
		{
			return this.perks.Contains(perk);
		}

		// Token: 0x0600BA1B RID: 47643 RVA: 0x0047BE94 File Offset: 0x0047A094
		public bool GivesPerk(HashedString perkId)
		{
			using (List<SkillPerk>.Enumerator enumerator = this.perks.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.IdHash == perkId)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x0600BA1C RID: 47644 RVA: 0x0011C6EA File Offset: 0x0011A8EA
		public string[] GetRequiredDlcIds()
		{
			return this.requiredDlcIds;
		}

		// Token: 0x0600BA1D RID: 47645 RVA: 0x0011C6F2 File Offset: 0x0011A8F2
		public string[] GetForbiddenDlcIds()
		{
			return this.forbiddenDlcIds;
		}

		// Token: 0x04009864 RID: 39012
		public string description;

		// Token: 0x04009865 RID: 39013
		public string[] requiredDlcIds;

		// Token: 0x04009866 RID: 39014
		public string[] forbiddenDlcIds;

		// Token: 0x04009867 RID: 39015
		public string skillGroup;

		// Token: 0x04009868 RID: 39016
		public string hat;

		// Token: 0x04009869 RID: 39017
		public string badge;

		// Token: 0x0400986A RID: 39018
		public int tier;

		// Token: 0x0400986B RID: 39019
		public bool deprecated;

		// Token: 0x0400986C RID: 39020
		public List<SkillPerk> perks;

		// Token: 0x0400986D RID: 39021
		public List<string> priorSkills;

		// Token: 0x0400986E RID: 39022
		public string requiredDuplicantModel;
	}
}
