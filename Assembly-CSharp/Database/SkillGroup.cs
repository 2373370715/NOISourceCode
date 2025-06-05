using System;
using System.Collections.Generic;
using Klei.AI;

namespace Database
{
	// Token: 0x02002231 RID: 8753
	public class SkillGroup : Resource, IListableOption
	{
		// Token: 0x0600BA14 RID: 47636 RVA: 0x0011C680 File Offset: 0x0011A880
		string IListableOption.GetProperName()
		{
			return Strings.Get("STRINGS.DUPLICANTS.SKILLGROUPS." + this.Id.ToUpper() + ".NAME");
		}

		// Token: 0x0600BA15 RID: 47637 RVA: 0x0011C6A6 File Offset: 0x0011A8A6
		public SkillGroup(string id, string choreGroupID, string name, string icon, string archetype_icon) : base(id, name)
		{
			this.choreGroupID = choreGroupID;
			this.choreGroupIcon = icon;
			this.archetypeIcon = archetype_icon;
		}

		// Token: 0x04009850 RID: 38992
		public string choreGroupID;

		// Token: 0x04009851 RID: 38993
		public List<Klei.AI.Attribute> relevantAttributes;

		// Token: 0x04009852 RID: 38994
		public List<string> requiredChoreGroups;

		// Token: 0x04009853 RID: 38995
		public string choreGroupIcon;

		// Token: 0x04009854 RID: 38996
		public string archetypeIcon;

		// Token: 0x04009855 RID: 38997
		public bool allowAsAptitude = true;
	}
}
