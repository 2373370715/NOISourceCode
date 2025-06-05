using System;
using System.Collections.Generic;
using STRINGS;

namespace Database
{
	// Token: 0x0200221D RID: 8733
	public class TeleportDuplicant : ColonyAchievementRequirement
	{
		// Token: 0x0600B9D0 RID: 47568 RVA: 0x0011C378 File Offset: 0x0011A578
		public override string GetProgress(bool complete)
		{
			return COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.TELEPORT_DUPLICANT;
		}

		// Token: 0x0600B9D1 RID: 47569 RVA: 0x004781DC File Offset: 0x004763DC
		public override bool Success()
		{
			using (List<WarpReceiver>.Enumerator enumerator = Components.WarpReceivers.Items.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.Used)
					{
						return true;
					}
				}
			}
			return false;
		}
	}
}
