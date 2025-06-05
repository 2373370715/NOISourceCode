using System;
using System.Collections;
using STRINGS;

namespace Database
{
	// Token: 0x0200221C RID: 8732
	public class LaunchedCraft : ColonyAchievementRequirement
	{
		// Token: 0x0600B9CD RID: 47565 RVA: 0x0011C36C File Offset: 0x0011A56C
		public override string GetProgress(bool completed)
		{
			return COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.LAUNCHED_ROCKET;
		}

		// Token: 0x0600B9CE RID: 47566 RVA: 0x0047817C File Offset: 0x0047637C
		public override bool Success()
		{
			using (IEnumerator enumerator = Components.Clustercrafts.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (((Clustercraft)enumerator.Current).Status == Clustercraft.CraftStatus.InFlight)
					{
						return true;
					}
				}
			}
			return false;
		}
	}
}
