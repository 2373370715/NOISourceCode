using System;
using STRINGS;

namespace Database
{
	// Token: 0x02002205 RID: 8709
	public class CoolBuildingToXKelvin : ColonyAchievementRequirement, AchievementRequirementSerialization_Deprecated
	{
		// Token: 0x0600B96F RID: 47471 RVA: 0x0011C064 File Offset: 0x0011A264
		public CoolBuildingToXKelvin(int kelvinToCoolTo)
		{
			this.kelvinToCoolTo = kelvinToCoolTo;
		}

		// Token: 0x0600B970 RID: 47472 RVA: 0x0011C073 File Offset: 0x0011A273
		public override bool Success()
		{
			return BuildingComplete.MinKelvinSeen <= (float)this.kelvinToCoolTo;
		}

		// Token: 0x0600B971 RID: 47473 RVA: 0x0011C086 File Offset: 0x0011A286
		public void Deserialize(IReader reader)
		{
			this.kelvinToCoolTo = reader.ReadInt32();
		}

		// Token: 0x0600B972 RID: 47474 RVA: 0x00476D5C File Offset: 0x00474F5C
		public override string GetProgress(bool complete)
		{
			float minKelvinSeen = BuildingComplete.MinKelvinSeen;
			return string.Format(COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.KELVIN_COOLING, minKelvinSeen);
		}

		// Token: 0x04009783 RID: 38787
		private int kelvinToCoolTo;
	}
}
