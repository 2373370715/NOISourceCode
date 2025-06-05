using System;
using STRINGS;

namespace Database
{
	// Token: 0x020021F4 RID: 8692
	public class ReachedSpace : VictoryColonyAchievementRequirement, AchievementRequirementSerialization_Deprecated
	{
		// Token: 0x0600B91B RID: 47387 RVA: 0x0011BDA2 File Offset: 0x00119FA2
		public ReachedSpace(SpaceDestinationType destinationType = null)
		{
			this.destinationType = destinationType;
		}

		// Token: 0x0600B91C RID: 47388 RVA: 0x0011BDB1 File Offset: 0x00119FB1
		public override string Name()
		{
			if (this.destinationType != null)
			{
				return string.Format(COLONY_ACHIEVEMENTS.DISTANT_PLANET_REACHED.REQUIREMENTS.REACHED_SPACE_DESTINATION, this.destinationType.Name);
			}
			return COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.REACH_SPACE_ANY_DESTINATION;
		}

		// Token: 0x0600B91D RID: 47389 RVA: 0x0011BDE0 File Offset: 0x00119FE0
		public override string Description()
		{
			if (this.destinationType != null)
			{
				return string.Format(COLONY_ACHIEVEMENTS.DISTANT_PLANET_REACHED.REQUIREMENTS.REACHED_SPACE_DESTINATION_DESCRIPTION, this.destinationType.Name);
			}
			return COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.REACH_SPACE_ANY_DESTINATION_DESCRIPTION;
		}

		// Token: 0x0600B91E RID: 47390 RVA: 0x00476154 File Offset: 0x00474354
		public override bool Success()
		{
			foreach (Spacecraft spacecraft in SpacecraftManager.instance.GetSpacecraft())
			{
				if (spacecraft.state != Spacecraft.MissionState.Grounded && spacecraft.state != Spacecraft.MissionState.Destroyed)
				{
					SpaceDestination destination = SpacecraftManager.instance.GetDestination(SpacecraftManager.instance.savedSpacecraftDestinations[spacecraft.id]);
					if (this.destinationType == null || destination.GetDestinationType() == this.destinationType)
					{
						if (this.destinationType == Db.Get().SpaceDestinationTypes.Wormhole)
						{
							Game.Instance.unlocks.Unlock("temporaltear", true);
						}
						return true;
					}
				}
			}
			return SpacecraftManager.instance.hasVisitedWormHole;
		}

		// Token: 0x0600B91F RID: 47391 RVA: 0x00476230 File Offset: 0x00474430
		public void Deserialize(IReader reader)
		{
			if (reader.ReadByte() <= 0)
			{
				string id = reader.ReadKleiString();
				this.destinationType = Db.Get().SpaceDestinationTypes.Get(id);
			}
		}

		// Token: 0x0600B920 RID: 47392 RVA: 0x0011BE0F File Offset: 0x0011A00F
		public override string GetProgress(bool completed)
		{
			if (this.destinationType == null)
			{
				return COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.LAUNCHED_ROCKET;
			}
			return COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.LAUNCHED_ROCKET_TO_WORMHOLE;
		}

		// Token: 0x04009770 RID: 38768
		private SpaceDestinationType destinationType;
	}
}
