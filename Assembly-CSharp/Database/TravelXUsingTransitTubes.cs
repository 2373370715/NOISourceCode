using System;
using STRINGS;

namespace Database
{
	// Token: 0x0200220B RID: 8715
	public class TravelXUsingTransitTubes : ColonyAchievementRequirement, AchievementRequirementSerialization_Deprecated
	{
		// Token: 0x0600B988 RID: 47496 RVA: 0x0011C13C File Offset: 0x0011A33C
		public TravelXUsingTransitTubes(NavType navType, int distanceToTravel)
		{
			this.navType = navType;
			this.distanceToTravel = distanceToTravel;
		}

		// Token: 0x0600B989 RID: 47497 RVA: 0x00477254 File Offset: 0x00475454
		public override bool Success()
		{
			int num = 0;
			foreach (MinionIdentity minionIdentity in Components.MinionIdentities.Items)
			{
				Navigator component = minionIdentity.GetComponent<Navigator>();
				if (component != null && component.distanceTravelledByNavType.ContainsKey(this.navType))
				{
					num += component.distanceTravelledByNavType[this.navType];
				}
			}
			return num >= this.distanceToTravel;
		}

		// Token: 0x0600B98A RID: 47498 RVA: 0x004772E8 File Offset: 0x004754E8
		public void Deserialize(IReader reader)
		{
			byte b = reader.ReadByte();
			this.navType = (NavType)b;
			this.distanceToTravel = reader.ReadInt32();
		}

		// Token: 0x0600B98B RID: 47499 RVA: 0x00477310 File Offset: 0x00475510
		public override string GetProgress(bool complete)
		{
			int num = 0;
			foreach (MinionIdentity minionIdentity in Components.MinionIdentities.Items)
			{
				Navigator component = minionIdentity.GetComponent<Navigator>();
				if (component != null && component.distanceTravelledByNavType.ContainsKey(this.navType))
				{
					num += component.distanceTravelledByNavType[this.navType];
				}
			}
			return string.Format(COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.TRAVELED_IN_TUBES, complete ? this.distanceToTravel : num, this.distanceToTravel);
		}

		// Token: 0x04009789 RID: 38793
		private int distanceToTravel;

		// Token: 0x0400978A RID: 38794
		private NavType navType;
	}
}
