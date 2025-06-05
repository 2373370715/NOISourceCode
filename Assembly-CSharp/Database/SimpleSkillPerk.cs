using System;

namespace Database
{
	// Token: 0x0200222F RID: 8751
	public class SimpleSkillPerk : SkillPerk
	{
		// Token: 0x0600BA11 RID: 47633 RVA: 0x0011C663 File Offset: 0x0011A863
		public SimpleSkillPerk(string id, string description) : base(id, description, null, null, null, false)
		{
		}

		// Token: 0x0600BA12 RID: 47634 RVA: 0x0011C671 File Offset: 0x0011A871
		public SimpleSkillPerk(string id, string description, string[] requiredDlcIds) : base(id, description, null, null, null, requiredDlcIds, false)
		{
		}
	}
}
