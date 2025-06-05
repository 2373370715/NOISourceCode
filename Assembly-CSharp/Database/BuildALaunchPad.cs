using System;
using STRINGS;

namespace Database
{
	// Token: 0x0200221F RID: 8735
	public class BuildALaunchPad : ColonyAchievementRequirement
	{
		// Token: 0x0600B9D6 RID: 47574 RVA: 0x0011C3A1 File Offset: 0x0011A5A1
		public override string GetProgress(bool complete)
		{
			return COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.BUILD_A_LAUNCHPAD;
		}

		// Token: 0x0600B9D7 RID: 47575 RVA: 0x0047823C File Offset: 0x0047643C
		public override bool Success()
		{
			foreach (LaunchPad component in Components.LaunchPads.Items)
			{
				WorldContainer myWorld = component.GetMyWorld();
				if (!myWorld.IsStartWorld && Components.WarpReceivers.GetWorldItems(myWorld.id, false).Count == 0)
				{
					return true;
				}
			}
			return false;
		}
	}
}
